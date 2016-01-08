using Scheduler.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Scheduler.AgentService
{
    [ServiceContract]
    public interface IAgent
    {
        [OperationContract(IsOneWay = true)]
        void Execute(string shellCommand);

        [OperationContract]
        void Create(ScheduleEntry scheduleEntry);

        [OperationContract]
        void Update(ScheduleEntry scheduleEntry);
    }
}
