$(document).ready(function () {

    const $tabLink = $('#tabs-section .tab-link');
    const $tabBody = $('#tabs-section .tab-body');
    let timerOpacity;
    debugger;
    //initialise variables
    var excelobj;
    var edate2;
    var sdate2;
    // define dates and assign values with date diff 365days
    now = new Date();
    edate2 = now.setDate(now.getDate() - 1);
    now = new Date();
    sdate2 = now.setDate(now.getDate() - 365);

    $("#sdate2").dxDateBox({
        type: "date",
        value: sdate2
    });

    $("#edate2").dxDateBox({
        type: "date",
        value: edate2
    });
    // assign data coming from controller into a javascript object
    var data = obj.MonthlyChartLocwisedata;
    debugger;
    excelobj = data;
    // on click of submit button redirect to a function
    document.getElementById("gbt").onclick = function () {
        debugger;
        Chartajax();
    }
    document.getElementById("gbt").disabled = true;
    document.getElementById("gbt").style.pointerEvents = 'none';
    var sdate1;
    var edate1;
    var datestrt;
    var dateed;
    sdate1 = data.StDate;
    edate1 = data.EdDate;
    debugger;
    $("#sdate1").dxDateBox({
        type: "date",
        value: sdate1
    });

    $("#edate1").dxDateBox({
        type: "date",
        value: edate1
    });

    debugger;
    generateChart1(data);

    document.getElementById('dates').style.display = "block";


    // validation of dates 
    function validateDate() {
        var start = $("#sdate1").dxDateBox("option", "value");
        var end = $("#edate1").dxDateBox("option", "value");
        /* change short date string to timestamp*/
        var dstart = new Date(start);
        var dend = new Date(end);
        // regular expression to match required date format
        re = /^\d{4}\/\d{2}\/\d{2}$/;
        //re = /^\d{4}\/\d{1,2}-\d{1,2}$/;
        debugger;
        if (document.getElementById('sdate1').value == '' || document.getElementById('edate1').value == '') {
            alert("Dates not selected ");
            document.getElementById('sdate1').focus();
            return false;
        }



        var sDate = Date.parse(dstart);
        var eDate = Date.parse(dend);

        if (sDate > eDate) {
            alert("Inappropriate Selection of Dates");
            $('.selectpicker').prop('disabled', true);
            //$('.selectpicker').selectpicker('refresh');

        }
        else {
            $('.selectpicker').prop('disabled', false);
            //$('.selectpicker').selectpicker('refresh');
        }
        return true;
    }

    function validatepage() {
        if (validateDate()) {
            return true;
        }
        else {
            return false;
        }
    }

    $(function () {

        $(".datepicker").datepicker();

    });

    debugger;

    // Side Menu Click
    $tabLink.off('click').on('click', function (e) {

        // Prevent Default
        e.preventDefault();
        e.stopPropagation();

        // Clear Timers
        window.clearTimeout(timerOpacity);

        // Toggle Class Logic
        // Remove Active Classes
        $tabLink.removeClass('active ');
        $tabBody.removeClass('active ');
        $tabBody.removeClass('active-content');

        // Add Active Classes
        $(this).addClass('active');
        $($(this).attr('href')).addClass('active');

        // Opacity Transition Class
        timerOpacity = setTimeout(() => {
            $($(this).attr('href')).addClass('active-content');
        }, 50);


    });

    $('#tabheader li:nth-child(1) a').click();


    // function for fetching the data for displaying chart
    function Chartajax() {
        validatepage();
        var strt = $("#sdate1").dxDateBox("option", "value");
        var ed = $("#edate1").dxDateBox("option", "value");

        /* change short date string to timestamp*/
        var dstrt = new Date(strt);
        var ded = new Date(ed);
        var ltype = "";
        var loc = "";
        datestrt = Date.parse(dstrt);
        dateed = Date.parse(ded);
        debugger;
        //send parameters and receive data from controller
        $.ajax({
            type: "POST",
            url: "../CPCWW/CPCCharts1/",
            data: JSON.stringify({ startdate: datestrt, enddate: dateed, type: ltype, location: loc }),

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                debugger;
                var data = data.data.MonthlyChartLocwisedata;
                excelobj = data;
                generateChart1(data);



            },
            error: function (err) {
                $.notify('Unable to fetch Chart Data. Please Try Again.', {
                    globalPosition: "top center",
                    className: "success"
                });
            }

        });
    }

    function generateChart1(data) {
        debugger;
        var chartdata = data.WWCPC_Charts;


    //$(() => {
    var chart = $('#cpcchart').dxChart({
        palette: 'Violet',
        //assign data coming from controller to dataSource
            dataSource: chartdata,
            size: {
                height: 400,
                width: 1100,
            },
          // code to hide and show a particular value's data(values belonging to the legend) on the chart
            onLegendClick: function (e) {
                var series = e.target;
                if (series.isVisible()) {
                    series.hide();
                } else {
                    series.show();
                }
            },


            resolveLabelOverlapping: 'stack',
        // assign argument field and value field values as the names attributes coming from data object
        commonSeriesSettings: {
                argumentField: 'Monthyear',
                valueField: 'TotalUtilization',
                //name: 'Utilization in Hours',
                type: 'line',

        },
        margin: {
            bottom: 20,
        },
        argumentAxis: {
            valueMarginsEnabled: false,
            discreteAxisDivisionMode: 'crossLabels',
            grid: {
                visible: true,
            },
            },

            valueAxis: [
                {
                    
                    title: {
                        text: "Utilization in Hours"
        },
                    //visualRange: {
                    //    startValue: 0,
                    //    endValue: 100

                    //},
                    //constantLines: [{
                    //    value: 100,
                    //    color: '#fc3535',
                    //    dashStyle: 'dash',
                    //    width: 2,
                    //    label: { visible: false },
                    //    label: {
                    //        text: year + ' Target',
                    //    },
                    //}],
                }

        ],
            //zooming
                zoomAndPan: {
                    argumentAxis: "both",
                    valueAxis: "both",
                    //dragToZoom: true,
                    allowMouseWheel: true,
        },
                //scrollng
                scrollBar:
                {
                    visible: true

                },
            // assign name field values as the names attributes coming from data object
            seriesTemplate: {
                //valueField: 'RBCode',

                nameField: 'RBCode',
                customizeSeries(valueFromNameField) {
                    for (i = 0; i < chartdata.Length; i++) {
                        debugger;
                        return valueFromNameField === chartdata[i - 1].RBCode ? { type: 'line', label: { visible: true }, color: '#ff3f7a' } : {};
                    }

                },

            },

        // label assigning and setting of angle
        argumentAxis: {
            label:
            {
                overlappingBehavior: "rotate",
                rotationAngle: 45,
                wordWrap: "none",
                    //title: 'Utilization in hours'

            },
        },

        valueAxis: [
            {
                name: "Total Utilization",
                grid: { visible: true },
                title: {
                    text: "Total Utilization in Hours"
                }
            }

        ],
        //positioning the legend
        legend: {
            verticalAlignment: 'bottom',
            horizontalAlignment: 'center',
            itemTextPosition: 'bottom',
        },
        title: {
                text: 'Month wise Location based utilization in hours ',

        },
        export: {
            enabled: true,
        },
        tooltip: {
            enabled: true,
        },
    }).dxChart('instance');

        $.notify('Month wise Location based utilization Chart retrieved succesfully.', {
            globalPosition: "top center",
            className: "success"
        });
    }
    //javascript code for export button
    $("#export").click(function () {
        $.notify('Exporting is in Progress. Please wait!', {
            globalPosition: "top center",
            className: "warn"
        });
        debugger;
        //send the data to Action result using ajax call
        $.ajax({

            type: "POST",
            url: "/CPCWW/ExportDataToExcel_Locationwise/",
            data: { 'excelobject': excelobj },
            success: function (exobj) {
                debugger;
                var bytes = new Uint8Array(exobj.FileContents);
                var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = exobj.FileDownloadName;
                link.click();
                $.notify(' Data is exported to an Excel Sheet.Save/Open to view the file.', {
                    globalPosition: "top center",
                    className: "success"
                });
            },
            error: function () {

                $.notify('Error in Exporting Data to an Excel Sheet. Please try again Later.', {
                    globalPosition: "top center",
                    className: "warn"
                });
}
        });
    });
    // onchange of start date field disable submit button
    const valueChangedHandler1 = function (e) {
        const previousValue = e.previousValue;
        const newValue = e.value;
        document.getElementById("gbt").disabled = false;
        document.getElementById("gbt").style.pointerEvents = 'auto';
    };
    // onchange of end date field disable submit button
    const valueChangedHandler2 = function (e) {
        const previousValue = e.previousValue;
        const newValue = e.value;
        document.getElementById("gbt").disabled = false;
        document.getElementById("gbt").style.pointerEvents = 'auto';
    };

    $("#sdate1").dxDateBox("instance")
        .on("valueChanged", valueChangedHandler1);

    $("#edate1").dxDateBox("instance")
        .on("valueChanged", valueChangedHandler2);

});