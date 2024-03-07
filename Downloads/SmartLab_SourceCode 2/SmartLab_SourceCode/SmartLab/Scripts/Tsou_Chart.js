var excelobjtsou;
var Location;
var Labtype;
var Labname = [];
var date1;
var date2;

function generateChartTSOU(data) {
  
    excelobjtsou = data.data.Labs;
       
    document.getElementById("chartdiv1").style.display = "block";
    var filtered = $('#selectLabId').val();

    var averageManHours = Math.round(data.data.OverallManualHours / excelobjtsou.length).toFixed(2);
    var averageAutoHours = Math.round(data.data.OverallAutomatedHours / excelobjtsou.length).toFixed(2);
    var averageManCaplHours = Math.round(data.data.OverallManualCaplHours / excelobjtsou.length).toFixed(2);
    var averageAutoCaplHours = Math.round(data.data.OverallAutomatedCaplHours / excelobjtsou.length).toFixed(2);
    var averageAllHours = Math.round((data.data.OverallAutomatedHours + data.data.OverallManualHours + data.data.OverallManualCaplHours + data.data.OverallAutomatedCaplHours) / excelobjtsou.length).toFixed(2);
    
    //var Labs = data.data.Labs;

    var startDate = data.data.StartDate;
    var endDate = data.data.EndDate;
    var sDate = startDate.replace('/Date(', '');
    var date_st = sDate.replace(')/', '');
    var stDate = parseInt(date_st);
    var eDate = endDate.replace('/Date(', '');
    var date_ed = eDate.replace(')/', '');
    var edDate = parseInt(date_ed);
    var StDate = new Date(stDate);
    var EndDate = new Date(edDate);
    debugger;
    

    date1 = data.data.StartDate_UI;
    date2 = data.data.EndDate_UI;



    if (document.getElementById('selectLabId').selectedIndex > 0) {
        Location = "";
        Labtype = "";
    }

   
    debugger;
    //var sDate = data.data.StartDate;
    //var eDate = data.data.EndDate;
    //date1 = sDate;
    //date2 = eDate;
    am4core.useTheme(am4themes_animated);
    am4core.options.queue = true;
    am4core.options.onlyShowOnViewport = true;
    var chart = am4core.create("chartdiv1", am4charts.XYChart);
    chart.hiddenState.properties.opacity = 0;

    chart.colors.step = 2;
    chart.padding(30, 30, 10, 30);
    chart.legend = new am4charts.Legend();
    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "name";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.labels.template.verticalCenter = "middle";
    categoryAxis.renderer.labels.template.horizontalCenter = "left";
    categoryAxis.renderer.grid.template.above = true;
    categoryAxis.renderer.grid.template.disabled = true;
    categoryAxis.renderer.labels.template.rotation = 270;
    
    categoryAxis.renderer.labels.template.horizontalCenter = "right";
    categoryAxis.renderer.labels.template.verticalCenter = "middle";
    categoryAxis.renderer.minGridDistance = 5;
   
    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    chart.events.on("ready", function (ev) {
        valueAxis.min = 0;
        valueAxis.max = 200;
    });
    valueAxis.strictMinMax = true;
    valueAxis.calculateTotals = true;
    valueAxis.renderer.minWidth = 30;
    valueAxis.title.text = "Utilization in Hours";
    valueAxis.title.rotation = 270;
    valueAxis.title.align = "center";
    valueAxis.title.valign = "middle";
    valueAxis.title.dy = -40;
    valueAxis.title.fontWeight = 600;



    var tempOemdata = [];
    var labData = [];
    var chartData = [];
    debugger;
    const diffTime = Math.abs(edDate - stDate);
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    const yval = diffDays * 12;
    const hours_100p = diffDays * 12;


    function groupBy(objectArray, property) {
        return objectArray.reduce((groupedlist, eachobjinlist) => {
            const key = eachobjinlist[property];
            if (!groupedlist[key]) {
                groupedlist[key] = [];
            }
            // Add object to list for given key's value
            groupedlist[key].push(eachobjinlist);

            return groupedlist;
        }, {});
    }
    const groupedbyOEM = groupBy(excelobjtsou, 'OEM');
    var cnt = 0;
    const keys = Object.keys(groupedbyOEM);
    for (let i = 0; i < keys.length; i++) {
        const key = keys[i];
        for (j = 0; j < groupedbyOEM[key].length; j++) {
            //debugger;
            //Location += groupedbyOEM[key][j].Location + '|';
            //Labtype += groupedbyOEM[key][j].Model + '|';
            //debugger;
            //excelobjtsou[j].Inventory = groupedbyOEM[key][j].Inventory;
            Labtype += groupedbyOEM[key][j].Model + '|';
            Location += groupedbyOEM[key][j].Location + '|';          
            //debugger;
            //tempOemdata[j] = key;
            labData[cnt] =
            {
               
                labOEM: groupedbyOEM[key][j].OEM,
                name: groupedbyOEM[key][j].TSOULabel,
                manual: Math.round((groupedbyOEM[key][j].ManualTotalHours) * 100) / 100,
                automated: Math.round((groupedbyOEM[key][j].AutomatedTotalHours) * 100) / 100,
                manual_capl: Math.round((groupedbyOEM[key][j].ManualCaplTotalHours) * 100) / 100,
                automated_capl: Math.round((groupedbyOEM[key][j].AutomatedCaplTotalHours) * 100) / 100,
                manualTime: Math.round(groupedbyOEM[key][j].ManualTotalHours),
                automatedTime: Math.round(groupedbyOEM[key][j].AutomatedTotalHours),


                reader: {
                    manual: Math.round((groupedbyOEM[key][j].ManualTotalHours)*100)/100,
                    automated: Math.round((groupedbyOEM[key][j].AutomatedTotalHours) * 100) / 100,
                    manual_capl: Math.round((groupedbyOEM[key][j].ManualCaplTotalHours) * 100) / 100,
                    automated_capl: Math.round((groupedbyOEM[key][j].AutomatedCaplTotalHours) * 100) / 100,
                    total: Math.round((groupedbyOEM[key][j].TotalSum) * 100) / 100
                }
            };
            cnt++;
        }
    }

    debugger;
    Location = Array.from(new Set(Location.split('|'))).toString();
    Labtype = Array.from(new Set(Labtype.split('|'))).toString();
    Location = Location.slice(0, -1);
    Labtype = Labtype.slice(0, -1);

    //for (i = 0; i <excelobjtsou.length; i++) {
    //    excelobjtsou[i].Labtype = Labtype;
    //    excelobjtsou[i].Location = Location;
    //}


    //debugger;
    var uniqueOEM = [];
    uniqueOEM = keys;

    //// process data ant prepare it for the chart
    uniqueOEM.forEach(function (l_oem) {
        //var providerData = data[providerName];
        //debugger;
        // add data of one provider to temp array
        var tempArray = [];
        var count = 0;
        // add items
        labData.forEach(function (labItem) {
            //debugger;
            if (labItem.labOEM == l_oem) {
                count++;
                // we generate unique category for each column (providerName + "_" + itemName) and store realName
                //debugger;
                tempArray.push({
                    name: labItem.name, manual: labItem.manual, manual_capl: labItem.manual_capl,
                    automated: labItem.automated,
                    automated_capl: labItem.automated_capl,
                    reader: labItem.reader, oem: l_oem, manualTime: labItem.manualTime, automatedTime: labItem.automatedTime
                })
            }
        })
        // push to the final data
        //debugger;
        am4core.array.each(tempArray, function (item) {
            chartData.push(item);
        })

        // create range (the additional label at the bottom)
        //debugger;
        var rangecat = categoryAxis.axisRanges.create();
        rangecat.category = tempArray[0].name;
        rangecat.endCategory = tempArray[tempArray.length - 1].name;
        rangecat.label.text = tempArray[0].oem +" ";
        rangecat.label.dy = 0;
        rangecat.label.horizontalCenter = "middle";
        rangecat.label.rotation = 0;
        rangecat.label.paddingTop = 180;
        rangecat.label.truncate = true;
        rangecat.label.fontWeight = "bold";
        rangecat.label.fontSize = 11;
        rangecat.grid.disabled = true;
        rangecat.tick.disabled = false;
        rangecat.tick.length = 150;
        rangecat.tick.strokeOpacity = 0.6;
        rangecat.tick.location = 0;
        //range.label.adapter.add("maxWidth", function (maxWidth, target) {
        //    var range = target.dataItem;
        //    var startPosition = categoryAxis.categoryToPosition(range.category, 0);
        //    var endPosition = categoryAxis.categoryToPosition(range.endCategory, 1);
        //    var startX = categoryAxis.positionToCoordinate(startPosition);
        //    var endX = categoryAxis.positionToCoordinate(endPosition);
        //    return endX - startX;
        //})
    })
    chart.data = chartData;

    // last tick
    var rangelast = categoryAxis.axisRanges.create();
    rangelast.category = chart.data[chart.data.length - 1].name;
    rangelast.label.disabled = true;
    rangelast.grid.location = 1;
    rangelast.tick.disabled = false;
    rangelast.tick.length = 150;
    rangelast.tick.location = 1;
    rangelast.tick.strokeOpacity = 0.6;
    ///END OF GROUPING CODE




    //debugger;
    //Code to add the range mark
    //var range = valueAxis.axisRanges.create();
    //range.value = 100; //100%
    //range.grid.stroke = am4core.color("#33cc33"); //green
    //range.grid.strokeWidth = 2;
    //range.grid.strokeOpacity = 1;
    //range.grid.above = true; //to display the grid line on top of the data.
    //range.label.inside = true;
    //range.label.text = "\xa0\xa0\xa0\xa0\xa0\xa0>100%";
    //range.label.fontWeight = 750;
    //range.label.fill = range.grid.stroke;
    //range.label.verticalCenter = "bottom";
    //var rangeRed = valueAxis.axisRanges.create();
    //rangeRed.value = 67; //67%
    //rangeRed.grid.stroke = am4core.color("#ff0000"); //red
    //rangeRed.grid.strokeWidth = 2;
    //rangeRed.grid.strokeOpacity = 1;
    //rangeRed.grid.above = true; //to display the grid line on top of the data.
    //rangeRed.label.inside = true;
    //rangeRed.label.text = "\xa0\xa0\xa0\xa0\xa0\xa0>67%";
    //rangeRed.label.fontWeight = 750;
    //rangeRed.label.fill = rangeRed.grid.stroke;
    //rangeRed.label.verticalCenter = "bottom";

    ////Code to shade the marked ranges
    ////background fill green for >100% utilization
    //var greenBG = valueAxis.axisRanges.create();
    //greenBG.value = 100
    //greenBG.endValue = 200;
    //greenBG.axisFill.fill = am4core.color("#99ff99");
    //greenBG.axisFill.fillOpacity = 0.2;
    //greenBG.grid.strokeOpacity = 0;

    ////background fill yellow for 100 > x > 50 utilization
    //var yellowBG = valueAxis.axisRanges.create();
    //yellowBG.value = 67;
    //yellowBG.endValue = 100;
    //yellowBG.axisFill.fill = am4core.color("#ffff99");
    //yellowBG.axisFill.fillOpacity = 0.2;
    //yellowBG.grid.strokeOpacity = 0;

    ////background fill red for <50% utilization
    //var redBG = valueAxis.axisRanges.create();
    //redBG.value = 0;
    //redBG.endValue = 67;
    //redBG.axisFill.fill = am4core.color("#ff6666");
    //redBG.axisFill.fillOpacity = 0.2;
    //redBG.grid.strokeOpacity = 0;



    var title = chart.titles.create();
    title.text = "Location:" + " " + Location + " " + "; LabType:" + " " + Labtype + " " + ";StartDate:" + " " + date1 + " " + " - " + " " + "EndDate: " + date2;
    title.fontSize = 20;

    debugger;
    var series1 = chart.series.push(new am4charts.ColumnSeries());
    series1.columns.template.width = am4core.percent(30);
    series1.columns.template.propertyFields.dummyData = "reader";
    series1.columns.template.tooltipText = "Manual Usage: {dummyData.manual} hrs \x0a Total Usage: {dummyData.total} hrs";
    //" Manual Hours: {valueY.totalPercent.formatNumber('#.00')}%}";
    series1.name = "Manual Usage  :" + Math.round(data.data.OverallManualHours)  + "hrs";
    series1.dataFields.categoryX = "name";
    series1.dataFields.valueY = "manual";
    series1.dataItems.template.locations.categoryX = 0.5;
    series1.stacked = true;
    series1.tooltip.pointerOrientation = "vertical";
    series1.tooltip.exportable = true;
    var bullet1 = series1.bullets.push(new am4charts.LabelBullet());
    bullet1.interactionsEnabled = false;
    bullet1.label.text = "Manual Usage: {valueY} hrs";
    bullet1.label.fill = am4core.color("#ffffff");
    bullet1.locationY = 0.5;

    var series2 = chart.series.push(new am4charts.ColumnSeries());
    series2.columns.template.width = am4core.percent(30);
    series2.columns.template.propertyFields.dummyData = "reader";
    series2.columns.template.tooltipText =
        "Automated Usage: {dummyData.automated} hrs \x0a Total Usage: {dummyData.total} hrs";
    series2.name = "Automated Usage :" + Math.round(data.data.OverallAutomatedHours)  + "hrs";
    series2.dataFields.categoryX = "name";
    series2.dataFields.valueY = "automated";
    series2.dataItems.template.locations.categoryX = 0.5;
    series2.stacked = true;
    series2.tooltip.pointerOrientation = "vertical";
    series2.tooltip.exportable = true;
    var bullet2 = series2.bullets.push(new am4charts.LabelBullet());
    bullet2.interactionsEnabled = false;
    bullet2.label.text = "Automated Usage: {valueY} hrs";
    bullet2.locationY = 0.5;
    bullet2.label.fill = am4core.color("#ffffff");


    var series3 = chart.series.push(new am4charts.ColumnSeries());
    series3.columns.template.width = am4core.percent(30);
    series3.columns.template.propertyFields.dummyData = "reader";
    series3.columns.template.tooltipText =
        "Automated-CAPL Usage: {dummyData.automated_capl} hrs \x0a Total Usage: {dummyData.total} hrs";
    series3.name = "Automated Capl Usage :" + Math.round(data.data.OverallAutomatedCaplHours) + "hrs";
    series3.dataFields.categoryX = "name";
    series3.dataFields.valueY = "automated_capl";
    series3.dataItems.template.locations.categoryX = 0.5;
    series3.stacked = true;
    series3.tooltip.pointerOrientation = "vertical";
    series3.tooltip.exportable = true;
    var bullet3 = series3.bullets.push(new am4charts.LabelBullet());
    bullet3.interactionsEnabled = false;
    bullet3.label.text = "Automated-CAPL Usage: {valueY} hrs";
    bullet3.locationY = 0.5;
    bullet3.label.fill = am4core.color("#ffffff");


    var series4 = chart.series.push(new am4charts.ColumnSeries());
    series4.columns.template.width = am4core.percent(30);
    series4.columns.template.propertyFields.dummyData = "reader";
    series4.columns.template.tooltipText =
        "Manual-CAPL Usage: {dummyData.manual_capl} hrs \x0a Total Usage: {dummyData.total} hrs";
    series4.name = "Manual Capl Usage  :" + Math.round(data.data.OverallManualCaplHours) + "hrs";
    series4.dataFields.categoryX = "name";
    series4.dataFields.valueY = "manual_capl";
    series4.dataItems.template.locations.categoryX = 0.5;
    series4.stacked = true;
    series4.tooltip.pointerOrientation = "vertical";
    series4.tooltip.exportable = true;
    var bullet4 = series4.bullets.push(new am4charts.LabelBullet());
    bullet4.interactionsEnabled = false;
    bullet4.label.text = "Manual-CAPL Usage: {valueY} hrs";
    bullet4.locationY = 0.5;
    bullet4.label.fill = am4core.color("#ffffff");





    chart.scrollbarX = new am4core.Scrollbar();
    chart.scrollbarX.parent = chart.topAxesContainer;
    chart.scrollbarY = new am4core.Scrollbar();
    chart.scrollbarY.parent = chart.leftAxesContainer;
    chart.chartContainer.wheelable = false;
    var AvgInformation = chart.createChild(am4core.Label);
    AvgInformation.text = "Total No of Systems:" + excelobjtsou.length + "| Average Manual Hours : " + averageManHours + " | Average Automated Hours : " + averageAutoHours + "| Average ManualCapl Hours : " + averageManCaplHours + "| Average AutomatedCapl Hours : " + averageAutoCaplHours + " | Average Overall Hours: " + averageAllHours;
    AvgInformation.fontSize = 14;
    AvgInformation.align = "center";
    chart.exporting.menu = new am4core.ExportMenu();
    chart.exporting.filePrefix = "TSOU_" + Location + "_" + Labtype + "_" + date1 + "-" + date2;

    debugger;
    document.getElementById("spin1").style.display = "none";
    document.querySelector('#export').style.pointerEvents = 'auto';
   
   
}

$("#export").click(function () {
    $.notify('Exporting is in Progress. Please wait!', {
        globalPosition: "top center",
        className: "warn"
    });
    $.ajax({
        
        type: "POST",
        url: "/Dashboard/ExportTSOUDataToExcel/",
        data: { 'excelobjtsou': excelobjtsou, 'sdate': date1, 'edate': date2, 'loc': Location, 'type': Labtype },
        success: function (exobj) {
           
            var bytes = new Uint8Array(exobj.FileContents);
            var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = exobj.FileDownloadName;
            link.click();
            $.notify(' TSOU Data is exported to an Excel Sheet.Save/Open to view the file.', {
                globalPosition: "top center",
                className: "success"
            });
        },
        error: function () {
            $.notify(' Error in Exporting TSOU Data to an Excel Sheet. Please try again Later.', {
                globalPosition: "top center",
                className: "warn"
            });
        }
    });
});