using CommunityToolkit.Diagnostics;
using iSun.Base;
using iSunWeatherApp.DataModels;
using iSunWeatherApp.Interfaces;

namespace iSun.Weather.Implementation
{
    public class WeatherReporterJob
    {
        IWeatherReporter m_Reporter;
        ILogger m_Logger;
        string[] m_Cities;
        int m_TimeToReportInMs;

        bool m_Working = true;

        public WeatherReporterJob(IWeatherReporter reporter, ILogger logger, string[] cities, int timeToReportInMs)
        {
            Guard.IsNotNull(reporter);
            Guard.IsNotEmpty(cities);
            Guard.IsInRange(timeToReportInMs, 1000, 60000);

            m_Reporter = reporter;
            m_Logger = logger;
            m_Cities = cities;
            m_TimeToReportInMs = timeToReportInMs;
        }

        public void Start()
        {
            m_Working = true;

            Task.Factory.StartNew(() => { 
                while (m_Working)
                {
                    List<WeatherModel> responses = new List<WeatherModel>();

                    for (int i = 0; i < m_Cities.Length; i++)
                    {
                        var result = m_Reporter.GetWeather(m_Cities[i]);
                        if (result != null)
                        {
                            ReportWeather?.Invoke(result);
                            responses.Add(result);
                        }
                    }

                    if (responses.Any())
                        ReportWeathers?.Invoke(new List<WeatherModel>(responses));

                    m_Logger.Log($"Waiting for {m_TimeToReportInMs}ms for next query...", Base.Enums.LogEventType.Info);
                    Thread.Sleep(m_TimeToReportInMs);
                }
            });
        }

        public void Stop() => m_Working = false;

        public delegate void WeatherReportHandler(WeatherModel weather);
        public delegate void WeathersReportHandler(List<WeatherModel> weathers);

        public event WeatherReportHandler? ReportWeather;
        public event WeathersReportHandler? ReportWeathers;

        
    }
}