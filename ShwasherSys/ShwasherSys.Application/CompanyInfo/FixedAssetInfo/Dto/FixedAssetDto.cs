using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ShwasherSys.CompanyInfo.FixedAssetInfo.Dto
{
    
    /// <summary>
    /// 设备固定资产维护
    /// </summary>   
    [AutoMapTo(typeof(FixedAsset)),AutoMapFrom(typeof(FixedAsset))]
    public class FixedAssetDto: EntityDto<int>
    {
        /// <summary>
        /// 资产编号
        /// </summary>   
		public string No  { get; set; }
        /// <summary>
        /// 资产名称
        /// </summary>   
		public string Name  { get; set; }
        /// <summary>
        /// 资产类型
        /// </summary>   
		public string Model  { get; set; }
        /// <summary>
        /// 资产描述
        /// </summary>   
		public string Description  { get; set; }
    }
}