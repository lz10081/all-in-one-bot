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
            TextRange range;
            FileStream fStream;
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\" + "account.txt"))
            {
                range = new TextRange(account.Document.ContentStart, account.Document.ContentEnd);
                fStream = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\" + "account.txt", System.IO.FileMode.OpenOrCreate);
                range.Load(fStream, System.Windows.DataFormats.Text);
                fStream.Close();
            }


        }

        private void Save(object sender, RoutedEventArgs e)
        {
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\"  + "discord.txt", hook.Text);
        }
        private void Remove(object sender, RoutedEventArgs e)
        {
            TextRange range;
            range = new TextRange(account.Document.ContentStart, account.Document.ContentEnd);
            range.Text = "";
        }
        private void Saveaccount(object sender, RoutedEventArgs e)

        {
            string richText = new TextRange(account.Document.ContentStart, account.Document.ContentEnd).Text;
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "account.txt", richText);
        }
        private void Test(object sender, RoutedEventArgs e)
        {
            try {
                DiscordWebhookClient s = new DiscordWebhookClient(hook.Text);
                var embed = new EmbedBuilder
                {
                    Title = "Zen Aio Webhook Test Results",
                    Description = "Testing.."

                };
                embed.AddField("Product",
            "Yeezy 350 V2")
          .WithFooter(footer => footer.Text = "Zen Aio")
           //.WithFooter(footer => )
           .WithColor(Discord.Color.Blue)
           .WithCurrentTimestamp()
           .WithImageUrl("https://www.flightclub.com/media/catalog/product/cache/1/image/1600x1140/9df78eab33525d08d6e5fb8d27136e95/2/0/201519_1.jpg")
           .Build();
                embed.Footer.IconUrl = "https://cdn.discordapp.com/attachments/561322271935823893/580919251670663168/Zen.jpg";
                s.SendMessageAsync(text: "COPPED!", embeds: new[] { embed.Build() });
            }
            catch
            {
                MessageBox.Show("Error on the webhook");
            }
           
      
        }
      


    }
}
