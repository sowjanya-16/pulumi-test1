
function GenerateHwDamage_chart_Month(hwdamage_date,view) {
    debugger;
    $.ajax({
        type: "POST",
        // contentType: "application/json; charset=utf-8",
        url: "../SLCockpit/GenerateHwDamage_chart",
        dataType: 'json',
        //traditional: true,
        data: { /*'year': filtered_yr,*/ 'charttype': "Month", 'hwDamage_date': hwdamage_date, 'view' : view },
        success: function (data) {
            debugger;

            var monthwise_damage = JSON.parse(data.data.Data.Content);
            var parts = hwdamage_date.split('-');
            var EndMonth = (new Date(parts[0], parts[1] - 1, parts[2]).toDateString()).split(' ')[1];
            debugger;
            var res1 = eval(monthwise_damage);

            $("#hwDamage_monthchart").dxChart({

                dataSource: res1,
                resolveLabelOverlapping: 'stack',
                //scrollBar: {
                //    visible: true
                //},
                
                argumentAxis: {
                    label: {
                        wordWrap: 'none',
                        overlappingBehavior: 'rotate',
                        font: {
                            color: "black",
                            family: "Arial",
                            size: 13

                        },
                    },
                },
                legend: {
                    visible:
                        false

                },
                //customizePoint() {
                //    debugger;
                //    if (this.argument == EndMonth)
                //        return { color: '#db9a9a', hoverStyle: { color: '#f5a2a2' } };
                //    //if (this.value > 1000) {
                //    //    return { color: '#ff7c7c', hoverStyle: { color: '#ff7c7c' } };
                //    //}
                //    //if (this.value < 500) {
                //    //    return { color: '#8c8cff', hoverStyle: { color: '#8c8cff' } };
                //    //}
                //    return null;
                //},
                //zoomAndPan: {
                //    argumentAxis: "both",

                //},
                ////height: 50,
               // size: {
                    height: 150,
                 //   width: 520//480,
              //  },

                commonSeriesSettings: {
                    argumentField: "Month",
                    type: "bar",
                    hoverMode: "allArgumentPoints",
                    selectionMode: "allArgumentPoints",
                    //barPadding: 5,
                    barWidth: 30



                },




                series: [

                    {
                        valueField: "Damage_Cost", name: "Damage_Cost",
                        label: {

                            visible: true,
                            backgroundColor:'#4080AD',
                                //'#005691',
                            //color: '#005691',
                            //format: {
                            //    type: "currency", /*currency: "EUR"*/
                            //    precision: 0
                            //},
                            customizeText: function () {
                                return "€" + this.value.toFixed(0);
                            }



                        },
                       // color: '#009E60'
                        color: '#40AADB',//'#4dbb90'
                    },


                ],





                valueAxis: [


                    {

                        position: 'left',
                        label: {
                            font: { color: "black" }
                        },
                        valueMarginsEnabled: false,
                        title: {
                            text: "Cost (in €)",
                            font: {
                                color: "black",
                                family: "Arial",
                                size: 13},
                           
                        },

                        //constantLines: [{
                        //    value: AverageVN_data,
                        //    color: '#1b4f72',
                        //    dashStyle: 'dash',
                        //    width: 2,
                        //    label: { visible: false },
                        //    label: {
                        //        text: 'VN Avg'
                        //    },
                        //}],
                    },


                ],

                //title: {
                //    text: 'Hardware Damage per Month (in EUR)',
                //},
                //export: {
                //    enabled: true,
                //    fileName: "Hardware Damage (Monthwise)"
                //},
                tooltip: {
                    enabled: true,
                    location: "edge",
                    customizeTooltip: function (arg) {
                        debugger;

                        return {
                            text: " Month: " + arg.argument + " - " + "€" + arg.value.toFixed(0)
                        }

                    }
                },
            });


        },

        error: function (jqXHR, exception) {
            debugger;
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            $('#post').html(msg);
        },
    });


}
function GenerateHwDamage_Total(hwdamage_date,view) {
    var genSpinner_damage = document.querySelector("#load_damage");
   // Label_HW_DamageTotal
   
    $.ajax({
        type: "POST",
        //contentType: "application/json; charset=utf-8",
        url: "../SLCockpit/GetHwDamage_Totals",
        dataType: 'json',
        data: { /*'year': filtered_yr,*/ 'hwDamage_date': hwdamage_date, 'view' : view},
        //traditional: true,
        success: function (data) {
            debugger;
            var currentsel_data, prevyr_data;
            if (data.data1 == "")
                currentsel_data = '-';
            else
                currentsel_data = "€" + data.data1;
            if (data.data2 == "")
                prevyr_data = '-';
            else
                prevyr_data = "€" + data.data2;

           

            document.getElementById('HW_DamageTotal').innerHTML = currentsel_data;
            document.getElementById('HW_DamagePreviousTotal').innerHTML = prevyr_data;
            genSpinner_damage.classList.remove('fa');
            genSpinner_damage.classList.remove('fa-spinner');
            genSpinner_damage.classList.remove('fa-pulse');
            $('#load_damage').prop('hidden', true);
            
        },

        error: function (jqXHR, exception) {
            debugger;
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            $('#post').html(msg);
        },
    });


    //document.getElementById("HW_DamageTotal").addEventListener("click", function () {
    //    GenerateHwDamage_chart_OEM(hwdamage_date);
    //}, false);
}

//document.getElementById("HW_DamageTotal").onclick = function (hwdamage_date) {
//    debugger;                  
//    GenerateHwDamage_chart_OEM(hwdamage_date);
//}

function GenerateHwDamage_chart_OEM(hwdamage_date, view) {
    debugger;
    var parts = hwdamage_date.split('-');
    var EndMonth = (new Date(parts[0], parts[1] - 1, parts[2]).toDateString()).split(' ')[1];

    document.getElementById("cards").style.display = "none";
    document.getElementById("hwDamage_oemchart").style.display = "block";
    $('#hc_grid_title').css("display", "none");
    //removePointerCursor(chartContainer);
    $('#backButton')
        .dxButton('instance')
        .option('visible', true);

    
    $.ajax({
        type: "POST",
        //contentType: "application/json; charset=utf-8",
        url: "../SLCockpit/GenerateHwDamage_chart",
        dataType: 'json',
        data: { /*'year': filtered_yr,*/ 'charttype': "OEM", 'hwDamage_date': hwdamage_date ,'view' : view},
        //traditional: true,
        success: function (data) {
            debugger;

            var oemwise_damage = JSON.parse(data.data.Data.Content);

            var res1 = eval(oemwise_damage);

            $("#hwDamage_oemchart").dxChart({

                dataSource: res1,
                resolveLabelOverlapping: 'stack',
                color: '#57b8bc',
                //scrollBar: {
                //    visible: true
                //},
                //customizePoint() {
                //    debugger;
                //    if (this.seriesName == EndMonth)
                //        return { color: '#db9a9a', hoverStyle: { color: '#f5a2a2' } };
                //    //if (this.value > 1000) {
                //    //    return { color: '#ff7c7c', hoverStyle: { color: '#ff7c7c' } };
                //    //}
                //    //if (this.value < 500) {
                //    //    return { color: '#8c8cff', hoverStyle: { color: '#8c8cff' } };
                //    //}
                //    return null;
                //},
                argumentAxis: {
                    label: {
                        wordWrap: 'none',
                        overlappingBehavior: 'rotate',
                        font: {
                            color: "black",
                            family: "Arial",
                            size: 13

                        },
                    },
                },

                //zoomAndPan: {
                //    argumentAxis: "both",

                //},
                ////height: 50,
                size: {
                    height: 400//200,
                    // width: 1500,
                },
                title: 'Hardware Damage Cost per OEM',
                commonSeriesSettings: {
                    argumentField: 'OEM',
                    valueField: 'Damage_Cost',
                    //color: '#57b8bc',
                    label: {
                        visible: true,
                        customizeText: function () {
                            return "€" + this.value.toFixed(0);
                        },
                        //backgroundColor:'violet'




                    },
                    type: 'bar',
                    barWidth: 50,

                },
                seriesTemplate: {
                    nameField: 'Month',
                    //color: '#57b8bc'

                },
                //color: '#57b8bc',
                palette: 'Violet',
                    //'Dark Moon',
                    //'Dark Violet',
                    //'Soft Blue',
                    //'Ocean',
                //onLegendClick: function (e) {
                //    debugger;
                //    var hiddenPoints, shownPoints;
                  
                //        hiddenPoints = e.component.getSeriesByName("Jan").getAllPoints();
                //    shownPoints = e.target.getAllPoints();
                //    //lastClickedSeriesName = e.target.name;
                    
                  
                //    $.each(shownPoints, function (index, point) {
                        
                //        point.showTooltip();//getLabel().show();
                //    });
                //}  ,
                onLegendClick: function (e) {
                    debugger;
                    var series_curr = e.target;
                   
                    //if (series.isVisible()) {
                    //    series.hide();
                    //} else {
                    //    series.show();
                    //}
                   // series.opacity = 1;
                    for (i = 0; i < 12; i++) {
                        if (e.component.series[i].name != series_curr.name) {
                            e.component.series[i].hide();
                        }
                        else {
                            shownPoints = e.target.getAllPoints();
                            e.component.series[i].show();
                            $.each(shownPoints, function (index, point) {
                                debugger;
                                point.showTooltip();//getLabel().show();
                            });
                           
                        }
                           

                    }
                   
                },
                //legend: {
                //    customizeHint: function (e) {
                //        debugger;
                //        e.seriesColor = '#ff7c7c';
                //    }

                //},
                //seriesHoverChanged: function (arg) {
                //    debugger;
                //    return {
                //    text: "Project Team: " + arg.argument + ", Month: " + arg.seriesName + " - " + "€*******" + arg.value.toFixed(0)
                //}
                //},
                //return {
                //    text: "Project Team: " + arg.argument + ", Month: " + arg.seriesName + " - " + "€" + arg.value.toFixed(0)
                //}



               
                    //'Harmony Light',
                    //'Soft Blue',
                    //'Ocean',
                //commonSeriesSettings: {
                //    argumentField: "OEM",
                //    type: "bar",
                //    hoverMode: "allArgumentPoints",
                //    selectionMode: "allArgumentPoints",
                //    //barPadding: 5,
                //    barWidth: 50,
                //    valueField: "Damage_Cost", //name: "Damage_Cost",
                //    label: {

                //        visible: true,



                //    }


                //},


                //seriesTemplate: {
                //    nameField: 'Month',

                //},

                //series: [

                //    {
                //        valueField: "Damage_Cost", name: "Damage_Cost",
                //        label: {

                //            visible: true,



                //        }
                //    },


                //],





                valueAxis: [


                    {

                        position: 'left',
                        label: {
                            font: { color: "black" }
                        },
                        valueMarginsEnabled: false,
                        title: {
                            text: "Cost (in EUR)",
                            font: { color: "black" },
                        },

                        //constantLines: [{
                        //    value: AverageVN_data,
                        //    color: '#1b4f72',
                        //    dashStyle: 'dash',
                        //    width: 2,
                        //    label: { visible: false },
                        //    label: {
                        //        text: 'VN Avg'
                        //    },
                        //}],
                    },


                ],

                //title: 'Hardware Damage per Project Team',

                //export: {
                //    enabled: true,
                //    fileName: "Hardware Damage (Project Teamwise)"
                //},
                //tooltip: {
                //    enabled: true
                //}
                tooltip: {
                    enabled: true,

                    location: "edge",
                    customizeTooltip: function (arg) {
                        debugger;
                        //if (arg.percentText != null)
                        //    return { text: arg.point.data.Location + ":" + arg.valueText + "\n" + arg.point.series.name + ":" + arg.percentText }
                        //else
                        //    return { text: arg.point.data.Location + ":" + arg.valueText }
                        return {
                            text: "Project Team: " + arg.argument + ", Month: " + arg.seriesName + " - " + "€" + arg.value.toFixed(0)
                        }

                    }
                },
            });


        },

        error: function (jqXHR, exception) {
            debugger;
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            $('#post').html(msg);
        },
    });
}
   
$('#backButton').dxButton({
    text: 'Back',
    icon: 'chevronleft',
    visible: false,
    onClick() {
        debugger;

        // addPointerCursor(chartContainer);
        document.getElementById("cards").style.display = "block";
        document.getElementById("DeliveryStatus").style.display = "none";
        document.getElementById("chartdiv_infra").style.display = "none";
        document.getElementById("hwDamage_oemchart").style.display = "none";
        document.getElementById("hc_grid").style.display = "none";
        document.getElementById("breadcrumb_nav1").style.display = "block";
        document.getElementById("breadcrumb_nav2").style.display = "none";
        document.getElementById("twoyr_view").style.display = "block";
        document.getElementById("costelement_view").style.display = "none";
        document.getElementById("category_view").style.display = "none";
        document.getElementById("item_view").style.display = "none";
        document.getElementById("threeyr_view").style.display = "none";
        var bd1 = document.getElementById("item_nav");
        bd1.style.color = "white";
        var bd2 = document.getElementById("costelement_nav");
        bd2.style.color = "white";
        var bd3 = document.getElementById("category_nav");
        bd3.style.color = "white";
        var bd4 = document.getElementById("twoyr_nav");
        bd4.style.color = "black";
        var bd5 = document.getElementById("threeyr_nav");
        bd5.style.color = "white";

        this.option('visible', false);
        //$('#cards').prop('hidden', false);
        //$('#chartdiv_infra').prop('hidden', true);
        //$('#hc_grid').prop('hidden', true);
        //$('#breadcrumb_nav1').prop('hidden', false);
        ////$('#backButton').dxButton('instance').option('visible', false);
        //$('#breadcrumb_nav2').prop('hidden', true);
        //$('#twoyr_view').prop('hidden', false);
        //$('#costelement_view').prop('hidden', true);
        //$('#category_view').prop('hidden', true);
        //$('#item_view').prop('hidden', true);
        //$('#threeyr_view').prop('hidden', true);
        //$('#twoyr_nav').css({ "color": "black" });



    },
});


//function GenerateHwDamage(month_list, loc_list, item_list, project_list, hwstatus_list) {
//    debugger;
//    var CostEUR;

//    $.ajax({
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//        url: "../HwDamage/GenerateHwDamage",
//        dataType: 'json',
//        //traditional: true,
//        success: function (data) {
//            debugger;
            
//            var res = JSON.parse(data.data.Data.Content);
           
//            //var res1 = eval(res);
//            $("#hwDamage_dataGrid").dxDataGrid({

//                dataSource: res,
//                keyExpr: "ID",
//                columnResizingMode: "nextColumn",
//                columnMinWidth: 50,
//                stateStoring: {
//                    enabled: true,
//                    type: "localStorage",
//                    storageKey: "RequestID"
//                },
//                onContentReady: function (e) {
//                    debugger;
//                    e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
//                },
//                //noDataText: encodeMessage(noDataTextUnsafe),
//                //showRequiredMark: true,
//                //RequiredMark: '*',
//                //columnResizingMode: "widget",
//                //columnMinWidth: 100,
//                onCellPrepared: function (e) {
//                    if (e.rowType === "header" || e.rowType === "filter") {
//                        e.cellElement.addClass("columnHeaderCSS");
//                        e.cellElement.find("input").addClass("columnHeaderCSS");
//                    }
//                },
//                noDataText: " ☺ No Item ! click '+' Add a row option on the top right",
//                editing: {
//                    mode: "row",
//                    allowUpdating: function (e) {
//                        return true;
//                    },
//                    allowDeleting:true,
//                    allowAdding: true,
//                    useIcons: true
//                },
//                onToolbarPreparing: function (e) {
//                    var dataGrid = e.component;

//                    e.toolbarOptions.items[0].showText = 'always';


//                },
//                //focusedRowEnabled: true,

//                allowColumnReordering: true,
//                allowColumnResizing: true,
//                columnChooser: {
//                    enabled: true
//                },

//                filterRow: {
//                    visible: true

//                },
//                showBorders: true,
//                headerFilter: {
//                    visible: true,
//                    applyFilter: "auto"
//                },
//                selection: {
//                    applyFilter: "auto"
//                },
//                loadPanel: {
//                    enabled: true
//                },
//                paging: {
//                    pageSize: 100
//                },
//                searchPanel: {
//                    visible: true,
//                    width: 240,
//                    placeholder: "Search..."
//                },
               
//                onEditorPreparing: function (e) {
//                    var component = e.component,
//                        rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex
                    
//                    if (e.dataField === "Hw_status") {
                        
//                        var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                        
//                        e.editorOptions.onValueChanged = function (e) {
                            
//                            onValueChanged.call(this, e);
//                            debugger;
//                            var Item_sel = component.cellValue(rowIndex, "Item_Name");
//                            if (component.cellValue(rowIndex, "Item_Name") != undefined && component.cellValue(rowIndex, "Item_Name") != null) {
//                                debugger;
//                                $.ajax({

//                                    type: "post",
//                                    url: "/HwDamage/GetCostEUR",
//                                    data: { Hw_status: e.value, ItemName: component.cellValue(rowIndex, "Item_Name")},
//                                    datatype: "json",
//                                    traditional: true,
//                                    success: function (data) {
//                                        debugger;
//                                        if (data.msg) {
//                                            $.notify(data.msg, {
//                                                globalPosition: "top center",
//                                                className: "success"
//                                            })
//                                        }
//                                        else {


//                                            CostEUR = data.Cost_EUR;
//                                            window.setTimeout(function () {
//                                                debugger;
//                                                component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
//                                            }, 1000);
//                                        }
                                       

//                                    }
//                                })
//                                //// Emulating a web service call
                            
//                            }
                            
                            
//                        }
//                    }
//                    if (e.dataField === "Item_Name") {

//                        var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data

//                        e.editorOptions.onValueChanged = function (e) {

//                            onValueChanged.call(this, e);
//                            debugger;
//                            var status_sel = component.cellValue(rowIndex, "Hw_status");
//                            if (status_sel != undefined && status_sel != null) {
//                                $.ajax({

//                                    type: "post",
//                                    url: "/HwDamage/GetCostEUR",
//                                    data: { Hw_status: status_sel, ItemName: e.value },
//                                    datatype: "json",
//                                    traditional: true,
//                                    success: function (data) {
//                                        debugger;
//                                        if (data.msg) {
//                                            $.notify(data.msg, {
//                                                globalPosition: "top center",
//                                                className: "warn"
//                                            })
//                                        }
//                                        else {
//                                            CostEUR = data.Cost_EUR;
//                                            //// Emulating a web service call
//                                            window.setTimeout(function () {
//                                                debugger;
//                                                component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
//                                            }, 1000);
//                                        }
                                            


//                                    }
//                                })
                                
//                            }


//                        }
//                    }
//                },


//                //onEditorPreparing: function (e) {
//                //    var component = e.component,
//                //        rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex



//                //    //if (e.dataField === "Item_Name") {

//                //    //    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
//                //    //    e.editorOptions.onValueChanged = function (e) {
//                //    //        onValueChanged.call(this, e);
//                //    //        $.ajax({
//                //    //            type: "post",
//                //    //            url: "/BudgetingRequest/GetUnitPrice",
//                //    //            data: { ItemName: e.value },
//                //    //            datatype: "json",
//                //    //            traditional: true,
//                //    //            success: function (data) {

//                //    //                if (data > 0)
//                //    //                    unitprice = data;

//                //    //            }
//                //    //        })

//                //    //        $.ajax({

//                //    //            type: "post",
//                //    //            url: "/BudgetingRequest/GetCategory",
//                //    //            data: { ItemName: e.value },
//                //    //            datatype: "json",
//                //    //            traditional: true,
//                //    //            success: function (data) {
//                //    //                category = data;

//                //    //            }
//                //    //        })

//                //    //        $.ajax({

//                //    //            type: "post",
//                //    //            url: "/BudgetingRequest/GetCostElement",
//                //    //            data: { ItemName: e.value },
//                //    //            datatype: "json",
//                //    //            traditional: true,
//                //    //            success: function (data) {
//                //    //                costelement = data;

//                //    //            }
//                //    //        })

//                //    //        $.ajax({

//                //    //            type: "post",
//                //    //            url: "/BudgetingRequest/GetActualAvailableQuantity",
//                //    //            data: { ItemName: e.value },
//                //    //            datatype: "json",
//                //    //            traditional: true,
//                //    //            success: function (data) {
//                //    //                debugger;
//                //    //                actualavailquantity = data;

//                //    //            }
//                //    //        })

//                //    //        window.setTimeout(function () {

//                //    //            component.cellValue(rowIndex, "Unit_Price", unitprice);
//                //    //            component.cellValue(rowIndex, "Category", category);
//                //    //            component.cellValue(rowIndex, "Cost_Element", costelement);
//                //    //            component.cellValue(rowIndex, "ActualAvailableQuantity", actualavailquantity);

//                //    //        },
//                //    //            1000);


//                //    //    }

//                //    //}


//                //},
//                //onEditorPreparing: function (e) {
//                //    debugger;
//                //    if ((e.parentType === "dataRow" || e.parentType === "filterRow") && e.dataField === "Month") {
//                //        e.editorName = "dxTagBox"
//                //        e.editorOptions.dataSource = month_list;
//                //        e.editorOptions.showSelectionControls = true;
//                //        //e.editorOptions.displayExpr = "Name";
//                //        //e.editorOptions.valueExpr = "ID";
//                //        e.editorOptions.value = e.value || [];
//                //        e.editorOptions.onValueChanged = function (args) {
//                //            e.setValue(args.value);
//                //        }
//                //    }
//                //},
            
//                columns: [
//                    {
//                        type: "buttons",
//                        width: 100,
//                        alignment: "left",
//                        buttons: [
//                            "edit", "delete"
                           
//                        ]
//                    },
//                    {

//                        alignment: "center",   
//                        columns: [

//                            {
//                                dataField: "Location",
                                
//                                width: 130,
//                                lookup: {
//                                    dataSource: function (options) {
                                       
//                                        return {

//                                            store: loc_list,
//                                            //filter: options.data ? ["Outdated", "=", false] : null


//                                        };
//                                    },

//                                    valueExpr: "ID",
//                                    displayExpr: "Location"

//                                },



//                            },
//                            {
//                                dataField: "Month",
                                
//                                validationRules: [{ type: "required" }],
//                               // editCellTemplate: tagBoxEditorTemplate,
//                                lookup: {
//                                    dataSource: function (options) {
                                       
//                                        return {
//                                            store: month_list,
//                                        };
//                                    },
//                                    valueExpr: "ID",
//                                    displayExpr: "Month"
//                                },
//                                //cellTemplate(container, options) {
//                                //    debugger;
//                                //    const noBreakSpace = '\u00A0';
//                                //    const text = (options.value || []).map((element) => options.column.lookup.calculateCellValue(element)).join(', ');
//                                //    container.text(text || noBreakSpace).attr('title', text);
//                                //},
//                                //calculateFilterExpression(filterValue, selectedFilterOperation, target) {
//                                //    debugger;
//                                //    if (target === 'search' && typeof (filterValue) === 'string') {
//                                //        return [this.dataField, 'contains', filterValue];
//                                //    }
//                                //    return function (data) {
//                                //        return (data.Month || []).indexOf(filterValue) !== -1;
//                                //    };
//                                //},
//                                //calculateDisplayValue: function (rowData) {
//                                //    debugger;
//                                //    var filterExpression = [],
//                                //        values = rowData.Month;
//                                //    for (var i = 0; i < values.length; i++) {
//                                //        debugger;
//                                //        if (i > 0) {
//                                //            filterExpression.push('or');
//                                //        }
//                                //        filterExpression.push(['ID', values[i]]);
//                                //    }
//                                //    var result = $.map(DevExpress.data.query(month_list).filter(filterExpression).toArray(), function (item) {
//                                //        debugger;
//                                //        return item.Name;
//                                //    }).join(',');
//                                //    return result;
//                                //},
//                                //calculateFilterExpression: function (filterValues, selectedFilterOperation) {
//                                //    console.log(new Date().toString());
//                                //    return function (itemData) {
//                                //        var array1 = itemData.Month;
//                                //        var array2 = filterValues;

//                                //        if (array2.length === 0)
//                                //            return true;

//                                //        return array1.length === array2.length && array1.every(function (value, index) { return value === array2[index] })
//                                //    };
//                                //}
//                            },
//                            {
//                                dataField: "Item_Name",       
//                                //caption: "Dept",
//                                validationRules: [{ type: "required" }],
//                                lookup: {
//                                    dataSource: function (options) {
                                       
//                                        return {

//                                            store: item_list,
//                                            //filter: options.data ? ["Outdated", "=", false] : null


//                                        };
//                                    },

//                                    valueExpr: "S#No",
//                                    displayExpr: "Item Name"

//                                },



//                            },

//                            {
//                                dataField: "Qty",
//                                //width: 100,
//                                validationRules: [
//                                    { type: "required" },
//                                    {
//                                        type: "range",
//                                        message: "Please enter valid count > 0",
//                                        min: 0,
//                                        max: 214783647
//                                    }],
//                                dataType: "number",
//                                setCellValue: function (rowData, value) {

//                                    rowData.Qty = value;

//                                },


//                            },
//                            {
//                                dataField: "Project_Team",//month_list, loc_list, item_list, project_list
//                                //caption: "Dept",
//                                validationRules: [{ type: "required" }],
//                                lookup: {
//                                    dataSource: function (options) {
//                                        debugger;
//                                        return {

//                                            store: project_list,
//                                            //filter: options.data ? ["Outdated", "=", false] : null


//                                        };
//                                    },

//                                    valueExpr: "ID",
//                                    displayExpr: "OEM"

//                                },



//                            },
//                            {
//                                dataField: "Hw_status",//month_list, loc_list, item_list, project_list
//                                //caption: "Dept",
//                                validationRules: [{ type: "required" }],
//                                lookup: {
//                                    dataSource: function (options) {
                                       
//                                        return {

//                                            store: hwstatus_list,
//                                            //filter: options.data ? ["Outdated", "=", false] : null


//                                        };
//                                    },

//                                    valueExpr: "Status",
//                                    displayExpr: "Status"

//                                },
//                            },
                                
                            
//                            {
//                                dataField: "Cost_inEUR",
//                                caption: "Cost (inEUR)",
//                                width: 70,

//                            },
                         
//                            {
//                                dataField: "Total_Price",
//                                width: 70,


//                                calculateCellValue: function (rowData) {
//                                    //update the bud-inv
//                                    var total;
//                                    if (rowData.Qty == undefined || rowData.Cost_inEUR == null || rowData.Qty == 0 || rowData.Cost_inEUR == undefined || rowData.Cost_inEUR == 0.0 || rowData.Qty == null) {

//                                        total = 0.0;
//                                    }
//                                    else if (rowData.Qty != null && rowData.Qty != undefined && rowData.Cost_inEUR != null && rowData.Cost_inEUR != undefined) {

//                                        total = rowData.Qty * rowData.Cost_inEUR;
//                                    }
                                    

//                                    return total;
//                                }

//                            },
//                            {
//                                dataField: "RequestorNT",
//                                caption: "Updated By"
//                            }
                            

//                        ]
//                    }],
               
//                onRowUpdated: function (e) {
//                    $.notify("Please wait, Updating details..!", {
//                        globalPosition: "top center",
//                        className: "success"
//                    })
//                    Selected = [];
//                    e.data.Total_Price = e.data.Qty * e.data.Cost_inEUR; 
//                    debugger;
//                    Selected.push(e.data);
//                    Update(Selected);
//                },

//                onRowInserting: function (e) {
//                    $.notify("Please wait, Saving details..!", {
//                        globalPosition: "top center",
//                        className: "success"
//                    })
                    
//                    Selected = [];
//                    debugger;
//                    e.data.Total_Price = e.data.Qty * e.data.Cost_inEUR; 
//                    Selected.push(e.data);



//                    Update(Selected);
//                },
//                onRowRemoving: function (e) {

//                    Delete(e.data.ID/*, filtered_yr*/);

//                }


//            });
//            //function tagBoxEditorTemplate(cellElement, cellInfo) {
//            //    debugger;
//            //    return $('<div>').dxTagBox({
//            //        dataSource: month_list,
//            //        value: cellInfo.value,
//            //        //valueExpr: 'ID',
//            //        //displayExpr: 'FullName',
//            //        showSelectionControls: true,
//            //        maxDisplayedTags: 3,
//            //        showMultiTagOnly: false,
//            //        applyValueMode: 'useButtons',
//            //        searchEnabled: true,
//            //        onValueChanged(e) {
//            //            debugger;
//            //            cellInfo.setValue(e.value);
//            //        },
//            //        onSelectionChanged() {
//            //            debugger;
//            //            cellInfo.component.updateDimensions();
//            //        },
//            //    });
//            //}
//        },

//        error: function (jqXHR, exception) {
//            debugger;
//            var msg = '';
//            if (jqXHR.status === 0) {
//                msg = 'Not connect.\n Verify Network.';
//            } else if (jqXHR.status == 404) {
//                msg = 'Requested page not found. [404]';
//            } else if (jqXHR.status == 500) {
//                msg = 'Internal Server Error [500].';
//            } else if (exception === 'parsererror') {
//                msg = 'Requested JSON parse failed.';
//            } else if (exception === 'timeout') {
//                msg = 'Time out error.';
//            } else if (exception === 'abort') {
//                msg = 'Ajax request aborted.';
//            } else {
//                msg = 'Uncaught Error.\n' + jqXHR.responseText;
//            }
//            $('#post').html(msg);
//        },
//    });


//    function Update(id1/*, filtered_yr*/) {
//        debugger;
//        $.ajax({
//            type: "POST",
//            url: encodeURI("../HwDamage/Save_HwDamagedata"),
//            data: { 'req': id1[0]},
//            success: function (data) {
//                debugger;
//                newobjdata = JSON.parse(data.data.Data.Content);

//                $("#hwDamage_dataGrid").dxDataGrid({ dataSource: newobjdata });

//                if (data.success == true) {
//                    $.notify("Sucess", {
//                        globalPosition: "top center",
//                        className: "success",
//                        autoHideDelay: 13000,
//                    })
//                }
//                else {
//                    $.notify("Fail", {
//                        globalPosition: "top center",
//                        className: "error",
//                        autoHideDelay: 10000,
//                    })
//                }

//            }

//        });

//    }

//    function Delete(id/*, filtered_yr*/) {

//        $.ajax({
//            type: "POST",
//            url: "/HwDamage/Delete",
//            data: { 'id': id/*, 'useryear': filtered_yr*/ },
//            success: function (data) {
                
//                $("#hwDamage_dataGrid").dxDataGrid({ dataSource: JSON.parse(data.data.Data.Content) });


//                if (data.success == false) {
//                    $.notify("Try again", {
//                        globalPosition: "top center",
//                        className: "error",
//                        autoHideDelay: 13000,
//                    })
//                }
//                else {
//                    $.notify("Deleted Successfully!", {
//                        globalPosition: "top center",
//                        className: "success",
//                        autoHideDelay: 10000,
//                    })
//                }


//            }

//        });
//    }


//}
