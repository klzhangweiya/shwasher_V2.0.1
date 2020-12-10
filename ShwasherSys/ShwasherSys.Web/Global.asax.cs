using System;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Castle.Facilities.Logging;
using ShwasherSys;

namespace ShwasherSys
{
    public class MvcApplication : AbpWebApplication<ShwasherWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig(Server.MapPath("log4net.config"))
            );
            this.LogInfo("Application -- System Starting!");
            base.Application_Start(sender, e);
            this.LogInfo("Application -- System Started!");
        }
        protected override void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
            this.LogInfo("Application -- System End!");
        }
        protected override void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码

            //获取到HttpUnhandledException异常，这个异常包含一个实际出现的异常
            Exception ex = Server.GetLastError();
            //实际发生的异常
            Exception innerException = ex.InnerException;
            if (innerException != null) ex = innerException;
            this.LogFatal(ex);

        }

        protected override void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码
            this.LogInfo("Session_Start");
        }
        protected override void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为 InProc 时，才会引发 Session_End 事件。
            // 如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。

            this.LogInfo("Session_End");
        }
    }
}
