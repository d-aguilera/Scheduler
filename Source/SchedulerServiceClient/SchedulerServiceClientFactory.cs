using Scheduler.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;

namespace Scheduler.SchedulerService.Client
{
    public class SchedulerServiceClientFactory : SchedulerFactory<IScheduler>
    {
        public SchedulerServiceClientFactory() : base(GetEndpointAddress())
        {
        }

        static EndpointAddress GetEndpointAddress()
        {
            var uriString = ConfigurationManager.AppSettings["Scheduler.SchedulerService.Url"];
            var uri = new Uri(uriString, UriKind.Absolute);
            var address = new EndpointAddress(uri, DnsEndpointIdentity.CreateDnsIdentity("localhost"));
            return address;
        }
    }
}
