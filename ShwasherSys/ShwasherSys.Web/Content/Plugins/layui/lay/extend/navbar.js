/** iwb_admin-v1.1.0 MIT License By http://www.iwbnet.com e-mail:yueyy@iwbnet.com */
;/**
 * Name:navbar.js
 * Author:Van
 * E-mail:yueyy@iwbnet.com
 * Website:http://www.iwbnet.com
 * LICENSE:MIT
 */
layui.define(['layer', 'laytpl', 'element'], function (exports) {
	var $ = layui.jquery,
        layer = layui.layer,
        //modName = 'navbar',
        //win = $(window),
        doc = $(document),
        //laytpl = layui.laytpl,
        element = layui.element;

	var navbar = {
		v: '1.0.4',
		config: {
			data: undefined, //静态数据
			remote: {
				url: undefined, //接口地址
				type: 'post', //请求方式
				jsonp: false //跨域
			},
			cached: false, //是否缓存
			elem: undefined, //容器
			filter: 'iwbNavbar' //过滤器名称
		},
		set: function (options) {
			var that = this;
			that.config.data = undefined;
			$.extend(true, that.config, options);
			return that;
		},
		/**
         * 是否已设置了elem
         */
		hasElem: function () {
			var that = this,
                config = that.config;
			if (config.elem === undefined && doc.find('ul[iwb-navbar]').length === 0 && $(config.elem)) {
				layui.hint().error('Navbar error:请配置Navbar容器.');
				return false;
			}
			return true;
		},
		/**
         * 获取容器的jq对象
         */
		getElem: function () {
			var config = this.config;
			return (config.elem !== undefined && $(config.elem).length > 0) ? $(config.elem) : doc.find('ul[iwb-navbar]');
		},
		/**
         * 绑定特定a标签的点击事件
         */
		bind: function (callback, params) {
			var that = this,
                config = that.config;
			var defaults = {
				target: undefined,
				showTips: true,
				config:config
			};
			$.extend(true, defaults, params);
			var target = defaults.target === undefined ? doc : $(defaults.target);
			// if (!that.hasElem())
			//     return that;
			// var _elem = that.getElem();
			target.find('a[iwb-target-tip]')
				.each(function() {
					var $that = $(this);
					var tipsId = undefined;

					if (defaults.showTips) {
						$that.hover(function() {
								var isSided = $(".iwb-side").hasClass("iwb-sided");
								if (isSided)
									tipsId = layer.tips($(this).children('cite').text(), this);
							},
							function() {
								if (tipsId)
									layer.close(tipsId);
							});
					}
				});
			target.find('a[iwb-target]').each(function () {
				var $that = $(this);
				var tipsId = undefined;
				
				if (defaults.showTips) {
					$that.hover(function () {
						var isSided = $(".iwb-side").hasClass("iwb-sided");
						if (isSided)
							tipsId = layer.tips($(this).children('cite').text(), this);
					}, function () {
						if (tipsId)
							layer.close(tipsId);
					});
				}
				$that.off('click').on('click', function () {
					var options = $that.data('options');
					var data=[];
					if (options !== undefined) {
						try {
							data = new Function('return ' + options)();
						} catch (e) {
							layui.hint().error('Navbar 组件a[data-options]配置项存在语法错误：' + options);
						}
					} else {
						data = {
							icon: $that.data('icon'),
							id: $that.data('id'),
							title: $that.data('title'),
							url: $that.data('url')
						};
					}
					typeof callback === 'function' && callback(data);
				});
			});
			$('#iwb-nav-fold-top').off('click').on('click', function () {
				toggleSideNav(doc);
			});
			$('#iwb-nav-fold-side').off('click').on('click', function () {
				toggleSideNav(doc);
			});
			return that;
		},
		/**
         * 渲染navbar
         */
		render: function (callback) {
			var that = this,
                config = that.config, //配置
                remote = config.remote, //远程参数配置

                //tpl = [
                //    '{{# layui.each(d,function(index, item){ }}',
                //    '{{# if(item.spread){ }}',
                //    '<li class="layui-nav-item layui-nav-itemed">',
                //    '{{# }else{ }}',
                //    '<li class="layui-nav-item">',
                //    '{{# } }}',
                //    '{{# var hasChildren = item.children!==undefined && item.children.length>0; }}',
                //    '{{# if(hasChildren){ }}',
                //    '<a href="javascript:;">',
                //    '{{# if (item.icon.indexOf("fa-") !== -1) { }}',
                //    '<i class="fa {{item.icon}}" aria-hidden="true"></i>',
                //    '{{# } else { }}',
                //    '<i class="layui-icon">{{item.icon}}</i>',
                //    '{{# } }}',
                //    '<span> {{item.title}}</span>',
                //    '</a>',
                //    '{{# var children = item.children; }}',
                //    '<dl class="layui-nav-child">',
                //    '{{# layui.each(children,function(childIndex, child){ }}',
                //    '<dd>',
                //    '<a href="javascript:;" iwb-target data-options="{url:\'{{child.url}}\',icon:\'{{child.icon}}\',title:\'{{child.title}}\',id:\'{{child.id}}\'}">',
                //    '{{# if (child.icon.indexOf("fa-") !== -1) { }}',
                //    '<i class="fa {{child.icon}}" aria-hidden="true"></i>',
                //    '{{# } else { }}',
                //    '<i class="layui-icon">{{child.icon}}</i>',
                //    '{{# } }}',
                //    '<span> {{child.title}}</span>',
                //    '</a>',
                //    '</dd>',
                //    '{{# }); }}',
                //    '</dl>',
                //    '{{# }else{ }}',
                //    '<a href="javascript:;" iwb-target data-options="{url:\'{{item.url}}\',icon:\'{{item.icon}}\',title:\'{{item.title}}\',id:\'{{item.id}}\'}">',
                //    '{{# if (item.icon.indexOf("fa-") !== -1) { }}',
                //    '<i class="fa {{item.icon}}" aria-hidden="true"></i>',
                //    '{{# } else { }}',
                //    '<i class="layui-icon">{{item.icon}}</i>',
                //    '{{# } }}',
                //    '<span> {{item.title}}</span>',
                //    '</a>',
                //    '{{# } }}',
                //    '</li>',
                //    '{{# }); }}'
                //], //模板
                data1 = [];
			var navbarLoadIndex = layer.load(2);
			if (!that.hasElem())
				return that;
			var elem = that.getElem();
			//本地数据优先
			if (config.data !== undefined && config.data.length > 0) {
				data1 = config.data;
			} else {
				var dataType = remote.jsonp ? 'jsonp' : 'json';
				var options = {
					url: remote.url,
					type: remote.type,
					async: false,
					dataType:dataType,
					error: function (xhr, status, thrown) {
						layui.hint().error('Navbar error:AJAX请求出错.' + thrown);
						navbarLoadIndex && layer.close(navbarLoadIndex);
					},
					success: function (res) {
						data1 = res;
					}
				};
				$.extend(true, options, remote.jsonp ? {
					dataType: 'jsonp',
					jsonp: 'callback',
					jsonpCallback: 'jsonpCallback'
				} : {
					dataType: 'json'
				});
				$.support.cors = true;
				$.ajax(options);
			}
			var tIndex = setInterval(function () {
				if (data1 != null && data1.length > 0) {
					clearInterval(tIndex);
					var html = getHtml(data1);
					elem.html(html);
					element.init();
					childClickEvent();
					//绑定a标签的点击事件
					that.bind(function(data) {
						typeof callback === 'function' && callback(data);
					});

					//渲染模板
					//laytpl(tpl.join('')).render(_data, function (html) {
					//	_elem.html(html);
					//	element.init();
					//	//绑定a标签的点击事件
					//	that.bind(function (data) {
					//		typeof callback === 'function' && callback(data);
					//	});
					//	//关闭等待层
					//	navbarLoadIndex && layer.close(navbarLoadIndex);
					//});
				} else {
					layui.hint().error('Navbar error:AJAX 未取到数据');
					clearInterval(tIndex);
				}
				//关闭等待层
				navbarLoadIndex && layer.close(navbarLoadIndex);
			}, 50);
			return that;
		}
	};
	exports('navbar', navbar);
});

function childClickEvent(select) {
	select = select || ".menu_ul li a[iwb-hasChild]";
	var aLink = document.querySelectorAll(select);
	for (var i = 0, len = aLink.length; i < len; i++) {
		aLink[i].addEventListener('click', function () {
			//console.log(this.parentNode.children[1]);
			if (this.parentNode.children[1].style.display === 'block') {
				this.parentNode.children[1].style.display = 'none';
				//this.parentNode.children[0].children[2].children[0].className = 'fa fa-angle-down';
				this.parentNode.className = 'layui-nav-item';
			} else {
				this.parentNode.children[1].style.display = 'block';
				this.parentNode.className = 'layui-nav-item layui-nav-itemed';
			}
		}, false);
	}
}

function getHtml(data) {
	var ulHtml = '<ul class="layui-nav layui-nav-tree" lay-filter="iwbNavbar" iwb-navbar>';
	ulHtml += '<li class="layui-nav-item"><a id="iwb-nav-fold-side" title="切换左侧菜单" style="color: #fef;text-align:center;"><i class="iwb-nav-toggle-side layui-icon" aria-hidden="true">&#xe65f;</i></a></li>';
	for (var i = 0; i < data.length; i++) {
		var child = data[i];
		//if (child.spread) {
		//	ulHtml += '<li class="layui-nav-item layui-nav-itemed">';
		//} else {
		//	ulHtml += '<li class="layui-nav-item">';
		//}
		ulHtml += '<li class="layui-nav-item">';
		if (child.children !== undefined && child.children.length > 0) {
			//console.log(i + "---child");
			ulHtml += '<a  href="javascript:;" iwb-hasChild iwb-target-tip>';
			ulHtml += getIcon(child.icon);
			ulHtml += '<cite>' + child.title + '</cite>';
			ulHtml += '<span class="layui-nav-more"></span>';
			ulHtml += '</a>';
			ulHtml += '<ul class="menu_ul layui-nav-child">';
			ulHtml += getChildHtml(child.children);
			ulHtml += '</ul>';
		} else {
			//ulHtml += '<a href="javascript:;"  iwb-target data-options="{url:\'' +
			//	child.href +
			//	'\',icon:\'' +
			//	child.icon +
			//	'\',title:\'' +
			//	child.title +
			//	'\',id:\'' +
			//	child.id +
			//	'\'}">';
			ulHtml += '<a href="javascript:;" data-url="' +
				child.href +
				'" data-icon="' +
				child.icon +
				'" data-title="' +
				child.title +
				'" data-id="' +
				child.id +
				'" iwb-target iwb-target-tip>';
			ulHtml += getIcon(child.icon);
			ulHtml += '<cite>' + child.title + '</cite>';
			ulHtml += '</a>';
		}
		ulHtml += '</li>';
	}
	ulHtml += '</ul>';
	//console.log(ulHtml);
	return ulHtml;
}
function getChildHtml(data) {
	var ulHtml = "";
	for (var i = 0; i < data.length; i++) {
		var child = data[i];
		if (child.children !== undefined && child.children.length > 0) {
			//ulHtml += '<dd title="' + child.title + '">';
			ulHtml += '<li>';
			ulHtml += '<a class="table" href="javascript:;" iwb-hasChild iwb-target-tip>';
			ulHtml += getIcon(child.icon);
			ulHtml += '<cite>' + child.title + '</cite>';
			ulHtml += '<span class="layui-nav-more"></span>';
			ulHtml += '</a>';
			ulHtml += '<ul class="layui-nav-child" style="display: none;">';
			ulHtml += getChildHtml(child.children);
			ulHtml += '</ul>';
			//ulHtml += '</dd>';
			ulHtml += '</li>';
		} else {
			//ulHtml += '<dd title="' + child.title + '">';
			ulHtml += '<li class="layui-nav-item">';
			//ulHtml += '<a class="table"  href="javascript:;"  iwb-target data-options="{url:\'' +
			//	child.href +
			//	'\',icon:\'' +
			//	child.icon +
			//	'\',title:\'' +
			//	child.title +
			//	'\',id:\'' +
			//	child.id +
			//	'\'}">';
			ulHtml += '<a href="javascript:;" data-url="' +
				child.href +
				'" data-icon="' +
				child.icon +
				'" data-title="' +
				child.title +
				'" data-id="' +
				child.id +
				'"  iwb-target iwb-target-tip >';
			ulHtml += getIcon(child.icon);
			ulHtml += '<cite>' + child.title + '</cite>';
			ulHtml += '</a>';
			//ulHtml += '</dd>';
			ulHtml += '</li>';
		}
	}
	return ulHtml;
}
function getIcon(icon) {
	var ulHtml = "";
	if (icon !== undefined && icon !== '' && icon != null) {
		if (icon.indexOf('fa-') !== -1) {
			ulHtml = '<i class="fa ' + icon + '" aria-hidden="true" data-icon="' + icon + '"></i>';
		} else {
			ulHtml = '<i class="layui-icon" data-icon="' + icon + '">' + icon + '</i>';
		}
	}
	return ulHtml;
}

function toggleSideNav(doc) {
	var icon1 = "iwb-nav-toggle-top layui-icon layui-icon-shrink-right",
		icon2 = "iwb-nav-toggle-top layui-icon layui-icon-spread-left",
		icon3 = "&#xe65f;",
		icon4 = "&#xe671;";
	var display = $('.iwb-side-fold span').css('display');
	var menu1 = doc.find('i.iwb-nav-toggle-top');
	var menu2 = doc.find('i.iwb-nav-toggle-side');
	if (display === 'inline' || display === 'inline-block') {
		$('.iwb-side-fold span').css('display', 'none');
	}
	if (display === 'none') {
		$('.iwb-side-fold span').css('display', 'inline-block');
	}
	var side = doc.find('div.iwb-side');
	if (side.hasClass('iwb-sided')) {
		side.removeClass('iwb-sided');
		doc.find('div.layui-body').removeClass('iwb-body-folded');
		doc.find('div.layui-footer').removeClass('iwb-footer-folded');
		menu1.attr('class', icon1);
		menu2.html(icon3);
		$('#logo span').css('display', 'block');
		//$('#logo').css('width', '200px').removeClass("iwb-header-sided");
		$('#logo').removeClass("iwb-header-sided");
		$('.tplay-left-icon').css('display', 'none');
		//$('.layui-layout-left').css('left', '220px');
	} else {
		side.addClass('iwb-sided');
		doc.find('div.layui-body').addClass('iwb-body-folded');
		doc.find('div.layui-footer').addClass('iwb-footer-folded');
		menu1.attr('class', icon2);
		menu2.html(icon4);
		$('#logo span').css('display', 'none');
		//$('#logo').css('width', '50px').addClass("iwb-header-sided");
		$('#logo').addClass("iwb-header-sided");
		$('.tplay-left-icon').css('display', 'block');
		//$('.layui-layout-left').css('left', '50px');
	}
}