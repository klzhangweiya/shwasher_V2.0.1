using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace IwbYue.BaseSysInfo
{
    [Table("Sys_AppGuids")]
    public class SysAppGuid : Entity<int>
    {
        public short Step { get; set; }

        public int LastId { get; set; }

        public short IdType { get; set; }
    }
}
