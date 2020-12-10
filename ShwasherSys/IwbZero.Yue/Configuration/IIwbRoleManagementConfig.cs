using System.Collections.Generic;

namespace IwbZero.Configuration
{

    public interface IIwbRoleManagementConfig
    {
        List<IwbStaticRoleDefinition> StaticRoles { get; }
    }


}
