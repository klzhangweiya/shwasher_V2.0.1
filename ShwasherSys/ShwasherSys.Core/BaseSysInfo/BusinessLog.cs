using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.BaseSysInfo
{
    public class BusinessLog:FullAuditedEntity<int>
    {
        public int LogType { get; set; }

        public string LogCommand { get; set; }

        public string LogMessage { get; set; }

        public string LogErrorMessage { get; set; }

        public DateTime? LogDate { get; set; }

        public string UserHostAddress { get; set; }

        public string Extend1Log { get; set; }
        public string Extend2Log { get; set; }
        public string Extend3Log { get; set; }
        public string Extend4Log { get; set; }
    }

    public enum BusinessLogTypeEnum
    {
        /// <summary>
        /// 订单
        /// </summary>
        OrderLog=1,
        /// <summary>
        /// 发货
        /// </summary>
        OrderSend = 2,
        /// <summary>
        /// 生产单
        /// </summary>
        ProductionOrderLog =3,
        /// <summary>
        /// 成品仓库
        /// </summary>
        PStore=4,
        /// <summary>
        /// 半成品品仓库
        /// </summary>
        SStore = 5,
        /// <summary>
        /// 原材料品仓库
        /// </summary>
        MStore = 6,
        /// <summary>
        /// 包装
        /// </summary>
        Package=7,
        /// <summary>
        /// 检验
        /// </summary>
        Inspect=8,
        /// <summary>
        /// 原材料仓库
        /// </summary>
        RStore = 9,
        /// <summary>
        /// 证照上传
        /// </summary>
        License = 10,
        
    }
    /*public enum LogCommandEnum
    {
        OrderEdit,
        OrderSendE
        ProductionEdit,

    }*/
}
