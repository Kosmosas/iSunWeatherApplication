using iSun.Weather.Implementation;
using iSunWeatherApp.DataModels;
using NUnit.Framework;

namespace iSun.Weather.Implementtion.Tests
{
    [TestFixture]
    public class WeatherFileDataWriterTests
    {
        [Test]
        public void HandleBadFileName()
        {
            Assert.Throws<Exception>(() => new WeatherCSVDataWriter(false, "C:][//bad filename??"));
        }

        [Test]
        public void CreatesFileIfNotExits()
        {
            var tempFile = Path.GetTempFileName();
            new WeatherCSVDataWriter(false, tempFile);
            Assert.True(File.Exists(tempFile), "Weather data file does not exists");
        }

        [Test]
        public void WritesCsvToFile()
        {
            var tempFile = Path.GetTempFileName();
            var writer = new WeatherCSVDataWriter(false, tempFile);
            WeatherModel model = new WeatherModel()
            {
                City = "City",
                Precipitation = 2,
                Summary = "Summary",
                Temperature = 1,
                WindSpeed = 1.5
            };
            WeatherModel model2 = new WeatherModel()
            {
                City = "City2",
                Precipitation = 1,
                Summary = "Summary2",
                Temperature = -2,
                WindSpeed = 1.7
            };
            writer.WriteData(model);
            writer.WriteData(model2);

            string? firstLine = "";
            string? secondLine = "";
            string? thirdLine = "";

            using (StreamReader sr = new StreamReader(tempFile))
            {
                firstLine = sr.ReadLine();
                secondLine = sr.ReadLine();
                thirdLine = sr.ReadLine();
            }

            Assert.True(string.Equals(firstLine,WeatherModel.CsvHeader),"CSV header is not correct");
            Assert.True(string.Equals(secondLine, model.CsvValue), "CSV 1st value is not correct");
            Assert.True(string.Equals(thirdLine, model2.CsvValue), "CSV 2d value is not correct");
        }

    }
}