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
using ShwasherSys.CompanyInfo.LicenseInfo.Dto;
namespace ShwasherSys.CompanyInfo.LicenseInfo
{
    [AbpAuthorize, AuditLog("证照类型")]
    public class LicenseTypeAppService : IwbZeroAsyncCrudAppService<LicenseType, LicenseTypeDto, int, IwbPagedRequestDto, LicenseTypeCreateDto, LicenseTypeUpdateDto >, ILicenseTypeAppService
    {
        public LicenseTypeAppService(
			ICacheManager cacheManager,
			IRepository<LicenseType, int> repository) : base(repository)
        {
            CacheManager = cacheManager;
        }

        protected override bool KeyIsAuto { get; set; } = false;

        #region GetSelect

        [DisableAuditing]
        public override async Task<List<SelectListItem>> GetSelectList()
        {
            var list = await Repository.GetAllListAsync();
            var sList = new List<SelectListItem> {new SelectListItem {Text = @"请选择证照类型...", Value = "", Selected = true}};
            foreach (var l in list)
            {
                sList.Add(new SelectListItem { Value = l.Name, Text = l.Name });
            }
            return sList;
        }
        [DisableAuditing]
        public override async Task<string> GetSelectStr()
        {
            var list = await Repository.GetAllListAsync();
            string str = "<option value=\"\" selected>请选择证照类型...</option>";
            foreach (var l in list)
            {
                str += $"<option value=\"{l.Name}\">{l.Name}</option>";
            }
            return str;
        }

        [DisableAuditing]
        public  async Task<List<SelectListItem>> GetSelectListByGroup(string groupName)
        {
            var list = await Repository.GetAllListAsync(a=>a.GroupName==groupName);
            var sList = new List<SelectListItem> {new SelectListItem {Text = @"请选择证照类型...", Value = "", Selected = true}};
            foreach (var l in list)
            {
                sList.Add(new SelectListItem { Value = l.Name, Text = l.Name });
            }
            return sList;
        }
        [DisableAuditing]
        public  async Task<string> GetSelectStrByGroup(string groupName)
        {
            var list = await Repository.GetAllListAsync(a=>a.GroupName==groupName);
            string str = "<option value=\"\" selected>请选择证照类型...</option>";
            foreach (var l in list)
            {
                str += $"<option value=\"{l.Name}\">{l.Name}</option>";
            }
            return str;
        }

        #endregion

        #region CURD

        [AbpAuthorize(PermissionNames.PagesBasicInfoLicenseTypeCreate)]
        public override async Task Create(LicenseTypeCreateDto input)
        {
            await CreateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesBasicInfoLicenseTypeUpdate)]
        public override async Task Update(LicenseTypeUpdateDto input)
        {
            await UpdateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesBasicInfoLicenseTypeDelete)]
        public override Task Delete(EntityDto<int> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesBasicInfoLicenseTypeQuery)]
        public override async Task<PagedResultDto<LicenseTypeDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<LicenseTypeDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesBasicInfoLicenseTypeQuery)]
        public override async Task<LicenseTypeDto> GetDto(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesBasicInfoLicenseTypeQuery)]
        public override async Task<LicenseTypeDto> GetDtoById(int id)
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
        [AbpAuthorize(PermissionNames.PagesBasicInfoLicenseTypeQuery)]
        public override async Task<LicenseTypeDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesBasicInfoLicenseTypeQuery)]
        public override async Task<LicenseType> GetEntity(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesBasicInfoLicenseTypeQuery)]
        public override async Task<LicenseType> GetEntityById(int id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesBasicInfoLicenseTypeQuery)]
        public override async Task<LicenseType> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{LicenseType}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<LicenseType> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<LicenseType>();
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
        //        var exp = objList.GetExp<LicenseType>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<LicenseType> ApplySorting(IQueryable<LicenseType> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<LicenseType> ApplyPaging(IQueryable<LicenseType> query, IwbPagedRequestDto input)
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
