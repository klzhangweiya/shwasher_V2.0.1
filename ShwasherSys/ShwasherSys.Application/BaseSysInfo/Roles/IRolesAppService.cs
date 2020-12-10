using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ShwasherSys.BaseSysInfo.Roles.Dto;
using IwbZero.AppServiceBase;

namespace ShwasherSys.BaseSysInfo.Roles
{
    public interface IRolesAppService : IAsyncCrudAppService<RoleDto, int, PagedRequestDto, RoleCreateDto, RoleUpdateDto>
    {
        Task<RoleDto> GetRoleByIdAsync(int roleId);
        List<SelectListItem> GetRoleTypeSelect();
        Task<bool> IsGrantedAsync(int roleId, string permissionNmae);
        Task<PagedResultDto<RoleDtoModel>> GetAllRole(PagedRequestDto input);
        Task<ListResultDto<PermissionDto>> GetAllPermissions();
        //Task<PagedResultDto<RoleDto>> GetRoles(PagedResultRequestDto input);
        Task Auth(AuthDto input);
    }
}
