using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Json;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Caching;
using Abp.Timing;
using IwbZero.IdentityFramework;
using MailKit;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using ShwasherSys.Authorization.Users;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BaseSysInfo.SysAttachFiles.Dto;
using ShwasherSys.BasicInfo;
using ShwasherSys.CustomerInfo;
using ShwasherSys.EntityFramework;
using ShwasherSys.Hubs;
using ShwasherSys.Inspection;
using ShwasherSys.NotificationInfo;
using ShwasherSys.Order;
using ShwasherSys.OrderSendInfo;
using ShwasherSys.ProductionOrderInfo;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.ProductStoreInfo.Dto;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys.Common
{
    [DisableAuditing]
    public class CommonAppService : ApplicationService,ICommonAppService
    {
        protected IRepository<ProductionOrder> ProductionOrderRepository { get; }
        protected IRepository<CustomerDisabledProduct> CdpRepository;
        protected IRepository<ShortMessage> ShortMsgRepository;
        protected IRepository<OrderHeader,string> OrderHeaderRepository;
        protected IRepository<ViewOrderSend> ViewOrderSendRepository;
        protected IRepository<ShortMsgDetail> ShortMsgDetailRepository;
        protected IRepository<SysUser,long> SysUserRepository;
        protected IRepository<SemiEnterStore> SemiEnterStoreRepository;
        protected IUnitOfWorkManager _UnitOfWorkManager;
        protected IRepository<SysAttachFile> SysAttachFileRepository;
        protected ISqlExecuter SqlExecuter;
        protected IEmailSender EmailSender;
        protected IRepository<CurrentProductStoreHouse> CurrentProductStoreHouseRepository { get; }
        protected IRepository<CurrentSemiStoreHouse> CurrentSemiStoreHouseRepository { get; }

        protected IRepository<StoreHouseLocation> StoreHouseLocationRepository { get; }
        protected IRepository<Customer,string> CustomerRepository { get; }
        protected IRepository<CustomerDisabledProduct> CustomerDisabledProductRepository { get; }
        protected IRepository<Department,string> DepartmentRepository { get; }

        public IHubContext IwbHub { get; }

        public CommonAppService(ICacheManager cacheManager,  IRepository<ShortMsgDetail> shortMsgDetailRepository, IRepository<ShortMessage> shortMsgRepository, IRepository<SysUser, long> sysUserRepository, IUnitOfWorkManager unitOfWorkManager, IRepository<SysAttachFile> sysAttachFileRepository, IRepository<OrderHeader, string> orderHeaderRepository, IRepository<ViewOrderSend> viewOrderSendRepository, ISqlExecuter sqlExecuter, IEmailSender emailSender, IRepository<CurrentProductStoreHouse> currentProductStoreHouseRepository, IRepository<CurrentSemiStoreHouse> currentSemiStoreHouseRepository, IRepository<StoreHouseLocation> storeHouseLocationRepository, IRepository<CustomerDisabledProduct> cdpRepository, IRepository<ProductionOrder> productionOrderRepository, ISmtpEmailSenderConfiguration smtpEmailSenderConfiguration, IRepository<Customer, string> customerRepository, IRepository<CustomerDisabledProduct> customerDisabledProductRepository, IRepository<SemiEnterStore> semiEnterStoreRepository, IRepository<Department, string> departmentRepository)
        {
            ShortMsgDetailRepository = shortMsgDetailRepository;
            ShortMsgRepository = shortMsgRepository;
            SysUserRepository = sysUserRepository;
            _UnitOfWorkManager = unitOfWorkManager;
            SysAttachFileRepository = sysAttachFileRepository;
            OrderHeaderRepository = orderHeaderRepository;
            ViewOrderSendRepository = viewOrderSendRepository;
            SqlExecuter = sqlExecuter;
            IwbHub = GlobalHost.ConnectionManager.GetHubContext<IwbHub>();
            EmailSender = emailSender;
            CurrentProductStoreHouseRepository = currentProductStoreHouseRepository;
            CurrentSemiStoreHouseRepository = currentSemiStoreHouseRepository;
            StoreHouseLocationRepository = storeHouseLocationRepository;
            CdpRepository = cdpRepository;
            ProductionOrderRepository = productionOrderRepository;
            SmtpEmailSenderConfiguration = smtpEmailSenderConfiguration;
            CustomerRepository = customerRepository;
            CustomerDisabledProductRepository = customerDisabledProductRepository;
            SemiEnterStoreRepository = semiEnterStoreRepository;
            DepartmentRepository = departmentRepository;
        }

        private  Action<ISqlExecuter,DateTime> CloseProductionOrder = (sqlExecuter, date) =>
        {
            //var list = repository.GetAllList(a => a.PlanProduceDate < date && a.ProductionOrderStatus != 5);
            //if (list.Any())
            //{
            //    foreach (var l in list)
            //    {
            //        l.ProductionOrderStatus = 5;
            //        repository.Update(l);
            //    }
            //}
            string sql =
                $"UPDATE [dbo].[ProductionOrders] SET ProductionOrderStatus=5 WHERE  EnterDate < '{date:yyyy-MM-dd}' AND ProductionOrderStatus=4 AND IsChecked=1 ";
            sqlExecuter.Execute(sql);
        };

        /// <summary>
        /// 关闭三个月前的排产单
        /// </summary>
        public async Task CloseProductOrder()
        {
            var date = DateTime.Now.AddMonths(-3);
            date= new DateTime(date.Year,date.Month,date.Day);
            if (await ProductionOrderRepository.CountAsync(a=>a.IsChecked==1 && a.EnterDate<date && a.ProductionOrderStatus==4)>0)
            {
                string sql =
                    $"update [dbo].[ProductionOrders] set ProductionOrderStatus=5 where  PlanProduceDate < '{date:yyyy-MM-dd}'";
                await SqlExecuter.ExecuteAsync(sql);
            }
           
        }

        public async Task WriteShortMessageByDep(string sendman,string departments,string title="",string content="")
        {
            string lcRecieveIds = "";
            var loArr = departments.Split(',');
            var ds = DepartmentRepository.GetAllList(i => loArr.Contains(i.DepartmentName));
            var dsIds = ds.Select(i => i.Id);
            var users = SysUserRepository.GetAllList(i => dsIds.Contains(i.DepartmentID));
            if (users.Any())
            {
                lcRecieveIds = string.Join(",", users.Select(i => i.UserName).ToArray());
                await WriteShortMessage(sendman, lcRecieveIds, title, content);
            }
           
        }
        /// <summary>
        /// 写入短消息
        /// </summary>
        /// <param name="sendman">发送人</param>
        /// <param name="recieveIds">接收用户名 eg:shenjianfang,menghanming,jiangjingeng</param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public async Task WriteShortMessage(string sendman, string recieveIds, string title = "", string content = "")
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
            var shortMsg =  await ShortMsgRepository.InsertAsync(shortMessage);
            await CurrentUnitOfWork.SaveChangesAsync();
            IwbHub.Clients.All.getShortMsg(shortMessage.ToJsonString());
          
            if (!recieveIds.IsNullOrEmpty())
            {
                var loArr = recieveIds.Split(',');
                if (loArr.Any())
                {
                    //MailMessage mail = new MailMessage();
                    foreach (var u in loArr)
                    {
                        ShortMsgDetail shortMsgDetail = new ShortMsgDetail()
                        {
                            IsRead = "N",
                            MsgID = shortMsg.Id,
                            RecvUserID = u,
                        };
                        //var user = SysUserRepository.FirstOrDefault(i => i.UserName == u);
                        //mail.To.Add(user.EmailAddress);
                        //await EmailSender.SendAsync(user.EmailAddress, "系统订单变动", content);
                        await ShortMsgDetailRepository.InsertAsync(shortMsgDetail);
                    }

                 
                }
            }

        }

        public void WriteShortMessage2(string sendman, string recieveIds, string title = "", string content = "")
        {
            if (!recieveIds.IsNullOrEmpty())
            {
                var loArr = recieveIds.Split(',');
                if (loArr.Any())
                {
                    MailMessage mail = new MailMessage();
                    foreach (var u in loArr)
                    {
                        var user = SysUserRepository.FirstOrDefault(i => i.UserName == u);
                        mail.To.Add(user.EmailAddress);
                        mail.Body = content;
                        mail.IsBodyHtml = true;
                        mail.Subject = title;
                    }

                    if (mail.To.Any())
                    {
                        EmailSender.Send(mail);
                    }
                }
            }
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public List<SysAttachFile> GetAttachFile(QueryAttachDto input)
        {
            var query = SysAttachFileRepository.GetAll().Where(i =>
                i.TableName == input.TableName && i.ColumnName == input.ColName && i.SourceKey == input.Key);
            return query.ToList();
        }
        /// <summary>
        /// 判断订单是否有发货记录
        /// </summary>
        /// <param name="pcOrderNo"></param>
        /// <returns></returns>
        public async Task<bool> CheckOrderHasSend(string pcOrderNo)
        {
            var orderSends = await ViewOrderSendRepository.GetAllListAsync(i => i.OrderNo == pcOrderNo);
            return orderSends.Any();
        }

       /// <summary>
       /// 检查改批次产品能否发货给某客户
       /// </summary>
       /// <param name="productOrderNo"></param>
       /// <param name="customerNo"></param>
       /// <returns></returns>
        public async Task<bool> CheckProductCanSendToCustomer(string productOrderNo,string customerNo)
        {
            return await CdpRepository.CountAsync(a=>a.CustomerNo==customerNo&&a.ProductOrderNo==productOrderNo)>0;
        }

       /// <summary>
       /// 检查改批次产品能否发货给某客户
       /// </summary>
       /// <param name="productOrderNos"></param>
       /// <param name="customerNo"></param>
       /// <returns></returns>
        public async Task<bool> CheckProductCanSendToCustomer(List<string> productOrderNos,string customerNo)
        {
            return await CdpRepository.CountAsync(a=>a.CustomerNo==customerNo&&productOrderNos.Contains(a.ProductOrderNo))>0;
        }

        public Task PreMonth()
        {
            return  SqlExecuter.ExecuteAsync("update CurrentProductStoreHouse set PreMonthQuantity = Quantity;update CurrentSemiStoreHouse set PreMonthQuantity = ActualQuantity;");
        }
        [UnitOfWork]
        [DisableAuditing]
        public int? GetAppGuid(AppGuidType type)
        {
            var sqlParms = new object[3];
            sqlParms[0] = new SqlParameter("@idtype", (int)type);
            sqlParms[1] = new SqlParameter("@nextid", SqlDbType.Int) { Direction = ParameterDirection.Output };
            sqlParms[2] = new SqlParameter("@maxid", SqlDbType.Int) { Direction = ParameterDirection.Output };
            SqlExecuter.Execute(@"exec [dbo].[Sp_AppGuid] @idtype,@nextid out,@maxid out", sqlParms);
            int guid = (int)((SqlParameter)sqlParms[2]).Value;
            return guid;
        }

        /// <summary>
        /// 检查库存记录是否可以更新
        /// </summary>
        /// <param name="houseType">仓库类型（1：成品 2：半成品）</param>
        /// <param name="houseStoreNo">库存记录编号</param>
        /// <returns></returns>
        [UnitOfWork]
        [DisableAuditing]
        public bool CheckStoreRecordCanUpdate(string houseStoreNo,int houseType=1)
        {
            if (houseType == 1)
            {
                var houseRecord =
                    CurrentProductStoreHouseRepository.FirstOrDefault(i =>
                        i.CurrentProductStoreHouseNo == houseStoreNo);
                if (houseRecord != null&&houseRecord.InventoryCheckState==2&&houseRecord.ReturnState==2)
                {
                    return false;
                }
            }
            if (houseType == 2)
            {
                var houseRecord =
                    CurrentSemiStoreHouseRepository.FirstOrDefault(i =>
                        i.CurrentSemiStoreHouseNo == houseStoreNo);
                if (houseRecord != null && houseRecord.InventoryCheckState == 2 && houseRecord.ReturnState == 2)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 检查库存记录是否可以更新
        /// </summary>
        /// <param name="houseType">仓库类型（1：成品 2：半成品）</param>
        /// <param name="locationNo">库位编号</param>
        /// <returns></returns>
        [UnitOfWork]
        [DisableAuditing]
        public bool CheckStoreCanUpdateByLocationNo(string locationNo,int houseType=1)
        {
            if (houseType == 1)
            {
                if (CurrentProductStoreHouseRepository.Count(a => a.StoreLocationNo == locationNo&& (a.InventoryCheckState==2 || a.ReturnState==2))>0)
                {
                    return false;
                }
            }else if (houseType == 2)
            {
                if (CurrentSemiStoreHouseRepository.Count(a => a.StoreLocationNo == locationNo&& (a.InventoryCheckState==2 || a.ReturnState==2))>0)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 查询库区/货架/排（排可以多选）
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="areaNo"></param>
        /// <param name="shelfNo"></param>
        /// <returns></returns>
        public List<SelectListItem> FilterLocationInfo(int storeId,string areaNo="",string shelfNo="")
        {
            var query = StoreHouseLocationRepository.GetAll().Where(i => i.StoreHouseId == storeId);
            if (areaNo.IsNullOrEmpty())
            {
                var result = query.Select(i => i.StoreAreaCode).Distinct();
                return result.Select(i=>new SelectListItem(){Text = i.ToString(),Value = i.ToString()}).ToList();
            }
            query = query.Where(i => i.StoreAreaCode == areaNo);
            if (shelfNo.IsNullOrEmpty())
            {
                var result = query.Select(i => i.ShelfNumber).Distinct();
                return result.Select(i => new SelectListItem() { Text = i.ToString(), Value = i.ToString() }).ToList();
            }
            query = query.Where(i => i.ShelfNumber == shelfNo);
            return  query.Select(i => i.ShelfLevel).Distinct().Select(i => new SelectListItem() { Text = i.ToString(), Value = i.ToString() }).ToList();
        }

       public List<ProductionOrderDisCustomerDto> GetDisCustomerInfo(EntityDto<string> input)
        {
            return CustomerDisabledProductRepository.GetAll().Where(i => i.ProductOrderNo == input.Id).Join(CustomerRepository.GetAll(), p => p.CustomerNo, c => c.Id,
                (p, c) =>
                    new ProductionOrderDisCustomerDto()
                    {
                        ProductionOrderNo = p.ProductOrderNo,
                        CustomerName = c.CustomerName,
                        CustomerId = p.CustomerNo
                    }).ToList();
        }
        #region 发送邮件

        public  ISmtpEmailSenderConfiguration SmtpEmailSenderConfiguration { get; }

        public void SendEmail(string toEmail,string title,string msg,bool isHtml)
        {
            //SmtpEmailSender emailSender = new SmtpEmailSender(SmtpEmailSenderConfiguration);
            //emailSender.Send("zhangwy@iwbnet.com",toEmail , title, msg,isHtml);
            //EmailSendHelper.SendEmail(toEmail,title,msg,"","",isHtml);
            EmailHelper.SendEmail(toEmail,title,msg,isHtml);
        }



        #endregion

        #region 创建排产单号
        public async Task<string> GetProductionOrderNo(string createType = "", string preOrderNo = "", int isOutsourcing = 0)
        {
            if (string.IsNullOrEmpty(createType))
            {
                return await NormalGetProductionOrderNo(isOutsourcing);
            }
            return await SpecialGetProductionOrderNo(createType, preOrderNo);
        }
        /// <summary>
        /// 常规获取批次号
        /// </summary>
        /// <param name="isOutsourcing">1是外购</param>
        /// <returns></returns>
        private async Task<string> NormalGetProductionOrderNo(int isOutsourcing)
        {
            string lcRetVal;
            DateTime loTiem = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01");
            //loTiem = loTiem.AddSeconds(-1);
            var orders = (await ProductionOrderRepository.GetAllListAsync(i => i.TimeCreated >= loTiem && i.ProcessingLevel == "1")).OrderByDescending(i => i.Id).ToList();
            var orderNo = orders.FirstOrDefault()?.ProductionOrderNo;
            if (!string.IsNullOrEmpty(orderNo))
            {
                var liTempNo = Convert.ToInt32(orderNo.Substring(3, 4));
                liTempNo++;
                lcRetVal = liTempNo.ToString();
                while (lcRetVal.Length < 4)
                {
                    lcRetVal = "0" + lcRetVal;
                }
            }
            else
            {
                lcRetVal = "0001";
            }
            DateTime loDate = DateTime.Today;
            //string lcMonth = liMonth < 10 ? liMonth + "" : Convert.ToString(liMonth, 16);
            lcRetVal = loDate.Date.Year + GetMonthString(isOutsourcing) + lcRetVal;
            lcRetVal = lcRetVal.Substring(2, lcRetVal.Length - 2);
            return lcRetVal;
        }
        /// <summary>
        /// 转换月份
        /// </summary>
        /// <param name="isOutsourcing"></param>
        /// <returns></returns>
        private string GetMonthString(int isOutsourcing)
        {
            DateTime loDate = DateTime.Today;
            int liMonth = loDate.Date.Month;
            if (isOutsourcing == 0)
            {
                return liMonth < 10 ? liMonth + "" : Convert.ToString(liMonth, 16).ToUpper();
            }

            string[] scource = { "", "G", "H", "W", "J", "K", "L", "M", "N", "T", "P", "Q", "R" };
            return scource[liMonth];
        }



        private string GetLastFourChar(string preOrderNo)
        {
            if (!string.IsNullOrEmpty(preOrderNo) && preOrderNo.Length == 11)
            {
                return preOrderNo.Substring(7, 4);
            }
            return "";
        }


        private async Task<string> SpecialGetProductionOrderNo(string createType, string preOrderNo = "")
        {
            string lcRetVal;
            DateTime loTiem = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01");
            string reg = @"/^[A-Za-z0-9]{3}" + createType + @"\w{3,7}$/";
            string orderNo = "";
            if (createType == "T")
            {
                var orders = (await SemiEnterStoreRepository
                    .GetAllListAsync(i => i.TimeCreated >= loTiem && Regex.IsMatch(i.ProductionOrderNo, @"/^[A-Za-z0-9]{3}T\w{3,7}$/"))).OrderByDescending(i => i.Id).ToList();
                orderNo = orders.FirstOrDefault()?.ProductionOrderNo;
            }else if (createType == "G")
            {
                var orders = (await ProductionOrderRepository
                    .GetAllListAsync(i => i.TimeCreated >= loTiem && Regex.IsMatch(i.ProductionOrderNo, @"/^[A-Za-z0-9]{3}T\w{3,7}$/"))).OrderByDescending(i => i.Id).ToList();
                orderNo = orders.FirstOrDefault()?.ProductionOrderNo;
            }
            if (!string.IsNullOrEmpty(orderNo))
            {
                var liTempNo = Convert.ToInt32(orderNo.Substring(4, 3));
                liTempNo++;
                lcRetVal = liTempNo.ToString();
                while (lcRetVal.Length < 3)
                {
                    lcRetVal = "0" + lcRetVal;
                }
            }
            else
            {
                lcRetVal = "T001";
            }
            DateTime loDate = DateTime.Today;
            //string lcMonth = liMonth < 10 ? liMonth + "" : Convert.ToString(liMonth, 16);
            lcRetVal = loDate.Date.Year + GetMonthString(0) + lcRetVal;
            lcRetVal = lcRetVal.Substring(2, lcRetVal.Length - 2);
            lcRetVal += GetLastFourChar(preOrderNo);
            return lcRetVal;
        }
        #endregion

    }
}
