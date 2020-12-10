﻿using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Abp.Runtime.Caching;

namespace ShwasherSys.Controllers
{
    [AbpMvcAuthorize]
    public class LicenseDocumentController : ShwasherControllerBase
    {

		public LicenseDocumentController( ICacheManager cacheManager )
        {
			CacheManager = cacheManager;
        }

        [AbpMvcAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}