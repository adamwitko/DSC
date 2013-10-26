var myConnection = $.connection("/chat");

myConnection.received(function (data) {
    $("#messages").append("<li>" + data.Name + ': ' + data.Message + "</li>");
});

myConnection.error(function (error) {
    console.warn(error);
});