using System.Web;
using System.Web.Optimization;

namespace titi
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region ======== client ============
            // js
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // css
            bundles.Add(new ScriptBundle("~/bundles/bootstrapClient").Include(
                        "~/Scripts/respond.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap.min.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/extend").Include(
                "~/Scripts/jquery.scrollUp.min.js",
                    "~/Scripts/price-range.js",
                    "~/Scripts/jquery.prettyPhoto.js",
                    "~/Scripts/main.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            #endregion

            #region ======== admin ==========
            // js
            bundles.Add(new ScriptBundle("~/bundles/jqueryAdmin").Include(
                        "~/Areas/Admin/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrapAdmin").Include(
                    "~/Areas/Admin/Scripts/bootstrap.js",
                      "~/Areas/Admin/Scripts/bootstrap.min.js",
                      "~/Areas/Admin/Scripts/respond.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/flotAdmin").Include(
                        "~/Areas/Admin/Scripts/plugins/flot/jquery.flot.js",
                        "~/Areas/Admin/Scripts/plugins/flot/jquery.flot.tooltip.min.js",
                        "~/Areas/Admin/Scripts/plugins/flot/jquery.flot.spline.js",
                        "~/Areas/Admin/Scripts/plugins/flot/jquery.flot.resize.js",
                        "~/Areas/Admin/Scripts/plugins/flot/jquery.flot.pie.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/datepickerAdmin").Include(
                         "~/Areas/Admin/Scripts/plugins/summernote/bootstrap-datetimepicker.min.js",
                         "~/Areas/Admin/Scripts/plugins/sweetalert.min.js"
                       ));
            // SlimScroll
            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                      "~/Areas/Admin/Scripts/jquery.signalR-2.2.1.js"));
            // jQuery Validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryvalAdmin").Include(
            "~/Areas/Admin/Scripts/jquery.validate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizrAdmin").Include(
                        "~/Areas/Admin/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/plugins/metsiMenu").Include(
                      "~/Areas/Admin/Scripts/plugins/metisMenu/jquery.metisMenu.js"
                      ));
            bundles.Add(new ScriptBundle("~/plugins/slimscroll").Include(
                      "~/Areas/Admin/Scripts/plugins/slimscroll/jquery.slimscroll.min.js"
                      ));
            bundles.Add(new ScriptBundle("~/plugins/Notification").Include(
                      "~/Areas/Admin/Scripts/plugins/Notification/toastr.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/inspinia").Include(
                      "~/Areas/Admin/Scripts/app/inspinia.js",
                      "~/Areas/Admin/Scripts/app/common.js",
                      //"~/Scripts/app/chat.js",
                      //"~/Scripts/app/Email.js",
                      "~/Areas/Admin/Scripts/app/Notification.js",
                      "~/Areas/Admin/Scripts/app/controlMenu.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqGrid").Include(
                      "~/Areas/Admin/Scripts/plugins/jqGrid/i18n/grid.locale-en.js",
                      "~/Areas/Admin/Scripts/plugins/jqGrid/jquery.jqGrid.min.js"
                      ));
            //// css
            bundles.Add(new StyleBundle("~/Content/cssAdmin").Include(
                      "~/Areas/Admin/Content/bootstrap.min.css",
                      "~/Areas/Admin/font-awesome/css/font-awesome.css",
                      "~/Areas/Admin/Content/plugins/toastr/toastr.min.css",
                      "~/Areas/Admin/Content/plugins/gritter/jquery.gritter.css",
                      "~/Areas/Admin/Content/animate.css",
                      "~/Areas/Admin/Content/style.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/cssjqgrid").Include(
                      "~/Areas/Admin/Content/plugins/jqGrid/ui.jqgrid.css"
                      ));
            #endregion

            BundleTable.EnableOptimizations = false;
        }
    }
}
