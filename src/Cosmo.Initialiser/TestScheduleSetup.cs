using Cosmo.Core.Config;
using Cosmo.Core.Constants;
using Cosmo.Core.Enums;
using Cosmo.Core.Types.CoreTypes;
using Cosmo.Core.Types.ErrorTypes;
using System.Collections.Generic;
using System.Linq;
using Cosmo.Core.Types.EndpointTypes;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Cosmo.Initialiser
{
    public class TestScheduleSetup
    {
        public IList<SetupError> Errors { get; set; }
        public bool bSuccessful => Errors.Any() ? false : true;

        public TestScheduleSetup()
        {
            Errors = new List<SetupError>();
        }

        public TestSchedule GenerateSchedule(TestConfig config, TestResources resources)
        {
            TestSchedule schedule = new TestSchedule();

            if (resources.SwaggerDocument == null)
                Errors.Add(
                            new SetupError
                            {
                                Severity = ErrorLevel.Fatal,
                                Type = InitialiserErrorType.TestScheduleSetup,
                                Message = InitialiserErrorMessages.TestSchedule_NullSwaggerDoc
                            });

            if (resources.SwaggerDocument.Paths == null || !(resources.SwaggerDocument.Paths.Any()))
                Errors.Add(
                            new SetupError
                            {
                                Severity = ErrorLevel.Fatal,
                                Type = InitialiserErrorType.TestScheduleSetup,
                                Message = InitialiserErrorMessages.TestSchedule_NoEndpointsinSwaggerDoc
                            });

            if (Errors.Any())
                return schedule;
            else
            {
                schedule.RepetitionsPerEndpoint = config.SimulatedUsers;
                schedule.EndpointProbeList = GetEndpointProbeList(resources, config);
                schedule.ProgrammeOfWork = GetProgrammeOfWork(schedule.EndpointProbeList.Count, schedule.RepetitionsPerEndpoint);
                schedule.SpinUpDelayInMilliseconds = (int) ((config.SpinUpTime * 1000) / config.SimulatedUsers);

                return schedule;
            }
        }

        private IList<EndpointProbe> GetEndpointProbeList(TestResources resources, TestConfig config)
        {
            BlockingCollection<EndpointProbe> list = new BlockingCollection<EndpointProbe>();

            Parallel.ForEach(
                resources.SwaggerDocument.Paths,
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                path =>
                {
                    Parallel.ForEach(
                        Defaults.HttpMethods.Intersect(config.TestMethods, StringComparer.OrdinalIgnoreCase),
                        new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                        method =>
                        {
                            CosmoOperation methodInfo = path.Value
                                         .GetType()
                                         .GetProperty(method)
                                         .GetValue(path.Value) as CosmoOperation;

                            if (methodInfo != null)
                            {
                                dynamic payload = GetEndpointPayload(path.Key, method, resources.PayloadDictionary);
                                list.Add(
                                    new EndpointProbe
                                    {
                                        Endpoint = path.Key,
                                        Method = new HttpMethod(method),
                                        AuthToken = GetEndpointAuthToken(path.Key, resources.AuthDictionary),
                                        Payload = payload,
                                        PayloadMIMEType = GetPayloadType(path.Key, methodInfo.Consumes, payload),
                                        ExpectedResponseMIMEType = GetResponseType(path.Key, methodInfo.Produces)
                                    }
                                );
                            }
                        }
                    );
                });
            
            list.CompleteAdding();

            return list.ToList();
        }

        private IList<int> GetProgrammeOfWork(int numberOfProbes, int repetitionsPerEndpoint)
        {
            int listLength = numberOfProbes * repetitionsPerEndpoint;
            var rng = new Random();
            IList<int> list = Enumerable.Range(0, listLength).OrderBy(x => rng.Next()).ToList();
            list = list.Select(x => x % numberOfProbes).ToList();

            return list;
        }

        private string GetEndpointAuthToken(string endpoint, Dictionary<string, string> authDictionary)
        {
            if (!authDictionary.Any())
                return null;

            foreach (KeyValuePair<string, string> item in authDictionary)
            {
                Regex regex = new Regex(item.Key);
                if (regex.Match(endpoint).Success)
                {
                    return item.Value;
                }
            }
             return null;
        }

        private dynamic GetEndpointPayload(
            string endpoint,
            string method, 
            ConcurrentDictionary<string, ConcurrentDictionary<string, dynamic>> payloadDictionary)
        {
            if (!payloadDictionary.Any())
                return null;

            if (payloadDictionary.TryGetValue(endpoint, out ConcurrentDictionary<string, dynamic> methodDict))
            {
                if (methodDict.TryGetValue(method, out dynamic payload))
                {
                    return payload;
                }                    
                else
                    return null;
            }                
            else
                return null;
        }

        private MediaTypeHeaderValue GetPayloadType(string endpoint, IList<string> payloadTypes, dynamic payload)
        {
            if (!payloadTypes.Any())
            {
                if (payload != null)
                {
                    Errors.Add(
                            new SetupError
                            {
                                Severity = ErrorLevel.Warning,
                                Type = InitialiserErrorType.EndpointMIMEError,
                                Message = InitialiserErrorMessages.EndpointMIMEError_NoMIMETypeSpecified
                                            .Replace("[[Endpoint]]", endpoint)
                                            .Replace("[[Type]]", "payload")
                            });

                    return new MediaTypeHeaderValue("application/json");
                }
                else
                    return null;
            }

            if (payloadTypes.Count > 1)
            {
                Errors.Add(
                            new SetupError
                            {
                                Severity = ErrorLevel.Warning,
                                Type = InitialiserErrorType.EndpointMIMEError,
                                Message = InitialiserErrorMessages.EndpointMIMEError_MultipleMIMETypesSpecified
                                            .Replace("[[Endpoint]]", endpoint)
                                            .Replace("[[FirstType]]", payloadTypes.First())
                                            .Replace("[[Type]]", "payload")
                            });
            }

            try
            {
                return new MediaTypeHeaderValue(payloadTypes.First());
            }
            catch
            {
                Errors.Add(
                    new SetupError
                    {
                        Severity = ErrorLevel.Warning,
                        Type = InitialiserErrorType.EndpointMIMEError,
                        Message = InitialiserErrorMessages.EndpointMIMEError_MIMETypeParsingError
                                    .Replace("[[Endpoint]]", endpoint)
                                    .Replace("[[FailingType]]", payloadTypes.First())
                                    .Replace("[[Type]]", "payload")
                    });

                return new MediaTypeHeaderValue("application/json");
            }
        }

        private MediaTypeHeaderValue GetResponseType(string endpoint, IList<string> responseTypes)
        {
            if (!responseTypes.Any())
            {
                Errors.Add(
                        new SetupError
                        {
                            Severity = ErrorLevel.Warning,
                            Type = InitialiserErrorType.EndpointMIMEError,
                            Message = InitialiserErrorMessages.EndpointMIMEError_NoMIMETypeSpecified
                                        .Replace("[[Endpoint]]", endpoint)
                                        .Replace("[[Type]]", "response")
                        });

                return new MediaTypeHeaderValue("application/json");
            }

            if (responseTypes.Count > 1)
            {
                Errors.Add(
                            new SetupError
                            {
                                Severity = ErrorLevel.Warning,
                                Type = InitialiserErrorType.EndpointMIMEError,
                                Message = InitialiserErrorMessages.EndpointMIMEError_MultipleMIMETypesSpecified
                                            .Replace("[[Endpoint]]", endpoint)
                                            .Replace("[[FirstType]]", responseTypes.First())
                                            .Replace("[[Type]]", "response")
                            });
            }

            try
            {
                return new MediaTypeHeaderValue(responseTypes.First());
            }
            catch
            {
                Errors.Add(
                    new SetupError
                    {
                        Severity = ErrorLevel.Warning,
                        Type = InitialiserErrorType.EndpointMIMEError,
                        Message = InitialiserErrorMessages.EndpointMIMEError_MIMETypeParsingError
                                    .Replace("[[Endpoint]]", endpoint)
                                    .Replace("[[FailingType]]", responseTypes.First())
                                    .Replace("[[Type]]", "response")
                    });

                return new MediaTypeHeaderValue("application/json");
            }
        }
    }
}