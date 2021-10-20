using System.Web;
using System.Web.Optimization;

namespace LoomCrm_Hospital
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-3.1.1.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery-ui.min.js",
                        "~/Scripts/moment.min.js",
                        "~/Scripts/fullcalendar.js",
                        "~/Scripts/tr.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/respond.js",
                        "~/Scripts/app.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/stiller").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/AdminLTE.css",
                      "~/Content/skins/allskins.css",
                      "~/Content/skins/skinblue.css",
                      "~/Content/jqueryui.min.css",
                      "~/Content/iCheck/flat/blue.css",
                      "~/Content/fullcalendar.css"
                      ));
        }
    }
}
