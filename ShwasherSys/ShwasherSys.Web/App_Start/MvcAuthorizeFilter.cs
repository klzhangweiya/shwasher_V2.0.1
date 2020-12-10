using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

using Abp.Authorization;
using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Events.Bus.Exceptions;
using Abp.Web.Models;
using Abp.Web.Mvc.Controllers.Results;
using Abp.Web.Mvc.Extensions;

using IwbZero.Authorization.AuthorizeFilter;
using IwbZero.Authorization.Users;
using IwbZero.Session;

namespace ShwasherSys
{
    public class IwbYueMvcAuthorizeFilter : IAuthorizationFilter, ITransientDependency
    {
        public IIwbSession AbpSession { get; set; }
        private readonly IwbCheckExpireTimeHelper _checkExpireTimeHelper;
        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly IErrorInfoBuilder _errorInfoBuilder;
        private readonly IEventBus _eventBus;

        public IwbYueMvcAuthorizeFilter(
            IwbCheckExpireTimeHelper checkExpireTimeHelper,
            IAuthorizationHelper authorizationHelper,
            IErrorInfoBuilder errorInfoBuilder,
            IEventBus eventBus)
        {
            _checkExpireTimeHelper = checkExpireTimeHelper;
            _authorizationHelper = authorizationHelper;
            _errorInfoBuilder = errorInfoBuilder;
            _eventBus = eventBus;
        }

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }
            var methodInfo = filterContext.ActionDescriptor.GetMethodInfoOrNull();
            if (methodInfo == null)
            {
                return;
            }
            try
            {
                _checkExpireTimeHelper.CheckUserHasExpire(AbpSession);
                if (AbpSession.UserName != UserBase.AdminUserName)
                    _authorizationHelper.Authorize(methodInfo, methodInfo.DeclaringType);
            }
            catch (IwbSessionExpireException ex)
            {
                //this.LogWarn(ex.ToString());
                HandleSessionExpireRequest(filterContext, methodInfo, ex);
            }
            catch (AbpAuthorizationException ex)
            {
                //this.LogWarn(ex.ToString());
                HandleUnauthorizedRequest(filterContext, methodInfo, ex);
            }
        }

        #region HandleUnauthorizedRequest

        protected virtual void HandleUnauthorizedRequest(
            AuthorizationContext filterContext,
            MethodInfo methodInfo,
            AbpAuthorizationException ex)
        {
            filterContext.HttpContext.Response.StatusCode =
                filterContext.RequestContext.HttpContext.User?.Identity?.IsAuthenticated ?? false
                    ? (int)HttpStatusCode.Forbidden
                    : (int)HttpStatusCode.Unauthorized;

            var isJsonResult = IsJsonResult(methodInfo);

            if (isJsonResult)
            {
                filterContext.Result = CreateUnAuthorizedJsonResult(ex);
            }
            else
            {
                filterContext.Result = CreateUnAuthorizedNonJsonResult(filterContext, ex);
            }

            if (isJsonResult || filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            }

            _eventBus.Trigger(this, new AbpHandledExceptionData(ex));
        }

        protected virtual AbpJsonResult CreateUnAuthorizedJsonResult(AbpAuthorizationException ex)
        {
            return new AbpJsonResult(
                new AjaxResponse(_errorInfoBuilder.BuildForException(ex), true))
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        protected virtual HttpStatusCodeResult CreateUnAuthorizedNonJsonResult(AuthorizationContext filterContext, AbpAuthorizationException ex)
        {
            return new HttpStatusCodeResult(filterContext.HttpContext.Response.StatusCode, ex.Message);
        }

        #endregion HandleUnauthorizedRequest

        #region HandleSessionExpireRequest

        protected virtual void HandleSessionExpireRequest(
            AuthorizationContext filterContext,
            MethodInfo methodInfo,
            IwbSessionExpireException ex)
        {
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var isJsonResult = IsJsonResult(methodInfo);

            if (isJsonResult)
            {
                filterContext.Result = CreateSessionExpireJsonResult(ex);
            }
            else
            {
                filterContext.Result = CreateSessionExpireNonJsonResult(filterContext, ex);
            }

            if (isJsonResult || filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            }

            _eventBus.Trigger(this, new AbpHandledExceptionData(ex));
        }

        protected virtual AbpJsonResult CreateSessionExpireJsonResult(IwbSessionExpireException ex)
        {
            return new AbpJsonResult(
                new AjaxResponse(_errorInfoBuilder.BuildForException(ex), true))
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        protected virtual HttpStatusCodeResult CreateSessionExpireNonJsonResult(AuthorizationContext filterContext, IwbSessionExpireException ex)
        {
            return new HttpStatusCodeResult(filterContext.HttpContext.Response.StatusCode, ex.Message);
        }

        #endregion HandleSessionExpireRequest

        public static bool IsJsonResult(MethodInfo method)
        {
            return typeof(JsonResult).IsAssignableFrom(method.ReturnType) ||
                   typeof(Task<JsonResult>).IsAssignableFrom(method.ReturnType);
        }
    }
}