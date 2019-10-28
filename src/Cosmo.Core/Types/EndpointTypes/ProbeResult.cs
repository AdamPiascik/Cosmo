namespace Cosmo.Core.Types.EndpointTypes
{
    public class ProbeResult
    {
        public string Endpoint { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string RequestPayload { get; set; } = string.Empty;
        public int UserID { get; set; }
        public int ConcurrentUsersAtRequestTime { get; set; }
        public bool bSuccessResponse { get; set; }
        public bool bRequestTimeout { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
        public long RoundTripTime { get; set; }

        public string ResultsString =>
            $"Test of {Endpoint}:\n"
            + $"Method: {Method}\n"
            + $"Payload: {RequestPayload}\n"
            + $"Response status code: {StatusCode} {StatusMessage}\n"
            + $"Round-trip time: {RoundTripTime} ms\n\n";
        public string PerformanceString =>
            $"{Endpoint},{Method},{UserID},{ConcurrentUsersAtRequestTime},{StatusCode},{RoundTripTime}\n";
    }
}