function GetPOdata() {
    debugger;
    var sdate = $("#startDate_po").val();
    var edate = $("#endDate_po").val();
    var view = getValue('PO_ddlView');


    $.ajax({
        type: "POST",
        //contentType: "application/json; charset=utf-8",
        url: "../SLCockpit/GetPO_Totals",
        dataType: 'json',
        data: { /*'year': filtered_yr,*/ 'SDate': sdate, 'EDate': edate, 'Views': view },
        //traditional: true,
        success: function (data) {
            debugger;
            var res = JSON.parse(data.data.Data.Content);
            if (res[0].VALUE == "")
                res[0].VALUE = '-';
            else
                res[0].VALUE = res[0].VALUE + " Nos";
           
            if (res[1].VALUE == "")
                res[1].VALUE = '-';
            else
                res[1].VALUE = res[1].VALUE + " Nos";
            
            document.getElementById('PO_Total').innerHTML = res[0].VALUE;
            document.getElementById('PO_OTD').innerHTML = res[1].VALUE;


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


    //document.getElementById("PO_Total").addEventListener("click", function () {
    //    debugger;
    //    //fnGetDeliveryStatus();
    //    //var sdate = $("#startDate_po").val();
    //    //var edate = $("#endDate_po").val();
    //    //<a href="@Url.Action(" WeeklyAverageCharts_SetUpType", "CPC" , new { Loc = "IN"})",=,
    //    //    target = "_blank" id = "nav_graph" class="nav-link" title = "Click here for Actual Delivery vs Requested Graphical view" style = "cursor:pointer" >
    //    //        RBEI Weekly Average Utilization
    //    //            </a >

    //}, false);
}


//function fnGetDeliveryStatus() {
//    debugger;
   
//    var sdate = $("#startDate_po").val();
//    var edate = $("#endDate_po").val();
//    //var newUrl = '@Url.Action("SDate","sdate")'; 
//    //var url = '@Html.Raw(Url.Action("GetDeliveryStatus", "SLCockpit", new { SDate = "_sdate", EDate = "_edate"}))';
//    //var params = url.replace('_sdate', sdate).replace('_edate', edate);
//    //window.location.href = params;
//    //window.open(params, '_blank');

//    $.ajax({
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//        url: "/SLCockpit/GetDeliveryStatus",
//        datatype: "json",
//        data: JSON.stringify({ SDate: sdate, EDate: edate }),
//        //async: true,
//        success: function (data) {
//            ////debugger;

//            //if (data.data.Data.Content.length > 0) {
//                debugger;
//                //var res = JSON.parse(data.data.Data.Content);
//                //var res = eval(data.data.Data.Content);
//                //$('#cards').css("display", "none");
//                //$('#DeliveryStatus').css("display", "block");
//                ////removePointerCursor(chartContainer);
//                //$('#backButton')
//                //    .dxButton('instance')
//                //    .option('visible', true);

//                //DeliveryStatus_Chart(res);
//                //location.href = '/SLCockpit/GetDeliveryStatus';
//                window.open('/SLCockpit/GetDeliveryStatus', '_blank');
//            //var url = '@Html.Raw(Url.Action("GetDeliveryStatus", "SLCockpit", new { sdate = "sdate", edate = "edate"}))';
//            //var params = url.replace('sdate', SDate).replace('edate', EDate);
//            //window.location.href = params;
//            //    $("#DeliveryStatus").html(data);
//            //}
//        }
//    });
//};

//function DeliveryStatus_Chart(data) {
//    debugger;


//    $("#DeliveryStatus").dxChart({

//        rotated: true,
//        width: 1000,
//        dataSource: data,
//        series: {
//            label: {
//                visible: true,
//                backgroundColor: '#c18e92',
//            },
//            color: '#79cac4',
//            type: 'bar',
//            argumentField: 'Description',
//            valueField: 'Value',
//        },
//        title: 'Actual delivery Vs Requested',
//        argumentAxis: {
//            label: {
//                //customizeText() {
//                //    return `Day ${this.valueText}`;
//                //},
//                visible: true,
//            },
//        },
//        valueAxis: {
//            //tick: {
//            //    visible: false,
//            //},
//            label: {
//                visible: false,
//            },
//        },
//        //export: {
//        //    enabled: true,
//        //},
//        legend: {
//            visible: false,
//        },
//    });
//}

function PurchaseOrderStatus_Chart(data) {
    debugger;
    $("#PurchaseStatus").dxChart({

        //rotated: true,
        dataSource: data,
        resolveLabelOverlapping: 'stack',
       // size: {
       //     height: 200,//150,//130,
      //      width: 410,
      //  },
        series: {
            label: {
                visible: true,
                backgroundColor:'#40AADB',
                    //'#009E60', //green
            },
            //color:  '#005691',
           // color: '#4080AD',//blue
           // color: '#e5369a', //pink
           // color: '#884ea0', //purple
            color: '#4080AD',
            type: 'bar',
            hoverMode: "allArgumentPoints",
            selectionMode: "allArgumentPoints",
            argumentField: 'Description',
            valueField: 'Value',
            barWidth: 30
        },
       // title: 'Purchase Order Details',
        argumentAxis: {
            label: {
                //customizeText() {
                //    return `Day ${this.valueText}`;
                //},
                visible: true,
                overlappingBehavior: "rotate",
                rotationAngle: 45,
                wordWrap: "none",
                font: {
                    color: "black",
                    family: "Arial",
                    size: 13

                },
                //wordWrap: 'none',
                //overlappingBehavior: "stagger",

            },
        },
        valueAxis: {
            //tick: {
            //    visible: false,
            //},
            label: {
                visible: true,
                font: { color: "black" }
            },
            title: {
                text: "Total PO",
                //font: { color: "black" },
                font: {
                    color: "black",
                    family: "Arial",
                    size: 13

                },
            },
        },
        //export: {
        //    enabled: true,
        //},
        legend: {
            visible: false,
        },
        tooltip: {
            enabled: true,
            location: "edge"
        }
    });
}


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




        debugger;
        document.getElementById("cards").style.display = "block";
        document.getElementById("DeliveryStatus").style.display = "none";
        document.getElementById("chartdiv_infra").style.display = "none";
        document.getElementById("hc_grid").style.display = "none";
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