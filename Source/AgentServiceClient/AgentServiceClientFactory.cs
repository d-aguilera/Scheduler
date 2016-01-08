using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.AgentService.Client
{
    public class AgentServiceClientFactory
    {
        public static IAgent CreateChannel()
        {
            var factory = new ChannelFactory<IAgent>("AgentService");
            var channel = factory.CreateChannel();
            return channel;
        }
    }
}
