using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using ShwasherSys.BaseSysInfo;

namespace ShwasherSys.Authorization
{
    public class AppSettingProvider: SettingProvider
    {
        protected readonly IIocManager IocManager;
        public AppSettingProvider(IIocManager iocManager)
        {
            IocManager = iocManager;
        }
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var list = new List<SettingDefinition>();
            using (var settingRepository = IocManager.ResolveAsDisposable<IRepository<SysSetting, int>>())
            {
                var settings = settingRepository.Object.GetAllList(a => a.SettingType == 1);
                foreach (var s in settings)
                {
                    var setting = new SettingDefinition(s.Code, s.Value, scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true);
                    if (!list.Contains(setting))
                    {
                        list.Add(setting);
                    }
                }
            }

            return list;
        }
    }
}
