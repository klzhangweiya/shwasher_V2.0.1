using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.ProductInfo.Dto;
using ShwasherSys.ProductInfo.Dto.FileUpload;

namespace ShwasherSys.ProductInfo
{
    public interface IRmProductAppService : IIwbZeroAsyncCrudAppService<RmProductDto, string, IwbPagedRequestDto, RmProductCreateDto, RmProductUpdateDto >
    {


		#region Get

		Task<RmProduct> GetEntity(EntityDto<string> input);
		Task<RmProduct> GetEntityById(string id);
		Task<RmProduct> GetEntityByNo(string no);

		#endregion

        bool ImportExcel(FileUploadInfoDto input);

    }
}
