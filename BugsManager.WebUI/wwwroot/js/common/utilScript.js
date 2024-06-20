

const utilScript = {
    API_URL: 'http://localhost:5108',
    swaggerUrl: function () {
        return utilScript.API_URL+'/swagger/v1/swagger.json';
    },
    swaggerClient: null,
    getSwaggerClient: function () {
        if (!this.swaggerClient) {
            utilScript.swaggerClient = new SwaggerClient({
                url: utilScript.swaggerUrl(),
                requestInterceptor: (req) => {
                    req.headers['Access-Control-Allow-Origin'] = "*";
                    return req;
                },
            });
        }
        return utilScript.swaggerClient;
    },
    getMethod: function () {
        return utilScript.modelId ? 'put' : 'post';
    },
    buildAction: function (modelAction = null, modelId = null, params = null) {
        console.log(modelAction)
        let result = utilScript.API_URL;
        if (modelAction) {
            utilScript.modelAction = modelAction;
            result = modelAction.includes("api") ? utilScript.API_URL : "";
        }
        console.log(modelId)
        if (modelId) {
            utilScript.modelId = modelId
        }


        if (utilScript.modelAction) {
            result += utilScript.modelAction
            console.log(utilScript.modelAction)
            if (utilScript.modelId) {
                result = result.replace('{id}', utilScript.modelId)
            }
        }
        console.log(params)
        if (params) {
            if (result.charAt(result.length - 1) !== '?') {
                result += '?';
            }
            result += $.param(params);
        }

        console.log(result)

        return result;
    },
    getJQValidationRules: function (path, method, content = 'application/json') {
        let rules = {};
        let schema = this.getSwaggerClient().spec.paths[path][method].requestBody.content[content].schema;
        console.log(schema);
        $.each(schema.properties, function (key, value) {
            key = utilScript.capitalizeFirstLetter(key);
            rules[key] = {};
            if (value.nullable === false) {
                rules[key].required = true;
            }
            if (value.maxLength) {
                rules[key].maxlength = value.maxLength;
            }
            if (value.minLength) {
                rules[key].minlength = value.minLength;
            }
            if (value.pattern) {
                rules[key].match = value.pattern;
            }

            if (value.format === 'email') {
                rules[key].email = true;
            }
        });

        $.each(schema.required, function (key, value) {
            value = utilScript.capitalizeFirstLetter(value);
            rules[value].required = true;
        });

        return rules;
    },
    dataTable: null, 
    modelName: null,
    modelBody: null,
    modelBtn: null,
    modelId: null, 
    modelAction: null,
    rowData: null, 
    modelRedirect: null, 
    method: null,

    statusCode400: function (data) {
        if (data.responseJSON.error !== undefined && data.responseJSON.error !== null) {
            toastr.error(data.responseJSON.error);
        } else if (data.responseJSON.Message !== undefined && data.responseJSON.Message !== null) {
            toastr.error(data.responseJSON.Message);
        } else {
            toastr.error("Another error occurred");
        }
    },
    statusCode403: function (data) {
        if (data.responseJSON.error !== undefined && data.responseJSON.error !== null) {
            toastr.error(data.responseJSON.error);
        } else if (data.responseJSON.Message !== undefined && data.responseJSON.Message !== null) {
            toastr.error(data.responseJSON.Message);
        } else {
            toastr.error("Another error occurred");
        }
    },
    statusCode404: function (data) {
        if (data.responseJSON.error !== undefined && data.responseJSON.error !== null) {
            toastr.error(data.responseJSON.error);
        } else if (data.responseJSON.Message !== undefined && data.responseJSON.Message !== null) {
            toastr.error(data.responseJSON.Message);
        } else if (data.responseJSON.title !== undefined && data.responseJSON.title !== null) {
            toastr.error(data.responseJSON.title);
        } else {
            toastr.error("Another error occurred");
        }
    },
    statusCode422: function (data) {
        if (data.responseJSON.error !== undefined && data.responseJSON.error !== null) {
            toastr.error(data.responseJSON.error);
        } else {
            toastr.error("Another error occurred");
        }
    },
    statusCode500: function (data) {
        if (data.responseJSON.error !== undefined && data.responseJSON.error !== null) {
            toastr.error(data.responseJSON.error);
        } else if (data.responseJSON.message !== undefined && data.responseJSON.message !== null) {
            toastr.error(data.responseJSON.message);
        } else {
            toastr.error("Another error occurred");
        }
    },
    statusDone: function (data) {
        if (data.info !== undefined && data.info !== null) {
            toastr.success(data.info);
            utilScript.modelRedirect += ("?info=" + data.info);
        }
    },

    clearRowData: function () {
        utilScript.rowData = null;
    },
    dataTables: {
        dom1: "rt<'row align-items-center'<'col'l><'col'i><'col'p>>",
        dataFilter: function (data) { // processing data received from the server
            let json = jQuery.parseJSON(data);
            json.recordsTotal = json.pagination.totalRecords;
            json.recordsFiltered = json.pagination.totalRecords;
            return JSON.stringify(json); // return JSON string
        },
        processData: function (d) { // Processing data before sent to the server
            if (d.length > 0) {
                d.pageIndex = (d.start / d.length) + 1;
                d.pageSize = d.length;
            } else {
                d.pageIndex = 1;
            }

            let orderBy = [];
            $.each(d.order, function (index, data) {
                let column = d.columns[data.column],
                    key = (column.name !== null && column.name !== '') ? column.name : column.data;
                if (data.dir === 'desc') {
                    key = '-' + key;
                }
                orderBy.push(key);
            });

            d.orderBy = orderBy.join(',');
            $.each(d.columns, function (index, data) {
                if (data.search.value !== '') {
                    let key = (data.name !== null && data.name !== '') ? data.name : data.data;
                    if (key) {
                        key = key.replace('.', '')
                        d['Search' + key] = data.search.value;
                    }
                }
            });
            d.columns = [];
            d.order = [];
            d.Active = null;
        },
        
        defaults: {
            stateSave: true,
            fixedHeader: false, // TODO no work
            orderCellsTop: true,
            paging: true,
            //pagingType: "simple",
            lengthMenu: [[5, 10, 25, 50, 100], [5, 10, 25, 50, 100]],
            lengthChange: true,
            processing: true,
            serverSide: true,
            searching: true,
            ordering: true,
            orderMulti: true,
            info: true,
            autoWidth: false,
            responsive: true,
            dom: "Brt<'row align-items-center'<'col'l><'col'i><'col'p>>",
        },
        buildButton: function (action) {
            return {
                buttons: [
                    {
                        text: '<i id="data-add" class="fa-solid fa-square-plus"></i>',
                        className: 'btn btn-success',
                        attr: {
                            'data-bs-toggle': "modal",
                            'data-bs-target': "#modal-form",
                            'data-model-action': action,
                            'data-model-name': "Add",
                            'data-tooltip': "tooltip",
                            'title': "Add"
                        }
                    },
                    {
                        text: '<i class="fa fa-file-excel-o"></i>',
                        extend: 'excel',
                        extension: '.xls',
                        attr: {
                            'data-tooltip': "tooltip",
                            'title': "Excel",
                        }
                    }, {
                        text: '<i class="fa fa-file-pdf-o"></i>',
                        extend: 'pdf',
                        extension: '.pdf',
                        attr: {
                            'data-tooltip': "tooltip",
                            'title': "PDF",
                        }
                    },
                    {
                        extend: 'print',
                        text: '<i class="fa fa-print"></i>',
                        exportOptions: {
                            modifier: {
                                selected: false,
                            },
                        },
                        attr: {
                            'data-tooltip': "tooltip",
                            'title': "Print",
                        }
                    },
                    {
                        text: "<i id=\"data-add\" class=\"fas fa-redo\"></i>",
                        attr: {
                            'data-tooltip': "tooltip",
                            'title': "Refresh",
                        },
                        action: function (e, dt) {
                            dt.state.clear();
                            window.location.reload();
                        }
                    }
                ],
            }
        },

        buildSingleButton: function (action, name) {
            return {
                buttons: [
                    {
                        text: "<i id=\"data-add-" + name + "\" class=\"fa fa-plus-square\"></i>",
                        className: 'btn btn-success',
                        attr: {
                            'data-toggle': "modal",
                            'data-target': "#modal-" + name,
                            'data-model-action': action,
                            'data-model-name': utilScript.lang.add,
                            'data-tooltip': "tooltip",
                            'title': utilScript.lang.addCustom,
                            'hidden': utilScript.NotifyPermissions.create === "False"
                        }
                    },
                    {
                        text: "<i id=\"data-add\" class=\"fas fa-redo\"></i>",
                        attr: {
                            'data-tooltip': "tooltip",
                            'title': utilScript.lang.refresh,
                        },
                        action: function (e, dt) {
                            dt.state.clear();
                            window.location.reload();
                        }
                    }
                ],
                dom: {
                    button: {
                        className: 'btn btn-primary'
                    },
                    container: {
                        className: null
                    }
                }
            }
        },

        columnDefsActive: function (config) {
            return {
                width: "85px",
                targets: config.targets,
                data: config.data,
                className: 'text-center',
                visible: config.visible !== null && config.visible !== undefined ? config.visible : true,
                render: function (data) {
                    return data === true ? '<i class="fas fa-check-circle"></i>' : '<i class="fas fa-ban"></i>';
                }
            };
        },
        columnDefsActios: function (config) {
            return {
                targets: config.targets,
                data: null,
                //defaultContent: utilScript.dataTables.defaultContent(config),
                orderable: false,
                className: 'text-center',
                render: function (data) {
                    //console.log(data)
                    //console.log(row)

                    let modelId = utilScript.dataTable.settings()[0].rowId;
                    
                    config.modelId = data[modelId];
                    return utilScript.dataTables.defaultContent(config);
                }
            }
        },
        defaultContent: function (config) {
            let result = '';
            
                result += $('<a/>', {
                    id: 'data-edit',
                    'class': 'btn btn-sm btn-primary fa-solid fa-pen-to-square mr-1',                
                    'data-toggle': "modal",
                    'data-target': "#modal-form",
                    'data-model-name': config.edit.title,
                    'data-model-action': config.edit.action,
                    'data-model-id': config.modelId,
                    'data-tooltip': "tooltip",
                    'title': config.edit.title
                }).prop('outerHTML');
            
           
                result += $('<a/>', {
                    id: 'data-delete',
                    'class': 'btn btn-sm btn-danger fa-solid fa-trash mr-1',            
                    'data-toggle': "modal",
                    'data-target': "#modal-delete",
                    'data-model-name': config.del.title,
                    'data-model-action': config.del.action,
                    'data-model-id': config.modelId,
                    'data-model-body': config.del.body ? config.del.body : null,
                    'data-tooltip': "tooltip",
                    'title': config.del.title
                }).prop('outerHTML');
           
            $.each(config, function (index, data) {
                if (data instanceof Object && index !== 'edit' && index !== 'del') {
                    if (data.aspAction) {
                        result += $('<a/>', {
                            id: 'data-' + index,
                            'class': data.class,
                            'data-tooltip': "tooltip",
                            'title': data.title,
                            'html': data.html || '',
                            'href': data.aspAction + config.modelId
                        }).prop('outerHTML');
                    } else {
                        result += $('<a/>', {
                            id: 'data-' + index,
                            'class': data.class,
                            'data-toggle': "modal",
                            'data-target': data.target,
                            'data-model-name': data.title,
                            'data-model-action': data.action,
                            'data-model-id': config.modelId,
                            'data-tooltip': "tooltip",
                            'title': data.title,
                            'html': data.html || '',
                        }).prop('outerHTML');
                    }
                }
            });
            return result;
        },
        columnDefsDateTime: function (config) {
            return {
                targets: config.targets,
                className: 'text-center',
                render: function (data) {
                    if (data !== null) {
                        // TODO pending locale and tz config
                        //return window.moment.tz(data, "America/New_York").format('DD-MM-YYYY HH:mm:ss');
                        return window.moment(data).format(config.format ? config.format : utilScript.dateTimeFormat);
                    }
                    return '';
                }
            };
        },
        columnDefsNumber: function (config) {
            return {
                className: "text-center",
                targets: config.targets,
                render: function (data) {
                    if (data !== null) {
                        return $.number(data, 0);
                    }
                    return '';
                }
            };
        },
        columnDefsDecimal: function (config) {
            return {
                className: "text-center",
                targets: config.targets,
                render: function (data) {
                    if (data !== null) {
                        return $.number(data, 2);
                    }
                    return '';
                }
            };
        },
        defaultContent: function (config) {
            let result = '';
            
            result += $('<a/>', {
                id: 'data-edit',
                'class': 'btn btn-sm btn-primary fas fa-edit mr-1',
                'data-bs-toggle': "modal",
                'data-bs-target': "#modal-form",
                'data-model-name': config.edit.title,
                'data-model-action': config.edit.action,
                'data-model-id': config.modelId,
                'data-tooltip': "tooltip",
                'title': config.edit.title
            }).prop('outerHTML');
            
            
            result += $('<a/>', {
                id: 'data-delete',
                'class': 'btn btn-sm btn-danger fas fa-trash-alt mr-1',
                'data-bs-toggle': "modal",
                'data-bs-target': "#modal-delete",
                'data-model-name': config.del.title,
                'data-model-action': config.del.action,
                'data-model-id': config.modelId,
                'data-model-body': config.del.body ? config.del.body : null,
                'data-tooltip': "tooltip",
                'title': config.del.title
            }).prop('outerHTML');
            
            $.each(config, function (index, data) {
                if (data instanceof Object && index !== 'edit' && index !== 'del') {
                    if (data.aspAction) {
                        result += $('<a/>', {
                            id: 'data-' + index,
                            'class': data.class,
                            'data-tooltip': "tooltip",
                            'title': data.title,
                            'html': data.html || '',
                            'href': data.aspAction + config.modelId
                        }).prop('outerHTML');
                    } else {
                        result += $('<a/>', {
                            id: 'data-' + index,
                            'class': data.class,
                            'data-toggle': "modal",
                            'data-target': data.target,
                            'data-model-name': data.title,
                            'data-model-action': data.action,
                            'data-model-id': config.modelId,
                            'data-tooltip': "tooltip",
                            'title': data.title,
                            'html': data.html || '',
                        }).prop('outerHTML');
                    }
                }
            });
            return result;
        },
        action: function (id) {
            utilScript.rowData = utilScript.dataTable.row($(this).parents('tr')).data();
            console.log(utilScript.rowData);
            $(this).attr('data-model-id', utilScript.rowData[id]);
            utilScript.rowData = utilScript.dataTable.row($(this).parents('tr')).data();
            $(this).attr('data-model-id', utilScript.rowData['companyId']);
            console.log(utilScript.rowData);
        },
        TableFilter: function (tableObject, tableId) {

            $('#' + tableId + ' thead tr:eq(1) th').each(function (i) {
                $(this).attr('filter', i);
                $('select', this).on('change', function () {
                    // TODO Reemplazar con objeto DT trabajar con dt init
                    if (tableObject.column(i).search() !== this.value) {
                        tableObject.column(i).search(this.value).draw();
                    }
                });
                $('input', this).on('keyup change', function () {
                    let that = this;
                    setTimeout(function () {
                        //Your code here
                        if (tableObject.column(i).search() !== that.value) {
                            tableObject.column(i).search(that.value).draw();
                        }
                    }, 2000); //Waits for 3 seconds after last keypress to execute the above lines of code
                });
            });

            tableObject.on('stateSaveParams.dt', function (e, settings, data) {
                new HeaderFilter('table thead tr:eq(1) th').stateSaveParams(settings, data);
            });

            tableObject.on('stateLoadParams.dt', function (e, settings, data) {
                new HeaderFilter('table thead tr:eq(1) th').stateLoadParams(settings, data);
            });

            tableObject.on('responsive-resize.dt', function (e, datatable, columns) {
                new HeaderFilter('table thead tr:eq(1) th').responsiveResize(e, datatable, columns);
            });


        },
    },

    select2: {
        prePagination: function (params) {
            return {
                searchString: params.term,
                pageIndex: params.page || 1,
                pageSize: params.pageSize || 20,
            }
        },
        postPagination: function (data, params) {
            return {
                more: true
            }
        },
        buildSelect2: function (selector, url = null, fieldId = null, fieldName = null, extraConfig = {}, object = null, groupFieldId = null, groupFieldName = null) {
            let config = {
                placeholder: $(selector).attr('placeholder') || '',
                width: '100%',
                allowClear: true,
                theme: 'bootstrap4'
            };
            if (url) {
                $.extend(true, config, {
                    ajax: {
                        url: utilScript.buildAction(url),
                        type: 'GET',
                        cache: true,
                        async: true,
                        dataType: 'json',
                        delay: 250,
                        data: utilScript.select2.prePagination,
                        processResults: function (result, params) {
                            params.pageSize = 20;
                            params.page = params.page || 1;
                            $.each(result.data, function (i, v) {

                                result.data[i].id = v[fieldId];
                                result.data[i].text = v[fieldName];
                            });
                            if (groupFieldId) {
                                let groupedData = [];
                                $.each(result.data, function (i, v) {
                                    let id = utilScript.resolve(groupFieldId, v, '.');
                                    if (groupedData.findIndex(x => x.id === id) === -1) {
                                        groupedData.push({
                                            id: id,
                                            text: utilScript.resolve(groupFieldName, v, '.'),
                                            children: []
                                        });
                                    }
                                    groupedData[groupedData.findIndex(x => x.id === id)].children.push(v);
                                });
                                result.data = groupedData;
                                console.log(groupedData);
                            }
                           
                            return {
                                results: result.data,
                                pagination: utilScript.select2.postPagination(result, params),
                            };
                        },
                        statusCode: {
                            422: utilScript.statusCode422
                        },
                    },
                });
            }
            $.extend(true, config, extraConfig);
            let select2 = $(selector).select2(config);
            select2.on('change.select2', function (e) {
               
                if (e.params) {
                    if (e.params.from === '_ModalForm' && object) {
                        let result = e.params.data[object];
                        if (result) {                           
                            select2.append(new Option(result[fieldName], result[fieldId], true, true));
                        }
                    }
                }
            });
            return select2
        },
    },

    dateFormat: 'DD/MM/YYYY',
    dateTimeFormat: 'DD/MM/YYYY HH:mm:ss',
    buildDateRangePicker: function (selector, single = false, dateFormat = false) {
        selector = $(selector);
        let myFormat = dateFormat ? utilScript.dateFormat : utilScript.dateTimeFormat;
        selector.daterangepicker({
            timePicker: !dateFormat,
            timePickerIncrement: 30,
            autoUpdateInput: false,
            autoApply: true,
            singleDatePicker: single,
            locale: {
                format: myFormat,
                applyLabel: "Apply",
                cancelLabel: "Clear",

            }
        }).on('show.daterangepicker', function (ev, picker) {
            // console.log(selector.val())
            $(this).val(selector.val()).trigger('change');
        }).on('cancel.daterangepicker', function (ev, picker) {
            //console.log(picker)
            selector.val(null).trigger('change');
            $(this).val(null).trigger('change');
        }).on('apply.daterangepicker', function (ev, picker) {
            let value = single ? picker.startDate.format(myFormat) : picker.startDate.format(myFormat) + ' - ' + picker.endDate.format(myFormat);
            utilScript.startDate = picker.startDate.format(myFormat);
            utilScript.endDate = single ? picker.startDate.format(myFormat) : picker.endDate.format(myFormat);
            //console.log(value);
            selector.val(value).trigger('change');
        }).trigger('hide.daterangepicker');
    },

}