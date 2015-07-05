using System.Configuration;

namespace TempestTrackerBot
{
    public static class SettingsManager
    {
        public static string LogDirectory
        {
            get
            {
                return _logDirectory;
            }
        }

        public static string DateFormat
        {
            get
            {
                return _dateFormat;
            }
        }

        public static string ApiUrl
        {
            get
            {
                return _apiUrl;
            }
        }

        private static string _logDirectory { get; set; }
        private static string _dateFormat { get; set; }
        private static string _apiUrl { get; set; }

        public static void Init()
        {
            _logDirectory = ConfigurationManager.AppSettings["LogDirectory"];
            _dateFormat = ConfigurationManager.AppSettings["DateFormat"];
            _apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        }
    }
}
