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

            var stringPayload = JsonConvert.SerializeObject(payload);
            Console.WriteLine("here" + stringPayload);
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {

                // Do the actual request and await the response
                var httpResponse = httpClient.PostAsync("https://www.footlocker.com/product/Nike-LeBron-16---Men-s/I1521001.html", httpContent);

                httpResponse.Wait();

                // If the response contains content we want to read it!
                if (httpResponse.Result.Content != null)
                {
                    var responseContent = httpResponse.Result.Content.ReadAsStringAsync();
                    responseContent.Wait();

                    Debug.Info("response.here: ", responseContent.Result);
                    // Console.WriteLine("here" + responseContent);
                    // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
                }
            }
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
