using RestfulTestTool.Core.Config;
using RestfulTestTool.Core.Types.EndpointTypes;
using RestfulTestTool.Core.Types.ErrorTypes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RestfulTestTool.Core.Types.CoreTypes
{
    public class TestSchedule
    {
        private int Progress { get; set; }
        public int RepetitionsPerEndpoint { get; set; }
        public ConcurrentBag<EndpointProbe> EndpointProbeList { get; set; }
        public IList<int> ProgrammeOfWork { get; set; }
        public ConcurrentDictionary<string, int> RecordOfWork { get; set; }
        public IList<SetupError> Errors { get; set; }

        public TestSchedule()
        {
            RepetitionsPerEndpoint = 0;
            EndpointProbeList = new ConcurrentBag<EndpointProbe>();
            ProgrammeOfWork = new List<int>();
            RecordOfWork = new ConcurrentDictionary<string, int>();
        }

        public bool HasBeenCompleted()
        {
            return Progress == ProgrammeOfWork.Count ? true : false;
        }
    }
}