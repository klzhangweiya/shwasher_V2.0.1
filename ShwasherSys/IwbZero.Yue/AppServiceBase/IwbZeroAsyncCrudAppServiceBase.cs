using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Logging;
using Abp.Runtime.Caching;
using Abp.UI;
using IwbZero.Authorization.Permissions;
using IwbZero.Helper;
using IwbZero.IdentityFramework;
using IwbZero.Session;
using IwbZero.Setting;
using Microsoft.AspNet.Identity;

namespace IwbZero.AppServiceBase
{

    public abstract class IwbZeroAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
      : ApplicationService,
          IIwbZeroAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
          where TEntity : class, IEntity<TPrimaryKey>
          where TEntityDto : IEntityDto<TPrimaryKey>
          where TUpdateInput : IEntityDto<TPrimaryKey>
          where TGetInput : IEntityDto<TPrimaryKey>
          where TDeleteInput : IEntityDto<TPrimaryKey>
    {
       
        protected readonly IRepository<TEntity, TPrimaryKey> Repository;
        protected IwbZeroAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository,string keyFiledName=null)
           
        {
            KeyFiledName = keyFiledName;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            LocalizationSourceName = IwbZeroConsts.IwbZeroLocalizationSourceName;
            Repository = repository;
        }

        #region 字段、属性
        
        public new IIwbPermissionManager PermissionManager { protected get; set; }
        public new IIwbSettingManager SettingManager { get; set; }
        public new IIwbSession AbpSession { get; set; }
        public ICacheManager CacheManager { get; set; }

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        /// <summary>
        /// 自定义唯一字段名称（主键）
        /// </summary>
        protected string KeyFiledName { get; set; }
        /// <summary>
        /// 是否自动生成自定义字段（GUID）
        /// </summary>
        protected virtual bool KeyIsAuto { get; set; } = true;
        /// <summary>
        /// 自定义唯一字段存在提示信息
        /// </summary>
        protected virtual string KeyExistMessage { get; set; }

        //protected virtual string ExistMessage => string.IsNullOrEmpty(KeyExistMessage) ? L("KeyExistMessage") : KeyExistMessage;
        protected virtual string ExistMessage => string.IsNullOrEmpty(KeyExistMessage) ?"编号已存在，请检查后再操作！" : KeyExistMessage;
        /// <summary>
        /// 自定义唯一字段不存在提示信息
        /// </summary>
        protected virtual string KeyNotExistMessage { get; set; }
        //protected virtual string NotExistMessage => string.IsNullOrEmpty(KeyNotExistMessage) ? L("KeyNotExistMessage") : KeyNotExistMessage;
        protected virtual string NotExistMessage => string.IsNullOrEmpty(KeyNotExistMessage) ? "编号不存在，请检查后再操作！" : KeyNotExistMessage;


        protected virtual string GetPermissionName { get; set; }
        protected virtual string GetAllPermissionName { get; set; }
        protected virtual string CreatePermissionName { get; set; }
        protected virtual string UpdatePermissionName { get; set; }
        protected virtual string DeletePermissionName { get; set; }

        #endregion

        #region SelectList


        /// <summary>
        /// 获取 Function 下拉框选项
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public virtual async Task<List<SelectListItem>> GetSelectList()
        {
            var list = await Repository.GetAllListAsync();
            var sList = new List<SelectListItem>();
            foreach (var l in list)
            {
                sList.Add(new SelectListItem() { Value = $"{l.Id}", Text = $"{l.Id}" });
            }

            return sList;
        }

        /// <summary>
        /// 获取 Function 下拉框选项
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public virtual async Task<string> GetSelectStr()
        {
            var list = await Repository.GetAllListAsync();
            string options = "";
            foreach (var l in list)
            {
                options += "<option value=\"" + $"{l.Id}" + "\">" + $"{l.Id}" + "</option>";
            }

            return options;
        }

        #endregion

        #region CURD

        #region Query

        #region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        public virtual async Task<TEntityDto> GetDto(TGetInput input)
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
        public virtual async Task<TEntityDto> GetDtoById(TPrimaryKey id)
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
        public virtual async Task<TEntityDto> GetDtoByNo(string no)
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
        public virtual async Task<TEntity> GetEntity(TGetInput input)
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
        public virtual async Task<TEntity> GetEntityById(TPrimaryKey id)
        {
            
            var exp = LambdaHelper.CreatePrimaryKeyExp<TEntity, TPrimaryKey>(id);
            if (exp == null)
            {
                CheckErrors(string.Format(L("NoEntity"),id));
            }
            return await Repository.FirstOrDefaultAsync(exp);
        }
        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        public virtual async Task<TEntity> GetEntityByNo(string no)
        {
            //CheckGetPermission();
            if (string.IsNullOrEmpty(KeyFiledName))
            {
                ThrowError("NoKeyFieldName");
            }
            //LambdaObject obj = new LambdaObject()
            //{
            //    FieldType = LambdaFieldType.S,
            //    FieldName = KeyFiledName,
            //    FieldValue = no,
            //    ExpType = LambdaExpType.Equal
            //};
            //var exp = obj.GetExp<TEntity>();
            var exp = LambdaHelper.CreateFieldExp<TEntity>(KeyFiledName,no);
            return exp == null ? null : await Repository.FirstOrDefaultAsync(exp);
        }

        #endregion

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        public virtual async Task<PagedResultDto<TEntityDto>> GetAll(TGetAllInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<TEntityDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetAllInput input)
        {
            ////Try to sort query if available
            //var sortInput = input as ISortedResultRequest;
            //if (sortInput != null)
            //{
            //    if (!sortInput.Sorting.IsNullOrWhiteSpace())
            //    {
            //        return query.OrderBy(sortInput.Sorting);
            //    }
            //}

            ////IQueryable.Task requires sorting, so we should sort if Take will be used.
            //if (input is ILimitedResultRequest)
            //{
            //    return query.OrderBy("CreationTime DESC");
            //}

            //No sorting
            return _ApplySorting(query,input);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected  IQueryable<T> _ApplySorting<T>(IQueryable<T> query, TGetAllInput input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput != null)
            {
                if (!sortInput.Sorting.IsNullOrWhiteSpace())
                {
                    return query.OrderBy(sortInput.Sorting);
                }
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderBy("CreationTime DESC");
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetAllInput input)
        {
            ////Try to use paging if available
            //var pagedInput = input as IPagedResultRequest;
            //if (pagedInput != null)
            //{
            //    return query.PageBy(pagedInput);
            //}

            ////Try to limit query result if available
            //var limitedInput = input as ILimitedResultRequest;
            //if (limitedInput != null)
            //{
            //    return query.Take(limitedInput.MaxResultCount);
            //}

            return _ApplyPaging(query,input);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected  IQueryable<T> _ApplyPaging<T>(IQueryable<T> query, TGetAllInput input)
        {
            //Try to use paging if available
            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            var limitedInput = input as ILimitedResultRequest;
            if (limitedInput != null)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        /// <summary>
        /// 根据给定的<see cref="TGetAllInput"/>创建 <see cref="IQueryable{TEntity}"/>过滤查询.
        /// </summary>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> CreateFilteredQuery(TGetAllInput input)
        {
            var query =  ApplyFilter(input);
            return query;
        }

        /// <summary>
        /// 过滤查询条件
        /// </summary>
        protected virtual IQueryable<TEntity> ApplyFilter(TGetAllInput input)
        {
            var query = Repository.GetAll();
            return ApplyFilter(query, input);
        }

        /// <summary>
        /// 过滤查询条件
        /// </summary>
        protected IQueryable<T> ApplyFilter<T>(IQueryable<T> query, TGetAllInput input)
        {
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
                var exp = obj.GetExp<T>();
                query = exp != null ? query.Where(exp) : query;
            }
            if (pagedInput.SearchList != null && pagedInput.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in pagedInput.SearchList)
                {
                    if (string.IsNullOrEmpty(o.KeyWords))
                        continue;
                    object keyWords = o.KeyWords;
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType
                    });
                }
                var exp = objList.GetExp<T>();
                query = exp != null ? query.Where(exp) : query;
            }
            return query;
        }

        #endregion

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task Create(TCreateInput input)
        {
            CheckCreatePermission();
            await CreateEntity(input);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected async Task<TEntityDto> CreateEntity(TCreateInput input)
        {
            if (!KeyFiledName.IsNullOrEmpty())
            {
                var fieldValue = input.GetFiledValue<string>(KeyFiledName);
                if (KeyIsAuto && fieldValue.IsNullOrEmpty())
                {
                    input.SetFiledValue(KeyFiledName, Guid.NewGuid().ToString("N"));
                }
                else
                {
                    //LambdaObject obj = new LambdaObject()
                    //{
                    //    FieldType = LambdaFieldType.S,
                    //    FieldName = KeyFiledName,
                    //    FieldValue = input.GetFiledValue<string>(KeyFiledName),
                    //    ExpType = LambdaExpType.Equal
                    //};
                    //var exp = obj.GetExp<TEntity>();
                    var exp = LambdaHelper.CreateFieldExp<TEntity>(KeyFiledName,fieldValue);
                    if (await Repository.FirstOrDefaultAsync(exp) != null)
                    {
                        CheckErrors(ExistMessage);
                    }
                }

            }
            var entity = MapToEntity(input);
            await Repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task Update(TUpdateInput input)
        {
            CheckUpdatePermission();
            await UpdateEntity(input);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task<TEntityDto> UpdateEntity(TUpdateInput input, TEntity entity = null)
        {
            entity = entity ?? await GetEntityById(input.Id);
            if (entity == null)
            {
                CheckErrors(NotExistMessage);
            }
            MapToEntity(input, entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual Task Delete(TDeleteInput input)
        {
            CheckDeletePermission();
            return Repository.DeleteAsync(input.Id);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isCheckNull"></param>
        /// <returns></returns>
        protected async Task DeleteEntity(TDeleteInput input, bool isCheckNull = true)
        {
            if (!string.IsNullOrEmpty(KeyFiledName))
            {
                LambdaObject obj = new LambdaObject()
                {
                    FieldType = LambdaFieldType.S,
                    FieldName = KeyFiledName,
                    FieldValue = input.GetFiledValue<string>(KeyFiledName),
                    ExpType = LambdaExpType.Equal
                };
                var exp = obj.GetExp<TEntity>();
                if (exp!=null && await Repository.FirstOrDefaultAsync(exp) == null)
                {
                    CheckErrors(NotExistMessage);
                }
            }

            await Repository.DeleteAsync(input.Id);
        }

        #endregion

        #region MAP

        protected virtual TEntityDto MapToEntityDto(TEntity entity)
        {
            return ObjectMapper.Map<TEntityDto>(entity);
        }

        protected virtual TEntity MapToEntity(TCreateInput createInput)
        {
            return ObjectMapper.Map<TEntity>(createInput);
        }

        protected virtual void MapToEntity(TUpdateInput updateInput, TEntity entity)
        {
            ObjectMapper.Map(updateInput, entity);
        }
        
        protected virtual TOut IwbMap<TOut>(object obj)
        {
           return ObjectMapper.Map<TOut>(obj);
        }
        protected virtual TOut IwbMap<TOut,TIn>(TIn obj)
        {
           return ObjectMapper.Map<TOut>(obj);
        }


        #endregion

        #region CHECK

        protected virtual int CheckGuid(int? guid)
        {
            if (guid == null || guid == 0)
            {
                ThrowError("GetGuidNoError");
                return 0;
            }
            return (int)guid;
        }

        protected virtual void CheckErrors(string error, LogSeverity severity = LogSeverity.Warn)
        {
            throw new UserFriendlyException(error, severity);
        }
        protected virtual void CheckErrors(IdentityResult identityResult, LogSeverity severity = LogSeverity.Warn)
        {
            identityResult.CheckErrors(LocalizationManager, severity:severity);
        }

        /// <summary>
        /// 抛出错误
        /// </summary>
        /// <param name="err"></param>
        /// <param name="isLocalization">是否要本地化</param>
        protected virtual void ThrowError(string err, bool isLocalization = true)
        {
            CheckErrors(isLocalization ? L(err) : err);
        }


        protected virtual void CheckPermission(string permissionName)
        {
            if (!string.IsNullOrEmpty(permissionName))
            {
                PermissionChecker.Authorize(permissionName);
            }
        }

        protected virtual void CheckGetPermission()
        {
            CheckPermission(GetPermissionName);
        }

        protected virtual void CheckGetAllPermission()
        {
            CheckPermission(GetAllPermissionName);
        }

        protected virtual void CheckCreatePermission()
        {
            CheckPermission(CreatePermissionName);
        }

        protected virtual void CheckUpdatePermission()
        {
            CheckPermission(UpdatePermissionName);
        }

        protected virtual void CheckDeletePermission()
        {
            CheckPermission(DeletePermissionName);
        }

        #endregion
    }



    #region AppServiceBase
    public abstract class IwbZeroAsyncCrudAppService<TEntity, TEntityDto>
   : IwbZeroAsyncCrudAppService<TEntity, TEntityDto, int>
   where TEntity : class, IEntity<int>
   where TEntityDto : IEntityDto<int>
    {
        protected IwbZeroAsyncCrudAppService(IRepository<TEntity, int> repository, string keyFiledName = null)
            : base(repository)
        {
            KeyFiledName = keyFiledName;
        }
    }

    public abstract class IwbZeroAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey>
        : IwbZeroAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected IwbZeroAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository, string keyFiledName = null)
            : base(repository)
        {
            KeyFiledName = keyFiledName;
        }
    }

    public abstract class IwbZeroAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput>
        : IwbZeroAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected IwbZeroAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository, string keyFiledName = null)
            : base(repository)
        {
            KeyFiledName = keyFiledName;
        }
    }

    public abstract class IwbZeroAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
        : IwbZeroAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TGetAllInput : IPagedAndSortedResultRequest
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
       where TCreateInput : IEntityDto<TPrimaryKey>
    {
        protected IwbZeroAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository, string keyFiledName = null)
            : base(repository)
        {
            KeyFiledName = keyFiledName;
        }
    }

    public abstract class IwbZeroAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        : IwbZeroAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected IwbZeroAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository, string keyFiledName = null)
            : base(repository)
        {
            KeyFiledName = keyFiledName;
        }
    }

    public abstract class IwbZeroAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
    : IwbZeroAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {
        protected IwbZeroAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository, string keyFiledName = null)
            : base(repository)
        {
            KeyFiledName = keyFiledName;
        }
    }

    #endregion

   
}
