using CommunityToolkit.Diagnostics;
using iSun.Base.Enums;
using iSunWeatherApp.Interfaces;

namespace iSun.Loggers
{
    public class AggregatedLogger : ILogger
    {
        private List<ILogger> m_Loggers;

        public AggregatedLogger(List<ILogger> loggers)
        {
            Guard.IsNotEmpty(loggers);
            m_Loggers = loggers;
        }

        public void Log(string message, LogEventType type)
        {
            foreach (var logger in m_Loggers)
                logger.Log(message, type);
        }
        public void Dispose()
        {
            foreach (var logger in m_Loggers)
                logger.Dispose();
        }
    }
}
