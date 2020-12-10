using System;
using System.Collections.Generic;

namespace IwbZero.Caching
{
    /// <summary>
    /// Used to cache roles and permissions of a user.
    /// </summary>
    [Serializable]
    public class IwbUserPermissionCacheItem
    {
        public const string CacheStoreName = "IwbAdminUserPermissions";

        public long UserId { get; set; }

        public List<int> RoleIds { get; set; }

        public HashSet<string> GrantedPermissions { get; set; }

        public HashSet<string> ProhibitedPermissions { get; set; }

        public IwbUserPermissionCacheItem()
        {
            RoleIds = new List<int>();
            GrantedPermissions = new HashSet<string>();
            ProhibitedPermissions = new HashSet<string>();
        }

        public IwbUserPermissionCacheItem(long userId)
            : this()
        {
            UserId = userId;
        }
    }
}
