using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public partial class PageMap 
    {
        public PageMap()
        {
            InitializeComponent();
            this.WindowTitle = "Карта Сейсмоактивности";
            string json;
            using (StreamReader stream = new StreamReader(@"C:\data\data.json"))
            {
                json = stream.ReadToEnd();
            }
           
            Rootobject allEarthquakes = JsonConvert.DeserializeObject<Rootobject>(json);
            foreach (var item in allEarthquakes.features)
            {
                 Pushpin point = new Pushpin();
                 point.ToolTip = "Место: " + item.properties.place +"\nВремя: "+DateTimeOffset.FromUnixTimeMilliseconds(item.properties.time)+ "\nМагнитуда: " + item.properties.mag + "\nШирина: " + item.geometry.coordinates[1] + "\nВысота: " + item.geometry.coordinates[0] + "\nГлубина: " + item.geometry.coordinates[2] + " км";
                 point.Location = new Location(item.geometry.coordinates[1], item.geometry.coordinates[0]);
                 map.Children.Add(point);
            }
        }
        
        private void Button_Back(object sender, RoutedEventArgs e)=>
            this.NavigationService.Navigate(new Menu());
    }
}
