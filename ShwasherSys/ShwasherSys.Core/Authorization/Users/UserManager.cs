using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Runtime.Caching;
using ShwasherSys.Authorization.Roles;
using IwbZero.Authorization.Permissions;
using IwbZero.Authorization.Users;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using IwbZero.Session;
using ShwasherSys.CompanyInfo;

namespace ShwasherSys.Authorization.Users
{
    public class UserManager : IwbUserManager<SysRole,SysUser>
    {
        protected IRepository<Employee> EmployeeRepository { get; }
        public UserManager(
            UserStore userStore,
            RoleManager roleManager,
            IIwbPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            //IRepository<OrganizationUnit, long> organizationUnitRepository,
            //IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            //IOrganizationUnitSettings organizationUnitSettings,
            ILocalizationManager localizationManager,
            IwbIdentityEmailMessageService emailService,
            IIwbSettingManager settingManager,
            IIwbUserTokenProviderAccessor iwbUserTokenProviderAccessor, IRepository<Employee> employeeRepository)
            : base(userStore,roleManager,permissionManager,unitOfWorkManager,cacheManager,localizationManager,emailService,settingManager,iwbUserTokenProviderAccessor, ShwasherConsts.LocalizationSourceName)
        {
            EmployeeRepository = employeeRepository;
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(SysUser user, string authenticationType)
        {
            var identity = await base.CreateIdentityAsync(user, authenticationType);
            identity.AddClaim(new Claim(IwbClaimTypes.UserName, user.UserName));
            identity.AddClaim(new Claim(IwbClaimTypes.RealName, user.RealName));
            identity.AddClaim(new Claim(IwbClaimTypes.UserType, user.UserType.ToString()));
            var employee = EmployeeRepository.FirstOrDefault(i => i.UserName == user.UserName);
            identity.AddClaim(new Claim(IwbClaimTypes.EmployeeNo, employee?.No??""));
            identity.AddClaim(new Claim(IwbClaimTypes.EmployeeName, employee?.Name??""));
            identity.AddClaim(new Claim(IwbClaimTypes.EmailAddress, user.EmailAddress));
            var roleList = await GetRolesAsync(user.Id);
            string userRoles = roleList.Any() ? string.Join(",", roleList.ToArray()) : "";
            identity.AddClaim(new Claim(IwbClaimTypes.UserRoles, userRoles));
            //if (user.TenantId.HasValue)
            //{
            //    identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.Value.ToString(CultureInfo.InvariantCulture)));
            //}
            return identity;
        }

    }
}
