using CommunityToolkit.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSun.Weather.Implementation
{
    public static class WeatherReportHelper
    {
        public static string[] ExtractAvailableCities(string[] providedCities, string[] availableCities, out string[] nonAvailableCities)
        {
            Guard.IsNotEmpty(providedCities);
            Guard.IsNotEmpty(availableCities);

            var citiesAvailable = availableCities.Where(city =>
                providedCities.Any(c => string.Equals(city, c, StringComparison.InvariantCultureIgnoreCase)));

            nonAvailableCities = providedCities.Where(city =>
                !citiesAvailable.Any(c => string.Equals(city, c, StringComparison.InvariantCultureIgnoreCase)))
                .ToArray();

            return citiesAvailable.ToArray();
        }

        static string MakeProperName(string city) => city[..1].ToUpper() + city[1..].ToLower();
    }
}
