using CommunityToolkit.Diagnostics;
using iSun.Base.Enums;
using iSun.Clients.WeatherClient;
using iSunWeatherApp.DataModels;
using iSunWeatherApp.Interfaces;
using System;
using System.Net.Http.Headers;

namespace iSunWeatherApp
{
    public class WeatherReporterClient : IWeatherReporter
    {
        private string m_Endpoint;
        private string m_User;
        private string m_Pass;

        string[] m_CitiesAvailable = { };
        string m_Token = "";
        ILogger m_Logger;        

        public WeatherReporterClient(ILogger logger, string? endpoint, string? user, string? pass)
        {
            Guard.IsNotNull(logger);
            Guard.IsNotNullOrEmpty(endpoint);
            Guard.IsNotNullOrEmpty(user);
            Guard.IsNotNullOrEmpty(pass);

            m_Logger = logger;
            m_Endpoint = endpoint;
            m_User = user;
            m_Pass = pass;

            using (var httpClient = new HttpClient())
            {
                logger.Log("Connecting to API client...", LogEventType.Info);
                Client client = new Client(m_Endpoint, httpClient);
                var authResponse = client.AuthorizeAsync(new AuthorizationRequest() { Password = pass, Username = m_User });

                m_Token = authResponse.Result.Token;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", m_Token);
                logger.Log("Athorization OK", LogEventType.Info);

                logger.Log("Fetching cities...", LogEventType.Info);
                m_CitiesAvailable = client.CitiesAsync().Result.ToArray();
                logger.Log("Cities fetched.", LogEventType.Info);
            }
        }

        public string[] Cities => m_CitiesAvailable;

        public WeatherModel GetWeather(string city) => GetWeatherAsync(city).Result;

        public async Task<WeatherModel> GetWeatherAsync(string city)
        {
            using (var httpClient = new HttpClient())
            {
                Client client = new Client(m_Endpoint, httpClient);
                m_Logger.Log($"Fetching city {city} data...", LogEventType.Info);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", m_Token);

                var weatherResult = client.WeathersAsync(city);

                return await ConvertWeatherResultAsync(weatherResult, city);
            }
        }

        private Task<WeatherModel> ConvertWeatherResultAsync(Task<WeatherResponse> weatherResult, string city) => 
            weatherResult.ContinueWith(t => 
            {
                if (t.IsFaulted || t.IsCanceled)
                {
                    m_Logger.Log($"Failed to fetch {city} data. " + t.Exception?.Message, LogEventType.Info);
                    return null;
                }
                else
                {
                    m_Logger.Log($"{t.Result.City} data fetched.", LogEventType.Info);
                    return ConvertWeatherResult(t.Result);
                }
            });
        
        WeatherModel ConvertWeatherResult(WeatherResponse weatherResult)
        {
            return new WeatherModel()
            {
                City = weatherResult.City,
                Precipitation = weatherResult.Precipitation,
                Summary = weatherResult.Summary,
                Temperature = weatherResult.Temperature,
                WindSpeed = weatherResult.WindSpeed
            };
        }
    }
}
