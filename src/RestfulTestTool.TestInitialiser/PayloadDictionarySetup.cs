using RestfulTestTool.Core.Config;
using RestfulTestTool.Core.Types.ErrorTypes;
using System.Collections.Generic;
using System.Linq;

namespace RestfulTestTool.TestInitialiser
{
    public class PayloadDictionarySetup
    {
        public IList<SetupError> Errors { get; set; }
        public bool bSuccessful => Errors.Any() ? false : true;

        public PayloadDictionarySetup()
        {
            Errors = new List<SetupError>();
        }

        public Dictionary<string, dynamic> GeneratePayloadDictionary(TestConfig config)
        {
            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();

            if (config.DataFiles.Any())
            {

            }

            return dict;
        }
    }    
}