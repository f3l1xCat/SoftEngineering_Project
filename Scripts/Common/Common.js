function getFormData($form) {
    var disabled = $form.find(':input:disabled').removeAttr('disabled');//解決disable field被忽略的問題
    var unindexed_array = $form.serializeArray();
    disabled.attr('disabled', 'disabled');
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

function IDCensored(ID) {
    if (ID.length != 10) {
        return ID;
    }
    return ID.substring(0, 3) + '****' + ID.substring(7, 10)
}