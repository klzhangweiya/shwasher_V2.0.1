using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Runtime.Session;
using ShwasherSys.Authorization.Users;
using IwbZero.Authorization.Permissions;
using IwbZero.IdentityFramework;
using IwbZero.Session;
using IwbZero.Setting;
using Microsoft.AspNet.Identity;

namespace ShwasherSys
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class ShwasherAppServiceBase : ApplicationService
    {
        public UserManager UserManager { get; set; }
        public new IIwbSession AbpSession { get; set; }
        protected ShwasherAppServiceBase()
        {
            LocalizationSourceName = ShwasherConsts.LocalizationSourceName;
        }

        public new IIwbPermissionManager PermissionManager { protected get; set; }
        protected new IIwbSettingManager SettingManager { get; set; }


        protected Task<SysUser> GetCurrentUserAsync()
        {
            var user = UserManager.FindByIdAsync(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }

            return user;
        }
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }


}