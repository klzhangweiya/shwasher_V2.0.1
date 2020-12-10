using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace IwbZero.AppServiceBase
{
    public class PagedRequestDto : IPagedResultRequest, IPagedRequest
    {
        [Range(0, int.MaxValue)]
        public virtual int SkipCount { get; set; }
        [Range(1, int.MaxValue)]
        public virtual int MaxResultCount { get; set; } = 10;
        public virtual string Sorting { get; set; }
        public virtual string KeyField { get; set; }
        public virtual string KeyWords { get; set; }
        public virtual List<MultiSearchDto> SearchList { get; set; }
    }


}
