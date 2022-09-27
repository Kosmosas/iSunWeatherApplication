using iSun.Base;

namespace iSunWeatherApp.DataModels
{
    public class WeatherModel
    {
        public string City { get; set; } = "";
        public int Temperature { get; set; }
        public int Precipitation { get; set; }
        public double WindSpeed { get; set; }
        public string Summary { get; set; } = "";

        public string WeatherReportString => $"{City}: Temperature - {Temperature}C; Precipitation - {Precipitation}; WindSpeed - {WindSpeed} m/s; Summary - {Summary}; ";
        public static string CsvHeader => "City;Temperature;Precipitation;WindSpeed;Summary".Replace(";",Helpers.CsvSeparator);
        public string CsvValue => 
            City + Helpers.CsvSeparator +
            Temperature.ToString() + Helpers.CsvSeparator +
            Precipitation.ToString() + Helpers.CsvSeparator +
            WindSpeed.ToString() + Helpers.CsvSeparator +
            Summary;
    }
}
