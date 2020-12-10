using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.CompanyInfo.DeviceInfo.Dto;
using ShwasherSys.CompanyInfo.MoldInfo.Dto;

namespace ShwasherSys.CompanyInfo.MoldInfo
{
    public interface IMoldAppService : IIwbZeroAsyncCrudAppService<MoldDto, int, IwbPagedRequestDto, MoldCreateDto, MoldUpdateDto >
    {

		#region Get

		Task<Mold> GetEntity(EntityDto<int> input);
		Task<Mold> GetEntityById(int id);
		Task<Mold> GetEntityByNo(string no);
	
        #endregion

    }
}
