$(document).ready(function () {

    document.querySelector(".loader-wrapper").style.display = "visible"
    document.getElementById('save').style.pointerEvents = 'none';
    document.querySelector('#card1').style.pointerEvents = 'none';
    document.querySelector('#card2').style.pointerEvents = 'none';

});

$
$('#startDate').change(function () {
    validatepage();
    document.getElementById('togglediv').style.display = 'none';
    document.getElementById('togglediv1').style.display = 'none';
    document.getElementById("triangle-up1").style.display = 'none';
    document.getElementById("triangle-up2").style.display = 'none';
    //document.getElementById('save').style.pointerEvents = 'auto';
});

$('#endDate').change(function () {
    validatepage();
    document.getElementById('togglediv').style.display = 'none';
    document.getElementById('togglediv1').style.display = 'none';
    document.getElementById("triangle-up1").style.display = 'none';
    document.getElementById("triangle-up2").style.display = 'none';
    //document.getElementById('save').style.pointerEvents = 'auto';

});

function validateDate() {
    // regular expression to match required date format
    re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
    //re = /^\d{4}-\d{1,2}-\d{1,2}$/;
    var startDate = document.getElementById('startDate').value;
    var endDate = document.getElementById('endDate').value;

    if (document.getElementById('startDate').value == '' || document.getElementById('endDate').value == '') {
        alert("Dates not selected ");
        document.getElementById('startDate').focus();
        return false;
    }

    if (document.getElementById('startDate').value != '' && !document.getElementById('startDate').value.match(re)) {
        alert("Invalid date format: " + document.getElementById('startDate').value);
        document.getElementById('startDate').focus();
        return false;
    }
    if (document.getElementById('endDate').value != '' && !document.getElementById('endDate').value.match(re)) {
        alert("Invalid time format: " + document.getElementById('endDate').value);
        document.getElementById('endDate').focus();
        return false;
    }
    var sDate = Date.parse(startDate);
    var eDate = Date.parse(endDate);

    if (sDate > eDate) {
        alert("Inappropriate Selection of Dates");
        $('.selectpicker').prop('disabled', true);
        $('.selectpicker').selectpicker('refresh');

    }
    else {
        $('.selectpicker').prop('disabled', false);
        $('.selectpicker').selectpicker('refresh');
    }
    return true;
}

function validatepage() {
    if ((validateDate()) && (document.getElementById('selectLabId').selectedIndex > 0)) {
        return true;
    }
    else {
        return false;
    }
}


function validatechart() {
    if (!(document.getElementById('selectLabId').selectedIndex != -1)) {
        alert("Input Parameters not selected")
    }
}



//function fnLabIDChange(labidselect) {
//    debugger;
//document.getElementById('togglediv').style.display = 'none';
//document.getElementById('togglediv1').style.display = 'none';
//document.getElementById("triangle-up1").style.display = 'none';
//document.getElementById("triangle-up2").style.display = 'none';


//document.querySelector('#card1').style.pointerEvents = 'auto';
//document.querySelector('#card2').style.pointerEvents = 'auto';
//    var laboptions = labidselect && labidselect.options;
//    var labopt;
//    for (var i = 0, iLen = laboptions.length; i < iLen; i++) {
//        labopt = laboptions[i];
//        if (labopt.selected) {

//            document.getElementById('selectLabId').value = document.getElementById('selectLabId').options[i].value;
//            document.getElementById('selectLabId').text = document.getElementById('selectLabId').options[i].text;
//        }
//    }
//document.querySelector('#save').style.pointerEvents = 'auto';

//}

function scrollToChart() {
    $('html,body').animate({
        scrollTop: $(".second").offset().top
    },
        'slow');
}
$(".datepicker").datepicker({
    changeMonth: true,
    changeYear: true
});

function fnOnclickTSOU(labidselect) {
    scrollToChart();
    debugger;
    
        document.querySelector('#card2').style.pointerEvents = 'auto';
        document.getElementById("spin1").style.display = "block";
        document.getElementById("chartdiv1").style.display = "none";
        var lab = [];
        var filtered = [];

        var obj = document.getElementById("selectLabId");
        var i;
        for (i = 1; i < obj.length; i++) {
            if (obj.options[i].value != 0) {
                filtered.push(obj.options[i].value);
            }

        }

        //var options = $('#selectLabId option');
        //var filtered = $.map(options, function (option) {
        //    return option.value;
        //});

        lab.StartDate = document.getElementById('startDate').value;
        lab.EndDate = document.getElementById('endDate').value;
        //for (i = 0; i < filtered.length; i++) {
        $.ajax({
            type: "POST",
            url: "/LabCar/TSOU_Chart/",
            data: JSON.stringify({ labIdvalue: filtered, startdate: lab.StartDate, enddate: lab.EndDate }),
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.success == true) {
                    generateReportChartTSOU(data);
                    //$.notify('TSOU Utlization Chart is retrieved succesfully.', {
                    //    globalPosition: "top center",
                    //    className: "success"
                    //});
                   
                }
                else {
                    document.getElementById("triangle-up2").style.display = 'none';
                    document.getElementById("togglediv").style.display = 'none';
                    document.getElementById("chartdiv1").style.display = "none";
                    $.notify('There are no Labs for selected configuration.', {
                        globalPosition: "top center",
                        className: "warn"
                    });
                }
            }, error: function (err) {
                
                //alert("Unable to display chart. Something went wrong.");
                //$("#mdbody").html("Unable to fetch TSOU Chart Data. Please Try Again.")
                //$('#myModal').modal('show');
                $.notify('Unable to fetch TSOU Chart Data. Please Try Again.', {
                    globalPosition: "top center",
                    className: "warn"
                });
            }
        });
        //}

    
   // }
}


debugger;
function fnOnclickTSIU() {
    if (document.getElementById('selectLabId').selectedIndex != -1 && document.getElementById('selectLabId').selectedIndex != 0) {
        $("#togglediv1").toggle();
        $("#triangle-up1").toggle();
        document.getElementById("triangle-up2").style.display = 'none';
        document.getElementById("togglediv").style.display = 'none';
        scrollToChart();
        if (!tsiuEnable) {
            document.getElementById("spin2").style.display = "block";
            document.getElementById("chartdiv2").style.display = "none";
            debugger;
            var lab = {};
            var tsiuid;
            lab.StartDate = document.getElementById('startDate').value;
            lab.EndDate = document.getElementById('endDate').value;


            tsiuid = document.getElementById('selectLabId').options[document.getElementById('selectLabId').selectedIndex].value;

            $.ajax({
                type: "POST",
                url: "/LabCar/TSIU_Chart/",
                data: JSON.stringify({ id: tsiuid, startdate: lab.StartDate, enddate: lab.EndDate }),
                async: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    debugger;
                    if (data.data.Data.Content.length > 0) {

                        debugger;
                        var res = JSON.parse(data.data.Data.Content);
                        generateReportChartTSIU(res);
                    }

                    //$.notify('TSIU Utlization Chart is retrieved succesfully.', {
                    //    globalPosition: "top center",
                    //    className: "success"
                    //});
                    tsiuEnable = false;
                }, error: function (err) {
                    tsiuEnable = false;
                    //alert("Unable to display chart. Something went wrong.");
                    //$("#mdbody").html("Unable to TSIU Chart Data. Please Try Again.")
                    //$('#myModal').modal('show');
                    $.notify('Unable to fetch TSIU Chart Data. Please Try Again.', {
                        globalPosition: "top center",
                        className: "warn"
                    });
                }
            });


            tsiuEnable = true;
        }
    } else {
        $.notify('Unable to fetch TSIU Chart Data. Please select the lab ID', {
            globalPosition: "top center",
            className: "warn"
        });
    }
}

document.getElementById("togglediv1").style.display = 'none';
document.getElementById("triangle-up1").style.display = 'none';
/*$('body').on("click touchstart", "#card1", function (e) {

    $("#togglediv1").toggle();
    $("#triangle-up1").toggle();
    document.getElementById("triangle-up2").style.display = 'none';
    document.getElementById("togglediv").style.display = 'none';
});*/
document.getElementById("togglediv").style.display = 'none';
document.getElementById("triangle-up2").style.display = 'none';
$('body').on("click touchstart", "#card2", function (e) {
    $("#togglediv").toggle();
    $("#triangle-up2").toggle();
    document.getElementById("triangle-up1").style.display = 'none';
    document.getElementById("togglediv1").style.display = 'none';
});
