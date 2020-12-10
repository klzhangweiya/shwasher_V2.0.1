using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Abp;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using IwbZero.Authorization.Users;
using IwbZero.BaseSysInfo;

namespace IwbZero.Authorization.Permissions
{
    /// <summary>
    /// Permission manager.
    /// </summary>
    public class IwbPermissionManager<TFun, TUser> :  IwbPermissionDefinitionContextBase, IIwbPermissionManager, ISingletonDependency
        where TUser:IwbSysUser<TUser>
        where TFun:IwbSysFunction<TUser>
    {
        public IAbpSession AbpSession { get; set; }

        protected readonly IIocManager IocManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public IwbPermissionManager(IIocManager iocManager)
        {
            IocManager = iocManager;
            AbpSession = NullAbpSession.Instance;
        }

        public virtual void Initialize()
        {
            using (var funRepository = IocManager.ResolveAsDisposable<IRepository<TFun, int>>())
            {
                var funs = funRepository.Object.GetAllList(a => a.IsDeleted == false);
                Initialize(funs);
            }
        }

        public virtual void Initialize(List<TFun> funs)
        {
            var topFunNo = System.Configuration.ConfigurationManager.AppSettings["SystemFunction.Top.FunctionNo"] ?? "HTSystem";
            var topFun = funs.FirstOrDefault(a => a.FunctionNo == topFunNo);
            var topPermName = topFun?.PermissionName ?? "Pages";
            var topPermission = GetPermissionOrNull(topPermName) ?? CreatePermission(topPermName);
            AddChildPermission(topPermission, funs, topFunNo);
            Permissions.AddAllPermissions();
        }

        public virtual Permission AddChildPermission(Permission permission, List<TFun> funs, string parentFunNo)
        {
            var childFuns = funs.Where(a => a.ParentNo == parentFunNo);
            foreach (var f in childFuns)
            {
                var childPermssion = permission.CreateChildPermission(f.PermissionName);
                AddChildPermission(childPermssion, funs, f.FunctionNo);
            }
            return permission;
        }

        public virtual Permission GetPermission(string name)
        {
            var permission = Permissions.GetOrDefault(name);
            if (permission == null)
            {
                throw new AbpException("There is no permission with name: " + name);
            }

            return permission;
        }

        public virtual IReadOnlyList<Permission> GetAllPermissions(bool tenancyFilter = true)
        {
            using (var featureDependencyContext = IocManager.ResolveAsDisposable<FeatureDependencyContext>())
            {
                var featureDependencyContextObject = featureDependencyContext.Object;
                return Permissions.Values
                    .WhereIf(tenancyFilter, p => p.MultiTenancySides.HasFlag(AbpSession.MultiTenancySide))
                    .Where(p =>
                        p.FeatureDependency == null ||
                        AbpSession.MultiTenancySide == MultiTenancySides.Host ||
                        p.FeatureDependency.IsSatisfied(featureDependencyContextObject)
                    ).ToImmutableList();
            }
        }

        public virtual IReadOnlyList<Permission> GetAllPermissions(MultiTenancySides multiTenancySides)
        {
            using (var featureDependencyContext = IocManager.ResolveAsDisposable<FeatureDependencyContext>())
            {
                var featureDependencyContextObject = featureDependencyContext.Object;
                return Permissions.Values
                    .Where(p => p.MultiTenancySides.HasFlag(multiTenancySides))
                    .Where(p =>
                        p.FeatureDependency == null ||
                        AbpSession.MultiTenancySide == MultiTenancySides.Host ||
                        (p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                         multiTenancySides.HasFlag(MultiTenancySides.Host)) ||
                        p.FeatureDependency.IsSatisfied(featureDependencyContextObject)
                    ).ToImmutableList();
            }
        }
    }
    
}
