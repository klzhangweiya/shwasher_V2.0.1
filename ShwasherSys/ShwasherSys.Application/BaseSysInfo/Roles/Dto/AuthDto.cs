using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace ShwasherSys.BaseSysInfo.Roles.Dto
{
    public class AuthDto : EntityDto<int>
    {
        public List<string> PermissionNames { get; set; }
    }
}
