using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Caching;
using ShWasher.BaseCore.Authorization.Users;
using ShWasher.BaseCore.BaseSysInfo;

namespace ShWasher.BaseCore.Auditing
{
    /// <summary>
    /// Implements <see cref="IAuditingStore"/> to save auditing informations to database.
    /// </summary>
    public class IwbAuditingStore : IAuditingStore, ITransientDependency
    {
        private readonly IRepository<SysLog, long> _auditLogRepository;
        private readonly IRepository<SysUser, long> _userRepository;
        private readonly IRepository<SysFunction, int> _funRepository;
        private readonly ICacheManager _cacheManager;

        private SysFunction SysFunction { get; set; }
        private Type Type { get; set; }
        private string MethondNameSuffix { get; set; }

        /// <summary>
        /// Creates  a new <see cref="IwbAuditingStore"/>.
        /// </summary>
        public IwbAuditingStore(IRepository<SysLog, long> auditLogRepository, IRepository<SysUser, long> userRepository, IRepository<SysFunction, int> funRepository, ICacheManager cacheManager)
        {
            SysFunction = null;
            Type = null;
            _auditLogRepository = auditLogRepository;
            _userRepository = userRepository;
            _funRepository = funRepository;
            _cacheManager = cacheManager;
        }

        public virtual Task SaveAsync(AuditInfo auditInfo)
        {
            var user = _cacheManager.GetCache(IwbConsts.SystemUserCache).Get(auditInfo.UserId, () =>
                 _userRepository.FirstOrDefault(a => a.Id == auditInfo.UserId));
            string userName = user?.UserName ?? "";
            if (auditInfo.MethodName.ToLower() == "login" || auditInfo.MethodName.ToLower().Contains("password"))
            {
                auditInfo.Parameters = "";
            }
            SysFunction = null;
            Type = null;
            MethondNameSuffix = "";
            var methodName = auditInfo.MethodName;
            var serviceName = _cacheManager.GetCache(IwbConsts.AuditLogDescCache).Get(
                auditInfo.ServiceName, () => GetServiceName(auditInfo.ServiceName));
            if (string.IsNullOrEmpty(serviceName))
            {
                serviceName = GetServiceName(auditInfo.ServiceName);
                if (string.IsNullOrEmpty(serviceName))
                    _cacheManager.GetCache(IwbConsts.AuditLogDescCache).Set(auditInfo.ServiceName, serviceName);
            }
            int logType = auditInfo.ServiceName == serviceName ? 0 : 1;
            if (logType != 0)
            {
                methodName = _cacheManager.GetCache(IwbConsts.AuditLogDescCache).Get(
                    auditInfo.ServiceName + "." + auditInfo.MethodName,
                    () => GetMethodName(auditInfo.MethodName, auditInfo.ServiceName));
                if (string.IsNullOrEmpty(methodName))
                {
                    methodName = GetMethodName(auditInfo.MethodName, auditInfo.ServiceName);
                    if (string.IsNullOrEmpty(methodName))
                        _cacheManager.GetCache(IwbConsts.AuditLogDescCache).Set(auditInfo.ServiceName + "." + auditInfo.MethodName, methodName);
                }
                logType = auditInfo.MethodName == methodName ? 0 : 1;
            }
            auditInfo.ServiceName = serviceName;
            auditInfo.MethodName = methodName;
            return _auditLogRepository.InsertAsync(SysLog.CreateFromAuditInfo(auditInfo, userName, logType));
        }

        private string GetServiceName(string serviceName)
        {
            if (serviceName.IsNullOrEmpty())
            {
                return null;
            }
            try
            {
                var funNoArray = serviceName.Split(".", StringSplitOptions.RemoveEmptyEntries);
                var funNo = funNoArray[funNoArray.Length - 1]?.Replace("sAppService", "").Replace("AppService", "").Replace("Controller", "");
                SysFunction = _funRepository.FirstOrDefault(a => a.FunctionNo == funNo);
                if (SysFunction == null)
                {
                    return GetServiceNameByAttr(serviceName);
                }
                return SysFunction.FunctionName;
            }
            catch (Exception e)
            {
                this.LogError(e);
                return null;
            }

        }

        private string GetServiceNameByAttr(string serviceName)
        {
            Type = Typen(serviceName);
            var attr = Type.GetSingleAttribute<AuditLogAttribute>();
            if (attr != null)
            {
                serviceName = attr.Name;
                MethondNameSuffix = attr.MethondNameSuffix ?? "";
            }
            return serviceName;
        }

        private string GetMethodName(string methodName, string serviceName)
        {
            if (methodName.IsNullOrEmpty())
            {
                return null;
            }
            try
            {

                if (SysFunction == null)
                {
                    return GetMethodNameByAttr(methodName, serviceName);
                }
                var funNoArray = methodName.Split(".", StringSplitOptions.RemoveEmptyEntries);
                var funNo = funNoArray[funNoArray.Length - 1]?.Replace("AppService", "AppService").Replace("sAppService", "");
                var fun = _funRepository.FirstOrDefault(a =>
                    a.FunctionNo == funNo && a.ParentNo == SysFunction.FunctionNo);
                if (fun == null)
                {
                    return GetMethodNameByAttr(methodName, serviceName);
                }
                return fun.FunctionName;
            }
            catch (Exception e)
            {
                typeof(SysLog).LogError(e);
                return methodName;
            }
        }

        private string GetMethodNameByAttr(string methodName, string serviceName)
        {
            Type = Type ?? Typen(serviceName);
            var member = Type?.GetMember(methodName).FirstOrDefault();
            if (member != null)
            {
                var attr = member.GetMemberSingleAttribute<AuditLogAttribute>();
                if (attr != null)
                {
                    methodName = attr.Name;
                }
                else
                {
                    switch (methodName.ToLower())
                    {
                        case "get":
                            methodName = "查询";
                            break;
                        case "getall":
                            methodName = "查询";
                            break;
                        case "create":
                            methodName = "创建";
                            break;
                        case "update":
                            methodName = "修改";
                            break;
                        case "delete":
                            methodName = "删除";
                            break;
                        case "auth":
                            methodName = "授权";
                            break;
                        case "refresh":
                            methodName = "刷新";
                            break;
                        case "moveup":
                            methodName = "上移";
                            break;
                        case "movedown":
                            methodName = "下移";
                            break;
                    }
                    if (MethondNameSuffix.IsNullOrEmpty())
                    {
                        MethondNameSuffix = GetMethondNameSuffixAttr(serviceName);
                    }
                    return methodName + MethondNameSuffix;
                }
            }
            return methodName;
        }

        private string GetMethondNameSuffixAttr(string serviceName)
        {
            Type = Typen(serviceName);
            var attr = Type.GetSingleAttribute<AuditLogAttribute>();
            if (attr != null)
            {
                return attr.MethondNameSuffix ?? "";
            }
            return "";
        }

        private Type Typen(string typeName)
        {
            var applicationAssemblyName = System.Configuration.ConfigurationManager.AppSettings["ApplicationAssemblyName"];
            var path = AppDomain.CurrentDomain.BaseDirectory + "bin/";
            Assembly assembly = Assembly.LoadFrom(path + applicationAssemblyName);
            var type = assembly.TypenFromAssembly(typeName);
            if (type == null)
            {
                var webAssemblyName = System.Configuration.ConfigurationManager.AppSettings["WebAssemblyName"];
                assembly = Assembly.LoadFrom(path + webAssemblyName);
                type = assembly.TypenFromAssembly(typeName);
            }
            return type;
        }
    }
}
