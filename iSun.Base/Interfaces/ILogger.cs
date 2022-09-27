using iSun.Base.Enums;

namespace iSunWeatherApp.Interfaces
{
    public interface ILogger : IDisposable
    {
        void Log(string message, LogEventType type);
    }
}
