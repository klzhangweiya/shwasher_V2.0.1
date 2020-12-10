using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using IwbZero.Auditing;
using IwbZero.AppServiceBase;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.ScrapStore.Dto;
namespace ShwasherSys.ScrapStore
{
    [AbpAuthorize]
    public class ScrapEnterStoreAppService : IwbZeroAsyncCrudAppService<ScrapEnterStore, ScrapEnterStoreDto, string, IwbPagedRequestDto, ScrapEnterStoreCreateDto, ScrapEnterStoreUpdateDto >, IScrapEnterStoreAppService
    {
        public ScrapEnterStoreAppService(
			ICacheManager cacheManager,
			IRepository<ScrapEnterStore, string> repository, IRepository<ViewScrapEnterStore, string> viewScrapEnterStoreRepository) : base(repository, "Id")
        {
            ViewScrapEnterStoreRepository = viewScrapEnterStoreRepository;
            CacheManager = cacheManager;
        }

        protected override bool KeyIsAuto { get; set; } = false;

        protected IRepository<ViewScrapEnterStore,string> ViewScrapEnterStoreRepository { get; set; }

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

        [AbpAuthorize(PermissionNames.PagesScrapStoreScrapStoreEnterMgQuery)]
        public override async Task Create(ScrapEnterStoreCreateDto input)
        {
            await CreateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesScrapStoreScrapStoreEnterMgUpdate)]
        public override async Task Update(ScrapEnterStoreUpdateDto input)
        {
            await UpdateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesScrapStoreScrapStoreEnterMgDelete)]
        public override Task Delete(EntityDto<string> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesScrapStoreScrapStoreEnterMgQuery)]
        public override async Task<PagedResultDto<ScrapEnterStoreDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ScrapEnterStoreDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesScrapStoreScrapStoreEnterMgQuery)]
        public  async Task<PagedResultDto<ViewScrapEnterStore>> GetViewAll(IwbPagedRequestDto input)
        {
            var query = ViewScrapEnterStoreRepository.GetAll();
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = _ApplySorting(query, input);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ViewScrapEnterStore>(totalCount, entities);
            return dtoList;
        }
        #region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesScrapStoreScrapStoreEnterMgQuery)]
        public override async Task<ScrapEnterStoreDto> GetDto(EntityDto<string> input)
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
        [AbpAuthorize(PermissionNames.PagesScrapStoreScrapStoreEnterMgQuery)]
        public override async Task<ScrapEnterStoreDto> GetDtoById(string id)
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
        [AbpAuthorize(PermissionNames.PagesScrapStoreScrapStoreEnterMgQuery)]
        public override async Task<ScrapEnterStoreDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesScrapStoreScrapStoreEnterMgQuery)]
        public override async Task<ScrapEnterStore> GetEntity(EntityDto<string> input)
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
        [AbpAuthorize(PermissionNames.PagesScrapStoreScrapStoreEnterMgQuery)]
        public override async Task<ScrapEnterStore> GetEntityById(string id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesScrapStoreScrapStoreEnterMgQuery)]
        public override async Task<ScrapEnterStore> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{ScrapEnterStore}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<ScrapEnterStore> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<ScrapEnterStore>();
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
        //        var exp = objList.GetExp<ScrapEnterStore>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<ScrapEnterStore> ApplySorting(IQueryable<ScrapEnterStore> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<ScrapEnterStore> ApplyPaging(IQueryable<ScrapEnterStore> query, IwbPagedRequestDto input)
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
