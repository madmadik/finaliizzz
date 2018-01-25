﻿using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    /// <summary>
    /// Логика взаимодействия для PageMap.xaml
    /// </summary>
    public partial class PageMap : Page
    {

        
        public PageMap()
        {
            InitializeComponent();
            string url = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
            string json;
            using (WebClient webclient = new WebClient())
            {
                json = webclient.DownloadString(url);
            }
            Rootobject allEarthquakes = JsonConvert.DeserializeObject<Rootobject>(json);
            foreach(var item in allEarthquakes.features)
            {
                 Pushpin point = new Pushpin();
                point.ToolTip = "Place: "+ item.properties.place+"\nMag: "+item.properties.mag+ "\nlon: "+ item.geometry.coordinates[1]+"\nlat: "+item.geometry.coordinates[0];

  
                 point.Location = new Location(item.geometry.coordinates[1], item.geometry.coordinates[0]);
                 map.Children.Add(point);
            }
           

        }
    }
}
