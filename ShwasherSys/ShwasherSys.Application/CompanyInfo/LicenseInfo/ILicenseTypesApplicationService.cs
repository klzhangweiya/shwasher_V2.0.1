using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.CompanyInfo.LicenseInfo.Dto;

namespace ShwasherSys.CompanyInfo.LicenseInfo
{
    public interface ILicenseTypeAppService : IIwbZeroAsyncCrudAppService<LicenseTypeDto, int, IwbPagedRequestDto, LicenseTypeCreateDto, LicenseTypeUpdateDto >
    {
        Task<List<SelectListItem>> GetSelectListByGroup(string groupName);
        Task<string> GetSelectStrByGroup(string groupName);

		#region Get

		Task<LicenseType> GetEntity(EntityDto<int> input);
		Task<LicenseType> GetEntityById(int id);
		Task<LicenseType> GetEntityByNo(string no);
	
        #endregion

    }
}
