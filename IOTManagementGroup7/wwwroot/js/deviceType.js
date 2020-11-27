var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/DeviceType/GetAll"
        },
        "columns": [
            { "data": "name", "width": "30%" },
            {
                "data": {
                    onImage: "onImage",
                },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                    <img style="height:100px" src="${data.onImage}" />
                            </div>
                            `;
                }, "width": "10%"
            },

            {
                "data": {
                    offImage: "offImage",
                },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                    <img style="height:100px" src="${data.offImage}" />
                            </div>
                            `;
                }, "width": "10%"
            },

            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/DeviceType/Upsert/${data}" class="btn btn-success bg-gradient-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Admin/DeviceType/Delete/${data}") class="btn btn-danger bg-gradient-danger text-white" style="cursor:pointer">
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
    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        );
                        dataTable.ajax.reload();
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Error',
                            'Can not delete this, maybe it not exit or error from sever',
                            'error'
                        )
                    }
                }
            })
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your record is safe :)',
                'error'
            )
        }
    })
}

const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})