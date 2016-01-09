using Scheduler.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Scheduler.Web.Controllers
{
    public class LogEntriesController : ControllerBase<LogEntry>
    {
        protected override IEnumerable<LogEntry> IndexSet
        {
            get
            {
                return Context.Set<LogEntry>().Include(l => l.Client).ToList();
            }
        }

        protected override LogEntry FindEntityForDetailsOrDelete(int id)
        {
            return Context.Set<LogEntry>()
                .Include(e => e.Client)
                .SingleOrDefault(e => e.Id == id);
        }

        public override ActionResult Create()
        {
            throw new InvalidOperationException();
        }

        public override ActionResult Create(LogEntry entity)
        {
            throw new InvalidOperationException();
        }

        public override ActionResult Edit(int? id)
        {
            throw new InvalidOperationException();
        }

        public override ActionResult Edit(LogEntry entity)
        {
            throw new InvalidOperationException();
        }
    }
}
