using Scheduler.DataContracts;
using Scheduler.Web.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Scheduler.Web.Controllers
{
    public abstract class IndexControllerBase<TEntity> : Controller where TEntity : Identifiable
    {
        private WebContext _dbContext = new WebContext();

        protected WebContext Context
        {
            get
            {
                return _dbContext;
            }
        }

        // GET: Entities
        public virtual ActionResult Index()
        {
            var set = Context.Set<TEntity>();
            return View(set.ToList());
        }

        // GET: Entities/Details/5
        public virtual ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var set = Context.Set<TEntity>();
            TEntity entity = set.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}