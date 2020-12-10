using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.CompanyInfo.MoldInfo.Dto
{
    
    /// <summary>
    /// 模具维护计划
    /// </summary>   
    [AutoMapTo(typeof(Mold)),AutoMapFrom(typeof(Mold))]
    public class MoldDto: EntityDto<int>
    {
        /// <summary>
        /// 模具编码
        /// </summary>   
		public string No  { get; set; }
        /// <summary>
        /// 模具名称
        /// </summary>   
		public string Name  { get; set; }
        /// <summary>
        /// 模具规格
        /// </summary>   
		public string Model  { get; set; }
        /// <summary>
        /// 模具材质
        /// </summary>   
		public string Material  { get; set; }
        /// <summary>
        /// 模具描述
        /// </summary>   
		public string Description  { get; set; }
        /// <summary>
        /// 有效期限
        /// </summary>   
		public DateTime ExpireDate  { get; set; }
        /// <summary>
        /// 维护周期
        /// </summary>   
		public int MaintenanceCycle  { get; set; }
        /// <summary>
        /// 维护时间
        /// </summary>   
		public DateTime MaintenanceDate  { get; set; }
        /// <summary>
        /// 下一次维护时间
        /// </summary>   
		public DateTime NextMaintenanceDate  { get; set; }

      //  [MaxLength(CustomerNameMaxLength)]
        public string CustomerName { get; set; }
      //  [MaxLength(ShelfNumMaxLength)]
        public string ShelfNum { get; set; }
      //  [MaxLength(OuterDiameterMaxLength)]
        public string OuterDiameter { get; set; }
      //  [MaxLength(InsideDiameterMaxLength)]
        public string InsideDiameter { get; set; }
     //   [MaxLength(ThicknessMaxLength)]
        public string Thickness { get; set; }
     //   [MaxLength(HeightMaxLength)]
        public string Height { get; set; }
     //   [MaxLength(RigidityMaxLength)]
        public string Rigidity { get; set; }
    }
}