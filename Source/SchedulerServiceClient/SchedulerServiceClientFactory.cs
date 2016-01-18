using Scheduler.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;

namespace Scheduler.SchedulerService.Client
{
    public class SchedulerServiceClientFactory
    {
        public static IScheduler CreateChannel()
        {
            var address = GetEndpointAddress();
            var factory = new SchedulerFactory<IScheduler>(address);
            var channel = factory.CreateChannel();
            return channel;
        }

        static EndpointAddress GetEndpointAddress()
        {
            var uriString = ConfigurationManager.AppSettings["SchedulerServiceAddress"];
            var uri = new Uri(uriString, UriKind.Absolute);
            var address = new EndpointAddress(uri);
            return address;
        }
    }
}
