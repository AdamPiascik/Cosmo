using RestfulTestTool.Core.Config;
using RestfulTestTool.Core.Enums;
using RestfulTestTool.Core.Handlers;
using RestfulTestTool.Core.Types.CoreTypes;
using RestfulTestTool.Core.Types.EndpointTypes;
using RestfulTestTool.TestInitialiser;
using System.Collections.Generic;

namespace RestfulTestTool.TestController
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
            bool successfulSetup = false;

            if (Configuration.UseLocalServer)
                successfulSetup = setup.TryStartLocalServer(Configuration.LocalServerPath, Configuration.LocalPort);

            if (!successfulSetup)
                ErrorHandler.InitialisationError(ErrorLevel.Fatal,
                                InitialiserErrorType.TargetAPISetup,
                                setup.Errors);

            return this;
        }

        public Test SetUpTestSchedule()
        {
            TestScheduleSetup setup = new TestScheduleSetup();
            bool successfulSetup = false;

            if (!successfulSetup)
                ErrorHandler.InitialisationError(ErrorLevel.Fatal,
                                InitialiserErrorType.TestScheduleSetup,
                                setup.Errors);
            
            return this;
        }

        public Test SetUpSwaggerDocuments()
        {
            SwaggerDocumentSetup setup = new SwaggerDocumentSetup();
            bool successfulSetup = false;

            if (!successfulSetup)
                ErrorHandler.InitialisationError(ErrorLevel.Fatal,
                                InitialiserErrorType.SwaggerDocumentSetup,
                                setup.Errors);

            return this;
        }

        public Test SetUpSimulatedUsers()
        {
            SimulatedUserSetup setup = new SimulatedUserSetup();
            bool successfulSetup = false;

            if (!successfulSetup)
                ErrorHandler.InitialisationError(ErrorLevel.Fatal,
                                InitialiserErrorType.SimulatedUserSetup,
                                setup.Errors);

            return this;
        }

        public Test SetUpAuthDictionaries()
        {
            AuthDictionarySetup setup = new AuthDictionarySetup();
            bool successfulSetup = false;

            if (!successfulSetup)
                ErrorHandler.InitialisationError(ErrorLevel.Fatal,
                                InitialiserErrorType.AuthDictionarySetup,
                                setup.Errors);

            return this;
        }

        public Test SetUpPayloadDictionaries()
        {
            PayloadDictionarySetup setup = new PayloadDictionarySetup();
            bool successfulSetup = false;

            if (!successfulSetup)
                ErrorHandler.InitialisationError(ErrorLevel.Fatal,
                                InitialiserErrorType.PayloadDictionarySetup,
                                setup.Errors);

            return this;
        }
    }
}