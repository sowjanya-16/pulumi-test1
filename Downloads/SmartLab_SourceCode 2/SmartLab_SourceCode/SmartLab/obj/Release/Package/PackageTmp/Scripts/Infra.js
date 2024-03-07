var Locations_sel, Labtype_sel;
//function infra_datagrid(data) {
//    debugger;

//    var cchil_loc = Array.from(Array(2), () => new Array(4));
//    var ccsil_loc = Array.from(Array(2), () => new Array(4));
//    var otb_loc = Array.from(Array(2), () => new Array(4));
//    var cs1000_loc = Array.from(Array(2), () => new Array(4));
//    var Locations_list = $("#ddlLocation").val();
//    for (i = 0; i < data.length; i++) {
//        if (data[i].Utilization == "CCHIL") {
//            debugger;
//            for (j = 0; j < Locations_list.length; j++) {
//                if (data[i].RBCode == Locations_list[j]) {
//                    cchil_loc[j][0] = data[i].AutoPer;
//                    cchil_loc[j][1] = data[i].ManualPer;
//                }

//            }
            
//            //if (data[i].RBCode == Locations_list[0]){
//            //    cchil_loc[0][0] = data[i].AutoPer;
//            //    cchil_loc[0][1] = data[i].ManualPer;
//            //}
//        }
//        else if (data[i].Utilization == "CCSIL") {
//            for (j = 0; j < Locations_list.length; j++) {
//                if (data[i].RBCode == Locations_list[j]) {
//                    ccsil_loc[j][0] = data[i].AutoPer;
//                    ccsil_loc[j][1] = data[i].ManualPer;
//                }
//            }
//        }
//        else if (data[i].Utilization == "OTB") {
//            for (j = 0; j < Locations_list.length; j++) {
//                if (data[i].RBCode == Locations_list[j]) {
//                    otb_loc[j][0] = data[i].AutoPer;
//                    otb_loc[j][1] = data[i].ManualPer;
//                }
//            }
//        }
        
//        else if (data[i].Utilization == "CS1000") {
//            for (j = 0; j < Locations_list.length; j++) {
//                if (data[i].RBCode == Locations_list[j]) {
//                    cs1000_loc[j][0] = data[i].AutoPer;
//                    cs1000_loc[j][1] = data[i].ManualPer;
//                }
//                debugger;
//            }
//        }
//        debugger;
//    }
//    debugger;
//    var tooltipInstance = $("#tooltipContainer").dxTooltip({
//        //position: "right"
//    }).dxTooltip("instance");

//    var pivotGrid = $("#infra_datagrid").dxPivotGrid({
//        //allowSortingBySummary: true,
//        allowSorting: false,
//        allowFiltering: true,
//        showBorders: true,
//        showColumnGrandTotals: true,
//        showRowGrandTotals: false,
//        showRowTotals: false,
//        showColumnTotals: false,
//        //height: 500,
//        //width: 1650,
//        height: 170,
//        width: 400,
//        //columnWidth: 60,
//        headerFilter: { visible: true },
//        onCellPrepared: function (e) {

//            e.cellElement.css('color', '#0000');
//        },
//        fieldPanel: {
           
//            showRowFields: true,
//            visible: false
//        },
//        fieldChooser: {
//            enabled: false,
//            //applyChangesMode: "instantly"
//        },
//        export: {
//            enabled: false
//        },
//        scrolling: {
//            //mode: 'virtual',
//            columnRenderingMode: "virtual"
//        },
//        //paging: {
//        //    enabled: true,
//        //    pageSize: 40
//        //},
//        dataSource: {
//            fields: [{
//                //    caption: "Setup Type",
//                //    width: 60,
//                //    dataField: "SetupType",
//                //    area: "row",
//                //    expanded: true
//                //}, {
//                width: 60,
//                dataField: "Utilization",
//                area: "row",
//                expanded: true
//            }, {
//                dataField: "RBCode",
//                area: "column",
//                width: 20,
//                sortBy: "none",
//                //dataType: "date",
//                //customizeText: function (options) {
//                //    return !options.value ? "" : moment(options.value).format("MMM/DD/YYYY").replace(".", "");
//                //},
//                expanded: true

//            }, {
//                groupname: "RBCode",
//                visible: false,
//                expanded: true
//            }, {

//                dataField: "TotalPer",
//                dataType: "number",
//                    summaryType: "sum",
//                    format: { type: 'fixedPoint', precision: 2 } ,
//                area: "data",
//                visible: true,
//                expanded: true,
//                    width: 20
                  
//            }],
//            store: data
            
//        },
//        onCellPrepared: function (e) {
//            debugger;
            
           
//            if (e.area === "data" && e.cell.columnType != "GT") {  //Tooltip - specific CostElement for Comparison
//                debugger;
               
                    


//                if (e.cell.rowPath[0] == "CCHIL") {
//                    //for (j = 0; j < Locations_list.length; j++) {
//                        if (e.cell.columnPath[0] == Locations_list[0]) {
//                            e.cellElement.mouseover(function (arg) {

//                                tooltipInstance.option("contentTemplate", function (contentElement) {
//                                    contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + cchil_loc[0][0] + ` , Manual: ` + cchil_loc[0][1] + `   , DownTime</b></div>`);//[0]->loc1 [1]loc2;  [0]->uto[1]->man
//                                });

//                                tooltipInstance.show(arg.target);
//                            });
//                    }
//                    if (e.cell.columnPath[0] == Locations_list[1]) {
//                        e.cellElement.mouseover(function (arg) {

//                            tooltipInstance.option("contentTemplate", function (contentElement) {
//                                contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + cchil_loc[1][0] + ` , Manual: ` + cchil_loc[1][1] + `   , DownTime</b></div>`);//[0]->loc1 [1]loc2;  [0]->uto[1]->man
//                            });

//                            tooltipInstance.show(arg.target);
//                        });
//                    }
//                    //}
//                }
//                else if (e.cell.rowPath[0] === "CCSIL") {
//                    //for (j = 0; j < Locations_list.length; j++) {
//                        if (e.cell.columnPath[0] == Locations_list[0]) {

//                            e.cellElement.mouseover(function (arg) {

//                                tooltipInstance.option("contentTemplate", function (contentElement) {
//                                    contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + ccsil_loc[0][0] + ` , Manual: ` + ccsil_loc[0][1] + `   , DownTime</b></div>`);
//                                });

//                                tooltipInstance.show(arg.target);
//                            });
//                    }
//                    if (e.cell.columnPath[0] == Locations_list[1]) {

//                        e.cellElement.mouseover(function (arg) {

//                            tooltipInstance.option("contentTemplate", function (contentElement) {
//                                contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + ccsil_loc[1][0] + ` , Manual: ` + ccsil_loc[1][1] + `   , DownTime</b></div>`);
//                            });

//                            tooltipInstance.show(arg.target);
//                        });
//                    }

//                    //}
//                }
//                else if (e.cell.rowPath[0] === "OTB") {
//                    //for (j = 0; j < Locations_list.length; j++) {
//                        if (e.cell.columnPath[0] == Locations_list[0]) {
//                            e.cellElement.mouseover(function (arg) {

//                                tooltipInstance.option("contentTemplate", function (contentElement) {
//                                    contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + otb_loc[0][0] + ` , Manual: ` + otb_loc[0][1] + `   , DownTime</b></div>`);
//                                });

//                                tooltipInstance.show(arg.target);
//                            });
//                    }
//                    if (e.cell.columnPath[0] === Locations_list[1]) {
//                        e.cellElement.mouseover(function (arg) {

//                            tooltipInstance.option("contentTemplate", function (contentElement) {
//                                contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + otb_loc[1][0] + ` , Manual: ` + otb_loc[1][1] + `   , DownTime</b></div>`);
//                            });

//                            tooltipInstance.show(arg.target);
//                        });
//                    }
//                    //}

//                }
//                else if (e.cell.rowPath[0] === "CS1000") {
//                    //for (j = 0; j < Locations_list.length; j++) {
//                        if (e.cell.columnPath[0] == Locations_list[0]) {
//                            e.cellElement.mouseover(function (arg) {

//                                tooltipInstance.option("contentTemplate", function (contentElement) {
//                                    contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + cs1000_loc[0][0] + ` , Manual: ` + cs1000_loc[0][1] + `   , DownTime</b></div>`);
//                                });

//                                tooltipInstance.show(arg.target);
//                            });
//                    }
//                    if (e.cell.columnPath[0] == Locations_list[1]) {
//                        e.cellElement.mouseover(function (arg) {

//                            tooltipInstance.option("contentTemplate", function (contentElement) {
//                                contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + cs1000_loc[1][0] + ` , Manual: ` + cs1000_loc[1][1] + `   , DownTime</b></div>`);
//                            });

//                            tooltipInstance.show(arg.target);
//                        });
//                    }
//                   // }
//                }
                        
                    

                
               


//                e.cellElement.mouseout(function (arg) {
//                    tooltipInstance.hide();
//                });
//            }





//            //if (e.area === "data" && e.cell.columnType != "GT") {  //Tooltip - specific CostElement for Comparison
//            //    debugger;
//            //    if (e.cell.rowPath[0] == "CCHIL") {
//            //        if (e.columnIndex == 0) {
//            //            e.cellElement.mouseover(function (arg) {

//            //                tooltipInstance.option("contentTemplate", function (contentElement) {
//            //                    contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + cchil_loc[0][0] + ` , Manual: ` + cchil_loc[0][1] + `   , DownTime</b></div>`);//[0]->loc1 [1]loc2;  [0]->uto[1]->man
//            //                });

//            //                tooltipInstance.show(arg.target);
//            //            });
//            //        }
//            //        if (e.columnIndex == 1) {
//            //            e.cellElement.mouseover(function (arg) {

//            //                tooltipInstance.option("contentTemplate", function (contentElement) {
//            //                    contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + cchil_loc[1][0] + ` , Manual: ` + cchil_loc[1][1] + `   , DownTime</b></div>`);
//            //                });

//            //                tooltipInstance.show(arg.target);
//            //            });
//            //        }

//            //    }
//            //    else if (e.cell.rowPath[0] == "CCSIL") {
//            //        if (e.columnIndex == 0) {
//            //            e.cellElement.mouseover(function (arg) {

//            //                tooltipInstance.option("contentTemplate", function (contentElement) {
//            //                    contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + ccsil_loc[0][0] + ` , Manual: ` + ccsil_loc[0][1] + `   , DownTime</b></div>`);
//            //                });

//            //                tooltipInstance.show(arg.target);
//            //            });
//            //        }
//            //        if (e.columnIndex == 1) {
//            //            e.cellElement.mouseover(function (arg) {

//            //                tooltipInstance.option("contentTemplate", function (contentElement) {
//            //                    contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + ccsil_loc[1][0] + ` , Manual: ` + ccsil_loc[1][1] + `   , DownTime</b></div>`);
//            //                });

//            //                tooltipInstance.show(arg.target);
//            //            });
//            //        }
//            //    }
//            //    else if (e.cell.rowPath[0] == "OTB") {
//            //        if (e.columnIndex == 0) {
//            //            e.cellElement.mouseover(function (arg) {

//            //                tooltipInstance.option("contentTemplate", function (contentElement) {
//            //                    contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + otb_loc[0][0] + ` , Manual: ` + otb_loc[0][1] + `   , DownTime</b></div>`);
//            //                });

//            //                tooltipInstance.show(arg.target);
//            //            });
//            //        }
//            //        if (e.columnIndex == 1) {
//            //            e.cellElement.mouseover(function (arg) {

//            //                tooltipInstance.option("contentTemplate", function (contentElement) {
//            //                    contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + otb_loc[1][0] + ` , Manual: ` + otb_loc[1][1] + `   , DownTime</b></div>`);
//            //                });

//            //                tooltipInstance.show(arg.target);
//            //            });
//            //        }
//            //    }
//            //    else if (e.cell.rowPath[0] == "CS1000") {
//            //        if (e.columnIndex == 0) {
//            //            e.cellElement.mouseover(function (arg) {

//            //                tooltipInstance.option("contentTemplate", function (contentElement) {
//            //                    contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` +  cs1000_loc[0][0] + ` , Manual: ` + cs1000_loc[0][1] + `   , DownTime</b></div>`);
//            //                });

//            //                tooltipInstance.show(arg.target);
//            //            });
//            //        }
//            //        if (e.columnIndex == 1) {
//            //            e.cellElement.mouseover(function (arg) {

//            //                tooltipInstance.option("contentTemplate", function (contentElement) {
//            //                    contentElement.html(`<div class='tooltipContent'><div><b>Auto: ` + cs1000_loc[1][0] + ` , Manual: ` + cs1000_loc[1][1] + `   , DownTime</b></div>`);
//            //                });

//            //                tooltipInstance.show(arg.target);
//            //            });
//            //        }
//            //    }
                

//            //    e.cellElement.mouseout(function (arg) {
//            //        tooltipInstance.hide();
//            //    });
//            //}
//        },
//        onCellClick: function (e) {
//            //debugger;
            

//            if (e.area === "data") {  //DrillDown 
//                // costelement_selected = e.value;
//                if (e.cell.columnType == "GT")//grnd total
//                {
//                    Locations_sel = getValue('ddlLocation');
//                }
//                else {
//                    Locations_sel = e.cell.columnPath[0] ;
//                }
//                Labtype_sel = e.cell.rowPath[0];
                  fnGetTSOU("Ban", "CCHIL");
//            }

            
//            $.notify('Loading, Please wait !', {
//                globalPosition: "top center",
//                className: "warn",
//                autoHideDelay: 5000
//            });


//        }
//    }).dxPivotGrid("instance");
   
//}

function fnGetTSOU(Locations_sel, Labtype_sel) {
    //debugger;
    var sdate = $("#startDate").val();
    var edate = $("#endDate").val();
    
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/SLCockpit/GetTSOU",
        datatype: "json",
        data: JSON.stringify({ SDate: sdate, EDate: edate, Location: Locations_sel, Labtype: Labtype_sel }),
        async: true,
        success: function (data) {
            //debugger;

            if (data.data.Labs.length > 0) {
                //debugger;
                //var res = JSON.parse(data.data.Data.Content);
                var res = eval(data.data);
                alert(res);
                slcockpit_tsou(res);
                //infra_datagrid(res);

            }
        }
    });

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/SLCockpit/GetTSOUOEM",
        datatype: "json",
        data: JSON.stringify({ SDate: sdate, EDate: edate, Location: Locations_sel, Labtype: Labtype_sel }),
        async: true,
        success: function (data) {
            //debugger;

            if (data.data.Labs.length > 0) {
                //debugger;
                //var res = JSON.parse(data.data.Data.Content);
                var res = eval(data.data);
                alert(res);
                slcockpit_oemtsou(res);
                //infra_datagrid(res);

            }
        }
    });
};


var excelobjtsou;
var Location;
var Labtype;
var Labname = [];
var date1;
var date2;
debugger;

function slcockpit_tsou(data) {
    //debugger;
    excelobjtsou = data.Labs;

    document.getElementById("chartdiv_tsou").style.display = "block";
   // var filtered = $('#selectLabId').val();

    var averageManHours = Math.round(data.OverallManualHours / excelobjtsou.length).toFixed(2);
    var averageAutoHours = Math.round(data.OverallAutomatedHours / excelobjtsou.length).toFixed(2);
    var averageManCaplHours = Math.round(data.OverallManualCaplHours / excelobjtsou.length).toFixed(2);
    var averageAutoCaplHours = Math.round(data.OverallAutomatedCaplHours / excelobjtsou.length).toFixed(2);
    var averageAllHours = Math.round((data.OverallAutomatedHours + data.OverallManualHours + data.OverallManualCaplHours + data.OverallAutomatedCaplHours) / excelobjtsou.length).toFixed(2);

    //var Labs = data.Labs;

    var startDate = data.StartDate;
    var endDate = data.EndDate;
    var sDate = startDate.replace('/Date(', '');
    var date_st = sDate.replace(')/', '');
    var stDate = parseInt(date_st);
    var eDate = endDate.replace('/Date(', '');
    var date_ed = eDate.replace(')/', '');
    var edDate = parseInt(date_ed);
    var StDate = new Date(stDate);
    var EndDate = new Date(edDate);
    //debugger;
    //var dd1 = StDate.getDate();
    //var mm1 = StDate.getMonth() + 1;
    //var yyyy1 = StDate.getFullYear();
    //if (dd1 < 10) {
    //    dd1 = '0' + dd1;
    //}
    //if (mm1 < 10) {
    //    mm1 = '0' + mm1;
    //}
    //date1 = mm1 + '/' + dd1 + '/' + yyyy1;

    //var dd2 = EndDate.getDate();
    //var mm2 = EndDate.getMonth() + 1;
    //var yyyy2 = EndDate.getFullYear();
    //if (dd2 < 10) {
    //    dd2 = '0' + dd2;
    //}
    //if (mm2 < 10) {
    //    mm2 = '0' + mm2;
    //}
    //date2 = mm2 + '/' + dd2 + '/' + yyyy2;

    date1 = data.StartDate_UI;


    date2 = data.EndDate_UI;



    //if (document.getElementById('selectLabId').selectedIndex > 0) {
    //    Location = "";
    //    Labtype = "";
    //}


    //debugger;
    //var sDate = data.StartDate;
    //var eDate = data.EndDate;
    //date1 = sDate;
    //date2 = eDate;
    am4core.useTheme(am4themes_animated);
    am4core.options.queue = true;
    am4core.options.onlyShowOnViewport = true;
    var chart = am4core.create("chartdiv_tsou", am4charts.XYChart);
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
    valueAxis.title.text = "Usage in hrs";
    valueAxis.title.rotation = 270;
    valueAxis.title.align = "center";
    valueAxis.title.valign = "middle";
    valueAxis.title.dy = -40;
    valueAxis.title.fontWeight = 600;



    var tempOemdata = [];
    var labData = [];
    var chartData = [];
    //debugger;
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
    debugger;
    for (let i = 0; i < keys.length; i++) {
        const key = keys[i];
        for (j = 0; j < groupedbyOEM[key].length; j++) {
            ////debugger;
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
                manual: Math.round(((groupedbyOEM[key][j].ManualTotalHours / hours_100p) * 100) * 100) / 100,
                automated: Math.round(((groupedbyOEM[key][j].AutomatedTotalHours / hours_100p) * 100) * 100) / 100,
                manual_capl: Math.round(((groupedbyOEM[key][j].ManualCaplTotalHours / hours_100p) * 100) * 100) / 100,
                automated_capl: Math.round(((groupedbyOEM[key][j].AutomatedCaplTotalHours / hours_100p) * 100) * 100) / 100,
                manualTime: Math.round(groupedbyOEM[key][j].ManualTotalHours),
                automatedTime: Math.round(groupedbyOEM[key][j].AutomatedTotalHours),
                reader: {
                    manual: Math.round((groupedbyOEM[key][j].ManualTotalHours) * 100) / 100,
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
        rangecat.label.text = tempArray[0].oem + " ";
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
    var range = valueAxis.axisRanges.create();
    range.value = 100; //100%
    range.grid.stroke = am4core.color("#33cc33"); //green
    range.grid.strokeWidth = 2;
    range.grid.strokeOpacity = 1;
    range.grid.above = true; //to display the grid line on top of the data.
    range.label.inside = true;
    range.label.text = "\xa0\xa0\xa0\xa0\xa0\xa0>12Hours";
    range.label.fontWeight = 750;
    range.label.fill = range.grid.stroke;
    range.label.verticalCenter = "bottom";
    var rangeRed = valueAxis.axisRanges.create();
    rangeRed.value = 67; //67%
    rangeRed.grid.stroke = am4core.color("#ff0000"); //red
    rangeRed.grid.strokeWidth = 2;
    rangeRed.grid.strokeOpacity = 1;
    rangeRed.grid.above = true; //to display the grid line on top of the data.
    rangeRed.label.inside = true;
    rangeRed.label.text = "\xa0\xa0\xa0\xa0\xa0\xa0>8Hours";
    rangeRed.label.fontWeight = 750;
    rangeRed.label.fill = rangeRed.grid.stroke;
    rangeRed.label.verticalCenter = "bottom";

    //Code to shade the marked ranges
    //background fill green for >100% utilization
    var greenBG = valueAxis.axisRanges.create();
    greenBG.value = 100
    greenBG.endValue = 200;
    greenBG.axisFill.fill = am4core.color("#99ff99");
    greenBG.axisFill.fillOpacity = 0.2;
    greenBG.grid.strokeOpacity = 0;

    //background fill yellow for 100 > x > 50 utilization
    var yellowBG = valueAxis.axisRanges.create();
    yellowBG.value = 67;
    yellowBG.endValue = 100;
    yellowBG.axisFill.fill = am4core.color("#ffff99");
    yellowBG.axisFill.fillOpacity = 0.2;
    yellowBG.grid.strokeOpacity = 0;

    //background fill red for <50% utilization
    var redBG = valueAxis.axisRanges.create();
    redBG.value = 0;
    redBG.endValue = 67;
    redBG.axisFill.fill = am4core.color("#ff6666");
    redBG.axisFill.fillOpacity = 0.2;
    redBG.grid.strokeOpacity = 0;



    var title = chart.titles.create();
    title.text = "Location:" + " " + Locations_sel + " " + "; LabType:" + " " + Labtype_sel + " " + ";StartDate:" + " " + date1 + " " + " - " + " " + "EndDate: " + date2;
    title.fontSize = 20;


    var series1 = chart.series.push(new am4charts.ColumnSeries());
    series1.columns.template.width = am4core.percent(30);
    series1.columns.template.propertyFields.dummyData = "reader";
    series1.columns.template.tooltipText = "Manual Usage: {dummyData.manual} hrs \x0a Total Usage: {dummyData.total} hrs";
    //" Manual Hours: {valueY.totalPercent.formatNumber('#.00')}%}";
    series1.name = "Manual Usage  :" + Math.round((data.OverallManualHours) * 100) / 100 + "hrs";
    series1.dataFields.categoryX = "name";
    series1.dataFields.valueY = "manual";
    series1.dataItems.template.locations.categoryX = 0.5;
    series1.stacked = true;
    series1.tooltip.pointerOrientation = "vertical";
    series1.tooltip.exportable = true;

    var bullet1 = series1.bullets.push(new am4charts.LabelBullet());
    bullet1.interactionsEnabled = false;
    bullet1.label.text = "Manual Usage: {valueY} %";
    bullet1.label.fill = am4core.color("#ffffff");
    bullet1.locationY = 0.5;


    var series2 = chart.series.push(new am4charts.ColumnSeries());
    series2.columns.template.width = am4core.percent(30);
    series2.columns.template.propertyFields.dummyData = "reader";
    series2.columns.template.tooltipText =
        "Automated Usage: {dummyData.automated} hrs \x0a Total Usage: {dummyData.total} hrs";
    series2.name = "Automated Usage :" + Math.round((data.OverallAutomatedHours) * 100) / 100 + "hrs";
    series2.dataFields.categoryX = "name";
    series2.dataFields.valueY = "automated";
    series2.dataItems.template.locations.categoryX = 0.5;
    series2.stacked = true;
    series2.tooltip.pointerOrientation = "vertical";
    series2.tooltip.exportable = true;

    var bullet2 = series2.bullets.push(new am4charts.LabelBullet());
    bullet2.interactionsEnabled = false;
    bullet2.label.text = "Automated Usage: {valueY} %";
    bullet2.locationY = 0.5;
    bullet2.label.fill = am4core.color("#ffffff");



    var series3 = chart.series.push(new am4charts.ColumnSeries());
    series3.columns.template.width = am4core.percent(30);
    series3.columns.template.propertyFields.dummyData = "reader";
    series3.columns.template.tooltipText =
        "Automated-CAPL Usage: {dummyData.automated_capl} hrs \x0a Total Usage: {dummyData.total} hrs";
    series3.name = "Automated Capl Usage :" + Math.round((data.OverallAutomatedCaplHours) * 100) / 100 + "hrs";
    series3.dataFields.categoryX = "name";
    series3.dataFields.valueY = "automated_capl";
    series3.dataItems.template.locations.categoryX = 0.5;
    series3.stacked = true;
    series3.tooltip.pointerOrientation = "vertical";
    series3.tooltip.exportable = true;

    var bullet3 = series3.bullets.push(new am4charts.LabelBullet());
    bullet3.interactionsEnabled = false;
    bullet3.label.text = "Automated-CAPL Usage: {valueY} %";
    bullet3.locationY = 0.5;
    bullet3.label.fill = am4core.color("#ffffff");


    var series4 = chart.series.push(new am4charts.ColumnSeries());
    series4.columns.template.width = am4core.percent(30);
    series4.columns.template.propertyFields.dummyData = "reader";
    series4.columns.template.tooltipText =
        "Manual-CAPL Usage: {dummyData.manual_capl} hrs \x0a Total Usage: {dummyData.total} hrs";
    series4.name = "Manual Capl Usage  :" + Math.round((data.OverallManualCaplHours) * 100) / 100 + "hrs";
    series4.dataFields.categoryX = "name";
    series4.dataFields.valueY = "manual_capl";
    series4.dataItems.template.locations.categoryX = 0.5;
    series4.stacked = true;
    series4.tooltip.pointerOrientation = "vertical";
    series4.tooltip.exportable = true;

    var bullet4 = series4.bullets.push(new am4charts.LabelBullet());
    bullet4.interactionsEnabled = false;
    bullet4.label.text = "Manual-CAPL Usage: {valueY} %";
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

    //debugger;
    //document.getElementById("spin1").style.display = "none";
    //document.querySelector('#export').style.pointerEvents = 'auto';


}



function slcockpit_oemtsou(data) {
    //debugger;
    excelobjtsou = data.Labs;

    document.getElementById("chartdiv_oemtsou").style.display = "block";
    // var filtered = $('#selectLabId').val();

    var totalManHours = Math.round(data.OverallManualHours /*/ excelobjtsou.length*/).toFixed(2);
    var totalAutoHours = Math.round(data.OverallAutomatedHours /*/ excelobjtsou.length*/).toFixed(2);
    
    var totalAllHours = Math.round((data.OverallAutomatedHours + data.OverallManualHours + data.OverallManualCaplHours + data.OverallAutomatedCaplHours) /*/ excelobjtsou.length*/).toFixed(2);
    var totalManPer = (Math.round(data.OverallManualHours / totalAllHours /*/ excelobjtsou.length*/).toFixed(2)) * 100;
    var totalAutoPer = (Math.round(data.OverallAutomatedCaplHours / totalAllHours /*/ excelobjtsou.length*/).toFixed(2)) * 100;
    //var Labs = data.Labs;

    var startDate = data.StartDate;
    var endDate = data.EndDate;
    var sDate = startDate.replace('/Date(', '');
    var date_st = sDate.replace(')/', '');
    var stDate = parseInt(date_st);
    var eDate = endDate.replace('/Date(', '');
    var date_ed = eDate.replace(')/', '');
    var edDate = parseInt(date_ed);
    var StDate = new Date(stDate);
    var EndDate = new Date(edDate);
    //debugger;
    //var dd1 = StDate.getDate();
    //var mm1 = StDate.getMonth() + 1;
    //var yyyy1 = StDate.getFullYear();
    //if (dd1 < 10) {
    //    dd1 = '0' + dd1;
    //}
    //if (mm1 < 10) {
    //    mm1 = '0' + mm1;
    //}
    //date1 = mm1 + '/' + dd1 + '/' + yyyy1;

    //var dd2 = EndDate.getDate();
    //var mm2 = EndDate.getMonth() + 1;
    //var yyyy2 = EndDate.getFullYear();
    //if (dd2 < 10) {
    //    dd2 = '0' + dd2;
    //}
    //if (mm2 < 10) {
    //    mm2 = '0' + mm2;
    //}
    //date2 = mm2 + '/' + dd2 + '/' + yyyy2;

    date1 = data.StartDate_UI;


    date2 = data.EndDate_UI;



    //if (document.getElementById('selectLabId').selectedIndex > 0) {
    //    Location = "";
    //    Labtype = "";
    //}


    //debugger;
    //var sDate = data.StartDate;
    //var eDate = data.EndDate;
    //date1 = sDate;
    //date2 = eDate;
    am4core.useTheme(am4themes_animated);
    am4core.options.queue = true;
    am4core.options.onlyShowOnViewport = true;
    var chart = am4core.create("chartdiv_oemtsou", am4charts.XYChart);
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
    valueAxis.title.text = "Usage in hrs";
    valueAxis.title.rotation = 270;
    valueAxis.title.align = "center";
    valueAxis.title.valign = "middle";
    valueAxis.title.dy = -40;
    valueAxis.title.fontWeight = 600;



    var tempOemdata = [];
    var labData = [];
    var chartData = [];
    //debugger;
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
    debugger;
    for (let i = 0; i < keys.length; i++) {
        const key = keys[i];
        for (j = 0; j < groupedbyOEM[key].length; j++) {
            ////debugger;
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
                name: groupedbyOEM[key][j].OEM,
                manual: Math.round(((groupedbyOEM[key][j].ManualTotalHours / hours_100p) * 100) * 100) / 100,
                automated: Math.round(((groupedbyOEM[key][j].AutomatedTotalHours / hours_100p) * 100) * 100) / 100,
                manual_capl: Math.round(((groupedbyOEM[key][j].ManualCaplTotalHours / hours_100p) * 100) * 100) / 100,
                automated_capl: Math.round(((groupedbyOEM[key][j].AutomatedCaplTotalHours / hours_100p) * 100) * 100) / 100,
                manualTime: Math.round(groupedbyOEM[key][j].ManualTotalHours),
                automatedTime: Math.round(groupedbyOEM[key][j].AutomatedTotalHours),
                reader: {
                    manual: Math.round((groupedbyOEM[key][j].ManualTotalHours) * 100) / 100,
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
        rangecat.label.text = tempArray[0].oem + " ";
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
    var range = valueAxis.axisRanges.create();
    range.value = 100; //100%
    range.grid.stroke = am4core.color("#33cc33"); //green
    range.grid.strokeWidth = 2;
    range.grid.strokeOpacity = 1;
    range.grid.above = true; //to display the grid line on top of the data.
    range.label.inside = true;
    range.label.text = "\xa0\xa0\xa0\xa0\xa0\xa0>12Hours";
    range.label.fontWeight = 750;
    range.label.fill = range.grid.stroke;
    range.label.verticalCenter = "bottom";
    var rangeRed = valueAxis.axisRanges.create();
    rangeRed.value = 67; //67%
    rangeRed.grid.stroke = am4core.color("#ff0000"); //red
    rangeRed.grid.strokeWidth = 2;
    rangeRed.grid.strokeOpacity = 1;
    rangeRed.grid.above = true; //to display the grid line on top of the data.
    rangeRed.label.inside = true;
    rangeRed.label.text = "\xa0\xa0\xa0\xa0\xa0\xa0>8Hours";
    rangeRed.label.fontWeight = 750;
    rangeRed.label.fill = rangeRed.grid.stroke;
    rangeRed.label.verticalCenter = "bottom";

    //Code to shade the marked ranges
    //background fill green for >100% utilization
    var greenBG = valueAxis.axisRanges.create();
    greenBG.value = 100
    greenBG.endValue = 200;
    greenBG.axisFill.fill = am4core.color("#99ff99");
    greenBG.axisFill.fillOpacity = 0.2;
    greenBG.grid.strokeOpacity = 0;

    //background fill yellow for 100 > x > 50 utilization
    var yellowBG = valueAxis.axisRanges.create();
    yellowBG.value = 67;
    yellowBG.endValue = 100;
    yellowBG.axisFill.fill = am4core.color("#ffff99");
    yellowBG.axisFill.fillOpacity = 0.2;
    yellowBG.grid.strokeOpacity = 0;

    //background fill red for <50% utilization
    var redBG = valueAxis.axisRanges.create();
    redBG.value = 0;
    redBG.endValue = 67;
    redBG.axisFill.fill = am4core.color("#ff6666");
    redBG.axisFill.fillOpacity = 0.2;
    redBG.grid.strokeOpacity = 0;



    var title = chart.titles.create();
    title.text = "Location:" + " " + Locations_sel + " " + "; LabType:" + " " + Labtype_sel + " " + ";StartDate:" + " " + date1 + " " + " - " + " " + "EndDate: " + date2;
    title.fontSize = 20;


    var series1 = chart.series.push(new am4charts.ColumnSeries());
    series1.columns.template.width = am4core.percent(30);
    series1.columns.template.propertyFields.dummyData = "reader";
    series1.columns.template.tooltipText = "Manual Usage: {dummyData.manual} hrs \x0a Total Usage: {dummyData.total} hrs";
    //" Manual Hours: {valueY.totalPercent.formatNumber('#.00')}%}";
    series1.name = "Manual Usage  :" + Math.round((data.OverallManualHours) * 100) / 100 + "hrs";
    series1.dataFields.categoryX = "name";
    series1.dataFields.valueY = "manual";
    series1.dataItems.template.locations.categoryX = 0.5;
    series1.stacked = true;
    series1.tooltip.pointerOrientation = "vertical";
    series1.tooltip.exportable = true;

    var bullet1 = series1.bullets.push(new am4charts.LabelBullet());
    bullet1.interactionsEnabled = false;
    bullet1.label.text = "Manual Usage: {valueY} %";
    bullet1.label.fill = am4core.color("#ffffff");
    bullet1.locationY = 0.5;


    var series2 = chart.series.push(new am4charts.ColumnSeries());
    series2.columns.template.width = am4core.percent(30);
    series2.columns.template.propertyFields.dummyData = "reader";
    series2.columns.template.tooltipText =
        "Automated Usage: {dummyData.automated} hrs \x0a Total Usage: {dummyData.total} hrs";
    series2.name = "Automated Usage :" + Math.round((data.OverallAutomatedHours) * 100) / 100 + "hrs";
    series2.dataFields.categoryX = "name";
    series2.dataFields.valueY = "automated";
    series2.dataItems.template.locations.categoryX = 0.5;
    series2.stacked = true;
    series2.tooltip.pointerOrientation = "vertical";
    series2.tooltip.exportable = true;

    var bullet2 = series2.bullets.push(new am4charts.LabelBullet());
    bullet2.interactionsEnabled = false;
    bullet2.label.text = "Automated Usage: {valueY} %";
    bullet2.locationY = 0.5;
    bullet2.label.fill = am4core.color("#ffffff");



    var series3 = chart.series.push(new am4charts.ColumnSeries());
    series3.columns.template.width = am4core.percent(30);
    series3.columns.template.propertyFields.dummyData = "reader";
    series3.columns.template.tooltipText =
        "Automated-CAPL Usage: {dummyData.automated_capl} hrs \x0a Total Usage: {dummyData.total} hrs";
    series3.name = "Automated Capl Usage :" + Math.round((data.OverallAutomatedCaplHours) * 100) / 100 + "hrs";
    series3.dataFields.categoryX = "name";
    series3.dataFields.valueY = "automated_capl";
    series3.dataItems.template.locations.categoryX = 0.5;
    series3.stacked = true;
    series3.tooltip.pointerOrientation = "vertical";
    series3.tooltip.exportable = true;

    var bullet3 = series3.bullets.push(new am4charts.LabelBullet());
    bullet3.interactionsEnabled = false;
    bullet3.label.text = "Automated-CAPL Usage: {valueY} %";
    bullet3.locationY = 0.5;
    bullet3.label.fill = am4core.color("#ffffff");


    var series4 = chart.series.push(new am4charts.ColumnSeries());
    series4.columns.template.width = am4core.percent(30);
    series4.columns.template.propertyFields.dummyData = "reader";
    series4.columns.template.tooltipText =
        "Manual-CAPL Usage: {dummyData.manual_capl} hrs \x0a Total Usage: {dummyData.total} hrs";
    series4.name = "Manual Capl Usage  :" + Math.round((data.OverallManualCaplHours) * 100) / 100 + "hrs";
    series4.dataFields.categoryX = "name";
    series4.dataFields.valueY = "manual_capl";
    series4.dataItems.template.locations.categoryX = 0.5;
    series4.stacked = true;
    series4.tooltip.pointerOrientation = "vertical";
    series4.tooltip.exportable = true;

    var bullet4 = series4.bullets.push(new am4charts.LabelBullet());
    bullet4.interactionsEnabled = false;
    bullet4.label.text = "Manual-CAPL Usage: {valueY} %";
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

    //debugger;
    //document.getElementById("spin1").style.display = "none";
    //document.querySelector('#export').style.pointerEvents = 'auto';


}


$("#export").click(function () {
    $.notify('Exporting is in Progress. Please wait!', {
        globalPosition: "top center",
        className: "warn"
    });
    $.ajax({
        type: "POST",
        url: "/LabCar/ExportTSOUDataToExcel/",
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
            //$("#mdbody").html("Error in Exporting TSOU Data to an Excel Sheet. Please try again Later.")
            //$('#myModal').modal('show');
            $.notify('Error in Exporting TSOU Data to an Excel Sheet. Please try again Later.', {
                globalPosition: "top center",
                className: "warn"
            });
        }
    });
});