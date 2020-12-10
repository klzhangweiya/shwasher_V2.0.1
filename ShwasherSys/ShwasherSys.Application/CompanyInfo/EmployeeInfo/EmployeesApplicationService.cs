using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.Authorization.Users;
using ShwasherSys.CompanyInfo.EmployeeInfo.Dto;

namespace ShwasherSys.CompanyInfo.EmployeeInfo
{
    [AbpAuthorize, AuditLog("人员信息维护")]
    public class EmployeeAppService : IwbZeroAsyncCrudAppService<Employee, EmployeeDto, int, IwbPagedRequestDto, EmployeeCreateDto, EmployeeUpdateDto >, IEmployeeAppService
    {
        public EmployeeAppService(
			ICacheManager cacheManager,
			IRepository<Employee, int> repository, IRepository<SysUser, long> userRepository) : base(repository, "No")
        {
            UserRepository = userRepository;
            CacheManager = cacheManager;
        }
        private IRepository<SysUser,long> UserRepository { get; }

        protected override bool KeyIsAuto { get; set; } = false;

        #region GetSelect

        [DisableAuditing]
        public override async Task<List<SelectListItem>> GetSelectList()
        {
            var list = await Repository.GetAllListAsync();
            var sList = new List<SelectListItem>();// {new SelectListItem {Text = @"请选择人员...", Value = "", Selected = true}};
            foreach (var l in list)
            {
                sList.Add(new SelectListItem { Value = l.Id+"", Text = l.Name });
            }
            return sList;
        }
        [DisableAuditing]
        public override async Task<string> GetSelectStr()
        {
            var list = await Repository.GetAllListAsync();
            string str = "";//"<option value=\"\" selected>请选择...</option>";
            foreach (var l in list)
            {
                str += $"<option value=\"{l.Id}\">{l.Name}</option>";
            }
            return str;
        }
        [DisableAuditing]
        public  async Task<string> GetSelectStr2(Expression<Func<Employee,bool>> predicate=null)
        {
            var list = predicate==null?await Repository.GetAllListAsync(): await Repository.GetAllListAsync(predicate);
            string str = "";//"<option value=\"\" selected>请选择...</option>";
            foreach (var l in list)
            {
                str += $"<option value=\"{l.Id}\">{l.Name}</option>";
            }
            return str;
        }
        #endregion

        #region CURD

        [AbpAuthorize(PermissionNames.PagesCompanyEmployeeCreate)]
        public override async Task Create(EmployeeCreateDto input)
        {
           
            var entity = await CreateEntity(input);
            //await CacheManager.GetCache(ShwasherConsts.EmployeeCache)
            //    .SetAsync(input.No, ObjectMapper.Map<Employee>(entity), null, null);
        }

        [AbpAuthorize(PermissionNames.PagesCompanyEmployeeUpdate)]
        public override async Task Update(EmployeeUpdateDto input)
        {
            var entity = await UpdateEntity(input);
            //await CacheManager.GetCache(ShwasherConsts.EmployeeCache)
            //    .SetAsync(input.No, ObjectMapper.Map<Employee>(entity), null, null);
        }

        [AbpAuthorize(PermissionNames.PagesCompanyEmployeeDelete)]
        public override Task Delete(EntityDto<int> input)
        {
            //var entity = Repository.Get(input.Id);
            //CacheManager.GetCache(ShwasherConsts.EmployeeCache).RemoveAsync(entity.No);
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeeQuery)]
        public override async Task<PagedResultDto<EmployeeDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
          
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<EmployeeDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeeQuery)]
        public override async Task<EmployeeDto> GetDto(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeeQuery)]
        public override async Task<EmployeeDto> GetDtoById(int id)
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
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeeQuery)]
        public override async Task<EmployeeDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeeQuery)]
        public override async Task<Employee> GetEntity(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeeQuery)]
        public override async Task<Employee> GetEntityById(int id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeeQuery)]
        public override async Task<Employee> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{Employee}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<Employee> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<Employee>();
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
        //        var exp = objList.GetExp<Employee>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<Employee> ApplySorting(IQueryable<Employee> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<Employee> ApplyPaging(IQueryable<Employee> query, IwbPagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #endregion

        
        #region Ex

        [DisableAuditing]
        public async Task<string> GetAccountUser()
        {
            var use = Repository.GetAll().Where(a => !string.IsNullOrEmpty(a.UserName))
                .Select(a => a.UserName).ToList();
            var list = await UserRepository.GetAllListAsync(a => a.IsActive && a.UserType >1 && !use.Contains(a.UserName));
            string str = "<option value=\"\" selected>请选择账号...</option>";
            foreach (var l in list)
            {
                str += $"<option value=\"{l.UserName}\">{l.UserName}</option>";
            }
            return str;
        }

        [AbpAuthorize(PermissionNames.PagesCompanyEmployeeBind), AuditLog("绑定账号")]
        public async Task Bind(AccountDto input)
        {
            var entity = await GetEntityById(input.Id);
            if (!string.IsNullOrEmpty(entity.UserName))
            {
                CheckErrors("账号已被其它用户绑定，请先解绑账号！");
            }

            //var entity =await GetEntityById(input.Id);
            entity.UserName = input.UserName;
            await Repository.UpdateAsync(entity);
        }
        [AbpAuthorize(PermissionNames.PagesCompanyEmployeeUnBind), AuditLog("解绑账号")]
        public async Task UnBind(EntityDto<int> input)
        {
            var entity = await GetEntityById(input.Id);
            entity.UserName = "";
            await Repository.UpdateAsync(entity);
        } 
        #endregion

    }
}
