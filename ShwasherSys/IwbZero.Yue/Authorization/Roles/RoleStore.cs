using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using IwbZero.Authorization.Permissions;
using IwbZero.Authorization.Users;
using Microsoft.AspNet.Identity;

namespace IwbZero.Authorization.Roles
{
    public abstract class IwbRoleStore<TRole,TUser> :
        IQueryableRoleStore<TRole, int>,
        IIwbRolePermissionStore<TRole>,
        ITransientDependency
        where TUser : IwbSysUser<TUser>
        where TRole : IwbSysRole<TUser>


    {
        private readonly IRepository<TRole> _roleRepository;
        private readonly IRepository<SysUserRole, long> _userRoleRepository;
        private readonly IRepository<SysPermission, long> _rolePermissionSettingRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        public IwbRoleStore(
            IRepository<TRole> roleRepository,
            IRepository<SysUserRole, long> userRoleRepository,
            IRepository<SysPermission, long> rolePermissionSettingRepository)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionSettingRepository = rolePermissionSettingRepository;
        }

        public virtual IQueryable<TRole> Roles => _roleRepository.GetAll();

        public virtual async Task CreateAsync(TRole role)
        {
            await _roleRepository.InsertAsync(role);
        }

        public virtual async Task UpdateAsync(TRole role)
        {
            await _roleRepository.UpdateAsync(role);
        }

        public virtual async Task DeleteAsync(TRole role)
        {
            await _userRoleRepository.DeleteAsync(ur => ur.RoleId == role.Id);
            await _roleRepository.DeleteAsync(role);
        }

        public virtual async Task<TRole> FindByIdAsync(int roleId)
        {
            return await _roleRepository.FirstOrDefaultAsync(roleId);
        }

        public virtual async Task<TRole> FindByNameAsync(string roleName)
        {
            return await _roleRepository.FirstOrDefaultAsync(
                role => role.Name == roleName
                );
        }

        public virtual async Task<TRole> FindByDisplayNameAsync(string displayName)
        {
            return await _roleRepository.FirstOrDefaultAsync(
                role => role.RoleDisplayName == displayName
                );
        }

        public virtual async Task AddPermissionAsync(TRole role, IwbPermissionGrantInfo iwbPermissionGrant)
        {
            if (await HasPermissionAsync(role.Id, iwbPermissionGrant))
            {
                return;
            }

            await _rolePermissionSettingRepository.InsertAsync(
                new SysPermission()
                {
                    PermissionNo = Guid.NewGuid().ToString("N"),
                    Master = 2,
                    MasterValue = role.Id + "",
                    PermissionName = iwbPermissionGrant.Name,
                    IsGranted = iwbPermissionGrant.IsGranted
                });
        }

        public virtual async Task RemovePermissionAsync(TRole role, IwbPermissionGrantInfo iwbPermissionGrant)
        {
            await _rolePermissionSettingRepository.DeleteAsync(
                p => p.Master == 2 &&
                     p.MasterValue == role.Id + "" &&
                     p.PermissionName == iwbPermissionGrant.Name &&
                     p.IsGranted == iwbPermissionGrant.IsGranted
            );
        }

        public virtual Task<IList<IwbPermissionGrantInfo>> GetPermissionsAsync(TRole role)
        {
            return GetPermissionsAsync(role.Id);
        }

        public async Task<IList<IwbPermissionGrantInfo>> GetPermissionsAsync(int roleId)
        {
            return (await _rolePermissionSettingRepository.GetAllListAsync(p => p.Master == 2 && p.MasterValue == roleId + ""))
                .Select(p => new IwbPermissionGrantInfo(p.PermissionName, p.IsGranted))
                .ToList();
        }

        public virtual async Task<bool> HasPermissionAsync(int roleId, IwbPermissionGrantInfo iwbPermissionGrant)
        {
            return await _rolePermissionSettingRepository.FirstOrDefaultAsync(
                p => p.Master == 2 &&
                     p.MasterValue == roleId + "" &&
                     p.PermissionName == iwbPermissionGrant.Name &&
                     p.IsGranted == iwbPermissionGrant.IsGranted
                ) != null;
        }

        public virtual async Task RemoveAllPermissionSettingsAsync(TRole role)
        {
            await _rolePermissionSettingRepository.DeleteAsync(p => p.Master == 2 && p.MasterValue == role.Id + "");
        }

        public virtual void Dispose()
        {
            //No need to dispose since using IOC.
        }
    }
}
