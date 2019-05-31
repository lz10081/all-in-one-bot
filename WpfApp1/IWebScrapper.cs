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
        /// <returns></returns>
        bool Available();

    }
}
