using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Scheduler.Web.Models
{
    public class LogEntry : Auditable
    {
        public int ClientId
        {
            get;
            set;
        }

        [Display(Name = "LogEntryStartedName", ShortName = "LogEntryStartedShortName", ResourceType = typeof(Resources))]
        public DateTime Started
        {
            get;
            set;
        }

        [Display(Name = "LogEntryFinishedName", ShortName = "LogEntryFinishedShortName", ResourceType = typeof(Resources))]
        public DateTime? Finished
        {
            get;
            set;
        }

        [Display(Name = "LogEntryExitCodeName", ShortName = "LogEntryExitCodeShortName", ResourceType = typeof(Resources))]
        public int? ExitCode
        {
            get;
            set;
        }

        [Display(Name = "LogEntryConsoleOutName", ShortName = "LogEntryConsoleOutShortName", ResourceType = typeof(Resources))]
        public string ConsoleOut
        {
            get;
            set;
        }

        [Display(Name = "LogEntryErrorOutName", ShortName = "LogEntryErrorOutShortName", ResourceType = typeof(Resources))]
        public string ErrorOut
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