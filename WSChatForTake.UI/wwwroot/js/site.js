// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var server = 'ws://localhost:5000';

var WEB_SOCKET = new WebSocket(server + '/ws');

WEB_SOCKET.onopen = function (evt) {
    console.log('Connection open ...');
    $('#msgList').val('websocket connection opened .');
};

WEB_SOCKET.onmessage = function (evt) {
    console.log('Received Message: ' + evt.data);
    if (evt.data) {
        var content = $('#msgList').val();
        content = content + '\r\n' + evt.data;

        $('#msgList').val(content);
    }
};

WEB_SOCKET.onclose = function (evt) {
    console.log('Connection closed.');
};

$('#btnJoin').on('click', function () {
    var roomNo = $('#txtRoomNo').val();
    var username = $('#txtusernameName').val();
    if (roomNo) {
        var msg = {
            Action: 'join',
            Room: roomNo,
            Username: username
        };
        WEB_SOCKET.send(JSON.stringify(msg));
    }
});

$('#btnSend').on('click', function () {
    var message = $('#txtMsg').val();
    var username = $('#txtusernameName').val();
    if (message) {
        WEB_SOCKET.send(JSON.stringify({
            Action: 'send_to_room',
            Message: message,
            Username: username
        }));
    }
});

$('#btnLeave').on('click', function () {
    var username = $('#txtusernameName').val();
    var msg = {
        Action: 'leave',
        Message: '',
        Username: username
    };
    WEB_SOCKET.send(JSON.stringify(msg));
});