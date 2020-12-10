using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Threading;

namespace IwbZero.Setting
{
    public class IwbSettingStore : ISettingStore
    {
        /// <summary>
        /// Gets singleton instance.
        /// </summary>
        public static IwbSettingStore Instance { get; } = new IwbSettingStore();
        private IwbSettingStore()
        {
        }
        public Task<SettingInfo> GetSettingOrNullAsync(int? tenantId, long? userId, string name)
        {
            //throw new NotImplementedException();
            var value = ConfigurationManager.AppSettings[name];

            if (value == null)
            {
                return Task.FromResult<SettingInfo>(null);
            }
            return Task.FromResult(new SettingInfo(tenantId, userId, name, value));
        }

        public Task DeleteAsync(SettingInfo setting)
        {
            //throw new NotImplementedException();
            return AbpTaskCache.CompletedTask;
        }

        public Task CreateAsync(SettingInfo setting)
        {
            //throw new NotImplementedException();
            return AbpTaskCache.CompletedTask;
        }

        public Task UpdateAsync(SettingInfo setting)
        {
            //throw new NotImplementedException();
            return AbpTaskCache.CompletedTask;
        }

        public Task<List<SettingInfo>> GetAllListAsync(int? tenantId, long? userId)
        {
            //throw new NotImplementedException();
            return Task.FromResult(new List<SettingInfo>());
        }
    }
}
