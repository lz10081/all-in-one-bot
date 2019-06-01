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
using System.Text.RegularExpressions;

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

        public Window1()
        {
            InitializeComponent();

            MyList = new ObservableCollection<JobTask>();
            savelist = new List<Int32>();

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

            // add all the site we support
            Sitelist.Items.Add("Footlocker");
            
            string path = Utils.GetPath("Profile.json");

            // Checks if a profile exists or not. If not, will create a default profile.
            // Else (does exist) prompts us to read the file in like normal.
            if (CreateDefaultProfile(ref path))
                ReadProfile(ref path);

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

                }

            }
        }

        private void Removeall(object sender, RoutedEventArgs e)
        {
            ObservableCollection<JobTask> Em = new ObservableCollection<JobTask>();
            List<Int32> em = new List<int>();
            MyList = Em;
            savelist = em;
            dataGridProxies.ItemsSource = MyList;
        }

        private void TaskStartClick(object sender, RoutedEventArgs e)
        {
            // Get the currently selected row using the SelectedRow property.
            var x = dataGridProxies.SelectedIndex; // get the current index number
            var q = MyList.Where(X => X.ID == x.ToString()).FirstOrDefault(); // can get all the information on the index
           

            //   MyList.IndexOf(); need to fixed the id first
            Debug.Info(x.ToString());
            Debug.Info(q.ID);
        }

        private void TaskStopClick(object sender, RoutedEventArgs e)
        {

        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
                MyList.Remove(MyList.Where(i => i.ID == (index-1).ToString()).Single());
              
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
      
        private void Start(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(url.Text) || string.IsNullOrWhiteSpace(Profile.Text) || string.IsNullOrWhiteSpace(size.Text))
                MessageBox.Show("You must enter all information");
            else
            {
                if (Sitelist.Text == "Footlocker")
                    Footlocker();
            }
            
        }
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

            int quantity = 0;

            // Only run this code if Quantity.Text is a valid parse.
            if (int.TryParse(Quantity.Text, out quantity) && quantity > 0)
            {
                for (var x = 1; x <= quantity; x++)
                {
                    JobTask playerList = new JobTask();
                    playerList.ID = MyList.Count.ToString();
                    playerList.Product = url.Text;
                    playerList.Site = "Footlocker";
                    playerList.Status = "";
                    playerList.Proxy = "12135489978";
                    playerList.Billing = Profile.Text;
                    playerList.Size = size.Text;
                    MyList.Add(playerList);
                    savelist.Add(MyList.Count);


                }

                dataGridProxies.ItemsSource = MyList;
            }

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
