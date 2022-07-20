using OnboardingStatus_Web.ClientService;
using OnboardingStatus_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnboardingStatus_Web.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        public ClientController( IClientService clientService)
        {
            _clientService = clientService;
        }


        public ActionResult OnboardingStatus()
        {
            OnboardingStatus viewModel = new OnboardingStatus();
            List<Models.OnboardingProcessItem> list = new List<Models.OnboardingProcessItem>();
            var model = _clientService.GetClientProcess("1111");
           

            if (model != null)
            {
                viewModel.pid = model.pid;
                viewModel.name_th = model.name_th;
                viewModel.name_en = model.name_en;
                viewModel.mobile_phone = model.mobile_phone;
                viewModel.onboarding_status = model.onboarding_status;
                viewModel.email = model.email;
                viewModel.date_of_birth = model.date_of_birth;
                viewModel.account_no = model.account_no;


                if (model.Items != null)
                {
                    list = model.Items
                    .Select(c => new Models.OnboardingProcessItem()
                    {
                        seq = c.seq,
                        step_desc = c.step_desc,
                        status_desc = c.status_desc,
                        step_date = c.step_date,
                        step_time = c.step_time,
                        remark = c.remark,
                        lvl = c.lvl
                    }).ToList();
                    viewModel.Items = list;
                }
            }
            //mock up data
            //model.CardID = "5555555555555";
            //model.SurnameTh = "นายจอห์น โด";
            //model.MobilePhone = "xxxxxxx";
            //model.DateOfBirth = DateTime.Now;
            //model.OnboardStatus = "Not Complete";
            //model.SurnameEng = "Mr.John Doe";
            //model.AccountNo = "200114";

            //List<OnboardingStateDetail> lst = new List<OnboardingStateDetail>();
            //OnboardingStateDetail lstDt = new OnboardingStateDetail();
            //lstDt.StateName = "Customer Profile";
            //lstDt.StateStatus = "";
            //lstDt.StateDateStr = "";
            //lstDt.StateTimeStr = "";
            //lstDt.StateRemark = "";
            //lst.Add(lstDt);
            //lstDt = new OnboardingStateDetail();
            //lstDt.StateName = "Step 1";
            //lstDt.StateStatus = "Not Start / Not Complete / Complete";
            //lstDt.StateDateStr = "DD/MM/YYYY";
            //lstDt.StateTimeStr = "HH:MM:SS";
            //lstDt.StateRemark = "";
            //lst.Add(lstDt);
            //lstDt = new OnboardingStateDetail();
            //lstDt.StateName = "Step 2";
            //lstDt.StateStatus = "Not Start / Not Complete / Complete";
            //lstDt.StateDateStr = "DD/MM/YYYY";
            //lstDt.StateTimeStr = "HH:MM:SS";
            //lstDt.StateRemark = "";
            //lst.Add(lstDt);
            //lstDt = new OnboardingStateDetail();
            //lstDt.StateName = "Step 3";
            //lstDt.StateStatus = "Not Start / Not Complete / Complete";
            //lstDt.StateDateStr = "DD/MM/YYYY";
            //lstDt.StateTimeStr = "HH:MM:SS";
            //lstDt.StateRemark = "";
            //lst.Add(lstDt);

            //model.onboardingStateList = lst;

            return View(viewModel);
        }
    }
}