using iSun.Weather.Implementation;
using iSun.Weather.Implementtion.Tests.Mocks;
using NUnit.Framework;

namespace iSun.Weather.Implementtion.Tests
{
    [TestFixture]
    internal class WeatherReporterJobTests
    {
        [Test]
        public void RunReporterJobTest()
        {
            var tempFile = Path.GetTempFileName();
            var reporterMock = new WeatherReporterMock();
            var loggerMock = new LoggerMock();
            var dataWriter = new WeatherCSVDataWriter(true, tempFile);
            var weatherReporter = new WeatherReporterJob(reporterMock, loggerMock, new string[] { reporterMock.Cities[0], reporterMock.Cities[2] }, 1001);

            int reportCount = 0;

            weatherReporter.ReportWeathers += (weathers) =>
            {
                foreach (var weather in weathers)
                {
                    dataWriter.WriteData(weather);
                    reportCount++;
                }
            };

            weatherReporter.Start();
            Thread.Sleep(50);
            weatherReporter.Stop();

            int i = File.ReadAllLines(tempFile).Length;
            Assert.Greater(reportCount, 1);
            Assert.AreEqual(i, reportCount+1,"");
        }
    }
}
