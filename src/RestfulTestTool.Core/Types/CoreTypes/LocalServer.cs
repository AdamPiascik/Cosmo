using System.Diagnostics;

namespace RestfulTestTool.Core.Types.CoreTypes
{
    public class LocalServer
    {
        public bool bRunning { get; set; }
        public Process Process { get; set; }
    }
}