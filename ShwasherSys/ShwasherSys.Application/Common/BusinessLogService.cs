using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Timing;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.NotificationInfo;
using ShwasherSys.NotificationInfo.Dto;
using ShwasherSys.ProductionOrderInfo;

namespace ShwasherSys.Common
{
    public static class BusinessLogService
    {
       
        public static void WriteLog(this IRepository<BusinessLog> logRepository, BusinessLog logInfo)
        {
            logRepository.InsertAsync(logInfo);
        }

        public static void WriteLog(this IRepository<BusinessLog> logRepository, BusinessLogTypeEnum logType, string logCommand, string logMsg,string logErrorMsg="",string logExtend1Info="", string logExtend2Info = "", string logExtend3Info = "", string logExtend4Info = "")
        {
            /*IRepository<BusinessLog> logRepository = IocManager.Resolve<IRepository<BusinessLog>>alize();*/
            BusinessLog log = new BusinessLog()
            {
                LogDate = Clock.Now,
                Extend1Log = logExtend1Info,
                Extend2Log = logExtend2Info,
                Extend3Log = logExtend3Info,
                Extend4Log = logExtend4Info,
                LogCommand = logCommand,
                LogErrorMessage = logErrorMsg,
                LogMessage = logMsg,
                LogType = logType.ToInt()
            };
            WriteLog(logRepository, log);
        }

        public static void WriteLog(this BusinessLogTypeEnum logType, IRepository<BusinessLog> logRepository, string logCommand, string logMsg, string logExt1 = "", string logExt2 = "", string logExt3 = "", string logExt4 = "" ,string eMsg = "")
        {
            //Action ac = () =>
            //{
            //    BusinessLog log = new BusinessLog()
            //    {
            //        LogDate = Clock.Now,
            //        Extend1Log = logExt1,
            //        Extend2Log = logExt2,
            //        Extend3Log = logExt3,
            //        Extend4Log = logExt4,
            //        LogCommand = logCommand,
            //        LogErrorMessage = eMsg,
            //        LogMessage = logMsg,
            //        LogType = logType.ToInt()
            //    };
            //    WriteLog(logRepository, log);
            //};
            //ac.BeginInvoke(null,null);
            BusinessLog log = new BusinessLog()
            {
                LogDate = Clock.Now,
                Extend1Log = logExt1,
                Extend2Log = logExt2,
                Extend3Log = logExt3,
                Extend4Log = logExt4,
                LogCommand = logCommand,
                LogErrorMessage = eMsg,
                LogMessage = logMsg,
                LogType = logType.ToInt()
            };
            WriteLog(logRepository, log);
        }
        public static void ErrorLog(this BusinessLogTypeEnum logType, IRepository<BusinessLog> logRepository, string logCommand, string eMsg, string logExt1 = "", string logExt2 = "", string logExt3 = "", string logExt4 = "" ,string logMsg = "")
        {
            BusinessLog log = new BusinessLog()
            {
                LogDate = Clock.Now,
                Extend1Log = logExt1,
                Extend2Log = logExt2,
                Extend3Log = logExt3,
                Extend4Log = logExt4,
                LogCommand = logCommand,
                LogErrorMessage = eMsg,
                LogMessage = logMsg,
                LogType = logType.ToInt()
            };
            WriteLog(logRepository, log);
        }

        /// <summary>
        /// 写入短消息
        /// </summary>
        /// <param name="msgRepository"></param>
        /// <param name="sendman">发送人</param>
        /// <param name="recieveIds">接收用户名 eg:shenjianfang,menghanming,jiangjingeng</param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public static void WriteShortMessage(this IRepository<ShortMessage> msgRepository,string sendman,string recieveIds,string title="",string content="")
        {
            ShortMessage shortMessage = new ShortMessage()
            {
                SendUserID = sendman,
                SendTime = Clock.Now,
                Title = title,
                Content = content,
                RecieveUserIds = recieveIds,
                IsDelete = "N"
            };
            msgRepository.InsertAsync(shortMessage);
        }

    }

    
}
