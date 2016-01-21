using Scheduler.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;

namespace Scheduler.AgentService.Client
{
    public class AgentServiceClientFactory : SchedulerFactory<IAgent>
    {
        public AgentServiceClientFactory(string networkName, int? port, string virtualDirectory)
            : base(GetEndpointAddress(networkName, port, virtualDirectory))
        {
        }

        static EndpointAddress GetEndpointAddress(string networkName, int? port, string virtualDirectory)
        {
            var uriString = string.Format(
                CultureInfo.InvariantCulture,
                "https://{0}{1}{2}/Scheduler/cert/Agent.svc",
                networkName,
                null == port ? null : (":" + Convert.ToString(port, CultureInfo.InvariantCulture)),
                string.IsNullOrWhiteSpace(virtualDirectory) ? null : ("/" + virtualDirectory.Trim())
                );
            var uri = new Uri(uriString, UriKind.Absolute);
            var address = new EndpointAddress(uri);
            return address;
        }
    }
}
