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
        public int Progress { get; set; }
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

                return new ProbeResult
                {
                    Endpoint = probe.Endpoint,
                    Method = probe.Method.Method,
                    RequestPayload = request.Content.ReadAsStringAsync().Result,
                    UserID = UserID,
                    ConcurrentUsersAtRequestTime = NumberOfConcurrentUsers,
                    bSuccessResponse = response.IsSuccessStatusCode ? true : false,
                    StatusCode = (int)response.StatusCode,
                    StatusMessage = response.StatusCode.ToString(),
                    RoundTripTime = timer.ElapsedMilliseconds                    
                };
            }
            catch (TaskCanceledException)
            {                
                return new ProbeResult
                {
                    Endpoint = probe.Endpoint,
                    Method = probe.Method.Method,
                    RequestPayload = request.Content.ReadAsStringAsync().Result,
                    UserID = UserID,
                    ConcurrentUsersAtRequestTime = NumberOfConcurrentUsers,
                    bRequestTimeout = true,
                    StatusMessage = $"Client-side request timeout (longer than {Defaults.RequestTimeoutInSeconds})"
                };
            }
            finally
            {
                ++Progress;
            }
        }
    }
}