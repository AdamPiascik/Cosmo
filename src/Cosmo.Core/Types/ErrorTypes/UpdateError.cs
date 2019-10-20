using Cosmo.Core.Enums;

namespace Cosmo.Core.Types.ErrorTypes
{
    public class UpdateError
    {
        public ErrorLevel Severity { get; set; }
        public UpdateErrorType Type { get; set; }
        public string Message { get; set; }
    }
}