/**
 * HeaderFilter
 * @type {HeaderFilter}
 */
const HeaderFilter = class {
    headerFilterSelector;

    constructor(selector) {
        this.headerFilterSelector = $(selector);
    };

    stateSaveParams(settings, data) {
        $('table thead tr:eq(1) th').each(function () {
            let th = $(this),
                id = th.attr('filter'),
                option = $('select option:selected', th);
            if (option.length > 0) {
                data.columns[id].search.searchLabel = option[0].label;
            }
        });
    };

    stateLoadParams(settings, data) {
        $('table thead tr:eq(1) th').each(function () {
            let th = $(this),
                id = th.attr('filter'),
                select = $('select', th);
            if (select.length > 0 && data.columns[id].search.searchLabel !== undefined) {
                let option = select.find('option[value="' + data.columns[id].search.search + '"]');
                if (option.length > 0) {
                    option[0].selected = true;
                } else {
                    option = new Option(data.columns[id].search.searchLabel, data.columns[id].search.search);
                    option.selected = true;
                    select.append(option);
                }
            }
        });
    };

    responsiveResize(e, datatable, columns) {
        $('table thead tr:eq(1) th').each(function () {
            let th = $(this),
                id = th.attr('filter');
            th.css('display', columns[id] ? '' : 'none');
            $('select', th).each(function () {
                $(this).val(datatable.column(id).search());
            });
            $('input', th).each(function () {
                $(this).val(datatable.column(id).search());
            });
        });
    }
};

$(function () {
    $('table thead tr:eq(1) th').each(function (i) {
        $(this).attr('filter', i);
        $('select', this).on('change', function () {
            // TODO Reemplazar con objeto DT trabajar con dt init
            if (utilScript.dataTable.column(i).search() !== this.value) {
                utilScript.dataTable.column(i).search(this.value).draw();
            }
        });
        $('input', this).on('keyup change', function () {
            let that = this;
            setTimeout(function () {
                //Your code here
                if (utilScript.dataTable.column(i).search() !== that.value) {
                    utilScript.dataTable.column(i).search(that.value).draw();
                }
            }, 2000); //Waits for 3 seconds after last keypress to execute the above lines of code
        });
    });

    $('table').on('stateSaveParams.dt', function (e, settings, data) {
        new HeaderFilter('table thead tr:eq(1) th').stateSaveParams(settings, data);
    });

    $('table').on('stateLoadParams.dt', function (e, settings, data) {
        new HeaderFilter('table thead tr:eq(1) th').stateLoadParams(settings, data);
    });

    $('table').on('responsive-resize.dt', function (e, datatable, columns) {
        new HeaderFilter('table thead tr:eq(1) th').responsiveResize(e, datatable, columns);
    });
});