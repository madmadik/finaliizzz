using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Finallllllll
{

    public partial class Menu : Page
    {

        async void GetChanges(Object source, System.Timers.ElapsedEventArgs e)
        {
            Rootobject allEarthquakes = await Async();
            int count = allEarthquakes.features.Length;
            string txt = "Последнее землетрясение: \nМагнитуда: " + allEarthquakes.features[count - 1].properties.mag + "\nРасположение: " + allEarthquakes.features[count - 1].properties.place + "\nВремя: " + DateTimeOffset.FromUnixTimeMilliseconds(allEarthquakes.features[count - 1].properties.time) + "\nГлубина: " + allEarthquakes.features[count - 1].geometry.coordinates[2] + "\nID: " + allEarthquakes.features[count - 1].id;

            await Dispatcher.BeginInvoke(new ThreadStart(delegate { LastData.Text = txt; }));
        }
        Task<Rootobject> Async()
        {
            string path = @"C:\data\data.json";
            string json;
            string url = @"https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString(url);
            }
            Rootobject allEarthquakes = JsonConvert.DeserializeObject<Rootobject>(json);
            int count = allEarthquakes.features.Length;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            Directory.CreateDirectory(@"C:\data");
            using (StreamWriter stream = File.CreateText(path))
            {
                stream.Write(json);
            }
            return Task.Run(() =>
                allEarthquakes);
        }

        public void Do(string txt) =>
            LastData.Text = txt;
        public Menu()
        {
            InitializeComponent();
            this.WindowTitle = "Система анализа землетрясении";
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Interval = 2000;
            aTimer.Elapsed += GetChanges;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        private void Button_Stat(object sender, RoutedEventArgs e) => this.NavigationService.Navigate(new PageMap());
        private void Button_Top(object sender, RoutedEventArgs e) => this.NavigationService.Navigate(new TopPage());
    }
}