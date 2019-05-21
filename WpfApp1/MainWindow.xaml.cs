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
using System.Collections.Specialized;
using System.Net;
namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
         
            InitializeComponent();
        }
        void ButtonClicked(object sender, RoutedEventArgs e)
        {
            Window1 user = new Window1();

            string content = textBoxkey.Text;
            string urlAddress = "http://www.yoursite.tld/somepage.php";
            var client = new CookieAwareWebClient();
            client.Encoding = Encoding.UTF8;
            //client.CookieContainer.SetCookies(new Uri("http://example.com//dl27929"), "username=john; password=secret;");

            if (content == "bate")
            {
                user.Show();
                this.Close();
            }

            /// new to add the code to sent to backend server for the login
            
        }
        private void MinusClicked(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        public class CookieAwareWebClient : WebClient
        {
            private CookieContainer cookieContainer = new CookieContainer();

            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);
                if (request is HttpWebRequest)
                {
                    (request as HttpWebRequest).CookieContainer = cookieContainer;
                }
                return request;
            }
        }
    }
}
