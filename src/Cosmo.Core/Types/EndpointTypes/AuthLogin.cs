using System.Collections.Generic;

namespace Cosmo.Core.Types.EndpointTypes
{
    public class Auth
    {
        public string LoginEndpoint { get; set; }
        public IList<string> TargetEndpoints { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}