$(function () {
    var myConnection = $.connection("/teamchat");

    myConnection.received(function (data) {
        for (var i = 0; i < data.length; i++) {
            var message = data[i];
            $('team-chatbox').append('<li>' +
                                 '<span class="name">' + message.Name + '</span>' +
                                 '<span class="body">' + message.Body + '</span>' +
                                 '<time>' + moment(message.Time).fromNow() + '</time>' +
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
                myConnection.send(JSON.stringify({ Name: "Josh", Body: body }));
            });
        });

     $('.atwho').atwho({
         at: "@",
         data: ["Josh", "Adam", "Olly", "James"],
     });
});
