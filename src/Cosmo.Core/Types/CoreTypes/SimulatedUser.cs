using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Cosmo.Core.Constants;
using Cosmo.Core.Enums;
using Cosmo.Core.Types.EndpointTypes;
using System;
using System.Diagnostics;
using System.Net;
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
        public bool bAsyncUser { get; set; }
        private int Progress { get; set; }
        public int EndpointsToHit { get; set; }
        public bool bHasStartedWork { get; set; } = false;
        public bool bHasFinishedWork => Progress == EndpointsToHit ? true : false;
        public int UserID { get; set; }
        public int NumberOfConcurrentUsers { get; set; }

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

            try
            {
                Stopwatch timer = Stopwatch.StartNew();
                HttpResponseMessage response = TargetAPI.SendAsync(request).Result;
                timer.Stop();

                ++Progress;

                string resultsString =
                    $"Test of {probe.Endpoint}:\n"
                    + $"Method: {probe.Method}\n"
                    + $"Payload: {request.Content.ReadAsStringAsync().Result}\n"
                    + $"Response status code: {(int)response.StatusCode} {response.StatusCode}\n"
                    + $"Round-trip time: {timer.ElapsedMilliseconds} ms\n\n";

                string performanceString =
                    $"{probe.Endpoint},{probe.Method},{UserID},{NumberOfConcurrentUsers},{(int)response.StatusCode},{timer.ElapsedMilliseconds}\n";

                return new ProbeResult
                {
                    ResultsString = resultsString,
                    PerformanceString = performanceString
                };
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
                return new ProbeResult
                {
                    ResultsString = string.Empty,
                    PerformanceString = string.Empty
                };
            }
        }
    }
}