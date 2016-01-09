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
            factory.Credentials.UserName.UserName = Helpers.FromBase64(ConfigurationManager.AppSettings["UserName"]);
            factory.Credentials.UserName.Password = Helpers.FromBase64(ConfigurationManager.AppSettings["Password"]);

            var channel = factory.CreateChannel();
            return channel;
        }
    }
}
