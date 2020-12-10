using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Castle.Core.Internal;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.Authorization.Roles;
using ShwasherSys.Authorization.Users;
using ShwasherSys.BaseSysInfo.Roles.Dto;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.BaseSysInfo.Users.Dto;
using ShwasherSys.Lambda;
using IwbZero;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.Authorization.Permissions;
using IwbZero.Authorization.Users;
using IwbZero.IdentityFramework;
using IwbZero.Session;
using IwbZero.Setting;
using Microsoft.AspNet.Identity;
using IwbZero.Caching;

namespace ShwasherSys.BaseSysInfo.Users
{
    [AbpAuthorize(PermissionNames.PagesSystemUsers), AuditLog("系统用户", "用户")]
    public class UsersAppService : ShwasherAsyncCrudAppService<SysUser, UserDto, long, PagedRequestDto, UserCreateDto, UserUpdateDto>, IUsersAppService
    {

        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<SysRole> _roleRepository;
        private readonly IStatesAppService _stateAppService;


        public UsersAppService(
            IRepository<SysUser, long> repository,
            UserManager userManager,
            IRepository<SysRole> roleRepository,
            IStatesAppService stateAppService,
            RoleManager roleManager,
            IwbSettingManager settingManager,
            ICacheManager cacheManager)
            : base(repository)
        {
            _userManager = userManager;
            _roleRepository = roleRepository;
            _stateAppService = stateAppService;
            _roleManager = roleManager;
            SettingManager = settingManager;
            CacheManager = cacheManager;
        }


        protected override string GetPermissionName { get; set; } = PermissionNames.PagesSystemUsers;
        protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesSystemUsers;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesSystemUsersCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesSystemUsersUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesSystemUsersDelete;


        [DisableAuditing]
        public List<SelectListItem> GetUserTypeSelect()
        {
            var slist = new List<SelectListItem>();
            var list = _stateAppService.GetStateList("SysUser", "UserType");
            foreach (var l in list)
            {
                if (int.TryParse(l.CodeValue, out var userType))
                {
                    if (userType <= AbpSession.UserType && AbpSession?.UserName.ToLower() != "admin")
                    {
                        continue;
                    }
                    slist.Add(new SelectListItem { Text = l.DisplayValue, Value = l.CodeValue });
                }
            }
            return slist;
        }

        #region Roles


        [DisableAuditing]
        public async Task<string[]> GetUserRoles(long userId)
        {
            var roleList = await _userManager.GetRolesAsync(userId);
            string[] roles = roleList.ToArray();
            return roles;
        }


        [DisableAuditing]
        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync(a =>
                (AbpSession.UserName == UserBase.AdminUserName || a.RoleType > AbpSession.UserType) );
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }
        [DisableAuditing]
        public List<SelectListItem> GetRoleSelects()
        {
            var slist = new List<SelectListItem>();
            var list = _roleRepository.GetAllList(a =>
                (AbpSession.UserName == UserBase.AdminUserName || a.RoleType > AbpSession.UserType));
            foreach (var l in list)
            {
                slist.Add(new SelectListItem { Text = l.RoleDisplayName, Value = l.Name });
            }
            return slist;
        }

        #endregion


        [AbpAuthorize(PermissionNames.PagesSystemUsersResetPassword), AuditLog("重置密码")]
        public async Task ResetPassword(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            if (user == null)
            {
                CheckErrors(IwbIdentityResult.Failed("用户不存在，请检查后再操作！"));
                return;
            }
            var password = await SettingManager.GetSettingValueAsync(SettingNames.UserDefaultPassword);
            user.Password = new PasswordHasher().HashPassword(password);
            await Repository.UpdateAsync(user);
        }

        #region Auth

        [AbpAuthorize(PermissionNames.PagesSystemUsersAuth), AuditLog("用户权限配置")]
        public async Task Auth(AuthDto input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            var grantedPermissions = new List<Permission>();
            if (input.PermissionNames != null && input.PermissionNames.Any())
            {
                grantedPermissions = PermissionManager
                    .GetAllPermissions()
                    .Where(p => input.PermissionNames.Contains(p.Name))
                    .ToList();
            }

            await _userManager.SetUserGrantedPermissionsAsync(user, grantedPermissions);
            CacheManager.GetCache(IwbUserPermissionCacheItem.CacheStoreName).Remove(input.Id + "@" + (AbpSession.GetTenantId()));
        }

        [DisableAuditing]
        public Task<ListResultDto<PermissionDto>> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();

            return Task.FromResult(new ListResultDto<PermissionDto>(
                ObjectMapper.Map<List<PermissionDto>>(permissions)
            ));
        }

        [DisableAuditing]
        public async Task<UserDto> GetUserByIdAsync(long userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            return MapToEntityDto(user);
        }
        [DisableAuditing]
        public async Task<bool> IsGrantedOnlyUserAsync(long userId, string permissionNmae)
        {
            return await _userManager.IsGrantedOnlyUserAsync(userId, permissionNmae);
        }

        #endregion

        #region CURD

        [DisableAuditing]
        public async Task<PagedResultDto<UserDtoModel>> GetAllUser(PagedRequestDto input)
        {
            CheckGetAllPermission();
            var query = CreateFilteredQuery(input);
            if (AbpSession?.UserName?.ToLower() != "admin")
                query = query.Where(a => a.UserName.ToLower() != "admin" &&
                    (a.UserType > AbpSession.UserType || a.UserName == AbpSession.UserName));
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    object keyWords = o.KeyWords;
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType
                    });
                }
                var exp = objList.GetExp<SysUser>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<UserDtoModel>(
                totalCount,
                entities.Select(a => new UserDtoModel()
                {
                    Id = a.Id,
                    UserName = a.UserName,
                    UserType = a.UserType,
                    UserTypeName = _stateAppService.GetDisplayValue("SysUser", "UserType", a.UserType + ""),
                    RealName = a.RealName,
                    EmailAddress = a.EmailAddress,
                    IsActive = a.IsActive,
                    IsActiveName = _stateAppService.GetDisplayValue("SysUser", "IsActive", a.IsActive.ToString().ToLower()),
                    LastLoginTime = a.LastLoginTime,
                    LastModificationTime = a.LastModificationTime,
                    CreationTime = a.CreationTime,
                    LastModifierUserName = a.LastModifierUser?.UserName ?? "",
                    DepartmentID = a.DepartmentID,
                    DutyID = a.DutyID,
                    FactoryID = a.FactoryID
                }).ToList()
            );
            return dtos;
        }

        public override async Task<UserDto> Get(EntityDto<long> input)
        {
            var user = await base.Get(input);
            var userRoles = await _userManager.GetRolesAsync(user.Id);
            user.RoleNames = userRoles.Select(ur => ur).ToArray();
            return user;
        }

        [AbpAuthorize(PermissionNames.PagesSystemUsersCreate)]
        public override async Task<UserDto> Create(UserCreateDto input)
        {

            var user = ObjectMapper.Map<SysUser>(input);
            var password = await SettingManager.GetSettingValueAsync(SettingNames.UserDefaultPassword);
            user.Password = new PasswordHasher().HashPassword(password);
            user.IsEmailConfirmed = false;
            //Assign roles
            user.Roles = new Collection<SysUserRole>();
            if (!input.RoleNames.IsNullOrEmpty())
            {
                foreach (var roleName in input.RoleNames.Split(','))
                {
                    var role = await _roleManager.GetRoleByNameAsync(roleName);
                    user.Roles.Add(new SysUserRole(user.Id, role.Id));
                }
            }

            //var result = await _userManager.CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            //if (!result.Succeeded)
            //{
            //    CheckErrors(result);
            //}
            CheckErrors(await _userManager.CreateAsync(user));

            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(user);
        }

        [AbpAuthorize(PermissionNames.PagesSystemUsersUpdate)]
        public override async Task<UserDto> Update(UserUpdateDto input)
        {
            if (AbpSession?.UserName.ToLower() != "admin")
            {
                var oldUser = await _userManager.GetOldUserAsync(input.Id);
                if (oldUser.UserType <= AbpSession?.UserType)
                    CheckErrors(IwbIdentityResult.Failed("没有修改此用户的权限。"));
                if (input.UserType <= AbpSession?.UserType)
                    CheckErrors(IwbIdentityResult.Failed("没有此用户类型的权限，请检查用户类型后再操作！"));
            }

            var user = await _userManager.GetUserByIdAsync(input.Id);
            MapToEntity(input, user);
            CheckErrors(await _userManager.UpdateAsync(user));
            CheckErrors(await _userManager.SetRoles(user, input.RoleNames?.Split(',')));
            CacheManager.GetCache(IwbZeroConsts.SystemUserCache)
                .Set(input.Id + "", Repository.FirstOrDefault(input.Id));
            return new UserDto();
        }

        [AbpAuthorize(PermissionNames.PagesSystemUsersDelete)]
        public override async Task Delete(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            if (user.UserName.ToLower() == "admin" || user.UserName.ToLower() == "system" || user.UserType <= AbpSession?.UserType)
            {
                CheckErrors(IwbIdentityResult.Failed("当前用户不能被删除。"));
            }
            await _userManager.DeleteAsync(user);
            await CacheManager.GetCache(IwbZeroConsts.SystemUserCache).RemoveAsync(input.Id + "");
        }


        #endregion

        protected override IQueryable<SysUser> CreateFilteredQuery(PagedRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles);
        }

        protected override async Task<SysUser> GetEntityByIdAsync(long id)
        {
            var user = Repository.GetAllIncluding(x => x.Roles).FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(user);
        }

        protected override IQueryable<SysUser> ApplySorting(IQueryable<SysUser> query, PagedRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

    }
}
