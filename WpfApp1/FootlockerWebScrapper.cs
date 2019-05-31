using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenAIO
{
    public class FootlockerWebScrapper : WebScrapperBase
    {

        public FootlockerWebScrapper(Product product) : base(product)
        {
        }

        protected override bool Scrape()
        {
            return false;
        }

        public override bool Available()
        {
            if (!Download())
                return false;

            return Scrape();
        }

    }
}
