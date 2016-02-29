using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;

namespace Scheduler.ServiceContracts
{
    public class SchedulerFactory<TChannel> : ChannelFactory<TChannel>
    {
        public SchedulerFactory(EndpointAddress remoteAddress) : base(new SchedulerBinding(), remoteAddress)
        {
            SetClientCertificate();
        }

        void SetClientCertificate()
        {
            var subjectName = ConfigurationManager.AppSettings["Scheduler.Client.CertificateSubjectName"];
            Credentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, subjectName);
            var serialNumber = ConfigurationManager.AppSettings["Scheduler.Service.CertificateSerialNumber"];
            var sc = Credentials.ServiceCertificate;
            sc.SetDefaultCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySerialNumber, serialNumber);
            var auth = sc.Authentication;
            auth.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            auth.RevocationMode = X509RevocationMode.Online;
            auth.TrustedStoreLocation = StoreLocation.LocalMachine;
        }
    }
}
