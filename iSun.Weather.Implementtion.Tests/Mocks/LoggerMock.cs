using iSun.Base.Enums;
using iSunWeatherApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSun.Weather.Implementtion.Tests.Mocks
{
    internal class LoggerMock : ILogger
    {
        StringWriter sw = new StringWriter();

        public string LogString => sw.ToString();

        public LoggerMock()
        {

        }

        public void Dispose() 
        {
            sw.Close();
        }

        public void Log(string message, LogEventType type)
        {
            sw.WriteLine(message + "|" + type.ToString());
        }
    }
}
