﻿using System.Collections.Generic;
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
using ShwasherSys.BasicInfo.ScrapTypeInfo.Dto;
namespace ShwasherSys.BasicInfo.ScrapTypeInfo
{
    [AbpAuthorize, AuditLog("报废类型维护")]
    public class ScrapTypeAppService : IwbZeroAsyncCrudAppService<ScrapType, ScrapTypeDto, string, IwbPagedRequestDto, ScrapTypeCreateDto, ScrapTypeUpdateDto >, IScrapTypeAppService
    {
        public ScrapTypeAppService(
			ICacheManager cacheManager,
			IRepository<ScrapType, string> repository) : base(repository, "Id")
        {
            CacheManager = cacheManager;
        }

        protected override bool KeyIsAuto { get; set; } = true;

        #region GetSelect

        [DisableAuditing]
        public override async Task<List<SelectListItem>> GetSelectList()
        {
            var list = await Repository.GetAllListAsync();
            var sList = new List<SelectListItem> {new SelectListItem {Text = @"请选择报废类型...", Value = "", Selected = true}};
            foreach (var l in list)
            {
                sList.Add(new SelectListItem { Value = l.Id, Text = l.Name });
            }
            return sList;
        }
        [DisableAuditing]
        public override async Task<string> GetSelectStr()
        {
            var list = await Repository.GetAllListAsync();
            string str = "<option value=\"\" selected>请选择报废类型...</option>";
            foreach (var l in list)
            {
                str += $"<option value=\"{l.Id}\">{l.Name}</option>";
            }
            return str;
        }

        #endregion

        #region CURD

        [AbpAuthorize(PermissionNames.PagesBasicInfoScrapTypeCreate)]
        public override async Task Create(ScrapTypeCreateDto input)
        {
            await CreateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesBasicInfoScrapTypeUpdate)]
        public override async Task Update(ScrapTypeUpdateDto input)
        {
            await UpdateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesBasicInfoScrapTypeDelete)]
        public override Task Delete(EntityDto<string> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesBasicInfoScrapTypeQuery)]
        public override async Task<PagedResultDto<ScrapTypeDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ScrapTypeDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesBasicInfoScrapTypeQuery)]
        public override async Task<ScrapTypeDto> GetDto(EntityDto<string> input)
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
        [AbpAuthorize(PermissionNames.PagesBasicInfoScrapTypeQuery)]
        public override async Task<ScrapTypeDto> GetDtoById(string id)
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
        [AbpAuthorize(PermissionNames.PagesBasicInfoScrapTypeQuery)]
        public override async Task<ScrapTypeDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesBasicInfoScrapTypeQuery)]
        public override async Task<ScrapType> GetEntity(EntityDto<string> input)
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
        [AbpAuthorize(PermissionNames.PagesBasicInfoScrapTypeQuery)]
        public override async Task<ScrapType> GetEntityById(string id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesBasicInfoScrapTypeQuery)]
        public override async Task<ScrapType> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{ScrapType}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<ScrapType> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<ScrapType>();
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
        //        var exp = objList.GetExp<ScrapType>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<ScrapType> ApplySorting(IQueryable<ScrapType> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<ScrapType> ApplyPaging(IQueryable<ScrapType> query, IwbPagedRequestDto input)
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