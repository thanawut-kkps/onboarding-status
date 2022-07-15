using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace OnboardingStatus_Web {

    public class BundleConfig {

        public static void RegisterBundles(BundleCollection bundles) {

            var scriptBundle = new ScriptBundle("~/Scripts/bundle");
            var styleBundle = new StyleBundle("~/Content/bundle");

            // jQuery
            scriptBundle
                .Include("~/Scripts/jquery-{version}.js");

            // Bootstrap
            scriptBundle
                .Include("~/Scripts/bootstrap.js");

            // Bootstrap
            styleBundle
                .Include("~/Content/bootstrap.css");

            // bootbox
            scriptBundle
                .Include("~/Scripts/bootbox.js");

            // Custom Common
            scriptBundle
                .Include("~/Scripts/Common/dialog.js");

            // Custom site styles
            styleBundle
                .Include("~/Content/Site.css");

            bundles.Add(scriptBundle);
            bundles.Add(styleBundle);

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}