using RestfulTestTool.Core.Enums;

namespace RestfulTestTool.Core.Types.ErrorTypes
{
    public class ProbeError
    {
        public ErrorLevel Severity { get; set; }
        public ProbeErrorType Type { get; set; }
        public string Message { get; set; }
    }
}