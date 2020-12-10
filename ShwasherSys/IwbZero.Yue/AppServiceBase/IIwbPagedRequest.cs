using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace IwbZero.AppServiceBase
{
    public interface IIwbPagedRequest : ILimitedResultRequest, ISortedResultRequest
    {
        int SkipCount { get; set; }
        string KeyField { get; set; }
        string KeyWords { get; set; }
        int FieldType { get; set; }
        int ExpType { get; set; }
        List<MultiSearchDto> SearchList { get; set; }
    }

}
