using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Scheduler.ServiceContracts
{
    public class SchedulerServiceBase<TChannel>
    {
        public static void Configure(ServiceConfiguration config)
        {
            var baseUrl = ConfigurationManager.AppSettings["Scheduler.Service.BaseUrl"];
            var contract = ContractDescription.GetContract(typeof(TChannel));
            var binding = new SchedulerBinding();
            var agentUri = new Uri(baseUrl + "/cert/" + typeof(TChannel).Name.Substring(1) + ".svc");
            var mexUri = new Uri(baseUrl + "/mex");
            var address = new EndpointAddress(agentUri, EndpointIdentity.CreateDnsIdentity("localhost"));
            var endpoint = new ServiceEndpoint(contract, binding, address);
            var sn = ConfigurationManager.AppSettings["Scheduler.Service.CertificateSerialNumber"];
            config.AddServiceEndpoint(endpoint);
            config.Credentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySerialNumber, sn);
            config.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.UseAspNetRoles;
            config.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true, HttpGetUrl = mexUri });
            config.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });
        }
    }
}
