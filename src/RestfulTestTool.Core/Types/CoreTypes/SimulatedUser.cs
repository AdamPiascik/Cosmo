using RestfulTestTool.Core.Constants;
using RestfulTestTool.Core.Enums;
using RestfulTestTool.Core.Types.EndpointTypes;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace RestfulTestTool.Core.Types.CoreTypes
{
    public class SimulatedUser
    {
        public HttpClient TargetAPI { get; set; }
        public bool bSavePerformanceData { get; set; }
        public bool bSaveResponses { get; set; }

        public async void ExecuteProbe(EndpointProbe probe)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = probe.Method,
                Content = GenerateRequestPayload(probe),
            };

            Stopwatch stopwatch = new Stopwatch();

            if (bSavePerformanceData)
                stopwatch.Start();

            await TargetAPI.SendAsync(request);

            if (bSavePerformanceData)
            {
                stopwatch.Stop();
                // update performance record
            }

            if (bSaveResponses)
            {
                // update response dictionary
            }

            // trigger probe complete event
        }

        private HttpContent GenerateRequestPayload(EndpointProbe probe)
        {
            dynamic payload = probe.Payload;
            string payloadType = probe.PayloadMIMEType.MediaType;

            switch (payloadType)
            {
                case var contentType when new Regex(@"json").IsMatch(payloadType):
                    return new StringContent(payload, Encoding.UTF8, "application/json");

                case var contentType when new Regex(@"xml").IsMatch(payloadType):
                    return new StringContent(payload, Encoding.UTF8, "application/xml");

                case var contentType when new Regex(@"text/plain").IsMatch(payloadType):
                    return new StringContent(payload, Encoding.UTF8, "text/plain");

                default:
                    probe.AddError(ErrorLevel.Warning,
                                   ProbeErrorType.UnknownPayloadContentType,
                                   ProbeErrorMessages.Payload_UnknownContentType
                                      .Replace("[[Endpoint]]", probe.Endpoint
                                      .Replace("[[ContentType]]", payloadType)));                    
                    return new StringContent(payload.ToString(), Encoding.UTF8, "text/plain");
            }
        }
    }
}