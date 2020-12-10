using System.Collections.Generic;

namespace IwbZero.Configuration
{
    public interface IRoleManagementConfig
    {
        List<IwbStaticRoleDefinition> StaticRoles { get; }
    }
}