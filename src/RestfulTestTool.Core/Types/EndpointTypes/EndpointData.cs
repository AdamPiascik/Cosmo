namespace RestfulTestTool.Core.Types.EndpointTypes
{
    public class EndpointData
    {
        public string URL { get; set; }
        public string Method { get; set; } = "GET";
        public dynamic Data { get; set; }
    }
}