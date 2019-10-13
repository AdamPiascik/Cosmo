using System.Collections.Generic;

namespace RestfulTestTool.Core.Config
{
    public class ConfigError
    {
        public ErrorLevel Severity { get; set; }
        public ConfigErrorType Type { get; set; }
        public string Message { get; set; }
    }

    enum ConfigErrorType
    {
        JsonParser,
        
        InvalidCombination
    }
}