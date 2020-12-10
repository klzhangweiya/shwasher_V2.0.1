using System.Collections.Generic;
using System.Collections.Immutable;
using Abp;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Localization;
using IwbZero.Authorization.Users;
using IwbZero.BaseSysInfo;
using IwbZero.Configuration;

namespace IwbZero.Setting
{
    /// <summary>
    /// Implements <see cref="IIwbSettingDefinitionManager"/>.
    /// </summary>
    public class IwbSettingDefinitionManager<TSet,TUser> : IIwbSettingDefinitionManager,  ISingletonDependency
        where TUser : IwbSysUser<TUser>
        where TSet : IwbSysSetting<TUser>
    {
        protected readonly IIocManager IocManager;
        private readonly IDictionary<string, SettingDefinition> _settings;


        /// <summary>
        /// Constructor.
        /// </summary>
        public IwbSettingDefinitionManager(IIocManager iocManager
            //, ISettingsConfiguration settingsConfiguration
            )
        {
            IocManager = iocManager;
            _settings = new Dictionary<string, SettingDefinition>();
        }

        public virtual void Initialize()
        {
            using (var settingRepository = IocManager.ResolveAsDisposable<IRepository<TSet, int>>())
            {
                var settings = settingRepository.Object.GetAllList(a => a.IsDeleted == false);
                Initialize(settings);
            }
        }

        public void Initialize(List<TSet> settings)
        {
            //using (var settingRepository = IocManager.ResolveAsDisposable<IRepository<TSet, int>>())
            //{
            //    var settings = settingRepository.Object.GetAllList(a => a.IsDeleted == false);
                
            //}
            foreach (var s in settings)
            {
                var setting = new SettingDefinition(s.Code, s.Value, scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true);
                if (_settings.ContainsKey(s.Code))
                    _settings.Remove(s.Code);
                _settings[s.Code] = setting;
            }
            DefaultsSetting();
        }
        

        public virtual void Referesh()
        {
            Initialize();
        }

        public void ChangeSettingDefinition(string name, string value)
        {
            if (_settings.ContainsKey(name))
                _settings.Remove(name);
            var setting = new SettingDefinition(name, value, scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true);
            _settings[name] = setting;
        }

        private void DefaultsSetting()
        {
            _settings[IwbAdminSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin] = new SettingDefinition(
                IwbAdminSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin,
                "false",
                new FixedLocalizableString("Is email confirmation required for login."),
                scopes: SettingScopes.Application | SettingScopes.Tenant,
                clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
            );
            _settings[IwbAdminSettingNames.OrganizationUnits.MaxUserMembershipCount] =
                new SettingDefinition(
                    IwbAdminSettingNames.OrganizationUnits.MaxUserMembershipCount,
                    int.MaxValue.ToString(),
                    new FixedLocalizableString("Maximum allowed organization unit membership count for a user."),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                );
            _settings[IwbAdminSettingNames.UserManagement.TwoFactorLogin.IsEnabled] =
                new SettingDefinition(
                    IwbAdminSettingNames.UserManagement.TwoFactorLogin.IsEnabled,
                    "true",
                    new FixedLocalizableString("Is two factor login enabled."),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                );

            _settings[IwbAdminSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled] =
                new SettingDefinition(
                    IwbAdminSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled,
                    "true",
                    new FixedLocalizableString("Is browser remembering enabled for two factor login."),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                );

            _settings[IwbAdminSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled] =
                new SettingDefinition(
                    IwbAdminSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled,
                    "true",
                    new FixedLocalizableString("Is email provider enabled for two factor login."),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                );

            _settings[IwbAdminSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled] =
                new SettingDefinition(
                    IwbAdminSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled,
                    "true",
                    new FixedLocalizableString("Is sms provider enabled for two factor login."),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                );

            _settings[IwbAdminSettingNames.UserManagement.UserLockOut.IsEnabled] =
                new SettingDefinition(
                    IwbAdminSettingNames.UserManagement.UserLockOut.IsEnabled,
                    "true",
                    new FixedLocalizableString("Is user lockout enabled."),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                );

            _settings[IwbAdminSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout] =
                new SettingDefinition(
                    IwbAdminSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout,
                    "5",
                    new FixedLocalizableString("Maxumum Failed access attempt count before user lockout."),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                );

            _settings[IwbAdminSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds] =
                new SettingDefinition(
                    IwbAdminSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds,
                    "300", //5 minutes
                    new FixedLocalizableString("User lockout in seconds."),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                );

            _settings[IwbAdminSettingNames.UserManagement.PasswordComplexity.RequireDigit] =
                new SettingDefinition(
                    IwbAdminSettingNames.UserManagement.PasswordComplexity.RequireDigit,
                    "false",
                    new FixedLocalizableString("Require digit."),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                );

            _settings[IwbAdminSettingNames.UserManagement.PasswordComplexity.RequireLowercase] =
                new SettingDefinition(
                    IwbAdminSettingNames.UserManagement.PasswordComplexity.RequireLowercase,
                    "false",
                    new FixedLocalizableString("Require lowercase."),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                );

            _settings[IwbAdminSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric] =
                new SettingDefinition(
                    IwbAdminSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric,
                    "false",
                    new FixedLocalizableString("Require non alphanumeric."),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                );

            _settings[IwbAdminSettingNames.UserManagement.PasswordComplexity.RequireUppercase] =
                new SettingDefinition(
                    IwbAdminSettingNames.UserManagement.PasswordComplexity.RequireUppercase,
                    "false",
                    new FixedLocalizableString("Require upper case."),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                );

            _settings[IwbAdminSettingNames.UserManagement.PasswordComplexity.RequiredLength] =
                new SettingDefinition(
                    IwbAdminSettingNames.UserManagement.PasswordComplexity.RequiredLength,
                    "3",
                    new FixedLocalizableString("Required length."),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()
                );
        }

        public SettingDefinition GetSettingDefinition(string name)
        {
            if (!_settings.TryGetValue(name, out var settingDefinition))
            {
                throw new AbpException("There is no setting defined with name: " + name);
            }

            return settingDefinition;
        }

        public IReadOnlyList<SettingDefinition> GetAllSettingDefinitions()
        {
            return _settings.Values.ToImmutableList();
        }

        //private IDisposableDependencyObjectWrapper<SettingProvider> CreateProvider(Type providerType)
        //{
        //    return _iocManager.ResolveAsDisposable<SettingProvider>(providerType);
        //}
    }
}
