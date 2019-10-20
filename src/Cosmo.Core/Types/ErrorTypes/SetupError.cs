using Cosmo.Core.Enums;

namespace Cosmo.Core.Types.ErrorTypes
{
    public class SetupError
    {
        public ErrorLevel Severity { get; set; }
        public InitialiserErrorType Type { get; set; }
        public string Message { get; set; }
    }
}