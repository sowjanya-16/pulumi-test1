function GenerateHCChart() {
    //debugger;
    //var LabTypes = $('#selectLabType').val();
    //var Locations = $('#selectLocation').val();
    var hcchart_data;
   
    var isPlan_Utilize;
    var year_selected;
    //const chartContainer = $('#hc_chart');
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../SLCockpit/HCChart",
        dataType: 'json',
        //traditional: true,
        success: function (data) {
            //debugger;
            //var tooltipInstance1 = $("#tooltipContainer_HC").dxTooltip({
            //    //position: "right"
            //}).dxTooltip("instance");

            hcchart_data = JSON.parse(data.data.Data.Content);

            var chart_hc = $('#hc_chart').dxChart({

                dataSource: hcchart_data,
                // resolveLabelOverlapping: 'stack',
                //scrollBar: {
                //    visible: true
                //},    

                //zoomAndPan: {
                //    argumentAxis: "both",

                //},
                //height: 50,
               // size: {
                   // height: 130,//158,
                   // width: 480

               // },

                commonSeriesSettings: {
                    argumentField: "Year",

                    type: "bar",
                    barWidth: 30
                    //label: {
                    //    format: {
                    //        type: 'decimal',
                    //    },
                    //},
                    //hoverMode: "allArgumentPoints",
                    //selectionMode: "allArgumentPoints",

                },

                series: [

                    {
                        valueField: "Plan", name: "Plan",
                        label: {

                            visible: true,
                            backgroundColor: '#4080AD',



                        },
                       
                        color: '#40AADB',
                    },
                    {
                        valueField: "Utilize", name: "Utilize",
                        label: {

                            visible: true,
                            backgroundColor: '#005691',
                                //'#B90276',



                        },
                        color: '#4080AD',
                    }

                ],
                valueAxis: [


                    {

                        position: 'left',
                       
                        valueMarginsEnabled: false,
                        title: {
                            text: "HeadCount",
                           
                            font: {
                                color: "black",
                                family: "Arial",
                                size: 16
                            },
                        },
                        label: {

                            font: {
                                color: "black",
                                family: "Arial",
                                size: 14

                            },
                        },
                       

                    },

                ],
                tooltip: {
                    enabled: true,
                    location: "edge",

                    customizeTooltip: function (arg) {
                        //debugger;
                        //if (arg.percentText != null)
                        //    return { text: arg.point.data.Location + ":" + arg.valueText + "\n" + arg.point.series.name + ":" + arg.percentText }
                        //else
                        //    return { text: arg.point.data.Location + ":" + arg.valueText }
                        return {
                            text: "Click here to view HC " + arg.argument + " - " + arg.seriesName + " details"}

                    }
                },
                onDrawn: function (e) {
                    e.element.find(".dxc-series").hover(function () { $(this).css('cursor', 'pointer'); }, function () { $(this).css('cursor', 'auto'); });
                }  ,

                //export: {
                //    enabled: true,
                //    fileName: "HC_Chart"
                //},
                //tooltip: {
                //    enabled: true
                //},
                argumentAxis: {
                    label: {
                        
                        font: {
                            color: "black",
                            family: "Arial",
                            size: 14

                        },
                    },
                },
                onPointClick(e) {
                    //e.cellElement.css('color', '#0000');

                    //if (e.rowType === "header" || e.rowType === "filter") {
                    //    e.cellElement.css('font-weight', 'bold');

                    //    //e.cellElement.addClass("columnHeaderCSS");
                    //    //e.cellElement.find("input").addClass("columnHeaderCSS");
                    //}
                    //if (isFirstLevel) {
                    //    isFirstLevel = false;
                    //    $('#cards').css("display", "none");
                    //$('#hc_grid').css("display", "block");
                    //$('#hc_grid_title').css("display", "block");
                    
                        //removePointerCursor(chartContainer);
                        //$('#backButton')
                        //    .dxButton('instance')
                        //    .option('visible', true);
                    //}
                    //debugger;
                    isPlan_Utilize = e.target.series.name;
                    year_selected = e.target.argument;
                    fnHeadCount(isPlan_Utilize, year_selected);


                    //$.ajax({
                    //    type: "POST",
                    //    url: "../SLCockpit/HCChart_Drilldown",
                    //    dataType: 'json',
                    //    data: { 'isPlan_Utilize': isPlan_Utilize, 'year_selected': e.target.argument },
                    //    //traditional: true,
                    //    success: function (data) {
                    //        //debugger;
                    //        var res = JSON.parse(data.data.Data.Content);
                    //        $("#hc_grid").dxPivotGrid({
                    //            //allowSortingBySummary: true,
                    //            allowExpandAll: false,
                    //            allowSorting: false,
                    //            allowFiltering: true,
                    //            showBorders: true,
                    //            showColumnGrandTotals: false,
                    //            showRowGrandTotals: true,
                    //            showRowTotals: false,
                    //            showColumnTotals: false,
                    //            height: 700,
                    //            width: 600,
                    //            //columnWidth: 100,
                    //            headerFilter: { visible: true },
                    //            onCellPrepared: function (e) {

                    //                e.cellElement.css('color', '#0000');
                    //            },
                    //            fieldPanel: {
                    //                //showColumnFields: true,
                    //                //showDataFields: true,
                    //                showRowFields: true,
                    //                visible: true,
                    //                showFilterFields: true,
                    //                allowFieldDragging: true
                    //            },
                    //            fieldChooser: {
                    //                enabled: false
                    //                //applyChangesMode: "instantly"
                    //            },
                    //            export: {
                    //                enabled: true
                    //            },
                    //            scrolling: {
                    //                //mode: 'virtual',
                    //                columnRenderingMode: "virtual"
                    //            },
                    //            //paging: {
                    //            //    enabled: true,
                    //            //    pageSize: 10
                    //            //},
                    //            dataSource: {

                    //                fields: [{
                    //                    caption: "Role",
                    //                    width: 120,
                    //                    dataField: "Role",
                    //                    area: "row",
                    //                    expanded: true,

                    //                },
                    //                {
                    //                    //caption: "SkillSet",
                    //                    width: 250,
                    //                    dataField: "SkillSetName",
                    //                    area: "row",
                    //                    expanded: true
                    //                },

                    //                {
                    //                    dataField: "Year",
                    //                    area: "column",
                    //                    sortBy: "none",
                    //                    //dataType: "date",
                    //                    //customizeText: function (options) {
                    //                    //    return !options.value ? "" : moment(options.value).format("MMM/DD/YYYY").replace(".", "");
                    //                    //},
                    //                    expanded: true

                    //                },

                    //                {
                    //                    groupname: "Year",
                    //                    visible: false,
                    //                    expanded: true
                    //                },
                    //                {
                                        
                    //                    dataField: "Plan",
                    //                    caption: "Plan",
                    //                    dataType: "number",
                    //                    summaryType: "sum",
                    //                    area: "data",
                    //                    visible: function (e) {
                    //                        //debugger;
                    //                        if (isPlan_Utilize == "Plan")
                    //                            return true;
                    //                        else
                    //                            return false;
                    //                    },
                    //                    expanded: true,
                    //                },
                    //                {
                                       
                    //                    dataField: "Utilize",
                    //                    caption: "Utilize",
                    //                    dataType: "number",
                    //                    summaryType: "sum",
                    //                    area: "data",
                    //                    visible: function (e) {
                    //                        //debugger;
                    //                        if (isPlan_Utilize == "Utilize")
                    //                            return true;
                    //                        else
                    //                            return false;
                    //                    },
                    //                    expanded: true,
                    //                }
                    //                ],
                    //                store: res,
                    //                onFieldsPrepared: function (fields) {
                    //                    for (var i = 0; i < fields.length; i++) {
                    //                        if (fields[i].dataField != isPlan_Utilize && fields[i].area == "data")
                    //                            fields[i].visible = false;
                    //                    }
                    //                }, 
                    //            },

                    //        }).dxPivotGrid("instance");
                    //    }
                    //    ,
                    //    error: function (jqXHR, exception) {
                    //        //debugger;
                    //        var msg = '';
                    //        if (jqXHR.status === 0) {
                    //            msg = 'Not connect.\n Verify Network.';
                    //        } else if (jqXHR.status == 404) {
                    //            msg = 'Requested page not found. [404]';
                    //        } else if (jqXHR.status == 500) {
                    //            msg = 'Internal Server Error [500].';
                    //        } else if (exception === 'parsererror') {
                    //            msg = 'Requested JSON parse failed.';
                    //        } else if (exception === 'timeout') {
                    //            msg = 'Time out error.';
                    //        } else if (exception === 'abort') {
                    //            msg = 'Ajax request aborted.';
                    //        } else {
                    //            msg = 'Uncaught Error.\n' + jqXHR.responseText;
                    //        }
                    //        $('#post').html(msg);
                    //    },

                    //});
                }
            });


             $('#backButton').dxButton({
                 text: 'Back',
                 icon: 'chevronleft',
                 visible: false,
                 onClick() {
                     debugger;
                     //debugger;
                     //if (!isFirstLevel) {
                     //    isFirstLevel = true;
                         //addPointerCursor(chartContainer);
                        // chart_hc.option({ dataSource: hcchart_data });
                         //$("#hc_chart").dxChart({
                         //    dataSource: hcchart_data
                         //});
                         //chart_hc.option(
                         //    'dataSource', hcchart_data
                         //);
                        //$('#hc_grid').css("display", "none");
                        // $('#cards').css("display", "block");
                        
                    



                     document.getElementById("cards").style.display = "block";
                     document.getElementById("DeliveryStatus").style.display = "none";
                     document.getElementById("chartdiv_infra").style.display = "none";
                     //document.getElementById("hc_grid").style.display = "none";
                     //document.getElementById("hc_grid_title").style.display = "none";
                     document.getElementById("breadcrumb_nav1").style.display = "block";
                     document.getElementById("breadcrumb_nav2").style.display = "none";
                     document.getElementById("twoyr_view").style.display = "block";
                     document.getElementById("costelement_view").style.display = "none";
                     document.getElementById("category_view").style.display = "none";
                     document.getElementById("item_view").style.display = "none";
                     document.getElementById("threeyr_view").style.display = "none";
                     var bd1 = document.getElementById("item_nav");
                     bd1.style.color = "grey";
                     var bd2 = document.getElementById("costelement_nav");
                     bd2.style.color = "grey";
                     var bd3 = document.getElementById("category_nav");
                     bd3.style.color = "grey";
                     var bd4 = document.getElementById("twoyr_nav");
                     bd4.style.color = "black";
                     var bd5 = document.getElementById("threeyr_nav");
                     bd5.style.color = "grey";
                     this.option('visible', false);
                     //}
                 },
             });
            //addPointerCursor(chartContainer);
            //function addPointerCursor(container) {
            //    container.addClass('pointer-on-bars');
            //}

            //function removePointerCursor(container) {
            //    container.removeClass('pointer-on-bars');
            //}


        }
        ,
        error: function (jqXHR, exception) {
            //debugger;
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
