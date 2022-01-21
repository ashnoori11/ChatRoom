var activeRoomId = '';

// inject signalr connection hub into js file | hub connection on js
var agentConnection = new signalR.HubConnectionBuilder()
    .withUrl('/agentHub')
    .build();

// this event called by onConnected in agent hub and runs loadRooms function
agentConnection.on('ActiveRooms', loadRooms);

// agent handle disconnecting and send start connection request for agent when disconnevt every 20 secends
agentConnection.onclose(function () {

    agentConnectionHandler(startAgentConnection);
});

function agentConnectionHandler(tryConnect) {

    setTimeout(tryConnect, 20000);
}

function startAgentConnection() {

    agentConnection.start().catch(function (err) {
        console.log(err);
    });
}


// connection with chat hub 
var chatConnection = new signalR.HubConnectionBuilder()
    .withUrl('/chatHub')
    .build();

chatConnection.onclose(function () {

    // When the connection is closed we will try to re connect again with handleDisconnected method
    handleDisconnected(startChatConnection);
});

// in onConnected method on Chathub class , we have have line of code wich we used clients.caller and we saied if this line want to run ,
// must call ReciveMessage on js file , and js file will call an other function
chatConnection.on('ReciveMessage', addMessage);
agentConnection.on('ReciveMessages', addMessages);
// agentConnection.on('ReciveMessage', addMessages);

function startChatConnection() {
    chatConnection.start();
}

// try to reconnect if connection is closed
function handleDisconnected(tryConnect) {

    //if connection was closed , try to reConnect every 20 secends
    setTimeout(tryConnect, 20000);
}

function sendMessage(text) {

    if (text && text.length) {
        agentConnection.invoke('AgentSendMessageToUser', activeRoomId, text);
    }
}

// this function handle all things about connection to hubs and send messages
function ready() {

    // connect agent with both agent hub and chat hub
    startAgentConnection();
    startChatConnection();

    var chatFormEl = document.getElementById('chatForm');

    chatFormEl.addEventListener('submit', function (e) {
        e.preventDefault();

        var text = e.target[0].value;
        e.target[0].value = '';
        sendMessage(text);
    });
}

// get all rooms list
var roomLists = document.getElementById("roomList");

// get history of a rooom
var roomHistory = document.getElementById("chatHistory");

// by click on a room link in the list of rooms that shows to agent 
// this function runs and set the room button active
// then take roomid from data-id attribute and switch agent to The room clicked on by call switchActiveRoomTo and send room id as paramter to that function
roomLists.addEventListener('click', function (e) {
    roomHistory.style.display = 'block';
    setActiveRoomButton(e.target);

    var roomid = e.target.getAttribute('data-id');
    switchActiveRoomTo(roomid);
});

// this function take an element that agent clicked on and removes active class from all other <a> and set to our special button with a tag
function setActiveRoomButton(el) {
    var allButtons = roomLists.querySelectorAll('a.list-group-item');
    allButtons.forEach(function (btn) {

        btn.classList.remove('active');
    });

    el.classList.add('active');
}

// loadRooms function runs on agent connecting and returns all active rooms to show to agent
function loadRooms(rooms) {
    
    // if function paramteres or server response for this request was null , Do not continue
    if (!rooms) return;
     
    // if rooms was not null , then we find all room idies and put them in roomidies (room) parameter is a c# dictionery and room idies are guid
    var roomIdies = Object.keys(rooms);

    //then check if roomidies was null , Do not continue
    if (!roomIdies.length) return;

    //So that the agent can move between rooms
    switchActiveRoomTo(null);

    // remove rooms from list
    removeAllChildren(roomLists);

    //roomIdies.forEach(function (id) {
    //    var roomInfo = rooms[id];
    //    if (!roomInfo.roomName) return;
    //     
    //    // Means chat items inside a room
    //    var roomButton = createRoomButton(id, roomInfo);

    //    roomLists.appendChild(roomButton);
    //});
    
    roomIdies.forEach(function (id) {
        var roomInfo = rooms[id];
        if (!roomInfo.roomName) return;

        var roomButton = createRoomButton(id, roomInfo);
        roomLists.appendChild(roomButton);
    });
}

// It is required to returns the status of all chats in addition to all text messages in the room
function createRoomButton(id, roomifo) {
     
    var anchorEL = document.createElement('a');
    anchorEL.className = 'list-group-item list-group-item-action d-flex justify-content-between align-items-center';
    anchorEL.setAttribute('data-id', id);
    anchorEL.textContent = roomifo.roomName;
    anchorEL.href = "#";

    return anchorEL;
}

function switchActiveRoomTo(id) {
    

    //That is, it is now in the same room that it clicked on , Do not continue
    if (id === activeRoomId) return;

    // if activeRoomid not null
    if (activeRoomId) {

        // call agentLeaveRoom to run and agent will leave this recent group
        chatConnection.invoke('agentLeaveRoom', activeRoomId);
    }

    // now activeroomid take a new value a new room id
    activeRoomId = id;

    //then we remove all of messages on recent room
    removeAllChildren(roomHistory);

    // if there was no any new room id to switch do not continue
    if (!id) return;

    // if id has value of a new room id to swith on , then agent must join to room by calling AgentJoinRoom on chatHub class
    chatConnection.invoke('AgentJoinRoom', activeRoomId);
    agentConnection.invoke('LoadHistory', activeRoomId);
}

function addMessages(messages) {
    
    if (!messages) return;

    messages.forEach(function (m) {
        addMessage(m.senderName, m.sendAt,m.messageBody);
    });
}

function addMessage(name, time, message) {
    
    var nameSpan = document.createElement('span');
    nameSpan.className = 'name';
    nameSpan.textContent = name;

    var timeSpan = document.createElement('span');
    timeSpan.className = 'time';
    var friendlyTime = moment(time).format('HH:mm');
    timeSpan.textContent = friendlyTime;

    var headerDiv = document.createElement('div');
    headerDiv.appendChild(nameSpan);
    headerDiv.appendChild(timeSpan);

    var messageDiv = document.createElement('div');
    messageDiv.className = 'message';
    messageDiv.textContent = message;

    var newItem = document.createElement('li');
    newItem.appendChild(headerDiv);
    newItem.appendChild(messageDiv);

    roomHistory.appendChild(newItem);
    roomHistory.scrollTop = roomHistory.scrollHeight - roomHistory.clientHeight;
}

function removeAllChildren(node) {

    // if function paramteres or server response for this request was null , Do not continue
    if (!node) return;

    // delete all room childrens
    while (node.lastChild) {
        node.removeChild(node.lastChild);
    }
}

// call and run redy function on load page
document.addEventListener('DOMContentLoaded', ready);