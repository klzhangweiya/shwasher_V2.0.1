using System.Collections.Generic;
using Abp.Authorization;
using Abp.MultiTenancy;

namespace IwbZero.Configuration
{
    public class IwbStaticRoleDefinition
    {
        public string RoleName { get; }

        public bool GrantAllPermissionsByDefault { get; set; }

        public List<string> GrantedPermissions { get; }

        public MultiTenancySides Side { get; }

        public IwbStaticRoleDefinition(string roleName, MultiTenancySides side, bool grantAllPermissionsByDefault = false)
        {
            RoleName = roleName;
            Side = side;
            GrantAllPermissionsByDefault = grantAllPermissionsByDefault;
            GrantedPermissions = new List<string>();
        }

        public virtual bool IsGrantedByDefault(Permission permission)
        {
            return GrantAllPermissionsByDefault || GrantedPermissions.Contains(permission.Name);
        }
    }
}