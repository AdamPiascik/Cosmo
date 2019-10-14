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
    }    
}