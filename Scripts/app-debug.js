// Шаги алгоритма ECMA-262, 5-е издание, 15.4.4.19
// Ссылка (en): http://es5.github.com/#x15.4.4.19
// Ссылка (ru): http://es5.javascript.ru/x15.4.html#x15.4.4.19
if (!Array.prototype.map) {

    Array.prototype.map = function (callback, thisArg) {

        var T, A, k;

        if (this == null) {
            throw new TypeError(' this is null or not defined');
        }

        // 1. Положим O равным результату вызова ToObject с передачей ему
        //    значения |this| в качестве аргумента.
        var O = Object(this);

        // 2. Положим lenValue равным результату вызова внутреннего метода Get
        //    объекта O с аргументом "length".
        // 3. Положим len равным ToUint32(lenValue).
        var len = O.length >>> 0;

        // 4. Если вызов IsCallable(callback) равен false, выкидываем исключение TypeError.
        // Смотрите (en): http://es5.github.com/#x9.11
        // Смотрите (ru): http://es5.javascript.ru/x9.html#x9.11
        if (typeof callback !== 'function') {
            throw new TypeError(callback + ' is not a function');
        }

        // 5. Если thisArg присутствует, положим T равным thisArg; иначе положим T равным undefined.
        if (arguments.length > 1) {
            T = thisArg;
        }

        // 6. Положим A равным новому масиву, как если бы он был создан выражением new Array(len),
        //    где Array является стандартным встроенным конструктором с этим именем,
        //    а len является значением len.
        A = new Array(len);

        // 7. Положим k равным 0
        k = 0;

        // 8. Пока k < len, будем повторять
        while (k < len) {

            var kValue, mappedValue;

            // a. Положим Pk равным ToString(k).
            //   Это неявное преобразование для левостороннего операнда в операторе in
            // b. Положим kPresent равным результату вызова внутреннего метода HasProperty
            //    объекта O с аргументом Pk.
            //   Этот шаг может быть объединён с шагом c
            // c. Если kPresent равен true, то
            if (k in O) {

                // i. Положим kValue равным результату вызова внутреннего метода Get
                //    объекта O с аргументом Pk.
                kValue = O[k];

                // ii. Положим mappedValue равным результату вызова внутреннего метода Call
                //     функции callback со значением T в качестве значения this и списком
                //     аргументов, содержащим kValue, k и O.
                mappedValue = callback.call(T, kValue, k, O);

                // iii. Вызовем внутренний метод DefineOwnProperty объекта A с аргументами
                // Pk, Описатель Свойства
                // { Value: mappedValue,
                //   Writable: true,
                //   Enumerable: true,
                //   Configurable: true }
                // и false.

                // В браузерах, поддерживающих Object.defineProperty, используем следующий код:
                // Object.defineProperty(A, k, {
                //   value: mappedValue,
                //   writable: true,
                //   enumerable: true,
                //   configurable: true
                // });

                // Для лучшей поддержки браузерами, используем следующий код:
                A[k] = mappedValue;
            }
            // d. Увеличим k на 1.
            k++;
        }

        // 9. Вернём A.
        return A;
    };
}


// Шаги алгоритма ECMA-262, 5-е издание, 15.4.4.18
// Ссылка (en): http://es5.github.io/#x15.4.4.18
// Ссылка (ru): http://es5.javascript.ru/x15.4.html#x15.4.4.18
if (!Array.prototype.forEach) {

    Array.prototype.forEach = function (callback, thisArg) {

        var T, k;

        if (this == null) {
            throw new TypeError(' this is null or not defined');
        }

        // 1. Положим O равным результату вызова ToObject passing the |this| value as the argument.
        var O = Object(this);

        // 2. Положим lenValue равным результату вызова внутреннего метода Get объекта O с аргументом "length".
        // 3. Положим len равным ToUint32(lenValue).
        var len = O.length >>> 0;

        // 4. Если IsCallable(callback) равен false, выкинем исключение TypeError.
        // Смотрите: http://es5.github.com/#x9.11
        if (typeof callback !== 'function') {
            throw new TypeError(callback + ' is not a function');
        }

        // 5. Если thisArg присутствует, положим T равным thisArg; иначе положим T равным undefined.
        if (arguments.length > 1) {
            T = thisArg;
        }

        // 6. Положим k равным 0
        k = 0;

        // 7. Пока k < len, будем повторять
        while (k < len) {

            var kValue;

            // a. Положим Pk равным ToString(k).
            //   Это неявное преобразование для левостороннего операнда в операторе in
            // b. Положим kPresent равным результату вызова внутреннего метода HasProperty объекта O с аргументом Pk.
            //   Этот шаг может быть объединён с шагом c
            // c. Если kPresent равен true, то
            if (k in O) {

                // i. Положим kValue равным результату вызова внутреннего метода Get объекта O с аргументом Pk.
                kValue = O[k];

                // ii. Вызовем внутренний метод Call функции callback с объектом T в качестве значения this и
                // списком аргументов, содержащим kValue, k и O.
                callback.call(T, kValue, k, O);
            }
            // d. Увеличим k на 1.
            k++;
        }
        // 8. Вернём undefined.
    };
}


// Шаги алгоритма ECMA-262, 5-е издание, 15.4.4.14
// Ссылка (en): http://es5.github.io/#x15.4.4.14
// Ссылка (ru): http://es5.javascript.ru/x15.4.html#x15.4.4.14
if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (searchElement, fromIndex) {
        var k;

        // 1. Положим O равным результату вызова ToObject с передачей ему
        //    значения this в качестве аргумента.
        if (this == null) {
            throw new TypeError('"this" is null or not defined');
        }

        var O = Object(this);

        // 2. Положим lenValue равным результату вызова внутреннего метода Get
        //    объекта O с аргументом "length".
        // 3. Положим len равным ToUint32(lenValue).
        var len = O.length >>> 0;

        // 4. Если len равен 0, вернём -1.
        if (len === 0) {
            return -1;
        }

        // 5. Если был передан аргумент fromIndex, положим n равным
        //    ToInteger(fromIndex); иначе положим n равным 0.
        var n = +fromIndex || 0;

        if (Math.abs(n) === Infinity) {
            n = 0;
        }

        // 6. Если n >= len, вернём -1.
        if (n >= len) {
            return -1;
        }

        // 7. Если n >= 0, положим k равным n.
        // 8. Иначе, n<0, положим k равным len - abs(n).
        //    Если k меньше нуля 0, положим k равным 0.
        k = Math.max(n >= 0 ? n : len - Math.abs(n), 0);

        // 9. Пока k < len, будем повторять
        while (k < len) {
            // a. Положим Pk равным ToString(k).
            //   Это неявное преобразование для левостороннего операнда в операторе in
            // b. Положим kPresent равным результату вызова внутреннего метода
            //    HasProperty объекта O с аргументом Pk.
            //   Этот шаг может быть объединён с шагом c
            // c. Если kPresent равен true, выполним
            //    i.  Положим elementK равным результату вызова внутреннего метода Get
            //        объекта O с аргументом ToString(k).
            //   ii.  Положим same равным результату применения
            //        Алгоритма строгого сравнения на равенство между
            //        searchElement и elementK.
            //  iii.  Если same равен true, вернём k.
            if (k in O && O[k] === searchElement) {
                return k;
            }
            k++;
        }
        return -1;
    };
}

if (!Array.prototype.every) {
    Array.prototype.every = function (callbackfn, thisArg) {
        'use strict';
        var T, k;

        if (this == null) {
            throw new TypeError('this is null or not defined');
        }

        // 1. Положим O равным результату вызова ToObject над значением
        //    this, переданным в качестве аргумента.
        var O = Object(this);

        // 2. Положим lenValue равным результату вызова внутреннего метода Get
        //    объекта O с аргументом "length".
        // 3. Положим len равным ToUint32(lenValue).
        var len = O.length >>> 0;

        // 4. Если IsCallable(callbackfn) равен false, выкинем исключение TypeError.
        if (typeof callbackfn !== 'function') {
            throw new TypeError();
        }

        // 5. Если thisArg присутствует, положим T равным thisArg; иначе положим T равным undefined.
        if (arguments.length > 1) {
            T = thisArg;
        }

        // 6. Положим k равным 0.
        k = 0;

        // 7. Пока k < len, будем повторять
        while (k < len) {

            var kValue;

            // a. Положим Pk равным ToString(k).
            //   Это неявное преобразование для левостороннего операнда в операторе in
            // b. Положим kPresent равным результату вызова внутреннего метода
            //    HasProperty объекта O с аргументом Pk.
            //   Этот шаг может быть объединён с шагом c
            // c. Если kPresent равен true, то
            if (k in O) {

                // i. Положим kValue равным результату вызова внутреннего метода Get
                //    объекта O с аргументом Pk.
                kValue = O[k];

                // ii. Положим testResult равным результату вызова внутреннего метода Call
                //     функции callbackfn со значением T в качестве this и списком аргументов,
                //     содержащим kValue, k и O.
                var testResult = callbackfn.call(T, kValue, k, O);

                // iii. Если ToBoolean(testResult) равен false, вернём false.
                if (!testResult) {
                    return false;
                }
            }
            k++;
        }
        return true;
    };
}app = {};

webix.ready(function() {
    webix.ui({
        rows: [
            {
                view: 'view.toolbar',
                id: 'toolbar',
                on: {
                    'activate': onToolbarActivate
                }
            },
            {
                cols: [
                    {
                        id: "sidebar",
                        view: "sidebar",
                        collapsed: true,
                        data: [
                            {
                                id: "viewscroll.tablesettings",
                                icon: "table",
                                value: "Data log settings"
                            },
                            {
                                id: "viewscroll.logview",
                                icon: "search",
                                value: "View log data"
                            }
                        ],
                        on: {
                            onItemClick: function(id) {
                                $$(id).show(false, false);
                            }
                        }
                    },
                    {
                        id: 'viewscroll',
                        cells: [
                            {
                                id: 'viewscroll.tablesettings',
                                cols: [
                                    {
                                        view: 'view.table_list',
                                        id: 'table_list',
                                        icons: app.settings.icons,
                                        on: {
                                            'tableSelect': onTableSelect
                                        }
                                    },
                                    { view: 'resizer' },
                                    {
                                        view: 'view.table_editor', id: 'table_editor', icons: app.settings.icons,
                                        on: {
                                            'onTableChange': function (data) {
                                                $$('table_list').refreshItem(data);
                                            }
                                        }
                                    }
                                ]
                            },
                            {
                                id: 'viewscroll.logview',
                                cols: [
                                    {
                                        view: 'view.logtable',
                                        id: 'logtable',
                                        gravity: 1.5,
                                        on: {
                                            'onSelectChange': onLogTableSelect
                                        }
                                    },
                                    { view: 'resizer' },
                                    {
                                        view: 'view.logdetails',
                                        id: 'logdetails'
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        ]
    });

    $$('toolbar').restoreState();
    $$('sidebar').select('viewscroll.tablesettings');
});

var onToolbarActivate = function (logCfg) {
    
    if (!logCfg) {
        app.connection = null;
        var grid = $$("table.table_list");
        grid.disable();
        grid.clearAll();
        $$('table.log').clearAll();
        return;
    }

    app.connection = logCfg;
    $$('table_list').load(logCfg);
    $$('logtable').load(logCfg);
};

var onTableSelect = function(table) {
    $$("table_editor").load(app.connection, table);
};

var onLogTableSelect = function (data) {
    $$("logdetails").load(data);
};app.i18n = {

};app.settings = {
    icons: {
        'insert': 'fa-plus-square-o ',
        'update': 'fa-edit',
        'delete': 'fa-minus-square-o'
    },
    url: {
        tableList: "ChangeLog/Handlers/SelectTableList.ashx"
    }
};webix.protoUI({
    name: 'view.logdetails',
    $init: function(config) {
        webix.extend(this.defaults, {
            disabled:true,
            rows: [
                {
                    id:"table.logdetails",
                    view: 'datatable',
                    resizeColumn: true,
                    columns: [
                        { id: "Column", header: "Column", fillspace: 0.5 },
                        { id: "OldValue", header: "OldValue", fillspace: 1 },
                        { id: "NewValue", header: "NewValue", fillspace: 1 }
                    ]
                }
            ]
        });
    },

    load: function (data) {

        if (data) {
            this.enable();
            this.parseXml(data);
        } else {
            this.clear();
            this.disable();
        }
    },

    parseXml:function(data) {
        var parseFn = this["parse_" + data.changeType];
        var dataObject = parseFn.call(this, data.description);
        var table = $$('table.logdetails');
        table.clearAll();
        table.parse(dataObject);
    },
    
    xmlToObject:function(xml){
        var doc = webix.DataDriver.xml.toObject(xml);
        var tag = doc.childNodes[0];
        var a = tag.attributes;

        var rezult = {};
        if (a && a.length) {
            for (var i = 0; i < a.length; i++) {
                rezult[a[i].name] = a[i].value;
            }
        }
        return rezult;
    },

    parse_I: function(xml) {
        var obj = this.xmlToObject(xml);
        var rezult = [];
        for (var i in obj) {
            rezult.push({
                Column:i,
                NewValue:obj[i]
            });
        }
        return rezult;
    },

    parse_U: function (xml) {
        var splitIndex = xml.indexOf('>') + 1;
        var _new = this.xmlToObject(xml.substring(0, splitIndex));
        var _old = this.xmlToObject(xml.substring(splitIndex, xml.length));

        var rezult = [];
        var map = {};

        for (var i in _old) {
            map[i] = _old[i];
        }

        for (var i in _new) {
            if (!map[i]) map[i] = _new[i];
        }

        for (var i in map) {
            rezult.push({
                Column: i,
                OldValue: webix.isUndefined(_old[i]) ? 'NULL' : _old[i],
                NewValue: webix.isUndefined(_new[i]) ? 'NULL' : _new[i],
                $css: _old[i] == _new[i] ? "" : { "background-color": "papayawhip" }
            });
        }

        return rezult;
    },

    parse_D:function(xml) {
        var obj = this.xmlToObject(xml);
        var rezult = [];
        for (var i in obj) {
            rezult.push({
                Column: i,
                OldValue: obj[i]
            });
        }
        return rezult;
    },
    
    clear:function() {
        $$('table.logdetails').clearAll();
    }

}, webix.ui.layout);webix.protoUI({
    name: 'view.logtable',
    $init: function(cfg) {

        var dateToString = webix.Date.dateToStr("%d.%m.%Y %H:%i:%s");
        var me = this,
            bind = function(fn) {
                return webix.bind(me[fn], me);
            };

        webix.extend(this.defaults, {
            rows: [ 
                {
                    view: 'datatable',
                    id: 'table.log',
                    resizeColumn:true,
                    columns: [
                        { id: "idChangeLog", header: ["Id", {content: "serverFilter"}], width: 80, sort: 'server' },
                        { id: "date", header: ["Date",{content: "serverFilter"}], width: 150, sort: 'server' },
                        { id: "user", header: ["UserName", { content: "serverFilter"}], width: 260},
                        {
                            id: "changeType",
                            header: [
                                "ChangeType", {
                                    content: "serverSelectFilter",
                                    options: [{ id: "", value: "All" }, { id: "I", value: "Insert" }, { id: "U", value: "Update" }, { id: "D", value: "Delete"}]
                                }
                            ],
                            width: 100,
                            template: function (obj) {
                                var o = {
                                    I: 'Insert',
                                    U: 'Update',
                                    D: 'Delete'
                                };
                                return o[obj.changeType];
                            }
                        },
                        { id: "table", header: ["TableName", { content: "serverFilter"}], fillspace: true },
                        { id: "idString", header: ["IdString", { content: "serverFilter" }], width: 100 }
                    ],
                    dataThrottle: 500,
                    scheme: {
                        $init: function(obj) {
                            obj.date = dateToString(new Date(obj.date));
                        }
                    },
                    on: {
                        onBeforeLoad: bind("_onBeforeLoad"),
                        onAfterLoad: bind("_onAfterLoad"),
                        onSelectChange: bind("_onSelectChange")
                    },
                    select: 'row',
                    pager: 'pagerA'
                },
                {
                    paddingY: 7,
                    rows: [
                        {
                            view: "pager",
                            id: "pagerA",
                            template: "{common.first()} {common.prev()} {common.pages()} {common.next()} {common.last()} Page {common.page()} from #limit#",
                            size: 50,
                            group: 5
                        }
                    ]
                }
            ]
        });
    },


    _onBeforeLoad: function() {
        var grid = $$("table.log");

        grid.disable();
        grid.showOverlay("Loading <span class='webix_icon fa-spinner fa-spin'></span>");
    },
    _onAfterLoad: function() {
        var grid = $$("table.log");

        grid.hideOverlay();
        grid.enable();
    },

    _onSelectChange: function() {
        var grid = $$("table.log");
        var item = grid.getSelectedItem();
        this.callEvent("onSelectChange", [item]);
    },

    load: function(params) {

        var _params = webix.copy(params);
        _params.count = 50;

        $$('table.log').clearAll();

        $$('table.log').define("url", {
                $proxy: true,
                source: "/Handlers/SelectChangeLog.ashx",
                params: _params,
                load: function(view, callback, params) {
                    params = webix.extend(params || {}, this.params || {}, true);
                    webix.ajax().bind(view).post(this.source, params, callback);
                }
        });
    }

}, webix.ui.layout);webix.protoUI({
    name: 'view.table_editor',
    operations:[
        { id: "Insert", value: "<span class='webix_icon " + app.settings.icons["insert"] + "'></span><span style='padding-left: 4px'>Insert</span>" },
        { id: "Update", value: "<span class='webix_icon " + app.settings.icons['update'] + "'></span><span style='padding-left: 4px'>Update</span>" },
        { id: "Delete", value: "<span class='webix_icon " + app.settings.icons['delete'] + "'></span><span style='padding-left: 4px'>Delete</span>" }
    ],
    $init: function (config) {
        var icons = config.icons || {};
        var me = this;
        var bind = function(fn) {
            return webix.bind(me[fn], me);
        };
        
        webix.extend(this.defaults, {
            disabled: true,
            rows: [
                {
                    view: 'toolbar',
                    cols: [
                        {
                            view: "segmented",
                            id:"segmented.operation",
                            optionWidth: 120,
                            value: "None",
                            options: [
                                
                            ],
                            on: {
                                onChange: bind("onOperationChange")
                            }
                        }
                    ]
                },
                {
                    rows: [
                        {
                            view: 'segmented',
                            id: 'segmented.trigger',
                            multiview: true,
                            value: 'layout.columns',
                            options: [
                                { value: "<span class='webix_icon fa-columns'></span><span style='padding-left: 4px'>Columns</span>", id: 'layout.columns' },
                                { value: "<span class='webix_icon fa-file-text-o'></span><span style='padding-left: 4px'>TriggerText</span>", id: 'layout.triggertext' }
                            ]
                        },
                        {
                            cells: [
                                {
                                    id: 'layout.columns',
                                    rows: [{
                                            view: 'datatable',
                                            id: 'table.columns',
                                            columns: [
                                                { id: "Checked", header: { content: "masterCheckbox", css: "center" }, css:"center", template: "{common.checkbox()}", width: 40 },
                                                {
                                                    id: "ColumnName",
                                                    fillspace: true,
                                                    header: "Column Name",
                                                    template: function (obj) {
                                                        if (!obj.IsKey) return obj.ColumnName;
                                                        return "<span class='webix_icon fa-key' style='color:gold'></span> "+obj.ColumnName;
                                                    }
                                                }
                                            ],
                                            on: {
                                                'onCheck': bind("_onCheck")
                                            }
                                        },
                                        {
                                            height: 45,
                                            cols: [
                                                { view: "button", type: "iconButton", label: "Save", icon: "floppy-o", width: 95, on: { onItemClick: bind("saveTable")} },
                                                {}
                                            ]
                                        }
                                    ]
                                },
                                {
                                    id: "layout.triggertext",
                                    rows: [
                                        {
                                            view: 'textarea',
                                            id:'text.trigger'
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        });
    },

    load: function (connection, table) {
    if (table) {
            this.enable();
            this.params = webix.copy(connection);
            this.params["TableName"] = table;
            this._loadTable();
        } else {
            this.clear();
            this.disable();
        }
    },

    _loadTable: function () {

        var me = this;
        var table = $$("table.columns");

        me.clear();

        webix.ajax().post('/Handlers/SelectTable.ashx', this.params, function (data) {
            me.fillData(data);
        });
    },

    _getOptions : function() {
        var segmented = $$("segmented.operation");
        var settings = segmented._settings || segmented.s;
        return settings.options;
    },

    _setOptions:function(options) {
        var segmented = $$("segmented.operation");
        var settings = segmented._settings || segmented.s;
        settings.options = options;
    },
    _refreshOptions:function() {
        $$("segmented.operation").refresh();
    },

    clear: function () {
        this.setSegmentedTableName();
        this.clearView();
        delete this.data;

        this._setOptions([]);
        this._refreshOptions();
    },

    clearView:function() {
        $$("table.columns").clearAll();
        $$("text.trigger").setValue("");
    },

    fillData: function (data) {
        data = JSON.parse(data);
        this.data = data;

        this.records = {
            Insert: this.buildOperationRecord("Insert"),
            Update: this.buildOperationRecord("Update"),
            Delete: this.buildOperationRecord("Delete")
        };

        this.setSegmentedTableName(data.TableName);
        this.setOptions();
    },

    buildOperationRecord:function(operation) {
        var data = this.data;
        var trigger = data[operation];
        var rezult = [];

        var isKey = function (column) {
            return data.KeyColumns.indexOf(column) != -1;
        };

        var isChecked = function (column) {
            if (!trigger || !trigger.Columns) return false;
            return trigger.Columns.indexOf(column) != -1;
        };

        data.Columns.forEach(function (column) {
            rezult.push({
                ColumnName: column,
                IsKey: isKey(column),
                Checked: isChecked(column)
            });
        });

        return rezult;
    },

    setOptions:function() {

        var data = this.data;
        var optionValue;
        var options = [];
        var segment = $$('segmented.operation');

        this.eachOperations(function(operation) {
            var trigger = data[operation.id];
            if (!trigger) {
                options.push(operation); //todo ��������� �����
            } else {
                options.push(operation);
                if (!optionValue) {
                    optionValue = operation.id;
                }
            }
        });

        this._setOptions(options);
        this._refreshOptions();

        optionValue = optionValue || "Insert";
        segment.setValue(optionValue);
        this.onOperationChange(optionValue);
    },
    
    fillTriggerText:function(operation) {
        var trigger = this.data[operation];
        var triggerText = "";
        if (trigger && trigger.TriggerText) {
            triggerText = trigger.TriggerText;
        }
        $$('text.trigger').setValue(triggerText);
    },

    setSegmentedTableName: function (tableName) {

        var segmented = $$('segmented.trigger'),
            settings = segmented._settings || segmented.s;

        var columnOption = settings.options[0];
        columnOption.value = "<span class='webix_icon fa-columns'></span><span style='padding-left: 4px'>Columns " + (tableName || "") + "</span>";
        $$('segmented.trigger').refresh();
    },

    onOperationChange: function (operation) {
        this.clearView();
        $$("table.columns").parse(this.records[operation]);
        this.fillTriggerText(operation);
    },

    eachOperations: function (fn) {
        this.operations.forEach(fn);
    },

    _onCheck: function () {
        var record = this.operations[$$('segmented.operation').getValue()];
    },

    saveTable:function() {
        var data = this.data;
        var me = this;

        ["Insert", "Update", "Delete"].forEach(function(e) {
            var record = me.records[e];
            var columns = [];
            record.forEach(function(_column) {
                if (_column.Checked) {
                    columns.push(_column.ColumnName);
                }
            });
            if (columns.length == 0) {
                delete data[e];
            } else {
                if (!data[e]) {
                    data[e] = {
                        Operation:e,
                        TableName: data.TableName
                    };
                }
                data[e].Columns = columns;
            }
        });

        this.saveData();

    },

    saveData: function () {

        this.disable();

        var data = this.data;
        var me = this;
        var params = webix.copy(this.params);
        params.Table = data;
        webix.ajax().post('/Handlers/SaveTable.ashx', params, webix.bind(me.onSave, me));
    },

    onSave: function (data) {
        var dto = JSON.parse(data);
        this.callEvent("onTableChange", [dto]);
        
        this.fillData(data);
        this.enable();
    }


}, webix.ui.layout);webix.protoUI({
    name: 'view.table_list',
    $init: function(config) {

        var icons = config.icons || {};
        var me = this,
            bind = function(fn) {
                return webix.bind(me[fn], me);
            };

        webix.extend(this.defaults, {
            rows: [
                {
                    view: "datatable",
                    id: "table.table_list",
                    select: "row",
                    columns: [
                        {
                            id: 'Operations',
                            header: [
                                "Operations",
                                {
                                    content: "selectFilter",
                                    options: [{ id: "All", value: "All" }, { id: "Logging", value: "Logging" }, { id: "Not Logging", value: "Not Logging" }],
                                    compare: function(value, filter) {
                                        if (filter == "All") return true;
                                        return filter == "Logging" ? !!value : !value;
                                    }
                                }
                            ],
                            width: 130,
                            template: function(obj) {

                                if (!obj.Operations) return "";

                                var buttonTpl = webix.template("<span class='webix_icon #icon#' style='cursor:pointer'></span>");
                                var operations = obj.Operations;

                                return operations.map(function(o) {
                                    return buttonTpl({ icon: icons[o] || 'fa-question' });
                                }).join("");

                            }
                        },
                        { id: 'Name', fillspace: true, header: ["Table", { content: "textFilter" }], sort: "string" }
                    ],
                    on: {
                        onBeforeLoad: bind("_onBeforeLoad"),
                        onAfterLoad: bind("_onAfterLoad"),
                        onSelectChange: bind("_onSelectChange")
                    }
                }
            ]
        });
    },
    _onBeforeLoad: function() {
        var grid = $$("table.table_list");

        grid.disable();
        grid.clearAll();
        grid.showOverlay("Loading <span class='webix_icon fa-spinner fa-spin'></span>");
    },
    _onAfterLoad: function() {
        var grid = $$("table.table_list");

        grid.filterByAll();
        grid.hideOverlay();
        grid.enable();
    },

    _onSelectChange: function() {
        var grid = $$("table.table_list");
        var item = grid.getSelectedItem();
        var table = '';
        if (item) {
            table = item.Name;
        }
        this.callEvent("tableSelect", [table]);
    },

    load: function(params) {
        $$("table.table_list").load("post->"+app.settings.url.tableList, null, params);
    },

    refreshItem: function (item) {

        var grid = $$("table.table_list");
        var dataset = grid.data;

        var record = dataset.find(function (obj) {
            return obj.Name == item.TableName;
        },true);

        var operations = [];
        ["Insert", "Update", "Delete"].forEach(function(o) {
            if (item[o]) {
                operations.push(o.toLowerCase());
            }
        });

        if (operations.length == 0) {
            operations = null;
        }

        dataset.updateItem(record.id, { Operations: operations });
    }

}, webix.ui.layout);webix.protoUI({
    name: 'view.toolbar',
    defaultLogTable: "ChangeLog",
    stateName:'toolbar',
    minHeight: 78,
    maxHeight: 99,
    inserting: false,
    $init: function () {

        var me = this,
            bind = function (fn) {
                return webix.bind(me[fn], me);
            };

        webix.extend(this.defaults, {
            height: me.minHeight,
            cols: [
                {
                    id: 'button.add',
                    view: 'icon',
                    icon: 'plus-circle',
                    tooltip: 'Add database',
                    click: bind("toggleAdd")
                },
                {
                    id: 'layout.add',
                    hidden: true,
                    cols: [{
                        width: 250,
                        id:'layout.add.text',
                        rows: [
                                {
                                    id: 'text.server',
                                    label: 'server',
                                    dataIndex: 'server',
                                    view: 'text',
                                    height: 30,
                                    nextFocus: 'text.database',
                                    on: { 'onKeyPress': bind("onTextKeyPress") }
                                },
                                {
                                    id: 'text.database',
                                    dataIndex: 'database',
                                    label: 'database',
                                    view: 'text',
                                    height: 30,
                                    nextFocus: 'text.logtable',
                                    on: { 'onKeyPress': bind("onTextKeyPress") }
                                },
                                {
                                    id: 'text.logtable',
                                    dataIndex:'logtable',
                                    label: 'log table',
                                    view: 'text',
                                    value:this.defaultLogTable,
                                    height: 30,
                                    on: { 'onKeyPress': bind("onTextKeyPress") }
                                }
                            ]
                    }, {
                        width: 100,
                        rows: [
                            {},
                            { view: 'button', type: 'icon', height: 30, label: 'Cancel', icon: 'ban', click: bind("toggleAdd") },
                            { view: 'button', type: 'icon', height: 30, label: 'Ok', icon: 'check', click: bind("confirmAdd") }
                        ]
                    }]
                }]
        });

    },
   
    onTextKeyPress: function (key, event) {
    
        switch (key) {
        //ESC         
        case 27:
            {
                this.toggleAdd();
                return false;
            }
        //ENTER
        case 13:
            {
                var id = event.srcElement.parentNode.parentNode.getAttribute("view_id");
                var field = $$(id);

                if (field && field.config.nextFocus) {
                    var focusField = $$(field.config.nextFocus);
                    focusField.focus();
                    focusField.getInputNode().select();
                    return false;
                }

                this.confirmAdd();
                return false;
            }
        }
    },

    toggleAdd: function () {
        var isInserting = this.inserting;
        var height = isInserting ? this.minHeight : this.maxHeight;

        $$('layout.add')[isInserting ? "hide" : "show"]();
        $$('button.add')[isInserting ? "show" : "hide"]();

        this.eachButtons(function(b) {
            b[isInserting ?"enable":"disable"]();
        });

        if (!isInserting) {
            var server = $$("text.server");
            server.focus();
            server.getInputNode().select();
        } else {
            this.resetAddLayout();
        }

        this.define('height', height);
        this.resize();

        this.inserting = !this.inserting;
    },

    resetAddLayout:function() {
        this.eachAddFields(function(text) {
            text.setValue('');
        });
        $$('text.logtable').setValue(this.defaultLogTable);
    },

    eachAddFields: function (fn, scope) {
        $$('layout.add.text').getChildViews().forEach(fn, scope || this);
    },

    eachButtons:function(fn) {
        this.getChildViews().forEach(function(button) {
            if (button.name == "button") {
                fn(button);
            }
        });
    },

    getTargetActivationButton:function() {
        var rezult, me = this;
        this.getChildViews().every(function(button) {
            if (button.name == "button" && me.activeButton != button) {
                rezult = button;
                return false;
            }
            return true;
        });
        return rezult;
    },

    getAddConfig:function() {
        var cfg = {};
        this.eachAddFields(function(field) {
            cfg[field.config.dataIndex] = field.getValue();
        });
        return cfg;
    },
    
    
    confirmAdd:function() {
        var me = this;
        var addCfg = me.getAddConfig();

        me.disable();
        me.showOverlay("Connect <span class='webix_icon fa-spinner fa-spin'></span>");

        webix.ajax().post('Handlers/CheckConnection.ashx', addCfg, function (response) {

            me.enable();
            me.hideOverlay();

            response = JSON.parse(response);

            var errorMessage = "";
            if (!response.Server) {
                errorMessage = "Bad Server name";
            }
            if (!errorMessage && !response.Database) {
                errorMessage = "Bad Database name";
            }
            if (!errorMessage && response.BadTableName) {
                errorMessage = "Bad Log Table name";
            };

            if (errorMessage) {
                webix.alert({
                    title: 'Error',
                    type: 'alert-error',
                    text: errorMessage
                });
                return;
            };

            var addCfg = me.getAddConfig();
            addCfg.warning = !response.LogTable;

            me.toggleAdd();
            var button = me.addButton(addCfg);
            var $button = $$(button);
            me.activate($button);
            if ($button.config.logCfg.warning) {
                me.createLogTable($button);
            }

            me.saveState();    
        });
    },

    addButton: function (cfg) {
        var insertIndex = this.index($$("button.add"));
        var view = this.addView(this.createButton(cfg), insertIndex);
        return view;
    },

    createButton: function (cfg) {
        return {
            icon: 'database',
            autowidth: true,
            view: "button",
            type: "htmlbutton",
            css:'webix_segment_N',
            icon: "database",
            logCfg: cfg,
            label: this.buildButtonLabel(cfg),
            click: webix.bind(this.onButtonClick,this)
        };
    },

    buildButtonLabel:function(cfg) {
        var template = webix.template(
            '</span> <span class="webix_tab_close webix_icon fa-times"></span>' +
                ' <span class="webix_icon fa-icon fa-database" style="font-size:18px;"></span>' +
                    '<span> #server# #database# <br/>' +
                        (cfg.warning == true ? '<span class="webix_icon fa-icon fa-exclamation-triangle" style="font-size:18px; color: #ffd21a !important;">' : "") +
                        '</span>#logtable# </span> ');
        return template(cfg);
    },

    activate: function(button) {
        if (this.activeButton) {
            webix.html.removeCss(this.activeButton.getNode(), "webix_selected");
        }
        if (button) {
            this.activeButton = button;
            webix.html.addCss(button.getNode(), "webix_selected");
            this.callEvent("activate", [button.config.logCfg]);
        } else {
            this.activeButton = null;
            this.callEvent("activate", []);
        }
    },

    onButtonClick: function (id,e) {

        var button = $$(id);
        var target = e.target || e.srcElement;

        switch (target.className) {
        
            case "webix_tab_close webix_icon fa-times":
            {
                if (this.activeButton == button) {
                    var targetButton = this.getTargetActivationButton();
                    this.activate(targetButton);    
                }
                this.removeView(button);
                this.saveState();
                break;
            }

            case "webix_icon fa-icon fa-exclamation-triangle":
            {
                this.createLogTable(button);
                break;
            }

            default:
            {
                this.activate(button);
                this.saveState();  
            }
        }
        
    },
    
    restoreState: function () {
        var state = this.loadState();
        var me = this;
        if (state) {
            state.forEach(function (cfg) {

                var active = cfg.active || false;
                delete cfg.active;

                var button = me.addButton(cfg);
                if (active) {
                    me.activate($$(button));
                }
            });
        }
    },

    getState: function () {
        var state = [];
        var me = this;

        this.eachButtons(function(b) {
            var cfg = b.config.logCfg;
            if (me.activeButton && me.activeButton == b) {
                cfg.active = true;
            }
            state.push(cfg);
        });

        return JSON.stringify(state);
    },

    saveState: function () {
        webix.storage.local.put(this.stateName, this.getState());
    },

    loadState: function () {
        var state = webix.storage.local.get(this.stateName);
        if (state) return JSON.parse(state);
    },

    createLogTable: function (button) {
        var me = this;
        var cfg = button.config.logCfg;

        var message = "Table " + cfg.logtable + " not found in database " + cfg.database + ". Create table '" + cfg.logtable + "'?";
        webix.confirm({
            title: 'Not found table ' + cfg.logtable,
            type:"confirm-warning",
            text: message,
            callback : function(ok) {
                if (ok) {
                    webix.ajax().post('/Handlers/CreateTable.ashx', cfg, function() {
                        button.config.logCfg.warning = false;
                        button.define("label", me.buildButtonLabel(button.config.logCfg));
                        button.refresh();
                    });
                }
            }
        });
    }
    
}, webix.ui.toolbar, webix.OverlayBox);