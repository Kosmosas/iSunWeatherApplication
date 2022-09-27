using iSun.Base.Enums;
using iSunWeatherApp.Interfaces;

namespace iSun.Loggers
{
    public class FileLogger : ILogger
    {
        const string DEFAULT_LOG_PATH = "./log.txt";

        StreamWriter m_FileWriter;

        public FileLogger(bool overwrite = false, string fileName = "")
        {
            if (!string.IsNullOrEmpty(fileName) &&
                (Path.GetFileName(fileName).IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
                Path.GetDirectoryName(fileName).IndexOfAny(Path.GetInvalidPathChars()) >= 0))
                throw new Exception("Provided weather data writer location is not correct.");

            m_FileWriter = string.IsNullOrEmpty(fileName) ? 
                new StreamWriter(fileName, !overwrite) :
                new StreamWriter(DEFAULT_LOG_PATH, !overwrite);
        }

        public void Dispose()
        {
            if(m_FileWriter != null)
                m_FileWriter.Close();
        }

        public void Log(string message, LogEventType type)
        {
            if (m_FileWriter != null)
                m_FileWriter.WriteLine(DateTime.Now.ToString() + "|" + type.ToString() + "|" + message);

        }
    }
}