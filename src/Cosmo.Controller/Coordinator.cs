using Cosmo.Core.Constants;
using Cosmo.Core.Types.CoreTypes;
using Cosmo.Core.Types.EndpointTypes;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cosmo.Controller
{
    public class Coordinator
    {
        public Task TestStatus { get; set; }
        public TestSchedule TestSchedule { get; set; }
        public TestResources TestResources { get; set; }
        public IList<SimulatedUser> SimulatedUserList { get; set; }
        public IList<ProbeResult> ResultSet { get; set; }
        private delegate void FreeUserHandler(SimulatedUser user);
        private event FreeUserHandler RaiseNewFreeUser;

        public void RunTest()
        {
            Globals.LoggingHandler.LogPerformance(Defaults.PerformanceLogHeader);
            RaiseNewFreeUser += AssignProbe;

            int endpointsToHit = TestSchedule.EndpointProbeList.Count;

            // initially assign one probe to each user
            foreach (SimulatedUser user in SimulatedUserList)
            {
                user.EndpointsToHit = endpointsToHit;
                user.bHasStartedWork = true;
                AssignProbe(user);
                Thread.Sleep(Defaults.UserStartIntervalInMilliseconds);
            }

            while (!TestSchedule.HasBeenCompleted)
            {
                Thread.Sleep(50);
            };
        }

        public void HandleResultSet()
        {
            foreach (ProbeResult result in ResultSet)
            {
                Globals.LoggingHandler.LogResponse(result.ResultsString);
                Globals.LoggingHandler.LogPerformance(result.PerformanceString);
            }
        }

        private async void AssignProbe(SimulatedUser user)
        {
            if (user.bHasFinishedWork)
                return;

            user.NumberOfConcurrentUsers = 
                SimulatedUserList.Where(x => 
                    x.bHasStartedWork && !x.bHasFinishedWork ).Count();

            EndpointProbe probe = TestSchedule.GetNextProbe();

            await Task.Run(async () =>
            {
                if (probe != null)
                {
                    if (user.bAsyncUser)
                    {
                        AssignProbe(user);
                    }

                    ProbeResult probeResult = await user.ExecuteProbe(probe);

                    ResultSet.Add(probeResult);
                    RaiseNewFreeUser(user);
                }
            });
        }
    }
}