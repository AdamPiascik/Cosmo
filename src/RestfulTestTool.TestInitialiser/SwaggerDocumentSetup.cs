using Newtonsoft.Json;
using RestfulTestTool.Core.Enums;
using RestfulTestTool.Core.Constants;
using RestfulTestTool.Core.Types.CoreTypes;
using RestfulTestTool.Core.Types.ErrorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace RestfulTestTool.TestInitialiser
{
    public class SwaggerDocumentSetup
    {
        public IList<SetupError> Errors { get; set; }
        public bool bSuccessful => Errors.Any() ? false : true;

        public SwaggerDocumentSetup()
        {
            Errors = new List<SetupError>();
        }

        public SwaggerDocument FetchSwaggerDocument(ApiConnectionFactory factory, string swaggerDocName)
        {
            SwaggerDocument swaggerDoc = new SwaggerDocument();

            using (var connection = factory.NewConnection())
            {
                HttpResponseMessage response = connection.GetAsync(swaggerDocName).Result;
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        swaggerDoc = JsonConvert.DeserializeObject<SwaggerDocument>(response.Content.ReadAsStringAsync().Result);
                    }
                    catch (Exception ex)
                    {
                        Errors.Add(new SetupError
                        {
                            Severity = ErrorLevel.Fatal,
                            Type = InitialiserErrorType.SwaggerDocumentSetup,
                            Message =
                                InitialiserErrorMessages.Swagger_ParseError
                                    .Replace("[[Name]]", swaggerDocName)
                                    .Replace("[[Message]]", ex.Message)
                        });
                    }
                }
                
                else
                    Errors.Add(new SetupError
                    {
                        Severity = ErrorLevel.Fatal,
                        Type = InitialiserErrorType.SwaggerDocumentSetup,
                        Message =
                            InitialiserErrorMessages.Swagger_FetchError
                                .Replace("[[Name]]", swaggerDocName)
                                .Replace("[[StatusCode]]", ((int)response.StatusCode).ToString())
                                .Replace("[[StatusMessage]]", response.StatusCode.ToString())
                    });
            }

            return swaggerDoc;
        }
    }
}