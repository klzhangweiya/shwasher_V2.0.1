using System;
using System.ComponentModel.DataAnnotations;
using Abp.Extensions;
using IwbZero.Authorization.Users;
using Microsoft.AspNet.Identity;

namespace ShwasherSys.Authorization.Users
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class SysUser : IwbSysUser<SysUser>
    {

        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static SysUser CreateAdminUser(string emailAddress, string password = null)
        {
            password = password ?? DefaultPassword;
            var user = new SysUser()
            {
                UserName = AdminUserName,
                RealName = AdminUserName,
                EmailAddress = emailAddress,
                Password = new PasswordHasher().HashPassword(password)
            };
            return user;
        }
#region  shwasherExtend 
        public const int FactoryIDMaxLength = 20;
        public const int DepartmentIDMaxLength = 20;
        public const int DutyIDMaxLength = 20;
        public const int AddressMaxLength = 200;
        public const int ZIPMaxLength = 50;
        public const int RemarkMaxLength = 500;

        [StringLength(FactoryIDMaxLength)]
        public string FactoryID { get; set; }
        [StringLength(DepartmentIDMaxLength)]
        public string DepartmentID { get; set; }
        
        public int? DutyID { get; set; }

        [MaxLength(20)]
        public string AccountNo { get; set; }

        public DateTime? Birthday { get; set; }
        [StringLength(AddressMaxLength)]
        public string Address { get; set; }
        [StringLength(ZIPMaxLength)]
        public string ZIP { get; set; }
        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }
#endregion

    }

   
}
