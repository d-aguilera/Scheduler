using Scheduler.DataContracts;
using Scheduler.Web.Models;
using System;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;

namespace Scheduler.Web.Controllers
{
    public abstract class ControllerBase<TEntity> : IndexControllerBase<TEntity> where TEntity : Identifiable
    {
        // GET: ScheduleEntries/Create
        public virtual ActionResult Create()
        {
            AddSelectListsToViewBag();
            return View();
        }

        // POST: Entities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(TEntity entity)
        {
            if (ModelState.IsValid)
            {
                SetDefaultValuesForCreate(entity);
                Context.Set<TEntity>().Add(entity);
                Context.SaveChanges();

                OnCreate(entity);

                var toast = new Toast
                {
                    Title = "Created.",
                    Type = ToastTypes.Success
                };

                return RedirectToAction("Index", new { toastToken = toast.Encrypt() });
            }

            AddSelectListsToViewBag(entity);
            return View(entity);
        }

        protected virtual void OnCreate(TEntity entity)
        {
        }

        // GET: Entities/Edit/5
        public virtual ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = Context.Set<TEntity>().Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            AddSelectListsToViewBag(entity);
            return View(entity);
        }

        // POST: Entities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(TEntity entity)
        {
            if (ModelState.IsValid)
            {
                SetDefaultValuesForEdit(entity);
                Context.Entry(entity).State = EntityState.Modified;
                Context.SaveChanges();

                OnEdit(entity);

                var toast = new Toast
                {
                    Title = "Updated.",
                    Type = ToastTypes.Success
                };

                return RedirectToAction("Index", new { toastToken = toast.Encrypt() });
            }

            AddSelectListsToViewBag(entity);
            return View(entity);
        }

        protected virtual void OnEdit(TEntity entity)
        {
        }

        // GET: Entities/Delete/5
        public virtual ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = Context.Set<TEntity>().Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        // POST: Entities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteConfirmed(int id)
        {
            var set = Context.Set<TEntity>();
            var entity = set.Find(id);
            set.Remove(entity);
            Context.SaveChanges();

            OnDeleteConfirmed(id);

            var toast = new Toast
            {
                Title = "Deleted",
                Type = ToastTypes.Success
            };

            return RedirectToAction("Index", new { toastToken = toast.Encrypt() });
        }

        protected virtual void OnDeleteConfirmed(int id)
        {
        }

        protected virtual void SetDefaultValuesForCreate(Identifiable entity)
        {
            var auditable = entity as Auditable;
            if (null != auditable)
            {
                auditable.Created = DateTime.UtcNow;
                auditable.CreatedBy = User.Identity.Name;
                auditable.LastUpdated = null;
                auditable.LastUpdatedBy = null;
            }
        }

        protected virtual void SetDefaultValuesForEdit(Identifiable entity)
        {
            var auditable = entity as Auditable;
            if (null != auditable)
            {
                auditable.LastUpdated = DateTime.UtcNow;
                auditable.LastUpdatedBy = User.Identity.Name;
            }
        }

        protected virtual void AddSelectListsToViewBag(Identifiable entity = default(Identifiable))
        {
        }
    }
}