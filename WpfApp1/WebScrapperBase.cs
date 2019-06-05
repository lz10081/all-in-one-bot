using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZenAIO
{
    public abstract class WebScrapperBase : IWebScrapper
    {
        private static IList<Utils.Pair<Proxy, bool>> proxies;
        private static readonly object theLock = new object();
        private static bool useLocal = false;
        private static readonly Random random = new Random();

        protected Product product;

        public WebScrapperBase(Product product)
        {
            this.product = product;
        }

        /// <summary>
        /// Creates a proxy by parsing the string fields.
        /// </summary>
        /// <param name="fields">string array.</param>
        /// <returns>Proxy if parsed successfully, else returns null.</returns>
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

        /// <summary>
        /// Loads in the Proxies from the proxy.txt file.
        /// </summary>
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

        /// <summary>
        /// Randomly selects a proxy from proxy list.
        /// </summary>
        /// <returns>Proxy to use.</returns>
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

        /// <summary>
        /// Releases the proxy.
        /// </summary>
        /// <param name="proxy">Proxy to release</param>
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

        protected class Payload
        {
            [JsonProperty("productQuantity")]

            public int Quantity { get; set; }
            [JsonProperty("productId")]
            public string ProductID { get; set; }
        }

        /// <summary>
        /// Downloads the product from its URL.
        /// </summary>
        /// <param name="result">result output.</param>
        /// <returns>True if successfully downloaded, else false.</returns>
        protected bool Download(out string result, out string proxyUsed)
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
                proxyUsed = proxy.GetPrettyString();
                WebProxy webProxy = proxy.GetWebProxy();
                client.Proxy = webProxy;

                if (proxy.HasAuthentication())
                    client.Proxy.Credentials = new NetworkCredential(proxy.Username, proxy.Password);
            }

            else
                proxyUsed = "";

            RestRequest request = new RestRequest(Method.GET);

            request.AddHeader("postman-token", "62472814-e58d-de30-fa1b-678ab9d68f66");
            request.AddHeader("cache-control", "no-cache");

            var response = client.Execute(request);

            Debug.Info("response.StatusCode: " + ((int)response.StatusCode));

            if (!response.IsSuccessful)
            {

                result = "";

                if (!useLocal)
                    ReleaseProxy(ref proxy);

                return false;
            }

            result = response.Content;
            var reheader = response.Headers.ToList();
            for(int i = 0; i < reheader.Count; i++)
                Debug.Info("reheader: \n{0}", reheader[i].ToString()); // test dump
            //Debug.Info("Cookies: \n{0}", response.Cookies.ToString());
            Debug.Info("Content: \n{0}", result); // test dump

            // Make sure we release the proxy when we are done using it!
            if (!useLocal)
                ReleaseProxy(ref proxy);

            return true;
        }
        public void PurchaseRequest()
        {


   
            if (proxies == null || proxies.Count == 0)
            {
                HttpWebRequest.DefaultMaximumErrorResponseLength = 1048576;

                InitProxyList();

                // If still empty (no proxies available) return false.
                if (proxies == null || proxies.Count == 0)
                    useLocal = true;
            }

           var client = new RestClient("https://www.footlocker.com/product/adidas-alphaedge-4d-boys-grade-school/EF3453G.html");
  
        Proxy proxy = null;

            CookieContainer _cookieJar = new CookieContainer();
          
            client.CookieContainer = _cookieJar;
          
            if (!useLocal)
            {
                proxy = GetRandomProxy();
               // proxyUsed = proxy.GetPrettyString();
                WebProxy webProxy = proxy.GetWebProxy();
                client.Proxy = webProxy;

                if (proxy.HasAuthentication())
                    client.Proxy.Credentials = new NetworkCredential(proxy.Username, proxy.Password);
            }

          
            RestRequest request = new RestRequest("/resource/", Method.POST);

            request.AddHeader("authority", "www.facebook.com");
            request.AddHeader("scheme", "https");
            request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("accept-language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
            request.AddHeader("cache-control", "max-age=0");
            request.AddHeader("upgrade-insecure-requests", "https://www.footlocker.com/product/adidas-alphaedge-4d-boys-grade-school/EF3453G.html");
            request.AddHeader("origin", "https://www.footlocker.com");
            request.AddHeader("referer", "1");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36");

            request.RequestFormat = DataFormat.Json;
           // request.AddJsonBody(postdata);

            var response = client.Execute(request);

           // Debug.Info("response.StatusCode: " + ((int)response.StatusCode));
            //  result = response.Content;
           
            if (!useLocal)
                ReleaseProxy(ref proxy);
        }
        ///
        /// {"productQuantity":1,"productId":"21946223"} https://www.footlocker.com/product/nike-lebron-16-mens/862001.html size 8 we need to find the productId using the size first
        ///  [{"id":"21946223","type":"size","value":"08.0"},{"id":"21945281","type":"style","value":"James, Lebron | Black/White"}],"barCode":"193148944159","code":"21946223","isBackOrderable":false,"isPreOrder":false,"isRecaptchaOn":false,"price":    
        ///     
        ///
        /*
        :authority: www.footlocker.com
        :method: POST
        :path: /api/users/carts/current/entries? timestamp = 1559447145197
        :scheme: https
        accept: application/json
        accept-encoding: gzip, deflate, br
        accept-language: en-US,en;q=0.9,zh-CN;q=0.8,zh;q=0.7
        content-length: 44
        content-type: application/json
        cookie: _ga=GA1.2.1916991690.1543552412; _mibhv=anon-1543552413134-9325675786_2389; AAMC_footlocker_0=REGION%7C7; _scid=2251ea1d-53cb-4861-b055-5096ae908bc0; dtm_token=AQEHThWQPjV9UAFJ_K1jAQEJrgE; rskxRunCookie=0; rCookie=8qtd2qzidipgl7a36y4gar; _fbp=fb.1.1549641813354.1442695290; _fbc=fb.1.1550370870786.IwAR1haDdFif3c2pTpNjRubJYGlYtNhApKDrM8j5YLqMKuYQM_VuPwaNa8NH0; _gcl_au=1.1.1463988017.1551370996; s_ev85=%5B%5B%27affiliate-_-rakuten-_--_--_--_--_-p-_-prospecting-_-deeplink-_--_--_--_-text-_--_--_--_-%27%2C%271552965870670%27%5D%5D; userStatus=registered; userVIP=vip; _pxhd=b8cf4487461a198492ee790fa3028d888001ce59ed8085aa6643f8babcb74874:31bff551-52db-11e9-9ef7-4b71d5b1b6c3; rmStore=amid:3071|dmid:false|dcomm:0; BVBRANDID=a1bf57c9-078f-4578-9ab3-86ee1a09c25b; aamSegId=SegId%3D12081879%3B12179512%3B12688395%3B13312833%3B13312836%3B13312845%3B13312849%3B13312850%3B13312851%3B13312860%3B13312861%3B13312876%3B13312889%3B13312893%3B13312905%3B13312911%3B13314772%3B13314793%3B13325063%3B13325065%3B13325134%3B13325140%3B13325179%3B13325180%3B13332772%3B13332776%3B13332777%3B13343780%3B13346781%3B13347540%3B13369269%3B13369317%3B13369378%3B13369842%3B13391934%3B13391969%3B13392091%3B13392108%3B13392112%3B13392114%3B13392237%3B13392240%3B13392250%3B13393163%3B13393164%3B13393167%3B13393181%3B13439050%3B13439065%3B13547867%3B13547872%3B13547876%3B13557479%3B13584244; cart-guid=fe1e67ef-8da0-41cf-91e0-45ce0e29534f; DCT_Exp_HUNDRED=DCT; xyz_cr_100238_et_100==NaN&cr=100238&et=100; _abck=7C1F5BEDCFC1689A424824762C10C2C4 ~0~YAAQlQbSF1symglrAQAAzjDDCwFm3Fzih/mR2w0t6GB8kwimEOtwcx+r5tE9yBREItctlZCuVz3T9/9LbVW2wJ3juHxPNgAwLmpYuIeEU4kcs3rn60gYY1yNb9j54AfzYek6fV+qNCO8s8HLpPaYpPdwajrd+kZK/E8kKV8hNs0t9c8k7U3J6KAI+EdHv1mxzv/G1ew0pq2BzbwOEeR8W9ETam3GD53Rel+ZNj9nXIZWFxGe4aqHte1+YwBdtglrhZNESTmbdJbXM3ZrO6RwtC+bo6X2eyVgOsGcvO0Y2Z5n3dsFtAoa27nxVxwlgO2K4pKLKZWUCbdKiHOc7XWPw4YHpPovDA==~-1~1-xjVpCTpHmj-10000-100-3000~-1; check=true; AMCVS_40A3741F578E26BA7F000101%40AdobeOrg=1; _gid=GA1.2.1599724593.1559409967; s_pr_tbe65=1559409967439; s_cc=true; showMarketing=false; s_pr_tbe66=1559409968646; BVImplmain_site=8001; _sctr=1|1559361600000; bm_sz=8A83C3C1FAF0BB22241CCA9131B9CA7C ~YAAQ9oLXF+crlg1rAQAA/VyZFQN0M4yEBGb20VkQUUnlveEGNI9ZRzu1XWxCQyolAMtiIfZBYn3zrPeCXh8kzYCeBot4KnV+2L4R/75GTHWE3cciuwvlcr0dMooDR9DI9t7A3JUpl8q8P3xKebe2ZmtgKyY+HZ3NtYeMPju5TMf9A0rS6JFN4shD6NJU7C58hZFYQg==; mboxEdgeCluster=17; gpv_prevURL=https%3A%2F%2Fwww.footlocker.com%2Fproduct%2Fnike-lebron-16-mens%2F862001.html; gpv_products=862001; JSESSIONID=1dad2bnhwqwal1mkwyw6bdodhw.azupkaraf278880; s_lv_s=Less%20than%201%20day; s_vnum=1561953600435%26vn%3D3; s_invisit=true; s_vs=1; BVBRANDSID=f3fad3fb-ab7b-4790-ae84-dc9cfb96a72f; ak_bmsc=784A461ABDF16FC381ADDC70E34ACC0417D20695F11E00006343F35C124CE920 ~pltkxwtIPh99R8UGSG71/7TIas0y63mVvNwYMFRY9GlrI1V4vAq1Ubkg3wiKEI0h0ZbvePSHfibpcPqhlMdpURYO7cLsU4l7yjxvKRc7FzKsf07WZIrT/51KlyRPcHi+Jkkwaz400Gpwu76fFPhVeSN7hI+yAiEDQBrUbjhtDuO1I69iFvwxQ6UER1ZaZDOhYL8ZRCYqZD9oGSfpMqWuTxL0AY27Hr7aRVxlTYNNJg+CCWwaNqoy0wzK9iDICsXlururuid1kvxqF+BZ15eT+G+Q==; gpv_p25=fl%3A%20w%3A%20cart; gpv_events=no%20value; _gat_gtag_UA_116985434_4=1; _gat_gtag_UA_50007301_5=1; g_vs=1; _gat=1; bm_sv=F4CA060A28581B1736A5BBF7179246D3 ~Jgir1QL9U/2RtclRU7QHey9ngbmLiHsh1+oa2P9zzoaObBTmakuOj1CPafGGg9r3A+mLgYUJEWzSjIMwpM9ykluDryQ2Fokpj1rrm8A1D+1GNcnTVJbhncTR2JteyscloEoTOS6E/GfJLBqCHFNgIeCQmXGC5FXiFOAW3H8hGnE=; mp_footlocker_mixpanel=%7B%22distinct_id%22%3A%20%2216aecae147a531-0cc3abff54cde7-b78173e-384000-16aecae147b15e3%22%2C%22event_lang%22%3A%20%22en%22%7D; AMCV_40A3741F578E26BA7F000101%40AdobeOrg=-1303530583%7CMCIDTS%7C18049%7CMCMID%7C53519568199049162901986333651682984193%7CMCAAMLH-1560051922%7C7%7CMCAAMB-1560051922%7CRKhpRz8krg2tLO6pguXWp5olkAcUniQYPHaMWWgdJ3xzPWQmdj0y%7CMCOPTOUT-1559453439s%7CNONE%7CvVersion%7C3.3.0%7CMCCIDH%7C1050806805; _derived_epik=dj0yJnU9UVpoRlZONk9WUXF2R0N0dS1yUXVVMHo2WTBWSllOTFcmbj1xUnpLVVlJWmsyR21hdnhKVUxWVkRRJm09NyZ0PUFBQUFBRnp6UnRn; lastRskxRun=1559447124199; stc111427=env:1559446241%7C20190703033041%7C20190602041524%7C2%7C1011950:20200601034524|uid:1553943632679.661918987.4302063.111427.2145457305.:20200601034524|srchist:1011950%3A1559446241%3A20190703033041:20200601034524|nsc:1:20200329120625|tsa:1559446241780.1643690826.8414688.2722970867820502.:20190602041524; _px3=da15c31e22e308381c4da571f742400e149a7d0fb1e4c6a9d186988e2ef016d2:xEU1jnR1tsFkqygLfGMKogcimrsyFfZlWd2PfRVO2BjiPwQJcFT4WrJKDiHzIAj93osowNFibqscAqVHJAiuVw==:1000:rp1jtn3RumwHgBi3eCoqDwrHraDjBDCfpGQTrQrToPvxydPcc1YpEYceCNnI6kbGjqqxZQlsf/h8Y1/LPouKcOaZWtqBLy9TGjtDAP/La2V/eeIzEZbStTmUzurjxDTm85ZchGDrcx5D8BRKXR92yRc637dUHRj67QMJLUo2GkQ=; s_tp=5349; s_ppv=FL%253A%2520W%253A%2520PDP%2C23%2C23%2C1212; mbox=PC#22d5339546de49dd846e3dc570794e50.17_115#1622691039|session#cc131e47404e483ba28b060dc6ad5ed0#1559448988; s_lv=1559447145183; s_sq=footlockerglobalprod%252Cfootlockerfootlockerglobalenfootlocker%3D%2526c.%2526a.%2526activitymap.%2526page%253Dfl%25253A%252520w%25253A%252520pdp%2526link%253DADD%252520TO%252520CART%2526region%253DProductDetails%2526pageIDType%253D1%2526.activitymap%2526.a%2526.c%2526pid%253Dfl%25253A%252520w%25253A%252520pdp%2526pidt%253D1%2526oid%253DADD%252520TO%252520CART%2526oidt%253D3%2526ot%253DSUBMIT; _gali=ProductDetails
        origin: https://www.footlocker.com
        referer: https://www.footlocker.com/product/nike-lebron-16-mens/862001.html
        user-agent: Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1
        x-csrf-token: 8be30f3e-6429-4930-9698-bb9a008448c9
        x-fl-productid: 21946223
        x-fl-request-id: e6dc09d0-84e8-11e9-a575-c7fbc7e1a217
        */
        /// here is the post to add to cart will start working on it 
        /// <summary>
        /// Parses the downloaded data for information.
        /// </summary>
        /// <param name="content">content to parse.</param>
        /// <returns>True if data found, else false.</returns>
        protected abstract bool Scrape(ref string content);

        public virtual bool Available(out string proxyUsed)
        {
            string content;

            if (!Download(out content, out proxyUsed))
                return false;

            return Scrape(ref content);
        }

        public abstract void SendPurchaseRequest();

    }
}
