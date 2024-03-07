            //Javascript file for Budgeting Request Details - mae9cob    


var dataGridLEP, busummarytable, deptsummarytable;
var dataObjData, newobjdata;
var BU_list, Item_list, Category_list, OEM_list, Group_list, CostElement_list, OrderStatus_list, BudgetCodeList;
var Selected = [];
var unitprice, reviewer_2, category, costelement, leadtime, reviewer_1, BudgetCode;
var lookup_data, new_request;
var filtered_yr;
var leadtime1;
var genSpinner_load = document.querySelector("#load");
var countdownflag = false;
var BU_forItemFilter = 0;
var DEPT_list;
var is_TwoWPselected = false;
var is_XCselected = false;
var actualavailquantity;
var countDownDate;
var currentUserNTID;

$('input[type=checkbox]').each(function () { this.checked = false; });

$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});


function spinnerEnable() {
    var genSpinner = document.querySelector("#UploadSpinner");
    genSpinner.classList.add('fa');
    genSpinner.classList.add('fa-spinner');
    genSpinner.classList.add('fa-pulse');
}

//IsVKMSpoc();
//function IsVKMSpoc() {
//    $.ajax({

//        type: "GET",
//        url: "/BudgetingVKM/Get_IsVKMSpoc",
//        datatype: "json",
//        async: true,
//        success: function (data) {
//            debugger;
//            if (data.is_VKMSpoc == 1) {
//                document.getElementById("LEPlannerImport").style.display = "block";
//            }
//            else {
//                document.getElementById("LEPlannerImport").style.display = "none";
//            }
//        }
//    });
//}




window.addEventListener("submit", function (e) {
    //debugger;
    var form = e.target;
    if (form.getAttribute("enctype") === "multipart/form-data") {
        if (form.dataset.ajax) {
            //to sync form events with request events
            e.preventDefault();//written to block existing function -like double click on submit - incase of redundant call - only 1 submit fn should run at a time
            e.stopImmediatePropagation();

            //necessary for uploading files since event of uploading files should be synchronous b/w client and server though we use ajax(asynchronous call)
            var xhr = new XMLHttpRequest();//if another request sent->refresh browser- w/o refreshing pg
            //opening the import form
            xhr.open(form.method, form.action);//method-POST, action-webpg; link triggers the actnresult -> returning the view to index.cshtml->renders form

            //set the template (sending format) so that server understands how to parse it
            xhr.setRequestHeader("x-Requested-With", "XMLHttpRequest"); // this allows 'Request.IsAjaxRequest()' to work in the controller code

            xhr.onreadystatechange = function () {

                if (xhr.readyState === XMLHttpRequest.DONE && xhr.status == 200) { //function executes once response is rx from the server


                    try {
                        rdata = JSON.parse(xhr.responseText); //returned data to be parsed if it is a JSON object

                    }
                    catch (e) {
                        rdata = xhr.responseText;
                    }
                    if (form.dataset.ajaxSuccess) {
                        eval(form.dataset.ajaxSuccess); //converts function text to real function and executes (not very safe though)

                    }
                    else if (form.dataset.ajaxFailure) {
                        eval(form.dataset.ajaxFailure);
                    }

                    if (form.dataset.ajaxUpdate) {

                        var genSpinner = document.querySelector("#UploadSpinner");
                        genSpinner.classList.remove('fa');
                        genSpinner.classList.remove('fa-spinner');
                        genSpinner.classList.remove('fa-pulse');

                    }
                }
            };

            xhr.send(new FormData(form)); //send a request to server after importing
        }
    }
}, true);

function mySuccessFuntion(rdata) {
    debugger;

    if (rdata.success) {
        $.ajax({

            type: "GET",
            url: "/BudgetingRequest/Lookup",
            async: false,
            data: { 'year': filtered_yr },
            success: onsuccess_lookupdata,
            error: onerror_lookupdata
        })
        if (rdata.errormsg.substr(0, 8) == "") {
            //debugger;
            $.notify('Data Uploaded Successfully', {
                globalPosition: "top center",
                className: "success"
            });
        }
        else {
            //$.notify(rdata.errormsg, {
            //    globalPosition: "top center",
            //    className: "error"
            //});
            $.notify('Data Uploaded Successfully', {
                globalPosition: "top center",
                className: "success"
            });
        }

        //debugger;

    }
    else if (rdata.success == false) {
        //debugger;
        $.notify(rdata.errormsg, {
            globalPosition: "top center",
            className: "warn"
        });
    }
    else {

    }
    $.ajax({
        type: "GET",
        url: "/BudgetingRequest/GetData",
        data: { 'year': filtered_yr },
        datatype: "json",
        async: true,
        success: success_refresh_getdata,
        error: error_refresh_getdata

    });

    function success_refresh_getdata(response) {

        var getdata = response.data;
        $("#RequestTable").dxDataGrid({
            dataSource: getdata
        });
    }
    function error_refresh_getdata(response) {

        //$.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
        //    globalPosition: "top center",
        //    className: "warn"
        //});

    }


    //function success_refresh_getdata(response) {
    //   //debugger;
    //    var LabList = response.data;


    //}

    //function error_refresh_getdata(response) {
    //    //error
    //}






}

function myFailureFuntion() {
    //Failure code
    $.notify('Data Uploaded Successfully', {
        globalPosition: "top center",
        className: "success"
    });
}

// LE Planner authorization - if authorized, load the entire page; else notify the user to contact SmartLab Team for access
debugger;
$.ajax({
    async: false,
    type: "POST",
    url: encodeURI("../BudgetingRequest/LEPlanner_Authorise"),
    success: function (data) {
            debugger;
            if (data.data == "") {

                $.notify("Sorry! Current user is not authorized. Kindly contact SmartLab Tools Team for access!", {
                    globalPosition: "top center",
                    autoHideDelay: 20000,
                    className: "error"
                })
            }
            else {               
                debugger;
                //User is authorized
                currentUserNTID = data.data;
                /***Display CountDown Timer for the input window period to close***/
                //LOGIC begin///
                $.ajax({

                    type: "GET",
                    url: "/BudgetingRequest/CountDown",
                    async: false,
                    success: onsuccess_countdata,
                    error: onerror_countdata
                })

                function onsuccess_countdata(response) {
                    debugger;

                    countDownDate = new Date(parseInt(response.data.CurDate.substr(6, 13))).getTime();

                }

                function onerror_countdata(response) {

                    countDownDate = new Date("Jul 14, 2022 23:30:00").getTime();
                }
                //Region to add the countdown

                // Set the date we're counting down to

                //var countDownDate = new Date("Aug 23, 2021 23:30:00").getTime();
                // Update the count down every 1 second
                var x = setInterval(function () {

                    // Get today's date and time
                    var now = new Date().getTime();

                    // Find the distance between now and the count down date
                    var distance = countDownDate - now;

                    // Time calculations for days, hours, minutes and seconds
                    var days = Math.floor(distance / (1000 * 60 * 60 * 24));
                    var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                    var seconds = Math.floor((distance % (1000 * 60)) / 1000);

                    // Display the result in the element with id="demo"
                    document.getElementById("demo").innerHTML = "-- " + days + "d " + hours + "h "
                        + minutes + "m " + seconds + "s " + " left to fill in your inputs --";

                    // If the count down is finished, write some text
                    if (distance < 0) {

                        clearInterval(x);
                        countdownflag = true;
                        // $("#btnSubmitAll").prop("onclick", null).off("click");

                        document.getElementById("demo").innerHTML = "Input Collection Closed";
                        document.getElementById("btnSubmitAll").style.display = "none";
                        //document.getElementById('RequestTable').style.pointerEvents = 'none';
                    }
                }, 1000);
                //LOGIC end//

                $.ajax({
                    type: "GET",
                    url: encodeURI("../BudgetingRequest/InitRowValues"),
                    success: OnSuccessCall_dnew,
                    error: OnErrorCall_dnew

                });
             
                //Reference the DropDownList for Year to be selected by Requestor
                var ddlYears = document.getElementById("ddlYears");
                //Determine the Current Year.
                var currentYear = (new Date()).getFullYear();

                //Loop and add the Year values to DropDownList.
                for (var i = currentYear - 1; i <= currentYear + 1; i++) {
                    var option = document.createElement("OPTION");
                    option.innerHTML = i;
                    option.value = i;
                    ddlYears.appendChild(option);

                    if (option.value == (currentYear + 1)) {
                        //if (option.value == (currentYear - 2)) {
                        option.defaultSelected = true;
                        //option.defaultSelected = true;
                    }
                    filtered_yr = $("#ddlYears").val();
                    filtered_yr = parseInt(filtered_yr);
                    filtered_yr = filtered_yr.toString();
                   
                }
               
                if (filtered_yr == null) {
                    filtered_yr = new Date().getFullYear();
                }
                debugger;
                // Populates all the dropdowns for BU, Dept, Grp, Item etc                   
                $.ajax({

                    type: "GET",
                    url: "/BudgetingRequest/Lookup",
                    async: false,
                    data: { 'year': filtered_yr },
                    success: onsuccess_lookupdata,
                    error: onerror_lookupdata
                })

                //To manage the Details selection option available for the user (via checkbox to Request and Item details)
                var chkRequest, chkItem;
                $('#chkRequest').on('click', function () {
                    debugger;
                    if (this.checked)
                        chkRequest = true;
                    else
                        chkRequest = false;
                    if ($('.chkvkm:checked').length == 0) {
                        debugger;
                        var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
                        dataGridLEP1.beginUpdate();
                        dataGridLEP1.columnOption('OEM', 'visible', true);
                        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
                        dataGridLEP1.columnOption('Total_Price', 'visible', true);
                        dataGridLEP1.columnOption('Requestor', 'visible', false);
                        dataGridLEP1.columnOption('Reviewer_1', 'visible', true);
                        dataGridLEP1.columnOption('Reviewer_2', 'visible', true);
                        dataGridLEP1.columnOption('SubmitDate', 'visible', false);
                        dataGridLEP1.columnOption('Comments', 'visible', true);
                        dataGridLEP1.columnOption('Project', 'visible', true);
                        dataGridLEP1.columnOption('RequestDate', 'visible', false);
                        dataGridLEP1.columnOption('Request_Status', 'visible', true);
                        dataGridLEP1.columnOption('Category', 'visible', false);
                        dataGridLEP1.columnOption('Cost_Element', 'visible', false);
                        dataGridLEP1.columnOption('BudgetCode', 'visible', false); 
                        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', false);
                        dataGridLEP1.columnOption('OrderType', 'visible', false);
                        dataGridLEP1.columnOption('Unit_Price', 'visible', false);
                        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);

                        dataGridLEP1.columnOption('BU', 'visible', true);
                        dataGridLEP1.columnOption('DEPT', 'visible', true);
                        dataGridLEP1.columnOption('Group', 'visible', true);
                        dataGridLEP1.columnOption('Item_Name', 'visible', true);

                        dataGridLEP1.endUpdate();

                    }
                    else if ($('.chkvkm:checked').length == 2) {
                        var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
                        dataGridLEP1.beginUpdate();
                        dataGridLEP1.columnOption('OEM', 'visible', true);
                        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
                        dataGridLEP1.columnOption('Total_Price', 'visible', true);
                        dataGridLEP1.columnOption('Requestor', 'visible', true);
                        dataGridLEP1.columnOption('Reviewer_1', 'visible', true);
                        dataGridLEP1.columnOption('Reviewer_2', 'visible', true);
                        dataGridLEP1.columnOption('SubmitDate', 'visible', true);
                        dataGridLEP1.columnOption('Comments', 'visible', true);
                        dataGridLEP1.columnOption('Project', 'visible', true);
                        dataGridLEP1.columnOption('RequestDate', 'visible', true);
                        dataGridLEP1.columnOption('Request_Status', 'visible', true);

                        dataGridLEP1.columnOption('Category', 'visible', true);
                        dataGridLEP1.columnOption('Cost_Element', 'visible', true);
                        dataGridLEP1.columnOption('BudgetCode', 'visible', true);
                        dataGridLEP1.columnOption('Unit_Price', 'visible', true);
                        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
                        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', true);
                        dataGridLEP1.columnOption('OrderType', 'visible', true);

                        dataGridLEP1.columnOption('BU', 'visible', true);
                        dataGridLEP1.columnOption('DEPT', 'visible', true);
                        dataGridLEP1.columnOption('Group', 'visible', true);
                        dataGridLEP1.columnOption('Item_Name', 'visible', true);

                        dataGridLEP1.endUpdate();
                    }
                    else {
                        var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
                        dataGridLEP1.beginUpdate();
                        dataGridLEP1.columnOption('OEM', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
                        dataGridLEP1.columnOption('Total_Price', 'visible', true);
                        dataGridLEP1.columnOption('Requestor', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Reviewer_1', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Reviewer_2', 'visible', chkRequest);
                        dataGridLEP1.columnOption('SubmitDate', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Comments', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Project', 'visible', chkRequest);
                        dataGridLEP1.columnOption('RequestDate', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Request_Status', 'visible', chkRequest);

                        dataGridLEP1.columnOption('Category', 'visible', chkItem);
                        dataGridLEP1.columnOption('Cost_Element', 'visible', chkItem);
                        dataGridLEP1.columnOption('BudgetCode', 'visible', chkItem);
                        dataGridLEP1.columnOption('Unit_Price', 'visible', chkItem);
                        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', chkItem);
                        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', chkItem);
                        dataGridLEP1.columnOption('OrderType', 'visible', chkItem);

                        dataGridLEP1.columnOption('BU', 'visible', chkRequest);
                        dataGridLEP1.columnOption('DEPT', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Group', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Item_Name', 'visible', true);

                        dataGridLEP1.endUpdate();
                    }

                });

                $('#chkItem').on('click', function () {
                    debugger;

                    if (this.checked)
                        chkItem = true;
                    else
                        chkItem = false;
                    var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
                    if ($('.chkvkm:checked').length == 0) {
                        debugger;
                        var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
                        dataGridLEP1.beginUpdate();
                        dataGridLEP1.columnOption('OEM', 'visible', true);
                        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
                        dataGridLEP1.columnOption('Total_Price', 'visible', true);
                        dataGridLEP1.columnOption('Requestor', 'visible', false);
                        dataGridLEP1.columnOption('Reviewer_1', 'visible', true);
                        dataGridLEP1.columnOption('Reviewer_2', 'visible', true);
                        dataGridLEP1.columnOption('SubmitDate', 'visible', false);
                        dataGridLEP1.columnOption('Comments', 'visible', true);
                        dataGridLEP1.columnOption('Project', 'visible', true);
                        dataGridLEP1.columnOption('RequestDate', 'visible', false);
                        dataGridLEP1.columnOption('Request_Status', 'visible', true);
                        dataGridLEP1.columnOption('Category', 'visible', false);
                        dataGridLEP1.columnOption('Cost_Element', 'visible', false);
                        dataGridLEP1.columnOption('BudgetCode', 'visible', false);
                        dataGridLEP1.columnOption('Unit_Price', 'visible', false);
                        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
                        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', false);
                        dataGridLEP1.columnOption('OrderType', 'visible', false);


                        dataGridLEP1.columnOption('BU', 'visible', true);
                        dataGridLEP1.columnOption('DEPT', 'visible', true);
                        dataGridLEP1.columnOption('Group', 'visible', true);
                        dataGridLEP1.columnOption('Item_Name', 'visible', true);

                        dataGridLEP1.endUpdate();

                    }
                    else if ($('.chkvkm:checked').length == 2) {
                        var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
                        dataGridLEP1.beginUpdate();
                        dataGridLEP1.columnOption('OEM', 'visible', true);
                        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
                        dataGridLEP1.columnOption('Total_Price', 'visible', true);
                        dataGridLEP1.columnOption('Requestor', 'visible', true);
                        dataGridLEP1.columnOption('Reviewer_1', 'visible', true);
                        dataGridLEP1.columnOption('Reviewer_2', 'visible', true);
                        dataGridLEP1.columnOption('SubmitDate', 'visible', true);
                        dataGridLEP1.columnOption('Comments', 'visible', true);
                        dataGridLEP1.columnOption('Project', 'visible', true);
                        dataGridLEP1.columnOption('RequestDate', 'visible', true);
                        dataGridLEP1.columnOption('Request_Status', 'visible', true);

                        dataGridLEP1.columnOption('Category', 'visible', true);
                        dataGridLEP1.columnOption('Cost_Element', 'visible', true);
                        dataGridLEP1.columnOption('BudgetCode', 'visible', true);
                        dataGridLEP1.columnOption('Unit_Price', 'visible', true);
                        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
                        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', true);
                        dataGridLEP1.columnOption('OrderType', 'visible', true);

                        dataGridLEP1.columnOption('BU', 'visible', true);
                        dataGridLEP1.columnOption('DEPT', 'visible', true);
                        dataGridLEP1.columnOption('Group', 'visible', true);
                        dataGridLEP1.columnOption('Item_Name', 'visible', true);

                        dataGridLEP1.endUpdate();
                    }
                    else {
                        dataGridLEP1.beginUpdate();
                        dataGridLEP1.columnOption('Category', 'visible', chkItem);
                        dataGridLEP1.columnOption('Cost_Element', 'visible', chkItem);
                        dataGridLEP1.columnOption('BudgetCode', 'visible', chkItem);
                        dataGridLEP1.columnOption('Unit_Price', 'visible', chkItem);
                        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', chkItem);
                        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', chkItem);
                        dataGridLEP1.columnOption('OrderType', 'visible', chkItem);

                        dataGridLEP1.columnOption('OEM', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
                        dataGridLEP1.columnOption('Total_Price', 'visible', true);
                        dataGridLEP1.columnOption('Requestor', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Reviewer_1', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Reviewer_2', 'visible', chkRequest);
                        dataGridLEP1.columnOption('SubmitDate', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Comments', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Project', 'visible', chkRequest);
                        dataGridLEP1.columnOption('RequestDate', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Request_Status', 'visible', chkRequest);

                        dataGridLEP1.columnOption('BU', 'visible', chkRequest);
                        dataGridLEP1.columnOption('DEPT', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Group', 'visible', chkRequest);
                        dataGridLEP1.columnOption('Item_Name', 'visible', true);

                        dataGridLEP1.endUpdate();

                    }

                });


                //function to bring up the VKM Request UI View
                ajaxCallforRequestUI(filtered_yr);



                //Ajax call to Get Dept Summary Data
                $.ajax({
                    type: "GET",
                    url: "/BudgetingRequest/GetDeptSummaryData",
                    data: { 'year': filtered_yr },
                    datatype: "json",
                    async: true,
                    success: success_DeptSummaryTable,
                    error: error_DeptSummaryTable

                });


                //Ajax call to Get BU Summary Data
                //debugger;
                $.ajax({
                    type: "GET",
                    url: encodeURI("../BudgetingRequest/GetBUSummaryData"),
                    data: { 'year': filtered_yr },
                    success: OnSuccess_GetBUSummary,
                    error: OnError_GetBUSummary
                });

                $('#btnSubmitAll').click(function () {
                    HoEApprove(1999999999, filtered_yr);
                });
                //$(function () {
                //    // run the currently selected effect
                //    function runEffect() {
                //        // get effect type from
                //        var selectedEffect = "blind";

                //        var options = {};

                //        // Run the effect
                //        $("#effect").show(selectedEffect, options, 1000, callback);
                //    };

                //    function callback() {
                //        setTimeout(function () {
                //            $("#effect:visible").removeAttr("style").fadeOut();
                //        }, 60000);
                //    };

                //    // Set effect from select menu value
                //    $("#btn_summary").on("click", function () {
                //        runEffect();
                //    });

                //    $("#effect").hide();
                //});

                $("#btn_summary").on("click", function () {
                    //debugger;
                    var e = document.getElementById("effect");
                    $("#effect").prop("hidden", false);
                    //debugger;
                    if (e.style.display == 'block')
                        e.style.display = 'none';
                    else
                        e.style.display = 'block';
                });

                $("#buttonClearFilters").dxButton({
                    text: 'Clear Filters',
                    onClick: function () {
                        $("#RequestTable").dxDataGrid("clearFilter");
                    }
                });

                $('[data-toggle="tooltip"]').tooltip();

                //Export data
                $("#export").click(function () {
                    debugger;
                    var workbook = new ExcelJS.Workbook();
                    var worksheet = workbook.addWorksheet("SheetName");
                    var today = new Date();
                    DevExpress.excelExporter.exportDataGrid({
                        component: $("#RequestTable").dxDataGrid("instance"),
                        worksheet: worksheet
                    }).then(function () {
                        workbook.xlsx.writeBuffer().then(function (buffer) {
                            saveAs(new Blob([buffer], { type: "application/octet-stream" }), "VKM " + filtered_yr + ' ' + currentUserNTID + ' RequestList_'+ today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear() + ".xlsx");

                        });
                     
                    });

                    //commented since dxgrid export used
                    //$.ajax({

                    //    type: "POST",
                    //    url: "/BudgetingRequest/ExportDataToExcel/",
                    //    data: { 'useryear': filtered_yr },


                    //    success: function (export_result) {

                    //        var bytes = new Uint8Array(export_result.FileContents);
                    //        var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
                    //        var link = document.createElement('a');
                    //        link.href = window.URL.createObjectURL(blob);
                    //        link.download = export_result.FileDownloadName;
                    //        link.click();
                    //        $.notify('Your Item Request list is exported to an excel sheet. Please Open/Save to view the data!', {
                    //            globalPosition: "top center",
                    //            className: "success",
                    //            autoHideDelay: 8000
                    //        });

                    //    },
                    //    error: function () {
                    //        alert("Issue in Exporting data. Please check again!");
                    //    }

                    //});
                });

            }
        },
    error: function (data) {
            debugger;
            $.notify("Kindly retry after sometime !", {
                globalPosition: "top center",
                autoHideDelay: 13000,
                className: "error"
            })
        }


});

//triggered when the user selects/changes the year - this has been placed outside the authorization code block so that the YearChange fn can be triggered during onchange of #ddlYears (Year dropdown element)
function fnYearChange(yearselect) {
        filtered_yr = parseInt(yearselect.value);
        filtered_yr = filtered_yr.toString();
        debugger;
        $.ajax({

            type: "GET",
            url: "/BudgetingRequest/Lookup",
            async: false,
            data: { 'year': filtered_yr },
            success: onsuccess_lookupdata,
            error: onerror_lookupdata
        })

        //if (filtered_yr < new Date().getFullYear()) { //2023 < 2021
        //    $("#btn_summary").prop("hidden", true);
        //   $("#effect").prop("hidden", true);
        //}
        //else {
        //    $("#btn_summary").prop("hidden", false);//2023 < 2022 , 2022 < 2022
        //    //$("#effect").prop("hidden", false);
        //}

        debugger;
        if (filtered_yr == new Date().getFullYear() + 1) { //2023 == 2023
            $("#btnSubmitAll").prop("hidden", false);
            $("#btn_summary").prop("hidden", false);
            $("#effect").prop("hidden", false);
        }
        else {
            $("#btnSubmitAll").prop("hidden", true);//2023 < 2022 , 2022 < 2022
            $("#btn_summary").prop("hidden", true);
            $("#effect").prop("hidden", true);
        }
        ajaxCallforRequestUI(filtered_yr);


    }



// Populates all the dropdowns for BU, Dept, Grp, Item etc - this has been placed outside the authorization code block so that the YearChange fn can access
function onsuccess_lookupdata(response) {
        debugger;
        lookup_data = response.data;
        BU_list = lookup_data.BU_List;
        OEM_list = lookup_data.OEM_List;

        DEPT_list = lookup_data.DEPT_List;
        Group_list = lookup_data.Groups_test;
        //Item_list = lookup_data.Item_List;
        Category_list = lookup_data.Category_List;
        CostElement_list = lookup_data.CostElement_List;
        OrderStatus_list = lookup_data.OrderStatus_List;
        OrderType_list = lookup_data.Order_Type_List;
        BudgetCodeList = lookup_data.BudgetCodeList;
        //debugger;
    //Jsonserialzer length exceed issue resolution
    $.ajax({

        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../BudgetingOrder/Lookup_ItemList",
        async: false,
        //data: { 'year': filtered_yr },
        data: JSON.stringify({ year: parseInt(filtered_yr) - 1 }),
        dataType: 'json',
        success: function (data) {
            debugger;
            Item_list = data;
        },
        error: function (jqXHR, exception) {
            debugger;
        }

    })
    }

function onerror_lookupdata(response) {
        if (response.errormsg) {
            $.notify(response.errormsg, {
                globalPosition: "top center",
                className: "error",
                autoHideDelay: 15000,
            });
        }
        else {
            //alert("Error in fetching lookup");

        }

    }



//function to bring up the VKM Request UI View - this has been placed outside the authorization code block so that the YearChange fn can access
function ajaxCallforRequestUI(filtered_yr) {

        $.ajax({
            type: "GET",
            url: encodeURI("../BudgetingRequest/GetData"),
            data: { 'year': filtered_yr },
            success: OnSuccess_GetData,
            error: OnError_GetData
        });

    }


//Logic to Get VKM Request View
function OnSuccess_GetData(response) {
    debugger;
    var objdata;
    //If Current VKM year 
    if (filtered_yr == new Date().getFullYear() + 1) {
        $.notify('Your current VKM requests are being loaded, Please wait !', {
            globalPosition: "top center",
            className: "info",
            autoHideDelay: 13000,
        });
    }
    else {
        //If Previous VKM year chosen by the user
        $.notify('Previous Year VKM Requests are available . Kindly filter the data based on your preference !', {
            globalPosition: "top center",
            className: "info",
            autoHideDelay: 30000,
        });
    }

    objdata = (response.data);

    //Hide the Loading indicator once the Request List is fetched

    dataGridLEP = $("#RequestTable").dxDataGrid({

        dataSource: objdata,
        keyExpr: "RequestID",
        toolbar: {
            items: [
                'addRowButton',
                'columnChooserButton',
                {
                    location: 'after',
                    widget: 'dxButton',
                    options: {
                        icon: 'refresh',
                        text: 'Clear Request Filters',
                        hint: 'Clear Request Filters',
                        onClick() {
                            $("#RequestTable").dxDataGrid("clearFilter");
                             //$("#buttonClearFilters").dxButton({
            //    text: 'Clear Filters',
            //    onClick: function () {
            //        $("#RequestTable").dxDataGrid("clearFilter");
            //    }
            //});
                        },
                    },
                    
                
                },
                'exportButton'
            ]
        },
        onToolbarPreparing: function (e) {
            let toolbarItems = e.toolbarOptions.items;

                let addRowButton = toolbarItems.find(x => x.name === "addRowButton");
                if (addRowButton.options != undefined) { //undefined when any of the previous vkm year selected and add button is hidden
                    addRowButton.options.text = "Add New Request";
                    addRowButton.options.hint = "Add New Request";
                    addRowButton.showText = "always";
                }

                let columnChooserButton = toolbarItems.find(x => x.name === "columnChooserButton");
                columnChooserButton.options.text = "Hide Details";
                columnChooserButton.options.hint = "Hide Request Details";
                columnChooserButton.showText = "always";

                //let exportButton = toolbarItems.find(x => x.name === "exportButton");
                //exportButton.options.text = "Export to Excel";
                //exportButton.options.hint = "Click here to export data";
                //exportButton.showText = "always";

        },

        //columnResizingMode: "nextColumn",
        //wordWrapEnabled: true,
        loadPanel: {
            enabled: true
        },
       
        columnMinWidth: 50,
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        //export: {
        //    enabled: true
        //},
        hoverStateEnabled: {
            enabled: true
        },
        //stateStoring: {
        //    enabled: true,
        //    type: "localStorage",
        //    storageKey: "RequestID"
        //},
        columnFixing: {
            enabled: true
        },
        width: "97.7vw", //needed to allow horizontal scroll
        height: "67vh",
        columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
        //remoteOperations: false, - if true, tital summary will not work
        scrolling: {
            mode: "virtual",
            rowRenderingMode: "virtual",
            columnRenderingMode: "virtual"
        },
        onContentReady: function (e) {
            //debugger;
            e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
        },
        //noDataText: encodeMessage(noDataTextUnsafe),
        //showRequiredMark: true,
        //RequiredMark: '*',
        allowColumnResizing: false,
        columnResizingMode: "widget",
        //columnMinWidth: 100,
        onCellPrepared: function (e) {
            if (e.rowType === "header" || e.rowType === "filter") {
                e.cellElement.addClass("columnHeaderCSS");
                e.cellElement.find("input").addClass("columnHeaderCSS");
            }
        },
        noDataText: " ☺ No Item ! click '+' Add New Request option on the top right of VKM Requests",
        editing: {
            mode: "row",
            allowUpdating: function (e) {
                //Edit option available if the Request has not been submitted to HOE and if it is current VKM Request
                if (e.row.data.ApprovalHoE == undefined)
                    e.row.data.ApprovalHoE = false;
                return !e.row.data.ApprovalHoE && (filtered_yr == new Date().getFullYear() + 1)
            },
            allowDeleting: function (e) {
                //Delete option available if the Request has not been submitted to HOE and if it is current VKM Request
                if (e.row.data.ApprovalHoE == undefined)
                    e.row.data.ApprovalHoE = false;
                return !e.row.data.ApprovalHoE && (filtered_yr == new Date().getFullYear() + 1)
            },
            //Add option available if the window period is not closed and if it is current VKM Request
            allowAdding: !countdownflag && (filtered_yr == new Date().getFullYear() + 1)/*true*/,
            useIcons: true
        },
        // focusedRowEnabled: true,
        columnChooser: {
            enabled: true
        },

        //filterRow: {
          //  visible: true

        //},
        showBorders: true,
        headerFilter: {
            visible: true,
            applyFilter: "auto",
            allowSearch: true
        },
        selection: {
            applyFilter: "auto"
        },

        paging: {
            pageSize: 100
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },


        onEditorPreparing: function (e) {
            var component = e.component,
                rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

            if (e.parentType === "dataRow" && e.dataField === "Group") {

                e.editorOptions.disabled = (typeof e.row.data.DEPT !== "number");
                if (e.editorOptions.disabled)
                    e.editorOptions.placeholder = 'Select Dept first';
                if (!e.editorOptions.disabled)
                    e.editorOptions.placeholder = 'Select Group';

            }

            //if (e.parentType == 'dataRow' && e.dataField == 'DEPT') {
            //    e.editorOptions.dataSource = function (options) {
            //        //store: { data: DEPT_list }
            //        //store: {
            //        //    //type: 'array',
            //        //    data: states,
            //        //    key: 'ID',
            //        //} , 
            //        filter: function (item) {
            //            if (item.Outdated == false) {
            //                return false;
            //            }
            //        }
            //    }
            //}

            //if (e.parentType === 'dataRow' && e.dataField === 'BudgetCode') {
            //        //e.editorOptions.disabled = (e.row.data.CostElementID == null || e.row.data.CostElementID == '');
            //}


            if (e.dataField === "BU") {

                var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                e.editorOptions.onValueChanged = function (e) {
                    onValueChanged.call(this, e);
                    debugger;
                    BU_forItemFilter = 0; //reset the flag
                    if (e.value == 1 && is_XCselected == true) {
                        is_TwoWPselected = true;
                        BU_forItemFilter = 4;
                        //window.setTimeout(function () {
                        //    component.cellValue(rowIndex, "Reviewer_2", "Sheeba Rani R");
                        //}, 1000);
                    }

                    if (e.value == 1 || e.value == 3) {
                        BU_forItemFilter = 3;
                    }
                    else if (e.value == 2 || e.value == 4)
                        BU_forItemFilter = 4;

                    //else if (e.value == 4)
                    //    BU_forItemFilter = 4;
                    //else if (e.value == 5)
                    //    BU_forItemFilter = 5;
                    //////////// UNCOMMENTED BECAUSE THE NEW LOGIC TO GET VKM SPOC IS BASED ON USER'S SECTION &  BU
                    $.ajax({
                    
                        type: "post",
                        url: "/BudgetingRequest/GetReviewer_VKMSPOC",
                        data: { DEPT: component.cellValue(rowIndex, "DEPT"), BU: e.value },
                        datatype: "json",
                        traditional: true,
                        success: function (data) {

                            if (data.success)
                                reviewer_2 = data.data;
                            else {
                                $.notify("Unable to find " + data.data + " 's VKM SPOC Details. Kindly contact SmartLab Team for assistance", {
                                    globalPosition: "top center",
                                    className: "warn"
                                })
                                reviewer_2 = "NA";
                            }
                            //reviewer_2 = data;
                            //if (e.value == 1 && is_XCselected == true) {
                            //    is_TwoWPselected = true;
                            //    BU_forItemFilter = 4;
                            //    //reviewer_2 = "Sheeba Rani R";
                            //                                }

                        }
                    })


                    //$.ajax({
                    //    type: "post",
                    //    url: "/BudgetingRequest/GetMasterList",
                    //    data: { BU: e.value },
                    //    datatype: "json",
                    //    traditional: true,
                    //    success: function (data) {
                    //        //debugger;
                    //        if (data > 0)
                    //            Item_list_New = data;

                    //    }
                    //})
                    // Emulating a web service call
                    window.setTimeout(function () {
                        debugger;
                        component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
                    }, 1000);
                }
            }

            //debugger;
            if (e.dataField === "DEPT") {

                var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                e.editorOptions.onValueChanged = function (e) {
                    onValueChanged.call(this, e);
                    //if (is_TwoWPselected && (e.value > 59 && e.value < 104)) {
                    //    BU_forItemFilter = 4;
                    //    window.setTimeout(function () {
                    //        component.cellValue(rowIndex, "Reviewer_2", "Sheeba Rani R");
                    //    }, 1000);
                    //}

                    //debugger;

                    $.ajax({

                        type: "post",
                        url: "/BudgetingRequest/GetReviewer_HoE",
                        data: { DEPT: e.value },
                        datatype: "json",
                        traditional: true,
                        success: function (data) {
                            //debugger;
                            if (data.success)
                                reviewer_1 = data.data;
                            else {
                                $.notify("Unable to find "+data.data+ " 's HOE Details. Kindly contact SmartLab Team for assistance", {
                                    globalPosition: "top center",
                                    className: "warn"
                                })
                                reviewer_1 = "NA";
                            }

                        }
                    })
                    debugger;
                    $.ajax({

                        type: "post",
                        url: "/BudgetingRequest/GetReviewer_VKMSPOC",
                        data: { DEPT: e.value, BU: component.cellValue(rowIndex, "BU")},
                        datatype: "json",
                        traditional: true,
                        success: function (data) {

                            if (data.success)
                                reviewer_2 = data.data;
                            else {
                                $.notify("Unable to find " + data.data + " 's VKM SPOC Details. Kindly contact SmartLab Team for assistance", {
                                    globalPosition: "top center",
                                    className: "warn"
                                })
                                reviewer_2 = "NA";
                            }
                            //reviewer_2 = data;
                            if (e.value == 1 && is_XCselected == true) {
                                is_TwoWPselected = true;
                                BU_forItemFilter = 4;
                                //reviewer_2 = "Sheeba Rani R";
                            }

                        }
                    })


                    // Emulating a web service call
                    window.setTimeout(function () {
                        //debugger;
                        component.cellValue(rowIndex, "Reviewer_1", reviewer_1);
                        component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
                    }, 1000);
                }
            }
            //Reviewer_2
            //if (e.dataField === "DEPT") {
            //    //debugger;
            //    //var Dept = e.row.data.DEPT;
            //    //var BU = e.row.data.BU;
            //}




            if (e.dataField === "Required_Quantity") {
                //debugger;
                e.editorOptions.valueChangeEvent = "keyup";

                var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                //debugger;
                e.editorOptions.onValueChanged = function (e) {
                   // debugger;
                    onValueChanged.call(this, e);
                   // debugger;
                    var UnitPr_sel = component.cellValue(rowIndex, "Unit_Price");
                   // debugger;
                    if (component.cellValue(rowIndex, "Unit_Price") != undefined && component.cellValue(rowIndex, "Unit_Price") != null) {
                       // debugger;
                        $.ajax({

                            type: "post",
                            url: "/BudgetingVKM/GetRevCost",
                            data: { Reviewed_Quantity: e.value, Unit_Price: component.cellValue(rowIndex, "Unit_Price") },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                //debugger;
                                //if (data.msg) {
                                //    CostEUR = "";
                                //    window.setTimeout(function () {
                                //       //debugger;
                                //        component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
                                //    }, 1000);

                                //    //$.notify(data.msg, {
                                //    //    globalPosition: "top center",
                                //    //    className: "success"
                                //    //})
                                //}
                                //else {


                                RevCost = data.RevCost;
                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "Total_Price", RevCost);
                                }, 1000);
                                //}


                            }
                        })
                        //// Emulating a web service call

                    }


                }
            }


        },

        columns: [
            {
                type: "buttons",
                width: 90,
                alignment: "left",
                fixed: true,
                fixedPosition: "left",
                buttons: [
                    "edit", "delete",
                    {
                        hint: "Submit",
                        icon: "check",
                        text: "Submit Item Request",
                        visible: function (e) {
                            if (e.row.data.ApprovalHoE == undefined)
                                e.row.data.ApprovalHoE = false;

                            return !countdownflag && !e.row.isEditing && !e.row.data.ApprovalHoE && (filtered_yr == new Date().getFullYear() + 1)/*&& !isOrderApproved(e.row.data.OrderStatus)*/;

                        },
                        onClick: function (e) {
                            HoEApprove(e.row.data.RequestID, filtered_yr);
                            e.component.refresh(true);
                            e.event.preventDefault();
                        }
                    }]
            },
            {

                alignment: "center",
                columns: [
                    {
                        dataField: "RequestID",
                        allowEditing: false,
                        visible: false
                    },
                    {
                        dataField: "BU",
                        width: 87,
                        validationRules: [{ type: "required" }],

                        setCellValue: function (rowData, value) {
                            //debugger;
                            rowData.BU = value;
                            rowData.Item_Name = null;

                        },
                        lookup: {
                            dataSource: function (options) {
                                debugger;
                                if (options.data && options.data.BU && options.data.BU == 6) {
                                    return {

                                        store: BU_list,
                                        filter: options.data ? ["ID", "=", 6] : null
                                    };
                                }
                                else {
                                    return {

                                        store: BU_list,
                                    };
                                }
                               

                            },
                            valueExpr: "ID",
                            displayExpr: "BU"
                        },
                        allowEditing: !countdownflag
                    },

                    {
                        dataField: "OEM",
                        validationRules: [{ type: "required" }],
                        width: 85,
                        lookup: {
                            dataSource: OEM_list,
                            valueExpr: "ID",
                            displayExpr: "OEM"
                        },
                        allowEditing: !countdownflag

                    },
                    {
                        dataField: "Project",
                        allowEditing: !countdownflag,
                        width: 88

                    },
                    {
                        dataField: "DEPT",
                        caption: "Dept",
                        validationRules: [{ type: "required" }],
                        setCellValue: function (rowData, value) {
                            //debugger;
                            rowData.DEPT = value;
                            rowData.Group = null;

                        },
                        minWidth: 130,
                        lookup: {
                            dataSource: function (options) {
                                //debugger;
                                return {

                                    store: DEPT_list,
                                    filter: options.data ? ["Outdated", "=", false] : null


                                };
                            },

                            valueExpr: "ID",
                            displayExpr: "DEPT"

                        },
                        allowEditing: !countdownflag


                    },
                    {
                        dataField: "Group",
                        minWidth: 120,
                        validationRules: [{ type: "required" }],
                        lookup: {
                            dataSource: function (options) {

                                return {

                                    store: Group_list,

                                    filter: options.data ? ["Dept", "=", options.data.DEPT] : null
                                };

                            },
                            valueExpr: "ID",
                            displayExpr: "Group"
                        },
                        allowEditing: !countdownflag

                    },
                    {
                        dataField: "Item_Name",
                        minWidth: 240,
                        caption: "Item",
                        setCellValue: function (rowData, value) {
                            debugger;
                            ////if value.length > 1 => it means that the item list is filtered based on the ordertype selected. At that instance, no need to fetch other details
                            //if (value.length == 1) { //then the user has selected an item. The corresponding item's details needs to be auto-set.
                            if (value.constructor.name == "Number") {
                                rowData.Item_Name = value;
                                rowData.Category = Item_list.find(x => x.S_No == value).Category;
                                rowData.Cost_Element = Item_list.find(x => x.S_No == value).Cost_Element;
                                rowData.Unit_Price = Item_list.find(x => x.S_No == value).UnitPriceUSD;
                                rowData.ActualAvailableQuantity = Item_list.find(x => x.S_No == value).Actual_Available_Quantity;
                                rowData.BudgetCode = Item_list.find(x => x.S_No == value).BudgetCode;
                                rowData.BudgetCodeDescription = BudgetCodeList.find(x => x.Budget_Code == rowData.BudgetCode).Budget_Code_Description;
                                rowData.OrderType = parseInt(Item_list.find(x => x.S_No == value).Order_Type);
                            }
                              
                        },
                        validationRules: [{ type: "required" }],
                        //CAS 1: testing for separate masterlist for bus
                        //lookup: {
                        //    dataSource: function (options) {
                        //        //debugger;
                        //        if (BU_Item == 1) {
                        //            //debugger;
                        //            return {



                        //                store: Item_list_New /*DEPT_list*/, 

                        //            };
                        //            //debugger;
                        //        }
                        //        else {
                        //            //debugger;
                        //            return {


                        //                store: Item_list /*Group_list*/,

                        //            };
                        //            //debugger;
                        //        }


                        //    },
                        //    valueExpr: "S_No", /*"ID",*/
                        //    displayExpr: "Item_Name" /*"DEPT"*/
                        //},

                        //CAS 2: testing for same masterlist for bus
                        lookup: {
                            dataSource: function (options) {
                               debugger;
                                return {


                                    store: Item_list,
                                    paginate: true, //enable paging when list has huge amount of data, else takes more time to load
                                    pageSize: 30,
                                    //filter: options.data ? [["BU", "=", options.data.BU != 0 && options.data.BU != 1 && options.data.BU != 2 ? options.data.BU : BU_forItemFilter], 'and', ["Deleted", "=", false]] : null
                                    filter: options.data ? [["BU", "=", BU_forItemFilter != 0 ? BU_forItemFilter : (options.data.BU == 1 ? 3 : (options.data.BU == 2 ? 4 : options.data.BU))],'and',["Category","<>",null]] : null
                                       //filter: options.data ? [["BU", "=", options.data.BU], 'and', ["Order_Type", "=", options.data.OrderType], 'and', ["Deleted", "=", false]] : null


                                }


                            },
                            valueExpr: "S_No",
                            displayExpr: "Item_Name"
                        },


                        calculateSortValue: function (data) {
                            //debugger;
                            const value = this.calculateCellValue(data);
                            return this.lookup.calculateCellValue(value);
                        },
                        allowEditing: !countdownflag

                    }, {
                        dataField: "ActualAvailableQuantity",
                        caption: "Available Qty",
                        allowEditing: false,
                        width: 102
                    },

                    {
                        dataField: "Category",
                        caption: "Category",
                        validationRules: [{ type: "required" }],

                        lookup: {
                            dataSource: Category_list,
                            valueExpr: "ID",
                            displayExpr: "Category"
                        },
                        allowEditing: false,
                        visible: false

                    },
                    {
                        dataField: "Cost_Element",
                        //setCellValue(rowData, value) {
                        //    rowData.Cost_Element = value;
                        //    rowData.BudgetCode = null;
                        //},
                        lookup: {
                            dataSource: CostElement_list,
                            valueExpr: "ID",
                            displayExpr: "CostElement"
                        },
                        allowEditing: false,
                        visible: false


                    },

                    {
                        dataField: "BudgetCode",
                        //lookup: {
                        //    dataSource(options) {
                        //        return {
                        //            store: BudgetCodeList,
                        //            filter: options.data ? ['CostElementID', '=', options.data.Cost_Element] : null,
                        //        };
                        //    },
                        //    valueExpr: "BudgetCode",
                        //    displayExpr: "BudgetCode"
                        //},

                        //lookup: {
                        //    dataSource: BudgetCodeList,
                        //    valueExpr: "BudgetCode",
                        //    displayExpr: "BudgetCode"
                        //},

                        allowEditing: false,
                        visible: false


                    },
                    {
                        dataField: "BudgetCodeDescription",
                        caption: "Budget Code Description",
                        visible: false,
                        allowEditing: false,
                    },
                    {
                        dataField: "OrderType",
                        caption: "Order Type",
                        
                        lookup: {
                            dataSource: function (options) {
                                debugger;
                                return {

                                    store: OrderType_list,
                                }

                            },
                            valueExpr: "ID",
                            displayExpr: "Order_Type"
                        },
                        visible: false,
                        allowEditing: false

                    },

                    {
                        dataField: "Required_Quantity",
                        caption: "Qty",
                        width: 60,
                        validationRules: [
                            { type: "required" },
                            {
                                type: "range",
                                message: "Please enter valid count > 0",
                                min: 0,
                                max: 214783647
                            }],
                        dataType: "number",
                        setCellValue: function (rowData, value) {

                            rowData.Required_Quantity = value;

                        },
                        allowEditing: !countdownflag


                    },
                    {
                        dataField: "Unit_Price",
                        caption: "Unit Price",
                        dataType: "number",
                        format: { type: "currency", precision: 0 },
                        valueFormat: "#0",
                        allowEditing: false,
                        validationRules: [{ type: "required" }, {
                            type: "range",
                            message: "Please enter valid price > 0",
                            min: 0.01,
                            max: Number.MAX_VALUE
                        }],
                        allowEditing: false,
                        visible: false


                    },
                    {
                        dataField: "Total_Price",
                        //width: 100,
                        caption: "Amt",
                        calculateCellValue: function (rowData) {

                            if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
                                return rowData.Unit_Price * rowData.Required_Quantity;
                            }
                            else
                                return 0.0;
                        },

                        dataType: "number",
                        format: { type: "currency", precision: 0 },
                        valueFormat: "#0",
                        allowEditing: false
                    },
                    {
                        dataField: "Reviewed_Quantity",
                        caption: "Review Qty",
                        dataType: "number",
                        allowEditing: false,
                        visible: false

                    },
                    {
                        dataField: "Reviewed_Cost",
                        //width: 100,
                        caption: "Review Cost",                       
                        dataType: "number",
                        format: { type: "currency", precision: 0 },
                        allowEditing: false,
                        visible: false
                    },
                    {
                        dataField: "Requestor",
                        allowEditing: false,
                        visible: false,
                        width: 120
                    },
                    {
                        dataField: "Reviewer_1",
                        caption: "HoE",
                        allowEditing: false,
                        width: 110,
                        //visible: false
                    },
                    {
                        dataField: "Reviewer_2",
                        caption: "VKM SPOC",
                        allowEditing: false,
                        width: 120,
                        //visible: false
                    },
                    {
                        dataField: "Comments",
                        caption: "Remark",
                        width: 100,
                        allowResizing: true

                    },
                    {
                        dataField: "RequestDate",
                        allowEditing: false,
                        width: 100,
                        visible: false


                    },

                    {
                        dataField: "Request_Status",
                        caption: "Plan Status",
                        allowEditing: false
                        //width: 100

                    },
                    {
                        dataField: "OrderStatus",
                        caption: "Order Status",
                        lookup: {
                            dataSource: OrderStatus_list,
                            valueExpr: "ID",
                            displayExpr: "OrderStatus"
                        },
                        allowEditing: false,
                        width: 102
                    },


                ]
            }],
        //onInitNewRow: function (e) {
        //    debugger;
        //    e.data.Requestor = new_request.Requestor;

        //    e.data.DEPT = new_request.DEPT;
        //    e.data.Group = new_request.Group;
        //    e.data.Reviewer_1 = new_request.Reviewer_1;
        //    if (e.data.DEPT > 59 && e.data.DEPT < 104)
        //        is_XCselected = true;
        //    else
        //        is_XCselected = false;

        //},

            summary: {
                recalculateWhileEditing: true,
                totalItems: [{
                    column: "Item_Name",
                    summaryType: "count",
                    valueFormat: "number",
                    customizeText: function (e) {
                        //debugger;
                        //I tried add 
                        //console.log(e.value)
                        return "Item Count: " + e.value;
                    }
                },  {
                        column: 'Total_Price',
                    summaryType: 'sum',
                        valueFormat: 'currency',
                        //customizeText: function (e) {
                        //    //debugger;
                        //    //I tried add 
                        //    //console.log(e.value)
                        //    //return "Request Totals: " + e.value;
                        //}
                }],
            },
            onInitNewRow: function (e) {
                //debugger;

            e.data.Requestor = new_request.Requestor;
            if (new_request.BU != 0)
                e.data.BU = new_request.BU;
            if (new_request.OEM != 0)
                e.data.OEM = new_request.OEM;
            if (new_request.Reviewer_2 != 0)
                e.data.Reviewer_2 = new_request.Reviewer_2;
            e.data.DEPT = new_request.DEPT;
            e.data.Group = new_request.Group;
            e.data.Reviewer_1 = new_request.Reviewer_1;
            if (e.data.DEPT > 59 && e.data.DEPT < 104)
                is_XCselected = true;
            else
                is_XCselected = false;


        },

        onRowUpdated: function (e) {
            $.notify(" Your Item Request is being Updated...Please wait!", {
                globalPosition: "top center",
                className: "success"
            })
            Selected = [];
            e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
            e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
            Selected.push(e.data);
            Update(Selected, filtered_yr);
        },

        onRowInserting: function (e) {
            $.notify("New Item is being added to your cart...Please wait!", {
                globalPosition: "top center",
                className: "success"
            })
            e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
            e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
            Selected = [];
            //debugger;
            Selected.push(e.data);



            Update(Selected, filtered_yr);
        },
        onRowRemoving: function (e) {

            Delete(e.data.RequestID, filtered_yr);

        }


    });

}

function OnError_GetData(response) {
    $("#RequestTable").prop('hidden', false);
    $.notify(data.message, {
        globalPosition: "top center",
        className: "warn"
    })
}

function OnSuccess_GetBUSummary(response) {
    var objdata = eval(response.data.data);
    //if (response.message == "") {
    //    busummarytable = $("#BUSummaryTable").dxDataGrid({

    //        loadPanel: {
    //            enabled: true
    //        },
    //        onEditorPreparing: function (e) {

    //            e.editorOptions.disabled = true;
    //        },
    //        dataSource: objdata,
    //        columns: [
    //            {

    //                alignment: "center",
    //                showBorders: true,
    //                columns: [
    //                    {
    //                        dataField: "Category",
    //                        caption: "Category"
    //                    },
    //                    {
    //                        dataField: "AS"

    //                    },
    //                    {
    //                        dataField: "OSS",

    //                    },
    //                    {
    //                        dataField: "DA",

    //                    },
    //                    { dataField: "AD" },

    //                    { dataField: "Two_Wheeler" },

    //                ]
    //            }],

    //    });

    //}
    // else if (response.message == "CC") {
    busummarytable = $("#BUSummaryTable").dxDataGrid({

        loadPanel: {
            enabled: true
        },
        onEditorPreparing: function (e) {

            e.editorOptions.disabled = true;
        },
        dataSource: objdata,
        //columns: [
        //    {

        //        alignment: "center",
        //        showBorders: true,
        //        columns: [
        //            {
        //                dataField: "Category",
        //                caption: "Category"
        //            },
        //            {
        //                dataField: "AS"

        //            },
        //            {
        //                dataField: "OSS",

        //            },
        //            { dataField: "Two_Wheeler" }


        //        ]
        //    }],

    });

    //}
    //else if (response.message == "XC") {
    //    busummarytable = $("#BUSummaryTable").dxDataGrid({

    //        loadPanel: {
    //            enabled: true
    //        },
    //        onEditorPreparing: function (e) {

    //            e.editorOptions.disabled = true;
    //        },
    //        dataSource: objdata,
    //        columns: [
    //            {

    //                alignment: "center",
    //                showBorders: true,
    //                columns: [
    //                    {
    //                        dataField: "Category",
    //                        caption: "Category"
    //                    },

    //                    {
    //                        dataField: "DA",

    //                    },
    //                    { dataField: "AD" },

    //                    { dataField: "Two_Wheeler" },

    //                ]
    //            }],

    //    });

    //}
}


function OnError_GetBUSummary(response) {
    //debugger;
    //$.notify('Unable to load BU Summary right now, Please Try again later!', {
    //    globalPosition: "top center",
    //    className: "warn"
    //});
}



function success_DeptSummaryTable(data) {
    dataObjData = eval(data.data.data);
    deptsummarytable = $("#deptsummarytable").dxDataGrid({

        dataSource: dataObjData,

        loadPanel: {
            enabled: true
        },
        showBorders: true,
        onEditorPreparing: function (e) {

            e.editorOptions.disabled = true;
        }
    });
}

function error_DeptSummaryTable(response) {
    //$.notify('Unable to load Dept Summary right now, Please Try again later!', {
    //    globalPosition: "top center",
    //    className: "warn"
    //});
}




function HoEApprove(id, filtered_yr) {
    //debugger;
    if (confirm('Do you confirm to send this Request Record for Review ?')) {
        var genSpinner = document.querySelector("#SubmitSpinner");
        if (id == 1999999999) {
            genSpinner.classList.add('fa');
            genSpinner.classList.add('fa-spinner');
            genSpinner.classList.add('fa-pulse');
        }

        $.ajax({
            type: "POST",
            url: encodeURI("../BudgetingRequest/HoEApprove"),
            data: { 'id': id, 'useryear': filtered_yr },
            success: function (data) {
                //debugger;
                if (id == 1999999999) {

                    genSpinner.classList.remove('fa');
                    genSpinner.classList.remove('fa-spinner');
                    genSpinner.classList.remove('fa-pulse');
                }


                $.ajax({
                    type: "GET",
                    url: "/BudgetingRequest/GetData",
                    data: { 'year': filtered_yr },
                    datatype: "json",
                    async: true,
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });
                function success_refresh_getdata(response) {
                    //debugger;
                    var getdata = response.data;
                    $("#RequestTable").dxDataGrid({
                        dataSource: getdata
                    });
                }
                function error_refresh_getdata(response) {

                    //$.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                    //    globalPosition: "top center",
                    //    className: "warn"
                    //});

                }


                $.ajax({
                    type: "GET",
                    url: "/BudgetingRequest/GetDeptSummaryData",
                    data: { 'year': filtered_yr },
                    datatype: "json",
                    async: true,
                    success: success_refresh_deptsummary,
                    error: error_refresh_deptsummary

                });
                function success_refresh_deptsummary(response) {

                    var deptsummary = eval(response.data.data);
                    $("#deptsummarytable").dxDataGrid({
                        dataSource: deptsummary
                    });
                }
                function error_refresh_deptsummary(response) {

                    //$.notify('Unable to Refresh Dept Summary right now, Please Try again later!', {
                    //    globalPosition: "top center",
                    //    className: "warn"
                    //});

                }

                //debugger;



                $.ajax({
                    type: "GET",
                    url: encodeURI("../BudgetingRequest/GetBUSummaryData"),
                    data: { 'year': filtered_yr },
                    success: success_refresh_busummary,
                    error: error_refresh_busummary
                });
                function success_refresh_busummary(response) {

                    $("#BUSummaryTable").dxDataGrid({ dataSource: eval(response.data.data) });
                }
                function error_refresh_busummary(response) {
                    //$.notify('Unable to Refresh BU Summary right now, Please Try again later!', {
                    //    globalPosition: "top center",
                    //    className: "warn"
                    //});

                }



                if (data.success == false) {
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "error",
                        autoHideDelay: 13000,
                    })
                }
                else {
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success",
                        autoHideDelay: 10000,
                    })
                }

                //debugger;
                if (data.data) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        url: encodeURI("../Budgeting/SendEmail"),
                        data: { 'emailnotify': data.data },
                        success: success_email,
                        error: error_email
                    });
                }


                function success_email(response) {
                    //debugger;
                    $.notify("Mail has been sent to your L2 Reviewer for further review process!", {
                        globalPosition: "top center",
                        autoHideDelay: 10000,
                        className: "success"
                    })

                }
                function error_email(response) {
                    //debugger;
                    //$.notify("Unable to send mail to your L2 Reviewer for further review process, Please try later!", {
                    //    globalPosition: "top center",
                    //    className: "warn"
                    //})

                }
            }
        });
    }
}

function Update(id1, filtered_yr) {
    //debugger;
    $.ajax({
        type: "POST",
        url: encodeURI("../BudgetingRequest/AddOrEdit"),
        data: { 'req': id1[0], 'useryear': filtered_yr },
        success: function (data) {

            newobjdata = data.data;

            $("#RequestTable").dxDataGrid({ dataSource: newobjdata });

            $.ajax({
                type: "POST",
                url: "/BudgetingRequest/Notify_ifalready_ordered",
                data: {
                    'req': id1[0]
                },
                datatype: "json",
                async: true,
                success: success_Notify_ifalready_ordered
            });
            function success_Notify_ifalready_ordered(response) {
                //debugger;

                if (response.message != "") {
                    $.notify(response.message, {
                        globalPosition: "top right",
                        autoHideDelay: 40000,
                        className: "info"
                    });

                }

            }

            $.ajax({
                type: "GET",
                url: encodeURI("../BudgetingRequest/InitRowValues"),
                success: OnSuccessCall_dnew,
                error: OnErrorCall_dnew

            });

            $.ajax({
                type: "GET",
                url: "/BudgetingRequest/GetDeptSummaryData",
                data: { 'year': filtered_yr },
                datatype: "json",
                async: true,
                success: success_refresh_deptsummary,
                error: error_refresh_deptsummary

            });
            function success_refresh_deptsummary(response) {

                var deptsummary = eval(response.data.data);
                $("#deptsummarytable").dxDataGrid({
                    dataSource: deptsummary
                });
            }
            function error_refresh_deptsummary(response) {

                //$.notify('Unable to Refresh Dept Summary right now, Please Try again later!', {
                //    lobalPosition: "top center",
                //    className: "warn"
                //});
            }

            $.ajax({
                type: "GET",
                url: encodeURI("../BudgetingRequest/GetBUSummaryData"),
                data: { 'year': filtered_yr },

                success: success_refresh_busummary,
                error: error_refresh_busummary
            });
            function success_refresh_busummary(response) {

                $("#BUSummaryTable").dxDataGrid({ dataSource: eval(response.data.data) });
            }
            function error_refresh_busummary(response) {
                //$.notify('Unable to Refresh BU Summary right now, Please Try again later!', {
                //    globalPosition: "top center",
                //    className: "warn"
                //});
            }

            if (data.success == false) {
                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "error",
                    autoHideDelay: 13000,
                })
            }
            else {
                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "success",
                    autoHideDelay: 15000,
                })
            }

        }

    });

}


function Delete(id, filtered_yr) {

    $.ajax({
        type: "POST",
        url: "/BudgetingRequest/Delete",
        data: { 'id': id, 'useryear': filtered_yr },
        success: function (data) {
            newobjdata = data.data;
            $("#RequestTable").dxDataGrid({ dataSource: newobjdata });


            $.ajax({
                type: "GET",
                url: "/BudgetingRequest/GetDeptSummaryData",
                data: { 'year': filtered_yr },
                datatype: "json",
                async: true,
                success: success_refresh_deptsummary,
                error: error_refresh_deptsummary

            });
            function success_refresh_deptsummary(response) {

                var deptsummary = eval(response.data.data);
                $("#deptsummarytable").dxDataGrid({
                    dataSource: deptsummary
                });
            }

            function error_refresh_deptsummary(response) {
                //$.notify('Unable to Refresh Dept Summary right now, Please Try again later!', {
                //    globalPosition: "top center",
                //    className: "warn"
                //});
            }





            $.ajax({
                type: "GET",
                url: encodeURI("../BudgetingRequest/GetBUSummaryData"),
                data: { 'year': filtered_yr },

                success: success_refresh_busummary,
                error: error_refresh_busummary
            });
            function success_refresh_busummary(response) {

                $("#BUSummaryTable").dxDataGrid({ dataSource: eval(response.data.data) });
            }
            function error_refresh_busummary(response) {
                //$.notify('Unable to Refresh BU Summary right now, Please Try again later!', {
                //    globalPosition: "top center",
                //    className: "warn"
                //});
            }


            if (data.success == false) {
                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "error",
                    autoHideDelay: 13000,
                })
            }
            else {
                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "success",
                    autoHideDelay: 10000,
                })
            }


        }

    });
}

function OnSuccessCall_dnew(response) {
    debugger;
    if (response.success == true)
        new_request = response.data;
    else {
        new_request = response.data;
        $.notify("Unable to find " + response.userNT + "'s Reviewer details. Kindly contact SmartLab Tools Team !", {
            globalPosition: "top center",
            className: "error",
            autoHideDelay: 40000,
        });
    }

}
function OnErrorCall_dnew(response) {
    new_request = response.data;
    $.notify("Unable to find " + response.userNT + "'s Reviewer details. Kindly contact SmartLab Tools Team !", {
        globalPosition: "top center",
        className: "error",
        autoHideDelay: 40000,
    });
}

