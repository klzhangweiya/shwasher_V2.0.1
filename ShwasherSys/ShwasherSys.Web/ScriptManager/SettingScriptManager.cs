﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.Web.Authorization;
using Abp.Web.Http;
using Abp.Web.Settings;
using IwbZero.Authorization.Permissions;

namespace ShwasherSys.ScriptManager
{

    public class IwbSettingScriptManager : ISettingScriptManager, ISingletonDependency
    {
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly ISettingManager _settingManager;
        private readonly IAbpSession _abpSession;
        private readonly IIocResolver _iocResolver;

        public IwbSettingScriptManager(
            ISettingDefinitionManager settingDefinitionManager,
            ISettingManager settingManager,
            IAbpSession abpSession,
            IIocResolver iocResolver)
        {
            _settingDefinitionManager = settingDefinitionManager;
            _settingManager = settingManager;
            _abpSession = abpSession;
            _iocResolver = iocResolver;
        }

        public async Task<string> GetScriptAsync()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine("    abp.setting = abp.setting || {};");
            script.AppendLine("    abp.setting.values = {");

            var settingDefinitions = _settingDefinitionManager
                .GetAllSettingDefinitions();

            var added = 0;

            using (var scope = _iocResolver.CreateScope())
            {
                foreach (var settingDefinition in settingDefinitions)
                {
                    if (!await settingDefinition.ClientVisibilityProvider.CheckVisible(scope))
                    {
                        continue;
                    }

                    if (added > 0)
                    {
                        script.AppendLine(",");
                    }
                    else
                    {
                        script.AppendLine();
                    }

                    var settingValue = await _settingManager.GetSettingValueAsync(settingDefinition.Name);

                    script.Append("        '" +
                                  settingDefinition.Name.Replace("'", @"\'") + "': " +
                                  (settingValue == null ? "null" : "'" + HttpEncode.JavaScriptStringEncode(settingValue) + "'"));

                    ++added;
                }
            }

            script.AppendLine();
            script.AppendLine("    };");

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}
