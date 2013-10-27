$(function () {
    var proxy = $.connection.teamSauceHub;

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
    
    proxy.client.loginFailed = function () {
        alert("Incorrect username or password");
    };

    $.connection.hub.start().done(init);
});
