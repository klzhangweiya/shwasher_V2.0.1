using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Web.Authorization;
using Abp.Web.Mvc;
using Abp.Web.Mvc.Authorization;

using Castle.MicroKernel.Registration;

using Microsoft.Owin.Security;

using ShwasherSys.ScriptManager;

//<%@ Application Codebehind="Global.asax.cs" Inherits="IwbYue.MvcApplication" Language="C#" %>
namespace ShwasherSys
{
    [DependsOn(
        typeof(ShwasherDataModule),
        typeof(AbpWebMvcModule),
        typeof(ShwasherApplicationModule),
        typeof(IwbYueWebApiModule))]
    public class ShwasherWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
            //    new BigCamelCasePropertyNamesContractResolver();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //Configuration.Modules.AbpWebCommon().SendAllExceptionsToClients = true;
            SetCacheExpireTime(Configuration);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.IocContainer.Register(
                Component
                    .For<IAuthenticationManager>()
                    .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                    .LifestyleTransient()
            );

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override void PostInitialize()
        {
            GlobalFilters.Filters.Remove(IocManager.Resolve<AbpMvcAuthorizeFilter>());
            GlobalFilters.Filters.Add(IocManager.Resolve<IwbYueMvcAuthorizeFilter>());
            ReplaceScriptManager();
        }

        /// <summary>
        /// 设置缓存过期时间
        /// </summary>
        /// <param name="configuration"></param>
        private void SetCacheExpireTime(IAbpStartupConfiguration configuration)
        {
            //配置所有Cache的默认过期时间为2小时
            configuration.Caching.ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(2);
            });
            //配置UserExpireTime Cache的过期时间
            configuration.Caching.Configure(ShwasherConsts.UserExpireTimeCache, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(int.Parse(
                    System.Configuration.ConfigurationManager.AppSettings["AuthSession.ExpireTimeInMinutes"] ??
                    "30"));
            });
        }

        public void ReplaceScriptManager()
        {
            IocManager.IocContainer.Register(
                Component.For<IAuthorizationScriptManager>().Named("IIwbAuthorizationScriptManager")
                    .ImplementedBy<IwbAuthorizationScriptManager>().IsDefault());

            //IocManager.IocContainer.Register(
            //    Component.For<IAbpWebLocalizationConfiguration>().Named("IWebLocalizationConfiguration")
            //        .ImplementedBy<IwbWebLocalizationConfiguration>().IsDefault());

            //IocManager.Register<ILocalizationScriptManager, IwbLocalizationScriptManager>();
            //IocManager.Register<INavigationScriptManager, IwbNavigationScriptManager>();
            //IocManager.Register<ISettingScriptManager, IwbSettingScriptManager>();
        }
    }
}