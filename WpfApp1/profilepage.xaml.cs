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
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using ZenAIO;

namespace WpfApp1
{
    /// <summary>
    /// profilepage.xaml 的交互逻辑
    /// </summary>
    public partial class profilepage : Page
    {

        private static readonly int currentYear = DateTime.Now.Year;

        public profilepage()
        {
            InitializeComponent();
            Country.ItemsSource = GetCountryList();
            Country.SelectedIndex = 134; // set to us
            State.ItemsSource = StateArray.States();
            StateB.ItemsSource = StateArray.States();

            for (var a = 1; a <= 12; a++)
            {
                Month.Items.Add(a);
            }

            for (var a = currentYear; a <= currentYear + 10; a++)
            {
                Year.Items.Add(a);
            }

            string path = Utils.GetPath("Profile.json");

            // Checks if a profile exists or not. If not, will create a default profile.
            // Else (does exist) prompts us to read the file in like normal.
            if (CreateDefaultProfile(ref path))
                ReadProfile(ref path);
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
                    PorfileSaved.Items.Add(playerList[0].nanme);
                }
                else if (line.Contains("2type"))
                {
                    var playerList = JsonConvert.DeserializeObject<List<DataWithShip>>(line);
                    PorfileSaved.Items.Add(playerList[0].nanme);
                }
                else if (line.Contains("3type"))
                {
                    var playerList = JsonConvert.DeserializeObject<List<NonUSDataWithoutShip>>(line);
                    PorfileSaved.Items.Add(playerList[0].nanme);
                }
                else if (line.Contains("4type"))
                {
                    var playerList = JsonConvert.DeserializeObject<List<NotUSDataWithShip>>(line);
                    PorfileSaved.Items.Add(playerList[0].nanme);
                }
                else
                {

                }

            }
        }

        private void cbValueType_DropDownClosed(object sender, EventArgs e)
        {
            String s = Country.Text;
            if (s != "Canada" && s != "United States")
            {

                State.IsEnabled = false;
                StateB.IsEnabled = false;
            }
            else
            {
                if ((bool)checkBox.IsChecked == true)

                    State.IsEnabled = true;
                else
                {
                    State.IsEnabled = true;
                    StateB.IsEnabled = true;
                }

            }
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
        public class DataWithShip
        {
            public string nanme { get; set; }
            public string type { get; set; }
            public string dCountry { get; set; }
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
        public class NotUSDataWithShip
        {
            public string nanme { get; set; }
            public string type { get; set; }
            public string dCountry { get; set; }
            public string dFirst { get; set; }
            public string dLast { get; set; }
            public string dAddress { get; set; }
            public string dApt { get; set; }
            public string dCity { get; set; }
            public long dZip { get; set; }
            public long dPhone { get; set; }
            public string dEmail { get; set; }
            public string dFirstb { get; set; }
            public string dLastb { get; set; }
            public string dAddressb { get; set; }
            public string dAptb { get; set; }
            public string dCityb { get; set; }
            public long dZipb { get; set; }
            public long dCard { get; set; }
            public long dCvv { get; set; }
            public int dMonth { get; set; }
            public int dYear { get; set; }


        }
        public class NonUSDataWithoutShip
        {
            public string nanme { get; set; }
            public string type { get; set; }
            public string dCountry { get; set; }
            public string dFirst { get; set; }
            public string dLast { get; set; }
            public string dAddress { get; set; }
            public string dApt { get; set; }
            public string dCity { get; set; }
            public long dZip { get; set; }
            public long dPhone { get; set; }
            public string dEmail { get; set; }
            public long dCard { get; set; }
            public long dCvv { get; set; }
            public int dMonth { get; set; }
            public int dYear { get; set; }

        }
        public class DataWithoutShip
        {
            public string nanme { get; set; }
            public string type { get; set; }
            public string dCountry { get; set; }
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
            string path = Utils.GetPath("Profile.json");

            //cmd.Parameters.AddWithValue("@country", this.countryComboBox.SelectedItem.ToString()); add country

            //https://stackoverflow.com/questions/16921652/how-to-write-a-json-file-in-c
            // need to check which data input we use by checking the check box
            String s = Country.Text;
            if (s != "Canada" && s != "United States")
            {
                if ((bool)checkBox.IsChecked == true)
                {
                    // easy case dataWoutship
                    if (String.IsNullOrEmpty(CVV.Text) || String.IsNullOrEmpty(Card.Text) || String.IsNullOrEmpty(Email.Text) || String.IsNullOrEmpty(Phone.Text) || String.IsNullOrEmpty(Country.Text) || String.IsNullOrEmpty(First.Text) || String.IsNullOrEmpty(Last.Text) || String.IsNullOrEmpty(Address.Text) || String.IsNullOrEmpty(City.Text) || String.IsNullOrEmpty(Zip.Text))
                    {
                        Console.WriteLine("is null");
                        // Do something...
                    }
                    else
                    {
                        List<NonUSDataWithoutShip> _data = new List<NonUSDataWithoutShip>();
                        try
                        {

                            _data.Add(new NonUSDataWithoutShip()
                            {
                                nanme = Profilename.Text,
                                type = "3type",
                                dCountry = Country.Text,
                                dFirst = First.Text,
                                dLast = Last.Text,
                                dAddress = Address.Text,
                                dApt = Apt.Text,
                                dCity = City.Text,

                                dZip = Int64.Parse(Zip.Text),
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
                        var a = true;
                        foreach (var item in PorfileSaved.Items)
                        {
                            if (item.ToString() == Profilename.Text)
                            {
                                MessageBox.Show("Profilename can't be the same");
                                a = false;
                                break;

                            }
                            else
                            {
                                a = true;
                            }

                            // do something with your item
                        }
                        if (a)
                        {
                            string json = JsonConvert.SerializeObject(_data.ToArray());
                            PorfileSaved.Items.Add(Profilename.Text);
                            if (File.Exists(path))
                            {
                                StreamWriter sw = new StreamWriter(path, true);
                                sw.Write("\n");
                                sw.Write(json);

                                sw.Dispose();
                            }
                            else
                            {
                                System.IO.File.WriteAllText(path, json); /// need to save to our data file
                            }
                        }

                        //write string to file

                    }

                }

                else
                {

                    List<NotUSDataWithShip> _data = new List<NotUSDataWithShip>();

                    if (String.IsNullOrEmpty(FirstB.Text) || String.IsNullOrEmpty(LastB.Text) || String.IsNullOrEmpty(CVV.Text) || String.IsNullOrEmpty(Card.Text) || 
                        String.IsNullOrEmpty(Email.Text) || String.IsNullOrEmpty(Phone.Text) || String.IsNullOrEmpty(Country.Text) || String.IsNullOrEmpty(First.Text) || 
                        String.IsNullOrEmpty(Last.Text) || String.IsNullOrEmpty(Address.Text) || String.IsNullOrEmpty(City.Text) || String.IsNullOrEmpty(Zip.Text))
                    {
                        Console.WriteLine("is null");
                        // Do something...
                    }
                    else
                    {

                        try
                        {
                            _data.Add(new NotUSDataWithShip()
                            {
                                nanme = Profilename.Text,
                                type = "4type",
                                dCountry = Country.Text,
                                dFirst = First.Text,
                                dLast = Last.Text,
                                dAddress = Address.Text,
                                dApt = Apt.Text,
                                dCity = City.Text,
                                dZip = Int64.Parse(Zip.Text),
                                dFirstb = FirstB.Text,
                                dLastb = LastB.Text,
                                dAddressb = AddressB.Text,
                                dAptb = AptB.Text,
                                dCityb = CityB.Text,
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
                        var a = true;
                        foreach (var item in PorfileSaved.Items)
                        {
                            if (item.ToString() == Profilename.Text)
                            {
                                MessageBox.Show("Profilename can't be the same");
                                a = false;
                                break;

                            }
                            else
                            {
                                a = true;
                            }

                            // do something with your item
                        }
                        if (a)
                        {
                            string json = JsonConvert.SerializeObject(_data.ToArray());
                            PorfileSaved.Items.Add(Profilename.Text);

                            if (File.Exists(path))
                            {
                                StreamWriter sw = new StreamWriter(path, true);
                                sw.Write("\n");
                                sw.Write(json);

                                sw.Dispose();
                            }

                            else
                            {
                                System.IO.File.WriteAllText(path, json); /// need to save to our data file
                            }
                        }
                    }
                }
            }
            else
            {
                if ((bool)checkBox.IsChecked == true)
                {
                    // easy case dataWoutship
                    if (String.IsNullOrEmpty(CVV.Text) || String.IsNullOrEmpty(Card.Text) || String.IsNullOrEmpty(Email.Text) || String.IsNullOrEmpty(Phone.Text) || 
                        String.IsNullOrEmpty(Country.Text) || String.IsNullOrEmpty(First.Text) || String.IsNullOrEmpty(Last.Text) || String.IsNullOrEmpty(Address.Text) || 
                        String.IsNullOrEmpty(City.Text) || String.IsNullOrEmpty(Zip.Text))
                    {
                        Console.WriteLine("is null");
                        // Do something...
                    }
                    else
                    {
                        List<DataWithoutShip> _data = new List<DataWithoutShip>();
                        try
                        {

                            _data.Add(new DataWithoutShip()
                            {
                                nanme = Profilename.Text,
                                type = "1type",
                                dCountry = Country.Text,
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
                        catch (OverflowException exc)
                        {
                            Console.WriteLine(exc);
                        }
                        var a = true;
                        foreach (var item in PorfileSaved.Items)
                        {
                            if (item.ToString() == Profilename.Text)
                            {
                                MessageBox.Show("Profilename can't be the same");
                                a = false;
                                break;

                            }
                            else
                            {
                                a = true;
                            }

                            // do something with your item
                        }
                        if (a)
                        {
                            string json = JsonConvert.SerializeObject(_data.ToArray());
                            PorfileSaved.Items.Add(Profilename.Text);
                            if (File.Exists(path))
                            {
                                StreamWriter sw = new StreamWriter(path, true);
                                sw.Write("\n");
                                sw.Write(json);

                                sw.Dispose();
                            }
                            else
                            {
                                System.IO.File.WriteAllText(path, json); /// need to save to our data file
                            }
                        }

                    }


                }

                else
                {

                    List<DataWithShip> _data = new List<DataWithShip>();

                    if (String.IsNullOrEmpty(FirstB.Text) || String.IsNullOrEmpty(LastB.Text) || String.IsNullOrEmpty(CVV.Text) || String.IsNullOrEmpty(Card.Text) || 
                        String.IsNullOrEmpty(Email.Text) || String.IsNullOrEmpty(Phone.Text) || String.IsNullOrEmpty(Country.Text) || String.IsNullOrEmpty(First.Text) || 
                        String.IsNullOrEmpty(Last.Text) || String.IsNullOrEmpty(Address.Text) || String.IsNullOrEmpty(City.Text) || String.IsNullOrEmpty(Zip.Text))
                    {
                        Console.WriteLine("is null");
                        // Do something...
                    }
                    else
                    {

                        try
                        {
                            _data.Add(new DataWithShip()
                            {
                                nanme = Profilename.Text,
                                type = "2type",
                                dCountry = Country.Text,
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
                        var a = true;
                        foreach (var item in PorfileSaved.Items)
                        {
                            if (item.ToString() == Profilename.Text)
                            {
                                MessageBox.Show("Profilename can't be the same");
                                a = false;
                                break;

                            }
                            else
                            {
                                a = true;
                            }

                            // do something with your item
                        }
                        if (a)
                        {
                            string json = JsonConvert.SerializeObject(_data.ToArray());
                            PorfileSaved.Items.Add(Profilename.Text);
                            if (File.Exists(path))
                            {
                                StreamWriter sw = new StreamWriter(path, true);
                                sw.Write("\n");
                                sw.Write(json);

                                sw.Dispose();
                            }
                            else
                            {
                                System.IO.File.WriteAllText(path, json); /// need to save to our data file
                            }
                        }
                    }

                }
            }


        }



        private void Loadporfile(object sender, RoutedEventArgs e)
        {
            var lines = File.ReadLines(AppDomain.CurrentDomain.BaseDirectory + @"\ Profile.json");
            //var playerList = JsonConvert.DeserializeObject<List<NotusdataWship>>(json);
            foreach (var line in lines)
            {

                if (line.Contains(PorfileSaved.Text))
                {
                    if (line.Contains("1type"))
                    {

                        var playerList = JsonConvert.DeserializeObject<List<DataWithoutShip>>(line);
                        Profilename.Text = playerList[0].nanme;
                        Country.Text = playerList[0].dCountry;
                        First.Text = playerList[0].dFirst;
                        Last.Text = playerList[0].dLast;
                        Address.Text = playerList[0].dAddress;
                        State.Text = playerList[0].dState;
                        Apt.Text = playerList[0].dApt;
                        City.Text = playerList[0].dCity;
                        Zip.Text = playerList[0].dZip.ToString();
                        Phone.Text = playerList[0].dPhone.ToString();
                        Email.Text = playerList[0].dEmail;
                        Card.Text = playerList[0].dCard.ToString();
                        CVV.Text = playerList[0].dCvv.ToString();
                        Month.Text = playerList[0].dMonth.ToString(); ;
                        Year.Text = playerList[0].dYear.ToString();
                        checkBox.IsChecked = true;
                        FirstB.IsEnabled = false;
                        LastB.IsEnabled = false;
                        AptB.IsEnabled = false;
                        CityB.IsEnabled = false;
                        StateB.IsEnabled = false;
                        ZipB.IsEnabled = false;
                        AddressB.IsEnabled = false;
                    }
                    else if (line.Contains("2type"))
                    {

                        var playerList = JsonConvert.DeserializeObject<List<DataWithShip>>(line);
                        Profilename.Text = playerList[0].nanme;
                        Country.Text = playerList[0].dCountry;
                        First.Text = playerList[0].dFirst;
                        Last.Text = playerList[0].dLast;
                        Address.Text = playerList[0].dAddress;
                        State.Text = playerList[0].dState;
                        Apt.Text = playerList[0].dApt;
                        City.Text = playerList[0].dCity;
                        Zip.Text = playerList[0].dZip.ToString();
                        Phone.Text = playerList[0].dPhone.ToString();
                        Email.Text = playerList[0].dEmail;
                        Card.Text = playerList[0].dCard.ToString();
                        CVV.Text = playerList[0].dCvv.ToString();
                        Month.Text = playerList[0].dMonth.ToString(); ;
                        Year.Text = playerList[0].dYear.ToString();
                        checkBox.IsChecked = false;
                        FirstB.Text = playerList[0].dFirstb;
                        LastB.Text = playerList[0].dLastb;
                        AddressB.Text = playerList[0].dAddressb;
                        AptB.Text = playerList[0].dAptb;
                        CityB.Text = playerList[0].dCityb;
                        StateB.Text = playerList[0].dStateb;
                        ZipB.Text = playerList[0].dZipb.ToString();
                        FirstB.IsEnabled = true;
                        LastB.IsEnabled = true;
                        AptB.IsEnabled = true;
                        CityB.IsEnabled = true;
                        StateB.IsEnabled = true;
                        ZipB.IsEnabled = true;
                        AddressB.IsEnabled = true;

                    }
                    else if (line.Contains("3type"))
                    {

                        var playerList = JsonConvert.DeserializeObject<List<NonUSDataWithoutShip>>(line);
                        Profilename.Text = playerList[0].nanme;
                        Country.Text = playerList[0].dCountry;
                        First.Text = playerList[0].dFirst;
                        Last.Text = playerList[0].dLast;
                        State.IsEnabled = false;
                        Address.Text = playerList[0].dAddress;
                        Apt.Text = playerList[0].dApt;
                        City.Text = playerList[0].dCity;
                        Zip.Text = playerList[0].dZip.ToString();
                        Phone.Text = playerList[0].dPhone.ToString();
                        Email.Text = playerList[0].dEmail;
                        Card.Text = playerList[0].dCard.ToString();
                        CVV.Text = playerList[0].dCvv.ToString();
                        Month.Text = playerList[0].dMonth.ToString(); ;
                        Year.Text = playerList[0].dYear.ToString();
                        checkBox.IsChecked = true;
                        FirstB.IsEnabled = false;
                        LastB.IsEnabled = false;
                        AptB.IsEnabled = false;
                        CityB.IsEnabled = false;
                        StateB.IsEnabled = false;
                        ZipB.IsEnabled = false;
                        AddressB.IsEnabled = false;
                    }
                    else if (line.Contains("4type"))
                    {

                        var playerList = JsonConvert.DeserializeObject<List<NotUSDataWithShip>>(line);
                        Profilename.Text = playerList[0].nanme;
                        Country.Text = playerList[0].dCountry;
                        First.Text = playerList[0].dFirst;
                        Last.Text = playerList[0].dLast;
                        Address.Text = playerList[0].dAddress;
                        State.IsEnabled = false;
                        Apt.Text = playerList[0].dApt;
                        City.Text = playerList[0].dCity;
                        Zip.Text = playerList[0].dZip.ToString();
                        Phone.Text = playerList[0].dPhone.ToString();
                        Email.Text = playerList[0].dEmail;
                        Card.Text = playerList[0].dCard.ToString();
                        CVV.Text = playerList[0].dCvv.ToString();
                        Month.Text = playerList[0].dMonth.ToString(); ;
                        Year.Text = playerList[0].dYear.ToString();

                        FirstB.Text = playerList[0].dFirstb;
                        LastB.Text = playerList[0].dLastb;
                        AddressB.Text = playerList[0].dAddressb;
                        AptB.Text = playerList[0].dAptb;
                        CityB.Text = playerList[0].dCityb;

                        ZipB.Text = playerList[0].dZipb.ToString();
                        FirstB.IsEnabled = true;
                        LastB.IsEnabled = true;
                        AptB.IsEnabled = true;
                        CityB.IsEnabled = true;
                        StateB.IsEnabled = false;
                        ZipB.IsEnabled = true;
                        AddressB.IsEnabled = true;
                    }
                    else
                    {

                    }
                }


            }
        }


        //cmd.Parameters.AddWithValue("@country", this.countryComboBox.SelectedItem.ToString()); add country
        private void Removeporfile(object sender, RoutedEventArgs e)
        {
            string path = Utils.GetPath("Profile.json");

            var lines = File.ReadLines(path);
            //var playerList = JsonConvert.DeserializeObject<List<NotusdataWship>>(json);
            var a = 0;
            foreach (var line in lines)
            {

                if (line.Contains(PorfileSaved.Text))
                {
                    if (line.Contains("1type"))
                    {
                        var playerList = JsonConvert.DeserializeObject<List<DataWithoutShip>>(line);
                        PorfileSaved.Items.Remove(playerList[0].nanme);
                    }
                    else if (line.Contains("2type"))
                    {
                        var playerList = JsonConvert.DeserializeObject<List<DataWithShip>>(line);
                        PorfileSaved.Items.Remove(playerList[0].nanme);
                    }
                    else if (line.Contains("3type"))
                    {
                        var playerList = JsonConvert.DeserializeObject<List<NonUSDataWithoutShip>>(line);
                        PorfileSaved.Items.Remove(playerList[0].nanme);
                    }
                    else if (line.Contains("4type"))
                    {
                        var playerList = JsonConvert.DeserializeObject<List<NotUSDataWithShip>>(line);
                        PorfileSaved.Items.Remove(playerList[0].nanme);
                    }
                    else
                    {

                    }
                    Console.WriteLine(a);
                    break;
                }
                else
                {
                    a = a + 1;
                }


            }
            List<string> quotelist = File.ReadAllLines(path).ToList();
            quotelist.RemoveAt(a);
            File.WriteAllLines(path, quotelist.ToArray());



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


