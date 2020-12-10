using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Caching;
using IwbZero.Auditing;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BasicInfo.OutFactory.Dto;
using ShwasherSys.Lambda;
namespace ShwasherSys.BasicInfo.OutFactory
{
    [AbpAuthorize]
    public class OutFactoryAppService : ShwasherAsyncCrudAppService<OutFactory, OutFactoryDto, string, PagedRequestDto, OutFactoryCreateDto, OutFactoryUpdateDto>, IOutFactoryAppService
    {
        public OutFactoryAppService(
			IIwbSettingManager settingManager, 
			ICacheManager cacheManager,
			IRepository<OutFactory, string> repository) : base(repository)
        {
			SettingManager = settingManager;
            CacheManager = cacheManager;
        }

        protected override bool KeyIsAuto { get; set; } = false;

        #region GetSelect

        [DisableAuditing]
        public async Task<List<SelectListItem>> GetSelectList()
        {
            var list = await Repository.GetAllListAsync();
            var slist = new List<SelectListItem> {new SelectListItem {Text = @"请选择...", Value = "", Selected = true}};
            foreach (var l in list)
            {
                //slist.Add(new SelectListItem { Text = l., Value = l. });
            }
            return slist;
        }
        [DisableAuditing]
        public async Task<string> GetSelectStr()
        {
            var list = await Repository.GetAllListAsync();
            string str = "<option value=\"\" selected>请选择...</option>";
            foreach (var l in list)
            {
                //str += $"<option value=\"{l.}\">{l.}</option>";
            }
            return str;
        }

        #endregion

        #region CURD

        [AbpAuthorize(PermissionNames.PagesBasicInfoOutFactoryCreate)]
        public override async Task<OutFactoryDto> Create(OutFactoryCreateDto input)
        {
            return await CreateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesBasicInfoOutFactoryUpdate)]
        public override async Task<OutFactoryDto> Update(OutFactoryUpdateDto input)
        {
            return await UpdateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesBasicInfoOutFactoryDelete)]
        public override Task Delete(EntityDto<string> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesBasicInfoOutFactory)]
        public override async Task<PagedResultDto<OutFactoryDto>> GetAll(PagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<OutFactoryDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

	

        #region Hide

        //protected override IQueryable<OutFactory> ApplyFilter(IQueryable<OutFactory> query, TGetAllInput input)
        //{
        //    if (!input.KeyWords.IsNullOrEmpty())
        //    {
        //        object keyWords = input.KeyWords;
        //        LambdaObject obj = new LambdaObject()
        //        {
        //            FieldType = (LambdaFieldType)input.FieldType,
        //            FieldName = input.KeyField,
        //            FieldValue = keyWords,
        //            ExpType = (LambdaExpType)input.ExpType
        //        };
        //        var exp = obj.GetExp<OutFactory>();
        //        query = query.Where(exp);
        //    }
        //    if (input.SearchList != null && input.SearchList.Count > 0)
        //    {
        //        List<LambdaObject> objList = new List<LambdaObject>();
        //        foreach (var o in input.SearchList)
        //        {
        //            if (o.KeyWords.IsNullOrEmpty())
        //                continue;
        //            object keyWords = o.KeyWords;
        //            objList.Add(new LambdaObject
        //            {
        //                FieldType = (LambdaFieldType)o.FieldType,
        //                FieldName = o.KeyField,
        //                FieldValue = keyWords,
        //                ExpType = (LambdaExpType)o.ExpType
        //            });
        //        }
        //        var exp = objList.GetExp<OutFactory>();
        //        query = query.Where(exp);
        //    }
        //    return query;
        //}

        //protected override IQueryable<OutFactory> ApplySorting(IQueryable<OutFactory> query, PagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<OutFactory> ApplyPaging(IQueryable<OutFactory> query, PagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #endregion


    }
}
