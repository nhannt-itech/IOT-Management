var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/ApplicationUser/GetAll"
        },
        "columns": [
            {
                "data": { id: "id", imageUrl: "imageUrl" },
                "render": function (data) {
                    return `<img src="${data.imageUrl}" style="border-radius:50%" width="60" height="60" />`
                }
            },
            { "data": "name" },
            { "data": "email" },
            { "data": "phoneNumber" },
            { "data": "address" },
            { "data": "role" },
            {
                "data": { id: "id", lockoutEnd: "lockoutEnd" },

                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                            <a  onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                <i class="fas fa-lock-open"></i>  Unlock
                            </a>
                        </div>
                    `;
                    }
                    else {
                        return `
                            <a  onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                <i class="fas fa-lock"></i>  Lock
                            </a>
                        </div>
                    `;
                    }
                }

            }
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/ApplicationUser/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $('#tblData').DataTable().ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    });
}