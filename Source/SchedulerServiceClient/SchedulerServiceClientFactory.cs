using Scheduler.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.SchedulerService.Client
{
    public class SchedulerServiceClientFactory
    {
        public static IScheduler CreateChannel()
        {
            var binding = new BasicHttpBinding
            {
                Security = new BasicHttpSecurity
                {
                    Mode = BasicHttpSecurityMode.Transport,
                    Transport = new HttpTransportSecurity
                    {
                        ClientCredentialType = HttpClientCredentialType.Certificate
                    }
                }
            };
            var uri = new Uri(ConfigurationManager.AppSettings["SchedulerServiceAddress"]);
            var address = new EndpointAddress(uri, EndpointIdentity.CreateDnsIdentity("localhost"));
            var factory = new ChannelFactory<IScheduler>(binding, address);
            var findValue = ConfigurationManager.AppSettings["ClientCertificateSubjectName"];
            factory.Credentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, findValue);

            var channel = factory.CreateChannel();

            return channel;
        }
    }
}
