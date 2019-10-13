using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using RestfulTestTool.Core.Handlers;

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
        public List<string> SwaggerDocs { get; set; }

        // Flags
        public bool SavePerformanceData { get; set; } = true;
        public bool SaveResponses { get; set; } = false;

        // Load testing
        public int SimulatedUsers { get; set; } = 1;

        // Config validation
        public IList<ConfigError> ConfigErrors { get; set; }
        public bool IsValid()
        {
            if (SaveResponses && SimulatedUsers > 1)
            {
                AddConfigError(ConfigErrorType.InvalidCombination,
                                ErrorLevel.Warning,
                                ConfigErrorMessages.Mismatch_SaveResponsesAndSimulatedUsers);
                SaveResponses = false;
            }
                
            if (configErrors.Any())
                return false;
            else
                return true;
        }

        public IList<ConfigError> AddConfigError(ConfigErrorType errorType, string message)
        {
            ConfigErrors.Add(new ConfigError
            {
                Type = errorType,
                Message = message
            });
        }
    }
}
