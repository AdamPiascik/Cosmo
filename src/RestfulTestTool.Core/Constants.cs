namespace RestfulTestTool.Core.Constants
{
    public class ConfigErrorMessages
    {
        public const string Mismatch_SaveResponsesAndSimulatedUsers = 
            "You can't save responses when simulating more than one user. Response saving has been turned off.";
    }

    public class ProbeErrorMessages
    {
        public const string Payload_UnknownContentType = 
            "The content type of the request payload for [[Endpoint]] wasn't recognised. Content type (from Swagger): [[ContentType]]";
    }

    public class InitialiserErrorMessages
    {
        public const string Swagger_FetchError = 
            "The Swagger document [[Name]] couldn't be fetched. Response code: [[StatusCode]] [[StatusMessage]]";
        public const string Swagger_ParseError = 
            "The Swagger document [[Name]] couldn't be parsed. Exception message: [[Message]]";
    }

    public class Defaults
    {
        public const string ConfigFile = @".\rtt.config.json";
        public const double ConnectionScaleFactor = 1.2;
        public const int RequestTimeoutInSeconds = 10;
    }
}