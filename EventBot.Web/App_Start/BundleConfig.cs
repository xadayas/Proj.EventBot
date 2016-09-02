using System.Web;
using System.Web.Optimization;

namespace EventBot.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/underscore-min.js",
                        //"~/Scrips/moment.js",
                        "~/Scripts/anypicker.js"));


            //Bootstrap-datetimepicker and moment.js

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                "~/Scripts/moment.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/moment-with-locales.js",
                "~/Scripts/moment-with-locales.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datetimepicker").Include(
                "~/Scripts/botstrap-datetimepicker.js",
                "~/Scripts/bootstrap-datetimepicker.min.js"));
            //bundles.Add(new StyleBundle("~/Content/bootstrap-datetimepicker").Include(
            //    "~/Content/bootstrap-datetimepicker.css",
            //    "~/Content/bootstrap-datetimepicker.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-select.js",
                      "~/Scripts/bootstrap-tagsinput.js",
                      "~/Scripts/bootstrap3-typeahead.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/animate.css",
                "~/Content/eventbot.css",
                "~/Content/anypicker.css",
                "~/Content/bootstrap-select.css",
                "~/Content/bootstrap-tagsinput.css",
                "~/Content/bootstrap-datetimepicker.css",
                "~/Content/bootstrap-datetimepicker.min.css"));
        }
    }
}
