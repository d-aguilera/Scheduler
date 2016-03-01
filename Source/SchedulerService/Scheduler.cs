using Scheduler.AgentService.Client;
using Scheduler.Common;
using Scheduler.DataAccess;
using Scheduler.DataContracts;
using Scheduler.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceProcess;

namespace Scheduler.SchedulerService
{
    public class Scheduler : SchedulerServiceBase<IScheduler>, IScheduler
    {
        const string EventLogSource = "Scheduler.SchedulerService";

        [PrincipalPermission(SecurityAction.Demand, Name = PrincipalNames.CronService)]
        public void ExecuteMany(IEnumerable<int> scheduleEntryIds)
        {
            if (null == scheduleEntryIds)
                return;

            foreach (var scheduleEntryId in scheduleEntryIds)
            {
                try
                {
                    ExecuteOne(scheduleEntryId);
                }
                catch
                {
                    // Should continue. Exception has been logged in ExecuteOne
                }
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Name = PrincipalNames.Web)]
        public void ExecuteInteractive(int scheduleEntryId, string operatorName)
        {
            ExecuteOne(scheduleEntryId, operatorName);
        }

        void ExecuteOne(int scheduleEntryId, string operatorName = null)
        {
            ScheduleEntry scheduleEntry;

            using (var context = new WebContext())
            {
                scheduleEntry = context.ScheduleEntries
                    .Include(se => se.Client)
                    .Single(se => se.Id == scheduleEntryId);
            }

            var logEntryId = CreateLogEntry(
                scheduleEntry.ShellCommand,
                scheduleEntry.WorkingDirectory,
                scheduleEntry.Id,
                scheduleEntry.ClientId,
                operatorName
                );

            var client = scheduleEntry.Client;

            try
            {
                using (var factory = new AgentServiceClientFactory(client.NetworkName, client.AgentPort, client.AgentVirtualDirectory))
                {
                    WcfHelpers.Using(factory, channel =>
                    {
                        channel.Execute(
                            logEntryId,
                            scheduleEntry.ShellCommand,
                            scheduleEntry.WorkingDirectory
                        );
                    });
                }
            }
            catch (Exception ex)
            {
                var message = Helpers.GetFullExceptionMessage(ex, "Could not contact agent service.", new Dictionary<string, object> {
                    { "scheduleEntry", scheduleEntry },
                    { "client", client },
                });

                Helpers.LogException(message, EventLogSource);

                UpdateLogEntry(logEntryId, null, null, null, null, null, message);

                throw;
            }
        }

        int CreateLogEntry(string shellCommand, string workingDirectory, int scheduleEntryId, int clientId, string operatorName, DateTime? started = null, DateTime? finished = null, int? exitCode = null, int? processId = null, string consoleOut = null, string errorOut = null)
        {
            bool forced;
            string createdBy;
            if (string.IsNullOrEmpty(operatorName))
            {
                createdBy = ServiceSecurityContext.Current.PrimaryIdentity.Name;
                forced = false;
            }
            else
            {
                createdBy = operatorName;
                forced = true;
            }

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

                using (var context = new WebContext())
                {
                    context.Set<LogEntry>().Add(logEntry);
                    context.SaveChanges();
                    return logEntry.Id;
                }
            }
            catch (Exception ex)
            {
                var message = Helpers.GetFullExceptionMessage(ex, "Could not create log entry.", new Dictionary<string, object> {
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
                });

                Helpers.LogException(message, EventLogSource);

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
                using (var context = new WebContext())
                {
                    var logEntry = context.Set<LogEntry>().Find(logEntryId);
                    logEntry.Started = started;
                    logEntry.Finished = finished;
                    logEntry.ExitCode = exitCode;
                    logEntry.ProcessId = processId;
                    logEntry.ConsoleOut = consoleOut;
                    logEntry.ErrorOut = errorOut;
                    logEntry.LastUpdated = DateTime.UtcNow;
                    logEntry.LastUpdatedBy = lastUpdatedBy;

                    context.Entry(logEntry).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var message = Helpers.GetFullExceptionMessage(ex, "Could not update log entry.", new Dictionary<string, object> {
                    { "logEntryId", logEntryId },
                    { "started", started },
                    { "finished", finished },
                    { "exitCode", exitCode },
                    { "processId", processId },
                    { "consoleOut", consoleOut },
                    { "errorOut", errorOut }
                });

                Helpers.LogException(message, EventLogSource);

                throw;
            }
        }

        public void Reload()
        {
            const int CronReload = 201;

            string cronMachine = null;

            try
            {
                cronMachine = ConfigurationManager.AppSettings["Scheduler.CronService.MachineName"] ?? "localhost";
                var sc = new ServiceController("Scheduler.CronService", cronMachine);
                sc.ExecuteCommand(CronReload);
            }
            catch (Exception ex)
            {
                var message = Helpers.GetFullExceptionMessage(ex, "Could not contact cron service.", new Dictionary<string, object> {
                    { "Operation", "Reload" },
                    { "cronMachine", cronMachine },
                });

                Helpers.LogWarning(message, EventLogSource);
            }
        }
    }
}
