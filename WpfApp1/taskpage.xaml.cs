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
namespace WpfApp1
{
    /// <summary>
    /// taskpage.xaml 的交互逻辑
    /// </summary>
    public partial class taskpage : Page
    {
        bool taskSolebox = false;
        public taskpage()
        {
            InitializeComponent();
           this.KeepAlive = true;
            DirectoryInfo d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.json"); //Getting Text files
            string str = "";
            foreach (FileInfo file in Files)
            {
                str = str + ", " + file.Name;

                Profile.Items.Add(file.Name);
            }
           




        }
        private void SoleboxClicked(object sender, RoutedEventArgs e)
        {
            taskSolebox = true;
        }
        private void Start(object sender, RoutedEventArgs e)
        {
            if(taskSolebox)
             Soleboxmain();
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
