var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/Light/GetAll"
        },
        "columns": [
            { "data": "applicationUser.userName", "width": "30%" },
            { "data": "name", "width": "10%" },
            {
                "data": {
                    powerStatus: "powerStatus"
                },
                "render": function (data) {

                    if (data.powerStatus == false) {
                        return `
                            <div class="text-center">
                                <i class="btn btn-secondary bg-gradient-secondary text-white fas fa-ban"></i> 
                            </div>
                            `;
                    }
                    else {
                        return `
                            <div class="text-center">
                                <i class="btn btn-info bg-gradient-info text-white fas fa-check"></i> 
                            </div>
                            `;
                    }
                }, "width": "10%" 
            },
            {
                "data": {
                    connectionStatus: "connectionStatus"
                },
                "render": function (data) {

                    if (data.connectionStatus == false) {
                        return `
                            <div class="text-center">
                                <i class="btn btn-secondary bg-gradient-secondary text-white fas fa-ban"></i> 
                            </div>
                            `;
                    }
                    else {
                        return `
                            <div class="text-center">
                                <i class="btn btn-info bg-gradient-info text-white fas fa-check"></i> 
                            </div>
                            `;
                    }
                }, "width": "10%"
            },
            { "data": "voltageRange", "width": "10%" },
            { "data": "dim", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/Light/Upsert/${data}" class="btn btn-success bg-gradient-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Admin/Light/Delete/${data}") class="btn btn-danger bg-gradient-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                            `;
                }, "width": "15%"
            }
        ]
    })
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}