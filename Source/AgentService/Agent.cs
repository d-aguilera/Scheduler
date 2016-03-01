using Scheduler.Common;
using Scheduler.SchedulerService.Client;
using Scheduler.ServiceContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Permissions;

namespace Scheduler.AgentService
{
    public class Agent : SchedulerServiceBase<IAgent>, IAgent
    {
        const string EventLogSource = "Scheduler.AgentService";

        [PrincipalPermission(SecurityAction.Demand, Name = PrincipalNames.SchedulerService)]
        public void Execute(int logEntryId, string shellCommand, string workingDirectory)
        {
            var started = DateTime.UtcNow;
            var finished = default(DateTime);
            var exitCode = default(int);
            var processId = default(int?);
            var consoleOut = string.Empty;
            var errorOut = string.Empty;

            try
            {
                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                        WorkingDirectory = workingDirectory,
                        FileName = "cmd.exe",
                        Arguments = "/C " + shellCommand,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    process.OutputDataReceived += (sender, args) => consoleOut += args.Data + Environment.NewLine;
                    process.ErrorDataReceived += (sender, args) => errorOut += args.Data + Environment.NewLine;
                    process.Start();
                    processId = process.Id;
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();

                    started = process.StartTime;
                    finished = process.ExitTime;
                    exitCode = process.ExitCode;

                };
            }
            catch (Exception ex)
            {
                var message = Helpers.GetFullExceptionMessage(ex, "Could not execute command.", new Dictionary<string, object> {
                    { "logEntryId", logEntryId },
                    { "shellCommand", shellCommand },
                    { "workingDirectory", workingDirectory },
                });
                Helpers.LogException(message, EventLogSource);

                var win32ex = ex as Win32Exception;
                if (null != win32ex)
                {
                    finished = DateTime.UtcNow;
                    exitCode = win32ex.ErrorCode;
                    errorOut += Environment.NewLine + message;
                }
                else
                    throw;
            }

            try
            {
                UpdateLogEntry(logEntryId, started, finished, exitCode, processId, consoleOut, errorOut);
            }
            catch (Exception ex)
            {
                var message = Helpers.GetFullExceptionMessage(ex, "Could not update log entry.", new Dictionary<string, object> {
                    { "logEntryId", logEntryId },
                    { "shellCommand", shellCommand },
                    { "workingDirectory", workingDirectory },
                    { "started", started },
                    { "finished", finished },
                    { "exitCode", exitCode },
                    { "processId", processId },
                    { "consoleOut", consoleOut },
                    { "errorOut", errorOut },
                });
                Helpers.LogException(message, EventLogSource);

                throw;
            }
        }

        void UpdateLogEntry(int logEntryId, DateTime started, DateTime finished, int? exitCode, int? processId, string consoleOut, string errorOut)
        {
            using (var factory = new SchedulerServiceClientFactory())
            {
                WcfHelpers.Using(factory, channel =>
                {
                    channel.UpdateLogEntry(logEntryId, started, finished, exitCode, processId, consoleOut, errorOut);
                });
            }
        }
    }
}
