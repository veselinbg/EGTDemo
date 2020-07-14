var sendButton = document.getElementById("sendButton");

sendButton.disabled = true;

sendButton.addEventListener("click", function (event) {

    var message = document.getElementById("messageInput").value;

    connection.invoke("SaveMessage", message);

    event.preventDefault();
});

var url = document.getElementById("messageHubUrl").value;

var connection = new signalR.HubConnectionBuilder()
    .withUrl(url)
    .withAutomaticReconnect()
    .build();

connection.start().then(function () {

    connection.on("SendMessageToClient", function (message) {

        var div = document.createElement("div");
        div.className = "row";
        document.getElementById("messagesList").appendChild(div);

        div.innerHTML = "<div class='col-1'><img id='" + message.messageId + "' src='images/send-message-icon.png' title='Send' width='45' height='45' /></div><div class='col-11' style ='align-content:baseline'>" + message.messageData + "</div>";

        connection.invoke("MessageDelivered", message.messageId);
    });

    connection.on("MessageDeliveredToAllClients", function (messageId) {

        var img = document.getElementById(messageId);

        img.src = "images/message-delevered-icon.png";
        img.title = "Delevered message";
    });

    connection.invoke("RegisterManagementClient").then(function () {

        sendButton.disabled = false;
    });
});