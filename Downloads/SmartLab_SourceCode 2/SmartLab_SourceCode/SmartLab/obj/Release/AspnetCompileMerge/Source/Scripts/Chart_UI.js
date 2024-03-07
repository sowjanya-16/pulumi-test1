
$(document).ready(function () {
      
    document.querySelector(".loader-wrapper").style.display = "visible"
    document.getElementById('save').style.pointerEvents = 'none';
    document.querySelector('#card1').style.pointerEvents = 'none';
    document.querySelector('#card2').style.pointerEvents = 'none';
    
    $.ajax({
        type: "GET",
        url: "/Dashboard/TSOU_Chart",
        datatype: "json",
        async: true,
        success: function (data) {
            
            getLabIDs(data);

        }
    });
});

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
    if ((validateDate()) && (document.getElementById('selectLabId').selectedIndex > 0))   {
        return true;
    }
    else {       
        return false;
    }
}

function fnLabIDChange(labidselect) {
    
    document.getElementById('togglediv').style.display = 'none';
    document.getElementById('togglediv1').style.display = 'none';
    document.getElementById("triangle-up1").style.display = 'none';
    document.getElementById("triangle-up2").style.display = 'none';

    if (document.getElementById('selectLabId').selectedIndex > 0) {
        LCLabID.value = "";
        LCLabID.text = "";
    }
    document.querySelector('#card1').style.pointerEvents = 'auto';
    document.querySelector('#card2').style.pointerEvents = 'auto';
    
    var laboptions = labidselect && labidselect.options;
    var labopt;
    for (var i = 0, iLen = laboptions.length; i < iLen; i++) {
        labopt = laboptions[i];
        if (labopt.selected) {
            
            LCLabID.value += document.getElementById('selectLabId').options[i].value + "|";
            LCLabID.text += document.getElementById('selectLabId').options[i].text + "|";            
        }
    }
    document.querySelector('#save').style.pointerEvents = 'auto';

}



function validatechart() {
    if (!(document.getElementById('selectLabId').selectedIndex != -1)) {
        alert("Input Parameters not selected")
    }
}

// Save Function
function saveAsTextFile(selectid) {
    
    var selLabIds = LCLabID.value;
    var split_labs = selLabIds.split('|');
    var filtered = split_labs.filter(function (el) { return el; });
    var data = { StartDate: startdate, EndDate: enddate, LabID: filtered };
    var json = JSON.stringify(data);
    const textToSaveBlob = new Blob([json], { type: "application/json" });
    const sFileName = 'Dashboard_Attributes.json';
    let newLink = document.createElement("a");
    newLink.download = sFileName;
    if (window.webkitURL != null) {
        newLink.href = window.webkitURL.createObjectURL(textToSaveBlob);
    }
    else {
        newLink.href = window.URL.createObjectURL(textToSaveBlob);
        newLink.style.display = "none";
        document.body.appendChild(newLink);
    }

    newLink.click();
}

// Read Json file
function checkFileAPI() {
    if (window.File && window.FileReader && window.FileList && window.Blob) {
        reader = new FileReader();
        return true;
    } else {
        alert('The File APIs are not fully supported by your browser. Fallback required.');
        return false;
    }
}

function readText(filePath) {
    
    if (document.getElementById('selectLabId').selectedIndex > 0) {
        document.getElementById('selectLabId').value = "";
        document.getElementById('selectLabId').text = "";
    }

    
    document.getElementById('togglediv').style.display = 'none';
    document.getElementById('togglediv1').style.display = 'none';
    document.getElementById("triangle-up1").style.display = 'none';
    document.getElementById("triangle-up2").style.display = 'none';
    var output = "";
    if (filePath.files && filePath.files[0]) {
        reader.onload = function (e) {
            output = e.target.result;
            jsondata = JSON.parse(output);
            
            document.getElementById('startDate').value = jsondata.StartDate;
            document.getElementById('endDate').value = jsondata.EndDate;
            var LabIDs = jsondata.LabID;
            for (var i = document.getElementById('selectLabId').options.length - 1; i >= 0; i--) {
                var jsondata;

                if (document.getElementById('selectLabId').value.indexOf(document.getElementById('selectLabId').options[i].value) != 0)
                    if (LabIDs.indexOf(document.getElementById('selectLabId').options[i].value) != -1) {
                        document.getElementById('selectLabId').options[i].selected = true;
                        document.getElementById('selectLabId').options[0].selected = false;
                        $('#selectLabId').selectpicker('refresh');

                    }
                    else {
                        document.getElementById('selectLabId').options[i].selected = false;
                        $('#selectLabId').selectpicker('refresh');
                    }

            }
                       
            document.getElementById('save').style.pointerEvents = 'auto';
            document.querySelector('#card1').style.pointerEvents = 'auto';
            document.querySelector('#card2').style.pointerEvents = 'auto';

        };
        reader.readAsText(filePath.files[0]);
    }
    else if (ActiveXObject && filePath) {
        try {
            reader = new ActiveXObject("Scripting.FileSystemObject");
            var file = reader.OpenTextFile(filePath, 1);
            output = file.ReadAll();
            file.Close();
            displayContents(output);
        } catch (e) {
            if (e.number == -2146827859) {
                alert('Unable to access local files due to browser security settings. ' +
                    'To overcome this, go to Tools->Internet Options->Security->Custom Level. ' +
                    'Find the setting for "Initialize and script ActiveX controls not marked as safe" and change it to "Enable" or "Prompt"');
            }
        }
    }
    else { //this is where you could fallback to Java Applet, Flash or similar
        return false;
    }
    return true;
}

$(function () {

    $("#startDate").datepicker();
    $("#endDate").datepicker();

});


function fnOnclickTSOU(labidselect) {
    if (!tsouEnable) {
        document.getElementById("spin1").style.display = "block";
        document.getElementById("chartdiv1").style.display = "none";
        var lab = [];
        var filtered = $('#selectLabId').val();
        lab.StartDate = document.getElementById('startDate').value;
        lab.EndDate = document.getElementById('endDate').value;

        $.ajax({
            type: "POST",
            url: "/Dashboard/TSOU_Chart/",
            data: JSON.stringify({ labIdvalue: filtered, startdate: lab.StartDate, enddate: lab.EndDate }),
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                generateChartTSOU(data);
                $.notify('TSOU Utlization Chart is retrieved succesfully.', {
                    globalPosition: "top center",
                    className: "success"
                });
                tsouEnable = false;
            }, error: function (err) {
                tsouEnable = false;
                //alert("Unable to display chart. Something went wrong.");
                $.notify('Unable to fetch TSOU Chart Data. Please Try Again.', {
                    globalPosition: "top center",
                    className: "warn"
                }); 
            }
        });
        tsouEnable = true;
    }
}

function fnOnclickTSIU() {
    debugger;
        var filtered = $('#selectLabId').val();
        var selLabIdnames = $('#selectLabId option:selected').toArray().map(item => item.text).join();
        var split_names = selLabIdnames.split(',');
        var filtered_names = split_names.filter(function (el) { return el; });
        var selectedLabIDs = document.getElementById("selectid");

        selectedLabIDs.innerHTML = "";
        var options = "";
        for (var i = 0; i < filtered.length; i++) {
            var opt = filtered[i];
            var val = filtered_names[i];
            options += "<option value=\"" + opt + "\">" + val + "</option>";
        }
        selectedLabIDs.innerHTML = options;
        $('#selectid').selectpicker('refresh');
        fnTSIUIDChange();
        
    
}

function fnTSIUIDChange() {
    debugger;
    if (!tsiuEnable) {
    document.getElementById("spin2").style.display = "block";
    document.getElementById("chartdiv2").style.display = "none";
   
        var lab = {};
        var tsiuid;
        lab.StartDate = document.getElementById('startDate').value;
        lab.EndDate = document.getElementById('endDate').value;
        if (document.getElementById('selectid').selectedIndex != -1) {

            tsiuid = document.getElementById('selectid').options[document.getElementById('selectid').selectedIndex].value;
        }

        $.ajax({
            type: "POST",
            url: "/Dashboard/TSIU_Chart/",
            data: JSON.stringify({ id: tsiuid, startdate: lab.StartDate, enddate: lab.EndDate }),
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.success==false)
                {
                    $.notify('Unable to fetch TSIU Chart Data. Please Try Again.', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                    document.getElementById('togglediv').style.display = 'none';
                    document.getElementById('togglediv1').style.display = 'none';
                    document.getElementById("triangle-up1").style.display = 'none';
                    document.getElementById("triangle-up2").style.display = 'none';
                }
                else(data.data.Data.Content.length > 0)
                {

                    debugger;
                    var res = JSON.parse(data.data.Data.Content);
                    generateChartTSIU(res);
                    $.notify('TSIU Utlization Chart is retrieved succesfully.', {
                        globalPosition: "top center",
                        className: "success"
                    });
                }              

               
                tsiuEnable = false;
            }, error: function (err) {
                tsiuEnable = false;
                //alert("Unable to display chart. Something went wrong.");
                $.notify('Unable to fetch TSIU Chart Data. Please Try Again.', {
                    globalPosition: "top center",
                    className: "warn"
                });
            }
        });
        tsiuEnable = true;
    }
}
document.getElementById("togglediv1").style.display = 'none';
document.getElementById("triangle-up1").style.display = 'none';
$('body').on("click touchstart", "#card1", function (e) {

    $("#togglediv1").toggle();
    $("#triangle-up1").toggle();
    document.getElementById("triangle-up2").style.display = 'none';
    document.getElementById("togglediv").style.display = 'none';
});
document.getElementById("togglediv").style.display = 'none';
document.getElementById("triangle-up2").style.display = 'none';
$('body').on("click touchstart", "#card2", function (e) {
    $("#togglediv").toggle();
    $("#triangle-up2").toggle();
    document.getElementById("triangle-up1").style.display = 'none';
    document.getElementById("togglediv1").style.display = 'none';    
});

