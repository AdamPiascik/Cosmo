using RestfulTestTool.Core.Types.CoreTypes;
using RestfulTestTool.Core.Types.ErrorTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RestfulTestTool.TestInitialiser
{
    public class TargetAPISetup
    {
        public IList<SetupError> Errors { get; set; }

        public bool TryStartLocalServer(string serverPath, int port, string environment, out LocalServer localServer)
        {
            Console.Write("Starting local test server...");

            localServer = new LocalServer();

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
            Console.WriteLine("Done!\n");

            return true;
        }
    }    
}