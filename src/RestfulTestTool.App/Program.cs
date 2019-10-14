using RestfulTestTool.Core.Config;
using RestfulTestTool.Core.Handlers;
using RestfulTestTool.Core.Types.ResultTypes;
using RestfulTestTool.TestController;

namespace RestfulTestTool.App
{
    class Program
    {
        static int Main(string[] args)
        {
            TestConfig testConfig = ConfigHandler.ConfigureTestParameters(args);

            Test test = new Test()
                            .SetUpTargetAPI()
                            .SetUpSwaggerDocuments()
                            .SetUpAuthDictionaries()
                            .SetUpPayloadDictionaries()
                            .SetUpSimulatedUsers()
                            .SetUpTestSchedule();

            ResultSet resultSet = test.Run();

            ResultsHandler.HandleResultSet(resultSet);
            
            return 0;
        }
    }
}
