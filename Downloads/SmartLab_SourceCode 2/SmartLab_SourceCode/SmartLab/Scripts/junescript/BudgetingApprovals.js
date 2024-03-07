            //Javascript file for Budgeting Approvals Details - mae9cob

                var Selected = [];
                var dataGridLEP, busummarytable, deptsummarytable;
                var dataObjData, newobjdata;
                var BU_list, DEPT_list, Item_list, Category_list, OEM_list, Group_list, CostElement_list, OrderStatus_list;
                var filtered_yr;
var lookup_data;
var genSpinner = document.querySelector("#UploadSpinner");
var is_EditControlsgiven_flag;
//var hoe_data;

//var genSpinner_load = document.querySelector("#load");
//$("#RequestTable").prop('hidden', true);


//genSpinner_load.classList.add('fa');
//genSpinner_load.classList.add('fa-spinner');
//genSpinner_load.classList.add('fa-pulse');
//document.getElementById("loadpanel").style.display = "block";
               //Loading indicator on load of the Approvals module while fetching the Item Requests
               //window.onload = function () {
$('input[type=checkbox]').each(function () { this.checked = false; });               
$.notify('Requests in your Queue for HOE Review is being loaded, Please wait!', {
    globalPosition: "top center",
    className: "info",
    autoHideDelay: 10000,
});            
                   
               //}

               

                function onsuccess_lookupdata(response) {

                    lookup_data = response.data;
                    BU_list = lookup_data.BU_List;
                    OEM_list = lookup_data.OEM_List;
                    DEPT_list = lookup_data.DEPT_List;
                    Group_list = lookup_data.Groups_test;
                    Item_list = lookup_data.Item_List;
                    Category_list = lookup_data.Category_List;
                    CostElement_list = lookup_data.CostElement_List;


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


function spinnerEnable() {
    
    genSpinner.classList.add('fa');
    genSpinner.classList.add('fa-spinner');
    genSpinner.classList.add('fa-pulse');
}

//////*********HOE Proxy********//
//$.ajax({
//    type: "POST",
//    url: encodeURI("../BudgetingApprovals/GetHOE_ProxyDetails"),
//    success: onsuccess_hoeproxy,
//    error: onerror_hoeproxy
//});
//function onsuccess_hoeproxy(data) {
//    debugger;
//    var hoe_data = data.data;

//    $("#RequestTable_HOE").dxDataGrid({
//        dataSource: hoe_data,
             
//            editing: {
//                mode: "row",

//                //allowAdding: true,

//                allowUpdating: function (e) {
//                    return true;
//                },
//                allowDeleting: function (e) {

//                    return true;
//                },
//                useIcons: true
//            },
             
//            allowColumnReordering: true,
//            allowColumnResizing: true,
            
//            filterRow: {
//                visible: true

//            },
//            showBorders: true,
//            headerFilter: {
//                visible: true,
//                applyFilter: "auto"
//            },
//            selection: {
//                applyFilter: "auto"
//            },
//            loadPanel: {
//                enabled: true
//            },
//            paging: {
//                pageSize: 15
//            },
           

//            columns: [
//                {
//                    type: "buttons",
//                    width: 90,
//                    alignment: "left",
//                    buttons: [
//                        "edit", "delete"
//                    ]
//                },
//                {

//                    alignment: "center",
//                    columns: [
//                        {
                            
//                            alignment: "center",
//                            columns: [

//                                "HOE_NTID",

//                                "HOE_FullName",

//                                {
//                                    dataField: "Department",
//                                    validationRules: [{ type: "required" }],
//                                    setCellValue: function (rowData, value) {
//                                        rowData.Department = value;
//                                        rowData.Group = null;

//                                    },

//                                    lookup: {
//                                        dataSource: function (options) {
//                                            return {

//                                                store: DEPT_list
//                                                //filter: options.data ? ["Outdated", "=", true] : null


//                                            };
//                                        },

//                                        valueExpr: "ID",
//                                        displayExpr: "DEPT"

//                                    },
//                                    allowEditing: true


//                                },
//                                "Proxy_NTID",

//                                "Proxy_FullName",
//                                {
//                                    dataField: "Updated_By",
//                                    allowEditing: false
//                                }
//                            ],



//                        }],
//                }],



//            onEditorPreparing: function (e) {



//                var component = e.component,
//                    rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex


//                if (e.dataField === "HOE_NTID") {
//                    debugger;
//                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
//                    e.editorOptions.onValueChanged = function (e) {
//                        onValueChanged.call(this, e);
//                        var FullName, Department, Ntid;



//                        $.ajax({

//                            type: "post",
//                            url: "/BudgetingLabAdmin/GetRequestorDetails_Planning_HOE",
//                            data: { NTID: e.value },
//                            datatype: "json",
//                            traditional: true,
//                            success: function (data) {

//                                if (data.success) {
//                                    FullName = data.data.FullName;
//                                    Department = data.data.Department;
//                                    //Group = data.data.Group;
//                                    Ntid = data.data.NTID;

//                                }
//                                else {
//                                    $.notify(data.message, {
//                                        globalPosition: "top center",
//                                        className: "error",
//                                        autoHideDelay: 8000,
//                                    })
//                                }




//                            }
//                        })
//                        // Emulating a web service call
//                        window.setTimeout(function () {
//                            component.cellValue(rowIndex, "HOE_FullName", FullName);
//                            component.cellValue(rowIndex, "Department", Department);
//                            component.cellValue(rowIndex, "HOE_NTID", Ntid);
//                        }, 1000);
//                    }
//                }


//                if (e.dataField === "Proxy_NTID") {
//                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
//                    e.editorOptions.onValueChanged = function (e) {
//                        onValueChanged.call(this, e);
//                        var FullName, Ntid;
//                        $.ajax({

//                            type: "post",
//                            url: "/BudgetingLabAdmin/GetRequestorDetails_Planning_HOEProxy",
//                            data: { NTID: e.value },
//                            datatype: "json",
//                            traditional: true,
//                            success: function (data) {

//                                if (data.success) {
//                                    FullName = data.data.FullName;
//                                    Ntid = data.data.NTID;

//                                }
//                                else {
//                                    $.notify(data.message, {
//                                        globalPosition: "top center",
//                                        className: "error"
//                                    })
//                                }




//                            }
//                        })
//                        // Emulating a web service call
//                        window.setTimeout(function () {
//                            component.cellValue(rowIndex, "Proxy_FullName", FullName);
//                            component.cellValue(rowIndex, "Proxy_NTID", Ntid);
//                        }, 1000);
//                    }
//                }






//            },
//            onRowUpdated: function (e) {
//                $.notify(" The Details are being Updated...Please wait!", {
//                    globalPosition: "top center",
//                    className: "success",
//                    autoHideDelay: 2000,
//                })
//                Selected = [];

//                Selected.push(e.data);
//                Update1(Selected);
                
//                //}

//            },

//            onRowInserting: function (e) {

//                Selected = [];
//                Selected.push(e.data);



//                Update1(Selected);
               
//            },
//            onRowRemoving: function (e) {
//                debugger;
//                Delete1(e.data.HOE_NTID);

//            }

//         });
//    debugger;
//    }

//function onerror_hoeproxy(data) {
//    debugger;
//        $("#RequestTable_HOE").prop('hidden', false);
//        $.notify("Unable to fetch HOE-Proxy Details, Please Try later!", {
//            globalPosition: "top center",
//            className: "warn"
//        })
//    }

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
            debugger;
        }
        
        if (filtered_yr == null) {
    filtered_yr = new Date().getFullYear();
}
var chkRequest;
var chkItem;
$('#chkRequest').on('click', function () {
  
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
        dataGridLEP1.columnOption('Reviewer_1', 'visible', false);
        dataGridLEP1.columnOption('Reviewer_2', 'visible', false);
        dataGridLEP1.columnOption('SubmitDate', 'visible', false);
        dataGridLEP1.columnOption('Comments', 'visible', true);
        dataGridLEP1.columnOption('Project', 'visible', false);
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
        dataGridLEP1.columnOption('Reviewer_1', 'visible', false);
        dataGridLEP1.columnOption('Reviewer_2', 'visible', false);
        dataGridLEP1.columnOption('SubmitDate', 'visible', false);
        dataGridLEP1.columnOption('Comments', 'visible', true);
        dataGridLEP1.columnOption('Project', 'visible', false);
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
    debugger;
            //$("#RequestTable").prop('hidden', true);
            //document.getElementById("loadpanel").style.display = "block";

            // genSpinner_load = document.querySelector("#load");
            //genSpinner_load.classList.add('fa');
            //genSpinner_load.classList.add('fa-spinner');
            //genSpinner_load.classList.add('fa-pulse');
            //document.getElementById("loadpanel").style.display = "block";
            filtered_yr = parseInt(yearselect.value);
            filtered_yr = filtered_yr.toString();
   
    if (filtered_yr < new Date().getFullYear()) {
        $("#btn_summary").prop("hidden", true);
        $("#effect").prop("hidden", true);
    }
    else {
        $("#btn_summary").prop("hidden", false);
        //$("#effect").prop("hidden", false);
    }
    debugger;
    if (filtered_yr == new Date().getFullYear() + 1) { //2023 == 2023
        $("#btnApproveAll").prop("hidden", false);
    }
    else {
        $("#btnApproveAll").prop("hidden", true);//2023 < 2022 , 2022 < 2022
        //$("#effect").prop("hidden", false);
    }
            ajaxCallforRequestUI(filtered_yr);


        }

 $.ajax({ 

                    type: "GET",
                    url: "/BudgetingApprovals/LookupApprovals",
                     async: false,
                     data: { 'year': filtered_yr },
                    success: onsuccess_lookupdata,
                    error: onerror_lookupdata
                }) 


//Ajax call to Get BU Summary Data
debugger;
             


function OnSuccess_GetBUSummary(response) {

    var objdata = eval(response.data.data);
    //if (response.message == "") {
    //    busummarytable = $("#BUSummaryTable").dxDataGrid({

    //        loadPanel: {
    //            enabled: true
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

    //        onEditorPreparing: function (e) {

    //            e.editorOptions.disabled = true;
    //        }

    //    });

    //}
   // else if (response.message == "CC") {
        busummarytable = $("#BUSummaryTable").dxDataGrid({

            loadPanel: {
                enabled: true
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

            //            { dataField: "Two_Wheeler" },

            //        ]
            //    }],

            onEditorPreparing: function (e) {

                e.editorOptions.disabled = true;
            }

        });

   // }
    //else if (response.message == "XC") {
    //    busummarytable = $("#BUSummaryTable").dxDataGrid({

    //        loadPanel: {
    //            enabled: true
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

    //        onEditorPreparing: function (e) {

    //            e.editorOptions.disabled = true;
    //        }

    //    });

    //}
}
              function OnError_GetBUSummary(response) {

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

                    //$.notify('Unable to load Dept Summary right now, Please wait for a while!', {
                    //    globalPosition: "top center",
                    //    className: "warn"
                    //});
              }


            //Ajax call to Get Item Data waiting for approval
function ajaxCallforRequestUI(filtered_yr) {
    $.ajax({

        type: "POST",
        url: encodeURI("../BudgetingApprovals/GetData"),

        data: { 'year': filtered_yr },
        success: OnSuccess_GetData,
        error: OnError_GetData
    });

    //Ajax call to Get Dept Summary Data
    $.ajax({
        type: "GET",
        url: "/BudgetingApprovals/GetDeptSummaryData",
        datatype: "json",
        data: { 'year': filtered_yr },
        async: true,
        success: success_DeptSummaryTable,
        error: error_DeptSummaryTable
    });

    $.ajax({
        type: "GET",
        url: encodeURI("../BudgetingApprovals/GetBUSummaryData"),
        data: { 'year': filtered_yr },
        success: OnSuccess_GetBUSummary,
        error: OnError_GetBUSummary
    });

}
             

                function OnSuccess_GetData(response) {
    
    //if (response.is_ProxyButtonHide) {
    //    $("#proxysetting").prop('hidden', true);
    //}
    if (response.success && response.message) {
        $.notify(response.message, {
            globalPosition: "top center",
            className: "success",
            autoHideDelay: 13000,
        })
    }
    if (!response.success && response.message) {
        $.notify(response.message, {
            globalPosition: "top center",
            className: "error",
            autoHideDelay: 13000,
        })
    }

    is_EditControlsgiven_flag = response.is_EditControlsgiven;
    debugger;
                    var objdata = (response.data);
               
                    
                    

                    dataGridLEP = $("#RequestTable").dxDataGrid({

                        dataSource: objdata,
                        //wordWrapEnabled: true,
                        editing: {
                            mode: "row",
                            allowUpdating: function (e) {
                                    return (filtered_yr == new Date().getFullYear() + 1)
                            },
                                


                            useIcons: true
                        },
                        allowColumnReordering: true,
                        allowColumnResizing: true,
                        columnChooser: {
                            enabled: true
                        },
                        noDataText: " ☺ No VKM Item Request is available in your queue. Kindly note that either all requests are reviewed or request not submitted yet ! ",
                        keyExpr: "RequestID",
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
                            width: "98vw", //needed to allow horizontal scroll
                            height: "72vh",
                            columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
                            remoteOperations: true,
                            scrolling: {
                                mode: "virtual",
                                rowRenderingMode: "virtual",
                                columnRenderingMode: "virtual"
                            },
                        columnResizingMode: "nextColumn",
                        columnMinWidth: 50,
                        onCellPrepared: function (e) {
                            if (e.rowType === "header" || e.rowType === "filter") {
                                e.cellElement.addClass("columnHeaderCSS");
                                e.cellElement.find("input").addClass("columnHeaderCSS");
                            }
                        },
                        //stateStoring: {
                        //    enabled: true,
                        //    type: "localStorage",
                        //    storageKey: "RequestID"
                        //},
                        onContentReady: function (e) {
                            debugger;
                            e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
                        },
                       // focusedRowEnabled: true,
                        showBorders: true,
                        filterRow: {
                            visible: true

                        },
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
                        loadPanel: {
                            enabled: true
                        },
                       
                        columns: [
                        {
                                type: "buttons",
                                width: 90,
                                alignment: "left",
                                fixed: true,
                                fixedPosition: "left",
                            buttons: [
                                "edit",
                                {
                                    hint: "Submit Item",
                                    icon: "check",
                                    visible: function (e) {
                                        return !e.row.isEditing && (filtered_yr == new Date().getFullYear() + 1);
                                    },
                                    onClick: function (e) {
                                        SHApprove(e.row.data.RequestID);
                                        e.component.refresh(true);
                                        e.event.preventDefault();
                                    }
                                },
                                {
                                    hint: "Send Back Item",
                                    icon: "fa fa-send",
                                    visible: function (e) {
                                        return !e.row.isEditing && (filtered_yr == new Date().getFullYear() + 1);
                                    },
                                    onClick: function (e) {
                                        SendBack(e.row.data.RequestID);
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
                                        validationRules: [{type: "required" }],
                                        //width: 70,
                                        lookup: {
                                            dataSource: BU_list,
                                            valueExpr: "ID",
                                            displayExpr: "BU"
                                        },
                                        allowEditing: false

                                    },
                                    {
                                        dataField: "OEM",
                                        validationRules: [{type: "required" }],
                                        width:70,
                                        lookup: {
                                            dataSource: OEM_list,
                                            valueExpr: "ID",
                                            displayExpr: "OEM"
                                        },
                                        allowEditing: false
                                    },
                                    {
                                        dataField: "DEPT",
                                        caption: "Dept",
                                        validationRules: [{ type: "required" }],
                                        setCellValue: function (rowData, value) {

                                            rowData.DEPT = value;
                                            rowData.Group = null;

                                        },
                                        lookup: {
                                            dataSource: function (options) {

                                                return {

                                                    store: DEPT_list,


                                                };
                                            },

                                            valueExpr: "ID",
                                            displayExpr: "DEPT"

                                        },
                                        allowEditing: false

                                    },

                                    {
                                        dataField: "Group",
                                        validationRules: [{type: "required" }],
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
                                        allowEditing: false
                                    },
                                 {
                                     dataField: "Project",
                                     width: 100,
                                     allowEditing: false,
                                     visible: false
                                  

                                 },

                                    {
                                        dataField: "Item_Name",
                                        caption: "Item",
                                        validationRules: [{type: "required" }],
                                        lookup: {
                                            dataSource: function (options) {

                                                return {

                                                    store: Item_list,
                                                    filter: options.data ? ["Deleted", "=", false] : null
                                                    
                                                };

                                            },

                                            valueExpr: "S_No",
                                            displayExpr: "Item_Name"
                                        },
                                        allowEditing: false,
                                    minWidth: 250

                                 },
                                 {
                                     dataField: "ActualAvailableQuantity",
                                     caption: "Available Qty",
                                     allowEditing: false,
                                     width:110

                                    },
                                    {
                                        dataField: "Category",
                                        caption: "Category",
                                        validationRules: [{type: "required" }],

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
                                        validationRules: [
                                        {type: "required" },
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
                                        allowEditing: false

                                    },
                                    {
                                        dataField: "Unit_Price",
                                        caption: "Unit Price",
                                        dataType: "number",
                                        format: {type: "currency", precision: 0 },
                                        valueFormat: "#0",
                                        allowEditing: false,
                                        validationRules: [{type: "required" }, {
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
                                        caption: "Amt",
                                        calculateCellValue: function (rowData) {

                                            if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
                                                return rowData.Unit_Price * rowData.Required_Quantity;
                                            }
                                            else
                                                return 0.0;
                                        },

                                        dataType: "number",
                                        format: {type: "currency", precision: 0 },
                                        valueFormat: "#0",
                                        allowEditing: false
                                    },


                                    {
                                        dataField: "Reviewed_Quantity",
                                        width: 140,
                                        caption: "Review Qty",
                                        validationRules: [
                                        {type: "required" },
                                        {
                                            type: "range",
                                            message: "Please enter valid count > 0",
                                            min: 0.01,
                                            max: 214783647
                                        }],
                                        dataType: "number",
                                        setCellValue: function (rowData, value) {

                                            rowData.Reviewed_Quantity = value;

                                        }



                                    },
                                    {
                                        dataField: "Reviewed_Cost",
                                        caption: "Review Amt",
                                        calculateCellValue: function (rowData) {

                                            if (rowData.Reviewed_Quantity > 0 && rowData.Unit_Price > 0) {
                                                return rowData.Unit_Price * rowData.Reviewed_Quantity;
                                            }
                                            else
                                                return 0.0;
                                        },

                                        dataType: "number",
                                        format: {type: "currency", precision: 0 },
                                        valueFormat: "#0",
                                        allowEditing: false
                                    },


                                    {
                                        dataField: "Requestor",
                                        allowEditing: false,
                                        width:120
                                    },

                                    {
                                        dataField: "Comments",
                                        caption: "Remark",
                                    },
                                    {
                                        dataField: "SubmitDate",
                                        caption: "Submit Dt",
                                        allowEditing: false,
                                        visible: false
                                    },

                                    {
                                        dataField: "Reviewer_1",
                                        allowEditing: false,
                                        caption: "HoE",
                                        visible: false
                                    },
                                    {
                                        dataField: "Reviewer_2",
                                        caption: "VKM SPOC",
                                        allowEditing: false,
                                        visible: false
                                 }
                                 //,
                                 //{
                                 //    dataField: "HOEView_ActionHistory",
                                 //    allowEditing: false,
                                     
                                 //}
                                 

                                    ]
                            }],
                        onEditorPreparing: function (e) {
                            var component = e.component,
                                rowindex = e.row && e.row.rowindex;

                            if (e.dataField === "Reviewed_Quantity") {
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
                                                    component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                                                }, 1000);
                                                //}


                                            }
                                        })
                                        //// Emulating a web service call

                                    }


                                }
                            }

                        },
                        onRowUpdated: function (e) {
                                $.notify("Item in your Queue is being Updated...Please wait!", {
                                    globalPosition: "top center",
                                    className: "success"
                                })
                                Selected = [];
                                e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
                            e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
                            debugger;
                                Selected.push(e.data);
                                Update(Selected);
                        }

                    }).dxDataGrid('instance').refresh();

                    //$("#RequestTable").prop('hidden', false);

                    //genSpinner_load.classList.remove('fa');
                    //genSpinner_load.classList.remove('fa-spinner');
                    //genSpinner_load.classList.remove('fa-pulse');
                    //document.getElementById("loadpanel").style.display = "none"
                }

function OnError_GetData(response) {
    //$.notify(response.message, {
    //                    globalPosition: "top center",
    //                    className: "warn"
    //                })
}   





                function Update(id1) {

                    $.ajax({
                        type: "POST",
                        url: encodeURI("../BudgetingApprovals/AddOrEdit"),
                        data: { 'req': id1[0] },
                        success: function (data) {


                            newobjdata = data.data;

                            $("#RequestTable").dxDataGrid({ dataSource: newobjdata });

                            $.ajax({
                                type: "GET",
                                url: "/BudgetingApprovals/GetDeptSummaryData",
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
                                //globalPosition: "top center",
                                //className: "warn"
                                //});
                            }


                            $.ajax({
                                type: "GET",
                                url: encodeURI("../BudgetingApprovals/GetBUSummaryData"),
                                data: { 'year': filtered_yr },
                                success: success_refresh_busummary,
                                error: error_refresh_busummary
                            });

                            function success_refresh_busummary(response) {

                                $("#BUSummaryTable").dxDataGrid({ dataSource: eval(response.data.data) });
                            }
                            function error_refresh_busummary(response) {
                                //$.notify('Unable to Refresh BU Summary right now, Please Try again later!', {
                                //globalPosition: "top center",
                                //className: "warn"
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


                function SendBack(id) {
                    if(confirm('Are You Sure to Send Back this Request Record ?'))
                    {
                    
                        $.ajax({
                            type: "POST",
           
                            url: encodeURI("../BudgetingApprovals/Sendback"),
                            data: { 'id': id },
                            success: function (data) {
                                debugger;
                                $.ajax({
                                    type: "POST",
                                    url: "/BudgetingApprovals/GetData",
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

                                $.ajax({
                                    type: "GET",
                                    url: "/BudgetingApprovals/GetDeptSummaryData",
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
                                    url: encodeURI("../BudgetingApprovals/GetBUSummaryData"),
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



                                if (data.success) {
                                    $.notify(data.message, {
                                        globalPosition: "top center",
                                        className: "success",
                                        autoHideDelay: 10000
                                    })
                                }
                                else if (!data.success) {
                                    $.notify(data.message, {
                                        globalPosition: "top center",
                                        className: "error",
                                        autoHideDelay: 13000
                                    })
                                }
                                

                                if (data.data) {
                                    $.ajax({
                                        type: "POST",
                                        url: encodeURI("../Budgeting/SendEmail"),
                                        data: { 'emailnotify': data.data },
                                        success: success_email,
                                        error: error_email
                                    });

                                    function success_email(response) {
                                        $.notify("Mail has been sent to the Requestor on the sent back item!", {
                                            globalPosition: "top center",
                                            className: "success",
                                            autoHideDelay: 13000,
                                        })

                                    }
                                    function error_email(response) {
                                        //$.notify("Unable to send mail to the Requestor on the sent back item!", {
                                        //    globalPosition: "top center",
                                        //    className: "warn"
                                        //})

                                    }
                                }
                            }

                        });

                    }
                }

            


                $('#btnApproveAll').click(function () {
                    SHApprove(1999999999);
    
                });



                 function SHApprove(id) {
    
        
                    if (confirm('Do you confirm to move this Request Record for Section VKM review ?')) {
                        var genSpinner = document.querySelector("#SubmitSpinner");
                        if (id == 1999999999) {
           
                            genSpinner.classList.add('fa');
                            genSpinner.classList.add('fa-spinner');
                            genSpinner.classList.add('fa-pulse');
                        }
       
                        $.ajax({
                            type: "POST",
                            url: encodeURI("../BudgetingApprovals/SHApprove"),
                            data: { 'id': id },

                            success: function (data) {
                                debugger;
               
                                if (id == 1999999999) {
                    
                                    genSpinner.classList.remove('fa');
                                    genSpinner.classList.remove('fa-spinner');
                                    genSpinner.classList.remove('fa-pulse');
                                }
                
                
                                $.ajax({
                                    type: "POST",
                                    url: "/BudgetingApprovals/GetData",
                                    datatype: "json",
                                    async: true,
                                    data: { 'year': filtered_yr },
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


                                $.ajax({
                                    type: "GET",
                                    url: "/BudgetingApprovals/GetDeptSummaryData",
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
                                    url: encodeURI("../BudgetingApprovals/GetBUSummaryData"),
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

                               
                                if (data.success) {
                                    $.notify(data.message, {
                                        globalPosition: "top center",
                                        className: "success",
                                        autoHideDelay: 10000
                                    })
                                }
                                else if (!data.success) {
                                    $.notify(data.message, {
                                        globalPosition: "top center",
                                        className: "error",
                                        autoHideDelay: 13000
                                    })
                                }

                                if (data.data) {
                                    $.ajax({
                                        type: "POST",
                                        url: encodeURI("../Budgeting/SendEmail"),
                                        data: {
                                            'emailnotify': data.data
                                        },
                                        success: success_email,
                                        error: error_email
                                    });
                                    function success_email(response) { 

                                        $.notify("Mail has been sent to your L3 Reviewer for further review process!", {
                                            globalPosition: "top center",
                                            className: "success",
                                            autoHideDelay: 13000,
                                        })

                                    }
                                    function error_email(response) {

                                        //$.notify("Unable to send mail to your L3 Reviewer for further review process, Please try later!", {
                                        //    globalPosition: "top center",
                                        //    className: "warn"
                                        //})

                                    }
                                }
                                

                

                
                            }

                        });

                    }
                 }


                 $('#btnrequests').click(function () {
                     var url='/BudgetingRequest/Index';
                     window.location.href=url;
                });


                $('[data-toggle="tooltip"]').tooltip();

           
                $("#buttonClearFilters").dxButton({
                    text: 'Clear Filters',
                    onClick: function () {
                        $("#RequestTable").dxDataGrid("clearFilter");
                    }
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
                //        }, 30000);
                //    };


                //    // Set effect from select menu value
                //    $("#btn_summary").on("click", function () {
                //        runEffect();
                //    });


                //    $("#effect").hide();
                //});
$("#btn_summary").on("click", function () {
    debugger;
    $("#effect").prop("hidden", false);
    var e = document.getElementById("effect");
    debugger;
    if (e.style.display == 'block')
        e.style.display = 'none';
    else
        e.style.display = 'block';
});




                //Export data
                $("#export").click(function () {
  
                    var genSpinner = document.querySelector("#ExportSpinner");

                    genSpinner.classList.add('fa');
                    genSpinner.classList.add('fa-spinner');
                    genSpinner.classList.add('fa-pulse');





                    $.ajax({

                        type: "POST",
                        url: "/BudgetingApprovals/ExportDataToExcel/",
                        data: { 'useryear': filtered_yr },
                        success: function (export_result) {
                            debugger;

                            if (!export_result.success) {
                                $.notify("Error: " + export_result.message, {
                                    globalPosition: "top center",
                                    className: "warn"
                                });
                                genSpinner.classList.remove('fa');
                                genSpinner.classList.remove('fa-spinner');
                                genSpinner.classList.remove('fa-pulse');
                            }
                            else {
                                var bytes = new Uint8Array(export_result.data.FileContents);
                                var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
                                var link = document.createElement('a');
                                link.href = window.URL.createObjectURL(blob);
                                link.download = export_result.data.FileDownloadName;
                                link.click();

                                genSpinner.classList.remove('fa');
                                genSpinner.classList.remove('fa-spinner');
                                genSpinner.classList.remove('fa-pulse');

                                $.notify('Your Request list is exported to an excel sheet. Please Open/Save to view the data!', {
                                    globalPosition: "top center",
                                    className: "success",
                                    autoHideDelay: 8000
                                });
                            }
                            

                        },
                        error: function () {
                            alert("Error in Export");
                        }

                    });
                });

$('#proxysetting').click(function () {
    debugger;
    $.ajax({

        type: "POST",
        url: "/BudgetingApprovals/ChangeButtonName_enabledisable/",

        success: function (data) {
            debugger;
            if (data.data == undefined || data.data == null)
                document.getElementById("proxy_enabledisable").innerHTML = "Enable Proxy";
            else if (data.data == false)
                document.getElementById("proxy_enabledisable").innerHTML = "Enable Proxy";
            else if (data.data == true)
                document.getElementById("proxy_enabledisable").innerHTML = "Disable Proxy";

        }

    });

    var e = document.getElementById("effect1");
    
    if (e.style.display == 'block')
        e.style.display = 'none';
    else
        e.style.display = 'block';
    
});


function Update1(id1) {
    debugger;


    $.ajax({
        type: "POST",
        url: encodeURI("../BudgetingLabAdmin/AddOrEdit_VKMPlanning_HOE"),
        data: { 'req': id1[0] },
        success: function (data) {
            debugger;
            //newobjdata = data.data;

            //$("#RequestTable").dxDataGrid({dataSource: newobjdata });
            $.ajax({
                type: "POST",
                url: "/BudgetingApprovals/GetHOE_ProxyDetails",
                datatype: "json",
                success: success_refresh_getdata1,
                error: error_refresh_getdata1

            });

            function success_refresh_getdata1(response) {
                debugger;
                var getdata = response.data;
                $("#RequestTable_HOE").dxDataGrid({
                    dataSource: getdata
                });
            }
            function error_refresh_getdata1(response) {

                //$.notify('Unable to Refresh HOE List right now, Please Try again later!', {
                //    globalPosition: "top center",
                //    className: "warn"
                //});

            }



            if (data.success) {
                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "success",
                    autoHideDelay: 13000,
                })
            }
            else {
                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "error"
                })
            }



        }

    });


}


function Delete1(id) {

    debugger;
    $.ajax({
        type: "POST",
        url: "/BudgetingLabAdmin/Delete_VKMPlanning_HOE",
        data: { 'ntid': id },
        success: function (data) {
            //newobjdata = data.data;
            //$("#RequestTable_HOE").dxDataGrid({ dataSource: newobjdata });
            $.ajax({
                type: "POST",
                url: "/BudgetingApprovals/GetHOE_ProxyDetails",
                datatype: "json",
                success: success_refresh_getdata1,
                error: error_refresh_getdata1

            });

            function success_refresh_getdata1(response) {
                debugger;
                var getdata = response.data;
                $("#RequestTable_HOE").dxDataGrid({
                    dataSource: getdata
                });
            }
            function error_refresh_getdata1(response) {

                //$.notify('Unable to Refresh HOE List right now, Please Try again later!', {
                //    globalPosition: "top center",
                //    className: "warn"
                //});

            }


            $.notify(data.message, {
                globalPosition: "top center",
                className: "success",
                autoHideDelay: 3000
            })
        }



    });



}


$('#proxy_enabledisable').click(function () {
    debugger;
    

    $.ajax({

        type: "POST",
        url: "/BudgetingApprovals/ChangeFlag_enabledisable/",
        async: false,
        data: { 'is_enableDisable': document.getElementById("proxy_enabledisable").innerHTML },
        success: function (data) {
            debugger;

            if (data.success == true) {
                if (data.message) {
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success",
                        autoHideDelay: 10000,
                    })
                }
               
               

                $.ajax({
                    type: "POST",
                    url: "/BudgetingApprovals/GetData",
                    datatype: "json",
                    data: { 'year': filtered_yr },
                    async: false,
                    success: OnSuccess_GetData,
                    error: OnError_GetData

                });
                if (document.getElementById("proxy_enabledisable").innerHTML == "Enable Proxy")
                    document.getElementById("proxy_enabledisable").innerHTML = "Disable Proxy";
                else
                    document.getElementById("proxy_enabledisable").innerHTML = "Enable Proxy";
            }
            if (data.success == false && data.message) {
                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "error",
                    autoHideDelay: 10000,
                })
                $.ajax({
                    type: "POST",
                    url: "/BudgetingApprovals/GetData",
                    data: { 'year': filtered_yr },
                    datatype: "json",
                    async: false,
                    success: OnSuccess_GetData,
                    error: OnError_GetData

                });
            }
            genSpinner.classList.remove('fa');
            genSpinner.classList.remove('fa-spinner');
            genSpinner.classList.remove('fa-pulse');

           

        },
        error: function (data) {
            debugger;
            $.notify("Unable change Proxy settings, Please Try again later!", {
                globalPosition: "top center",
                className: "warn",
                autoHideDelay: 8000,
            })

            $.ajax({
                type: "POST",
                url: "/BudgetingApprovals/GetData",
                data: { 'year': filtered_yr },
                datatype: "json",
                async: false,
                success: OnSuccess_GetData,
                error: OnError_GetData

            });

            genSpinner.classList.remove('fa');
            genSpinner.classList.remove('fa-spinner');
            genSpinner.classList.remove('fa-pulse');

        }

    });

    
    //function success_refresh_getdata(response) {

    //    var getdata = response.data;
    //    $("#RequestTable").dxDataGrid({
    //        dataSource: getdata
    //    });
    //}
    //function error_refresh_getdata(response) {

    //    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
    //        globalPosition: "top center",
    //        className: "warn"
    //    });

    //}



   

});

        