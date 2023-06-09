function init(element, params) {
    if (!element) return ({ success: false });
    var canvas = element.querySelector("canvas");

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
    if (chart.success === false) {
        return;
    }
    chart.data.datasets = newData;
    chart.update();
}

export { init, update };