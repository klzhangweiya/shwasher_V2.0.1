﻿using System.Collections.Generic;
using System.Linq;
using Abp;
using Abp.Authorization;

namespace IwbZero.Authorization.Permissions
{
    public class IwbPermissionDictionary : Dictionary<string, Permission>
    {
        /// <summary>
        /// Adds all child permissions of current permissions recursively.
        /// </summary>
        public void AddAllPermissions()
        {
            foreach (var permission in Values.ToList())
            {
                AddPermissionRecursively(permission);
            }
        }

        /// <summary>
        /// Adds a permission and it's all child permissions to dictionary.
        /// </summary>
        /// <param name="permission">Permission to be added</param>
        private void AddPermissionRecursively(Permission permission)
        {
            //Prevent multiple adding of same named permission.
            if (TryGetValue(permission.Name, out var existingPermission))
            {
                if (existingPermission != permission)
                {
                    throw new AbpInitializationException("Duplicate permission name detected for " + permission.Name);
                }
            }
            else
            {
                this[permission.Name] = permission;
            }

            //Add child permissions (recursive call)
            foreach (var childPermission in permission.Children)
            {
                AddPermissionRecursively(childPermission);
            }
        }
    }
}
