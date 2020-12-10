/** iwb_admin-v1.1.0 MIT License By http://www.iwbnet.com e-mail:yueyy@iwbnet.com */
;/**
 * Name:message.js
 * Author:Van
 * E-mail:yueyy@iwbnet.com
 * Website:http://www.iwbnet.com
 * LICENSE:MIT
 */
layui.define(['jquery', 'iwbConfig'], function (exports) {
	var $ = layui.jquery,
        iwbConfig = layui.iwbConfig,
        //modName = 'message',
        //doc = $(document),
        body = $('body'),
        msgSelect = 'iwb-message';
	var message = {
		v: '1.0.0',
		times: 1,
		_message: function () {
			var msg = $("." + msgSelect);
			if (msg.length > 0)
				return msg;
			body.append('<div class="'+msgSelect+'"></div>');
			return $("." + msgSelect);
		},
		show: function (options) {
			options = options || {};
			var that = this,
                
                id = that.times,
                skin = options.skin === undefined ? 'blue' : options.skin,
                msg = options.msg === undefined ? '请输入一些提示信息!' : options.msg,
                autoClose = options.autoClose === undefined ? true : options.autoClose,
				ms = that._message();
			var tpl = [
                '<div class="iwb-message-item layui-anim layui-anim-upbit" data-times="' + id + '">',
                '<div class="iwb-message-body iwb-skin-' + skin + '">',
                msg,
                '</div>',
                '<div class="iwb-close iwb-skin-' + skin + '"><i class="fa fa-times" aria-hidden="true"></i></div>',
                '</div>'
			];
			ms.append(tpl.join(''));
			var times = ms.children('div[data-times=' + id + ']').find('i.fa-times');
			times.off('click').on('click', function () {
				var t = $(this).parents('div.iwb-message-item').removeClass('layui-anim-upbit').addClass('layui-anim-fadeout');
				setTimeout(function () {
					t.remove();
				}, 1000);
			});
			if (autoClose) { //是否自动关闭
				setTimeout(function () {
					times.click();
				}, 3000);
			}
			that.times++;
		}
	};
	layui.link(iwbConfig.resourcePath + 'plugins/layui/css/extend/message.css');
	exports('message', message);
});