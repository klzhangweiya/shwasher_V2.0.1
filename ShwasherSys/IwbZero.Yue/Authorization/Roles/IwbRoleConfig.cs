using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.MultiTenancy;
using IwbZero.Configuration;

namespace IwbZero.Authorization.Roles
{
    public class IwbRoleConfig
    {
        public static void Configure(IIwbRoleManagementConfig roleManagementConfig)
        {
            //Static host roles

            roleManagementConfig.StaticRoles.Add(
                new IwbStaticRoleDefinition(
                    RoleBase.AdminRoleName,
                    MultiTenancySides.Host)
            );

            //Static tenant roles

            //roleManagementConfig.StaticRoles.Add(
            //    new StaticRoleDefinition(
            //        StaticRoleNames.Tenants.Admin,
            //        MultiTenancySides.Tenant)
            //);
        }
    }

    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Admin = "Admin";
        }

        //public static class Tenants
        //{
        //    public const string Admin = "Admin";
        //}
    }
}
