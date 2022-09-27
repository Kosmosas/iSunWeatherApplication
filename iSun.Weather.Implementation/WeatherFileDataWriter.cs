using CommunityToolkit.Diagnostics;
using iSun.Base.Interfaces;
using iSunWeatherApp.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSun.Weather.Implementation
{
    public class WeatherCSVDataWriter : IWeatherDataWriter
    {
        const string DEFAULT_PATH = "./data.txt";

        string m_FileName;

        public WeatherCSVDataWriter(bool overwrite = false, string fileName = "")
        {
            fileName = String.IsNullOrEmpty(fileName) ? DEFAULT_PATH : fileName;

            if(Path.GetFileName(fileName).IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
                Path.GetDirectoryName(fileName).IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                throw new Exception("Provided weather data writer location is not correct.");

            m_FileName = fileName;

            if(overwrite || !File.Exists(fileName) || new FileInfo(fileName).Length == 0)
                using (StreamWriter sw = new StreamWriter(fileName,false))                
                    sw.WriteLine(WeatherModel.CsvHeader);

        }

        public void WriteData(WeatherModel weather)
        {
            using (StreamWriter sw = new StreamWriter(m_FileName, true))
                sw.WriteLine(weather.CsvValue);
        }
    }
}
