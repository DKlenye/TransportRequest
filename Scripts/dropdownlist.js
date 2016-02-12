$(document).ready(function () {
    var config = {
        '.chzn-select': { no_results_text: 'Не найдено совпадений:' },
        '.chzn-select-deselect': { allow_single_deselect: true },
        '.chzn-select-no-single': { disable_search_threshold: 10 },
        '.chzn-select-no-results': { no_results_text: 'Ничего не найдено!' },
        '.chzn-select-width': { width: "95%" }
    }
    for (var selector in config) {
        $(selector).chosen(config[selector]);
    }
});