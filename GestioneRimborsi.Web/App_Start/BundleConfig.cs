using System.Web;
using System.Web.Optimization;

namespace GestioneRimborsi.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // SCRIPTs
            bundles.Add(new ScriptBundle("~/bundles/standard").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/jQuery.tmpl.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/moment-js").Include(
                        "~/Scripts/moment-with-locales.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // PLUGIN
            bundles.Add(new ScriptBundle("~/bundles/plugin").Include(
                        "~/plugin/pnotify/pnotify.custom.min.js",
                        "~/plugin/datepicker/bootstrap-datepicker.js",
                        "~/plugin/datetimepicker/collapse.js",
                        "~/plugin/datetimepicker/transition.js",
                        "~/plugin/datetimepicker/moment.js",
                        //"~/plugin/datetimepicker/bootstrap-datetimepicker.min.js",
                        //"~/plugin/datetimepicker/bootstrap-datetimepicker.js",                        
                        "~/plugin/datetimepicker/datetimepicker.js",
                        "~/plugin/DataTables/media/js/jquery.dataTables.js",
                        "~/plugin/DataTables/media/js/dataTables.bootstrap.js",
                        "~/plugin/magic-suggest-2.0.0/magicsuggest.js",
                        "~/plugin/DropZone/dropzone.js"));

            // CUSTOM
            bundles.Add(new ScriptBundle("~/bundles/gruppocap-custom").Include(
                      "~/Scripts/datatables-setup.js",
                      "~/Scripts/cap-common.js",
                      "~/Scripts/cap-validation.js",
                      "~/Scripts/BonusIdrico/Validate.js"));


            // STYLESHEET
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"));

            // PLUGIN 
            bundles.Add(new StyleBundle("~/Content/plugin-css").Include(
                      "~/plugin/font-awesome-4.5.0/css/font-awesome.css",
                      "~/plugin/DataTables/media/css/dataTables.bootstrap.css",
                      "~/plugin/datepicker/datepicker.css",
                      //"~/plugin/datetimepicker/bootstrap-datetimepicker.min.css",
                      //"~/plugin/datetimepicker/bootstrap-datetimepicker.css",
                      "~/plugin/datetimepicker/datetimepicker.css",
                      "~/plugin/pnotify/pnotify.custom.min.css",
                      "~/plugin/magic-suggest-2.0.0/magicsuggest.css",
                      "~/plugin/DropZone/dropzone.css"));

            // Custom Styles
            bundles.Add(new StyleBundle("~/Content/custom-css").Include(
                      "~/Content/gestionerimborsi.css",
                      "~/Content/BonusIdrico/BonusIdrico.css"));


        }
    }
}
