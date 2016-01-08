using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Scheduler.Web.Models;

namespace Scheduler.Web.Controllers
{
    public class ScheduleEntriesController : ControllerBase<ScheduleEntry>
    {
        // GET: ScheduleEntries
        public override ActionResult Index()
        {
            var schedules = Context.Schedules.Include(s => s.Client);
            return View(schedules.ToList());
        }

        // POST: ScheduleEntries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Create([Bind(Include = "Id,ClientId,CronExpression,ShellCommand,Enabled,Created,CreatedBy,LastUpdated,LastUpdatedBy")] ScheduleEntry scheduleEntry)
        {
            return base.Create(scheduleEntry);
        }

        // POST: ScheduleEntries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Edit([Bind(Include = "Id,ClientId,CronExpression,ShellCommand,Enabled,Created,CreatedBy,LastUpdated,LastUpdatedBy")] ScheduleEntry scheduleEntry)
        {
            return base.Edit(scheduleEntry);
        }

        protected override void AddSelectListsToViewBag(Identifiable entity = default(Identifiable))
        {
            ViewBag.ClientId = entity == default(Identifiable)
                ? new SelectList(Context.Clients, "Id", "NetworkName")
                : new SelectList(Context.Clients, "Id", "NetworkName", ((ScheduleEntry)entity).ClientId)
                ;
        }
    }
}
