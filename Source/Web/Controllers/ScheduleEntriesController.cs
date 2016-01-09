using Newtonsoft.Json;
using Scheduler.Common;
using Scheduler.DataContracts;
using Scheduler.SchedulerService.Client;
using Scheduler.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace Scheduler.Web.Controllers
{
    public class ScheduleEntriesController : ControllerBase<ScheduleEntry>
    {
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        const string BindInclude = "Id,ClientId,CronExpression,ShellCommand,WorkingDirectory,Enabled,Created,CreatedBy,LastUpdated,LastUpdatedBy";

        // GET: ScheduleEntries
        public override ActionResult Index(Toast toast)
        {
            ViewBag.Toast = toast;
            return View(Context.Set<ScheduleEntry>().Include(s => s.Client).ToList());
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
            var entity = Context.Set<ScheduleEntry>().Find(id);
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
            var scheduleEntry = Context.Set<ScheduleEntry>().Find(id);

            var schedulerServiceClient = SchedulerServiceClientFactory.CreateChannel();

            try
            {
                schedulerServiceClient.Execute(scheduleEntry, true);

                var toast = new Toast
                {
                    Title = "Command is executing.",
                    Message = "Check the <a href=\"" + Url.Action("Index", "LogEntries") + "\">Log page</a> for details.",
                    Type = ToastTypes.Success
                };

                return RedirectToAction("Index", new { toast = toast });
            }
            catch (Exception ex)
            {
                var source = "Scheduler";
                var args = new Dictionary<string, object> {
                    { "id", id },
                    { "ScheduleEntry", scheduleEntry }
                };
                var message = Helpers.GetFullExceptionMessage("Could not launch execution.", ex, args);

                EventLog.WriteEntry(source, message, EventLogEntryType.Error);

                var toast = new Toast
                {
                    Title = "Could not execute command",
                    Message = ex.Message,
                    Type = ToastTypes.Error
                };

                return RedirectToAction("Index", new { toast = toast });
            }
        }

        protected override void AddSelectListsToViewBag(Identifiable entity = default(Identifiable))
        {
            var set = Context.Set<Client>().ToList();
            ViewBag.ClientId = entity == default(Identifiable)
                ? new SelectList(set, "Id", "NetworkName")
                : new SelectList(set, "Id", "NetworkName", ((ScheduleEntry)entity).ClientId)
                ;
        }
    }
}
