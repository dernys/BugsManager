$(document).ready(function () {
    BugsCRUD();
})
function BugsCRUD() {
    let columns = [
        {
            data: function (row) {
                return row.project ? row.project.name : null;
            }, name: 'project.name'
        },

        {
            data: function (row) {
                return row.user ? row.user.name : null;
            }, name: 'user.name'
        },
        { data: "description" },
        { data: "creationDate" },

        { data: null, width: "110px" }
    ];
    utilScript.dataTable = $('#data-table').DataTable({
        rowId: 'id',
        responsive: true,
        dom: 'Bftrip',
        ajax: {
            url: 'http://localhost:5108/api/Bugs',
            method: 'GET',
            dataSrc: "data",
        },
        columns: columns,
        columnDefs: [
            utilScript.dataTables.columnDefsActios({
                targets: 4,
                edit: {
                    title: "Edit",
                    action: '/api/Bugs/{id}'
                },
                del: {
                    title: "Delete",
                    action: '/api/Bugs/{id}',
                    target: '#modal-delete',
                    class: 'btn btn-sm btn-danger fas fa-trash-alt',
                },
            }),
            utilScript.dataTables.columnDefsDateTime({
                targets: [3],
                format: utilScript.dateFormat
            }),
        ],
        buttons: utilScript.dataTables.buildButton("/api/Bugs")
       
    })
    utilScript.select2.buildSelect2("#UserId", '/api/Users', 'id', 'name', 
        { width: '100%', dropdownParent: $('#modal-form'), dropdownCss: { 'z-index': 10000 } }
        , 'user');
    utilScript.select2.buildSelect2("#ProjectId", '/api/Projects', 'id', 'name', 
        { width: '100%', dropdownParent: $('#modal-form'), dropdownCss: { 'z-index': 10000 } },
        'project');
    utilScript.buildDateRangePicker('#CreationDate', true, false);
    utilScript.buildDateRangePicker('#searchDate', false, false);

    
   
};