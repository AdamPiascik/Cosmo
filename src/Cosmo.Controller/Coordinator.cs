using Cosmo.Core.Types.CoreTypes;
using Cosmo.Core.Types.EndpointTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmo.TestController
{
    public class Coordinator
    {
        public Task TestStatus { get; set; }
        public TestSchedule TestSchedule { get; set; }
        public TestResources TestResources { get; set; }
        public IList<SimulatedUser> SimulatedUserList { get; set; }
        public IList<ProbeResult> ResultSet { get; set; }

        public void RunTest()
        {
            // coordinator jobs: assign tasks to users, monitor progress, collect/collate results

            while(!TestSchedule.HasBeenCompleted)
            {
                ResultSet.Add(AssignProbe(SimulatedUserList.First()));
            };

            foreach(ProbeResult result in ResultSet)
            {
                Console.WriteLine(result.TextResults);
            }

            // while (!TestSchedule.HasBeenCompleted)
            // {
            //     foreach(SimulatedUser user in SimulatedUserList)
            //     {
            //         AssignProbe(user);
                    
            //         // user.ExecuteProbe();
            //     }
            //     // if any free users, assign next task
            // }

            // await TestStatus;
        }

        private ProbeResult AssignProbe(SimulatedUser user)
        {
            return user.ExecuteProbe(TestSchedule.GetNextProbe()).Result;
        }
    }
}