using Abp;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Localization;
using Abp.MultiTenancy;

namespace IwbZero.Authorization.Permissions
{

    public class IwbPermissionDefinitionContextBase : IPermissionDefinitionContext
    {
        protected readonly IwbPermissionDictionary Permissions;

        protected IwbPermissionDefinitionContextBase()
        {
            Permissions = new IwbPermissionDictionary();
        }

        public Permission CreatePermission(
            string name,
            ILocalizableString displayName = null,
            ILocalizableString description = null,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant,
            IFeatureDependency featureDependency = null)
        {
            if (Permissions.ContainsKey(name))
            {
                throw new AbpException("There is already a permission with name: " + name);
            }

            var permission = new Permission(name, displayName, description, multiTenancySides, featureDependency);
            Permissions[permission.Name] = permission;
            return permission;
        }

        public Permission GetPermissionOrNull(string name)
        {
            return Permissions.GetOrDefault(name);
        }

        public void RemovePermission(string name)
        {
            Permissions.Remove(name);
        }
    }
}
