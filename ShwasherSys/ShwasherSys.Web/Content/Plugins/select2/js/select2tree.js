(function ($) {
    $.fn.select2tree = function (options) {
        var defaults = {
            language: "zh-CN",
            minimumResultsForSearch: -1
            /*theme: "bootstrap"*/
        };
        var opts = $.extend(defaults, options);
        opts.templateResult = function (data, container) {
            if (data.element) {
                //insert span element and add 'parent' property
                var $wrapper = $("<span></span><span>" + data.text + "</span>");
                var $element = $(data.element);
                $(container).attr("val", $element.val());
                if ($element.attr("parent")) {
                    $(container).attr("parent", $element.attr("parent"));
                }
                return $wrapper;
            } else {
                return data.text;
            }
        };
        var $that = $(this);
        $(this).select2(opts).on("select2:open", open);
    };

    function moveOption(id, index) {
        index = index || 0;
        if (id) {
            $(".select2-results__options li[parent=" + id + "]").insertAfter(".select2-results__options li[val=" + id + "]");
            $(".select2-results__options li[parent=" + id + "]").each(function () {
                $(this).attr('level', index);
                moveOption($(this).attr("val"), index + 1);
            });
        } else {
            $(".select2-results__options li:not([parent])").appendTo(".select2-results__options ul");
            $(".select2-results__options li:not([parent])").each(function () {
                $(this).attr('level', index);
                moveOption($(this).attr("val"), index + 1);
            });
        }
    }

    //deal switch action
    function switchAction(id, open, isAll) {
        if (!id || $(".select2-results__options li[parent='" + id + "']").length <= 0) {
            return;
        }
        if (isAll && $(".select2-results__options li[val=" + id + "][level='1']").length > 0) {
            return;
        }
        if (open) {
            $(".select2-results__options li[val=" + id + "] span[class]:eq(0)").removeClass("icon-add").addClass("icon-delete");
            //$(".select2-results__options li[parent='" + id + "']").slideDown();
        } else {
            $(".select2-results__options li[val=" + id + "] span[class]:eq(0)").addClass("icon-add").removeClass("icon-delete");
            // $(".select2-results__options li[parent='" + id + "']").slideUp();
        }
        $(".select2-results__options li[parent='" + id + "']").each(function () {
            var $that = $(this);
            open ? $that.slideDown() : $that.slideUp();
            switchAction($that.attr("val"), open, isAll);
        });
    }

    function open() {
        setTimeout(function () {
            moveOption();

            $(".select2-results__options li").each(function () {
                var $this = $(this);
                $this.find("span:eq(0)").addClass('iconfont icon-folder');
                if ($this.attr("parent")) {
                    $(this).siblings("li[val=" + $this.attr("parent") + "]").find("span:eq(0)")
                        .removeClass("icon-folder").addClass("icon-add switch").css({ "cursor": "pointer" });
                    $(this).siblings("li[val=" + $this.attr("parent") + "]").find("span:eq(1)").css("font-weight", "bold");
                }
                if (!$this.attr("style")) {
                    var level = $this.attr("level");
                    var paddingLeft = level;
                    $("li[parent='" + $this.attr("parent") + "']").css("display", "none").css("padding-left", paddingLeft + "em");
                }

            });

            $(".select2-results__options li[level='0']").each(function () {
                var $this = $(this);
                var id = $this.attr("val");
                switchAction(id, true, true);
            });

            //override mousedown for collapse/expand 
            $(".switch").off("mousedown").on("mousedown", function (event) {
                switchAction($(this).parent().attr("val"), $(this).hasClass("icon-add"));
                event.stopPropagation();
                event.preventDefault();
            });

            //override mouseup to nothing
            $(".switch").off("mouseup").on("mouseup", function () {
                return false;
            });
        }, 0);
    }
})(jQuery);
