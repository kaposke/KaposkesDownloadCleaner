using System;
using System.IO;
using System.Linq;

namespace DownloadCleanerService
{
    public class ConfigHelper
    {
        private static string cfgFilePath;
        public static void ReadOrCreateConfig()
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            DirectoryInfo cfgFolder = new DirectoryInfo(userPath + @"\.KaposkeDownloadsCleanerCfg");
            if (!cfgFolder.Exists)
                cfgFolder.Create();

            cfgFilePath = cfgFolder + @"\config.ini";
            if (!File.Exists(cfgFilePath))
            {
                string[] config = new string[] {
                    "# Kaposke's old downloads cleaner configs.",
                    "[Config]",
                    "# Delay in minutes between cleanup attempts",
                    "delayBetweenCleanupChecks=120",
                    "# How many days a file has to stay untouched to be deleted",
                    "maximumAge=60",
                    "# Attempt cleanup at system startup",
                    "cleanAtStartup=false",
                    "# Should folders also be deleted?",
                    "deleteFolders=false"
                };
                File.AppendAllLines(cfgFilePath, config);
            }
        }

        public static void WriteConfigs()
        {
            Console.WriteLine($"delayBetweenChecks = {GetDelay()}");
            Console.WriteLine($"maximumAge = {GetMaximumAge()}");
            Console.WriteLine($"cleanAtStartup = {GetCleanAtStartup()}");
            Console.WriteLine($"deleteFolders = {GetDeleteFolders()}");
        }

        public static string GetPropertyValue(string property)
        {
            string line = File.ReadAllLines(cfgFilePath).Where(w => w.StartsWith(property)).FirstOrDefault().ToString();
            if(line != null)
                return line.Split('=')[1];
            return null;
        }

        public static int GetDelay ()
        {
            int result = 60;
            int.TryParse(GetPropertyValue("delayBetweenCleanupChecks"), out result);
            return result;
        }

        public static int GetMaximumAge()
        {
            int result = 60;
            int.TryParse(GetPropertyValue("maximumAge"), out result);
            return result;
        }

        public static bool GetCleanAtStartup()
        {
            return GetPropertyValue("cleanAtStartup").ToLowerInvariant() == "true";
        }

        public static bool GetDeleteFolders()
        {
            return GetPropertyValue("deleteFolders").ToLowerInvariant() == "true";
        }
    }
}
