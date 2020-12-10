using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BasicInfo.Departments.Dto;

namespace ShwasherSys.BasicInfo.Departments
{
    [AbpAuthorize]
    public class DepartmentsAppService : ShwasherAsyncCrudAppService<Department, DepartmentDto, string, PagedRequestDto, DepartmentCreateDto, DepartmentUpdateDto >, IDepartmentsAppService
    {
        public DepartmentsAppService(IRepository<Department, string> repository) : base(repository)
        {
            KeyIsAuto = false;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesBasicInfoDepartments;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesBasicInfoDepartments;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesBasicInfoDepartmentsCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesBasicInfoDepartmentsUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesBasicInfoDepartmentsDelete;


        public override async Task Delete(EntityDto<string> input)
        {
            CheckDeletePermission();
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            /*if ((await Repository.GetAllListAsync(a => a.FatherRegionID == entity.Id && a.IsLock == "N")).Any())
            {
                CheckErrors(IwbIdentityResult.Failed("此菜单下还有子区域，不能删除"));
            }*/

            entity.IsLock = "Y";
           // await Repository.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public List<SelectListItem> GetDepartmentsSelects()
        {
            var slist = new List<SelectListItem>();
            var list = Repository.GetAll();
            foreach (var l in list)
            {
                slist.Add(new SelectListItem { Text = l.DepartmentName, Value = l.Id });
            }
            return slist;
        }
    }
}
