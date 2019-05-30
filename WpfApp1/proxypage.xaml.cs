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

namespace WpfApp1
{
    /// <summary>
    /// proxypage.xaml 的交互逻辑
    /// </summary>
    public partial class proxypage : Page
    {
        ObservableCollection<Ip> MyList = new ObservableCollection<Ip>();
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
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "proxy.txt", richText);
        }
        private void Remove(object sender, RoutedEventArgs e)
        {
            TextRange range;
            range = new TextRange(proxytest.Document.ContentStart, proxytest.Document.ContentEnd);
            range.Text = "";
        }
        public bool check2(String proxyURL, int port, String username, String password, String testurl) // RestSharp Client
        {
            bool status = false;
            var client = new RestClient(testurl);
            client.Proxy = new WebProxy(proxyURL + ":" + port, false);
            if (username != null && password != null) client.Proxy.Credentials = new NetworkCredential(username, password);
            var response =  client.Execute(new RestRequest());
           
            Console.WriteLine(response.ResponseStatus);
            if (response.ResponseStatus.ToString() == "Completed")
            {
                status = true;
            }
              

            Ip playerList = new Ip();
            playerList.Status = status.ToString();
            MyList.Add(playerList);

            return status;
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
                ip = proxyParts[0];
                string replacement = Regex.Replace(proxyParts[1].ToString(), @"\t|\n|\r", "");
                port = replacement;
            }
            catch (Exception)
            {
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
            return true;

        }
       

        private void buttonRemoveFalse_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private  void proxyCheckWorker()
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
                    status =  check2(linenumber[0], Int32.Parse(replacement), null, null, url.Text);
                    Console.WriteLine(status);
                    addLable(status.ToString());

                }
                else if (linenumber.Count == 4)
                {

                        string replacement = Regex.Replace(linenumber[3].ToString(), @"\t|\n|\r", "");
                        bool status = false;
                        Stopwatch s = new Stopwatch();
                        s.Start();
                         status =  check2(linenumber[0], Int32.Parse(linenumber[1].ToString()), linenumber[2].ToString(), replacement, url.Text);
                    s.Stop();
                    Console.WriteLine(status);
                    Console.WriteLine(s.ElapsedMilliseconds.ToString() + "ms"
                        );
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

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

