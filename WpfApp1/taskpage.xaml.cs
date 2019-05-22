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
            Soleboxmain();
        }
        public void Soleboxmain()
        {
            //https://stackoverflow.com/questions/7198005/c-sharp-httpwebrequest-website-sign-in set up CookieContainer 

        }
    }
}
