$(function () {
    var myConnection = $.connection("/teamchat");

    myConnection.received(function (data) {
//<<<<<<< HEAD
//        if (data[0] == undefined) {
//            $("#messages").append("<li>" + data.Name + ': ' + data.Body + ': ' + data.Time + "</li>");
//        } else {
//            for (var idx = 0; idx < data.length; idx++) {
//                $("#messages").append("<li>" + data[idx].Name + ': ' + data[idx].Body + ': ' + data[idx].Time + "</li>");
//            }
//        }
//=======
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
            $("#chat-submit").click(function() {
                var body = $('.chat-message').val();
                myConnection.send(JSON.stringify({ name: "Josh", body: body }));
            });
        });
});
