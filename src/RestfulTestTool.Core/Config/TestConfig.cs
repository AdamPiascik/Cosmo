using System;
using System.Collections.Generic;
using System.Linq;
using RestfulTestTool.Core.Constants;
using RestfulTestTool.Core.Enums;
using RestfulTestTool.Core.Types.ErrorTypes;

namespace RestfulTestTool.Core.Config
{
    public class TestConfig
    {
        // Customisation
        public string TestName { get; set; } = DateTime.Now.ToString();
        public string ProjectName { get; set; }
        public bool Verbose { get; set; }
        public List<string> DataFiles { get; set; }

        // For local server
        public bool UseLocalServer { get; set; } = false;
        public int LocalPort { get; set; }
        public string LocalServerPath { get; set; }
        public string Environment { get; set; }

        // For remote server        
        public string URL { get; set; }

        // Endpoint-related
        public List<string> AuthEndpoints { get; set; }
        public List<string> Include { get; set; }
        public List<string> Exclude { get; set; }
        public string SwaggerDoc { get; set; }

        // Flags
        public bool SavePerformanceData { get; set; } = true;
        public bool SaveResponses { get; set; } = false;

        // Load testing
        public int SimulatedUsers { get; set; } = 1;

        // Config validation
        public IList<ConfigError> Errors { get; set; }

        public TestConfig()
        {
            Errors = new List<ConfigError>();
        }
        public bool HasErrors()
        {
            if (SaveResponses && SimulatedUsers > 1)
            {
                Errors.Add(new ConfigError
                {
                    Severity = ErrorLevel.Warning,
                    Type = ConfigErrorType.InvalidCombination,
                    Message = ConfigErrorMessages.Mismatch_SaveResponsesAndSimulatedUsers
                });
                SaveResponses = false;
            }
                
            if (Errors.Any())
                return true;
            else
                return false;
        }
    }
}
