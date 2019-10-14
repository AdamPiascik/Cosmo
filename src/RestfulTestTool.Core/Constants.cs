using System.Collections.Generic;

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
}