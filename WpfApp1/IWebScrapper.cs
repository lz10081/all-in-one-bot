using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenAIO
{
    public interface IWebScrapper
    {

        /// <summary>
        /// Checks if the web scrapper found an availbility.
        /// </summary>
        /// <returns>True if in-stock/available, else false.</returns>
        bool Available();

        /// <summary>
        /// Sends a web request to purchase the product.
        /// </summary>
        void SendPurchaseRequest();

    }
}
