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
using Abp.Domain.Entities;
using Abp.Timing;

namespace ShwasherSys.BasicInfo
{
    [AbpAuthorize]
    public class CurrencyAppService : ShwasherAsyncCrudAppService<Currency, CurrencyDto, string, PagedRequestDto, CurrencyCreateDto, CurrencyUpdateDto >, ICurrencyAppService
    {
        public CurrencyAppService(
			IIwbSettingManager settingManager, 
			ICacheManager cacheManager,
			IRepository<Currency, string> repository, IRepository<CurrencyExchangeRate> currencyExchangeRateRepository) : base(repository, "CurrencyName")
        {
            CurrencyExchangeRateRepository = currencyExchangeRateRepository;
            SettingManager = settingManager;
            CacheManager = cacheManager;
        }
        protected IRepository<CurrencyExchangeRate> CurrencyExchangeRateRepository { get; }
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

        [AbpAuthorize(PermissionNames.PagesBasicInfoCurrencyCreate)]
        public override async Task<CurrencyDto> Create(CurrencyCreateDto input)
        {
            Currency entity = MapToEntity(input);
            entity.TimeLastMod = Clock.Now;
            entity.TimeCreated = Clock.Now;
            entity.UserIDLastMod = AbpSession.UserName;
            await Repository.InsertAsync(entity);

            var rates = input.CurrencyExchangeRates;
            if (rates != null && rates.Any())
            {
                rates.Reverse();
                var rs = rates.DistinctBy(i => i.ToCurrencyId).ToList();
                foreach (var r in rs)
                {
                    CurrencyExchangeRate rate = new CurrencyExchangeRate()
                    {
                        ExchangeRate =r.ExchangeRate,
                        FromCurrencyId = input.Id,
                        ToCurrencyId = r.ToCurrencyId,
                        TimeLastMod = Clock.Now,
                        TimeCreated = Clock.Now,
                        UserIDLastMod = AbpSession.UserName
                    };
                    await CurrencyExchangeRateRepository.InsertAsync(rate);
                }
            }
            return MapToEntityDto(entity);
        }

        [AbpAuthorize(PermissionNames.PagesBasicInfoCurrencyUpdate)]
        public override async Task<CurrencyDto> Update(CurrencyUpdateDto input)
        {
            Currency entity = Repository.FirstOrDefault(input.Id);
            if (entity == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未找到货币！"));
            }
            entity.CurrencyName = input.CurrencyName;
            entity.TimeLastMod = Clock.Now;
            entity.UserIDLastMod = AbpSession.UserName;
            var rates = input.CurrencyExchangeRates;
            await CurrencyExchangeRateRepository.DeleteAsync(i => i.FromCurrencyId == input.Id);
            if (rates != null && rates.Any())
            {
                rates.Reverse();
                var rs = rates.DistinctBy(i => i.ToCurrencyId).ToList();
                foreach (var r in rs)
                {
                    CurrencyExchangeRate rate = new CurrencyExchangeRate()
                    {
                        ExchangeRate = r.ExchangeRate,
                        FromCurrencyId = input.Id,
                        ToCurrencyId = r.ToCurrencyId,
                        TimeLastMod = Clock.Now,
                        TimeCreated = Clock.Now,
                        UserIDLastMod = AbpSession.UserName
                    };
                    await CurrencyExchangeRateRepository.InsertAsync(rate);
                }
            }
            return MapToEntityDto(entity);
        }

        [AbpAuthorize(PermissionNames.PagesBasicInfoCurrencyDelete)]
        public override Task Delete(EntityDto<string> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesBasicInfoCurrency)]
        public override async Task<PagedResultDto<CurrencyDto>> GetAll(PagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<CurrencyDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

		#region Get

		[DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesBasicInfoCurrency)]
        public  Task<Currency> GetEntityById(string id)
        {
            return Repository.FirstOrDefaultAsync(id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesBasicInfoCurrency)]
        public Task<Currency> GetEntityByNo(string no)
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
            var exp = obj.GetExp<Currency>();
            return Repository.FirstOrDefaultAsync(exp);
        }

		[DisableAuditing]
         [AbpAuthorize(PermissionNames.PagesBasicInfoCurrency)]
        public  async Task<CurrencyDto> GetDtoById(string id)
        {
            var entity = await GetEntityById(id);
            CurrencyDto enDto = MapToEntityDto(entity);
            var rates =await CurrencyExchangeRateRepository.GetAllListAsync(i => i.FromCurrencyId == id);
            enDto.CurrencyExchangeRates = rates;
            return enDto;
        }
		[DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesBasicInfoCurrency)]
        public async Task<CurrencyDto> GetDtoByNo(string no)
        {
            var entity = await GetEntityByNo(no);
            return MapToEntityDto(entity);
        }

      

       

        #endregion

        #region Hide

        //protected override IQueryable<Currency> ApplyFilter(IQueryable<Currency> query, TGetAllInput input)
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
        //        var exp = obj.GetExp<Currency>();
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
        //        var exp = objList.GetExp<Currency>();
        //        query = query.Where(exp);
        //    }
        //    return query;
        //}

        //protected override IQueryable<Currency> ApplySorting(IQueryable<Currency> query, PagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<Currency> ApplyPaging(IQueryable<Currency> query, PagedRequestDto input)
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
