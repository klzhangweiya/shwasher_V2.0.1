using Abp.Dependency;
using Abp.Web.Mvc.Views;

using IwbZero.Session;
using IwbZero.Setting;

using ShwasherSys.Authorization.Permissions;

namespace ShwasherSys.Views
{
    public abstract class IwbWebViewPageBase : IwbWebViewPageBase<dynamic>
    {
    }

    public abstract class IwbWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected IwbWebViewPageBase()
        {
            LocalizationSourceName = ShwasherConsts.LocalizationSourceName;
            SettingManager = SingletonDependency<IIwbSettingManager>.Instance;
            AbpSession = SingletonDependency<IIwbSession>.Instance;
            PermissionChecker = SingletonDependency<ShwasherPermissionChecker>.Instance;
        }

        public new IIwbSettingManager SettingManager { get; set; }
        public ShwasherPermissionChecker PermissionChecker { get; set; }
        public IIwbSession AbpSession { get; set; }
    }
}