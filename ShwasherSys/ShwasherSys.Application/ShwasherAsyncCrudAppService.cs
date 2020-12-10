using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.ObjectMapping;
using ShwasherSys.Lambda;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using Microsoft.AspNet.Identity;

namespace ShwasherSys
{
    public abstract class ShwasherAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
      : IwbCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
          where TEntity : class, IEntity<TPrimaryKey>
          where TEntityDto : IEntityDto<TPrimaryKey>
          where TUpdateInput : IEntityDto<TPrimaryKey>
          where TGetInput : IEntityDto<TPrimaryKey>
          where TDeleteInput : IEntityDto<TPrimaryKey>
          where TGetAllInput : IPagedRequest
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        protected string KeyFiledName { get; set; }
        protected virtual bool KeyIsAuto { get; set; } = true;
        protected virtual string ExistMessage { get; set; } = "编号已存在，请检查后再操作！";
        protected virtual string NotExistMessage { get; set; } = "编号不存在，请检查后再操作！";

        protected ShwasherAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository, string keyFiledName = null)
            : base(repository)
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            LocalizationSourceName = ShwasherConsts.LocalizationSourceName;
            ObjectMapper = NullObjectMapper.Instance;
            KeyFiledName = keyFiledName;
        }
        [DisableAuditing]
        public virtual async Task<TEntityDto> Get(TGetInput input)
        {
            CheckGetPermission();
            if (!KeyFiledName.IsNullOrEmpty() && input.Id == null)
            {
                return await GetDtoByNoAsync(input.GetFiledValue<string>(KeyFiledName));
            }
            var entity = await GetEntityByIdAsync(input.Id);
            return MapToEntityDto(entity);
        }
        [DisableAuditing]
        public virtual async Task<PagedResultDto<TEntityDto>> GetAll(TGetAllInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            var property = typeof(TEntity).GetProperty("IsLock");
            if (property != null)
            {
                LambdaObject objLambdaObject = new LambdaObject()
                {
                    FieldType = LambdaFieldType.S,
                    FieldName = "IsLock",
                    FieldValue = "N",
                    ExpType = LambdaExpType.Equal
                };
                var expIsLock = objLambdaObject.GetExp<TEntity>();
                query = query.Where(expIsLock);
            }
            //if (!input.KeyWords.IsNullOrEmpty())
            //{
            //    LambdaObject obj = new LambdaObject()
            //    {
            //        FieldType = LambdaFieldType.S,
            //        FieldName = input.KeyField,
            //        FieldValue = input.KeyWords,
            //        ExpType = LambdaExpType.Contains
            //    };
            //    var exp = obj.GetExp<TEntity>();
            //    query = query.Where(exp);
            //}
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {
                    if (o.KeyWords.IsNullOrEmpty())
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
                var exp = objList.GetExp<TEntity>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<TEntityDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
            return dtos;
        }
        public virtual async Task<TEntityDto> Create(TCreateInput input)
        {
            CheckCreatePermission();
            return await CreateEntity(input);
        }
        public virtual async Task<TEntityDto> Update(TUpdateInput input)
        {
            CheckCreatePermission();
            return await UpdateEntity(input);
        }
        public virtual Task Delete(TDeleteInput input)
        {
            CheckDeletePermission();

            return Repository.DeleteAsync(input.Id);
        }
        protected virtual IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query, TGetAllInput input)
        {
            
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {
                    if (o.KeyWords.IsNullOrEmpty())
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
                var exp = objList.GetExp<TEntity>();
                query = query.Where(exp);
            }

            return query;
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected new virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetAllInput input)
        {
            //Try to sort query if available
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (input is ISortedResultRequest sortInput)
            {
                if (!sortInput.Sorting.IsNullOrWhiteSpace())
                {
                    return query.OrderBy(sortInput.Sorting);
                }
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            // ReSharper disable once IsExpressionAlwaysTrue
            if (input is ILimitedResultRequest)
            {
                if (!input.Sorting.IsNullOrWhiteSpace())
                {
                    return query.OrderBy(input.Sorting);
                }
                return query.OrderByDescending(e => e.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected new virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetAllInput input)
        {
            //Try to use paging if available
            if (input is IPagedResultRequest pagedInput)
            {
                return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        protected virtual Task<TEntity> GetEntityByIdAsync(TPrimaryKey id)
        {
            return Repository.GetAsync(id);
        }
        protected virtual Task<TEntity> GetEntityByNoAsync(string no)
        {
            if (KeyFiledName.IsNullOrEmpty())
            {
                CheckErrors(IwbIdentityResult.Failed("编码/编号字段不明确，请检查后再操作！"));
            }
            LambdaObject obj = new LambdaObject()
            {
                FieldType = LambdaFieldType.S,
                FieldName = KeyFiledName,
                FieldValue = no,
                ExpType = LambdaExpType.Equal
            };
            var exp = obj.GetExp<TEntity>();
            return Repository.FirstOrDefaultAsync(exp);
        }
        protected virtual async Task<TEntityDto> GetDtoByNoAsync(string no)
        {
            var entity = await GetEntityByNoAsync(no);
            return MapToEntityDto(entity);
        }
        #region 使用ABP实体
        protected async Task<TEntityDto> CreateEntity1(TCreateInput input)
        {
            if (!KeyFiledName.IsNullOrEmpty())
            {
                if (KeyIsAuto)
                {
                    input.SetFiledValue(KeyFiledName, Guid.NewGuid().ToString("N"));
                }
                else
                {
                    LambdaObject obj = new LambdaObject()
                    {
                        FieldType = LambdaFieldType.S,
                        FieldName = KeyFiledName,
                        FieldValue = input.GetFiledValue<string>(KeyFiledName),
                        ExpType = LambdaExpType.Equal
                    };
                    var exp = obj.GetExp<TEntity>();
                    if (await Repository.FirstOrDefaultAsync(exp) != null)
                    {
                        CheckErrors(IwbIdentityResult.Failed(ExistMessage));
                    }
                }

            }
            var entity = MapToEntity(input);



            await Repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        protected async Task<TEntityDto> UpdateEntity1(TUpdateInput input)
        {

            if (!KeyIsAuto && !KeyFiledName.IsNullOrEmpty())
            {
                LambdaObject obj = new LambdaObject()
                {
                    FieldType = LambdaFieldType.S,
                    FieldName = KeyFiledName,
                    FieldValue = input.GetFiledValue<string>(KeyFiledName),
                    ExpType = LambdaExpType.Equal
                };
                var exp = obj.GetExp<TEntity>();
                if (await Repository.FirstOrDefaultAsync(exp) == null)
                {
                    CheckErrors(IwbIdentityResult.Failed(NotExistMessage));
                }
            }
            var entity = await GetEntityByIdAsync(input.Id);
           

            MapToEntity(input, entity);

           
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        #endregion
        protected async Task<TEntityDto> CreateEntity(TCreateInput input)
        {
            if (!KeyFiledName.IsNullOrEmpty())
            {
                if (KeyIsAuto)
                {
                    input.SetFiledValue(KeyFiledName, Guid.NewGuid().ToString("N"));
                }
                else
                {
                    LambdaObject obj = new LambdaObject()
                    {
                        FieldType = LambdaFieldType.S,
                        FieldName = KeyFiledName,
                        FieldValue = input.GetFiledValue<string>(KeyFiledName),
                        ExpType = LambdaExpType.Equal
                    };
                    var exp = obj.GetExp<TEntity>();
                    if (await Repository.FirstOrDefaultAsync(exp) != null)
                    {
                        CheckErrors(IwbIdentityResult.Failed(ExistMessage));
                    }
                }

            }
            var entity = MapToEntity(input);
           
            #region shwasher temp
            AddCommonPropertyValue("TimeCreated", DateTime.Now, ref entity);
            AddCommonPropertyValue("TimeLastMod", DateTime.Now, ref entity);
            AddCommonPropertyValue("UserIDLastMod", AbpSession.UserName, ref entity);
            AddCommonPropertyValue("CreatorUserId", AbpSession.UserName, ref entity);
            AddCommonPropertyValue("IsLock", "N", ref entity);
            #endregion
            await Repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        protected async Task<TEntityDto> UpdateEntity(TUpdateInput input)
        {

            if (!KeyIsAuto && !KeyFiledName.IsNullOrEmpty())
            {
                LambdaObject obj = new LambdaObject()
                {
                    FieldType = LambdaFieldType.S,
                    FieldName = KeyFiledName,
                    FieldValue = input.GetFiledValue<string>(KeyFiledName),
                    ExpType = LambdaExpType.Equal
                };
                var exp = obj.GetExp<TEntity>();
                if (await Repository.FirstOrDefaultAsync(exp) == null)
                {
                    CheckErrors(IwbIdentityResult.Failed(NotExistMessage));
                }
            }
            var entity = await GetEntityByIdAsync(input.Id);
            #region shwasher temp
            GetCommonPropertyValue("TimeCreated", entity, ref input);
            GetCommonPropertyValue("TimeLastMod", entity, ref input);
            GetCommonPropertyValue("IsLock", entity, ref input);
            #endregion

            MapToEntity(input, entity);

            #region shwasher temp
            AddCommonPropertyValue("TimeLastMod", DateTime.Now, ref entity);
            AddCommonPropertyValue("UserIDLastMod", AbpSession.UserName, ref entity);
            AddCommonPropertyValue("IsLock", "N", ref entity);
            #endregion
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        /// <summary>
        /// shwasher temp GetCommonProperty 
        /// </summary>
        /// <param name="pcPropertyName"></param>
        /// <param name="entity"></param>
        /// <param name="input"></param>
        protected void GetCommonPropertyValue(string pcPropertyName, TEntity entity, ref TUpdateInput input)
        {
            var property = input.GetType().GetProperty(pcPropertyName);
            var value= entity.GetType().GetValue(pcPropertyName);
            if (property != null&& value!=null)
            {
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    property.SetValue(input,
                        Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? throw new InvalidOperationException(), null));
                }
                else
                {
                    property.SetValue(input, Convert.ChangeType(value, property.PropertyType), null);
                }
            }
        }
        /// <summary>
        /// shwasher temp AddCommonProperty 
        /// </summary>
        /// <param name="pcPropertyName"></param>
        /// <param name="value"></param>
        /// <param name="entity"></param>
        protected void AddCommonPropertyValue(string pcPropertyName, object value, ref TEntity entity)
        {
            var property = entity.GetType().GetProperty(pcPropertyName);
            if (property != null)
            {
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    property.SetValue(entity,
                        Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? throw new InvalidOperationException(), null));
                }
                else
                {
                    property.SetValue(entity, Convert.ChangeType(value, property.PropertyType), null);
                }
            }
        }
        public virtual async Task<TEntityDto> LockRecord(TDeleteInput input)
        {
            CheckDeletePermission();
            var entity = await GetEntityByIdAsync(input.Id);
            
            #region shwasher temp
            AddCommonPropertyValue("TimeLastMod", DateTime.Now, ref entity);
            AddCommonPropertyValue("UserIDLastMod", AbpSession.UserName, ref entity);
            AddCommonPropertyValue("IsLock", "Y", ref entity);
            #endregion
            await CurrentUnitOfWork.SaveChangesAsync();
            return MapToEntityDto(entity);
        }
       
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }

    #region AppService

    public abstract class ShwasherAsyncCrudAppService<TEntity, TEntityDto>
        : ShwasherAsyncCrudAppService<TEntity, TEntityDto, int>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
    {
        protected ShwasherAsyncCrudAppService(IRepository<TEntity, int> repository, string keyFiledName = null)
            : base(repository)
        {
            KeyFiledName = keyFiledName;
        }
    }

    public abstract class ShwasherAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey>
        : ShwasherAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, IPagedRequest>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected ShwasherAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository, string keyFiledName = null)
            : base(repository)
        {
            KeyFiledName = keyFiledName;
        }
    }

    public abstract class ShwasherAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput>
        : ShwasherAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetAllInput : IPagedRequest
    {
        protected ShwasherAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository, string keyFiledName = null)
            : base(repository)
        {
            KeyFiledName = keyFiledName;
        }
    }

    public abstract class ShwasherAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
        : ShwasherAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TGetAllInput : IPagedRequest
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {
        protected ShwasherAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository, string keyFiledName = null)
            : base(repository)
        {
            KeyFiledName = keyFiledName;
        }
    }

    public abstract class ShwasherAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        : ShwasherAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetAllInput : IPagedRequest
    {
        protected ShwasherAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository, string keyFiledName = null)
            : base(repository)
        {
            KeyFiledName = keyFiledName;
        }
    }

    public abstract class ShwasherAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
    : ShwasherAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TGetAllInput : IPagedRequest
    {

        protected ShwasherAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository, string keyFiledName = null)
            : base(repository)
        {
            KeyFiledName = keyFiledName;
        }
    }

    #endregion
}
