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
        private const double ConnectionScaleFactor = 1.2;
        public TestConfig Configuration { get; set; }
        public LocalServer LocalServer { get; set; }
        public TestSchedule Schedule { get; set; }
        public IList<SimulatedUser> SimulatedUserList { get; set; }
        public IList<EndpointProbeResult> ListResults { get; set; }
        
        public Test SetUpTargetAPI()
        {
            TargetAPISetup setup = new TargetAPISetup();
            bool successfulSetup = false;
            Uri targetApiUrl = null;

            if (Configuration.UseLocalServer)
            {
                successfulSetup =
                    setup.TryStartLocalServer(
                            Configuration.LocalServerPath,
                            Configuration.LocalPort,
                            Configuration.Environment,
                            out LocalServer localServer);

                targetApiUrl = new Uri($"localhost:{Configuration.LocalPort}");
                LocalServer = localServer;
            }
            else
            {
                try
                {
                    targetApiUrl = new Uri(Configuration.URL);
                }
                catch (Exception ex)
                {
                    setup.Errors.Add(
                        new SetupError
                        {
                            Severity = ErrorLevel.Fatal,
                            Type = InitialiserErrorType.TargetAPISetup,
                            Message = ex.Message
                        });
                }
            }

            ServicePointManager
                .FindServicePoint(targetApiUrl)
                .ConnectionLimit = Convert.ToInt32(Configuration.SimulatedUsers * ConnectionScaleFactor);

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

        public ResultSet Run()
        {
            ResultSet results = new ResultSet();

            Coordinator coordinator = new Coordinator();

            // coordinator jobs: assign tasks to users, monitor progress, collect/collate results

            return results;
        }
    }
}