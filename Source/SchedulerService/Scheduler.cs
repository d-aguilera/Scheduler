using Scheduler.AgentService.Client;
using Scheduler.Common;
using Scheduler.DataAccess;
using Scheduler.DataContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Scheduler.SchedulerService
{
    public class Scheduler : IScheduler
    {
        const string EventLogSource = "Scheduler.SchedulerService";

        private WebContext _dbContext = new WebContext();

        WebContext Context
        {
            get
            {
                return _dbContext;
            }
        }

        public void Execute(ScheduleEntry scheduleEntry, bool forced)
        {
            if (null == scheduleEntry)
                throw new ArgumentNullException("scheduleEntry");

            var client = Context.Set<Client>().Find(scheduleEntry.ClientId);

            var agentServiceClient = AgentServiceClientFactory.CreateChannel(
                client.NetworkName,
                client.AgentPort,
                client.AgentVirtualDirectory
                );

            try
            {
                agentServiceClient.Execute(
                    scheduleEntry.ShellCommand,
                    scheduleEntry.WorkingDirectory,
                    scheduleEntry.Id,
                    scheduleEntry.ClientId,
                    forced
                    );
            }
            catch (Exception ex)
            {
                var args = new Dictionary<string, object> {
                    { "scheduleEntry", scheduleEntry },
                    { "client", client },
                    { "agentServiceClient", agentServiceClient },
                };
                var message = Helpers.GetFullExceptionMessage("Could not execute command.", ex, args);

                EventLog.WriteEntry(EventLogSource, message, EventLogEntryType.Error);

                throw;
            }
        }

        public void LogExecution(LogEntry logEntry)
        {
            try
            {
                Context.Set<LogEntry>().Add(logEntry);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                var args = new Dictionary<string, object> {
                    { "logEntry", logEntry}
                };
                var message = Helpers.GetFullExceptionMessage("Could not log execution.", ex, args);

                EventLog.WriteEntry(EventLogSource, message, EventLogEntryType.Error);

                throw;
            }
        }
    }
}
