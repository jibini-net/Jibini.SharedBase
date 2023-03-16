function init(guid, params) {
    var canvas = document.getElementById(guid).querySelector("canvas");
    if (!canvas) return null;

    var config = {
        type: params.type,
        options: {
            animation: {
                duration: 0
            },
            plugins: {
                legend: {
                    display: true,
                    position: "right"
                },
                title: {
                    display: true,
                    text: "Test Graph"
                },
            },
            scales: {
                x: {
                    display: true,
                    position: "bottom",
                    type: "linear",
                    title: {
                        display: true,
                        text: params.xAxisLabel
                    },
                    ticks: {
                        display: true
                    }
                },
                y: {
                    display: true,
                    position: "left",
                    type: "linear",
                    title: {
                        display: true,
                        text: params.yAxisLabel
                    },
                    ticks: {
                        display: true
                    }
                }
            },
            maintainAspectRatio: false,
            responsive: true
        }
    };

    return new Chart(canvas, config);
}

function update(chart, newData) {
    chart.data.datasets = newData;
    chart.update();
}

export { init, update };