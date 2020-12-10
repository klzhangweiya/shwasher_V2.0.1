var abp = abp || {};
(function ($) {

    if (!$) {
        return;
    }

    /* JQUERY ENHANCEMENTS ***************************************************/

    // abp.ajax -> uses $.ajax ------------------------------------------------

    abp.ajax = function (userOptions) {
        userOptions = userOptions || {};

        var options = $.extend(true, {}, abp.ajax.defaultOpts, userOptions);
        var oldBeforeSendOption = options.beforeSend;		
        options.beforeSend = function(xhr) {
            abp.ajax.blockUI(options);
            if (oldBeforeSendOption) {
                 oldBeforeSendOption(xhr);
            }

            xhr.setRequestHeader("Pragma", "no-cache");
            xhr.setRequestHeader("Cache-Control", "no-cache");
            xhr.setRequestHeader("Expires", "Sat, 01 Jan 2000 00:00:00 GMT");
        };

        options.success = undefined;
        options.error = undefined;

        return $.Deferred(function ($dfd) {
            $.ajax(options)
                .done(function (data, textStatus, jqXhr) {
                    abp.ajax.unblockUI(options);
                    if (data.__abp) {
                        abp.ajax.handleResponse(data, userOptions, $dfd, jqXhr);
                    } else {
                        $dfd.resolve(data);
                        userOptions.success && userOptions.success(data);
                    }
                }).fail(function (jqXhr) {
                    abp.ajax.unblockUI(options);
                    if (jqXhr.responseJSON && jqXhr.responseJSON.__abp) {
                        abp.ajax.handleResponse(jqXhr.responseJSON, userOptions, $dfd, jqXhr);
                    } else {
                        abp.ajax.handleNonAbpErrorResponse(jqXhr, userOptions, $dfd);
                    }
                });
        });
    };

    $.extend(abp.ajax, {
        defaultOpts: {
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        },

        defaultError: {
            Message: 'An error has occurred!',
            Details: 'Error detail not sent by server.'
        },

        defaultError401: {
            Message: 'You are not authenticated!',
            Details: 'You should be authenticated (sign in) in order to perform this operation.'
        },

        defaultError403: {
            Message: 'You are not authorized!',
            Details: 'You are not allowed to perform this operation.'
        },

        defaultError404: {
            Message: 'Resource not found!',
            Details: 'The resource requested could not found on the server.'
        },


        logError: function (error) {
            abp.log.error(error);
        },

        showError: function (error) {
            if (error.details) {
                return abp.message.error(error.details, error.message);
            } else {
                if (error.message&&error.message.indexOf("登陆超时") >= 0) {
                    return abp.message.error(error.message).done(function() {
                        top.location.reload();
                    });
                } else {
                    return abp.message.error(error.message || abp.ajax.defaultError.message);

                }
            }
        },

        handleTargetUrl: function (targetUrl) {
            if (!targetUrl) {
                location.href = abp.appPath;
            } else {
                location.href = targetUrl;
            }
        },

        handleNonAbpErrorResponse: function (jqXHR, userOptions, $dfd) {
            if (userOptions.abpHandleError !== false) {
                switch (jqXHR.status) {
                    case 401:
                        abp.ajax.handleUnAuthorizedRequest(
                            abp.ajax.showError(abp.ajax.defaultError401),
                            abp.appPath
                        );
                        break;
                    case 403:
                        abp.ajax.showError(abp.ajax.defaultError403);
                        break;
                    case 404:
                        abp.ajax.showError(abp.ajax.defaultError404);
                        break;
                    default:
                        abp.ajax.showError(abp.ajax.defaultError);
                        break;
                }
            }

            $dfd.reject.apply(this, arguments);
            userOptions.error && userOptions.error.apply(this, arguments);
        },

        handleUnAuthorizedRequest: function (messagePromise, targetUrl) {
            if (messagePromise) {
                messagePromise.done(function () {
                    abp.ajax.handleTargetUrl(targetUrl);
                });
            } else {
                abp.ajax.handleTargetUrl(targetUrl);
            }
        },

        handleResponse: function (data, userOptions, $dfd, jqXHR) {
            if (data) {
                if (data.success === true) {
                    $dfd && $dfd.resolve(data.result, data, jqXHR);
                    userOptions.success && userOptions.success(data.result, data, jqXHR);

                    if (data.targetUrl) {
                        abp.ajax.handleTargetUrl(data.targetUrl);
                    }
                } else if (data.success === false) {
                    var messagePromise = null;

                    if (data.error) {
                        if (userOptions.abpHandleError !== false) {
                            messagePromise = abp.ajax.showError(data.error);
                        }
                    } else {
                        data.error = abp.ajax.defaultError;
                    }

                    abp.ajax.logError(data.error);

                    $dfd && $dfd.reject(data.error, jqXHR);
                    userOptions.error && userOptions.error(data.error, jqXHR);

                    if (jqXHR.status === 401 && userOptions.abpHandleError !== false) {
                        abp.ajax.handleUnAuthorizedRequest(messagePromise, data.targetUrl);
                    }
                } else { //not wrapped result
                    $dfd && $dfd.resolve(data, null, jqXHR);
                    userOptions.success && userOptions.success(data, null, jqXHR);
                }
            } else { //no data sent to back
                $dfd && $dfd.resolve(jqXHR);
                userOptions.success && userOptions.success(jqXHR);
            }
        },

        blockUI: function (options) {
            if (options.blockUI) {
                if (options.blockUI === true) { //block whole page
                    abp.ui.setBusy();
                } else { //block an element
                    abp.ui.setBusy(options.blockUI);
                }
            }
        },
        unblockUI: function (options) {
            if (options.blockUI) {
                if (options.blockUI === true) { //unblock whole page
                    abp.ui.clearBusy();
                } else { //unblock an element
                    abp.ui.clearBusy(options.blockUI);
                }
            }
        },
        ajaxSendHandler: function (event, request, settings) {
            var token = abp.security.antiForgery.getToken();
            if (!token) {
                return;
            }

            if (!abp.security.antiForgery.shouldSendToken(settings)) {
                return;
            }

            if (!settings.headers || settings.headers[abp.security.antiForgery.tokenHeaderName] === undefined) {
                request.setRequestHeader(abp.security.antiForgery.tokenHeaderName, token);
            }
        },

        lowerJsonKey:function (jsonObj) {
            if(jsonObj=== undefined || jsonObj === null) {
                return [];
            }
            if (jsonObj.length !== undefined) {
                for (var i = 0; i < jsonObj.length; i++) {
                    lowerJsonKeyChild(jsonObj[i]);
                }
            } else {
                lowerJsonKeyChild(jsonObj);
            }
            return jsonObj;
        },
        lowerJsonKeyChild :function (jsonObj) {
            for (var k in jsonObj) {
                if (jsonObj.hasOwnProperty(k)) {
                    var obj = jsonObj[k];
                    if (typeof (obj) === "object" && (Object.prototype.toString.call(obj).toLowerCase() === "[object object]" || Object.prototype.toString.call(obj).toLowerCase() === '[object array]')) {
                        jsonObj[k.substring(0, 1).toLowerCase() + k.substring(1)] = lowerJsonKey(obj);
                        delete (jsonObj[k]);
                    } else {
                        jsonObj[k.substring(0, 1).toLowerCase() + k.substring(1)] = jsonObj[k];
                        delete (jsonObj[k]);
                    }
                }
            }
            return jsonObj;
        }

    });

    $(document).ajaxSend(function (event, request, settings) {
        return abp.ajax.ajaxSendHandler(event, request, settings);
    });

    /* JQUERY PLUGIN ENHANCEMENTS ********************************************/

    /* jQuery Form Plugin 
     * http://www.malsup.com/jquery/form/
     */

    // abpAjaxForm -> uses ajaxForm ------------------------------------------

    if ($.fn.ajaxForm) {
        $.fn.abpAjaxForm = function (userOptions) {
            userOptions = userOptions || {};

            var options = $.extend({}, $.fn.abpAjaxForm.defaults, userOptions);

            options.beforeSubmit = function () {
                abp.ajax.blockUI(options);
                userOptions.beforeSubmit && userOptions.beforeSubmit.apply(this, arguments);
            };

            options.success = function (data) {
                abp.ajax.handleResponse(data, userOptions);
            };

            //TODO: Error?

            options.complete = function () {
                abp.ajax.unblockUI(options);
                userOptions.complete && userOptions.complete.apply(this, arguments);
            };

            return this.ajaxForm(options);
        };

        $.fn.abpAjaxForm.defaults = {
            method: 'POST'
        };
    }

    abp.event.on('abp.dynamicScriptsInitialized', function () {
		abp.ajax.defaultError.Message = abp.localization.abpWeb('DefaultError');
		abp.ajax.defaultError.Details = abp.localization.abpWeb('DefaultErrorDetail');
		abp.ajax.defaultError401.Message = abp.localization.abpWeb('DefaultError401');
		abp.ajax.defaultError401.Details = abp.localization.abpWeb('DefaultErrorDetail401');
		abp.ajax.defaultError403.Message = abp.localization.abpWeb('DefaultError403');
		abp.ajax.defaultError403.Details = abp.localization.abpWeb('DefaultErrorDetail403');
		abp.ajax.defaultError404.Message = abp.localization.abpWeb('DefaultError404');
		abp.ajax.defaultError404.Details = abp.localization.abpWeb('DefaultErrorDetail404');
    });

})(jQuery);
