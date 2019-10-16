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

            Test test = new Test(testConfig)
                            .SetUpTargetAPI()
                            .SetUpSwaggerDocuments()
                            .SetUpAuthDictionaries()
                            .SetUpPayloadDictionaries()
                            .SetUpSimulatedUsers()
                            .SetUpTestSchedule();

            foreach(EndpointProbe item in test.TestSchedule.EndpointProbeList)
            {
                Console.WriteLine($"{item.Endpoint}:\n\t{item.Method.Method.ToUpper()}:\n\t\t{JsonConvert.SerializeObject(item.Payload)}\n\t\t{item.PayloadMIMEType},\n\t\t{item.ExpectedResponseMIMEType}");
            }

            // test.Run();

            // ResultsHandler.HandleResultSet(test.ResultSet);
            
            return 0;
        }
    }
}