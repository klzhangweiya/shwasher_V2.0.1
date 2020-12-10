using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IwbZero.Authorization.Permissions;

namespace IwbZero.Authorization.Users
{
    /// <summary>
    /// Used to perform permission database operations for a user.
    /// </summary>
    public interface IIwbUserPermissionStore<in TUser>
        where TUser : UserBase
    {
        /// <summary>
        /// Adds a permission grant setting to a user.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="iwbPermissionGrant">Permission grant setting info</param>
        Task AddPermissionAsync(TUser user, IwbPermissionGrantInfo iwbPermissionGrant);

        /// <summary>
        /// Removes a permission grant setting from a user.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="iwbPermissionGrant">Permission grant setting info</param>
        Task RemovePermissionAsync(TUser user, IwbPermissionGrantInfo iwbPermissionGrant);

        /// <summary>
        /// Gets permission grant setting informations for a user.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List of permission setting informations</returns>
        Task<IList<IwbPermissionGrantInfo>> GetPermissionsAsync(long userId);

        /// <summary>
        /// Checks whether a role has a permission grant setting info.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="iwbPermissionGrant">Permission grant setting info</param>
        /// <returns></returns>
        Task<bool> HasPermissionAsync(long userId, IwbPermissionGrantInfo iwbPermissionGrant);

        /// <summary>
        /// Deleted all permission settings for a role.
        /// </summary>
        /// <param name="user">User</param>
        Task RemoveAllPermissionSettingsAsync(TUser user);
    }
}
