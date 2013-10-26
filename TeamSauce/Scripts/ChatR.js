﻿$(function () {
    var myConnection = $.connection("/teamchat");

    myConnection.received(function (data) {
        for (var i = 0; i < data.length; i++) {
            var message = data[i];
            $('.chatbox').append('<li>' +
                                 '<span class="name">' + message.name + '</span>' +
                                 '<span class="body">' + message.body + '</span>' +
                                 '<time>' + moment(message.time).fromNow() + '</time>' +
                                 '</li>');
        }
    });

    myConnection.error(function (error) {
        console.warn(error);
    });

    myConnection.start()
        .promise()
        .done(function () {
            $("#chat-submit").click(function() {
                var body = $('.chat-message').val();
                myConnection.send(JSON.stringify({ name: "Josh", body: body }));
            });
        });
});
