$(function () {
    var proxy = $.connection.teamSauceHub;
    var answers = [];
    var questionnaireId;
    
    var initLogin = function () {
        $("#signin").click(function () {
            $(this).prop("disabled", !$(this).prop("disabled"));
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
    
    var initQuestionnaireR = function () {
        $('#submitQuestionnaire').click(function () {
            var questionnaireResponse = {
                ratings: answers
            };

            proxy.server.complete(questionnaireId, JSON.stringify(questionnaireResponse));

            answers = [];
            questionnaireId = undefined;
            $('#questions').empty();
            $('#questionnaireModal').modal('hide');
        });
    };
    
    proxy.client.completed = function (connectionId) {
        $('#loginArea').toggle();
        $('#app').toggle();
        $('.dropdown-toggle').dropdown();
        
        $('#signout').click(function() {
            proxy.server.logOut();
            
            var signIn = $('#signin');
            signIn.text("Sign in");
            signIn.prop("disabled", !signIn.prop("disabled"));
            $('#username').val("");
            $('#password').val("");
            
            $('#loginArea').toggle();
            $('#app').toggle();
        });
        
        initSponsorChatR();
        initQuestionnaireR();
        initTeamChatR();
    };

    proxy.client.loginFailed = function (connectionId) {
        var signIn = $('#signin');
        signIn.text("Failed to sign in. Try again!");
        signIn.prop("disabled", !signIn.prop("disabled"));
    };

    proxy.client.messagesLoaded = function (messages) {
        for (var idx = 0; idx < messages.length; idx++) {
            if (messages[idx].MessageType == 'Sponsor') {
                $('#sponsor-feed').prepend('<li class="feed-sponsor">' +
                                '<div><span class="name">' + messages[idx].Sender + '</span>' +
                                '<time>' + moment(messages[idx].Time).fromNow() + '</time></div>' +
                                '<div class="feed-body-div"><span class="body">' + messages[idx].Message + '</span></div>' +
                                '</li>');
            } else {
                $('#sponsor-feed').prepend('<li>' +
                   '<div><span class="name">' + messages[idx].Sender + ' (' + messages[idx].TeamId + ')' + '</span>' +
                    '<time>' + moment(messages[idx].Time).fromNow() + '</time></div>' +
                    '<div class="feed-body-div"><span class="body">' + messages[idx].Message + '</span></div>' +
                    '</li>');
            }
        }
        
        $('#sponsor-feed-holder').scrollTop($('#sponsor-feed-holder')[0].scrollHeight + 200);
    };

    proxy.client.userMessage = function (message) {
        $('#sponsor-feed').append('<li>' +
           '<div><span class="name">' + message.Sender + ' (' + message.TeamId + ')' + '</span>' +
            '<time>' + moment(message.Time).fromNow() + '</time></div>' +
            '<div class="feed-body-div"><span class="body">' + message.Message + '</span></div>' +
            '</li>');

        $('#sponsor-feed-holder').scrollTop($('#sponsor-feed-holder')[0].scrollHeight);
    };

    proxy.client.sponsorMessage = function (message) {
        $('#sponsor-feed').append('<li class="feed-sponsor">' +
            '<div><span class="name">' + message.Sender + '</span>' +
            '<time>' + moment(message.Time).fromNow() + '</time></div>' +
            '<div class="feed-body-div"><span class="body">' + message.Message + '</span></div>' +
            '</li>');
        
        $('#sponsor-feed-holder').scrollTop($('#sponsor-feed-holder')[0].scrollHeight);
    };

    proxy.client.sentOutQuestionnaire = function (questionnaire) {
        questionnaireId = questionnaire.id;

        $.each(questionnaire.categoryQuestions, function (index, value) {
            $('#questions').prepend('<div class="question">' +
                '<span class="glyphicon glyphicon-record"></span><span>' + value.text +
                '</span><div class="star" data-category=' + value.category + '/></div>');
        });

        $('#questionnaireModal').modal('show');

        $('.star').raty({
            number: 10,
            'click': function (score) {
                answers.push({
                    categorytype: $(this).attr('data-category'),
                    value: score
                });
            }
        });
    };

    var initTeamChatR = function () {
        $("#send-team-message").click(function () {
            if (sponsorChatEnabled) {
                var body = $('#team-message').val();
                $('#team-message').val('');
                $('#send-team-message').val('Sending...');
                $('#send-team-message').addClass('btn-disabled');

                sponsorChatEnabled = false;
                proxy.server.teamMessageReceived(body).done(function () {
                    $('#send-team-message').val('Send');
                    $('#send-team-message').removeClass('btn-disabled');
                    sponsorChatEnabled = true;
                });
            }
        });

        proxy.server.getMessages();
    };

    proxy.client.teamMessage = function (message) {
        $('#team-feed').append('<li>' +
           '<div><span class="name">' + message.Sender + '</span>' +
            '<time>' + moment(message.Time).fromNow() + '</time></div>' +
            '<div class="feed-body-div"><span class="body">' + message.Message + '</span></div>' +
            '</li>');
        
        $('#team-feed-holder').scrollTop($('#team-feed-holder')[0].scrollHeight);
    };

    $.connection.hub.start().done(function () {
        initLogin();
    });
});
