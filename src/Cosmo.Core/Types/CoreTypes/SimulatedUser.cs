using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Cosmo.Core.Constants;
using Cosmo.Core.Enums;
using Cosmo.Core.Types.EndpointTypes;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cosmo.Core.Types.CoreTypes
{
    public class SimulatedUser
    {
        public HttpClient TargetAPI { get; set; }
        public bool bSavePerformanceData { get; set; }
        public bool bSaveResponses { get; set; }

        public async Task<ProbeResult> ExecuteProbe(EndpointProbe probe)
        {
            string payloadString =
                    probe.Payload?.GetType() == typeof(JObject) ?
                    JsonConvert.SerializeObject(probe.Payload) :
                    probe.Payload?.ToString() ?? string.Empty;

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = probe.Method,
                Content = new StringContent(
                        payloadString,
                        Encoding.UTF8,
                        "application/json")               
            };

            Stopwatch timer = Stopwatch.StartNew();

            HttpResponseMessage response = TargetAPI.SendAsync(request).Result;

            timer.Stop();

            string textResults = $"Test of {probe.Endpoint}:\n";
            textResults +=$"\tPayload: {request.Content.ReadAsStringAsync().Result}\n";
            textResults +=$"\tResponse status code: {(int)response.StatusCode} {response.StatusCode}\n";
            textResults +=
                bSavePerformanceData ?
                $"\tRound-trip time: {timer.ElapsedMilliseconds} ms\n" :
                "\n";

            return new ProbeResult
                    {
                        TextResults = textResults
                    };
        }
    }
}