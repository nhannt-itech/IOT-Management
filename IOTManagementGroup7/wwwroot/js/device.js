function TurnOnOff(id, on, off) {
    var sensorId = document.getElementById("sensorId").value;

    $.ajax({
        type: "POST",
        url: "/Customer/Device/TurnOnOff/" + id,
        success: function (data) {
            if (data.success) {
                if (data.sensorId == sensorId) {
                    toastr.success(data.message);
                }
                $('#' + id + '.image').attr('src', on);
            }
            else {
                if (data.sensorId == sensorId) {
                    toastr.error(data.message);
                }
                $('#' + id + '.image').attr('src', off);
            }
        }
    });
}


function ChangeRangeSlider(id) {
    var value = $('#' + id + '.myRangeInput').val();

    $.ajax({
        type: "POST",
        url: "/Customer/Device/ChangeRangeSlider/?id=" + id + "&value=" + value,
        success: function (data) {
            if (data.success) {
                $('#' + id + '.myRangeOutput').attr('value', "Pwr: " + value + "/" + data.maxvalue);
            }
        }
    });
}

function Delete(url, id) {
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
                        const myNode = document.getElementById(id);
                        myNode.remove();
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


