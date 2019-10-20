using Cosmo.Core.Config;
using Cosmo.Core.Types.CoreTypes;
using Cosmo.Core.Types.ErrorTypes;
using System.Collections.Generic;
using System.Linq;

namespace Cosmo.Initialiser
{
    public class SimulatedUserSetup
    {
        public IList<SetupError> Errors { get; set; }
        public bool bSuccessful => Errors.Any() ? false : true;

        public SimulatedUserSetup()
        {
            Errors = new List<SetupError>();
        }

        public IList<SimulatedUser> PopulateUserList(TestConfig config, ApiConnectionFactory factory)
        {
            IList<SimulatedUser> list = new List<SimulatedUser>();

            for (int i = 0; i < config.SimulatedUsers; ++i)
            {
                list.Add(
                    new SimulatedUser
                    {
                        TargetAPI = factory.NewConnection(),
                        bSaveResponses = config.SaveResponses,
                        bSavePerformanceData = config.SavePerformanceData
                    });
            }

            return list;
        }
    }    
}