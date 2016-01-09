using System.Web.Mvc;
using Scheduler.Web.Models;
using Scheduler.DataContracts;

namespace Scheduler.Web.Controllers
{
    public class ClientsController : ControllerBase<Client>
    {
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        const string BindInclude = "Id,NetworkName,AgentPort,AgentVirtualDirectory,Description,Created,CreatedBy,LastUpdated,LastUpdatedBy";

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Create([Bind(Include = BindInclude)] Client client)
        {
            return base.Create(client);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Edit([Bind(Include = BindInclude)] Client client)
        {
            return base.Edit(client);
        }
    }
}
