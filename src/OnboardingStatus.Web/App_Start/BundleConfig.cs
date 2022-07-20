using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace OnboardingStatus_Web {

    public class BundleConfig {

        public static void RegisterBundles(BundleCollection bundles) {

            var scriptBundle = new ScriptBundle("~/Scripts/bundle");



            // jQuery
            scriptBundle
                .Include("~/Scripts/jquery-{version}.js");

            // Bootstrap
            scriptBundle
                .Include("~/Scripts/bootstrap.js");

            // bootbox
            scriptBundle
                .Include("~/Scripts/bootbox.js");

            // Custom Common
            scriptBundle
                .Include("~/Scripts/Common/dialog.js");

            bundles.Add(scriptBundle);

            // CSS
            bundles.Add(new StyleBundle("~/Content/bundle").Include(
                "~/Content/bootstrap.css",
                "~/Content/body.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/bootstrap-mvc-validation.css",
                "~/Content/bootstrap-datepicker.css",
                "~/Content/bootstrap-datetimepicker.css",
                "~/Content/font-awesome.css",
                "~/Content/paging.css",
                "~/Content/bootstrap-sortable.css",
                "~/Content/animate.css",
                "~/Content/Site.css"
            ));


#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}