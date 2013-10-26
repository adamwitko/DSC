$(function () {

    $.getJSON('/que/allaverage', function(data) {
        var greatData = {};

        for (var i = 0; i < data.length; i++) {
            var q = data[i], time = q.time, cats = q.categories;

            for (var name in cats) {
                if (greatData[name]) {
                    greatData[name] += [cats[name]];
                } else {
                    greatData[name] = [cats[name]];
                }
            }
        }

        var names = [];
        for (var name in greatData) {
            names += [name];
        }

        var datasets = [];
        for (var name in greatData) {
            datasets += [{
                fillColor : "rgba(220,220,220,0.5)",
                strokeColor : "rgba(220,220,220,1)",
                pointColor : "rgba(220,220,220,1)",
                pointStrokeColor : "#fff",
                data : greatData[name];
            }];
        }

        var data = {
            labels : names,
            datasets : datasets
        }

        var ctx = document.getElementById("that-big-graph").getContext("2d");

        new Chart(ctx).Line(data);
    });
});
