using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Scheduler.ServiceContracts
{
    public class SchedulerFactory<TChannel> : ChannelFactory<TChannel>
    {
        public SchedulerFactory(EndpointAddress remoteAddress) : base(new SchedulerBinding(), remoteAddress)
        {
            var findValue = ConfigurationManager.AppSettings["ClientCertificateSubjectName"];
            Credentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, findValue);
        }
    }
}
