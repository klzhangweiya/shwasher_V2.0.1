/** navbar.js By Beginner */
layui.define(['element', 'common'], function(exports) {
	"use strict";
	var $ = layui.jquery,
		//layer = parent.layer === undefined ? layui.layer : parent.layer,
		element = layui.element,
		common = layui.common,
		cacheName = 'tb_navbar';

// ReSharper disable once InconsistentNaming
	var Navbar = function() {
		/**
		 *  默认配置 
		 */
		this.config = {
			elem: undefined, //容器
			data: undefined, //数据源
			url: undefined, //数据源地址
			type: 'post', //读取方式
			cached: false, //是否使用缓存
			spreadOne: false, //设置是否只展开一个二级菜单
			async:true//是否异步请求
		};
		this.v = '0.0.1';
	};
	Navbar.prototype.render = function() {
		var that = this;
		var config = that.config;
		if(typeof(config.elem) !== 'string' && typeof(config.elem) !== 'object') {
			common.throwError('Navbar error: elem参数未定义或设置出错，具体设置格式请参考文档API.');
		}
		var $container;
		if(typeof(config.elem) === 'string') {
			$container = $('' + config.elem + '');
		}
		if(typeof(config.elem) === 'object') {
			$container = config.elem;
		}
// ReSharper disable once UsageOfPossiblyUnassignedValue
// ReSharper disable once QualifiedExpressionMaybeNull
		if($container.length === 0) {
			common.throwError('Navbar error:找不到elem参数配置的容器，请检查.');
		}
		if(config.data === undefined && config.url === undefined) {
			common.throwError('Navbar error:请为Navbar配置数据源.');
		}
		var html;
		if(config.data !== undefined && typeof(config.data) === 'object') {
			html = getHtml(config.data);
			$container.html(html);
			element.init();
			that.config.elem = $container;
		} else {
			if(config.cached) {
				var cacheNavbar = layui.data(cacheName);
				if(cacheNavbar.navbar === undefined) {
					$.ajax({
						type: config.type,
						url: config.url,
						async: config.async,
						dataType: 'json',
						success: function(result) {
							//添加缓存
							layui.data(cacheName, {
								key: 'navbar',
								value: result
							});
							var html = getHtml(result);
							$container.html(html);
							element.init();
						},
						error: function(xhr, status, error) {
							common.msgError('Navbar error:' + error);
						},
						complete: function() {
							that.config.elem = $container;
						}
					});
				} else {
					html = getHtml(cacheNavbar.navbar);
					$container.html(html);
					element.init();
					that.config.elem = $container;
				}
			} else {
				//清空缓存
				layui.data(cacheName, null);
				$.ajax({
					type: config.type,
					url: config.url,
					async: false, //_config.async,
					dataType: 'json',
					success: function(result) {
						var html = getHtml(result);
						$container.html(html);
						element.init();
					},
					error: function(xhr, status, error) {
						common.msgError('Navbar error:' + error);
					},
					complete: function() {
						that.config.elem = $container;
					}
				});
			}
		}
		
		//只展开一个二级菜单
		if(config.spreadOne){
			var $ul = $container.children('ul');
			$ul.find('li.layui-nav-item').each(function(){
				$(this).on('click',function(){
					$(this).siblings().removeClass('layui-nav-itemed');
				});
			});
		}
		return that;
	};
	/**
	 * 配置Navbar
	 * @param {Object} options
	 */
	Navbar.prototype.set = function(options) {
		var that = this;
		that.config.data = undefined;
		$.extend(true, that.config, options);
		return that;
	};
	/**
	 * 绑定事件
	 * @param {String} events
	 * @param {Function} callback
	 */
	Navbar.prototype.on = function(events, callback) {
		var that = this;
		var con = that.config.elem;
		if(typeof(events) !== 'string') {
			common.throwError('Navbar error:事件名配置出错，请参考API文档.');
		}
		var lIndex = events.indexOf('(');
		var eventName = events.substr(0, lIndex);
		//var filter = events.substring(lIndex + 1, events.indexOf(')'));
		if(eventName === 'click') {
			if(con.attr('lay-filter') !== undefined) {
				con.children('ul').find('li').each(function() {
					var $this = $(this);
					if ($this.find('dl').length > 0) {
						//var $dd =
						 $this.find('dd').each(function() {
							$(this).on('click', function() {
								var $a = $(this).children('a');
								var href = $a.data('url');
								var icon = $a.children('i:first').data('icon');
								var title = $a.children('cite').text();
								var data = {
									elem: $a,
									field: {
										href: href,
										icon: icon,
										title: title
									}
								}
								callback(data);
							});
						});
					} else {
						$this.on('click', function() {
							var $a = $this.children('a');
							var href = $a.data('url');
							var icon = $a.children('i:first').data('icon');
							var title = $a.children('cite').text();
							var data = {
								elem: $a,
								field: {
									href: href,
									icon: icon,
									title: title
								}
							}
							callback(data);
						});
					}
				});
			}
		}
	};
	/**
	 * 清除缓存
	 */
	Navbar.prototype.cleanCached = function(){
		layui.data(cacheName,null);
	};
	/**
	 * 获取html字符串
	 * @param {Object} data
	 */
	//function getHtml(data) {
	//	var ulHtml = '<ul class="layui-nav layui-nav-tree beg-navbar">';
	//	for(var i = 0; i < data.length; i++) {
	//		if(data[i].spread) {
	//			ulHtml += '<li class="layui-nav-item layui-nav-itemed">';
	//		} else {
	//			ulHtml += '<li class="layui-nav-item">';
	//		}
	//		if(data[i].children !== undefined && data[i].children.length > 0) {
	//			ulHtml += '<a href="javascript:;">';
	//			if(data[i].icon !== undefined && data[i].icon !== '') {
	//				if(data[i].icon.indexOf('fa-') !== -1) {
	//					ulHtml += '<i class="fa ' + data[i].icon + '" aria-hidden="true" data-icon="' + data[i].icon + '"></i>';
	//				} else {
	//					ulHtml += '<i class="layui-icon" data-icon="' + data[i].icon + '">' + data[i].icon + '</i>';
	//				}
	//			}
	//			ulHtml += '<cite>' + data[i].title + '</cite>';
	//			ulHtml += '</a>';
	//			ulHtml += '<dl class="layui-nav-child">';
	//			for(var j = 0; j < data[i].children.length; j++) {
	//				ulHtml += '<dd title="'+data[i].children[j].title+'">';
	//				ulHtml += '<a href="javascript:;" data-url="' + data[i].children[j].href + '">';
	//				if(data[i].children[j].icon !== undefined && data[i].children[j].icon !== '') {
	//					if(data[i].children[j].icon.indexOf('fa-') !== -1) {
	//						ulHtml += '<i class="fa ' + data[i].children[j].icon + '" data-icon="' + data[i].children[j].icon + '" aria-hidden="true"></i>';
	//					} else {
	//						ulHtml += '<i class="layui-icon" data-icon="' + data[i].children[j].icon + '">' + data[i].children[j].icon + '</i>';
	//					}
	//				}
	//				ulHtml += '<cite>' + data[i].children[j].title + '</cite>';
	//				ulHtml += '</a>';
	//				ulHtml += '</dd>';
	//			}
	//			ulHtml += '</dl>';
	//		} else {
	//			var dataUrl = (data[i].href !== undefined && data[i].href !== '') ? 'data-url="' + data[i].href + '"' : '';
	//			ulHtml += '<a href="javascript:;" ' + dataUrl + '>';
	//			if(data[i].icon !== undefined && data[i].icon !== '') {
	//				if(data[i].icon.indexOf('fa-') !== -1) {
	//					ulHtml += '<i class="fa ' + data[i].icon + '" aria-hidden="true" data-icon="' + data[i].icon + '"></i>';
	//				} else {
	//					ulHtml += '<i class="layui-icon" data-icon="' + data[i].icon + '">' + data[i].icon + '</i>';
	//				}
	//			}
	//			ulHtml += '<cite>' + data[i].title + '</cite>';
	//			ulHtml += '</a>';
	//		}
	//		ulHtml += '</li>';
	//	}
	//	ulHtml += '</ul>';
	//	return ulHtml;
	//}

	function getHtml(data) {
		var ulHtml = '<ul class="layui-nav layui-nav-tree beg-navbar">';
		for (var i = 0; i < data.length; i++) {
			if (data[i].spread) {
				ulHtml += '<li class="layui-nav-item layui-nav-itemed">';
			} else {
				ulHtml += '<li class="layui-nav-item">';
			}
			if (data[i].children !== undefined && data[i].children.length > 0) {
				console.log(i+"---child");
				ulHtml += '<a href="javascript:;">';
				ulHtml += getIcon(data[i].icon);
				ulHtml += '<cite>' + data[i].title + '</cite>';
				ulHtml += '</a>';
				ulHtml += '<dl class="layui-nav-child">';
				ulHtml += getChildHtml(data[i].children);
				ulHtml += '</dl>';
			} else {
				var dataUrl = (data[i].href !== undefined && data[i].href !== '') ? 'data-url="' + data[i].href + '"' : '';
				ulHtml += '<a href="javascript:;" ' + dataUrl + '>';
				ulHtml += getIcon(data[i].icon);
				ulHtml += '<cite>' + data[i].title + '</cite>';
				ulHtml += '</a>';
			}
			ulHtml += '</li>';
		}
		ulHtml += '</ul>';
		console.log(ulHtml);
		return ulHtml;
	}
	function getChildHtml(data) {
		var ulHtml = "";
		for (var i = 0; i < data.length; i++) {
			if (data[i].children !== undefined && data[i].children.length > 0) {
				ulHtml += '<a href="javascript:;">';
				ulHtml += getIcon(data[i].icon);
				ulHtml += '<cite>' + data[i].title + '</cite>';
				ulHtml += '</a>';
				//ulHtml += '<dl class="layui-nav-child">';
				ulHtml += getChildHtml(data[i].children);
				//ulHtml += '</dl>';
			} else {
				ulHtml += '<dd title="' + data[i].title + '">';
				ulHtml += '<a href="javascript:;" data-url="' + data[i].href + '">';
				ulHtml += getIcon(data[i].icon);
				ulHtml += '<cite>' + data[i].title + '</cite>';
				ulHtml += '</a>';
				ulHtml += '</dd>';
			}
		}
		return ulHtml;
	}
	function getIcon(icon) {
		var ulHtml = "";
		if (icon !== undefined && icon !== ''&& icon!=null) {
			if (icon.indexOf('fa-') !== -1) {
				ulHtml = '<i class="fa ' + icon + '" aria-hidden="true" data-icon="' + icon + '"></i>';
			} else {
				ulHtml = '<i class="layui-icon" data-icon="' + icon + '">' + icon + '</i>';
			}
		}
		return ulHtml;
	}

	var navbar = new Navbar();

	exports('navbar', function(options) {
		return navbar.set(options);
	});
});