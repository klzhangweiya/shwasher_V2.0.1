using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ShwasherSys.Authorization.Users;

namespace ShwasherSys.CompanyInfo
{
    
    [Table("MoldInfo")]
    public class Mold : FullAuditedEntity<int, SysUser>
    {
        public const int NoMaxLength = 50;
        public const int NameMaxLength = 50;
        public const int DescMaxLength = 500;
        public const int ModelMaxLength = 50;
        public const int MaterialMaxLength = 50;
        public const int SurfaceColorMaxLength = 50;
        public const int RigidityMaxLength = 50;
        public const int CustomerNameMaxLength = 50;
        public const int ShelfNumMaxLength = 50;
        public const int OuterDiameterMaxLength = 50;
        public const int InsideDiameterMaxLength = 50;
        public const int ThicknessMaxLength = 50;
        public const int HeightMaxLength = 50;
        /// <summary>
        /// 模具编码
        /// </summary>
        [MaxLength(NoMaxLength)]
        public string No { get; set; }
        /// <summary>
        /// 模具名称
        /// </summary>
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
        /// <summary>
        /// 模具规格
        /// </summary>

        [MaxLength(ModelMaxLength)]
        public string Model { get; set; }

        /// <summary>
        /// 模具材质
        /// </summary>
        [MaxLength(MaterialMaxLength)]
        public string Material { get; set; }
        /// <summary>
        /// 模具描述
        /// </summary>
        [MaxLength(DescMaxLength)]
        public string Description { get; set; }

       

        public const int RemarkMaxLength = 500;
        [MaxLength(RemarkMaxLength)]
        public string Remark { get; set; }

        [MaxLength(CustomerNameMaxLength)]
        public string CustomerName { get; set; }
        [MaxLength(ShelfNumMaxLength)]
        public string ShelfNum { get; set; }
        [MaxLength(OuterDiameterMaxLength)]
        public string OuterDiameter { get; set; }
        [MaxLength(InsideDiameterMaxLength)]
        public string InsideDiameter { get; set; }
        [MaxLength(ThicknessMaxLength)]
        public string Thickness { get; set; }
        [MaxLength(HeightMaxLength)]
        public string Height { get; set; }
        [MaxLength(RigidityMaxLength)]
        public string Rigidity { get; set; }

    }
}