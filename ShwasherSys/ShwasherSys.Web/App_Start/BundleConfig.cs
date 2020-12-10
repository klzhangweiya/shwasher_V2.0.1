using System.Web.Optimization;

namespace ShwasherSys
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            BundleTable.EnableOptimizations = false;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/Scripts/Jquery/jquery-{version}.js",
                        "~/Content/Plugins/bootstrap-3.3.7/js/bootstrap.min.js",
                        "~/Content/Plugins/jquery.validate/jquery.validate.min.js",
                        "~/Content/Plugins/jquery.validate/localization/messages_zh.js",
                        "~/Content/Scripts/Abp/abp.js"
                       ));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/Scripts/Jquery/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Css/normalize.css",
                "~/Content/Plugins/bootstrap-3.3.7/css/bootstrap.min.css",
                "~/Content/Icon/iconfont.css"));

            bundles.Add(new ScriptBundle("~/bundles/abp").Include(
                    "~/Content/Plugins/sweetalert2/sweetalert2.min.js",
                    "~/Content/Plugins/sweetalert2/promise/promise.min.js",
                    "~/Content/Plugins/spin.js/spin.min.js",
                    "~/Content/Plugins/spin.js/jquery.spin.js",
                    "~/Content/Plugins/blockUI/jquery.blockUI.js",
                    //"~/Content/Plugins/toastr/toastr.min.js",
                    //"~/Content/Scripts/Abp/abp.js",
                    "~/Content/Scripts/Abp/libs/abp.jquery.js",
                    // "~/Content/Scripts/Abp/libs/abp.toastr.js",
                    "~/Content/Scripts/Abp/libs/abp.blockUI.js",
                    "~/Content/Scripts/Abp/libs/abp.spin.js",
                    "~/Content/Scripts/Abp/libs/abp.sweet-alert.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                    "~/Content/Plugins/AdminLTE-2.4.3/js/adminlte.min.js",
                    "~/Content/Plugins/bootstrap-table/bootstrap-table.js",
                    "~/Content/Plugins/bootstrap-table/bootstrap-table-mobile.min.js",
                    "~/Content/Plugins/bootstrap-table/extensions/page-jumpto/bootstrap-table-jumpto.js",
                    "~/Content/Plugins/bootstrap-table/locale/bootstrap-table-zh-CN.js",
                    "~/Content/Plugins/bootstrap-table/locale/bootstrap-table-en-US.js",
                    "~/Content/Plugins/select2/js/select2.min.js",
                    "~/Content/Plugins/icheck/icheck.min.js",
                    "~/Content/Plugins/Waves/dist/waves.js",
                    "~/Content/Plugins/viewer/viewer.min.js",
                    "~/Content/Plugins/layui/layui.js", "~/Content/Plugins/scroll/js/scrollBar.js"
                    ));

            bundles.Add(new StyleBundle("~/Content/plugins").Include(
                    "~/Content/Plugins/bootstrap-table/bootstrap-table.min.css",
                    "~/Content/Plugins/bootstrap-table/extensions/page-jumpto/bootstrap-table-jumpto.css",
                    "~/Content/Plugins/AdminLTE-2.4.3/css/adminlte.min.css",
                    "~/Content/Plugins/AdminLTE-2.4.3/css/skins/_all-skins.min.css",
                    "~/Content/Plugins/AdminLTE-2.4.3/css/skins/skin-self-wr.css",
                    "~/Content/Plugins/famfamfam-flags/famfamfam-flags.min.css",
                    "~/Content/Plugins/select2/css/select2.min.css",
                    "~/Content/Plugins/sweetalert2/sweetalert2.min.css",
                    "~/Content/Plugins/icheck/skins/all.css",
                    "~/Content/Css/materialize.css",
                    "~/Content/Plugins/Waves/dist/waves.css",
                    "~/Content/Plugins/viewer/viewer.min.css", "~/Content/Plugins/scroll/css/scrollBar.css"
            ));
            bundles.Add(new StyleBundle("~/Content/plugins-selfCss").Include(
                //"~/Content/Plugins/bootstrap-table/bootstrap-table-wr.css",
                "~/Content/Plugins/select2/css/select2-wr.css",
                "~/Content/Plugins/sweetalert2/sweetalert2-wr.css"
            ));
            bundles.Add(new StyleBundle("~/Content/Util").Include(
                    "~/Content/Css/Site.css"
                    ));
            bundles.Add(new ScriptBundle("~/bundles/Util").Include(
                    "~/Content/Scripts/MyScript/iwb.js",
                    "~/Content/Scripts/MyScript/UtilJs.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/Util_New").Include(
                    "~/Content/Scripts/MyScript/iwb.js",
                    "~/Content/Scripts/MyScript/util.js"
                    ));

            bundles.Add(
                new StyleBundle("~/Bundles/Account/css")
                    .Include("~/Content/Css/normalize.css",
                        "~/Content/Icon/iconfont.css",
                        "~/Content/Plugins/sweetalert2/sweetalert2.min.css",
                        "~/Content/Plugins/sweetalert2/sweetalert2-wr.css",
                        "~/Content/Css/Login.css")
            );
            bundles.Add(
                new ScriptBundle("~/Bundles/Account/js")
                    //.Include("~/Content/Plugins/jquery.validate/jquery.validate.min.js")
                    //.Include("~/Content/Plugins/jquery.validate/localization/messages_zh.js")
                    .Include("~/Content/Plugins/sweetalert2/sweetalert2.min.js")
                    .Include("~/Content/Plugins/sweetalert2/promise/promise.min.js")
                    .Include("~/Content/Plugins/spin.js/spin.min.js")
                    .Include("~/Content/Plugins/spin.js/jquery.spin.js")
                    .Include("~/Content/Plugins/blockUI/jquery.blockUI.js")
                    .Include("~/Content/Plugins/Waves/dist/waves.js")
                    //.Include("~/Content/Scripts/Abp/abp.js")
                    .Include("~/Content/Scripts/Abp/libs/abp.jquery.js")
                    .Include("~/Content/Scripts/Abp/libs/abp.blockUI.js")
                    .Include("~/Content/Scripts/Abp/libs/abp.spin.js")
                    .Include("~/Content/Scripts/Abp/libs/abp.sweet-alert.js")
            );
            bundles.Add(
                new StyleBundle("~/Content/datetimepicker/css")
                    .Include(
                        "~/Content/Plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css",
                        "~/Content/Plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker-wr.css"
                        )
            );
            bundles.Add(
                new ScriptBundle("~/Bundles/datetimepicker/js")
                    .Include("~/Content/Plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js")
                    .Include("~/Content/Plugins/bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.zh-CN.js")
            );
        }
    }
}