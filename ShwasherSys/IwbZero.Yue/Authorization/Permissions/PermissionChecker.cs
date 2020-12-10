using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using IwbZero.Authorization.Roles;
using IwbZero.Authorization.Users;

namespace IwbZero.Authorization.Permissions
{
    /// <summary>
    /// Application should inherit this class to implement <see cref="IPermissionChecker"/>.
    /// </summary>

    public abstract class IwbPermissionChecker<TRole, TUser> : IPermissionChecker, ITransientDependency, IIocManagerAccessor
        where TRole : IwbSysRole<TUser>, new()
        where TUser : IwbSysUser<TUser>, new()
    {
        private readonly IwbUserManager<TRole, TUser> _userManager;

        public IIocManager IocManager { get; set; }

        public ILogger Logger { get; set; }

        public IAbpSession AbpSession { get; set; }

        public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected IwbPermissionChecker(IwbUserManager<TRole, TUser> userManager)
        {
            _userManager = userManager;

            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        public  async Task<bool> IsGrantedAsync(string permissionName)
        {
            if (AbpSession.UserId.HasValue)
            {
                if (AbpSession.UserId == 1)
                    return true;
                return await _userManager.IsGrantedAsync(AbpSession.UserId.Value, permissionName);
            }

            return false;
        }

        public  async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            return await _userManager.IsGrantedAsync(userId, permissionName);
        }

        [UnitOfWork]
        public  async Task<bool> IsGrantedAsync(UserIdentifier user, string permissionName)
        {
            if (CurrentUnitOfWorkProvider == null || CurrentUnitOfWorkProvider.Current == null)
            {
                return await IsGrantedAsync(user.UserId, permissionName);
            }

            using (CurrentUnitOfWorkProvider.Current.SetTenantId(user.TenantId))
            {
                return await _userManager.IsGrantedAsync(user.UserId, permissionName);
            }
        }
    }
}
