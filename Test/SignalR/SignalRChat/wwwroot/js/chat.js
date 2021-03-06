// The following sample code uses modern ECMAScript 6 features 
// that aren't supported in Internet Explorer 11.
// To convert the sample for environments that do not support ECMAScript 6, 
// such as Internet Explorer 11, use a transpiler such as 
// Babel at http://babeljs.io/. 
//
// See Es5-chat.js for a Babel transpiled version of the following code:

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://weathermanagement.azurewebsites.net/notifications")
    .build();

connection.on("ReceiveMessage", ( message) => {
    const encodedMsg =  message;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    console.log(message);
    document.getElementById("messagesList").appendChild(li);
});

document.getElementById("sendButton").addEventListener("click", event => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(err => console.error(err.message));
    event.preventDefault();
});


document.getElementById("connectRoomButton").addEventListener("click", event => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    connection.invoke("JoinGroup", user).catch(err => console.error(err.message));
    event.preventDefault();
});




connection.start().then(function () {
    const li = document.createElement("li");
    li.textContent = "ConnetionId: " + connection.connectionId;
    console.log(connection.connectionId);
    document.getElementById("messagesList").appendChild(li);

}).catch(err => console.error(err.message));