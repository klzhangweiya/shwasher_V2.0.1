using System;
using System.Collections.Generic;

namespace IwbZero.Caching
{
    /// <summary>
    /// Used to cache permissions of a role.
    /// </summary>
    [Serializable]
    public class IwbRolePermissionCacheItem
    {
        public const string CacheStoreName = "IwbAdminRolePermissions";

        public long RoleId { get; set; }

        public HashSet<string> GrantedPermissions { get; set; }

        public IwbRolePermissionCacheItem()
        {
            GrantedPermissions = new HashSet<string>();
        }

        public IwbRolePermissionCacheItem(int roleId)
            : this()
        {
            RoleId = roleId;
        }
    }
}
