using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.ProductInfo
{
    [Table("SemiProducts")]
    public class SemiProducts:Entity<string>
    {
        public const int SemiProductNameMaxLength = 50;
        public const int ModelMaxLength = 50;
        public const int MaterialMaxLength = 50;
        public const int ProductDescMaxLength = 200;
        public const int SurfaceColorMaxLength = 50;
        public const int RigidityMaxLength = 50;
        public const int UserIDLastModMaxLength = 20;
        public const int PartNoMaxLength = 50;
        public const int IsLockMaxLength = 1;

        public const int IsStandardMaxLength = 1;

        /*[Key]
        [StringLength(30)]
        public string ProductNo { get; set; }*/


        [StringLength(SemiProductNameMaxLength)]
        public string SemiProductName { get; set; }

        [StringLength(ModelMaxLength)]
        public string Model { get; set; }

      

        [StringLength(MaterialMaxLength)]
        public string Material { get; set; }

        [StringLength(ProductDescMaxLength)]
        public string ProductDesc { get; set; }

        [StringLength(SurfaceColorMaxLength)]
        public string SurfaceColor { get; set; }

        [StringLength(RigidityMaxLength)]
        public string Rigidity { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }
        [Required]
        [StringLength(IsStandardMaxLength)]
        public string IsStandard { get; set; }
        public int Sequence { get; set; }


        [Required]
        [StringLength(IsLockMaxLength)]
        public string IsLock { get; set; }

        [StringLength(PartNoMaxLength)]
        public string PartNo { get; set; }

        [DecimalPrecision]
        public decimal? TranUnitValue { get; set; }

        public string InsertSql()
        {
            return
                $" insert into SemiProducts (SemiProductNo,SemiProductName,Model,Material,ProductDesc,SurfaceColor,Rigidity,TimeCreated,TimeLastMod,UserIDLastMod,IsStandard,Sequence,IsLock ,PartNo) values ('{Id}','{SemiProductName}','{Model}','{Material}','{ProductDesc}','{SurfaceColor}','{Rigidity}','{TimeCreated}','{TimeLastMod}','{UserIDLastMod}','{IsStandard}','{Sequence}','{IsLock}','{PartNo}');";
        }
    }
}
