 //Javascript library for YoY Charts - mae9cob

$(document).ready(function () {
    //Initially disable/hide the buttons
    $("#selectSite1").prop('disabled', false);
    $("#gen1").prop('disabled', true);
    $("#export").prop('hidden', true);
    $("#export_multiplecharts").prop('hidden', true);
    
});

// Add Sites chosen by user to sites_chosen array
var sites_chosen = [];
//Onchange function of Site
debugger;
function fnSitesChange1(siteselect) {
    debugger; 
    sites_chosen = [];

    for (var i = 0, len = siteselect.options.length; i < len; i++) {
        if (document.getElementById('selectSite1').selectedIndex != -1) {
            options = siteselect.options;
            opt1 = options[i];
            if (opt1.selected) {
                //store the sites chosen by user from dropdown to process the relevant chart data
                sites_chosen.push(opt1.value);
            }
        }
    }

    //Once user selects the sites, Generate Charts-filter button enabled
    $("#gen1").prop('disabled', false);
}



//Onclick of Generate Chart button
$("#gen1").click(function () {
    debugger;
    //Hide Chart region and Export to excel icon till the new data is processed and prepared for chart
    $("#chartContainer1").prop('hidden', true);
    $("#pieChartContainer1").prop('hidden', true);
    $("#export").prop('hidden', true);
    $("#export_multiplecharts").prop('hidden', true);

   
    var genText1 = document.querySelector("#genText1");
    var genSpinner1 = document.querySelector("#genSpinner1");   
    var str = "Please Wait, Fetching Data...  ";
    var result = str.bold();
    genText1.innerHTML = result;

    
    genSpinner1.classList.add('fa');
    genSpinner1.classList.add('fa-spinner');
    genSpinner1.classList.add('fa-pulse');

    //Notification for user that navigation to other location/website/path is not required and the Chart will be displayed here
    $.notify("Expected Charts will be loaded here", {
        className: "success",
        globalPosition: "top center",
        autoHideDelay: 7000
    });
    

    //Pass the user chosen sites to extract the data relevant for the chart
    debugger;
    $.ajax({
        type: "POST",
        url: encodeURI("../Diagnostics/GetYoY_LCTypeData"),
        data: { 'location_to_rbcode': sites_chosen },
        success: OnSuccess_YoYcharts,
        error: OnErrorCall_YoYcharts
    });
});
var objdata;

function OnSuccess_YoYcharts(lc_Type_stats) 
{
    debugger;
    var pie_data = [];
    debugger;
    //code to generate array of objects to assign the datasource for piechart generation
    for (i = 1; i < lc_Type_stats.data.length; i++) {
            
        if (lc_Type_stats.data[i - 1].Year != lc_Type_stats.data[i].Year)
            pie_data.push({ ET_yearcnt: lc_Type_stats.data[i - 1].et_cnt_yr, CCSIL_yearcnt: lc_Type_stats.data[i - 1].ccsil_cnt_yr, Year_Location: lc_Type_stats.data[i - 1].Year + " " + lc_Type_stats.data[i - 1].Location, Location: lc_Type_stats.data[i - 1].Location, Year: lc_Type_stats.data[i - 1].Year });

        if (i == (lc_Type_stats.data.length - 1))
            pie_data.push({ ET_yearcnt: lc_Type_stats.data[i].et_cnt_yr, CCSIL_yearcnt: lc_Type_stats.data[i].ccsil_cnt_yr, Year_Location: lc_Type_stats.data[i].Year + " " + lc_Type_stats.data[i].Location, Location: lc_Type_stats.data[i].Location, Year: lc_Type_stats.data[i].Year });



        if (i == 1)
            location_filename = lc_Type_stats.data[i].Location;

        if (lc_Type_stats.data[i - 1].Location != lc_Type_stats.data[i].Location) {
            location_filename = location_filename + "_" + lc_Type_stats.data[i].Location;
        }
        debugger;
    }
    debugger;


    objdata = (lc_Type_stats.data);
    var str = "Generate Charts";
    var result = str.bold();
    genText1.innerHTML = result;
    $("#genSpinner1").removeClass("fa fa-spinner fa-spin");
    $("#chartContainer1").prop('hidden', false);
    $("#pieChartContainer1").prop('hidden', false);
   

     

    var chartInstance = $("#chartContainer1").dxChart({
            dataSource: objdata,           
            resolveLabelOverlapping: 'stack',
            series: [               
                {
                    axis: "Total",
                    valueField: "ETcnt_of_pc_instances",
                    tagField: "Location",
                    name: "ET Count",
                    type: "spline",
                    color: "green",
                    
                    label: {
                        visible: true,
                        font: { color: 'black' },
                        connector: {
                            visible: true
                        },
                        customizeText: function (arg) {
                            if (arg.point.tag != null)
                                return arg.point.data.ETcnt_of_pc_instances;
                            else
                                return arg.point.data.ETcnt_of_pc_instances;
                        }
                    },          

                }, {
                    axis: "Total",
                    tagField: "Location",
                    valueField: "CCSILcnt_of_pc_instances",
                    name: "CCSIL Count",
                    type: "spline",
                    color: "red",
                    label: {
                        visible: true,
                        font: { color: 'black' },
                        connector: {
                            visible: true
                        },
                        customizeText: function (arg) {
                            if (arg.point.tag != null)
                                return arg.point.data.CCSILcnt_of_pc_instances;
                            else
                                return arg.point.data.CCSILcnt_of_pc_instances;
                        }
                    },          
                    
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

            valueAxis: [               
                {
                    name: "Total",
                    grid: { visible: true },
                    title: {
                        text: "LC Type Count"
                    }
                }

            ],
            legend:
            {
                verticalAlignment: "Bottom",
                horizontalAlignment: "Center",
                border: {
                    visible: true
                }
            },
      
            commonSeriesSettings: {
                argumentField: "Month_Location",
            },          
            export: {
                enabled: true
            },
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
            
            crosshair: {
                enabled: true,
                color: "#949494",
                width: 3,
                dashStyle: "dot",
                label:
                {
                    visible: true,
                    backgroundColor: "#949494",
                    font:
                    {
                        color: "#fff",
                        size: 12
                    }
                }
        }
           
            
    }).dxChart("instance");




    //Show Export to excel icon once file is ready to download
    $("#export").prop('hidden', false);
    



    debugger;
    var pieChartInstance = $("#pieChartContainer1").dxPieChart({
        dataSource: pie_data,

        title: "LC Type Percentage Stats",
        commonAnnotationSettings: {
            type: "text"
            
        },
        
        size: {
            height: 700,
            width: 800
        },
        resolveLabelOverlapping: 'shift',
        commonSeriesSettings: {
            argumentField: "Year_Location", 
        }, 
        series: [{
            
            valueField: "CCSIL_yearcnt",            
            name: "CCSIL Percentage Ratio",
            label: {
                visible: true,
                font: {
                    size: 16
                },
                connector: {
                    visible: true,
                    width: 0.5
                },
                position: "columns",
                customizeText: function (arg) {
                    return "CCSIL" + ":" + arg.valueText + " (" + arg.percentText + ")";
                }
            }
           
        },
            {
            valueField: "ET_yearcnt",
            name: "ET Percentage Ratio", //shown in series name while hover over chart segments
            label: {
                visible: true,
                font: {
                    size: 16
                },
                connector: {
                    visible: true,
                    width: 0.5
                },
                position: "columns",
                customizeText: function (arg) {
                    return "ET" + ":" +  arg.valueText + " (" + arg.percentText + ")";
                }
            }

                      
        }],
        tooltip: {
            enabled: true,
            location: "edge",
            customizeTooltip: function (arg) {
                debugger;
                if (arg.percentText != null)
                    return { text: arg.point.data.Location + ":" + arg.valueText + "\n" + arg.point.series.name + ":" + arg.percentText }
                else
                    return { text: arg.point.data.Location + ":" + arg.valueText }
                debugger;

            }
        },
        legend:
        {
            verticalAlignment: "Bottom",
            horizontalAlignment: "Center",
            border: {
                visible: true
            }
        },
        export: {
            enabled: true
        }
        
    }).dxPieChart("instance");
    debugger;
    $("#export_multiplecharts").prop('hidden', false);
    
    
    //The DevExpress.viz.exportWidgets(widgetInstances, options) method allows you to export several charts to a single image or document.
    //The widgetInstances parameter accepts an array - Each nested array contains UI component instances that should be in the same row in the exported document.
    //The options parameter accepts an object whose fields allow you to configure export properties.
    $("#export_multiplecharts").dxButton({ 
        icon: "export",
        text: "Export charts",
        type: "default",
        width: 165,
        onClick: function () {
            DevExpress.viz.exportWidgets([[chartInstance], [pieChartInstance]], {
                fileName: "LCType Charts",
                format: 'PDF'
            });
        }
    });
     
}
function OnErrorCall_YoYcharts(counts) {
    var str = "Try Again";
    var result = str.bold();
    genText1.innerHTML = result;
    $("#genSpinner1").removeClass("fa fa-spinner fa-spin");
    $.notify('Error in YoY LC Type', {
        globalPosition: "top center",
        className: "warn"
    });
    
} 




//Export chart data to Excel - on click of excel icon 
$("#export").click(function () {
    $.ajax({

        type: "POST",
        url: "/Diagnostics/ExportYoYDetailsToExcel/",
        data: { 'excel_obj': objdata },


        success: function (export_result) {
           
            var bytes = new Uint8Array(export_result.FileContents);
            var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = export_result.FileDownloadName + "_" + location_filename + "_" + ".xlsx";
            link.click();

        },
        error: function () {
            alert("error");
        }

    });
});
//end: Export chart to Excel 












                                        