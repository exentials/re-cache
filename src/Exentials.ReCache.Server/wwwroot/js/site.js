"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/monitorHub").build();

connection.on("ReceiveLog", function (message) {
    var li = document.createElement("li");
    document.getElementById("monitorLog").appendChild(li);
    li.textContent = message;
});

connection
    .start()
    .then(function () {

    })
    .catch(function (err) {
        return console.error(err.toString());
    });

