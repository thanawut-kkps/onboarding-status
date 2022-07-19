using OnboardingStatus.Data;
using OnboardingStatus.Managers;
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
        private readonly IOnboardingManager _onboardingManager;

        public ClientService()
        {
            _onboardingManager = new OnboardingManager();
        }

        public OnboardingProcessHeader GetClientProcess(string pid)
        {
            return _onboardingManager.GetClientProcess(pid);
        }

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

        [OperationContract]
        OnboardingProcessHeader GetClientProcess(string pid);
    }
}
