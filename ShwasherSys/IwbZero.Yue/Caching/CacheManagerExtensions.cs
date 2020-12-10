using Abp.Runtime.Caching;

namespace IwbZero.Caching
{
    public static class IwbCacheManagerExtensions
    {
        public static ITypedCache<string, IwbUserPermissionCacheItem> GetUserPermissionCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, IwbUserPermissionCacheItem>(IwbUserPermissionCacheItem.CacheStoreName);
        }

        public static ITypedCache<string, IwbRolePermissionCacheItem> GetRolePermissionCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, IwbRolePermissionCacheItem>(IwbRolePermissionCacheItem.CacheStoreName);
        }

        //public static ITypedCache<int, TenantFeatureCacheItem> GetTenantFeatureCache(this ICacheManager cacheManager)
        //{
        //    return cacheManager.GetCache<int, TenantFeatureCacheItem>(TenantFeatureCacheItem.CacheStoreName);
        //}

        //public static ITypedCache<int, EditionfeatureCacheItem> GetEditionFeatureCache(this ICacheManager cacheManager)
        //{
        //    return cacheManager.GetCache<int, EditionfeatureCacheItem>(EditionfeatureCacheItem.CacheStoreName);
        //}
    }
}
