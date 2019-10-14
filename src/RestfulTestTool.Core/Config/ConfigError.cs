using RestfulTestTool.Core.Handlers;

namespace RestfulTestTool.Core.Config
{
    public class ConfigError
    {
        public ErrorLevel Severity { get; set; }
        public ConfigErrorType Type { get; set; }
        public string Message { get; set; }
    }

    public enum ConfigErrorType
    {
        JsonParser,
        
        InvalidCombination
    }
}