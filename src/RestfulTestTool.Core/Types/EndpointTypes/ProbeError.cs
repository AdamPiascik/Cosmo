using RestfulTestTool.Core.Handlers;

namespace RestfulTestTool.Core.Types.EndpointTypes
{
    public class ProbeError
    {
        public ErrorLevel Severity { get; set; }
        public ProbeErrorType Type { get; set; }
        public string Message { get; set; }
    }

    public enum ProbeErrorType
    {
        UnknownPayloadContentType
    }
}