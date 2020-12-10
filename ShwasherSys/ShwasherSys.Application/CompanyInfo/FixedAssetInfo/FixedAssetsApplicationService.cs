using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.Helper;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.CompanyInfo.FixedAssetInfo.Dto;

namespace ShwasherSys.CompanyInfo.FixedAssetInfo
{
    [AbpAuthorize, AuditLog("设备固定资产维护")]
    public class FixedAssetAppService : IwbZeroAsyncCrudAppService<FixedAsset, FixedAssetDto, int, IwbPagedRequestDto, FixedAssetCreateDto, FixedAssetUpdateDto >, IFixedAssetAppService
    {
        public FixedAssetAppService(
			ICacheManager cacheManager,
			IRepository<FixedAsset, int> repository) : base(repository, "No")
        {
            CacheManager = cacheManager;
        }

        protected override bool KeyIsAuto { get; set; } = false;

        #region GetSelect

        [DisableAuditing]
        public override async Task<List<SelectListItem>> GetSelectList()
        {
            var list = await Repository.GetAllListAsync();
            var sList = new List<SelectListItem> {new SelectListItem {Text = @"请选择设备固定资产...", Value = "", Selected = true}};
            foreach (var l in list)
            {
                sList.Add(new SelectListItem { Value = l.No, Text = l.Name });
            }
            return sList;
        }
        [DisableAuditing]
        public override async Task<string> GetSelectStr()
        {
            var list = await Repository.GetAllListAsync();
            string str = "<option value=\"\" selected>请选择设备固定资产...</option>";
            foreach (var l in list)
            {
                str += $"<option value=\"{l.No}\">{l.Name}</option>";
            }
            return str;
        }
        [DisableAuditing]
        public  async Task<List<SelectListItem>> GetSelectListName()
        {
            var list = await Repository.GetAllListAsync();
            var sList = new List<SelectListItem> {new SelectListItem {Text = @"请选择设备固定资产...", Value = "", Selected = true}};
            foreach (var l in list)
            {
                sList.Add(new SelectListItem { Value = l.Name, Text = l.Name });
            }
            sList.Add(new SelectListItem {Text = @"其他", Value = "其他"});
            return sList;
        }
        [DisableAuditing]
        public  async Task<string> GetSelectStrName()
        {
            var list = await Repository.GetAllListAsync();
            string str = "<option value=\"\" selected>请选择设备固定资产...</option>";
            foreach (var l in list)
            {
                str += $"<option value=\"{l.Name}\">{l.Name}</option>";
            }
            str+= $"<option value=\"其他\">其他</option>";
            return str;
        }

        #endregion

        #region CURD

        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceFixedAsset)]
        public override async Task Create(FixedAssetCreateDto input)
        {
            input.No = await MaintainTypeDefinition.GetDeviceNo(Repository);
            await CreateEntity(input);
        }
        private  async Task<string>  GetFixedAssetNo()
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var lastEntity = await Repository.GetAll().OrderByDescending(a => a.CreationTime)
                    .FirstOrDefaultAsync();
                int noLength = 4, index = 0;
                if (lastEntity != null) 
                {
                    var entityNo = lastEntity.No;
                    int.TryParse(entityNo.Substring(entityNo.Length - noLength), out index);
                }
                index++;
                string no = $"SHSD-{index.LeftPad(noLength)}";
                if ((await Repository.CountAsync(a=>a.No==no)) > 0) 
                {
                    no = await GetFixedAssetNo();
                }
                return no;
            }
            
        }
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceFixedAssetUpdate)]
        public override async Task Update(FixedAssetUpdateDto input)
        {
            await UpdateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceFixedAssetDelete)]
        public override Task Delete(EntityDto<int> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceFixedAssetQuery)]
        public override async Task<PagedResultDto<FixedAssetDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<FixedAssetDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceFixedAssetQuery)]
        public override async Task<FixedAssetDto> GetDto(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceFixedAssetQuery)]
        public override async Task<FixedAssetDto> GetDtoById(int id)
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
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceFixedAssetQuery)]
        public override async Task<FixedAssetDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceFixedAssetQuery)]
        public override async Task<FixedAsset> GetEntity(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceFixedAssetQuery)]
        public override async Task<FixedAsset> GetEntityById(int id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceFixedAssetQuery)]
        public override async Task<FixedAsset> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{FixedAsset}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<FixedAsset> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<FixedAsset>();
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
        //        var exp = objList.GetExp<FixedAsset>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<FixedAsset> ApplySorting(IQueryable<FixedAsset> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<FixedAsset> ApplyPaging(IQueryable<FixedAsset> query, IwbPagedRequestDto input)
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
