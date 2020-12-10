﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using Abp.Domain.Entities;

namespace IwbZero.BaseSysInfo
{
    [Table("Sys_AuditLogs")]
    public class IwbSysLog : Entity<long>
    {
        /// <summary>
        /// Maximum length of <see cref="ServiceName"/> property.
        /// </summary>
        public static int MaxServiceNameLength = 256;

        /// <summary>
        /// Maximum length of <see cref="MethodName"/> property.
        /// </summary>
        public static int MaxMethodNameLength = 256;

        /// <summary>
        /// Maximum length of <see cref="Parameters"/> property.
        /// </summary>
        public static int MaxParametersLength = 1024;

        /// <summary>
        /// Maximum length of <see cref="ClientIpAddress"/> property.
        /// </summary>
        public static int MaxClientIpAddressLength = 64;

        /// <summary>
        /// Maximum length of <see cref="ClientName"/> property.
        /// </summary>
        public static int MaxClientNameLength = 128;

        /// <summary>
        /// Maximum length of <see cref="BrowserInfo"/> property.
        /// </summary>
        public static int MaxBrowserInfoLength = 512;

        /// <summary>
        /// Maximum length of <see cref="Exception"/> property.
        /// </summary>
        public static int MaxExceptionLength = 2000;

        /// <summary>
        /// Maximum length of <see cref="CustomData"/> property.
        /// </summary>
        public static int MaxCustomDataLength = 2000;

        ///// <summary>
        ///// TenantId.
        ///// </summary>
        //public virtual int? TenantId { get; set; }

        /// <summary>
        /// UserId.
        /// </summary>
        public virtual long? UserId { get; set; }
        /// <summary>
        /// UserId.
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Service (class/interface) name.
        /// </summary>
        public virtual string ServiceName { get; set; }

        /// <summary>
        /// Executed method name.
        /// </summary>
        public virtual string MethodName { get; set; }

        /// <summary>
        /// Calling parameters.
        /// </summary>
        public virtual string Parameters { get; set; }

        /// <summary>
        /// Start time of the method execution.
        /// </summary>
        public virtual DateTime ExecutionTime { get; set; }

        /// <summary>
        /// Total duration of the method call as milliseconds.
        /// </summary>
        public virtual int ExecutionDuration { get; set; }

        /// <summary>
        /// IP address of the client.
        /// </summary>
        public virtual string ClientIpAddress { get; set; }

        /// <summary>
        /// Name (generally computer name) of the client.
        /// </summary>
        public virtual string ClientName { get; set; }

        /// <summary>
        /// Browser information if this method is called in a web request.
        /// </summary>
        public virtual string BrowserInfo { get; set; }

        /// <summary>
        /// Exception object, if an exception occured during execution of the method.
        /// </summary>
        public virtual string Exception { get; set; }

        /// <summary>
        /// <see cref="AuditInfo.ImpersonatorUserId"/>.
        /// </summary>
        public virtual long? ImpersonatorUserId { get; set; }

        /// <summary>
        /// <see cref="AuditInfo.ImpersonatorTenantId"/>.
        /// </summary>
        public virtual int? ImpersonatorTenantId { get; set; }

        
        public virtual string CustomData { get; set; }
        public virtual int LogType { get; set; }


        public virtual IwbSysLog CreateFromAuditInfo(AuditInfo auditInfo)
        {
            return null;
        }
        public override string ToString()
        {
            return
                $"AUDIT LOG: {ServiceName}.{MethodName} is executed by user {UserId} in {ExecutionDuration} ms from {ClientIpAddress} IP address.";
        }

    }
}
