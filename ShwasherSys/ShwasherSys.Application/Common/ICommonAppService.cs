using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BaseSysInfo.States.Dto;
using ShwasherSys.BaseSysInfo.SysAttachFiles.Dto;
using ShwasherSys.CustomerInfo;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.ProductStoreInfo.Dto;

namespace ShwasherSys.Common
{
    public interface ICommonAppService:IApplicationService
    {
        Task CloseProductOrder();
        Task WriteShortMessageByDep(string sendman, string departments, string title = "", string content = "");
        Task WriteShortMessage(string sendman, string recieveIds, string title = "", string content = "");

        void WriteShortMessage2(string sendman, string recieveIds, string title = "", string content = "");

        List<SysAttachFile> GetAttachFile(QueryAttachDto input);
        Task<bool> CheckProductCanSendToCustomer(string productOrderNo, string customerNo);
        Task<bool> CheckProductCanSendToCustomer(List<string> productOrderNos, string customerNo);
        Task<bool> CheckOrderHasSend(string pcOrderNo);

        Task PreMonth();
        int? GetAppGuid(AppGuidType type);

        bool CheckStoreRecordCanUpdate(string houseStoreNo, int houseType = 1);
        bool CheckStoreCanUpdateByLocationNo(string locationNo, int houseType = 1);
        List<SelectListItem> FilterLocationInfo(int storeId, string areaNo = "", string shelfNo = "");
       void SendEmail(string toEmail, string title, string msg, bool isHtml);

       List<ProductionOrderDisCustomerDto> GetDisCustomerInfo(EntityDto<string> input);


       Task<string> GetProductionOrderNo(string createType="",string preOrderNo="", int isOutsourcing = 0);
    }
}
