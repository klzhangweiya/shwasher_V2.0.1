using System.Linq;
using ShwasherSys.Authorization.Roles;
using ShwasherSys.Authorization.Users;
using ShwasherSys.EntityFramework;
using IwbZero.Authorization.Permissions;
using IwbZero.Authorization.Roles;
using IwbZero.Authorization.Users;
using Microsoft.AspNet.Identity;

namespace ShwasherSys.Migrations.SeedData
{
    public class DefaultRoleAndUserCreator
    {
        private readonly ShwasherDbContext _context;

        public DefaultRoleAndUserCreator(ShwasherDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateAdminRoleAndUsers();
            CreateSystemRoleAndUsers();
        }
        #region Admin

        private void CreateAdminRoleAndUsers()
        {
            //Admin role for host

            var adminRole = _context.Roles.FirstOrDefault(r => r.Name == RoleBase.AdminRoleName);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new SysRole
                {
                    Name = RoleBase.AdminRoleName,
                    RoleDisplayName = RoleBase.AdminRoleDisplayName,
                    IsStatic = true,
                    RoleType = 1
                });
                _context.SaveChanges();
                AddRolePermission(adminRole.Id);
            }
            else
            {
                AddRolePermission(adminRole.Id);
            }

            //Admin user 
            var adminUser = _context.Users.FirstOrDefault(u => u.UserName == UserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = _context.Users.Add(
                    new SysUser
                    {
                        UserName = UserBase.AdminUserName,
                        RealName = "Administrator",
                        //AccountType = 1,
                        UserType = 1,
                        //Surname = "Administrator",
                        EmailAddress = "admin@iwbnet.com",
                        IsEmailConfirmed = true,
                        Password = new PasswordHasher().HashPassword(SysUser.DefaultPassword)
                    });
                _context.SaveChanges();
                AddUserPermission(adminUser.Id);
                _context.UserRoles.Add(new SysUserRole(adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }
            else
            {
                AddUserPermission(adminUser.Id);
                _context.SaveChanges();
            }
        }
        private void CreateSystemRoleAndUsers()
        {
            //System role for host
            var systemRole = _context.Roles.FirstOrDefault(r => r.Name == "System");
            if (systemRole == null)
            {
                systemRole = _context.Roles.Add(new SysRole
                {
                    Name = "System",
                    RoleDisplayName = "System",
                    IsStatic = true,
                    RoleType = 2
                });
                _context.SaveChanges();
                AddRolePermission(systemRole.Id);
            }
            else
            {
                AddRolePermission(systemRole.Id);
            }

            //System user 
            var systemUser = _context.Users.FirstOrDefault(u => u.UserName == "System");
            if (systemUser == null)
            {
                systemUser = _context.Users.Add(
                    new SysUser
                    {
                        UserName = "System",
                        RealName = "SystemManager",
                        //Surname = "Administrator",
                        //AccountType = 1,
                        UserType = 2,
                        EmailAddress = "System@iwbnet.com",
                        IsEmailConfirmed = true,
                        Password = new PasswordHasher().HashPassword("system")
                    });
                _context.SaveChanges();
                _context.UserRoles.Add(new SysUserRole(systemUser.Id, systemRole.Id));
                _context.SaveChanges();
            }


        }

        /// <summary>
        /// 添加用户权限
        /// </summary>
        /// <param name="userId"></param>
        private void AddUserPermission(long userId)
        {
            var funs = _context.Functions.Where(a => a.IsDeleted == false).OrderBy(a => a.CreationTime);
            foreach (var fun in funs)
            {
                //if (_context.Permissions.FirstOrDefault(a => a.Master == 1 && a.MasterValue == userId + "") == null) 
                //{
                //}
                _context.Permissions.Add(
                    new SysPermission
                    {
                        PermissionName = fun.PermissionName,
                        IsGranted = true,
                        Master = 1,
                        MasterValue = userId + ""
                    });
            }
            _context.SaveChanges();
        }
        /// <summary>
        /// 添加角色权限
        /// </summary>
        /// <param name="roleId"></param>
        private void AddRolePermission(int roleId)
        {
            var funs = _context.Functions.Where(a => a.IsDeleted == false).OrderBy(a => a.CreationTime);
            foreach (var fun in funs)
            {
                //if (_context.Permissions.FirstOrDefault(a => a.Master == 2 && a.MasterValue == roleId + "") == null) 
                //{
                //}
                _context.Permissions.Add(
                    new SysPermission
                    {
                        PermissionName = fun.PermissionName,
                        IsGranted = true,
                        Master = 2,
                        MasterValue = roleId + ""
                    });
            }
            _context.SaveChanges();
        }

        #endregion
    }
}