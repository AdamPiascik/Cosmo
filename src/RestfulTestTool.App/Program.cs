using Newtonsoft.Json;
using RestfulTestTool.Core.Config;
using RestfulTestTool.Core.Constants;
using RestfulTestTool.Core.Handlers;
using RestfulTestTool.Core.Types.EndpointTypes;
using RestfulTestTool.TestController;
using System;
using System.Collections.Generic;
using System.IO;

namespace RestfulTestTool.App
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
                            .SetUpAuthDictionaries()
                            .SetUpPayloadDictionaries()
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