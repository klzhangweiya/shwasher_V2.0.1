using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using IwbZero.Authorization.Permissions;
using IwbZero.Session;
using IwbZero.Setting;

namespace IwbZero.AppServiceBase
{
    public abstract class IwbCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> : CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected IwbCrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {

        }

        public new IIwbPermissionManager PermissionManager { protected get; set; }
        protected new IIwbSettingManager SettingManager { get; set; }
        public new IIwbSession AbpSession { get; set; }
        protected ICacheManager CacheManager { get; set; }

    }

}
