using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Json;
using Abp.Runtime.Caching;
using Abp.Timing;
using IwbZero.Auditing;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using Newtonsoft.Json;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.ProductStoreInfo.Dto;
using ShwasherSys.RmStore.Dto;
namespace ShwasherSys.RmStore
{
    [AbpAuthorize]
    public class CurrentRmStoreHouseAppService : IwbZeroAsyncCrudAppService<CurrentRmStoreHouse, CurrentRmStoreHouseDto, string, IwbPagedRequestDto, CurrentRmStoreHouseCreateDto, CurrentRmStoreHouseUpdateDto >, ICurrentRmStoreHouseAppService
    {
        public CurrentRmStoreHouseAppService(
			ICacheManager cacheManager,
			IRepository<CurrentRmStoreHouse, string> repository, IRepository<ViewCurrentRmStoreHouse, string> viewCurrentRmStoreHouseRepository, IRepository<RmEnterStore, string> rmEnterStoreRepository, IRepository<RmOutStore, string> rmOutStoreRepository) : base(repository, "Id")
        {
            ViewCurrentRmStoreHouseRepository = viewCurrentRmStoreHouseRepository;
            RmEnterStoreRepository = rmEnterStoreRepository;
            RmOutStoreRepository = rmOutStoreRepository;
            CacheManager = cacheManager;
        }

        protected override bool KeyIsAuto { get; set; } = false;

        protected IRepository<ViewCurrentRmStoreHouse,string> ViewCurrentRmStoreHouseRepository { get; }
        protected IRepository<RmEnterStore,string> RmEnterStoreRepository { get; }
        protected IRepository<RmOutStore,string> RmOutStoreRepository { get; }

        #region GetSelect

        [DisableAuditing]
        public override async Task<List<SelectListItem>> GetSelectList()
        {
            var list = await Repository.GetAllListAsync();
            var sList = new List<SelectListItem> {new SelectListItem {Text = @"请选择...", Value = "", Selected = true}};
            foreach (var l in list)
            {
                //sList.Add(new SelectListItem { Value = l.Id, Text = l. });
            }
            return sList;
        }
        [DisableAuditing]
        public override async Task<string> GetSelectStr()
        {
            var list = await Repository.GetAllListAsync();
            string str = "<option value=\"\" selected>请选择...</option>";
            foreach (var l in list)
            {
                //str += $"<option value=\"{l.Id}\">{l.}</option>";
            }
            return str;
        }

        #endregion

        #region CURD

        

       
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmCurrentStoreMg)]
        public override async Task<PagedResultDto<CurrentRmStoreHouseDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<CurrentRmStoreHouseDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmCurrentStoreMgQuery)]
        public override async Task<CurrentRmStoreHouseDto> GetDto(EntityDto<string> input)
        {
            var entity = await GetEntity(input);
            return MapToEntityDto(entity);
        }

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmCurrentStoreMgQuery)]
        public override async Task<CurrentRmStoreHouseDto> GetDtoById(string id)
        {
            var entity = await GetEntityById(id);
            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 查询实体Dto（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmCurrentStoreMgQuery)]
        public override async Task<CurrentRmStoreHouseDto> GetDtoByNo(string no)
        {
            var entity = await GetEntityByNo(no);
            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmCurrentStoreMgQuery)]
        public override async Task<CurrentRmStoreHouse> GetEntity(EntityDto<string> input)
        {
            var entity = await GetEntityById(input.Id);
            return entity;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmCurrentStoreMgQuery)]
        public override async Task<CurrentRmStoreHouse> GetEntityById(string id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmCurrentStoreMgQuery)]
        public override async Task<CurrentRmStoreHouse> GetEntityByNo(string no)
        {
            //CheckGetPermission();
            if (string.IsNullOrEmpty(KeyFiledName))
            {
                ThrowError("NoKeyFieldName");
            }
            return await base.GetEntityByNo(no);
        }

        #endregion

        #region Hide

        ///// <summary>
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{CurrentRmStoreHouse}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<CurrentRmStoreHouse> CreateFilteredQuery(IwbPagedRequestDto input)
        //{
        //    var query = Repository.GetAll();
        //    var pagedInput = input as IIwbPagedRequest;
        //    if (pagedInput == null)
        //    {
        //        return query;
        //    }
        //    if (!string.IsNullOrEmpty(pagedInput.KeyWords))
        //    {
        //        object keyWords = pagedInput.KeyWords;
        //        LambdaObject obj = new LambdaObject()
        //        {
        //            FieldType = (LambdaFieldType)pagedInput.FieldType,
        //            FieldName = pagedInput.KeyField,
        //            FieldValue = keyWords,
        //            ExpType = (LambdaExpType)pagedInput.ExpType
        //        };
        //        var exp = obj.GetExp<CurrentRmStoreHouse>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    if (pagedInput.SearchList != null && pagedInput.SearchList.Count > 0)
        //    {
        //        List<LambdaObject> objList = new List<LambdaObject>();
        //       foreach (var o in pagedInput.SearchList)
        //        {
        //            if (string.IsNullOrEmpty(o.KeyWords))
        //                continue;
        //           object keyWords = o.KeyWords;
        //            objList.Add(new LambdaObject
        //            {
        //                FieldType = (LambdaFieldType)o.FieldType,
        //                FieldName = o.KeyField,
        //                FieldValue = keyWords,
        //                ExpType = (LambdaExpType)o.ExpType
        //            });
        //        }
        //        var exp = objList.GetExp<CurrentRmStoreHouse>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<CurrentRmStoreHouse> ApplySorting(IQueryable<CurrentRmStoreHouse> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<CurrentRmStoreHouse> ApplyPaging(IQueryable<CurrentRmStoreHouse> query, IwbPagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #endregion


        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmCurrentStoreMgQuery)]
        public async Task<PagedResultDto<ViewCurrentRmStoreHouse>> GetAllView(IwbPagedRequestDto input)
        {
            var query = ViewCurrentRmStoreHouseRepository.GetAll();
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = _ApplySorting(query, input);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ViewCurrentRmStoreHouse>(totalCount, entities);
            return dtoList;
        }

        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmCurrentStoreMgAddEnter)]
        public async Task<RmEnterStore> AddEnter(AddRmEnterStore input)
        {
            //string jsonInput = input.ToJsonString();
            //RmEnterStore enter = JsonConvert.DeserializeObject<RmEnterStore>(jsonInput);
            RmEnterStore enter = ObjectMapper.Map<RmEnterStore>(input);
            enter.Id = Guid.NewGuid().ToString("N");
            enter.ApplyQuantity = input.Quantity;
            enter.Quantity = 0;
            enter.ApplyEnterDate = Clock.Now;
            enter.ApplyStatus = RmEnterOutStatusEnum.Applying.ToInt();
            enter.CreateSourceType = CreateSourceType.Manual.ToInt();
            enter.IsClose = false;
            return await RmEnterStoreRepository.InsertAsync(enter);
        }
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmCurrentStoreMgAddOut)]
        public async Task<RmOutStore> AddOut(AddRmOutStoreDto input)
        {
            var entity =
                await Repository.FirstOrDefaultAsync(i =>
                    i.Id == input.CurrentRmStoreHouseNo);
            var canUserQuantity = entity.Quantity - entity.FreezeQuantity;
            if (canUserQuantity < input.Quantity)
            {
                CheckErrors(new IwbIdentityResult("出库数量不能大于可用数量！"));
            }
            entity.FreezeQuantity += input.Quantity ?? 0;
            RmOutStore outStore = new RmOutStore()
            {
                Id = Guid.NewGuid().ToString("N"),
                RmProductNo = entity.RmProductNo,
                StoreHouseId = entity.StoreHouseId,
                ApplyStatus = RmEnterOutStatusEnum.Applying.ToInt(),
                IsClose = false,
                IsConfirm = false,
                Quantity = input.Quantity ?? 0,
                ActualQuantity =  0,
                ApplyOutDate = Clock.Now,
                CreateSourceType = CreateSourceType.Manual.ToInt(),
                ProductBatchNum = entity.ProductBatchNum,
                CurrentRmStoreHouseNo = input.CurrentRmStoreHouseNo
            };
            await Repository.UpdateAsync(entity);
            return await RmOutStoreRepository.InsertAsync(outStore);
        }

    }
}
