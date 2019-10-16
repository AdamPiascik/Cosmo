using System.Collections.Generic;
using System.Collections.Concurrent;

namespace RestfulTestTool.Core.Types.CoreTypes
{
    public class TestResources
    {
        public SwaggerDocument SwaggerDocument { get; set; }
        public Dictionary<string, string> AuthDictionary { get; set; }
        public ConcurrentDictionary<string, ConcurrentDictionary<string, dynamic>> PayloadDictionary { get; set; }

        public TestResources()
        {
            AuthDictionary = new Dictionary<string, string>();
            PayloadDictionary = new ConcurrentDictionary<string, ConcurrentDictionary<string, dynamic>>();
        }
    }
}