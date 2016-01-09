using Scheduler.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Scheduler.SchedulerService
{
    [ServiceContract]
    public interface IScheduler
    {
        [OperationContract(IsOneWay = true)]
        void Execute(ScheduleEntry scheduleEntry, bool forced);

        [OperationContract(IsOneWay = true)]
        void LogExecution(LogEntry logEntry);
    }
}