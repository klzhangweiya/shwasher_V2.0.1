using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.ProductInfo.Dto;

namespace ShwasherSys.ProductInfo
{
    public interface IProductPropertyAppService : IIwbZeroAsyncCrudAppService<ProductPropertyDto, int, IwbPagedRequestDto, ProductPropertyCreateDto, ProductPropertyUpdateDto >
    {


		#region Get

		Task<ProductProperty> GetEntity(EntityDto<int> input);
		Task<ProductProperty> GetEntityById(int id);
		Task<ProductProperty> GetEntityByNo(string no);
	
        #endregion

    }
}
