using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.ProductInfo.Dto;

namespace ShwasherSys.ProductInfo
{
    public interface IProductsAppService : IIwbAsyncCrudAppService<ProductDto, string, PagedRequestDto, ProductCreateDto, ProductUpdateDto >
    {
        List<SelectListItem> GetProductPropertyList(string pcPropertyName);


        Task<PagedResultDto<ProductDto>> GetQueryCustomerDefaultProduct(PagedRequestDto input);


        Task<string> ExportExcel(List<MultiSearchDto> input);
    }
}
