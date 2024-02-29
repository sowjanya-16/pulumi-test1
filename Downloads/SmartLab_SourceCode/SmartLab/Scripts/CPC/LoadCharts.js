////////// TSG4
function RegAverageChart_TSG4(AvgRegionChart_TSG4, year) {
    debugger;
    var chartInstance = $("#locationchart_TSG4").dxChart({
        dataSource: AvgRegionChart_TSG4,
        size: {
            height: 500,
            width: 1500
        },
        
        resolveLabelOverlapping: 'stack',
        tooltip: {
                enabled: true
            },
        series: [
            {
                axis: "Total",   
                valueField: "CN",
                //tagField: "Location",
                name: "CN",
                type: "spline",
               // color: "green",                

                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.ETcnt_of_pc_instances;
                //        else
                //            return arg.point.data.ETcnt_of_pc_instances;
                //    }
                //},

            }, {
                axis: "Total",
                //tagField: "Location",
                valueField: "EU",
                name: "EU",
                type: "spline",
                //color: "red",
                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //        else
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //    }
                //},

            },
            {
                axis: "Total",
                valueField: "NA",
                //tagField: "Location",
                name: "NA",
                type: "spline",
                //color: "green",

                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.ETcnt_of_pc_instances;
                //        else
                //            return arg.point.data.ETcnt_of_pc_instances;
                //    }
                //},

            }, {
                axis: "Total",
                //tagField: "Location",
                valueField: "RBEI",
                name: "RBEI",
                type: "spline",
                //color: "red",
                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //        else
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //    }
                //},

            },
            {
                axis: "Total",
                valueField: "RBJP",
                //tagField: "Location",
                name: "RBJP",
                type: "spline",
                //color: "green",

                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.ETcnt_of_pc_instances;
                //        else
                //            return arg.point.data.ETcnt_of_pc_instances;
                //    }
                //},

            }, {
                axis: "Total",
               // tagField: "Location",
                valueField: "RBVH",
                name: "RBVH",
                type: "spline",
                //color: "red",
                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //        else
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //    }
                //},

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
                },
      visualRange: {
              startValue: 0,
             endValue: 100
        
    } ,
                constantLines: [{
                    value: 100,
                    color: '#fc3535',
                    dashStyle: 'dash',
                    width: 2,
                    label: { visible: false },
                    label: {
                        text: year + ' Target',
                    },
                }],
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

        title: {
            text: 'TSG4 Weekly Average Utilisation per Region (in hrs)',
        },

        commonSeriesSettings: {
            argumentField: "WeekNo",
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

        //crosshair: {
        //    enabled: true,
        //    color: "#949494",
        //    width: 3,
        //    dashStyle: "dot",
        //    label:
        //    {
        //        visible: true,
        //        backgroundColor: "#949494",
        //        font:
        //        {
        //            color: "#fff",
        //            size: 12
        //        }
        //    }
        //}


    }).dxChart("instance");

}

////////// PBOX
function RegAverageChart_PBOX(AvgRegionChart_PBOX, year) {
    debugger;
    var chartInstance = $("#locationchart_PBOX").dxChart({
        dataSource: AvgRegionChart_PBOX,
        size: {
            height: 500,
            width: 1500
        },

        resolveLabelOverlapping: 'stack',
        tooltip: {
            enabled: true
        },
        series: [
            {
                axis: "Total",
                valueField: "CN",
                //tagField: "Location",
                name: "CN",
                type: "spline",
                // color: "green",                

                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.ETcnt_of_pc_instances;
                //        else
                //            return arg.point.data.ETcnt_of_pc_instances;
                //    }
                //},

            }, {
                axis: "Total",
                //tagField: "Location",
                valueField: "EU",
                name: "EU",
                type: "spline",
                //color: "red",
                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //        else
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //    }
                //},

            },
            {
                axis: "Total",
                valueField: "NA",
                //tagField: "Location",
                name: "NA",
                type: "spline",
                //color: "green",

                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.ETcnt_of_pc_instances;
                //        else
                //            return arg.point.data.ETcnt_of_pc_instances;
                //    }
                //},

            }, {
                axis: "Total",
                //tagField: "Location",
                valueField: "RBEI",
                name: "RBEI",
                type: "spline",
                //color: "red",
                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //        else
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //    }
                //},

            },
            {
                axis: "Total",
                valueField: "RBJP",
                //tagField: "Location",
                name: "RBJP",
                type: "spline",
                //color: "green",

                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.ETcnt_of_pc_instances;
                //        else
                //            return arg.point.data.ETcnt_of_pc_instances;
                //    }
                //},

            }, {
                axis: "Total",
                // tagField: "Location",
                valueField: "RBVH",
                name: "RBVH",
                type: "spline",
                //color: "red",
                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //        else
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //    }
                //},

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
                },
                visualRange: {
                    startValue: 0,
                    endValue: 100

                },
                constantLines: [{
                    value: 100,
                    color: '#fc3535',
                    dashStyle: 'dash',
                    width: 2,
                    label: { visible: false },
                    label: {
                        text: year + ' Target',
                    },
                }],
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

        title: {
            text: 'PBOX Weekly Average Utilisation per Region (in hrs)',
        },

        commonSeriesSettings: {
            argumentField: "WeekNo",
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

        //crosshair: {
        //    enabled: true,
        //    color: "#949494",
        //    width: 3,
        //    dashStyle: "dot",
        //    label:
        //    {
        //        visible: true,
        //        backgroundColor: "#949494",
        //        font:
        //        {
        //            color: "#fff",
        //            size: 12
        //        }
        //    }
        //}


    }).dxChart("instance");

}

////////// MLC
function RegAverageChart_MLC(AvgRegionChart_MLC, year) {
    debugger;
    var chartInstance = $("#locationchart_MLC").dxChart({
        dataSource: AvgRegionChart_MLC,
        size: {
            height: 500,
            width: 1500
        },

        resolveLabelOverlapping: 'stack',
        tooltip: {
            enabled: true
        },
        series: [
            {
                axis: "Total",
                valueField: "CN",
                //tagField: "Location",
                name: "CN",
                type: "spline",
                // color: "green",                

                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.ETcnt_of_pc_instances;
                //        else
                //            return arg.point.data.ETcnt_of_pc_instances;
                //    }
                //},

            }, {
                axis: "Total",
                //tagField: "Location",
                valueField: "EU",
                name: "EU",
                type: "spline",
                //color: "red",
                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //        else
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //    }
                //},

            },
            {
                axis: "Total",
                valueField: "NA",
                //tagField: "Location",
                name: "NA",
                type: "spline",
                //color: "green",

                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.ETcnt_of_pc_instances;
                //        else
                //            return arg.point.data.ETcnt_of_pc_instances;
                //    }
                //},

            }, {
                axis: "Total",
                //tagField: "Location",
                valueField: "RBEI",
                name: "RBEI",
                type: "spline",
                //color: "red",
                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //        else
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //    }
                //},

            },
            {
                axis: "Total",
                valueField: "RBJP",
                //tagField: "Location",
                name: "RBJP",
                type: "spline",
                //color: "green",

                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.ETcnt_of_pc_instances;
                //        else
                //            return arg.point.data.ETcnt_of_pc_instances;
                //    }
                //},

            }, {
                axis: "Total",
                // tagField: "Location",
                valueField: "RBVH",
                name: "RBVH",
                type: "spline",
                //color: "red",
                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //        else
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //    }
                //},

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
                },
                visualRange: {
                    startValue: 0,
                    endValue: 100

                },
                constantLines: [{
                    value: 100,
                    color: '#fc3535',
                    dashStyle: 'dash',
                    width: 2,
                    label: { visible: false },
                    label: {
                        text: year + ' Target',
                    },
                }],
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

        title: {
            text: 'MLC Weekly Average Utilisation per Region (in hrs)',
        },

        commonSeriesSettings: {
            argumentField: "WeekNo",
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

        //crosshair: {
        //    enabled: true,
        //    color: "#949494",
        //    width: 3,
        //    dashStyle: "dot",
        //    label:
        //    {
        //        visible: true,
        //        backgroundColor: "#949494",
        //        font:
        //        {
        //            color: "#fff",
        //            size: 12
        //        }
        //    }
        //}


    }).dxChart("instance");

}

////////// ACUROT
function RegAverageChart_ACUROT(AvgRegionChart_ACUROT, year) {
    debugger;
    var chartInstance = $("#locationchart_ACUROT").dxChart({
        dataSource: AvgRegionChart_ACUROT,
        size: {
            height: 500,
            width: 1500
        },

        resolveLabelOverlapping: 'stack',
        tooltip: {
            enabled: true
        },
        series: [
            {
                axis: "Total",
                valueField: "CN",
                //tagField: "Location",
                name: "CN",
                type: "spline",
                // color: "green",                

                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.ETcnt_of_pc_instances;
                //        else
                //            return arg.point.data.ETcnt_of_pc_instances;
                //    }
                //},

            }, {
                axis: "Total",
                //tagField: "Location",
                valueField: "EU",
                name: "EU",
                type: "spline",
                //color: "red",
                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //        else
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //    }
                //},

            },
            {
                axis: "Total",
                valueField: "NA",
                //tagField: "Location",
                name: "NA",
                type: "spline",
                //color: "green",

                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.ETcnt_of_pc_instances;
                //        else
                //            return arg.point.data.ETcnt_of_pc_instances;
                //    }
                //},

            }, {
                axis: "Total",
                //tagField: "Location",
                valueField: "RBEI",
                name: "RBEI",
                type: "spline",
                //color: "red",
                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //        else
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //    }
                //},

            },
            {
                axis: "Total",
                valueField: "RBJP",
                //tagField: "Location",
                name: "RBJP",
                type: "spline",
                //color: "green",

                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.ETcnt_of_pc_instances;
                //        else
                //            return arg.point.data.ETcnt_of_pc_instances;
                //    }
                //},

            }, {
                axis: "Total",
                // tagField: "Location",
                valueField: "RBVH",
                name: "RBVH",
                type: "spline",
                //color: "red",
                //label: {
                //    visible: true,
                //    font: { color: 'black' },
                //    connector: {
                //        visible: true
                //    },
                //    customizeText: function (arg) {
                //        if (arg.point.tag != null)
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //        else
                //            return arg.point.data.CCSILcnt_of_pc_instances;
                //    }
                //},

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
                },
                visualRange: {
                    startValue: 0,
                    endValue: 100

                },
                constantLines: [{
                    value: 100,
                    color: '#fc3535',
                    dashStyle: 'dash',
                    width: 2,
                    label: { visible: false },
                    label: {
                        text: year + ' Target',
                    },
                }],
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

        title: {
            text: 'ACUROT Weekly Average Utilisation per Region (in hrs)',
        },

        commonSeriesSettings: {
            argumentField: "WeekNo",
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

        //crosshair: {
        //    enabled: true,
        //    color: "#949494",
        //    width: 3,
        //    dashStyle: "dot",
        //    label:
        //    {
        //        visible: true,
        //        backgroundColor: "#949494",
        //        font:
        //        {
        //            color: "#fff",
        //            size: 12
        //        }
        //    }
        //}


    }).dxChart("instance");

}



///////// RegionWise - TSG4
function LoadChart_TSG4(PrgAvgIN_TSG4_data, AverageTSG4_data, title_data, year) {


    debugger;
    $("#AvgchartIN_TSG4").dxChart({

        dataSource: PrgAvgIN_TSG4_data,
        resolveLabelOverlapping: 'stack',
        scrollBar: {
            visible: true
        },
        argumentAxis: {
            label: {
                wordWrap: 'none',
                overlappingBehavior: 'rotate',
            },
        },

        //zoomAndPan: {
        //    argumentAxis: "both",

        //},
        ////height: 50,
        size: {
            height: 500,
            width: 1500,
        },

        commonSeriesSettings: {
            argumentField: "Project",
            type: "bar",
            hoverMode: "allArgumentPoints",
            selectionMode: "allArgumentPoints",
            //color: "#1b4f72",
            //barPadding: 1.5,
        },


        series: [

            {
                valueField: "TAverage", name: "Prj Avg",
                label: {

                    visible: true,



                }
            },

            {
                valueField: "DAverage",
                axis: "spline_chart",
                type: "line",
                name: "DownTime",

            }


        ],

        valueAxis: [


                {
                    visualRange: {
                        startValue: 0,
                        endValue: 100,


                    },
                    position: 'left',
                    label: {
                        font: { color: "black" }
                    },
                    valueMarginsEnabled: false,
                    title: {
                        text: "Utilization Hours (in hrs)",
                        font: { color: "black" },
                    },
                    
                    constantLines: [{
                        value: AverageTSG4_data,
                        color: '#1b4f72',
                        dashStyle: 'dash',
                        width: 2,
                        label: { visible: false },
                        label: {
                            text: 'Avg'
                        },
                    }],
                },

                {
                    name: "spline_chart",
                    visualRange: {
                        startValue: 0,
                        endValue: 100,


                    },
                    position: "right",
                    grid: {
                        visible: true
                    },
                    constantLines: [{
                        value: 100,
                        color: '#006400',
                        dashStyle: 'dash',
                        width: 2,
                        label: {
                            text: year + ' Target',
                        }

                       
                    }],

                }
            ],
            title: {
                //text: 'RBEI Weekly Average Utilisation per Project (in hrs)',
                text: title_data,
            },
            export: {
                enabled: true,
                fileName: "Utilization"
        },

            tooltip: {
                enabled: true
            }
        });
}


///////// RegionWise - PBOX
function LoadChart_PBOX(PrgAvgIN_PBOX_data, AveragePBOX_data, title_data, year) {


    debugger;
    $("#AvgchartIN_PBOX").dxChart({

        dataSource: PrgAvgIN_PBOX_data,
        resolveLabelOverlapping: 'stack',
        scrollBar: {
            visible: true
        },
        argumentAxis: {
            label: {
                wordWrap: 'none',
                overlappingBehavior: 'rotate',
            },
        },

        //zoomAndPan: {
        //    argumentAxis: "both",

        //},
        ////height: 50,
        size: {
            height: 500,
            width: 1500,
        },

        commonSeriesSettings: {
            argumentField: "Project",
            type: "bar",
            hoverMode: "allArgumentPoints",
            selectionMode: "allArgumentPoints",
            //color: "#1b4f72",
            //barPadding: 1.5,
        },


        series: [

            {
                valueField: "TAverage", name: "Prj Avg",
                label: {

                    visible: true,



                }
            },

            {
                valueField: "DAverage",
                axis: "spline_chart",
                type: "line",
                name: "DownTime",

            }


        ],

        valueAxis: [


            {
                visualRange: {
                    startValue: 0,
                    endValue: 100,


                },
                position: 'left',
                label: {
                    font: { color: "black" }
                },
                valueMarginsEnabled: false,
                title: {
                    text: "Utilization Hours (in hrs)",
                    font: { color: "black" },
                },

                constantLines: [{
                    value: AveragePBOX_data,
                    color: '#1b4f72',
                    dashStyle: 'dash',
                    width: 2,
                    label: { visible: false },
                    label: {
                        text: 'Avg'
                    },
                }],
            },

            {
                name: "spline_chart",
                visualRange: {
                    startValue: 0,
                    endValue: 100,


                },
                position: "right",
                grid: {
                    visible: true
                },
                constantLines: [{
                    value: 100,
                    color: '#006400',
                    dashStyle: 'dash',
                    width: 2,
                    label: {
                        text: year + ' Target',
                    }


                }],

            }
        ],
        title: {
            //text: 'RBEI Weekly Average Utilisation per Project (in hrs)',
            text: title_data,
        },
        export: {
            enabled: true,
            fileName: "Utilization"
        },

        tooltip: {
            enabled: true
        }
    });
}

///////// RegionWise - MLC
function LoadChart_MLC(PrgAvgIN_MLC_data, AverageMLC_data, title_data, year) {


    debugger;
    $("#AvgchartIN_MLC").dxChart({

        dataSource: PrgAvgIN_MLC_data,
        resolveLabelOverlapping: 'stack',
        scrollBar: {
            visible: true
        },
        argumentAxis: {
            label: {
                wordWrap: 'none',
                overlappingBehavior: 'rotate',
            },
        },

        //zoomAndPan: {
        //    argumentAxis: "both",

        //},
        ////height: 50,
        size: {
            height: 500,
            width: 1500,
        },

        commonSeriesSettings: {
            argumentField: "Project",
            type: "bar",
            hoverMode: "allArgumentPoints",
            selectionMode: "allArgumentPoints",
            //color: "#1b4f72",
            //barPadding: 1.5,
        },


        series: [

            {
                valueField: "TAverage", name: "Prj Avg",
                label: {

                    visible: true,



                }
            },

            {
                valueField: "DAverage",
                axis: "spline_chart",
                type: "line",
                name: "DownTime",

            }


        ],

        valueAxis: [


            {
                visualRange: {
                    startValue: 0,
                    endValue: 100,


                },
                position: 'left',
                label: {
                    font: { color: "black" }
                },
                valueMarginsEnabled: false,
                title: {
                    text: "Utilization Hours (in hrs)",
                    font: { color: "black" },
                },

                constantLines: [{
                    value: AverageMLC_data,
                    color: '#1b4f72',
                    dashStyle: 'dash',
                    width: 2,
                    label: { visible: false },
                    label: {
                        text: 'Avg'
                    },
                }],
            },

            {
                name: "spline_chart",
                visualRange: {
                    startValue: 0,
                    endValue: 100,


                },
                position: "right",
                grid: {
                    visible: true
                },
                constantLines: [{
                    value: 100,
                    color: '#006400',
                    dashStyle: 'dash',
                    width: 2,
                    label: {
                        text: year + ' Target',
                    }


                }],

            }
        ],
        title: {
            //text: 'RBEI Weekly Average Utilisation per Project (in hrs)',
            text: title_data,
        },
        export: {
            enabled: true,
            fileName: "Utilization"
        },

        tooltip: {
            enabled: true
        }
    });
}

///////// RegionWise - ACUROT
function LoadChart_ACUROT(PrgAvgIN_ACUROT_data, AverageACUROT_data, title_data, year) {


    debugger;
    $("#AvgchartIN_ACUROT").dxChart({

        dataSource: PrgAvgIN_ACUROT_data,
        resolveLabelOverlapping: 'stack',
        scrollBar: {
            visible: true
        },
        argumentAxis: {
            label: {
                wordWrap: 'none',
                overlappingBehavior: 'rotate',
            },
        },

        //zoomAndPan: {
        //    argumentAxis: "both",

        //},
        ////height: 50,
        size: {
            height: 500,
            width: 1500,
        },

        commonSeriesSettings: {
            argumentField: "Project",
            type: "bar",
            hoverMode: "allArgumentPoints",
            selectionMode: "allArgumentPoints",
            //color: "#1b4f72",
            //barPadding: 1.5,
        },


        series: [

            {
                valueField: "TAverage", name: "Prj Avg",
                label: {

                    visible: true,



                }
            },

            {
                valueField: "DAverage",
                axis: "spline_chart",
                type: "line",
                name: "DownTime",

            }


        ],

        valueAxis: [


            {
                visualRange: {
                    startValue: 0,
                    endValue: 100,


                },
                position: 'left',
                label: {
                    font: { color: "black" }
                },
                valueMarginsEnabled: false,
                title: {
                    text: "Utilization Hours (in hrs)",
                    font: { color: "black" },
                },

                constantLines: [{
                    value: AverageACUROT_data,
                    color: '#1b4f72',
                    dashStyle: 'dash',
                    width: 2,
                    label: { visible: false },
                    label: {
                        text: 'Avg'
                    },
                }],
            },

            {
                name: "spline_chart",
                visualRange: {
                    startValue: 0,
                    endValue: 100,


                },
                position: "right",
                grid: {
                    visible: true
                },
                constantLines: [{
                    value: 100,
                    color: '#006400',
                    dashStyle: 'dash',
                    width: 2,
                    label: {
                        text: year + ' Target',
                    }


                }],

            }
        ],
        title: {
            //text: 'RBEI Weekly Average Utilisation per Project (in hrs)',
            text: title_data,
        },
        export: {
            enabled: true,
            fileName: "Utilization"
        },

        tooltip: {
            enabled: true
        }
    });
}




/////////////// Region VN
//function Load_VNChart(PrgAvgVN_data, AverageVN_data) {


//    debugger;
//    $("#AvgchartVN").dxChart({

//        dataSource: PrgAvgVN_data,
//        resolveLabelOverlapping: 'stack',
//        scrollBar: {
//            visible: true
//        },
//        argumentAxis: {
//            label: {
//                wordWrap: 'none',
//                overlappingBehavior: 'rotate',
//            },
//        },

//        //zoomAndPan: {
//        //    argumentAxis: "both",

//        //},
//        ////height: 50,
//        size: {
//            height: 500,
//            width: 1500,
//        },

//        commonSeriesSettings: {
//            argumentField: "Project",
//            type: "bar",
//            hoverMode: "allArgumentPoints",
//            selectionMode: "allArgumentPoints",
//            //barPadding: 5,
//            barWidth: 50,


//        },




//        series: [

//            {
//                valueField: "TAverage", name: "Prj Avg",
//                label: {

//                    visible: true,



//                }
//            },

//            {
//                valueField: "DAverage",
//                axis: "spline_chart",
//                type: "line",
//                name: "DownTime",

//            }


//        ],





//        valueAxis: [


//            {
//                visualRange: {
//                    startValue: 0,
//                    endValue: 100,


//                },
//                position: 'left',
//                label: {
//                    font: { color: "black" }
//                },
//                valueMarginsEnabled: false,
//                title: {
//                    text: "Utilization Hours (in hrs)",
//                    font: { color: "black" },
//                },

//                constantLines: [{
//                    value: AverageVN_data,
//                    color: '#1b4f72',
//                    dashStyle: 'dash',
//                    width: 2,
//                    label: { visible: false },
//                    label: {
//                        text: 'VN Avg'
//                    },
//                }],
//            },

//            {
//                name: "spline_chart",
//                visualRange: {
//                    startValue: 0,
//                    endValue: 100,


//                },
//                position: "right",
//                grid: {
//                    visible: true
//                },
//                constantLines: [{
//                    value: 100,
//                    color: '#006400',
//                    dashStyle: 'dash',
//                    width: 2,
//                    label: {
//                        text: '2021Target',
//                    }


//                }],

//            }
//        ],

//        title: {
//            text: 'VN Weekly Average Utilisation per Project (in hrs)',
//        },
//        export: {
//            enabled: true,
//            fileName: "Utilization"
//        },
//        tooltip: {
//            enabled: true
//        }
//    });
//}

/////////////// Region CN
//function Load_CNChart(PrgAvgCN_data, AverageCN_data) {


//    debugger;
//    $("#AvgchartCN").dxChart({

//        dataSource: PrgAvgCN_data,
//        resolveLabelOverlapping: 'stack',
//        scrollBar: {
//            visible: true
//        },
//        argumentAxis: {
//            label: {
//                wordWrap: 'none',
//                overlappingBehavior: 'rotate',
//            },
//        },

//        //zoomAndPan: {
//        //    argumentAxis: "both",

//        //},
//        ////height: 50,
//        size: {
//            height: 500,
//            width: 1500,
//        },

//        commonSeriesSettings: {
//            argumentField: "Project",
//            type: "bar",
//            hoverMode: "allArgumentPoints",
//            selectionMode: "allArgumentPoints",
//            //barPadding: 1.5,
//            barWidth: 50,
//        },




//        series: [

//            {
//                valueField: "TAverage", name: "Prj Avg",
//                label: {

//                    visible: true,



//                }
//            },

//            {
//                valueField: "DAverage",
//                axis: "spline_chart",
//                type: "line",
//                name: "DownTime",

//            }


//        ],





//        valueAxis: [


//            {
//                visualRange: {
//                    startValue: 0,
//                    endValue: 100,


//                },
//                position: 'left',
//                label: {
//                    font: { color: "black" }
//                },
//                valueMarginsEnabled: false,
//                title: {
//                    text: "Utilization Hours (in hrs)",
//                    font: { color: "black" },
//                },

//                constantLines: [{
//                    value: AverageCN_data,
//                    color: '#1b4f72',
//                    dashStyle: 'dash',
//                    width: 2,
//                    label: { visible: false },
//                    label: {
//                        text: 'CN Avg'
//                    },
//                }],
//            },

//            {
//                name: "spline_chart",
//                visualRange: {
//                    startValue: 0,
//                    endValue: 100,


//                },
//                position: "right",
//                grid: {
//                    visible: true
//                },
//                constantLines: [{
//                    value: 100,
//                    color: '#006400',
//                    dashStyle: 'dash',
//                    width: 2,
//                    label: {
//                        text: '2021Target',
//                    }


//                }],

//            }
//        ],

//        title: {
//            text: 'CN Weekly Average Utilisation per Project (in hrs)',
//        },
//        export: {
//            enabled: true,
//            fileName: "Utilization"
//        },
//        tooltip: {
//            enabled: true
//        }
//    });
//}


/////////////// Region JP
//function Load_JPChart(PrgAvgJP_data, AverageJP_data) {


//    debugger;
//    $("#AvgchartJP").dxChart({

//        dataSource: PrgAvgJP_data,
//        resolveLabelOverlapping: 'stack',
//        scrollBar: {
//            visible: true
//        },
//        argumentAxis: {
//            label: {
//                wordWrap: 'none',
//                overlappingBehavior: 'rotate',
//            },
//        },

//        //zoomAndPan: {
//        //    argumentAxis: "both",

//        //},
//        ////height: 50,
//        size: {
//            height: 500,
//            width: 1500,
//        },

//        commonSeriesSettings: {
//            argumentField: "Project",
//            type: "bar",
//            hoverMode: "allArgumentPoints",
//            selectionMode: "allArgumentPoints",
//            barWidth: 50,
//        },




//        series: [

//            {
//                valueField: "TAverage", name: "Prj Avg",
//                label: {

//                    visible: true,



//                }
//            },

//            {
//                valueField: "DAverage",
//                axis: "spline_chart",
//                type: "line",
//                name: "DownTime",

//            }


//        ],





//        valueAxis: [


//            {
//                visualRange: {
//                    startValue: 0,
//                    endValue: 100,


//                },
//                position: 'left',
//                label: {
//                    font: { color: "black" }
//                },
//                valueMarginsEnabled: false,
//                title: {
//                    text: "Utilization Hours (in hrs)",
//                    font: { color: "black" },
//                },

//                constantLines: [{
//                    value: AverageJP_data,
//                    color: '#1b4f72',
//                    dashStyle: 'dash',
//                    width: 2,
//                    label: { visible: false },
//                    label: {
//                        text: 'JP Avg'
//                    },
//                }],
//            },

//            {
//                name: "spline_chart",
//                visualRange: {
//                    startValue: 0,
//                    endValue: 100,


//                },
//                position: "right",
//                grid: {
//                    visible: true
//                },
//                constantLines: [{
//                    value: 100,
//                    color: '#006400',
//                    dashStyle: 'dash',
//                    width: 2,
//                    label: {
//                        text: '2021Target',
//                    }


//                }],

//            }
//        ],

//        title: {
//            text: 'JP Weekly Average Utilisation per Project (in hrs)',
//        },
//        export: {
//            enabled: true,
//            fileName: "Utilization"
//        },
//        tooltip: {
//            enabled: true
//        }
//    });
//}

/////////////// Region DE
//function Load_DEChart(PrgAvgDE_data, AverageDE_data) {


//    debugger;
//    $("#AvgchartDE").dxChart({

//        dataSource: PrgAvgDE_data,
//        resolveLabelOverlapping: 'stack',
//        scrollBar: {
//            visible: true
//        },
//        argumentAxis: {
//            label: {
//                wordWrap: 'none',
//                overlappingBehavior: 'rotate',
//            },
//        },

//        //zoomAndPan: {
//        //    argumentAxis: "both",

//        //},
//        ////height: 50,
//        size: {
//            height: 500,
//            width: 1500,
//        },

//        commonSeriesSettings: {
//            argumentField: "Project",
//            type: "bar",
//            hoverMode: "allArgumentPoints",
//            selectionMode: "allArgumentPoints",
//            barWidth: 50,
//        },




//        series: [

//            {
//                valueField: "TAverage", name: "Prj Avg",
//                label: {

//                    visible: true,



//                }
//            },

//            {
//                valueField: "DAverage",
//                axis: "spline_chart",
//                type: "line",
//                name: "DownTime",

//            }


//        ],





//        valueAxis: [


//            {
//                visualRange: {
//                    startValue: 0,
//                    endValue: 100,
//                    aggregationInterval: 10,

//                },
//                position: 'left',
//                label: {
//                    font: { color: "black" }
//                },
//                valueMarginsEnabled: false,
//                title: {
//                    text: "Utilization Hours (in hrs)",
//                    font: { color: "black" },
//                },

//                constantLines: [{
//                    value: AverageDE_data,
//                    color: '#1b4f72',
//                    dashStyle: 'dash',
//                    width: 2,
//                    label: { visible: false },
//                    label: {
//                        text: 'DE Avg'
//                    },
//                }],
//            },

//            {
//                name: "spline_chart",
//                visualRange: {
//                    startValue: 0,
//                    endValue: 100,
//                    aggregationInterval: 10,

//                },
//                position: "right",
//                grid: {
//                    visible: true
//                },
//                constantLines: [{
//                    value: 100,
//                    color: '#006400',
//                    dashStyle: 'dash',
//                    width: 2,
//                    label: {
//                        text: '2021Target',
//                    }


//                }],

//            }
//        ],

//        title: {
//            text: 'DE Weekly Average Utilisation per Project (in hrs)',
//        },
//        export: {
//            enabled: true,
//            fileName: "Utilization"
//        },
//        tooltip: {
//            enabled: true
//        }
//    });
//}


/////////////// Region US
//function Load_USChart(PrgAvgUS_data, AverageUS_data) {


//    debugger;
//    $("#AvgchartUS").dxChart({

//        dataSource: PrgAvgUS_data,
//        resolveLabelOverlapping: 'stack',
//        scrollBar: {
//            visible: true
//        },
//        argumentAxis: {
//            label: {
//                wordWrap: 'none',
//                overlappingBehavior: 'rotate',
//            },
//        },

//        //zoomAndPan: {
//        //    argumentAxis: "both",

//        //},
//        ////height: 50,
//        size: {
//            height: 500,
//            width: 1500,
//        },

//        commonSeriesSettings: {
//            argumentField: "Project",
//            type: "bar",
//            hoverMode: "allArgumentPoints",
//            selectionMode: "allArgumentPoints",
//            barWidth: 50,
//        },




//        series: [

//            {
//                valueField: "TAverage", name: "Prj Avg",
//                label: {

//                    visible: true,



//                }
//            },

//            {
//                valueField: "DAverage",
//                axis: "spline_chart",
//                type: "line",
//                name: "DownTime",

//            }


//        ],





//        valueAxis: [


//            {
//                visualRange: {
//                    startValue: 0,
//                    endValue: 100,
//                    tickInterval: 10,

//                },

//                position: 'left',
//                label: {
//                    font: { color: "black" }
//                },
//                valueMarginsEnabled: false,
//                title: {
//                    text: "Utilization Hours (in hrs)",
//                    font: { color: "black" },
//                },

//                constantLines: [{
//                    value: AverageUS_data,
//                    color: '#1b4f72',
//                    dashStyle: 'dash',
//                    width: 2,
//                    label: { visible: false },
//                    label: {
//                        text: 'US Avg'
//                    },
//                }],
//            },

//            {
//                name: "spline_chart",
//                visualRange: {
//                    startValue: 0,
//                    endValue: 100,
                    

//                },

//                position: "right",
//                grid: {
//                    visible: true
//                },
//                constantLines: [{
//                    value: 100,
//                    color: '#006400',
//                    dashStyle: 'dash',
//                    width: 2,
//                    label: {
//                        text: '2021Target',
//                    }


//                }],

//            }
//        ],

//        title: {
//            text: 'US Weekly Average Utilisation per Project (in hrs)',
//        },
//        export: {
//            enabled: true,
//            fileName: "Utilization"
//        },
//        tooltip: {
//            enabled: true
//        }
//    });
//}

          
    




