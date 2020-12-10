using System;
using System.Configuration;

using Abp.Owin;

using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;

using Owin;

using ShwasherSys;
using ShwasherSys.Api.Controllers;

[assembly: OwinStartup(typeof(Startup))]

namespace ShwasherSys
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAbp();

            app.UseOAuthBearerAuthentication(AccountController.OAuthBearerOptions);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = ShwasherConsts.AuthenticationTypes,
                LoginPath = new PathString("/Account/Login"),
                // by setting following values, the auth cookie will expire after the configured amount of time (default 14 days) when user set the (IsPermanent == true) on the login
                ExpireTimeSpan = new TimeSpan(int.Parse(ConfigurationManager.AppSettings["AuthSession.ExpireTimeInDays.WhenPersistent"] ?? "14"), 0, 0, 0),
                SlidingExpiration = bool.Parse(ConfigurationManager.AppSettings["AuthSession.SlidingExpirationEnabled"] ?? bool.FalseString)
            });
            app.MapSignalR();
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //ENABLE TO USE HANGFIRE dashboard (Requires enabling Hangfire in IwbYueWebModule)
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new AbpHangfireAuthorizationFilter() } //You can remove this line to disable authorization
            //});
        }
    }
}