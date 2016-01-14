using Scheduler.DataAccess;
using Scheduler.DataContracts;
using Scheduler.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Scheduler.Web.Controllers
{
    public abstract class IndexControllerBase<TEntity> : Controller where TEntity : Identifiable
    {
        protected const string EventLogSource = "Scheduler.Web";

        private WebContext _dbContext = new WebContext();

        protected WebContext Context
        {
            get
            {
                return _dbContext;
            }
        }

        // GET: Entities
        public virtual ActionResult Index(string toastToken)
        {
            if (!string.IsNullOrEmpty(toastToken))
            {
                var toast = Toast.Decrypt(toastToken);
                ViewBag.Toast = toast;
            }
            return View(IndexSet);
        }

        protected virtual IEnumerable<TEntity> IndexSet
        {
            get
            {
                return Context.Set<TEntity>().OrderBy(e => e.Id).ToList();
            }
        }

        // GET: Entities/Details/5
        public virtual ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = FindEntityForDetailsOrDelete(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        protected virtual TEntity FindEntityForDetailsOrDelete(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}