using Scheduler.DataContracts;
using Scheduler.SchedulerService.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Scheduler.AgentService
{
    public class Agent : IAgent
    {
        public void Execute(string shellCommand, string workingDirectory, int scheduleEntryId, int clientId, bool forced)
        {
            DateTime started = DateTime.UtcNow;
            int? processId = null;
            string consoleOut = null;
            string errorOut = null;

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
                    process.OutputDataReceived += (sender, args) => consoleOut = args.Data;
                    process.ErrorDataReceived += (sender, args) => errorOut = args.Data;
                    process.Start();
                    processId = process.Id;
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                    AddLogEntry(shellCommand, workingDirectory, scheduleEntryId, clientId, process.StartTime, process.ExitTime, process.ExitCode, process.Id, consoleOut, errorOut);
                };
            }
            catch (Win32Exception ex)
            {
                AddLogEntry(shellCommand, workingDirectory, scheduleEntryId, clientId, started, DateTime.UtcNow, ex.ErrorCode, processId, consoleOut, errorOut + Environment.NewLine + ex.ToString());
            }
        }

        void AddLogEntry(string shellCommand, string workingDirectory, int scheduleEntryId, int clientId, DateTime started, DateTime finished, int? exitCode, int? processId, string consoleOut, string errorOut)
        {
            var sc = ServiceSecurityContext.Current;
            var createdBy = null == sc ? "?" : sc.PrimaryIdentity.Name;

            var logEntry = new LogEntry
            {
                ShellCommand = shellCommand,
                WorkingDirectory = workingDirectory,
                ScheduleEntryId = scheduleEntryId,
                ClientId = clientId,
                Started = started,
                Finished = finished,
                ExitCode = exitCode,
                ConsoleOut = consoleOut,
                ErrorOut = errorOut,
                Created = DateTime.UtcNow,
                CreatedBy = createdBy,
                LastUpdated = null,
                LastUpdatedBy = null,
            };
            var channel = SchedulerServiceClientFactory.CreateChannel();
            channel.LogExecution(logEntry);
        }
    }
}
