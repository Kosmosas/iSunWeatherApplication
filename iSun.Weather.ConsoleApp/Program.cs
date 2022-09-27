using iSun.Base.Enums;
using iSun.Base.Interfaces;
using iSun.Loggers;
using iSun.Weather.Implementation;
using iSunWeatherApp;
using iSunWeatherApp.DataModels;
using iSunWeatherApp.Interfaces;
using System.Configuration;

// Initialize logger
var overwriteLogFile = bool.TryParse(ConfigurationManager.AppSettings["OVERWRITE_LOG_FILE"], out var overwrite) && overwrite;
using (ILogger logger = new FileLogger(overwriteLogFile, ConfigurationManager.AppSettings["LOG_FILE"]))
{
    try
    {
        logger.Log("Starting console application", LogEventType.Info);

        // Check if provided arguments are correct and retrieve provided cities
        var cities = new List<string>();
        if (!CheckArguments(args, ref cities))
        {
            logger.Log("Bad arguments provided - exiting application", LogEventType.Info);
            return;
        }

        // Initialize "reporter", aka API Client
        IWeatherReporter reporter = new WeatherReporterClient(
            logger,
            ConfigurationManager.AppSettings["API_ENDPOINT"],
            ConfigurationManager.AppSettings["USER"],
            ConfigurationManager.AppSettings["PASS"]);

        // Initialize CSV file writer
        var overwriteFileDataFile = bool.TryParse(ConfigurationManager.AppSettings["OVERWRITE_DATA_FILE"], out var _overwrite) && _overwrite;
        IWeatherDataWriter dataWriter = new WeatherCSVDataWriter(overwriteFileDataFile, ConfigurationManager.AppSettings["DATA_FILE"]);

        // Check if provided cities are available
        cities = WeatherReportHelper.ExtractAvailableCities(cities.ToArray(), reporter.Cities, out var notAvailableCities).ToList();
        if (!CheckCities(logger, cities, notAvailableCities))
            return;

        if (!int.TryParse(ConfigurationManager.AppSettings["UPDATE_TIME_MS"], out var waitTimeMs))
            throw new Exception("Bad update time provided in config");

        // Start reporter service
        WeatherReporterJob weatherReporter = new WeatherReporterJob(reporter, logger, cities.ToArray(), waitTimeMs);
        weatherReporter.ReportWeathers += (weathers) =>
        {
            // Write data on weather update and print out it to console
            foreach (var weather in weathers)
                dataWriter.WriteData(weather);

            WriteWeatherToConsole(weathers);
        };
        weatherReporter.Start();

        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Escape)
                break;
        }
    }
    catch (Exception e)
    {
        logger.Log("Something went wrong: " + e.Message, LogEventType.Error);
        Console.WriteLine("Something went wrong: " + e.Message, LogEventType.Error);
        return;
    }
}


void WriteWeatherToConsole(IEnumerable<WeatherModel> weathers)
{
    Console.Clear();
    var dateTime = DateTime.Now;
    Console.WriteLine($"Last Update: {dateTime.ToShortDateString()} {dateTime.ToLongTimeString()}");
    foreach (var weather in weathers)
        Console.WriteLine(weather.WeatherReportString);

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("Press Escape to exit the application");
}

static bool CheckArguments(string[] args, ref List<string> cities)
{
    if (!args.Any())
    {
        Console.WriteLine("Try `iSun -h' for more information.");
        return false;
    }

    if (args.Contains("-h"))
    {
        Console.WriteLine("-h|-?|--help - Shows this message ant exits");
        Console.WriteLine("--cities - select cities to show data, f.e. iSun --cities Vilnius, Kaunas, Klaipėda");
        return false;
    }

    if (args.Contains("--cities"))
    {
        var startIndex = Array.IndexOf(args, "--cities");
        if (startIndex >= 0)
            cities = args.Skip(startIndex + 1).ToList();
    }

    if (!cities.Any())
    {
        Console.WriteLine("No cities provided");
        Console.WriteLine("Try `iSun -h' for more information.");
        return false;
    }

    //Remove commas from arguments
    for (int i = 0; i < cities.Count; i++)
        if (cities[i].Last() == ',')
            cities[i] = new string(cities[i].ToCharArray(), 0, cities[i].Length - 1);

    return true;
}

static bool CheckCities(ILogger logger, List<string> cities, string[]? notAvailableCities)
{
    if (notAvailableCities.Length > 0)
    {
        string notAvailableCitiesStr = string.Join(", ", notAvailableCities);
        logger.Log($"Provided cities: {notAvailableCitiesStr} are not available.", LogEventType.Warning);
        Console.WriteLine($"Provided cities: {notAvailableCitiesStr} are not available.");
    }

    if (cities.Count == 0)
    {
        logger.Log($"There are no correct city name provided. Exiting the program.", LogEventType.Error);
        Console.WriteLine($"There are no correct city name provided. Exiting the program.");
        return false;
    }

    return true;
}