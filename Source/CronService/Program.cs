using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.CronService
{
    static class Program
    {
        static void Main()
        {
            var service = new Service();
            if (Environment.UserInteractive)
            {
                Console.WriteLine("Starting {0}...", service.ServiceName);

                var onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
                onStartMethod.Invoke(service, new object[] { new string[0] });

                Console.WriteLine("Service is running. Press any key to stop the service.");
                Console.ReadKey();
                Console.WriteLine();

                var onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
                onStopMethod.Invoke(service, null);

                Console.WriteLine("Service stopped.");
            }
            else
            {
                ServiceBase.Run(service);
            }
        }
    }
}
