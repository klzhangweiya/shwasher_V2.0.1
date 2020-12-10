var iwbfuns;
function GetFuns(ele) {
    var $table = typeof ele === 'string' ? $('#' + ele) : $(ele);
    return $table.iwbTable('getFuns');
}
function LoadTableBackFun(option) {
    if (!option) {
        option = { table: "table" };
    }
    option.table = option.table ? option.table : "table";
    var $table = typeof option.table === 'string' ? $('#' + option.table) : $(option.table);
    if ($table.length < 1) {
        console.log('没有发现表格:', option.table);
    }
    option.lang = option.lang ? option.lang : window.lang;
    $table.iwbTable(option);
    var funs = GetFuns($table);
    return funs;
}
function LoadTable(option) {
    if (!option) {
        option = { table: "table" };
    }
    option.table = option.table ? option.table : "table";
    var $table = typeof option.table === 'string' ? $('#' + option.table) : $(option.table);
    if ($table.length < 1) {
        console.log('没有发现表格:', option.table);
    }
    $table.iwbTable(option);
    window.iwbfuns = GetFuns($table);
    return $table;
}

function LoadTreeTable(option) {
    var $table = !option.table ? $('#table') : typeof option.table === 'string' ? $('#' + option.table) : $(option.table);
    if ($table.length < 1) {
        console.log('没有发现表格:', option.table);
        throw "没有发现表格";
    }
    $.metPageCss('/Content/Libs/bootstrap-table/extensions/treegrid/jquery.treegrid.min.css', "dy-css-treegrid");
    $.metPageJs('/Content/Libs/bootstrap-table/extensions/treegrid/bootstrap-table-treegrid.js', "dy-js-bootstrap-treegrid");
    $.metPageJs('/Content/Libs/bootstrap-table/extensions/treegrid/jquery.treegrid.min.js', "dy-js-treegrid");
    var defaultOption = {
        table: $table,
        rootNo: 0,
        idField: 'no',
        treeShowField: 'name',
        parentIdField: 'parentNo',
        level: "depth",
        customDataField:['path'],
        height: $(window).height() - 200,
        onLoadSuccess: function () {
            $table.treegrid({
                treeColumn: 1,
                expanderExpandedClass: 'iconfont icon-folderopen-fill',
                expanderCollapsedClass: 'iconfont icon-folder-fill'
            });
            $(".level-2").closest("tr").treegrid("collapse");
            $("td span.treegrid-expander:not(.iconfont)").addClass("iconfont icon-folder");
            abp.ui.clearBusy();
            $("td").off("dblclick").on("dblclick", function() { $($(this).closest("tr")).treegrid("toggle"); });
        }
    };
    option = $.extend({},defaultOption, option);
    $table.iwbTable(option);
    $(".btn-toolbar").find(".btn[data-type]").each(function (i, e) {
        var btnType = $(e).data("type").replace("_", "");
        
        if (btnType !== "btnRefresh") {
            var btnUrl = $(e).data("url") || "";
            var btnName = $(e).text();
            var btnClass = $(e).attr("class");
            var btnIcon = $(e).find("i").attr("class");
            actions.push({ type: btnType, name: btnName, "class": btnClass, icon: btnIcon, url: btnUrl });
            $(e).remove();
        } else {
            $(e).prop("disabled", false).attr("onclick", "Refresh('" + $(e).data("url") + "')");
        }
    });
    $(".btn-toolbar").css("display", "block");
    var funs = GetFuns($table);
    var getCustomDataStr = function (row) {
        var str = "";
        if (option.customDataField && option.customDataField.length > 0) {
            for (var i = 0; i < option.customDataField.length; i++) {
                var key = option.customDataField[i];
                str += ',"' + key + '":"' + row[key] + '"';
            }
        }
        return str;
    };
    funs['btnCreate'] = function(url, id) {
        var row;
        if (typeof id === "string" && id !== "") {
            row = $table.bootstrapTable("getRowByUniqueId", id);
        }
        if (row) {
            var dataStr = '{"' + option.parentIdField + '":"' + row[option.idField] + '","' + option.level + '":"' + (row[option.level] + 1) + '"' + getCustomDataStr(row)+'}';
            console.log('create', dataStr);
            BtnCreate({
                table: $table,
                url: url,
                data: JSON.parse(dataStr),
                disabled: option.parentIdField
            });
        } else
            abp.message.warn(abp.localization.iwbZero('SelectRecordOperation'));
    };

    funs['btnUpdate'] = function(url, id) {
        console.log('update',id);
        var row;
        if (typeof id === "string" && id !== "") {
            row = $table.bootstrapTable("getRowByUniqueId", id);
        }
        if (row) {
            BtnUpdate({
                table: $table,
                url: url,
                data: row,
                disabled: option.parentIdField + ',' + option.idField
            }, row);
        } else
            abp.message.warn(abp.localization.iwbZero('SelectRecordOperation'));
    };

    funs['btnDelete'] = function(url, id) {
        console.log('delete', id);
        var row;
        if (typeof id === "string" && id !== "") {
            row = $table.bootstrapTable("getRowByUniqueId", id);
        }
        if (row) {
            BtnDelete({
                table: $table,
                url: url,
                data: row
            }, row);
        } else
            abp.message.warn(abp.localization.iwbZero('SelectRecordOperation'));
    };

    funs['btnMoveUp'] = function(url, id, that) {
        console.log("MoveUp", id);
        var row;
        if (typeof id === "string" && id !== "") {
            row = $table.bootstrapTable("getRowByUniqueId", id);
        }
        if (row) {
            var tr = $(that).closest("tr");
            var funId = tr.data("uniqueid"), parentNo = tr.data("parent");
            var prevs = tr.prevAll().filter("[data-parent='" + parentNo + "']");
            if (prevs.length > 0) {
                var prevId = prevs.first().data("uniqueid");
                //console.log(prevId);
                $.iwbAjax1({ url: url, data: { Id: funId, PrevId: prevId } });
            } else {
                abp.message.warn(abp.localization.iwbZero("RecordAtTop"));
            }
        } else {
            abp.message.warn(abp.localization.iwbZero('SelectRecordOperation'));
        }
    };

    funs['btnMoveDown'] = function(url, id, that) {
        console.log("MoveDown", id);
        var row;
        if (typeof id === "string" && id !== "") {
            row = $table.bootstrapTable("getRowByUniqueId", id);
        }
        if (row) {
            var tr = $(that).closest("tr");
            var funId = $(tr).data("uniqueid"), parentNo = $(tr).data("parent");
            var nexts = tr.nextAll().filter("[data-parent='" + parentNo + "']");
            if (nexts.length > 0) {
                var nextId = nexts.first().data("uniqueid");
                //console.log(nextId);
                $.iwbAjax1({ url: url, data: { Id: funId, NextId: nextId } });
            } else {
                abp.message.warn(abp.localization.iwbZero("RecordAtBottom"));
            }
        } else {
            abp.message.warn(abp.localization.iwbZero("SelectRecordOperation"));
        }
    };

    funs['Refresh'] = function(url) {
        console.log("Refresh");
        $.iwbAjax1({ url: url});
    };

    
    return funs;
}

function RefreshTable(ele, isForce) {
    ele = ele || "table";
    isForce = isForce === undefined;
    var $table = typeof ele === 'string' ? $('#' + ele) : $(ele);
    if ($table.length < 1) {
        console.log('没有发现表格:', ele);
    }
    try {
        $table.iwbTable('refresh', isForce);
    } catch (e) {
        console.log("RefreshTable", e);
    }
}
function BtnCreate(option) {
    option = option || {};
    option.table = option.table ? option.table : "table";
    var $table = typeof option.table === 'string' ? $('#' + option.table) : $(option.table);
    var url = option.url || $table.find('.btn-toolbar').find('.btn[data-type="_btnCreate"]').data('url') ||  $table.find('.btn-toolbar').find('.btn[data-type="btnCreate"]').data('url');
    option.type = 'post';

    $table.iwbTable('defaultCreate', url, option);
}
function BtnUpdate(option, row) {
    option = option || {};
    option.table = option.table ? option.table : "table";
    var $table = typeof option.table === 'string' ? $('#' + option.table) : $(option.table);
    var url = option.url ||  $table.find('.btn-toolbar').find('.btn[data-type="btnUpdate"]').data('url');
    row = row || option.row || $table.bootstrapTable("getSelections")[0];
    option.data = option.data || row;
    option.type = 'post';
    $table.iwbTable('defaultUpdate', url, option, row);
}
function BtnDelete(option, row) {
    option = option || {};
    option.table = option.table ? option.table : "table";
    var $table = typeof option.table === 'string' ? $('#' + option.table) : $(option.table);
    var url = option.url ||  $table.find('.btn-toolbar').find('.btn[data-type="btnDelete"]').data('url');
    row = row || option.row || $table.bootstrapTable("getSelections")[0];
    option.type = 'post';
    $table.iwbTable('defaultDelete', url, option, row);
}

function BtnConfirm(message, title, url, rowOrTableId,data) {
    var row;
    if (rowOrTableId) {
        if (typeof rowOrTableId === 'object' && !(rowOrTableId instanceof jQuery)) {
            row = rowOrTableId;
        } else {
            var $table = typeof rowOrTableId === 'string' ? $('#' + rowOrTableId) : $(rowOrTableId);
            row = $table.bootstrapTable("getSelections")[0];
        }
    } else {
        row = $("#table").bootstrapTable("getSelections")[0];
    }
    if (row) {
        data = data || { Id: row.id };
        MsgConfirm(message, title,function () {
                $.iwbAjax({ url: url, data: data, isValidate: false });
            });
    } else
        abp.message.warn(abp.localization.iwbZero('SelectRecordOperation'));
}
function MsgConfirm(message,title,callback,opt) {
    abp.message.confirm(message, title,function (isConfirmed) {
        if (isConfirmed&& callback) {
            callback(opt);
        }
    });
}


function OpenModal(opt) {
    opt = opt || {};
    var ele = opt.modal ? opt.modal:"modal";
    var $modal = typeof ele === 'string' ? $('#' + ele) : $(ele);
    if ($modal.length < 1) {
        console.log('没有发现模态框:', ele);
    }
    try {
        $modal.iwbModal(opt);
    } catch (e) {
        console.log("OpenModal", e);
    }
}
function ShowModal(ele) {
    ele = ele || "modal";
    var $modal = typeof ele === 'string' ? $('#' + ele) : $(ele);
    if ($modal.length < 1) {
        console.log('没有发现模态框:', ele);
    }
    try {
        $modal.iwbModal('show');
    } catch (e) {
        console.log("ShowModal", e);
    }
}

var queryModalTarget = '';
function ShowQueryModal(ele,target) {
    ele = ele || "modal";
    queryModalTarget = !target ? '' : target.indexOf('.') === 0 ? target : target.indexOf('#') === 0 ? target : '#' + target;
    var $modal = typeof ele === 'string' ? $('#' + ele) : $(ele);
    if ($modal.length < 1) {
        console.log('没有发现模态框:', ele);
    }
    try {
        $modal.iwbModal('show2');
    } catch (e) {
        console.log("ShowModal", e);
    }
}


function FileUpload(that, opt) { $(that).iwbFileUpload(opt); }
function FileShow(that, opt) { $(that).iwbFileShow(opt); }

function SaveAjax(url, options) {
    $.iwbAjax(url, options);
}


//function SaveAjax(url, options) {
//    var defaultOption = {
//        async: true,
//        type: "Post",
//        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
//        data: null,
//        dataType: "json",
//        success: function() { RefreshTable() },
//        error: null,
//        isAlert: true,
//        isValidate: true,
//        modal: $("#modal"),
//        form: $("#form"),
//        errorPlacement: function(error, element) {
//            element.after(error);
//            element.focus();
//        },
//        rules: {},
//        blockUI: true
//    };
//    if (!options) {
//        options = url;
//        url = options.url;
//    }
//    if (!options.form && options.modal) {
//        defaultOption.form = options.modal.find("form");
//    }
//    defaultOption = $.extend(defaultOption, options);
    
//    var isValidated = true;
//    if (defaultOption.isValidate) {
//        isValidated = FormValidate(defaultOption);
//    }
		

//    if (isValidated) {
//        //console.log(defaultOption.data)
//        if (defaultOption.isAlert) {
//            var success= defaultOption.success;
//            defaultOption.success = function(res) {
//                abp.message.success(abp.localization.localize("OpSuccess")).done(success(res));
//            };
//        }
//        //var blockUi = defaultOption.blockUi != null
//        //    ? defaultOption.blockUi
//        //    : (defaultOption.modal == undefined || defaultOption.modal == null)
//        //    ? true
//        //    : defaultOption.modal;
//        var ajaxOption = {
//            url: url,
//            async: defaultOption.async,
//            type: defaultOption.type,
//            contentType: defaultOption.contentType,
//            data: defaultOption.data === null ? formUtil.Serialize(defaultOption.form) : defaultOption.data,
//            dataType: defaultOption.dataType,
//            success: defaultOption.success,
//            error: defaultOption.error,
//            blockUI: defaultOption.blockUI,
//            unblockUI: defaultOption.blockUI
//        };
//        abp.ajax(ajaxOption);
//    }
//}

/*ajax全局设置*/
$.ajaxSetup({
    type: 'Post',
    timeout: 1000 * 60 * 2,
    //contentType: "application/x-www-form-urlencoded;charset=utf-8",
    contentType: "application/json",
    dataType: "json",
    error: function () {
        abp.ui.clearBusy();
    },
    complete: function (xmlHttpRequest, textStatus) {
        //console.log("Complete - " + textStatus, xmlHttpRequest);
        abp.ui.clearBusy();
        if (textStatus === "timeout") {
            abp.message.error(abp.localization.iwbZero('OpTimeout'));
            console.log("Complete - 操作超时");
        } else if (textStatus !== "success") {
            console.log("Complete - " + textStatus, xmlHttpRequest.responseJSON);
            if (xmlHttpRequest.responseJSON) {
                //var json = xmlHttpRequest.responseJSON;
            } else if (xmlHttpRequest.responseText === "") {
                abp.message.error(abp.localization.iwbZero('OpServerError')).done(function() {
                    top.location.reload();
                    //top.location.href = "/Account/Login/?ReturnUrl=%2F";
                });
            }
        } else {
            var result = xmlHttpRequest.responseJSON;
            if (result && !result.success && result.error && result.error.message.indexOf('登陆超时') > -1){
                top.location.reload();
            }
            
        }
    }
});

// ReSharper disable once NativeTypePrototypeExtending
String.prototype.format = function(args) {
    var result = this;
    if (arguments.length > 0) {
        var reg;
        if (arguments.length === 1 && typeof args === "object") {
            for (var key in args) {
                if (args.hasOwnProperty(key)) {
                    if (args[key] !== undefined) {
                        reg = new RegExp("({" + key + "})", "g");
                        result = result.replace(reg, args[key]);
                    }
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] !== undefined) {
                    reg = new RegExp("({[" + i + "]})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
};

String.format = function(str) {
    var args = arguments, re = new RegExp("%([1-" + args.length + "])", "g");
    return String(str).replace(re,function($1, $2) {
        return args[$2];
    }
);
};
//调用方法很简单：
//element.innerHTML = String.format('<a href="%1" onclick="alert(\’%2\’);">%3</a>', url, msg, text);

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
// ReSharper disable once NativeTypePrototypeExtending
Date.prototype.format = function(fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) {
        if (o.hasOwnProperty(k))
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1,RegExp.$1.length === 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    }
    return fmt;
};
function DateFormatter(v,r,i,f) {
    if (!v) {
        return "";
    }
    var date = new Date(v);
    var str = date.format('yyyy-MM-dd');
    if (r&&f) {
        r[f] = str;
    }
    return str;
}

function DateTimeFormatter(v,r,i,f) {
    if (!v) {
        return "";
    }
    var date = new Date(v);
    var str = date.format('yyyy-MM-dd hh:mm:ss');
    if (r&&f) {
        r[f] = str;
    }
    return str;
}



$(function () {
    //Configure blockUI
    if ($.blockUI) {
        $.blockUI.defaults.baseZ = 2000;
    }

    $('.iwb-date-time').iwbDate({ language: window.lang});
    $('.iwb-date').iwbDate({ language: window.lang });

});

