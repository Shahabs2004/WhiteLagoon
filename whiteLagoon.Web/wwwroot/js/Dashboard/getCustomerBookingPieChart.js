
$(document).ready(function() {
    loadCustomerBookingsPieChart();
});

function loadCustomerBookingsPieChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: "/Dashboard/GetBookingPieChartData",
        type: "GET",
        dataType: "json",
        success: function(data) {

            loadPieChart("customerBookingsPieChart", data);

            $(".chart-spinner").hide();
        }
    });
}


function loadPieChart(id, data) {
    const chartColors = getChartColorsArray(id);
    const options = {
        series: data.series,
        labels: data.labels,
        colors: chartColors,
        chart: {
            type: 'donut',
            width:380
        },
        legend: {
            position: 'bottom',
            horizontalAlign: 'center',
            labels: {
                colors: "#fff",
                useSerialsColors:true
            }
        }
    };
    const chart = new ApexCharts(document.querySelector(`#${id}`), options);
    chart.render();
}