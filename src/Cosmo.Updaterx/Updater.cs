using Cosmo.Core.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;

namespace Cosmo.Updater
{
    public class Updater
    {
        private static string LatestVersionTag { get; set; }
        private static string LatestReleaseExeURL = $"/AdamPiascik/Cosmo/releases/download/{LatestVersionTag}/cosmo.exe";
        public static void StartUpdateProcess(string latestVersionTag)
        {
            LatestVersionTag = latestVersionTag;

            using (var client = new WebClient())
            {
                client.DownloadFile(LatestReleaseExeURL, "cosmo.exe");
            }
        }
    }
}