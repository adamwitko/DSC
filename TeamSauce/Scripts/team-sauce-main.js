$(function() {
    var proxy = $.connection.teamSauceHub;

    var init_LoginR = function() {
        $("#log-in").click(function() {
            var name = $('#username').val();
            var password = $('#password').val();
            proxy.server.logIn(name, password);
        });
    };

    proxy.client.completed = function() {
        init_SponsorChatR();
    };

    proxy.client.loginFailed = function() {
        alert('Username or password is incorrect');
    };

    var init_SponsorChatR = function() {

        $("#send-sponsor-message").click(function() {
            var body = $('#sponsor-message').val();
            proxy.server.messageReceived(body);
        });

        proxy.server.getMessages();
    };

    proxy.client.messagesLoaded = function(messages) {
        alert('Messages Returned' + messages + ':' + messages.length);
        for (var idx = 0; idx < messages.length; idx++) {
            if (messages[idx].MessageType == 'Sponsor') {
                $('#sponsor-feed').append('<li class="feed-sponsor">' +
                    '<span class="name">' + messages[idx].Sender + '</span>' +
                    '<span class="body">' + messages[idx].Message + '</span>' +
                    '<time>' + messages[idx].Time + '</time>' +
                    '</li>');
            } else {
                $('#sponsor-feed').append('<li>' +
                    '<span class="name">' + messages[idx].Sender + '</span>' +
                    '<span class="body">' + messages[idx].Message + '</span>' +
                    '<time>' + messages[idx].Time + '</time>' +
                    '</li>');
            }
        }
    };


    proxy.client.userMessage = function(message) {
        $('#sponsor-feed').append('<li>' +
            '<span class="name">' + message.Sender + '</span>' +
            '<span class="body">' + message.Message + '</span>' +
            '<time>' + message.Time + '</time>' +
            '</li>');
    };

    proxy.client.sponsorMessage = function(message) {
        $('#sponsor-feed').append('<li class="feed-sponsor">' +
            '<span class="name">' + message.Sender + '</span>' +
            '<span class="body">' + message.Message + '</span>' +
            '<time>' + message.Time + '</time>' +
            '</li>');
    };

    $.connection.hub.start().done(function() {
        init_LoginR();
    });
});