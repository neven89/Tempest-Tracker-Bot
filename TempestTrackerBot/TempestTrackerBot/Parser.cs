using System;
using System.Globalization;

namespace TempestTrackerBot
{
    public static class Parser
    {
        public static DateTime ParseDate(string input)
        {
            var pieces = input.Split(' ');
            var dateString = pieces[0] + " " + pieces[1];
            try
            {
                return DateTime.ParseExact(dateString, SettingsManager.DateFormat, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {   //Logging, probably
                return DateTime.MinValue;
            }
        }

        public static string ParseName(string input)
        {
            int startIndex = input.IndexOf('@');
            if (startIndex < 0)
            {   //Not a PM or bad string
                return "";
            }

            int endIndex = input.IndexOf(':', startIndex);
            return input.Substring(startIndex + 1, endIndex - startIndex);
        }

        public static void ParseReport(string input, out string mapName, out string prefix, out string suffix)
        {
            mapName = "";
            prefix = "";
            suffix = "";

            try
            {
                string reportString = input.Substring(input.IndexOf("!report") + 7).Trim();

                var parts = reportString.Split(',');

                if (parts.Length != 3) return;
                mapName = parts[0].Trim().Replace(' ', '_');
                prefix = parts[1].Trim().Replace(' ', '_');
                suffix = parts[2].Trim().Replace(' ', '_');
            }
            catch (Exception ex)
            {
                //Logging, probbably
                mapName = "";
                prefix = "";
                suffix = "";
            }
        }
    }
}
