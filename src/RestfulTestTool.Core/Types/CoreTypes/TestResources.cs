using System.Collections.Generic;
using System.Linq;

namespace RestfulTestTool.Core.Types.CoreTypes
{
    public class TestResources
    {
        public SwaggerDocument SwaggerDocument { get; set; }
        public Dictionary<string, string> AuthDictionary { get; set; }
        public Dictionary<string, dynamic> PayloadDictionary { get; set; }

        public TestResources()
        {
            AuthDictionary = new Dictionary<string, string>();
            PayloadDictionary = new Dictionary<string, dynamic>();
        }
    }
}