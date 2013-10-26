$(function () {
    var proxy = $.connection.usersHub;

    var init = function() {
        $("#log-in").click(function() {
            var name = $('#username').val();
            var password = $('#password').val();
            proxy.server.logIn(name, password);
        });
    };
    
    proxy.client.completed = function (connectionId) {
        alert(connectionId);
        window.location = '/Home/Index';
    };

    $.connection.hub.start().done(init);
});
