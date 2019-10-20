using Cosmo.Core.Constants;
using Cosmo.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Cosmo.Core.Types.ErrorTypes;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

namespace Cosmo.App
{
    public class UpdateChecker
    {
        private const string LatestReleaseDetailsURL = "/repos/AdamPiascik/Cosmo/releases/latest";
        private Version CurrentVersion { get; set; }
        private string CurrentDirectory { get; set; }
        public IList<UpdateError> Errors { get; set; }
        public bool bSuccessful => Errors.Any() ? false : true;

        public UpdateChecker(Version currentVersion, string currentDirectory)
        {
            CurrentVersion = currentVersion;
            CurrentDirectory = currentDirectory;
            Errors = new List<UpdateError>();
        }

        public bool Execute()
        {
            Version latestVersion = new Version();
            string latestVersionTag = string.Empty;
            using (HttpClient gitHubApi = new HttpClient())
            {
                gitHubApi.BaseAddress = new Uri("https://api.github.com");
                gitHubApi.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                gitHubApi.DefaultRequestHeaders.UserAgent.TryParseAdd("CosmoUpdater");

                try
                {
                    string response = gitHubApi.GetStringAsync(LatestReleaseDetailsURL).Result;

                    JObject responseJson = JObject.Parse(response);

                    latestVersionTag = responseJson["tag_name"].Value<string>();

                    latestVersion = new Version(latestVersionTag.Replace("v", string.Empty));
                }
                catch (Exception ex)
                {
                    Errors.Add(
                                new UpdateError
                                {
                                    Severity = ErrorLevel.Warning,
                                    Type = UpdateErrorType.UpdaterNotFound,
                                    Message = ex.Message
                                });
                    return false;
                }
            }

            if (latestVersion != CurrentVersion)
            {
                bool bUpdate = GetUserInput(latestVersion);

                if (bUpdate)
                {
                    Globals.LoggingHandler.LogConsole($"\nUpdating Cosmo to {latestVersion}; this process will exit.");

                    try
                    {
                        Process updater = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = "update_cosmo.exe",
                                Arguments = $"\"{latestVersionTag}\" \"{CurrentDirectory}\"",
                                UseShellExecute = true,
                                CreateNoWindow = false
                            }
                        };

                        updater.Start();
                    }
                    catch (Exception ex)
                    {
                        Errors.Add(
                                new UpdateError
                                {
                                    Severity = ErrorLevel.Warning,
                                    Type = UpdateErrorType.UpdaterNotFound,
                                    Message = ex.Message
                                });
                    }

                    return true;
                }

                return false;
            }

            return false;
        }

        private bool GetUserInput(Version latestVersion)
        {
            Globals.LoggingHandler.LogConsole($"\nYou're not using the latest version of Cosmo. Would you like to update to {latestVersion}? (Y)es/(N)o/(Q)uit:");

            IList<string> positives = new List<string> { "y", "yes" };
            IList<string> negatives = new List<string> { "n", "no" };
            IList<string> exits = new List<string> { "q", "quit" };
            IList<string> allowedAnswers = positives.Concat(negatives).Concat(exits).ToList();
            bool bAnswered = false;
            string answer = "";

            while (!bAnswered)
            {
                answer = Console.ReadLine().ToLower();

                if (allowedAnswers.Contains(answer))
                {
                    bAnswered = true;
                    continue;
                }

                Globals.LoggingHandler.LogConsole("Please enter either (Y)es or (N)o:");
            }

            if (exits.Contains(answer))
            {
                Globals.LoggingHandler.LogConsole("Quitting...");
                Environment.Exit(0);
            }

            if (positives.Contains(answer))
                return true;
            else
                return false;
        }
    }
}