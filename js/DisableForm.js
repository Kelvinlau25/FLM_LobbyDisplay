function disable_form_elements(ele,opt) {
    $(ele).find(':input').each(function () {
        if (opt == true) {
            $(this).attr('disabled', true);
        } else {
            $(this).removeAttr('disabled');
        };
    });
}