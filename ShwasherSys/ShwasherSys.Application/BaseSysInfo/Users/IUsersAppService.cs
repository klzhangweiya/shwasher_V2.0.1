using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using ShwasherSys.BaseSysInfo.Roles.Dto;
using ShwasherSys.BaseSysInfo.Users.Dto;
using IwbZero.AppServiceBase;

namespace ShwasherSys.BaseSysInfo.Users
{
    public interface IUsersAppService : IIwbAsyncCrudAppService<UserDto, long, PagedRequestDto, UserCreateDto, UserUpdateDto>
    {
        List<SelectListItem> GetUserTypeSelect();
        Task<UserDto> GetUserByIdAsync(long userId);
        Task<ListResultDto<PermissionDto>> GetAllPermissions();
        Task<ListResultDto<RoleDto>> GetRoles();
        List<SelectListItem> GetRoleSelects();
        Task<PagedResultDto<UserDtoModel>> GetAllUser(PagedRequestDto input);
        Task Auth(AuthDto input);
        Task ResetPassword(EntityDto<long> input);
        Task<string[]> GetUserRoles(long userId);
        Task<bool> IsGrantedOnlyUserAsync(long userId, string permissionNmae);
    }
}
