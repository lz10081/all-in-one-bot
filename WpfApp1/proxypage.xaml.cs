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
using System.Collections.ObjectModel;
using RestSharp;
using ZenAIO;

namespace WpfApp1
{
    /// <summary>
    /// proxypage.xaml 的交互逻辑
    /// </summary>
    public partial class proxypage : Page
    {
        ObservableCollection<Ip> MyList = new ObservableCollection<Ip>();
        List<String> IPList = new List<String>();

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
            #region dataGridProxies Settings
            dataGridProxies.ItemsSource = MyList;
            dataGridProxies.AutoGenerateColumns = false;
            dataGridProxies.IsReadOnly = true;
            dataGridProxies.SelectionMode = DataGridSelectionMode.Single;
            DataGridTextColumn c = new DataGridTextColumn();
            c = new DataGridTextColumn();
            c.Header = "IP";
            c.Binding = new Binding("IP");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Port";
            c.Binding = new Binding("Port");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Username";
            c.Binding = new Binding("Username");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Password";
            c.Binding = new Binding("Password");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Status";
            c.Binding = new Binding("Status");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);
            #endregion

        }
        public class Ip
        {
            public string IP { get; set; }
            public string Port { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }

            public string Status { get; set; }

        }

        private void Save(object sender, RoutedEventArgs e)
        {
            string richText = new TextRange(proxy.Document.ContentStart, proxy.Document.ContentEnd).Text;
            System.IO.File.WriteAllText(Utils.GetPath("proxy.txt"), richText);
        }
        private void Remove(object sender, RoutedEventArgs e)
        {
            TextRange range;
            range = new TextRange(proxytest.Document.ContentStart, proxytest.Document.ContentEnd);
            range.Text = "";
            dataGridProxies.ItemsSource = "";
        }
        public bool check2(String proxyURL, int port, String username, String password, String testurl) // RestSharp Client
        {
            bool status = false;

            var client = new RestClient(testurl);
            client.Proxy = new WebProxy(proxyURL + ":" + port, false);
            if (username != null && password != null) client.Proxy.Credentials = new NetworkCredential(username, password);
            var response = client.Execute(new RestRequest());
            //  Console.WriteLine(response.ResponseStatus);
            if (response.ResponseStatus.ToString() == "Completed")
            {
                status = true;
            }
            return status;
        }
        private void Export(object sender, RoutedEventArgs e)
        {
            string path = Utils.GetPath("workingproxy.txt");
            var list = MyList.ToList();
            System.IO.File.WriteAllText(path, "");

            foreach (var item in list)
            {

                StreamWriter sw = new StreamWriter(path, true);

                if (item.Password != null)
                    sw.Write(item.IP + ":" + item.Port + ":" + item.Username + ":" + item.Password);
                else
                    sw.Write(item.IP + ":" + item.Port);
                sw.Write("\n");
                sw.Dispose();

            }

            //  System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "workingproxy.txt", list.ToString());
        }

        public async Task<bool> check(String proxyURL, int port, String username, String password, String testurl) // HttpClient didn't work on some site move on to RestSharp now
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

            Ip playerList = new Ip();
            playerList.Status = status.ToString();
            MyList.Add(playerList);

            return status;
        }

        private void buttonImportProxies_Click(object sender, RoutedEventArgs e)
        {
            MyList = new ObservableCollection<Ip>();
            import.Visibility = Visibility.Visible;


        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string richText = new TextRange(proxytest.Document.ContentStart, proxytest.Document.ContentEnd).Text;
            var lines = richText.Split('\n').ToList();

            foreach (string line in lines)
            {
                if (!parseProxy(line))
                {

                    break;
                }
            }

            import.Visibility = Visibility.Hidden;
        }
        private bool parseProxy(string proxy)
        {
            string[] proxyParts = null;
            string ip;
            string port;
            try
            {
                proxyParts = proxy.Split(':');

                if (proxyParts == null || proxyParts.Length <= 1)
                    return false;

                ip = proxyParts[0];
                string replacement = Regex.Replace(proxyParts[1].ToString(), @"\t|\n|\r", "");
                port = replacement;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            string username = null;
            string password = null;

            try
            {
                if (proxyParts.Length == 4)
                {
                    username = proxyParts[2];
                    string replacement = Regex.Replace(proxyParts[3].ToString(), @"\t|\n|\r", "");
                    password = replacement;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Ip playerList = new Ip();
            playerList.IP = ip;
            playerList.Port = port;
            playerList.Username = username;
            playerList.Password = password;
            MyList.Add(playerList);
            dataGridProxies.ItemsSource = MyList;
            return true;

        }


        private void buttonRemoveFalse_Click(object sender, RoutedEventArgs e)
        {
            var list = MyList.ToList();
            var change = new ObservableCollection<Ip>();
            bool isnull = false;
            for (var x = 0; x < list.Count; x++)
            {
                if (list.Count == 0)
                {
                    MessageBox.Show("Mast check before you can remove");
                    isnull = true;
                    break;
                }
                if (list[x].Status == "Bad")
                    continue;
                else
                    change.Add(list[x]);
            }
            if (!isnull)
            {
                MyList = change;
                dataGridProxies.ItemsSource = MyList;
                IPList.Clear();
            }

        }

        private void proxyCheckWorker()
        {
            string richText = new TextRange(proxytest.Document.ContentStart, proxytest.Document.ContentEnd).Text;
            var lines = richText.Split('\r', '\n').ToList();

            if (string.IsNullOrEmpty(url.Text))
            {
                MessageBox.Show("Must enter url to test");
                return;
            }

            ProxyCheck proxyCheck = new ProxyCheck();
            proxyCheck.TestURL = url.Text;

            foreach (var line in lines) // read text line by line
            {
                // Only process lines with information!
                if (string.IsNullOrEmpty(line))
                    continue;

                var linenumber = line.Split(':').ToList();  // 4  is user with pass word 2 just ip + port

                proxyCheck.ProxyURL = linenumber[0];

                if (linenumber.Count == 2)
                {
                    string replacement = Regex.Replace(linenumber[1].ToString(), @"\t|\n|\r", "");
                    int port = 0;

                    if (!int.TryParse(replacement, out port))
                    {
                        Console.WriteLine("Failed to parse port number \"{0}\"", replacement);
                        continue;
                    }

                    proxyCheck.Port = port;
                    proxyCheck.Username = null;
                    proxyCheck.Password = null;
                }

                else if (linenumber.Count == 4)
                {
                    string replacement = Regex.Replace(linenumber[1].ToString(), @"\t|\n|\r", ""); // fixed the error replacement was now linenumber[3]
                    int port = 0;

                    if (!int.TryParse(replacement, out port))
                    {
                        Console.WriteLine("Failed to parse port number \"{0}\"", replacement);
                        continue;
                    }

                    proxyCheck.Port = port;
                    proxyCheck.Username = linenumber[2].ToString();
                    proxyCheck.Password = linenumber[3].ToString(); // was set to port
                }

                else
                {
                    continue;
                }

                long elapsedMS = 0;
                bool status = proxyCheck.check(out elapsedMS);

                if (status)
                    IPList.Add(elapsedMS + "ms");
                else
                    IPList.Add("Bad");
            }

        }

        private void Test(object sender, RoutedEventArgs e)
        {
            // Test code test web scrapper with Foot Locker.
            if (true)
            {
                // Out of stock example as of 5/31/19 @20:09
                // Product product = new Product("https://www.footlocker.com/product/model/nike-lebron-16-mens/299649.html", "Lebron 16", 7, "default");

                // In stock example as of 5/31/19 @20:09
                 Product product = new Product("https://www.footlocker.com/product/model/nike-lebron-16-mens/299649.html", "Lebron 16", 8, "default");

                // Out of stock example as of 5/31/19 @23:16 
              //  Product product = new Product("https://www.footlocker.com/product/nike-lebron-16-mens/862001.html", "Lebron 16", 7, "default");

                // In of stock example as of 5/31/19 @23:16 
                // Product product = new Product("https://www.footlocker.com/product/nike-lebron-16-mens/862001.html", "Lebron 16", 8.5F, "default");

                IWebScrapper webScrapper = new FootlockerWebScrapper(product);

                bool result = webScrapper.Available();

                Console.WriteLine("webScrapper result: " + result);

                return;
            }

            proxyCheckWorker();

            var list = MyList.ToList();

            // Only track changes if the lists match, otherwise an out of bounds exception will be thrown!
            if (list.Count == IPList.Count)
            {
                var change = new ObservableCollection<Ip>();
                for (var x = 0; x < list.Count; x++)
                {

                    list[x].Status = IPList[x];
                    change.Add(list[x]);
                }

                MyList = change;
                dataGridProxies.ItemsSource = MyList;
                IPList.Clear();
            }
        }

        private class ProxyCheck
        {

            /// <summary>
            /// Default constructor.
            /// </summary>
            public ProxyCheck() : this(null, -1, null, null, null)
            {

            }

            public ProxyCheck(string proxyURL, int port, string username, string password, string testURL)
            {
                ProxyURL = proxyURL;
                Port = port;
                Username = username;
                Password = password;
                TestURL = testURL;
            }

            public string ProxyURL
            {
                get; set;
            }

            public int Port
            {
                get; set;
            }

            public string Username
            {
                get; set;
            }

            public string Password
            {
                get; set;
            }

            public string TestURL
            {
                get; set;
            }

            public bool check(out long elapsedMS)
            {
                bool status = false;
                RestClient client = new RestClient();

                //var client = new RestClient(TestURL);
                client.Proxy = new WebProxy("http://" + ProxyURL + ":" + Port, false);
                Console.WriteLine("http://" + ProxyURL + ":" + Port);
                if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                    client.Proxy.Credentials = new NetworkCredential(Username, Password);

                Console.WriteLine(Username + ":" + Password);
                Stopwatch s = new Stopwatch();
                s.Start();

                var response = client.Execute(new RestRequest(TestURL));

                s.Stop();
                elapsedMS = s.ElapsedMilliseconds;

                if (response.IsSuccessful)
                {
                    status = true;
                }

                return status;
            }

        }

    }
}

