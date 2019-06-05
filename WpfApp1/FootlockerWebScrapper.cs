using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RestSharp;

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

        public override void SendPurchaseRequest()
        {
            var payload = new Payload
            {
                Quantity = 1,
                ProductID = "21946329"  //https://www.footlocker.com/product/Nike-LeBron-16---Men-s/I1521001.html
            };

           
        }
        public void SendPurchaseReques() //solebox Access to this page has been denied because we believe you are using automation tools to browse the website.
        {


            var client = new RestClient("https://www.solebox.com/mein-konto/");
            RestRequest request = new RestRequest("/", Method.GET);

            request.AddHeader("authority", "www.solebox.com");
            request.AddHeader("scheme", "https");
            request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("accept-language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
            request.AddHeader("cache-control", "max-age=0");
            request.AddHeader("upgrade-insecure-requests", "1");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36");
            request.AddHeader("cookie", "cto_lwid=083fed1b-38d9-4fb5-ad3f-17a53f2afbf2; mlid=undefined; displayedCookiesNotification=1; __utmz=1.1557283576.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); sid_key=oxid; px-abgroup=A; px-abper=100; __utmc=1; _fbp=fb.1.1559752431107.1312876194; cto_idcpy=d836f8b2-2cbc-4ab8-b80d-410288d34136; klarnaAddressValidated=0; language=1; __utma=1.1294710284.1557283576.1559758562.1559763477.5; __utmt=1; cto_clc=1; cto_red_atmpt=1; sid=2q7sg85mkhd1fe9vrd8lfraao6; __utmb=1.2.10.1559763477; _px3=b3780d91446af69c8e1d57e6583132a8ade98a4c4b34c84c515a863b6f0257ac:wCMykZK5TCSRg75bdERs3bbh+fRmA0a2GU1FPCOqeuLo8qU/YyoNIG8mG6zMP+dasN/N7MWTJxauftKvvLNV/A==:1000:G6TKS9vchd/M1Xds7ZzDBrStzFgf05Hrup3KHWXhg1qBHAbAOvz7H/2iVa8CCJ9xChfDbLSWVfIzIseD+/apmvLV66bz03QJ3my0K1hE2dwXdOdsGmfy9FP3yjD+KQYIGk2TbGtNDiQccfXkajWtMBJFv0mj1tdR0UCEknT4R8M=");
            var response = client.Execute(request);

            Debug.Info("response.StatusCode: " + ((int)response.StatusCode));
            //  result = response.Content;
            Debug.Info("Content: \n{0}", response.Content);

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
