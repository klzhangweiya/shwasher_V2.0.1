using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.CompanyInfo.DeviceInfo.Dto;

namespace ShwasherSys.CompanyInfo.DeviceInfo
{
    public interface IDeviceMgPlanAppService : IIwbZeroAsyncCrudAppService<DeviceMgPlanDto, int, IwbPagedRequestDto, DeviceMgPlanCreateDto, DeviceMgPlanUpdateDto >
    {
        Task Maintain(MaintainDto input);

		#region Get

		Task<DeviceMgPlan> GetEntity(EntityDto<int> input);
		Task<DeviceMgPlan> GetEntityById(int id);
		Task<DeviceMgPlan> GetEntityByNo(string no);
	
        #endregion

    }
}
