using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

namespace WpfForecast
{
    public class Coord
    {
        public float lon { get; set; }
        public float lat { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        public float temp { get; set; }
        public float feels_like { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    public class Wind
    {
        public float speed { get; set; }
        public int deg { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public long sunrise { get; set; }
        public long sunset { get; set; }
    }

    public class WeatherData
    {
        public Coord coord { get; set; }
        public Weather[] weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public long dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }
    public partial class MainWindow : Window
    {
        private const string ApiKey = "0603a06157949b5bba4c921b7eb79f3f";

        public MainWindow()
        {
            InitializeComponent();
            GetWeatherData();
        }

        private async void GetWeatherData()
        {
            var location = "Dubai"; // Hava durumu bilgisini almak istediğiniz lokasyonu buraya girin

            var weatherData = await GetWeatherDataAsync(location);

            if (weatherData != null)
            {
                Location.Text = $"Location: {weatherData.name}";
                Temperature.Text = $"Temperature: {weatherData.main.temp}°C";
                FeelsLike.Text = $"Feels Like: {weatherData.main.feels_like}°C";
                string iconCode = weatherData.weather[0].icon;
                string iconUrl = $"http://openweathermap.org/img/wn/{iconCode}.png";

                // Image kontrolü için BitmapImage oluştur
                BitmapImage iconImage = new BitmapImage();
                iconImage.BeginInit();
                iconImage.UriSource = new Uri(iconUrl);
                iconImage.EndInit();

                // Image kontrolünü güncelle
                WeatherImage.Source = iconImage;
            }
            else
            {
                MessageBox.Show("Hava durumu bilgisi alınamadı.");
            }
        }

        private async Task<WeatherData> GetWeatherDataAsync(string location)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={location}&appid={ApiKey}&units=metric");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                WeatherData weatherData = JsonSerializer.Deserialize<WeatherData>(responseBody);
                return weatherData;
            }
        }

      
    }
}

