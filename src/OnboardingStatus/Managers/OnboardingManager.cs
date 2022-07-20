using Newtonsoft.Json;
using OnboardingStatus.Data;
using Phatra.Core.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OnboardingStatus.Managers
{
    public class OnboardingManager : IOnboardingManager
    {
        private ILogger _logger;
        public OnboardingManager()
        {
            _logger = Log4NetLogger.CreateLoggerFor(typeof(OnboardingManager));
        }

        public OnboardingProcessHeader GetClientProcess(string pid)
        {
            OnboardingProcessHeader result = null;
            var onboardingApiUrl = $"http://ptsecmsweb161d1/MockApi/Onboarding/ClientProgress";
            var urlParameters = $"?PID={pid}";


            var httpClientHandler = new HttpClientHandler()
            {
                //Credentials = new NetworkCredential("suktatha", "Thailand_33", "phatrasec"),
                Credentials = CredentialCache.DefaultNetworkCredentials
            };

            using (var client = new HttpClient(httpClientHandler))
            {
 
                client.BaseAddress = new Uri(onboardingApiUrl);
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    result = response.Content.ReadAsAsync<OnboardingProcessHeader>().Result;
                }
                else
                {
                    _logger.Error($"{(int)response.StatusCode} ({response.ReasonPhrase})");
                    throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase})");
                }

                client.Dispose();
                return result;
            }

            //result = JsonConvert.DeserializeObject<OnboardingProcessHeader>(json);
            //return result;
        }
    }

    public interface IOnboardingManager
    {
        OnboardingProcessHeader GetClientProcess(string pid);
    }
}
