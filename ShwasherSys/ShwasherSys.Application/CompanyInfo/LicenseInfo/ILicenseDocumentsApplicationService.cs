using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.CompanyInfo.LicenseInfo.Dto;

namespace ShwasherSys.CompanyInfo.LicenseInfo
{
    public interface ILicenseDocumentAppService : IIwbZeroAsyncCrudAppService<LicenseDocumentDto, int, IwbPagedRequestDto, LicenseDocumentCreateDto, LicenseDocumentUpdateDto >
    {


		#region Get

		Task<LicenseDocument> GetEntity(EntityDto<int> input);
		Task<LicenseDocument> GetEntityById(int id);
		Task<LicenseDocument> GetEntityByNo(string no);
	
        #endregion

    }
}
