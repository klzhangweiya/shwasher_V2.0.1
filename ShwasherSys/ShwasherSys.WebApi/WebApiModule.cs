using System;
using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Json;
using Abp.Modules;
using Abp.Timing;
using Abp.WebApi;
using Abp.WebApi.Authorization;
using Abp.WebApi.Configuration;
using Newtonsoft.Json.Serialization;

namespace ShwasherSys
{
    [DependsOn(typeof(AbpWebApiModule), typeof(ShwasherApplicationModule))]
    public class IwbYueWebApiModule : AbpModule
    {
        public override void PreInitialize()
        {
            //配置所有Cache的默认过期时间为2小时
            Configuration.Caching.ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(2);
            });
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(ShwasherApplicationModule).Assembly, "app")
                .Build();

        }

        public override void PostInitialize()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new IwbCamelCasePropertyNamesContractResolver();
            var httpConfiguration = IocManager.Resolve<IAbpWebApiConfiguration>().HttpConfiguration;
            //httpConfiguration.Services.Replace(typeof(AbpApiAuthorizeFilter), IocManager.Resolve<IwbApiAuthorizeFilter>());
            httpConfiguration.Filters.Remove(IocManager.Resolve<AbpApiAuthorizeFilter>());
            httpConfiguration.Filters.Add(IocManager.Resolve<ShwasherApiAuthorizeFilter>());
        }
    }

    public class IwbCamelCasePropertyNamesContractResolver : AbpCamelCasePropertyNamesContractResolver
    {
        protected override void ModifyProperty(MemberInfo member, JsonProperty property)
        {
            if (property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(DateTime?))
            {
                return;
            }
            if (member.GetMemberSingleAttribute<DisableDateTimeNormalizationAttribute>() == null)
            {
                property.Converter = new AbpDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            }
        }
    }
}