using System.Web;
using System.Web.Optimization;

namespace TestFlight
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // готово к выпуску, используйте средство сборки по адресу https://modernizr.com, чтобы выбрать только необходимые тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootbox.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/dataTables.bootstrap4.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                      "~/Scripts/cartzilla/vendor.min.js",
                      "~/Scripts/cartzilla/theme.min.js",
                      "~/Scripts/cartzilla/myscript.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/cartzilla/vendor.min.css",
                      "~/Content/cartzilla/theme.min.css"));

            bundles.Add(new StyleBundle("~/Content/service").Include(
                "~/Content/bootstrap.css",
                "~/Content/DataTables/css/dataTables.bootstrap4.css",
                "~/Content/Site.css"));
        }
    }
}
