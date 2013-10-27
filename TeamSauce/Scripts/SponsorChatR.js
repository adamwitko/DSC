$(function () {
    var proxy = $.connection.sponsorChatHub;
    
    var init = function () {
        
        $("#send-sponsor-message").click(function () {
            var body = $('#sponsor-message').val();
            proxy.server.messageReceived(body);
        });

        proxy.server.getMessages().done(function(messages) {
            alert(messages);
        });
    };
    
    proxy.client.userMessage = function (message) {
        alert('User Message');

        $('#sponsor-feed').append('<li>' +
                                 '<span class="name">' + message.Name + '</span>' +
                                 '<span class="body">' + message.Body + '</span>' +
                                 '<time>' + message.Time + '</time>' +
                                 '</li>');
    };
    
    proxy.client.sponsorMessage = function (message) {
        alert('Sponsor Message');

        $('#sponsor-feed').append('<li>' +
                                 '<span class="name">' + message.Name + '</span>' +
                                 '<span class="body">' + message.Body + '</span>' +
                                 '<time>' + message.Time + '</time>' +
                                 '</li>');
    };

    proxy.client.loginFailed = function () {
        alert("Incorrect username or password");
    };

    $.connection.hub.start().done(init);
});
