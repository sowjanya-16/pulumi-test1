$(document).ready(function () {
    debugger;
    
    

    loadLabTypes();
    
    var typeobj;
    var locobj;

    const $tabLink = $('#tabs-section .tab-link');
    const $tabBody = $('#tabs-section .tab-body');
    let timerOpacity;
    debugger;
    var excelobj=[];
    /* retrieve Model data from Action result */

    // assign data coming from controller into a javascript object
    var data = obj.MonthlyChartLocwisedata;
    debugger;
    excelobj = data;
    // on click of submit button redirect to a function
    document.getElementById("gbt1").onclick = function () {
        debugger;
        Chartajax1();
    }
    document.getElementById("gbt1").disabled = true;
    document.getElementById("gbt1").style.pointerEvents = 'none';
    var sdate2;
    var edate2;
    var datestrt;
    var dateed;
    sdate2 = data.StDate;
    edate2 = data.EdDate;
    debugger;
    $("#sdate2").dxDateBox({
        type: "date",
        value: sdate2
    });

    $("#edate2").dxDateBox({
        type: "date",
        value: edate2
    });

    debugger;
    //generateChart2(data);

    document.getElementById('dates1').style.display = "block";


    // validation of dates 
    function validateDate() {
        var start = $("#sdate2").dxDateBox("option", "value");
        var end = $("#edate2").dxDateBox("option", "value");
        /* change short date string to timestamp*/
        var dstart = new Date(start);
        var dend = new Date(end);
        // regular expression to match required date format
        re = /^\d{4}\/\d{2}\/\d{2}$/;
        //re = /^\d{4}\/\d{1,2}-\d{1,2}$/;
        debugger;
        if (document.getElementById('sdate2').value == '' || document.getElementById('edate2').value == '') {
            alert("Dates not selected ");
            document.getElementById('sdate2').focus();
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
        $('.selectpicker').selectpicker();
       
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
    function Chartajax1() {
        validatepage();
        debugger;
        var strt = $("#sdate2").dxDateBox("option", "value");
        var ed = $("#edate2").dxDateBox("option", "value");



        debugger;
        //var typeobj = $('#selectLabType').val(); 
        //display lab types as dropdown values
        var e = document.getElementById("selectLabType");
         typeobj = e.options[e.selectedIndex].value;

        //var locobj = $('#selectLocation').val();        
        //display lab locations as dropdown values
        var e = document.getElementById("selectLocation");
         locobj = e.options[e.selectedIndex].value;

        /* change short date string to timestamp*/
        var dstrt = new Date(strt);
        var ded = new Date(ed);
        datestrt = Date.parse(dstrt);
        dateed = Date.parse(ded);
        debugger;
        //send parameters and receive data from controller
        $.ajax({
            type: "POST",
            url: "../CPCWW/CPCCharts1/",
            data: JSON.stringify({ startdate: datestrt, enddate: dateed, type: typeobj, location: locobj }),

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                debugger;
                var data = data.data.MonthlyChartModelwisedata;
                excelobj = data;
                generateChart2(data);



            },
            error: function (err) {
                $.notify('Unable to fetch Chart Data. Please Try Again.', {
                    globalPosition: "top center",
                    className: "success"
                });
            }

        });
    }

    function generateChart2(data) {
        debugger;
        var chartdata = data.WWCPC_Charts;


        var chart = $('#cpcchart1').dxChart({
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
            // assign argument field value as the names attributes coming from data object
            commonSeriesSettings: {
                argumentField: 'Monthyear',
                //valueField: 'TotalUtilization',
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
               // title: 'Utilization in hours'
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
                //scrolling
                scrollBar:
                {
                    visible: true

                },

            // assign value field and name field values as the names attributes coming from data object
            series: [
                {
                    //axis: "Total",
                    valueField: "ManualUsage",
                    //tagField: "Location",
                    name: "Manual Usage",
                    type: "line",
                    // color: "green",                

                  

                }, {
                    //axis: "Total",
                    //tagField: "Location",
                    valueField: "AutomatedUsage",
                    name: "Automated Usage",
                    type: "line",
                   

                },
                {
                    //axis: "Total",
                    valueField: "ManualCAPLUsage",
                    //tagField: "Location",
                    name: "Manual CAPL Usage",
                    type: "line",
                    //color: "green",


                }, {
                    //axis: "Total",
                    //tagField: "Location",
                    valueField: "AutomatedCAPLUsage",
                    name: "Automated CAPL Usage",
                    type: "line",
                    

                },
                {
                    //axis: "Total",
                    //tagField: "Location",
                    valueField: "TotalUtilization",
                    name: "Total Utilization",
                    type: "line",


                }
            ],


            argumentAxis: {
                label:
                {
                    overlappingBehavior: "rotate",
                    rotationAngle: 45,
                    wordWrap: "none",

                },
            },

            //positioning the legend
            legend: {
                verticalAlignment: 'bottom',
                horizontalAlignment: 'center',
                itemTextPosition: 'bottom',
            },
            title: {
                text: 'Month wise ' + ' ' + data.Type + ' ' + '&' + ' ' + data.RbCode + ' ' + 'based utilization in hours ',

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
    //javascript code for export button
    $("#export1").click(function () {
        $.notify('Exporting is in Progress. Please wait!', {
            globalPosition: "top center",
            className: "warn"
        });
        debugger;
        //send the data to Action result using ajax call by passing the parameters
        $.ajax({

            type: "POST",
            url: "/CPCWW/ExportDataToExcel_ModelLocationwise/",
            data: { 'excelobject': excelobj, type: typeobj, location: locobj },
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
        document.getElementById("gbt1").disabled = false;
        document.getElementById("gbt1").style.pointerEvents = 'auto';
    };
    // onchange of end date field disable submit button
    const valueChangedHandler2 = function (e) {
        const previousValue = e.previousValue;
        const newValue = e.value;
        document.getElementById("gbt1").disabled = false;
        document.getElementById("gbt1").style.pointerEvents = 'auto';
    };

    $("#sdate2").dxDateBox("instance")
        .on("valueChanged", valueChangedHandler1);

    $("#edate2").dxDateBox("instance")
        .on("valueChanged", valueChangedHandler2);

});