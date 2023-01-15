var TestGraph = {
    init: (guid) => {
        var canvas = document.getElementById(guid);
        if (!canvas) return null;

        var config = {
            type: "line",
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
                maintainAspectRatio: false,
                responsive: true
            },
            scales: {
                x: {
                    title: {
                        display: true,
                        text: "X-axis"
                    },
                    ticks: {
                        display: true
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: "Y-axis"
                    },
                    ticks: {
                        display: true
                    }
                }
            }
        };

        return new Chart(canvas, config);
    },
    update: (chart, newData) => {
        chart.data.datasets = newData.map((it) => {
            if (!chart.data.labels || it.series.length > chart.data.labels.length) {
                var i = 0;
                chart.data.labels = it.series.map((_) => i++);
            }
            return {
                label: it.name,
                data: it.series,
                borderColor: it.color
            };
        });
        chart.update();
    }
};

export { TestGraph };