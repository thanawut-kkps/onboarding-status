using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingStatus.Data
{
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
