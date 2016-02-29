using System;
using System.ServiceModel;

namespace Scheduler.ServiceContracts
{
    public class SchedulerBinding : BasicHttpBinding
    {
        public SchedulerBinding() : base()
        {
            Security = new BasicHttpSecurity
            {
                Mode = BasicHttpSecurityMode.Message,
                Message =
                {
                    ClientCredentialType = BasicHttpMessageCredentialType.Certificate
                }
            };
        }
    }
}
