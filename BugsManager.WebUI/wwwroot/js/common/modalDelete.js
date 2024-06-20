let data = null, msgSuccefully;
let modalDelete = $('#modal-delete').on('show.bs.modal', function (event) {
    let button = $(event.relatedTarget); // Button that triggered the modal
    utilScript.modelName = button.data('model-name'); // Extract info from data-* attributes
    if(button.data('model-id'))
        utilScript.modelId = button.data('model-id'); // Extract info from data-* attributes
    utilScript.modelAction = button.data('model-action');
    utilScript.modelRedirect = button.data('model-redirect');
    utilScript.method = button.data('model-method');
    utilScript.modelBody = button.data('model-body');
    utilScript.modelBtn = button.data('model-btn');
    
    if (utilScript.modelBtn) {
        modalDelete.find('#btn-delete').html(utilScript.modelBtn);
    } else {
        modalDelete.find('#btn-delete').text("Delete");
    }
   
    data = button.data('model-url');
    modalDelete.find('.modal-title').text(utilScript.modelName);
    if (utilScript.modelBody) {
        modalDelete.find('#modal-body').text(utilScript.modelBody);
    } else {

        modalDelete.find('#modal-body').text(modalDelete.find('#modal-body').text());
    }
    modalDelete.find('#btn-delete').show();
    if(button.data('model-error') && button.data('model-error')!==""){
        modalDelete.find('#modal-body').text(button.data('model-error'));
        modalDelete.find('#btn-delete').hide();
    }
})


modalDelete.on('click', '#btn-delete', function () {
    $.ajax({
        url: utilScript.buildAction(),
        type: utilScript.method ? utilScript.method : 'DELETE',
        data: JSON.stringify(data),
        contentType: 'application/json',
      
        statusCode: {
            400: function(data) {
                utilScript.statusCode403(data)
                modalDelete.modal('hide')
            },
            403: function(data) {
                utilScript.statusCode403(data)
                modalDelete.modal('hide')
            },
            404: function () {
                utilScript.statusCode404(data)
                modalDelete.modal('hide');
                utilScript.dataTable.ajax.reload( null, false );
            },
            422: function(data) {
                utilScript.statusCode422(data)
                modalDelete.modal('hide')
            },
            500: function(data) {
                utilScript.statusCode500(data)
                modalDelete.modal('hide')
            }
        }
    }).done(function (response) {
        modalDelete.modal('hide');
        utilScript.dataTable.ajax.reload( null, false );
        //ActionMessage popup
       ActionMessages.setSuccessMessage("Task Successfully");
        
        modalDelete.trigger( "modal-delete:delete", { success: true, response: response} );

    }).fail(function (response) {
        modalDelete.trigger( "modal-delete:delete", { success: false, response: response} );
        console.log(response)
        ActionMessages.setErrorMessage("FAIL");
    });
});