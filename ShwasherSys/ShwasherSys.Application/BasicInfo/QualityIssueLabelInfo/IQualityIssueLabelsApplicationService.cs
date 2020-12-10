using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.BasicInfo.QualityIssueLabelInfo.Dto;

namespace ShwasherSys.BasicInfo.QualityIssueLabelInfo
{
    public interface IQualityIssueLabelAppService : IIwbZeroAsyncCrudAppService<QualityIssueLabelDto, string, IwbPagedRequestDto, QualityIssueLabelCreateDto, QualityIssueLabelUpdateDto >
    {


		#region Get

		Task<QualityIssueLabel> GetEntity(EntityDto<string> input);
		Task<QualityIssueLabel> GetEntityById(string id);
		Task<QualityIssueLabel> GetEntityByNo(string no);
	
        #endregion

    }
}
