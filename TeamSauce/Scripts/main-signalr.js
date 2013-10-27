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

    $.connection.hub.start().done(function () {
        initLogin();
    });
});
