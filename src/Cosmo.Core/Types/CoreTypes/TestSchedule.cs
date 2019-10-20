using Cosmo.Core.Config;
using Cosmo.Core.Types.EndpointTypes;
using Cosmo.Core.Types.ErrorTypes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Cosmo.Core.Types.CoreTypes
{
    public class TestSchedule
    {
        private int Progress { get; set; }
        private int NumberOfProbes => EndpointProbeList.Count;
        public bool HasBeenCompleted => Progress == ProgrammeOfWork.Count ? true : false;
        public int RepetitionsPerEndpoint { get; set; }
        public IList<EndpointProbe> EndpointProbeList { get; set; }
        public IList<int> ProgrammeOfWork { get; set; }
        public ConcurrentDictionary<string, int> RecordOfWork { get; set; }
        public IList<SetupError> Errors { get; set; }

        public TestSchedule()
        {
            Progress = 0;
            RepetitionsPerEndpoint = 0;
            EndpointProbeList = new List<EndpointProbe>();
            ProgrammeOfWork = new List<int>();
            RecordOfWork = new ConcurrentDictionary<string, int>();
        }

        public EndpointProbe GetNextProbe()
        {
            EndpointProbe probe = EndpointProbeList[ProgrammeOfWork[Progress]];
            ++Progress;
            return probe;
        }
    }
}