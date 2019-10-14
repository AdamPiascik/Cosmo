using RestfulTestTool.Core.Config;
using RestfulTestTool.Core.Types.EndpointTypes;
using RestfulTestTool.TestInitialiser;
using System.Collections.Generic;

namespace RestfulTestTool.Core.Types.CoreTypes
{
    public class Test
    {
        public TestConfig Configuration { get; set; }
        public TestSchedule Schedule { get; set; }
        public IList<SimulatedUser> SimulatedUserList { get; set; }
        public IList<EndpointProbeResult> ListResults { get; set; }
        
        public Test SetUpTargetAPI()
        {
            TargetAPISetup setup = new TargetAPISetup();

            if (Configuration.UseLocalServer)
                setup.StartLocalServer(Configuration.LocalServerPath, Configuration.LocalPort);

            return this;
        }

        public Test SetUpTestSchedule()
        {
            TestScheduleSetup setup = new TestScheduleSetup();
            return this;
        }

        public Test SetUpSwaggerDocuments()
        {
            SwaggerDocumentSetup setup = new SwaggerDocumentSetup();
            return this;
        }

        public Test SetUpSimulatedUsers()
        {
            SimulatedUserSetup setup = new SimulatedUserSetup();
            return this;
        }

        public Test SetUpAuthDictionaries()
        {
            AuthDictionarySetup setup = new AuthDictionarySetup();
            return this;
        }

        public Test SetUpPayloadDictionaries()
        {
            PayloadDictionarySetup setup = new PayloadDictionarySetup();
            return this;
        }
    }
}