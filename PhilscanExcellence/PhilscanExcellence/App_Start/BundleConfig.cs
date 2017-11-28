using System.Web;
using System.Web.Optimization;

namespace PhilscanExcellence
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap/css/bootstrap.min.css",
                    "~/Content/metisMenu/metisMenu.min.css",
                    "~/Content/dist/css/sb-admin-2.css",
                    "~/Content/font-awesome/css/font-awesome.min.css",
                    "~/Content/angular-growl.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Script").Include(
                    "~/Content/jquery/jquery.min.js",
                    "~/Content/bootstrap/js/bootstrap.min.js",
                    "~/Content/metisMenu/metisMenu.min.js",
                    "~/Content/dist/js/sb-admin-2.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                    "~/Scripts/angular.js",
                    "~/Scripts/angular-growl.min.js",
                    "~/Scripts/select2.full.min.js",

                    "~/App/App.js",
                    "~/App/Controller/Login.js"
                ));
        }
    }
}
