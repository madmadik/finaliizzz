using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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

namespace Finallllllll
{
    public partial class TopPage : Page
    {
        public List<String> GetList(IOrderedEnumerable<Rootobject.Feature> list)
        {
            List<String> info = new List<String>();
            foreach (var item in list)
            {
                string element = "Магнитуда: " + item.properties.mag + "\nРасположение: " + item.properties.place + "\nВремя: " + DateTimeOffset.FromUnixTimeMilliseconds(item.properties.time) + "\nГлубина: " + item.geometry.coordinates[2] + "\nID: " + item.id;
                info.Add(element);
            }
            return info;
        }

        public TopPage()
        {
            InitializeComponent();
            this.WindowTitle = "Топ 20";
            Rootobject allEarthquakes = new Rootobject();
            Rootobject topDay = new Rootobject();
            Rootobject topMonth = new Rootobject();
            string url = @"https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
            string url2 = @"https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_month.geojson";
            string path = @"C:\data\data.json";
            string path2 = @"C:\data\dataMonth.json";
            List<String> sortedDay, sortedMonth;
            if (File.Exists(path))
            {
                using (StreamReader stream = new StreamReader(path))
                {
                    string json = stream.ReadToEnd();
                    allEarthquakes = JsonConvert.DeserializeObject<Rootobject>(json);
                }
                var sorted = from i in allEarthquakes.features orderby i.properties.mag descending select i;
                sortedDay = GetList(sorted);

            }
            else
            {
                Directory.CreateDirectory(@"C:\data");
                using (StreamWriter stream = File.CreateText(path))
                {
                    using (WebClient client = new WebClient())
                    {
                        string json = client.DownloadString(url);
                        allEarthquakes = JsonConvert.DeserializeObject<Rootobject>(json);
                    }
                    var sorted = from i in allEarthquakes.features orderby i.properties.mag descending select i;
                    sortedDay = GetList(sorted);
                }
            }

            using (StreamWriter stream = File.CreateText(path2))
            {
                using (WebClient client = new WebClient())
                {
                    string json = client.DownloadString(url2);
                    allEarthquakes = JsonConvert.DeserializeObject<Rootobject>(json);
                }
                var sorted = from i in allEarthquakes.features orderby i.properties.mag descending select i;
                sortedMonth = GetList(sorted);
            }
            dataDay.ItemsSource = sortedDay.Take(20);
            dataMonth.ItemsSource = sortedMonth.Take(20);

        }
        private void Button_Back(object sender,RoutedEventArgs e)=>
            this.NavigationService.Navigate(new Menu());
    }
}