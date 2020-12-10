using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.UI;
using Castle.Core.Internal;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.Authorization.Roles;
using ShwasherSys.Authorization.Users;
using ShwasherSys.BaseSysInfo.Roles.Dto;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.Lambda;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.Authorization.Permissions;
using IwbZero.Caching;
using IwbZero.IdentityFramework;

namespace ShwasherSys.BaseSysInfo.Roles
{
    [AbpAuthorize(PermissionNames.PagesSystemRoles), AuditLog("系统角色", "角色")]
    public class RolesAppService : ShwasherAsyncCrudAppService<SysRole, RoleDto, int, PagedRequestDto, RoleCreateDto, RoleUpdateDto>, IRolesAppService
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly IRepository<SysUser, long> _userRepository;
        private readonly IRepository<SysUserRole, long> _userRoleRepository;
        private readonly IStatesAppService _stateAppService;

        protected override string GetPermissionName { get; set; } = PermissionNames.PagesSystemRolesCreate;
        protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesSystemRolesCreate;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesSystemRolesCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesSystemRolesUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesSystemRolesDelete;

        public RolesAppService(
            IRepository<SysRole, int> repository,
            RoleManager roleManager,
            UserManager userManager,
            IStatesAppService stateAppService,
            IRepository<SysUser, long> userRepository,
            IRepository<SysUserRole, long> userRoleRepository,
            ICacheManager cacheManager)
            : base(repository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _stateAppService = stateAppService;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            CacheManager = cacheManager;
            LocalizationSourceName = ShwasherConsts.LocalizationSourceName;
        }


        [DisableAuditing]
        public List<SelectListItem> GetRoleTypeSelect()
        {
            var slist = new List<SelectListItem>();
            var list = _stateAppService.GetStateList("SysRole", "RoleType");
            foreach (var l in list)
            {
                if (int.TryParse(l.CodeValue, out var roleType))
                {
                    if (roleType <= AbpSession.UserType && AbpSession?.UserName.ToLower() != "admin")
                    {
                        continue;
                    }
                    slist.Add(new SelectListItem { Text = l.DisplayValue, Value = l.CodeValue });
                }
            }
            return slist;
        }


        [DisableAuditing]
        public async Task<RoleDto> GetRoleByIdAsync(int roleId)
        {
            var role = await _roleManager.GetRoleByIdAsync(roleId);
            return MapToEntityDto(role);
        }

        [DisableAuditing]
        public async Task<PagedResultDto<RoleDtoModel>> GetAllRole(PagedRequestDto input)
        {
            CheckGetAllPermission();
            var query = CreateFilteredQuery(input);
            if (AbpSession?.UserName?.ToLower() != "admin")
                query = query.Where(a => a.Name.ToLower() != "admin" && a.RoleType > AbpSession.UserType);
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
                var exp = objList.GetExp<SysRole>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = new PagedResultDto<RoleDtoModel>(
                totalCount,
                entities.Select(a => new RoleDtoModel()
                {
                    Id = a.Id,
                    Name = a.Name,
                    RoleDisplayName = a.RoleDisplayName,
                    RoleType = a.RoleType,
                    RoleTypeName = _stateAppService.GetDisplayValue("SysRole", "RoleType", a.RoleType + ""),
                    Description = a.Description,
                    IsStatic = a.IsStatic,
                    LastModificationTime = a.LastModificationTime,
                    LastModifierUserName = a.LastModifierUser?.UserName ?? ""
                }).ToList()
            );
            return dtos;
        }

        public override async Task<RoleDto> Create(RoleCreateDto input)
        {
            var role = ObjectMapper.Map<SysRole>(input);
            CheckCreatePermission();
            //var result=  await _roleManager.CheckDuplicateRoleNameAsync(input.Id, input.Name, input.RoleDisplayName);
            //if (!result.Succeeded)
            //{
            //    CheckErrors(result);
            //}
            CheckErrors(await _roleManager.CreateAsync(role));

            await CurrentUnitOfWork.SaveChangesAsync();

            return new RoleDto();
        }
        public override async Task<RoleDto> Update(RoleUpdateDto input)
        {
            if (input.RoleType <= AbpSession.UserType && AbpSession?.UserName.ToLower() != "admin")
            {
                CheckErrors(IwbIdentityResult.Failed("没有此角色类型的权限，请检查后再操作！"));
            }
            CheckUpdatePermission();
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            MapToEntity(input, role);
            CheckErrors(await _roleManager.UpdateAsync(role));
            //var result=  await _roleManager.CheckDuplicateRoleNameAsync(input.Id, input.Name, input.RoleDisplayName);
            //if (!result.Succeeded)
            //{
            //    CheckErrors(result);
            //}
            //var entity = await GetEntityByIdAsync(input.Id);

            //MapToEntity(input, entity);
            //await CurrentUnitOfWork.SaveChangesAsync();
            return new RoleDto();
        }
        public override async Task Delete(EntityDto<int> input)
        {
            CheckDeletePermission();

            var role = await _roleManager.FindByIdAsync(input.Id);
            if (role.IsStatic)
            {
                throw new UserFriendlyException("CannotDeleteAStaticRole");
            }

            var users = await GetUsersInRoleAsync(role.Name);

            foreach (var user in users)
            {
                CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.Name));
            }

            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        [AbpAuthorize(PermissionNames.PagesSystemRolesAuth), AuditLog("角色权限配置")]
        public async Task Auth(AuthDto input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            if (AbpSession?.UserType == 1 && AbpSession?.UserName.ToLower() != "admin")
            {
                CheckErrors(IwbIdentityResult.Failed("超级管理员权限不能修改"));
            }
            var grantedPermissions = new List<Permission>();
            if (input.PermissionNames != null && input.PermissionNames.Any())
            {
                grantedPermissions = PermissionManager
                    .GetAllPermissions()
                    .Where(p => input.PermissionNames.Contains(p.Name))
                    .ToList();
            }
            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
            CacheManager.GetCache(IwbRolePermissionCacheItem.CacheStoreName).Remove(input.Id + "@" + (AbpSession.GetTenantId()));
        }

        private Task<List<long>> GetUsersInRoleAsync(string roleName)
        {
            var users = (from user in _userRepository.GetAll()
                         join userRole in _userRoleRepository.GetAll() on user.Id equals userRole.UserId
                         join role in Repository.GetAll() on userRole.RoleId equals role.Id
                         where role.Name == roleName
                         select user.Id).Distinct().ToList();

            return Task.FromResult(users);
        }
        [DisableAuditing]
        public async Task<bool> IsGrantedAsync(int roleId, string permissionNmae)
        {
            return await _roleManager.IsGrantedAsync(roleId, permissionNmae);
        }

        [DisableAuditing]
        public Task<ListResultDto<PermissionDto>> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();

            return Task.FromResult(new ListResultDto<PermissionDto>(
                ObjectMapper.Map<List<PermissionDto>>(permissions)
            ));
        }

        //protected override IQueryable<SysRole> CreateFilteredQuery(PagedRequestDto input)
        //{
        //    return Repository.GetAllIncluding(x => x.Permissions);
        //}

        //protected override Task<SysRole> GetEntityByIdAsync(int id)
        //{
        //    var role = Repository.GetAllIncluding(x => x.Permissions).FirstOrDefault(x => x.Id == id);
        //    return Task.FromResult(role);
        //}

        protected override IQueryable<SysRole> ApplySorting(IQueryable<SysRole> query, PagedRequestDto input)
        {
            return query.OrderBy(r => r.RoleDisplayName);
        }

    }
}