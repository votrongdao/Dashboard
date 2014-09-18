$(document).ready(function () {
    /*---watermark functions ------------*/
    $(".js_watermark").each(function () {
        $(this).after("");
    });
    $(".js_watermark").focus(function () {
        var wm_text = $(this).next('.js_watermar_text').html();

        if ($(this).val() == wm_text) { $(this).val(''); }
    });
    $(".js_watermark").blur(function () {
        var wm_text = $(this).next('.js_watermar_text').html();
        if ($.trim($(this).val()) == "") { $(this).val(wm_text); }
    });
    /*--------------------------------------*/
});