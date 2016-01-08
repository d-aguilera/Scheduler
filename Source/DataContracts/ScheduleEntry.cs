using Scheduler.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.DataContracts
{
    [DataContract]
    public class ScheduleEntry : Auditable
    {
        [DataMember]
        public int ClientId
        {
            get;
            set;
        }

        [DataMember]
        [RegularExpression(@"^(\*|\d|[0-5]\d)\s+(\*|\d|[0-1]\d|2[0-3])\s+(\*|0?[1-9]|[1-2]\d|3[0-1])\s+(\*|0?[1-9]|1[0-2])\s+(\*|[0-6])$", ErrorMessageResourceName = "ScheduleEntryCronExpressionRegex", ErrorMessageResourceType = typeof(Resources))]
        [Required]
        [Display(Name = "ScheduleEntryCronExpressionName", ShortName = "ScheduleEntryCronExpressionShortName", ResourceType = typeof(Resources))]
        public string CronExpression
        {
            get;
            set;
        }

        [DataMember]
        [Required]
        [Display(Name = "ScheduleEntryShellCommandName", ShortName = "ScheduleEntryShellCommandShortName", ResourceType = typeof(Resources))]
        public string ShellCommand
        {
            get;
            set;
        }

        [DataMember]
        [Display(Name = "ScheduleEntryEnabledName", ShortName = "ScheduleEntryEnabledShortName", ResourceType = typeof(Resources))]
        public bool Enabled
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
