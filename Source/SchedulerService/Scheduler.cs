using Scheduler.AgentService.Client;
using Scheduler.Common;
using Scheduler.DataAccess;
using Scheduler.DataContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;

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

        public void ExecuteMany(IEnumerable<int> scheduleEntryIds, bool forced)
        {
            if (null == scheduleEntryIds)
                return;

            foreach (var scheduleEntryId in scheduleEntryIds)
            {
                try
                {
                    var scheduleEntry = Context.ScheduleEntries.Find(scheduleEntryId);
                    Execute(scheduleEntry, forced);
                }
                catch
                {
                    // Should continue. Exception has been logged in Execute(ScheduleEntry, bool)
                }
            }
        }

        public void Execute(ScheduleEntry scheduleEntry, bool forced)
        {
            if (null == scheduleEntry)
                return;

            int logEntryId = CreateLogEntry(
                scheduleEntry.ShellCommand,
                scheduleEntry.WorkingDirectory,
                scheduleEntry.Id,
                scheduleEntry.ClientId,
                null,
                null,
                null,
                null,
                null,
                null,
                forced
                );

            var client = Context.Set<Client>().Find(scheduleEntry.ClientId);

            var agentServiceClient = AgentServiceClientFactory.CreateChannel(
                client.NetworkName,
                client.AgentPort,
                client.AgentVirtualDirectory
                );

            try
            {
                agentServiceClient.Execute(
                    logEntryId,
                    scheduleEntry.ShellCommand,
                    scheduleEntry.WorkingDirectory
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

                EventLog.WriteEntry(EventLogSource, message, EventLogEntryType.Error, 1);

                UpdateLogEntry(logEntryId, null, null, null, null, null, message);

                throw;
            }
        }

        int CreateLogEntry(string shellCommand, string workingDirectory, int scheduleEntryId, int clientId, DateTime? started, DateTime? finished, int? exitCode, int? processId, string consoleOut, string errorOut, bool forced)
        {
            var sc = ServiceSecurityContext.Current;
            var createdBy = null == sc ? "?" : sc.PrimaryIdentity.Name;

            started = started != null && started.HasValue ? started.Value.ToUniversalTime() : started;
            finished = finished != null && finished.HasValue ? finished.Value.ToUniversalTime() : finished;

            try
            {
                var logEntry = new LogEntry
                {
                    ShellCommand = shellCommand,
                    WorkingDirectory = workingDirectory,
                    ScheduleEntryId = scheduleEntryId,
                    ClientId = clientId,
                    Started = started,
                    Finished = finished,
                    ExitCode = exitCode,
                    ProcessId = processId,
                    ConsoleOut = consoleOut,
                    ErrorOut = errorOut,
                    Forced = forced,
                    Created = DateTime.UtcNow,
                    CreatedBy = createdBy,
                };

                Context.Set<LogEntry>().Add(logEntry);
                Context.SaveChanges();
                return logEntry.Id;
            }
            catch (Exception ex)
            {
                var args = new Dictionary<string, object> {
                    { "shellCommand", shellCommand },
                    { "workingDirectory", workingDirectory },
                    { "scheduleEntryId", scheduleEntryId },
                    { "clientId", clientId },
                    { "started", started },
                    { "finished", finished },
                    { "exitCode", exitCode },
                    { "processId", processId },
                    { "consoleOut", consoleOut },
                    { "errorOut", errorOut },
                    { "forced", forced }
                };
                var message = Helpers.GetFullExceptionMessage("Could not create log entry.", ex, args);

                EventLog.WriteEntry(EventLogSource, message, EventLogEntryType.Error, 1);

                throw;
            }
        }

        public void UpdateLogEntry(int logEntryId, DateTime? started, DateTime? finished, int? exitCode, int? processId, string consoleOut, string errorOut)
        {
            var sc = ServiceSecurityContext.Current;
            var lastUpdatedBy = null == sc ? "?" : sc.PrimaryIdentity.Name;

            started = started != null && started.HasValue ? started.Value.ToUniversalTime() : started;
            finished = finished != null && finished.HasValue ? finished.Value.ToUniversalTime() : finished;

            try
            {
                var logEntry = Context.Set<LogEntry>().Find(logEntryId);
                logEntry.Started = started;
                logEntry.Finished = finished;
                logEntry.ExitCode = exitCode;
                logEntry.ProcessId = processId;
                logEntry.ConsoleOut = consoleOut;
                logEntry.ErrorOut = errorOut;
                logEntry.LastUpdated = DateTime.UtcNow;
                logEntry.LastUpdatedBy = lastUpdatedBy;

                Context.Entry(logEntry).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                var args = new Dictionary<string, object> {
                    { "logEntryId", logEntryId },
                    { "started", started },
                    { "finished", finished },
                    { "exitCode", exitCode },
                    { "processId", processId },
                    { "consoleOut", consoleOut },
                    { "errorOut", errorOut }
                };
                var message = Helpers.GetFullExceptionMessage("Could not update log entry.", ex, args);

                EventLog.WriteEntry(EventLogSource, message, EventLogEntryType.Error, 1);

                throw;
            }
        }

        public void Reload()
        {
            const int CronReload = 1;

            try
            {
                var cronMachine = ConfigurationManager.AppSettings["Scheduler.CronService.MachineName"] ?? "localhost";
                var sc = new ServiceController("Scheduler.CronService", cronMachine);
                sc.ExecuteCommand(CronReload);
            }
            catch (Exception ex)
            {
                var message = Helpers.GetFullExceptionMessage("Could not contact cron service.", ex);
                EventLog.WriteEntry(EventLogSource, message, EventLogEntryType.Warning, 2);
            }
        }
    }
}
