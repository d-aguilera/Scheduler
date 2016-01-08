using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduler.Web.Models
{
    public class Schedule : Auditable
    {
        public int ClientId
        {
            get;
            set;
        }

        public string CronExpression
        {
            get;
            set;
        }

        public string ShellCommand
        {
            get;
            set;
        }

        public bool Enabled
        {
            get;
            set;
        }
    }
}