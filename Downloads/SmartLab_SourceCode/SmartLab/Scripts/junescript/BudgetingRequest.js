            //Javascript file for Budgeting Request Details - mae9cob    


            var dataGridLEP, busummarytable, deptsummarytable;
            var dataObjData, newobjdata;
            var BU_list, Item_list, Category_list, OEM_list, Group_list, CostElement_list;//, OrderStatus_list;
            var Selected = [];
            var unitprice, reviewer_2, category, costelement, leadtime, reviewer_1;
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


            $(".custom-file-input").on("change", function () {
                var fileName = $(this).val().split("\\").pop();
                $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
            });

 


            $.notify('Your Item Requests are being loaded, Please wait!', {
                 globalPosition: "top center",
                 className: "info",
                 autoHideDelay: 10000,
            });
$('input[type=checkbox]').each(function () { this.checked = false; });
//$('#exampleModal').on('shown.bs.modal', function () {
//    $('#exampleModal').trigger('focus')
//})


$.ajax({

    type: "GET",
    url: "/BudgetingRequest/CCXC_CountDown",
    async: false,
    success: onsuccess_countdata,
    error: onerror_countdata
})

    
function onsuccess_countdata(response) {
    //debugger;
    //if (response.data == "XC_Authorised")
    //    countDownDate = new Date("Aug 27, 2021 23:59:59").getTime();       
    //else if (response.data == "CC_Authorised")
    //    countDownDate = new Date("Aug 26, 2022 23:59:59").getTime();
    //else
        countDownDate = new Date("Aug 23, 2022 23:30:00").getTime();
        
    

}

function onerror_countdata(response) {
    
    countDownDate = new Date("Aug 23, 2022 23:30:00").getTime();
}
//            ////region to add the countdown

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

          


            function onsuccess_lookupdata(response) {
    //debugger;
                lookup_data = response.data;
                BU_list = lookup_data.BU_List;
    OEM_list = lookup_data.OEM_List;
    //var dpt = lookup_data.DEPT_List;
    //for (i = 0; i < dpt.length; i++) {
        
    //    if (dpt[i].Outdated == false) {
          
    //        DEPT_list.push(dpt[i]);
    //    }
    //}
                DEPT_list = lookup_data.DEPT_List;
                Group_list = lookup_data.Groups_test;
                Item_list = lookup_data.Item_List;
                Category_list = lookup_data.Category_List;
                CostElement_list = lookup_data.CostElement_List;
                //debugger;
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
            
            $.ajax({
                type: "GET",
                url: encodeURI("../BudgetingRequest/InitRowValues"),
                success: OnSuccessCall_dnew,
                error: OnErrorCall_dnew

            });
            function OnSuccessCall_dnew(response) {
                debugger;
                new_request = response.data;

            }
            function OnErrorCall_dnew(response) {

                //$.notify('Add new error!', {
                //    globalPosition: "top center",
                //    className: "warn"
                //});
            }

    
//Reference the DropDownList for Year to be selected by Requestor
        var ddlYears = document.getElementById("ddlYears");
        //Determine the Current Year.
        var currentYear = (new Date()).getFullYear();

//Loop and add the Year values to DropDownList.
for (var i = currentYear - 1; i <= currentYear+1; i++) {
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
            //debugger;
        }

            //Loading indicator on load of the Request module while fetching the Item Requests
           // window.onload = function () {
                
            //    document.getElementById("loadpanel").style.display = "block";

                
             //   genSpinner_load.classList.add('fa');
             //   genSpinner_load.classList.add('fa-spinner');
            //    genSpinner_load.classList.add('fa-pulse');
            //    $("#RequestTable").prop('hidden', true);
           // };

            

            
           
//filtered_yr = new Date().getFullYear();

if (filtered_yr == null) {
    filtered_yr = new Date().getFullYear();
}

//$('.chkvkm').on('click', function () {
//    debugger;
//    if ($('.chkvkm:checked').length == 0) {
//        debugger;
//        var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
//        dataGridLEP1.columnOption('OEM', 'visible', true);
//        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
//        dataGridLEP1.columnOption('Total_Price', 'visible', true);
//        dataGridLEP1.columnOption('Requestor', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Reviewer_1', 'visible', true);
//        dataGridLEP1.columnOption('Reviewer_2', 'visible', true);
//        dataGridLEP1.columnOption('Comments', 'visible', true);
//        dataGridLEP1.columnOption('Project', 'visible', true);
//        dataGridLEP1.columnOption('Request_Status', 'visible', true);

//        dataGridLEP1.columnOption('Unit_Price', 'visible', true);
//        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
//        dataGridLEP1.beginUpdate();
      
//    } 
//});    

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
        dataGridLEP1.columnOption('Unit_Price', 'visible', false);
        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);

        dataGridLEP1.columnOption('BU', 'visible', true);
        dataGridLEP1.columnOption('DEPT', 'visible', true);
        dataGridLEP1.columnOption('Group', 'visible', true);
        dataGridLEP1.columnOption('Item_Name', 'visible', true); 

        dataGridLEP1.endUpdate();

    }
    else if ($('.chkvkm:checked').length == 2)
    {
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
        dataGridLEP1.columnOption('Unit_Price', 'visible', true);
        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);

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
        dataGridLEP1.columnOption('Unit_Price', 'visible', chkItem);
        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', chkItem);

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
        dataGridLEP1.columnOption('Unit_Price', 'visible', true);
        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);

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
        dataGridLEP1.columnOption('Unit_Price', 'visible', chkItem);
        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', chkItem);

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



        ajaxCallforRequestUI(filtered_yr);


        function fnYearChange(yearselect) {
            //$("#RequestTable").prop('hidden', true);
            //document.getElementById("loadpanel").style.display = "block";

            // genSpinner_load = document.querySelector("#load");
            //genSpinner_load.classList.add('fa');
            //genSpinner_load.classList.add('fa-spinner');
            //genSpinner_load.classList.add('fa-pulse');
            //document.getElementById("loadpanel").style.display = "block";
            filtered_yr = parseInt(yearselect.value);
            filtered_yr = filtered_yr.toString();
            //debugger;
            if (filtered_yr < new Date().getFullYear()) { //2023 < 2021
                $("#btn_summary").prop("hidden", true);
               $("#effect").prop("hidden", true);
            }
            else {
                $("#btn_summary").prop("hidden", false);//2023 < 2022 , 2022 < 2022
                //$("#effect").prop("hidden", false);
            }
               
            debugger;
            if (filtered_yr == new Date().getFullYear() + 1) { //2023 == 2023
                $("#btnSubmitAll").prop("hidden", false);
            }
            else {
                $("#btnSubmitAll").prop("hidden", true);//2023 < 2022 , 2022 < 2022
                //$("#effect").prop("hidden", false);
            }
            ajaxCallforRequestUI(filtered_yr);


        }


            function ajaxCallforRequestUI(filtered_yr) {
                $.ajax({

                    type: "GET",
                    url: "/BudgetingRequest/Lookup",
                    async: false,
                    data: { 'year': filtered_yr },
                    success: onsuccess_lookupdata,
                    error: onerror_lookupdata
                })

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
                
                //const noDataTextUnsafe = " ☺ There is no Item Request in your bucket list. To start with," + " click Add a row option on the top right of VKM Requests ! <img src=1 onerror=alert('XSS') \/>";
                //const encodeMessage = (message) => {
                //    // ...
                //    // Encode the `message` string with your favorite sanitizing tool
                //    // ...
                //    return encodedMessage;
                //};




                //Ajax call to Get BU Summary Data
                //debugger;
                $.ajax({
                    type: "GET",
                    url: encodeURI("../BudgetingRequest/GetBUSummaryData"),
                    data: { 'year': filtered_yr },
                    success: OnSuccess_GetBUSummary,
                    error: OnError_GetBUSummary
                });



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




                //Ajax call to Get Request Item Data
  
                $.ajax({
                    type: "GET",
                    url: encodeURI("../BudgetingRequest/GetData"),
                    data: { 'year': filtered_yr },
                    success: OnSuccess_GetData,
                    error: OnError_GetData
                });


                function OnSuccess_GetData(response) {
                    //debugger;
                    var objdata;
                    //debugger;
                    if (!response.success) {
                        $.notify(response.message, {
                            globalPosition: "top center",
                            autoHideDelay: 13000,
                            className: "error"
                        })
                    }
                    else {
                        //$("#exampleModal").modal('show');
                        //$(".show-modal").click(function () {
                        //    $("#exampleModal").modal({
                        //        backdrop: 'static',
                        //        keyboard: false
                        //    });
                        //});
                        //function alignModal() {
                        //    var modalDialog = $(this).find(".modal-dialog");

                        //    // Applying the top margin on modal to align it vertically center
                        //    modalDialog.css("margin-top", Math.max(0, ($(window).height() - modalDialog.height()) / 2));
                        //}
                        //// Align modal when it is displayed
                        //$(".modal").on("shown.bs.modal", alignModal);

                        //// Align modal when user resize the window
                        //$(window).on("resize", function () {
                        //    $(".modal:visible").each(alignModal);
                        //});   

                        objdata = (response.data);


                        //Hide the Loading indicator once the Request List is fetched

                        //debugger;

                        dataGridLEP = $("#RequestTable").dxDataGrid({

                            dataSource: objdata,
                            keyExpr: "RequestID",
                            //columnResizingMode: "nextColumn",
                            //wordWrapEnabled: true,
                            loadPanel: {
                                enabled: true
                            },
                            hoverStateEnabled: {
                                enabled: true
                            },
                            columnMinWidth: 50,
                            showColumnLines: true,
                            showRowLines: true,
                            rowAlternationEnabled: true,
                            //stateStoring: {
                            //    enabled: true,
                            //    type: "localStorage",
                            //    storageKey: "RequestID"
                            //},
                            columnFixing: {
                                enabled: true
                            },
                            width: "98.03vw", //needed to allow horizontal scroll
                            height: "67vh",
                            columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
                            remoteOperations: true,
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
                            //columnResizingMode: "widget",
                            //columnMinWidth: 100,
                            onToolbarPreparing: function (e) {
                                e.toolbarOptions.items.push({
                                    widget: 'dxButton',
                                    showText: 'always',
                                    options: {
                                        icon: 'find',
                                        text: 'Column Chooser',
                                        onClick: function () {
                                            e.component.showColumnChooser();
                                        }
                                    },
                                    location: 'after'
                                });
                            },
                            onCellPrepared: function (e) {
                                if (e.rowType === "header" || e.rowType === "filter") {
                                    e.cellElement.addClass("columnHeaderCSS");
                                    e.cellElement.find("input").addClass("columnHeaderCSS");
                                }
                            },
                            noDataText: " ☺ No Item ! click '+' Add a row option on the top right of VKM Requests",
                            editing: {           
                                mode: "row",
                                allowUpdating: function (e) {
                                    //debugger;
                                    if (e.row.data.ApprovalHoE == undefined)
                                        e.row.data.ApprovalHoE = false;
                                    return !e.row.data.ApprovalHoE && (filtered_yr == new Date().getFullYear() + 1)
                                },
                                allowDeleting: function (e) {
                                    //debugger;
                                    if (e.row.data.ApprovalHoE == undefined)
                                        e.row.data.ApprovalHoE = false;
                                       // e.row.data.ApprovalHoE = false;
                                    return !e.row.data.ApprovalHoE && (filtered_yr == new Date().getFullYear() + 1)
                                },//true,
                                allowAdding: !countdownflag && (filtered_yr == new Date().getFullYear() + 1)/*true*/,
                                useIcons: true
                            },
                            onToolbarPreparing: function (e) {
                                var dataGrid = e.component;

                                e.toolbarOptions.items[0].showText = 'always';

                                
                            },
                           // focusedRowEnabled: true,
                           
                            columnChooser: {
                                enabled: true
                            },
                           
                            filterRow: {
                                visible: true

                            },
                            showBorders: true,
                            headerFilter: {
                                visible: true,
                                applyFilter: "auto"
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


                                if (e.dataField === "BU") {

                                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                                    e.editorOptions.onValueChanged = function (e) {
                                        onValueChanged.call(this, e);
                                        //debugger;
                                        if (e.value == 1 && is_XCselected == true) {
                                            is_TwoWPselected = true;
                                            BU_forItemFilter = 4;
                                            window.setTimeout(function () {
                                                component.cellValue(rowIndex, "Reviewer_2", "Sheeba Rani R");
                                            }, 1000);
                                        }
                                            
                                        if (e.value == 1 || e.value == 3) {
                                            BU_forItemFilter = 3;
                                        }
                                        else if (e.value == 2 || e.value == 4)
                                            BU_forItemFilter = 4;
                                        //else if (e.value == 4)
                                        //    BU_forItemFilter = 4;
                                        else
                                            BU_forItemFilter = 5;
                                        $.ajax({

                                            type: "post",
                                            url: "/BudgetingRequest/GetReviewer_VKMSPOC",
                                            data: { BU: e.value },
                                            datatype: "json",
                                            traditional: true,
                                            success: function (data) {

                                                reviewer_2 = data;
                                                if (e.value == 1 && is_XCselected == true) {
                                                    is_TwoWPselected = true;
                                                    BU_forItemFilter = 4;
                                                    reviewer_2 = "Sheeba Rani R";

                                                }

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
                                            component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
                                        }, 1000);
                                    }
                                }

                                //debugger;
                                if (e.dataField === "DEPT") {

                                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                                    e.editorOptions.onValueChanged = function (e) {
                                        onValueChanged.call(this, e);
                                        if (is_TwoWPselected && (e.value > 59 && e.value < 104)) {
                                            BU_forItemFilter = 4;
                                            window.setTimeout(function () {
                                                component.cellValue(rowIndex, "Reviewer_2", "Sheeba Rani R");
                                            }, 1000);
                                        }
                                            
                                        //debugger;

                                        $.ajax({

                                            type: "post",
                                            url: "/BudgetingRequest/GetReviewer_HoE",
                                            data: { DEPT: e.value },
                                            datatype: "json",
                                            traditional: true,
                                            success: function (data) {
                                                //debugger;
                                                reviewer_1 = data.data;

                                            }
                                        })


                                        // Emulating a web service call
                                        window.setTimeout(function () {
                                            //debugger;
                                            component.cellValue(rowIndex, "Reviewer_1", reviewer_1);
                                        }, 1000);
                                    }
                                }
                                //Reviewer_2
                                //if (e.dataField === "DEPT") {
                                //    //debugger;
                                //    //var Dept = e.row.data.DEPT;
                                //    //var BU = e.row.data.BU;
                                //}



                                if (e.dataField === "Item_Name") {

                                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                                    e.editorOptions.onValueChanged = function (e) {
                                        onValueChanged.call(this, e);
                                        $.ajax({
                                            type: "post",
                                            url: "/BudgetingRequest/GetUnitPrice",
                                            data: { ItemName: e.value },
                                            datatype: "json",
                                            traditional: true,
                                            success: function (data) {

                                                if (data > 0)
                                                    unitprice = data;

                                            }
                                        })

                                        $.ajax({

                                            type: "post",
                                            url: "/BudgetingRequest/GetCategory",
                                            data: { ItemName: e.value },
                                            datatype: "json",
                                            traditional: true,
                                            success: function (data) {
                                                category = data;

                                            }
                                        })

                                        $.ajax({

                                            type: "post",
                                            url: "/BudgetingRequest/GetCostElement",
                                            data: { ItemName: e.value },
                                            datatype: "json",
                                            traditional: true,
                                            success: function (data) {
                                                costelement = data;

                                            }
                                        })

                                        $.ajax({

                                            type: "post",
                                            url: "/BudgetingRequest/GetActualAvailableQuantity",
                                            data: { ItemName: e.value },
                                            datatype: "json",
                                            traditional: true,
                                            success: function (data) {
                                                //debugger;
                                                actualavailquantity = data;

                                            }
                                        })

                                        window.setTimeout(function () {

                                            component.cellValue(rowIndex, "Unit_Price", unitprice);
                                            component.cellValue(rowIndex, "Category", category);
                                            component.cellValue(rowIndex, "Cost_Element", costelement);
                                            component.cellValue(rowIndex, "ActualAvailableQuantity", actualavailquantity);

                                        },
                                            1000);


                                    }

                                }
                                if (e.dataField === "Required_Quantity") {
                                    debugger;
                                    e.editorOptions.valueChangeEvent = "keyup";

                                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                                    debugger;
                                    e.editorOptions.onValueChanged = function (e) {
                                        debugger;
                                        onValueChanged.call(this, e);
                                        debugger;
                                        var UnitPr_sel = component.cellValue(rowIndex, "Unit_Price");
                                        debugger;
                                        if (component.cellValue(rowIndex, "Unit_Price") != undefined && component.cellValue(rowIndex, "Unit_Price") != null) {
                                            debugger;
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
                                                        debugger;
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
                                            dataField: "BU",
                                            width: 70,
                                            validationRules: [{ type: "required" }],

                                            setCellValue: function (rowData, value) {
                                                //debugger;
                                                rowData.BU = value;

                                            },
                                            lookup: {
                                                dataSource: function (options) {
                                                    //debugger;
                                                    return {

                                                        store: BU_list,
                                                    };

                                                },
                                                valueExpr: "ID",
                                                displayExpr: "BU"
                                            },
                                            allowEditing: !countdownflag
                                        },
                                         


                                        {
                                            dataField: "OEM",
                                            validationRules: [{ type: "required" }],
                                            width: 80,
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
                                            //width: 130,
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
                                            width: 110,
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
                                                    //debugger;
                                                    return {


                                                        store: /*Item_list_bkp*/ /*Item_list_New*/Item_list,

                                                        filter: options.data ? [["BU", "=", BU_forItemFilter != 0 ? BU_forItemFilter : options.data.BU], 'and', ["Deleted", "=", false]] : null
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
                                            lookup: {
                                                dataSource: CostElement_list,
                                                valueExpr: "ID",
                                                displayExpr: "CostElement"
                                            },
                                            allowEditing: false,
                                            visible: false


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
                                            caption:"VKM SPOC",
                                            allowEditing: false,
                                            width: 120,
                                            //visible: false
                                        },
                                        {
                                            dataField: "Comments",
                                            caption:"Remark",
                                            width: 100,

                                        },
                                        {
                                            dataField: "RequestDate",
                                            allowEditing: false,
                                            width: 100,
                                            visible: false


                                        },

                                        //{
                                        //    dataField: "LeadTime",
                                        //    width: 70,
                                        //    caption: "LeadTime (in days)",
                                        //    allowEditing: !countdownflag,
                                        //    calculateCellValue: function (rowData) {
                                        //        //update the LeadTime
                                        //        if (rowData.Item_Name == undefined || rowData.Item_Name == null) {

                                        //            leadtime1 = "";
                                        //        }

                                        //        else {

                                        //            $.ajax({

                                        //                type: "GET",
                                        //                url: "/BudgetingRequest/GetLeadTime",
                                        //                data: { 'ItemName': rowData.Item_Name },
                                        //                datatype: "json",
                                        //                async: false,
                                        //                success: success_getleadtime,

                                        //            });

                                        //            function success_getleadtime(response) {

                                        //                if (response == 0)
                                        //                    leadtime1 = "";
                                        //                else
                                        //                    leadtime1 = response;

                                        //            }

                                        //        }

                                        //        return leadtime1;
                                        //    }

                                        //},
                                        {
                                            dataField: "Request_Status",
                                            caption: "Status",
                                            allowEditing: false
                                            //width: 100

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

                            onInitNewRow: function (e) {
                                debugger;

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
                     
                    

                     
                }

                function OnError_GetData(response) {
                    $("#RequestTable").prop('hidden', false);
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "warn"
                    })
                }

            }

            


            $('#btnSubmitAll').click(function () {
                  HoEApprove(1999999999, filtered_yr);
            });




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
                        data: {'id': id, 'useryear': filtered_yr },
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
                    data: {'req': id1[0], 'useryear': filtered_yr },
                    success: function (data) {

                        newobjdata = data.data;

                        $("#RequestTable").dxDataGrid({dataSource: newobjdata });

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
                                autoHideDelay: 10000,
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

            //BULookup,OEMLookup,DeptLookup,GroupLookup,ItemNameLookup,CostElementLookup,CategoryLookup
            



            




            //Export data
            $("#export").click(function () {
                //debugger;
                $.ajax({

                    type: "POST",
                    url: "/BudgetingRequest/ExportDataToExcel/",
                    data: {'useryear': filtered_yr },


                    success: function (export_result) {

                        var bytes = new Uint8Array(export_result.FileContents);
                        var blob = new Blob([bytes], {type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
                        var link = document.createElement('a');
                        link.href = window.URL.createObjectURL(blob);
                        link.download = export_result.FileDownloadName;
                        link.click();
                        $.notify('Your Item Request list is exported to an excel sheet. Please Open/Save to view the data!', {
                            globalPosition: "top center",
                            className: "success",
                            autoHideDelay: 8000
                        });

                    },
                    error: function () {
                        alert("Issue in Exporting data. Please check again!");
                    }

                });
            });
