using RestfulTestTool.Core.Types.ErrorTypes;
using System.Collections.Generic;

namespace RestfulTestTool.TestInitialiser
{
    public class TargetAPISetup
    {
        public IList<SetupError> Errors { get; set; }

        public bool TryStartLocalServer(string serverPath, int port)
        {
            return true;
        }
    }    
}