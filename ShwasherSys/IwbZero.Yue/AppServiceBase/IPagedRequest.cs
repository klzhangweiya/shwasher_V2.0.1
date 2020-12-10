using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace IwbZero.AppServiceBase
{
    public interface IPagedRequest : ILimitedResultRequest
    {
        string Sorting { get; set; }
        int SkipCount { get; set; }
        string KeyField { get; set; }
        string KeyWords { get; set; }
        List<MultiSearchDto> SearchList { get; set; }
    }

    public class MultiSearchDto
    {
        public virtual string KeyField { get; set; }
        public virtual string KeyWords { get; set; }
        public virtual int FieldType { get; set; }
        public virtual int ExpType { get; set; }
    }
}
