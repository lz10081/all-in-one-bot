using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp;

namespace ZenAIO
{
    public abstract class WebScrapperBase : IWebScrapper
    {
        private static IList<Utils.Pair<Proxy, bool>> proxies;
        private static readonly object theLock = new object();
        private static bool useLocal = false;
        private static Random random = new Random();

        protected Product product;

        public WebScrapperBase(Product product)
        {
            this.product = product;
        }

        private Proxy CreateProxy(ref string[] fields)
        {
            if (fields.Length == 2 || fields.Length == 4)
            {
                Proxy proxy = new Proxy();
                string replacement;

                if (!Utils.GetReplacement(ref fields[0], @"\t|\n|\r", "", out replacement))
                    return null;

                // string replacement = Regex.Replace(fields[0].ToString(), @"\t|\n|\r", "");
                proxy.URL = replacement;

                if (!Utils.GetReplacement(ref fields[1], @"\t|\n|\r", "", out replacement))
                    return null;

                int port = 0;
                if (!int.TryParse(replacement, out port))
                {
                    Debug.Error("Failed to parse port number for address " + proxy.URL);
                    return null; 
                }

                proxy.Port = port;

                // Has username and password
                if (fields.Length == 4)
                {
                    if (!Utils.GetReplacement(ref fields[2], @"\t|\n|\r", "", out replacement))
                        return null;

                    proxy.Username = replacement;

                    if (!Utils.GetReplacement(ref fields[3], @"\t|\n|\r", "", out replacement))
                        return null;

                    proxy.Password = replacement;
                }

                return proxy;
            }

            else
            {
                Debug.Error("Invalid proxy.txt");
            }

            return null;
        }

        private void InitProxyList()
        {
            proxies = new List<Utils.Pair<Proxy, bool>>();
            string path = Utils.GetPath("proxy.txt");

            if (!File.Exists(path))
            {
                Task.Run(() =>
                {
                    using (var stream = File.CreateText(path))
                    {
                        stream.Close();
                    }
                });

                return;
            }

            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                string[] fields = line.Split(':');

                Proxy proxy = CreateProxy(ref fields);

                if (proxy != null)
                    proxies.Add(Utils.MakePair(proxy, false));
            }
        }

        private Proxy GetRandomProxy()
        {
            int attempts = 0;

            do
            {
                int index = random.Next(0, proxies.Count);

                Utils.Pair<Proxy, bool> pair = proxies[index];

                lock (theLock)
                {
                    if (!pair.item2)
                    {
                        pair.item2 = true;
                        return pair.item1;
                    }
                }
            }
            while (++attempts < 20);

            return null;
        }

        private void ReleaseProxy(ref Proxy proxy)
        {
            for (int i = 0; i < proxies.Count; i++)
            {
                Utils.Pair<Proxy, bool> cur = proxies[i];

                if (cur.item2 && cur.item1.Equals(proxy))
                {
                    lock (theLock)
                    {
                        cur.item2 = false;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Downloads the product from its URL.
        /// </summary>
        /// <param name="result">result output.</param>
        /// <returns>True if successfully downloaded, else false.</returns>
        protected bool Download(out string result)
        {
            if (proxies == null || proxies.Count == 0)
            {
                HttpWebRequest.DefaultMaximumErrorResponseLength = 1048576;

                InitProxyList();

                // If still empty (no proxies available) return false.
                if (proxies == null || proxies.Count == 0)
                    useLocal = true;
            }

            var client = new RestClient(product.URL);
            Proxy proxy = null;

            if (!useLocal)
            {
                proxy = GetRandomProxy();
                WebProxy webProxy = proxy.GetWebProxy();
                client.Proxy = webProxy;

                if (proxy.HasAuthentication())
                    client.Proxy.Credentials = new NetworkCredential(proxy.Username, proxy.Password);
            }

            var request = new RestRequest(Method.GET);

            request.AddHeader("postman-token", "62472814-e58d-de30-fa1b-678ab9d68f66");
            request.AddHeader("cache-control", "no-cache");

            var response = client.Execute(request);

            Debug.Info("response.StatusCode: " + ((int) response.StatusCode));

            if (!response.IsSuccessful)
            {
                result = "";

                if (!useLocal)
                    ReleaseProxy(ref proxy);

                return false;
            }

            result = response.Content;

            Debug.Info("Content: \n{0}", result); // test dump

            // Make sure we release the proxy when we are done using it!
            if (!useLocal)
                ReleaseProxy(ref proxy);

            return true;
        }

        /// <summary>
        /// Parses the downloaded data for information.
        /// </summary>
        /// <param name="content">content to parse.</param>
        /// <returns>True if data found, else false.</returns>
        protected abstract bool Scrape(ref string content);

        public virtual bool Available()
        {
            string content;

            if (!Download(out content))
                return false;

            return Scrape(ref content);
        }

    }
}
