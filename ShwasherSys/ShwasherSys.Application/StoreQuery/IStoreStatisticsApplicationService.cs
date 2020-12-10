using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.Common.Dto;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.StoreQuery.Dto;

namespace ShwasherSys.StoreQuery
{
    public interface IStoreStatisticsApplicationService: IIwbAsyncCrudAppService<ViewEnterOutProductStoreDto, string, PagedRequestDto, ViewEnterOutProductStoreDto, ViewEnterOutProductStoreDto>
    {
        Task<List<ViewEnterOutProductStore>> QueryProductEnterOutRecord(string productNo);
        Task<PagedResultDto<ViewEnterOutLogCus>> QueryEnterOutRecord(PagedRequestDto input);

        Task<PagedResultDto<ViewCurrentStoreTotal>> QueryCurrentStoreTotal(PagedRequestDto input);

        Task<ViewCurrentStoreTotal> QueryStoreTotalByProduct(string pcProductNo);

        Task<string> ExportExcel(List<MultiSearchDtoExt> input);

        Task<List<ViewEnterOutProductStore>> QueryEnterOutRecordTmp(List<MultiSearchDtoExt> input);

        Task<List<ViewEnterOutSemiProductStore>> QuerySemiEnterOutRecord(string productNo);

        Task<PagedResultDto<ViewCurrentSemiStoreTotal>> QuerySemiCurrentStoreTotal(PagedRequestDto input);
        Task<ViewCurrentSemiStoreTotal> QuerySemiCurrentStoreTotalByProduct(string pcProductNo);
    }
}
