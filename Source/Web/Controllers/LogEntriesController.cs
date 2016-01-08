using Scheduler.DataContracts;
using Scheduler.Web.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Scheduler.Web.Controllers
{
    public class LogEntriesController : IndexControllerBase<LogEntry>
    {
        // GET: LogEntries
        public override ActionResult Index()
        {
            var logEntries = Context.LogEntries.Include(l => l.Client);
            return View(logEntries.ToList());
        }
    }
}
