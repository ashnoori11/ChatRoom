////var signalRCdn = document.createElement('script');
////signalRCdn.setAttribute('src', 'https://unpkg.com/@@aspnet/signalr@@1.1.0/dist/browser/signalr.js');
////document.head.appendChild(signalRCdn);

var chattingPerson = "visitor";

var mainchatdialog = document.getElementById('chatDialog');

// Initialize SingalR

var chatHubconnection = new signalR.HubConnectionBuilder()
    .withUrl('/chatHub')
    .build();

chatHubconnection.on('ReciveMessage', renderMessage);

chatHubconnection.onclose(function () {
    onDisconnected();
    console.log('ReConnectiong in 5 second ...');
    setTimeout(startConnection, 5000);
});


function startConnection() {
    chatHubconnection.start().then(onConnected).catch(function (err) {
        console.log(err);
    });
}

function onDisconnected() {
    mainchatdialog.classList.add('disconnected');
}

function onConnected() {
    mainchatdialog.classList.remove('disconnected');

    var messageTextBox = document.getElementById('messageTextBox');
    messageTextBox.focus();
}

function ready() {
    var formChat = document.getElementById('chatForm');
    formChat.addEventListener('submit', function (e) {
        e.preventDefault();
        var text = e.target[0].value;
        e.target[0].value = '';
        sendMessage(text);
    });

    var takeUserNameform = document.getElementById('takeName');
    takeUserNameform.addEventListener('submit', function (e) {
        e.preventDefault();

        var name = e.target[0].value;
        if (name && name.length) {
            takeUserNameform.style.display = 'none';
            chattingPerson = name;
            startConnection();
        }
    });
}

function sendMessage(text) {
    debugger
    if (text && text.length) {
        chatHubconnection.invoke('SendMessage', chattingPerson, text);
    }
}

function renderMessage(name, time, message) {
    var nameSpan = document.createElement('span');
    nameSpan.className = 'name';
    nameSpan.textContent = name;

    var timeSpan = document.createElement('span');
    timeSpan.className = 'time';

    var timeFriendly = moment(time).format('H:mm');
    timeSpan.textContent = timeFriendly;

    var headerDiv = document.createElement('div');
    headerDiv.appendChild(nameSpan);
    headerDiv.appendChild(timeSpan);

    var messageDiv = document.createElement('div');
    messageDiv.className = 'message';
    messageDiv.textContent = message;

    var newItem = document.createElement('li');
    newItem.appendChild(headerDiv);
    newItem.appendChild(messageDiv);

    var chatHistory = document.getElementById('chatHistory');
    chatHistory.appendChild(newItem);

    chatHistory.scrollTop = chatHistory.scrollHeight - chatHistory.clientHeight;

}

document.addEventListener('DOMContentLoaded', ready);

