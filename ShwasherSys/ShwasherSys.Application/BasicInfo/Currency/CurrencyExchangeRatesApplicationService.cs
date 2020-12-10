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
using ShwasherSys.Lambda;
using ShwasherSys.BasicInfo.Dto;
using Abp.Configuration;

namespace ShwasherSys.BasicInfo
{
    [AbpAuthorize]
    public class CurrencyExchangeRateAppService : ShwasherAsyncCrudAppService<CurrencyExchangeRate, CurrencyExchangeRateDto, int, PagedRequestDto, CurrencyExchangeRateCreateDto, CurrencyExchangeRateUpdateDto >, ICurrencyExchangeRateAppService
    {
        public CurrencyExchangeRateAppService(
			IIwbSettingManager settingManager, 
			ICacheManager cacheManager,
			IRepository<CurrencyExchangeRate, int> repository) : base(repository)
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

        [AbpAuthorize(PermissionNames.PagesBasicInfoCurrency)]
        public override async Task<CurrencyExchangeRateDto> Create(CurrencyExchangeRateCreateDto input)
        {
            return await CreateEntity(input);
        }

        //[AbpAuthorize(PermissionNames.PagesCurrencyExchangeRateUpdate)]
        public override async Task<CurrencyExchangeRateDto> Update(CurrencyExchangeRateUpdateDto input)
        {
            return await UpdateEntity(input);
        }

        //[AbpAuthorize(PermissionNames.PagesCurrencyExchangeRateDelete)]
        public override Task Delete(EntityDto<int> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        //[AbpAuthorize(PermissionNames.PagesCurrencyExchangeRate)]
        public override async Task<PagedResultDto<CurrencyExchangeRateDto>> GetAll(PagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<CurrencyExchangeRateDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

		#region Get

		[DisableAuditing]
       // [AbpAuthorize(PermissionNames.PagesCurrencyExchangeRate)]
        public  Task<CurrencyExchangeRate> GetEntityById(int id)
        {
            return Repository.FirstOrDefaultAsync(id);
        }

        [DisableAuditing]
        //[AbpAuthorize(PermissionNames.PagesCurrencyExchangeRate)]
        public  Task<CurrencyExchangeRate> GetEntityByNo(string no)
        {
            if (KeyFiledName.IsNullOrEmpty())
            {
                CheckErrors(IwbIdentityResult.Failed("编码/编号字段不明确，请检查后再操作！"));
            }
            LambdaObject obj = new LambdaObject()
            {
                FieldType = LambdaFieldType.S,
                FieldName = KeyFiledName,
                FieldValue = no,
                ExpType = LambdaExpType.Equal
            };
            var exp = obj.GetExp<CurrencyExchangeRate>();
            return Repository.FirstOrDefaultAsync(exp);
        }

		[DisableAuditing]
        //[AbpAuthorize(PermissionNames.PagesCurrencyExchangeRate)]
        public  async Task<CurrencyExchangeRateDto> GetDtoById(int id)
        {
            var entity = await GetEntityById(id);
            return MapToEntityDto(entity);
        }
		[DisableAuditing]
        //[AbpAuthorize(PermissionNames.PagesCurrencyExchangeRate)]
        public  async Task<CurrencyExchangeRateDto> GetDtoByNo(string no)
        {
            var entity = await GetEntityByNo(no);
            return MapToEntityDto(entity);
        }

        #endregion

		#region Hide
       
        //protected override IQueryable<CurrencyExchangeRate> ApplyFilter(IQueryable<CurrencyExchangeRate> query, TGetAllInput input)
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
        //        var exp = obj.GetExp<CurrencyExchangeRate>();
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
        //        var exp = objList.GetExp<CurrencyExchangeRate>();
        //        query = query.Where(exp);
        //    }
        //    return query;
        //}

        //protected override IQueryable<CurrencyExchangeRate> ApplySorting(IQueryable<CurrencyExchangeRate> query, PagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<CurrencyExchangeRate> ApplyPaging(IQueryable<CurrencyExchangeRate> query, PagedRequestDto input)
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
