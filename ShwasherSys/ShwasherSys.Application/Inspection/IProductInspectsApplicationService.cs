using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Web.Security.AntiForgery;
using IwbZero.AppServiceBase;
using ShwasherSys.BaseSysInfo.SysAttachFiles.Dto;
using ShwasherSys.Inspection.Dto;
using ShwasherSys.ProductionOrderInfo.Dto;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys.Inspection
{
    public interface IProductInspectAppService : IIwbAsyncCrudAppService<ProductInspectDto, int, PagedRequestDto, ProductInspectCreateDto, ProductInspectUpdateDto >
    {
        PagedResultDto<ViewSemiEnterStore> GetSemiEnterStoreCheck(PagedRequestDto input);
        Task Check(EntityDto<int> input);
        Task UnCheck(EntityDto<int> input);
        Task<PagedResultDto<ProductionOrderDto>> GetAllInspect(PagedRequestDto input);
        Task<PagedResultDto<ProductInspectReportDto>> GetAllReport(PagedRequestDto input);
        [DisableAbpAntiForgeryTokenValidation]
        Task Template(ProductInspectTemplateDto input);
        [DisableAbpAntiForgeryTokenValidation]
        Task<ProductInspectDto> CreateInspect(ProductInspectCreateDto input);

        [DisableAbpAntiForgeryTokenValidation]
        Task ConfirmReport(ProductReportConfirmDto input);
        //[DisableAbpAntiForgeryTokenValidation]
        //Task<ProductInspectDto> UpdateInspect(ProductInspectUpdateDto input);
        Task<string> QueryReport(string no, int isProduct);
        Task<List<SysAttachFileDto>> QueryAttach(QueryAttachDto input);
        Task DeleteAttach(string attachNo);
        Task<List<SelectListItem>> GetSelectList();
		Task<string> GetSelectStr();

		#region Get
		Task<ProductInspectInfo> GetEntityById(int id);
		Task<ProductInspectInfo> GetEntityByNo(string no);
		Task<ProductInspectDto> GetDtoById(int id);
		Task<ProductInspectDto> GetDtoByNo(string no);
        #endregion

    }
}
