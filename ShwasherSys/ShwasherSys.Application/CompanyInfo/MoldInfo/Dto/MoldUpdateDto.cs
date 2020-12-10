using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.CompanyInfo.MoldInfo.Dto
{
    
    /// <summary>
    /// 模具维护计划
    /// </summary>   
    [AutoMapTo(typeof(Mold))]
    public class MoldUpdateDto: EntityDto<int>
    {
        
        /// <summary>
        /// 模具编码
        /// </summary>   
        [Required] 
        [StringLength(Mold.NoMaxLength)]
		public string No  { get; set; }
        
        /// <summary>
        /// 模具名称
        /// </summary>   
        [Required] 
        [StringLength(Mold.NameMaxLength)]
		public string Name  { get; set; }
        
        /// <summary>
        /// 模具规格
        /// </summary>   
        [StringLength(Mold.ModelMaxLength)]
		public string Model  { get; set; }
        
        /// <summary>
        /// 模具材质
        /// </summary>   
        [StringLength(Mold.MaterialMaxLength)]
		public string Material  { get; set; }
        
        /// <summary>
        /// 模具描述
        /// </summary>   
        [StringLength(Mold.DescMaxLength)]
		public string Description  { get; set; }
        
        /// <summary>
        /// 有效期限
        /// </summary>   
		public DateTime? ExpireDate  { get; set; }
        
        /// <summary>
        /// 维护周期
        /// </summary>   
		public int MaintenanceCycle  { get; set; }

        //      /// <summary>
        //      /// 维护时间
        //      /// </summary>   
        //public DateTime? MaintenanceDate  { get; set; }

        //      /// <summary>
        //      /// 下一次维护时间
        //      /// </summary>   
        //public DateTime? NextMaintenanceDate  { get; set; }

        [StringLength(Mold.CustomerNameMaxLength)]
        public string CustomerName { get; set; }
        [StringLength(Mold.ShelfNumMaxLength)]
        public string ShelfNum { get; set; }
        [StringLength(Mold.OuterDiameterMaxLength)]
        public string OuterDiameter { get; set; }
        [StringLength(Mold.InsideDiameterMaxLength)]
        public string InsideDiameter { get; set; }
        [StringLength(Mold.ThicknessMaxLength)]
        public string Thickness { get; set; }
        [StringLength(Mold.HeightMaxLength)]
        public string Height { get; set; }
        [StringLength(Mold.RigidityMaxLength)]
        public string Rigidity { get; set; }
    }
}