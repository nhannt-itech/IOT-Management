function TurnOnOff(id,on,off) {
    $.ajax({
        type: "POST",
        url: "/AuthCustomer/Device/TurnOnOff/" + id,
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $('#' + id + '.image').attr('src', on);
            }
            else {
                toastr.error(data.message);
                $('#' + id + '.image').attr('src', off);
            }
        }
    });
}