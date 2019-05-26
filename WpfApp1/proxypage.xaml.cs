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
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.Text);
                    string replacement = Regex.Replace(linenumber[1].ToString(), @"\t|\n|\r", "");
                    WebProxy myproxy = proxysent("http://" + linenumber[0], Int32.Parse(replacement));

                    request.Proxy = myproxy;   // set proxy here
                    request.Timeout = 10000;
                    request.Method = "HEAD";
                    Stopwatch sw = Stopwatch.StartNew();
                    try
                    {
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {

                             Console.WriteLine(response.StatusCode);
                        }
                        sw.Stop();
                        //  Console.WriteLine("Request took {0}", sw.Elapsed.Milliseconds);
                    }
                    catch
                    {
                         Console.WriteLine("404");
                    }
                    // Do something.
                }
                else if (linenumber.Count == 4)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.Text);
                 
                    if(linenumber[3].ToString() != "")
                    {
                      


                        string replacement = Regex.Replace(linenumber[3].ToString(), @"\t|\n|\r", ""); 
                        WebProxy myproxy = upsent("http://" + linenumber[0].ToString(), Int32.Parse(linenumber[1].ToString()), linenumber[2].ToString(), replacement); ///"xyeb"   fixed the the reason didn't work was new line didn't get remove
                        // WebProxy myproxy = upsent("http://" + linenumber[0].ToString(), Int32.Parse(linenumber[1].ToString()), linenumber[2].ToString(), linenumber[3].ToString());

                        request.Proxy = myproxy;   // set proxy here
                        request.Timeout = 10000;
                        request.Method = "HEAD";
                        Stopwatch sw = Stopwatch.StartNew();
                        try
                        {
                            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                            {

                                Console.WriteLine(response.StatusCode );                                    // it show it the cmd now just need to think how we can show it to the user and retrun in ms 
                            }
                            sw.Stop();
                            //  Console.WriteLine("Request took {0}", sw.Elapsed.Milliseconds);
                        }
                        catch
                        {
                            Console.WriteLine("404");
                        }
                    }
                
                }
                // we want to check the type of proxy first 
                //Console.WriteLine(line);
                // access a line, evaluate it, etc.
            }

          
             
            
           
          
          
        }

        public WebProxy upsent(String proxyURL, int port, String username, String password) // set up username password proxy
        {
            //Validate proxy address
            var proxyURI = new Uri(string.Format("{0}:{1}", proxyURL, port));

            //Set credentials
            ICredentials credentials = new NetworkCredential(username, password);

            //Set proxy
            WebProxy proxy = new WebProxy(proxyURI, true, null, credentials);
            return proxy;
        }
        public WebProxy proxysent(String proxyURL, int port) // set up non  username password  proxy
        {
            //Validate proxy address
            var proxyURI = new Uri(string.Format("{0}:{1}", proxyURL, port));

            //Set proxy
            WebProxy proxy = new WebProxy(proxyURI, true, null, null);
            return proxy;
        }





    }
}

