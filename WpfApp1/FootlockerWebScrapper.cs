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

        public FootlockerWebScrapper(Product product) : base(product)
        {
        }

        private string GetProductQuery()
        {
            StringBuilder builder = new StringBuilder(0x20);
            builder.Append('"');

            if (product.Size < 10.0F)
                builder.Append('0');

            builder.Append(product.Size.ToString());
            builder.Append("\",\"isDisabled\":");

            return builder.ToString();
        }

        protected override bool Scrape(ref string content)
        {
            // "07.0","isDisabled":true
            // "size", "isDisabled":true -> true out-of-stock, false in-stock

            string query = GetProductQuery();
            Regex regex = new Regex(query);

            Match match = regex.Match(content);

            if (!match.Success)
                return false;

            Console.WriteLine("Match result: " + match.Value);

            return false;
        }

    }
}
