using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace TempestTrackerBot
{
    public static class ChatListener
    {
        private static DateTime LastReadTime { get; set; }

        private static List<string> PlayerNames { get; set; }

        private static int LastHour { get; set; }

        public static void Init()
        {
            SettingsManager.Init();
            LastReadTime = DateTime.MinValue;
            PlayerNames = new List<string>();
            LastHour = DateTime.Now.Hour;
            Start();
        }

        private static void Start()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = SettingsManager.LogDirectory;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "Client.txt";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;

            Task.Factory.StartNew(() => StartCleanUpInterval());
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            using (FileStream stream = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read))
            using (StreamReader file = new StreamReader(stream))
            {
                while (!file.EndOfStream)
                {
                    ParseLine(file.ReadLine());
                }
            }
        }

        public static void ParseLine(string line)
        {   
            var playerName = Parser.ParseName(line);
            if (playerName == "" || PlayerNames.Contains(playerName)) return;

            var timeStamp = Parser.ParseDate(line);
            if (timeStamp < LastReadTime) return;
            LastReadTime = timeStamp;

            string mapName;
            string prefix;
            string suffix;
            Parser.ParseReport(line, out mapName, out prefix, out suffix);

            if (mapName == "") return;

            PlayerNames.Add(playerName);
            
            //Debug
            Console.WriteLine(string.Format("{0} {1} {2} by {3} at {4}", mapName, prefix, suffix, playerName, timeStamp));
            //Don't keep the main thread busy, communcation can hang and we don't want that if we can avoid it
            Task.Factory.StartNew(() => SendData(mapName, prefix, suffix));
        }

        private static void SendData(string mapName, string prefix, string suffix)
        {            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(SettingsManager.ApiUrl);
                var pairs = new[] 
                {
                    new KeyValuePair<string, string>("map", mapName),
                    new KeyValuePair<string, string>("tempest", suffix)
                };
                var content = new FormUrlEncodedContent(pairs);
                var result = client.PostAsync("", content).Result;
                //Don't really need the result but there it is...
            }
        }

        public static void StartCleanUpInterval()
        {
            var cleanUpTimer = new System.Timers.Timer(30000);
            cleanUpTimer.Elapsed += CleanUp;
            cleanUpTimer.Enabled = true;
        }

        private static void CleanUp(Object source, ElapsedEventArgs e)
        {
            if (LastHour != e.SignalTime.Hour)
            {
                PlayerNames.Clear();
                LastHour = e.SignalTime.Hour;
            }

            //Clear the Client.txt - we want it to be small so that it reads faster
            var fullPath = Path.Combine(SettingsManager.LogDirectory, "Client.txt");
            var backupPath = Path.Combine(SettingsManager.LogDirectory, string.Format("Client {0}.txt", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")));
            File.Copy(fullPath, backupPath);
            File.WriteAllText(fullPath, string.Empty);
        }
    }
}
