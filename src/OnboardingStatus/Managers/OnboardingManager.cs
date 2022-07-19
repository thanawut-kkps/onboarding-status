using Newtonsoft.Json;
using OnboardingStatus.Data;
using Phatra.Core.Logging;
using System;
using System.Collections.Generic;
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
            //var onboardingApiUrl = $"http://ptsecmsapiu1:8087/Onboarding/ClientProgress";
            //var urlParameters = $"?PID={pid}";


            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(onboardingApiUrl);
            //    // Add an Accept header for JSON format.
            //    client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/json"));

            //    HttpResponseMessage response = client.GetAsync(urlParameters).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        // Parse the response body.
            //        result = response.Content.ReadAsAsync<OnboardingProcessHeader>().Result;  
            //    }
            //    else
            //    {
            //        _logger.Error($"{(int)response.StatusCode} ({response.ReasonPhrase})");
            //        throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase})");
            //    }

            //    // Make any other calls using HttpClient here.

            //    // Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            //    client.Dispose();

            //    return result;
            //}

            string json = @"{
        ""pid"": ""11111111"",
        ""onboarding_status"": ""Not Complete"",
        ""name_en"": ""Mr.John Doe"",
        ""name_th"": ""นายจอห์น โด"",
        ""mobile_phone"": ""0813334444"",
        ""email"": ""john.doe@mail.com"",
        ""date_of_birth"": ""27 Mar 1991"",
        ""account_no"": ""200114"",
        ""Items"": 
            [{
                    ""seq"": 1,
                ""step_desc"": ""Customer Profile"",
                ""status_desc"": """",
                ""step_date"": """",
                ""step_time"": """",
                ""remark"": """",
                ""lvl"": 0
            },
            {
                    ""seq"": 2,
                ""step_desc"": ""Step1"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""1 Jan 2022"",
                ""step_time"": ""08:30"",
                ""remark"": """",
                ""lvl"": 1
            },
            {
                    ""seq"": 3,
                ""step_desc"": ""Step2"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""2 Jan 2022"",
                ""step_time"": ""09:30"",
                ""remark"": """",
                ""lvl"": 1
            },
            {
                    ""seq"": 4,
                ""step_desc"": ""Step3"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""3 Jan 2022"",
                ""step_time"": ""10:30"",
                ""remark"": """",
                ""lvl"": 1
            },
            {
                    ""seq"": 5,
                ""step_desc"": ""Investment Profile"",
                ""status_desc"": """",
                ""step_date"": """",
                ""step_time"": """",
                ""remark"": """",
                ""lvl"": 0
            },
            {
                    ""seq"": 6,
                ""step_desc"": ""Step1 Knowledge Assessment"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""4 Jan 2022"",
                ""step_time"": ""11:30"",
                ""remark"": """",
                ""lvl"": 1
            },
            {
                    ""seq"": 7,
                ""step_desc"": ""Step2 Investor Profile Questionaire"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""5 Jan 2022"",
                ""step_time"": ""12:30"",
                ""remark"": """",
                ""lvl"": 1
            },
            {
                    ""seq"": 8,
                ""step_desc"": ""Step3 Risk level result & Suggestion Profolio"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""6 Jan 2022"",
                ""step_time"": ""13:30"",
                ""remark"": """",
                ""lvl"": 1
            },
            {
                    ""seq"": 9,
                ""step_desc"": ""Document Scan"",
                ""status_desc"": """",
                ""step_date"": """",
                ""step_time"": """",
                ""remark"": """",
                ""lvl"": 0
            },
            {
                    ""seq"": 10,
                ""step_desc"": ""ID Card (Front)"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""7 Jan 2022"",
                ""step_time"": ""14:30"",
                ""remark"": """",
                ""lvl"": 1
            },
            {
                    ""seq"": 11,
                ""step_desc"": ""Your Selfie"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""8 Jan 2022"",
                ""step_time"": ""15:30"",
                ""remark"": """",
                ""lvl"": 1
            },
            {
                    ""seq"": 12,
                ""step_desc"": ""Signature"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""9 Jan 2022"",
                ""step_time"": ""15:31"",
                ""remark"": """",
                ""lvl"": 1
            },
            {
                    ""seq"": 13,
                ""step_desc"": ""Terms and Conditions"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""10 Jan 2022"",
                ""step_time"": ""15:32"",
                ""remark"": """",
                ""lvl"": 0
            },
            {
                    ""seq"": 14,
                ""step_desc"": ""Account Opening"",
                ""status_desc"": """",
                ""step_date"": """",
                ""step_time"": """",
                ""remark"": """",
                ""lvl"": 0
            },
            {
                    ""seq"": 15,
                ""step_desc"": ""Status"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""11 Jan 2022"",
                ""step_time"": ""15:33"",
                ""remark"": """",
                ""lvl"": 1
            },
            {
                    ""seq"": 16,
                ""step_desc"": ""KKPSS Account"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""12 Jan 2022"",
                ""step_time"": ""15:34"",
                ""remark"": """",
                ""lvl"": 1
            },
            {
                    ""seq"": 17,
                ""step_desc"": ""Investment Account"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""13 Jan 2022"",
                ""step_time"": ""15:35"",
                ""remark"": ""TestRemark"",
                ""lvl"": 1
            },
            {
                    ""seq"": 18,
                ""step_desc"": ""Investment username"",
                ""status_desc"": ""Complete"",
                ""step_date"": ""14 Jan 2022"",
                ""step_time"": ""15:36"",
                ""remark"": """",
                ""lvl"": 1
            }]
        }";

            result = JsonConvert.DeserializeObject<OnboardingProcessHeader>(json);
            return result;
        }
    }

    public interface IOnboardingManager
    {
        OnboardingProcessHeader GetClientProcess(string pid);
    }
}
