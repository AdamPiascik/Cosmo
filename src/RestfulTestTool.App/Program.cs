using RestfulTestTool.Core.Config;
using RestfulTestTool.Core.Handlers;
using RestfulTestTool.Core.Types.CoreTypes;

namespace RestfulTestTool.App
{
    class Program
    {
        static void Main(string[] args)
        {
            TestConfig testConfig = ConfigHandler.ConfigureTestParameters(args);

            Test test = new Test()
                            .SetUpTargetAPI()
                            .SetUpSwaggerDocuments()
                            .SetUpAuthDictionaries()
                            .SetUpPayloadDictionaries()
                            .SetUpSimulatedUsers()
                            .SetUpTestSchedule();

            // RunTest;

            // ParseResults;
        }
    }
}
