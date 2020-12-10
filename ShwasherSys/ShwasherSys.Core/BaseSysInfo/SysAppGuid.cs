using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ShwasherSys.BaseSysInfo
{
    [Table("Sys_AppGuids")]
    public class SysAppGuid : Entity<int>
    {
        public short Step { get; set; }
        public int LastId { get; set; }
        public short IdType { get; set; }
    }
    public enum AppGuidType
    {
       PackageEnterBill=1
    }
}
