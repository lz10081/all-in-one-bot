using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace ZenAIO
{
    public abstract class WebScrapperBase : IWebScrapper
    {

        private static IList<Proxy> proxies;
        private static bool useLocal = false;

        protected Product product;

        public WebScrapperBase(Product product)
        {
            this.product = product;
        }

        private void InitProxyList()
        {
            proxies = new List<Proxy>();
        }

        /// <summary>
        /// Downloads the product from its URL.
        /// </summary>
        /// <returns>True if successfully downloaded, else false.</returns>
        protected bool Download()
        {
            if (proxies == null || proxies.Count == 0)
            {
                InitProxyList();

                // If still empty (no proxies available) return false.
                if (proxies == null || proxies.Count == 0)
                    useLocal = true;
            }



            return false;
        }

        /// <summary>
        /// Parses the downloaded data for information.
        /// </summary>
        /// <returns>True if data found, else false.</returns>
        protected abstract bool Scrape();

        public abstract bool Available();

    }
}
