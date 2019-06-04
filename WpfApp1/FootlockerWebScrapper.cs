using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZenAIO
{
    public class FootlockerWebScrapper : WebScrapperBase
    {

        private string query;
        private Regex regex;

        public FootlockerWebScrapper(Product product) : base(product)
        {
            query = GetProductQuery(ref product);
            regex = new Regex(query);
        }

        private static string GetProductQuery(ref Product product)
        {
            StringBuilder builder = new StringBuilder(0x20);
            builder.Append('"');

            builder.Append(product.Size.ToString("00.0#"));
            builder.Append("\",\"isDisabled\":true");// {"name":"08.0","code":"21946329","isDisabled":false}

            return builder.ToString();
        }

        protected override bool Scrape(ref string content)
        {
            // "07.0","isDisabled":true
            // "size", "isDisabled":true -> true out-of-stock, false in-stock

            Match match = regex.Match(content);

            if (match.Success)
                return false;

            // var otherLang = regex.Match(content).Groups[1];
            // Debug.Info("Match result: " + match.Value);
            // Debug.Info(otherLang);

            return true;
        }

    }
}
