using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.ProductInfo
{
    [Table("RmProduct")]
    public class RmProduct:FullAuditedEntity<string>
    {
        public const int ProductNameMaxLength = 50;
        public const int ModelMaxLength = 50;
        public const int MaterialMaxLength = 50;
        public const int ProductDescMaxLength = 200;

        [StringLength(ProductNameMaxLength)]
        public string ProductName { get; set; }

        [StringLength(MaterialMaxLength)]
        public string Material { get; set; }

        [StringLength(ModelMaxLength)]
        public string Model { get; set; }

        [StringLength(ProductDescMaxLength)]
        public string ProductDesc { get; set; }

        public string InsertSql()
        {
            return
                $" insert into RmProduct (Id,ProductName,Material,Model,ProductDesc,IsDeleted,CreationTime,CreatorUserId) values ('{Id}','{ProductName}','{Material}','{Model}','{ProductDesc}',0,'{CreationTime}',{CreatorUserId});";
        }
    }
}
