using Scheduler.Common;
using Scheduler.DataContracts;
using Scheduler.SchedulerService.Client;
using Scheduler.ServiceContracts;
using Scheduler.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Scheduler.Web.Controllers
{
    public class ScheduleEntriesController : ControllerBase<ScheduleEntry>
    {
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        const string BindInclude = "Id,ClientId,CronExpression,ShellCommand,WorkingDirectory,Enabled,Created,CreatedBy,LastUpdated,LastUpdatedBy";

        protected override IEnumerable<ScheduleEntry> IndexSet
        {
            get
            {
                return Context.Set<ScheduleEntry>()
                    .Include(s => s.Client)
                    .OrderBy(s => s.Client.NetworkName)
                    .ThenBy(s => s.Id)
                    .ToList();
            }
        }

        protected override ScheduleEntry FindEntityForDetailsOrDelete(int id)
        {
            return Context.Set<ScheduleEntry>()
                .Include(e => e.Client)
                .SingleOrDefault(e => e.Id == id);
        }

        // POST: ScheduleEntries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Create([Bind(Include = BindInclude)] ScheduleEntry scheduleEntry)
        {
            return base.Create(scheduleEntry);
        }

        protected override void OnCreate(ScheduleEntry entity)
        {
            base.OnCreate(entity);
            NotifyCronService();
        }

        // POST: ScheduleEntries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Edit([Bind(Include = BindInclude)] ScheduleEntry scheduleEntry)
        {
            return base.Edit(scheduleEntry);
        }

        protected override void OnEdit(ScheduleEntry entity)
        {
            base.OnEdit(entity);
            NotifyCronService();
        }

        protected override void OnDeleteConfirmed(int id)
        {
            base.OnDeleteConfirmed(id);
            NotifyCronService();
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
            Toast toast;

            try
            {
                using (var factory = new SchedulerServiceClientFactory())
                {
                    WcfHelpers.Using(factory, channel =>
                    {
                        channel.ExecuteInteractive(id, User.Identity.Name);
                    });
                }

                toast = new Toast
                {
                    Title = "Command is executing.",
                    Message = "Check the <a href=\"" + Url.Action("Index", "LogEntries") + "\">Log page</a> for details.",
                    Type = ToastTypes.Success
                };
            }
            catch (Exception ex)
            {
                var message = Helpers.GetFullExceptionMessage(ex, "Could not launch execution.", new Dictionary<string, object> {
                    { "id", id },
                });

                Helpers.LogException(message, EventLogSource);

                toast = new Toast
                {
                    Title = "Could not execute command",
                    Message = ex.Message,
                    Type = ToastTypes.Error
                };
            }

            return RedirectToAction("Index", new { toastToken = toast.Encrypt() });
        }

        protected override void AddSelectListsToViewBag(Identifiable entity = default(Identifiable))
        {
            var set = Context.Set<Client>().ToList();
            ViewBag.ClientId = entity == default(Identifiable)
                ? new SelectList(set, "Id", "NetworkName")
                : new SelectList(set, "Id", "NetworkName", ((ScheduleEntry)entity).ClientId)
                ;
        }

        static void NotifyCronService()
        {
            try
            {
                using (var factory = new SchedulerServiceClientFactory())
                {
                    WcfHelpers.Using(factory, channel =>
                    {
                        channel.Reload();
                    });
                }
            }
            catch (Exception ex)
            {
                var message = Helpers.GetFullExceptionMessage(ex, "Could not contact scheduler service.", new Dictionary<string, object> {
                    { "Operation", "Reload" },
                });

                Helpers.LogWarning(message, EventLogSource);
            }
        }
    }
}
