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

namespace WpfApp1
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            Main.Content = new taskpage();
        }
        private void TasksClicked(object sender, RoutedEventArgs e)
        {
           Main.Content = new taskpage();
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
    }     
}
