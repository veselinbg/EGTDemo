var url = document.getElementById("messageHubUrl").value;

var connection = new signalR.HubConnectionBuilder()
    .withUrl(url)
    .withAutomaticReconnect()
    .build();

connection.on("SendMessageToClient", function (message) {

    var msg = message.messageData;

    var li = document.createElement("li");
    li.textContent = msg;

    document.getElementById("messagesList").appendChild(li);

    connection.invoke("MessageDelivered", message.messageId);
});

var registerButton = document.getElementById('registerButton');

registerButton.addEventListener("click", function () {

    if (connection.connectionState != "Disconnected") {

        connection.invoke("DeleteClient");

        connection.stop().then(function () {

            registerButton.value = 'UnRegistered';
            registerButton.className = "btn btn-danger";

        });
    }
    else {
        connection.start().then(function () {

            registerButton.value = 'Registered';
            registerButton.className = "btn btn-success";

        });
    }
});