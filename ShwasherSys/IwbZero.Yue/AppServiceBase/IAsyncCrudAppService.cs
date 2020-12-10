using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace IwbZero.AppServiceBase
{
    public interface IIwbAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput, in TDeleteInput> : IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {

        //Task<TEntityDto> Get(TGetInput input);

        //Task<PagedResultDto<TEntityDto>> GetAll(TGetAllInput input);

        //Task<TEntityDto> Create(TCreateInput input);

        //Task<TEntityDto> Update(TUpdateInput input);

        //Task Delete(TDeleteInput input);
    }

    #region AppService

    public interface IIwbAsyncCrudAppService<TEntityDto>
        : IIwbAsyncCrudAppService<TEntityDto, int>
        where TEntityDto : IEntityDto<int>
    {

    }

    public interface IIwbAsyncCrudAppService<TEntityDto, TPrimaryKey>
        : IIwbAsyncCrudAppService<TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface IIwbAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput>
        : IIwbAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface IIwbAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput>
        : IIwbAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface IIwbAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput>
        : IIwbAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface IIwbAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput>
        : IIwbAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {

    }

    #endregion
}
