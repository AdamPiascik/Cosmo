using Cosmo.Core.Constants;
using Cosmo.Core.Config;
using Cosmo.Core.Types.CoreTypes;
using System;
using System.Net;
using System.Net.Http;

namespace Cosmo.Core.Types.CoreTypes
{
    public class ApiConnectionFactory
    {
        public Uri URL { get; set; }
        public int MaxOutgoingConnections { get; set; } = 10;

        public HttpClient NewConnection()
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                MaxConnectionsPerServer = MaxOutgoingConnections
            };

            HttpClient httpClient = new HttpClient(handler)
            {
                BaseAddress = URL,
                Timeout = TimeSpan.FromSeconds(Defaults.RequestTimeoutInSeconds),

            };

            httpClient.DefaultRequestHeaders.Add("bTestData", new string[] { "1" });

            return httpClient;
        }

        public void SetConnectionLimit(TestConfig config, SwaggerDocument swaggerDoc)
        {
            int connections =
                Convert.ToInt32(
                    config.UseAsyncUsers ?
                    (config.SimulatedUsers * swaggerDoc.Paths.Count) :
                    config.SimulatedUsers * Defaults.ConnectionScaleFactor);

            ServicePointManager
                .FindServicePoint(URL)
                .ConnectionLimit = connections > 1500 ? 1500 : connections;                   

            MaxOutgoingConnections = connections > 1500 ? 1500 : connections;
        }
    }
}