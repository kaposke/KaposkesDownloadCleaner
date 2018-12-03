using System;
using Topshelf;

namespace DownloadCleanerService
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x => {

                x.Service<DownloadCleaner>(s => {
                    s.ConstructUsing(downloadCleaner => new DownloadCleaner());
                    s.WhenStarted(downloadCleaner => downloadCleaner.Start());
                    s.WhenStopped(downloadCleaner => downloadCleaner.Stop());
                });

                x.RunAsLocalSystem();

                x.SetServiceName("KaposkesDownloadCleaner");
                x.SetDisplayName("Kaposke's Download Cleaner");
                x.SetDescription("Limpa arquivos antigos da pasta de downloads.");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
