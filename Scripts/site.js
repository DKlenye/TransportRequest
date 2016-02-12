
//
//  Custom javascript goes in here
//

$(function () {

    $(document).ready(function () {
        $.datepicker.regional['ru'] = {
            closeText: 'Закрыть',
            prevText: '<Пред',
            nextText: 'След>',
            currentText: 'Сегодня',
            monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
	    'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
            monthNamesShort: ['Янв', 'Фев', 'Мар', 'Апр', 'Май', 'Июн',
	    'Июл', 'Авг', 'Сен', 'Окт', 'Ноя', 'Дек'],
            dayNames: ['воскресенье', 'понедельник', 'вторник', 'среда', 'четверг', 'пятница', 'суббота'],
            dayNamesShort: ['вск', 'пнд', 'втр', 'срд', 'чтв', 'птн', 'сбт'],
            dayNamesMin: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
            weekHeader: 'Не',
            dateFormat: 'dd.mm.yy',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        };
        $.datepicker.setDefaults($.datepicker.regional['ru']);

        $(":input[data-datepicker]").datepicker();
        $("input.knowledgedate").datepicker();

        $(".slingerRow").live("click", function () {
            $("input.knowledgedate").removeClass("hasDatepicker").datepicker();
        });

        //Добавление строки с грузом       
        $("#addCargoRow").click(function (event) {
            event.preventDefault();

            $("a.btn-del-cargo").remove();

            var h;
            
            var lastInputId = $("input.decimal-value:last")[0].attributes["id"].value //Для опреления порядкового номера
            var digitsInInputId = lastInputId.match(/\d+/g);
            var newId = parseInt(digitsInInputId[0]) + 1;

            $.ajax({
                async: false, 
                url: this.href + "/" + newId,
                cache: false,
                success: function (html) { h = html; }
            });
            
            $("#cargo_container").append(h);
            return false;
        });

        //Добавление строки со стропальщиком
        $("#addSlingerRow").click(function (event) {
            event.preventDefault();

            $("a.btn-del-slinger").remove();

            var h;
            
            var lastInputId = $("input.hasDatepicker:last")[0].attributes["id"].value //Для опреления порядкового номера
            var digitsInInputId = lastInputId.match(/\d+/g);
            var newId = parseInt(digitsInInputId[0]) + 1;

            $.ajax({
                async: false,
                url: this.href + "/" + newId,
                cache: false,
                success: function (html) { h = html; }
            });

            $("#slinger_container").append(h);
            return false;
        });

        //Удаление строки с грузом
        $("a.btn-del-cargo").live("click", function () {
            $(this).parents("tr.cargoRow:first").remove();

            //            Добавление кнопки удалить к последнему столбцу
            if ($(".cargoRow").length > 1) {
                var html = "<a href=\"#\" class=\"btn btn-danger btn-mini btn-del-cargo\">удалить</a>";
                var btnDiv = $(".cargoRow:last > td > div:last");

                if (btnDiv.length != 0) {
                    if (btnDiv[0].attributes["id"].value === "row-btn") {
                        btnDiv.append(html);
                    }
                }
                else {

                }
            }
            return false;
        });

        //Удаление строки со стропальщиком
        $("a.btn-del-slinger").live("click", function () {
            $(this).parents("tr.slingerRow:first").remove();

            //            Добавление кнопки удалить к последнему столбцу
            if ($(".slingerRow").length > 1) {
                var html = "<a href=\"#\" class=\"btn btn-danger btn-mini btn-del-slinger\">удалить</a>";
                var btnDiv = $(".slingerRow:last > td > div:last");

                if (btnDiv.length != 0) {
                    if (btnDiv[0].attributes["id"].value === "row-btn") {
                        btnDiv.append(html);
                    }
                }
                else {

                }
            }
            return false;
        });
        ////

        if ($(".cargoRow").length > 1) {
            var html = "<a href=\"#\" class=\"btn btn-danger btn-mini btn-del-cargo\">удалить</a>";
            var btnDiv = $(".cargoRow:last > td > div:last");

            if (btnDiv.length != 0) {
                if (btnDiv[0].attributes["id"].value === "row-btn") {
                    btnDiv.append(html);
                }
            }
        }

        if ($(".slingerRow").length > 1) {
            var html = "<a href=\"#\" class=\"btn btn-danger btn-mini btn-del-slinger\">удалить</a>";
            var btnDiv = $(".slingerRow:last > td > div:last");

            if (btnDiv.length != 0) {
                if (btnDiv[0].attributes["id"].value === "row-btn") {
                    btnDiv.append(html);
                }
            }
        }

        $("input.decimal-value").change(function () {
            var val = this.value;
            this.value = val.replace(".", ",");
        });

        $("#PowerLineExists").click(function () {
            // If checked
            if ($("#PowerLineExists").is(":checked")) {
                //show the hidden div
                $("#lepPowerPermission").show("fast");
            }
            else {
                //otherwise, hide it
                $("#PowerPermission").val("");
                $("#lepPowerPermission").hide("fast");
            }
        });

        if ($("#PowerLineExists").is(":checked")) {
            //show the hidden div
            $("#lepPowerPermission").show("fast");
        }

        $("[id*=DateKnowledge]").css("width", "150px");

        //Files
        $("#addItem").click(function () {
            $.ajax({
                url: this.href,
                cache: false,
                //success: function (hmtl) { $("#tmp_container").append(hmtl); }
                success: function (hmtl) { $("#filesRows").append(hmtl); }
            });            
            return false;
        });

        $("a.deleteRow").live("click", function () {
            $(this).parents("div.fileDelRow:first").remove();
            return false;
        });

        $(".myTooltip").tooltip();

        //filter redirect
        $('#searchForm').submit(function () {
            if ($('#searchDate').val() == "") {
                window.location.replace("http://tr.lan.naftan.by");
            }
        });

    });

});

//a1 = $("input.decimal-value:last")[0].attributes["id"].value

//Kdn fix decimal validation with point and comma
$.validator.methods.number = function (a,b){return this.optional(b)||/^-?(?:\d+|\d{1,3}(?:,\d+)+)?(?:\.\d+)?$/.test(a)}