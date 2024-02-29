$(document).ready(function () {

    const $tabLink = $('#tabs-section .tab-link');
    const $tabBody = $('#tabs-section .tab-body');
    let timerOpacity;
    debugger;
    var excelobj;
    // initialise dates
    var edate5;
    var sdate5;
    // assign default date values with 365 days difference
    now = new Date();
    edate5 = now.setDate(now.getDate() - 1);
    now = new Date();
    sdate5 = now.setDate(now.getDate() - 365);
    //code to display the date picker and date box
    $("#sdate5").dxDateBox({
        type: "date",
        value: sdate5
    });
    //code to display the date picker and date box
    $("#edate5").dxDateBox({
        type: "date",
        value: edate5
    });
    //assign obj from Controller to javascript obj
    var data = obj.MonthlyIndexValueChartdata;
    debugger;
    //data assigning to another object used for excel export
    excelobj = data;
    //on click of Generate button
    document.getElementById("gbt5").onclick = function () {
        debugger;
        // fucntion call
        Chartajax();
    }

    document.getElementById("gbt5").disabled = true;
    document.getElementById("gbt5").style.pointerEvents = 'none';
    var sdate1;
    var edate1;
    var datestrt;
    var dateed;
    sdate1 = data.StDate;
    edate1 = data.EdDate;
    debugger;
   

    debugger;
    generateChart5(data);

    document.getElementById('dates').style.display = "block";


    //validate dates
    function validateDate() {
        var start = $("#sdate5").dxDateBox("option", "value");
        var end = $("#edate5").dxDateBox("option", "value");
        /* change short date string to timestamp*/
        var dstart = new Date(start);
        var dend = new Date(end);
        // regular expression to match required date format
        re = /^\d{4}\/\d{2}\/\d{2}$/;
        //re = /^\d{4}\/\d{1,2}-\d{1,2}$/;
        debugger;
        if (document.getElementById('sdate5').value == '' || document.getElementById('edate5').value == '') {
            alert("Dates not selected ");
            document.getElementById('sdate5').focus();
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


    // function to fetch the data required for the chart
    function Chartajax() {
        validatepage();
        // get start and end dates values
        var strt = $("#sdate5").dxDateBox("option", "value");
        var ed = $("#edate5").dxDateBox("option", "value");

        /* change short date string to timestamp*/
        var dstrt = new Date(strt);
        var ded = new Date(ed);
        var ltype = "";
        var loc = "";
        datestrt = Date.parse(dstrt);
        dateed = Date.parse(ded);
        debugger;
        // send the parameters from javascipt to Controller
        $.ajax({
            type: "POST",
            url: "../CPCWW/CPCCharts1/",
            data: JSON.stringify({ startdate: datestrt, enddate: dateed, type: ltype, location: loc }),

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                debugger;
                var data = data.data.MonthlyIndexValueChartdata;
                excelobj = data;
                // pass data to function for chart disply
                generateChart5(data);



            },
            error: function (err) {
                $.notify('Unable to fetch Chart Data. Please Try Again.', {
                    globalPosition: "top center",
                    className: "success"
                });
            }

        });
    }

    function generateChart5(data) {
        debugger;
        // assign data to javascript variable
        var chartdata = data.WWCPC_Charts_IndexVal;

        // chart code
        var chart = $('#cpcchart5').dxChart({
            palette: 'Violet',
            // assign data to datasource
            dataSource: chartdata,
            size: {
                height: 400,
                width: 1100,
            },
            // legend show and hidew
            onLegendClick: function (e) {
                var series = e.target;
                if (series.isVisible()) {
                    series.hide();
                } else {
                    series.show();
                }
            },


            resolveLabelOverlapping: 'stack',
            // assign argument and value fields with data attribute names from data source
            commonSeriesSettings: {
                argumentField: 'Monthyear',
                valueField: 'IndexValue',
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
            //zooming and panning
            zoomAndPan: {
                argumentAxis: "both",
                valueAxis: "both",
                //dragToZoom: true,
                allowMouseWheel: true,
            },
            //scrolling
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
                text: 'Month wise Location based Index Value Chart ',

            },
            export: {
                enabled: true,
            },
            tooltip: {
                enabled: true,
            },
        }).dxChart('instance');

        $.notify(' Month wise Location based Index Value Chart retrieved succesfully.', {
            globalPosition: "top center",
            className: "success"
        });
    }
    //excel exporting
    $("#export5").click(function () {
        $.notify('Exporting is in Progress. Please wait!', {
            globalPosition: "top center",
            className: "warn"
        });
        debugger;
        $.ajax({
            //send the object to the controller 
            type: "POST",
            url: "/CPCWW/ExportDataToExcel_IndexvalLocationwise/",
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
        document.getElementById("gbt5").disabled = false;
        document.getElementById("gbt5").style.pointerEvents = 'auto';
    };
  
    const valueChangedHandler2 = function (e) {
        const previousValue = e.previousValue;
        const newValue = e.value;
        document.getElementById("gbt5").disabled = false;
        document.getElementById("gbt5").style.pointerEvents = 'auto';
    };
    // on change of Start date, disable button
    $("#sdate5").dxDateBox("instance")
        .on("valueChanged", valueChangedHandler1);
    // on change of End date, disable button
    $("#edate5").dxDateBox("instance")
        .on("valueChanged", valueChangedHandler2);

});