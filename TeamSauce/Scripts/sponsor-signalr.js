$(function () {
    var proxy = $.connection.teamSauceHub;
    var answers = [];
    var questionnaireId;
    
    var initLogin = function () {
        $("#signin").click(function () {
            var username = $('#username').val();
            var password = $('#password').val();
            proxy.server.logIn(username, password);
        });
    };

    var sponsorChatEnabled = true;

    var initSponsorChatR = function () {
        $("#send-sponsor-message").click(function () {
            if (sponsorChatEnabled) {
                var body = $('#sponsor-message').val();
                $('#sponsor-message').val('');
                $('#send-sponsor-message').val('Sending...');
                $('#send-sponsor-message').addClass('btn-disabled');

                sponsorChatEnabled = false;
                proxy.server.messageReceived(body).done(function() {
                    $('#send-sponsor-message').val('Send');
                    $('#send-sponsor-message').removeClass('btn-disabled');
                    sponsorChatEnabled = true;
                });
            }
        });

        proxy.server.getMessages();
    };
    
    proxy.client.completed = function (connectionId) {
        $('#loginArea').toggle();
        $('#app').toggle();
        $('.dropdown-toggle').dropdown();
        
        $('#signout').click(function() {
            proxy.server.logOut();
            $('#loginArea').toggle();
            $('#app').toggle();
        });
        
        initSponsorChatR();
        initQuestionnaireR();
    };

    proxy.client.loginFailed = function (connectionId) {
        alert('Fail ' + connectionId);
    };

    proxy.client.messagesLoaded = function (messages) {
        for (var idx = 0; idx < messages.length; idx++) {
            if (messages[idx].MessageType == 'Sponsor') {

                $('#sponsor-feed').append('<li class="feed-sponsor">' +
                                '<div><span class="name">' + messages[idx].Sender + '</span>' +
                                '<time>' + messages[idx].Time + '</time></div>' +
                                '<div class="feed-body-div"><span class="body">' + messages[idx].Message + '</span></div>' +
                                '</li>');

            } else {
                $('#sponsor-feed').append('<li>' +
                    '<div><span class="name">' + messages[idx].Sender + '</span>' +
                    '<time>' + messages[idx].Time + '</time></div>' +
                    '<div class="feed-body-div"><span class="body">' + messages[idx].Message + '</span></div>' +
                    '</li>');
            }
        }
    };

    proxy.client.userMessage = function (message) {
        $('#sponsor-feed').prepend('<li>' +
            '<div><span class="name">' + message.Sender + '</span>' +
            '<time>' + message.Time + '</time></div>' +
            '<div class="feed-body-div"><span class="body">' + message.Message + '</span></div>' +
            '</li>');
    };

    proxy.client.sponsorMessage = function (message) {
        $('#sponsor-feed').prepend('<li class="feed-sponsor">' +
            '<div><span class="name">' + message.Sender + '</span>' +
            '<time>' + message.Time + '</time></div>' +
            '<div class="feed-body-div"><span class="body">' + message.Message + '</span></div>' +
            '</li>');
    };

    $.connection.hub.start().done(function () {
        initLogin();
    });
});