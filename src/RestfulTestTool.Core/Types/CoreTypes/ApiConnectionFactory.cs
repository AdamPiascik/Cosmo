using RestfulTestTool.Core.Constants;
using System;
using System.Net.Http;

namespace RestfulTestTool.Core.Types.CoreTypes
{
    public class ApiConnectionFactory
    {
        public Uri URL { get; set; }

        public HttpClient NewConnection()
        {
            HttpClient httpClient =
                new HttpClient
                {
                    BaseAddress = URL,
                    Timeout = TimeSpan.FromSeconds(Defaults.RequestTimeoutInSeconds)
                };

            httpClient.DefaultRequestHeaders.Add("bTestData", new string[] { "1" });

            return httpClient;
        }
    }
}