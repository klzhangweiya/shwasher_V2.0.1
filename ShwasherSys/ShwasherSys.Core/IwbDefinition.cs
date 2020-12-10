using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using IwbZero.Helper;
using ShwasherSys.CompanyInfo;
using ShwasherSys.Inspection;
using ShwasherSys.Order;

namespace ShwasherSys
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public class OrderTypeDefinition
    {
        /// <summary>
        /// 内销
        /// </summary>
        public const int InSale = 1;
        /// <summary>
        /// 外销
        /// </summary>
        public const int OutSale = 2;
        /// <summary>
        /// 换货
        /// </summary>
        public const int Exchange = 3;

        public  static async Task<string>  GetNewOrderNo(IRepository<OrderHeader, string> orderHeaderRepository)
        {
            string lcRetVal = "";
            DateTime loTemp = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01");
            loTemp = loTemp.AddSeconds(-1);
            var order =await orderHeaderRepository.GetAll().Where(i => i.TimeCreated > loTemp).OrderByDescending(i => i.Id).FirstOrDefaultAsync();
            if (order!=null)
            {
                int liTempNo = 0;
                liTempNo = Convert.ToInt32(order.Id);
                liTempNo++;
                lcRetVal = liTempNo.ToString();
                if (lcRetVal.Length < 10)
                {
                    lcRetVal = "0" + lcRetVal;
                }
            }
            else
            {
                DateTime loDate = DateTime.Today;
                int liMonth = loDate.Date.Month;
                string lcMonth = liMonth < 10 ? "0" + liMonth : liMonth.ToString();
                lcRetVal = loDate.Date.Year + lcMonth + "0001";
            }
            return lcRetVal;
        }
    }

    /// <summary>
    /// 产品类型定义
    /// </summary>
    public class ProductTypeDefinition
    {
        /// <summary>
        /// 半成品
        /// </summary>
        public const int Semi = 2;
        /// <summary>
        /// 成品
        /// </summary>
        public const int Finish = 1;

        public static async Task<string>  GetDisProductNo(IRepository<DisqualifiedProduct>  repository,int type)
        {
            string startNo = $"{DateTime.Now:yyMMdd}";
            var lastEntity = await repository.GetAll().Where(a => a.DisqualifiedNo.StartsWith(startNo)).OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();
            int noLength = 4, index = 0;
            if (lastEntity != null) 
            {
                var entityNo = lastEntity.DisqualifiedNo;
                index = Convert.ToInt32(entityNo.Substring(entityNo.Length - noLength));
            }
            index++;
            string no = $"{startNo}{type}{index.LeftPad(noLength)}";
            if ((await repository.CountAsync(a=>a.DisqualifiedNo==no)) > 0) 
            {
                no = await GetDisProductNo(repository,type);
            }
            return no;
        }
    }

    public class DisProductStateDefinition
    {
        /// <summary>
        /// 未处理
        /// </summary>
        public const int NoHandle = 1;
        /// <summary>
        /// 降级使用
        /// </summary>
        public const int Downgrade = 2;
        /// <summary>
        /// 报废处理
        /// </summary>
        public const int Scrapped = 3;
        /// <summary>
        /// 不报废降级
        /// </summary>
        public const int ScrappedDowngrade = 4;
        /// <summary>
        /// 返工反镀
        /// </summary>
        public const int AntiPlating = 5;
        /// <summary>
        /// 正常退货（无需检验）
        /// </summary>
        public const int NormalReturn = 6;
        /// <summary>
        /// 已降级
        /// </summary>
        public const int DowngradeHandled = 8;
        /// <summary>
        /// 已报废
        /// </summary>
        public const int ScrappedHandled = 9;
        /// <summary>
        /// 外购退货
        /// </summary>
        public const int OutPurchaseReturnGood = 10;
        /// <summary>
        /// 特殊采购（特采）
        /// </summary>
        public const int SpecialPurchase = 11;

    }

    public class LicenseTypeDefinition
    {
        /// <summary>
        /// 公司证照
        /// </summary>
        public const string Company = "公司证照";
        /// <summary>
        /// 员工证照
        /// </summary>
        public const string Employee = "员工证照";
        /// <summary>
        /// 设备证照
        /// </summary>
        public const string Device = "设备证照";
        /// <summary>
        /// 文书存档
        /// </summary>
        public const string Document = "文书存档";
        /// <summary>
        /// 其他
        /// </summary>
        public const string Other = "其他";
    }
    /// <summary>
    /// 维护设备类型
    /// </summary>
    public class MaintainTypeDefinition
    {
        /// <summary>
        /// 模具
        /// </summary>
        public const int Mold = 1;
        /// <summary>
        /// 设备
        /// </summary>
        public const int Device = 2;
        /// <summary>
        /// 其他
        /// </summary>
        public const int Other = 3;

        public static async Task<string> GetMoldNo(IRepository<Mold,int> repository)
        {
            string startNo = "SDA";
            var lastEntity = await repository.GetAll().Where(a => a.No.StartsWith(startNo)).OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();
            int noLength=6,index = 0;
            if (lastEntity!=null)
            {
                var entityNo = lastEntity.No;
                index = Convert.ToInt32(entityNo.Substring(entityNo.Length - noLength));
            }
            index++;
            string no = $"{startNo}{index.LeftPad(noLength)}";
            if ((await repository.CountAsync(a=>a.No==no)) > 0) 
            {
                no = await GetMoldNo(repository);
            }
            return no;
        }

        public static async Task<string> GetDeviceNo(IRepository<FixedAsset,int> repository)
        {
            string startNo = "SDB";
            var lastEntity = await repository.GetAll().Where(a => a.No.StartsWith(startNo)).OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();
            int noLength=6,index = 0;
            if (lastEntity!=null)
            {
                var entityNo = lastEntity.No;
                index = Convert.ToInt32(entityNo.Substring(entityNo.Length - noLength));
            }
            index++;
            string no = $"{startNo}{index.LeftPad(noLength)}";
            if ((await repository.CountAsync(a=>a.No==no)) > 0) 
            {
                no = await GetDeviceNo(repository);
            }
            return no;
        }


        public static async Task<string> GetDeviceMgPlanNo(IRepository<DeviceMgPlan,int>  repository,int type)
        {
            string typeStr = type == 1 ? "SDA" : type == 2 ? "SDB" : "SDZ";
            string startNo = $"{typeStr}{DateTime.Now:yyMMdd}";
            var lastEntity = await repository.GetAll().Where(a => a.No.StartsWith(startNo)).OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();
            int noLength=4,index = 0;
            if (lastEntity!=null)
            {
                var entityNo = lastEntity.No;
                index = Convert.ToInt32(entityNo.Substring(entityNo.Length - noLength));
            }
            index++;
            string no = $"{startNo}{index.LeftPad(noLength)}";
            if ((await repository.CountAsync(a=>a.No==no)) > 0) 
            {
                no = await GetDeviceMgPlanNo(repository,type);
            }
            return no;
        }

        public static async Task<string> GetMaintainRecordNo(IRepository<MaintenanceRecord,string>  repository,int type)
        {
            string startNo = $"{DateTime.Now:yyMM}";
            var lastEntity = await repository.GetAll().Where(a => a.Id.StartsWith(startNo)).OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();
            int noLength=5,index = 0;
            if (lastEntity!=null)
            {
                var entityNo = lastEntity.Id;
                index = Convert.ToInt32(entityNo.Substring(entityNo.Length - noLength));
            }
            index++;
            string no = $"{startNo}{type}{index.LeftPad(noLength)}";
            if ((await repository.CountAsync(a=>a.Id==no)) > 0) 
            {
                no = await GetMaintainRecordNo(repository,type);
            }
            return no;
        }
    }
    /// <summary>
    /// 维护记录状态
    /// </summary>
    public class MaintainStateDefinition
    {
        /// <summary>
        /// 新建
        /// </summary>
        public const int New = 1;
        /// <summary>
        /// 开始
        /// </summary>
        public const int Start = 2;
        /// <summary>
        /// 完成
        /// </summary>
        public const int Complete = 3;
        /// <summary>
        /// 结束
        /// </summary>
        public const int End = 4;

        public static string GetTypeName(int type)
        {
            var str = "";
            switch (type)
            {
                case 1:
                    str = "新建";
                    break;
                case 2:
                    str = "开始";
                    break;
                case 3:
                    str = "完成";
                    break;
                case 4:
                    str = "结束";
                    break;
                
            }

            return str;
        }
    }
    public class WorkTypeDefinition
    {
        /// <summary>
        /// 生产
        /// </summary>
        public const int Product = 1;
        /// <summary>
        /// 包装
        /// </summary>
        public const int Package = 2;
        /// <summary>
        /// 包装核件
        /// </summary>
        public const int VerifyPackage = 3;
        /// <summary>
        /// 模具维护
        /// </summary>
        public const int MoldMg = 4;
        /// <summary>
        /// 设备维护
        /// </summary>
        public const int DeviceMg = 5;

        public static string GetWorkTypeName(int type)
        {
            var str = "";
            switch (type)
            {
                case 1:
                    str = "车间生产";
                    break;
                case 2:
                    str = "包装负责";
                    break;
                case 3:
                    str = "包装核件";
                    break;
                case 4:
                    str = "模具维护";
                    break;
                case 5:
                    str = "设备维护";
                    break;
            }

            return str;
        }
        public static async Task<string>  GetPerformanceNo(IRepository<EmployeeWorkPerformance>  repository,int type)
        {
            string startNo = $"{DateTime.Now:yyMMdd}";
            var lastEntity = await repository.GetAll().Where(a => a.PerformanceNo.StartsWith(startNo)).OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();
            int noLength=5,index = 0;
            if (lastEntity!=null)
            {
                var entityNo = lastEntity.PerformanceNo;
                index = Convert.ToInt32(entityNo.Substring(entityNo.Length - noLength));
            }
            index++;
            string no = $"{startNo}{type}{index.LeftPad(noLength)}";
            if ((await repository.CountAsync(a=>a.PerformanceNo==no)) > 0) 
            {
                no = await GetPerformanceNo(repository,type);
            }
            return no;
        }
    }

    public class InspectConfirmStateDefinition
    {
        /// <summary>
        /// 未确认
        /// </summary>
        public const int New = 1;
        /// <summary>
        /// 已确认
        /// </summary>
        public const int Confirm = 2;

        public static async Task<string>  GetReportNo(IRepository<ProductInspectReport>  repository)
        {
            string startNo = $"{DateTime.Now:yyMM}";
            var lastEntity = await repository.GetAll().Where(a => a.ProductInspectReportNo.StartsWith(startNo)).OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();
            int noLength = 6, index = 0;
            if (lastEntity != null) 
            {
                var entityNo = lastEntity.ProductInspectReportNo;
                index = Convert.ToInt32(entityNo.Substring(entityNo.Length - noLength));
            }
            index++;
            string no = $"{startNo}{index.LeftPad(noLength)}";
            if ((await repository.CountAsync(a=>a.ProductInspectReportNo==no)) > 0) 
            {
                no = await GetReportNo(repository);
            }
            return no;
        }
    }
    public class StoreHouseType
    {
        /// <summary>
        /// 成品仓库
        /// </summary>
        public const int Finish = 1;
        /// <summary>
        /// 半成品仓库
        /// </summary>
        public const int SemiFinish = 2;
        /// <summary>
        /// 原材料仓库
        /// </summary>
        public const int Rm = 3;
    }
    //1:新建 2:盘点中 3:盘点完成  4:取消关闭
    public class InventoryCheckState
    {
        public const int New = 1;

        public const int Checking = 2;

        public const int Finish = 3;

        public const int Closed = 4;
    }

    //1:生产检验不合格  2：退货检验不合格
    public class DisqualifiedType
    {
        public const int ProductionCheck = 1;

        public const int ReturnCheck = 2;
    }
    
    
    /// <summary>
    /// 退货/换货
    /// </summary>
    public class ReturnGoodType
    {
        public const int Return = 1;

        public const int Change = 2;
    }
   /// <summary>
   /// 退货单状态
   /// </summary>
    public class ReturnGoodStateDefinition
    {
        /// <summary>
        /// 新建
        /// </summary>
        public const int New = 1;
        /// <summary>
        /// 检验中
        /// </summary>
        public const int Check = 2;
        /// <summary>
        /// 已检验
        /// </summary>
        public const int HasChecked = 3;
        /// <summary>
        /// 申请退款
        /// </summary>
        public const int RefundApply = 4;
        /// <summary>
        /// 确认退款
        /// </summary>
        public const int RefundConfirm = 5;
        /// <summary>
        /// 结束
        /// </summary>
        public const int End = 9;
    }

   /// <summary>
   ///发票类型
   /// </summary>
   public class InvoiceTypeDefinition
   {
       /// <summary>
       /// 正常发票
       /// </summary>
       public const int Normal = 0;
        /// <summary>
        /// 红冲发票(退款)
        /// </summary>
       public const int RedReturn = 1;
        /// <summary>
        /// 红冲发票(少收)
        /// </summary>
       public const int RedLess = 2;
        /// <summary>
        /// 红冲发票(多收)
        /// </summary>
       public const int RedOver = 3;
   }

    /// <summary>
    /// 订单明细紧急程度
    /// </summary>
   public class OrderItemEmergencyLevel
   {
        /// <summary>
        /// 正常（默认）
        /// </summary>
       public const int Normal = 1;
        /// <summary>
        /// 紧急
        /// </summary>
       public const int Urge = 2;
        /// <summary>
        /// 延期
        /// </summary>
       public const int Delay = 3;
   }
}