using iSunWeatherApp.DataModels;

namespace iSunWeatherApp.Interfaces
{
    public interface IWeatherReporter
    {
        public string[] Cities { get; }
        WeatherModel GetWeather(string city);
        Task<WeatherModel> GetWeatherAsync(string city);
    }
}
