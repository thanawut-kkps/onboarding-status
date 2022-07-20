using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingStatus_Web.Models {
    public class OnboardingStatus
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

    public class OnboardingProcessItem
    {
        public int seq { get; set; }
        public string step_desc { get; set; }
        public string status_desc { get; set; }
        public string step_date { get; set; }
        public string step_time { get; set; }
        public string remark { get; set; }
        public int lvl { get; set; }

    }
}
