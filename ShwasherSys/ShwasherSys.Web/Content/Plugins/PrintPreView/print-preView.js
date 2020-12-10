;
(function($) {
    // Private functions
    var modal, size, printFrame, printModal, printControls;
    var printOption = {};
    var printPreview = {
        defaults: { //printPreview默认配置
            printBody: 'body',
            modalWidth: 794,
            cssUrl: null,
            printCssUrl: null,
            screenCssUrl: null,
            watermark: null //水印，1.0版本暂不支持
        },
        loadPrintPreview: function(options) {
            printOption = $.extend({}, printPreview.defaults, options);

            printModal = $('<div id="print-modal"></div>');
            printControls = $('<div id="print-modal-controls">' +
                '<a href="#" class="print btn" title="打印页面">打印页面</a>' +
                '<a href="#" class="close btn" title="关闭预览">关闭预览</a>').hide();
            printFrame =
                $('<iframe id="print-modal-content" scrolling="no" border="0" frameborder="0" name="print-frame" />');
            // Raise print preview window from the dead, zooooooombies
            printModal.hide().append(printControls).append(printFrame).appendTo('body');

            // The frame lives
            var printFrameRef;
            for (var i = 0; i < window.frames.length; i++) {
                if (window.frames[i].name === "print-frame") {
                    printFrameRef = window.frames[i].document;
                    break;
                }
            }
            printFrameRef.open();
            printFrameRef.write(
                '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">' +
                '<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">' +
                '<head><title>' +
                document.title +
                '</title></head>' +
                '<body></body>' +
                '</html>');
            printFrameRef.close();

            // Grab contents and apply stylesheet
            var $iframeHead = $('head link[media*=print], head link[media=all]').clone();
            //$iframeHead = $('head link').clone();
            var $iframeBody = $(printOption.printBody + ' > *:not(#print-modal):not(script)').clone();

            $iframeHead.each(function() {
                $(this).attr('media', 'all');
            });
            //if (!$.browser.msie && !($.browser.version < 7)) {
            //    $('head', printFrameRef).append($iframeHead);
            //    $('body', printFrameRef).append($iframeBody);
            //} else {
            //    $('body > *:not(#print-modal):not(script)').clone().each(function() {
            //        $('body', printFrameRef).append(this.outerHTML);
            //    });
            //    $('head link[media*=print], head link[media=all]').each(function() {
            //        $('head', printFrameRef).append($(this).clone().attr('media', 'all')[0].outerHTML);
            //    });
            //}
            $('head', printFrameRef).append($iframeHead);
            // Introduce print styles
            $('head', printFrameRef)
                .append(
                    '<style type="text/css">* {color: ##000# !important;margin: 0;padding: 0;}@media print{/* -- Print Preview --*/#print-modal-mask,#print-modal {display: none !important;}}</style><style media="print">@page{size:A4;margin: 0mm;margin-left: 15mm;margin-right: 15mm;}body{margin-top:25mm;}</style>');
            var arr;
            if (printOption.cssUrl !== null) {
                arr = printOption.cssUrl.split(",");
                for (var j = 0; j < arr.length; j++) {
                    if (arr[j] !== "") {
                        $('head', printFrameRef)
                            .append('<link type="text/css" rel="stylesheet" href="' + arr[j] + '"/>');
                    }
                }
            }
            if (printOption.printCssUrl !== null) {
                arr = printOption.printCssUrl.split(",");
                for (var h = 0; h < arr.length; h++) {
                    if (arr[h] !== "") {
                        $('head', printFrameRef).append('<link type="text/css" rel="stylesheet" href="' +
                            arr[h] +
                            '" media="print" />');
                    }
                }
            }
            if (printOption.screenCssUrl !== null) {
                arr = printOption.screenCssUrl.split(",");
                for (var k = 0; k < arr.length; k++) {
                    if (arr[k] !== "") {
                        $('head', printFrameRef).append('<link type="text/css" rel="stylesheet" href="' +
                            arr[k] +
                            '" media="screen" />');
                    }
                }
            }

            $('body', printFrameRef).append($iframeBody);
           
            // Disable scrolling
            //$('#print-modal-content').css({ overflowY: 'hidden', height: "100%" });


            // Disable all links
            $('a', printFrameRef).bind('click.printPreview', function(e) { e.preventDefault(); });

            // Load mask
            printPreview.loadModal();

            // Position modal            
            var startingPosition = $(window).height() + $(window).scrollTop();
            var width = printOption.modalWidth+10;
            var left = $(window).width() - width - 140;
            left = left < 0 ? 0 : left / 2;
            var css = {
                top: startingPosition,
                height: 'auto',
                overflowY: 'auto',
                zIndex: 10000,
                display: 'block',
                width: width + 'px',
                left: left + 'px'
            };
            printModal.css(css).animate({ top: $(window).scrollTop() },
                400,
                'linear',
                function() {
                    printControls.fadeIn('slow').focus();
                });
            // Bind closure
            $('a', printControls).bind('click',
                function(e) {
                    e.preventDefault();
                    if ($(this).hasClass('print')) {
                        printFrame[0].contentWindow.print();
                    } else {
                        printPreview.distroyPrintPreview();
                    }
                });
        },
        distroyPrintPreview: function() {
            printControls.fadeOut(100);
            printModal.animate({ top: $(window).scrollTop() - $(window).height(), opacity: 1 },
                400,
                'linear',
                function() {
                    printModal.remove();
                    $('body').css({ overflowY: 'auto', height: 'auto' });
                });
            modal.fadeOut('slow',
                function() {
                    modal.remove();
                });

            $(document).unbind("keydown.printPreview.mask");
            modal.unbind("click.printPreview.mask");
            $(window).unbind("resize.printPreview.mask");
        },

        /* -- Modal Functions --*/
        loadModal: function() {
            size = printPreview.sizeUpModal();
            modal = $('<div id="print-modal-mask" class="modal-backdrop" />').appendTo($('body'));
            //modal.css({
            //    position: 'absolute',
            //    top: 0,
            //    left: 0,
            //    width: size[0],
            //    height: size[1],
            //    display: 'none',
            //    opacity: 0,
            //    zIndex: 9999,
            //    backgroundColor: '#000'
            //});
            modal.css({ display: 'block' }).fadeTo('400', 0.75,
                function() {
            printPreview.setIframeHeight(document.getElementById('print-modal-content'));

                });

            $(window).bind("resize.printPreview.mask",
                function() {
                    printPreview.updateModalSize();
                });

            modal.bind("click.printPreview.mask",
                function() {
                    printPreview.distroyPrintPreview();
                });

            $(document).bind("keydown.printPreview.mask",
                function(e) {
                    if (e.keyCode === 27) {
                        printPreview.distroyPrintPreview();
                    }
                });
        },

        sizeUpModal: function() {
            //if ($.browser.msie) {
            //    // if there are no scrollbars then use window.height
            //    var d = $(document).height(), w = $(window).height();
            //    return [
            //        window.innerWidth || // ie7+
            //        document.documentElement.clientWidth || // ie6  
            //        document.body.clientWidth, // ie6 quirks mode
            //        d - w < 20 ? w : d
            //    ];
            //} else {
            //    return [$(document).width(), $(document).height()];
            //}
            return [$(document).width(), $(document).height()];
        },
        updateModalSize: function () {

            var size = printPreview.sizeUpModal();

            modal.css({ width: size[0], height: size[1] });
        },
        setIframeHeight(iframe) {
            if (iframe) {
                var iframeWin = iframe.contentWindow || iframe.contentDocument.parentWindow;
                if (iframeWin.document.body) {
                    iframe.height = iframeWin.document.documentElement.scrollHeight ||
                        iframeWin.document.body.scrollHeight;
                }
            }
        }

};

    // Initialization
    $.fn.printPreview = function (options) {
        this.each(function () {
            $(this).bind('click', function (e) {
                e.preventDefault();
                if (!$('#print-modal').length) {
                    printPreview.loadPrintPreview(options);
                }
            });
        });
        return this;
    };
})(jQuery);