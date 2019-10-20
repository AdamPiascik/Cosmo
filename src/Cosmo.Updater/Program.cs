using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;

namespace Cosmo.Updater
{
    class Program
    {
        private static string LatestVersionTag { get; set; }
        private static string CosmoDirectory { get; set; }
        private static string LatestReleaseExeURL => $"https://github.com/AdamPiascik/Cosmo/releases/download/{LatestVersionTag}/cosmo.exe";
        public static void UpdateCosmoApp()
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.Headers.Add("User-Agent", "CosmoUpdater");
                    client.DownloadFile(LatestReleaseExeURL, "cosmo.temp.exe");

                    File.Delete("cosmo.exe");
                    File.Move("cosmo.temp.exe", "cosmo.exe");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
        }

        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid arguments: don't run this updater directly!");
                Console.ReadKey();
                return 0;
            }

            LatestVersionTag = args[0];
            CosmoDirectory = args[1];

            Console.WriteLine($"Updating Cosmo executable at {CosmoDirectory} to {LatestVersionTag}...");

            UpdateCosmoApp();

            Console.WriteLine("Update finished! Press any key to exit.");
            Console.ReadKey();

            return 0;
        }
    }
}
