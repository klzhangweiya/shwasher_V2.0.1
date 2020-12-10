using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;

namespace ShwasherSys.BaseSysInfo.Roles.Dto
{
    [AutoMapFrom(typeof(Permission))]
    public class PermissionDto : EntityDto<long>
    {
        public PermissionDto Parent { get; set; }
        public string Name { get; set; }
        public string PermDisplayName { get; set; }
        public int Sort { get; set; }
        public bool IsAuth { get; set; }
        public List<PermissionDto> Children { get; set; }


    }
}
