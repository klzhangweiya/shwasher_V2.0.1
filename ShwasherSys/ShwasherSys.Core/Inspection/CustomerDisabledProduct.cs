using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ShwasherSys.Authorization.Users;

namespace ShwasherSys.Inspection
{
    [Table("CustomerDisProductInfo")]
    public class CustomerDisabledProduct:CreationAuditedEntity<int,SysUser>
    {
        public string ProductOrderNo { get; set; }

        public string CustomerNo { get; set; }
    }
}