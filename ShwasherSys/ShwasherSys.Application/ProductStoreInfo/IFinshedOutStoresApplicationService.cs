using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.ProductStoreInfo.Dto;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys.ProductStoreInfo
{
    public interface IFinshedOutStoreAppService : IIwbAsyncCrudAppService<ProductOutStoreDto, int, PagedRequestDto, ProductOutStoreCreateDto, ProductOutStoreUpdateDto >
    {
        Task<PagedResultDto<ViewProductOutStore>> GetViewAll(PagedRequestDto input);

        Task<ProductOutStoreDto> Audit(ProductOutStoreAuditDto input);
        Task<ProductOutStoreDto> Refuse(EntityDto<int> input);
        Task<ProductOutStoreDto> Recovery(EntityDto<int> input);

        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AuditBatch(ProductOutStoreBatchAuditDto input);
        Task<string> PackageApply(ProductOutStoreDto input);
        Task<ProductOutStore> RePlating(ProductOutStoreDto input);
    }
}
