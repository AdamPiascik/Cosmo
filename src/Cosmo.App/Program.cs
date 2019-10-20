using Newtonsoft.Json;
using Cosmo.Core.Config;
using Cosmo.Core.Constants;
using Cosmo.Core.Handlers;
using Cosmo.Core.Types.EndpointTypes;
using Cosmo.TestController;
using System;
using System.Collections.Generic;
using System.IO;

namespace Cosmo.App
{
    class Program
    {
        static int Main(string[] args)
        {
            Globals.bProgramRunning  = true;

            TestConfig testConfig = ConfigHandler.ConfigureTestParameters(args);
            Globals.LoggingHandler = new LoggingHandler(testConfig.TestName);
            Globals.LoggingHandler.StartLogQueueWatcher();

            Globals.LoggingHandler.LogConsole($"Initialising {testConfig.TestName}...\n");

            Test test = new Test(testConfig)
                            .SetUpTargetAPI()
                            .SetUpSwaggerDocuments()
                            .SetUpPayloadDictionaries()
                            .SetUpAuthDictionaries()
                            .SetUpSimulatedUsers()
                            .SetUpTestSchedule();

            Globals.LoggingHandler.LogConsole($"Running {testConfig.TestName}...\n");

            Globals.LoggingHandler.LogConsole(test.TestSchedule.EndpointProbeList.Count.ToString());

            // test.Run();

            // ResultsHandler.HandleResultSet(test.ResultSet);

            Globals.LoggingHandler.WaitForLoggingCompletion();

            Globals.bProgramRunning  = false;           

            return 0;
        }
    }
}