
//
//  Custom javascript goes in here
//

$(function () {

    $(document).ready(function() {
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


        $.timepicker.regional['ru'] = {
            currentText: 'Сейчас',
            closeText: 'Закрыть',
            ampm: false,
            amNames: ['AM', 'A'],
            pmNames: ['PM', 'P'],
            timeFormat: 'hh:mm tt',
            timeSuffix: '',
            timeOnlyTitle: 'Выбор времени',
            timeText: 'Время',
            hourText: 'Часы',
            minuteText: 'Минуты',
            secondText: 'Секунды',
            isRTL: false
        };
        $.timepicker.setDefaults($.timepicker.regional['ru']);

        $(":input[data-datepicker]").datepicker();
        $("input.knowledgedate").datepicker();

        $(":input[data-timepicker]").timepicker({ stepMinute: 5 });


        $(".slingerRow").live("click", function() {
            $("input.knowledgedate").removeClass("hasDatepicker").datepicker();
        });

        //Добавление строки с грузом       
        $("#addCargoRow").click(function(event) {
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
                success: function(html) { h = html; }
            });

            $("#cargo_container").append(h);
            return false;
        });

        //Добавление строки со стропальщиком
        $("#addSlingerRow").click(function(event) {
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
                success: function(html) { h = html; }
            });

            $("#slinger_container").append(h);
            return false;
        });

        //Удаление строки с грузом
        $("a.btn-del-cargo").live("click", function() {
            $(this).parents("tr.cargoRow:first").remove();

            //            Добавление кнопки удалить к последнему столбцу
            if ($(".cargoRow").length > 1) {
                var html = "<a href=\"#\" class=\"btn btn-danger btn-mini btn-del-cargo\">удалить</a>";
                var btnDiv = $(".cargoRow:last > td > div:last");

                if (btnDiv.length != 0) {
                    if (btnDiv[0].attributes["id"].value === "row-btn") {
                        btnDiv.append(html);
                    }
                } else {

                }
            }
            return false;
        });

        //Удаление строки со стропальщиком
        $("a.btn-del-slinger").live("click", function() {
            $(this).parents("tr.slingerRow:first").remove();

            //            Добавление кнопки удалить к последнему столбцу
            if ($(".slingerRow").length > 1) {
                var html = "<a href=\"#\" class=\"btn btn-danger btn-mini btn-del-slinger\">удалить</a>";
                var btnDiv = $(".slingerRow:last > td > div:last");

                if (btnDiv.length != 0) {
                    if (btnDiv[0].attributes["id"].value === "row-btn") {
                        btnDiv.append(html);
                    }
                } else {

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

        $("input.decimal-value").change(function() {
            var val = this.value;
            this.value = val.replace(".", ",");
        });

        $("#PowerLineExists").click(function() {
            // If checked
            if ($("#PowerLineExists").is(":checked")) {
                //show the hidden div
                $("#lepPowerPermission").show("fast");
            } else {
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
        $("#addItem").click(function() {
            $.ajax({
                url: this.href,
                cache: false,
                //success: function (hmtl) { $("#tmp_container").append(hmtl); }
                success: function(hmtl) { $("#filesRows").append(hmtl); }
            });
            return false;
        });

        $("a.deleteRow").live("click", function() {
            $(this).parents("div.fileDelRow:first").remove();
            return false;
        });

        $(".myTooltip").tooltip();

        //filter redirect
        $('#searchForm').submit(function() {
            if ($('#searchDate').val() == "" ) {
                window.location.replace("http://tr.lan.naftan.by");
            }
        });
        
        $('#searchForm').submit(function () {
            if ($('#searchUserFio').val() == "") {
                window.location.replace("http://tr.lan.naftan.by/International");
            }
        });


        $('.Select_WithInvoice').change(function() {

            var agreementId = 1;
            var withInvoice = $(".Select_WithInvoice");

            if (withInvoice.length > 0) {
                agreementId = $(".Select_WithInvoice").val() == "True" ? 3 : 2;
            }

            $.post("/Freight/_FindDirection", {
                    agreementId: agreementId,
                    departmentGroupId: $(".Select_Department").val()
                },
                function(data) {
                    $("#Request_DirectionId").html(data);
                    $("#Request_DirectionId").trigger("liszt:updated");
                }
            );

            $.post("/Freight/_FindPurpose", {
                    agreementId: agreementId,
                    departmentGroupId: $(".Select_Department").val()
                },
                function(data) {
                    $("#Request_AgreementPurposeId").html(data);
                    $("#Request_AgreementPurposeId").trigger("liszt:updated");
                }
            );
        });

        $('.Select_Department').change(function() {

            var agreementId = 1;
            var withInvoice = $(".Select_WithInvoice");


            if ($('.Select_Way').length > 0) {
                agreementId = 3;
            }


            if (withInvoice.length > 0) {
                agreementId = $(".Select_WithInvoice").val() == "True" ? 3 : 2;
            }

            $.post("/Freight/_FindDirection", {
                    agreementId: agreementId,
                    departmentGroupId: $(".Select_Department").val()
                },
                function(data) {
                    $("#Request_DirectionId").html(data);
                    $("#Request_DirectionId").trigger("liszt:updated");
                }
            );

            $.post("/Freight/_FindPurpose", {
                    agreementId: agreementId,
                    departmentGroupId: $(".Select_Department").val()
                },
                function(data) {
                    $("#Request_AgreementPurposeId").html(data);
                    $("#Request_AgreementPurposeId").trigger("liszt:updated");
                }
            );

        });

        var report = function(reportName) {
            var reportUrl = 'http://db2.lan.naftan.by/ReportServer?/PublicReports/' + reportName + '&rs%3AClearSession=true&rs%3AFormat=Word&requestId=' + window.location.href.split('/').slice(-1)[0];
            location.href = reportUrl;
        };


        $('a#ExportInternational').click(function() {
            report("RequestInternational");
        });

        $('a#ExportInternational1').click(function() {
            report("RequestInternational1");
        });
        $('a#ExportInternational2').click(function() {
            report("RequestInternational2");
        });
        $('a#ExportInternational3').click(function() {
            report("RequestInternational3");
        });
        $('a#ExportInternational4').click(function() {
            report("RequestInternational4");
        });

        if ($('.Select_Way').length > 0) {


            var days = {
                holidays: [
                    "02.01.2017", "08.03.2017", "24.04.2017", "25.04.2017", "08.05.2017", "09.05.2017", "03.07.2017", "06.11.2017", "07.11.2017", "25.12.2017"
                ],
                workdays: [
                    "21.01.2017", "29.04.2017", "06.05.2017", "04.11.2017"
                ]
            };


            var toDate = function(dateStr) {
                var parts = dateStr.split(".");
                return new Date(parts[2], parts[1] - 1, parts[0]);
            };

            var isWork = function(e) {

                var flag = false;

                $.each(days.holidays, function(i, v) {
                    var d = toDate(v);
                    if (!(d < e || d > e)) {
                        flag = true;
                        return false;
                    }
                });

                if (flag) {
                    return [false, ""];
                }

                $.each(days.workdays, function(i, v) {
                    var d = toDate(v);
                    if (!(d < e || d > e)) {
                        flag = true;
                        return false;
                    }
                });

                if (flag) {
                    return [true, ""];
                }

                var t = e.getDay();
                return [t > 0 && t < 6, ""];
            };


            var nextWorkDate = function(date, skipWorkDays) {

                var d = new Date(date.getTime());

                while (skipWorkDays) {
                    d.setDate(d.getDate() + 1);
                    if (isWork(d)[0]) {
                        skipWorkDays--;
                    }
                }

                return d;

            };


            var minDate = nextWorkDate(new Date(), 4);

            $('#Request_RequestDate')
                .datepicker("option", {
                    minDate: minDate,
                    beforeShowDay: isWork,
                    onClose: function() {

                        $("#DeliveryDate").datepicker(
                            "option",
                            {
                                minDate: nextWorkDate($('#Request_RequestDate').datepicker("getDate"), 2)
                            }
                        );
                        
                        $("#DeliveryDateEnd").datepicker(
                            "option",
                            {
                                minDate: nextWorkDate($('#Request_RequestDate').datepicker("getDate"), 2)
                            }
                        );
                        
                        $("#RequestDateEnd").datepicker(
                            "option",
                            {
                                minDate: $('#Request_RequestDate').datepicker("getDate")
                            }
                        );

                    }
                }).datepicker("setDate", (minDate)).attr('readOnly', 'true');

            $('#RequestDateEnd')
            .datepicker("option", {
                minDate: minDate,
                beforeShowDay: isWork
            }).datepicker("setDate", (minDate)).attr('readOnly', 'true');
            

            $("#DeliveryDate")
                .datepicker("option", {
                    minDate: nextWorkDate(minDate, 2),
                    beforeShowDay: isWork,
                    onClose: function () {

                        $("#DeliveryDateEnd").datepicker(
                            "option",
                            {
                                minDate: $('#DeliveryDate').datepicker("getDate")
                            }
                        );

                    }
                }).datepicker("setDate", (nextWorkDate(minDate, 2))).attr('readOnly', 'true');
            
            $("#DeliveryDateEnd")
                .datepicker("option", {
                    minDate: nextWorkDate(minDate, 2),
                    beforeShowDay: isWork
                }).datepicker("setDate", (nextWorkDate(minDate, 2))).attr('readOnly', 'true');


            $('.Select_Way').change(function() {
                var isBY = $(".Select_Way").val() == "Внутри Республики Беларусь";
                isBY ? $(".hide_way").hide(300) : $(".hide_way").show(300);
                isBY ? $(".show_way").show(300) : $(".show_way").hide(300);
            });


            var isBY = $(".Select_Way").val() == "Внутри Республики Беларусь";
            isBY ? $(".hide_way").hide() : $(".hide_way").show();
            isBY ? $(".show_way").show() : $(".show_way").hide();

            var loadings = $('#LoadingType').val();

            if (loadings) {
                $("#LoadingTypeSelect").val(loadings.split(",")).trigger('liszt:updated');
            }

            $("#LoadingTypeSelect").change(function () {
                var val = $("#LoadingTypeSelect").val() || [];
                $('#LoadingType').val(val.join(','));
            });


        }

        

    });

});

//a1 = $("input.decimal-value:last")[0].attributes["id"].value

//Kdn fix decimal validation with point and comma
$.validator.methods.number = function (a, b) { return this.optional(b) || /^-?(?:\d+|\d{1,3}(?:,\d+)+)?(?:\.\d+)?$/.test(a) }



