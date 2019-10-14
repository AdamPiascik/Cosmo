using RestfulTestTool.Core.Config;
using RestfulTestTool.Core.Handlers;
using RestfulTestTool.Core.Types.ResultTypes;
using RestfulTestTool.TestController;
using System;

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

            test.Run();

            ResultsHandler.HandleResultSet(test.ResultSet);
            
            return 0;
        }
    }
}
