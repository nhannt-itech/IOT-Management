function TurnOnOff(id, on, off) {
    $.ajax({
        type: "POST",
        url: "/Customer/Device/TurnOnOff/" + id,
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

