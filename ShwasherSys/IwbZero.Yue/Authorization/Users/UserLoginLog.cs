using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;

namespace IwbZero.Authorization.Users
{/// <summary>
 /// Used to save a login attempt of a user.
 /// </summary>
    [Table("Sys_UserLoginLogs")]
    public class UserLoginAttempt : Entity<long>, IHasCreationTime
    {

        public const int MaxUserNameOrEmailAddressLength = 255;

        /// <summary>
        /// Maximum length of <see cref="ClientIpAddress"/> property.
        /// </summary>
        public const int MaxClientIpAddressLength = 64;

        /// <summary>
        /// Maximum length of <see cref="ClientName"/> property.
        /// </summary>
        public const int MaxClientNameLength = 128;

        /// <summary>
        /// Maximum length of <see cref="BrowserInfo"/> property.
        /// </summary>
        public const int MaxBrowserInfoLength = 512;

        /// <summary>
        /// User's Id, if <see cref="UserNameOrEmailAddress"/> was a valid username or email address.
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// User name or email address
        /// </summary>
        [MaxLength(MaxUserNameOrEmailAddressLength)]
        public string UserNameOrEmailAddress { get; set; }

        /// <summary>
        /// IP address of the client.
        /// </summary>
        [MaxLength(MaxClientIpAddressLength)]
        public string ClientIpAddress { get; set; }

        /// <summary>
        /// Name (generally computer name) of the client.
        /// </summary>
        [MaxLength(MaxClientNameLength)]
        public string ClientName { get; set; }

        /// <summary>
        /// Browser information if this method is called in a web request.
        /// </summary>
        [MaxLength(MaxBrowserInfoLength)]
        public string BrowserInfo { get; set; }

        /// <summary>
        /// Login attempt result.
        /// </summary>
        public AbpLoginResultType Result { get; set; }

        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginAttempt"/> class.
        /// </summary>
        public UserLoginAttempt()
        {
            CreationTime = Clock.Now;
        }
    }
}
