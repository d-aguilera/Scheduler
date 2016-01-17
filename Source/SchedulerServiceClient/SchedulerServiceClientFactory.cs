using Scheduler.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.SchedulerService.Client
{
    public class SchedulerServiceClientFactory
    {
        public static IScheduler CreateChannel()
        {
            var factory = new ChannelFactory<IScheduler>("SchedulerService");
            var channel = factory.CreateChannel();
            return channel;
        }
    }
}
