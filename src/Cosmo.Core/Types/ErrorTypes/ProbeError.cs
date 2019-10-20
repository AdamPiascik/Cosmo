using Cosmo.Core.Enums;

namespace Cosmo.Core.Types.ErrorTypes
{
    public class ProbeError
    {
        public ErrorLevel Severity { get; set; }
        public ProbeErrorType Type { get; set; }
        public string Message { get; set; }
    }
}