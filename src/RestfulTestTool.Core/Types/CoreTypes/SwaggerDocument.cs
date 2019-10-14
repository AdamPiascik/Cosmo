using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace RestfulTestTool.Core.Types.CoreTypes
{
    public class SwaggerDoc
    {
        public string Swagger { get; set; }
        public Info Info { get; set; }
        public string Host { get; set; }
        public string BasePath { get; set; }
        public IList<string> Schemes { get; set; }
        public IList<string> Consumes { get; set; }
        public IList<string> Produces { get; set; }
        public IDictionary<string, rttPathItem> Paths { get; set; }
        public IDictionary<string, Schema> Definitions { get; set; }
        public IDictionary<string, rttParameter> Parameters { get; set; }
        public IDictionary<string, Response> Responses { get; set; }
        public IDictionary<string, rttSecurityScheme> SecurityDefinitions { get; set; }
        public IList<IDictionary<string, IEnumerable<string>>> Security { get; set; }
        public IList<Tag> Tags { get; set; }
        public ExternalDocs ExternalDocs { get; set; }
    }

    public class rttParameter
    {
        string Name { get; set; }

        string In { get; set; }

        string Description { get; set; }

        bool Required { get; set; }
    }

    public class rttPathItem
    {

        [JsonProperty("$ref")]
        public string Ref { get; set; }
        public rttOperation Get { get; set; }
        public rttOperation Put { get; set; }
        public rttOperation Post { get; set; }
        public rttOperation Delete { get; set; }
        public rttOperation Options { get; set; }
        public rttOperation Head { get; set; }
        public rttOperation Patch { get; set; }
        public IList<rttParameter> Parameters { get; set; }
    }

    public class rttOperation
    {
        public IList<string> Tags { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public ExternalDocs ExternalDocs { get; set; }
        public string OperationId { get; set; }
        public IList<string> Consumes { get; set; }
        public IList<string> Produces { get; set; }
        public IList<rttParameter> Parameters { get; set; }
        public IDictionary<string, Response> Responses { get; set; }
        public IList<string> Schemes { get; set; }
        public bool? Deprecated { get; set; }
        public IList<IDictionary<string, IEnumerable<string>>> Security { get; set; }
        [JsonExtensionData]
        public Dictionary<string, object> Extensions { get; private set; }
    }

    public class rttSecurityScheme
    {
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
