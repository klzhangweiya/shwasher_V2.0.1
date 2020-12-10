using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using IwbZero.AppServiceBase;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BasicInfo.Dutys.Dto;

namespace ShwasherSys.BasicInfo.Dutys
{
    [AbpAuthorize]
    public class DutysAppService : ShwasherAsyncCrudAppService<Duty, DutyDto, int, PagedRequestDto, DutyCreateDto, DutyUpdateDto >, IDutysAppService
    {
        public DutysAppService(IRepository<Duty, int> repository) : base(repository)
        {
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesBasicInfoDutys;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesBasicInfoDutys;
		protected override string CreatePermissionName { get; set; } = PermissionNames.PagesBasicInfoDutysCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesBasicInfoDutysUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesBasicInfoDutysDelete;

        public override async Task Delete(EntityDto<int> input)
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

        public List<SelectListItem> GetDutysSelects()
        {
            var slist = new List<SelectListItem>();
            var list = Repository.GetAll();
            foreach (var l in list)
            {
                slist.Add(new SelectListItem { Text = l.DutyName, Value = l.Id+"" });
            }
            return slist;
        }
    }
}
