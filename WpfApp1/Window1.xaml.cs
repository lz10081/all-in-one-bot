﻿using System;
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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Windows.Navigation;
using System.IO;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using static WpfApp1.profilepage;
using Newtonsoft.Json;
using ZenAIO;
using System.Text.RegularExpressions;
using System.Globalization;
using RestSharp;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace WpfApp1
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class Window1 : Window
    {

        private ObservableCollection<JobTask> MyList;
        private List<Int32> savelist;
        private List<String> proxylist;
        public Window1()
        {
            InitializeComponent();

            MyList = new ObservableCollection<JobTask>();
            savelist = new List<Int32>();
            proxylist = new List<String>();
            Main.Content = new profilepage();

            if (false)
            {
                DirectoryInfo d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*.json"); //Getting Text files
                string str = "";
                foreach (FileInfo file in Files)
                {
                    str = str + ", " + file.Name;

                    //Profile.Items.Add(file.Name);
                }
            }
            Additem();
            // add all the site we support
            Sitelist.Items.Add("Footlocker");

            string path = Utils.GetPath("Profile.json");
            string path2 = Utils.GetPath("proxy.txt");
            // Checks if a profile exists or not. If not, will create a default profile.
            // Else (does exist) prompts us to read the file in like normal.
            if (CreateDefaultProfile(ref path))
                ReadProfile(ref path);

            if (CreateDefaultProfile(ref path2)) //  for proxylist
                ReadProfile(ref path2);

            #region dataGridProxies Settings
            dataGridProxies.ItemsSource = MyList;

            dataGridProxies.AutoGenerateColumns = false;
            dataGridProxies.IsReadOnly = true;
            dataGridProxies.SelectionMode = DataGridSelectionMode.Single;
            DataGridTextColumn c = new DataGridTextColumn();
            DataGridTemplateColumn x = new DataGridTemplateColumn();
            c = new DataGridTextColumn();
            c.Header = "#";
            c.Binding = new Binding("ID");
            c.Width = new DataGridLength(0.2, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Site";
            c.Binding = new Binding("Site");
            c.Width = new DataGridLength(0.5, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Size";
            c.Binding = new Binding("Size");
            c.Width = new DataGridLength(0.5, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Product";
            c.Binding = new Binding("Product");
            c.Width = new DataGridLength(0.5, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Billing";
            c.Binding = new Binding("Billing");
            c.Width = new DataGridLength(0.5, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Proxy";
            c.Binding = new Binding("Proxy");
            c.Width = new DataGridLength(0.5, DataGridLengthUnitType.Star);
            dataGridProxies.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Status";
            c.Binding = new Binding("Status");
            c.Width = new DataGridLength(0.3, DataGridLengthUnitType.Star);

            dataGridProxies.Columns.Add(c);

            x = new DataGridTemplateColumn();
            x.Width = new DataGridLength(0.3, DataGridLengthUnitType.Star);
            DataTemplate buttonTemplate = new DataTemplate();
            FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
            buttonTemplate.VisualTree = buttonFactory;
            buttonFactory.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(TaskStartClick));
            buttonFactory.SetValue(ContentProperty, "Start");
            x.CellTemplate = buttonTemplate;
            // x.CellTemplate.Template = t1.Template;
            dataGridProxies.Columns.Add(x);

            x = new DataGridTemplateColumn();
            x.Width = new DataGridLength(0.3, DataGridLengthUnitType.Star);
            DataTemplate buttonTemplate2 = new DataTemplate();
            FrameworkElementFactory buttonFactory2 = new FrameworkElementFactory(typeof(Button));
            buttonTemplate2.VisualTree = buttonFactory2;
            buttonFactory2.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(TaskStopClick));
            buttonFactory2.SetValue(ContentProperty, "Stop");
            x.CellTemplate = buttonTemplate2;
            // x.CellTemplate.Template = t1.Template;
            dataGridProxies.Columns.Add(x);

            x = new DataGridTemplateColumn();
            x.Width = new DataGridLength(0.3, DataGridLengthUnitType.Star);
            DataTemplate buttonTemplate3 = new DataTemplate();
            FrameworkElementFactory buttonFactory3 = new FrameworkElementFactory(typeof(Button));
            buttonTemplate3.VisualTree = buttonFactory3;
            buttonFactory3.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(TaskRemoveClick));
            buttonFactory3.SetValue(ContentProperty, "Remove");
            x.CellTemplate = buttonTemplate3;
            dataGridProxies.Columns.Add(x);
            #endregion

        }
        public class JobTask
        {
            public string ID { get; set; }
            public string Site { get; set; }
            public string Product { get; set; }
            public string Billing { get; set; }
            public string Proxy { get; set; }
            public string Status { get; set; }
            public string Acotion { get; set; }
            public string Size { get; set; }

        }

        private void ReadProfile(ref string path)
        {
            var lines = File.ReadAllLines(path);
            //var playerList = JsonConvert.DeserializeObject<List<NotusdataWship>>(json);
            foreach (var line in lines)
            {
                // I'm not sure what this is doing
                if (line.Contains("1type"))
                {
                    var playerList = JsonConvert.DeserializeObject<List<DataWithoutShip>>(line);
                    Profile.Items.Add(playerList[0].nanme);
                }
                else if (line.Contains("2type"))
                {
                    var playerList = JsonConvert.DeserializeObject<List<DataWithShip>>(line);
                    Profile.Items.Add(playerList[0].nanme);
                }
                else if (line.Contains("3type"))
                {
                    var playerList = JsonConvert.DeserializeObject<List<NonUSDataWithoutShip>>(line);
                    Profile.Items.Add(playerList[0].nanme);
                }
                else if (line.Contains("4type"))
                {
                    var playerList = JsonConvert.DeserializeObject<List<NotUSDataWithShip>>(line);
                    Profile.Items.Add(playerList[0].nanme);
                }
                else
                {

                    proxylist.Add(line);
                }

            }
        }
        private void Additem()
        {


            for (var a = 3; a <= 18; a++)
            {
                if (a < 13)
                {
                    size.Items.Add(a);
                    size.Items.Add(a + 0.5);
                    startsize.Items.Add(a);
                    startsize.Items.Add(a + 0.5);
                    endsize.Items.Add(a);
                    endsize.Items.Add(a + 0.5);
                }
                else
                {
                    size.Items.Add(a);
                    startsize.Items.Add(a);
                    endsize.Items.Add(a);
                }

            }
            size.Items.Add("Random");
        }

        private void Removeall(object sender, RoutedEventArgs e)
        {
            ObservableCollection<JobTask> Em = new ObservableCollection<JobTask>();
            List<Int32> em = new List<int>();
            MyList = Em;
            savelist = em;
            dataGridProxies.ItemsSource = MyList;
        }
        private void Stopall(object sender, RoutedEventArgs e)
        {
            savelist.Sort();
            for (int a = 0; a < savelist.Count; a++)
            {
                int index = savelist[a];

                var item = MyList.FirstOrDefault(X => X.ID == (index - 1).ToString());

                if (item != null)
                {
                    item.Status = "Stoped";


                }

                try
                {
                    MyList.Remove(MyList.Where(i => i.ID == (index - 1).ToString()).Single());
                    MyList.Insert((index - 1), item);
                }
                catch
                {

                    Debug.Error("unknow error");
                }
            }
        }
        private void Startall(object sender, RoutedEventArgs e)
        {
            savelist.Sort();
            for (int a = 0; a < savelist.Count; a++)
            {
                int index = savelist[a];

                var item = MyList.FirstOrDefault(X => X.ID == (index - 1).ToString());

                if (item != null)
                {
                    item.Status = "Started";


                }

                try
                {
                    MyList.Remove(MyList.Where(i => i.ID == (index - 1).ToString()).Single());
                    MyList.Insert((index - 1), item);
                }
                catch
                {

                    Debug.Error("unknow error");
                }
            }
        }

        private class WebTask : ITask
        {
            private readonly object theLock = new object();
            private ObservableCollection<JobTask> MyList;
            private int index;
            private int current;
            private DataGrid dataGridProxies;
            private JobTask item;

            public WebTask(Product product, ref ObservableCollection<JobTask> myList, int index, int current, ref DataGrid dataGridProxies, ref JobTask item)
            {
                TheProduct = product;
                MyList = myList;
                this.index = index;
                this.current = current;
                this.dataGridProxies = dataGridProxies;
                this.item = item;
            }

            public Product TheProduct
            {
                get;
                private set;
            }

            public bool Completed
            {
                get;
                private set;
            }

            public void Run()
            {
                IWebScrapper webScrapper = new FootlockerWebScrapper(TheProduct);

                string proxyUsed; // use this variable to get the proxy used...
                bool result = webScrapper.Available(out proxyUsed);
                Debug.Info("Proxy used: " + proxyUsed);

             
            }

            public void Callback()
            {
                // var found = MyList.FirstOrDefault(X => X.ID == (index - 1).ToString());
                try
                {
                    // Debug.Info("webScrapper result: " + result);

                    MyList.Remove(MyList.Where(i => i.ID == (index - 1).ToString()).Single());
                    MyList.Insert((current), item);
                    
                }
                catch
                {
                    // Console.WriteLine(x);
                    Debug.Error("unknow error");
                }


                // MyList.Insert(MyList.IndexOf(q), item);
                //MyList.Remove(q);
                //   MyList.IndexOf(); need to fixed the id first
                // Debug.Info(x.ToString());
                //Debug.Info(MyList[i].Status);
              //  dataGridProxies.ItemsSource = MyList;
            }

        }

        // TODO: Convert tasks to an ITask object and insert them into the TaskManager
        private void TaskStartClick(object sender, RoutedEventArgs e)
        {
            savelist.Sort();
            // Get the currently selected row using the SelectedRow property.
            var x = dataGridProxies.SelectedIndex; // get the current index number
            int index = savelist[x];
            int current = savelist.IndexOf(index);
            //  var q = MyList.Where(X => X.ID == x.ToString()).FirstOrDefault(); // can get all the information on the index
            var item = MyList.FirstOrDefault(X => X.ID == (index - 1).ToString());
            //  Debug.Info(item.ID);
            if (item != null)
            {
                item.Status = "Started";
                //  Debug.Info(item.Status);

            }

            Debug.Info("Product???" + item.Product); // didn't show the proxy info idk y
                                                     //Console.WriteLine("wtf"+item.Proxy);
            Product product = new Product(item.Product, "TODO:GET PRODUCT NAME", int.Parse(item.ID), float.Parse(item.Size, CultureInfo.InvariantCulture.NumberFormat), item.Billing, item.Proxy);

            WebTask webTask = new WebTask(product, ref MyList, index, current, ref dataGridProxies, ref item);
            TaskManager.Instance.Add(product.Name, webTask);

            //// var found = MyList.FirstOrDefault(X => X.ID == (index - 1).ToString());
            //try
            //{
            //    // Debug.Info("webScrapper result: " + result);

            //    MyList.Remove(MyList.Where(i => i.ID == (index - 1).ToString()).Single());
            //    MyList.Insert((current), item);
            //}
            //catch
            //{
            //    // Console.WriteLine(x);
            //    Debug.Error("unknow error");
            //}


            //// MyList.Insert(MyList.IndexOf(q), item);
            ////MyList.Remove(q);
            ////   MyList.IndexOf(); need to fixed the id first
            //// Debug.Info(x.ToString());
            ////Debug.Info(MyList[i].Status);
            //dataGridProxies.ItemsSource = MyList;

        }

        private void TaskStopClick(object sender, RoutedEventArgs e)
        {
            WebRequest();
            savelist.Sort();
            var x = dataGridProxies.SelectedIndex; // get the current index number
            int index = savelist[x];
            int current = savelist.IndexOf(index);
            // Console.WriteLine("here"+savelist.IndexOf(index));
            //  Console.WriteLine(index);
            var item = MyList.FirstOrDefault(X => X.ID == (index - 1).ToString());
            // Console.WriteLine(item.ID);
            if (item != null)
            {
                item.Status = "Stoped";


            }

            try
            {
              
                MyList.Remove(MyList.Where(i => i.ID == (index - 1).ToString()).Single());
                //savelist.Remove(current);
                MyList.Insert((current), item);
                // savelist.Insert(index, index);
            }
            catch
            {

                Debug.Error("unknow error");
            }
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        protected class Payload
        {
            [JsonProperty("pid")]

            public string PID { get; set; }
            [JsonProperty("qty")]
            public string qty { get; set; }
        }
        public  void SendPurchaseReques()
        {
            var payload = new Payload
            {
                PID = "34180539",
                qty = "1"  //https://www.footlocker.com/product/Nike-LeBron-16---Men-s/I1521001.html
            };

            var client = new RestClient("https://www.hibbett.com/on/demandware.store/Sites-Hibbett-US-Site/default/Cart-Modal?format=ajax");
            RestRequest request = new RestRequest("/", Method.POST);
            CookieContainer cookieJar = new CookieContainer();
            request.AddHeader("authority", "www.hibbett.com");
            request.AddHeader("scheme", "https");
            request.AddHeader("accept", "/");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("accept-language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-length", "69");
            request.AddHeader("origin", "https://www.hibbett.com/");
            request.AddHeader("pragma", "no-cache");
         //   request.AddHeader("origin", "https://www.hibbett.com/");
            request.AddHeader("referer", "https://www.hibbett.com/nike-air-max-97-have-a-nike-day-mens-shoe/M0608.html?dwvar_M0608_color=0136&cgid=men-shoes-runningshoes");
           // request.AddHeader("cache-control", "max-age=0");

          //  request.AddHeader("referer", "https://www.solebox.com/en/home/");
            ///request.AddHeader("upgrade-insecure-requests", "1");

            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.80 Safari/537.36");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddParameter("products", "%5B%7B%22pid%22%3A%2234180539%22%2C%22qty%22%3A%221%22%7D%5D");
          

            // :authority: www.solebox.com
            //  :method: GET
            // : path: / en / my - account /
            //  :scheme: https
            //  accept: text / html,application / xhtml + xml,application / xml; q = 0.9,image / webp,image / apng,*/*;q=0.8
            //   accept-encoding: gzip, deflate, br
            // accept-language: en-US,en;q=0.9,zh-CN;q=0.8,zh;q=0.7
            //  cache-control: max-age=0
            //  cookie: cto_lwid=083fed1b-38d9-4fb5-ad3f-17a53f2afbf2; mlid=undefined; displayedCookiesNotification=1; __utmz=1.1557283576.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); sid_key=oxid; px-abgroup=A; px-abper=100; __utmc=1; _fbp=fb.1.1559752431107.1312876194; cto_idcpy=d836f8b2-2cbc-4ab8-b80d-410288d34136; klarnaAddressValidated=0; cto_clc=1; cto_red_atmpt=1; sid=tn45e0dgon5eanntl2av48b9v3; __utma=1.1294710284.1557283576.1559781699.1559784155.7; language=1; __utmt=1; __utmb=1.4.10.1559784155; _px3=140066c0c8488936a8164e111a449aeb7a356a16440e4932ceb97d310edc1904:viqJyp98T/Cqznzu7GNXz94EPUgdt+H1vNpiZ+mvTaKbvLGmO1Id24DAtzUR1a1tgccRD0jLH4COClA0HLwZNw==:1000:jcST2cB0RWvXbNB+lA42GYyJ4tzQ/MXnkZI/7l9PjhEqpzrFtYaWcvMMVDXMMDmPT7nHCNCsXjxmEiBNjVvVxxNFhgkain+5HHPyD/W1ilCnhyXFFsgZtxGzhIICu8cO1rTXv53prjbtadWcjVXU/G75HlhDS9kNFCJOjymyWtA=
            ///  referer: https://www.solebox.com/
            //  upgrade-insecure-requests: 1
            // user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.80 Safari/537.36

            // request.AddHeader("cookie", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.80 Safari/537.36");
            client.CookieContainer = cookieJar;
            //   request.AddHeader("cookie", "cto_lwid=083fed1b-38d9-4fb5-ad3f-17a53f2afbf2; mlid=undefined; displayedCookiesNotification=1; __utmz=1.1557283576.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); sid_key=oxid; px-abgroup=A; px-abper=100; __utmc=1; _fbp=fb.1.1559752431107.1312876194; cto_idcpy=d836f8b2-2cbc-4ab8-b80d-410288d34136; klarnaAddressValidated=0; language=1; __utma=1.1294710284.1557283576.1559758562.1559763477.5; __utmt=1; cto_clc=1; cto_red_atmpt=1; sid=2q7sg85mkhd1fe9vrd8lfraao6; __utmb=1.2.10.1559763477; _px3=b3780d91446af69c8e1d57e6583132a8ade98a4c4b34c84c515a863b6f0257ac:wCMykZK5TCSRg75bdERs3bbh+fRmA0a2GU1FPCOqeuLo8qU/YyoNIG8mG6zMP+dasN/N7MWTJxauftKvvLNV/A==:1000:G6TKS9vchd/M1Xds7ZzDBrStzFgf05Hrup3KHWXhg1qBHAbAOvz7H/2iVa8CCJ9xChfDbLSWVfIzIseD+/apmvLV66bz03QJ3my0K1hE2dwXdOdsGmfy9FP3yjD+KQYIGk2TbGtNDiQccfXkajWtMBJFv0mj1tdR0UCEknT4R8M=");
            var response = client.Execute(request);

            Debug.Info("response.StatusCode: " + ((int)response.StatusCode));
            //  result = response.Content;
            Debug.Info("Content: \n{0}", response.Content);

        }
        private static void qWebRequest()
        {
           
            var client = new RestClient("https://www.solebox.com/mein-konto/");
            var request = new RestRequest(Method.GET);
          //  request.AddHeader("authority", "www.solebox.com");
         //   request.AddHeader("scheme", "https");
           // request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
           // request.AddHeader("accept-encoding", "gzip, deflate, br");
          //  request.AddHeader("accept-language", "en-US,en;q=0.9,zh-CN;q=0.8,zh;q=0.7");
           // request.AddHeader("cache-control", "max-age=0");
           // request.AddHeader("referer", "https://google.com/");
           // request.AddHeader("upgrade-insecure-requests", "1");
            request.AddHeader("User-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
 
            var response = client.Execute(request);
            

            Debug.Info("response.StatusCode: " + ((int)response.StatusCode));
            Debug.Info("Content: \n{0}", response.Content);
        }
        private static void WebRequest()

            
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            var option = new ChromeOptions();
            option.AddArguments( "--no-sandbox", "--disable-web-security", "--disable-gpu", "--incognito", "--proxy-bypass-list=*", "--proxy-server='direct://'", "--log-level=3", "--hide-scrollbars"); //"--headless" onlu use if after test is done
            ChromeDriver driver = new ChromeDriver(chromeDriverService, option);


            //Navigate to google page
            driver.Navigate().GoToUrl("https://www.eastbay.com/");

            //Maximize the window
            //driver.Manage().Window.Maximize();

            //Find the Search text box using xpath
           // IWebElement element = driver.FindElement(By.XPath("//*[@title='Search']"));

            //Enter some text in search text box
           // element.SendKeys("learn-automation");

            //Close the browser
          //  driver.Close();

            //var html = @"https://www.solebox.com/mein-konto/";

            //HtmlWeb web = new HtmlWeb();

            // var htmlDoc = web.Load(html);

            //var node = htmlDoc.DocumentNode.SelectSingleNode("//head/title");

            // Console.WriteLine("Node Name: " + node.Name + "\n" + node.OuterHtml);

        }

        private void TaskRemoveClick(object sender, RoutedEventArgs e)
        {
            
            savelist.Sort();
            // (savelist).ForEach(Console.WriteLine);
            var x = dataGridProxies.SelectedIndex; // get the current index number
            int index = savelist[x];

            try
            {
                // Console.WriteLine("this is inderx" + index);
                //  Console.WriteLine("this is x" + x);
                MyList.Remove(MyList.Where(i => i.ID == (index - 1).ToString()).Single());

                savelist.Remove(index);
                // Console.WriteLine("///////////////////");
                //   (savelist).ForEach(Console.WriteLine);
                //  Console.WriteLine(index);
            }
            catch
            {
                // Console.WriteLine(x);
                Debug.Error("unknow error");
            }
        }

        private void mouse(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void TasksClicked(object sender, RoutedEventArgs e)
        {
            Main.NavigationService.Navigate(new taskpage());
            // Main.Content = new taskpage();
        }

        private void ProfilesClicked(object sender, RoutedEventArgs e)
        {
            Main.Content = new profilepage();
        }

        private void ProxyClicked(object sender, RoutedEventArgs e)
        {
            Main.Content = new proxypage();
        }

        private void CoppedClicked(object sender, RoutedEventArgs e)
        {
            Main.Content = new coppedpage();
        }

        private void ViewClicked(object sender, RoutedEventArgs e)
        {
            Main.Content = new viewpage();
        }

        private void SettingClicked(object sender, RoutedEventArgs e)
        {
            Main.Content = new Settingpage();
        }

        private void MinusClicked(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }
        /// <summary>
        /// Checks if all inputbox exists, if not
        /// show MessageBox
        /// </summary>
        private void Start(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(url.Text) || string.IsNullOrWhiteSpace(Sitelist.Text))
                MessageBox.Show("You must enter all information");
            else if (SizeruncheckBox.IsChecked == true && string.IsNullOrWhiteSpace(startsize.Text) || SizeruncheckBox.IsChecked == true && string.IsNullOrWhiteSpace(endsize.Text))
                MessageBox.Show("You must enter start size and end size");
            else if (SizeruncheckBox.IsChecked == true && (Double.Parse(startsize.Text) >= Double.Parse(endsize.Text)))
                MessageBox.Show("Start size must less than end size ");
            else if (SizeruncheckBox.IsChecked == true && string.IsNullOrWhiteSpace(Quantity2.Text))
                MessageBox.Show("You enter quantity ");
            else if (SizeruncheckBox.IsChecked == false && string.IsNullOrWhiteSpace(Quantity.Text))
                MessageBox.Show("You enter quantity");
            else if (SizeruncheckBox.IsChecked == false && string.IsNullOrWhiteSpace(size.Text))
                MessageBox.Show("You enter size");
            else if (UseallcheckBox.IsChecked == false && string.IsNullOrWhiteSpace(Profile.Text))
                MessageBox.Show("You must enter your profile information");
            else
            {
                if (Sitelist.Text == "Footlocker")
                    Footlocker();
            }

        }
        /// <summary>
        /// Enabled Profile checkbox if IsChecked , if not
        /// disable
        /// </summary>
        private void Useall(object sender, RoutedEventArgs e)
        {
            if (UseallcheckBox?.IsChecked == true)
            {
                Profile.IsEnabled = false;

            }
            else
            {

                Profile.IsEnabled = true;
            }

        }
        private void Sizebox(object sender, RoutedEventArgs e)
        {
            if (SizeruncheckBox?.IsChecked == true)
            {
                startsize.IsEnabled = true;
                endsize.IsEnabled = true;
                Quantity2.IsEnabled = true;
                size.IsEnabled = false;
                Quantity.IsEnabled = false;
            }

            else
            {
                startsize.IsEnabled = false;
                endsize.IsEnabled = false;
                Quantity2.IsEnabled = false;
                size.IsEnabled = true;
                Quantity.IsEnabled = true;
            }

        }

        public void Footlocker() // will make a other cs file for each site 
        {
            string path = Utils.GetPath("proxy.txt");

            if (new FileInfo(path).Length == 0)
            {
                Console.WriteLine("running localhost");
            }
            else
            {
                Console.WriteLine("running proxy");
            }
            Console.WriteLine("my proxy" + proxylist.Count);
            int quantity = 0;
            if (SizeruncheckBox.IsChecked == false) // easy case 
            {
                /// <summary>
                /// normal case no use all profile and use range box
                /// </summary>
                /// 
                bool t = false;
                if (Localhost.IsChecked == true || (new FileInfo(path).Length == 0))
                    t = true;
                Random rand = new Random();
                if (int.TryParse(Quantity.Text, out quantity) && quantity > 0 && UseallcheckBox.IsChecked == false) // easy case 
                {
                    for (var x = 1; x <= quantity; x++)
                    {


                        int nextVal = rand.Next(proxylist.Count);

                        JobTask playerList = new JobTask();
                        playerList.ID = MyList.Count.ToString();
                        playerList.Product = url.Text;
                        playerList.Site = Sitelist.Text;
                        playerList.Status = "";
                        if (t)
                            playerList.Proxy = "Localhost";
                        else
                            playerList.Proxy = proxylist[nextVal];
                        playerList.Billing = Profile.Text;
                        playerList.Size = size.Text;
                        MyList.Add(playerList);
                        savelist.Add(MyList.Count);


                    }

                    dataGridProxies.ItemsSource = MyList;
                }
                /// <summary>
                ///  case with use all profile not use range box
                /// </summary>
                else if (quantity > 0 && UseallcheckBox.IsChecked == true)
                {
                    int Profilesize = Profile.Items.Count;
                    for (var y = 0; y <= Profilesize - 1; y++)
                    {
                        for (var x = 1; x <= quantity; x++)
                        {
                            int nextVal = rand.Next(proxylist.Count);
                            JobTask playerList = new JobTask();
                            playerList.ID = MyList.Count.ToString();
                            playerList.Product = url.Text;
                            playerList.Site = Sitelist.Text;
                            playerList.Status = "";
                            if (t)
                                playerList.Proxy = "Localhost";
                            else
                                playerList.Proxy = proxylist[nextVal];
                            playerList.Billing = Profile.Items[y].ToString();
                            playerList.Size = size.Text;
                            MyList.Add(playerList);
                            savelist.Add(MyList.Count);


                        }
                    }

                }
            }
            else if (SizeruncheckBox.IsChecked == true)
            {
                /// <summary>
                ///  case not use all profile  use range box
                /// </summary>
                /// 

                bool t = false;
                if (Localhost.IsChecked == true || (new FileInfo(path).Length == 0))
                    t = true;
                Random rand = new Random();

                if (quantity > 0 && UseallcheckBox.IsChecked == false) // hard case 1
                {


                    // int count = 1;
                    Double currentsize = Double.Parse(startsize.Text);
                    while (currentsize - 0.5 != Double.Parse(endsize.Text))
                    {
                        for (var x = 1; x <= quantity; x++)
                        {
                            int nextVal = rand.Next(proxylist.Count);
                            JobTask playerList = new JobTask();
                            playerList.ID = MyList.Count.ToString();
                            playerList.Product = url.Text;
                            playerList.Site = Sitelist.Text;
                            playerList.Status = "";
                            if (t)
                                playerList.Proxy = "Localhost";
                            else
                                playerList.Proxy = proxylist[nextVal];
                            playerList.Billing = Profile.Text;
                            playerList.Size = currentsize.ToString();
                            MyList.Add(playerList);
                            savelist.Add(MyList.Count);


                        }
                        currentsize = currentsize + 0.5;

                    }
                    //Debug.Info(count.ToString());

                    dataGridProxies.ItemsSource = MyList;
                }
                /// <summary>
                ///  case use all profile  use range box
                /// </summary>
                /// 
                else if (quantity > 0 && UseallcheckBox.IsChecked == true)
                {
                    int Profilesize = Profile.Items.Count;
                    for (var y = 0; y <= Profilesize - 1; y++)
                    {
                        Double currentsize = Double.Parse(startsize.Text);
                        while (currentsize - 0.5 != Double.Parse(endsize.Text))
                        {
                            for (var x = 1; x <= quantity; x++)
                            {
                                int nextVal = rand.Next(proxylist.Count);
                                JobTask playerList = new JobTask();
                                playerList.ID = MyList.Count.ToString();
                                playerList.Product = url.Text;
                                playerList.Site = Sitelist.Text;
                                playerList.Status = "";
                                if (t)
                                    playerList.Proxy = "Localhost";
                                else
                                    playerList.Proxy = proxylist[nextVal];
                                playerList.Billing = Profile.Items[y].ToString();
                                playerList.Size = currentsize.ToString();
                                MyList.Add(playerList);
                                savelist.Add(MyList.Count);


                            }
                            currentsize = currentsize + 0.5;

                        }
                    }
                }

            }
            // Only run this code if Quantity.Text is a valid parse.


            //https://stackoverflow.com/questions/7198005/c-sharp-httpwebrequest-website-sign-in set up CookieContainer 

        }

        /// <summary>
        /// Checks if profile exists, if not
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool CreateDefaultProfile(ref string path)
        {
            if (!File.Exists(path))
            {
                using (FileStream stream = File.Create(path))
                {
                    stream.Close();
                }

                // After we create the file, we need to fill it with default data
                // or ask the user to fill it in for us.

                return false;
            }

            return true;
        }
    }
}
