$(function () {
    $.getJSON('/que/allaverage', function(data) {
        var labels = [];
        for (var i = 0; i < data.length; i++) {
            labels[labels.length] = data[i].time;
        }

        // dict containing name of category with list of data points over time
        var result = {};
        for (var name in data[0].categories) {
            result[name] = [];
            for (var i = 0; i < data.length; i++) {
                result[name][i] = data[i].categories[name];
            }
        }

        var datasets = [];
        for (var name in result) {
            datasets[datasets.length] = {
                fillColor : "rgba(220,220,220,0.5)",
                strokeColor : "rgba(220,220,220,1)",
                pointColor : "rgba(220,220,220,1)",
                pointStrokeColor : "#fff",
                data : result[name]
            };
        }

        var data = {
            labels : labels,
            datasets : datasets
        }

        var ctx = document.getElementById("that-big-graph").getContext("2d");

        new Chart(ctx).Line(data);
    });
});
