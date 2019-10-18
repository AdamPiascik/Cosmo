using Newtonsoft.Json;
using RestfulTestTool.Core.Config;
using RestfulTestTool.Core.Handlers;
using RestfulTestTool.Core.Types.EndpointTypes;
using RestfulTestTool.TestController;
using System;
using System.Collections.Generic;

namespace RestfulTestTool.App
{
    class Program
    {
        static int Main(string[] args)
        {
            TestConfig testConfig = ConfigHandler.ConfigureTestParameters(args);

            Console.WriteLine($"Initialising {testConfig.TestName}...\n");

            Test test = new Test(testConfig)
                            .SetUpTargetAPI()
                            .SetUpSwaggerDocuments()
                            .SetUpAuthDictionaries()
                            .SetUpPayloadDictionaries()
                            .SetUpSimulatedUsers()
                            .SetUpTestSchedule();

            Console.WriteLine($"Running {testConfig.TestName}...\n");

            Console.WriteLine(test.TestSchedule.EndpointProbeList.Count);

            // test.Run();

            // ResultsHandler.HandleResultSet(test.ResultSet);
            
            return 0;
        }
    }
}