using iSun.Weather.Implementation;
using iSun.Weather.Implementtion.Tests.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSun.Weather.Implementtion.Tests
{

    [TestFixture]
    internal class WeatherReportHelperTests
    {
        [Test]
        public void ExtractAvailableCities()
        {
            var reporterMock = new WeatherReporterMock();

            var availableCities = WeatherReportHelper.ExtractAvailableCities(                
                new string[] { "badCity", reporterMock.Cities[0], reporterMock.Cities[2].ToUpper() },
                reporterMock.Cities,
                out var nonAvailableCities);

            Assert.AreEqual(availableCities.Length, 2);
            Assert.AreEqual(availableCities[0], reporterMock.Cities[0]);
            Assert.AreEqual(availableCities[1], reporterMock.Cities[2]);
            Assert.AreEqual(nonAvailableCities.Length, 1);
            Assert.AreEqual(nonAvailableCities[0], "badCity");
        }
    }
}
