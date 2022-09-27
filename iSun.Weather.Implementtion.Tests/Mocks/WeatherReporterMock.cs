using iSunWeatherApp.DataModels;
using iSunWeatherApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSun.Weather.Implementtion.Tests.Mocks
{
    internal class WeatherReporterMock : IWeatherReporter
    {
        static string[] cities = { "Test1", "Test2", "Test3", "Test4" };
        public string[] Cities => cities;

        public WeatherModel GetWeather(string city) 
        { 
            Random random = new Random();
            return new WeatherModel
            {
                City = city,
                Precipitation = random.Next(0, 10),
                Summary = "OK",
                Temperature = random.Next(-30, 30),
                WindSpeed = random.Next(0, 1000) / 100.0
            };
        }

        public Task<WeatherModel> GetWeatherAsync(string city)
        {
            return Task.Run(() => GetWeather(city));
        }
    }
}
