using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingStatus.Services
{
    public class ClientService : IClientService
    {
        public string GetHelloWorld()
        {
            return "Hello World!";
        }
    }

    [ServiceContract]
    public interface IClientService
    {
        [OperationContract]
        string GetHelloWorld();
    }
}
