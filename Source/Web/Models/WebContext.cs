using Scheduler.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Scheduler.Web.Models
{
    public class WebContext : DbContext
    {
        public WebContext() : base("WebContext")
        {
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<ScheduleEntry> ScheduleEntries { get; set; }

        public DbSet<LogEntry> LogEntries { get; set; }
    }
}
