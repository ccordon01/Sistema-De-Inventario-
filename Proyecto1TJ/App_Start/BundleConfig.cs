using System.Web;
using System.Web.Optimization;

namespace Proyecto1TJ
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-theme.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/ContentAdmin/css").Include(
                      "~/Content/bootstrap-admin.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/ContentManager/css").Include(
                      "~/Content/bootstrap-bodega.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/ContentEmp/css").Include(
                      "~/Content/bootstrap-emp.css",
                      "~/Content/site.css"));
        }
    }
}
