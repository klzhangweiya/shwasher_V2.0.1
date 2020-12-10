using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace IwbZero.AppServiceBase
{
    public interface IIwbZeroAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput, in TDeleteInput> : IApplicationService
    {
        Task<List<SelectListItem>> GetSelectList();
        Task<string> GetSelectStr();

        Task<TEntityDto> GetDto(TGetInput input);
        Task<TEntityDto> GetDtoById(TPrimaryKey id);
        Task<TEntityDto> GetDtoByNo(string no);
        Task<PagedResultDto<TEntityDto>> GetAll(TGetAllInput input);

        Task Create(TCreateInput input);

        Task Update(TUpdateInput input);

        Task Delete(TDeleteInput input);
    }

    #region AppService

    public interface IIwbZeroAsyncCrudAppService<TEntityDto>
        : IIwbZeroAsyncCrudAppService<TEntityDto, int>
        where TEntityDto : IEntityDto<int>
    {

    }

    public interface IIwbZeroAsyncCrudAppService<TEntityDto, TPrimaryKey>
        : IIwbZeroAsyncCrudAppService<TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface IIwbZeroAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput>
        : IIwbZeroAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface IIwbZeroAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput>
        : IIwbZeroAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface IIwbZeroAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput>
        : IIwbZeroAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface IIwbZeroAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput>
        : IIwbZeroAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {

    }

    #endregion
}
