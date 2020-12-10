/** BTable.js By Beginner*/
layui.define(['element', 'common', 'paging', 'form'], function (exports) {
    "use strict";
    var $ = layui.jquery,
        layerTips = parent.layer === undefined ? layui.layer : parent.layer,
        layer = layui.layer,
        element = layui.element(),
        common = layui.common,
        paging = layui.paging(),
        form = layui.form();

    var bTable = function () {
		/**
		 *  默认配置 
		 */
        this.config = {
            elem: undefined, //容器
            //data: undefined, //数据源
            columns:[],
            url: undefined, //数据源地址
            type: 'GET', //读取方式
            even: false, //是否开启偶数行背景
            skin: undefined, //风格样式 ，可选参数 line/row/nob
            field: 'id',//主键属性名
            paged: false, //是否显示分页组件
            singleSelect: false, //是否只能选择一行
            checkbox: true //显示多选
        };
        this.v = '1.0.0';
    };
	/**
	 * 配置BTable
	 * @param {Object} options
	 */
    bTable.prototype.set = function (options) {
        var that = this;
        $.extend(true, that.config, options);
        return that;
    };
	/**
	 * 渲染table
	 */
    bTable.prototype.render = function () {
        var that = this;
        var _config = that.config;

        var columns = _config.columns;
        var th = '';
        for (var i = 0; i < columns.length; i++) {
            th += '<th>' + columns[i].fieldName + '</th>';
        }
        if (_config.checkbox && !_config.singleSelect) {
            th = '<th style="width:28px;"><input type="checkbox" lay-filter="allselector" lay-skin="primary" /></th>' + th;
        } else if (_config.checkbox) {
            th = '<th style="width:28px;">#</th>' + th;
        }
        var tpl = '<div class="btable">';
        if (_config.skin !== undefined && (_config.skin === 'line' || _config.skin === 'row' || _config.skin === 'nob') && _config.even) {
            tpl += '<table class="layui-table layui-form" lay-even lay-skin="' + _config.skin + '">';
        } else if (_config.skin !== undefined && (_config.skin === 'line' || _config.skin === 'row' || _config.skin === 'nob')) {
            tpl += '<table class="layui-table layui-form" lay-skin="' + _config.skin + '">';
        } else if (_config.even) {
            tpl += '<table class="layui-table layui-form" lay-even>';
        } else {
            tpl += '<table class="layui-table layui-form">';
        }
        tpl += '<thead><tr>' + th + '</tr></thead>';
        tpl += '<tbody class="btable-content"></tbody>';
        tpl += '</table>';
        if (_config.paged) {
            tpl += '<div data-type="paged" class="btable-paged"></div>';
        }
        tpl += '</div>';

        $(_config.elem).html(tpl);

        paging.init({
            url: _config.url, //地址
            elem: '.btable-content', //内容容器
            type: _config.type,
            tempType: 1,
            tempElem: getTpl({
                columns: _config.columns,
                checkbox: _config.checkbox,
                field: _config.field
            }), //模块容器
            paged: _config.paged,
            pageConfig: { //分页参数配置
                skip:true,
                elem: $(_config.elem).find('div[data-type=paged]'),//'#paged', //分页容器
                pageSize: _config.pageSize || 15 //分页大小
            },
            success: function () { //完成的回调
                //重新渲染复选框
                form.render('checkbox');
                form.on('checkbox(allselector)', function (data) {
                    var elem = data.elem;

                    $(_config.elem).find('tbody.btable-content').children('tr').each(function () {
                        var $that = $(this);
                        //全选或反选
                        $that.children('td').eq(0).children('input[type=checkbox]')[0].checked = elem.checked;
                        form.render('checkbox');
                    });
                });
                //绑定选择行事件
                $(_config.elem).find('tbody.btable-content').children('tr').each(function (e) {
                    //e.preventDefault();
                    //e.stopPropagation();

                    var $that = $(this);
                    $that.on('click', function () {
                        //只允许选择一行
                        if (_config.singleSelect) {
                            $that.siblings().each(function () {
                                $(this).children('td').eq(0).children('input[type=checkbox]')[0].checked = false
                            });
                            $that.children('td').eq(0).children('input[type=checkbox]')[0].checked = true;
                        } else {
                            //获取当前的状态
                            var currState = $that.children('td').eq(0).children('input[type=checkbox]')[0].checked;
                            $that.children('td').eq(0).children('input[type=checkbox]')[0].checked = !currState;

                            //当前已选择的总行数
                            var cbxCount = 0;
                            $that.parent('tbody').children('tr').each(function () {
                                var $that = $(this);
                                if ($that.children('td:first-child').children('input')[0].checked) {
                                    cbxCount++;
                                }
                            });
                            $(_config.elem).find('thead').children('tr').children('th:first-child').children('input[type=checkbox]')[0].checked =
                                $that.parent('tbody').children('tr').length === cbxCount;
                        }
                        form.render('checkbox');
                    });
                });
            }
        });
        return that;
    };
	/**
	 * 获取选择的行。
	 */
    bTable.prototype.getSelected = function (callback) {
        var that = this;
        var _config = that.config;
        if (!_config.singleSelect)
            return callback({});
        var $tbody = $(_config.elem).find('tbody.btable-content');
        $tbody.children('tr').each(function () {
            var $that = $(this);
            var $input = $that.children('td:first-child').children('input')
            if ($input[0].checked) {
                callback({
                    elem: $that,
                    id: $input.data('id')
                });
            }
        });
        return that;
    };
	/**
	 * 获取选择的所有行数据
	 */
    bTable.prototype.getSelections = function (callback) {
        var that = this;
        var _config = that.config;
        var $tbody = $(_config.elem).find('tbody.btable-content');
        var dom = [];
        var ids = [];
        var index = 0;
        $tbody.children('tr').each(function () {
            var $that = $(this);
            var $input = $that.children('td:first-child').children('input')
            if ($input[0].checked) {
                dom[index] = $that;
                ids[index] = $input.data('id');
                index++;
            }
        });
        return callback({
            elem: dom,
            ids: ids,
            count: dom.length
        });
    };

	/**
	 * 获取模板
	 * @param {Object} options
	 */
    function getTpl(options) {
        var columns = options.columns;
        var tpl = '{{# if(d.list.length>0 && d.list!=undefined){ }}';
        tpl += '{{# layui.each(d.list, function(index, item){ }}';
        var tds = '';
        for (var i = 0; i < columns.length; i++) {
            tds += '<td>{{ item.' + columns[i].field + ' }}</td>';
        }
        if (options.checkbox) {
            tds = '<td><input type="checkbox" data-id="{{ item.' + options.field + ' }}" lay-skin="primary" /></td>' + tds;
        } else {
            tds = '<td style="display:none;"><input type="hidden" data-id="{{ item.id }}" name="id" /></td>' + tds;
        }
        tpl += '<tr>' + tds + '</tr>'
        tpl += '{{# }); }}';
        tpl += '{{# }else{ }}';
        var colLength = options.checkbox && !options.singleSelect ? columns.length + 1 : columns.length;
        tpl += '<tr col="' + colLength + '">暂无数据.</tr>';
        tpl += '{{# } }}';
        return tpl;
    }

    var btable = new bTable();

    exports('btable', function (options) {
        return btable.set(options);
    });
});