using System;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using IwbZero.AppServiceBase;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.ProductInfo.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace ShwasherSys.ProductInfo
{
    [AbpAuthorize]
    public class StandardsAppService : ShwasherAsyncCrudAppService<Standard, StandardDto, int, PagedRequestDto, StandardCreateDto, StandardUpdateDto >, IStandardsAppService
    {
        public StandardsAppService(IRepository<Standard, int> repository) : base(repository, "StandardName")
        {
            KeyIsAuto = false;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesProductInfoStandards;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesProductInfoStandards;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesProductInfoStandardsCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesProductInfoStandardsUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesProductInfoStandardsDelete;

        [DisableAuditing]
        public List<SelectListItem> GetStandardsList()
        {
            var objList = new List<SelectListItem>();
           
            var entitys = Repository.GetAll();
            foreach (var standard in entitys)
            {
                objList.Add(new SelectListItem()
                {
                    Text = standard.StandardName,
                    Value = standard.Id+""
                });
            }
            return objList;
        }

    }
}
