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

namespace WpfApp1
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    /// 
    
    public partial class Window1 : Window
    {
        bool taskSolebox = false;
        ObservableCollection<JobTask> MyList = new ObservableCollection<JobTask>();
        public Window1()
        {
            InitializeComponent();
          
            Main.Content = new profilepage();
            DirectoryInfo d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.json"); //Getting Text files
            string str = "";
            foreach (FileInfo file in Files)
            {
                str = str + ", " + file.Name;

                //Profile.Items.Add(file.Name);
            }
            
            string path = Utils.GetPath("Profile.json");

            // Checks if a profile exists or not. If not, will create a default profile.
            // Else (does exist) prompts us to read the file in like normal.
            if (CreateDefaultProfile(ref path))
                ReadProfile(ref path);

            #region dataGridProxies Settings
            dataGridProxies.ItemsSource = "";
           
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

                }

            }
        }

        private void TaskStartClick(object sender, RoutedEventArgs e)
        {
           
        }
        private void TaskStopClick(object sender, RoutedEventArgs e)
        {

        }

        private void TaskRemoveClick(object sender, RoutedEventArgs e)
        {

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
        private void SoleboxClicked(object sender, RoutedEventArgs e)
        {
            taskSolebox = true;
        }
        private void Start(object sender, RoutedEventArgs e)
        {
            if (url.Text == "" || Profile.Text == "" || size.Text == "")
                MessageBox.Show("You must enter all information");
            else
            {
                if (taskSolebox)
                    Soleboxmain();
            }
            
        }
        public void Soleboxmain() // will make a other cs file for each site 

        {

            string path = GetPath("proxy.txt");

            if (new FileInfo(path).Length == 0)
            {
                Console.WriteLine("running localhost");
            }
            else
            {
                Console.WriteLine("running proxy");
            }
            JobTask playerList = new JobTask();
            var xx = 1;
            playerList.ID = xx.ToString();
            playerList.Product = url.Text;
            playerList.Site = "Solebox";
            playerList.Status = "";
            playerList.Proxy = "12135489978";
            playerList.Billing = Profile.Text;
            playerList.Size = size.Text;
            for (var x = 1; x <= Int32.Parse( Quantity.Text); x++)
            {
                
                MyList.Add(playerList);
               
                xx = xx + 1;
            }
            dataGridProxies.ItemsSource = MyList;


            //https://stackoverflow.com/questions/7198005/c-sharp-httpwebrequest-website-sign-in set up CookieContainer 

        }
        private static string GetPath(string file)
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
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
