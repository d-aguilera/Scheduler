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
        #region CronExpression Regex

        const string MinuteValueExpr = @"(\*|\d|[0-5]\d)";
        const string HourValueExpr = @"(\*|\d|[0-1]\d|2[0-3])";
        const string DayValueExpr = @"(\*|0?[1-9]|[1-2]\d|3[0-1])";
        const string MonthValueExpr = @"(\*|0?[1-9]|1[0-2])";
        const string WeekdayValueExpr = @"(\*|[0-6])";

        const string MinuteRangeExpr = MinuteValueExpr + "(-" + MinuteValueExpr + ")?";
        const string HourRangeExpr = HourValueExpr + "(-" + HourValueExpr + ")?";
        const string DayRangeExpr = DayValueExpr + "(-" + DayValueExpr + ")?";
        const string MonthRangeExpr = MonthValueExpr + "(-" + MonthValueExpr + ")?";
        const string WeekdayRangeExpr = WeekdayValueExpr + "(-" + WeekdayValueExpr + ")?";

        const string MinuteListExpr = MinuteRangeExpr + "(," + MinuteRangeExpr + ")*";
        const string HourListExpr = HourRangeExpr + "(," + HourRangeExpr + ")*";
        const string DayListExpr = DayRangeExpr + "(," + DayRangeExpr + ")*";
        const string MonthListExpr = MonthRangeExpr + "(," + MonthRangeExpr + ")*";
        const string WeekdayListExpr = WeekdayRangeExpr + "(," + WeekdayRangeExpr + ")*";

        const string Start = "^";
        const string Space = @"\s+";
        const string End = "$";

        const string CronExpressionRegex = Start + MinuteListExpr + Space + HourListExpr + Space + DayListExpr + Space + MonthListExpr + Space + WeekdayListExpr + End;

        #endregion

        [DataMember]
        public int ClientId
        {
            get;
            set;
        }

        [DataMember]
        [RegularExpression(CronExpressionRegex, ErrorMessageResourceName = "ScheduleEntryCronExpressionRegex", ErrorMessageResourceType = typeof(Resources))]
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
        [Display(Name = "ScheduleEntryWorkingDirectoryName", ShortName = "ScheduleEntryWorkingDirectoryShortName", ResourceType = typeof(Resources))]
        public string WorkingDirectory
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
