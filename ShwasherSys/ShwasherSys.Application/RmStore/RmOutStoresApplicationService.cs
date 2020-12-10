using System;
using System.Collections.Generic;
using System.Linq;
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
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.RmStore.Dto;
namespace ShwasherSys.RmStore
{
    [AbpAuthorize]
    public class RmOutStoreAppService : IwbZeroAsyncCrudAppService<RmOutStore, RmOutStoreDto, string, IwbPagedRequestDto, RmOutStoreCreateDto, RmOutStoreUpdateDto >, IRmOutStoreAppService
    {
        public RmOutStoreAppService(
			ICacheManager cacheManager,
			IRepository<RmOutStore, string> repository, IRepository<ViewRmOutStore, string> viewRmOutStoreRepository, IRepository<CurrentRmStoreHouse, string> currentRmStoreHouseRepository) : base(repository, "Id")
        {
            ViewRmOutStoreRepository = viewRmOutStoreRepository;
            CurrentRmStoreHouseRepository = currentRmStoreHouseRepository;
            CacheManager = cacheManager;
        }

        protected override bool KeyIsAuto { get; set; } = false;

        protected IRepository<ViewRmOutStore,string> ViewRmOutStoreRepository { get; }

        protected IRepository<CurrentRmStoreHouse, string> CurrentRmStoreHouseRepository { get; }

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

        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreOutMg)]
        public override async Task Create(RmOutStoreCreateDto input)
        {
            await CreateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreOutMgUpdate)]
        public override async Task Update(RmOutStoreUpdateDto input)
        {
            await UpdateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreOutMgUpdate)]
        public override Task Delete(EntityDto<string> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreOutMgQuery)]
        public override async Task<PagedResultDto<RmOutStoreDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<RmOutStoreDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreOutMgQuery)]
        public override async Task<RmOutStoreDto> GetDto(EntityDto<string> input)
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
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreOutMgQuery)]
        public override async Task<RmOutStoreDto> GetDtoById(string id)
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
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreOutMgQuery)]
        public override async Task<RmOutStoreDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreOutMgQuery)]
        public override async Task<RmOutStore> GetEntity(EntityDto<string> input)
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
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreOutMgQuery)]
        public override async Task<RmOutStore> GetEntityById(string id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreOutMgQuery)]
        public override async Task<RmOutStore> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{RmOutStore}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<RmOutStore> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<RmOutStore>();
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
        //        var exp = objList.GetExp<RmOutStore>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<RmOutStore> ApplySorting(IQueryable<RmOutStore> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<RmOutStore> ApplyPaging(IQueryable<RmOutStore> query, IwbPagedRequestDto input)
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
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreOutMgQuery)]
        public async Task<PagedResultDto<ViewRmOutStore>> GetAllView(IwbPagedRequestDto input)
        {
            var query = ViewRmOutStoreRepository.GetAll();
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = _ApplySorting(query, input);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ViewRmOutStore>(totalCount, entities);
            return dtoList;
        }
        [AbpAuthorize(PermissionNames.PagesRawMaterialStoreRmStoreEnterMgUpdate, PermissionNames.PagesRawMaterialStoreRmStoreEnterMgDelete)]
        public async Task UpdateState(RwOutStatusUpdateDto input)
        {
            var entity = await Repository.GetAsync(input.Id);
            entity.ApplyStatus = input.ApplyStatus;
            entity.AuditDate = Clock.Now;
            var current = CurrentRmStoreHouseRepository.FirstOrDefault(i =>
                i.Id == entity.CurrentRmStoreHouseNo);
            if (entity.ApplyStatus == RmEnterOutStatusEnum.Stored.ToInt())
            {
                entity.ActualQuantity = input.ActualQuantity;
                entity.OutStoreDate = Clock.Now;
                entity.OutStoreUser = AbpSession.UserName;

                current.FreezeQuantity -= entity.Quantity;//减去申请时的冻结数量
                current.Quantity -= entity.ActualQuantity;
            }

            if (entity.ApplyStatus == RmEnterOutStatusEnum.Canceled.ToInt())
            {
                current.FreezeQuantity -= entity.Quantity;//减去申请时的冻结数量
            }

            await CurrentRmStoreHouseRepository.UpdateAsync(current);
            await Repository.UpdateAsync(entity);
        }
    }
}
