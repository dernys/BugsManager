

let quickForm = $('#quickForm'),
    btnSaveClose = $('#btn-savenclose'),
    modalForm = $('#modal-form').on('show.bs.modal', function (event) {
        let formInputs = $('#modal-form input'),
            formSelects = $('#modal-form select'),
            validationSummary = $('#validation-summary');
        let button = $(event.relatedTarget); // Button that triggered the modal
        utilScript.modelName = button.data('model-name'); // Extract info from data-* attributes
        utilScript.modelId = button.data('model-id'); // Extract info from data-* attributes
        utilScript.modelAction = button.data('model-action'); // Extract info from data-* attributes
        modalForm.find('.modal-title').text(utilScript.modelName);
        validationSummary.text('');
        let content = 'application/json';
        
       

        if (utilScript.modelId !== undefined && utilScript.modelId !== null) {
            utilScript.rowData = utilScript.dataTable.row('#' + utilScript.modelId).data();
        } else {
            utilScript.rowData = null;
        }

        if (utilScript.rowData) {
            
            formInputs.each(function () {
                let name = "";
                if ($(this).attr('name')) {
                    name = $(this).attr('name')
                    name = name.charAt(0).toLowerCase() + name.slice(1);
                }

                //console.log($(this).attr('name'))
                if($(this).attr('name')===null || $(this).attr('name')===undefined){
                   //
                }
                
                $(this).attr('value', utilScript.rowData[name])
                if ($(this).attr('type') === 'checkbox') {
                    $(this).attr('checked', utilScript.rowData[name]);
                }
                
            });
            formSelects.each(function () {
                let name = $(this)[0].name;
                name = name.charAt(0).toLowerCase() + name.slice(1);
                if ($(this).data('select2')) {
                    $(this).val(utilScript.rowData[name]).trigger({
                        type: 'change.select2',
                        params: {
                            data: utilScript.rowData,
                            from: '_ModalForm'
                        }
                    });
                } else {
                    $(this).val(utilScript.rowData[name]).trigger('change');
                }
            });
        } else {
            
            formInputs.each(function () {
                if ($(this).attr('type') !== 'hidden') {
                    $(this).attr('value', '');
                }
                let name = "";
                if ($(this).attr('name')) {
                    name = $(this).attr('name')
                    name = name.charAt(0).toLowerCase() + name.slice(1);
                }

           
            });
            formSelects.each(function () {
                if ($(this).data('select2')) {
                    $(this).val(null).trigger('change.select2');
                } else {
                    $(this).val(null);
                }
            });
            $('#model-id').each(function () {
                $(this).attr('value', 0)
            });
        }
      /*  let rules = utilScript.getJQValidationRules(utilScript.modelAction, utilScript.getMethod(), content);
        quickForm.validate().destroy();

        quickForm.validate({
            rules: rules,
        });*/
     
    })
modalForm.on('click', '#btn-savenclose', function () {
    saveForm(true);
})
modalForm.on('click', '#btn-save', function () {
    let closeOnFinish = btnSaveClose.length === 0;    
    saveForm(closeOnFinish);
  
})



function saveForm(closeOnFinish) {
    let form = quickForm;

    let data, fileType = false, dataFile = new FormData(), searchData = [], searchName = [];
    
    
    if (form.valid()) {
        data = form.serializeArray({checkboxesAsBools: true}).reduce(function (obj, item) {
            let input = $('input[name="' + item.name + '"]'),
                type = input[0] !== undefined ? input[0].type : null;
            if (item.value !== '') {
               
                if ((type == null)){
                    searchData.push(item);
                    if(searchName.indexOf(item.name)===-1)
                        searchName.push(item.name);
                }else if (!isNaN(item.value) && (type === 'hidden' || type === 'number')) {
                    obj[item.name] = Number(item.value)
                    
                } else {
                    obj[item.name] = item.value;
                }
                if (item.name.includes('Date') || type === 'date') {
                    obj[item.name] = moment(item.value, 'DD/MM/YYYY').utc(item.value);
                }
            }
            return obj;
        }, {});
      
        
        if (searchData.length > 0) {
            $.each(searchName, function (i, value) {
                data[value] =  searchData.map(function(val){
                    if(val.name===value){
                        return val.value;
                    }
                });
                data[value] =  data[value].filter(val => val!==undefined);
                if(data[value].length===1){
                    data[value] = data[value][0];
                }

            });
        }
        
            $.ajax({
                url: utilScript.buildAction(),
                type: utilScript.getMethod(),
                data: fileType ? dataFile : JSON.stringify(data),
                dataType: 'json',
                contentType: fileType ? false : 'application/json',
                
                processData: !fileType,
                statusCode: {
                    400: function (data) {
                        $('#validation-summary').text(data.responseJSON.title);
                        let arrayErrors = data.responseJSON.errors
                        $.each(arrayErrors, function (name, errors) {
                            arrayErrors[name] = errors[0];
                        });
                        
                },
                404: function () {

                    modalForm.modal('hide');
                    utilScript.dataTable.ajax.reload(null, false);
                },
                422: function (data) {
                    if (data.responseJSON.error !== undefined) {
                        $('#validation-summary').text(data.responseJSON.error);
                    }
                }
            }
        }).done(function (response) {
            if (closeOnFinish) {
                quickForm[0].reset();
                quickForm.validate().resetForm();
                form[0].reset();
               
                modalForm.modal('hide');
            } else {
                $('#validation-summary').text("");
            }
            
            if (utilScript.dataTable) {
                utilScript.dataTable.ajax.reload(null, false);
                utilScript.dataTable.order = [[1, "desc"]];
            }
            modalForm.trigger("modal-form:save", {success: true, response: response});
            
            ActionMessages.setSuccessMessage("Task Successfully");
            ActionMessages.ShowActionMessage();

            if (closeOnFinish) {
                modalForm.modal('hide');
            } else {
                $('#validation-summary').text("");
            }
        }).fail(function (response) {
            modalForm.trigger("modal-form:save", {success: false, response: response});
        });
    
}

else
{
    $("#btn-savenclose").attr("class", "btn btn-primary disabled")
    $("#btn-save").attr("class", "btn btn-primary disabled")
}
}

$("#modal-form input").on('change', function () {
    $("#btn-save").attr("class", "btn btn-primary");
    $("#btn-savenclose").attr("class", "btn btn-primary");
})

$("#modal-form select").on('change', function () {
    $("#btn-save").attr("class", "btn btn-primary");
    $("#btn-savenclose").attr("class", "btn btn-primary");
})


modalForm.on('click', 'button', function () {
    quickForm[0].reset();
    quickForm.validate().resetForm();
});