using Scheduler.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Scheduler.DataContracts
{
    [DataContract]
    public class LogEntry : Auditable
    {
        [DataMember]
        [Display(Name = "LogEntryShellCommandName", ShortName = "LogEntryShellCommandShortName", ResourceType = typeof(Resources))]
        public string ShellCommand
        {
            get;
            set;
        }

        [DataMember]
        [Display(Name = "LogEntryWorkingDirectoryName", ShortName = "LogEntryWorkingDirectoryShortName", ResourceType = typeof(Resources))]
        public string WorkingDirectory
        {
            get;
            set;
        }

        [DataMember]
        public int ScheduleEntryId
        {
            get;
            set;
        }

        [DataMember]
        public int ClientId
        {
            get;
            set;
        }

        [DataMember]
        [Display(Name = "LogEntryStartedName", ShortName = "LogEntryStartedShortName", ResourceType = typeof(Resources))]
        public DateTime Started
        {
            get;
            set;
        }

        [DataMember]
        [Display(Name = "LogEntryFinishedName", ShortName = "LogEntryFinishedShortName", ResourceType = typeof(Resources))]
        public DateTime? Finished
        {
            get;
            set;
        }

        [DataMember]
        [Display(Name = "LogEntryExitCodeName", ShortName = "LogEntryExitCodeShortName", ResourceType = typeof(Resources))]
        public int? ExitCode
        {
            get;
            set;
        }

        [DataMember]
        [Display(Name = "LogEntryConsoleOutName", ShortName = "LogEntryConsoleOutShortName", ResourceType = typeof(Resources))]
        public string ConsoleOut
        {
            get;
            set;
        }

        [DataMember]
        [Display(Name = "LogEntryErrorOutName", ShortName = "LogEntryErrorOutShortName", ResourceType = typeof(Resources))]
        public string ErrorOut
        {
            get;
            set;
        }

        [ForeignKey("ScheduleEntryId")]
        public ScheduleEntry ScheduleEntry
        {
            get;
            set;
        }

        [ForeignKey("ClientId")]
        public Client Client
        {
            get;
            set;
        }
    }
}