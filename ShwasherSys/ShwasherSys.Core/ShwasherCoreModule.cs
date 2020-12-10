using System.Reflection;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using ShwasherSys.Authorization.Users;
using ShwasherSys.BaseSysInfo;
using IwbZero;
using IwbZero.Authorization.Permissions;
using IwbZero.Navigation;
using IwbZero.Setting;
using ShwasherSys.Authorization;

namespace ShwasherSys
{
    [DependsOn(typeof(IwbZeroModule))]
    public class ShwasherCoreModule : AbpModule
    {

        public override void PreInitialize()
        {

            //Configuration.Settings.Providers.Add<IwbSettingProvider>();
            //替换IAbpSession的实现类
            //Configuration.ReplaceService<IAbpSession, IwbSession>(DependencyLifeStyle.Transient);
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            //Add/remove languages for your application
            foreach (var languageInfo in Configuration.Localization.Languages)
            {
                Configuration.Localization.Languages.Remove(languageInfo);
            }
            Configuration.Localization.Languages.Add(new LanguageInfo("zh-CN", "简体中文", "famfamfam-flag-cn", true));
            //Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flag-england"));

            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    ShwasherConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "ShwasherSys.Localization.Language"
                    )
                )
            );
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

        }

        public override void PostInitialize()
        {
            //var httpConfiguration = IocManager.Resolve<IAbpWebApiConfiguration>().HttpConfiguration;
            ////httpConfiguration.Services.Replace(typeof(AbpApiAuthorizeFilter), IocManager.Resolve<IwbApiAuthorizeFilter>());
            //httpConfiguration.Filters.Remove(IocManager.Resolve<AbpApiAuthorizeFilter>());
            //httpConfiguration.Filters.Add(IocManager.Resolve<IwbApiAuthorizeFilter>());
            //GlobalFilters.Filters.Remove(IocManager.Resolve<AbpMvcAuthorizeFilter>());
            //GlobalFilters.Filters.Add(IocManager.Resolve<IwbMvcAuthorizeFilter>());

            IocManager.Register<IIwbPermissionManager, IwbPermissionManager<SysFunction, SysUser>>();
            IocManager.Register<IIwbNavigationManager, IwbNavigationManager<SysFunction, SysUser>>();
            IocManager.Register<IIwbSettingDefinitionManager, IwbSettingDefinitionManager<SysSetting, SysUser>>();
            //IocManager.Register<IPermissionChecker, IwbYuePermissionChecker>();
            //IocManager.Register<IPermissionChecker,IwbPermissionChecker<SysRole, SysUser>>();
            IocManager.Resolve<IwbPermissionManager<SysFunction, SysUser>>().Initialize();
            IocManager.Resolve<IwbSettingDefinitionManager<SysSetting, SysUser>>().Initialize();

         
        }
    }
}
