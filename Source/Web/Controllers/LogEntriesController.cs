using Scheduler.DataContracts;
using Scheduler.Web.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Scheduler.Web.Controllers
{
    public class LogEntriesController : IndexControllerBase<LogEntry>
    {
        // GET: LogEntries
        public override ActionResult Index(Toast toast)
        {
            ViewBag.Toast = toast;
            return View(Context.Set<LogEntry>().Include(l => l.Client).ToList());
        }

        // GET: LogEntries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = Context.Set<LogEntry>().Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        // POST: LogEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var set = Context.Set<LogEntry>();
            var entity = set.Find(id);
            set.Remove(entity);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
