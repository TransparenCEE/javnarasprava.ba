var resources = {}; // Global variable.

(function ($) {
    $.getJSON("/Resources/GetResources", function (data) {
        resources = data;
    });
})(jQuery);