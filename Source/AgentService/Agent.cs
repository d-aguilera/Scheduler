using Scheduler.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Scheduler.AgentService
{
    public class Agent : IAgent
    {
        public void Execute(string shellCommand)
        {
        }

        public void Create(ScheduleEntry scheduleEntry)
        {
        }

        public void Update(ScheduleEntry scheduleEntry)
        {
        }
    }
}
