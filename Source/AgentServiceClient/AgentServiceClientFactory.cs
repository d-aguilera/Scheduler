using Scheduler.Common;
using Scheduler.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.AgentService.Client
{
    public class AgentServiceClientFactory
    {
        public static IAgent CreateChannel(string networkName, int? port, string virtualDirectory)
        {
            var binding = new SchedulerBinding();
            var uriString = string.Format(
                CultureInfo.InvariantCulture,
                "https://{0}{1}{2}/Scheduler/cert/Agent.svc",
                networkName,
                null == port ? null : (":" + Convert.ToString(port, CultureInfo.InvariantCulture)),
                string.IsNullOrWhiteSpace(virtualDirectory) ? null : ("/" + virtualDirectory.Trim())
                );
            var uri = new Uri(uriString, UriKind.Absolute);
            var address = new EndpointAddress(uri);

            var factory = new ChannelFactory<IAgent>(binding, address);
            var findValue = ConfigurationManager.AppSettings["ClientCertificateSubjectName"];
            factory.Credentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, findValue);

            var channel = factory.CreateChannel(address);
            

            return channel;
        }
    }
}
