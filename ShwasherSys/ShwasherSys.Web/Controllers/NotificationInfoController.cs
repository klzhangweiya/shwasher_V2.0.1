using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using IwbZero.Auditing;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo.States;

namespace ShwasherSys.Controllers
{
    [AbpMvcAuthorize, AuditLog("消息管理")]
    public class NotificationInfoController : ShwasherControllerBase
    {
        public NotificationInfoController(IStatesAppService statesAppService)
        {
            StatesAppService = statesAppService;
        }
        [AbpMvcAuthorize(PermissionNames.PagesNotificationInfoBulletinInfos),AuditLog("系统通告管理")]
        // GET: NotificationInfo
        public ActionResult BulletinInfos()
        {
            ViewBag.BulletinType = StatesAppService.GetSelectLists("BulletinInfo", "BulletinType");
            return View();
        }

        public ActionResult ShortMsgMg()
        {
            return View();
        }
    }
}