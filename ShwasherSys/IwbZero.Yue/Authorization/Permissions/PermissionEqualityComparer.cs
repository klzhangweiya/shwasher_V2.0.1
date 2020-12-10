using System.Collections.Generic;
using Abp.Authorization;

namespace IwbZero.Authorization.Permissions
{

    internal class IwbPermissionEqualityComparer : IEqualityComparer<Permission>
    {
        public static IwbPermissionEqualityComparer Instance => new IwbPermissionEqualityComparer();
        //private static readonly PermissionEqualityComparer _instance = new PermissionEqualityComparer();

        public bool Equals(Permission x, Permission y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return Equals(x.Name, y.Name);
        }

        public int GetHashCode(Permission permission)
        {
            return permission.Name.GetHashCode();
        }
    }
}
