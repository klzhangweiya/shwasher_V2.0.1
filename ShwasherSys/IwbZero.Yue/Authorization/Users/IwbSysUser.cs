using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;
using IwbZero.Authorization.Permissions;
using Microsoft.AspNet.Identity;

namespace IwbZero.Authorization.Users
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public abstract class IwbSysUser<TUser> : UserBase, IUser<long>, IFullAudited<TUser>
        where TUser : IwbSysUser<TUser>
    {
        public virtual TUser DeleterUser { get; set; }

        public virtual TUser CreatorUser { get; set; }

        public virtual TUser LastModifierUser { get; set; }


        //public static IwbUser CreateAdminUser(string emailAddress, string password = null)
        //{
        //    password = password ?? DefaultPassword;
        //    var user = new IwbUser()
        //    {
        //        UserName = AdminUserName,
        //        RealName = AdminUserName,
        //        EmailAddress = emailAddress,
        //        Password = new PasswordHasher().HashPassword(password)
        //    };
        //    return user;
        //}
    }

    

    [Table("Sys_Users")]
    public abstract class UserBase : FullAuditedEntity<long>, IPassivable
    {
        /// <summary>
        /// Maximum length of the <see cref="UserName"/> property.
        /// </summary>
        public const int MaxUserNameLength = 32;

        /// <summary>
        /// Maximum length of the <see cref="EmailAddress"/> property.
        /// </summary>
        public const int MaxEmailAddressLength = 256;

        /// <summary>
        /// Maximum length of the <see cref="RealName"/> property.
        /// </summary>
        public const int MaxNameLength = 32;

        ///// <summary>
        ///// Maximum length of the <see cref="Surname"/> property.
        ///// </summary>
        //public const int MaxSurnameLength = 32;

        /// <summary>
        /// Maximum length of the <see cref="AuthenticationSource"/> property.
        /// </summary>
        public const int MaxAuthenticationSourceLength = 64;

        /// <summary>
        /// UserName of the admin.
        /// admin can not be deleted and UserName of the admin can not be changed.
        /// </summary>
        public const string AdminUserName = "admin";
        public const string SystemUserName = "system";

        /// <summary>
        /// Maximum length of the <see cref="Password"/> property.
        /// </summary>
        public const int MaxPasswordLength = 128;

        /// <summary>
        /// Maximum length of the <see cref="Password"/> without hashed.
        /// </summary>
        public const int MaxPlainPasswordLength = 32;

        /// <summary>
        /// Maximum length of the <see cref="EmailConfirmationCode"/> property.
        /// </summary>
        public const int MaxEmailConfirmationCodeLength = 328;

        /// <summary>
        /// Maximum length of the <see cref="PasswordResetCode"/> property.
        /// </summary>
        public const int MaxPasswordResetCodeLength = 328;

        /// <summary>
        /// Maximum length of the <see cref="PhoneNumber"/> property.
        /// </summary>
        public const int MaxPhoneNumberLength = 32;

        /// <summary>
        /// Maximum length of the <see cref="SecurityStamp"/> property.
        /// </summary>
        public const int MaxSecurityStampLength = 128;


        /// <summary>
        /// User name.
        /// User name must be unique for it's tenant.
        /// </summary>
        [Required]
        [StringLength(MaxUserNameLength)]
        public virtual string UserName { get; set; }
        public virtual int UserType { get; set; }

        /// <summary>
        /// Authorization source name.
        /// It's set to external authentication source name if created by an external source.
        /// Default: null.
        /// </summary>
        [MaxLength(MaxAuthenticationSourceLength)]
        public virtual string AuthenticationSource { get; set; }

        /// <summary>
        /// Email address of the user.
        /// Email address must be unique for it's tenant.
        /// </summary>
        [Required]
        [StringLength(MaxEmailAddressLength)]
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string RealName { get; set; }

        ///// <summary>
        ///// Surname of the user.
        ///// </summary>
        //[Required]
        //[StringLength(MaxSurnameLength)]
        //public virtual string Surname { get; set; }

        ///// <summary>
        ///// Return full name (Name Surname )
        ///// </summary>
        //[NotMapped]
        //public virtual string FullName { get { return this.RealName + " " + this.Surname; } }

        /// <summary>
        /// Password of the user.
        /// </summary>
        [Required]
        [StringLength(MaxPasswordLength)]
        public virtual string Password { get; set; }

        /// <summary>
        /// Confirmation code for email.
        /// </summary>
        [StringLength(MaxEmailConfirmationCodeLength)]
        public virtual string EmailConfirmationCode { get; set; }

        /// <summary>
        /// Reset code for password.
        /// It's not valid if it's null.
        /// It's for one usage and must be set to null after reset.
        /// </summary>
        [StringLength(MaxPasswordResetCodeLength)]
        public virtual string PasswordResetCode { get; set; }

        /// <summary>
        /// Lockout end date.
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the access failed count.
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Gets or sets the lockout enabled.
        /// </summary>
        public virtual bool IsLockoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [StringLength(MaxPhoneNumberLength)]
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Is the <see cref="PhoneNumber"/> confirmed.
        /// </summary>
        public virtual bool IsPhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the security stamp.
        /// </summary>
        [StringLength(MaxSecurityStampLength)]
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// Is two factor auth enabled.
        /// </summary>
        public virtual bool IsTwoFactorEnabled { get; set; }


        /// <summary>
        /// Roles of this user.
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<SysUserRole> Roles { get; set; }


        /// <summary>
        /// Is the <see cref="AbpUserBase.EmailAddress"/> confirmed.
        /// </summary>
        public virtual bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Is this user active?
        /// If as user is not active, he/she can not use the application.
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// The last time this user entered to the system.
        /// </summary>
        public virtual DateTime? LastLoginTime { get; set; }

        protected UserBase()
        {
            IsActive = true;
            IsLockoutEnabled = true;
            SecurityStamp = SequentialGuidGenerator.Instance.Create().ToString();
        }

        public virtual void SetNewPasswordResetCode()
        {
            PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(MaxPasswordResetCodeLength);
        }

        public virtual void SetNewEmailConfirmationCode()
        {
            EmailConfirmationCode = Guid.NewGuid().ToString("N").Truncate(MaxEmailConfirmationCodeLength);
        }

        /// <summary>
        /// Creates <see cref="UserIdentifier"/> from this User.
        /// </summary>
        /// <returns></returns>
        public virtual UserIdentifier ToUserIdentifier()
        {
            return new UserIdentifier(null, Id);
        }

        public override string ToString()
        {
            return $"[User {Id}] {UserName}";
        }
    }
}
