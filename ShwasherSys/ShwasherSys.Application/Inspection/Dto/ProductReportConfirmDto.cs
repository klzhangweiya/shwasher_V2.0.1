using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.BaseSysInfo.SysAttachFiles.Dto;

namespace ShwasherSys.Inspection.Dto
{
    public class ProductReportConfirmDto
    {
        /// <summary>
        /// 检验编号
        /// </summary>   
        public string ProductInspectReportNo  { get; set; }
        /// <summary>
        /// 检验报告
        /// </summary>
        public string ReportContent { get; set; }
        /// <summary>
        /// 检验详情
        /// </summary>   
        [StringLength(ProductInspectInfo.InspectContentMaxLength)]
        public string InspectContent  { get; set; }
        public List<SysAttachFileCreateDto> AttachFiles { get; set; }
       
    }
}