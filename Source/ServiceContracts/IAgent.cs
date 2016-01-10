using Scheduler.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Scheduler.AgentService
{
    [ServiceContract]
    public interface IAgent
    {
        [OperationContract(IsOneWay = true)]
        void Execute(int logEntryId, string shellCommand, string workingDirectory);
    }
}
