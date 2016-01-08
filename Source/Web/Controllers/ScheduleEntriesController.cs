using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Scheduler.Web.Models;
using Scheduler.DataContracts;
using System.Net;
using Scheduler.AgentService.Client;

namespace Scheduler.Web.Controllers
{
    public class ScheduleEntriesController : ControllerBase<ScheduleEntry>
    {
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        const string BindInclude = "Id,ClientId,CronExpression,ShellCommand,WorkingDirectory,Enabled,Created,CreatedBy,LastUpdated,LastUpdatedBy";

        // GET: ScheduleEntries
        public override ActionResult Index(string message = "")
        {
            var schedules = Context.ScheduleEntries.Include(s => s.Client);
            ViewBag.Message = message;
            return View(schedules.ToList());
        }

        // POST: ScheduleEntries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Create([Bind(Include = BindInclude)] ScheduleEntry scheduleEntry)
        {
            return base.Create(scheduleEntry);
        }

        // POST: ScheduleEntries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Edit([Bind(Include = BindInclude)] ScheduleEntry scheduleEntry)
        {
            return base.Edit(scheduleEntry);
        }

        // GET: Entities/Execute/5
        public ActionResult Execute(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = Context.ScheduleEntries.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        // POST: Entities/Execute/5
        [HttpPost, ActionName("Execute")]
        [ValidateAntiForgeryToken]
        public ActionResult ExecuteConfirmed(int id)
        {
            var set = Context.ScheduleEntries;
            var scheduleEntry = set.Find(id);
            var agentServiceClient = AgentServiceClientFactory.CreateChannel();
            agentServiceClient.Execute(
                scheduleEntry.ShellCommand, 
                scheduleEntry.WorkingDirectory, 
                scheduleEntry.Id, 
                scheduleEntry.ClientId, 
                true
                );
            return RedirectToAction("Index", new { message = "Launched!" });
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
