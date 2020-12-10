jQuery.extend({
	createUploadIframe: function(id, uri) {
		//create frame
		var frameId = 'jUploadFrame' + id;
		var iframeHtml = '<iframe id="' +
			frameId +
			'" name="' +
			frameId +
			'" style="position:absolute; top:-9999px; left:-9999px"';
		if (window.ActiveXObject) {
			if (typeof uri == 'boolean') {
				iframeHtml += ' src="' + 'javascript:false' + '"';
			} else if (typeof uri == 'string') {
				iframeHtml += ' src="' + uri + '"';
			}
		}
		iframeHtml += ' />';
		jQuery(iframeHtml).appendTo(document.body);
		return jQuery('#' + frameId).get(0);
	},
	createUploadForm: function(id, fileElementId, data) {
		//create form
		var formId = 'jUploadForm' + id;
		var fileId = 'jUploadFile' + id;
		var form = jQuery('<form  action="" method="POST" name="' +
			formId +
			'" id="' +
			formId +
			'" enctype="multipart/form-data"></form>');
		if (data) {
			for (var i in data) {
				if (data[i] != null&&data[i].name != null && data[i].value != null) {
					jQuery('<input type="hidden" name="' + data[i].name + '" value="' + data[i].value + '" />').appendTo(form);
				} else if (data[i] != null) {
					jQuery('<input type="hidden" name="' + i + '" value="' + data[i] + '" />').appendTo(form);
				}
            }
		    var token = abp.security.antiForgery.getToken();
        }
        var fileElementIds = fileElementId.split(",");
        for (var j = 0; j < fileElementIds.length; j++) {
            var oldElement = jQuery('#' + fileElementIds[j]);
            var newElement = jQuery(oldElement).clone();
            jQuery(oldElement).attr('id', fileId+j);
            jQuery(oldElement).before(newElement);
            jQuery(oldElement).appendTo(form);
	    }
		//var oldElement = jQuery('#' + fileElementId);
		//var newElement = jQuery(oldElement).clone();
		//jQuery(oldElement).attr('id', fileId);
		//jQuery(oldElement).before(newElement);
		//jQuery(oldElement).appendTo(form);
		//set attributes
		jQuery(form).css('position', 'absolute');
		jQuery(form).css('top', '-1200px');
		jQuery(form).css('left', '-1200px');
		jQuery(form).appendTo('body');
		return form;
	},
	ajaxFileUpload: function(s) {
		// TODO introduce global settings, allowing the client to modify them for all requests, not only timeout
	    var token = abp.security.antiForgery.getToken();

        s = jQuery.extend({}, jQuery.ajaxSettings, s, { headers: { "X-XSRF-TOKEN": token }});
		var id = new Date().getTime()
		var form = jQuery.createUploadForm(id, s.fileElementId, (typeof (s.data) == 'undefined' ? false : s.data));
		var io = jQuery.createUploadIframe(id, s.secureuri);
		var frameId = 'jUploadFrame' + id;
        var formId = 'jUploadForm' + id;

		// Watch for a new set of requests
		if (s.global && !jQuery.active++) {
			jQuery.event.trigger("ajaxStart");
		}
		var requestDone = false;
		// Create the request object
        var xml = {};
		if (s.global)
			jQuery.event.trigger("ajaxSend", [xml, s]);
		// Wait for a response to come back
		var uploadCallback = function(isTimeout) {
			var io = document.getElementById(frameId);
			try {
				if (io.contentWindow) {
					xml.responseText = io.contentWindow.document.body ? io.contentWindow.document.body.innerHTML : null;
					xml.responseXML = io.contentWindow.document.XMLDocument
						? io.contentWindow.document.XMLDocument
						: io.contentWindow.document;
				} else if (io.contentDocument) {
					xml.responseText = io.contentDocument.document.body ? io.contentDocument.document.body.innerHTML : null;
					xml.responseXML = io.contentDocument.document.XMLDocument
						? io.contentDocument.document.XMLDocument
						: io.contentDocument.document;
				}
			} catch (e) {
				jQuery.handleError(s, xml, null, e);
			}
			if (xml || isTimeout == "timeout") {
				requestDone = true;
				var status;
				try {
					status = isTimeout != "timeout" ? "success" : "error";
					// Make sure that the request was successful or notmodified
					if (status != "error") {
						// process the data (runs the xml through httpData regardless of callback)
						var data = jQuery.uploadHttpData(xml, s.dataType);
						// If a local callback was specified, fire it and pass it the data
						if (s.success)
							s.success(data, status);
						// Fire the global callback
						if (s.global)
							jQuery.event.trigger("ajaxSuccess", [xml, s]);
					} else
						jQuery.handleError(s, xml, status);
				} catch (e) {
					status = "error";
					jQuery.handleError(s, xml, status, e);
				}
				// The request was completed
				if (s.global)
					jQuery.event.trigger("ajaxComplete", [xml, s]);
				// Handle the global AJAX counter
				if (s.global && ! --jQuery.active)
					jQuery.event.trigger("ajaxStop");
				// Process result
				if (s.complete)
					s.complete(xml, status);
				jQuery(io).unbind()
			    setTimeout(function() {
			            try {
			                jQuery(io).remove();
			                jQuery(form).remove();
			            } catch (e) {
			                jQuery.handleError(s, xml, null, e);
			            }
			        },
			        100);
				xml = null
			}
		}
		// Timeout checker
		if (s.timeout > 0) {
			setTimeout(function() {
					// Check to see if the request is still happening
					if (!requestDone) uploadCallback("timeout");
				},
				s.timeout);
		}
		try {
			form = jQuery('#' + formId);
			jQuery(form).attr('action', s.url);
			jQuery(form).attr('method', 'POST');
			jQuery(form).attr('target', frameId);
			if (form.encoding) {
				jQuery(form).attr('encoding', 'multipart/form-data');
			} else {
				jQuery(form).attr('enctype', 'multipart/form-data');
            }

			jQuery(form).submit();
		} catch (e) {
			jQuery.handleError(s, xml, null, e);
		}
		jQuery('#' + frameId).load(uploadCallback);
		return { abort: function() {} };
	},
	uploadHttpData: function(r, type) {
		var data = !type;
		var start;
		var end;
		if (!type) {
			data = r.responseText;
			start = data.indexOf(">");
			if (start !== -1) {
				end = data.indexOf("<", start + 1);
				if (end !== -1) {
					data = data.substring(start + 1, end);
				}
			}
		}
		if (type === "xml")
			data = r.responseXML;
		//data = type == "xml" || data ? r.responseXML : r.responseText;
		// If the type is "script", eval it in global context
		if (type === "script")
			jQuery.globalEval(data);
		// Get the JavaScript object, if JSON is used.
		if (type == "json") {
			////////////Fix the issue that it always throws the error "unexpected token '<'"///////////////
			data = r.responseText;
			start = data.indexOf(">");
			if (start !== -1) {
				end = data.indexOf("<", start + 1);
				if (end !== -1) {
					data = data.substring(start + 1, end);
				}
			}
			///////////////////////////////////////////////////////////////////////////////////////////////
			eval("data = " + data);
		}
		// evaluate scripts within html
		if (type == "html")
			jQuery("<div>").html(data).evalScripts();
		return data;
	},
	handleError: function(s, xhr, status, e) {
		// If a local callback was specified, fire it
		if (s.error) {
			s.error.call(s.context || s, xhr, status, e);
		}
		// Fire the global callback
		if (s.global) {
			(s.context ? jQuery(s.context) : jQuery.event).trigger("ajaxError", [xhr, s, e]);
		}
    },
});
function CreateXMLHttpRequest() {
    //对于DOM 2 规范的浏览器
    if (window.XMLHttpRequest) {
        var objXMLHttp = new XMLHttpRequest();
    }
    //对于Internet Explorer浏览器
    else {
        //将Internet Explorer内置的所有XMLHTTP ActiveX控制设置成数组
        var MSXML = ['MSXML2.XMLHTTP.5.0', 'MSXML2.XMLHTTP.4.0',
            'MSXML2.XMLHTTP.3.0', 'MSXML2.XMLHTTP', 'Microsoft.XMLHTTP'];
        //依次对Internet Explorer内置的XMLHTTP控件初始化，尝试创建XMLHttpRequest对象
        for (var n = 0; n < MSXML.length; n++) {
            try {
                //如果可以正常创建XMLHttpRequest对象，使用break跳出循环
                var objXMLHttp = new ActiveXObject(MSXML[n]);
                break;
            }
            catch (e) {
            }
        }
    }
    //Mozilla某些版本没有readyState属性
    if (objXMLHttp.readyState == null) {
        //直接设置其readyState为0
        objXMLHttp.readyState = 0;
        //对于哪些没有readyState属性的浏览器，将load动作与下面的函数关联起来
        objXMLHttp.addEventListener("load", function () {
            //当从服务器加载数据完成后，将readyState状态设为4
            objXMLHttp.readyState = 4;
            if (typeof objXMLHttp.onreadystatechange == "function") {
                objXMLHttp.onreadystatechange();
            }
        }, false);
    }
    return objXMLHttp;
}

function CheckFileSize(idStr) {
	var maxsize = 2 * 1024 * 1024;//2M
	var errMsg = "上传的附件文件不能超过2M！！！";
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
	try{
		var objFile = document.getElementById(idStr);
		if(objFile.value===""){
			//layer.alert("请先选择上传文件", { icon: 7, title: "提示信息" });
			return;
		}
		var filesize;
		if(browserCfg.firefox || browserCfg.chrome ){
			filesize = objFile.files[0].size;
		}else if(browserCfg.ie){
			var objImg = document.createElement("img");
			objImg.id = "tempImg";
			objImg.style.display = "none";
			document.body.appendChild(objImg);
			objImg.dynsrc=objFile.value;
			filesize = objImg.fileSize;
		}else{
			layer.alert(tipMsg, { icon: 7, title: "提示信息" });
			return;
		}
		if(filesize===-1){
			layer.alert(tipMsg, { icon: 7, title: "提示信息" });
			return;
		}else if(filesize>maxsize){
			layer.alert(errMsg,{icon:7,title:"提示信息"});
			return;
		}else{
			//alert("文件大小符合要求");
			return;
		}
	}catch(e){
		layer.alert(e);
	}
}