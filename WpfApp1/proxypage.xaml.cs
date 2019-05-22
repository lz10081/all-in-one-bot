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
       
            int counter = 0;
            int total = 0;
            int good = 0;
            Console.WriteLine("Grabbing proxies from proxies.txt file");
            List<string> goodProxies = new List<string>();
            FileStream fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\" + "proxy.txt", FileMode.Open, FileAccess.Read);

            Console.WriteLine("Checking proxies at 1000 threads");
            StreamReader sr = new StreamReader(fileStream);

            DateTime date = DateTime.Now;

            int threads = 0;
            while (!sr.EndOfStream)
            {
                while (threads >= 1000) Thread.Sleep(100);
                counter++;
                string temp = sr.ReadLine();
                new Thread(() =>
                {
                    threads++;
                    if (CheckProxy(temp))
                    {
                        good++;
                        using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"\" + "goodProxies.txt"))
                            sw.WriteLine(temp);
                    }
                    threads--;
                    Console.Write("\rTested: {0}, threads: {2}, for {1}", ++total, (DateTime.Now - date).ToString(), threads);
                }).Start();
            }

            while (total != counter)
                Thread.Sleep(1000);
            TimeSpan took = DateTime.Now - date;
            sr.Close();
            fileStream.Close();
            //System.IO.File.WriteAllLines(@"goodProxies.txt", goodProxies.ToArray());
            Console.WriteLine("\nDone. Good proxies: " + good + " in total of " + total + ", took " + (DateTime.Now - date).ToString());
            Console.Read();

        }
        static bool CheckProxy(string proxy)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.google.com/");
                httpWebRequest.Proxy = (IWebProxy)new WebProxy(proxy);
                httpWebRequest.Method = "GET";
                httpWebRequest.Timeout = 2000;
                httpWebRequest.ReadWriteTimeout = 5000;
                DateTime now1 = DateTime.Now;
                Task<WebResponse> responseAsync = httpWebRequest.GetResponseAsync();
                while ((responseAsync.Status == TaskStatus.WaitingForActivation || responseAsync.Status == TaskStatus.WaitingToRun) && (now1.AddSeconds(10.0) > DateTime.Now))
                    Thread.Sleep(500);
                DateTime now2 = DateTime.Now;
                while (responseAsync.Status == TaskStatus.Running && (now2.AddSeconds(2.0) > DateTime.Now))
                    Thread.Sleep(500);
                return responseAsync.Status == TaskStatus.RanToCompletion;
            }
            catch { }
            return false;
        }
    }
}

