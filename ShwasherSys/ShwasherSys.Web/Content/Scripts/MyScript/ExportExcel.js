var idTmr;
function getExplorer() {
    var explorer = window.navigator.userAgent;
    //ie
    if (explorer.indexOf("MSIE") >= 0) {
        return 'ie';
    }
    //firefox
    else if (explorer.indexOf("Firefox") >= 0) {
        return 'Firefox';
    }
    //Chrome
    else if (explorer.indexOf("Chrome") >= 0) {
        return 'Chrome';
    }
    //Opera
    else if (explorer.indexOf("Opera") >= 0) {
        return 'Opera';
    }
    //Safari
    else if (explorer.indexOf("Safari") >= 0) {
        return 'Safari';
    }
}
function ExportExcel(tableid, name,style) {//整个表格拷贝到EXCEL中
    if (getExplorer() == 'ie') {
        var curTbl = document.getElementById(tableid);
        var oXL = new ActiveXObject("Excel.Application");

        //创建AX对象excel
        var oWB = oXL.Workbooks.Add();
        //获取workbook对象
        var xlsheet = oWB.Worksheets(1);
        //激活当前sheet
        var sel = document.body.createTextRange();
        sel.moveToElementText(curTbl);
        //把表格中的内容移到TextRange中
        sel.select;
        //全选TextRange中内容
        sel.execCommand("Copy");
        //复制TextRange中内容
        xlsheet.Paste();
        //粘贴到活动的EXCEL中
        oXL.Visible = true;
        //设置excel可见属性

        try {
            var fname = oXL.Application.GetSaveAsFilename("Excel.xls", "Excel Spreadsheets (*.xls), *.xls");
        } catch (e) {
            print("Nested catch caught " + e);
        } finally {
            oWB.SaveAs(fname);

            oWB.Close(savechanges = false);
            //xls.visible = false;
            oXL.Quit();
            oXL = null;
            //结束excel进程，退出完成
            //window.setInterval("Cleanup();",1);
            idTmr = window.setInterval("Cleanup();", 1);
        }
    } else {
        //tableToExcel(tableid, name, style);
        tableToExcel2(tableid, name, style);
    }
}
function Cleanup() {
    window.clearInterval(idTmr);
    CollectGarbage();
}

/*
    template ： 定义文档的类型，相当于html页面中顶部的<!DOCTYPE> 声明。（个人理解，不确定）
    encodeURIComponent:解码
    unescape() 函数：对通过 escape() 编码的字符串进行解码。
    window.btoa(window.encodeURIComponent(str)):支持汉字进行解码。
    \w ：匹配包括下划线的任何单词字符。等价于’[A-Za-z0-9_]’
    replace()方法：用于在字符串中用一些字符替换另一些字符，或替换一个与正则表达式匹配的子串。
    {(\w+)}：匹配所有 {1个或更多字符} 形式的字符串；此处匹配输出内容是 “worksheet”
    正则中的() ：是为了提取匹配的字符串。表达式中有几个()就有几个相应的匹配字符串。
    讲解(/{(\w+)}/g, function(m, p) { return c[p]; } ：
        /{(\w+)}/g 匹配出所有形式为“{worksheet}”的字符串；
        function参数：  m  正则所匹配到的内容，即“worksheet”；
                        p  正则表达式中分组的内容,即“(\w+)”分组中匹配到的内容，为“worksheet”；
        c ：为object，见下图3
        c[p] : 为“worksheet”

*/
var tableToExcel = (function () {
    var uri = 'data:application/vnd.ms-excel;base64,',
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office"  xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>${worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]-->{style}</head><body><table>{table}</table></body></html>',
        base64 = function (s) {
            return window.btoa(unescape(encodeURIComponent(s)));
        },
        // 下面这段函数作用是：将template中的变量替换为页面内容ctx获取到的值
        format = function (s, c) {
            return s.replace(/{(\w+)}/g,
                function (m, p) {
                    return c[p];
                }
            );
        };
    return function (table, name, style) {
        //if (!table.nodeType) {
        //    table = document.getElementById(table);
        //}
        var tableHtml = $("#" + table).html();
        // 获取表单的名字和表单查询的内容
        var ctx = { worksheet: name || 'Worksheet', table: tableHtml,style:$("#"+style).html() };
        // format()函数：通过格式操作使任意类型的数据转换成一个字符串
        // base64()：进行编码
        console.log(format(template, ctx));
        var ex = getExplorer();
        if (ex == 'Firefox' || ex == 'Chrome') {
            console.log(format(template, ctx));
            var blob = new Blob([format(template, ctx)]);
            a.href = URL.createObjectURL(blob);//解决由于数据量太大导致chrome导出出现网络错误（由于url长度限制）
            $("body").append('<a id="cLink" href="' +
                uri +
                base64(format(template, ctx)) +
                '" download="' +
                name +
                Math.floor(Math.random() * 100 + 1) +
                '.xls"></a>');
            $("#cLink")[0].click();
            $("#cLink").remove();
        } else {
            window.location.href = uri + base64(format(template, ctx));
        }

        //window.location.href = uri + base64(format(template, ctx));
        /*var exportA = document.createElement('a');

        exportA.setAttribute('download',name+'123.xls');
        exportA.setAttribute('href', uri + base64(format(template, ctx)));

        exportA.click(); */
    }
})();

var tableToExcel2 = function (table, fileName, style) {
    var format = function (s, c) {
        return s.replace(/{(\w+)}/g,
            function (m, p) {
                return c[p];
            }
        );
    };
    var uri = 'data:application/vnd.ms-excel;base64,'
        , fileName = fileName || 'excelexport'
        , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><meta name="renderer" content="webkit"><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]-->{style}</head><body><table>{table}</table></body></html>';
    var tableHtml = $("#" + table).html();
    var ctx = { worksheet: 'Worksheet', table: tableHtml, style: $("#" + style).html() };
    var a = document.createElement('a');
    document.body.appendChild(a);
    a.hreflang = 'zh';
    a.charset = 'utf8';
    a.type = "application/vnd.ms-excel";

    var blob = new Blob([format(template, ctx)]);
    a.href = URL.createObjectURL(blob);//解决由于数据量太大导致chrome导出出现网络错误（由于url长度限制）
    //        a.href = uri + Base64.encode(format(template,ctx)); 
    a.target = '_blank';
    a.download = fileName + '.xls';
    a.tableBorder = 1;
    a.click();

};
