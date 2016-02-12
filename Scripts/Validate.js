$(document).ready(function () {
    
    jQuery.validator.addMethod('futuredate', function (value, element, params) {
        if (!/Invalid|NaN/.test(new Date(value))) {
            return new Date(value) > new Date();
        }
        return isNaN(value) && isNaN($(params).val()) || (parseFloat(value) > parseFloat($(params).val()));
    }, '');

    jQuery.validator.unobtrusive.adapters.add('futuredate', {}, function (options) {
        options.rules['futuredate'] = true;
        options.messages['futuredate'] = options.message;
    }); 
});