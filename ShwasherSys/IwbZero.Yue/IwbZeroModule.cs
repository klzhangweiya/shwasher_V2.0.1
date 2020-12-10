using System.Linq;
using System.Reflection;
using Abp;
using Abp.Dependency;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Reflection;
using IwbZero.Authorization.Roles;
using IwbZero.Authorization.Users;
using IwbZero.Configuration;

namespace IwbZero
{
    [DependsOn(typeof(AbpKernelModule))]
    public class IwbZeroModule : AbpModule
    {

        public override void PreInitialize()
        {
            RegisterService();

            Configuration.Settings.Providers.Add<IwbSettingProvider>();
            IwbRoleConfig.Configure(Configuration.Modules.IwbZero().RoleManagement);

            //Add/remove languages for your application
            //Configuration.Localization.Languages.Add(new LanguageInfo("zh-CN", "简体中文", "famfamfam-flag-cn", true));
            //Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flag-england"));
            //Configuration.Localization.Languages.Add(new LanguageInfo("tr", "Türkçe", "famfamfam-flag-tr"));
            //Configuration.Localization.Languages.Add(new LanguageInfo("ja", "日本語", "famfamfam-flag-jp"));

            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    IwbZeroConsts.IwbZeroLocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "IwbZero.Localization.IwbZero"
                    )
                )
            );
        }
        public override void Initialize()
        {
            FillMissingEntityTypes();
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            

            //IocManager.Resolve<IwbPermissionManager>().Initialize();
            //IocManager.Resolve<IwbSettingDefinitionManager>().Initialize();
        }

        private void RegisterService()
        {
            IocManager.RegisterIfNot<IIwbZeroEntityTypes, IwbZeroEntityTypes>(); //Registered on services.AddAbpIdentity() for Abp.ZeroCore.

            IocManager.Register<IIwbUserManagementConfig, IwbUserManagementConfig>();
            IocManager.Register<IIwbRoleManagementConfig, IwbRoleManagementConfig>();
            IocManager.Register<IIwbLanguageManagementConfig, IwbLanguageManagementConfig>();
            IocManager.Register<IIwbZeroConfig, IwbZeroConfig>();
            //IocManager.Register<IIwbSettingDefinitionManager>();
            //IocManager.Register<IIwbPermissionManager>();
            
        }


        private void FillMissingEntityTypes()
        {
            using (var entityTypes = IocManager.ResolveAsDisposable<IIwbZeroEntityTypes>())
            {
                if (entityTypes.Object.User != null &&
                    entityTypes.Object.Role != null )
                {
                    return;
                }

                using (var typeFinder = IocManager.ResolveAsDisposable<ITypeFinder>())
                {
                    var types = typeFinder.Object.FindAll();
                    entityTypes.Object.Role = types.FirstOrDefault(t => typeof(RoleBase).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract);
                    entityTypes.Object.User = types.FirstOrDefault(t => typeof(UserBase).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract);
                }
            }
        }
    }
}
