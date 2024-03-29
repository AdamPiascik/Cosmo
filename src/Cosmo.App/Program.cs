﻿using Newtonsoft.Json;
using Cosmo.Core.Config;
using Cosmo.Core.Constants;
using Cosmo.Core.Handlers;
using Cosmo.Core.Types.EndpointTypes;
using Cosmo.Controller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Cosmo.App
{
    class Program
    {
        static int Main(string[] args)
        {
            Globals.bProgramRunning = true;

            Stopwatch stopwatch = Stopwatch.StartNew();

            TestConfig testConfig = ConfigHandler.ConfigureTestParameters(args);
            Globals.LoggingHandler = new LoggingHandler(testConfig);
            Globals.LoggingHandler.StartLogQueueWatcher();

            Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

            Globals.LoggingHandler.LogConsole($"\nCosmo {currentVersion.Major}.{currentVersion.Minor}.{currentVersion.Build}\n");

            UpdateChecker updateChecker = new UpdateChecker(currentVersion, Environment.CurrentDirectory);

            bool bUpdateApp = updateChecker.Execute();

            ResultsSummary resultsSummary = new ResultsSummary();

            if (!bUpdateApp)
            {
                Globals.LoggingHandler.LogConsole($"\nInitialising {testConfig.TestName}...");

                Test test = new Test(testConfig)
                                .SetUpTargetAPI()
                                .SetUpSwaggerDocuments()
                                .SetUpPayloadDictionaries()
                                .SetUpAuthDictionaries()
                                .SetUpSimulatedUsers()
                                .SetUpTestSchedule();

                Globals.LoggingHandler.LogConsole("finished!\n");
                Globals.LoggingHandler.LogConsole($"\nRunning {testConfig.TestName}...");

                test.Run();

                resultsSummary = test.HandleResultSet();
            }

            Globals.LoggingHandler.LogConsole("finished!\n\n");

            Globals.LoggingHandler.LogConsole
                (
                    Defaults.TestSummaryMessage
                        .Replace("[[Endpoints]]", resultsSummary.EndpointsTested.ToString())
                        .Replace("[[Successes]]", resultsSummary.EndpointsPassed.ToString())
                        .Replace("[[Failures]]", resultsSummary.EndpointsFailed.ToString())
                );

            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            Globals.LoggingHandler.LogConsole("\nWriting logs...");

            Globals.LoggingHandler.WaitForLoggingCompletion();

            Globals.LoggingHandler.LogConsole("finished!\n");

            Globals.bProgramRunning = false;

            return 0;
        }
    }
}