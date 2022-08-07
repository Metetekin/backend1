var Form = {
    SetData: function (formId, data) {
        //Result nesnesinin property bilgilerini toplar.
        var $props = [];
        for (var prop in data) {
            $props.push(prop);
        };
        //Form icerisindeki prop data nesnelerinde dolasir ve result nesnesinin property bilgisi ile eslestigi kontrole veri baglar.
        $("#" + formId + " [name]").each(function (inputIndex, input) {
            $.each($props, function (propIndex, prop) {
                if ($(input).attr('name') == prop) {
                    switch (input.type) {
                        case 'text':
                        case 'number':
                        case 'password':
                            if (data[prop] != null && data[prop] != "") {
                                if ($(input).hasClass('tags')) {
                                    var $tags = data[prop].toString().split(',');
                                    $.each($tags, function (i, tag) {
                                        $(input).addTag(tag);
                                    });
                                } else if ($(input).hasClass('datetime') || $(input).hasClass('datetime2') || $(input).data('type') == "datetime") {
                                    $(input).datepicker("setDate", Data.DateTimeAutoFormat(data[prop]));
                                } else if ($(input).data('type') == "decimal") {
                                    var $decimalValue = data[prop];
                                    if (!isNaN(Number(data[prop]))) {
                                        $decimalValue = Number(data[prop]).toFixed(2);
                                    };
                                    $(input).val($decimalValue).trigger('input');
                                } else {
                                    $(input).val(data[prop]);
                                };
                            };
                            break;
                        case 'select-multiple':
                            if (data[prop] != null && data[prop].length > 0) {
                                if ($(input).hasClass('select2') || $(input).hasClass('select2me')) {
                                    var $multipleSelect = [];
                                    $.each(data[prop], function (i, item) {
                                        if ($(input).data('itemprop') != null && $(input).data('itemprop') != undefined) {
                                            $multipleSelect.push(item[$(input).data('itemprop')]);
                                        } else {
                                            $multipleSelect.push(item);
                                        };
                                    });
                                    $(input).select2('val', $multipleSelect);
                                } else {
                                    //gerekirse yap.
                                };
                            };
                            break;
                        case 'select-one':
                            if (data[prop] != null && data[prop] != "") {
                                if ($(input).hasClass('select2') || $(input).hasClass('select2me')) {
                                    $(input).select2('val', data[prop]);
                                } else {
                                    $(input).val(data[prop]);
                                };
                            };
                            break;
                        case 'textarea':
                            if (data[prop] != null && data[prop] != "") {
                                $(input).val(data[prop]);
                            };
                            break;
                        case 'checkbox':
                            if (data[prop] != null) {

                                $(input).prop('checked', data[prop]);
                                $(input).val(data[prop]);
                                ///*$.uniform.update(input);*/
                                $(input).change();
                            };
                            break;
                        case 'radio':
                            if (data[prop] != null) {
                                var $radio = $('input[type="radio"][name="' + prop + '"][value="' + String(data[prop]) + '"]');
                                $radio.prop('checked', true);
                                if ($radio.closest('label').length > 0) {
                                    $radio.closest('.radio-list').find('span').each(function (i2, span) {
                                        $(span).removeClass('checked');
                                    });
                                    $radio.closest('label').parent().click();
                                    if ($radio.data('firstchecked') == null || $radio.data('firstchecked') == undefined) {
                                        $radio.data('firstchecked', "true");
                                        $radio.change();
                                    };
                                    $.uniform.update($radio);
                                };
                            };
                            break;
                        case 'file':
                            break;
                        case 'hidden':
                            if (data[prop] != null && data[prop] != "") {
                                $(input).val(data[prop]);
                            };
                            break;
                    };
                };
            });
        });
    },
    Reset: function (formId) {
      
        $('#' + formId + ' :input').not(':button').each(function () {
            switch (this.type) {
                case 'text':
                    if ($(this).hasClass('tags')) {
                        var $tagInput = $(this);
                        var $tags = $(this).val().split(',');
                        if ($tags.length > 0) {
                            $.each($tags, function (i, tag) {
                                $($tagInput).removeTag(tag);
                            });
                        };
                    } else {
                        $(this).val('');
                    };
                    break;
                case 'password':
                    $(this).val('');
                    break;
                case 'select-multiple':
                    if ($(this).hasClass('select2') || $(this).hasClass('select2me')) {
                        $(this).select2('val', '');
                    };
                    $(this).empty();
                    break;
                case 'select-one':
                    if ($(this).hasClass('select2') || $(this).hasClass('select2me')) {
                        $(this).select2('val', '');
                    };
                    $(this).empty();
                    break;
                case 'textarea':
                    $(this).val('');
                    break;
                case 'checkbox':
                    $(this).prop('checked', false);
                    $(this)[0].checked = false;
                    /*$.uniform.update($(this));*/
                    break;
                case 'radio':
                    $(this).removeData('firstchecked');
                    if (Boolean($(this).data('default'))) {
                        $(this).prop('checked', true);
                        if ($(this).closest('label').length > 0) {
                            $(this).closest('.radio-list').find('span').each(function () {
                                $(this).removeClass('checked');
                            });
                            $(this).closest('label').parent().click();
                            $(this).change();
                            $.uniform.update($(this));
                        };
                    };
                    break;
                case 'file':
                    $(this).replaceWith($(this).val('').clone(true));
                    break;
                case 'hidden':
                    if ($(this).hasClass('select2') || $(this).hasClass('select2me')) {
                        $(this).select2('val', '');
                    };
                    $(this).val('');
                    break;
            };
        });
        $("#" + formId + " table > tbody").empty();
    },
    Post: function (formId, successEvent, showLoading) {
        try {
            Ajax.Post($('#' + formId).attr('action'), $('#' + formId).serialize(), successEvent, showLoading);
        } catch (e) {
        };
    },
    
};
