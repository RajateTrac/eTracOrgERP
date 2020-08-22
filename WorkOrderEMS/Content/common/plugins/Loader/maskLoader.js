function fn_notify() {
    var t = this;
    this.maskloader = {
        start: function (e) {
            e.ob.find(".eliteMask").remove();
            var o = $("<Span></Span>")
                .css({
                    height: e.height,
                    width: e.width,
                    position: e.position ? e.position : "absolute",
                    "background-color": "#000000",
                    filter: "alpha(opacity=20)",
                    opacity: e.opacity ? e.opacity : "0.6",
                    "text-align": "center",
                    "z-index": "20000000",
                    top: "0px",
                })
                .html(
                    '<span class="eliteMask_Span" style=" position: absolute; text-align: center; color: white;  padding: 6px 9px 3px 7px;  height: 33px; border-right-width: 16px;  border-bottom-width: 0px;left:' +
                        e.left +
                        " ;color:  White;position: absolute; text-align: center; top: 45%;border-radius:" +
                        (e.textbrad ? e.textbrad : "2px") +
                        ';"><span>' +
                        e.text +
                        "</span>" +
                        (void 0 == e.requireloader
                            ? '<img style="margin-right: 3px;top:44%;" src="' + $_JqueryLoader + 'Images/Car-Loader.gif">'
                            : void 0 == e.requireloader
                            ? '<img style="margin-right: 3px;top:44%;width=200px;" src="' + $_JqueryLoader + 'Images/Car-Loader.gif">'
                            : "") +
                        "</span>"
                )
                .addClass("eliteMask")
                .prependTo(e.ob);
            return e.ctop && o.css({ top: e.ctop }), t;
        },
        hide: function (e) {
            return (
                e.ob.find(".eliteMask").fadeOut("slow", function () {
                    $(this).remove();
                }),
                t
            );
        },
    };
}
function fn_showMaskloader(t) {
    new fn_notify().maskloader.start({
        ob: $("body"),
        position: "fixed",
        height: "100%",
        text: "&nbsp;&nbsp;&nbsp;&nbsp;" + t + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",
        left: "40%",
        textbackcolor: "Blue",
        textbrad: "10px",
        zindex: $("body").css("z-index") + 1,
        width: "100%",
    });
}
function fn_hideMaskloader() {
    new fn_notify().maskloader.hide({ ob: $("body") });
}
function fn_common() {
    (funthis = this),
        (this.loader = {
            start: function (t) {
                t.ob.find(".ezmasker").remove();
                var e = $("<div></div>")
                    .css({
                        height: t.height,
                        width: t.width,
                        position: t.position ? t.position : "absolute",
                        "background-color": "#FFFFFF",
                        filter: "alpha(opacity=20)",
                        opacity: t.opacity ? t.opacity : "0.6",
                        "text-align": "center",
                        "z-index": "20000000",
                    })
                    .append(
                        '<span style=" position: absolute; text-align: center; color: white;background: ' +
                            (t.textbackcolor ? t.textbackcolor : "transparent") +
                            " ; padding-left: 7px; border-right-width: 12px; padding-right: 9px; border-bottom-width: 0px; padding-bottom: 3px;left:" +
                            t.left +
                            " ;color: white;position: absolute; text-align: center; top: 45%;border-radius:" +
                            (t.textbrad ? t.textbrad : "0px") +
                            ';">' +
                            (void 0 == t.requireloader
                                ? '<img style="margin-right: 3px;top:44%;" src="Images/Car-Loader.gif">'
                                : void 0 == t.requireloader
                                ? '<img style="margin-right: 3px;top:44%;" src="Images/Car-Loader.gif">'
                                : "") +
                            '<br /><span style="color:black">' +
                            t.text +
                            "</span></span>"
                    )
                    .addClass("ezmasker")
                    .prependTo(t.ob);
                return t.ctop && e.css({ top: t.ctop }), funthis;
            },
            hide: function (t) {
                return (
                    t.ob.find(".ezmasker").fadeOut("slow", function () {
                        $(this).remove();
                    }),
                    funthis
                );
            },
        });
}
