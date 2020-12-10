using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace IwbZero.Authorization.Permissions
{
    [Table("Sys_UserRoles")]
    public class SysUserRole : CreationAuditedEntity<long>
    {

        /// <summary>
        /// User id.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Role id.
        /// </summary>
        public virtual int RoleId { get; set; }

        /// <summary>
        /// Creates a new <see cref="UserRole"/> object.
        /// </summary>
        public SysUserRole()
        {

        }

        /// <summary>
        /// Creates a new <see cref="UserRole"/> object.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roleId">Role id</param>
        public SysUserRole(long userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
