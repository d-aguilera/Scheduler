using Scheduler.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.AgentService.Client
{
    public class AgentServiceClientFactory
    {
        public static IAgent CreateChannel(string networkName, int? port, string virtualDirectory)
        {
            var factory = new ChannelFactory<IAgent>("AgentService");

            var uriString = string.Format(
                CultureInfo.InvariantCulture,
                "https://{0}{1}{2}/Scheduler/cert/Agent.svc",
                networkName,
                null == port ? null : (":" + Convert.ToString(port, CultureInfo.InvariantCulture)),
                string.IsNullOrWhiteSpace(virtualDirectory) ? null : ("/" + virtualDirectory.Trim())
                );
            var uri = new Uri(uriString, UriKind.Absolute);
            var address = new EndpointAddress(uri);

            var channel = factory.CreateChannel(address);
            

            return channel;
        }
    }
}
