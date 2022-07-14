using OnboardingStatus.Services;
using Phatra.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingStatus.HostConsole
{
    class Program
    {
        private static readonly ILogger _logger = Log4NetLogger.CreateLoggerFor(typeof(Program));
        private static BasicHttpBinding _serverbinding = null;
        private static string _domainNamePort = "80";
        private static string _hostName = "localhost";
        private static string _serviceName = "";
        private static List<ServiceHost> _hostList;


        static void Main(string[] args)
        {
            _domainNamePort = System.Configuration.ConfigurationManager.AppSettings["DomainNamePort"];
            _serviceName = System.Configuration.ConfigurationManager.AppSettings["ServiceName"];

            _logger.Info($"{_serviceName} is starting ...");
            _logger.Info($"Service is hosted at MachineName: {Environment.MachineName}");


            _serverbinding = new BasicHttpBinding();
            _serverbinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            _serverbinding.OpenTimeout = new TimeSpan(0, 10, 0);
            _serverbinding.CloseTimeout = new TimeSpan(0, 10, 0);
            _serverbinding.SendTimeout = new TimeSpan(0, 10, 0);
            _serverbinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            _serverbinding.MaxReceivedMessageSize = int.MaxValue;
            _serverbinding.MaxBufferSize = int.MaxValue;
            _serverbinding.MaxBufferPoolSize = int.MaxValue;

            try
            {
                _hostList = new List<ServiceHost>
                {
                    RegisterServiceHost<IClientService>(typeof(ClientService), typeof(IClientService), "Client"),

                };

                foreach (var host in _hostList)
                {
                    host.Open();
                    _logger.Info($"http://{_hostName}:{_domainNamePort}/{_serviceName}/{host.Description.Name} Running...");
                }
                _logger.Debug("The host been Opened.");

                _logger.Info($"{_serviceName} is already started ...");


            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }

            Console.Read();
        }

        private static ServiceHost RegisterServiceHost<TInterface>(Type Service, Type IService, string ServiceName)
        {
            var stringAddress = $"http://{_hostName}:{_domainNamePort}/{_serviceName}/{ServiceName}";
            var serviceAddress = new Uri(stringAddress);
            var serviceMetaBehavior = new ServiceMetadataBehavior() { HttpGetUrl = serviceAddress, HttpGetEnabled = true };

            var service = new ServiceHost(Service, serviceAddress);
            service.AddServiceEndpoint(IService, _serverbinding, serviceAddress);
            //service.AddDependencyInjectionBehavior<TInterface>(_container);
            service.Description.Behaviors.Add(serviceMetaBehavior);
            service.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            service.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });
            service.Description.Name = ServiceName;
            var throting = new ServiceThrottlingBehavior()
            {
                MaxConcurrentCalls = 1000,
                MaxConcurrentInstances = 2000,
                MaxConcurrentSessions = 1000
            };
            service.Description.Behaviors.Remove(typeof(ServiceThrottlingBehavior));
            service.Description.Behaviors.Add(throting);

            return service;
        }
    }
}
