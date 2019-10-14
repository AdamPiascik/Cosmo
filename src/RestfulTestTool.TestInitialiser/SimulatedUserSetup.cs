using RestfulTestTool.Core.Types.ErrorTypes;
using System.Collections.Generic;
using System.Linq;

namespace RestfulTestTool.TestInitialiser
{
    public class SimulatedUserSetup
    {
        public IList<SetupError> Errors { get; set; }
        public bool bSuccessful => Errors.Any() ? false : true;

        public SimulatedUserSetup()
        {
            Errors = new List<SetupError>();
        }
    }    
}