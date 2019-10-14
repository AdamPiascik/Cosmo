using RestfulTestTool.Core.Config;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulTestTool.Core.Types.CoreTypes
{
    public class TestSchedule
    {
        public int RepetitionsPerEndpoint { get; set; }
        public ConcurrentBag<string> EndpointList { get; set; }
        public IList<int> OrderofAccess { get; set; }
        public ConcurrentDictionary<string, int> RecordOfWork { get; set; }

        public TestSchedule(TestConfig config, SwaggerDocument swaggerDoc)
        {
            RepetitionsPerEndpoint = config.SimulatedUsers;
            EndpointList = GenerateEndpointList(swaggerDoc);
            OrderofAccess = GenerateOrderOfAccess();
            RecordOfWork = InitialiseRecordOfWork();
        }

        private ConcurrentBag<string> GenerateEndpointList(SwaggerDocument swaggerDoc)
        {
            ConcurrentBag<string> list = new ConcurrentBag<string>();

            Parallel.ForEach(
                swaggerDoc.Paths.Keys,
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                path =>
                {
                    list.Add(path);
                });

            return list;
        }

        private IList<int> GenerateOrderOfAccess()
        {
            int listLength = EndpointList.Count * RepetitionsPerEndpoint;
            var rng = new Random();
            IList<int> list = Enumerable.Range(1, listLength).OrderBy(x => rng.Next()).ToList();

            return list;
        }

        private ConcurrentDictionary<string, int> InitialiseRecordOfWork()
        {
            ConcurrentDictionary<string, int> dict = new ConcurrentDictionary<string, int>();

            Parallel.ForEach(
                EndpointList,
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                endpoint =>
                {
                    dict.AddOrUpdate(endpoint, 0, (matchedKey, value) => value);
                });

            return dict;
        }
    }
}