$(function () {
    var myConnection = $.connection("/teamchat");

    myConnection.received(function (data) {
        $('.chatbox').append('<li>' +
                             '<span class="name">' + message.name + '</span>' +
                             '<span class="body">' + message.body + '</span>' +
                             '<time>' + moment(message.time).fromNow() + '</time>' +
                             '</li>');
    });

    myConnection.error(function (error) {
        console.warn(error);
    });

    myConnection.start()
        .promise()
        .done(function () {
            $('.chat-message').keydown(function(event){
                var keycode = (event.keyCode ? event.keyCode : event.which);

                if (keycode == '13') {
                    var body = $('.chat-message').val();

                    myConnection.send(JSON.stringify({ name: currentUser, body: body }));
                    event.preventDefault();
                }
            });
        });
});
