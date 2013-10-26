$(function () {
    var myConnection = $.connection("/teamchat");

    myConnection.received(function (data) {
        alert('Received');
        $("#messages").append("<li>" + data.Name + ': ' + data.Body + ': ' + data.Time + "</li>");
    });

    myConnection.error(function (error) {
        console.warn(error);
    });

    myConnection.start()
        .promise()
        .done(function () {
            $("#send").click(function() {
                var myName = $("#Name").val();
                var myMessage = $("#Message").val();
                myConnection.send(JSON.stringify({ name: myName, body: myMessage }));
            });
        });
});