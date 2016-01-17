using Scheduler.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Scheduler.SchedulerService
{
    [ServiceContract]
    public interface IScheduler
    {
        [OperationContract]
        void ExecuteInteractive(int scheduleEntryId, string operatorName);

        [OperationContract]
        void ExecuteMany(IEnumerable<int> scheduleEntryIds);

        [OperationContract]
        void UpdateLogEntry(int logEntryId, DateTime? started, DateTime? finished, int? exitCode, int? processId, string consoleOut, string errorOut);

        [OperationContract(IsOneWay = true)]
        void Reload();
    }
}