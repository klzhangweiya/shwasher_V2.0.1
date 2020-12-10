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
using Abp.Runtime.Caching;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.CompanyInfo.Performance.Dto;
using ShwasherSys.Lambda;

namespace ShwasherSys.CompanyInfo.Performance
{
    [AbpAuthorize]
    public class PerformanceAppService : IwbZeroAsyncCrudAppService<EmployeeWorkPerformance, PerformanceDto, int, IwbPagedRequestDto, PerformanceCreateDto, PerformanceUpdateDto >, IPerformanceAppService
    {
        public PerformanceAppService(
			ICacheManager cacheManager,
			IRepository<EmployeeWorkPerformance, int> repository) : base(repository, "PerformanceNo")
        {
            CacheManager = cacheManager;
        }

        protected override bool KeyIsAuto { get; set; } = false;

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

       // [AbpAuthorize(PermissionNames.PagesCompanyEmployeePerformanceCreate)]
        public override async Task Create(PerformanceCreateDto input)
        {
            await CreateEntity(input);
        }

       // [AbpAuthorize(PermissionNames.PagesCompanyEmployeePerformanceUpdate)]
        public override async Task Update(PerformanceUpdateDto input)
        {
            await UpdateEntity(input);
        }

       // [AbpAuthorize(PermissionNames.PagesCompanyEmployeePerformanceDelete)]
        public override Task Delete(EntityDto<int> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeePerformanceQuery)]
        public override async Task<PagedResultDto<PerformanceDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<PerformanceDto>(totalCount, entities.Select(a=>new PerformanceDto()
            {
                Id = a.Id,
                PerformanceNo = a.PerformanceNo,
                ProductOrderNo = a.ProductOrderNo,
                EmployeeId = a.EmployeeId,
                EmployeeNo = a.EmployeeInfo.No,
                EmployeeName = a.EmployeeInfo.Name,
                Performance = a.Performance??0,
                PerformanceUnit = a.PerformanceUnit,
                PerformanceDesc = a.PerformanceDesc,
                WorkType = a.WorkType,
                RelatedNo = a.RelatedNo,
                CreationTime =a.CreationTime
            }).ToList());
            return dtoList;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeePerformanceQuery)]
        public override async Task<PerformanceDto> GetDto(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeePerformanceQuery)]
        public override async Task<PerformanceDto> GetDtoById(int id)
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
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeePerformanceQuery)]
        public override async Task<PerformanceDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeePerformanceQuery)]
        public override async Task<EmployeeWorkPerformance> GetEntity(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeePerformanceQuery)]
        public override async Task<EmployeeWorkPerformance> GetEntityById(int id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeePerformanceQuery)]
        public override async Task<EmployeeWorkPerformance> GetEntityByNo(string no)
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

        /// <summary>
        /// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{EmployeeWorkPerformance}"/>过滤查询.
        /// </summary>
        /// <param name="input">The input.</param>
        protected override IQueryable<EmployeeWorkPerformance> CreateFilteredQuery(IwbPagedRequestDto input)
        {
            var query = Repository.GetAllIncluding(a=>a.EmployeeInfo);
            var pagedInput = input as IIwbPagedRequest;
            if (pagedInput == null)
            {
                return query;
            }
            if (!string.IsNullOrEmpty(pagedInput.KeyWords))
            {
                object keyWords = pagedInput.KeyWords;
                LambdaObject obj = new LambdaObject()
                {
                    FieldType = (LambdaFieldType)pagedInput.FieldType,
                    FieldName = pagedInput.KeyField,
                    FieldValue = keyWords,
                    ExpType = (LambdaExpType)pagedInput.ExpType
                };
                var exp = obj.GetExp<EmployeeWorkPerformance>();
                query = exp != null ? query.Where(exp) : query;
            }
            if (pagedInput.SearchList != null && pagedInput.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in pagedInput.SearchList)
                {
                    if (string.IsNullOrEmpty(o.KeyWords))
                        continue;
                    if (o.KeyField.ToLower() == "employeeno")
                    {
                        query = query.Where(a => a.EmployeeInfo.No.Contains(o.KeyWords));
                        continue;
                    }
                    if (o.KeyField.ToLower() == "employeename")
                    {
                        query = query.Where(a => a.EmployeeInfo.Name.Contains(o.KeyWords));
                        continue;
                    }
                    object keyWords = o.KeyWords;
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType
                    });
                }
                var exp = objList.GetExp<EmployeeWorkPerformance>();
                query = exp != null ? query.Where(exp) : query;
            }
            return query;
        }



        //protected override IQueryable<EmployeeWorkPerformance> ApplySorting(IQueryable<EmployeeWorkPerformance> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<EmployeeWorkPerformance> ApplyPaging(IQueryable<EmployeeWorkPerformance> query, IwbPagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #endregion

        [AuditLog("员工绩效查询")]
        public async Task<string> PerformanceTotalQuery(PerformanceTotalQueryDto input)
        {
            DateTime startDate, endDate;
            if (input.Month == null)
            {
                startDate= new DateTime(input.Year,1,1);
                endDate = startDate.AddYears(1);
            }
            else
            {
                startDate= new DateTime(input.Year,input.Month??0,1);
                endDate = startDate.AddMonths(1);
            }

            var query = Repository.GetAll().Where(a =>
                a.EmployeeId == input.EmployeeId && a.WorkType == input.WorkType && a.CreationTime >= startDate &&
                a.CreationTime < endDate);
            var total =await query.SumAsync(a => a.Performance);
            return total + "";
        }

       
    }
}
