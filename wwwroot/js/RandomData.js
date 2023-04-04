"use strict"

const { signalR } = require("../lib/microsoft/signalr/dist/browser/signalr")

var connection = new signalR.HubConnectionBuilder.withUrl("/iot").build();
connection.start();
connection.on("Receive", function(msg))  {
    var li = document.createElement("li");
    li.textContent = msg;
    document.getElementById("ss").appendChild(li);
});