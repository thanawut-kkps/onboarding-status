using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingStatus.Data
{
    public class OnboardingProcessHeader
    {
        public string pid { get; set; }
        public string onboarding_status { get; set; }
        public string name_en { get; set; }
        public string name_th { get; set; }
        public string mobile_phone { get; set; }
        public string email { get; set; }
        public string date_of_birth { get; set; }
        public string account_no { get; set; }
        public List<OnboardingProcessItem> Items { get; set; }
    }
}
