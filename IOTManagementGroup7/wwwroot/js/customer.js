var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    //var defaultImg = "~//asset//img//defaultuser.png";
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/AuthCustomer/Customer/GetAll"
        },
        "columns": [
            {
                "data": { id: "id", imageUrl: "imageUrl" },
                "render": function (data) {
                    if (data.imageUrl != null) {
                        return `
                        <div class="text-center">              
                            <img src="${data.imageUrl}" style="border-radius:50%" width="60" height="60" />
                        </div>`
                    }
                    else {
                        return `
                        <div class="text-center">              
                            <img src="https://blog.cpanel.com/wp-content/uploads/2019/08/user-01.png" style="border-radius:50%" width="60" height="60" />
                        </div>`
                    }
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
                        <div class="text-center">
                            <a  onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                <i class="fas fa-lock-open"></i> &nbsp; Mở
                            </a>
                        </div>
                    `;
                    }
                    else {
                        return `
                        <div class="text-center">
                            <a  onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                <i class="fas fa-lock"></i> &nbsp; Khóa
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
        url: '/AuthCustomer/Customer/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}