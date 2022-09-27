using iSunWeatherApp.DataModels;

namespace iSun.Base.Interfaces
{
    public interface IWeatherDataWriter
    {
        public void WriteData(WeatherModel weather);
    }
}
