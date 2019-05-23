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
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\" + "proxy.txt"))
            {
                string text = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "proxy.txt");
               // proxy.Document = text;
            }
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            string richText = new TextRange(proxy.Document.ContentStart, proxy.Document.ContentEnd).Text;
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "proxy.txt", richText);
        }
        private void Remove(object sender, RoutedEventArgs e)
        {
           
        }
        private void Test(object sender, RoutedEventArgs e)
        {
       
            Console.WriteLine("Grabbing proxies from proxies.txt file");
            List<string> goodProxies = new List<string>();
            FileStream fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\" + "proxy.txt", FileMode.Open, FileAccess.Read);

            Console.WriteLine("Checking proxies at 1000 threads");
            List<string> proxies = new List<string>();
            using (StreamReader sr = new StreamReader(fileStream))
            {
                while (!sr.EndOfStream)
                {
                    proxies.Add(sr.ReadLine());
                }
            }
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");

            WebProxy ProxyString = new WebProxy("http://94.131.113.200:5123", true);
            //set network credentials may be optional
            NetworkCredential proxyCredential = new NetworkCredential("xxne", "gten");
            ProxyString.Credentials = proxyCredential;
            WebRequest req = WebRequest.Create("https://www.youtube.com/");
            req.Timeout = 5000;
            req.GetResponse();

            Console.WriteLine(  req.GetResponse());



        }

    }
}

