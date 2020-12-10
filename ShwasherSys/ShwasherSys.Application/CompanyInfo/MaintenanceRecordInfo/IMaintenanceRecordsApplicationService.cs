using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.CompanyInfo.MaintenanceRecordInfo.Dto;

namespace ShwasherSys.CompanyInfo.MaintenanceRecordInfo
{
    public interface IMaintainRecordAppService : IIwbZeroAsyncCrudAppService<MaintenanceRecordDto, string, IwbPagedRequestDto, MaintenanceRecordCreateDto, MaintenanceRecordUpdateDto >
    {
        Task<PagedResultDto<MemberDto>> GetAllMember(IwbPagedRequestDto input);
        Task AddMember(MemberDto input);
        Task UpdateMember(MemberDto input);
        Task DeleteMember(EntityDto<string> input);
        Task Complete(CompleteDto input);
		#region Get

		Task<MaintenanceRecord> GetEntity(EntityDto<string> input);
		Task<MaintenanceRecord> GetEntityById(string id);
		Task<MaintenanceRecord> GetEntityByNo(string no);
	
        #endregion

    }
}
