using Syroot.Windows.IO;
using System;
using System.IO;
using System.Linq;
using System.Timers;

namespace DownloadCleanerService
{
    public class DownloadCleaner
    {
        private Timer _timer;
        int delay;

        public DownloadCleaner()
        {
            ConfigHelper.ReadOrCreateConfig();
            ConfigHelper.WriteConfigs();

            delay = ConfigHelper.GetDelay();

            AssignTimer(delay);

            if (ConfigHelper.GetCleanAtStartup())
                CleanOldDownloads();
        }

        private void AssignTimer(int delay)
        {
            _timer = new Timer(delay * 1000 * 60) { AutoReset = true };
            _timer.Elapsed += Tick;
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            int newDelay = ConfigHelper.GetDelay();
            if (delay != newDelay)
            {
                delay = newDelay;
                AssignTimer(delay);
            }
            CleanOldDownloads();
        }

        private static void CleanOldDownloads()
        {
            string downloadsPath = new KnownFolder(KnownFolderType.Downloads).Path;

            Console.WriteLine($"Acessing folder {downloadsPath}.");

            DirectoryInfo downloadsFolder = new DirectoryInfo(downloadsPath);
            FileInfo[] files = downloadsFolder.GetFiles().Where(w => w.LastAccessTime.CompareTo(DateTime.Now.AddDays(-ConfigHelper.GetMaximumAge())) < 0).ToArray();

            if(!ConfigHelper.GetDeleteFolders())
            {
                files = files.Where(w => !w.Attributes.HasFlag(FileAttributes.Directory)).ToArray();
            }

            int fileNumber = files.Length;

            Console.WriteLine($"Found {fileNumber} files.");
            foreach (FileInfo file in files)
            {
                //file.Delete;

                Console.WriteLine($"Deleting { file.Name }");
            }

            Console.WriteLine($"Deleted {fileNumber} files.");
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
