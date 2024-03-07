var excelobjtsiu;
var Labname;
var Location;
var LabType;
var LabID;
var SDate;
var EDate;
var startdatetime;
var enddatetime;
var manualhrs;
var automatedhrs;
var man_caplhrs;
var auto_caplhrs;
//var manhrs = 0;
//var autohrs = 0;
//var mancapl = 0;
//var autocapl = 0;
function generateReportChartTSIU(data) {
    debugger;
    document.getElementById("chartdiv2").style.display = "block";
    excelobjtsiu = data;

    LabID = document.getElementById('selectLabId').options[document.getElementById('selectLabId').selectedIndex].value
    am4core.useTheme(am4themes_animated);


    var chart = am4core.create("chartdiv2", am4charts.XYChart);
    chart.hiddenState.properties.opacity = 0; // this creates initial fade-in
    chart.paddingRight = 30;
    chart.dateFormatter.inputDateFormat = "yyyy-MM-dd HH:mm:ss";
    if (data.length == 0) {
        var url = "/LabCar/Lab_Report";
        window.location.href = url;
    }
    var colorSet = new am4core.ColorSet();
    colorSet.saturation = 0.4;
    var cnt = 0;
    var colourToUse;
    var dayChangeFlag = false;
    var count = 0;

    for (i = 0; i < data.length; i++) {
        excelobjtsiu[i].LC_Name = $("#selectLabId option:selected").html();
        debugger;
        var startDate = data[i].startTime;
        var endDate = data[i].endTime;
        Location = data[i].LC_Location;
        LabType = data[i].LC_Lab_Type;
        Labname = excelobjtsiu[i].LC_Name;

        var weekday = new Array(7);
        weekday[0] = "Sun";
        weekday[1] = "Mon";
        weekday[2] = "Tue";
        weekday[3] = "Wed";
        weekday[4] = "Thu";
        weekday[5] = "Fri";
        weekday[6] = "Sat";
        debugger;
        var sDate = startDate.replace('/Date(', '');
        var date_st = sDate.replace(')/', '');
        var stDate = parseInt(date_st);
        var eDate = endDate.replace('/Date(', '');
        var date_ed = eDate.replace(')/', '');
        var edDate = parseInt(date_ed);
        debugger;
        //var StDate = Date.parse(stDate);
        startdatetime = new Date(stDate);
        //startdatetime = data[i].startTime;
        excelobjtsiu[i].s_day = weekday[startdatetime.getDay()].toString();

        //var Time1 = StDate.toLocaleTimeString();
        debugger;
        var Time1 = startdatetime.toTimeString();
        var time = Time1.match(/(\d+):(\d+):(\d+)/);
        var hours = Number(time[1]);
        var minutes = Number(time[2]);
        var seconds = Number(time[3]);
        //var meridian = time[4].toLowerCase();
        //if (meridian == 'p' && hours < 12) {
        //    hours += 12;
        //}
        //else if (meridian == 'a' && hours == 12) {
        //    hours -= 12;
        //}
        excelobjtsiu[i].Startdate = startdatetime.toLocaleDateString();
        //SDate = excelobjtsiu[i].Startdate;

        excelobjtsiu[i].startTime = hours + ":" + minutes + ":" + seconds;
        //var StartDateTime = excelobjtsiu[i].Startdate + " " + excelobjtsiu[i].startTime;
        debugger;
        //var EndDate = Date.parse(edDate);
        enddatetime = new Date(edDate);
        //enddatetime = data[i].endTime;
        excelobjtsiu[i].e_day = weekday[enddatetime.getDay()].toString();

        excelobjtsiu[i].Enddate = enddatetime.toLocaleDateString();
        ////var Time2 = EndDate.toLocaleTimeString();
        var Time2 = enddatetime.toTimeString();
        var time = Time2.match(/(\d+):(\d+):(\d+)/);
        var hours = Number(time[1]);
        var minutes = Number(time[2]);
        var seconds = Number(time[3]);
        //var meridian = time[4].toLowerCase();
        //if (meridian == 'p' && hours < 12) {
        //    hours += 12;
        //}
        //else if (meridian == 'a' && hours == 12) {
        //    hours -= 12;
        //}
        excelobjtsiu[i].endTime = hours + ":" + minutes + ":" + seconds;
        //EDate = excelobjtsiu[i].Enddate;
        //var EndDateTime = excelobjtsiu[i].Enddate + " " + excelobjtsiu[i].endTime;

        var strManual = "Manual";
        var strAuto = "Automated";
        var strMancapl = "Manual_CAPL";
        var strAutocapl = "Automated_CAPL";
        //debugger;      

        if (strAuto.localeCompare(data[i].TypeofUsage) == 0) {
            colourToUse = am4core.color("green").lighten(0.3);
        }
        if (strAutocapl.localeCompare(data[i].TypeofUsage) == 0) {
            colourToUse = am4core.color("orange");
        }
        if (strManual.localeCompare(data[i].TypeofUsage) == 0) {
            colourToUse = am4core.color("red").lighten(0.3);
        }
        if (strMancapl.localeCompare(data[i].TypeofUsage) == 0) {
            colourToUse = am4core.color("blue").lighten(0.3);
        }


        var strEndDay = excelobjtsiu[i].Enddate;
        if (strEndDay.localeCompare(excelobjtsiu[i].Startdate) == 0) {
            dayChangeFlag = false;
        }
        else {
            dayChangeFlag = true;
        }

        if (!dayChangeFlag) {
            chart.data[cnt] = {
                "days": (excelobjtsiu[i].s_day + "," + excelobjtsiu[i].Startdate),
                "start": excelobjtsiu[i].startTime,
                "end": excelobjtsiu[i].endTime,
                "color": colourToUse,
                "task": data[i].TypeofUsage.toString().concat(count.toString())
            };
            cnt = cnt + 1;
            count = count + 1;
        }

        else {
            debugger;
            var daydiff = (Math.ceil(EndDate - StDate)) / (1000 * 3600 * 24);
            var tempstart = startdatetime;
            var daydiffer = (Math.ceil(EndDate - StDate)) / (1000 * 3600 * 24);

            do {
                debugger;
                if (daydiff >-1 && daydiff < 0) {                    

                    chart.data[cnt] = {
                        "days": (excelobjtsiu[i].e_day + "," + excelobjtsiu[i].Enddate),
                        "start": "00:00:01",
                        "end": excelobjtsiu[i].endTime,
                        "color": colourToUse,
                        "task": data[i].TypeofUsage.toString().concat(count.toString())

                    };
                    break;
                }
                else if (daydiff == (Math.ceil(EndDate - StDate)) / (1000 * 3600 * 24)) {
                    debugger;
                    var hms = startdatetime.toTimeString();
                    var time = hms.match(/(\d+):(\d+):(\d+)/);
                    var hours = Number(time[1]);
                    var minutes = Number(time[2]);
                    var seconds = Number(time[3]);
                    var totalSeconds = (+hours) * 60 * 60 + (+minutes) * 60 + (+seconds);
                    var totalmins = (+hours) * 60 + (+minutes) + (+seconds) / 60;
                    debugger;
                    var tempdate = startdatetime.getTime() + 86400000;
                    var new_Startdate = new Date(tempdate);
                    new_Startdate.setSeconds(new_Startdate.getSeconds() + (-(totalSeconds + 1)));



                    var stime = new_Startdate.toTimeString();
                    var time = stime.match(/(\d+):(\d+):(\d+)/);
                    var hours = Number(time[1]);
                    var minutes = Number(time[2]);
                    var seconds = Number(time[3]);
                    var s_Time = hours + ":" + minutes + ":" + seconds;

                    chart.data[cnt] = {
                        "days": (excelobjtsiu[i].s_day + "," + excelobjtsiu[i].Startdate),
                        "start": excelobjtsiu[i].startTime,
                        "end": s_Time,
                        "color": colourToUse,
                        "task": data[i].TypeofUsage.toString().concat(count.toString())
                    };

                    var tempsdate = startdatetime.getTime() + 86400000;
                    var temps = new Date(tempsdate);
                    temps.setMinutes(temps.setMinutes() + (-(totalmins)));
                }
                else {
                    var EndDate = enddatetime;
                    EndDate.setDate(EndDate.getDate() + (-daydiff))

                    chart.data[cnt] = {
                        "days": (excelobjtsiu[i].e_day + "," + EndDate.getDate() + "/" + EndDate.getMonth() + "/" + EndDate.getFullYear()),
                        "start": "00:00:01",
                        "end": "23:59:59",
                        "color": colourToUse,
                        "task": data[i].TypeofUsage.toString().concat(count.toString())
                    };

                }

                cnt = cnt + 1;
                count = count + 1;

                daydiff--;
                //if (daydiff < 0) {
                //    daydiff = 0;
                //}
            }
            while (daydiff >-1);
        }


    }
    debugger;
    var title = chart.titles.create();
    title.text = "Lab Name:" + Labname + " " + "; Location:" + Location + " " + ";  LabID:" + LabID;
    title.fontSize = 20;
    chart.dateFormatter.dateFormat = "HH:mm:ss";
    chart.dateFormatter.inputDateFormat = "HH:mm:ss";
    var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "days";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.inversed = false;
    categoryAxis.renderer.minGridDistance = 5;
    categoryAxis.renderer.maxGridDistance = 30;
    categoryAxis.title.text = "Days";
    var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
    dateAxis.renderer.minGridDistance = 70;
    dateAxis.baseInterval = { count: 1, timeUnit: "second" };
    dateAxis.dateFormats.setKey("second", "HH:mm:ss");
    dateAxis.renderer.tooltipLocation = 0;
    var series1 = chart.series.push(new am4charts.ColumnSeries());
    series1.dataFields.openDateX = "start";
    series1.dataFields.dateX = "end";
    series1.dataFields.categoryY = "days";
    series1.columns.template.width = am4core.percent(100);
    series1.columns.template.tooltipText = "{task}: [bold]{openDateX}[/] - [bold]{dateX}[/]";
    series1.columns.template.propertyFields.fill = "color"; // get color from data
    series1.columns.template.propertyFields.stroke = "color";
    series1.columns.template.strokeOpacity = 1;
    chart.scrollbarX = new am4core.Scrollbar();
    var manhrs = 0;
    var autohrs = 0;
    var mancapl = 0;
    var autocapl = 0;
    debugger;
    for (i = 0; i < data.length; i++) {
        manhrs += data[i].LC_TotalManualHours.TotalHours;
        autohrs += data[i].LC_AutomatedTotalHours.TotalHours;
        mancapl += data[i].LC_TotalManualCAPLHours.TotalHours;
        autocapl += data[i].LC_AutomatedCAPLTotalHours.TotalHours;
    }

    chart.legend = new am4charts.Legend();
    chart.legend.position = "bottom";
    chart.legend.contentAlign = "center";

    chart.legend.data = [
        {
            "name": "Manual Hours - " + Math.round(manhrs * 100) / 100 + "hrs",
            "fill": am4core.color("red").lighten(0.2)
        },
        {
            "name": "Automated Hours - " + Math.round(autohrs * 100) / 100 + "hrs",
            "fill": am4core.color("green")
        },
        {
            "name": "Manual Capl Hours - " + Math.round(mancapl * 100) / 100 + "hrs",
            "fill": am4core.color("blue").lighten(0.2)
        },
        {
            "name": "Automated Capl Hours - " + Math.round(autocapl * 100) / 100 + "hrs",
            "fill": am4core.color("orange")
        }


    ];

    manualhrs = Math.round(manhrs, 1);
    automatedhrs = Math.round(autohrs, 1);
    man_caplhrs = Math.round(mancapl, 1);
    auto_caplhrs = Math.round(autocapl, 1)

    chart.exporting.menu = new am4core.ExportMenu();
    chart.exporting.menu.align = "right";
    chart.exporting.menu.verticalAlign = "top";
    chart.exporting.filePrefix = "TSIU_" + Location + "_" + LabType + "_" + excelobjtsiu[0].Startdate + "_" + excelobjtsiu[excelobjtsiu.length - 1].Enddate;
    var cellSize = 50;
    chart.events.on("datavalidated", function (ev) {
        var chart = ev.target;
        chart.scrollbarY = new am4core.Scrollbar();
        chart.scrollbarY.parent = chart.leftAxesContainer;
    });


    document.getElementById("spin2").style.display = "none";
    document.querySelector('#export2').style.pointerEvents = 'auto';


}
//, 'manual': manhrs, 'auto': autohrs, 'manualcapl': mancapl, 'automatedcapl': autocapl 
$("#export2").click(function () {
    debugger;
    $.notify('Exporting is in Progress. Please wait!', {
        globalPosition: "top center",
        className: "warn"
    });
    $.ajax({
        type: "POST",
        url: "/LabCar/ExportTSIUDataToExcel/",
        data: { 'excel_obj': excelobjtsiu, 'loc': Location, 'type': LabType, 'mantotalhrs': manualhrs, 'autototalhrs': automatedhrs, 'mancapltotalhrs': man_caplhrs, 'autocapltotalhrs': auto_caplhrs },
        success: function (fileobj) {
            debugger;


            var bytes = new Uint8Array(fileobj.FileContents);
            var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileobj.FileDownloadName;
            link.click();
            $.notify(' TSIU Data is exported to an Excel Sheet.Save/Open to view the file.', {
                globalPosition: "top center",
                className: "success"
            });
        },
        error: function () {
            debugger;
            //$("#mdbody").html("Error in Exproting TSOU Data to an Excel Sheet. Please try again Later.")
            //$('#myModal').modal('show');
            $.notify('Error in Exporting TSIU Data to an Excel Sheet. Please try again Later.', {
                globalPosition: "top center",
                className: "warn"
            });
        }

    });
});