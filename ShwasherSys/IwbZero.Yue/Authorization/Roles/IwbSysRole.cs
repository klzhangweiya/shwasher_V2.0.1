using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using IwbZero.Authorization.Users;
using Microsoft.AspNet.Identity;

namespace IwbZero.Authorization.Roles
{

    public abstract class IwbSysRole<TUser> : RoleBase, IRole<int>, IFullAudited<TUser>
        where TUser : IwbSysUser<TUser>
    {
        public virtual TUser CreatorUser { get; set; }
        public virtual TUser LastModifierUser { get; set; }
        public virtual TUser DeleterUser { get; set; }
    }

    [Table("Sys_Roles")]
    public abstract class RoleBase : FullAuditedEntity<int>
    {
        public const string AdminRoleName = "admin";
        public const string AdminRoleDisplayName = "admin";

        /// <summary>
        /// Maximum length of the <see cref="RoleDisplayName"/> property.
        /// </summary>
        public const int MaxDisplayNameLength = 64;
        public const int MaxDescriptionLength = 1000;


        /// <summary>
        /// Maximum length of the <see cref="Name"/> property.
        /// </summary>
        public const int MaxNameLength = 32;
        /// <summary>
        /// Unique name of this role.
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Display name of this role.
        /// </summary>
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string RoleDisplayName { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        public int RoleType { get; set; }

        /// <summary>
        /// Is this a static role?
        /// Static roles can not be deleted, can not change their name.
        /// They can be used programmatically.
        /// </summary>
        public virtual bool IsStatic { get; set; }

        /// <summary>
        /// Is this role will be assigned to new users as default?
        /// </summary>
        public virtual bool IsDefault { get; set; }

        ///// <summary>
        ///// List of permissions of the role.
        ///// </summary>
        //[ForeignKey("RoleId")]
        //public virtual ICollection<RolePermissionSetting> Permissions { get; set; }

        protected RoleBase()
        {
            Name = Guid.NewGuid().ToString("N");
        }

        protected RoleBase(string roleDisplayName)
            : this()
        {
            RoleDisplayName = roleDisplayName;
        }

        protected RoleBase(string name, string displayName)
            : this(displayName)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"[Role {Id}, Name={Name}]";
        }
    }
}
