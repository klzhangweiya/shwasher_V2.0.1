var abp = abp || {};
(function ($) {
    if (!sweetAlert || !$) {
        return;
    }

    /* DEFAULTS *************************************************/

    abp.libs = abp.libs || {};
    abp.libs.sweetAlert = {
        config: {
            'default': {

            },
            info: {
                type: 'info'
            },
            success: {
                type: 'success'
            },
            warning: {
                type: 'warning'
            },
            error: {
                type: 'error'
            },
            confirm: {
				title: 'Are you sure?',
				type: 'warning',
				showCancelButton: true,
				/*confirmButtonColor: '#3085d6',
				cancelButtonColor: '#d33',*/
				confirmButtonText: abp.localization
					.iwbZero("Confirm"),
				cancelButtonText: abp.localization
					.iwbZero("Cancel")
            }
        }
    };

    /* MESSAGE **************************************************/

    var showMessage = function (type, message, title) {
        if (!title) {
            title = message;
            message = undefined;
        }

        var opts = $.extend(
            {},
            abp.libs.sweetAlert.config['default'],
            abp.libs.sweetAlert.config[type],
            {
                title: title,
                text: message
            }
        );

        return $.Deferred(function ($dfd) {
            sweetAlert(opts).then(function () {
                $dfd.resolve();
            });
        });
    };

    abp.message.info = function (message, title) {
        return showMessage('info', message, title);
    };

    abp.message.success = function (message, title) {
		title = title || "Success";
		return showMessage('success', message, title);
    };

    abp.message.warn = function (message, title) {
		title = title || "Warning";
		return showMessage('warning', message, title);
    };

	abp.message.error = function (message, title) {
		title = title || "Error";
        return showMessage('error', message, title);
    };

    abp.message.confirm = function (message, titleOrCallback, callback) {
        var userOpts = {
            text: message
        };

        if ($.isFunction(titleOrCallback)) {
            callback = titleOrCallback;
        } else if (titleOrCallback) {
            userOpts.title = titleOrCallback;
        };

        var opts = $.extend(
            {},
            abp.libs.sweetAlert.config['default'],
            abp.libs.sweetAlert.config.confirm,
			userOpts,
			opts
        );

        return $.Deferred(function ($dfd) {
			sweetAlert(opts).then(function (result) {
				if (result.value) {
					callback && callback(result);
				}
                $dfd.resolve(result);
            });
        });
    };

    abp.event.on('abp.dynamicScriptsInitialized', function () {
        abp.libs.sweetAlert.config.confirm.title = abp.localization.abpWeb('AreYouSure');
        abp.libs.sweetAlert.config.confirm.buttons = [abp.localization.abpWeb('Cancel'), abp.localization.abpWeb('Yes')];
    });

})(jQuery);