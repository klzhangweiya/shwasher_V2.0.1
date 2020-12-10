using System;
using Abp.Runtime.Session;

namespace IwbZero.Session
{
    public interface IIwbSession : IAbpSession
    {
        string UserName { get; }
        string RealName { get; }
        string[] UserRoles { get; }
        int? UserType { get; }
        bool? RememberMe { get; }
        DateTimeOffset? ExpireTime { get; }

        string EmailAddress { get; }

        string EmployeeNo { get; }
    }
}
