using RestfulTestTool.Core.Config;
using RestfulTestTool.Core.Types.ErrorTypes;
using System.Collections.Generic;
using System.Linq;

namespace RestfulTestTool.TestInitialiser
{
    public class AuthDictionarySetup
    {
        public IList<SetupError> Errors { get; set; }
        public bool bSuccessful => Errors.Any() ? false : true;

        public AuthDictionarySetup()
        {
            Errors = new List<SetupError>();
        }

        public Dictionary<string, string> GenerateAuthDictionary(TestConfig config)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (config.AuthEndpoints.Any())
            {

            }

            return dict;
        }
    }    
}