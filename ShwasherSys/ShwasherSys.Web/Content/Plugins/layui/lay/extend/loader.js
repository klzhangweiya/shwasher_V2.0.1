/** iwb_admin-v1.1.0 MIT License By http://www.iwbnet.com e-mail:yueyy@iwbnet.com */
;/**
 * Name:loader.js
 * Author:Van
 * E-mail:yueyy@iwbnet.com
 * Website:http://www.iwbnet.com
 * LICENSE:MIT
 */
layui.define(['jquery', 'nprogress'], function (exports) {
	var $ = layui.jquery,
        modName = 'loader';

	var loader = {
		version: '1.0.1',
		load: function (options) {
			NProgress.start();
			var url = options.url,
                name = options.name,
                id = options.id,
                elem = options.elem !== undefined ? $(options.elem) : $('#container');
			elem.load(url, function (res, status, xhr) {
				if (status === "error" && typeof options.onError === 'function') {
					options.onError();
				}
				if (status === 'success') {
					elem.html(res);
					typeof options.onSuccess === 'function' && options.onSuccess({ name: name, id: id });
				}
				typeof options.onComplate === 'function' && options.onComplate();
				NProgress.done();
			});
		},
		//动态加载script
		getScript: function (url, callback) {
			$.getScript(url, callback);
		}
	};
	exports('loader', loader);
});