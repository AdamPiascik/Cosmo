using RestfulTestTool.Core.Handlers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RestfulTestTool.Core.Types.EndpointTypes
{
    public class Endpoint
    {
        public string Name { get; set; }
        public string URL { get; set; }
    }
}