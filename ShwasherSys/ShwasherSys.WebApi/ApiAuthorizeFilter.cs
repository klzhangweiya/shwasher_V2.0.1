using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Events.Bus.Exceptions;
using Abp.Localization;
using Abp.Web;
using Abp.Web.Models;
using Abp.WebApi.Configuration;
using Abp.WebApi.Validation;
using IwbZero.Authorization.AuthorizeFilter;
using IwbZero.Authorization.Users;
using IwbZero.Session;

namespace ShwasherSys
{
    public class ShwasherApiAuthorizeFilter : IAuthorizationFilter, ITransientDependency
    {
        public bool AllowMultiple => false;

        public IIwbSession AbpSession { get; set; }
        private readonly IwbCheckExpireTimeHelper _checkExpireTimeHelper;
        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly IAbpWebApiConfiguration _configuration;
        private readonly ILocalizationManager _localizationManager;
        private readonly IEventBus _eventBus;

        public ShwasherApiAuthorizeFilter(
            IwbCheckExpireTimeHelper checkExpireTimeHelper,
            IAuthorizationHelper authorizationHelper,
            IAbpWebApiConfiguration configuration,
            ILocalizationManager localizationManager,
            IEventBus eventBus)
        {
            _checkExpireTimeHelper = checkExpireTimeHelper;
            _authorizationHelper = authorizationHelper;
            _configuration = configuration;
            _localizationManager = localizationManager;
            _eventBus = eventBus;
        }

        public virtual async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() ||
                actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return await continuation();
            }

            var methodInfo = actionContext.ActionDescriptor.GetMethodInfoOrNull();
            if (methodInfo == null)
            {
                return await continuation();
            }

            //if (actionContext.ActionDescriptor.IsDynamicAbpAction())
            //{
            //    return await continuation();
            //}

            try
            {
                _checkExpireTimeHelper.CheckUserHasExpire(AbpSession);
                if (AbpSession.UserName != UserBase.AdminUserName)
                    await _authorizationHelper.AuthorizeAsync(methodInfo, methodInfo.DeclaringType);
                return await continuation();
            }
            catch (IwbSessionExpireException ex)
            {
                //this.LogWarn(ex.ToString());
                _eventBus.Trigger(this, new AbpHandledExceptionData(ex));
                return CreateSessionExpireResponse(actionContext, "登陆超时，请重新登陆！");
            }
            catch (AbpAuthorizationException ex)
            {
                //this.LogWarn(ex.ToString());
                _eventBus.Trigger(this, new AbpHandledExceptionData(ex));
                return CreateUnAuthorizedResponse(actionContext);
            }
        }

        protected virtual HttpResponseMessage CreateSessionExpireResponse(HttpActionContext actionContext, string message = null)
        {
            var statusCode = HttpStatusCode.Unauthorized;

            var wrapResultAttribute = GetWrapResultAttributeOrNull(actionContext.ActionDescriptor) ?? _configuration.DefaultWrapResultAttribute;
            var error = message != null ? new ErrorInfo(message) : GetUnAuthorizedErrorMessage(statusCode);
            if (!wrapResultAttribute.WrapOnError)
            {
                return new HttpResponseMessage(statusCode)
                {
                    Content = new ObjectContent<string>(message ?? "",
                        _configuration.HttpConfiguration.Formatters.JsonFormatter)
                };
            }

            return new HttpResponseMessage(statusCode)
            {
                Content = new ObjectContent<AjaxResponse>(new AjaxResponse(error, true),
                    _configuration.HttpConfiguration.Formatters.JsonFormatter)
            };
        }
        protected virtual HttpResponseMessage CreateUnAuthorizedResponse(HttpActionContext actionContext)
        {
            var statusCode = GetUnAuthorizedStatusCode(actionContext);

            var wrapResultAttribute = GetWrapResultAttributeOrNull(actionContext.ActionDescriptor) ?? _configuration.DefaultWrapResultAttribute;

            if (!wrapResultAttribute.WrapOnError)
            {
                return new HttpResponseMessage(statusCode);
            }

            return new HttpResponseMessage(statusCode)
            {
                Content = new ObjectContent<AjaxResponse>(
                    new AjaxResponse(
                        GetUnAuthorizedErrorMessage(statusCode),
                        true
                    ),
                    _configuration.HttpConfiguration.Formatters.JsonFormatter
                )
            };
        }
        private ErrorInfo GetUnAuthorizedErrorMessage(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Forbidden)
            {
                return new ErrorInfo(
                    _localizationManager.GetString(AbpWebConsts.LocalizaionSourceName, "DefaultError403"),
                    _localizationManager.GetString(AbpWebConsts.LocalizaionSourceName, "DefaultErrorDetail403")
                );
            }

            return new ErrorInfo(
                _localizationManager.GetString(AbpWebConsts.LocalizaionSourceName, "DefaultError401"),
                _localizationManager.GetString(AbpWebConsts.LocalizaionSourceName, "DefaultErrorDetail401")
            );
        }

        private static HttpStatusCode GetUnAuthorizedStatusCode(HttpActionContext actionContext)
        {
            return (actionContext.RequestContext.Principal?.Identity?.IsAuthenticated ?? false)
                ? HttpStatusCode.Forbidden
                : HttpStatusCode.Unauthorized;
        }

        public static WrapResultAttribute GetWrapResultAttributeOrNull(HttpActionDescriptor actionDescriptor)
        {
            if (actionDescriptor == null)
            {
                return null;
            }

            //Try to get for dynamic APIs (dynamic web api actions always define __AbpDynamicApiDontWrapResultAttribute)
            var wrapAttr = actionDescriptor.Properties.GetOrDefault("__AbpDynamicApiDontWrapResultAttribute") as WrapResultAttribute;
            if (wrapAttr != null)
            {
                return wrapAttr;
            }

            //Get for the action
            wrapAttr = actionDescriptor.GetCustomAttributes<WrapResultAttribute>(true).FirstOrDefault();
            if (wrapAttr != null)
            {
                return wrapAttr;
            }

            //Get for the controller
            wrapAttr = actionDescriptor.ControllerDescriptor.GetCustomAttributes<WrapResultAttribute>(true).FirstOrDefault();
            if (wrapAttr != null)
            {
                return wrapAttr;
            }

            //Not found
            return null;
        }


    }
}