var config = {
    table: $("#table"),
    tableTool: $("#tableTool"),
    form: $("#form"),
    modal: $("#modal")
};

/********************** bootstrapTable  Start **/
function LoadTable(url, options) {
    GetSearchList();
    var defaultOption = {
        table: config.table,
        onAll: OnAll,
        onClickRow: OnClickRow,
        onCheck: OnCheck,
        onUncheck:OnUnCheck,
        queryParams: QueryParams,
        //onLoadSuccess: ICheckTableInit_SingleSelect
        onLoadSuccess: OnLoadSuccess,
        onPostBody: OnPostBody,
        responseHandler: ResponseHandler,
    };
	if (!options) {
		options = !url || typeof(url)==="string" ? {} : url;
	} 
	if (options.hasOwnProperty("url")) {
		defaultOption.url = options.url;
	} else if (typeof (url) === "string") {
		defaultOption.url = url;
	}
	$.extend(defaultOption, options);
	
	$.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales[window.lang]);
	defaultOption.table.bootstrapTable(defaultOption );
    return defaultOption.table;
}
function RefreshTable(table) {
    abp.ui.setBusy();
    var $thisTable = config.table;
    if (table) {
        if (typeof (table) === "string") {
            $thisTable = $("#" + table);
        } else {
            $thisTable = $(table);
        }
    }
    $thisTable.bootstrapTable("refresh", { silent: true});
    setTimeout(function () { abp.ui.clearBusy(); _isSearching = false; }, 8 * 1000);
}
function QueryParams(params) { //bootstrapTable自带参数  
    var sorting = '';
    if (params.sort) {
        sorting = params.sort;
        if (params.order) {
            sorting += ' ' + params.order;
        }
    }
	return {
		//limit: params.limit, //页面大小
		//page: (params.offset / params.limit) + 1, //页码
		MaxResultCount: params.limit,
		SkipCount: params.offset, 
        Sorting: sorting, //排序列名  
		sortOrder: params.order, //排位命令（desc，asc） 
		SearchList: _searchList
	};
}
function ResponseHandler(res) {
	if (res.success) {
        var data = JSON.parse('{"total":' + res.result.totalCount + ',"rows":' + JSON.stringify(res.result.items) + '}');
		console.log(data);
		return data;
    } else {
	    console.log("Table load failed");
	    if (res.error) {
            if (res.error.details) {
                return abp.message.error(res.error.details, res.error.message);
            } else {
                if (error.message && error.message.indexOf("登陆超时") >= 0) {
                    return abp.message.error(error.message).done(function () {
                        top.location.reload();
                    });
                } else {
                    return abp.message.error(error.message || abp.ajax.defaultError.message);
                }
            }
	    }
	}
	return JSON.parse('{"total":0,"rows":[]}');
}
function ResponseHandlerNoPage(res) {
    if (res.success) {
        var data = res.result;
        //console.log("data:::"+data);
        return data;
    } else {
        console.log("Table load failed");
        if (res.error) {
            if (res.error.details) {
                return abp.message.error(res.error.details, res.error.message);
            } else {
                if (error.message && error.message.indexOf("登陆超时") >= 0) {
                    return abp.message.error(error.message).done(function () {
                        top.location.reload();
                    });
                } else {
                    return abp.message.error(error.message || abp.ajax.defaultError.message);
                }
            }
        }
    }
    return JSON.parse('[]');
}
function OnAll(eName, eData, table) {
    //console.log(eName, eData);
    var $table = config.table;
    if (table) {
        if (typeof (table) === "string") {
            $table = $("#" + table);
        } else {
            $table = $(table);
        }
    }
    _isSearching = false;
    $table.closest(".table-box").find("#tableTool .btn[data-type^='btn']")
		.prop('disabled', $table.bootstrapTable('getSelections').length !== 1);
    $table.closest(".table-box").find("#tableTool .btn[data-type^='a_btn']")
		.prop('disabled', $table.bootstrapTable('getSelections').length === 0);
}
function OnLoadSuccess(data,table) {
    var $table = config.table;
    if (table) {
        if (typeof (table) === "string") {
            $table = $("#" + table);
        } else {
            $table = $(table);
        }
    }
    $table.find(".bs-checkbox").find("input").addClass("filled-in").after("<label></label");
    $table.find("thead th.bs-checkbox").off("click.checkOnTable").on(
        "click.checkOnTable",
        function () {
            $(this).find("input").click();
        });
    _isSearching = false;
    abp.ui.clearBusy();
    console.log('endTime:' + new Date());
}

function OnPostBody(data,table) {
    var $table = config.table;
    if (table) {
        if (typeof (table) === "string") {
            $table = $("#" + table);
        } else {
            $table = $(table);
        }
    }
    $table.find('.bootstrap-table tr th').each(function() { $(this).css("text-align", "center") });
    $('.bootstrap-table tr td').each(function() {
        var text = $(this).text();
        $(this).attr("title", text);
        //var width = $(this).width(); 
        $(this).css({
            //"max-width": width,
            "overflow": "hidden",
            "text-overflow": "ellipsis",
            "white-space": "nowrap"
        });
    });
}
function OnClickRow(row, $element, field) {

}
function OnCheck(row, $element) {

}
function OnUnCheck(row, $element) {

}
//function UpperJsonKey(jsonObj) {
//    if (jsonObj===undefined || jsonObj===null) {
//        return [];
//    }
//    if (jsonObj.length !== undefined) {
       
//        for (var i = 0; i < jsonObj.length; i++) {
//            UpperJsonKeyChild(jsonObj[i]);
//            //for (var key in jsonObj[i]) {
//                //if (jsonObj[i].hasOwnProperty(key)) {
//                //    jsonObj[i][key.substring(0, 1).toUpperCase() + key.substring(1)] = jsonObj[i][key];
//                //    delete (jsonObj[i][key]);
//                //}
                
//            //}
//        }
//    } else {
//        UpperJsonKeyChild(jsonObj);
//        //for (var k in jsonObj) {
//        //    if (jsonObj.hasOwnProperty(k)) {
//        //        jsonObj[k.substring(0, 1).toUpperCase() + k.substring(1)] = jsonObj[k];
//        //        delete (jsonObj[k]);
//        //    }
//        //}
//    }
	
//	return jsonObj;
//}
//function UpperJsonKeyChild(jsonObj) {
//    for (var k in jsonObj) {
//        if (jsonObj.hasOwnProperty(k)) {
//            var obj = jsonObj[k];
//            if (typeof (obj) === "object" && (Object.prototype.toString.call(obj).toLowerCase() === "[object object]" || Object.prototype.toString.call(obj).toLowerCase() === '[object array]')) {
//                jsonObj[k.substring(0, 1).toUpperCase() + k.substring(1)] = UpperJsonKey(obj);
//                delete (jsonObj[k]);
//            } else {
//                jsonObj[k.substring(0, 1).toUpperCase() + k.substring(1)] = jsonObj[k];
//                delete (jsonObj[k]);
//            }
//        }
//    }
//    return jsonObj;
//}

/** bootstrapTable END **/

/********************** DataCurd Start **/
var funs = {
	btnCreate: function (url) { BtnCreate(url); },
	btnUpdate: function (url) { BtnUpdate(url); },
	btnDelete: function (url) { BtnDelete(url); },
	btnSearch: function () { BtnSearch(); },
	none: function () {console.log("No type");}
};
config.tableTool.find('.btn').on('click', function () {
	var type = $(this).data('type').replace("a_","").replace("_","");
	var url = $(this).data('url') || "";
	funs[type] ? funs[type].call(this, url) : funs["none"].call(this);
});
function BtnCreate(url, options) {
	console.log("Add");
    var defaultOption = {
        modal: config.modal,
        modaltitle: window.opCreate,
        url: url,
        readonly: "",
        disabled: "",
        select2: true,
        select2tree: false,
        save: null,
        table: null
    };
    if (!options) {
        options = !url || typeof (url) === "string" ? {} : url;
    }
    if (options.hasOwnProperty("url")) {
        defaultOption.url = options.url;
    } else if (typeof (url) === "string") {
        defaultOption.url = url;
    } else {
        defaultOption.url = config.tableTool.find('.btn[data-type=_btnCreate]').data('url');
    }
    $.extend(defaultOption, options);
    OpenModal(defaultOption);
}
function BtnUpdate(url, options) {
    console.log("Update");
    var $table = config.table;
    if (!options) {
        options = !url || typeof (url) === "string" ? {} : url;
    }
    if (options.table) {
        $table = options.table;
    }
    if (typeof ($table) === "string") {
        $table = $("#" + $table);
    } else {
        $table = $($table);
    }
	var rows = $table.bootstrapTable("getSelections");
	if (rows.length === 1) {
		var defaultOption = {
			modal: config.modal,
			modalContent: "",
			modaltitle: window.opUpdate,
			data: rows[0],
			savebtn: null,
			form: null,
			readonly: "",
			disabled: "",
            url: "",
			select2: true,
            select2tree: false,
            save: null,
            table: null,
            isValidate: true
		};
		
		if (options.hasOwnProperty("url")) {
			defaultOption.url = options.url;
		} else if (typeof (url) === "string") {
			defaultOption.url = url;
		} else {
			defaultOption.url = config.tableTool.find('.btn[data-type=btnUpdate]').data('url');
		}
		$.extend(defaultOption, options);
		OpenModal(defaultOption);
	} else
		abp.message.warn("选择一条记录操作！");
}
function BtnDelete(url,table) {
    console.log("Delete");
    var $table = config.table;
    if (table) {
        if (typeof (table) === "string") {
            $table = $("#" + table);
        } else {
            $table = $(table);
        }
    }
    var rows = $table.bootstrapTable("getSelections");
	if (rows.length === 1) {
		abp.message.confirm(abp.localization.localize("DeleteConfirmMsg"), abp.localization.localize("DeleteConfirmTitle"),function() {
            SaveAjax({ url: url, data: { Id: rows[0].id }, isValidate: false, table: $table});
		});
	} else
		abp.message.warn("选择一条记录操作！");
}

var _isSearching = false;
var _searchList = [];
function BtnSearch(isCheck) {
    clearTimeout(t);
    if (_isSearching) {
        return;
    }
    _isSearching = true;
    GetSearchList();
    if (!isCheck&&_searchList.length <= 0) {
        console.log("Search-Multi-None");
        return;
    }
    var isValidated = FormValidate({ form: $("#SearchForm") });
    if (isValidated) {
        RefreshTable();
        console.log("Search");
    } else {
        console.log("Search_Validate_Faild");
    }
    
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
            SaveAjax({ url: url, data: data, isValidate: false });
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

function GetSearchList() {
    var count = $("#SearchForm").find(".KeyWords").length;
    _searchList = [];
    for (var i = 1; i <= count; i++) {
        var keyWords = $("#KeyWords-" + i).val();
        if (keyWords) {
            var keyField = $("#KeyField-" + i).val();
            var fieldType = $("#FieldType-" + i).val();
            var expType = $("#ExpType-" + i).val();
            _searchList.push({ KeyWords: keyWords, KeyField: keyField, FieldType: fieldType, ExpType: expType });
        }
    }
}
/** DataCurd END **/

/************************ Modal、Form  Start **/
function ShowModal(modal) {
    var $modal = typeof (modal) === "string" ? $("#" + modal) : $(modal);
    $modal.off('shown.bs.modal').on('shown.bs.modal', function () {
        $(this).css('display', 'block');
        var topHeight = $(window).height() - $(this).find('.modal-dialog').height() - 50;
        if (topHeight < 30) {
            topHeight = 30;
        }
        $(this).find('.modal-dialog').animate({ 'marginTop': topHeight / 2 + "px" });
    });
    $modal.modal("show", { keyboard: true});
}

var targetDom="";
function ShowQueryModal(modal, pcTargetDom) {
    targetDom = !pcTargetDom ? "" : pcTargetDom.indexOf('.') === 0 ? pcTargetDom : pcTargetDom.indexOf('#') === 0 ? pcTargetDom : '#' + pcTargetDom;
    ShowModal(modal);
}
function OpenModal(url,options) {
    var defaultOption =
    {
        modal: config.modal,
        modalContent: "",
        modaltitle: window.opCreate,
        modalShown:null,
        data: null,
        savebtn: null,
        form: null,
        errorPlacement: function (error, element) {
            if (element.is("select")) {
                if (!element.next().next().hasClass("error")) {
                    element.next().after(error);
                    element.focus();
                }
                

            } else  {
                element.after(error);
                element.focus();
            }
        },
        rules: {},
        readonly: "",
        disabled: "",
        success: null,
        select2: true,
        select2tree: false,
        save: null,
        table: null,
        isValidate:true
    };
	if (!options) {
		options = !url || typeof (url) === "string" ? {} : url;
	}
	if (options.hasOwnProperty("url")) {
		defaultOption.url = options.url;
	} else if (typeof (url) === "string") {
		defaultOption.url = url;
	} else {
		defaultOption.url = config.tableTool.find('.btn[data-type=btnUpdate]').data('url');
	}
	$.extend(defaultOption, options);
	if (defaultOption.form === null || defaultOption.form.length <= 0 ) {
		defaultOption.form = defaultOption.modal.find("form");
	}
	if (defaultOption.savebtn === null || defaultOption.savebtn.length <= 0) {
		defaultOption.savebtn = defaultOption.modal.find("button.save-btn");
		if (defaultOption.savebtn.length <= 0) {
			defaultOption.savebtn = defaultOption.modal.find("#Save");
		}
	}
	
	var $modal = defaultOption.modal;
    $modal.off('show.bs.modal').on('show.bs.modal', function () {
        if (defaultOption.modalContent === "") {
            if (defaultOption.modalShown && typeof (defaultOption.modalShown) === "function") {
                defaultOption.modalShown();
            }
			$modal.find("input").val("").prop("readonly", false).prop("disabled", false).removeClass("error valid");
		    $modal.find("select").val("").prop("readonly", false).prop("disabled", false).removeClass("error valid");
		    $modal.find("textarea").val("").prop("readonly", false).prop("disabled", false).removeClass("error valid");
			$modal.find("label.error").remove();
			$modal.find(".modal-title-span").html(defaultOption.modaltitle);
			if (defaultOption.readonly !== "") {
				var readOnly = defaultOption.readonly.split(',');
				for (var i = 0; i < readOnly.length; i++) {
				    $modal.find('#' + readOnly[i]).prop("readonly", true);
				}
			}
			if (defaultOption.disabled !== "") {
				var disabled = defaultOption.disabled.split(',');
				for (var j = 0; j < disabled.length; j++) {
				    $modal.find('#' + disabled[j]).prop("disabled", true);
				}
            }
            if (defaultOption.select2) {
                $modal.find("select").select2();
                if (defaultOption.select2tree) {
                    $modal.find('#' +defaultOption.select2tree).select2tree();
                } 
		    }
			if (defaultOption.data !== null)
                formUtil.Deserialize($modal, defaultOption.data);
		    $modal.find('input[type="file"]').each(function(index, item) {
                ClearFile($(item).attr("id"));
		       
		    });
            $modal.find('input:not([type="hidden"]):not([type="disabled"]):first').focus();
            Draggable($modal);
            //: not([readonly]): not([disabled])
        } else {
			$modal.find(".modal-body").empty().append(defaultOption.modalContent);
        }
        $('select').off("change.ff").on("change.ff",  function (e) {
            $(this).focus();
	        $(this).next(".error").remove();
            $(this).blur();
        });

        
	    //defaultOption.form.validate({
	    //    errorPlacement: defaultOption.errorPlacement,
     //       rules: defaultOption.rules
     //   }).settings.ignore = ":disabled";
        if (defaultOption.url !== "")
            if (defaultOption.save && typeof (defaultOption.save) ==="function") {
                defaultOption.savebtn.off("click.save").on("click.save", function () {
                    //$(this).prop('disabled', true);
                    //setTimeout(() => { $(this).prop('disabled', false); }, 2000);
                    TimeOutDisableDom($(this));
                    defaultOption.save({
                    url: defaultOption.url,
                    success: defaultOption.success === null ? function () { $modal.modal("hide"); RefreshTable(defaultOption.table); } : defaultOption.success,
                    form: defaultOption.form
                })});
            } else {
                defaultOption.savebtn.off("click.save").on("click.save", function () {
                    //$(this).prop('disabled', true);
                    //setTimeout(() => { $(this).prop('disabled', false); }, 2000);
                    TimeOutDisableDom($(this));
                    SaveAjax({
                        url: defaultOption.url,
                        isValidate: defaultOption.isValidate,
                        success: defaultOption.success === null ? function () { $modal.modal("hide"); RefreshTable(defaultOption.table); } : defaultOption.success,
                        form: defaultOption.form
                    });
                });
            }
        
    });
    $modal.off('shown.bs.modal').on('shown.bs.modal', function() {
        $(this).css('display', 'block');
        var topHeight = $(window).height() - $(this).find('.modal-dialog').height() - 50;
        if (topHeight < 30) {
            topHeight = 30;
        }
        $(this).find('.modal-dialog').animate({ 'marginTop': topHeight / 2 + "px" });  
    });
    $modal.modal("show", { keyboard: true});

}

function Draggable($modal) {
    //var $that = this;
    //var $modal = $that.getModal();
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
        function(e) {
            //webkit内核和火狐禁止文字被选中
            $('body').addClass('drag-select');
            //ie浏览器禁止文字选中
            document.body.onselectstart = document.body.ondrag = function() {
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
        function() {
            dragModal.mouseDragDown = false;
            dragModal.moveTarget = undefined;
            dragModal.mouseStartPoint = { 'left': 0, 'top': 0 };
            dragModal.basePoint = { 'left': 0, 'top': 0 };
        });
    $modal.off('mousemove.drag').on('mousemove.drag',
        function(e) {
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
        function() {
            $('.modal-dialog').css({ 'top': '0', 'left': '0' });
            $('body').removeClass('drag-select');
            document.body.onselectstart = document.body.ondrag = null;
        });

};
var FormConvert = function() {};
FormConvert.prototype = {
	Deserialize: function (body, array) {
		var input = body.find("input");
		var tel = body.find("input[type='tel']");
		var email = body.find("input[type='email']");
		var hidden = body.find("input[type='hidden']");
		var textarea = body.find("input[type='textarea']");
		var textarea2 = body.find("textarea");
		var select = body.find("select");
		var checkbox = body.find("input[type='checkbox']");
		var radio = body.find("input[type='radio']");
		$.merge(input, tel);
		$.merge(input, email);
		//$.merge(inputArray, checkbox);
		//$.merge(inputArray, radio);
		$.merge(input, hidden);
		$.merge(input, textarea);
		$.merge(input, textarea2);
		$.merge(input, select);
		//console.log(checkbox);
		//console.log(radio);
		input.each(function () {
			var input = $(this);
			//var name = input.attr("name").replace(/(\w)/, function (v) { return v.toUpperCase() });
			var name = input.attr("name");
			if (array[name] !== "") {
				input.val(array[name]);
			}
		});
		select.each(function () {
			var input = $(this);
			//var name = input.attr("name").replace(/(\w)/, function (v) { return v.toUpperCase() });
			var name = input.attr("name");
			if (typeof array[name] === "boolean") {
				array[name] = array[name] + "";
			}
			//console.log(name, array[name]);
			if (array[name] !== "") {
				//input.val(array[name]);
				//seleect2 赋值
				input.val(array[name]).trigger('change');
			}
		});
		checkbox.each(function () {
			var input = $(this);
			//var name = input.attr("name").replace(/(\w)/, function (v) { return v.toUpperCase() });
			var name = input.attr("name");
			if (array[name] !== "") {
				//console.log(array[name]);
				//console.log("---");
				input.val(array[name] === "True" || input.val(array[name]) === "1" || input.val(array[name]) === "true");
			}
		});
		radio.each(function () {
			var input = $(this);
			//var name = input.attr("name").replace(/(\w)/, function (v) { return v.toUpperCase() });
			var name = input.attr("name");
			if (array[name] !== "") {
				//console.log(array[name]);
				$("input[name='" + name + "'][value='" + array[name] + "']").prop("checked", true);
				$("input[name='" + name + "'][value!='" + array[name] + "']").prop("checked",false);
			}
		});
	},
	//将form表单元素的值序列化成对象
	Serialize: function (form) {
		var disableEle = form.find("[disabled]");
		disableEle.each(function(i, e) {
			$(e).prop("disabled", false);
		});
		var o = {};
		$.each(form.serializeArray(), function () {
			if (o[this['name']]) {
				o[this['name']] = o[this['name']] + "," + this['value'];
			} else {
				o[this['name']] = this['value'];
			}
		});
		disableEle.each(function (i, e) {
			$(e).prop("disabled", true);
		});
		return o;
	}
}
var formUtil = new FormConvert();
/** Modal、Form  END **/


/************************ Form Submit validate  Start **/
function SaveAjax(url, options) {
    var defaultOption = {
        async: true,
        type: "Post",
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: null,
        dataType: "json",
        success: null,
        error: null,
        isAlert: true,
        isValidate: true,
        modal: config.modal,
        form: config.form,
        errorPlacement: function(error, element) {
            element.after(error);
            element.focus();
        },
        rules: {},
        blockUI: true,
        table:  config.table
};
	if (!options) {
		options = url;
		url = options.url;
    }
    if (!options.form && options.modal) {
        defaultOption.form = options.modal.find("form");
    }
    defaultOption = $.extend(defaultOption, options);
    
    var isValidated = true;
    if (defaultOption.isValidate) {
        isValidated = FormValidate(defaultOption);
    }
		

	if (isValidated) {
		//console.log(defaultOption.data)
        defaultOption.success =defaultOption.success?defaultOption.success : function() {
            RefreshTable(defaultOption.table);
            defaultOption.modal.modal('hide');
        };
        if (defaultOption.isAlert) {
            var success= defaultOption.success;
            defaultOption.success = function(res) {
                abp.message.success(abp.localization.localize("OpSuccess")).done(success(res));
            };
        }
	    //var blockUi = defaultOption.blockUi != null
	    //    ? defaultOption.blockUi
	    //    : (defaultOption.modal == undefined || defaultOption.modal == null)
	    //    ? true
	    //    : defaultOption.modal;
		var ajaxOption = {
			url: url,
			async: defaultOption.async,
			type: defaultOption.type,
			contentType: defaultOption.contentType,
			data: defaultOption.data === null ? formUtil.Serialize(defaultOption.form) : defaultOption.data,
			dataType: defaultOption.dataType,
			success: defaultOption.success,
            error: defaultOption.error,
            blockUI: defaultOption.blockUI,
            unblockUI: defaultOption.blockUI
		};
		abp.ajax(ajaxOption);
	}
}

function FormValidate() {
    var defaultOption = {
        form: $("#form"),
        errorPlacement: function(error, element) {
            element.after(error);
            element.focus();
        },
        rules: {}
    };
    $.extend(defaultOption, arguments[0]);
    var form = defaultOption.form;
    form.validate({
        errorPlacement: defaultOption.errorPlacement,
        rules: defaultOption.rules
    }).settings.ignore = ":disabled";
    return form.valid();
}
function TimeOutDisableDom(that) {
    $(that).prop('disabled', true);
    setTimeout(() => { $(that).prop('disabled', false); }, 2000);
}

/******************************************** Form validate END **/



/************************ Common script  Start **/

$.ajaxSetup({
    type: 'Post',
    timeout: 1000 * 60 * 2,
    contentType: "application/x-www-form-urlencoded;charset=utf-8",
    dataType: "json",
    //beforeSend:function() {
    //    var blockUi =
    //        ' <div id="beforeBlockUi" style="position: absolute; top: 0; left: 0; bottom: 0; right: 0;background:#ccc;"></div>';
    //    $('body').append(blockUi);
    //},

    complete: function (xmlHttpRequest, textStatus) {
        //console.log("Complete - " + textStatus, xmlHttpRequest);
       
       // $("#beforeBlockUi").remove();
        if (textStatus === "timeout") {
            abp.message.error("服务器响应超时，请稍后再试。");
            console.log("Complete - 操作超时");
        } else if(textStatus !== "success") {
            console.log("Complete - " + textStatus, xmlHttpRequest.responseJSON);
            if (xmlHttpRequest.responseJSON) {
                var json = xmlHttpRequest.responseJSON;
                //if (!json.Success && !json.UnAuthorizedRequest && json.Error.Message.indexOf("登陆超时")>=0)
                //    abp.message.error(json.Error.Message).done(function () {
                //        //top.location.href = "/Account/Login/?ReturnUrl=%2F";
                //        top.location.reload();
                //    });
            }
            //else if (xmlHttpRequest.responseText === "") {
            //   // top.location.href = "/Account/Login/?ReturnUrl=%2F&1=";
            //        abp.message.error("服务器出错！请稍后重试。。。").done(function () {
            //            top.location.href = "/Account/Login/?ReturnUrl=%2F";
            //        });
            //}
            
        }
       
    }
});


/**
* 在页面中任何嵌套层次的窗口中获取顶层窗口
* @@return 当前页面的顶层窗口对象
**/
function GetTopWinow() {
	var p = window;
	while (p !== p.parent) {
		p = p.parent;
	}
	return p;
}
/**
 * 获取浏览器
 * @returns {} 
 */
function MyBrowser() {
	var userAgent = navigator.userAgent; //取得浏览器的userAgent字符串
	if (userAgent.indexOf("Opera") > -1) {
		return "Opera";
	}; //判断是否Opera浏览器
	if (userAgent.indexOf("Firefox") > -1) {
		return "FF";
	} //判断是否Firefox浏览器
	if (userAgent.indexOf("Chrome") > -1) {
		return "Chrome";
	}
	if (userAgent.indexOf("Safari") > -1) {
		return "Safari";
	} //判断是否Safari浏览器
	if (userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 ) {
		return "IE";
	}; //判断是否IE浏览器
	return "";
}
//以下是调用上面的函数
//var mb = MyBrowser();
//if ("IE" === mb) {
//	alert("我是 IE");
//}
//if ("FF" === mb) {
//	alert("我是 Firefox");
//}
//if ("Chrome" === mb) {
//	alert("我是 Chrome");
//}
//if ("Opera" === mb) {
//	alert("我是 Opera");
//}
//if ("Safari" === mb) {
//	alert("我是 Safari");
//}

/**
	 * 设置未来(全局)的AJAX请求默认选项
	 * 主要设置了AJAX请求遇到Session过期的情况
**/

function BeforeSend() {

}

//获取当前时间
function GetNowFormatDate(time) {
	time = time || false;
	var currentdate;
	var seperator1 = "-";
	var seperator2 = ":";
	var date = new Date();
	var month = date.getMonth() + 1;
	var strDate = date.getDate();
	if (month >= 1 && month <= 9) {
		month = "0" + month;
	}
	if (strDate >= 0 && strDate <= 9) {
		strDate = "0" + strDate;
	}
	if (time) {
		currentdate = date.getFullYear() +
			seperator1 +
			month +
			seperator1 +
			strDate +
			" " +
			date.getHours() +
			seperator2 +
			date.getMinutes() +
			seperator2 +
			date.getSeconds();
	} else {
		currentdate = date.getFullYear() +
			seperator1 +
			month +
			seperator1 +
			strDate;
	}

	return currentdate;
}

function FileUpload(that, opt) { $(that).iwbFileUpload(opt); }
function FileShow(that, opt) { $(that).iwbFileShow(opt); }


//打开选择文件窗口
function OpenUploadWindow(that) {
    //$(that).find(".showFileName").html("");
    $(that).removeClass("file-success").removeClass("file-error");
    //$(that).find("input[type='file']")[0].click();
}

//检查文件
function FileInputCheck(that, idStr, isImage, maxSize, callback,checkFileType) {
    if (CheckFileSize($(that).attr("id"), maxSize)) {
       
        //var filePath = $(that).val();
        var fileName = $(that).val().split("\\").pop();
        //$(that).closest('.upload').addClass("file-success").find(".showFileName").html(fileName);
        $(that).closest('.custom-file ').addClass("file-success").find("label").text(fileName);
        var file = document.getElementById($(that).attr("id")).files[0];
        if (checkFileType && typeof (checkFileType) === "function") {
            checkFileType(fileName);
        }
        if (idStr) {
            ReadFile(file, idStr, isImage, checkFileType);
        }
        if (callback && typeof (callback) === "function") {
            callback(fileName, file);
        }
    } else {
        // $(that).closest('.upload').addClass("file-error");
        $(that).closest('.custom-file ').addClass("file-error");
    }
}
function ReadFile(file, idStr, isImage) {
    //判断是否是图片类型
    //console.log(file.type);
    if (isImage && !/image\/\w+/.test(file.type)) {
        abp.message.warn("只能选择图片");
        return false;
    }
    
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function (e) {
        //txshow.src = this.result;
        //console.log(this.result);
        $("#" + idStr).val(this.result.substring(this.result.indexOf(',') + 1));
    };
}

//清空文件域
function ClearFile(idStr) {
    var file = $("#" + idStr);
    file.closest('.upload').removeClass("file-success").removeClass("file-error").find(".showFileName").html("");
    file.after(file.clone().val(""));
    file.remove();
}
//检查文件大小

function CheckFileSize(idStr, maxSize) {
    maxSize = maxSize || 2;
    var maxsize = maxSize * 1024 * 1024;
    var errMsg = "上传的附件文件不能超过" + maxSize + "M。";
    var tipMsg = "您的浏览器暂不支持计算上传文件的大小，确保上传文件不要超过2M，建议使用IE、FireFox、Chrome浏览器。";
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
        var objFile = document.getElementById(idStr);
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
            ClearFile(idStr);
            abp.message.warn(tipMsg);
            return false;
        }
        if (filesize === -1) {
            ClearFile(idStr);
            abp.message.warn(tipMsg);
            return false;
        } else if (filesize > maxsize) {
            ClearFile(idStr);
            abp.message.warn(errMsg);
            return false;
        } else {


            return true;
        }
    } catch (e) {
        ClearFile(idStr);
        abp.message.error(e);
    }
}  
//打开图片或文件
function OpenFile(that, type, defaultOption) {
    var url;
    if (typeof (that) === "string") {
        url = that;
    } else {
        url = $(that).data("url");
        if (!url) {
            url = $(that).attr("src");
        }
        type = $(that).data("type");
    }
    if (url.indexOf("/")!==0) {
        url = "/" + url;
    }
    console.log(url);
    if (type === 'img') {
        if ($("#Img-Modal").length > 0) {
            $("#Img-Modal").remove();
        }
        $("body").append(' <ul id="Img-Modal"><li><img src="' + url + '" data-original="' + url + '" alt="Picture" /></li></ul>');
        var $image = $('#Img-Modal');
        /*$image.find("img").error(function () {
            $(this).attr("src", "/Content/images/no-pic.png");
            $(this).attr("data-original", "/Content/images/no-pic.png");
        });*/
        var options = {
            // inline: true,
            url: 'data-original',
            button: true,
            navbar: false,
            title: false,
            toolbar: false,
            ready: function (e) {
                console.log(e.type);
            },
            //show: function (e) {
            //    console.log(e.type);
            //},
            //shown: function (e) {
            //    console.log(e.type);
            //},
            //hide: function (e) {
            //    console.log(e.type);
            //},
            //hidden: function (e) {
            //    console.log(e.type);
            //},
            //view: function (e) {
            //    console.log(e.type);
            //},
            //viewed: function (e) {
            //    console.log(e.type);
            //}
        };
        $image.on({
            //ready: function (e) {
            //    console.log(e.type);
            //},
            show: function (e) {
                console.log(e.type);
            },
            //shown: function (e) {
            //    console.log(e.type);
            //},
            //hide: function (e) {
            //    console.log(e.type);
            //},
            //hidden: function (e) {
            //    console.log(e.type);
            //},
            //view: function (e) {
            //    console.log(e.type);
            //},
            //viewed: function (e) {
            //    console.log(e.type);
            //}
        }).viewer(options);
        $image.viewer("show");
        $image.css("display","none");
    } else {
        FileOpen(url, type, defaultOption);
        //if ($("#File-Modal").length <= 0) {
        //    $("body").append(
        //        '    <section><div class="modal fade" id="File-Modal" role="dialog" tabindex="-1" aria-labelledby="ModalLabel"aria-hidden="true"><div class="modal-dialog  modal-dialog-centered" role="document" style="width: calc(100% - 300px)") ;><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true"><span aria-hidden="true">&times;</span></button><h4 class="modal-title">文件预览</h4></div><div class="modal-body"  style="min-height: 500px; overflow: auto"></div><div class="modal-footer" style="text-align: center;"><button type="button" class="btn btn-success waves-effect" data-dismiss="modal" style="min-width: 100px;background: #F0985D;border: 1px solid #DDDDDD;">关闭窗口</button></div></div></div></div></section>');
        // }
        //if (type === "office") {
        //    url = 'https://view.officeapps.live.com/op/view.aspx?src=http://' + document.domain + '/' + url;
        //} else {
        //    url = '/' + url;
        //}
        //console.log(url);
        //$("#File-Modal").find('.modal-body')
        //    .html('<iframe src="' + url + '" width="100%"; style="min-height:500px;" frameborder="0"></iframe>');
        //$("#File-Modal").modal("show");
    }
   
}
function FileOpen(url, type, options) {
    var defaultOption = {
        gapWidth: 300,
        height:500
    };
    $.extend(defaultOption, options);
    if ($("#File-Modal").length <= 0) {
        $("body").append(
            '    <section><div class="modal fade" id="File-Modal" role="dialog" tabindex="-1" aria-labelledby="ModalLabel"aria-hidden="true"><div class="modal-dialog  modal-dialog-centered" role="document" style="width: calc(100% - ' + defaultOption.gapWidth + 'px)") ;><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true"><span aria-hidden="true">&times;</span></button><h4 class="modal-title">文件预览</h4></div><div class="modal-body"  style="min-height: '+ defaultOption.height+'px; overflow: auto"></div><div class="modal-footer" style="text-align: center;"><button type="button" class="btn btn-success waves-effect" data-dismiss="modal" style="min-width: 100px;background: #F0985D;border: 1px solid #DDDDDD;">关闭窗口</button><button id="download" type="button" class="btn btn-success waves-effect"  style="min-width: 100px;background-color: #678ceb;border: 1px solid #678ceb;border: 1px solid #DDDDDD;" data-url="" onclick="DownloadFile(this)">下载文件</button></div></div></div></div></section>');
    }
    //url = '/' + url;
    if (url.indexOf("/") !== 0) {
        url = "/" + url;
    }
    var url2;
    if (type === "office") {
        // url = 'https://view.officeapps.live.com/op/view.aspx?src=http://' + document.domain + '/' + url;
        url2 = 'http://ow365.cn/?i=17314&furl=http://' + location.host + url;
    } else {
        url2 = url;
    }
    console.log(url2);
    $("#File-Modal").find('.modal-body')
        .html('<iframe src="' + url2 + '" width="100%"; style="min-height:' + defaultOption.height+'px;" frameborder="0"></iframe>');
    $("#File-Modal").find('#download').data("url", url);
    $("#File-Modal").modal("show");
}

function DownloadFile(that) {
    var url = $(that).data("url");
    var fileName = $(that).val().split("/").pop();
    $("body").append('<a id="tempLink" href="' + url + '" download="' + fileName+'"></a>');
    $("#tempLink")[0].click();
    $("#tempLink").remove();
}
function DownLoadFilePath(that) {
    if (typeof (that) === "string") {
        var fileName = that.split("/").pop();
        $("body").append('<a id="tempLink" href="' + that + '" download="' + fileName + '"></a>');
        $("#tempLink")[0].click();
        $("#tempLink").remove();
    }
}
var officeFileExt = ['doc', 'docx', 'xls', 'xlsx', 'xlsm', 'ppt', 'pptx'];
/** Common script  END **/
var t = 0;
$(function () {
    $('.iwb-date-time').iwbDate({ language: window.lang});
    $('.iwb-date').iwbDate({ language: window.lang });
    
    $('.modal').on('show.bs.modal', function () {
        // 关键代码，如没将modal设置为 block，则$modala_dialog.height() 为零
        $(this).css('display', 'block');
        var topHeight = $(window).height() - $(this).find('.modal-dialog').height() - 50;
        if (topHeight < 30) {
            topHeight = 30;
        }
        $(this).find('.modal-dialog').animate({ 'marginTop': topHeight / 2 + "px" });
        Draggable($(this));
    });
    document.addEventListener("error", function (e) {
        var elem = e.target;
        if (elem.tagName.toLowerCase() === 'img') {
            //elem.src = "/Content/images/no-pic.png";
            console.log($(elem));
            $(elem).attr("src", "/Content/images/no-pic.png");
        }
    }, true /*指定事件处理函数在捕获阶段执行*/);
    AddSearchEvents();

    // 身份证号码验证 
    jQuery.validator.addMethod("isIdCardNo", function(value, element) {
        return this.optional(element) || idCardNoUtil.checkIdCardNo(value);//调用验证的方法
    }, "请正确填写身份证号码");
    document.addEventListener("error", function (e) {
        var elem = e.target;
        if (elem.tagName.toLowerCase() === 'img') {
            //elem.src = "/Content/images/no-pic.png";
            console.log($(elem));
            $(elem).attr("src", "/Content/images/logo.png");
        }
    }, true /*指定事件处理函数在捕获阶段执行*/);

});

function AddSearchEvents() {
    $("#SearchForm .KeyWords:not(.multi)").on("keyup", function () {
        clearTimeout(t);
        t = setTimeout(BtnSearch, 1500);
    });
    //$("#SearchForm .KeyWords").on("blur", function () {
    //    clearTimeout(t);
    //    t = setTimeout(BtnSearch, 1500);
    //});
    $("#SearchForm .KeyWords:not(.multi)").off("change.searching").on("change.searching", function () {
        clearTimeout(t);
        BtnSearch();
        //t = setTimeout(BtnSearch, 2000);
    });
    $("#SearchForm .KeyWords").on("focus", function () {
        clearTimeout(t);
    });
    $("#SearchForm .KeyField").off("change.searching").on("change.searching", function () {
        clearTimeout(t);
    });
}

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
//调用方法很简单：
//element.innerHTML = String.format('<a href="%1" onclick="alert(\’%2\’);">%3</a>', url, msg, text);

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
// ReSharper disable once NativeTypePrototypeExtending
Date.prototype.Format = function(fmt) {
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
    r[f] = str;
    return str;
}

function DateTimeFormatter(v,r,i,f) {
    if (!v) {
        return "";
    }
    var date = new Date(v);
    var str = date.format('yyyy-MM-dd hh:mm:ss');
    r[f] = str;
    return str;
}
//计算时间差的函数
function DateDiff(sDate1, sDate2,type) { //sDate1和sDate2是2019-3-12格式  type为1：天数  2：小时
    var  iValue;
    type = type || 1;
    sDate2 = sDate2 || new Date();
    var spanOff = parseInt(Math.abs(new Date(sDate1).getTime() - new Date(sDate2).getTime()));
    switch (type) {
        case 1:
        default:
            iValue = spanOff/1000/60/60/24;
            break;
        case 2:
            iValue = spanOff/1000/60/60;
            break;
        case 3:
            iValue = spanOff/1000/60;
            break;
    }

    return iValue.toFixed(2);
}

//前后时间差
function DateDiffBeforeToAfter(sDate1, sDate2, type) { //sDate1和sDate2是2019-3-12格式  type为1：天数  2：小时
    var iValue = DateDiff(sDate1, sDate2, type);
    var d1 = new Date(sDate1).getTime();
    var d2 = new Date(sDate2).getTime();
    iValue = d1 > d2 ? iValue : (0 - iValue);
    return  iValue.toFixed(2);
}

function guid(options) {
    var defaultOption =
    {
        removeBar: true,
        isUpper:false
    };
    $.extend(defaultOption, options);
    var result =  'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
    if (defaultOption.removeBar) {
        result = result.replace(/-/g, '');
    }
    if (defaultOption.isUpper) {
        result = result.toUpperCase();
    }
    console.log(result);
    return result;
}
var cnmsg = {
    required: "必填",
    remote: "请修正该字段",
    email: "请输入正确格式的电子邮件",
    url: "请输入合法的网址",
    date: "请输入合法的日期",
    dateISO: "请输入日期(yyyy-MM-dd)",
    number: "请输入合法的数字",
    digits: "请输入合法的整数",
    creditcard: "请输入合法的信用卡号",
    equalTo: "请再次输入相同的值",
    accept: "请输入拥有合法后缀名的字符串",
    //maxlength: jQuery.format("请输入一个长度最多是 {0} 的字符串"),
    //minlength: jquery.format("请输入一个长度最少是 {0} 的字符串"),
    //rangelength: jQuery.format("请输入一个长度介于 {0} 和 {1} 之间的字符串"),
    //range: jQuery.format("请输入一个介于 {0} 和 {1} 之间的值"),
    //max: jQuery.format("请输入一个最大为 {0} 的值"),
    //min: jQuery.format("请输入一个最小为 {0} 的值")
};
jQuery.extend(jQuery.validator.messages, cnmsg);




var idCardNoUtil = {
    provinceAndCities: {
        11: "北京",
        12: "天津",
        13: "河北",
        14: "山西",
        15: "内蒙古",
        21: "辽宁",
        22: "吉林",
        23: "黑龙江",
        31: "上海",
        32: "江苏",
        33: "浙江",
        34: "安徽",
        35: "福建",
        36: "江西",
        37: "山东",
        41: "河南",
        42: "湖北",
        43: "湖南",
        44: "广东",
        45: "广西",
        46: "海南",
        50: "重庆",
        51: "四川",
        52: "贵州",
        53: "云南",
        54: "西藏",
        61: "陕西",
        62: "甘肃",
        63: "青海",
        64: "宁夏",
        65: "新疆",
        71: "台湾",
        81: "香港",
        82: "澳门",
        91: "国外"
    },
 
    powers: ["7", "9", "10", "5", "8", "4", "2", "1", "6", "3", "7", "9", "10", "5", "8", "4", "2"],
    parityBit: ["1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2"],
    genders: { male: "男", female: "女" },
 
    checkAddressCode: function(addressCode) {
        var check = /^[1-9]\d{5}$/.test(addressCode);
        if (!check) return false;
        if (idCardNoUtil.provinceAndCities[parseInt(addressCode.substring(0, 2))]) {
            return true;
        } else {
            return false;
        }
    },
    checkBirthDayCode: function(birDayCode) {
        var check = /^[1-9]\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))$/.test(birDayCode);
        if (!check) return false;
        var yyyy = parseInt(birDayCode.substring(0, 4), 10);
        var mm = parseInt(birDayCode.substring(4, 6), 10);
        var dd = parseInt(birDayCode.substring(6), 10);
        var xdata = new Date(yyyy, mm - 1, dd);
        if (xdata > new Date()) {
            return false; //生日不能大于当前日期
        } else if ((xdata.getFullYear() === yyyy) && (xdata.getMonth() === mm - 1) && (xdata.getDate() === dd)) {
            return true;
        } else {
            return false;
        }
    },
 
    getParityBit: function(idCardNo) {
        var id17 = idCardNo.substring(0, 17);
        var power = 0;
        for (var i = 0; i < 17; i++) {
            power += parseInt(id17.charAt(i), 10) * parseInt(idCardNoUtil.powers[i]);
        }
        var mod = power % 11;
        return idCardNoUtil.parityBit[mod];
    },
 
    checkParityBit: function(idCardNo) {
      
        var parityBit = idCardNo.charAt(17).toUpperCase();   
        if (idCardNoUtil.getParityBit(idCardNo) === parityBit) {
            return true;
        } else {
            return false;
        }
    },
 
    checkIdCardNo: function(idCardNo) {
 
        //15位和18位身份证号码的基本校验
        var check = /^\d{15}|(\d{17}(\d|x|X))$/.test(idCardNo);
 
        if (!check) return false;
 
        //判断长度为15位或18位
        if (idCardNo.length === 15) {
            return idCardNoUtil.check15IdCardNo(idCardNo);
        } else if (idCardNo.length === 18) {
            return idCardNoUtil.check18IdCardNo(idCardNo);
        } else {
            return false;
        }
    },
 
    //校验15位的身份证号码
    check15IdCardNo: function(idCardNo) {
        //15位身份证号码的基本校验
        var check = /^[1-9]\d{7}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\d{3}$/.test(idCardNo);
        if (!check) return false;
        //校验地址码
        var addressCode = idCardNo.substring(0, 6);
        check = idCardNoUtil.checkAddressCode(addressCode);
        if (!check) return false;
        var birDayCode = '19' + idCardNo.substring(6, 12);
        //校验日期码
        return idCardNoUtil.checkBirthDayCode(birDayCode);
    },
 
    //校验18位的身份证号码
    check18IdCardNo: function(idCardNo) {
        //18位身份证号码的基本格式校验
        var check = /^[1-9]\d{5}[1-9]\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\d{3}(\d|x|X)$/.test(idCardNo);
 
        if (!check) return false;
          
        //校验地址码
        var addressCode = idCardNo.substring(0, 6);
        check = idCardNoUtil.checkAddressCode(addressCode);
        if (!check) return false;
 
        //校验日期码
        var birDayCode = idCardNo.substring(6, 14);
        check = idCardNoUtil.checkBirthDayCode(birDayCode);
        if (!check) return false;
   
        //验证校检码
        return idCardNoUtil.checkParityBit(idCardNo);
    }
    ,
    formateDateCN: function(day) {
        var yyyy = day.substring(0, 4);
        var mm = day.substring(4, 6);
        var dd = day.substring(6);
        return yyyy + '-' + mm + '-' + dd;
    },
    //获取信息
    getIdCardInfo: function(idCardNo) {
        var idCardInfo = {
            gender: "", //性别
            birthday: "" // 出生日期(yyyy-mm-dd)
        };
        var aday;
        if (idCardNo.length === 15) {
            aday = '19' + idCardNo.substring(6, 12);

            idCardInfo.birthday = idCardNoUtil.formateDateCN(aday);
 
            if (parseInt(idCardNo.charAt(14)) % 2 === 0) {
                idCardInfo.gender = idCardNoUtil.genders.female;
            } else {
                idCardInfo.gender = idCardNoUtil.genders.male;
            }
        } else if (idCardNo.length === 18) {
            aday = idCardNo.substring(6, 14);

            idCardInfo.birthday = idCardNoUtil.formateDateCN(aday);
 
            if (parseInt(idCardNo.charAt(16)) % 2 === 0) {
                idCardInfo.gender = idCardNoUtil.genders.female;
            } else {
                idCardInfo.gender = idCardNoUtil.genders.male;
            }
        }
        return idCardInfo;
    },
 
    getId15: function(idCardNo) {
        if (idCardNo.length === 15) {
            return idCardNo;
        } else if (idCardNo.length === 18) {
            return idCardNo.substring(0, 6) + idCardNo.substring(8, 17);
        } else {
            return null;
        }
    },
 
    getId18: function(idCardNo) {
        if (idCardNo.length === 15) {
            var id17 = idCardNo.substring(0, 6) + '19' + idCardNo.substring(6);
            var parityBit = idCardNoUtil.getParityBit(id17);
            return id17 + parityBit;
        } else if (idCardNo.length === 18) {
            return idCardNo;
        } else {
            return null;
        }
    }
};　


//除法函数，用来得到精确的除法结果
//说明：javascript的除法结果会有误差，在两个浮点数相除的时候会比较明显。这个函数返回较为精确的除法结果。
//调用：accDiv(arg1,arg2)
//返回值：arg1除以arg2的精确结果
function accDiv(arg1, arg2) {
    var t1 = 0, t2 = 0, r1, r2;
    try { t1 = arg1.toString().split(".")[1].length } catch (e) { }
    try { t2 = arg2.toString().split(".")[1].length } catch (e) { }
    with (Math) {
        r1 = Number(arg1.toString().replace(".", ""))
        r2 = Number(arg2.toString().replace(".", ""))
        return (r1 / r2) * pow(10, t2 - t1);
    }
}

//给Number类型增加一个div方法，调用起来更加 方便。
Number.prototype.Newdiv = function (arg) {
    return accDiv(this, arg);
}

//乘法函数，用来得到精确的乘法结果
//说明：javascript的乘法结果会有误差，在两个浮点数相乘的时候会比较明显。这个函数返回较为精确的乘法结果。
//调用：accMul(arg1,arg2)
//返回值：arg1乘以 arg2的精确结果
function accMul(arg1, arg2) {
    var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
    try { m += s1.split(".")[1].length } catch (e) { }
    try { m += s2.split(".")[1].length } catch (e) { }
    return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m)
}

// 给Number类型增加一个mul方法，调用起来更加方便。
Number.prototype.NewMul = function (arg) {
    return accMul(arg, this);
}

//加法函数，用来得到精确的加法结果
//说明：javascript的加法结果会有误差，在两个浮点数相加的时候会比较明显。这个函数返回较为精确的加法结果。
//调用：accAdd(arg1,arg2)
// 返回值：arg1加上arg2的精确结果
function accAdd(arg1, arg2) {
    var r1, r2, m;
    try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
    try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
    m = Math.pow(10, Math.max(r1, r2))
    return (arg1 * m + arg2 * m) / m
}

//给Number类型增加一个add方法，调用起来更加方便。
Number.prototype.NewAdd = function (arg) {
    return accAdd(arg, this);
}