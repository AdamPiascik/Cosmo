using Cosmo.Core.Enums;

namespace Cosmo.Core.Types.ErrorTypes
{
    public class ConfigError
    {
        public ErrorLevel Severity { get; set; }
        public ConfigErrorType Type { get; set; }
        public string Message { get; set; }
    }
}