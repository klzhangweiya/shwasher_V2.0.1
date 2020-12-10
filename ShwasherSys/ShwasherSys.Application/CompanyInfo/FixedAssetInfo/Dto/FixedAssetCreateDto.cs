using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using IwbZero.AppServiceBase;

namespace ShwasherSys.CompanyInfo.FixedAssetInfo.Dto
{
    
    /// <summary>
    /// 设备固定资产维护
    /// </summary>   
    [AutoMapTo(typeof(FixedAsset))]
    public class FixedAssetCreateDto:IwbEntityDto<int>
    {
		
        /// <summary>
        /// 资产编号
        /// </summary>   
        [StringLength(FixedAsset.NoMaxLength)]
		public string No  { get; set; }
        /// <summary>
        /// 资产名称
        /// </summary>   
        [StringLength(FixedAsset.NameMaxLength)]
		public string Name  { get; set; }
        /// <summary>
        /// 资产类型
        /// </summary>   
        [StringLength(FixedAsset.ModelMaxLength)]
		public string Model  { get; set; }
        /// <summary>
        /// 资产描述
        /// </summary>   
        [StringLength(FixedAsset.DescMaxLength)]
		public string Description  { get; set; }
    }
}
