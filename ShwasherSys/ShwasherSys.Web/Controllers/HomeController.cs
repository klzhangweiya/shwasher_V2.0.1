using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using IwbZero.Auditing;
using IwbZero.Setting;

namespace ShwasherSys.Controllers
{
    [AuditLog("管理系统")]
    public class HomeController : ShwasherControllerBase
    {
        public HomeController(IIwbSettingManager settingManager)
        {
            SettingManager = settingManager;
        }

        [AbpMvcAuthorize, AuditLog("主页面")]
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = await SettingManager.GetSettingValueAsync(SettingNames.AdminSystemName);
            return View();
        }
    }
}