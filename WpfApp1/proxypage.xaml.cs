using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Net.Sockets;

namespace WpfApp1
{
    /// <summary>
    /// proxypage.xaml 的交互逻辑
    /// </summary>
    public partial class proxypage : Page
    {
        public proxypage()
        {
            InitializeComponent();
            TextRange range;
            FileStream fStream;
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\" + "proxy.txt"))
            {
                range = new TextRange(proxy.Document.ContentStart, proxy.Document.ContentEnd);
                fStream = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\" + "proxy.txt", System.IO.FileMode.OpenOrCreate);
                range.Load(fStream, System.Windows.DataFormats.Text);
                fStream.Close();
            }

        }
        private void Save(object sender, RoutedEventArgs e)
        {
            string richText = new TextRange(proxy.Document.ContentStart, proxy.Document.ContentEnd).Text;
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "proxy.txt", richText);
        }
        private void Remove(object sender, RoutedEventArgs e)
        {
            TextRange range;
            range = new TextRange(proxytest.Document.ContentStart, proxytest.Document.ContentEnd);
            range.Text = "";
        }
        private void Test(object sender, RoutedEventArgs e)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.supremenewyork.com/shop");
            WebProxy myproxy = sent("http://94.131.113.199", 5123, "elgv", "xyeb");
            request.Proxy = myproxy;   // set proxy here
            request.Timeout = 10000;
            request.Method = "HEAD";
            Stopwatch sw = Stopwatch.StartNew();
            try {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {

                    Console.WriteLine(response.StatusCode);
                }
                sw.Stop();
                Console.WriteLine("Request took {0}", sw.Elapsed.Milliseconds);
            }
            catch
            {
                Console.WriteLine("404");
            }
          
        }

        public WebProxy sent(String proxyURL, int port, String username, String password)
        {
            //Validate proxy address
            var proxyURI = new Uri(string.Format("{0}:{1}", proxyURL, port));

            //Set credentials
            ICredentials credentials = new NetworkCredential(username, password);

            //Set proxy
            WebProxy proxy = new WebProxy(proxyURI, true, null, credentials);
            return proxy;
        }





    }
}

