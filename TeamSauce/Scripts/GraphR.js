$(function () {
    var conn = $.connection("/qs");

    conn.received(function (data) {
        var greatData = {};

        for (var i = 0; i < data.Qs.length; i++) {
            var q = data.Qs[i];

            for (var j = 0; j < q.Categories.length; j++) {
                var c = q.Categories[j];

                if (greatData[c.Name]) {
                    greatData[c.Name] += [c.Value];
                } else {
                    greatData[c.Name] = [c.Value];
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

    conn.error(function (error) {
        console.warn(error);
    });

    conn.start()
        .promise()
        .done(function () {
            // on start up
        });
});
