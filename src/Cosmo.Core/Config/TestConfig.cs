using System;
using System.Collections.Generic;
using System.Linq;
using Cosmo.Core.Constants;
using Cosmo.Core.Enums;
using Cosmo.Core.Types.EndpointTypes;
using Cosmo.Core.Types.ErrorTypes;

namespace Cosmo.Core.Config
{
    public class TestConfig
    {
        // Customisation
        public string TestName { get; set; } = $"Cosmo API Test {DateTime.Now.ToString()}";
        public string ProjectName { get; set; }
        public List<string> DataFiles { get; set; }

        // For local server
        public bool UseLocalServer { get; set; } = false;
        public int LocalPort { get; set; }
        public string LocalServerPath { get; set; }
        public string Environment { get; set; }

        // For remote server        
        public string URL { get; set; }

        // Endpoint-related
        public List<Auth> Auths { get; set; } = new List<Auth>();
        public List<string> Include { get; set; } = new List<string>();
        public List<string> Exclude { get; set; }  = new List<string>();
        public List<string> TestMethods { get; set; }
            = new List<string> { "GET", "POST" };
        public string SwaggerDoc { get; set; } = "v1/doc.json";

        // Flags
        public bool UseAsyncUsers => bLoadTest ? false : true;
        public bool CheckForUpdates { get; set; } = false;
        public bool bLogResponses => bLoadTest ? false : true;
        public bool bLogErrors => bLoadTest ? false : true;
        public bool bLogWarnings => bLoadTest ? false : true;

        // Load testing
        public int SimulatedUsers { get; set; } = 1;
        public bool bLoadTest => SimulatedUsers > 1 ? true : false;

        // Config validation
        public IList<ConfigError> Errors { get; set; }

        public TestConfig()
        {
            Errors = new List<ConfigError>();
        }
        public bool HasErrors()
        {
            if (UseAsyncUsers && bLoadTest)
            {
                Errors.Add(new ConfigError
                {
                    Severity = ErrorLevel.PromptWarning,
                    Type = ConfigErrorType.AsyncLoadTesting,
                    Message = 
                        ConfigErrorMessages.Mismatch_UseAsyncUsersAndLoadTest
                            .Replace("[[Users]]", SimulatedUsers.ToString())
                });
            }
                
            if (Errors.Any())
                return true;
            else
                return false;
        }
    }
}
