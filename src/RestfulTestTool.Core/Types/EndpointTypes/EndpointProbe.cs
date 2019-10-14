using RestfulTestTool.Core.Handlers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RestfulTestTool.Core.Types.EndpointTypes
{
    public class EndpointProbe
    {
        public Endpoint Endpoint { get; set; }
        public HttpMethod Method { get; set; }
        public dynamic Payload { get; set; }
        public MediaTypeHeaderValue PayloadMIMEType { get; set; }
        public MediaTypeHeaderValue ExpectedResponseMIMEType { get; set; }
        public IList<ProbeError> Errors { get; set; }

        public bool HasErrors()
        {                
            if (Errors.Any())
                return false;
            else
                return true;
        }

        public void AddError(ErrorLevel level, ProbeErrorType type, string message)
        {
            Errors.Add( new ProbeError 
            {
                Severity = level,
                Type = type,
                Message = message
            });
        }
    }
}