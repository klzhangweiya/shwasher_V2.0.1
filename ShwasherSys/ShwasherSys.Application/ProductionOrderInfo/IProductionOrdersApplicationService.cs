using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using IwbZero.AppServiceBase;
using ShwasherSys.ProductionOrderInfo.Dto;
using ShwasherSys.ProductStoreInfo.Dto;
using ShwasherSys.SemiProductStoreInfo;
using ShwasherSys.SemiProductStoreInfo.Dto;

namespace ShwasherSys.ProductionOrderInfo
{
    public interface IProductionOrdersAppService : IIwbAsyncCrudAppService<ProductionOrderDto, int, PagedRequestDto, ProductionOrderCreateDto, ProductionOrderUpdateDto >
    {
        Task<string> GetNewProductionOrderNo();
        Task<string> GetNewProductionOrderNo(int isOutsourcing);
        Task<string> ExcelExport(ExportDto input);
        Task<ProductionOrderDto> ChangeProductionOrderStatus(ChangeProductionOrderStatusDto input);
        Task<ProductionOrderDto> ConfirmEnterStore(ConfirmEnterStoreDto input);
        Task<ProductionOrderDto> CreateOutProductionOrder(CreateOutProductionOrderDto input);
        Task<ProductionOrderDto> UpdateOutProductionOrder(UpdateOutProductionOrderDto input);
        Task DeleteOutProductionOrder(EntityDto<int> input);
        Task<SemiEnterStoreDto> CreateEnterStoreApply(CreateEnterStoreApplyDto input);
        Task<SemiEnterStoreDto> ConfirmSemiEnterStoreQuantity(EntityDto<int> input);
        Task<SemiEnterStoreDto> CancelSemiEnterStoreApplyStatus(EntityDto<int> input);
        Task<SemiEnterStoreDto> CloseEnterStoreApply(EntityDto<int> input);
        Task<SemiEnterStoreDto> RecoverySemiEnterStoreApplyStatus(EntityDto<int> input);

        PagedResultDto<ViewSemiEnterStore> GetSemiEnterStoreApply(PagedRequestDto input);


        //Task<SemiEnterStoreDto> DeleteEnterStoreApply(EntityDto<int> input);
        Task<SemiEnterStoreDto> UpdateEnterStoreApply(UpdateSemiEnterStoreDto input);


        PagedResultDto<ViewSemiOutStore> GetSemiOutStoreApply(PagedRequestDto input);
        ViewSemiOutStore GetSemiOutStoreApplyById(int id);

        Task<SemiOutStoreDto> CreateOutStoreApply(SemiOutStoreCreateDto input);
       // Task<SemiOutStoreDto> DeleteOutStoreApply(EntityDto<int> input);
        Task<SemiOutStoreDto> UpdateOutStoreApply(SemiOutStoreUpdateDto input);

        Task<SemiOutStoreDto> ConfirmSemiOutStoreQuantity(EntityDto<int> input);
        Task<SemiOutStoreDto> CancelSemiOutStoreApplyStatus(EntityDto<int> input);
        Task<SemiOutStoreDto> CloseOutStoreApply(EntityDto<int> input);
        Task<SemiOutStoreDto> RecoverySemiOutStoreApplyStatus(EntityDto<int> input);

        Task<ProductionReportDto> QueryProductionReport(QueryProductionReportDto input);
        Task<ProductionReportDto> QueryOutsourcingReport(QueryProductionReportDto input);
        Task<string> ExportOutsourcingReport(QueryProductionReportDto input);
        Task<string> ExcelExportOut(EntityDto<string> input);


        PagedResultDto<ViewProductOutStore> GetRePlatingOutStoreApply(PagedRequestDto input);
        Task<ProductOutStoreDto> CancelFinishOutStoreApply(EntityDto<int> input);

        Task<string> RePlatingExportApply(EntityDto<int> input);
    }
}
