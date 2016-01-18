using Scheduler.DataContracts;
using Scheduler.SchedulerService.Client;
using Scheduler.ServiceContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace Scheduler.AgentService
{
    public class Agent : IAgent
    {
        public static void Configure(ServiceConfiguration config)
        {
            var binding = new SchedulerBinding();
            var address = new Uri("cert/Agent.svc", UriKind.Relative);
            config.AddServiceEndpoint(typeof(IAgent), binding, address);
            config.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpsGetEnabled = true });
            config.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });
            config.Description.Behaviors.Add(new ServiceAuthorizationBehavior { PrincipalPermissionMode = PrincipalPermissionMode.UseAspNetRoles });
        }

        [PrincipalPermission(SecurityAction.Demand, Name = "CN=Scheduler Service; 0934C34632B2A5BF21E21EAD970992B63FAA4F3C")]
        public void Execute(int logEntryId, string shellCommand, string workingDirectory)
        {
            DateTime started = DateTime.UtcNow;
            int? processId = null;
            string consoleOut = string.Empty;
            string errorOut = string.Empty;

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
            var channel = SchedulerServiceClientFactory.CreateChannel();
            channel.UpdateLogEntry(logEntryId, started, finished, exitCode, processId, consoleOut, errorOut);
        }
    }
}
