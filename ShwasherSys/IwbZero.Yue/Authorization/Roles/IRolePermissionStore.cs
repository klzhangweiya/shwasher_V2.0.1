using System.Collections.Generic;
using System.Threading.Tasks;
using IwbZero.Authorization.Permissions;
using IwbZero.Authorization.Users;

namespace IwbZero.Authorization.Roles
{
    /// <summary>
    /// Used to perform permission database operations for a role.
    /// </summary>
    public interface IIwbRolePermissionStore<in TRole>
        where TRole : RoleBase
    {
        /// <summary>
        /// Adds a permission grant setting to a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="iwbPermissionGrant">Permission grant setting info</param>
        Task AddPermissionAsync(TRole role, IwbPermissionGrantInfo iwbPermissionGrant);

        /// <summary>
        /// Removes a permission grant setting from a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="iwbPermissionGrant">Permission grant setting info</param>
        Task RemovePermissionAsync(TRole role, IwbPermissionGrantInfo iwbPermissionGrant);

        /// <summary>
        /// Gets permission grant setting informations for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>List of permission setting informations</returns>
        Task<IList<IwbPermissionGrantInfo>> GetPermissionsAsync(TRole role);

        /// <summary>
        /// Gets permission grant setting informations for a role.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>List of permission setting informations</returns>
        Task<IList<IwbPermissionGrantInfo>> GetPermissionsAsync(int roleId);

        /// <summary>
        /// Checks whether a role has a permission grant setting info.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="iwbPermissionGrant">Permission grant setting info</param>
        /// <returns></returns>
        Task<bool> HasPermissionAsync(int roleId, IwbPermissionGrantInfo iwbPermissionGrant);

        /// <summary>
        /// Deleted all permission settings for a role.
        /// </summary>
        /// <param name="role">Role</param>
        Task RemoveAllPermissionSettingsAsync(TRole role);
    }
}
