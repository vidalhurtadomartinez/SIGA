using System.Web;
using System.Web.Optimization;

namespace SIGAA
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                //  "~/Scripts/jquery.unobtrusive-ajax.js",
                    "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/jquery-{version}.js"
                //,"~/Scripts/jquery-ui.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

             bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap-theme.css",
                "~/Content/site.css",
                "~/Content/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/Contentt/css").Include(
                "~/Content/bootstrap-theme - Copia.css",
                "~/Content/site.css",
                "~/Content/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                //"~/Areas/OYM/Scripts/kendo/jquery.min.js",
                //"~/Areas/OYM/Scripts/kendo/kendo.all.min.js", //muestra contenido del content del Grid
                //"~/Areas/OYM/Scripts/kendo/kendo.aspnetmvc.min.js",
                //"~/Areas/OYM/Scripts/kendo.modernizr.custom.js",
                //"~/Areas/OYM/Scripts/kendo/cultures/kendo.culture.es-ES.min.js"
           
                "~/Scripts/kendo/jquery.min.js",
                "~/Scripts/kendo/kendo.all.min.js", //muestra contenido del content del Grid
                "~/Scripts/kendo/kendo.aspnetmvc.min.js",
                "~/Scripts/kendo/jszip.min.js",
                "~/Scripts/kendo/kendo.modernizr.custom.js",
                "~/Scripts/kendo/cultures/kendo.culture.es-ES.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
                //"~/Areas/OYM/Content/kendo/kendo.common-bootstrap.min.css",
                //"~/Areas/OYM/Content/kendo/kendo.bootstrap.min.css",
            
                "~/Content/kendo/kendo.common-bootstrap.min.css",
                "~/Content/kendo/kendo.bootstrap.min.css",
                "~/Content/kendo/kendo.dataviz.min.css",
                "~/Content/kendo/kendo.dataviz.bootstrap.min.css",
                "~/Content/kendo/kendo.mobile.all.min.css"
               ));
        }
    }
}
