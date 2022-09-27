using iSun.Base.Enums;
using iSunWeatherApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSun.Weather.ConsoleApp
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message, LogEventType type)
        {
            Console.WriteLine(DateTime.Now.ToString() + "|" + type.ToString() + "|" + message);
        }

        public void Dispose() { }
    }
}
