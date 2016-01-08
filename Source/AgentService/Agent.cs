using Scheduler.DataContracts;
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
        public void Execute(string shellCommand, string workingDirectory, int? scheduleEntryId, int clientId, bool forced)
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
                    AddLogEntry(scheduleEntryId, clientId, process.StartTime, process.ExitTime, process.ExitCode, process.Id, consoleOut, errorOut);
                };
            }
            catch (Win32Exception ex)
            {
                AddLogEntry(scheduleEntryId, clientId, started, DateTime.UtcNow, ex.ErrorCode, processId, consoleOut, errorOut + Environment.NewLine + ex.ToString());
            }
        }

        void AddLogEntry(int? scheduleEntryId, int clientId, DateTime started, DateTime finished, int? exitCode, int? processId, string consoleOut, string errorOut)
        {
        }
    }
}
