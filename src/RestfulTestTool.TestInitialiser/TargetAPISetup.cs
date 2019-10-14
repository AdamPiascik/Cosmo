using RestfulTestTool.Core.Config;
using RestfulTestTool.Core.Constants;
using RestfulTestTool.Core.Enums;
using RestfulTestTool.Core.Types.CoreTypes;
using RestfulTestTool.Core.Types.ErrorTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace RestfulTestTool.TestInitialiser
{
    public class TargetAPISetup
    {
        private Uri TargetApiUrl { get; set; }
        public IList<SetupError> Errors { get; set; }
        public bool bSuccessful => Errors.Any() ? false : true;

        public TargetAPISetup()
        {
            Errors = new List<SetupError>();
        }

        public bool TryStartLocalServer(string serverPath, int port, string environment, out LocalServer localServer)
        {
            Console.Write("Starting local test server...");

            localServer = new LocalServer();

            try
            {
                localServer.Process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $@" run --project ""{serverPath}"" --launch-profile ""{environment.ToLower()}""",
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        RedirectStandardError = false,
                        CreateNoWindow = true
                    }
                };

                localServer.Process.Start();
                localServer.bRunning = true;
            }
            catch (Exception ex)
            {
                Errors.Add(
                        new SetupError
                        {
                            Severity = ErrorLevel.Fatal,
                            Type = InitialiserErrorType.TargetAPISetup,
                            Message = ex.Message
                        });
            }

            Console.WriteLine("Done!\n");

            return true;
        }

        public void SetConnectionLimit(TestConfig config)
        {
            ServicePointManager
                .FindServicePoint(TargetApiUrl)
                .ConnectionLimit = Convert.ToInt32(config.SimulatedUsers * Defaults.ConnectionScaleFactor);

        }

        public ApiConnectionFactory CreateApiConnectionFactory(TestConfig config)
        {
            try
            {
                if (config.UseLocalServer)
                    TargetApiUrl = new Uri(config.URL);
                else
                    TargetApiUrl = new Uri($"localhost:{config.LocalPort}");                
            }
            catch (Exception ex)
            {
                Errors.Add(
                    new SetupError
                    {
                        Severity = ErrorLevel.Fatal,
                        Type = InitialiserErrorType.TargetAPISetup,
                        Message = ex.Message
                    });
            }

            return new ApiConnectionFactory()
            {
                URL = TargetApiUrl
            };
        }
    }
}