$(document).ready(function () {
    UsersCRUD();
})
function UsersCRUD() {
    let columns = [
        { data: "name" },
        { data: "surname" },

        { data: null, width: "110px" }
    ];
    utilScript.dataTable = $('#data-table').DataTable({
        rowId: 'id',
        responsive: true,
        dom: 'Bftrip',
        ajax: {
            url: 'http://localhost:5108/api/users',
            method: 'GET',
            dataSrc: "data",
        },
        columns: columns,
        columnDefs: [
            utilScript.dataTables.columnDefsActios({
                targets: 2,
                edit: {
                    title: "Edit",
                    action: '/api/Users/{id}'
                },
                del: {
                    title: "Delete",
                    action: '/api/Users/{id}',
                    target: '#modal-delete',
                    class: 'btn btn-sm btn-danger fas fa-trash-alt',
                },
            }),
        ],
        buttons: utilScript.dataTables.buildButton("/api/Users")
       
    })
   
};