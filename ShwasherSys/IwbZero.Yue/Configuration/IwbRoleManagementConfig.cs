using System.Collections.Generic;

namespace IwbZero.Configuration
{
    internal class IwbRoleManagementConfig : IIwbRoleManagementConfig
    {
        public List<IwbStaticRoleDefinition> StaticRoles { get; }

        public IwbRoleManagementConfig()
        {
            StaticRoles = new List<IwbStaticRoleDefinition>();
        }
    }
}
