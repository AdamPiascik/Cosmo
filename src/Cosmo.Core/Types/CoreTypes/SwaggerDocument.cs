using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace Cosmo.Core.Types.CoreTypes
{
    public class SwaggerDocument
    {
        public string Swagger { get; set; }
        public Info Info { get; set; }
        public string Host { get; set; }
        public string BasePath { get; set; }
        public IList<string> Schemes { get; set; }
        public IList<string> Consumes { get; set; }
        public IList<string> Produces { get; set; }
        public IDictionary<string, CosmoPathItem> Paths { get; set; }
        public IDictionary<string, Schema> Definitions { get; set; }
        public IDictionary<string, CosmoParameter> Parameters { get; set; }
        public IDictionary<string, Response> Responses { get; set; }
        public IDictionary<string, CosmoSecurityScheme> SecurityDefinitions { get; set; }
        public IList<IDictionary<string, IEnumerable<string>>> Security { get; set; }
        public IList<Tag> Tags { get; set; }
        public ExternalDocs ExternalDocs { get; set; }
    }

    public class CosmoParameter
    {
        string Name { get; set; }

        string In { get; set; }

        string Description { get; set; }

        bool Required { get; set; }
    }

    public class CosmoPathItem
    {

        [JsonProperty("$ref")]
        public string Ref { get; set; }
        public CosmoOperation Get { get; set; }
        public CosmoOperation Put { get; set; }
        public CosmoOperation Post { get; set; }
        public CosmoOperation Delete { get; set; }
        public CosmoOperation Options { get; set; }
        public CosmoOperation Head { get; set; }
        public CosmoOperation Patch { get; set; }
        public IList<CosmoParameter> Parameters { get; set; }
    }

    public class CosmoOperation
    {
        public IList<string> Tags { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public ExternalDocs ExternalDocs { get; set; }
        public string OperationId { get; set; }
        public IList<string> Consumes { get; set; }
        public IList<string> Produces { get; set; }
        public IList<CosmoParameter> Parameters { get; set; }
        public IDictionary<string, Response> Responses { get; set; }
        public IList<string> Schemes { get; set; }
        public bool? Deprecated { get; set; }
        public IList<IDictionary<string, IEnumerable<string>>> Security { get; set; }
        [JsonExtensionData]
        public Dictionary<string, object> Extensions { get; private set; }
    }

    public class CosmoSecurityScheme
    {
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
