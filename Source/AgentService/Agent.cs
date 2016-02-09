using Scheduler.SchedulerService.Client;
using Scheduler.ServiceContracts;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Permissions;

namespace Scheduler.AgentService
{
    public class Agent : IAgent
    {
        [PrincipalPermission(SecurityAction.Demand, Name = PrincipalNames.SchedulerService)]
        public void Execute(int logEntryId, string shellCommand, string workingDirectory)
        {
            var started = DateTime.UtcNow;
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
                    UpdateLogEntry(logEntryId, process.StartTime, process.ExitTime, process.ExitCode, processId, consoleOut, errorOut);
                };
            }
            catch (Win32Exception ex)
            {
                UpdateLogEntry(logEntryId, started, DateTime.UtcNow, ex.ErrorCode, processId, consoleOut, errorOut + Environment.NewLine + ex.ToString());
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
