using System.Collections.Generic;
using System.Threading.Tasks;
using IwbZero.AppServiceBase;
using ShwasherSys.ProductInfo.Dto;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using ShwasherSys.ProductInfo.Dto.FileUpload;

namespace ShwasherSys.ProductInfo
{
    public interface ISemiProductsAppService : IIwbAsyncCrudAppService<SemiProductDto, string, PagedRequestDto, SemiProductCreateDto, SemiProductUpdateDto >
    {
        bool ImportExcel(FileUploadInfoDto input);

        Task<string> ExportExcel();
    }
}
