$(document).ready(function () {

    const $tabLink = $('#tabs-section .tab-link');
    const $tabBody = $('#tabs-section .tab-body');
    let timerOpacity;
    debugger;
    var excelobj;
    //var edate2;
    //var sdate2;

    //now = new Date();
    //edate2 = now.setDate(now.getDate() - 1);
    //now = new Date();
    //sdate2 = now.setDate(now.getDate() - 365);

    //$("#sdate2").dxDateBox({
    //    type: "date",
    //    value: sdate2
    //});

    //$("#edate2").dxDateBox({
    //    type: "date",
    //    value: edate2
    //});

    var data = obj.MonthlyAvgTotalUtilizationdata;
    debugger;
    excelobj = data;
    document.getElementById("gbt4").onclick = function () {
        debugger;
        Chartajax();
    }
    document.getElementById("gbt4").disabled = true;
    document.getElementById("gbt4").style.pointerEvents = 'none';
    var sdate4;
    var edate4;
    var datestrt;
    var dateed;
    sdate4 = data.StDate;
    edate4 = data.EdDate;
    debugger;
    $("#sdate4").dxDateBox({
        type: "date",
        value: sdate4
    });

    $("#edate4").dxDateBox({
        type: "date",
        value: edate4
    });

    debugger;
    generateChart4(data);

    document.getElementById('dates').style.display = "block";



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
        $.ajax({
            type: "POST",
            url: "../CPCWW/CPCCharts1/",
            data: JSON.stringify({ startdate: datestrt, enddate: dateed, type: ltype, location: loc }),

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                debugger;
                var data = data.data.MonthlyAvgTotalUtilizationdata;
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

    function generateChart4(data) {
        debugger;
        var chartdata = data.WWCPC_Charts;


        var chart = $('#cpcchart4').dxChart({
            palette: 'Violet',
            dataSource: chartdata,
            size: {
                height: 400,
                width: 1100,
            },

            onLegendClick: function (e) {
                var series = e.target;
                if (series.isVisible()) {
                    series.hide();
                } else {
                    series.show();
                }
            },


            resolveLabelOverlapping: 'stack',

            commonSeriesSettings: {
                argumentField: 'Monthyear',
                valueField: 'AverageTotalUtilization',
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
                        text: "Average Utilization in Hours"
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

            zoomAndPan: {
                argumentAxis: "both",
                valueAxis: "both",
                //dragToZoom: true,
                allowMouseWheel: true,
            },
            scrollBar:
            {
                visible: true

            },

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


            argumentAxis: {
                label:
                {
                    overlappingBehavior: "rotate",
                    rotationAngle: 45,
                    wordWrap: "none",
                    //title: 'Utilization in hours'

                },
            },


            legend: {
                verticalAlignment: 'bottom',
                horizontalAlignment: 'center',
                itemTextPosition: 'bottom',
            },
            title: {
                text: 'Month wise Location based Average of Total Utilization in hours ',

            },
            export: {
                enabled: true,
            },
            tooltip: {
                enabled: true,
            },
        }).dxChart('instance');

        $.notify(' Chart retrieved succesfully.', {
            globalPosition: "top center",
            className: "success"
        });
    }

    $("#export4").click(function () {
        $.notify('Exporting is in Progress. Please wait!', {
            globalPosition: "top center",
            className: "warn"
        });
        debugger;
        $.ajax({

            type: "POST",
            url: "/CPCWW/ExportDataToExcel_AvgTotalLocationwise/",
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

    const valueChangedHandler1 = function (e) {
        const previousValue = e.previousValue;
        const newValue = e.value;
        document.getElementById("gbt").disabled = false;
        document.getElementById("gbt").style.pointerEvents = 'auto';
    };

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