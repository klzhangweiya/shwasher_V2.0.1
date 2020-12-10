/** iwb_admin-v1.1.0 MIT License By http://www.iwbnet.com e-mail:yueyy@iwbnet.com */
/**
* Name:tab.js
* Author:Van
* E-mail:yueyy@iwbnet.com
* Website:http://www.iwbnet.com
* LICENSE:MIT
*/
layui.define(['jquery', 'element', 'nprogress', 'utils'], function (exports) {
	var $ = layui.jquery,
        //modName = 'tab',
        element = layui.element,
        utils = layui.utils,
        doc = $(document),
        win = $(window),
        renderType = {
        	page: 'page',
        	iframe: 'iframe'
        };
	//私用对象
	var tab = {
		_config: {},
		_filter: 'iwbTab', //过滤器名
		_title: undefined,
		_content: undefined,
		_parentElem: undefined, //要存放的容器
		//检查选项卡DOM是否存在
		tabDomExists: function () {
			var that = this;
			if (doc.find('div.iwb-tab').length > 0) {
				that._title = $('.iwb-tab ul.layui-tab-title');
				that._content = $('.iwb-tab div.layui-tab-content');
				return true;
			}
			return false;
		},
		/**
         * 创建选项卡DOM
         */
		createTabDom: function () {
			var that = this,
                config = that._config;
			that._parentElem = config.elem;
			if (that.tabDomExists())
				return;
			//模板
			var tpl = [
				'<div class="layui-tab layui-tab-card iwb-tab" lay-filter="' + that._filter + '">',
				'<ul class="layui-tab-title">',
				'<li class="layui-this" lay-id="-1" data-url="' +
				config.mainUrl +
				'"><i class="layui-icon layui-icon-home"></i></li>',
				'</ul>',
				'<i class="fa fa-chevron-down iwb-tab-tool"></i>',
				'<div class="iwb-tab-tool-body layui-anim layui-anim-upbit" id="kuaijie">',
				'<ul id="menudiv">',
				// '<li class="iwb-item"><a class="iwb-item" data-target="refresh">刷新当前标签页</a></li>',
				// '<li class="iwb-item"><a class="iwb-item" data-target="closeCurrent">关闭当前标签页</a></li>',
				'<li class="iwb-item"><a class="iwb-item" data-target="closeOther">关闭其他标签页</a></li>',
				'<li class="iwb-item"><a class="iwb-item" data-target="closeAll">关闭全部标签页</a></li>',
				'</ul>',
				'</div>',
				'<div class="layui-tab-content">',
				'<div class="layui-tab-item layui-show" lay-item-id="-1">{{content}}</div>',
				'</div>',
				'</div>'
			];
			var html = tpl.join('');
			switch (config.renderType) {
				case renderType.page:
					html = html.replace('{{content}}', that.getBodyContent(config.mainUrl + '?v=' + new Date().getTime()));
					break;
				case renderType.iframe:
					html = html.replace('{{content}}', '<iframe src="' + config.mainUrl + '"></iframe>');
					break;
			}
			//渲染
			$(config.elem).html(html);
			that._title = $('.iwb-tab ul.layui-tab-title');
			that._content = $('.iwb-tab div.layui-tab-content');
			var tool = $('.iwb-tab-tool'),
                toolBody = $('.iwb-tab-tool-body'),
                tooMenu = $('#kuaijie');
			//监听操作点击事件
			tool.on('click', function () {
				tooMenu.toggle();
			});
			//tooMenu.mouseout(function () {
			//	tooMenu.hide();
			//});
			//监听操作项点击事件
			toolBody.find('a.iwb-item').each(function () {
				var $that = $(this);
				var target = $that.data('target');
				$that.off('click').on('click', function () {
					var layId = that._title.children('li[class=layui-this]').attr('lay-id');
					switch (target) {
						case 'refresh': //刷新
							switch (config.renderType) {
								case renderType.page:
									var loadIndex = that.load();
									var url = that._title.children('li[lay-id=' + layId + ']').data('url');
									that._content.children('div[lay-item-id=' + layId + ']')
                                        .html(that.getBodyContent(url + '?v=' + new Date().getTime(), function () {
                                        	that.closeLoad(loadIndex);
                                        }));
									break;
								case renderType.iframe:
									var item = that._content.children('div[lay-item-id=' + layId + ']').children('iframe');
									item.attr('src', item.attr('src'));
									break;
							}
							break;
						case 'closeCurrent': //关闭当前
							if (layId !== "-1")
								that.tabDelete(layId);
							break;
						case 'closeOther': //关闭其他
							that._title.children('li[lay-id]').each(function () {
								var curId = $(this).attr('lay-id');
								if (curId !== layId && curId !== "-1")
									that.tabDelete(curId);
							});
							break;
						case 'closeAll': //关闭所有
							that._title.children('li[lay-id]').each(function () {
								var curId = $(this).attr('lay-id');
								if (curId !== "-1")
									that.tabDelete(curId);
							});
							that.tabChange(-1);
							break;
					}
					//_tool.click();
					tooMenu.hide();
				});
			});

			//监听浏览器窗口改变事件
			that.winResize();
		},
		load: function () {
			return layer.load(0, { shade: [0.3, '#333'] });
		},
		closeLoad: function (index) {
			setTimeout(function () {
				index && layer.close(index);
			}, 500);
		},
		getBodyContent: function (url, callback) {
			return utils.getBodyContent(utils.loadHtml(url, callback));
		},
		/**
         * 监听浏览器窗口改变事件
         */
		winResize: function () {
			var that = this,
                config = that._config;
			win.on('resize', function () {
				var currBoxHeight = $(that._parentElem).height(); //获取当前容器的高度
				switch (config.renderType) {
					case renderType.page:
						$('.iwb-tab .layui-tab-content').height(currBoxHeight - 43);
						break;
					case renderType.iframe:
						$('.iwb-tab .layui-tab-content iframe').height(currBoxHeight - 47);
						break;
				}
			}).resize();
		},
		/**
         * 检查选项卡是否存在
         */
		tabExists: function (layId) {
			var that = this;
			return that._title.find('li[lay-id=' + layId + ']').length > 0;
		},
		/**
         * 删除选项卡
         */
		tabDelete: function (layId) {
			element.tabDelete(this._filter, layId);
		},
		/**
         * 设置选中选项卡
         */
		tabChange: function (layId) {
			element.tabChange(this._filter, layId);
		},
		/**
         * 获取选项卡对象
         */
		getTab: function (layId) {
			return this._title.find('li[lay-id=' + layId + ']');
		},
		/**
         * 添加一个选项卡，已存在则获取焦点
         */
		tabAdd: function (options) {
			var that = this,
                config = that._config,
                loadIndex = undefined;
			options = options || {
				id: new Date().getTime(),
				title: '新标签页',
				icon: 'fa-file',
				url: '404.html'
			};
			var title = options.title,
                icon = options.icon,
                url = options.url,
                id = options.id;
			if (that.tabExists(id)) {
				that.tabChange(id);
				$('#refresh').click();
				return;
			}
			NProgress.start();
			if (config.openWait)
				loadIndex = that.load();
			var titleHtml = ['<li class="layui-this" lay-id="' + id + '" data-url="' + url + '">'];
			 if (icon) {
			     if (icon.indexOf('fa-') !== -1) {
			         titleHtml.push('<i class="fa ' + icon + '" aria-hidden="true"></i>');
			     } else {
			         titleHtml.push('<i class="layui-icon">' + icon + '</i>');
			     }
			 }
			titleHtml.push('&nbsp;' + title);
			titleHtml.push('<i class="layui-icon layui-unselect layui-tab-close">&#x1006;</i>');
			titleHtml.push('</li>');
			var contentHtm = '<div class="layui-tab-item layui-show" lay-item-id="' + id + '">{{content}}</div>';
			switch (config.renderType) {
				case renderType.page:
					contentHtm = contentHtm.replace('{{content}}', that.getBodyContent(url + '?v=' + new Date().getTime(), function () {
						setTimeout(function () {
							NProgress.done();
							config.openWait && loadIndex && that.closeLoad(loadIndex);
						}, 500);
					}));
					break;
				case renderType.iframe:
					contentHtm = contentHtm.replace('{{content}}', '<iframe src="' + url + '"></iframe>');
					break;
			}
			//追加html
			that._title.append(titleHtml.join(''));
			that._content.append(contentHtm);
			if (config.renderType === renderType.iframe) {
				that._content.find('div[lay-item-id=' + id + ']').find('iframe').on('load', function () {
					NProgress.done();
					config.openWait && loadIndex && that.closeLoad(loadIndex);
				});
			}
			//监听选项卡关闭事件
			that.getTab(id).find('i.layui-tab-close').off('click').on('click', function () {
				//关闭之前
				if (config.closeBefore) {
					if (config.closeBefore(options)) {
						that.tabDelete(id);
					}
				} else {
					that.tabDelete(id);
				}
			});
			that.tabChange(id);
			that.winResize();
			if (config.onSwitch) {
				element.on('tab(' + that._filter + ')', function (data) {
					config.onSwitch({
						index: data.index,
						elem: data.elem,
						layId: that._title.children('li').eq(data.index).attr('lay-id')
					});
				});
			}
		},
		/**
         * 获取当前选项卡的id
         */
		getCurrLayId: function () {
			return this._title.find('li.layui-this').attr('lay-id');
		}
	};

// ReSharper disable once InconsistentNaming
	var Tab = function () {
		this.config = {
			elem: undefined,
			mainUrl: "/Main/Home",
			renderType: 'iframe',
			openWait: false
		};
		this.v = '1.0.5';
	};
	Tab.fn = Tab.prototype;
	Tab.fn.set = function (options) {
		var that = this;
		$.extend(true, that.config, options);
		return that;
	};
	/**
     * 渲染选项卡
     */
	Tab.fn.render = function () {
		var that = this,
            config = that.config;
		if (config.elem === undefined) {
			layui.hint().error('Tab error:请配置选择卡容器.');
			return that;
		}
		tab._config = config;
		tab.createTabDom();
		return that;
	};
	/**
     * 添加一个选项卡
     */
	Tab.fn.tabAdd = function (params) {
		tab.tabAdd(params);
	};
	/**
     * 关闭一个选项卡
     */
	Tab.fn.close = function (layId) {
		tab.tabDelete(layId);
	};
	Tab.fn.getId = function () {
		return tab.getCurrLayId();
	};
	

	var t = new Tab();

	exports('tab', t);
});



//layui.define(['jquery', 'element', 'nprogress', 'utils'], function (exports) {
//	var $ = layui.jquery,
//        //modName = 'tab',
//        element = layui.element,
//        utils = layui.utils,
//        doc = $(document),
//        win = $(window),
//        renderType = {
//        	page: 'page',
//        	iframe: 'iframe'
//        };
//	var Tab = function () {
//		this.config = {
//			elem: undefined,
//			mainUrl: "/Home/index",
//			renderType: 'iframe',
//			openWait: false
//		};
//		this.v = '1.0.5';
//	};
//	Tab.fn = Tab.prototype;
//	Tab.fn.set = function (options) {
//		var that = this;
//		$.extend(true, that.config, options);
//		return that;
//	};
//	//私用对象
//	var tab = {
//		_config: {},
//		_filter: 'iwbTab', //过滤器名
//		_title: undefined,
//		_content: undefined,
//		_parentElem: undefined, //要存放的容器
//		//检查选项卡DOM是否存在
//		tabDomExists: function () {
//			var that = this;
//			if (doc.find('div.iwb-tab').length > 0) {
//				that._title = $('.iwb-tab ul.layui-tab-title');
//				that._content = $('.iwb-tab div.layui-tab-content');
//				return true;
//			}
//			return false;
//		},
//		/**
//         * 创建选项卡DOM
//         */
//		createTabDom: function () {
//			var that = this,
//                config = that._config;
//			that._parentElem = config.elem;
//			if (that.tabDomExists())
//				return;
//			//模板
//			var tpl = [
//				'<div class="layui-tab layui-tab-card iwb-tab" lay-filter="' + that._filter + '">',
//				'<ul class="layui-tab-title">',
//				'<li class="layui-this" lay-id="-1" data-url="' +
//				config.mainUrl +
//				'"><i class="layui-icon layui-icon-home"></i></li>',
//				'</ul>',
//				'<i class="fa fa-chevron-down iwb-tab-tool"></i>',
//				'<div class="iwb-tab-tool-body layui-anim layui-anim-upbit" id="kuaijie">',
//				'<ul id="menudiv">',
//				// '<li class="iwb-item"><a class="iwb-item" data-target="refresh">刷新当前标签页</a></li>',
//				// '<li class="iwb-item"><a class="iwb-item" data-target="closeCurrent">关闭当前标签页</a></li>',
//				'<li class="iwb-item"><a class="iwb-item" data-target="closeOther">关闭其他标签页</a></li>',
//				'<li class="iwb-item"><a class="iwb-item" data-target="closeAll">关闭全部标签页</a></li>',
//				'</ul>',
//				'</div>',
//				'<div class="layui-tab-content">',
//				'<div class="layui-tab-item layui-show" lay-item-id="-1">{{content}}</div>',
//				'</div>',
//				'</div>'
//			];
//			var html = tpl.join('');
//			switch (config.renderType) {
//				case renderType.page:
//					html = html.replace('{{content}}', that.getBodyContent(config.mainUrl + '?v=' + new Date().getTime()));
//					break;
//				case renderType.iframe:
//					html = html.replace('{{content}}', '<iframe src="' + config.mainUrl + '"></iframe>');
//					break;
//			}
//			//渲染
//			$(config.elem).html(html);
//			that._title = $('.iwb-tab ul.layui-tab-title');
//			that._content = $('.iwb-tab div.layui-tab-content');
//			var tool = $('.iwb-tab-tool'),
//                toolBody = $('.iwb-tab-tool-body'),
//                tooMenu = $('#kuaijie');
//			//监听操作点击事件
//			tool.on('click', function () {
//				tooMenu.toggle();
//			});
//			tooMenu.mouseout(function() {
//				tooMenu.hide();
//			});
//			//监听操作项点击事件
//			toolBody.find('a.iwb-item').each(function () {
//				var $that = $(this);
//				var target = $that.data('target');
//				$that.off('click').on('click', function () {
//					var layId = that._title.children('li[class=layui-this]').attr('lay-id');
//					switch (target) {
//						case 'refresh': //刷新
//							switch (config.renderType) {
//								case renderType.page:
//									var loadIndex = that.load();
//									var url = that._title.children('li[lay-id=' + layId + ']').data('url');
//									that._content.children('div[lay-item-id=' + layId + ']')
//                                        .html(that.getBodyContent(url + '?v=' + new Date().getTime(), function () {
//                                        	that.closeLoad(loadIndex);
//                                        }));
//									break;
//								case renderType.iframe:
//									var item = that._content.children('div[lay-item-id=' + layId + ']').children('iframe');
//									item.attr('src', item.attr('src'));
//									break;
//							}
//							break;
//						case 'closeCurrent': //关闭当前
//							if (layId !== -1)
//								that.tabDelete(layId);
//							break;
//						case 'closeOther': //关闭其他
//							that._title.children('li[lay-id]').each(function () {
//								var curId = $(this).attr('lay-id');
//								if (curId !== layId && curId !== -1)
//									that.tabDelete(curId);
//							});
//							break;
//						case 'closeAll': //关闭所有
//							that._title.children('li[lay-id]').each(function () {
//								var curId = $(this).attr('lay-id');
//								if (curId !== -1)
//									that.tabDelete(curId);
//							});
//							that.tabChange(-1);
//							break;
//					}
//					//_tool.click();
//					tooMenu.hide();
//				});
//			});

//			//监听浏览器窗口改变事件
//			that.winResize();
//		},
//		load: function () {
//			return layer.load(0, { shade: [0.3, '#333'] });
//		},
//		closeLoad: function (index) {
//			setTimeout(function () {
//				index && layer.close(index);
//			}, 500);
//		},
//		getBodyContent: function (url, callback) {
//			return utils.getBodyContent(utils.loadHtml(url, callback));
//		},
//		/**
//         * 监听浏览器窗口改变事件
//         */
//		winResize: function () {
//			var that = this,
//                config = that._config;
//			win.on('resize', function () {
//				var currBoxHeight = $(that._parentElem).height(); //获取当前容器的高度
//				switch (config.renderType) {
//					case renderType.page:
//						$('.iwb-tab .layui-tab-content').height(currBoxHeight - 43);
//						break;
//					case renderType.iframe:
//						$('.iwb-tab .layui-tab-content iframe').height(currBoxHeight - 47);
//						break;
//				}
//			}).resize();
//		},
//		/**
//         * 检查选项卡是否存在
//         */
//		tabExists: function (layId) {
//			var that = this;
//			return that._title.find('li[lay-id=' + layId + ']').length > 0;
//		},
//		/**
//         * 删除选项卡
//         */
//		tabDelete: function (layId) {
//			element.tabDelete(this._filter, layId);
//		},
//		/**
//         * 设置选中选项卡
//         */
//		tabChange: function (layId) {
//			element.tabChange(this._filter, layId);
//		},
//		/**
//         * 获取选项卡对象
//         */
//		getTab: function (layId) {
//			return this._title.find('li[lay-id=' + layId + ']');
//		},
//		/**
//         * 添加一个选项卡，已存在则获取焦点
//         */
//		tabAdd: function (options) {
//			var that = this,
//                config = that._config,
//                loadIndex = undefined;
//			options = options || {
//				id: new Date().getTime(),
//				title: '新标签页',
//				icon: 'fa-file',
//				url: '404.html'
//			};
//			var title = options.title,
//                icon = options.icon,
//                url = options.url,
//                id = options.id;
//			if (that.tabExists(id)) {
//				that.tabChange(id);
//				return;
//			}
//			NProgress.start();
//			if (config.openWait)
//				loadIndex = that.load();
//			var titleHtml = ['<li class="layui-this" lay-id="' + id + '" data-url="' + url + '">'];
//			if (icon) {
//			    if (icon.indexOf('fa-') !== -1) {
//			        titleHtml.push('<i class="fa ' + icon + '" aria-hidden="true"></i>');
//			    } else {
//			        titleHtml.push('<i class="layui-icon">' + icon + '</i>');
//			    }
//			}
//			titleHtml.push('&nbsp;' + title);
//			titleHtml.push('<i class="layui-icon layui-unselect layui-tab-close">&#x1006;</i>');
//			titleHtml.push('</li>');
//			var contentHtm = '<div class="layui-tab-item layui-show" lay-item-id="' + id + '">{{content}}</div>';
//			switch (config.renderType) {
//				case renderType.page:
//					contentHtm = contentHtm.replace('{{content}}', that.getBodyContent(url + '?v=' + new Date().getTime(), function () {
//						setTimeout(function () {
//							NProgress.done();
//							config.openWait && loadIndex && that.closeLoad(loadIndex);
//						}, 500);
//					}));
//					break;
//				case renderType.iframe:
//					contentHtm = contentHtm.replace('{{content}}', '<iframe src="' + url + '"></iframe>');
//					break;
//			}
//			//追加html
//			that._title.append(titleHtml.join(''));
//			that._content.append(contentHtm);
//			if (config.renderType === renderType.iframe) {
//				that._content.find('div[lay-item-id=' + id + ']').find('iframe').on('load', function () {
//					NProgress.done();
//					config.openWait && loadIndex && that.closeLoad(loadIndex);
//				});
//			}
//			//监听选项卡关闭事件
//			that.getTab(id).find('i.layui-tab-close').off('click').on('click', function () {
//				//关闭之前
//				if (config.closeBefore) {
//					if (config.closeBefore(options)) {
//						that.tabDelete(id);
//					}
//				} else {
//					that.tabDelete(id);
//				}
//			});
//			that.tabChange(id);
//			that.winResize();
//			if (config.onSwitch) {
//				element.on('tab(' + that._filter + ')', function (data) {
//					config.onSwitch({
//						index: data.index,
//						elem: data.elem,
//						layId: that._title.children('li').eq(data.index).attr('lay-id')
//					});
//				});
//			}
//		},
//		/**
//         * 获取当前选项卡的id
//         */
//		getCurrLayId: function () {
//			return this._title.find('li.layui-this').attr('lay-id');
//		}
//	};
//	/**
//     * 渲染选项卡
//     */
//	Tab.fn.render = function () {
//		var that = this,
//            config = that.config;
//		if (config.elem === undefined) {
//			layui.hint().error('Tab error:请配置选择卡容器.');
//			return that;
//		}
//		tab._config = config;
//		tab.createTabDom();
//		return that;
//	};
//	/**
//     * 添加一个选项卡
//     */
//	Tab.fn.tabAdd = function (params) {
//		tab.tabAdd(params);
//	};
//	/**
//     * 关闭一个选项卡
//     */
//	Tab.fn.close = function (layId) {
//		tab.tabDelete(layId);
//	};
//	Tab.fn.getId = function () {
//		return tab.getCurrLayId();
//	};
	

//	var t = new Tab();

//	exports('tab', t);
//});