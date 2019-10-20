using Cosmo.Core.Handlers;
using System;
using System.Collections.Generic;

namespace Cosmo.Core.Constants
{
    public static class ConfigErrorMessages
    {
        public const string Mismatch_SaveResponsesAndSimulatedUsers = 
            "You can't save responses when simulating more than one user. Response saving has been turned off.";
    }

    public static class ProbeErrorMessages
    {
        public const string Payload_UnknownContentType = 
            "The content type of the request payload for [[Endpoint]] wasn't recognised. Content type (from Swagger): [[ContentType]]";
    }

    public static class InitialiserErrorMessages
    {
        public const string Swagger_FetchError = 
            "The Swagger document [[Name]] couldn't be fetched. Response code: [[StatusCode]] [[StatusMessage]]";
        public const string Swagger_ParseError = 
            "The Swagger document [[Name]] couldn't be parsed. Exception message: [[Message]]";
        public const string TestSchedule_NullSwaggerDoc = 
            "The test schedule couldn't be generated as a null Swagger document was passed in";
        public const string TestSchedule_NoEndpointsinSwaggerDoc = 
            "The test schedule couldn't be generated as the passed Swagger document contained no endpoints";
        public const string EndpointMIMEError_NoMIMETypeSpecified = 
            "No [[Type]] MIME type was specified for [[Endpoint]]. The [[Type]] will be treated as application/json";
        public const string EndpointMIMEError_MultipleMIMETypesSpecified = 
            "Multiple [[Type]] MIME types were specified for [[Endpoint]]. The [[Type]] will be treated as the first one ([[FirstType]])";
        public const string EndpointMIMEError_MIMETypeParsingError = 
            "The specified [[Type]] MIME type for [[Endpoint]] (\"[[FailingType]]\") couldn't be parsed. The [[Type]] will be treated as application/json";
        public const string PayloadDictionary_NoDataFile = 
            "The provided data file ([[File]]) could not be found";
        public const string PayloadDictionary_DataFileParseError = 
            "The provided data file ([[File]]) could not be parsed. Exception message: [[Message]]";
    }

    public static class Defaults
    {
        public const string BasePath = @".";
        public static string ThisTestDirectory =>
            $"{DateTime.Now.Date.ToString("dd-MM-yyy")}";
        public const string ConfigFile = @".\cosmo.config.json";
        public const string ConsoleLogFile = @"console.log";
        public const string ErrorsFile = @"errors.log";
        public const string WarningsFile = @"warnings.log";
        public const string ResponseFile = @"responses.log";
        public const string PerformanceFile = @"performance.log";
        public const double ConnectionScaleFactor = 1.2;
        public const int RequestTimeoutInSeconds = 10;
        public static IList<string> HttpMethods =
            new List<string>
            {
                "Get", "Post"
            };
    }

    public static class Globals
    {
        public static LoggingHandler LoggingHandler{ get; set; }
        public static bool bProgramRunning { get; set; }
    }
}