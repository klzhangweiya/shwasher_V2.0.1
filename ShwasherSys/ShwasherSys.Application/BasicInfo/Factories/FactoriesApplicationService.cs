using System.Collections.Generic;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using IwbZero.AppServiceBase;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BasicInfo.Factory.Dto;

namespace ShwasherSys.BasicInfo.Factory
{
    [AbpAuthorize]
    public class FactoriesAppService : ShwasherAsyncCrudAppService<Factories, FactoriesDto, string, PagedRequestDto, FactoriesCreateDto, FactoriesUpdateDto >, IFactoriesAppService
    {
        public FactoriesAppService(IRepository<Factories, string> repository) : base(repository)
        {
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesBasicInfoFactories;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesBasicInfoFactories;
		protected override string CreatePermissionName { get; set; } = PermissionNames.PagesBasicInfoFactoriesCreate;
		protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesBasicInfoFactoriesUpdate;
		protected override string DeletePermissionName { get; set; } = PermissionNames.PagesBasicInfoFactoriesDelete;

        [DisableAuditing]
        public List<SelectListItem> GetFactoriesSelects()
        {
            var slist = new List<SelectListItem>();
            var list = Repository.GetAll();
            foreach (var l in list)
            {
                slist.Add(new SelectListItem { Text = l.FactoryName, Value = l.Id });
            }
            return slist;
        }

      

    }
}
