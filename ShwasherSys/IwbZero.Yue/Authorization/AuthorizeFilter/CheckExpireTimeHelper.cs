using System;
using Abp.Dependency;
using Abp.Runtime.Caching;
using IwbZero.Session;

namespace IwbZero.Authorization.AuthorizeFilter
{
    public class IwbCheckExpireTimeHelper : ITransientDependency
    {
        private readonly ICacheManager _cacheManager;
        public IwbCheckExpireTimeHelper(
            ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void CheckUserHasExpire(IIwbSession session)
        {
            if (CheckUserExpireTime(session))
            {
                throw new IwbSessionExpireException("登陆超时！");
            }
        }
        public bool CheckUserExpireTime(IIwbSession session)
        {
            if (session?.UserId == null)
                return true;
            if (session.RememberMe != null && (bool)session.RememberMe)
                return false;

            DateTimeOffset expireUtc = _cacheManager.GetCache(IwbZeroConsts.UserExpireTimeCache)
                                            .Get(session.UserId + "", () => session.ExpireTime) ??
                                        default(DateTimeOffset);
            if (expireUtc.CompareTo(DateTimeOffset.UtcNow) > 0)
            {
                _cacheManager.GetCache(IwbZeroConsts.UserExpireTimeCache).Set(session.UserId + "",
                    DateTimeOffset.UtcNow.AddMinutes(int.Parse(
                        System.Configuration.ConfigurationManager.AppSettings["AuthSession.ExpireTimeInMinutes"] ??
                        "30")));
                return false;
            }
            expireUtc = session.ExpireTime ?? default(DateTimeOffset);
            if (expireUtc != null && expireUtc.CompareTo(DateTimeOffset.UtcNow) > 0)
            {
                _cacheManager.GetCache(IwbZeroConsts.UserExpireTimeCache).Set(session.UserId + "",
                   DateTimeOffset.UtcNow.AddMinutes(int.Parse(
                       System.Configuration.ConfigurationManager.AppSettings["AuthSession.ExpireTimeInMinutes"] ??
                       "30")));
                return false;
            }

            return true;
        }
    }
}
