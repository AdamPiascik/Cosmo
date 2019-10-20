using Newtonsoft.Json;
using Cosmo.Core.Config;
using Cosmo.Core.Constants;
using Cosmo.Core.Enums;
using Cosmo.Core.Types.CoreTypes;
using Cosmo.Core.Types.EndpointTypes;
using Cosmo.Core.Types.ErrorTypes;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmo.Initialiser
{
    public class PayloadDictionarySetup
    {
        public IList<SetupError> Errors { get; set; }
        public bool bSuccessful => Errors.Any() ? false : true;

        public PayloadDictionarySetup()
        {
            Errors = new List<SetupError>();
        }

        public ConcurrentDictionary<string, ConcurrentDictionary<string, dynamic>> GeneratePayloadDictionary(
            TestConfig config, SwaggerDocument swaggerDoc)
        {
            ConcurrentDictionary<string, ConcurrentDictionary<string, dynamic>> dict =
                new ConcurrentDictionary<string, ConcurrentDictionary<string, dynamic>>(Environment.ProcessorCount, 20);

            if (!config.DataFiles.Any())
                return dict;

            if (!File.Exists(config.DataFiles.First()))
            {
                Errors.Add(
                            new SetupError
                            {
                                Severity = ErrorLevel.Fatal,
                                Type = InitialiserErrorType.PayloadDictionarySetup,
                                Message = InitialiserErrorMessages.PayloadDictionary_NoDataFile
                                            .Replace("[[File]]", config.DataFiles.First())
                            });
                
                return dict;
            }

            IList<EndpointData> endpointData = new List<EndpointData>();

            try
            {
                using (StreamReader file = File.OpenText(config.DataFiles.First()))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    endpointData = (IList<EndpointData>)serializer
                                        .Deserialize(file, typeof(IList<EndpointData>));
                }
            }
            catch (Exception ex)
            {
                Errors.Add(
                            new SetupError
                            {
                                Severity = ErrorLevel.Fatal,
                                Type = InitialiserErrorType.PayloadDictionarySetup,
                                Message = InitialiserErrorMessages.PayloadDictionary_DataFileParseError
                                            .Replace("[[File]]", config.DataFiles.First())
                                            .Replace("[[Message]]", ex.Message)
                            });
                
                return dict;
            }

            Parallel.ForEach(
                swaggerDoc.Paths,
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                path =>
                {
                    Parallel.ForEach(
                        Defaults.HttpMethods,
                        new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                        method =>
                        {
                            CosmoOperation methodInfo = path.Value
                                         .GetType()
                                         .GetProperty(method)
                                         .GetValue(path.Value) as CosmoOperation;

                            if (methodInfo != null)
                            {
                                ConcurrentDictionary<string, dynamic> methodDataDictionary = 
                                    new ConcurrentDictionary<string, dynamic>(Environment.ProcessorCount, 2);

                                dynamic methodData =
                                    endpointData.Where(x =>
                                                        x.URL == path.Key
                                                        && x.Method.ToLower() == method.ToLower())
                                                        .FirstOrDefault()?.Data;

                                methodDataDictionary.TryAdd(method, methodData);

                                dict.AddOrUpdate(
                                    path.Key,
                                    methodDataDictionary,
                                    ((key, val) => val.TryAdd(method, methodData)));
                            }
                        }
                    );
                });
            
            return dict;
        }
    }
}