using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using IwbZero.AppServiceBase;

namespace ShwasherSys.CompanyInfo.MoldInfo.Dto
{
    
    /// <summary>
    /// 模具维护计划
    /// </summary>   
    [AutoMapTo(typeof(Mold))]
    public class MoldCreateDto:IwbEntityDto<int>
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
        //      /// <summary>
        //      /// 有效期限
        //      /// </summary>   
        //public DateTime ExpireDate  { get; set; }
        //      /// <summary>
        //      /// 维护周期
        //      /// </summary>   
        //public int MaintenanceCycle  { get; set; }
        //      /// <summary>
        //      /// 维护时间
        //      /// </summary>   
        //public DateTime MaintenanceDate  { get; set; }
        //      /// <summary>
        //      /// 下一次维护时间
        //      /// </summary>   
        //public DateTime? NextMaintenanceDate  { get; set; }

       [MaxLength(Mold.CustomerNameMaxLength)]
        public string CustomerName { get; set; }
       [MaxLength(Mold.ShelfNumMaxLength)]
        public string ShelfNum { get; set; }
          [MaxLength(Mold.OuterDiameterMaxLength)]
        public string OuterDiameter { get; set; }
          [MaxLength(Mold.InsideDiameterMaxLength)]
        public string InsideDiameter { get; set; }
           [MaxLength(Mold.ThicknessMaxLength)]
        public string Thickness { get; set; }
           [MaxLength(Mold.HeightMaxLength)]
        public string Height { get; set; }
         [MaxLength(Mold.RigidityMaxLength)]
        public string Rigidity { get; set; }
    }
}
