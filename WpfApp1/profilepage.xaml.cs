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
using System.Globalization;
using Newtonsoft.Json;
using System.Windows.Markup;
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// profilepage.xaml 的交互逻辑
    /// </summary>
    public partial class profilepage : Page
    {
        public profilepage()
        {
            InitializeComponent();
            Counrty.ItemsSource = GetCountryList();
            State.ItemsSource = StateArray.States();
            StateB.ItemsSource = StateArray.States();
            for (var a = 1; a < 12; a++)
            {
                Month.Items.Add(a);
            }
            for (var a = 2019 ; a < 2029; a++)
            {
                Year.Items.Add(a);
            }
            DirectoryInfo d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.json"); //Getting Text files
            string str = "";
            foreach (FileInfo file in Files)
            {
                str = str + ", " + file.Name;

                PorfileSaved.Items.Add(file.Name);
            }
        }
        private void cbValueType_DropDownClosed(object sender, EventArgs e)
        {
            String s = Counrty.Text;
            if (s != "Canada" && s != "United States")
            {
                State.IsEnabled = false;
                StateB.IsEnabled = false;  
            }
            else
            {
                State.IsEnabled = true;
                StateB.IsEnabled = true;
            }    
        }

     
        static class StateArray
        {

            static List<US_State> states;

            static StateArray()
            {
                states = new List<US_State>(62);
                states.Add(new US_State("AL", "Alabama"));
                states.Add(new US_State("AK", "Alaska"));
                states.Add(new US_State("AZ", "Arizona"));
                states.Add(new US_State("AR", "Arkansas"));
                states.Add(new US_State("CA", "California"));
                states.Add(new US_State("CO", "Colorado"));
                states.Add(new US_State("CT", "Connecticut"));
                states.Add(new US_State("DE", "Delaware"));
                states.Add(new US_State("DC", "District Of Columbia"));
                states.Add(new US_State("FL", "Florida"));
                states.Add(new US_State("GA", "Georgia"));
                states.Add(new US_State("HI", "Hawaii"));
                states.Add(new US_State("ID", "Idaho"));
                states.Add(new US_State("IL", "Illinois"));
                states.Add(new US_State("IN", "Indiana"));
                states.Add(new US_State("IA", "Iowa"));
                states.Add(new US_State("KS", "Kansas"));
                states.Add(new US_State("KY", "Kentucky"));
                states.Add(new US_State("LA", "Louisiana"));
                states.Add(new US_State("ME", "Maine"));
                states.Add(new US_State("MD", "Maryland"));
                states.Add(new US_State("MA", "Massachusetts"));
                states.Add(new US_State("MI", "Michigan"));
                states.Add(new US_State("MN", "Minnesota"));
                states.Add(new US_State("MS", "Mississippi"));
                states.Add(new US_State("MO", "Missouri"));
                states.Add(new US_State("MT", "Montana"));
                states.Add(new US_State("NE", "Nebraska"));
                states.Add(new US_State("NV", "Nevada"));
                states.Add(new US_State("NH", "New Hampshire"));
                states.Add(new US_State("NJ", "New Jersey"));
                states.Add(new US_State("NM", "New Mexico"));
                states.Add(new US_State("NY", "New York"));
                states.Add(new US_State("NC", "North Carolina"));
                states.Add(new US_State("ND", "North Dakota"));
                states.Add(new US_State("OH", "Ohio"));
                states.Add(new US_State("OK", "Oklahoma"));
                states.Add(new US_State("OR", "Oregon"));
                states.Add(new US_State("PA", "Pennsylvania"));
                states.Add(new US_State("RI", "Rhode Island"));
                states.Add(new US_State("SC", "South Carolina"));
                states.Add(new US_State("SD", "South Dakota"));
                states.Add(new US_State("TN", "Tennessee"));
                states.Add(new US_State("TX", "Texas"));
                states.Add(new US_State("UT", "Utah"));
                states.Add(new US_State("VT", "Vermont"));
                states.Add(new US_State("VA", "Virginia"));
                states.Add(new US_State("WA", "Washington"));
                states.Add(new US_State("WV", "West Virginia"));
                states.Add(new US_State("WI", "Wisconsin"));
                states.Add(new US_State("WY", "Wyoming"));
                states.Add(new US_State("CC", "—Canada—"));
                /// canada
                states.Add(new US_State("AB", "Alberta"));
                states.Add(new US_State("BC", "British Columbia"));
                states.Add(new US_State("MB", "Manitoba"));
                states.Add(new US_State("NB", "New Brunswick"));
                states.Add(new US_State("NL", "Newfoundland and Labrador"));
                states.Add(new US_State("NT", "Northwest Territories"));
                states.Add(new US_State("ON", "Ontario"));
                states.Add(new US_State("PE", "Prince Edward Island"));
                states.Add(new US_State("QC", "Quebec"));
                states.Add(new US_State("SK", "Saskatchewan"));
                states.Add(new US_State("YT", "Yukon"));
               
            }

            public static string[] Abbreviations()
            {
                List<string> abbrevList = new List<string>(states.Count);
                foreach (var state in states)
                {
                    abbrevList.Add(state.Abbreviations);
                }
                return abbrevList.ToArray();
            }

            public static string[] Names()
            {
                List<string> nameList = new List<string>(states.Count);
                foreach (var state in states)
                {
                    nameList.Add(state.Name);
                }
                return nameList.ToArray();
            }

            public static US_State[] States()
            {
                return states.ToArray();
            }

        }

        class US_State
        {

            public US_State()
            {
                Name = null;
                Abbreviations = null;
            }

            public US_State(string ab, string name)
            {
                Name = name;
                Abbreviations = ab;
            }

            public string Name { get; set; }

            public string Abbreviations { get; set; }

            public override string ToString()
            {
                return string.Format("{0} - {1}", Abbreviations, Name);
            }

        }
        private void Boxclick(object sender, RoutedEventArgs e)
        {
            if ((bool)checkBox.IsChecked == true)
            {
                FirstB.IsEnabled = false;
                LastB.IsEnabled = false;
                AptB.IsEnabled = false;
                CityB.IsEnabled = false;
                StateB.IsEnabled = false;
                ZipB.IsEnabled = false;
                AddressB.IsEnabled = false;
            }
            else
            {
                FirstB.IsEnabled = true;
                LastB.IsEnabled = true;
                AptB.IsEnabled = true;
                CityB.IsEnabled = true;
                StateB.IsEnabled = true;
                ZipB.IsEnabled = true;
                AddressB.IsEnabled = true;
            }


        }
        // userprofile 
        public class dataWship
        {
            public string dCounrty { get; set; }
            public string dFirst { get; set; }
            public string dLast { get; set; }
            public string dAddress { get; set; }
            public string dApt { get; set; }
            public string dCity { get; set; }
            public string dState { get; set; }
            public long dZip { get; set; }
            public long dPhone { get; set; }
            public string dEmail { get; set; }
            public string dFirstb { get; set; }
            public string dLastb { get; set; }
            public string dAddressb { get; set; }
            public string dAptb { get; set; }
            public string dCityb { get; set; }
            public string dStateb { get; set; }
            public long dZipb { get; set; }
            public long dCard { get; set; }
            public long dCvv { get; set; }
            public int dMonth { get; set; }
            public int dYear { get; set; }
           

        }
        public class dataWoutship
        {
            public string dCounrty { get; set; }
            public string dFirst { get; set; }
            public string dLast { get; set; }
            public string dAddress { get; set; }
            public string dApt { get; set; }
            public string dCity { get; set; }
            public string dState { get; set; }
            public long dZip { get; set; }
            public long dPhone { get; set; }
            public string dEmail { get; set; } 
            public long dCard { get; set; }
            public long dCvv { get; set; }
            public int dMonth { get; set; }
            public int dYear { get; set; }
           
        }

        private void Createporfile(object sender, RoutedEventArgs e)
        {
            //cmd.Parameters.AddWithValue("@country", this.countryComboBox.SelectedItem.ToString()); add country

            //https://stackoverflow.com/questions/16921652/how-to-write-a-json-file-in-c
            // need to check which data input we use by checking the check box
            if ((bool)checkBox.IsChecked == true) {
                // easy case dataWoutship
                List<dataWoutship> _data = new List<dataWoutship>();
                try
                {
                   
                    _data.Add(new dataWoutship()
                    {
                        dCounrty = Counrty.Text,
                        dFirst = First.Text,
                        dLast = Last.Text,
                        dAddress = Address.Text,
                        dApt = Apt.Text,
                        dCity = City.Text,
                        dState = State.SelectedItem.ToString(),
                        dZip = Int64.Parse(Zip.Text),                                                                               
                        dPhone = Int64.Parse(Phone.Text),                                                                   // fixed
                        dEmail = Email.Text,
                        dCard = Int64.Parse(Card.Text),
                        dCvv = Int64.Parse(CVV.Text),
                        dMonth = Int32.Parse(Month.Text),
                        dYear = Int32.Parse(Year.Text),
                    });
                }
                catch(OverflowException exc)
                {
                    Console.WriteLine(exc);
                }
                string json = JsonConvert.SerializeObject(_data.ToArray());

                //write string to file
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + Profilename.Text+ ".json", json); /// need to save to our data file
            }

            else {

                List<dataWship> _data = new List<dataWship>();
                try
                {
                    _data.Add(new dataWship()
                    {
                        dCounrty = Counrty.Text,
                        dFirst = First.Text,
                        dLast = Last.Text,
                        dAddress = Address.Text,
                        dApt = Apt.Text,
                        dCity = City.Text,
                        dState = State.SelectedItem.ToString(),
                        dZip = Int64.Parse(Zip.Text),
                        dFirstb = FirstB.Text,
                        dLastb = LastB.Text,
                        dAddressb = AddressB.Text,
                        dAptb = AptB.Text,
                        dCityb = CityB.Text,
                        dStateb = StateB.SelectedItem.ToString(),
                        dZipb = Int64.Parse(ZipB.Text),
                        dPhone = Int64.Parse(Phone.Text),                                                                   // fixed
                        dEmail = Email.Text,
                        dCard = Int64.Parse(Card.Text),
                        dCvv = Int64.Parse(CVV.Text),
                        dMonth = Int32.Parse(Month.Text),
                        dYear = Int32.Parse(Year.Text),


                    });
                }
                catch (OverflowException exc)
                {
                    Console.WriteLine(exc);
                }
                string json = JsonConvert.SerializeObject(_data.ToArray());

                //write string to file
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + Profilename.Text + ".json", json); /// need to save to our data file
            }

        }
        private void Loadporfile(object sender, RoutedEventArgs e)
        {
            //cmd.Parameters.AddWithValue("@country", this.countryComboBox.SelectedItem.ToString()); add country
        }
        private void Removeporfile(object sender, RoutedEventArgs e)
        {
            //cmd.Parameters.AddWithValue("@country", this.countryComboBox.SelectedItem.ToString()); add country
        }
        public static List<string> GetCountryList()
        {
            List<string> cultureList = new List<string>();

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo culture in cultures)
            {
                RegionInfo region = new RegionInfo(culture.LCID);

                if (!(cultureList.Contains(region.EnglishName)))
                {
                    cultureList.Add(region.EnglishName);
                }
            }
            cultureList.Sort();
            return cultureList;
        }

        private void First_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
    }


