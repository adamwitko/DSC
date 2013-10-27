$(function () {
    var updateGraph = function (data) {
        var somecoloursets = [{
                fillColor: "rgba(220,220,220,0)",
                strokeColor: "rgba(220,220,220,1)",
                pointColor: "rgba(220,220,220,0.5)",
                pointStrokeColor: "#fff"
            },
            {
                fillColor: "rgba(220,80,220,0)",
                strokeColor: "rgba(220,80,220,1)",
                pointColor: "rgba(220,80,220,0.5)",
                pointStrokeColor: "#fff"
            },
            {
                fillColor: "rgba(220,220,80,0)",
                strokeColor: "rgba(220,220,80,1)",
                pointColor: "rgba(220,220,80,0.5)",
                pointStrokeColor: "#fff"
            },
            {
                fillColor: "rgba(80,220,220,0)",
                strokeColor: "rgba(80,220,220,1)",
                pointColor: "rgba(80,220,220,0.5)",
                pointStrokeColor: "#fff"
            }];

        var ctx = document.getElementById("that-big-graph").getContext("2d");

        var opts = {
            //Boolean - If we want to override with a hard coded scale
            scaleOverride: true,

            //** Required if scaleOverride is true **
            //Number - The number of steps in a hard coded scale
            scaleSteps: 10,
            //Number - The value jump in the hard coded scale
            scaleStepWidth: 1,
            //Number - The scale starting value
            scaleStartValue: 0
        };

        if (data.length < 1) {
            new Chart(ctx).Line({ labels: [], datasets: [] }, opts);
            return;
        }

        var labels = [];
        for (var i = 0; i < data.length; i++) {
            labels[labels.length] = moment(data[i].time).fromNow();
        }

        // dict containing name of category with list of data points over time
        var result = { };
        for (var category in data[0].categories) {
            result[category] = [];
            for (var i = 0; i < data.length; i++) {
                result[category][i] = data[i].categories[category];
            }
        }
        
        var graphKeysList = $('.graph-key ul');
        graphKeysList.empty();
        
        var datasets = [];
        for (var name in result) {
            var colourset = somecoloursets.pop();
            colourset.data = result[name];

            datasets[datasets.length] = colourset;

            graphKeysList.append('<li style="background-color:' +
                colourset.pointColor + '">' +
                '<span>' + name + '</span>' +
                '</li>');
        }

        new Chart(ctx).Line({ labels: labels, datasets: datasets }, opts);
    };
    
    var proxy = $.connection.teamSauceHub;
    
    proxy.client.getData = updateGraph;
    
    proxy.client.showQuestionnaireCompleteNotification = function (data) {
        $.pnotify.defaults.history = false;
        $.pnotify({
            title: data.title,
            text: data.text
        });
    };

    $.connection.hub.start().done(function () {
        proxy.server.getData();
    });
});
