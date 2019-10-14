using RestfulTestTool.Core.Config;
using RestfulTestTool.Core.Enums;
using RestfulTestTool.Core.Handlers;
using RestfulTestTool.Core.Types.CoreTypes;
using RestfulTestTool.Core.Types.EndpointTypes;
using RestfulTestTool.Core.Types.ErrorTypes;
using RestfulTestTool.Core.Types.ResultTypes;
using RestfulTestTool.TestInitialiser;
using System;
using System.Collections.Generic;
using System.Net;

namespace RestfulTestTool.TestController
{
    public class Test
    {
        public TestConfig Configuration { get; set; }
        public ApiConnectionFactory ApiConnectionFactory { get; set; }
        public LocalServer LocalServer { get; set; }
        public TestSchedule TestSchedule { get; set; }
        public TestResources TestResources { get; set; }
        public IList<SimulatedUser> SimulatedUserList { get; set; }

        public ResultSet ResultSet { get; set; }

        public Test(TestConfig testconfig)
        {
            Configuration = testconfig;
            SimulatedUserList = new List<SimulatedUser>();
            TestResources = new TestResources();
        }

        public Test SetUpTargetAPI()
        {
            TargetAPISetup setup = new TargetAPISetup();

            if (Configuration.UseLocalServer)
            {
                setup.TryStartLocalServer(
                        Configuration.LocalServerPath,
                        Configuration.LocalPort,
                        Configuration.Environment,
                        out LocalServer localServer);

                LocalServer = localServer;
            }

            ApiConnectionFactory = setup.CreateApiConnectionFactory(Configuration);
            setup.SetConnectionLimit(Configuration);

            if (!setup.bSuccessful)
                ErrorHandler.InitialisationError(ErrorLevel.Fatal,
                                InitialiserErrorType.TargetAPISetup,
                                setup.Errors);

            return this;
        }

        public Test SetUpTestSchedule()
        {
            TestScheduleSetup setup = new TestScheduleSetup();

            if (!setup.bSuccessful)
                ErrorHandler.InitialisationError(ErrorLevel.Fatal,
                                InitialiserErrorType.TestScheduleSetup,
                                setup.Errors);

            return this;
        }

        public Test SetUpSwaggerDocuments()
        {
            SwaggerDocumentSetup setup = new SwaggerDocumentSetup();
            TestResources.SwaggerDocument = setup.FetchSwaggerDocument(ApiConnectionFactory, Configuration.SwaggerDoc);

            if (!setup.bSuccessful)
                ErrorHandler.InitialisationError(ErrorLevel.Fatal,
                                InitialiserErrorType.SwaggerDocumentSetup,
                                setup.Errors);

            return this;
        }

        public Test SetUpSimulatedUsers()
        {
            SimulatedUserSetup setup = new SimulatedUserSetup();

            if (!setup.bSuccessful)
                ErrorHandler.InitialisationError(ErrorLevel.Fatal,
                                InitialiserErrorType.SimulatedUserSetup,
                                setup.Errors);

            return this;
        }

        public Test SetUpAuthDictionaries()
        {
            AuthDictionarySetup setup = new AuthDictionarySetup();

            if (!setup.bSuccessful)
                ErrorHandler.InitialisationError(ErrorLevel.Fatal,
                                InitialiserErrorType.AuthDictionarySetup,
                                setup.Errors);

            return this;
        }

        public Test SetUpPayloadDictionaries()
        {
            PayloadDictionarySetup setup = new PayloadDictionarySetup();

            if (!setup.bSuccessful)
                ErrorHandler.InitialisationError(ErrorLevel.Fatal,
                                InitialiserErrorType.PayloadDictionarySetup,
                                setup.Errors);

            return this;
        }

        public void Run()
        {
            ResultSet = new ResultSet();

            Coordinator coordinator = new Coordinator();

            // coordinator jobs: assign tasks to users, monitor progress, collect/collate results
        }
    }
}