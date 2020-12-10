using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.CompanyInfo.Performance.Dto;

namespace ShwasherSys.CompanyInfo.Performance
{
    public interface IPerformanceAppService : IIwbZeroAsyncCrudAppService<PerformanceDto, int, IwbPagedRequestDto, PerformanceCreateDto, PerformanceUpdateDto >
    {
        Task<string> PerformanceTotalQuery(PerformanceTotalQueryDto input);

		#region Get

		Task<EmployeeWorkPerformance> GetEntity(EntityDto<int> input);
		Task<EmployeeWorkPerformance> GetEntityById(int id);
		Task<EmployeeWorkPerformance> GetEntityByNo(string no);
	
        #endregion

    }
}
