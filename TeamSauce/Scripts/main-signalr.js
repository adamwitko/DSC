$(function () {
    var proxy = $.connection.teamSauceHub;

    var init_LoginR = function () {
        $("#log-in").click(function () {
            var name = $('#username').val();
            var password = $('#password').val();
            proxy.server.logIn(name, password);
        });
    };

    proxy.client.completed = function (connectionId) {
        init_SponsorChatR();
        init_QuestionnaireR();
    };

    proxy.client.loginFailed = function (connectionId) {
        alert('Fail ' + connectionId);
    };

    var init_SponsorChatR = function () {

        $("#send-sponsor-message").click(function () {
            var body = $('#sponsor-message').val();
            proxy.server.messageReceived(body);
        });

        proxy.server.getMessages();
    };

    proxy.client.messagesLoaded = function (messages) {
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

    proxy.client.userMessage = function (message) {
        $('#sponsor-feed').append('<li>' +
            '<span class="name">' + message.Sender + '</span>' +
            '<span class="body">' + message.Message + '</span>' +
            '<time>' + message.Time + '</time>' +
            '</li>');
    };

    proxy.client.sponsorMessage = function (message) {
        $('#sponsor-feed').append('<li class="feed-sponsor">' +
            '<span class="name">' + message.Sender + '</span>' +
            '<span class="body">' + message.Message + '</span>' +
            '<time>' + message.Time + '</time>' +
            '</li>');
    };

    var answers = [];
    var questionnaireId;

    var init_QuestionnaireR = function () {
        $('#submitQuestionnaire').click(function () {
            var questionnaireResponse = {
                ratings: answers
            };

            proxy.server.complete(questionnaireId, JSON.stringify(questionnaireResponse));

            answers = [];
            questionnaireId = undefined;
            $('#mymodal').modal('hide');
        });
    };

    proxy.client.sentOutQuestionnaire = function (questionnaire) {
        questionnaireId = questionnaire.id;

        $.each(questionnaire.categoryQuestions, function (index, value) {
            $('#questions').append('<div class="question" ng-repeat="question in questions">' +
                '<span class="glyphicon glyphicon-record"></span><span>' + value.text +
                '</span><div class="star" data-category=' + value.category + '/></div>');
        });

        $('#mymodal').modal('show');

        $('.star').raty({
            'click': function (score) {
                answers.push({
                    categorytype: $(this).attr('data-category'),
                    value: score
                });
            }
        });
    };

    $.connection.hub.start().done(function () {
        init_LoginR();
    });
});