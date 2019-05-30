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
        public async Task<bool> check(String proxyURL, int port, String username, String password, String testurl)
        {
            bool status = false;

            HttpClientHandler handler = new HttpClientHandler();
            handler.UseProxy = true;
            WebProxy proxy = new WebProxy(proxyURL + ":" + port, false);
            if (username != null && password != null) proxy.Credentials = new NetworkCredential(username, password);
            handler.Proxy = proxy;
            handler.ClientCertificateOptions = ClientCertificateOption.Automatic;

            using (HttpClient client = new HttpClient(handler))
            {
                // timeout - make it cutomizable
                client.Timeout = TimeSpan.FromSeconds(60);

                try
                {
                    using (HttpResponseMessage response = await client.GetAsync(testurl))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            //Console.WriteLine("Proxy cool");
                            status = true;
                        }
                        else
                        {
                            //Console.WriteLine(response.StatusCode.ToString());
                        }
                    }
                }
                catch
                {
                    status = false;
                }

            }



            return status;
        }
        private async void proxyCheckWorker()
        {
            string richText = new TextRange(proxytest.Document.ContentStart, proxytest.Document.ContentEnd).Text;
            var lines = richText.Split('\n').ToList();

            foreach (var line in lines) // read text line by line
            {
                if (url.Text == "")
                {
                    MessageBox.Show("Must enter url to test");
                    break;
                }
                var linenumber = line.Split(':').ToList();  // 4  is user with pass word 2 just ip + port


                if (linenumber.Count == 2)
                {
                    string replacement = Regex.Replace(linenumber[1].ToString(), @"\t|\n|\r", "");
                    bool status = false;
                    status = await check(linenumber[0], Int32.Parse(replacement), null, null, url.Text);
                    Console.WriteLine(status);
                    addLable(status.ToString());

                }
                else if (linenumber.Count == 4)
                {

                        string replacement = Regex.Replace(linenumber[3].ToString(), @"\t|\n|\r", "");
                        bool status = false;
                        status = await check(linenumber[0], Int32.Parse(linenumber[1].ToString()), linenumber[2].ToString(), replacement, url.Text);
                        Console.WriteLine(status);
                        addLable(status.ToString());
                }

            }

        }


        private void Test(object sender, RoutedEventArgs e)
        {

            proxyCheckWorker();

        }

        public System.Windows.Controls.Label addLable(String text) /// set next to proxy tester not working yet
        {
            System.Windows.Controls.Label label = new System.Windows.Controls.Label();
            label.Content = text;
            var x = 74;
            label.Margin = new Thickness(933, 100, 0, 0);
            x = x + 10;
            return label;
        }

    }
}

