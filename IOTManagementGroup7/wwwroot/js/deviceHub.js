"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/deviceHub").build();

connection.on("ReceiveSignalDevice", function (obj) {
    TurnOnOff(obj.id.toString(), obj.deviceType.onImage.toString(), obj.deviceType.offImage.toString());
});

connection.start();

//---------------------------TURN ON OFF REAL TIME---------------------------
function TurnOnOffSignalR(deviceId) {
    connection.invoke("SendSignalDevice", deviceId.toString()).catch(function (err) {
        return console.error(err.toString());
    });
}
//---------------------------TURN ON OFF REAL TIME---------------------------

//TẠO BUTTON KÍCH HOẠT CONNECTION
//connection.start().then(function () {
//    document.getElementById("sendButton").disabled = false;
//}).catch(function (err) {
//    return console.error(err.toString());
//});





