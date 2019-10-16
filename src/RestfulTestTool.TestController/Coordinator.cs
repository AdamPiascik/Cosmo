using RestfulTestTool.Core.Types.CoreTypes;
using RestfulTestTool.Core.Types.ResultTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestfulTestTool.TestController
{
    public class Coordinator
    {
        public Task TestStatus { get; set; }
        public TestSchedule TestSchedule { get; set; }
        public TestResources TestResources { get; set; }
        public IList<SimulatedUser> SimulatedUserList { get; set; }
        public ResultSet ResultSet { get; set; }

        public async void RunTest()
        {
            // coordinator jobs: assign tasks to users, monitor progress, collect/collate results

            while (!TestSchedule.HasBeenCompleted())
            {
                foreach(SimulatedUser user in SimulatedUserList)
                {
                    // Get(user);
                    // user.ExecuteProbe();
                }
                // if any free users, assign next task
            }

            await TestStatus;
        }

        private async void GetProbe(SimulatedUser user)
        {

        }
    }
}