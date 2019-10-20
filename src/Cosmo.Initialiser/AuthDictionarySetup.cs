using Cosmo.Core.Config;
using Cosmo.Core.Types.CoreTypes;
using Cosmo.Core.Types.EndpointTypes;
using Cosmo.Core.Types.ErrorTypes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Cosmo.Initialiser
{
    public class AuthDictionarySetup
    {
        public IList<SetupError> Errors { get; set; }
        public bool bSuccessful => Errors.Any() ? false : true;

        public AuthDictionarySetup()
        {
            Errors = new List<SetupError>();
        }

        public Dictionary<string, string> GenerateAuthDictionary(TestConfig config, ApiConnectionFactory api)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (config.Auths.Any())
            {
                foreach (Auth authItem in config.Auths)
                {

                    AuthLogin login = new AuthLogin(authItem.Username, authItem.Password);
                    var authRequestContent = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = api.NewConnection().PostAsync(authItem.LoginEndpoint, authRequestContent).Result;

                    JObject authJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    string token = authJson["access_token"].Value<string>();

                    foreach (string endpoint in authItem.TargetEndpoints)
                    {
                        dict.Add(ConvertToRegex(endpoint), token);
                    }
                }
            }

            return dict;
        }

        private string ConvertToRegex(string toConvert)
        {
            return toConvert
                    .Replace("*", ".*");
        }

        public class AuthLogin
        {
            public string Username { get; set; }
            public string Password { get; set; }

            public AuthLogin(string username, string password)
            {
                Username = username;
                Password = password;
            }
        }
    }
}