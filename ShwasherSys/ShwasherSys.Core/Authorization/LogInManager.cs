using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Timing;
using ShwasherSys.Authorization.Roles;
using ShwasherSys.Authorization.Users;
using IwbZero.Authorization;
using IwbZero.Authorization.Users;
using IwbZero.Configuration;
using IwbZero.Helper;
using IwbZero.Setting;
using Microsoft.AspNet.Identity;
using ShwasherSys.CompanyInfo;

namespace ShwasherSys.Authorization
{
    public class LogInManager : IwbLogInManager<SysRole,SysUser>

    {
      
        public LogInManager(
            UserManager userManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IIwbSettingManager settingManager,
            IIwbUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            RoleManager roleManager, IRepository<Employee> employeeRepository)
        :base(userManager,userLoginAttemptRepository,unitOfWorkManager,settingManager,userManagementConfig,iocResolver,roleManager)
        {
            EmployeeRepository = employeeRepository;
        }
        protected IRepository<Employee> EmployeeRepository { get; }
        protected override async Task<IwbLoginResult<SysUser>> CreateLoginResultAsync(SysUser user)
        {
            if (!user.IsActive)
            {
                return new IwbLoginResult<SysUser>(AbpLoginResultType.UserIsNotActive);
            }

            if (await IsEmailConfirmationRequiredForLoginAsync() && !user.IsEmailConfirmed)
            {
                return new IwbLoginResult<SysUser>(AbpLoginResultType.UserEmailIsNotConfirmed);
            }

            user.LastLoginTime = Clock.Now;

            await UserManager.UserStore.UpdateAsync(user);

            await UnitOfWorkManager.Current.SaveChangesAsync();

            return new IwbLoginResult<SysUser>(user, await UserManager.CreateIdentityAsync(user, ShwasherConsts.AuthenticationTypes)
            );
        }

        
        protected override async Task<IwbLoginResult<SysUser>> LoginAsyncInternal(string userNameOrEmailAddress, string plainPassword, bool shouldLockout)
        {
            if (userNameOrEmailAddress.IsEmpty())
            {
                throw new ArgumentNullException(nameof(userNameOrEmailAddress));
            }

            if (plainPassword.IsEmpty())
            {
                throw new ArgumentNullException(nameof(plainPassword));
            }
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {

                var user = await UserManager.UserStore.FindByNameOrEmailAsync(null, userNameOrEmailAddress);
                if (user == null)
                {


                      //打开注释可以用工号登入
                //    string userName =(await EmployeeRepository.FirstOrDefaultAsync(a=>a.No==userNameOrEmailAddress))?.UserName;
                //    user = userName.IsEmpty()
                //        ? null
                //        : await UserManager.UserStore.FindByNameOrEmailAsync(null, userName);


                    if (user == null)
                    {
                        return new IwbLoginResult<SysUser>(AbpLoginResultType.InvalidUserNameOrEmailAddress);
                    }
                }
               

                if (await UserManager.IsLockedOutAsync(user.Id))
                {
                    return new IwbLoginResult<SysUser>(AbpLoginResultType.LockedOut, user);
                }

                UserManager.InitializeLockoutSettings();
                var verificationResult = UserManager.PasswordHasher.VerifyHashedPassword(user.Password, plainPassword);
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return await GetFailedPasswordValidationAsLoginResultAsync(user, shouldLockout);
                }

                if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    return await GetSuccessRehashNeededAsLoginResultAsync(user);
                }

                await UserManager.ResetAccessFailedCountAsync(user.Id);
                return await CreateLoginResultAsync(user);
            }
        }
    }
}
