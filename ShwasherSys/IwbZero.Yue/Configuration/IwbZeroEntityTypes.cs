using System;
using Abp;
using IwbZero.Authorization.Roles;
using IwbZero.Authorization.Users;

namespace IwbZero.Configuration
{
    public class IwbZeroEntityTypes : IIwbZeroEntityTypes
    {
        public Type User
        {
            get => _user;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!typeof (UserBase).IsAssignableFrom(value))
                {
                    throw new AbpException(value.AssemblyQualifiedName + " should be derived from " + typeof(UserBase).AssemblyQualifiedName);
                }

                _user = value;
            }
        }
        private Type _user;

        public Type Role
        {
            get => _role;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!typeof(RoleBase).IsAssignableFrom(value))
                {
                    throw new AbpException(value.AssemblyQualifiedName + " should be derived from " + typeof(RoleBase).AssemblyQualifiedName);
                }

                _role = value;
            }
        }
        private Type _role;

       
    }
}