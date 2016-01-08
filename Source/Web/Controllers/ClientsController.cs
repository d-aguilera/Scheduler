using System.Web.Mvc;
using Scheduler.Web.Models;
using Scheduler.DataContracts;

namespace Scheduler.Web.Controllers
{
    public class ClientsController : ControllerBase<Client>
    {
        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Create([Bind(Include = "Id,NetworkName,Description,Created,CreatedBy,LastUpdated,LastUpdatedBy")] Client client)
        {
            return base.Create(client);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Edit([Bind(Include = "Id,NetworkName,Description,Created,CreatedBy,LastUpdated,LastUpdatedBy")] Client client)
        {
            return base.Edit(client);
        }
    }
}
