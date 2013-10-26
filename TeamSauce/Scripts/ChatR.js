$(function () {
    var myConnection = $.connection("/teamchat");

    myConnection.received(function (data) {
        if (data[0] == undefined) {
            $("#messages").append("<li>" + data.Name + ': ' + data.Body + ': ' + data.Time + "</li>");
        } else {
            for (var idx = 0; idx < data.length; idx++) {
                $("#messages").append("<li>" + data[idx].Name + ': ' + data[idx].Body + ': ' + data[idx].Time + "</li>");
            }
        }
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