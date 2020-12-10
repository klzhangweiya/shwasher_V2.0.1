using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.Timing;
using IwbZero.Auditing;
using IwbZero.AppServiceBase;
using IwbZero.Helper;
using NPOI.HPSF;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.RmStore.Dto;
namespace ShwasherSys.RmStore
{
    [AbpAuthorize]
    public class RmEnterStoreAppService : IwbZeroAsyncCrudAppService<RmEnterStore, RmEnterStoreDto, string, IwbPagedRequestDto, RmEnterStoreCreateDto, RmEnterStoreUpdateDto >, IRmEnterStoreAppService
    {
        public RmEnterStoreAppService(
			ICacheManager cacheManager,
			IRepository<RmEnterStore, string> repository, IRepository<ViewRmEnterStore, string> viewRmEnterStoreRepository, IRepository<CurrentRmStoreHouse, string> currentRmStoreHouseRepository) : base(repository, "Id")
        {
            ViewRmEnterStoreRepository = viewRmEnterStoreRepository;
            CurrentRmStoreHouseRepository = currentRmStoreHouseRepository;
            CacheManager = cacheManager;
        }

        protected override bool KeyIsAuto { get; set; } = false;

        protected IRepository<ViewRmEnterStore,string> ViewRmEnterStoreRepository { get; }

        protected IRepository<CurrentRmStoreHouse,string> CurrentRmStoreHouseRepository { get; }

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

        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreEnterMgCreate)]
        public override async Task Create(RmEnterStoreCreateDto input)
        {
            input.Id =Guid.NewGuid().ToString("N");
            input.ProductBatchNum = await GetBatchNum();
            input.ApplyEnterDate = Clock.Now;
            input.ApplyStatus = RmEnterOutStatusEnum.Applying.ToInt();
            await CreateEntity(input);
        }

        private async Task<string> GetBatchNum()
        {
            string startNo = $"{DateTime.Now:yyMM}";
            var lastEntity = await Repository.GetAll().Where(a => a.ProductBatchNum.StartsWith(startNo)).OrderByDescending(a=>a.ProductBatchNum).ThenByDescending(a => a.Id)
                .FirstOrDefaultAsync();
            int noLength = 3, index = 0;
            if (lastEntity != null) 
            {
                var entityNo = lastEntity.ProductBatchNum;
                index = Convert.ToInt32(entityNo.Substring(entityNo.Length - noLength));
            }
            index++;
            string no = $"{startNo}{index.LeftPad(noLength)}";
            while ((await Repository.CountAsync(a=>a.ProductBatchNum==no)) > 0)
            {
                index++;
                no = $"{startNo}{index.LeftPad(noLength)}";
                Thread.Sleep(100);
            }
            return no;
        }
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreEnterMgUpdate)]
        public override async Task Update(RmEnterStoreUpdateDto input)
        {

            await UpdateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreEnterMgDelete)]
        public override Task Delete(EntityDto<string> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreEnterMgQuery)]
        public override async Task<PagedResultDto<RmEnterStoreDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<RmEnterStoreDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreEnterMgQuery)]
        public override async Task<RmEnterStoreDto> GetDto(EntityDto<string> input)
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
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreEnterMgQuery)]
        public override async Task<RmEnterStoreDto> GetDtoById(string id)
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
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreEnterMgQuery)]
        public override async Task<RmEnterStoreDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreEnterMgQuery)]
        public override async Task<RmEnterStore> GetEntity(EntityDto<string> input)
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
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreEnterMgQuery)]
        public override async Task<RmEnterStore> GetEntityById(string id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreEnterMgQuery)]
        public override async Task<RmEnterStore> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{RmEnterStore}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<RmEnterStore> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<RmEnterStore>();
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
        //        var exp = objList.GetExp<RmEnterStore>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<RmEnterStore> ApplySorting(IQueryable<RmEnterStore> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<RmEnterStore> ApplyPaging(IQueryable<RmEnterStore> query, IwbPagedRequestDto input)
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
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreEnterMgQuery)]
        public  async Task<PagedResultDto<ViewRmEnterStore>> GetAllView(IwbPagedRequestDto input)
        {
            var query = ViewRmEnterStoreRepository.GetAll();
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = _ApplySorting(query, input);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ViewRmEnterStore>(totalCount, entities);
            return dtoList;
        }
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreEnterMgUpdate, PermissionNames.PagesRawMaterialStoreRmStoreEnterMgDelete)]
        public async Task UpdateState(RwEnterStatusUpdateDto input)
        {
            var entity = await Repository.GetAsync(input.Id);
            entity.ApplyStatus = input.ApplyStatus;
            if (entity.ApplyStatus == RmEnterOutStatusEnum.Stored.ToInt())
            {
                entity.Quantity = input.Quantity;
                entity.EnterStoreDate = Clock.Now;
                entity.EnterStoreUser = AbpSession.UserName;
                entity.AuditDate = Clock.Now;
               var isExist =  CurrentRmStoreHouseRepository.FirstOrDefault(i =>
                    i.ProductBatchNum == entity.ProductBatchNum && i.StoreLocationNo == entity.StoreLocationNo&&i.StoreHouseId == entity.StoreHouseId);
               if (isExist!=null)
               {
                   isExist.Quantity += entity.Quantity;
                   await CurrentRmStoreHouseRepository.UpdateAsync(isExist);
               }
               else
               {
                   CurrentRmStoreHouse crsh = new CurrentRmStoreHouse()
                   {
                       Id = Guid.NewGuid().ToString("N"),
                       RmProductNo = entity.RmProductNo,
                       Quantity = entity.Quantity,
                       StoreLocationNo = entity.StoreLocationNo,
                       ProductionOrderNo = entity.ProductionOrderNo,
                       ProductBatchNum = entity.ProductBatchNum,
                       FreezeQuantity = 0,
                       StoreHouseId = entity.StoreHouseId
                   };
                   CurrentRmStoreHouseRepository.Insert(crsh);
               }
            }
            await Repository.UpdateAsync(entity);
        }
    }
}
