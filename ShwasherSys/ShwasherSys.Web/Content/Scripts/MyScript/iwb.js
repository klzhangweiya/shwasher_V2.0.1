if (typeof jQuery === "undefined") {
    throw new Error("jQuery plugins need to be before this file");
}
var log = true, abp = abp || {};
/*JQuery扩展*/
$.extend({
    //表单赋值
    formDeserialize: function ($form, data) {
        //isSelect2 = isSelect2 === undefined;
        if (!data) {
            return;
        }
        var $input = $form.find("input");
        var $textarea = $form.find("textarea");
        var $select = $form.find("select");
        var $checkbox = $form.find("input[type='checkbox']");
        var $radio = $form.find("input[type='radio']");
        $.merge($input, $textarea);
        $input.each(function () {
            var $input = $form.find($(this));
            var name = $input.attr("name");
            if (data[name] !== "") {
                $input.val(data[name]);
            }
        });
        $select.each(function () {
            var $select = $form.find($(this));
            var name = $select.attr("name");
            if (typeof data[name] === "boolean") {
                data[name] = data[name] + "";
            }
            if (data[name] !== "") {
                $select.val(data[name]);

                //if (isSelect2) {
                //    $select.val(data[name]).select2();
                //} else {
                //}
            }
        });
        $checkbox.each(function () {
            var input = $form.find($(this));
            //var name = input.attr("name").replace(/(\w)/, function (v) { return v.toUpperCase() });
            var name = input.attr("name");
            if (data[name] !== "") {
                //console.log(array[name]);
                //console.log("---");
                input.val(data[name] === "True" ||
                    input.val(data[name]) === "1" ||
                    input.val(data[name]) === "true");
            }
        });
        $radio.each(function () {
            var input = $form.find($(this));
            //var name = input.attr("name").replace(/(\w)/, function (v) { return v.toUpperCase() });
            var name = input.attr("name");
            if (data[name] !== "") {
                //console.log(array[name]);
                $("input[name='" + name + "'][value='" + data[name] + "']").prop("checked", true);
                $("input[name='" + name + "'][value!='" + data[name] + "']").prop("checked", false);
            }
        });
    },
    //将form表单元素的值序列化成对象
    formSerialize: function ($form) {
        var disableEle = $form.find("[disabled]");
        disableEle.each(function (i, e) {
            $(e).prop("disabled", false);
        });
        var data = {};
        $.each($form.serializeArray(),
            function () {
                if (data[this['name']]) {
                    data[this['name']] = data[this['name']] + "," + this['value'];
                } else {
                    data[this['name']] = this['value'];
                }
            });
        disableEle.each(function (i, e) {
            $(e).prop("disabled", true);
        });
        return data;
    },
    //表单验证
    formValidate: function ($form, opt) {
        var defaults = {
            noValid: false,
            form: "form",
            modal: "modal",
            errorPlacement: function (error, element) {
                element.parent().before(error);
                element.focus();
            },
            rules: {}
        };
        opt = opt || {};
        var options = $.extend({}, defaults, opt);
        var $modal = options.modal
            ? typeof (options.modal) === 'string'
                ? $('#' + options.modal)
                : $(options.modal)
            : null;
        $form = $form ? $form : options.form ? $(options.form) : $modal.find('form');
        $form.validate({
            errorPlacement: options.errorPlacement,
            rules: options.rules
        }).settings.ignore = ":disabled";
        if (!options.noValid) {
            return $form.valid();
        }
        return options.noValid;
    },
    formatterDate: function (fmt, date, isFix) {
        date = date || new Date();
        isFix = isFix === undefined ? true : isFix;
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        fmt = fmt.replace("yyyy", year);
        fmt = fmt.replace("yy", year % 100);
        fmt = fmt.replace("MM", fix(month));
        fmt = fmt.replace("dd", fix(date.getDate()));
        fmt = fmt.replace("HH", fix(date.getHours()));
        fmt = fmt.replace("mm", fix(date.getMinutes()));
        fmt = fmt.replace("ss", fix(date.getSeconds()));
        return fmt;
        function fix(n) {
            return isFix ? (n < 10 ? "0" + n : n) : n;
        }
    },
    blinkTitle:{ 
        start: function (msg) {
            msg = msg || abp.localization.iwbZero('NewNotificationRemind');
            this.title = document.title;
            this.messages = [msg];
            if (!this.action) {
                try {
                    this.element = document.getElementsByTagName('title')[0];
                    this.element.innerHTML = this.title;
                    this.action = function (ttl) {
                        this.element.innerHTML = ttl;
                    };
                } catch (e) {
                    this.action = function(ttl) {
                        document.title = ttl;
                    };
                    delete this.element;
                }
                this.toggleTitle = function () {
                    this.index = this.index === 0 ? 1 : 0;
                    this.action('【' + this.messages[this.index] + '】'+this.title);
                };
            }
            var n = msg.length;
            var s = '';
            if (this.element) {
                var num = msg.match(/\w/g);
                if (num) {
                    var n2 = num.length;
                    n -= n2;
                    while (n2 > 0) {
                        s += " ";
                        n2--;
                    }
                }
            }
            while (n > 0) {
                s += '　';
                n--;
            }
            this.messages.push(s);
            this.index = 0;
            //this.title = this.title.replace("【" + msg + "】", "").replace("【" + s + "】", "");
            var that = this;
            this.timer = setInterval(function () {
                that.toggleTitle();
            }, 500);
        },
        stop: function () {
            if (this.timer) {
                clearInterval(this.timer);
                var t = this.title ? this.title : document.title;
                this.action(t);
                delete this.timer;
                delete this.messages;
            }
        }
    } ,
    metPageCss: function (url, id) {
        id = id || 'dy-css';
        if ($('#' + id).length > 0) {
            return;
        }
        var link = document.createElement('link');
        link.type = 'text/css';
        link.rel = 'stylesheet';
        link.id = id;
        link.href = url + '?v=' + Math.floor(Math.random() * 100000);
        var flag = document.getElementById('flag');
        var head = document.getElementsByTagName('head')[0];
        if (flag) {
            head.insertBefore(link, flag.nextSibling);
        } else {
            head.appendChild(link);
        }
    },
    metPageJs: function (src, id) {
        id = id || 'dy-js';
        if ($('#' + id).length > 0) {
            return;
        }
        var script = document.createElement('script');
        script.id = id;
        script.type = 'text/javascript';
        script.charset = 'UTF-8';
        script.src = src + '?v=' + Math.floor(Math.random() * 100000);
        $('head').append(script);
    },

    loadScript: function(url, loadCallback, failCallback) {
        /* UrlStates enum */
        var urlStates = {
            LOADING: 'LOADING',
            LOADED: 'LOADED',
            FAILED: 'FAILED'
        };
        /* UrlInfo class */
        function UrlInfo() {
            this.state = urlStates.LOADING;
            this.loadCallbacks = [];
            this.failCallbacks = [];
        }
        UrlInfo.prototype.succeed = function () {
            this.state = urlStates.LOADED;
            for (var i = 0; i < this.loadCallbacks.length; i++) {
                this.loadCallbacks[i]();
            }
        };
        UrlInfo.prototype.failed = function () {
            this.state = urlStates.FAILED;
            for (var i = 0; i < this.failCallbacks.length; i++) {
                this.failCallbacks[i]();
            }
        };
        UrlInfo.prototype.handleCallbacks = function (loadCallback, failCallback) {
            switch (this.state) {
                case urlStates.LOADED:
                    loadCallback && loadCallback();
                    break;
                case urlStates.FAILED:
                    failCallback && failCallback();
                    break;
                case urlStates.LOADING:
                    this.addCallbacks(loadCallback, failCallback);
                    break;
            }
        };
        UrlInfo.prototype.addCallbacks = function (loadCallback, failCallback) {
            loadCallback && this.loadCallbacks.push(loadCallback);
            failCallback && this.failCallbacks.push(failCallback);
        };

        var urlInfos = {};

        var loadScript = function (url, loadCallback, failCallback) {
            var urlInfo = urlInfos[url];
            if (urlInfo) {
                urlInfo.handleCallbacks(loadCallback, failCallback);
                return;
            }
            urlInfos[url] = urlInfo = new UrlInfo();
            urlInfo.addCallbacks(loadCallback, failCallback);

            $.getScript(url).done(function (script, textStatus) {
                urlInfo.succeed();
            }).fail(function (jqxhr, settings, exception) {
                urlInfo.failed();
            });
        };
        loadScript(url, loadCallback, failCallback);
    }
});

/*AJAX*/
$.extend({
    //ajax
    iwbAjax: function (url, opt) {
        this.defaults = {
            async: true,
            type: "Post",
            contentType: 'application/json; charset=UTF-8',
            //contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: null,
            log: window.log,
            dataType: "json",
            isAlert: true,
            isValidate: true,
            isRefresh: true,//默认刷新表格
            success: null,
            error: null,
            table: undefined,
            modal: undefined,
            form: undefined,
            errorPlacement: function (error, element) {
                element.parent().before(error);
                element.focus();
            },
            rules: {},
            blockUI: true
        };
        if (!opt) {
            opt = url;
            url = opt.url;
        }
        
        var options = $.extend({}, this.defaults, opt);
        var $table = !options.table ? undefined : typeof options.table === 'string' ? $('#' + options.table) : $(options.table);
        var $modal = !options.modal ? undefined : typeof options.modal === 'string' ? $('#' + options.modal) : $(options.modal);
        var $form = !options.form ? ($modal? $modal.find("form"):undefined ): typeof options.form === 'string' ? $('#' + options.form) : $(options.form);

        var isValidated = true;
        if (options.isValidate && $form) {
            isValidated = $.formValidate($form);
        }
        if (isValidated) {
            options.data = options.data ? (typeof options.data === 'function' ? options.data() : options.data) : $form ? $.formSerialize($form) : undefined;
            var guid = Math.floor(Math.random() * 10000);
            var log = options.log;
            if (log) {
                console.log('[' + guid + ']url:' + url, 'data:', options.data);
            }
            var success = options.success && typeof options.success === 'function'
                ? function (res) {
                    if (log) { console.log('[' + guid + ']', res); }
                    options.success(res);
                    if (options.isRefresh && $table) {
                        $table.iwbTable('refresh', true);
                    }
                    if ($modal) {
                        $modal.iwbModal('hide');
                    }
                }
                : function (res) {
                    if (log) { console.log('[' + guid + ']', res); }
                    if (options.isRefresh && $table) {
                        $table.iwbTable('refresh', true);
                    }
                    if ($modal) {
                        $modal.iwbModal('hide');
                    }
                };

            var ajaxSuccess = options.isAlert ? function (res) { abp.message.success(abp.localization.iwbZero('OpSuccess')).done(success(res)); } : success;
            var data = typeof (options.data) === 'string' ? options.data : JSON.stringify(options.data);
            
            var ajaxOption = {
                url: url,
                async: options.async,
                type: options.type,
                contentType: options.contentType,
                data: data,
                dataType: options.dataType,
                success: ajaxSuccess,
                error: options.error,
                abpHandleError: options.isAlert,
                blockUI: options.blockUI
            };
            abp.ajax(ajaxOption);
        }
    }
});
$.extend({
    iwbAjax1: function (opt) {
        opt = $.extend({}, { isValidate: false }, opt);
        $.iwbAjax(opt);
    },
    iwbAjax2: function (opt) {
        opt = $.extend({}, { isAlert: false }, opt);
        $.iwbAjax(opt);
    },
    iwbAjax3: function (opt) {
        opt = $.extend({}, { isValidate: false, isAlert: false ,blockUI:false}, opt);
        $.iwbAjax(opt);
    },
    iwbAjax4: function (opt) {
        opt = $.extend({}, { isValidate: false, isAlert: false, isRefresh: false ,blockUI:false}, opt);
        $.iwbAjax(opt);
    },
    iwbGet: function (opt) {
        opt = $.extend({}, { type: 'get', modal: null, form: null }, opt);
        $.iwbAjax4(opt);
    }

});

/*Table*/
(function ($, window, document, undefined) {
    //构造函数
    'use strict';
    var abp = window.abp || {},
        searchList = [],
        isSearching,
        modalDefaults = {
            modal: 'modal',
            modaltitle: '',
            data: null,
            form: null,
            errorPlacement: function (error, element) {
                if (element.is('select')) {
                    if (!element.next().next().hasClass('error')) {
                        element.next().after(error);
                        element.focus();
                    }
                } else {
                    element.after(error);
                    element.focus();
                }
            },
            rules: {},
            readonly: '',
            disabled: '',
            select2: true,
            select2tree: false,
            url: '',
            savebtn: null,
            save: null,
            success: null,
            shownBefore: null,
            shownAfter: null,
            draggable: false,
            searchValidate: true
        };
    //modalOptions = {};
    var getSearchList = function ($that) {
            var o = $that.options;
            var $searchForm = (typeof (o.searchForm) === 'string' ? $('#' + o.searchForm) : $(o.searchForm));
            searchList = [];
            if (o.searchList && o.searchList.length > 0) {
                o.searchList.forEach(function (v) {
                    if (v.KeyWords) {
                        searchList.push(v);
                    }
                });
            }
            if (!o.onlySearchList) {
                $searchForm.find('.KeyWords').each(function () {
                    var $this = $(this), keyWords = $this.val();
                    if (keyWords) {
                        if ($this.hasClass('dataRange')) {
                            try {
                                var dates = keyWords.split(' - ');
                                searchList.push({
                                    KeyWords: dates[0],
                                    KeyField: $this.data('field'),
                                    FieldType: $this.data('ftype'),
                                    ExpType: 4
                                });
                                if (dates.length===2) {
                                    var dateArr = dates[1].split(' ')[0].split('-');
                                    var date = new Date(dateArr[0], dateArr[1] - 1, dateArr[2], 0, 0, 0);
                                    var newDate = new Date(date.getTime() + 24 * 60 * 60 * 1000 - 1);
                                    keyWords = $.formatterDate('yyyy-MM-dd HH:mm:ss', newDate);
                                    searchList.push({ KeyWords: keyWords, KeyField: $this.data('field'), FieldType: $this.data('ftype'), ExpType: 5 });
                                }
                                
                            } catch (e) {
                                console.log(e);
                            }
                        } else {
                            searchList.push({ KeyWords: keyWords, KeyField: $this.data('field'), FieldType: $this.data('ftype'), ExpType: $this.data('etype') });

                        }
                    }
                });

            }

        },
        queryParams = function (params, $that) {
            var o = $that.options;
            getSearchList($that);
            var $searchForm = (typeof (o.searchForm) === 'string' ? $('#' + o.searchForm) : $(o.searchForm));
            var $keyWord = $searchForm.find('#keyWords');
            if (o.onlySearchList) {
                return {
                    MaxResultCount: params.limit,
                    SkipCount: params.offset,
                    //sort: params.sort, //排序列名  
                    //sortOrder: params.order, //排位命令（desc，asc） 
                    sorting: params.sort?params.sort+' '+params.order:'',
                    SearchList: searchList
                };
            }
            return {
                MaxResultCount: params.limit,
                SkipCount: params.offset,
                sorting: params.sort?params.sort+' '+params.order:'',
                keyField: $keyWord.data('field'),
                fieldType: $keyWord.data('ftype'),
                expType: $keyWord.data('etype'),
                keyWords: $keyWord.val(),
                SearchList: searchList
            };
        },
        responseHandler = function (res) {
            if (res.success) {
                var data = JSON.parse('{"total":' +
                    res.result.totalCount +
                    ',"rows":' +
                    JSON.stringify(res.result.items) +
                    '}');
                console.log(data);
                return data;
            } else {
                console.log('Table load failed');
                if (res.error) {
                    if (res.error.details) {
                        return abp.message.error(res.error.details, res.error.message);
                    } else {
                        if (res.error.message && res.error.message.indexOf('登陆超时') >= 0) {
                            return abp.message.error(res.error.message).done(function () {
                                window.top.location.reload();
                            });
                        } else {
                            return abp.message.error(res.error.message || abp.ajax.defaultError.message);
                        }
                    }
                }
            }
            return JSON.parse('{"total":0,"rows":[]}');
        },
        responseHandlerNoPage = function (res) {
            if (res.success) {
                var data = res.result;
                console.log(data);
                return data;
            } else {
                console.log('Table load failed');
                if (res.error) {
                    if (res.error.details) {
                        return abp.message.error(res.error.details, res.error.message);
                    } else {
                        if (res.error.message && res.error.message.indexOf('登陆超时') >= 0) {
                            return abp.message.error(res.error.message).done(function () {
                                window.top.location.reload();
                            });
                        } else {
                            return abp.message.error(res.error.message || abp.ajax.defaultError.message);
                        }
                    }
                }
            }
            return JSON.parse('[]');
        },
        onAll = function (eName, eData, $that) {
            isSearching = false;
            var o = $that.options;
            var $table = (typeof (o.table) === 'string' ? $('#' + o.table) : $(o.table));
            $table.closest('.table-box').find('.tableTool .menu-btn[data-type^=\'btn\']')
                .prop('disabled', $table.bootstrapTable('getSelections').length !== 1);
        },
        onLoadSuccess = function (data, $that) {
            var o = $that.options;
            var $table = (typeof (o.table) === 'string' ? $('#' + o.table) : $(o.table));
            // $table.find('.bs-checkbox').find('input').addClass('filled-in').after('<label></label');
            
        },
        onPostBody = function (data, $that) {
            var o = $that.options;
            $(document);
            var $table = (typeof (o.table) === 'string' ? $('#' + o.table) : $(o.table));
         /*   $table.find("td.bs-checkbox").each(function () {
                var input = $(this).find('input');
                var $label = $('<span class="iwb-checkbox"></span>');
                $label.append(input);
                $label.append('<span></span>');
                $(this).html($label);
            });*/
            $table.find(".bs-checkbox").find("input").addClass("filled-in").after("<label></label");
		    $table.find("thead th.bs-checkbox").off("click.checkOnTable").on(
		        "click.checkOnTable",
		        function () {
		            $(this).find("input").click();
	        });
            isSearching = false;
            var $tableTool = o.tableTool
                ? typeof (o.tableTool) === 'string'
                    ? $table.closest('.table-box').find('#' + o.tableTool)
                    : $table.closest('.table-box').find(o.tableTool)
                : $table.closest('.table-box').find('.btn-toolbar');

            $tableTool.find('.menu-btn').off('click.menubtn').on('click.menubtn', function () {
                var funs = $that.getFuns();
                var type = $(this).data('type');
                var index = type.lastIndexOf('_');
                type = index > -1 ? type.substr(index + 1) : type;
                var url = $(this).data('url') || "";
                funs[type] ? funs[type].call(this, url) : funs["none"].call(this);
            });
            abp.ui.clearBusy();
            $table.find('tr th').each(function () { $(this).css('text-align', 'center') });
            $table.find('tr td.iwb-tips').each(function () {
               var text= $(this).text();
                $(this).tooltip({ 'title': text,'placement': 'bottom', 'container': 'body','delay':800 });
            });
        },
        
        getModal = function (url, opt, title, type, $that) {
            var o = $that.options;
            var modalOptions = $.extend({}, modalDefaults, opt);
            var $table = typeof o.table === 'string' ? $('#' + o.table) : $(o.table);
            var $tableTool = o.tableTool
                ? typeof o.tableTool === 'string'
                    ? $table.closest('.table-box').find('#' + o.tableTool)
                    : $table.closest('.table-box').find(o.tableTool)
                : $table.closest('.table-box').find('.btn-toolbar');
            modalOptions.modaltitle = title;
            modalOptions.draggable = $that.options.modalDrag;
            if (!opt) {
                opt = !url || typeof url === "string" ? {} : url;
            }
            if (opt.hasOwnProperty("url")) {
                modalOptions.url = opt.url;
            } else if (typeof url === "string") {
                modalOptions.url = url;
            } else {
                modalOptions.url = $tableTool.find('.menu-btn[data-type=' + type + ']').data('url');
                modalOptions.url = modalOptions.url
                    ? modalOptions.url
                    : $tableTool.find('.menu-btn[data-type=_' + type + ']').data('url');
            }
            modalOptions.modal = typeof $that.options.modal === 'string' ? $("#" + $that.options.modal) : $($that.options.modal);
            modalOptions.table = $table;
            //modalOptions.type = type.toLowerCase().indexOf('btnupdate') === 0 ? 'put' : 'post';
            modalOptions.type = 'post';
            return modalOptions;

        };
    var Table = function (ele, opt) {
        var $that = this;
        this.defaults = {
            url: undefined,
            table: undefined,
            tableTool: '',
            resetView: true,
            height: undefined,
            searchForm: 'search-form',
            searchList: [],
            onlySearchList: false,
            searchValidate: true,
            queryParams: function (p) { return queryParams(p, $that); },
            onAll: function (e, d) { onAll(e, d, $that); },
            onLoadSuccess: function (d) { onLoadSuccess(d, $that); },
            onPostBody: function (d) { onPostBody(d, $that); },
            isPage: true,
            lang: 'zh-CN',
            funs: undefined,
            modal: 'modal',
            form: 'form',
            modalDrag: true
        };
        $that.options = $.extend({}, this.defaults, opt || {});
        if (!$that.options.onAll) {
            $that.options.onAll = function (e, d) {
                onAll(e, d, $that);
            };
        }
        this.$ele = ele,
            this.loadTable();
        return this;
    };

    Table.prototype.loadTable = function () {
        var $that = this;
        $that.options.responseHandler = $that.options.responseHandler
            ? $that.options.responseHandler
            : $that.options.isPage
            ? responseHandler
            : responseHandlerNoPage;
        $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales[$that.options.lang]);
        var o = $that.options;
        //searchList = o.searchList;
        getSearchList($that);
        var $table = (typeof (o.table) === 'string' ? $('#' + o.table) : $(o.table));
        $table.bootstrapTable($that.options);
        if (o.resetView) {
            var h = o.height ? o.height : $(window).height() - 150;
            $that.resetView(h);
            $(window).resize(function () {
                $that.resetView();
            });
        }
        return this;
    };
    Table.prototype.refresh = function (isForce) {
        var $that = this;
        abp.ui.setBusy();
        setTimeout(function () { abp.ui.clearBusy(); isSearching = false; }, 5 * 1000);
        if (isSearching) {
            return;
        }
        isSearching = true;
        //getSearchList($that);
        //if (!isForce && searchList.length <= 0) {
        //    console.log("Search-Multi-None");
        //    return;
        //}
        var o = $that.options;
        var $searchForm = (typeof (o.searchForm) === 'string' ? $('#' + o.searchForm) : $(o.searchForm));
        var isValidated = true;
        if (o.searchValidate && $searchForm && $searchForm.length > 0) {
            isValidated = $.formValidate($searchForm);
        }
        if (isValidated) {
            var $table = (typeof (o.table) === 'string' ? $('#' + o.table) : $(o.table));
            $table.bootstrapTable('refresh', { silent: true });
            console.log("Search");
        } else {
            console.log("Search_Validate_Faild");
        }

    };
    Table.prototype.resetView = function (height) {
        var $that = this;
        var o = $that.options;
        var $table = (typeof (o.table) === 'string' ? $('#' + o.table) : $(o.table));
        $table.bootstrapTable('resetView', { height: height });
    };
    Table.prototype.destroy = function () {
        var $that = this;
        var o = $that.options;
        var $table = (typeof (o.table) === 'string' ? $('#' + o.table) : $(o.table));
        $table.bootstrapTable('destroy', { silent: true });
    };
    Table.prototype.getFuns = function () {
        var $that = this;
        $that.options.funs = $that.options.funs ||
            {
                btnCreate: function (url) { $that.defaultCreate(url); },
                btnUpdate: function (url) { $that.defaultUpdate(url); },
                btnDelete: function (url) { $that.defaultDelete(url); },
                btnSearch: function () { $that.refresh(true); },
                none: function () { console.log("No type"); }
            };
        return $that.options.funs;
    };
    Table.prototype.addFuns = function (key, fun) {
        var $that = this;
        var funs = $that.getFuns();
        funs[key] = fun;
        $that.options.funs = funs;
    };
    Table.prototype.defaultCreate = function (url, opt) {
        var $that = this;
        console.log("Add");
        opt = opt || {};
        var modalOptions = getModal(url, opt, abp.localization.iwbZero('OpCreate'), 'btnCreate', $that);
        var $modal = typeof modalOptions.modal === 'string' ? $('#' + modalOptions.modal) : $(modalOptions.modal);
        $modal.iwbModal(modalOptions);
    };
    Table.prototype.defaultUpdate = function (url, opt, row) {
        var $that = this;
        console.log("Update");
        var o = $that.options;
        var $table = (typeof (o.table) === 'string' ? $('#' + o.table) : $(o.table));
        opt = opt || {};
        row = row || opt.row|| $table.bootstrapTable("getSelections")[0];
        if (row) {
            var modalOptions = getModal(url, opt, abp.localization.iwbZero('OpUpdate'), 'btnUpdate', $that);
            if (!modalOptions.data) {
                modalOptions.data = row;
            }
            var $modal = typeof (modalOptions.modal) === 'string' ? $('#' + modalOptions.modal) : $(modalOptions.modal);
            $modal.iwbModal(modalOptions);
        } else
            abp.message.warn(abp.localization.iwbZero('SelectRecordOperation'));
    };
    Table.prototype.defaultDelete = function (url, opt,row) {
        console.log("Delete");
        var $that = this;
        var o = $that.options;
        var $table = (typeof (o.table) === 'string' ? $('#' + o.table) : $(o.table));
        opt = opt || {};
        row = row || opt.row || $table.bootstrapTable("getSelections")[0];
        if (row) {
            
           var data = opt.data || { id: row.id };
            abp.message.confirm(abp.localization.iwbZero('DeleteConfirmContent'),
                abp.localization.iwbZero('DeleteConfirm'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        $.iwbAjax({ table: $table,url: url + '?id=' + data.id, data: data, type: 'post', isValidate: false });
                    }
                });
        } else
            abp.message.warn(abp.localization.iwbZero('SelectRecordOperation'));
    };
    Table.prototype.setSearchList = function (obj) {
        var $that = this;
        var o = $that.options;
        o.searchList = obj;
    };
    Table.prototype.getSelections = function () {
        var $that = this;
        var o = $that.options;
        var $table = (typeof (o.table) === 'string' ? $('#' + o.table) : $(o.table));
        return $table.bootstrapTable("getSelections");
    }
    var allowedMethods = [
        'loadTable',
        'refresh',
        'getFuns',
        'defaultCreate',
        'defaultUpdate',
        'defaultDelete',
        'resetView',
        'destroy',
        'setSearchList',
        'getSelections'
    ];
    $.fn.iwbTable = function (option) {
        var value,
            args = Array.prototype.slice.call(arguments, 1);
        this.each(function () {
            var $this = $(this);

            var data = $this.data('iwb.table'),
                options = $.extend({}, $this.data(),
                    typeof option === 'object' && option);
            if (typeof option === 'string') {
                if ($.inArray(option, allowedMethods) < 0) {
                    throw new Error("Unknown method: " + option);
                }
                if (!data) {
                    return;
                }
                value = data[option].apply(data, args);
                if (option === 'destroy') {
                    $this.removeData('iwb.table');
                    $this.removeData('iwbtable.init');
                    return;
                }
            }
            if ($this.data('iwbtable.init')) {
                return;
            }
            $this.data('iwbtable.init', true);
            if (!data) {
                data = new Table(this, options);
                $this.data('iwb.table', data);
            }
        });

        return typeof value === 'undefined' ? this : value;
    };
})(jQuery, window, document);

/*Modal*/
(function ($, window, document, undefined) {
    'use strict';
    var Modal = function (ele, opt) {
        this.$ele = ele,
            this.defaults = {
                modal: ele,
                modaltitle: '',
                data: null,
                form: null,
                table: undefined,
                errorPlacement: function (error, element) {
                    element.parent().before(error);
                    //if (element.is('select')) {
                    //    if (!element.next().next().hasClass('error')) {
                    //        element.next().after(error);
                    //        element.focus();
                    //    }
                    //} else {
                    //    element.parent().before(error);
                    //    element.focus();
                    //}
                },
                rules: {},
                readonly: '',
                disabled: '',
                select2: true,
                select2tree: false,
                url: '',
                type:'post',
                savebtn: null,
                save: null,
                success: null,
                shownBefore: null,
                shownAfter: null,
                isRefresh:true,
                draggable: true,
                backdrop: 'static',
                keyboard: true,
                kindeditorOption: {},
                content:undefined,
                abp: window.abp
            },
            this.options = $.extend({}, this.defaults, opt);
        this.draggable();
    };
    //var abp = window.abp || {};
    Modal.prototype.getModal = function () {
        var $that = this;
        var $modal = typeof ($that.options.modal) === 'string' ? $('#' + $that.options.modal) : $($that.options.modal);
        return $modal;
    };
    Modal.prototype.open = function () {
        var $that = this;
        var $modal = $that.getModal();
        $modal.off('show.bs.modal').on('show.bs.modal',
            function () {
                $that.showBefore();
                $that.init();
                $that.setData();
                $that.setSelect2();
                $that.initKindeditor();
                $that.verticalCenter();
                $that.bindSave();
                $.formValidate($modal.find('form'), { noValid: true });
                $that.showAfter();
            });
        $that.show2();
    };
    Modal.prototype.show = function () {
        var $that = this;
        var $modal = $that.getModal();
        $modal.off('show.bs.modal').on('show.bs.modal',
            function () {
                $that.showBefore();
                $that.setData();
                $that.setSelect2();
                $that.initKindeditor();
                $that.verticalCenter();
                $.formValidate($modal.find('form'), { noValid: true });
                $that.showAfter();
            });
        $that.show2();
    };
    Modal.prototype.show2 = function () {
        var $that = this, o = $that.options;
        var $modal = $that.getModal();
        $modal.modal( {backdrop: o.backdrop,  keyboard: o.keyboard });
        $modal.modal( 'show');
        $that.verticalCenter();
    };
    Modal.prototype.create = function() {
        var $that = this,o=$that.options;
        if (!o.modalId) {
            o.modalId = 'Modal_' + (Math.floor((Math.random() * 1000000))) + new Date().getTime();
        }
        if (o.modalSize === null) {
            o.modalSize = "";
        }
        else if (o.modalSize) {
            o.modalSize = o.modalSize;
        } else {
            o.modalSize = 'modal-lg';
        }
        var createContainer = function(modalId, modalSize) {
            abp.ui.setBusy($("body"));
            $that.removeContainer(modalId);
            var containerId = modalId + 'Container';
            return $('<div id="' + containerId + '"></div>')
                .append(
                    '<div id="' + modalId + '" class="modal fade">' +
                    '  <div class="modal-dialog ' + modalSize + '">' +
                    '    <div class="modal-content"></div>' +
                    '  </div>' +
                    '</div>'
                ).appendTo('body');
        };
        var show = function() {
            $('#'+o.modalId).off('show.bs.modal').on('show.bs.modal',function () {
                    $that.showBefore();
                    $that.setSelect2();
                    $that.bindSave();
                    $.formValidate($('#'+o.modalId).find('form'), { noValid: true });
                    $that.showAfter();
                });
            $that.show2();
            abp.ui.clearBusy($("body"));
        };
        var $modalContent=createContainer(o.modalId, o.modalSize).find('.modal-content');
        if (o.viewUrl) {
            $modalContent.load(o.viewUrl, o.args, function (response, status, xhr) {
                if (status === "error") {
                    abp.message.warn(abp.localization.abpWeb('InternalServerError'));
                    return;
                }
                if (o.scriptUrl) {
                    $.loadScript(o.scriptUrl, function () {
                        show();
                    });
                } else {
                        show();
                }
            });
        } else if (o.content) {
            $modalContent.append(o.content);
            show();
        } else {
            return;
        }
    };
    Modal.prototype.hide = function () {
        var $that = this, o = $that.options;
        var $modal = $that.getModal();
        if (o.modalId) {
            $that.removeContainer(o.modalId);
        }
        return $modal.modal('hide');
    };
    Modal.prototype.removeContainer = function(modalId) {
        var containerId = modalId + 'Container';
        var containerSelector = '#' + containerId;
        var $container = $(containerSelector);
        if ($container.length) {
            $container.remove();
        }
    };
    Modal.prototype.showBefore = function () {
        var $that = this;
        if ($that.options.shownBefore && typeof ($that.options.shownBefore) === 'function') {
            $that.options.shownBefore($that);
        }
    };
    Modal.prototype.showAfter = function () {
        var $that = this;
        if ($that.options.shownAfter && typeof ($that.options.shownAfter) === 'function') {
            $that.options.shownAfter($that);
        }
    };
    Modal.prototype.init = function () {
        var $that = this;
        var $modal = $that.getModal();
        $modal.find('input,select,textarea').val('').removeClass('error valid');
        $modal.find('input:not(.disabled),select:not(.disabled),textarea:not(.disabled)').prop('disabled', false);
        $modal.find('input:not(.readonly),select:not(.readonly),textarea:not(.readonly)').prop('readonly', false);
        $modal.find('label.error').remove();
        $modal.find('.modal-title-span').html($that.options.modaltitle);
        $that.setReadonly();
        $that.setDisabled();
        $that.cleanFile();
        $modal.find('input:not(:disabled,[type=\'hidden\'],[readonly=\'readonly\']):first').focus();
    };
    Modal.prototype.setReadonly = function () {
        var $that = this;
        var $modal = $that.getModal();
        if (!$that.options.readonly)
            return;
        var readonly = $that.options.readonly.split(',');
        for (var i = 0; i < readonly.length; i++) {
            if (readonly[i]) {
                $modal.find('#' + readonly[i]).prop('readonly', true);
            }
        }
    };
    Modal.prototype.setDisabled = function () {
        var $that = this;
        var $modal = $that.getModal();
        if (!$that.options.disabled)
            return;
        var disabled = $that.options.disabled.split(',');
        for (var i = 0; i < disabled.length; i++) {
            if (disabled[i]) {
                $modal.find('#' + disabled[i]).prop('disabled', true);

            }
        }
    };
    Modal.prototype.setData = function () {
        var $that = this;
        var $modal = $that.getModal();
        var $form = $that.options.form ? $($that.options.form) : $modal.find('form');
        $.formDeserialize($form, $that.options.data);
    };
    Modal.prototype.setSelect2 = function () {
        var $that = this;
        var $modal = $that.getModal();
        if ($that.options.select2) {
            $modal.find("select").select2();
            $modal.find('select').off("change.ff").on("change.ff",
                function () {
                    $(this).focus();
                    $(this).next(".error").remove();
                    $(this).blur();
                });
        }
        if ($that.options.select2tree) {
            var tree = $that.options.select2tree.split(',');
            for (var i = 0; i < tree.length; i++) {
                $modal.find('#' + tree[i]).select2tree();
            }
        }
    };
    Modal.prototype.initKindeditor = function () {
        var $that = this;
        var $modal = $that.getModal();
        $modal.find('textarea.kindeditor').each(function (i, v) {
            if ($(v).length && $.fn.iwbKindeditor) {
                var val = $(v).val();
                if ($(v).data("iwb.kindEditor")) {
                    $(v).iwbKindeditor('remove');
                }
                var option = $that.options.kindeditorOption || {};
                $(v).iwbKindeditor(option);
                window.editor[$(v).attr('id')].html(val);
            }
        });
    };
    Modal.prototype.cleanFile = function () {
        var $that = this;
        var $modal = $that.getModal();
        $modal.find('input[type="file"]').each(function (i, v) {
            $(v).iwbFileUpload('cleanFile');
        });
    };
    Modal.prototype.bindSave = function () {
        var $that = this;
        var $modal = $that.getModal();
        if ($that.options.url) {
            var $saveBtn = $that.options.savebtn
                ? $($that.options.savebtn)
                : $modal.find('.save-btn').length > 0
                    ? $modal.find('.save-btn')
                    : $modal.find('.save').length > 0
                        ? $modal.find('.save')
                        : $modal.find('#save');
            var opt = $that.getSaveOptions();
            if ($that.options.save && typeof ($that.options.save) === "function") {
                $saveBtn.off('click.save').on('click.save',function () {$that.options.save(opt);});
            } else {
                $saveBtn.off('click.save').on('click.save',function () {$.iwbAjax($that.options.url, opt);});
            }
            $modal.find('.modal-body').keydown(function(e) {
                if (e.which === 13) {
                    if (e.target.tagName.toLocaleLowerCase() === "textarea") {
                        e.stopPropagation();
                    } else {
                        e.preventDefault();
                        var opt = $that.getSaveOptions();
                        if ($that.options.save && typeof $that.options.save === "function") {
                            $that.options.save(opt);
                        } else {
                            $.iwbAjax($that.options.url, opt);
                        }
                    }

                }
            });
        }
    };
    Modal.prototype.getSaveOptions = function () {
        var $that = this;
        var $modal = $that.getModal();
        var $form = $that.options.form ? $($that.options.form) : $modal.find('form');
        return {
            url: $that.options.url,
            type: $that.options.type,
            success: $that.options.success,
            form: $form,
            data:$that.options.data,
            table: $that.options.table,
            modal: $that.options.modal,
            blockUI: $modal.find('.modal-dialog'),
            isRefresh: $that.options.isRefresh
        };
    };
    Modal.prototype.verticalCenter = function () {
        var $that = this;
        var $modal = $that.getModal();
        $modal.css('display', 'block');
        var topHeight = $(window).height() - $modal.find('.modal-dialog').height() - 150;
        if (topHeight < 30) {
            topHeight = 30;
        }
        $modal.find('.modal-dialog').animate({ 'marginTop': topHeight / 2 + "px" });
    };
    Modal.prototype.draggable = function () {
        var $that = this;
        var $modal = $that.getModal();
        if ($that.options.draggable) {
            //console.log('draggable');
            $('.modal-header').css('cursor', 'move');
            /** 拖拽模态框*/
            var dragModal = {
                mouseStartPoint: { 'left': 0, 'top': 0 },
                mouseEndPoint: { 'left': 0, 'top': 0 },
                mouseDragDown: false,
                basePoint: { 'left': 0, 'top': 0 },
                moveTarget: null,
                topleng: 0
            };
            $modal.off('mousedown.drag').on('mousedown.drag',
                '.modal-header',
                function (e) {
                    
                    //webkit内核和火狐禁止文字被选中
                    $('body').addClass('drag-select');
                    //ie浏览器禁止文字选中
                    document.body.onselectstart = document.body.ondrag = function () {
                        return false;
                    };
                    if ($(e.target).hasClass('close')) //点关闭按钮不能移动对话框  
                        return;
                    dragModal.mouseDragDown = true;
                    dragModal.moveTargetHeader = $(this);
                    dragModal.moveTarget = $(this).closest('.modal-content');
                    dragModal.mouseStartPoint = { 'left': e.clientX, 'top': e.pageY };
                    dragModal.basePoint = dragModal.moveTarget.offset();
                    dragModal.topLeng = e.pageY - e.clientY;
                });
            $modal.off('mouseup.drag').on('mouseup.drag',
                function () {
                    dragModal.mouseDragDown = false;
                    dragModal.moveTarget = undefined;
                    dragModal.mouseStartPoint = { 'left': 0, 'top': 0 };
                    dragModal.basePoint = { 'left': 0, 'top': 0 };
                });
            $modal.off('mousemove.drag').on('mousemove.drag',
                function (e) {
                    if (!dragModal.mouseDragDown || dragModal.moveTarget === undefined) return;
                    var mousX = e.clientX;
                    var mousY = e.pageY;
                    if (mousX < 0) mousX = 0;
                    if (mousY < 0) mousY = 25;
                    dragModal.mouseEndPoint = { 'left': mousX, 'top': mousY };
                    var width = dragModal.moveTarget.width();
                    var height = dragModal.moveTargetHeader.height();
                    var clientWidth = document.body.clientWidth;
                    var clientHeight = document.body.clientHeight;
                    if (dragModal.mouseEndPoint.left < dragModal.mouseStartPoint.left - dragModal.basePoint.left) {
                        dragModal.mouseEndPoint.left = 0;
                    } else if (dragModal.mouseEndPoint.left >=
                        clientWidth - width + dragModal.mouseStartPoint.left - dragModal.basePoint.left) {
                        dragModal.mouseEndPoint.left = clientWidth - width - 38;
                    } else {
                        dragModal.mouseEndPoint.left =
                            dragModal.mouseEndPoint.left -
                            (dragModal.mouseStartPoint.left - dragModal.basePoint.left); //移动修正，更平滑  
                    }
                    if (dragModal.mouseEndPoint.top - (dragModal.mouseStartPoint.top - dragModal.basePoint.top) <
                        dragModal.topLeng) {
                        dragModal.mouseEndPoint.top = dragModal.topLeng;
                    } else if (dragModal.mouseEndPoint.top - dragModal.topLeng >
                        clientHeight - height + dragModal.mouseStartPoint.top - dragModal.basePoint.top) {
                        dragModal.mouseEndPoint.top = clientHeight - height - 38 + dragModal.topLeng;
                    } else {
                        dragModal.mouseEndPoint.top = dragModal.mouseEndPoint.top -
                            (dragModal.mouseStartPoint.top - dragModal.basePoint.top);
                    }
                    dragModal.moveTarget.offset(dragModal.mouseEndPoint);
                });
            $(document).on('hidden.bs.modal',
                '.modal',
                function () {
                    $('.modal-content').css({ 'top': 0, 'left': 0 });
                    $('body').removeClass('drag-select');
                    document.body.onselectstart = document.body.ondrag = null;
                });
        } else {
            $('.modal-header').css('cursor', 'default');
        }
    };

    var allowedMethods = [
        'open',
        'show',
        'show2',
        'hide'
    ];

    $.fn.iwbModal = function (option) {
        var value,
            args = Array.prototype.slice.call(arguments, 1);
        this.each(function () {
            var modal = new Modal(this, option);
            if (typeof option === 'string') {
                if ($.inArray(option, allowedMethods) < 0) {
                    throw new Error("Unknown method: " + option);
                }
                value = modal[option].apply(modal, args);
            } else {
                modal.open();
            }
        });
        return typeof value === 'undefined' ? this : value;
    };

})(jQuery, window, document);

/*FileUpload*/
(function ($, window, document, undefined) {
    'use strict';
    var abp = window.abp || {};
    var File = function (ele, opt) {
        this.$ele = ele,
            this.defaults = {
                file: ele,
                maxSize: 2,
                isImage: true,
                targetInfo: undefined,
                targetName: "fileName",
                targetExt: "fileExt",
                callback: undefined,
                abp: window.abp
            },
            this.options = $.extend({}, this.defaults, opt || {});
        //var $that = this;
        //$that.show();
    };

    File.prototype.getFile = function () {
        var $that = this;
        var $file = typeof ($that.options.file) === 'string' ? $('#' + $that.options.file) : $($that.options.file);
        return $file;
    };
    File.prototype.show = function () {
        var $that = this;
        var $file = $that.getFile();
        $file.off('change.file.check').on('change.file.check', function () {
            $that.fileCheck($that);
        });
        $file.closest('.iwb-file ').removeClass("file-success").removeClass("file-error");
        //return $file[0].click();
    };
    File.prototype.fileCheck = function ($that) {
        var $file = $that.getFile();
        if ($that.checkSize()) {
            var fileName = $file.val().split("\\").pop();
            $file.closest('.iwb-file ').addClass("file-success").find("label").text(fileName);
            var file = document.getElementById($file.attr("id")).files[0];
            $that.readFile(file);
            if ($that.options.targetName && $that.options.targetName !== undefined) {
                var name = fileName.substring(0, fileName.lastIndexOf("."));
                if (!$that.options.targetName)
                    return;
                var targetName = $that.options.targetName.split(',');
                for (var i = 0; i < targetName.length; i++) {
                    $('#' + targetName[i]).val(name);
                }
            }
            if ($that.options.targetExt && $that.options.targetExt !== undefined) {
                var ext = fileName.substring(fileName.lastIndexOf(".") + 1, fileName.length);
                if (!$that.options.targetExt)
                    return;
                var targetExt = $that.options.targetExt.split(',');
                for (var j = 0; j < targetExt.length; j++) {
                    $('#' + targetExt[j]).val(ext);
                }
            }
            var callback = $that.options.callback;
            if (callback && typeof (callback) === "function") {
                callback(fileName, file);
            }
        } else {
            var target = $that.options.targetInfo ? $that.options.targetInfo : $file.attr('id').replace('_file', '');
            $('#' + target).closest('.iwb-file ').addClass("file-error").find("label").text(abp.localization.iwbZero('SelectFile'));
        }
    };
    File.prototype.checkSize = function () {
        var $that = this;
        var $file = $that.getFile();
        var maxsize = $that.options.maxSize * 1024 * 1024;
        //var errMsg = "上传的附件文件不能超过" + $that.options.maxSize + "M。";
        //var tipMsg = "您的浏览器暂不支持计算上传文件的大小，确保上传文件不要超过" + $that.options.maxSize + "M，建议使用IE、FireFox、Chrome浏览器。";
        var errMsg = abp.localization.iwbZero('FileUploadErrorMsg', $that.options.maxSize );
        var tipMsg = abp.localization.iwbZero('FileUploadTipMsg', $that.options.maxSize);
        var browserCfg = {};
        var ua = window.navigator.userAgent;
        if (ua.indexOf("MSIE") >= 1) {
            browserCfg.ie = true;
        } else if (ua.indexOf("Firefox") >= 1) {
            browserCfg.firefox = true;
        } else if (ua.indexOf("Chrome") >= 1) {
            browserCfg.chrome = true;
        }
        try {
            var objFile = document.getElementById($file.attr("id"));
            if (objFile.value === "") {
                //layer.alert("请先选择上传文件", { icon: 7, title: "提示信息" });
                return false;
            }
            var filesize;
            if (browserCfg.firefox || browserCfg.chrome) {
                filesize = objFile.files[0].size;
            } else if (browserCfg.ie) {
                var objImg = document.createElement("img");
                objImg.id = "tempImg";
                objImg.style.display = "none";
                document.body.appendChild(objImg);
                objImg.dynsrc = objFile.value;
                filesize = objImg.fileSize;
            } else {
                $that.cleanFile();
                abp.message.warn(tipMsg);
                return false;
            }
            if (filesize === -1) {
                $that.cleanFile();
                abp.message.warn(tipMsg);
                return false;
            } else if (filesize > maxsize) {
                $that.cleanFile();
                abp.message.warn(errMsg);
                return false;
            } else {
                return true;
            }
        } catch (e) {
            $that.cleanFile();
            abp.message.error(e);
            return false;
        }
    };
    File.prototype.readFile = function (file) {
        //判断是否是图片类型
        //console.log(file.type);
        if (!file) {
            return;
        }
        var $that = this;
        var $file = $that.getFile();
        if ($that.options.isImage && !/image\/\w+/.test(file.type)) {
            abp.message.warn(abp.localization.iwbZero('FileUploadOnlyImage'));
            return;
        }
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            var fileinfo = this.result.substring(this.result.indexOf(',') + 1);
            console.log(fileinfo);
            var target = $that.options.targetInfo ? $that.options.targetInfo : $file.attr('id').replace('_file', '');
            $('#' + target).val(fileinfo);
        };
    };
    File.prototype.cleanFile = function () {
        var $that = this;
        var $file = $that.getFile();
        $file.closest('.custom-file ').removeClass("file-success").removeClass("file-error").find("label").text(abp.localization.iwbZero('SelectFile'));
        $file.after($file.clone().val(""));
        $file.remove();
    };

    var allowedMethods = [
        'show',
        'readFile',
        'cleanFile',
        'fileCheck',
        'checkSize'
    ];

    $.fn.iwbFileUpload = function (option) {
        var value,
            args = Array.prototype.slice.call(arguments, 1);
        this.each(function () {
            var $this = $(this);
            var data = $this.data('iwb.FileUpload');
            if (!data) {
                data = new File(this, option);
                $this.data('iwb.FileUpload', data);
            }
            if (typeof option === 'string') {
                if ($.inArray(option, allowedMethods) < 0) {
                    throw new Error("Unknown method: " + option);
                }
                value = data[option].apply(data, args);
            } else {
                data.show();
            }
        });
        return typeof value === 'undefined' ? this : value;
    };

})(jQuery, window, document);

/*FileOpenShow*/
(function ($, window, document, undefined) {
    'use strict';
    var File = function (ele, opt) {
        this.$ele = ele,
            this.defaults = {
                url: undefined,
                type: 'img'
            },

            this.options = $.extend({}, this.defaults, opt);
        if ($(this.$ele).data('type')) {
            this.options.type = $(this.$ele).data('type');
        }
        if (!this.options.url) {
            this.options.url = $(this.$ele).data('url');
        }
        if (this.options.url && this.options.url.indexOf("/") !== 0) {
            this.options.url = "/" + this.options.url;
        }
    };
    //var abp = window.abp || {};
    File.prototype.show = function () {
        var $that = this;
        switch ($that.options.type) {
            case 'img':
            case 'image':
                this.showImage();
                break;
            case 'office':
                this.showOffice();
                break;
            default:
                this.showFile();
                return;
        }
    };
    File.prototype.showImage = function () {
        var $that = this;
        $.metPageCss('/Content/Libs/viewer/viewer.min.css', "dy-css-viewer");
        $.metPageJs('/Content/Libs/viewer/viewer.min.js', "dy-js-viewer");
        if ($("#Img-Modal").length > 0) {
            $("#Img-Modal").remove();
        }
        $("body").append(' <ul id="Img-Modal" style="display:none"><li><img src="' +
            $that.options.url +
            '" data-original="' +
            $that.options.url +
            '" alt="Picture" /></li></ul>');
        var $image = $('#Img-Modal');
        var options = {
            // inline: true,
            url: 'data-original',
            button: true,
            navbar: false,
            title: false,
            toolbar: false,
            ready: function (e) {
                console.log(e.type);
            }
        };
        $image.on({
            show: function (e) {
                console.log(e.type);
            },
            hidden: function(e) {
                $image.viewer('destroy');
                $image.remove();
            }
        }).viewer(options);
        $image.viewer("show");
        $image.css("display", "none");

    };
    File.prototype.showOffice = function () {
        var $that = this;
        var url = 'http://ow365.cn/?i=17314&furl=http://' + window.location.host + $that.options.url;
        $that.showFile(url);
    };
    File.prototype.showFile = function (url) {
        var $that = this;
        url = url || $that.options.url;
        if ($('#File-Modal').length <= 0) {
            $('body').append(
                '<section><div class="modal fade" id="File-Modal" role="dialog" tabindex="-1" aria-hidden="true"><div class="modal-dialog  modal-dialog-centered" role="document" style="min-width: calc(100% - 600px);"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true"><span aria-hidden="true">&times;</span></button><h4 class="modal-title">文件预览</h4></div><div class="modal-body"  style=" overflow: hide"></div><div class="modal-footer" style="text-align: center;"><button type="button" class="btn btn-success waves-effect" data-dismiss="modal" style="min-width: 100px;background: #F0985D;border: 1px solid #DDDDDD;">关闭窗口</button><button id="download" type="button" class="btn btn-success waves-effect"  style="min-width: 100px;background-color: #678ceb;border: 1px solid #678ceb;border: 1px solid #DDDDDD;">下载文件</button></div></div></div></div></section>');
        }
        $('#File-Modal').find('.modal-body')
            .html('<iframe src="' + url + '" width="100%"; style="min-height: calc(100vh - 200px);" frameborder="0"></iframe>');
        $(document).off('click.download', '#File-Modal #download').on('click.download', '#File-Modal #download',
            function () { $that.download(url)});
        $('#File-Modal').iwbModal('show2');
    };
    File.prototype.download = function (url) {
        var $that = this;
        url = url || $that.options.url;
        if ($('#tempLink-fd').length>0) {
            $('#tempLink-fd').remove();
        }
        $('body').append('<a id="tempLink-fd" href="' + url + '" style="display:none"></a>');
        $('#tempLink-fd')[0].click();
        $('#tempLink-fd').remove();
    };
    var allowedMethods = [
        'show',
        'download'
    ];
    $.fn.iwbFileShow = function (option) {
        var value,
            args = Array.prototype.slice.call(arguments, 1);
        this.each(function () {
            var $this = $(this);
            var data = $this.data('iwb.FileShow');
            if (!data || typeof option !== 'string') {
                data = new File(this, option);
                $this.data('iwb.FileShow', data);
            }
            if (typeof option === 'string') {
                if ($.inArray(option, allowedMethods) < 0) {
                    throw new Error("Unknown method: " + option);
                }
                value = data[option].apply(data, args);
            } else {
                data.show();
            }
        });
        return typeof value === 'undefined' ? this : value;
    };
})(jQuery, window, document);


/*TreeView*/
(function ($, window, document, undefined) {
    'use strict';
    var TreeView = function (ele, opt) {
        var that = this;
        var tempId = '';
        var onNodeSelected = function (e, data, that) {

            var o = that.options;
            if (!data || !data[o.selectFiledName]) {
                return;
            }
            var $input = (typeof (o.selectFiledInput) === 'string' ? $('#' + o.selectFiledInput) : $(o.selectFiledInput));
            tempId = data[o.selectFiledName];
            $input.val(tempId);
            var $table = (typeof (o.table) === 'string' ? $('#' + o.table) : $(o.table));
            $table.iwbTable('refresh', true);
        };
        var onNodeUnselected = function (e, data, that) {

            var o = that.options;
            var $input = (typeof (o.selectFiledInput) === 'string' ? $('#' + o.selectFiledInput) : $(o.selectFiledInput));

            $input.val('');
            var $table = (typeof (o.table) === 'string' ? $('#' + o.table) : $(o.table));
            $table.iwbTable('refresh', true);
        };
        this.$ele = ele,
            this.defaults = {
                url: undefined,
                data: undefined,
                levels: 2,
                emptyIcon: "iconfont icon-delete",
                collapseIcon: 'iconfont icon-delete ',
                expandIcon: 'iconfont icon-add',
                selectedBackColor: '#563d7c',
                onNodeSelected: function (e, d) {
                    onNodeSelected(e, d, that);
                },
                onNodeUnselected: function (e, d) {
                    onNodeUnselected(e, d, that);
                },
                table: 'table',
                selectFiledName: 'id',
                selectFiledInput: 'keyWords'
            },
            this.options = $.extend({}, this.defaults, opt);

    };


    //var abp = window.abp || {};
    TreeView.prototype.init = function () {
        var that = this, o = that.options;
        $.metPageCss('/Content/Libs/bootstrap-treeview/bootstrap-treeview.min.css', "dy-css-treeview");
        $.metPageJs('/Content/Libs/bootstrap-treeview/bootstrap-treeview.min.js', "dy-js-treeview");
        if (o.data) {
            that.loadTree();
        } else if (o.url) {
            that.postData();
        }
    };
    TreeView.prototype.loadTree = function (data) {
        var that = this, o = that.options;
        var option = $.extend({}, o);
        if (data) {
            option.data = data;
        }
        $(that.$ele).treeview(option);
    };
    TreeView.prototype.postData = function (url) {
        var that = this, o = that.options;
        url = url || o.url;
        $.iwbAjax({
            url: window.appUrl + url,
            isAlert: false,
            isValidate: false,
            isRefresh: false,
            success: function (res) {
                if (res) {
                    that.loadTree(res);
                }
            }
        });
    };

    var allowedMethods = [
        'init'
    ];
    $.fn.iwbTreeView = function (option) {
        var value,
            args = Array.prototype.slice.call(arguments, 1);
        this.each(function () {
            var $this = $(this);
            var data = $this.data('iwb.TreeView');
            if (!data) {
                data = new TreeView(this, option);
                $this.data('iwb.TreeView', data);
                data.init();
            }
            if (typeof option === 'string') {
                if ($.inArray(option, allowedMethods) < 0) {
                    throw new Error("Unknown method: " + option);
                }
                value = data[option].apply(data, args);
            }
        });
        return typeof value === 'undefined' ? this : value;
    };
})(jQuery, window, document);


/*DateTime*/
(function ($, window, document, undefined) {
    'use strict';
    var DateTime = function (ele, opt) {
        this.$ele = ele,
            this.defaults = {
                language: 'zh-CN',
                format: "yyyy-mm-dd", //yyyy-mm-ddThh:ii:ssZ
                autoclose: true, //当选择一个日期之后是否立即关闭此日期时间选择器。
                startView: 2, //日期时间选择器打开之后首先显示的视图。 可接受的值：0 or 'hour' \ 1 or 'day' \2 or 'month' (the default )\3 or 'year' \4 or 'decade' for the 10-year overview.
                minView: 2, //日期时间选择器所能够提供的最精确的时间选择视图。
                maxView: 4, //日期时间选择器最高能展示的选择范围视图。
                todayBtn: true,
                weekStart: 0, //一周从哪一天开始。0（星期日）到6（星期六）
                startDate: '2015-01-01', //开始时间
                endDate: null, //结束时间,
                forceParse: true, //强制解析
                minuteStep: 10, //分钟视图中分钟间隔
                twoDateId: undefined,//双日期外部Id(默认找里面的 .startTime和.endTime)
                startId: undefined,//开始日期Id,不填就是本身
                endId: undefined,//结束日期Id
                isAutoSetDate: true,//双日期自动填写另一个日期
                startEndInterval: 30,//双日期自动填写间隔（天）
                defaultDate: undefined//默认日期时间

            },
            this.options = $.extend({}, this.defaults, opt);

    };


    //var abp = window.abp || {};
    DateTime.prototype.init = function () {
        var that = this;//, o = that.options;
        $.metPageCss('/Content/Libs/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css', "dy-css-dateTime");
        //$.metPageCss('/Content/Plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker-wr.css', "dy-css-dateTime-self");
        $.metPageJs('/Content/Libs/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js', "dy-js-dateTime");
        $.metPageJs('/Content/Libs/bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.zh-CN.js', "dy-js-dateTime-zhCN");
    };
    DateTime.prototype.date = function () {
        var that = this, o = that.options;
        o.format = "yyyy-mm-dd";
        o.startView = 2;
        o.minView = 2;
        that.load();
    };
    DateTime.prototype.dateTime = function () {
        var that = this, o = that.options;
        o.format = "yyyy-mm-dd hh:ii";
        o.startView = 2;
        o.minView = 1;
        that.load();
    };
    DateTime.prototype.dateSecond = function () {
        var that = this, o = that.options;
        o.format = "yyyy-mm-dd hh:ii:ss";
        o.startView = 2;
        o.minView = 0;
        that.load();
    };
    DateTime.prototype.twoDate = function () {
        var that = this, o = that.options;
        var $start, $end;
        if (o.twoDateId) {
            $start = $('#' + o.twoDateId).find('.startTime');
            $end = $('#' + o.twoDateId).find('.endTime');
        } else {
            $start = $('#' + o.startId);
            $end = $('#' + o.endId);
        }
        $start = $start.length > 0 ? $start : $(that.$ele);
        if ($end.length <= 0) {
            $end = $start.parent().find('.endTime');
            if ($end.length <= 0) {
                throw new Error("Unknown EndDate Selector");
            }
        }
        $start.datetimepicker('remove');
        $start.datetimepicker(o).on('show',
            function (event) {
                event.preventDefault();
                event.stopPropagation();
            }).on('hide',
                function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                }).on("changeDate",
                    function (e) {
                        //console.log(e);
                        //console.log(e.date.valueOf());
                        $end.datetimepicker('setStartDate', e.date);
                        if (o.isAutoSetDate && o.startEndInterval && o.startEndInterval > 0) {
                            $end.datetimepicker('setDate',
                                new Date(e.date.valueOf() + 1000 * 60 * 60 * 24 * o.startEndInterval));
                        }
                    });
        $end.datetimepicker('remove');
        $end.datetimepicker(o).on('show',
            function (event) {
                event.preventDefault();
                event.stopPropagation();
            }).on('hide',
                function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                }).on("changeDate",
                    function (e) {
                        //console.log(e);
                        //console.log(e.date.valueOf());
                        $start.datetimepicker('setEndDate', e.date);
                        if (o.isAutoSetDate && o.startEndInterval && o.startEndInterval > 0) {
                            $start.datetimepicker('setDate', new Date(e.date.valueOf() - 1000 * 60 * 60 * 24 * o.startEndInterval));
                        }
                    });
        if (o.isAutoSetDate) {
            var eDate = o.defaultDate ? o.defaultDate : new Date();
            $end.datetimepicker("setDate", eDate);
            $start.datetimepicker("setDate",
                new Date(eDate.valueOf() - 1000 * 60 * 60 * 24 * (o.startEndInterval ? o.startEndInterval : 30)));
        }
    };
    DateTime.prototype.load = function () {
        var that = this, o = that.options;
        $(that.$ele).datetimepicker('remove');
        $(that.$ele).datetimepicker(o).on('show', function (event) {
            event.preventDefault();
            event.stopPropagation();
        }).on('hide', function (event) {
            event.preventDefault();
            event.stopPropagation();
        });

    };

    //var allowedMethods = [
    //    'date',
    //    'dateTime',
    //    'dateSecond',
    //    'twoDate'
    //];
    //$.fn.iwbDate2 = function (option) {
    //    var value,
    //        args = Array.prototype.slice.call(arguments, 1);
    //    this.each(function () {
    //        var $this = $(this);
    //        var data = $this.data('iwb.dateTime');
    //        if (!data) {
    //            data = new DateTime(this, option ? option : {});
    //            $this.data('iwb.dateTime', data);
    //            data.init();
    //        }
    //        if (typeof option === 'string') {
    //            if ($.inArray(option, allowedMethods) < 0) {
    //                throw new Error("Unknown method: " + option);
    //            }
    //            value = data[option].apply(data, args);
    //        } else if (option && option.method && typeof option.method === 'string') {
    //            if ($.inArray(option.method, allowedMethods) < 0) {
    //                throw new Error("Unknown method: " + option.method);
    //            }
    //            value = data[option.method].apply(data, args);
    //        }
    //        else {
    //            if (!option && $this.hasClass('iwb-date')) {
    //                data.date();
    //            } else if (!option && ($this.hasClass('iwb-date-time') || $this.hasClass('iwb-dateTime'))) {
    //                data.dateTime();
    //            } else if (!option && ($this.hasClass('iwb-date-second') || $this.hasClass('iwb-dateSecond'))) {
    //                data.dateSecond();
    //            } else if (!option && ($this.hasClass('startTime') || $this.hasClass('iwb-date-two') || $this.hasClass('iwb-twoDate'))) {
    //                data.twoDate();
    //            } else {
    //                data.load();
    //            }
    //        }
    //    });
    //    return typeof value === 'undefined' ? this : value;
    //};
    //$.fn.iwbTwoDate2 = function (option) {
    //    this.each(function () {
    //        var $this = $(this);
    //        var data = $this.data('iwb.dateTime');
    //        if (!data) {
    //            data = new DateTime(this, option);
    //            $this.data('iwb.dateTime', data);
    //            data.init();
    //            data.twoDate();
    //        }
    //    });
    //    return this;
    //};
    //$('.iwb-date-time').iwbDate();
    //$('.iwb-date').iwbDate();
})(jQuery, window, document);


/*DateTime*/
(function ($, window, document, undefined) {
    'use strict';
    var DateTime = function(ele, opt) {
        this.$ele = ele,
            this.defaults = {
                language: 'zh-CN',
                singleDatePicker: true,
                dayStep:false,
                opens: "center",
                drops: "down",
                format: "YYYY-MM-DD", //YYYY-MM-DD HH:MM:SS
                showEvent:undefined,
                showCalendarEvent:undefined,
                hideEvent:undefined,
                hideCalendarEvent:undefined,
                applyEvent:undefined,
                cancelEvent:undefined,
                minDate: '2015-01-01',
                maxDate: false,
                showWeekNumbers: false,
                showISOWeekNumbers: false,
                timePicker: false,
                timePicker24Hour: true,
                timePickerIncrement: 5,
                timePickerSeconds: false,
                linkedCalendars: true,
                showCustomRangeLabel: false,
                alwaysShowCalendars: true,
                autoApply: true,
                autoUpdateInput: true,
                buttonClasses: "btn btn-sm",
                applyButtonClasses: "btn-success",
                cancelClass: "btn-default",
                startDate: undefined, //开始时间
                endDate: undefined, //结束时间,
                maxSpan: undefined,
                locale: undefined,
                ranges: undefined,
                hasRange: false
            },
            this.options = $.extend({}, this.defaults, opt);
    };
    var dateOpt = {};
    var timeSelect2 = function() {
        $('.daterangepicker .calendar-time > select').select2();
    }
    //var abp = window.abp || {};
    DateTime.prototype.init = function () {
        var that = this; var o = that.options;
        $.metPageCss('/Content/Plugins/bootstrap-daterangepicker/daterangepicker.css', "dy-css-daterangepicker");
        $.metPageJs('/Content/Plugins/bootstrap-daterangepicker/moment-with-locales.min.js', "dy-js-daterangepicker.moment");
        $.metPageJs('/Content/Plugins/bootstrap-daterangepicker/daterangepicker.js', "dy-js-daterangepicker");
        var lang = o.language.toLowerCase();
        if (lang === 'zh-hans') {
            lang = 'zh-cn';
        }
        moment.locale(lang);
        var localeName = {
                applyLabel: 'Apply',
                cancelLabel: 'Cancel',
                fromLabel: "",
                toLabel: "",
                customRangeLabel: 'Custom Range',
                daysOfWeek: moment.weekdaysMin(),
                monthNames: moment.monthsShort()
            },
            ranges = {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [
                    moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')
                ]
            };
        //if (o.language.toLowerCase() === 'zh-cn' || o.language.toLowerCase() === 'zh-hans') {
        //    localeName = {
        //        applyLabel: "确定",
        //        cancelLabel: "清空",
        //        fromLabel: "起始时间",
        //        toLabel: "结束时间'",
        //        customRangeLabel: "自定义",
        //        weekLabel: "W",
        //        daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
        //        monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],

        //    };
        //    ranges = {
        //        '今日': [moment(), moment()],
        //        '昨日': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
        //        '最近 7 日': [moment().subtract(6, 'days'), moment()],
        //        '最近 30 最近': [moment().subtract(29, 'days'), moment()],
        //        '上个月': [moment().startOf('month'), moment().endOf('month')],
        //        '本月': [
        //            moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')
        //        ]
        //    };
        //}

        dateOpt = {
            format: o.format, //YYYY-MM-DD HH:MM:SS
            opens: o.opens,
            startDate:o.startDate?o.startDate:false,
            endDate:o.endDate?o.endDate:false,
            singleDatePicker: o.singleDatePicker,
            drops: o.drops,
            minDate: o.minDate,
            maxDate: o.maxDate,
            showCustomRangeLabel: o.showCustomRangeLabel ? o.hasRange : o.showCustomRangeLabel,
            linkedCalendars: o.linkedCalendars,
            showWeekNumbers: o.showWeekNumbers,
            showISOWeekNumbers: o.showISOWeekNumbers,
            timePicker: o.timePicker,
            timePicker24Hour: o.timePicker24Hour,
            timePickerIncrement: o.timePickerIncrement,
            timePickerSeconds: o.timePickerSeconds,
            buttonClasses: o.buttonClasses,
            applyButtonClasses: o.applyButtonClasses,
            cancelClass: o.cancelClass,
            autoApply: o.autoApply,
            autoUpdateInput: o.autoUpdateInput,
            alwaysShowCalendars: o.alwaysShowCalendars,
            maxSpan: o.maxSpan ? o.maxSpan : o.dayStep ? {
                    days: o.dayStep
                } : false,
            locale: o.locale ? o.locale: {
                    direction: 'ltr',
                    format: o.format,
                    separator: " - ", //
                    applyLabel: localeName.applyLabel,
                    cancelLabel: localeName.cancelLabel,
                    fromLabel: localeName.fromLabel,
                    toLabel: localeName.toLabel,
                    customRangeLabel: localeName.customRangeLabel,
                    weekLabel: localeName.weekLabel,
                    daysOfWeek: localeName.daysOfWeek,
                    monthNames: localeName.monthNames,
                    firstDay: 1
                },
            ranges: !o.hasRange ? false : o.ranges ? o.ranges : ranges
    };
    
    };
    DateTime.prototype.date = function () {
        var that = this, o = that.options;
        var opt = {locale: {}};
        opt.locale.format = "YYYY-MM-DD";
        that.load(opt);
    };
    DateTime.prototype.dateTime = function () {
        var that = this//, o = that.options;
        var opt = {locale: {}};
        opt.locale.format = "YYYY-MM-DD HH:mm";
        opt.timePicker = true;
        that.load(opt);
    };
    DateTime.prototype.dateSecond = function () {
        var that = this//, o = that.options;
        var opt = {locale: {}};
        opt.locale.format = "YYYY-MM-DD HH:mm:SS";
        opt.timePicker = true;
        opt.timePickerSeconds = true;
        that.load(opt);
    };
    DateTime.prototype.dataRange = function () {
        var that = this, o = that.options;
        var opt = {locale: {}};
        opt.singleDatePicker = false;
        that.load(opt);
    };
    DateTime.prototype.dataRangeTime = function () {
        var that = this, o = that.options;
        var opt = {locale: {}};
        opt.locale.format = "YYYY-MM-DD HH:mm";
        opt.timePicker = true;
        opt.singleDatePicker = false;
        that.load(opt);
    };
    DateTime.prototype.dataRangeSecond = function () {
        var that = this, o = that.options;
        var opt = {locale: {}};
        opt.locale.format = "YYYY-MM-DD HH:mm:SS";
        opt.singleDatePicker = false;
        opt.timePicker = true;
        opt.timePickerSeconds = true;
        that.load(opt);
    };
    DateTime.prototype.load = function (opt) {
        var that = this, o = that.options;
        var option = $.extend({}, dateOpt, opt || {});
        $(that.$ele).daterangepicker(option, function (start, end, label) {
            console.log('Data-Change: ' + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD') + ' ( Range: ' + label + ')');
        });
        var cancelEvent = o.cancelEvent && o.cancelEvent.isFunction() ? o.cancelEvent : function () { $(that.$ele).val('')}
        $(that.$ele).on('cancel.daterangepicker', cancelEvent);
        if (o.applyEvent && o.applyEvent.isFunction()) {
            $(that.$ele).on('apply.daterangepicker', o.applyEvent);
        }
        
        $(that.$ele).on('show.daterangepicker', function () {
            //timeSelect2();
            if (o.showEvent && o.showEvent.isFunction()) {
                o.showEvent();
            } 
        });
        if (o.hideEvent && o.hideEvent.isFunction()) {
            $(that.$ele).on('hide.daterangepicker', o.hideEvent);
        }
        if (o.showCalendarEvent && o.showCalendarEvent.isFunction()) {
            $(that.$ele).on('showCalendar.daterangepicker', o.showCalendarEvent);
        }
        if (o.hideCalendarEvent && o.hideCalendarEvent.isFunction()) {
            $(that.$ele).on('hideCalendar.daterangepicker', o.hideCalendarEvent);
        }
        $(that.$ele).val('');
    };
    DateTime.prototype.setStartDate = function (data) {
        var that = this, o = that.options;
        $(that.$ele).data('daterangepicker').setStartDate(data);
    };
    DateTime.prototype.setEndDate = function (data) {
        var that = this, o = that.options;
        $(that.$ele).data('daterangepicker').setEndDate(data);
    };

    var allowedMethods = [
        'date',
        'dateTime',
        'dateSecond',
        'dataRange',
        'dataRangeTime',
        'dataRangeSecond',
        'setStartDate',
        'setEndDate'
    ];
    $.fn.iwbDate = function (option) {
        var value,
            args = Array.prototype.slice.call(arguments, 1);
        this.each(function () {
            var $this = $(this);
            var data = $this.data('iwb.date');
            if (!data) {
                data = new DateTime(this, option ? option : {});
                $this.data('iwb.date', data);
                data.init();
            }
            if (typeof option === 'string') {
                if ($.inArray(option, allowedMethods) < 0) {
                    throw new Error("Unknown method: " + option);
                }
                value = data[option].apply(data, args);
            } else if (option && option.method && typeof option.method === 'string') {
                if ($.inArray(option.method, allowedMethods) < 0) {
                    throw new Error("Unknown method: " + option.method);
                }
                value = data[option.method].apply(data, args);
            }
            else {
                if (!option && $this.hasClass('iwb-date')) {
                    data.date();
                } else if ((!option || !option.method) && ($this.hasClass('iwb-date-time') || $this.hasClass('iwb-dateTime'))) {
                    data.dateTime();
                } else if ((!option || !option.method) && ($this.hasClass('iwb-date-second') || $this.hasClass('iwb-dateSecond'))) {
                    data.dateSecond();
                } else if ((!option || !option.method) && ($this.hasClass('iwb-date-range') || $this.hasClass('iwb-range-date'))) {
                    data.dataRange();
                } else if ((!option || !option.method)&& ($this.hasClass('iwb-date-range-time') || $this.hasClass('iwb-range-dateTime'))) {
                    data.dataRangeTime();
                } else if ((!option || !option.method) && ($this.hasClass('iwb-date-range-second') || $this.hasClass('iwb-range-dateSecond'))) {
                    data.dataRangeSecond();
                } else {
                    data.load();
                }
            }
        });
        return typeof value === 'undefined' ? this : value;
    };
    $.fn.iwbDateRange = function (option) {
        var value,
            args = Array.prototype.slice.call(arguments, 1);
        this.each(function () {
            var $this = $(this);
            var data = $this.data('iwb.rangeDate');
            if (!data) {
                data = new DateTime(this, option);
                $this.data('iwb.rangeDate', data);
                data.init();
                data.dataRange();
            }
            if (typeof option === 'string') {
                if ($.inArray(option, allowedMethods) < 0) {
                    throw new Error("Unknown method: " + option);
                }
                value = data[option].apply(data, args);
            } 
        });
        return typeof value === 'undefined' ? this : value;
    };
    $('.iwb-date-time').iwbDate();
    $('.iwb-date').iwbDate();
})(jQuery, window, document);


/*Kindeditor*/
(function ($, window, document, undefined) {
    'use strict';
    window.editor = {};
    var Kindeditor = function (ele, opt) {
        this.$ele = ele,
            this.defaults = {
                isModal: true,
                tools: 'simpleTools',
                resizeMode: 1,
                width: '100%',
                height: '300px',
                bodyClass: 'article-content',
                urlType: '',
                uploadJson: '/system/KindEditorUploadFile',
                filterMode: false,
                allowFileManager: false,
                langType: 'zh-CN',
                cssData: 'html,body {background: none}.article-content, .article-content table td, .article-content table th {line-height: 1.3846153846; font-size: 13px;}.article-content .table-auto {width: auto!important; max-width: 100%;}',
                placeholder: '请输入...',
                placeholderStyle: { fontSize: '13px', color: '#888' },
                pasteImage: { postUrl: '/system/KindEditorUploadFile', placeholder: "\u53ef\u4ee5\u5728\u7f16\u8f91\u5668\u76f4\u63a5\u8d34\u56fe\u3002" },
                syncAfterBlur: true,
                afterChange: function () { this.sync(); },
                afterBlur: function () { this.sync(); },
                spellcheck: false
            },
            this.options = $.extend({}, this.defaults, opt);

    };
    var bugTools =
        ['formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline', '|',
            'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist', 'insertunorderedlist', '|',
            'emoticons', 'image', 'code', 'link', '|', 'removeformat', 'undo', 'redo', 'fullscreen', 'source', 'about'];
    var simpleTools =
        ['formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline', '|',
            'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist', 'insertunorderedlist', '|',
            'emoticons', 'image', 'code', 'link', 'table', '|', 'removeformat', 'undo', 'redo', 'fullscreen', 'source', 'about'];
    var fullTools =
        ['formatblock', 'fontname', 'fontsize', 'lineheight', '|', 'forecolor', 'hilitecolor', '|', 'bold', 'italic', 'underline', 'strikethrough', '|',
            'justifyleft', 'justifycenter', 'justifyright', 'justifyfull', '|',
            'insertorderedlist', 'insertunorderedlist', '|',
            'emoticons', 'image', 'insertfile', 'hr', '|', 'link', 'unlink', '/',
            'undo', 'redo', '|', 'selectall', 'cut', 'copy', 'paste', '|', 'plainpaste', 'wordpaste', '|', 'removeformat', 'clearhtml', 'quickformat', '|',
            'indent', 'outdent', 'subscript', 'superscript', '|',
            'table', 'code', '|', 'pagebreak', 'anchor', '|',
            'fullscreen', 'source', 'preview', 'about'];
    var editorToolsMap = { fullTools: fullTools, simpleTools: simpleTools, bugTools: bugTools };

    //var abp = window.abp || {};
    Kindeditor.prototype.init = function (options) {
        var that = this, o = that.options;
        options = options || {};
        $.metPageCss('/Content/Libs/kindeditor/themes/default/default.css', "dy-css-kindeditor");
        $.metPageJs('/Content/Libs/kindeditor/kindeditor-all-min.js', "dy-js-kindeditor");
        $.metPageJs('/Content/Libs/kindeditor/lang/zh-CN.js', "dy-js-kindeditor-zh-CN");
        that.setKindeditor(options);
    };
    Kindeditor.prototype.setKindeditor = function (options) {
        var that = this, o = that.options;
        var $editor = $(that.$ele);
        var editorId = $editor.attr('id');
        options = options || {};
        options = $.extend({}, o, $editor.data(), options);
        if (editorId === undefined) {
            editorId = 'kindeditor-' + Math.floor(Math.random() * 10000000);
            $editor.attr('id', editorId);
        }
        var editorTool = editorToolsMap[o.tools] || simpleTools;

        /* Remove fullscreen in modal. */
        if (o.isModal) {
            var newEditorTool = new Array();
            var i;
            for (i in editorTool) {
                if (editorTool.hasOwnProperty(i)) {
                    if (editorTool[i] !== 'fullscreen') newEditorTool.push(editorTool[i]);
                }
            }
            editorTool = newEditorTool;
        }

        $.extend(options,
            {
                items: editorTool,
                placeholder: $editor.attr('placeholder') || options.placeholder || ''
            });

        try {
            $editor.length && window.KindEditor && window.KindEditor.remove(editorId);
            var editor = window.KindEditor.create('#' + editorId, options);
            window.editor['#'] = window.editor[editorId] = editor;
            return editor;
        } catch (e) {
            console.log('kindEditor', e);
            return false;
        }
    };
    Kindeditor.prototype.remove = function () {
        var that = this, o = that.options;
        var $editor = $(that.$ele);
        var editorId = $editor.attr('id');
        if (editorId && window.KindEditor) {
            window.KindEditor.remove(editorId);
            $editor.data('iwb.kindEditor', undefined);
        }
    };
    var allowedMethods = [
        'init',
        'remove',
        'setKindeditor'
    ];
    $.fn.iwbKindeditor = function (option) {
        var value,
            args = Array.prototype.slice.call(arguments, 1);
        this.each(function () {
            var $this = $(this);
            var data = $this.data('iwb.kindEditor');
            if (!data) {
                data = new Kindeditor(this, option);
                $this.data('iwb.kindEditor', data);
                data.init();
            }
            if (typeof option === 'string') {
                if ($.inArray(option, allowedMethods) < 0) {
                    throw new Error("Unknown method: " + option);
                }
                value = data[option].apply(data, args);
            }
        });
        return typeof value === 'undefined' ? this : value;
    };
})(jQuery, window, document);

