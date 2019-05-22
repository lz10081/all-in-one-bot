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
using Discord;
using Discord.Webhook;
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Settingpage.xaml 的交互逻辑
    /// </summary>
    public partial class Settingpage : Page
    {
        public Settingpage()
        {
            InitializeComponent();
            if(File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\" + "discord.txt"))
            {
                string text = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "discord.txt");
                hook.Text = text;
            }
           
        }
      
        private void Save(object sender, RoutedEventArgs e)
        {
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\"  + "discord.txt", hook.Text);
        }
        private void Remove(object sender, RoutedEventArgs e)
        {

        }
        private void Test(object sender, RoutedEventArgs e)
        {
            DiscordWebhookClient s = new DiscordWebhookClient(hook.Text);
            var embed = new EmbedBuilder
            {
                Title = "Webhook Test Results",
                Description = "Testing.."

            };
            embed.AddField("Product",
        "Yeezy 350 V2")
       .WithFooter(footer => footer.Text = "Zen Aio")
       .WithColor(Discord.Color.Blue)
       .WithCurrentTimestamp()
       .WithImageUrl ("https://www.flightclub.com/media/catalog/product/cache/1/image/1600x1140/9df78eab33525d08d6e5fb8d27136e95/2/0/201519_1.jpg")
       .Build();

            s.SendMessageAsync(text: "COPPED!", embeds: new[] { embed.Build() });
        }
    }
}
