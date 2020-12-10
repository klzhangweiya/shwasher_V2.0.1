using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.BaseSysInfo.Functions.Dto
{
    public class MoveUpFunctionDto : Entity<int>
    {
        [Required]
        public int PrevId { get; set; }
    }
    public class MoveDownFunctionDto : Entity<int>
    {
        [Required]
        public int NextId { get; set; }
    }
}
