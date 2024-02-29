//Javascript file for Budgeting VKM Details - mae9cob
var ChildList;
var dataGridLEP, busummarytable, deptsummarytable, sectionsummarytable;
var BU_list, DEPT_list, Item_list, Category_list, OEM_list, Group_list, CostElement_list, OrderStatus_list, Fund_list, BudgetCodeList, PurchaseType_list, UOM_list, UnloadingPoint_list, OrderType_list, BudgetCodedesc, RFOApprover_list, CostCenter_list, BudgetCenter_list, Currency_list, BudgetCodeList, OrderDescription_list;

//var BU_list, DEPT_list, Item_list, Category_list, OEM_list, Group_list, CostElement_list, OrderStatus_list, Fund_list, Currency_list, BudgetCodeList, PurchaseType_list;
var dataObjData, dataObjData_section, newobjdata, SRBuyer_list, SRManager_list;
var Selected = [];
var unitprice, reviewer_2, category, costelement, BudgetCode;
var lookup_data, filtered_yr, flag, popupedit, gridedit, allowadd, editmode;
var genSpinner_load = document.querySelector("#load");
var genSpinner_load_masterlist = document.querySelector("#load_masterlist");

var dataGrid;
var BU_forItemFilter = 0;

var is_TwoWPselected = false;
var is_XCselected = false;
var RevCost = 0;
var is_newitem = false;
var isPurchaseSPOC = false;
var Item_headerFilter, DEPT_headerFilter, Group_headerFilter, BU_headerFilter, OEM_headerFilter, Category_headerFilter, CostElement_headerFilter, OrderStatus_headerFilter, BudgetCode_headerFilter, OrderDescription_headerFilter;
var Item_FilterRow;
var AvailUnusedAmt = 0;
var file; //uploaded file info is stored in this variable
var popupContentTemplate;

var RequestSource, RequestToOrder, VKMSPOC_Approval, Reviewed_Quantity, RequestorNTID;
//var prev_orderingDept, prev_orderingGroup;
var reqIDarray = [];   
var justDeselected;

//var genSpinner_load_reqlist1 = document.querySelector("#load_reqlist1");
//var genSpinner_load_reqlist = document.querySelector("#load_reqlist");



IsVKMSpoc();
function convert(str) {
    //debugger;
    var date = new Date(str),
        mnth = ("0" + (date.getMonth() + 1)).slice(-2),
        day = ("0" + date.getDate()).slice(-2);
    return [mnth, day, date.getFullYear(),].join("/");
}
function IsVKMSpoc() {
    $.ajax({

        type: "GET",
        url: "/BudgetingVKM/Get_IsVKMSpoc",
        datatype: "json",
        async: true,
        success: function (data) {

            if (data.gridadd == 1) {
                allowadd = true;
            }
          
            popupedit = data.popupedit;
            gridedit = data.gridedit;
            if (data.popupedit == 1) {
                editmode = "popup";
                //BGSW
                document.getElementById("POImport").style.display = "none";
            }
            else if (data.gridedit == 1) {
                editmode = "row";
            }

            if (data.data == "1") {

                $('#btnOk').prop('disabled', false);
                $('#btnCancel').prop('disabled', false);

                $('#days').dxNumberBox({
                    value: 1,
                    min: 1,
                    max: 15,
                    showSpinButtons: true,
                    disabled: false,
                });

                $('#hours').dxNumberBox({
                    value: 0,
                    min: 0,
                    max: 24,
                    showSpinButtons: true,
                    disabled: false,
                });

                $('#minutes').dxNumberBox({
                    value: 0,
                    min: 0,
                    max: 60,
                    showSpinButtons: true,
                    disabled: false,
                });
            }
            else {

                $('#btnOk').prop('disabled', true);
                $('#btnCancel').prop('disabled', true);

                $('#days').dxNumberBox({
                    value: 1,
                    min: 1,
                    max: 15,
                    showSpinButtons: true,
                    disabled: true,
                });

                $('#hours').dxNumberBox({
                    value: 0,
                    min: 0,
                    max: 24,
                    showSpinButtons: true,
                    disabled: true,
                });

                $('#minutes').dxNumberBox({
                    value: 0,
                    min: 0,
                    max: 60,
                    showSpinButtons: true,
                    disabled: true,
                });
            }
        },
        error: function (data) {
            //debugger;

            $('#days').dxNumberBox({
                value: 1,
                min: 1,
                max: 15,
                showSpinButtons: true,
                disabled: true,
            });

            $('#hours').dxNumberBox({
                value: 0,
                min: 0,
                max: 24,
                showSpinButtons: true,
                disabled: true,
            });

            $('#minutes').dxNumberBox({
                value: 0,
                min: 0,
                max: 60,
                showSpinButtons: true,
                disabled: true,
            });
        }
    });

}

$('input[type=checkbox]').each(function () { this.checked = false; });

$.ajax({
    type: "POST",
    url: encodeURI("../Budgeting/is_PurchaseSPOC"),
    async: false,
    success: OnSuccess1
});
function OnSuccess1(data) {

    isPurchaseSPOC = true;
    document.getElementById("btn_summary").style.display = "block";
    if (data.success) {
        document.getElementById("VKMTitle").innerHTML = "Ordering Team View";
        document.getElementById("multiSelect_Submit").style.display = "none";
        document.getElementById("VKMTitle").style.fontWeight = "700";
        document.getElementById("VKMTitle").style.fontSize = "45";
        document.getElementById("VKMTitle_icon").style.display = "block";
        document.getElementById("btnApproveAll").style.display = "none"; //no Submit All Button view for Purchse SPOC & VKMAdmin
        document.getElementById("btn_summary").style.display = "none";
        document.getElementById("POImport").style.display = "block";
        $('#RequestTable_VKMSPOC').css("marginTop", "5px");
        $(".hide").prop("hidden", false);
        $.notify('Kindly ensure that the template is downloaded and provided with purchase details on every import!', {
            globalPosition: "top center",
            autoHideDelay: 30000,
            className: "success"
        });

    }
    else {
        isPurchaseSPOC = false;
        document.getElementById("VKMTitle").innerHTML = "VKM SPOC Reviews";
       // document.getElementById("multiSelect_Submit").style.display = "block";
        document.getElementById("VKMTitle").style.fontWeight = "700";
        document.getElementById("VKMTitle").style.fontSize = "45";
        document.getElementById("VKMTitle").style.display = "block";
        document.getElementById("bonaparte").style.display = "none";
        document.getElementById("btnConfigure").style.display = "none";
        document.getElementById("VKMTitle_icon").style.display = "block";
        $('#RequestTable_VKMSPOC').css("marginTop", "0px");
        $(".hide").prop("hidden", true);
        document.getElementById("btn_summary").style.display = "block";

        document.getElementById("POImport").style.display = "none";
        $.notify('Requests in your Queue for VKM SPOC Review is being loaded, Please wait!', {
            globalPosition: "top center",
            className: "info",
            autoHideDelay: 10000,
        });
    }
}


function spinnerEnable() {
    var genSpinner = document.querySelector("#UploadSpinner");
    genSpinner.classList.add('fa');
    genSpinner.classList.add('fa-spinner');
    genSpinner.classList.add('fa-pulse');
}

//on import a file - file name displayed in placeholder

$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});
//$("#RequestTable_VKMSPOC").prop('hidden', true);

//genSpinner_load.classList.add('fa');
//genSpinner_load.classList.add('fa-spinner');
//genSpinner_load.classList.add('fa-pulse');
//document.getElementById("loadpanel").style.display = "block"; 
//window.onload = function () {
//document.getElementById("DeptDetails").style.display = "none";
//$("#updateDeptGrpButton").prop('hidden', true);
////$("#ddlDepts").prop('hidden', true);
////document.getElementById("ddlDepts").style.display = "none";
//$(".selectpicker").selectpicker('refresh');
//$("#Depts").prop('hidden', true);


//$.ajax({

//    type: "GET",
//    url: "/BudgetingVKM/LookupVKM",
//    async: false,
//    success: onsuccess_lookupdata,
//    error: onerror_lookupdata
//})


//function onsuccess_lookupdata(response) {

//    lookup_data = response.data;
//    BU_list = lookup_data.BU_List;
//    OEM_list = lookup_data.OEM_List;
//    DEPT_list = lookup_data.DEPT_List;
//    Group_list = lookup_data.Groups_List;
//    Item_list = lookup_data.Item_List;
//    Category_list = lookup_data.Category_List;
//    CostElement_list = lookup_data.CostElement_List;
//    OrderStatus_list = lookup_data.OrderStatus_List; 
//    Fund_list = lookup_data.Fund_List;
//    Currency_list = lookup_data.Currency_List;


//}

//function onerror_lookupdata(response) {
//    alert("Error lookup");

//}

//}


//DEPT MAPPING CHANGE LIST DROPDOWN


//var ddlDepts = document.getElementById("ddlDepts");
//for (var i = 0; i < DEPT_list.length; i++) {
//    var option = document.createElement("OPTION");
//    option.innerHTML = DEPT_list[i].DEPT;
//    option.value = DEPT_list[i].ID;
//    ddlDepts.appendChild(option);
//}
////debugger;
//var ddlGroups = document.getElementById("ddlGroups");
//for (var i = 0; i < Group_list.length; i++) {
//    var option = document.createElement("OPTION");
//    option.innerHTML = Group_list[i].Group;
//    option.value = Group_list[i].ID;
//    ddlGroups.appendChild(option);
//}


//Reference the DropDownList for Year to be selected by Requestor
var ddlYears = document.getElementById("ddlYears");
//Determine the Current Year.
var currentYear = (new Date()).getFullYear();

//Loop and add the Year values to DropDownList.
for (var i = currentYear + 1; i >= 2021; i--) {
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
    filtered_yr = parseInt(filtered_yr) - 1;
    filtered_yr = filtered_yr.toString();
    ////debugger;
}
//Loop and add the Year values to DropDownList.





//$('#chkRequest').on('click', function () {
//    var chkRequest;
//    if (this.checked)
//        chkRequest = true;
//    else
//        chkRequest = false;
//    var dataGridLEP1 = $("#RequestTable_VKMSPOC").dxDataGrid("instance");
//    if ($('.chkvkm:checked').length == 0) {
//        //debugger;
//        var dataGridLEP1 = $("#RequestTable_VKMSPOC").dxDataGrid("instance");
//        dataGridLEP1.beginUpdate();
//        dataGridLEP1.columnOption('OEM', 'visible', true);
//        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
//        dataGridLEP1.columnOption('Total_Price', 'visible', false);
//        dataGridLEP1.columnOption('Requestor', 'visible', false);
//        dataGridLEP1.columnOption('Reviewer_1', 'visible', false);
//        dataGridLEP1.columnOption('Reviewer_2', 'visible', false);
//        dataGridLEP1.columnOption('SubmitDate', 'visible', false);
//        dataGridLEP1.columnOption('Comments', 'visible', true);
//        dataGridLEP1.columnOption('Project', 'visible', false);
//        dataGridLEP1.columnOption('RequestDate', 'visible', false);
//        dataGridLEP1.columnOption('Request_Status', 'visible', false);
//        dataGridLEP1.columnOption('Category', 'visible', false);
//        dataGridLEP1.columnOption('Cost_Element', 'visible', false);
//        dataGridLEP1.columnOption('Unit_Price', 'visible', false);
//        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);

//        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', false);
//        dataGridLEP1.columnOption('LeadTime', 'visible', false);
//        dataGridLEP1.columnOption('OrderStatus', 'visible', true);
//        dataGridLEP1.columnOption('RequiredDate', 'visible', false);
//        dataGridLEP1.columnOption('RequestOrderDate', 'visible', false);
//        dataGridLEP1.columnOption('OrderDate', 'visible', false);
//        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', false);
//        dataGridLEP1.columnOption('OrderID', 'visible', false);
//        dataGridLEP1.columnOption('OrderPrice', 'visible', false);
//        dataGridLEP1.columnOption('OrderedQuantity', 'visible', false);

//        dataGridLEP1.columnOption('Customer_Name', 'visible', false);
//        dataGridLEP1.columnOption('Customer_Dept', 'visible', false);
//        dataGridLEP1.columnOption('BM_Number', 'visible', false);
//        dataGridLEP1.columnOption('Task_ID', 'visible', false);
//        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', false);
//        dataGridLEP1.columnOption('PIF_ID', 'visible', false);
//        dataGridLEP1.endUpdate();

//    }
//    else {
//        dataGridLEP1.beginUpdate();
//        dataGridLEP1.columnOption('OEM', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Required_Quantity', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Total_Price', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Requestor', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Reviewer_1', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Reviewer_2', 'visible', chkRequest);
//        dataGridLEP1.columnOption('SubmitDate', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Comments', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Project', 'visible', chkRequest);
//        dataGridLEP1.endUpdate();
//    }
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
//   //// $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
//   // //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
//   // //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


//});

//$('#chkItem').on('click', function () {
//    var chkItem;
//    if (this.checked)
//        chkItem = true;
//    else
//        chkItem = false;
//    var dataGridLEP1 = $("#RequestTable_VKMSPOC").dxDataGrid("instance");
//    if ($('.chkvkm:checked').length == 0) {
//        //debugger;
//        var dataGridLEP1 = $("#RequestTable_VKMSPOC").dxDataGrid("instance");
//        dataGridLEP1.beginUpdate();
//        dataGridLEP1.columnOption('OEM', 'visible', true);
//        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
//        dataGridLEP1.columnOption('Total_Price', 'visible', false);
//        dataGridLEP1.columnOption('Requestor', 'visible', false);
//        dataGridLEP1.columnOption('Reviewer_1', 'visible', false);
//        dataGridLEP1.columnOption('Reviewer_2', 'visible', false);
//        dataGridLEP1.columnOption('SubmitDate', 'visible', false);
//        dataGridLEP1.columnOption('Comments', 'visible', true);
//        dataGridLEP1.columnOption('Project', 'visible', false);
//        dataGridLEP1.columnOption('RequestDate', 'visible', false);
//        dataGridLEP1.columnOption('Request_Status', 'visible', false);
//        dataGridLEP1.columnOption('Category', 'visible', false);
//        dataGridLEP1.columnOption('Cost_Element', 'visible', false);
//        dataGridLEP1.columnOption('Unit_Price', 'visible', false);
//        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);

//        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', false);
//        dataGridLEP1.columnOption('LeadTime', 'visible', false);
//        dataGridLEP1.columnOption('OrderStatus', 'visible', true);
//        dataGridLEP1.columnOption('RequiredDate', 'visible', false);
//        dataGridLEP1.columnOption('RequestOrderDate', 'visible', false);
//        dataGridLEP1.columnOption('OrderDate', 'visible', false);
//        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', false);
//        dataGridLEP1.columnOption('OrderID', 'visible', false);
//        dataGridLEP1.columnOption('OrderPrice', 'visible', false);
//        dataGridLEP1.columnOption('OrderedQuantity', 'visible', false);

//        dataGridLEP1.columnOption('Customer_Name', 'visible', false);
//        dataGridLEP1.columnOption('Customer_Dept', 'visible', false);
//        dataGridLEP1.columnOption('BM_Number', 'visible', false);
//        dataGridLEP1.columnOption('Task_ID', 'visible', false);
//        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', false);
//        dataGridLEP1.columnOption('PIF_ID', 'visible', false);
//        dataGridLEP1.endUpdate();

//    }
//    else {
//        dataGridLEP1.beginUpdate();
//        dataGridLEP1.columnOption('Category', 'visible', chkItem);
//        dataGridLEP1.columnOption('Cost_Element', 'visible', chkItem);
//        dataGridLEP1.columnOption('Unit_Price', 'visible', chkItem);
//        dataGridLEP1.endUpdate();
//    }
//    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
//    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
//    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


//});
var chkRequest;
var chkItem;
var chkProjected;
var chkRFO;
var chkNonVKM;
$('#chkProjected').on('click', function () {

    if (this.checked)
        chkProjected = true;
    else
        chkProjected = false;
    checkboxdata();

});

$('#chkRequest').on('click', function () {

    if (this.checked)
        chkRequest = true;
    else
        chkRequest = false;
    checkboxdata();
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
    //// $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
    // //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
    // //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


});

$('#chkItem').on('click', function () {

    if (this.checked)
        chkItem = true;
    else
        chkItem = false;
    checkboxdata();
    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


});


$('#chkRFO').on('click', function () {

    if (this.checked)
        chkRFO = true;
    else
        chkRFO = false;
    checkboxdata();
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO); -- tking more time
    //// $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
    //// $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
    //// $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'OrderID', 'visible', chkRFO);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'OrderPrice', 'visible', chkRFO);
    // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'OrderedQuantity', 'visible', chkRFO);



});

$('#chkNonVKM').on('click', function () {

    if (this.checked)
        chkNonVKM = true;
    else
        chkNonVKM = false;

    checkboxdata();
    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Customer_Name', 'visible', NonVKM);
    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Customer_Dept', 'visible', NonVKM);
    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'BM_Number', 'visible', NonVKM);
    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Task_ID', 'visible', NonVKM);
    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Resource_Group_Id', 'visible', NonVKM);
    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'PIF_ID', 'visible', NonVKM);




});

function checkboxdata() {

    if ($('.chkvkm:checked').length == 0) {
        //debugger;
        var dataGridLEP1 = $("#RequestTable_VKMSPOC").dxDataGrid("instance");
        dataGridLEP1.beginUpdate();
        dataGridLEP1.columnOption('OEM', 'visible', true);
        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
        dataGridLEP1.columnOption('Total_Price', 'visible', false);
        dataGridLEP1.columnOption('Requestor', 'visible', false);
        dataGridLEP1.columnOption('Reviewer_1', 'visible', false);
        dataGridLEP1.columnOption('Reviewer_2', 'visible', false);
        dataGridLEP1.columnOption('SubmitDate', 'visible', false);
        dataGridLEP1.columnOption('Comments', 'visible', true);
        dataGridLEP1.columnOption('Project', 'visible', false);
        dataGridLEP1.columnOption('RequestDate', 'visible', false);
        dataGridLEP1.columnOption('Request_Status', 'visible', false);
        dataGridLEP1.columnOption('Category', 'visible', false);
        dataGridLEP1.columnOption('Cost_Element', 'visible', false);
        dataGridLEP1.columnOption('BudgetCode', 'visible', false);
        dataGridLEP1.columnOption('Unit_Price', 'visible', false);
        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);


        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', false);
        dataGridLEP1.columnOption('LeadTime', 'visible', false);
        //dataGridLEP1.columnOption('OrderStatus', 'visible', true);
        dataGridLEP1.columnOption('RequiredDate', 'visible', false);
        dataGridLEP1.columnOption('RequestOrderDate', 'visible', false);
        dataGridLEP1.columnOption('OrderDate', 'visible', false);
        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', false);
        dataGridLEP1.columnOption('OrderID', 'visible', false);
        dataGridLEP1.columnOption('Currency', 'visible', false);
        dataGridLEP1.columnOption('SR_Value', 'visible', false);
        dataGridLEP1.columnOption('PR_Value', 'visible', false);
        dataGridLEP1.columnOption('Invoice_Value', 'visible', false);
        dataGridLEP1.columnOption('OrderPrice_UserInput', 'visible', false);
        dataGridLEP1.columnOption('OrderPrice', 'visible', false);
        dataGridLEP1.columnOption('OrderedQuantity', 'visible', false);

        dataGridLEP1.columnOption('Customer_Name', 'visible', false);
        dataGridLEP1.columnOption('Customer_Dept', 'visible', false);
        dataGridLEP1.columnOption('BM_Number', 'visible', false);
        dataGridLEP1.columnOption('Task_ID', 'visible', false);
        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', false);
        dataGridLEP1.columnOption('PIF_ID', 'visible', false);

        dataGridLEP1.columnOption('BU', 'visible', true);
        dataGridLEP1.columnOption('DEPT', 'visible', true);
        dataGridLEP1.columnOption('Group', 'visible', true);
        dataGridLEP1.columnOption('Item_Name', 'visible', true);

        dataGridLEP1.columnOption('isProjected', 'visible', false);
        dataGridLEP1.columnOption('Q1', 'visible', false);
        dataGridLEP1.columnOption('Q2', 'visible', false);
        dataGridLEP1.columnOption('Q3', 'visible', false);
        dataGridLEP1.columnOption('Q4', 'visible', false);
        dataGridLEP1.columnOption('Projected_Amount', 'visible', false);
        dataGridLEP1.columnOption('Unused_Amount', 'visible', false);

        dataGridLEP1.endUpdate();

    }
    else if (('.chkvkm:checked').length == $('.chkvkm').length) {//chk if purchase spoc / vkm spoc
        var dataGridLEP1 = $("#RequestTable_VKMSPOC").dxDataGrid("instance");
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
        dataGridLEP1.columnOption('Category', 'visible', true);
        dataGridLEP1.columnOption('Cost_Element', 'visible', true);
        dataGridLEP1.columnOption('BudgetCode', 'visible', true);
        dataGridLEP1.columnOption('Unit_Price', 'visible', true);
        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);


        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('LeadTime', 'visible', chkRFO);
        //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRFO);
        dataGridLEP1.columnOption('RequiredDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
        dataGridLEP1.columnOption('Currency', 'visible', chkRFO);
        dataGridLEP1.columnOption('SR_Value', 'visible', chkRFO);
        dataGridLEP1.columnOption('PR_Value', 'visible', chkRFO);
        dataGridLEP1.columnOption('Invoice_Value', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderPrice_UserInput', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);
        dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('POSpocNTID', 'visible', chkRFO);
        dataGridLEP1.columnOption('ELOSubmittedDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('DaysTaken', 'visible', chkRFO);
        dataGridLEP1.columnOption('SRSubmitted', 'visible', chkRFO);
        dataGridLEP1.columnOption('RFQNumber', 'visible', chkRFO);
        dataGridLEP1.columnOption('PRNumber', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('SRAwardedDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('SRApprovalDays', 'visible', chkRFO);
        dataGridLEP1.columnOption('SRResponsibleBuyerNTID', 'visible', chkRFO);
        dataGridLEP1.columnOption('SRManagerNTID', 'visible', chkRFO);
        dataGridLEP1.columnOption('Customer_Name', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Customer_Dept', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('BM_Number', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Task_ID', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('PIF_ID', 'visible', chkNonVKM);

        dataGridLEP1.columnOption('SupplierName_with_Address', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Purchase_Type', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Project_ID', 'visible', chkNonVKM);

        dataGridLEP1.columnOption('BU', 'visible', true);
        dataGridLEP1.columnOption('DEPT', 'visible', true);
        dataGridLEP1.columnOption('Group', 'visible', true);
        dataGridLEP1.columnOption('Item_Name', 'visible', true);

        dataGridLEP1.columnOption('isProjected', 'visible', true);
        dataGridLEP1.columnOption('Q1', 'visible', true);
        dataGridLEP1.columnOption('Q2', 'visible', true);
        dataGridLEP1.columnOption('Q3', 'visible', true);
        dataGridLEP1.columnOption('Q4', 'visible', true);
        dataGridLEP1.columnOption('Projected_Amount', 'visible', true);
        dataGridLEP1.columnOption('Unused_Amount', 'visible', true);

        /*BGSW RFO FIELDS*/
        dataGridLEP1.columnOption('OrderType', 'visible', true);
        dataGridLEP1.columnOption('CostCenter', 'visible', true);
        dataGridLEP1.columnOption('BudgetCenterID', 'visible', true);
        dataGridLEP1.columnOption('UnitofMeasure', 'visible', true);
        dataGridLEP1.columnOption('UnloadingPoint', 'visible', true);
        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', true);
        dataGridLEP1.columnOption('LabName', 'visible', true);
        dataGridLEP1.columnOption('RFOReqNTID', 'visible', true);
        dataGridLEP1.columnOption('RFOApprover', 'visible', true);
        dataGridLEP1.columnOption('QuoteAvailable', 'visible', true);
        dataGridLEP1.columnOption('GoodsRecID', 'visible', true);

        dataGridLEP1.columnOption('Material_Part_Number', 'visible', true);
        dataGridLEP1.columnOption('SupplierName_with_Address', 'visible', true);
        dataGridLEP1.columnOption('Purchase_Type', 'visible', true);
        dataGridLEP1.columnOption('Project_ID', 'visible', true);


        dataGridLEP1.endUpdate();
    }
    else {
        var dataGridLEP1 = $("#RequestTable_VKMSPOC").dxDataGrid("instance");
        dataGridLEP1.beginUpdate();
        dataGridLEP1.columnOption('OEM', 'visible', chkRequest || chkProjected);
        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
        dataGridLEP1.columnOption('Total_Price', 'visible', true);
        dataGridLEP1.columnOption('Requestor', 'visible', chkRequest);
        dataGridLEP1.columnOption('Reviewer_1', 'visible', chkRequest);
        dataGridLEP1.columnOption('Reviewer_2', 'visible', chkRequest);
        dataGridLEP1.columnOption('SubmitDate', 'visible', chkRequest);
        dataGridLEP1.columnOption('Comments', 'visible', chkRequest);
        dataGridLEP1.columnOption('Project', 'visible', chkRequest);

        dataGridLEP1.columnOption('Category', 'visible', chkItem);
        dataGridLEP1.columnOption('Cost_Element', 'visible', chkItem || chkProjected);
        dataGridLEP1.columnOption('BudgetCode', 'visible', chkItem || chkProjected);
        dataGridLEP1.columnOption('Unit_Price', 'visible', chkItem);
        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', chkItem);

        dataGridLEP1.columnOption('BU', 'visible', chkRequest);
        dataGridLEP1.columnOption('DEPT', 'visible', chkRequest);
        dataGridLEP1.columnOption('Group', 'visible', chkRequest);
        dataGridLEP1.columnOption('Item_Name', 'visible', true);

        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('LeadTime', 'visible', chkRFO);
        //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRequest);
        dataGridLEP1.columnOption('RequiredDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
        dataGridLEP1.columnOption('Currency', 'visible', chkRFO);
        dataGridLEP1.columnOption('SR_Value', 'visible', chkRFO);
        dataGridLEP1.columnOption('PR_Value', 'visible', chkRFO);
        dataGridLEP1.columnOption('Invoice_Value', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderPrice_UserInput', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO || chkProjected);
        dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);
        dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('POSpocNTID', 'visible', chkRFO);
        dataGridLEP1.columnOption('ELOSubmittedDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('DaysTaken', 'visible', chkRFO);
        dataGridLEP1.columnOption('SRSubmitted', 'visible', chkRFO);
        dataGridLEP1.columnOption('RFQNumber', 'visible', chkRFO);
        dataGridLEP1.columnOption('PRNumber', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('SRAwardedDate', 'visible', chkRFO);
        dataGridLEP1.columnOption('SRApprovalDays', 'visible', chkRFO);
        dataGridLEP1.columnOption('SRResponsibleBuyerNTID', 'visible', chkRFO);
        dataGridLEP1.columnOption('SRManagerNTID', 'visible', chkRFO);
        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);

        dataGridLEP1.columnOption('Customer_Name', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Customer_Dept', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('BM_Number', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Task_ID', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('PIF_ID', 'visible', chkNonVKM);


        dataGridLEP1.columnOption('isProjected', 'visible', chkProjected);
        dataGridLEP1.columnOption('Q1', 'visible', chkProjected);
        dataGridLEP1.columnOption('Q2', 'visible', chkProjected);
        dataGridLEP1.columnOption('Q3', 'visible', chkProjected);
        dataGridLEP1.columnOption('Q4', 'visible', chkProjected);
        dataGridLEP1.columnOption('Projected_Amount', 'visible', chkProjected);
        dataGridLEP1.columnOption('Unused_Amount', 'visible', chkProjected);

        /*BGSW RFO FIELDS*/
        dataGridLEP1.columnOption('OrderType', 'visible', chkRFO );
        dataGridLEP1.columnOption('CostCenter', 'visible', chkRFO );
        dataGridLEP1.columnOption('BudgetCenterID', 'visible', chkRFO );
        dataGridLEP1.columnOption('UnitofMeasure', 'visible', chkRFO );
        dataGridLEP1.columnOption('UnloadingPoint', 'visible', chkRFO );
        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', chkItem);
        dataGridLEP1.columnOption('LabName', 'visible', chkRFO );
        dataGridLEP1.columnOption('RFOReqNTID', 'visible', chkRFO );
        dataGridLEP1.columnOption('RFOApprover', 'visible', chkRFO );
        dataGridLEP1.columnOption('QuoteAvailable', 'visible', chkRFO );
        dataGridLEP1.columnOption('GoodsRecID', 'visible', chkRFO );


        dataGridLEP1.columnOption('Material_Part_Number', 'visible', chkNonVKM );
        dataGridLEP1.columnOption('SupplierName_with_Address', 'visible', chkNonVKM );
        dataGridLEP1.columnOption('Purchase_Type', 'visible', chkNonVKM );
        dataGridLEP1.columnOption('Project_ID', 'visible', chkNonVKM );

        dataGridLEP1.endUpdate();
    }
}
//$('#chkRFO').on('click', function () {
//    var chkRFO;
//    if (this.checked)
//        chkRFO = true;
//    else
//        chkRFO = false;
//    var dataGridLEP1 = $("#RequestTable_VKMSPOC").dxDataGrid("instance");
//    if ($('.chkvkm:checked').length == 0) {
//        //debugger;
//        var dataGridLEP1 = $("#RequestTable_VKMSPOC").dxDataGrid("instance");
//        dataGridLEP1.beginUpdate();
//        dataGridLEP1.columnOption('OEM', 'visible', true);
//        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
//        dataGridLEP1.columnOption('Total_Price', 'visible', false);
//        dataGridLEP1.columnOption('Requestor', 'visible', false);
//        dataGridLEP1.columnOption('Reviewer_1', 'visible', false);
//        dataGridLEP1.columnOption('Reviewer_2', 'visible', false);
//        dataGridLEP1.columnOption('SubmitDate', 'visible', false);
//        dataGridLEP1.columnOption('Comments', 'visible', true);
//        dataGridLEP1.columnOption('Project', 'visible', false);
//        dataGridLEP1.columnOption('RequestDate', 'visible', false);
//        dataGridLEP1.columnOption('Request_Status', 'visible', false);
//        dataGridLEP1.columnOption('Category', 'visible', false);
//        dataGridLEP1.columnOption('Cost_Element', 'visible', false);
//        dataGridLEP1.columnOption('Unit_Price', 'visible', false);
//        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);

//        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', false);
//        dataGridLEP1.columnOption('LeadTime', 'visible', false);
//        dataGridLEP1.columnOption('OrderStatus', 'visible', true);
//        dataGridLEP1.columnOption('RequiredDate', 'visible', false);
//        dataGridLEP1.columnOption('RequestOrderDate', 'visible', false);
//        dataGridLEP1.columnOption('OrderDate', 'visible', false);
//        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', false);
//        dataGridLEP1.columnOption('OrderID', 'visible', false);
//        dataGridLEP1.columnOption('OrderPrice', 'visible', false);
//        dataGridLEP1.columnOption('OrderedQuantity', 'visible', false);

//        dataGridLEP1.columnOption('Customer_Name', 'visible', false);
//        dataGridLEP1.columnOption('Customer_Dept', 'visible', false);
//        dataGridLEP1.columnOption('BM_Number', 'visible', false);
//        dataGridLEP1.columnOption('Task_ID', 'visible', false);
//        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', false);
//        dataGridLEP1.columnOption('PIF_ID', 'visible', false);
//        dataGridLEP1.endUpdate();

//    }
//    else {
//        dataGridLEP1.beginUpdate();
//        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', chkRFO);
//        dataGridLEP1.columnOption('LeadTime', 'visible', chkRFO);
//        //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRFO);
//        dataGridLEP1.columnOption('RequiredDate', 'visible', chkRFO);
//        //dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
//        dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
//        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', chkRFO);
//        dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
//        dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
//        dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);
//        dataGridLEP1.endUpdate();
//    }
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO); -- tking more time
//   //// $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
//   //// $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
//   //// $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'OrderID', 'visible', chkRFO);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'OrderPrice', 'visible', chkRFO);
//   // $('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'OrderedQuantity', 'visible', chkRFO);



//});

//$('#chkNonVKM').on('click', function () {
//    var NonVKM;
//    if (this.checked)
//        NonVKM = true;
//    else
//        NonVKM = false;
//    var dataGridLEP1 = $("#RequestTable_VKMSPOC").dxDataGrid("instance");
//    if ($('.chkvkm:checked').length == 0) {
//        //debugger;
//        var dataGridLEP1 = $("#RequestTable_VKMSPOC").dxDataGrid("instance");
//        dataGridLEP1.beginUpdate();
//        dataGridLEP1.columnOption('OEM', 'visible', true);
//        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
//        dataGridLEP1.columnOption('Total_Price', 'visible', false);
//        dataGridLEP1.columnOption('Requestor', 'visible', false);
//        dataGridLEP1.columnOption('Reviewer_1', 'visible', false);
//        dataGridLEP1.columnOption('Reviewer_2', 'visible', false);
//        dataGridLEP1.columnOption('SubmitDate', 'visible', false);
//        dataGridLEP1.columnOption('Comments', 'visible', true);
//        dataGridLEP1.columnOption('Project', 'visible', false);
//        dataGridLEP1.columnOption('RequestDate', 'visible', false);
//        dataGridLEP1.columnOption('Request_Status', 'visible', false);
//        dataGridLEP1.columnOption('Category', 'visible', false);
//        dataGridLEP1.columnOption('Cost_Element', 'visible', false);
//        dataGridLEP1.columnOption('Unit_Price', 'visible', false);
//        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);

//        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', false);
//        dataGridLEP1.columnOption('LeadTime', 'visible', false);
//        dataGridLEP1.columnOption('OrderStatus', 'visible', true);
//        dataGridLEP1.columnOption('RequiredDate', 'visible', false);
//        dataGridLEP1.columnOption('RequestOrderDate', 'visible', false);
//        dataGridLEP1.columnOption('OrderDate', 'visible', false);
//        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', false);
//        dataGridLEP1.columnOption('OrderID', 'visible', false);
//        dataGridLEP1.columnOption('OrderPrice', 'visible', false);
//        dataGridLEP1.columnOption('OrderedQuantity', 'visible', false);

//        dataGridLEP1.columnOption('Customer_Name', 'visible', false);
//        dataGridLEP1.columnOption('Customer_Dept', 'visible', false);
//        dataGridLEP1.columnOption('BM_Number', 'visible', false);
//        dataGridLEP1.columnOption('Task_ID', 'visible', false);
//        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', false);
//        dataGridLEP1.columnOption('PIF_ID', 'visible', false);
//        dataGridLEP1.endUpdate();

//    }
//    else {
//        dataGridLEP1.beginUpdate();
//        dataGridLEP1.columnOption('Customer_Name', 'visible', NonVKM);
//        dataGridLEP1.columnOption('Customer_Dept', 'visible', NonVKM);
//        dataGridLEP1.columnOption('BM_Number', 'visible', NonVKM);
//        dataGridLEP1.columnOption('Task_ID', 'visible', NonVKM);
//        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', NonVKM);
//        dataGridLEP1.columnOption('PIF_ID', 'visible', NonVKM);
//        dataGridLEP1.endUpdate();
//    }
//    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Customer_Name', 'visible', NonVKM);
//    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Customer_Dept', 'visible', NonVKM);
//    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'BM_Number', 'visible', NonVKM);
//    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Task_ID', 'visible', NonVKM);
//    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'Resource_Group_Id', 'visible', NonVKM);
//    //$('#RequestTable_VKMSPOC').dxDataGrid('columnOption', 'PIF_ID', 'visible', NonVKM);




//});


//At load, Display the data for Current year
if (filtered_yr == null) {
    filtered_yr = new Date().getFullYear();
}
ajaxCallforRequestUI(filtered_yr);


//Function to change year from dropdown
function fnYearChange(yearselect) {


    //Submit all option will be available for vkm spocs always, since during ordering also, need to approve f02 200 series - hence commented 
    //if (parseInt(yearselect.value) == new Date().getFullYear() + 1) { //2023 == 2023
    //    $("#btnApproveAll").prop("hidden", false);
    //}
    //else {
    //    $("#btnApproveAll").prop("hidden", true);//2023 < 2022 , 2022 < 2022
    //}

    filtered_yr = parseInt(yearselect.value) - 1;

    filtered_yr = filtered_yr.toString();
    ajaxCallforRequestUI(filtered_yr);


}



$(".selectpicker").selectpicker('refresh');
//Ajax call to Get Request Item Data, BU and Dept Summary
function ajaxCallforRequestUI(filtered_yr) {
    $(':checkbox').prop('checked', false); //to uncheck every pg refresh

    $.ajax({

        type: "GET",
        url: "/BudgetingVKM/LookupVKM",
        async: false,
        data: { 'year': filtered_yr },
        success: onsuccess_lookupdata,
        error: onerror_lookupdata
    })


    function onsuccess_lookupdata(response) {
        //debugger;
        lookup_data = response.data;
        BU_list = lookup_data.BU_List;
        OEM_list = lookup_data.OEM_List;
        DEPT_list = lookup_data.DEPT_List;
        //if (lookup_data.Groups_List != null)
        //    Group_list = lookup_data.Groups_List;
        //else
        Group_list = lookup_data.Groups_test;//Groups_oldList;
       // Item_list = lookup_data.Item_List;

        Item_headerFilter = lookup_data.Item_HeaderFilter;
        DEPT_headerFilter = lookup_data.DEPT_HeaderFilter;
        Group_headerFilter = lookup_data.Group_HeaderFilter;
        BU_headerFilter = lookup_data.BU_HeaderFilter;
        OEM_headerFilter = lookup_data.OEM_HeaderFilter;
        Category_headerFilter = lookup_data.Category_HeaderFilter;
        CostElement_headerFilter = lookup_data.CostElement_HeaderFilter;
        OrderStatus_headerFilter = lookup_data.OrderStatus_HeaderFilter;
        BudgetCode_headerFilter = lookup_data.BudgetCode_HeaderFilter;
        OrderDescription_headerFilter = lookup_data.OrderDescription_HeaderFilter;

        //  Item_FilterRow = lookup_data.Item_FilterRow;
        //debugger;
        Category_list = lookup_data.Category_List;
        CostElement_list = lookup_data.CostElement_List;
        OrderStatus_list = lookup_data.OrderStatus_List;
        Fund_list = lookup_data.Fund_List;
        Currency_list = lookup_data.Currency_List;
        BudgetCodeList = lookup_data.BudgetCodeList;
        SRBuyer_list = lookup_data.SRBuyerList;
        SRManager_list = lookup_data.SRManagerList;
        PurchaseType_list = lookup_data.PurchaseType_List;
        UnloadingPoint_list = lookup_data.UnloadingPoint_List;
        OrderType_list = lookup_data.Order_Type_List;
        UOM_list = lookup_data.UOM_List;
        //RFOApprover_list = lookup_data.RFOApprover_List;
        //CostCenter_list = lookup_data.CostCenter_List;
        BudCenter = lookup_data.BudgetCenter_List;
        OrderDescription_list = lookup_data.OrderDescription_List;
        ////debugger;

        $.ajax({

            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../BudgetingOrder/Lookup_ItemList",
            async: false,
            //data: { 'year': filtered_yr },
            data: JSON.stringify({ year: filtered_yr }),
            dataType: 'json',
            success: function (data) {
                //debugger;
                Item_list = data;
            },
            error: function (jqXHR, exception) {
                //debugger;
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




    //Dept Summary
    $.ajax({
        type: "GET",
        url: "/BudgetingVKM/GetDeptSummaryData",
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
            noDataText: "For VKM 2021, the above mentioned Departments are not applicable !",
            loadPanel:
            {
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
        //globalPosition: "top center",
        //className: "warn"
        //});
    }





    //Ajax call to Get BU Summary Data
    ////debugger;
    $.ajax({
        type: "GET",
        url: encodeURI("../BudgetingVKM/GetBUSummaryData"),
        data: { 'year': filtered_yr },
        success: OnSuccess_GetBUSummary,
        error: OnError_GetBUSummary
    });

    ////debugger;

    function OnSuccess_GetBUSummary(response) {
        ////debugger;
        var objdata = eval(response.data.data);
        ////debugger;

        if (response.message != null && response.message == "") {

            $("#BUSummaryTable").prop('hidden', true);
        }
        else
            $("#BUSummaryTable").prop('hidden', false);
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
        //else if (response.message == "CC") {
        busummarytable = $("#BUSummaryTable").dxDataGrid({

            loadPanel: {
                enabled: true
            },
            dataSource: objdata,
            noDataText: "For VKM 2021, the above mentioned BUs are not applicable !",
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

        chart();
    }

    function OnError_GetBUSummary(response) {

        //$.notify('Unable to load BU Summary right now, Please Try again later!', {
        //globalPosition: "top center",
        //className: "warn"
        //});
    }


    chart();



    //Ajax call to Get Section Summary Data
    $.ajax({
        type: "GET",
        url: "/BudgetingVKM/GetSectionSummaryData",
        data: { 'year': filtered_yr },
        datatype: "json",
        async: true,
        success: OnSuccess_GetSectionSummary,
        error: OnError_GetSectionSummary

    });

    function OnSuccess_GetSectionSummary(data) {
        dataObjData_section = eval(data.data.data);
        sectionsummarytable = $("#sectionsummarytable").dxDataGrid({

            loadPanel: {
                enabled: true
            },
            showBorders: true,
            noDataText: "For VKM 2021, the above mentioned Sections are not applicable !",
            dataSource: dataObjData_section,


            onEditorPreparing: function (e) {

                e.editorOptions.disabled = true;
            }
        });

    }

    function OnError_GetSectionSummary(response) {

        //$.notify('Unable to load Section Summary right now, Please Try again later!', {
        //globalPosition: "top center",
        //className: "warn"
        //});
    }




    //Ajax call to Get Request Item Data
    ////debugger;
    $.ajax({
        type: "GET",
        url: encodeURI("../BudgetingVKM/GetData"),
        data: { 'year': filtered_yr },
        success: OnSuccess_GetData,
        error: OnError_GetData
    });



}

function chart() {
    ////debugger;
    $.ajax({
        type: "GET",
        url: encodeURI("../BudgetingVKM/GetBUSummaryData"),
        data: { 'year': filtered_yr, 'chart': true },
        success: OnSuccess_ChartSummaryData,
        error: OnError_ChartSummaryData
    });
}



function OnSuccess_ChartSummaryData(data) {
    ////debugger;
    dataObj = eval(data.data.data);
    if (data.message != null && data.message == "") {

        $("#chart").prop('hidden', true);
    }
    else if (data.message != null && data.message == "CC") {
        $("#chart").prop('hidden', false);
        $("#chart").dxChart({

            dataSource: dataObj,
            title: {
                text: "VKM Planning Comparison Chart (" + (new Date()).getFullYear() + "-" + ((new Date()).getFullYear() + 1) + ")",
                font: {
                    size: 26,
                    weight: 800
                },

            },

            commonSeriesSettings: {
                argumentField: "Year",
                type: "bar",
                hoverMode: "allArgumentPoints",
                selectionMode: "allArgumentPoints",

            },
            series: [

                {
                    valueField: "MB", name: "MB",
                    label: {

                        visible: true,
                        format: {
                            type: "currency",
                            precision: 0
                        },

                    }
                },
                {
                    valueField: "OSS", name: "OSS",
                    label: {

                        visible: true,
                        format: {
                            type: "currency",
                            precision: 0
                        }
                    }
                },
                {
                    valueField: "2WP", name: "2WP",
                    label: {

                        visible: true,
                        format: {
                            type: "currency",
                            precision: 0
                        }
                    }
                }
                ,
                {
                    valueField: "Totals", name: "Totals",
                    label: {

                        visible: true,
                        format: {
                            type: "currency",
                            precision: 0
                        }
                    }
                }


                //{ valueField: "plMAE", name: "Planned MAE" },
                //{ valueField: "utMAE", name: "Utilized MAE" },
                //{ valueField: "plNMAE", name: "Planned Non-MAE" },
                //{ valueField: "utNMAE", name: "Utilized Non-MAE" },
                //{ valueField: "plsw", name: "Planned Software" },
                //{ valueField: "utsw", name: "Utilized Software" },
                //{ valueField: "plTotals", name: "Planned Total" },
                //{ valueField: "utTotals", name: "Utilized Total" },


            ],



            //legend: {
            //    horizontalAlignment: "right",
            //    horizontalAlignment: "center", // or "left" | "right"
            //    // verticalAlignment: "bottom",
            //    border: { visible: true },
            //    columnCount: 3,

            //},

            valueAxis: [
                {
                    position: 'left',

                    valueMarginsEnabled: false,
                    title: {
                        text: "VKM Planning Budget (in Dollars)"
                    }
                },

                //{
                //    name: "Percentage_VKM",
                //    position: "right",
                //    grid: {
                //        visible: true
                //    },
                //    title: {
                //        text: "Percentage Utilization"
                //    }
                //}
            ],


            export: {
                enabled: true,
                fileName: "VKM Comparison Chart"
            },
            tooltip: {
                enabled: true
            },
            zoomAndPan: {
                argumentAxis: "both",
                valueAxis: "both",
                dragToZoom: true,
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

        });


    }
    else {
        $("#chart").prop('hidden', false);
        $("#chart").dxChart({

            dataSource: dataObj,
            title: {
                text: "VKM Planning Comparison Chart (" + (new Date()).getFullYear() + "-" + ((new Date()).getFullYear() + 1) + ")",
                font: {
                    size: 26,
                    weight: 800
                },

            },

            commonSeriesSettings: {
                argumentField: "Year",
                type: "bar",
                hoverMode: "allArgumentPoints",
                selectionMode: "allArgumentPoints",

            },
            series: [

                {
                    valueField: "DA", name: "DA",
                    label: {

                        visible: true,
                        format: {
                            type: "currency",
                            precision: 0
                        },

                    }
                },
                {
                    valueField: "AD", name: "AD",
                    label: {

                        visible: true,
                        format: {
                            type: "currency",
                            precision: 0
                        }
                    }
                },
                {
                    valueField: "2WP", name: "2WP",
                    label: {

                        visible: true,
                        format: {
                            type: "currency",
                            precision: 0
                        }
                    }
                }
                //,
                //{
                //    valueField: "Totals", name: "Totals",
                //    label: {

                //        visible: true,
                //        format: {
                //            type: "currency",
                //            precision: 0
                //        }
                //    }
                //}


                //{ valueField: "plMAE", name: "Planned MAE" },
                //{ valueField: "utMAE", name: "Utilized MAE" },
                //{ valueField: "plNMAE", name: "Planned Non-MAE" },
                //{ valueField: "utNMAE", name: "Utilized Non-MAE" },
                //{ valueField: "plsw", name: "Planned Software" },
                //{ valueField: "utsw", name: "Utilized Software" },
                //{ valueField: "plTotals", name: "Planned Total" },
                //{ valueField: "utTotals", name: "Utilized Total" },


            ],



            //legend: {
            //    horizontalAlignment: "right",
            //    horizontalAlignment: "center", // or "left" | "right"
            //    // verticalAlignment: "bottom",
            //    border: { visible: true },
            //    columnCount: 3,

            //},

            valueAxis: [
                {

                    position: 'left',

                    valueMarginsEnabled: false,
                    title: {
                        text: "VKM Planning Budget (in Dollars)"
                    }
                },

                //{
                //    name: "Percentage_VKM",
                //    position: "right",
                //    grid: {
                //        visible: true
                //    },
                //    title: {
                //        text: "Percentage Utilization"
                //    }
                //}
            ],


            export: {
                enabled: true,
                fileName: "VKM Comparison Chart"
            },
            tooltip: {
                enabled: true
            },
            zoomAndPan: {
                argumentAxis: "both",
                valueAxis: "both",
                dragToZoom: true,
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

        });


    }

    //var str = "Compare";
    //var result = str.bold();
    //genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");


    //$.notify('Comparison Chart loaded successfully !', {
    //    globalPosition: "top center",
    //    className: "success"
    //});

    $("#gen").prop('disabled', true);

}


function OnError_ChartSummaryData(response) {
    ////debugger;
    //$.notify('Unable to load VKM Cockpit Chart right now, Please Try again later!', {
    //    globalPosition: "top center",
    //    className: "warn"
    //});

    //var str = "Try Again";
    //var result = str.bold();
    //genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");
}




function OnSuccess_GetData(response) {
     var objdata = (response.data);

    var yr_list = [];
    var currentYear = (new Date()).getFullYear();
    for (var i = currentYear + 1; i >= 2021; i--) {
        yr_list.push({ "Year": i });

    }

    var admin = false;
    //debugger;
    if (response.message == "PurchaseSPOC")//|| response.message == "VKMAdmin")
    {
        flag = true;//if the presentuser is a purchase spoc/VKM admin

    }
    else
        flag = false;//if the presentuser is a vkm spoc
    if (response.message == "VKMAdmin")
        admin = true;
    if (flag || response.message == "VKMAdmin") {

        //if (flag) {
        //    document.title = "Purchase SPOC Reviews";
        //    document.getElementById("VKMTitle").innerHTML = "Purchase SPOC Reviews";
        //    document.getElementById("VKMTitle").style.fontWeight = "700";
        //    document.getElementById("VKMTitle").style.fontSize = "18.72";
        //}
        if (response.message == "VKMAdmin") {
            document.title = "VKM Admin View";
            document.getElementById("VKMTitle").innerHTML = "VKM SPOC View";
            document.getElementById("VKMTitle").style.fontWeight = "700";
            document.getElementById("VKMTitle").style.fontSize = "18.72";
        }

    }
    var isF03F01 = function (position) {

        if (position == undefined)
            return true;
        else
            return position && [1, 3].indexOf(position) >= 0;

    };
    /**
* January 1st - March 31st  = First Quarter
* April 1st - June 30th = Second Quarter
* July 1st - September 30th = Third Quarter
* October 1st - December 31st = Fourth Quarter
*/

    var getQuarter = function (date = new Date()) {
        return Math.floor(date.getMonth() / 3 + 1);
    }
    //var objdata = (response.data);
    var objdata = (response.data.filter(item => item.LinkedRequestID == ""));

    ChildList = (response.data.filter(item => item.LinkedRequestID != ""));
    debugger; 
    if (flag) { // if po spoc - then show parent-child view for items linked (items from a single quotation)
        dataGridLEP = $("#RequestTable_VKMSPOC").dxDataGrid({

            dataSource: objdata,
            grouping: {
                autoExpandAll: true,
            },
            twoWayBindingEnabled: false
            ,
            hoverStateEnabled: {
                enabled: true
            },
            showColumnLines: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            columnMinWidth: 50,
            showColumnLines: true,
            showRowLines: true,
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
                                $("#RequestTable_VKMSPOC").dxDataGrid("clearFilter");
                                //$("#buttonClearFilters").dxButton({
                                //    text: 'Clear Filters',
                                //    onClick: function () {
                                //        $("#RequestTable_VKMSPOC").dxDataGrid("clearFilter");
                                //    }
                                //});
                            },
                        },


                    }
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
                columnChooserButton.options.text = "Hide Fields";
                columnChooserButton.options.hint = "Hide Fields";
                columnChooserButton.showText = "always";

            },
            summary: {
                recalculateWhileEditing: true,
                totalItems: [{
                    column: "Item_Name",
                    summaryType: "count",
                    valueFormat: "number",
                    customizeText: function (e) {
                        ////debugger;
                        //I tried add 
                        //console.log(e.value)
                        return "Item Count: " + e.value;
                    }
                }, {
                    column: 'Reviewed_Cost',
                    summaryType: 'sum',
                    valueFormat: 'currency',
                    //customizeText: function (e) {
                    //    ////debugger;
                    //    //I tried add 
                    //    //console.log(e.value)
                    //    return "Review Totals: " + e.value;
                    //}
                }],
            },
            //wordWrapEnabled: true,
            noDataText: " ☺ No VKM Item Request is available in your queue !",
            columnFixing: {
                enabled: true
            },
            width: "97vw", //needed to allow horizontal scroll
            height: "80vh",
            columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
            //remoteOperations: true,
            /*scrolling: {
                mode: "virtual",
                rowRenderingMode: "virtual",
                columnRenderingMode: "virtual"
            },*/
            //scrolling: {
            //    rowRenderingMode: 'virtual',
            //},
            paging: {
                pageSize: 15,
            },
            pager: {
                visible: true,
                allowedPageSizes: [15, 30, 'all'],
                showPageSizeSelector: true,
                showInfo: true,
                showNavigationButtons: true,
            },
            //paging: {
            //    pageSize: 50
            //},

            editing: {
                // mode: popupedit == 1 ? "popup" : "grid", //"popup",
                mode: editmode,
                // mode: "popup",
                allowUpdating: function (e) {    //Edit access to labteam when requestortoorder triggered ; //Edit access to vkm spoc when approvedsh != true
                    //debugger;
                    //return true;
                    RequestSource = e.row.data.RequestSource; // get request source to check whether it is HOE or RFO

                    return (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && RequestSource == 'RFO' && !e.row.data.VKMSPOC_Approval) || (flag && e.row.data.RequestToOrder && e.row.data.VKMSPOC_Approval == 0 && e.row.data.RequestSource != 'RFO') || (flag && e.row.data.RequestToOrder && e.row.data.VKMSPOC_Approval == 1 && e.row.data.RequestSource == 'RFO') || (!flag && !e.row.data.ApprovedSH && !admin)//with purchase phase //(!flag && e.row.data.RequestToOrder || (!flag && !e.row.data.ApprovedSH))  --ithout purchase phasse
                    //true 
                    //if vkm spoc
                    //if vkm admin
                    //if purchase spoc
                },
                //allowDeleting: function (e) {
                //    ////debugger;
                //    return (flag && e.row.data.RequestToOrder) && isF03F01(e.row.data.Fund); //delete access to remove newly added f03 items by purchase spoc id not needed
                //},
                allowAdding: allowadd,//since both vkm spoc as well as purchase spoc can add new items

                useIcons: true,
                //popup: {
                //    //title: "",
                //    width: 900,
                //    height: 600,
                //    showTitle: true,
                //    visible: true,
                //    hideOnOutsideClick: true,
                //    //width: 450,
                //    //height: 350,
                //    resizeEnabled: true,
                //},
                //popup: {
                //    title: "Purchase SPOC Reviews",
                //},
                popup: {
                    title: "Purchase SPOC Reviews",
                    showTitle: true,
                    visible: true,
                    hideOnOutsideClick: true,
                    //width: 450,
                    //height: 350,
                    resizeEnabled: true,
                },

                form:
                {
                    items: [
                        {
                            itemType: 'group',
                            caption: 'Request Details',
                            colCount: 2,
                            colSpan: 2,
                            items: [
                                {
                                    dataField: 'BU',
                                    label: {
                                        text: 'BU'
                                    },
                                    editorOptions: {
                                        /*value: 6*/
                                    }
                                },
                                {
                                    dataField: 'OEM',
                                    label: {
                                        text: 'OEM'
                                    },

                                },
                                {
                                    dataField: 'DEPT',
                                    label: {
                                        text: 'Department'
                                    },

                                },

                                {
                                    dataField: 'Group',
                                    label: {
                                        text: 'Group'
                                    },

                                },
                                {
                                    dataField: "POSpocNTID",
                                    label: {
                                        text: 'ELO Spoc NTID'
                                    },

                                },
                                {
                                    dataField: 'OrderStatus',
                                    label: {
                                        text: 'Order Status'
                                    },
                                },

                                {
                                    dataField: 'RequestOrderDate',
                                    label: {
                                        text: 'RFO Submitted Date'
                                    },
                                    editorType: 'dxDateBox',
                                    //validationRules: [{
                                    //    type: "required",
                                    //    message: "RFO Submitted Date is required"
                                    //}],
                                    editorOptions: {
                                        //displayFormat: 'datetim',
                                        format: {
                                            type: "shortDate",
                                        },
                                        disabled: true,
                                    },
                                },


                                {
                                    dataField: 'ELOSubmittedDate',
                                    label: {
                                        text: 'ELO Submitted Date'
                                    },
                                    editorType: 'dxDateBox',
                                    editorOptions: {
                                        //displayFormat: 'datetim',
                                        format: {
                                            type: "shortDate",

                                        },
                                        disabled: true,
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],

                                },

                                {
                                    dataField: 'SRManagerNTID',
                                    label: {
                                        text: 'SR Manager'
                                    },
                                    editorOptions: {
                                        //displayFormat: 'datetim',
                                        disabled: true
                                    },
                                    validationRules: [{
                                        type: "required",

                                    }],
                                },


                                {
                                    dataField: 'DaysTaken',
                                    label: {
                                        text: 'Days Taken'
                                    },
                                    editorOptions: {
                                        //displayFormat: 'datetim',

                                        disabled: true,
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],

                                },

                                {
                                    dataField: 'SRResponsibleBuyerNTID',
                                    label: {
                                        text: 'SR Responsible Buyer'
                                    },
                                    validationRules: [{
                                        type: "required",

                                    }],


                                },

                                {
                                    dataField: 'SR_Value',
                                    label: {
                                        text: 'SR Value'
                                    },
                                }, {
                                    dataField: 'PR_Value',
                                    label: {
                                        text: 'PR Value'
                                    },
                                }, {
                                    dataField: 'Invoice_Value',
                                    label: {
                                        text: 'Invoice Value'
                                    },
                                }, {
                                    dataField: 'OrderPrice_UserInput',
                                    label: {
                                        text: 'Order Price'
                                    },
                                },
                                //{
                                //    dataField: 'OrderPrice',
                                //    label: {
                                //        text: 'Order Price(USD)'
                                //    },
                                //},

                                {
                                    dataField: 'PRNumber',
                                    label: {
                                        text: 'PR Number'
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],

                                },


                                {
                                    dataField: 'Currency',
                                    label: {
                                        text: 'Currency'
                                    },
                                },

                                {
                                    dataField: 'RFQNumber',
                                    label: {
                                        text: 'RFQ Number'
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],

                                },

                                {
                                    dataField: 'OrderID',
                                    label: {
                                        text: 'PO Number'
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],

                                },


                                {
                                    dataField: 'SRSubmitted',
                                    label: {
                                        text: 'SR/PR Submitted Date'
                                    },
                                    editorType: 'dxDateBox',
                                    editorOptions: {
                                        //displayFormat: 'datetim',
                                        format: {
                                            type: "shortDate",
                                        },
                                        disabled: true,
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],
                                },


                                {
                                    dataField: 'OrderDate',
                                    label: {
                                        text: 'PO Release Date'
                                    },
                                    editorType: 'dxDateBox',
                                    editorOptions: {

                                        format: {
                                            type: "shortDate",
                                        },
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],
                                },

                                {
                                    dataField: 'SRAwardedDate',
                                    label: {
                                        text: 'SR Awarded Date'
                                    },
                                    //editorType: 'dxDateBox',
                                    //editorOptions: {
                                    //    format: {
                                    //        type: "shortDate",
                                    //    },
                                    //},

                                    //validationRules: [{
                                    //    type: "required",

                                    //}],
                                },

                                {
                                    dataField: 'OrderedQuantity',
                                    label: {
                                        text: 'Ordered Quantity'
                                    },
                                },


                                {
                                    dataField: 'SRApprovalDays',
                                    label: {
                                        text: 'SR Approval Days'
                                    },
                                    //editorType: 'dxDateBox',
                                    editorOptions: {
                                        //displayFormat: 'datetim',
                                        disabled: true
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],
                                },


                                //{
                                //    dataField: 'OrderDate',
                                //    label: {
                                //        text: 'Order Date'
                                //    },
                                //},

                                //{
                                //    dataField: 'OrderID',
                                //    label: {
                                //        text: 'OrderID'
                                //    },
                                //},


                                {
                                    dataField: 'PORemarks',
                                    label: {
                                        text: 'Justification'
                                    },
                                },

                                {
                                    dataField: 'TentativeDeliveryDate',
                                    label: {
                                        text: 'Tentative Delivery Date'
                                    },
                                },
                                {
                                    dataField: 'ActualDeliveryDate',
                                    label: {
                                        text: 'Actual Delivery Date'
                                    },
                                },
                                {
                                    dataField: 'Description',
                                    label: {
                                        text: 'Order Status Desciption'
                                    },
                                },
                                {
                                    dataField: "LinkedRequests",
                                    label: {
                                        text: 'Linked Requests'
                                    },
                                    allowEditing: false,
                                    disabled: true,

                                },

                            ]
                        }

                    ]
                }





            },
            onContentReady: function (e) {
                ////debugger;
                e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
            },
            onCellPrepared: function (e) {
                if (e.rowType === "data" && e.column.command === "expand") {
                    debugger;
                    if (ChildList.find(x => x.LinkedRequestID == e.row.data.RequestID) == undefined) {
                        e.cellElement.removeClass("dx-datagrid-expand");
                        e.cellElement.empty();
                    }
                }
                if (e.rowType === "header" || e.rowType === "filter") {
                    e.cellElement.addClass("columnHeaderCSS");
                    e.cellElement.find("input").addClass("columnHeaderCSS");
                }

                if (e.rowType === "data" && e.column.command === 'select') {

                    RequestSource = e.row.data.RequestSource; // get request source to check whether it is HOE or RFO

                    var submitAllowed = (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && RequestSource == 'RFO' && !e.row.data.VKMSPOC_Approval)
                        || (!flag && !e.row.data.ApprovedSH && !admin);

                    if (!submitAllowed) {
                        //  debugger;
                        var instance = e.cellElement.find('.dx-select-checkbox').dxCheckBox("instance");
                        instance.option("visible", false);
                        e.cellElement.off();
                    }

                }


            },
            onSelectionChanged(e) {
                debugger;

                var grid = e.component;

                var disabledKeys = e.selectedRowsData.filter(x => (x.RequestSource == 'RFO' && x.VKMSPOC_Approval) || ((x.RequestSource == 'HOE' || x.RequestSource == '') && x.ApprovedSH)).map(x => x.RequestID);
                //disabledkeys are those requestids which has already been submitted
                if (disabledKeys.length > 0) {
                    debugger;
                    if (justDeselected) {
                        justDeselected = false;
                        grid.deselectAll();
                    }
                    else {
                        justDeselected = true;
                        grid.deselectRows(disabledKeys);
                    }

                }


                //var editingAllowed = (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && RequestSource == 'RFO' && !e.row.data.VKMSPOC_Approval) || (flag && e.row.data.RequestToOrder && e.row.data.VKMSPOC_Approval == 0 && e.row.data.RequestSource != 'RFO') || (flag && e.row.data.RequestToOrder && e.row.data.VKMSPOC_Approval == 1 && e.row.data.RequestSource == 'RFO') || (!flag && !e.row.data.ApprovedSH && !admin);

                //already submitted - no edit - planning / ordering
                // requestsource == rfo & vkmspoc approval = 1
                // requestsource == "" / hoe && approvedsh = 1-

                //  if (e.currentSelectedRowKeys.length == e.selectedRowKeys.length) {
                //var disabledKeys = e.currentSelectedRowKeys.filter(i => (!i.editingAllowed));
                //    if (e.selectedRowKeys.length == 0) {//this means that user has clicked to select all ; when currentSelectedRowKeys and selectedRowkeys have different value, this means that user has already selected all and disabled rows were deselected, and now user wants to deselect all
                //    var disabledKeys = e.selectedRowsData.filter(x => x.RequestID > 6046).map(x => x.RequestID);
                //    if (disabledKeys.length > 0)
                //        e.component.deselectRows(disabledKeys);
                //    debugger;
                //}

            },
            //onKeyUp: function (e) {
            //    //debugger;
            //},
            //onKeyDown: function (e) {
            //    //debugger;
            //},
            //focusedRowEnabled: true,
            allowColumnReordering: true,
            allowColumnResizing: true,
            keyExpr: "RequestID",
            columnResizingMode: "widget",
            columnMinWidth: 50,
            selection: {
                mode: 'multiple',
                showCheckBoxesMode: 'always',
                applyFilter: "auto",
                allowSelectAll: false
            },
            grouping: {
                autoExpandAll: true,
            },
            groupPanel: {
                visible: true,
            },
            //stateStoring: {
            //    enabled: true,
            //    type: "localStorage",
            //    storageKey: "RequestID"
            //},

            columnChooser: {
                enabled: true
            },
            //filterRow: {
            //    visible: true,
            //    /*showAllText: "(All)",*/
            //   // resetOperationText : "RESET1"

            //},
            headerFilter: {
                visible: true,
                applyFilter: "auto",
                allowSearch: true

            },
            //selection: {
            //    applyFilter: "auto"
            //},
            showBorders: true,
            //scrolling: {
            //    mode: 'infinite'
            //},
            //onInitialized: function (e) {
            //    dataGrid = e.component;

            // },
            //    selection: {
            //        mode: "multiple",
            //        deferred: true
            //},

            searchPanel: {
                visible: true,
                width: 240,
                placeholder: "Search..."
            },
            loadPanel: {
                enabled: true
            },
            //onOptionChanged: function (e) {
            //    //debugger;
            //    if (e.name === "Reviewed_Quantity") {
            //        // handle the property change here
            //        //debugger;
            //    }
            //},
            //onInput: function (e) {
            //    //debugger;
            //},
            onEditorPreparing: function (e) {

                var component = e.component,
                    rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex
                if (e.parentType === "dataRow" && e.dataField === "isProjected") {

                    // e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0) //true -f01/ f03 or apprcost <= 0 or before vkm spoc approval
                    if (e.row.data.ApprovedSH == undefined)
                        e.row.data.ApprovedSH = false;
                    if (e.row.isEditing == undefined)
                        e.row.isEditing = false;
                    e.editorElement.dxCheckBox({
                        value: e.value,
                        disabled: (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || e.row.data.ApprovedSH == false || !flag || !e.row.isEditing),
                        //readOnly: e.readOnly,
                        onValueChanged: function (ea) {

                            var isProjected;
                            if (ea.value) {
                                e.component.option('value', 1);
                                isProjected = 1;
                                //e.setValue(1);
                            }
                            else {
                                e.component.option('value', 0);
                                isProjected = 0;
                            }
                            $.ajax({

                                type: "post",
                                url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                                data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: isProjected, useryear: filtered_yr, OrdStatus: 0 },
                                datatype: "json",
                                traditional: true,
                                success: function (data) {
                                    if (data.success) {

                                        //var objdata = data.data;
                                        //$("#RequestTable_VKMSPOC").dxDataGrid({
                                        //    dataSource: objdata
                                        //});
                                        window.setTimeout(function () {
                                            component.cellValue(rowIndex, "isProjected", data.isproj);
                                            component.cellValue(rowIndex, "Q1", data.q1);
                                            component.cellValue(rowIndex, "Q2", data.q2);
                                            component.cellValue(rowIndex, "Q3", data.q3);
                                            component.cellValue(rowIndex, "Q4", data.q4);
                                            component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                            component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                            component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                        }, 1000);
                                        // OnSuccess_GetData();
                                    }
                                    else {
                                        $.notify("Unable to update; Please try later !", {
                                            globalPosition: "top center",
                                            className: "warn"
                                        })
                                    }


                                }
                            })
                        }
                    });
                    e.cancel = true;
                }
                if (e.parentType === "dataRow" && e.dataField === "Q1") {

                    e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 1 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                }
                if (e.parentType === "dataRow" && e.dataField === "Q2") {
                    e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 2 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                }
                if (e.parentType === "dataRow" && e.dataField === "Q3") {

                    e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 3 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                }
                if (e.parentType === "dataRow" && e.dataField === "Q4") {
                    e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 4 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                }
                if (e.parentType === "dataRow" && e.dataField === "Group") {
                    e.editorOptions.disabled = (typeof e.row.data.DEPT !== "number") && !e.row.isNewRow;
                    if (e.editorOptions.disabled)
                        e.editorOptions.placeholder = 'Select Dept first';

                    if (!e.editorOptions.disabled)
                        e.editorOptions.placeholder = 'Select Group';

                    e.editorOptions.disabled = !e.row.isNewRow;
                }
                if (e.dataField === "OEM" && e.parentType === "dataRow") {
                    e.editorOptions.disabled = !e.row.isNewRow;
                }


                if (e.dataField === "BU") {
                    ////debugger;
                    if (e.parentType == "dataRow") {
                        ////debugger;
                        e.editorOptions.disabled = !e.row.isNewRow;
                    }

                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);

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
                            data: { 'DEPT': component.cellValue(rowIndex, "DEPT"), BU: e.value },
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
                        //    url: "/BudgetingRequest/GetReviewer_VKMSPOC",
                        //    data: { BU: e.value },
                        //    datatype: "json",
                        //    traditional: true,
                        //    success: function (data) {

                        //        reviewer_2 = data;

                        //    }
                        //})
                        // Emulating a web service call
                        window.setTimeout(function () {
                            component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
                        }, 1000);
                    }
                }

                if (e.dataField === "DEPT") {
                    if (e.parentType == "dataRow") {
                        ////debugger;
                        e.editorOptions.disabled = !e.row.isNewRow;
                    }
                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        var bu = component.cellValue(rowIndex, "BU");
                        if (is_TwoWPselected && (e.value > 59 && e.value < 104)) {
                            BU_forItemFilter = 4;
                            window.setTimeout(function () {
                                component.cellValue(rowIndex, "Reviewer_2", "Sheeba Rani R");
                            }, 1000);
                        }

                        $.ajax({

                            type: "post",
                            url: "/BudgetingRequest/GetReviewer_HoE",
                            data: { DEPT: e.value },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {

                                reviewer_1 = data.data;

                            }
                        })

                        $.ajax({

                            type: "post",
                            url: "/BudgetingRequest/GetReviewer_VKMSPOC",
                            data: { 'DEPT': e.value, BU: component.cellValue(rowIndex, "BU") },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                //debugger;
                                reviewer_2 = data.data;
                                if (e.value == 1 && is_XCselected == true) {
                                    is_TwoWPselected = true;
                                    BU_forItemFilter = 4;
                                    reviewer_2 = "Sheeba Rani R";

                                }

                            }
                        })

                        // Emulating a web service call
                        window.setTimeout(function () {
                            component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
                            component.cellValue(rowIndex, "Reviewer_1", reviewer_1);
                        }, 1000);
                    }
                }



                if (e.dataField === "Item_Name") {

                    if (e.parentType == "dataRow") {
                        ////debugger;
                        e.editorOptions.disabled = !e.row.isNewRow;
                    }
                    //if (e.parentType == "filterRow") {
                    //    //debugger;
                    //    e.editorOptions.dataSource = Item_FilterRow;
                    //    e.editorOptions.showClearButton = true;
                    //    //e.editorName = "dxTextBox";

                    //}
                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        //$.ajax({
                        //    type: "post",
                        //    url: "/BudgetingRequest/GetUnitPrice",
                        //    data: { ItemName: e.value },
                        //    datatype: "json",
                        //    ajax: false,
                        //    traditional: true,
                        //    success: function (data) {
                        //        ////debugger;
                        //        if (data > 0)
                        //            unitprice = data;


                        //        var RevQ_sel = component.cellValue(rowIndex, "Reviewed_Quantity");
                        //        if (component.cellValue(rowIndex, "Reviewed_Quantity") != undefined && component.cellValue(rowIndex, "Reviewed_Quantity") != null) {
                        //            ////debugger;
                        //            $.ajax({

                        //                type: "post",
                        //                url: "/BudgetingVKM/GetRevCost",
                        //                data: { Reviewed_Quantity: component.cellValue(rowIndex, "Reviewed_Quantity"), Unit_Price: unitprice },
                        //                datatype: "json",
                        //                traditional: true,
                        //                success: function (data) {
                        //                    ////debugger;
                        //                    //if (data.msg) {
                        //                    //    CostEUR = "";
                        //                    //    window.setTimeout(function () {
                        //                    //       ////debugger;
                        //                    //        component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
                        //                    //    }, 1000);

                        //                    //    //$.notify(data.msg, {
                        //                    //    //    globalPosition: "top center",
                        //                    //    //    className: "success"
                        //                    //    //})
                        //                    //}
                        //                    //else {


                        //                    RevCost = data.RevCost;
                        //                    window.setTimeout(function () {
                        //                        ////debugger;
                        //                        component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                        //                    }, 1000);
                        //                    //}

                        //                }
                        //            })
                        //        }
                        //    }
                        //});

                        //$.ajax({

                        //    type: "post",
                        //    url: "/BudgetingRequest/GetCategory",
                        //    data: { ItemName: e.value },
                        //    datatype: "json",
                        //    traditional: true,
                        //    success: function (data) {

                        //        category = data;

                        //    }
                        //})

                        //$.ajax({

                        //    type: "post",
                        //    url: "/BudgetingRequest/GetCostElement",
                        //    data: { ItemName: e.value },
                        //    datatype: "json",
                        //    traditional: true,
                        //    success: function (data) {
                        //        ////debugger;
                        //        costelement = data;

                        //    }
                        //})

                        //$.ajax({

                        //    type: "post",
                        //    url: "/BudgetingRequest/GetBudgetCode",
                        //    data: { ItemName: e.value },
                        //    datatype: "json",
                        //    traditional: true,
                        //    success: function (data) {
                        //        ////debugger;
                        //        BudgetCode = data;

                        //    }
                        //})

                        ////// Emulating a web service call

                        //window.setTimeout(function () {

                        //    component.cellValue(rowIndex, "Unit_Price", unitprice);
                        //    component.cellValue(rowIndex, "Category", category);
                        //    component.cellValue(rowIndex, "Cost_Element", costelement);
                        //    component.cellValue(rowIndex, "BudgetCode", BudgetCode);

                        //},
                        //    1000);

                    }


                    if (is_newitem == true) {
                        $.ajax({

                            type: "POST",
                            url: "/BudgetingVKM/GetUnusedAvailability",
                            data: { CostElement: component.cellValue(rowIndex, "Cost_Element"), BU: component.cellValue(rowIndex, "BU"), ItemName: component.cellValue(rowIndex, "Item_Name"), Dept: component.cellValue(rowIndex, "DEPT"), VKMYear: filtered_yr },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                //debugger;
                                AvailUnusedAmt = data.AvailableUnUsedAmt;

                            }
                        })
                    }

                }

                if (e.parentType === "dataRow" && e.dataField === "Fund") {

                    e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund)); //non-editbale if f02 ; editable if f01 or f03

                }

                if (e.dataField === "Reviewed_Quantity") {

                    if (e.parentType == "dataRow") {
                        ////debugger;
                        if (e.row.data.ApprovedSH == undefined)
                            e.row.data.ApprovedSH = false;
                        e.editorOptions.disabled = e.row.data.ApprovedSH && flag;


                    }

                    e.editorOptions.valueChangeEvent = "keyup";

                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data

                    e.editorOptions.onValueChanged = function (e) {

                        onValueChanged.call(this, e);

                        var UnitPr_sel = component.cellValue(rowIndex, "Unit_Price");

                        if (component.cellValue(rowIndex, "Unit_Price") != undefined && component.cellValue(rowIndex, "Unit_Price") != null) {

                            $.ajax({

                                type: "post",
                                url: "/BudgetingVKM/GetRevCost",
                                data: { Reviewed_Quantity: e.value, Unit_Price: component.cellValue(rowIndex, "Unit_Price") },
                                datatype: "json",
                                traditional: true,
                                success: function (data) {
                                    ////debugger;
                                    //if (data.msg) {
                                    //    CostEUR = "";
                                    //    window.setTimeout(function () {
                                    //       ////debugger;
                                    //        component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
                                    //    }, 1000);

                                    //    //$.notify(data.msg, {
                                    //    //    globalPosition: "top center",
                                    //    //    className: "success"
                                    //    //})
                                    //}
                                    //else {
                                    //debugger;
                                    RevCost = data.RevCost;
                                    //alert(RevCost);
                                    //alert(flag);
                                    //alert(RequestToOrder);
                                    //alert(RequestSource);
                                    //alert(VKMSPOC_Approval);


                                    //alert('3');
                                    ////debugger;
                                    window.setTimeout(function () {
                                        //debugger;
                                        component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                                    }, 1000);

                                }
                            });

                        }
                        //////// CTG amount validation to check the CTG budget allocated for the requestor
                        //if (flag == false && RequestToOrder == true && RequestSource == 'RFO' && VKMSPOC_Approval == false) {
                        //        //debugger;
                        //        //alert('1');
                        //        $.ajax({
                        //            type: "POST",
                        //            url: "/BudgetingVKM/ValidateCTGAmount",
                        //            data: { NTID: RequestorNTID, ApprovedAmount: (UnitPr_sel * e.value), Dept: component.cellValue(rowIndex, "DEPT")  },
                        //            datatype: "json",
                        //            traditional: true,
                        //            success: function (data) {
                        //                //alert('initiating success function');
                        //                if (data.success) {
                        //                    //debugger;
                        //                    //alert('success');
                        //                    if (data.isExceeded == true) {
                        //                        //alert('exceeded');
                        //                        $.notify("Can't Update. Budget Exceeded!", {
                        //                            globalPosition: "top center",
                        //                            className: "warn"
                        //                        });
                        //                        component.cellValue(rowIndex, "Reviewed_Quantity", e.previousValue);
                        //                        window.setTimeout(function () {
                        //                            //debugger;
                        //                            component.cellValue(rowIndex, "Reviewed_Cost", (e.previousValue * UnitPr_sel));
                        //                        }, 1000);
                        //                        return false;
                        //                    }
                        //                    else {
                        //                        //alert('not exceeded');
                        //                        window.setTimeout(function () {
                        //                            //debugger;
                        //                            component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                        //                        }, 1000);
                        //                    }
                        //                }
                        //                else {
                        //                    //debugger;
                        //                    //alert('error');
                        //                    $.notify("Can't Update.!", {
                        //                        globalPosition: "top center",
                        //                        className: "warn"
                        //                    });
                        //                    //}
                        //                }
                        //            },
                        //            error: function (data) {
                        //                //alert(data);
                        //                $.notify("Can't Update.!", {
                        //                    globalPosition: "top center",
                        //                    className: "warn"
                        //                });
                        //            }
                        //        });
                        //    }

                        ////window.setTimeout(function () {
                        ////    //debugger;
                        ////    component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                        ////}, 1000);
                    }
                }
                if (e.dataField === "Currency") {
                    e.editorOptions.valueChangeEvent = "keyup";
                    var onValueChanged = e.editorOptions.onValueChanged;
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({

                            type: "post",
                            url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                            data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: 2, useryear: filtered_yr, isChange_amount: 0, ord_price: component.cellValue(rowIndex, "OrderPrice_UserInput"), currency: e.value, OrdStatus: 0 },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                if (data.success) {
                                    //debugger;
                                    //var getdata = data.data;
                                    //$("#RequestTable_VKMSPOC").dxDataGrid({
                                    //    dataSource: getdata
                                    //});
                                    window.setTimeout(function () {
                                        component.cellValue(rowIndex, "isProjected", data.isproj);
                                        component.cellValue(rowIndex, "Q1", data.q1);
                                        component.cellValue(rowIndex, "Q2", data.q2);
                                        component.cellValue(rowIndex, "Q3", data.q3);
                                        component.cellValue(rowIndex, "Q4", data.q4);
                                        component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                        component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                        component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                        component.cellValue(rowIndex, "Currency", data.currency);
                                    }, 1000);
                                }
                                else {
                                    //debugger;
                                    $.notify("Unable to update; Please try later !", {
                                        globalPosition: "top center",
                                        className: "warn"
                                    })
                                }
                            }
                        })
                    }
                }

                if (e.dataField === "OrderPrice_UserInput") {
                    e.editorOptions.valueChangeEvent = "keyup";
                    var onValueChanged = e.editorOptions.onValueChanged;
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        //debugger;

                        if (flag == true && is_newitem == true) {
                            if (e.value > AvailUnusedAmt) {
                                $.notify("Can't Update. Order Price should not exceed the available unused amount!", {
                                    globalPosition: "top center",
                                    className: "warn"
                                });
                                component.cellValue(rowIndex, "OrderPrice_UserInput", 0);
                                return false;
                            }
                        }




                        //debugger;
                        $.ajax({

                            type: "post",
                            url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                            data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: 2, useryear: filtered_yr, isChange_amount: 0, ord_price: e.value, currency: component.cellValue(rowIndex, "Currency"), OrdStatus: 0 },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                if (data.success) {
                                    //debugger;
                                    //var getdata = data.data;
                                    //$("#RequestTable_VKMSPOC").dxDataGrid({
                                    //    dataSource: getdata
                                    //});
                                    window.setTimeout(function () {
                                        component.cellValue(rowIndex, "isProjected", data.isproj);
                                        component.cellValue(rowIndex, "Q1", data.q1);
                                        component.cellValue(rowIndex, "Q2", data.q2);
                                        component.cellValue(rowIndex, "Q3", data.q3);
                                        component.cellValue(rowIndex, "Q4", data.q4);
                                        component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                        component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                        component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                        component.cellValue(rowIndex, "Currency", data.currency);
                                    }, 1000);
                                }
                                else {
                                    //debugger;
                                    $.notify("Unable to update; Please try later !", {
                                        globalPosition: "top center",
                                        className: "warn"
                                    })
                                }
                            }
                        })
                    }
                }

                if (e.dataField === "Projected_Amount") {
                    //debugger;
                    if (e.parentType === "dataRow")
                        e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or before vkm spoc approval

                    e.editorOptions.valueChangeEvent = "keyup";
                    var onValueChanged = e.editorOptions.onValueChanged;
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        $.ajax({

                            type: "post",
                            url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                            data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: 2, useryear: filtered_yr, isChange_amount: 1, proj_price: e.value, OrdStatus: 0 },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                //debugger;
                                if (data.success) {
                                    //debugger;
                                    //var getdata = data.data;
                                    //$("#RequestTable_VKMSPOC").dxDataGrid({
                                    //    dataSource: getdata
                                    //});
                                    window.setTimeout(function () {
                                        component.cellValue(rowIndex, "isProjected", data.isproj);
                                        component.cellValue(rowIndex, "Q1", data.q1);
                                        component.cellValue(rowIndex, "Q2", data.q2);
                                        component.cellValue(rowIndex, "Q3", data.q3);
                                        component.cellValue(rowIndex, "Q4", data.q4);
                                        component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                        component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                        component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                    }, 1000);
                                }
                                else {
                                    $.notify("Unable to update; Please try later !", {
                                        globalPosition: "top center",
                                        className: "warn"
                                    })
                                }


                            }
                        })
                    }
                }

                if (e.dataField === "Unused_Amount") {
                    if (e.parentType === "dataRow")
                        e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or before vkm spoc approval

                    e.editorOptions.valueChangeEvent = "keyup";
                    var onValueChanged = e.editorOptions.onValueChanged;
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        $.ajax({

                            type: "post",
                            url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                            data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: 2, useryear: filtered_yr, isChange_amount: 2, unused_price: e.value, OrdStatus: 0 },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                //debugger;
                                if (data.success) {
                                    //debugger;
                                    //var getdata = data.data;
                                    //$("#RequestTable_VKMSPOC").dxDataGrid({
                                    //    dataSource: getdata
                                    //});
                                    window.setTimeout(function () {
                                        component.cellValue(rowIndex, "isProjected", data.isproj);
                                        component.cellValue(rowIndex, "Q1", data.q1);
                                        component.cellValue(rowIndex, "Q2", data.q2);
                                        component.cellValue(rowIndex, "Q3", data.q3);
                                        component.cellValue(rowIndex, "Q4", data.q4);
                                        component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                        component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                        component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                    }, 1000);
                                }
                                else {
                                    $.notify("Unable to update; Please try later !", {
                                        globalPosition: "top center",
                                        className: "warn"
                                    })
                                }


                            }
                        })
                    }
                }

                /////// Order Status Descrition field to be chosen or created
                if (e.dataField === 'Description') {
                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                    e.editorName = "dxAutocomplete";
                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                    }
                }

                if (e.dataField === "OrderStatus") {
                    e.editorOptions.valueChangeEvent = "keyup";
                    var onValueChanged = e.editorOptions.onValueChanged;
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        var Orderstatus_projectedUncheck = [2, 4, 5, 6, 7]; //ordered,dispatched,delivered,cancelled,closed,
                        var is_projected;
                        if (Orderstatus_projectedUncheck.indexOf(e.value) !== -1) { //projected has to be unchecked
                            is_projected = 0;
                        }
                        else {//projected has to be checked
                            is_projected = 1;
                        }
                        //debugger;
                        $.ajax({

                            type: "post",
                            url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                            data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: is_projected, useryear: filtered_yr, ord_price: component.cellValue(rowIndex, "OrderPrice"), currency: component.cellValue(rowIndex, "Currency"), OrdStatus: e.value },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                //debugger;
                                if (data.success) {
                                    //var getdata = data.data;
                                    //$("#RequestTable_VKMSPOC").dxDataGrid({
                                    //    dataSource: getdata
                                    //});
                                    window.setTimeout(function () {
                                        component.cellValue(rowIndex, "isProjected", data.isproj);
                                        component.cellValue(rowIndex, "Q1", data.q1);
                                        component.cellValue(rowIndex, "Q2", data.q2);
                                        component.cellValue(rowIndex, "Q3", data.q3);
                                        component.cellValue(rowIndex, "Q4", data.q4);
                                        component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                        component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                        component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                    }, 1000);
                                }
                                else {
                                    $.notify("Unable to update; Please try later !", {
                                        globalPosition: "top center",
                                        className: "warn"
                                    })
                                }


                            }
                        })
                    }
                }

                //if (e.dataField === "SRAwardedDate") {

                //    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                //    e.editorOptions.onValueChanged = function (arg) {
                //        //debugger;
                //        onValueChanged.call(this, e);
                //        //debugger;
                //        if (arg.value != undefined && component.cellValue(rowIndex, "SRSubmitted") != undefined) {
                //            srawardeddate = Date.parse(arg.value);
                //            srsubmitteddate = Date.parse(component.cellValue(rowIndex, "SRSubmitted"));

                //            var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                //            // To calculate the no. of days between two dates
                //            var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                //            var SRApprovalDays = Difference;
                //            component.cellValue(rowIndex, "SRApprovalDays", SRApprovalDays);
                //        }

                //    }
                //}

            },
            columns: [
                {

                    type: "buttons",
                    width: 100,
                    alignment: "left",
                    fixed: true,
                    fixedPosition: "left",
                    buttons: [
                        "edit", "delete",
                        {
                            hint: "Submit Item",
                            icon: "check",
                            visible: function (e) {
                                //debugger;
                                RequestSource = e.row.data.RequestSource;
                                RequestToOrder = e.row.data.RequestToOrder;
                                VKMSPOC_Approval = e.row.data.VKMSPOC_Approval;
                                Reviewed_Quantity = e.row.data.Reviewed_Quantity;
                                RequestorNTID = e.row.data.RequestorNTID;

                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return !flag && !e.row.isEditing && ((!flag && !e.row.data.ApprovedSH) || (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && e.row.data.RequestSource == "RFO" && !e.row.data.VKMSPOC_Approval && e.row.data.isCancelled != '2')) && !admin

                                // labteam - no option to submit
                                // vkmspoc approved sh=false - item not reviewed, so submit option available
                                // vkmspoc approved sh=true  - item reviewed, so submit option not needed


                            },
                            onClick: function (e) {
                                SHApprove(e.row.data.RequestID, filtered_yr);
                                e.component.refresh(true);
                                e.event.preventDefault();
                            }
                        },
                        {
                            hint: "Send Back Item",
                            icon: "fa fa-send",
                            visible: function (e) {

                                RequestSource = e.row.data.RequestSource;
                                RequestToOrder = e.row.data.RequestToOrder;
                                VKMSPOC_Approval = e.row.data.VKMSPOC_Approval;
                                Reviewed_Quantity = e.row.data.Reviewed_Quantity;
                                RequestorNTID = e.row.data.RequestorNTID;

                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                //return !flag && !e.row.isEditing && (!flag && !e.row.data.ApprovedSH) && !admin && (e.row.data.RequestSource != "RFO") //admin no option to update
                                return !flag && !e.row.isEditing && ((!flag && !e.row.data.ApprovedSH) || (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && e.row.data.RequestSource == "RFO" && !e.row.data.VKMSPOC_Approval && e.row.data.isCancelled != "2" && e.row.data.OrderStatus != '6')) && !admin
                            },
                            onClick: function (e) {
                                SendBack(e.row.data.RequestID, filtered_yr);
                                e.component.refresh(true);
                                e.event.preventDefault();
                            }

                        }, {
                            hint: 'File attachment',
                            icon: 'file',
                            visible: function (e) {
                                //debugger;
                                return (popupedit && e.row.data.LinkedRequestID == "");
                            },
                            onClick: function (rowItem) {
                                //debugger;
                                var fileListDisplay, noFileTmpDisplay;
                                var $ul = "";
                                noFileTmpDisplay = "none"
                                if (rowItem.row.data.Upload_File_Name != null) {
                                    if (rowItem.row.data.Upload_File_Name.toString().length == 0) {
                                        fileListDisplay = "none"
                                        noFileTmpDisplay = "block"
                                    } else {
                                        fileListDisplay = "block"
                                        noFileTmpDisplay = "none"
                                    }

                                    const myFileArray = rowItem.row.data.Upload_File_Name.split("||");

                                    if (myFileArray != "") {
                                        $ul = $('<ol style="font-size:12px;min-height:50px;max-height:80px;overflow:auto;">', { class: "mylist" }).append(
                                            myFileArray.map(country =>
                                                $("<li>").append($("<a>").text(country))
                                            )
                                        );
                                    } else {
                                        noFileTmpDisplay = "block"
                                    }
                                } else {
                                    noFileTmpDisplay = "block"
                                }

                                var linkedrequests = "NA";
                                if (rowItem.row.data.LinkedRequests != null) {
                                    linkedrequests = rowItem.row.data.LinkedRequests;
                                }

                                popupContentTemplate = function () {
                                    return $('<div style="font-size:12px;">').append(
                                        $(`<div style="display:` + fileListDisplay + `"> File Attached</div>`),
                                        $ul,
                                        $(`<div style=""> Linked Requests: ` + linkedrequests + ` </div >`),
                                        $(`<div style="display:` + noFileTmpDisplay + `;color:red;" id='fileList'>There is no file attached.</div>`),
                                        $(`<div id='FileUploadBtn'></div>`))
                                };
                                const popup = $('#widget').dxPopup({
                                    contentTemplate: popupContentTemplate,
                                    width: '30vw',
                                    height: '50vh',
                                    container: '.dx-viewport',
                                    showTitle: true,
                                    title: 'File Attachment',
                                    visible: true,
                                    dragEnabled: true,
                                    hideOnOutsideClick: true,
                                    showCloseButton: true,
                                    onHiding: function (e) {
                                        //console.log("onHiding")
                                    },
                                    onContentReady: function (e) {
                                        let fileUploader = $(FileUploadBtn).dxFileUploader({
                                            name: "file",
                                            multiple: true,
                                            accept: "*",
                                            uploadMode: "useForm",
                                            //uploadUrl: `${backendURL}AsyncFileUpload`,
                                            onValueChanged: function (e) {
                                                //debugger;

                                                //file = e.value;
                                                ////debugger;

                                                var uploadedfilename = document.querySelector('.dx-fileuploader-files-container');



                                                for (var i = 0; i < e.value.length; i++) {





                                                    //if (e.value[i].name.indexOf("Quot") != -1 || e.value[i].name.indexOf("Single Source Justification") != -1) {

                                                    file = e.value;


                                                    if (uploadedfilename != undefined) {

                                                        uploadedfilename.style.visibility = 'visible';

                                                        uploadedfilename.style.height = "100px";
                                                        uploadedfilename.style.overflow = "auto";
                                                        uploadedfilename.style.paddingTop = "0px";

                                                    }

                                                    //}

                                                    //else {

                                                    //    file = null;

                                                    //    alert('Invalid file');

                                                    //    if (uploadedfilename != undefined) {

                                                    //        uploadedfilename.style.visibility = 'hidden';

                                                    //        uploadedfilename.style.height = "0px";

                                                    //        uploadedfilename.style.paddingTop = "0px";

                                                    //    }

                                                    //}

                                                }


                                            },
                                            onUploaded: function (e) {
                                                //debugger;
                                                cellInfo.setValue("images/employees/" + e.request.responseText);
                                                retryButton.option("visible", false);
                                            },
                                            onUploadError: function (e) {
                                                //debugger;
                                                let xhttp = e.request;
                                                if (xhttp.status === 400) {
                                                    e.message = e.error.responseText;
                                                }
                                                if (xhttp.readyState === 4 && xhttp.status === 0) {
                                                    e.message = "Connection refused";
                                                }
                                                //retryButton.option("visible", true);
                                            }
                                        }).dxFileUploader("instance");
                                    },
                                    position: {
                                        at: 'center',
                                        my: 'center',
                                        collision: 'fit',
                                    },
                                    toolbarItems: [{
                                        widget: 'dxButton',
                                        toolbar: 'bottom',
                                        location: 'after',
                                        options: {
                                            text: 'Save',
                                            onClick() {
                                                //debugger;
                                                document.getElementById("loadpanel_uploadFiles").style.display = "block";
                                                if (file != undefined) {
                                                    let fileNameString = "";
                                                    let previousAttachedFileArray = rowItem.row.data.Upload_File_Name.toString().split("||");
                                                    if (file.length > 0) {
                                                        if (previousAttachedFileArray != "") {
                                                            fileNameString = rowItem.row.data.Upload_File_Name;
                                                            for (var i = 0; i < file.length; i++) {
                                                                if (!previousAttachedFileArray.includes(file[i].name)) {
                                                                    fileNameString = fileNameString + '||' + file[i].name;
                                                                }
                                                            }
                                                        } else {
                                                            for (var i = 0; i < file.length; i++) {
                                                                if (fileNameString == "") {
                                                                    fileNameString = file[i].name;
                                                                } else {
                                                                    fileNameString = fileNameString + '||' + file[i].name;
                                                                }

                                                            }
                                                        }
                                                    }
                                                    rowItem.row.data.Upload_File_Name = fileNameString;
                                                    Selected = [];
                                                    Selected.push(rowItem.row.data);
                                                    Update(Selected, filtered_yr, 'FileUpload');
                                                    popup.hide();
                                                } else {
                                                    alert("Please upload the any file.")
                                                }
                                            },
                                        },
                                    },
                                    {
                                        widget: 'dxButton',
                                        toolbar: 'bottom',
                                        location: 'after',
                                        options: {
                                            text: 'Cancel',
                                            onClick() {
                                                popup.hide();
                                            },
                                        },
                                    }
                                    ],
                                }).dxPopup('instance');

                            }
                        }]
                },
                {
                    caption: "Review Request Details",
                    alignment: "center",
                    columns: [
                        //{

                        //    dataField: "VKM_Year",
                        //    validationRules: [{ type: "required" }],
                        //    width: 100,
                        //    lookup: {
                        //        dataSource: yr_list,
                        //        valueExpr: "Year",
                        //        displayExpr: "Year"
                        //    },
                        //    allowEditing: false

                        //},
                        {

                            dataField: "RequestID",
                            allowEditing: false,
                            visible: true,
                            width:100
                        },
                        {

                            dataField: "isProjected",

                            width: 130,
                            dataType: "boolean",
                            //allowEditing: flag,
                            visible: false,
                            //disabled: function (e) {
                            //    //debugger;
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0) //true -f01/ f03 or apprcost <= 0 or before vkm spoc approval

                            //}
                        },
                        {

                            dataField: "Q1",

                            width: 70,
                            dataType: "boolean",
                            allowEditing: flag,
                            visible: false,
                            //disabled: function (e) {
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 1) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                            //}
                        },
                        {

                            dataField: "Q2",

                            width: 70,
                            dataType: "boolean",
                            allowEditing: flag,
                            visible: false,
                            //disabled: function (e) {
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 2) //true -f01/ f03 or apprcost <= 0

                            //}
                        },
                        {

                            dataField: "Q3",

                            width: 70,
                            dataType: "boolean",
                            allowEditing: flag,
                            visible: false,
                            //disabled: function (e) {
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 3) //true -f01/ f03 or apprcost <= 0

                            //}
                        },
                        {

                            dataField: "Q4",

                            width: 70,
                            dataType: "boolean",
                            allowEditing: flag,
                            visible: false,
                            //disabled: function (e) {
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 4) //true -f01/ f03 or apprcost <= 0

                            //}
                        },
                        {
                            dataField: "Projected_Amount",
                            caption: "Projected Amt ($)",
                            width: 150,
                            dataType: "number",
                            format: { type: "currency", precision: 2 },
                            allowEditing: flag,
                            visible: false,
                            //disabled: function (e) {
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0) //true -f01/ f03 or apprcost <= 0

                            //}


                        },
                        {
                            dataField: "Unused_Amount",
                            caption: "Unused Amt ($)",
                            width: 150,
                            dataType: "number",
                            format: { type: "currency", precision: 2 },
                            allowEditing: false,
                            visible: false,
                            setCellValue: function (rowData, value) {

                                rowData.Unused_Amount = value;

                            },
                            //disabled: function (e) {
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0) //true -f01/ f03 or apprcost <= 0

                            //}

                        },
                        {

                            dataField: "BU",
                            validationRules: [{ type: "required" }],
                            width: 70,
                            lookup: {
                                dataSource: BU_list,
                                valueExpr: "ID",
                                displayExpr: "BU"
                            },
                            allowEditing: true,
                            headerFilter: {
                                dataSource: BU_headerFilter,
                                allowSearch: true
                            },
                        },
                        {
                            dataField: "OEM",
                            width: 100,
                            validationRules: [{ type: "required" }],
                            headerFilter: {
                                dataSource: OEM_headerFilter,
                                allowSearch: true
                            },
                            lookup: {
                                dataSource: OEM_list,
                                valueExpr: "ID",
                                displayExpr: "OEM"
                            },
                            allowEditing: true

                        },
                        {
                            dataField: "DEPT",
                            caption: "Dept",
                            headerFilter: {
                                dataSource: DEPT_headerFilter,
                                allowSearch: true
                            },
                            validationRules: [{ type: "required" }],
                            setCellValue: function (rowData, value) {

                                rowData.DEPT = value;
                                rowData.Group = null;

                            },
                            width: 140,
                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: DEPT_list
                                        //filter: options.data ? ["Outdated", "=", false] : null


                                    };
                                },

                                valueExpr: "ID",
                                displayExpr: "DEPT"

                            },
                            allowEditing: true


                        },
                        {
                            dataField: "Group",
                            width: 150,
                            headerFilter: {
                                dataSource: Group_headerFilter,
                                allowSearch: true
                            },
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
                            allowEditing: true
                        },

                        {
                            dataField: "Project",
                            width: 90,
                            allowEditing: !flag,
                            visible: false

                        },
                        {
                            dataField: "Item_Name",
                            caption: "Item",
                            minWidth: 300,
                            validationRules: [{ type: "required" }],
                            lookup: {
                                dataSource: function (options) {
                                    // //debugger;
                                    return {


                                        store: /*Item_list_bkp*/ /*Item_list_New*/Item_list,

                                        filter: options.data ? [["BU", "=", BU_forItemFilter != 0 ? BU_forItemFilter : options.data.BU], 'and', ["Deleted", "=", false]] : null
                                    }


                                },
                                valueExpr: "S_No",
                                displayExpr: "Item_Name"
                            },
                            headerFilter: {
                                dataSource: Item_headerFilter,
                                allowSearch: true
                            },
                            setCellValue: function (rowData, value) {
                                ////debugger;
                                //if value.constructur.name == "Array" => it means that the item list is filtered based on the ordertype selected. At that instance, no need to fetch other details
                                if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                {
                                    rowData.Item_Name = value;
                                    rowData.Category = Item_list.find(x => x.S_No == value).Category;
                                    rowData.Cost_Element = parseInt(Item_list.find(x => x.S_No == value).Cost_Element);
                                    rowData.Unit_Price = Item_list.find(x => x.S_No == value).UnitPriceUSD;
                                    //rowData.ActualAvailableQuantity = Item_list.find(x => x.S_No == value).Actual_Available_Quantity;
                                    rowData.BudgetCode = Item_list.find(x => x.S_No == value).BudgetCode;
                                    rowData.BudgetCodeDescription = BudgetCodeList.find(x => x.Budget_Code == rowData.BudgetCode).Budget_Code_Description;
                                    rowData.UnitofMeasure = parseInt(Item_list.find(x => x.S_No == value).UOM);
                                }
                            },
                            //filterType: "include",
                            //filterValues: [3085],
                            //lookup: {
                            //    dataSource: function (options) {

                            //        return {

                            //            store: Item_list,
                            //            filter: options.data ? [["BU", "=", options.data.BU], 'and', ["Deleted", "=", false]] : null

                            //        };

                            //    },
                            //    valueExpr: "S_No",
                            //    displayExpr: "Item_Name"
                            //},
                            allowEditing: true
                        },
                        {
                            dataField: "OrderType",
                            caption: "Order Type",
                            setCellValue: function (rowData, value) {
                                ////debugger;
                                rowData.OrderType = value;
                                rowData.Item_Name = null;

                            },
                            lookup: {
                                dataSource: function (options) {
                                    //  //debugger;
                                    return {

                                        store: OrderType_list,
                                    }

                                },
                                valueExpr: "ID",
                                displayExpr: "Order_Type"
                            },
                            visible: false

                        },
                        {
                            dataField: "CostCenter",
                            caption: "Cost Center",
                            allowEditing: false,
                            visible: false
                        },
                        {
                            dataField: "BudgetCenterID",
                            caption: "Budget Center",
                            lookup: {
                                dataSource: function (options) {
                                    // //debugger;
                                    return {

                                        store: BudCenter,
                                    }

                                },
                                valueExpr: "ID",
                                displayExpr: "BudgetCenter",
                            },

                            visible: false,
                            //allowEditing: false,

                        },
                        {
                            dataField: "UnitofMeasure",
                            caption: "Unit of Measure",
                            lookup: {
                                dataSource: function (options) {
                                    // //debugger;
                                    return {

                                        store: UOM_list,


                                    }

                                },
                                valueExpr: "ID",
                                displayExpr: "UOM"
                            },
                            visible: false,
                            allowEditing: false,
                        },
                        {
                            dataField: "UnloadingPoint",
                            caption: "Unloading Point",

                            visible: false,
                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: UnloadingPoint_list,
                                    }

                                },
                                valueExpr: "ID",
                                displayExpr: "UnloadingPoint"
                            },
                        },

                        //{
                        //    dataField: 'Comments',
                        //    label: {
                        //        text: 'Comments'
                        //    },
                        //    dataType: 'string',
                        //},

                        {
                            dataField: "ActualAvailableQuantity",
                            caption: "Available Qty",
                            allowEditing: false,
                            width: 110


                        },
                        {
                            dataField: "Category",
                            caption: "Category",
                            //validationRules: [{ type: "required" }],
                            headerFilter: {
                                dataSource: Category_headerFilter,
                                allowSearch: true
                            },
                            lookup: {
                                dataSource: Category_list,
                                valueExpr: "ID",
                                displayExpr: "Category"
                            },
                            allowEditing: false,
                            visible: false

                        },
                        //{
                        //    dataField: "BudgetCode",
                        //    visible: false
                        //    },
                        {
                            dataField: "Cost_Element",
                            headerFilter: {
                                dataSource: CostElement_headerFilter,
                                allowSearch: true
                            },
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
                            headerFilter: {
                                dataSource: BudgetCode_headerFilter,
                                allowSearch: true
                            },
                            lookup: {
                                dataSource: BudgetCodeList,
                                valueExpr: "Budget_Code",
                                displayExpr: "Budget_Code"
                            },
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
                            dataField: "LabName",
                            caption: "Lab Name",
                            visible: false
                        },
                        {
                            dataField: 'RFOReqNTID',
                            setCellValue: function (rowData, value) {

                                rowData.RFOReqNTID = value;

                            },
                            visible: false,
                            allowEditing: false,
                        },
                        {
                            dataField: "RFOApprover",
                            caption: "RFO Approver",
                            visible: false

                        },
                        {
                            dataField: "QuoteAvailable",
                            caption: "Quote Available",
                            visible: false,
                            editCellTemplate: editCellTemplate


                        },
                        {
                            dataField: "GoodsRecID",
                            caption: "Goods Rec ID",
                            visible: false

                        },
                        {
                            dataField: "Required_Quantity",
                            caption: "Request Qty",
                            width: 100,
                            //validationRules: [
                            //    { type: "required" },
                            //{
                            //    type: "range",
                            //    message: "Quantity cannot be negative",
                            //    min: 0,
                            //    max: 214783647
                            //}],
                            dataType: "number",
                            setCellValue: function (rowData, value) {

                                rowData.Required_Quantity = value;

                            },
                            allowEditing: false,
                            visible: true





                        },
                        {
                            dataField: "Unit_Price",
                            caption: "Unit Price",
                            dataType: "number",
                            format: { type: "currency", precision: 0 },
                            valueFormat: "#0",
                            // validationRules: [{ type: "required" }, {
                            //    type: "range",
                            //    message: "Please enter valid price > 0",
                            //    min: 0.01,
                            //    max: Number.MAX_VALUE
                            //}],
                            allowEditing: false,
                            visible: false

                        },
                        {
                            dataField: "Total_Price",
                            caption: "Amt"
                            , calculateCellValue: function (rowData) {

                                if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
                                    return rowData.Unit_Price * rowData.Required_Quantity;
                                }
                                else
                                    return 0.0;
                            },

                            dataType: "number",
                            format: { type: "currency", precision: 0 },
                            valueFormat: "#0",
                            allowEditing: false,
                            visible: false
                        },


                        {
                            dataField: "Reviewed_Quantity",
                            caption: "Review Qty",
                            width: 100,
                            dataType: "number",
                            validationRules: [
                                { type: "required" },
                                {
                                    type: "range",
                                    message: "Quantity cannot be negative",
                                    min: -0.1,
                                    max: 214783647
                                }],
                            setCellValue: function (rowData, value) {
                                // //debugger;
                                rowData.Reviewed_Quantity = value;

                            },
                            allowEditing: function (e) {
                                // //debugger;
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return !e.row.data.ApprovedSH
                            },



                            //labteam - edit - true                  1 || 1
                            //vkmspoc - approvedsh - false - edit    0 || 1
                            //vkmspoc - approvedsh - true - not edit 0 || 0



                        },
                        {
                            dataField: "Reviewed_Cost",
                            caption: "Review Amt",
                            width: 120,
                            //calculateCellValue: function (rowData) {

                            //    //if (/*(rowData.Reviewed_Cost == null || rowData.Reviewed_Cost == undefined) && */rowData.Reviewed_Quantity > 0 && rowData.Unit_Price > 0) {
                            //    //    return rowData.Unit_Price * rowData.Reviewed_Quantity;
                            //    //}
                            //    //else if (rowData.Reviewed_Cost != null || rowData.Reviewed_Cost != undefined) {
                            //    //    return rowData.Reviewed_Cost;
                            //    //}
                            //    //else
                            //    //    return 0.0;
                            //},

                            dataType: "number",
                            format: { type: "currency", precision: 0 },
                            valueFormat: "#0",
                            allowEditing: false,
                            setCellValue: function (rowData, value) {
                                // //debugger;
                                rowData.Reviewed_Cost = value;

                            },
                        },


                        {
                            dataField: "Requestor",
                            allowEditing: false,
                            visible: false
                        },

                        {
                            dataField: "Comments",
                            caption: "Remark",
                            width: 140,
                            allowResizing: true
                            // allowEditing: false,
                        },
                        {
                            dataField: "PORemarks",
                            width: 140,
                            visible: false

                        },
                        {
                            dataField: "SubmitDate",
                            allowEditing: false,
                            visible: false
                        },
                        {
                            dataField: "OrderID",
                            caption: "PO Number",
                            visible: false,

                            allowEditing: function (e) {
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;

                                return flag || !e.row.data.ApprovedSH
                            },

                            //allowEditing: flag || !e.row.data.ApprovedSH  

                        },
                        {
                            dataField: "Currency",
                            lookup: {
                                dataSource: Currency_list,
                                valueExpr: "ID",
                                displayExpr: "Currency"
                            },
                            visible: false,
                            validationRules: [{
                                type: "required",
                                message: "Currency is required for SR,PR,PO,Invoice amounts"
                            }],
                        },
                        {
                            dataField: 'SR_Value',
                            caption: 'SR Value',
                            visible: false,

                        }, {
                            dataField: 'PR_Value',
                            caption: 'PR Value',
                            visible: false,

                        }, {
                            dataField: 'Invoice_Value',
                            visible: false,

                        }, {
                            dataField: 'OrderPrice_UserInput',
                            caption: "Order Price",
                            visible: false,
                        },
                        {
                            dataField: "OrderPrice",
                            visible: false,
                            caption: "Order Price (USD)",
                            //calculateCellValue: function (rowData) {
                            //    var orderpriceinusd;

                            //   ////debugger;
                            //    //based on currency chosen & price entered, convert the order price to usd value
                            //    if (rowData.OrderPrice >0 && rowData.Currency != undefined) {

                            //        $.ajax({

                            //            type: "GET",
                            //            url: "/BudgetingVKM/GetOrderPriceinUSD",
                            //            data: { 'OrderPrice': rowData.OrderPrice, 'Currency': rowData.Currency },
                            //            datatype: "json",
                            //            async: false,
                            //            success: success_getorder_priceusd,
                            //            error: error_getorder_priceusd

                            //        });

                            //        function success_getorder_priceusd(response) {
                            //           ////debugger;
                            //            orderpriceinusd = response;


                            //        }


                            //        function error_getorder_priceusd(response) {
                            //           ////debugger;
                            //            $.notify('Error in converting the entered Order Price to USD!', {
                            //                globalPosition: "top center",
                            //                className: "warn"
                            //            });
                            //        }
                            //    }

                            //    return orderpriceinusd;


                            //},

                            format: { type: "currency", precision: 2 },
                            valueFormat: "#0.00",
                            //allowEditing: flag || !e.row.data.ApprovedSH 
                            allowEditing: function (e) {
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return flag || !e.row.data.ApprovedSH
                            },

                        },

                        {
                            dataField: "OrderedQuantity",
                            caption: "Ordered Qty",
                            visible: false,
                            // allowEditing: flag || !e.row.data.ApprovedSH 
                            allowEditing: function (e) {
                                ////debugger;
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return flag || !e.row.data.ApprovedSH
                            },


                        },
                        {
                            dataField: "OrderStatus",
                            //headerFilter: {
                            //    dataSource: OrderStatus_headerFilter,
                            //    allowSearch: true
                            //},
                            visible: true,

                            //setCellValue: function (rowData, value) {
                            //    //debugger;
                            //    rowData.OrderStatus = value;

                            //    if (value == 3) {
                            //        var today = new Date();
                            //        var dd = String(today.getDate()).padStart(2, '0');
                            //        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                            //        var yyyy = today.getFullYear();

                            //        today = yyyy + '-' + mm + '-' + dd;
                            //        rowData.ELOSubmittedDate = today;
                            //    }
                            //    //else
                            //    //    rowData.ELOSubmittedDate = null;
                            //},
                            setCellValue: function (rowData, value, currentRowData) {
                                //debugger;
                                //if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                {
                                    rowData.OrderStatus = value;

                                    if (value == 3) {
                                        var today = new Date();
                                        var dd = String(today.getDate()).padStart(2, '0');
                                        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                                        var yyyy = today.getFullYear();

                                        today = yyyy + '-' + mm + '-' + dd;
                                        rowData.ELOSubmittedDate = today;
                                    } else
                                        rowData.ELOSubmittedDate = currentRowData.ELOSubmittedDate;



                                    if (currentRowData.RequestOrderDate != undefined) {
                                        rfodate = Date.parse(currentRowData.RequestOrderDate);
                                        elodate = Date.parse(rowData.ELOSubmittedDate);

                                        var Difference_In_Time = Math.abs(elodate - rfodate);

                                        // To calculate the no. of days between two dates
                                        var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                        rowData.DaysTaken = Difference;
                                    }


                                }

                            },

                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: OrderStatus_list,


                                    };
                                },

                                valueExpr: "ID",
                                displayExpr: "OrderStatus"

                            },
                            //allowEditing: flag || !e.row.data.ApprovedSH 
                            allowEditing: flag
                            //function (e) {
                            //if (e.row.data.ApprovedSH == undefined)
                            //    e.row.data.ApprovedSH = false;
                            //return flag //|| !e.row.data.ApprovedSH
                            //},



                        },
                        {
                            dataField: "Description",
                            caption: "Order Status Description",
                            visible: true,
                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: OrderDescription_list,


                                    };
                                },

                                valueExpr: "Description",
                                displayExpr: "Description"

                            },
                            allowEditing: flag

                        },
                        {
                            dataField: "LinkedRequests",
                            caption: 'Linked Requests',
                            visible: false,

                        },
                        //{
                        //    dataField: "LinkedRequestID",
                        //    caption: 'Linked Request ID',
                        //    visible: false,
                        //    groupIndex: 0,

                        //},



                        {
                            dataField: "Reviewer_1",
                            caption: "HOE",
                            allowEditing: false,
                            visible: false
                        },
                        {
                            dataField: "Reviewer_2",
                            caption: "VKM SPOC",
                            allowEditing: false,
                            visible: false
                        },
                        {
                            dataField: "RequiredDate",
                            dataType: "date",
                            allowEditing: true,
                            visible: false

                        },
                        //{
                        //    dataField: "RequestOrderDate",
                        //    dataType: "date",
                        //    allowEditing: false,
                        //    visible: false//flag

                        //},
                        {
                            dataField: "RequestOrderDate",
                            caption: "RFO Submitted Date",
                            dataType: "date",
                            allowEditing: false,
                            visible: false,
                            setCellValue: function (rowData, value) {

                                rowData.RFOSubmittedDate = value;

                            },
                        },
                        {
                            dataField: "OrderDate",
                            caption: "PO Release Date",
                            dataType: "date",
                            allowEditing: true,
                            visible: false//flag

                        },
                        {
                            dataField: "TentativeDeliveryDate",
                            caption: "Tentative Dt",
                            dataType: "date",
                            visible: /*flag*/false,
                            allowEditing: function (e) {
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return flag || !e.row.data.ApprovedSH
                            },
                            //allowEditing: flag || !e.row.data.ApprovedSH 

                        },
                        {
                            dataField: "ActualDeliveryDate",
                            dataType: "date",
                            caption: "Actual Dt",
                            visible: false,//flag,
                            allowEditing: function (e) {
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return flag || !e.row.data.ApprovedSH
                            },
                            //allowEditing: flag || !e.row.data.ApprovedSH 

                        },
                        {
                            dataField: "Fund",

                            setCellValue: function (rowData, value) {

                                rowData.Fund = value;


                            },
                            visible: flag,
                            allowEditing: function (e) {
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return flag || !e.row.data.ApprovedSH
                            },
                            //allowEditing: flag || !e.row.data.ApprovedSH ,
                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: Fund_list,


                                    };
                                },

                                valueExpr: "ID",
                                displayExpr: "Fund",

                            }
                        },

                        {
                            dataField: "Customer_Name",
                            allowEditing: true,
                            visible: false
                        },
                        {
                            dataField: "Customer_Dept",
                            allowEditing: true,
                            visible: false
                        },
                        {
                            dataField: "BM_Number",
                            allowEditing: true,
                            visible: false
                        },
                        {
                            dataField: "Task_ID",
                            allowEditing: true,
                            visible: false
                        },
                        {
                            dataField: "Resource_Group_Id",
                            allowEditing: true,
                            visible: false
                        },
                        {
                            dataField: "PIF_ID",
                            caption: "PIF ID (WBS)",
                            allowEditing: true,
                            visible: false
                        },
                        {
                            dataField: "Project_ID",
                            allowEditing: true,
                            visible: false

                        },
                        {
                            dataField: "Purchase_Type",
                            allowEditing: true,
                            visible: false,
                            lookup: {
                                dataSource: function (options) {
                                    return {
                                        store: PurchaseType_list,
                                    };
                                },
                                valueExpr: "ID",
                                displayExpr: "PurchaseType"
                            }

                        },
                        {
                            dataField: 'Material_Part_Number',
                            dataType: 'string',
                            visible: false,
                        },
                        {
                            dataField: "SupplierName_with_Address",
                            allowEditing: true,
                            visible: false

                        },
                        {
                            dataField: "POSpocNTID",
                            caption: "PO Spoc NTID",
                            allowEditing: false,
                            visible: false,

                        },
                        //{
                        //    dataField: "RequestOrderDate",
                        //    caption: "RFO Submitted Date",
                        //    allowEditing: false,
                        //    visible: false,
                        //    setCellValue: function (rowData, value) {

                        //        rowData.RFOSubmittedDate = value;

                        //    },
                        //},
                        {
                            dataField: "ELOSubmittedDate",
                            caption: "ELO Submitted Date",
                            allowEditing: true,
                            visible: false,
                            //setCellValue: function (rowData, value) {

                            //    rowData.ELOSubmittedDate = value;


                            //},
                            //setCellValue: function (rowData, value, currentRowData) {
                            //    //debugger;
                            //    //if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                            //    {

                            //        rowData.ELOSubmittedDate = value;
                            //        if (currentRowData.RequestOrderDate != undefined) {
                            //            rfodate = Date.parse(currentRowData.RequestOrderDate);
                            //            elodate = Date.parse(rowData.ELOSubmittedDate);

                            //            var Difference_In_Time = Math.abs(elodate - rfodate);

                            //            // To calculate the no. of days between two dates
                            //            var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                            //            rowData.DaysTaken = Difference;
                            //        }


                            //    }

                            //},

                            //calculateCellValue: function (rowData) {
                            //    //debugger;
                            //    if (rowData.OrderStatus == 3) {

                            //        var today = new Date();
                            //        var dd = String(today.getDate()).padStart(2, '0');
                            //        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                            //        var yyyy = today.getFullYear();

                            //        today = mm + '/' + dd + '/' + yyyy;

                            //        return today;
                            //    }
                            //    else
                            //        return 0;
                            //},
                        },
                        {
                            dataField: "DaysTaken",
                            caption: "Days Taken",
                            allowEditing: true,
                            visible: false,
                            dataType: "number",
                            customizeText: function (cellInfo) {
                                //debugger;
                                if (
                                    cellInfo.value === "" ||
                                    cellInfo.value === null ||
                                    cellInfo.value === undefined ||
                                    cellInfo.valueText === "NaN"
                                ) {
                                    return "NA";
                                } else {
                                    return cellInfo.valueText;
                                }
                            },
                            //calculateCellValue: function (rowData) {
                            //    //debugger;
                            //    if (rowData.ELOSubmittedDate != null && rowData.RequestOrderDate != null) {

                            //        rfodate = Date.parse(rowData.RequestOrderDate);
                            //        elodate = Date.parse(rowData.ELOSubmittedDate);

                            //        var Difference_In_Time = Math.abs(elodate - rfodate);

                            //        // To calculate the no. of days between two dates
                            //        var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                            //        return Difference;
                            //    }
                            //    else
                            //        return 0;
                            //},
                        },
                        {
                            dataField: "SRSubmitted",
                            caption: "SR Submitted",
                            dataType: "date",
                            allowEditing: true,
                            visible: false,
                            setCellValue: function (rowData, value, currentRowData) {
                                //debugger;
                                //if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                {

                                    rowData.SRSubmitted = value;
                                    if (currentRowData.SRAwardedDate != undefined) {
                                        srawardeddate = Date.parse(currentRowData.SRAwardedDate);
                                        srsubmitteddate = Date.parse(rowData.SRSubmitted);

                                        var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                                        // To calculate the no. of days between two dates
                                        var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                        rowData.SRApprovalDays = Difference;
                                    }


                                }

                            },

                            //setCellValue: function (rowData, value) { IF UNCOMMENTED, DATEBOX DOESNOT OPENS ON SINGLE CLK OF THE CELL ; OPENS AFTER TWO CLICKS ONLY 

                            //    rowData.SRSubmitted = value;
                            //}

                        },
                        {
                            dataField: "RFQNumber",
                            caption: "RFQ Number",
                            allowEditing: true,
                            visible: false,
                            setCellValue: function (rowData, value) {

                                rowData.RFQNumber = value;

                                if (value != null) {
                                    var today = new Date();
                                    var dd = String(today.getDate()).padStart(2, '0');
                                    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                                    var yyyy = today.getFullYear();

                                    today = yyyy + '-' + mm + '-' + dd;
                                    rowData.SRSubmitted = today;
                                }

                            },

                        },
                        {
                            dataField: "PRNumber",
                            caption: "PR Number",
                            allowEditing: true,
                            visible: false
                        },
                        //{
                        //    dataField: "OrderID",
                        //    caption: "PO Number",
                        //    allowEditing: true,
                        //    visible: false
                        //},
                        //{
                        //    dataField: "OrderDate",
                        //    caption: "PO Release Date",
                        //    allowEditing: true,
                        //    visible: false
                        //},
                        {
                            dataField: "SRAwardedDate",
                            caption: "SRAwardedDate",
                            dataType: "date",
                            allowEditing: true,
                            visible: false,
                            //setCellValue: function (rowData, value) {

                            //    rowData.SRAwardedDate = value;
                            //}
                            setCellValue: function (rowData, value, currentRowData) {
                                //debugger;
                                //if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                {

                                    rowData.SRAwardedDate = value;
                                    if (currentRowData.SRSubmitted != undefined) {
                                        srawardeddate = Date.parse(rowData.SRAwardedDate);
                                        srsubmitteddate = Date.parse(currentRowData.SRSubmitted);

                                        var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                                        // To calculate the no. of days between two dates
                                        var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                        rowData.SRApprovalDays = Difference;
                                    }


                                }

                            },

                        },
                        {
                            dataField: "SRApprovalDays",
                            caption: "SR Approval Days",
                            allowEditing: true,
                            visible: false,
                            dataType: "number",
                            customizeText: function (cellInfo) {
                                //debugger;
                                if (
                                    cellInfo.value === "" ||
                                    cellInfo.value === null ||
                                    cellInfo.value === undefined ||
                                    cellInfo.valueText === "NaN"
                                ) {
                                    return "NA";
                                } else {
                                    return cellInfo.valueText;
                                }
                            },
                            setCellValue: function (rowData, value) {
                                //debugger;
                                rowData.SRApprovalDays = value;
                            }
                            //calculateCellValue: function (rowData) {
                            //    //debugger;
                            //    if (rowData.SRAwardedDate != null && rowData.SRSubmitted != null) {

                            //        srawardeddate = Date.parse(rowData.SRAwardedDate);
                            //        srsubmitteddate = Date.parse(rowData.SRSubmitted);

                            //        var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                            //        // To calculate the no. of days between two dates
                            //        var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                            //        return Difference;
                            //    }
                            //    else
                            //        return 0;
                            //},

                        },
                        {
                            dataField: "SRResponsibleBuyerNTID",
                            caption: "SR Responsible Buyer",
                            allowEditing: true,
                            visible: false,
                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: SRBuyer_list,


                                    };
                                },

                                valueExpr: "NTID",
                                displayExpr: "BuyerName"

                            },
                            setCellValue: function (rowData, value) {
                                //debugger;
                                if (value.constructor.name == "String")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                {

                                    rowData.SRResponsibleBuyerNTID = value;
                                    var mgrID = SRBuyer_list.find(x => x.NTID == value).Manager_ID;
                                    rowData.SRManagerNTID = SRManager_list.find(x => x.ID == mgrID).NTID;
                                    //debugger;
                                    /*rowData.BudgetCodeDescription = BudgetCodeList.find(x /=> x.Budget_Code == rowData.BudgetCode).Budget_Code_Description;*/
                                }
                            },

                        },
                        {
                            dataField: "SRManagerNTID",
                            caption: "SR Manager",
                            allowEditing: true,
                            visible: false,
                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: SRManager_list,


                                    };
                                },

                                valueExpr: "NTID",
                                displayExpr: "ManagerName"

                            },
                        },
                        {
                            caption: "Item Change Log",
                            allowEditing: false,
                            visible: true,

                            width: 100,
                            alignment: 'center',
                            cellTemplate: function (container, options) {
                                $('<a/>').addClass('dx-link')
                                    .text('View')
                                    .on('dxclick', function () {
                                        //debugger;
                                        //var thePath = "\\bosch.com\dfsrb\DfsIN\LOC\Kor\BE-ES\ELO\Global";

                                        //window.open("file://///bosch.com\\dfsrb\\DfsIN\\LOC\\Kor\\BE-ES\\ELO\\Global"); 
                                        //Do something with options.data;  
                                        //debugger;
                                        //alert(options.data.RequestID);
                                        $.ajax({

                                            type: "POST",
                                            url: "/BudgetingVKM/GetL2Details",
                                            data: { 'RequestID': options.data.RequestID },
                                            datatype: "json",
                                            async: true,
                                            success: function (data) {
                                                //debugger;
                                                //$("#myModal").css({ "display": "block" });
                                                //alert(data.data.RequestDate);
                                                //alert(data.data.L1Remarks);
                                                //$("#myModal").modal("show");

                                                $("#popup").dxPopup({
                                                    showTitle: true,
                                                    title: "L2 Item Change Log",
                                                    visible: true,
                                                    hideOnOutsideClick: true,
                                                    width: 450,
                                                    height: 350,
                                                    resizeEnabled: true

                                                });
                                                //debugger;
                                                //$("#dxL1Details").dxDataGrid({
                                                //    dataSource: data.data,
                                                //    //keyExpr: "EmployeeID",
                                                //    visible: true,
                                                //    columns: [{
                                                //        caption:"Remarks",
                                                //        dataField: "L1Remarks"
                                                //    }, {
                                                //        caption: "Request Date",
                                                //        dataField: "RequestDate"
                                                //    }]
                                                //});

                                                $("#dxL2Details").dxForm({
                                                    //formData: {
                                                    //    Remarks: data.data.L1Remarks,
                                                    //    RequestDate: data.data.RequestDate
                                                    //}

                                                    formData: data.data,
                                                    items: [{
                                                        caption: "L1 Remarks",
                                                        dataField: 'L1Remarks',
                                                        editorOptions: {
                                                            disabled: true,
                                                        },
                                                    },
                                                    {
                                                        caption: "L1 Submit Date",
                                                        dataField: 'L1SubmitDate',
                                                        editorOptions: {
                                                            disabled: true,
                                                        },
                                                    },
                                                    {
                                                        caption: "L1 Qty",
                                                        dataField: 'L1Qty',
                                                        editorOptions: {
                                                            disabled: true,
                                                        },
                                                    },
                                                    {
                                                        caption: "L2 Remarks",
                                                        dataField: 'L2Remarks',
                                                        editorOptions: {
                                                            disabled: true,
                                                        },
                                                    },
                                                    {
                                                        caption: "L2 Review Date",
                                                        dataField: 'L2ReviewDate',
                                                        editorOptions: {
                                                            disabled: true,
                                                        },
                                                    },
                                                    {
                                                        caption: "L2 Qty",
                                                        dataField: 'L2Qty',
                                                        editorOptions: {
                                                            disabled: true,
                                                        },
                                                    }]
                                                });
                                            }
                                        });

                                    })
                                    .appendTo(container);
                            }

                        },
                        {
                            caption: "Uploaded Files",
                            //allowEditing: false,
                            visible: true,
                            width: 100,
                            alignment: 'center',
                            cellTemplate: function (container, options) {
                                $('<a/>').addClass('dx-link')
                                    .text(options.data.RequestID)
                                    .on('dxclick', function () {
                                        //Do something with options.data;  
                                        //debugger;
                                        //var url;
                                        $.ajax({



                                            type: "POST",
                                            url: "/BudgetingVKM/EncodeURL",
                                            data: { 'RequestID': options.data.RequestID },
                                            datatype: "json",
                                            async: true,
                                            success: function (data) {
                                                //debugger;
                                                //window.location.href = data;
                                                //url = data.HostURL;
                                                window.open(data.Result, '_blank');
                                            },
                                            error: function (e) {
                                                alert("Unable to redirect");
                                            },
                                        })
                                    }).appendTo(container);
                            }
                        },



                    ]
                },
            ],

            //onEditingStart: function (e) {
            //    //debugger;
            //    if (e.data.Reviewed_Quantity) {
            //        //debugger;
            //    }

            //}, 
            onInitNewRow: function (e) {
                //debugger;
                is_newitem = true;
                //e.data.Requestor = new_request.Requestor;
                //e.data.Reviewer_1 = new_request.Reviewer_1;
                //e.data.Reviewer_2 = new_request.Reviewer_2;
                //e.data.DEPT = new_request.DEPT;
                //    e.data.Group = new_request.Group;

                e.data.POSpocNTID = new_request.POSpocNTID;
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

                e.data.Reviewed_Quantity = new_request.Reviewed_Quantity;
                e.data.Reviewed_Cost = new_request.Reviewed_Cost;


            },
            onRowUpdated: function (e) {
                //debugger;
                $.notify("Item in your Queue is being Updated...Please wait!", {
                    globalPosition: "top center",
                    autoHideDelay: 15000,
                    className: "success"
                })

                ////debugger;
                //if (e.data.OrderedQuantity > e.data.Reviewed_Quantity) {
                //    $.notify("Ordered Quantity cannot be greater than Reviewed Quantity, Please check again!", {
                //        globalPosition: "top center",
                //        className: "error"
                //    })
                //}
                debugger;

                //Order Price to USD conversion commented - since user inputs are received for sr,pr,invoice,order amounts in either INR/EUR/USD and USD Converted amounts are maintained separately - taken care in sp
                //if (e.data.OrderPrice_UserInput > 0 && e.data.Currency != undefined) {
                //    //debugger;
                //    $.ajax({

                //        type: "GET",
                //        url: "/BudgetingVKM/GetOrderPriceinUSD",
                //        data: { 'OrderPrice': e.data.OrderPrice_UserInput, 'Currency': e.data.Currency },
                //        datatype: "json",
                //        async: false,
                //        success: success_getorder_priceusd,
                //        error: error_getorder_priceusd

                //    });

                //    function success_getorder_priceusd(response) {
                //        ////debugger;
                //        e.data.OrderPrice = response;


                //    }


                //    function error_getorder_priceusd(response) {
                //        ////debugger;
                //        //$.notify('Error in converting the entered Order Price to USD!', {
                //        //    globalPosition: "top center",
                //        //    className: "warn"
                //        //});
                //    }
                //}


                //if (e.data.SRAwardedDate != null && e.data.SRSubmitted != null) {
                //    //debugger;
                //    srawardeddate = Date.parse(e.data.SRAwardedDate);
                //    srsubmitteddate = Date.parse(e.data.SRSubmitted);

                //    var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                //    // To calculate the no. of days between two dates
                //    var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                //    e.data.SRApprovalDays = Difference;
                //}
                //else
                //    e.data.SRApprovalDays = 0;


                //if (e.data.RequestOrderDate != null && e.data.ELOSubmittedDate != null) {
                //    //debugger;
                //    srawardeddate = Date.parse(e.data.ELOSubmittedDate);
                //    srsubmitteddate = Date.parse(e.data.RequestOrderDate);

                //    var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                //    // To calculate the no. of days between two dates
                //    var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                //    e.data.DaysTaken = Difference;
                //}
                //else
                //    e.data.DaysTaken = 0;

                Selected = [];
                e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
                //e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;

                Selected.push(e.data);
                ////debugger;
                Update(Selected, filtered_yr, '');
            },
            onRowInserting: function (e) {
                new_request = false;
                $.notify("New Item is being added to your cart...Please wait!", {
                    globalPosition: "top center",
                    className: "success"
                })
                debugger;
                e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
                if (e.data.Reviewed_Cost == null || e.data.Reviewed_Cost == undefined)
                    e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
                //if (e.data.OrderPrice_UserInput > 0 && e.data.Currency != undefined) {

                //    $.ajax({

                //        type: "GET",
                //        url: "/BudgetingVKM/GetOrderPriceinUSD",
                //        data: { 'OrderPrice': e.data.OrderPrice_UserInput, 'Currency': e.data.Currency },
                //        datatype: "json",
                //        async: false,
                //        success: success_getorder_priceusd,
                //        error: error_getorder_priceusd

                //    });

                //    function success_getorder_priceusd(response) {
                //        debugger;
                //        e.data.OrderPrice = response;


                //    }


                //    function error_getorder_priceusd(response) {
                //        debugger;
                //        //$.notify('Error in converting the entered Order Price to USD!', {
                //        //    globalPosition: "top center",
                //        //    className: "warn"
                //        //});
                //    }
                //}

                Selected = [];
                Selected.push(e.data);
                Update(Selected, filtered_yr, '');
            },
            onRowRemoving: function (e) {

                Delete(e.data.RequestID, filtered_yr);

            },
            masterDetail: {
                enabled: true,

                template(container, options) {
                    debugger;
                    {
                       
                        //const currentRequestData = options.data;

                        $('<div>')
                            .addClass('master-detail-caption')
                            //.text(`${currentRequestData.Item_Name} Linked Requests:`)
                            .appendTo(container);
                        debugger;
                      
                          
                        var abcd = $('<div>')
                            .dxDataGrid({
                                // dataSource: objdata,
                                //showColumnHeaders: false,
                                dataSource: new DevExpress.data.DataSource({
                                    store: new DevExpress.data.ArrayStore({
                                        key: 'RequestID',
                                        data: ChildList,
                                    }),
                                    filter: ['LinkedRequestID', '=', options.key],
                                }),
                                grouping: {
                                    autoExpandAll: true,
                                },
                                twoWayBindingEnabled: false
                                ,
                                hoverStateEnabled: {
                                    enabled: true
                                },
                                showColumnLines: true,
                                showRowLines: true,
                                rowAlternationEnabled: true,
                                columnMinWidth: 50,
                                showColumnLines: true,
                                showRowLines: true,
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
                                                    $("#RequestTable_VKMSPOC").dxDataGrid("clearFilter");
                                                    //$("#buttonClearFilters").dxButton({
                                                    //    text: 'Clear Filters',
                                                    //    onClick: function () {
                                                    //        $("#RequestTable_VKMSPOC").dxDataGrid("clearFilter");
                                                    //    }
                                                    //});
                                                },
                                            },


                                        }
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
                                    columnChooserButton.options.text = "Hide Fields";
                                    columnChooserButton.options.hint = "Hide Fields";
                                    columnChooserButton.showText = "always";

                                },
                                summary: {
                                    recalculateWhileEditing: true,
                                    totalItems: [{
                                        column: "Item_Name",
                                        summaryType: "count",
                                        valueFormat: "number",
                                        customizeText: function (e) {
                                            ////debugger;
                                            //I tried add 
                                            //console.log(e.value)
                                            return "Item Count: " + e.value;
                                        }
                                    }, {
                                        column: 'Reviewed_Cost',
                                        summaryType: 'sum',
                                        valueFormat: 'currency',
                                        //customizeText: function (e) {
                                        //    ////debugger;
                                        //    //I tried add 
                                        //    //console.log(e.value)
                                        //    return "Review Totals: " + e.value;
                                        //}
                                    }],
                                },
                                //wordWrapEnabled: true,
                                noDataText: " ☺ No VKM Item Request is available in your queue !",
                                columnFixing: {
                                    enabled: true
                                },
                                width: "97vw", //needed to allow horizontal scroll
                                /*height: "70vh",set in layout pg*/
                                columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
                                //remoteOperations: true,
                                /*scrolling: {
                                    mode: "virtual",
                                    rowRenderingMode: "virtual",
                                    columnRenderingMode: "virtual"
                                },*/
                                scrolling: {
                                    rowRenderingMode: 'virtual',
                                },
                                paging: {
                                    pageSize: 15,
                                },
                                pager: {
                                    visible: true,
                                    allowedPageSizes: [15, 30, 'all'],
                                    showPageSizeSelector: true,
                                    showInfo: true,
                                    showNavigationButtons: true,
                                },
                                //paging: {
                                //    pageSize: 50
                                //},

                                editing: {
                                    // mode: popupedit == 1 ? "popup" : "grid", //"popup",
                                    mode: editmode,
                                    // mode: "popup",
                                    allowUpdating: function (e) {    //Edit access to labteam when requestortoorder triggered ; //Edit access to vkm spoc when approvedsh != true
                                        //debugger;
                                        //return true;
                                        RequestSource = e.row.data.RequestSource; // get request source to check whether it is HOE or RFO

                                        return (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && RequestSource == 'RFO' && !e.row.data.VKMSPOC_Approval) || (flag && e.row.data.RequestToOrder && e.row.data.VKMSPOC_Approval == 0 && e.row.data.RequestSource != 'RFO') || (flag && e.row.data.RequestToOrder && e.row.data.VKMSPOC_Approval == 1 && e.row.data.RequestSource == 'RFO') || (!flag && !e.row.data.ApprovedSH && !admin)//with purchase phase //(!flag && e.row.data.RequestToOrder || (!flag && !e.row.data.ApprovedSH))  --ithout purchase phasse
                                        //true 
                                        //if vkm spoc
                                        //if vkm admin
                                        //if purchase spoc
                                    },
                                    //allowDeleting: function (e) {
                                    //    ////debugger;
                                    //    return (flag && e.row.data.RequestToOrder) && isF03F01(e.row.data.Fund); //delete access to remove newly added f03 items by purchase spoc id not needed
                                    //},
                                    allowAdding: allowadd,//since both vkm spoc as well as purchase spoc can add new items

                                    useIcons: true,
                                    //popup: {
                                    //    //title: "",
                                    //    width: 900,
                                    //    height: 600,
                                    //    showTitle: true,
                                    //    visible: true,
                                    //    hideOnOutsideClick: true,
                                    //    //width: 450,
                                    //    //height: 350,
                                    //    resizeEnabled: true,
                                    //},
                                    //popup: {
                                    //    title: "Purchase SPOC Reviews",
                                    //},
                                    popup: {
                                        title: "Purchase SPOC Reviews",
                                        showTitle: true,
                                        visible: true,
                                        hideOnOutsideClick: true,
                                        //width: 450,
                                        //height: 350,
                                        resizeEnabled: true,
                                    },

                                    form:
                                    {
                                        items: [
                                            {
                                                itemType: 'group',
                                                caption: 'Request Details',
                                                colCount: 2,
                                                colSpan: 2,
                                                items: [
                                                    {
                                                        dataField: 'BU',
                                                        label: {
                                                            text: 'BU'
                                                        },
                                                        editorOptions: {
                                                            /*value: 6*/
                                                        }
                                                    },
                                                    {
                                                        dataField: 'OEM',
                                                        label: {
                                                            text: 'OEM'
                                                        },

                                                    },
                                                    {
                                                        dataField: 'DEPT',
                                                        label: {
                                                            text: 'Department'
                                                        },

                                                    },

                                                    {
                                                        dataField: 'Group',
                                                        label: {
                                                            text: 'Group'
                                                        },

                                                    },
                                                    {
                                                        dataField: "POSpocNTID",
                                                        label: {
                                                            text: 'ELO Spoc NTID'
                                                        },

                                                    },
                                                    {
                                                        dataField: 'OrderStatus',
                                                        label: {
                                                            text: 'Order Status'
                                                        },
                                                    },

                                                    {
                                                        dataField: 'RequestOrderDate',
                                                        label: {
                                                            text: 'RFO Submitted Date'
                                                        },
                                                        editorType: 'dxDateBox',
                                                        //validationRules: [{
                                                        //    type: "required",
                                                        //    message: "RFO Submitted Date is required"
                                                        //}],
                                                        editorOptions: {
                                                            //displayFormat: 'datetim',
                                                            format: {
                                                                type: "shortDate",
                                                            },
                                                            disabled: true,
                                                        },
                                                    },


                                                    {
                                                        dataField: 'ELOSubmittedDate',
                                                        label: {
                                                            text: 'ELO Submitted Date'
                                                        },
                                                        editorType: 'dxDateBox',
                                                        editorOptions: {
                                                            //displayFormat: 'datetim',
                                                            format: {
                                                                type: "shortDate",

                                                            },
                                                            disabled: true,
                                                        },
                                                        //validationRules: [{
                                                        //    type: "required",

                                                        //}],

                                                    },

                                                    {
                                                        dataField: 'SRManagerNTID',
                                                        label: {
                                                            text: 'SR Manager'
                                                        },
                                                        editorOptions: {
                                                            //displayFormat: 'datetim',
                                                            disabled: true
                                                        },
                                                        validationRules: [{
                                                            type: "required",

                                                        }],
                                                    },


                                                    {
                                                        dataField: 'DaysTaken',
                                                        label: {
                                                            text: 'Days Taken'
                                                        },
                                                        editorOptions: {
                                                            //displayFormat: 'datetim',

                                                            disabled: true,
                                                        },
                                                        //validationRules: [{
                                                        //    type: "required",

                                                        //}],

                                                    },

                                                    {
                                                        dataField: 'SRResponsibleBuyerNTID',
                                                        label: {
                                                            text: 'SR Responsible Buyer'
                                                        },
                                                        validationRules: [{
                                                            type: "required",

                                                        }],


                                                    },

                                                    {
                                                        dataField: 'SR_Value',
                                                        label: {
                                                            text: 'SR Value'
                                                        },
                                                    }, {
                                                        dataField: 'PR_Value',
                                                        label: {
                                                            text: 'PR Value'
                                                        },
                                                    }, {
                                                        dataField: 'Invoice_Value',
                                                        label: {
                                                            text: 'Invoice Value'
                                                        },
                                                    }, {
                                                        dataField: 'OrderPrice_UserInput',
                                                        label: {
                                                            text: 'Order Price'
                                                        },
                                                    },
                                                    //{
                                                    //    dataField: 'OrderPrice',
                                                    //    label: {
                                                    //        text: 'Order Price(USD)'
                                                    //    },
                                                    //},

                                                    {
                                                        dataField: 'PRNumber',
                                                        label: {
                                                            text: 'PR Number'
                                                        },
                                                        //validationRules: [{
                                                        //    type: "required",

                                                        //}],

                                                    },


                                                    {
                                                        dataField: 'Currency',
                                                        label: {
                                                            text: 'Currency'
                                                        },
                                                    },

                                                    {
                                                        dataField: 'RFQNumber',
                                                        label: {
                                                            text: 'RFQ Number'
                                                        },
                                                        //validationRules: [{
                                                        //    type: "required",

                                                        //}],

                                                    },

                                                    {
                                                        dataField: 'OrderID',
                                                        label: {
                                                            text: 'PO Number'
                                                        },
                                                        //validationRules: [{
                                                        //    type: "required",

                                                        //}],

                                                    },


                                                    {
                                                        dataField: 'SRSubmitted',
                                                        label: {
                                                            text: 'SR/PR Submitted Date'
                                                        },
                                                        editorType: 'dxDateBox',
                                                        editorOptions: {
                                                            //displayFormat: 'datetim',
                                                            format: {
                                                                type: "shortDate",
                                                            },
                                                            disabled: true,
                                                        },
                                                        //validationRules: [{
                                                        //    type: "required",

                                                        //}],
                                                    },


                                                    {
                                                        dataField: 'OrderDate',
                                                        label: {
                                                            text: 'PO Release Date'
                                                        },
                                                        editorType: 'dxDateBox',
                                                        editorOptions: {

                                                            format: {
                                                                type: "shortDate",
                                                            },
                                                        },
                                                        //validationRules: [{
                                                        //    type: "required",

                                                        //}],
                                                    },

                                                    {
                                                        dataField: 'SRAwardedDate',
                                                        label: {
                                                            text: 'SR Awarded Date'
                                                        },
                                                        //editorType: 'dxDateBox',
                                                        //editorOptions: {
                                                        //    format: {
                                                        //        type: "shortDate",
                                                        //    },
                                                        //},

                                                        //validationRules: [{
                                                        //    type: "required",

                                                        //}],
                                                    },

                                                    {
                                                        dataField: 'OrderedQuantity',
                                                        label: {
                                                            text: 'Ordered Quantity'
                                                        },
                                                    },


                                                    {
                                                        dataField: 'SRApprovalDays',
                                                        label: {
                                                            text: 'SR Approval Days'
                                                        },
                                                        //editorType: 'dxDateBox',
                                                        editorOptions: {
                                                            //displayFormat: 'datetim',
                                                            disabled: true
                                                        },
                                                        //validationRules: [{
                                                        //    type: "required",

                                                        //}],
                                                    },


                                                    //{
                                                    //    dataField: 'OrderDate',
                                                    //    label: {
                                                    //        text: 'Order Date'
                                                    //    },
                                                    //},

                                                    //{
                                                    //    dataField: 'OrderID',
                                                    //    label: {
                                                    //        text: 'OrderID'
                                                    //    },
                                                    //},


                                                    {
                                                        dataField: 'PORemarks',
                                                        label: {
                                                            text: 'Justification'
                                                        },
                                                    },

                                                    {
                                                        dataField: 'TentativeDeliveryDate',
                                                        label: {
                                                            text: 'Tentative Delivery Date'
                                                        },
                                                    },
                                                    {
                                                        dataField: 'ActualDeliveryDate',
                                                        label: {
                                                            text: 'Actual Delivery Date'
                                                        },
                                                    },
                                                    {
                                                        dataField: 'Description',
                                                        label: {
                                                            text: 'Order Status Desciption'
                                                        },
                                                    },
                                                    {
                                                        dataField: "LinkedRequests",
                                                        label: {
                                                            text: 'Linked Requests'
                                                        },
                                                        allowEditing: false,
                                                        disabled: true,

                                                    },

                                                ]
                                            }

                                        ]
                                    }





                                },
                                onContentReady: function (e) {
                                    ////debugger;
                                    e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
                                },
                                onCellPrepared: function (e) {
                                    if (e.rowType === "data") {
                                        
                                        e.cellElement.css("background-color", "#f7eded");
                                    }
                                    if (e.rowType === "header" || e.rowType === "filter") {
                                        e.cellElement.addClass("columnHeaderCSS");
                                        e.cellElement.find("input").addClass("columnHeaderCSS");
                                    }

                                    if (e.rowType === "data" && e.column.command === 'select') {

                                        RequestSource = e.row.data.RequestSource; // get request source to check whether it is HOE or RFO

                                        var submitAllowed = (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && RequestSource == 'RFO' && !e.row.data.VKMSPOC_Approval)
                                            || (!flag && !e.row.data.ApprovedSH && !admin);

                                        if (!submitAllowed) {
                                            //  debugger;
                                            var instance = e.cellElement.find('.dx-select-checkbox').dxCheckBox("instance");
                                            instance.option("visible", false);
                                            e.cellElement.off();
                                        }

                                    }


                                },
                                onSelectionChanged(e) {
                                    debugger;

                                    var grid = e.component;

                                    var disabledKeys = e.selectedRowsData.filter(x => (x.RequestSource == 'RFO' && x.VKMSPOC_Approval) || ((x.RequestSource == 'HOE' || x.RequestSource == '') && x.ApprovedSH)).map(x => x.RequestID);
                                    //disabledkeys are those requestids which has already been submitted
                                    if (disabledKeys.length > 0) {
                                        debugger;
                                        if (justDeselected) {
                                            justDeselected = false;
                                            grid.deselectAll();
                                        }
                                        else {
                                            justDeselected = true;
                                            grid.deselectRows(disabledKeys);
                                        }

                                    }


                                    //var editingAllowed = (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && RequestSource == 'RFO' && !e.row.data.VKMSPOC_Approval) || (flag && e.row.data.RequestToOrder && e.row.data.VKMSPOC_Approval == 0 && e.row.data.RequestSource != 'RFO') || (flag && e.row.data.RequestToOrder && e.row.data.VKMSPOC_Approval == 1 && e.row.data.RequestSource == 'RFO') || (!flag && !e.row.data.ApprovedSH && !admin);

                                    //already submitted - no edit - planning / ordering
                                    // requestsource == rfo & vkmspoc approval = 1
                                    // requestsource == "" / hoe && approvedsh = 1-

                                    //  if (e.currentSelectedRowKeys.length == e.selectedRowKeys.length) {
                                    //var disabledKeys = e.currentSelectedRowKeys.filter(i => (!i.editingAllowed));
                                    //    if (e.selectedRowKeys.length == 0) {//this means that user has clicked to select all ; when currentSelectedRowKeys and selectedRowkeys have different value, this means that user has already selected all and disabled rows were deselected, and now user wants to deselect all
                                    //    var disabledKeys = e.selectedRowsData.filter(x => x.RequestID > 6046).map(x => x.RequestID);
                                    //    if (disabledKeys.length > 0)
                                    //        e.component.deselectRows(disabledKeys);
                                    //    debugger;
                                    //}

                                },
                                //onKeyUp: function (e) {
                                //    //debugger;
                                //},
                                //onKeyDown: function (e) {
                                //    //debugger;
                                //},
                                //focusedRowEnabled: true,
                                allowColumnReordering: true,
                                allowColumnResizing: true,
                                keyExpr: "LinkedRequestID",
                                columnResizingMode: "widget",
                                columnMinWidth: 50,
                                selection: {
                                    mode: 'multiple',
                                    showCheckBoxesMode: 'always',
                                    applyFilter: "auto",
                                    allowSelectAll: false
                                },
                                grouping: {
                                    autoExpandAll: true,
                                },
                                groupPanel: {
                                    visible: true,
                                },
                                //stateStoring: {
                                //    enabled: true,
                                //    type: "localStorage",
                                //    storageKey: "RequestID"
                                //},

                                columnChooser: {
                                    enabled: true
                                },
                                //filterRow: {
                                //    visible: true,
                                //    /*showAllText: "(All)",*/
                                //   // resetOperationText : "RESET1"

                                //},
                                headerFilter: {
                                    visible: true,
                                    applyFilter: "auto",
                                    allowSearch: true

                                },
                                //selection: {
                                //    applyFilter: "auto"
                                //},
                                showBorders: true,
                                //scrolling: {
                                //    mode: 'infinite'
                                //},
                                //onInitialized: function (e) {
                                //    dataGrid = e.component;

                                // },
                                //    selection: {
                                //        mode: "multiple",
                                //        deferred: true
                                //},

                                searchPanel: {
                                    visible: true,
                                    width: 240,
                                    placeholder: "Search..."
                                },
                                loadPanel: {
                                    enabled: true
                                },
                                //onOptionChanged: function (e) {
                                //    //debugger;
                                //    if (e.name === "Reviewed_Quantity") {
                                //        // handle the property change here
                                //        //debugger;
                                //    }
                                //},
                                //onInput: function (e) {
                                //    //debugger;
                                //},
                                onEditorPreparing: function (e) {

                                    var component = e.component,
                                        rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex
                                    if (e.parentType === "dataRow" && e.dataField === "isProjected") {

                                        // e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0) //true -f01/ f03 or apprcost <= 0 or before vkm spoc approval
                                        if (e.row.data.ApprovedSH == undefined)
                                            e.row.data.ApprovedSH = false;
                                        if (e.row.isEditing == undefined)
                                            e.row.isEditing = false;
                                        e.editorElement.dxCheckBox({
                                            value: e.value,
                                            disabled: (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || e.row.data.ApprovedSH == false || !flag || !e.row.isEditing),
                                            //readOnly: e.readOnly,
                                            onValueChanged: function (ea) {

                                                var isProjected;
                                                if (ea.value) {
                                                    e.component.option('value', 1);
                                                    isProjected = 1;
                                                    //e.setValue(1);
                                                }
                                                else {
                                                    e.component.option('value', 0);
                                                    isProjected = 0;
                                                }
                                                $.ajax({

                                                    type: "post",
                                                    url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                                                    data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: isProjected, useryear: filtered_yr, OrdStatus: 0 },
                                                    datatype: "json",
                                                    traditional: true,
                                                    success: function (data) {
                                                        if (data.success) {

                                                            //var objdata = data.data;
                                                            //$("#RequestTable_VKMSPOC").dxDataGrid({
                                                            //    dataSource: objdata
                                                            //});
                                                            window.setTimeout(function () {
                                                                component.cellValue(rowIndex, "isProjected", data.isproj);
                                                                component.cellValue(rowIndex, "Q1", data.q1);
                                                                component.cellValue(rowIndex, "Q2", data.q2);
                                                                component.cellValue(rowIndex, "Q3", data.q3);
                                                                component.cellValue(rowIndex, "Q4", data.q4);
                                                                component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                                                component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                                                component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                                            }, 1000);
                                                            // OnSuccess_GetData();
                                                        }
                                                        else {
                                                            $.notify("Unable to update; Please try later !", {
                                                                globalPosition: "top center",
                                                                className: "warn"
                                                            })
                                                        }


                                                    }
                                                })
                                            }
                                        });
                                        e.cancel = true;
                                    }
                                    if (e.parentType === "dataRow" && e.dataField === "Q1") {

                                        e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 1 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                                    }
                                    if (e.parentType === "dataRow" && e.dataField === "Q2") {
                                        e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 2 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                                    }
                                    if (e.parentType === "dataRow" && e.dataField === "Q3") {

                                        e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 3 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                                    }
                                    if (e.parentType === "dataRow" && e.dataField === "Q4") {
                                        e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 4 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                                    }
                                    if (e.parentType === "dataRow" && e.dataField === "Group") {
                                        e.editorOptions.disabled = (typeof e.row.data.DEPT !== "number") && !e.row.isNewRow;
                                        if (e.editorOptions.disabled)
                                            e.editorOptions.placeholder = 'Select Dept first';

                                        if (!e.editorOptions.disabled)
                                            e.editorOptions.placeholder = 'Select Group';

                                        e.editorOptions.disabled = !e.row.isNewRow;
                                    }
                                    if (e.dataField === "OEM" && e.parentType === "dataRow") {
                                        e.editorOptions.disabled = !e.row.isNewRow;
                                    }


                                    if (e.dataField === "BU") {
                                        ////debugger;
                                        if (e.parentType == "dataRow") {
                                            ////debugger;
                                            e.editorOptions.disabled = !e.row.isNewRow;
                                        }

                                        var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                                        e.editorOptions.onValueChanged = function (e) {
                                            onValueChanged.call(this, e);

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
                                                data: { 'DEPT': component.cellValue(rowIndex, "DEPT"), BU: e.value },
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
                                            //    url: "/BudgetingRequest/GetReviewer_VKMSPOC",
                                            //    data: { BU: e.value },
                                            //    datatype: "json",
                                            //    traditional: true,
                                            //    success: function (data) {

                                            //        reviewer_2 = data;

                                            //    }
                                            //})
                                            // Emulating a web service call
                                            window.setTimeout(function () {
                                                component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
                                            }, 1000);
                                        }
                                    }

                                    if (e.dataField === "DEPT") {
                                        if (e.parentType == "dataRow") {
                                            ////debugger;
                                            e.editorOptions.disabled = !e.row.isNewRow;
                                        }
                                        var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                                        e.editorOptions.onValueChanged = function (e) {
                                            onValueChanged.call(this, e);
                                            var bu = component.cellValue(rowIndex, "BU");
                                            if (is_TwoWPselected && (e.value > 59 && e.value < 104)) {
                                                BU_forItemFilter = 4;
                                                window.setTimeout(function () {
                                                    component.cellValue(rowIndex, "Reviewer_2", "Sheeba Rani R");
                                                }, 1000);
                                            }

                                            $.ajax({

                                                type: "post",
                                                url: "/BudgetingRequest/GetReviewer_HoE",
                                                data: { DEPT: e.value },
                                                datatype: "json",
                                                traditional: true,
                                                success: function (data) {

                                                    reviewer_1 = data.data;

                                                }
                                            })

                                            $.ajax({

                                                type: "post",
                                                url: "/BudgetingRequest/GetReviewer_VKMSPOC",
                                                data: { 'DEPT': e.value, BU: component.cellValue(rowIndex, "BU") },
                                                datatype: "json",
                                                traditional: true,
                                                success: function (data) {
                                                    //debugger;
                                                    reviewer_2 = data.data;
                                                    if (e.value == 1 && is_XCselected == true) {
                                                        is_TwoWPselected = true;
                                                        BU_forItemFilter = 4;
                                                        reviewer_2 = "Sheeba Rani R";

                                                    }

                                                }
                                            })

                                            // Emulating a web service call
                                            window.setTimeout(function () {
                                                component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
                                                component.cellValue(rowIndex, "Reviewer_1", reviewer_1);
                                            }, 1000);
                                        }
                                    }



                                    if (e.dataField === "Item_Name") {

                                        if (e.parentType == "dataRow") {
                                            ////debugger;
                                            e.editorOptions.disabled = !e.row.isNewRow;
                                        }
                                        //if (e.parentType == "filterRow") {
                                        //    //debugger;
                                        //    e.editorOptions.dataSource = Item_FilterRow;
                                        //    e.editorOptions.showClearButton = true;
                                        //    //e.editorName = "dxTextBox";

                                        //}
                                        var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                                        e.editorOptions.onValueChanged = function (e) {
                                            onValueChanged.call(this, e);
                                            //$.ajax({
                                            //    type: "post",
                                            //    url: "/BudgetingRequest/GetUnitPrice",
                                            //    data: { ItemName: e.value },
                                            //    datatype: "json",
                                            //    ajax: false,
                                            //    traditional: true,
                                            //    success: function (data) {
                                            //        ////debugger;
                                            //        if (data > 0)
                                            //            unitprice = data;


                                            //        var RevQ_sel = component.cellValue(rowIndex, "Reviewed_Quantity");
                                            //        if (component.cellValue(rowIndex, "Reviewed_Quantity") != undefined && component.cellValue(rowIndex, "Reviewed_Quantity") != null) {
                                            //            ////debugger;
                                            //            $.ajax({

                                            //                type: "post",
                                            //                url: "/BudgetingVKM/GetRevCost",
                                            //                data: { Reviewed_Quantity: component.cellValue(rowIndex, "Reviewed_Quantity"), Unit_Price: unitprice },
                                            //                datatype: "json",
                                            //                traditional: true,
                                            //                success: function (data) {
                                            //                    ////debugger;
                                            //                    //if (data.msg) {
                                            //                    //    CostEUR = "";
                                            //                    //    window.setTimeout(function () {
                                            //                    //       ////debugger;
                                            //                    //        component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
                                            //                    //    }, 1000);

                                            //                    //    //$.notify(data.msg, {
                                            //                    //    //    globalPosition: "top center",
                                            //                    //    //    className: "success"
                                            //                    //    //})
                                            //                    //}
                                            //                    //else {


                                            //                    RevCost = data.RevCost;
                                            //                    window.setTimeout(function () {
                                            //                        ////debugger;
                                            //                        component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                                            //                    }, 1000);
                                            //                    //}

                                            //                }
                                            //            })
                                            //        }
                                            //    }
                                            //});

                                            //$.ajax({

                                            //    type: "post",
                                            //    url: "/BudgetingRequest/GetCategory",
                                            //    data: { ItemName: e.value },
                                            //    datatype: "json",
                                            //    traditional: true,
                                            //    success: function (data) {

                                            //        category = data;

                                            //    }
                                            //})

                                            //$.ajax({

                                            //    type: "post",
                                            //    url: "/BudgetingRequest/GetCostElement",
                                            //    data: { ItemName: e.value },
                                            //    datatype: "json",
                                            //    traditional: true,
                                            //    success: function (data) {
                                            //        ////debugger;
                                            //        costelement = data;

                                            //    }
                                            //})

                                            //$.ajax({

                                            //    type: "post",
                                            //    url: "/BudgetingRequest/GetBudgetCode",
                                            //    data: { ItemName: e.value },
                                            //    datatype: "json",
                                            //    traditional: true,
                                            //    success: function (data) {
                                            //        ////debugger;
                                            //        BudgetCode = data;

                                            //    }
                                            //})

                                            ////// Emulating a web service call

                                            //window.setTimeout(function () {

                                            //    component.cellValue(rowIndex, "Unit_Price", unitprice);
                                            //    component.cellValue(rowIndex, "Category", category);
                                            //    component.cellValue(rowIndex, "Cost_Element", costelement);
                                            //    component.cellValue(rowIndex, "BudgetCode", BudgetCode);

                                            //},
                                            //    1000);

                                        }


                                        if (is_newitem == true) {
                                            $.ajax({

                                                type: "POST",
                                                url: "/BudgetingVKM/GetUnusedAvailability",
                                                data: { CostElement: component.cellValue(rowIndex, "Cost_Element"), BU: component.cellValue(rowIndex, "BU"), ItemName: component.cellValue(rowIndex, "Item_Name"), Dept: component.cellValue(rowIndex, "DEPT"), VKMYear: filtered_yr },
                                                datatype: "json",
                                                traditional: true,
                                                success: function (data) {
                                                    //debugger;
                                                    AvailUnusedAmt = data.AvailableUnUsedAmt;

                                                }
                                            })
                                        }

                                    }

                                    if (e.parentType === "dataRow" && e.dataField === "Fund") {

                                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund)); //non-editbale if f02 ; editable if f01 or f03

                                    }

                                    if (e.dataField === "Reviewed_Quantity") {

                                        if (e.parentType == "dataRow") {
                                            ////debugger;
                                            if (e.row.data.ApprovedSH == undefined)
                                                e.row.data.ApprovedSH = false;
                                            e.editorOptions.disabled = e.row.data.ApprovedSH && flag;


                                        }

                                        e.editorOptions.valueChangeEvent = "keyup";

                                        var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data

                                        e.editorOptions.onValueChanged = function (e) {

                                            onValueChanged.call(this, e);

                                            var UnitPr_sel = component.cellValue(rowIndex, "Unit_Price");

                                            if (component.cellValue(rowIndex, "Unit_Price") != undefined && component.cellValue(rowIndex, "Unit_Price") != null) {

                                                $.ajax({

                                                    type: "post",
                                                    url: "/BudgetingVKM/GetRevCost",
                                                    data: { Reviewed_Quantity: e.value, Unit_Price: component.cellValue(rowIndex, "Unit_Price") },
                                                    datatype: "json",
                                                    traditional: true,
                                                    success: function (data) {
                                                        ////debugger;
                                                        //if (data.msg) {
                                                        //    CostEUR = "";
                                                        //    window.setTimeout(function () {
                                                        //       ////debugger;
                                                        //        component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
                                                        //    }, 1000);

                                                        //    //$.notify(data.msg, {
                                                        //    //    globalPosition: "top center",
                                                        //    //    className: "success"
                                                        //    //})
                                                        //}
                                                        //else {
                                                        //debugger;
                                                        RevCost = data.RevCost;
                                                        //alert(RevCost);
                                                        //alert(flag);
                                                        //alert(RequestToOrder);
                                                        //alert(RequestSource);
                                                        //alert(VKMSPOC_Approval);


                                                        //alert('3');
                                                        ////debugger;
                                                        window.setTimeout(function () {
                                                            //debugger;
                                                            component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                                                        }, 1000);

                                                    }
                                                });

                                            }
                                            //////// CTG amount validation to check the CTG budget allocated for the requestor
                                            //if (flag == false && RequestToOrder == true && RequestSource == 'RFO' && VKMSPOC_Approval == false) {
                                            //        //debugger;
                                            //        //alert('1');
                                            //        $.ajax({
                                            //            type: "POST",
                                            //            url: "/BudgetingVKM/ValidateCTGAmount",
                                            //            data: { NTID: RequestorNTID, ApprovedAmount: (UnitPr_sel * e.value), Dept: component.cellValue(rowIndex, "DEPT")  },
                                            //            datatype: "json",
                                            //            traditional: true,
                                            //            success: function (data) {
                                            //                //alert('initiating success function');
                                            //                if (data.success) {
                                            //                    //debugger;
                                            //                    //alert('success');
                                            //                    if (data.isExceeded == true) {
                                            //                        //alert('exceeded');
                                            //                        $.notify("Can't Update. Budget Exceeded!", {
                                            //                            globalPosition: "top center",
                                            //                            className: "warn"
                                            //                        });
                                            //                        component.cellValue(rowIndex, "Reviewed_Quantity", e.previousValue);
                                            //                        window.setTimeout(function () {
                                            //                            //debugger;
                                            //                            component.cellValue(rowIndex, "Reviewed_Cost", (e.previousValue * UnitPr_sel));
                                            //                        }, 1000);
                                            //                        return false;
                                            //                    }
                                            //                    else {
                                            //                        //alert('not exceeded');
                                            //                        window.setTimeout(function () {
                                            //                            //debugger;
                                            //                            component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                                            //                        }, 1000);
                                            //                    }
                                            //                }
                                            //                else {
                                            //                    //debugger;
                                            //                    //alert('error');
                                            //                    $.notify("Can't Update.!", {
                                            //                        globalPosition: "top center",
                                            //                        className: "warn"
                                            //                    });
                                            //                    //}
                                            //                }
                                            //            },
                                            //            error: function (data) {
                                            //                //alert(data);
                                            //                $.notify("Can't Update.!", {
                                            //                    globalPosition: "top center",
                                            //                    className: "warn"
                                            //                });
                                            //            }
                                            //        });
                                            //    }

                                            ////window.setTimeout(function () {
                                            ////    //debugger;
                                            ////    component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                                            ////}, 1000);
                                        }
                                    }
                                    if (e.dataField === "Currency") {
                                        e.editorOptions.valueChangeEvent = "keyup";
                                        var onValueChanged = e.editorOptions.onValueChanged;
                                        e.editorOptions.onValueChanged = function (e) {
                                            onValueChanged.call(this, e);
                                            //debugger;
                                            $.ajax({

                                                type: "post",
                                                url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                                                data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: 2, useryear: filtered_yr, isChange_amount: 0, ord_price: component.cellValue(rowIndex, "OrderPrice_UserInput"), currency: e.value, OrdStatus: 0 },
                                                datatype: "json",
                                                traditional: true,
                                                success: function (data) {
                                                    if (data.success) {
                                                        //debugger;
                                                        //var getdata = data.data;
                                                        //$("#RequestTable_VKMSPOC").dxDataGrid({
                                                        //    dataSource: getdata
                                                        //});
                                                        window.setTimeout(function () {
                                                            component.cellValue(rowIndex, "isProjected", data.isproj);
                                                            component.cellValue(rowIndex, "Q1", data.q1);
                                                            component.cellValue(rowIndex, "Q2", data.q2);
                                                            component.cellValue(rowIndex, "Q3", data.q3);
                                                            component.cellValue(rowIndex, "Q4", data.q4);
                                                            component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                                            component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                                            component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                                            component.cellValue(rowIndex, "Currency", data.currency);
                                                        }, 1000);
                                                    }
                                                    else {
                                                        //debugger;
                                                        $.notify("Unable to update; Please try later !", {
                                                            globalPosition: "top center",
                                                            className: "warn"
                                                        })
                                                    }
                                                }
                                            })
                                        }
                                    }

                                    if (e.dataField === "OrderPrice_UserInput") {
                                        e.editorOptions.valueChangeEvent = "keyup";
                                        var onValueChanged = e.editorOptions.onValueChanged;
                                        e.editorOptions.onValueChanged = function (e) {
                                            onValueChanged.call(this, e);
                                            //debugger;

                                            if (flag == true && is_newitem == true) {
                                                if (e.value > AvailUnusedAmt) {
                                                    $.notify("Can't Update. Order Price should not exceed the available unused amount!", {
                                                        globalPosition: "top center",
                                                        className: "warn"
                                                    });
                                                    component.cellValue(rowIndex, "OrderPrice_UserInput", 0);
                                                    return false;
                                                }
                                            }




                                            //debugger;
                                            $.ajax({

                                                type: "post",
                                                url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                                                data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: 2, useryear: filtered_yr, isChange_amount: 0, ord_price: e.value, currency: component.cellValue(rowIndex, "Currency"), OrdStatus: 0 },
                                                datatype: "json",
                                                traditional: true,
                                                success: function (data) {
                                                    if (data.success) {
                                                        //debugger;
                                                        //var getdata = data.data;
                                                        //$("#RequestTable_VKMSPOC").dxDataGrid({
                                                        //    dataSource: getdata
                                                        //});
                                                        window.setTimeout(function () {
                                                            component.cellValue(rowIndex, "isProjected", data.isproj);
                                                            component.cellValue(rowIndex, "Q1", data.q1);
                                                            component.cellValue(rowIndex, "Q2", data.q2);
                                                            component.cellValue(rowIndex, "Q3", data.q3);
                                                            component.cellValue(rowIndex, "Q4", data.q4);
                                                            component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                                            component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                                            component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                                            component.cellValue(rowIndex, "Currency", data.currency);
                                                        }, 1000);
                                                    }
                                                    else {
                                                        //debugger;
                                                        $.notify("Unable to update; Please try later !", {
                                                            globalPosition: "top center",
                                                            className: "warn"
                                                        })
                                                    }
                                                }
                                            })
                                        }
                                    }

                                    if (e.dataField === "Projected_Amount") {
                                        //debugger;
                                        if (e.parentType === "dataRow")
                                            e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or before vkm spoc approval

                                        e.editorOptions.valueChangeEvent = "keyup";
                                        var onValueChanged = e.editorOptions.onValueChanged;
                                        e.editorOptions.onValueChanged = function (e) {
                                            onValueChanged.call(this, e);
                                            $.ajax({

                                                type: "post",
                                                url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                                                data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: 2, useryear: filtered_yr, isChange_amount: 1, proj_price: e.value, OrdStatus: 0 },
                                                datatype: "json",
                                                traditional: true,
                                                success: function (data) {
                                                    //debugger;
                                                    if (data.success) {
                                                        //debugger;
                                                        //var getdata = data.data;
                                                        //$("#RequestTable_VKMSPOC").dxDataGrid({
                                                        //    dataSource: getdata
                                                        //});
                                                        window.setTimeout(function () {
                                                            component.cellValue(rowIndex, "isProjected", data.isproj);
                                                            component.cellValue(rowIndex, "Q1", data.q1);
                                                            component.cellValue(rowIndex, "Q2", data.q2);
                                                            component.cellValue(rowIndex, "Q3", data.q3);
                                                            component.cellValue(rowIndex, "Q4", data.q4);
                                                            component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                                            component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                                            component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                                        }, 1000);
                                                    }
                                                    else {
                                                        $.notify("Unable to update; Please try later !", {
                                                            globalPosition: "top center",
                                                            className: "warn"
                                                        })
                                                    }


                                                }
                                            })
                                        }
                                    }

                                    if (e.dataField === "Unused_Amount") {
                                        if (e.parentType === "dataRow")
                                            e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or before vkm spoc approval

                                        e.editorOptions.valueChangeEvent = "keyup";
                                        var onValueChanged = e.editorOptions.onValueChanged;
                                        e.editorOptions.onValueChanged = function (e) {
                                            onValueChanged.call(this, e);
                                            $.ajax({

                                                type: "post",
                                                url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                                                data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: 2, useryear: filtered_yr, isChange_amount: 2, unused_price: e.value, OrdStatus: 0 },
                                                datatype: "json",
                                                traditional: true,
                                                success: function (data) {
                                                    //debugger;
                                                    if (data.success) {
                                                        //debugger;
                                                        //var getdata = data.data;
                                                        //$("#RequestTable_VKMSPOC").dxDataGrid({
                                                        //    dataSource: getdata
                                                        //});
                                                        window.setTimeout(function () {
                                                            component.cellValue(rowIndex, "isProjected", data.isproj);
                                                            component.cellValue(rowIndex, "Q1", data.q1);
                                                            component.cellValue(rowIndex, "Q2", data.q2);
                                                            component.cellValue(rowIndex, "Q3", data.q3);
                                                            component.cellValue(rowIndex, "Q4", data.q4);
                                                            component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                                            component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                                            component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                                        }, 1000);
                                                    }
                                                    else {
                                                        $.notify("Unable to update; Please try later !", {
                                                            globalPosition: "top center",
                                                            className: "warn"
                                                        })
                                                    }


                                                }
                                            })
                                        }
                                    }

                                    /////// Order Status Descrition field to be chosen or created
                                    if (e.dataField === 'Description') {
                                        var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                                        e.editorName = "dxAutocomplete";
                                        e.editorOptions.onValueChanged = function (e) {
                                            //debugger;
                                            onValueChanged.call(this, e);
                                        }
                                    }

                                    if (e.dataField === "OrderStatus") {
                                        e.editorOptions.valueChangeEvent = "keyup";
                                        var onValueChanged = e.editorOptions.onValueChanged;
                                        e.editorOptions.onValueChanged = function (e) {
                                            onValueChanged.call(this, e);
                                            var Orderstatus_projectedUncheck = [2, 4, 5, 6, 7]; //ordered,dispatched,delivered,cancelled,closed,
                                            var is_projected;
                                            if (Orderstatus_projectedUncheck.indexOf(e.value) !== -1) { //projected has to be unchecked
                                                is_projected = 0;
                                            }
                                            else {//projected has to be checked
                                                is_projected = 1;
                                            }
                                            //debugger;
                                            $.ajax({

                                                type: "post",
                                                url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                                                data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: is_projected, useryear: filtered_yr, ord_price: component.cellValue(rowIndex, "OrderPrice"), currency: component.cellValue(rowIndex, "Currency"), OrdStatus: e.value },
                                                datatype: "json",
                                                traditional: true,
                                                success: function (data) {
                                                    //debugger;
                                                    if (data.success) {
                                                        //var getdata = data.data;
                                                        //$("#RequestTable_VKMSPOC").dxDataGrid({
                                                        //    dataSource: getdata
                                                        //});
                                                        window.setTimeout(function () {
                                                            component.cellValue(rowIndex, "isProjected", data.isproj);
                                                            component.cellValue(rowIndex, "Q1", data.q1);
                                                            component.cellValue(rowIndex, "Q2", data.q2);
                                                            component.cellValue(rowIndex, "Q3", data.q3);
                                                            component.cellValue(rowIndex, "Q4", data.q4);
                                                            component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                                            component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                                            component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                                        }, 1000);
                                                    }
                                                    else {
                                                        $.notify("Unable to update; Please try later !", {
                                                            globalPosition: "top center",
                                                            className: "warn"
                                                        })
                                                    }


                                                }
                                            })
                                        }
                                    }

                                    //if (e.dataField === "SRAwardedDate") {

                                    //    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                                    //    e.editorOptions.onValueChanged = function (arg) {
                                    //        //debugger;
                                    //        onValueChanged.call(this, e);
                                    //        //debugger;
                                    //        if (arg.value != undefined && component.cellValue(rowIndex, "SRSubmitted") != undefined) {
                                    //            srawardeddate = Date.parse(arg.value);
                                    //            srsubmitteddate = Date.parse(component.cellValue(rowIndex, "SRSubmitted"));

                                    //            var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                                    //            // To calculate the no. of days between two dates
                                    //            var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                    //            var SRApprovalDays = Difference;
                                    //            component.cellValue(rowIndex, "SRApprovalDays", SRApprovalDays);
                                    //        }

                                    //    }
                                    //}

                                },

                                columns: [
                                    {

                                        type: "buttons",
                                        width: 100,
                                        alignment: "left",
                                        fixed: true,
                                        fixedPosition: "left",
                                        buttons: [
                                            "edit", "delete",
                                            {
                                                hint: "Submit Item",
                                                icon: "check",
                                                visible: function (e) {
                                                    //debugger;
                                                    RequestSource = e.row.data.RequestSource;
                                                    RequestToOrder = e.row.data.RequestToOrder;
                                                    VKMSPOC_Approval = e.row.data.VKMSPOC_Approval;
                                                    Reviewed_Quantity = e.row.data.Reviewed_Quantity;
                                                    RequestorNTID = e.row.data.RequestorNTID;

                                                    if (e.row.data.ApprovedSH == undefined)
                                                        e.row.data.ApprovedSH = false;
                                                    return !flag && !e.row.isEditing && ((!flag && !e.row.data.ApprovedSH) || (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && e.row.data.RequestSource == "RFO" && !e.row.data.VKMSPOC_Approval && e.row.data.isCancelled != '2')) && !admin

                                                    // labteam - no option to submit
                                                    // vkmspoc approved sh=false - item not reviewed, so submit option available
                                                    // vkmspoc approved sh=true  - item reviewed, so submit option not needed


                                                },
                                                onClick: function (e) {
                                                    SHApprove(e.row.data.RequestID, filtered_yr);
                                                    e.component.refresh(true);
                                                    e.event.preventDefault();
                                                }
                                            },
                                            {
                                                hint: "Send Back Item",
                                                icon: "fa fa-send",
                                                visible: function (e) {

                                                    RequestSource = e.row.data.RequestSource;
                                                    RequestToOrder = e.row.data.RequestToOrder;
                                                    VKMSPOC_Approval = e.row.data.VKMSPOC_Approval;
                                                    Reviewed_Quantity = e.row.data.Reviewed_Quantity;
                                                    RequestorNTID = e.row.data.RequestorNTID;

                                                    if (e.row.data.ApprovedSH == undefined)
                                                        e.row.data.ApprovedSH = false;
                                                    //return !flag && !e.row.isEditing && (!flag && !e.row.data.ApprovedSH) && !admin && (e.row.data.RequestSource != "RFO") //admin no option to update
                                                    return !flag && !e.row.isEditing && ((!flag && !e.row.data.ApprovedSH) || (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && e.row.data.RequestSource == "RFO" && !e.row.data.VKMSPOC_Approval && e.row.data.isCancelled != "2" && e.row.data.OrderStatus != '6')) && !admin
                                                },
                                                onClick: function (e) {
                                                    SendBack(e.row.data.RequestID, filtered_yr);
                                                    e.component.refresh(true);
                                                    e.event.preventDefault();
                                                }

                                            }, {
                                                hint: 'File attachment',
                                                icon: 'file',
                                                visible: function (e) {
                                                    /*debugger;*/
                                                    return (popupedit && e.row.data.LinkedRequestID == "");
                                                },
                                                onClick: function (rowItem) {
                                                    //debugger;
                                                    var fileListDisplay, noFileTmpDisplay;
                                                    var $ul = "";
                                                    noFileTmpDisplay = "none"
                                                    if (rowItem.row.data.Upload_File_Name != null) {
                                                        if (rowItem.row.data.Upload_File_Name.toString().length == 0) {
                                                            fileListDisplay = "none"
                                                            noFileTmpDisplay = "block"
                                                        } else {
                                                            fileListDisplay = "block"
                                                            noFileTmpDisplay = "none"
                                                        }

                                                        const myFileArray = rowItem.row.data.Upload_File_Name.split("||");

                                                        if (myFileArray != "") {
                                                            $ul = $('<ol style="font-size:12px;min-height:50px;max-height:80px;overflow:auto;">', { class: "mylist" }).append(
                                                                myFileArray.map(country =>
                                                                    $("<li>").append($("<a>").text(country))
                                                                )
                                                            );
                                                        } else {
                                                            noFileTmpDisplay = "block"
                                                        }
                                                    } else {
                                                        noFileTmpDisplay = "block"
                                                    }

                                                    var linkedrequests = "NA";
                                                    if (rowItem.row.data.LinkedRequests != null) {
                                                        linkedrequests = rowItem.row.data.LinkedRequests;
                                                    }

                                                    popupContentTemplate = function () {
                                                        return $('<div style="font-size:12px;">').append(
                                                            $(`<div style="display:` + fileListDisplay + `"> File Attached</div>`),
                                                            $ul,
                                                            $(`<div style=""> Linked Requests: ` + linkedrequests + ` </div >`),
                                                            $(`<div style="display:` + noFileTmpDisplay + `;color:red;" id='fileList'>There is no file attached.</div>`),
                                                            $(`<div id='FileUploadBtn'></div>`))
                                                    };
                                                    const popup = $('#widget').dxPopup({
                                                        contentTemplate: popupContentTemplate,
                                                        width: '30vw',
                                                        height: '50vh',
                                                        container: '.dx-viewport',
                                                        showTitle: true,
                                                        title: 'File Attachment',
                                                        visible: true,
                                                        dragEnabled: true,
                                                        hideOnOutsideClick: true,
                                                        showCloseButton: true,
                                                        onHiding: function (e) {
                                                            //console.log("onHiding")
                                                        },
                                                        onContentReady: function (e) {
                                                            let fileUploader = $(FileUploadBtn).dxFileUploader({
                                                                name: "file",
                                                                multiple: true,
                                                                accept: "*",
                                                                uploadMode: "useForm",
                                                                //uploadUrl: `${backendURL}AsyncFileUpload`,
                                                                onValueChanged: function (e) {
                                                                    //debugger;

                                                                    //file = e.value;
                                                                    ////debugger;

                                                                    var uploadedfilename = document.querySelector('.dx-fileuploader-files-container');



                                                                    for (var i = 0; i < e.value.length; i++) {





                                                                        //if (e.value[i].name.indexOf("Quot") != -1 || e.value[i].name.indexOf("Single Source Justification") != -1) {

                                                                        file = e.value;


                                                                        if (uploadedfilename != undefined) {

                                                                            uploadedfilename.style.visibility = 'visible';

                                                                            uploadedfilename.style.height = "100px";
                                                                            uploadedfilename.style.overflow = "auto";
                                                                            uploadedfilename.style.paddingTop = "0px";

                                                                        }

                                                                        //}

                                                                        //else {

                                                                        //    file = null;

                                                                        //    alert('Invalid file');

                                                                        //    if (uploadedfilename != undefined) {

                                                                        //        uploadedfilename.style.visibility = 'hidden';

                                                                        //        uploadedfilename.style.height = "0px";

                                                                        //        uploadedfilename.style.paddingTop = "0px";

                                                                        //    }

                                                                        //}

                                                                    }


                                                                },
                                                                onUploaded: function (e) {
                                                                    //debugger;
                                                                    cellInfo.setValue("images/employees/" + e.request.responseText);
                                                                    retryButton.option("visible", false);
                                                                },
                                                                onUploadError: function (e) {
                                                                    //debugger;
                                                                    let xhttp = e.request;
                                                                    if (xhttp.status === 400) {
                                                                        e.message = e.error.responseText;
                                                                    }
                                                                    if (xhttp.readyState === 4 && xhttp.status === 0) {
                                                                        e.message = "Connection refused";
                                                                    }
                                                                    //retryButton.option("visible", true);
                                                                }
                                                            }).dxFileUploader("instance");
                                                        },
                                                        position: {
                                                            at: 'center',
                                                            my: 'center',
                                                            collision: 'fit',
                                                        },
                                                        toolbarItems: [{
                                                            widget: 'dxButton',
                                                            toolbar: 'bottom',
                                                            location: 'after',
                                                            options: {
                                                                text: 'Save',
                                                                onClick() {
                                                                    //debugger;
                                                                    document.getElementById("loadpanel_uploadFiles").style.display = "block";
                                                                    if (file != undefined) {
                                                                        let fileNameString = "";
                                                                        let previousAttachedFileArray = rowItem.row.data.Upload_File_Name.toString().split("||");
                                                                        if (file.length > 0) {
                                                                            if (previousAttachedFileArray != "") {
                                                                                fileNameString = rowItem.row.data.Upload_File_Name;
                                                                                for (var i = 0; i < file.length; i++) {
                                                                                    if (!previousAttachedFileArray.includes(file[i].name)) {
                                                                                        fileNameString = fileNameString + '||' + file[i].name;
                                                                                    }
                                                                                }
                                                                            } else {
                                                                                for (var i = 0; i < file.length; i++) {
                                                                                    if (fileNameString == "") {
                                                                                        fileNameString = file[i].name;
                                                                                    } else {
                                                                                        fileNameString = fileNameString + '||' + file[i].name;
                                                                                    }

                                                                                }
                                                                            }
                                                                        }
                                                                        rowItem.row.data.Upload_File_Name = fileNameString;
                                                                        Selected = [];
                                                                        Selected.push(rowItem.row.data);
                                                                        Update(Selected, filtered_yr, 'FileUpload');
                                                                        popup.hide();
                                                                    } else {
                                                                        alert("Please upload the any file.")
                                                                    }
                                                                },
                                                            },
                                                        },
                                                        {
                                                            widget: 'dxButton',
                                                            toolbar: 'bottom',
                                                            location: 'after',
                                                            options: {
                                                                text: 'Cancel',
                                                                onClick() {
                                                                    popup.hide();
                                                                },
                                                            },
                                                        }
                                                        ],
                                                    }).dxPopup('instance');

                                                }
                                            }]
                                    },
                                   // {
                                        //    /*caption: "Review Request Details",*/
                                        //    alignment: "center",
                                       // columns: [
                                            //{

                                            //    dataField: "VKM_Year",
                                            //    validationRules: [{ type: "required" }],
                                            //    width: 100,
                                            //    lookup: {
                                            //        dataSource: yr_list,
                                            //        valueExpr: "Year",
                                            //        displayExpr: "Year"
                                            //    },
                                            //    allowEditing: false

                                            //},
                                            {

                                                dataField: "RequestID",
                                                allowEditing: false,
                                                visible: true,
                                                width: 100
                                            },
                                            {

                                                dataField: "isProjected",

                                                width: 130,
                                                dataType: "boolean",
                                                //allowEditing: flag,
                                                visible: false,
                                                //disabled: function (e) {
                                                //    //debugger;
                                                //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0) //true -f01/ f03 or apprcost <= 0 or before vkm spoc approval

                                                //}
                                            },
                                            {

                                                dataField: "Q1",

                                                width: 70,
                                                dataType: "boolean",
                                                allowEditing: flag,
                                                visible: false,
                                                //disabled: function (e) {
                                                //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 1) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                                                //}
                                            },
                                            {

                                                dataField: "Q2",

                                                width: 70,
                                                dataType: "boolean",
                                                allowEditing: flag,
                                                visible: false,
                                                //disabled: function (e) {
                                                //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 2) //true -f01/ f03 or apprcost <= 0

                                                //}
                                            },
                                            {

                                                dataField: "Q3",

                                                width: 70,
                                                dataType: "boolean",
                                                allowEditing: flag,
                                                visible: false,
                                                //disabled: function (e) {
                                                //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 3) //true -f01/ f03 or apprcost <= 0

                                                //}
                                            },
                                            {

                                                dataField: "Q4",

                                                width: 70,
                                                dataType: "boolean",
                                                allowEditing: flag,
                                                visible: false,
                                                //disabled: function (e) {
                                                //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 4) //true -f01/ f03 or apprcost <= 0

                                                //}
                                            },
                                            {
                                                dataField: "Projected_Amount",
                                                caption: "Projected Amt ($)",
                                                width: 150,
                                                dataType: "number",
                                                format: { type: "currency", precision: 2 },
                                                allowEditing: flag,
                                                visible: false,
                                                //disabled: function (e) {
                                                //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0) //true -f01/ f03 or apprcost <= 0

                                                //}


                                            },
                                            {
                                                dataField: "Unused_Amount",
                                                caption: "Unused Amt ($)",
                                                width: 150,
                                                dataType: "number",
                                                format: { type: "currency", precision: 2 },
                                                allowEditing: false,
                                                visible: false,
                                                setCellValue: function (rowData, value) {

                                                    rowData.Unused_Amount = value;

                                                },
                                                //disabled: function (e) {
                                                //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0) //true -f01/ f03 or apprcost <= 0

                                                //}

                                            },
                                            {

                                                dataField: "BU",
                                                validationRules: [{ type: "required" }],
                                                width: 70,
                                                lookup: {
                                                    dataSource: BU_list,
                                                    valueExpr: "ID",
                                                    displayExpr: "BU"
                                                },
                                                allowEditing: true,
                                                headerFilter: {
                                                    dataSource: BU_headerFilter,
                                                    allowSearch: true
                                                },
                                            },

                                            //},
                                            {
                                                dataField: "OEM",
                                                width: 100,
                                                validationRules: [{ type: "required" }],
                                                headerFilter: {
                                                    dataSource: OEM_headerFilter,
                                                    allowSearch: true
                                                },
                                                lookup: {
                                                    dataSource: OEM_list,
                                                    valueExpr: "ID",
                                                    displayExpr: "OEM"
                                                },
                                                allowEditing: true

                                            },
                                            {
                                                dataField: "DEPT",
                                                caption: "Dept",
                                                headerFilter: {
                                                    dataSource: DEPT_headerFilter,
                                                    allowSearch: true
                                                },
                                                validationRules: [{ type: "required" }],
                                                setCellValue: function (rowData, value) {

                                                    rowData.DEPT = value;
                                                    rowData.Group = null;

                                                },
                                                width: 140,
                                                lookup: {
                                                    dataSource: function (options) {

                                                        return {

                                                            store: DEPT_list
                                                            //filter: options.data ? ["Outdated", "=", false] : null


                                                        };
                                                    },

                                                    valueExpr: "ID",
                                                    displayExpr: "DEPT"

                                                },
                                                allowEditing: true


                                            },
                                            {
                                                dataField: "Group",
                                                width: 150,
                                                headerFilter: {
                                                    dataSource: Group_headerFilter,
                                                    allowSearch: true
                                                },
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
                                                allowEditing: true
                                            },

                                            {
                                                dataField: "Project",
                                                width: 90,
                                                allowEditing: !flag,
                                                visible: false

                                            },
                                            {
                                                dataField: "Item_Name",
                                                caption: "Item",
                                                minWidth: 300,
                                                validationRules: [{ type: "required" }],
                                                lookup: {
                                                    dataSource: function (options) {
                                                        // //debugger;
                                                        return {


                                                            store: /*Item_list_bkp*/ /*Item_list_New*/Item_list,

                                                            filter: options.data ? [["BU", "=", BU_forItemFilter != 0 ? BU_forItemFilter : options.data.BU], 'and', ["Deleted", "=", false]] : null
                                                        }


                                                    },
                                                    valueExpr: "S_No",
                                                    displayExpr: "Item_Name"
                                                },
                                                headerFilter: {
                                                    dataSource: Item_headerFilter,
                                                    allowSearch: true
                                                },
                                                setCellValue: function (rowData, value) {
                                                    ////debugger;
                                                    //if value.constructur.name == "Array" => it means that the item list is filtered based on the ordertype selected. At that instance, no need to fetch other details
                                                    if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                                    {
                                                        rowData.Item_Name = value;
                                                        rowData.Category = Item_list.find(x => x.S_No == value).Category;
                                                        rowData.Cost_Element = parseInt(Item_list.find(x => x.S_No == value).Cost_Element);
                                                        rowData.Unit_Price = Item_list.find(x => x.S_No == value).UnitPriceUSD;
                                                        //rowData.ActualAvailableQuantity = Item_list.find(x => x.S_No == value).Actual_Available_Quantity;
                                                        rowData.BudgetCode = Item_list.find(x => x.S_No == value).BudgetCode;
                                                        rowData.BudgetCodeDescription = BudgetCodeList.find(x => x.Budget_Code == rowData.BudgetCode).Budget_Code_Description;
                                                        rowData.UnitofMeasure = parseInt(Item_list.find(x => x.S_No == value).UOM);
                                                    }
                                                },
                                                //filterType: "include",
                                                //filterValues: [3085],
                                                //lookup: {
                                                //    dataSource: function (options) {

                                                //        return {

                                                //            store: Item_list,
                                                //            filter: options.data ? [["BU", "=", options.data.BU], 'and', ["Deleted", "=", false]] : null

                                                //        };

                                                //    },
                                                //    valueExpr: "S_No",
                                                //    displayExpr: "Item_Name"
                                                //},
                                                allowEditing: true
                                            },
                                            {
                                                dataField: "OrderType",
                                                caption: "Order Type",
                                                setCellValue: function (rowData, value) {
                                                    ////debugger;
                                                    rowData.OrderType = value;
                                                    rowData.Item_Name = null;

                                                },
                                                lookup: {
                                                    dataSource: function (options) {
                                                        //  //debugger;
                                                        return {

                                                            store: OrderType_list,
                                                        }

                                                    },
                                                    valueExpr: "ID",
                                                    displayExpr: "Order_Type"
                                                },
                                                visible: false

                                            },
                                            {
                                                dataField: "CostCenter",
                                                caption: "Cost Center",
                                                allowEditing: false,
                                                visible: false
                                            },
                                            {
                                                dataField: "BudgetCenterID",
                                                caption: "Budget Center",
                                                lookup: {
                                                    dataSource: function (options) {
                                                        // //debugger;
                                                        return {

                                                            store: BudCenter,
                                                        }

                                                    },
                                                    valueExpr: "ID",
                                                    displayExpr: "BudgetCenter",
                                                },

                                                visible: false,
                                                //allowEditing: false,

                                            },
                                            {
                                                dataField: "UnitofMeasure",
                                                caption: "Unit of Measure",
                                                lookup: {
                                                    dataSource: function (options) {
                                                        // //debugger;
                                                        return {

                                                            store: UOM_list,


                                                        }

                                                    },
                                                    valueExpr: "ID",
                                                    displayExpr: "UOM"
                                                },
                                                visible: false,
                                                allowEditing: false,
                                            },
                                            {
                                                dataField: "UnloadingPoint",
                                                caption: "Unloading Point",

                                                visible: false,
                                                lookup: {
                                                    dataSource: function (options) {

                                                        return {

                                                            store: UnloadingPoint_list,
                                                        }

                                                    },
                                                    valueExpr: "ID",
                                                    displayExpr: "UnloadingPoint"
                                                },
                                            },

                                            //{
                                            //    dataField: 'Comments',
                                            //    label: {
                                            //        text: 'Comments'
                                            //    },
                                            //    dataType: 'string',
                                            //},

                                            {
                                                dataField: "ActualAvailableQuantity",
                                                caption: "Available Qty",
                                                allowEditing: false,
                                                width: 110


                                            },
                                            {
                                                dataField: "Category",
                                                caption: "Category",
                                                //validationRules: [{ type: "required" }],
                                                headerFilter: {
                                                    dataSource: Category_headerFilter,
                                                    allowSearch: true
                                                },
                                                lookup: {
                                                    dataSource: Category_list,
                                                    valueExpr: "ID",
                                                    displayExpr: "Category"
                                                },
                                                allowEditing: false,
                                                visible: false

                                            },
                                            //{
                                            //    dataField: "BudgetCode",
                                            //    visible: false
                                            //    },
                                            {
                                                dataField: "Cost_Element",
                                                headerFilter: {
                                                    dataSource: CostElement_headerFilter,
                                                    allowSearch: true
                                                },
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
                                                headerFilter: {
                                                    dataSource: BudgetCode_headerFilter,
                                                    allowSearch: true
                                                },
                                                lookup: {
                                                    dataSource: BudgetCodeList,
                                                    valueExpr: "Budget_Code",
                                                    displayExpr: "Budget_Code"
                                                },
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
                                                dataField: "LabName",
                                                caption: "Lab Name",
                                                visible: false
                                            },
                                            {
                                                dataField: 'RFOReqNTID',
                                                setCellValue: function (rowData, value) {

                                                    rowData.RFOReqNTID = value;

                                                },
                                                visible: false,
                                                allowEditing: false,
                                            },
                                            {
                                                dataField: "RFOApprover",
                                                caption: "RFO Approver",
                                                visible: false

                                            },
                                            {
                                                dataField: "QuoteAvailable",
                                                caption: "Quote Available",
                                                visible: false,
                                                editCellTemplate: editCellTemplate


                                            },
                                            {
                                                dataField: "GoodsRecID",
                                                caption: "Goods Rec ID",
                                                visible: false

                                            },
                                            {
                                                dataField: "Required_Quantity",
                                                caption: "Request Qty",
                                                width: 100,
                                                //validationRules: [
                                                //    { type: "required" },
                                                //{
                                                //    type: "range",
                                                //    message: "Quantity cannot be negative",
                                                //    min: 0,
                                                //    max: 214783647
                                                //}],
                                                dataType: "number",
                                                setCellValue: function (rowData, value) {

                                                    rowData.Required_Quantity = value;

                                                },
                                                allowEditing: false,
                                                visible: true





                                            },
                                            {
                                                dataField: "Unit_Price",
                                                caption: "Unit Price",
                                                dataType: "number",
                                                format: { type: "currency", precision: 0 },
                                                valueFormat: "#0",
                                                // validationRules: [{ type: "required" }, {
                                                //    type: "range",
                                                //    message: "Please enter valid price > 0",
                                                //    min: 0.01,
                                                //    max: Number.MAX_VALUE
                                                //}],
                                                allowEditing: false,
                                                visible: false

                                            },
                                            {
                                                dataField: "Total_Price",
                                                caption: "Amt"
                                                , calculateCellValue: function (rowData) {

                                                    if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
                                                        return rowData.Unit_Price * rowData.Required_Quantity;
                                                    }
                                                    else
                                                        return 0.0;
                                                },

                                                dataType: "number",
                                                format: { type: "currency", precision: 0 },
                                                valueFormat: "#0",
                                                allowEditing: false,
                                                visible: false
                                            },
                                            {
                                                dataField: "Reviewed_Quantity",
                                                caption: "Review Qty",
                                                width: 100,
                                                dataType: "number",
                                                validationRules: [
                                                    { type: "required" },
                                                    {
                                                        type: "range",
                                                        message: "Quantity cannot be negative",
                                                        min: -0.1,
                                                        max: 214783647
                                                    }],
                                                setCellValue: function (rowData, value) {
                                                    // //debugger;
                                                    rowData.Reviewed_Quantity = value;

                                                },
                                                allowEditing: function (e) {
                                                    // //debugger;
                                                    if (e.row.data.ApprovedSH == undefined)
                                                        e.row.data.ApprovedSH = false;
                                                    return !e.row.data.ApprovedSH
                                                },



                                                //labteam - edit - true                  1 || 1
                                                //vkmspoc - approvedsh - false - edit    0 || 1
                                                //vkmspoc - approvedsh - true - not edit 0 || 0



                                            },
                                            {
                                                dataField: "Reviewed_Cost",
                                                caption: "Review Amt",
                                                width: 120,
                                                //calculateCellValue: function (rowData) {

                                                //    //if (/*(rowData.Reviewed_Cost == null || rowData.Reviewed_Cost == undefined) && */rowData.Reviewed_Quantity > 0 && rowData.Unit_Price > 0) {
                                                //    //    return rowData.Unit_Price * rowData.Reviewed_Quantity;
                                                //    //}
                                                //    //else if (rowData.Reviewed_Cost != null || rowData.Reviewed_Cost != undefined) {
                                                //    //    return rowData.Reviewed_Cost;
                                                //    //}
                                                //    //else
                                                //    //    return 0.0;
                                                //},

                                                dataType: "number",
                                                format: { type: "currency", precision: 0 },
                                                valueFormat: "#0",
                                                allowEditing: false,
                                                setCellValue: function (rowData, value) {
                                                    // //debugger;
                                                    rowData.Reviewed_Cost = value;

                                                },
                                            },
                                            {
                                                dataField: "Requestor",
                                                allowEditing: false,
                                                visible: false
                                            },
                                            {
                                                dataField: "Comments",
                                                caption: "Remark",
                                                width: 140,
                                                allowResizing: true
                                                // allowEditing: false,
                                            },
                                            {
                                                dataField: "PORemarks",
                                                width: 140,
                                                visible: false

                                            },
                                            {
                                                dataField: "SubmitDate",
                                                allowEditing: false,
                                                visible: false
                                            },
                                            {
                                                dataField: "OrderID",
                                                caption: "PO Number",
                                                visible: false,

                                                allowEditing: function (e) {
                                                    if (e.row.data.ApprovedSH == undefined)
                                                        e.row.data.ApprovedSH = false;

                                                    return flag || !e.row.data.ApprovedSH
                                                },

                                                //allowEditing: flag || !e.row.data.ApprovedSH  

                                            },
                                            {
                                                dataField: "Currency",
                                                lookup: {
                                                    dataSource: Currency_list,
                                                    valueExpr: "ID",
                                                    displayExpr: "Currency"
                                                },
                                                visible: false,
                                                validationRules: [{
                                                    type: "required",
                                                    message: "Currency is required for SR,PR,PO,Invoice amounts"
                                                }],
                                            },
                                            {
                                                dataField: 'SR_Value',
                                                caption: 'SR Value',
                                                visible: false,

                                            }, {
                                                dataField: 'PR_Value',
                                                caption: 'PR Value',
                                                visible: false,

                                            }, {
                                                dataField: 'Invoice_Value',
                                                visible: false,

                                            }, {
                                                dataField: 'OrderPrice_UserInput',
                                                caption: "Order Price",
                                                visible: false,
                                            },
                                            {
                                                dataField: "OrderPrice",
                                                visible: false,
                                                caption: "Order Price (USD)",
                                                //calculateCellValue: function (rowData) {
                                                //    var orderpriceinusd;

                                                //   ////debugger;
                                                //    //based on currency chosen & price entered, convert the order price to usd value
                                                //    if (rowData.OrderPrice >0 && rowData.Currency != undefined) {

                                                //        $.ajax({

                                                //            type: "GET",
                                                //            url: "/BudgetingVKM/GetOrderPriceinUSD",
                                                //            data: { 'OrderPrice': rowData.OrderPrice, 'Currency': rowData.Currency },
                                                //            datatype: "json",
                                                //            async: false,
                                                //            success: success_getorder_priceusd,
                                                //            error: error_getorder_priceusd

                                                //        });

                                                //        function success_getorder_priceusd(response) {
                                                //           ////debugger;
                                                //            orderpriceinusd = response;


                                                //        }


                                                //        function error_getorder_priceusd(response) {
                                                //           ////debugger;
                                                //            $.notify('Error in converting the entered Order Price to USD!', {
                                                //                globalPosition: "top center",
                                                //                className: "warn"
                                                //            });
                                                //        }
                                                //    }

                                                //    return orderpriceinusd;


                                                //},

                                                format: { type: "currency", precision: 2 },
                                                valueFormat: "#0.00",
                                                //allowEditing: flag || !e.row.data.ApprovedSH 
                                                allowEditing: function (e) {
                                                    if (e.row.data.ApprovedSH == undefined)
                                                        e.row.data.ApprovedSH = false;
                                                    return flag || !e.row.data.ApprovedSH
                                                },

                                            },

                                            {
                                                dataField: "OrderedQuantity",
                                                caption: "Ordered Qty",
                                                visible: false,
                                                // allowEditing: flag || !e.row.data.ApprovedSH 
                                                allowEditing: function (e) {
                                                    ////debugger;
                                                    if (e.row.data.ApprovedSH == undefined)
                                                        e.row.data.ApprovedSH = false;
                                                    return flag || !e.row.data.ApprovedSH
                                                },


                                            },
                                            {
                                                dataField: "OrderStatus",
                                                //headerFilter: {
                                                //    dataSource: OrderStatus_headerFilter,
                                                //    allowSearch: true
                                                //},
                                                visible: true,

                                                //setCellValue: function (rowData, value) {
                                                //    //debugger;
                                                //    rowData.OrderStatus = value;

                                                //    if (value == 3) {
                                                //        var today = new Date();
                                                //        var dd = String(today.getDate()).padStart(2, '0');
                                                //        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                                                //        var yyyy = today.getFullYear();

                                                //        today = yyyy + '-' + mm + '-' + dd;
                                                //        rowData.ELOSubmittedDate = today;
                                                //    }
                                                //    //else
                                                //    //    rowData.ELOSubmittedDate = null;
                                                //},
                                                setCellValue: function (rowData, value, currentRowData) {
                                                    //debugger;
                                                    //if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                                    {
                                                        rowData.OrderStatus = value;

                                                        if (value == 3) {
                                                            var today = new Date();
                                                            var dd = String(today.getDate()).padStart(2, '0');
                                                            var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                                                            var yyyy = today.getFullYear();

                                                            today = yyyy + '-' + mm + '-' + dd;
                                                            rowData.ELOSubmittedDate = today;
                                                        } else
                                                            rowData.ELOSubmittedDate = currentRowData.ELOSubmittedDate;



                                                        if (currentRowData.RequestOrderDate != undefined) {
                                                            rfodate = Date.parse(currentRowData.RequestOrderDate);
                                                            elodate = Date.parse(rowData.ELOSubmittedDate);

                                                            var Difference_In_Time = Math.abs(elodate - rfodate);

                                                            // To calculate the no. of days between two dates
                                                            var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                                            rowData.DaysTaken = Difference;
                                                        }


                                                    }

                                                },

                                                lookup: {
                                                    dataSource: function (options) {

                                                        return {

                                                            store: OrderStatus_list,


                                                        };
                                                    },

                                                    valueExpr: "ID",
                                                    displayExpr: "OrderStatus"

                                                },
                                                //allowEditing: flag || !e.row.data.ApprovedSH 
                                                allowEditing: flag
                                                //function (e) {
                                                //if (e.row.data.ApprovedSH == undefined)
                                                //    e.row.data.ApprovedSH = false;
                                                //return flag //|| !e.row.data.ApprovedSH
                                                //},



                                            },
                                            {
                                                dataField: "Description",
                                                caption: "Order Status Description",
                                                visible: true,
                                                lookup: {
                                                    dataSource: function (options) {

                                                        return {

                                                            store: OrderDescription_list,


                                                        };
                                                    },

                                                    valueExpr: "Description",
                                                    displayExpr: "Description"

                                                },
                                                allowEditing: flag

                                            },
                                            {
                                                dataField: "LinkedRequests",
                                                caption: 'Linked Requests',
                                                visible: false,

                                            },
                                            {
                                                dataField: "LinkedRequestID",
                                                caption: 'Linked Request ID',
                                                visible: false,
                                                // groupIndex: 0,

                                            },
                                            {
                                                dataField: "Reviewer_1",
                                                caption: "HOE",
                                                allowEditing: false,
                                                visible: false
                                            },
                                            {
                                                dataField: "Reviewer_2",
                                                caption: "VKM SPOC",
                                                allowEditing: false,
                                                visible: false
                                            },
                                            {
                                                dataField: "RequiredDate",
                                                dataType: "date",
                                                allowEditing: true,
                                                visible: false

                                            },
                                            //{
                                            //    dataField: "RequestOrderDate",
                                            //    dataType: "date",
                                            //    allowEditing: false,
                                            //    visible: false//flag

                                            //},
                                            {
                                                dataField: "RequestOrderDate",
                                                caption: "RFO Submitted Date",
                                                dataType: "date",
                                                allowEditing: false,
                                                visible: false,
                                                setCellValue: function (rowData, value) {

                                                    rowData.RFOSubmittedDate = value;

                                                },
                                            },
                                            {
                                                dataField: "OrderDate",
                                                caption: "PO Release Date",
                                                dataType: "date",
                                                allowEditing: true,
                                                visible: false//flag

                                            },
                                            {
                                                dataField: "TentativeDeliveryDate",
                                                caption: "Tentative Dt",
                                                dataType: "date",
                                                visible: /*flag*/false,
                                                allowEditing: function (e) {
                                                    if (e.row.data.ApprovedSH == undefined)
                                                        e.row.data.ApprovedSH = false;
                                                    return flag || !e.row.data.ApprovedSH
                                                },
                                                //allowEditing: flag || !e.row.data.ApprovedSH 

                                            },
                                            {
                                                dataField: "ActualDeliveryDate",
                                                dataType: "date",
                                                caption: "Actual Dt",
                                                visible: false,//flag,
                                                allowEditing: function (e) {
                                                    if (e.row.data.ApprovedSH == undefined)
                                                        e.row.data.ApprovedSH = false;
                                                    return flag || !e.row.data.ApprovedSH
                                                },
                                                //allowEditing: flag || !e.row.data.ApprovedSH 

                                            },
                                            {
                                                dataField: "Fund",

                                                setCellValue: function (rowData, value) {

                                                    rowData.Fund = value;


                                                },
                                                visible: flag,
                                                allowEditing: function (e) {
                                                    if (e.row.data.ApprovedSH == undefined)
                                                        e.row.data.ApprovedSH = false;
                                                    return flag || !e.row.data.ApprovedSH
                                                },
                                                //allowEditing: flag || !e.row.data.ApprovedSH ,
                                                lookup: {
                                                    dataSource: function (options) {

                                                        return {

                                                            store: Fund_list,


                                                        };
                                                    },

                                                    valueExpr: "ID",
                                                    displayExpr: "Fund",

                                                }
                                            },

                                            {
                                                dataField: "Customer_Name",
                                                allowEditing: true,
                                                visible: false
                                            },
                                            {
                                                dataField: "Customer_Dept",
                                                allowEditing: true,
                                                visible: false
                                            },
                                            {
                                                dataField: "BM_Number",
                                                allowEditing: true,
                                                visible: false
                                            },
                                            {
                                                dataField: "Task_ID",
                                                allowEditing: true,
                                                visible: false
                                            },
                                            {
                                                dataField: "Resource_Group_Id",
                                                allowEditing: true,
                                                visible: false
                                            },
                                            {
                                                dataField: "PIF_ID",
                                                caption: "PIF ID (WBS)",
                                                allowEditing: true,
                                                visible: false
                                            },
                                            {
                                                dataField: "Project_ID",
                                                allowEditing: true,
                                                visible: false

                                            },
                                            {
                                                dataField: "Purchase_Type",
                                                allowEditing: true,
                                                visible: false,
                                                lookup: {
                                                    dataSource: function (options) {
                                                        return {
                                                            store: PurchaseType_list,
                                                        };
                                                    },
                                                    valueExpr: "ID",
                                                    displayExpr: "PurchaseType"
                                                }

                                            },
                                            {
                                                dataField: 'Material_Part_Number',
                                                dataType: 'string',
                                                visible: false,
                                            },
                                            {
                                                dataField: "SupplierName_with_Address",
                                                allowEditing: true,
                                                visible: false

                                            },
                                            {
                                                dataField: "POSpocNTID",
                                                caption: "PO Spoc NTID",
                                                allowEditing: false,
                                                visible: false,

                                            },
                                            //{
                                            //    dataField: "RequestOrderDate",
                                            //    caption: "RFO Submitted Date",
                                            //    allowEditing: false,
                                            //    visible: false,
                                            //    setCellValue: function (rowData, value) {

                                            //        rowData.RFOSubmittedDate = value;

                                            //    },
                                            //},
                                            {
                                                dataField: "ELOSubmittedDate",
                                                caption: "ELO Submitted Date",
                                                allowEditing: true,
                                                visible: false,
                                                //setCellValue: function (rowData, value) {

                                                //    rowData.ELOSubmittedDate = value;


                                                //},
                                                //setCellValue: function (rowData, value, currentRowData) {
                                                //    //debugger;
                                                //    //if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                                //    {

                                                //        rowData.ELOSubmittedDate = value;
                                                //        if (currentRowData.RequestOrderDate != undefined) {
                                                //            rfodate = Date.parse(currentRowData.RequestOrderDate);
                                                //            elodate = Date.parse(rowData.ELOSubmittedDate);

                                                //            var Difference_In_Time = Math.abs(elodate - rfodate);

                                                //            // To calculate the no. of days between two dates
                                                //            var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                                //            rowData.DaysTaken = Difference;
                                                //        }


                                                //    }

                                                //},

                                                //calculateCellValue: function (rowData) {
                                                //    //debugger;
                                                //    if (rowData.OrderStatus == 3) {

                                                //        var today = new Date();
                                                //        var dd = String(today.getDate()).padStart(2, '0');
                                                //        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                                                //        var yyyy = today.getFullYear();

                                                //        today = mm + '/' + dd + '/' + yyyy;

                                                //        return today;
                                                //    }
                                                //    else
                                                //        return 0;
                                                //},
                                            },
                                            {
                                                dataField: "DaysTaken",
                                                caption: "Days Taken",
                                                allowEditing: true,
                                                visible: false,
                                                dataType: "number",
                                                customizeText: function (cellInfo) {
                                                    //debugger;
                                                    if (
                                                        cellInfo.value === "" ||
                                                        cellInfo.value === null ||
                                                        cellInfo.value === undefined ||
                                                        cellInfo.valueText === "NaN"
                                                    ) {
                                                        return "NA";
                                                    } else {
                                                        return cellInfo.valueText;
                                                    }
                                                },
                                                //calculateCellValue: function (rowData) {
                                                //    //debugger;
                                                //    if (rowData.ELOSubmittedDate != null && rowData.RequestOrderDate != null) {

                                                //        rfodate = Date.parse(rowData.RequestOrderDate);
                                                //        elodate = Date.parse(rowData.ELOSubmittedDate);

                                                //        var Difference_In_Time = Math.abs(elodate - rfodate);

                                                //        // To calculate the no. of days between two dates
                                                //        var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                                //        return Difference;
                                                //    }
                                                //    else
                                                //        return 0;
                                                //},
                                            },
                                            {
                                                dataField: "SRSubmitted",
                                                caption: "SR Submitted",
                                                dataType: "date",
                                                allowEditing: true,
                                                visible: false,
                                                setCellValue: function (rowData, value, currentRowData) {
                                                    //debugger;
                                                    //if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                                    {

                                                        rowData.SRSubmitted = value;
                                                        if (currentRowData.SRAwardedDate != undefined) {
                                                            srawardeddate = Date.parse(currentRowData.SRAwardedDate);
                                                            srsubmitteddate = Date.parse(rowData.SRSubmitted);

                                                            var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                                                            // To calculate the no. of days between two dates
                                                            var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                                            rowData.SRApprovalDays = Difference;
                                                        }


                                                    }

                                                },

                                                //setCellValue: function (rowData, value) { IF UNCOMMENTED, DATEBOX DOESNOT OPENS ON SINGLE CLK OF THE CELL ; OPENS AFTER TWO CLICKS ONLY 

                                                //    rowData.SRSubmitted = value;
                                                //}

                                            },
                                            {
                                                dataField: "RFQNumber",
                                                caption: "RFQ Number",
                                                allowEditing: true,
                                                visible: false,
                                                setCellValue: function (rowData, value) {

                                                    rowData.RFQNumber = value;

                                                    if (value != null) {
                                                        var today = new Date();
                                                        var dd = String(today.getDate()).padStart(2, '0');
                                                        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                                                        var yyyy = today.getFullYear();

                                                        today = yyyy + '-' + mm + '-' + dd;
                                                        rowData.SRSubmitted = today;
                                                    }

                                                },

                                            },
                                            {
                                                dataField: "PRNumber",
                                                caption: "PR Number",
                                                allowEditing: true,
                                                visible: false
                                            },
                                            //{
                                            //    dataField: "OrderID",
                                            //    caption: "PO Number",
                                            //    allowEditing: true,
                                            //    visible: false
                                            //},
                                            //{
                                            //    dataField: "OrderDate",
                                            //    caption: "PO Release Date",
                                            //    allowEditing: true,
                                            //    visible: false
                                            //},
                                            {
                                                dataField: "SRAwardedDate",
                                                caption: "SRAwardedDate",
                                                dataType: "date",
                                                allowEditing: true,
                                                visible: false,
                                                //setCellValue: function (rowData, value) {

                                                //    rowData.SRAwardedDate = value;
                                                //}
                                                setCellValue: function (rowData, value, currentRowData) {
                                                    //debugger;
                                                    //if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                                    {

                                                        rowData.SRAwardedDate = value;
                                                        if (currentRowData.SRSubmitted != undefined) {
                                                            srawardeddate = Date.parse(rowData.SRAwardedDate);
                                                            srsubmitteddate = Date.parse(currentRowData.SRSubmitted);

                                                            var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                                                            // To calculate the no. of days between two dates
                                                            var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                                            rowData.SRApprovalDays = Difference;
                                                        }


                                                    }

                                                },

                                            },
                                            {
                                                dataField: "SRApprovalDays",
                                                caption: "SR Approval Days",
                                                allowEditing: true,
                                                visible: false,
                                                dataType: "number",
                                                customizeText: function (cellInfo) {
                                                    //debugger;
                                                    if (
                                                        cellInfo.value === "" ||
                                                        cellInfo.value === null ||
                                                        cellInfo.value === undefined ||
                                                        cellInfo.valueText === "NaN"
                                                    ) {
                                                        return "NA";
                                                    } else {
                                                        return cellInfo.valueText;
                                                    }
                                                },
                                                setCellValue: function (rowData, value) {
                                                    //debugger;
                                                    rowData.SRApprovalDays = value;
                                                }
                                                //calculateCellValue: function (rowData) {
                                                //    //debugger;
                                                //    if (rowData.SRAwardedDate != null && rowData.SRSubmitted != null) {

                                                //        srawardeddate = Date.parse(rowData.SRAwardedDate);
                                                //        srsubmitteddate = Date.parse(rowData.SRSubmitted);

                                                //        var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                                                //        // To calculate the no. of days between two dates
                                                //        var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                                //        return Difference;
                                                //    }
                                                //    else
                                                //        return 0;
                                                //},

                                            },
                                            {
                                                dataField: "SRResponsibleBuyerNTID",
                                                caption: "SR Responsible Buyer",
                                                allowEditing: true,
                                                visible: false,
                                                lookup: {
                                                    dataSource: function (options) {

                                                        return {

                                                            store: SRBuyer_list,


                                                        };
                                                    },

                                                    valueExpr: "NTID",
                                                    displayExpr: "BuyerName"

                                                },
                                                setCellValue: function (rowData, value) {
                                                    //debugger;
                                                    if (value.constructor.name == "String")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                                    {

                                                        rowData.SRResponsibleBuyerNTID = value;
                                                        var mgrID = SRBuyer_list.find(x => x.NTID == value).Manager_ID;
                                                        rowData.SRManagerNTID = SRManager_list.find(x => x.ID == mgrID).NTID;
                                                        //debugger;
                                                        /*rowData.BudgetCodeDescription = BudgetCodeList.find(x /=> x.Budget_Code == rowData.BudgetCode).Budget_Code_Description;*/
                                                    }
                                                },

                                            },
                                            {
                                                dataField: "SRManagerNTID",
                                                caption: "SR Manager",
                                                allowEditing: true,
                                                visible: false,
                                                lookup: {
                                                    dataSource: function (options) {

                                                        return {

                                                            store: SRManager_list,


                                                        };
                                                    },

                                                    valueExpr: "NTID",
                                                    displayExpr: "ManagerName"

                                                },
                                            },
                                            {
                                                caption: "Item Change Log",
                                                allowEditing: false,
                                                visible: true,

                                                width: 100,
                                                alignment: 'center',
                                                cellTemplate: function (container, options) {
                                                    $('<a/>').addClass('dx-link')
                                                        .text('View')
                                                        .on('dxclick', function () {
                                                            //debugger;
                                                            //var thePath = "\\bosch.com\dfsrb\DfsIN\LOC\Kor\BE-ES\ELO\Global";

                                                            //window.open("file://///bosch.com\\dfsrb\\DfsIN\\LOC\\Kor\\BE-ES\\ELO\\Global"); 
                                                            //Do something with options.data;  
                                                            //debugger;
                                                            //alert(options.data.RequestID);
                                                            $.ajax({

                                                                type: "POST",
                                                                url: "/BudgetingVKM/GetL2Details",
                                                                data: { 'RequestID': options.data.RequestID },
                                                                datatype: "json",
                                                                async: true,
                                                                success: function (data) {
                                                                    //debugger;
                                                                    //$("#myModal").css({ "display": "block" });
                                                                    //alert(data.data.RequestDate);
                                                                    //alert(data.data.L1Remarks);
                                                                    //$("#myModal").modal("show");

                                                                    $("#popup").dxPopup({
                                                                        showTitle: true,
                                                                        title: "L2 Item Change Log",
                                                                        visible: true,
                                                                        hideOnOutsideClick: true,
                                                                        width: 450,
                                                                        height: 350,
                                                                        resizeEnabled: true

                                                                    });
                                                                    //debugger;
                                                                    //$("#dxL1Details").dxDataGrid({
                                                                    //    dataSource: data.data,
                                                                    //    //keyExpr: "EmployeeID",
                                                                    //    visible: true,
                                                                    //    columns: [{
                                                                    //        caption:"Remarks",
                                                                    //        dataField: "L1Remarks"
                                                                    //    }, {
                                                                    //        caption: "Request Date",
                                                                    //        dataField: "RequestDate"
                                                                    //    }]
                                                                    //});

                                                                    $("#dxL2Details").dxForm({
                                                                        //formData: {
                                                                        //    Remarks: data.data.L1Remarks,
                                                                        //    RequestDate: data.data.RequestDate
                                                                        //}

                                                                        formData: data.data,
                                                                        items: [{
                                                                            caption: "L1 Remarks",
                                                                            dataField: 'L1Remarks',
                                                                            editorOptions: {
                                                                                disabled: true,
                                                                            },
                                                                        },
                                                                        {
                                                                            caption: "L1 Submit Date",
                                                                            dataField: 'L1SubmitDate',
                                                                            editorOptions: {
                                                                                disabled: true,
                                                                            },
                                                                        },
                                                                        {
                                                                            caption: "L1 Qty",
                                                                            dataField: 'L1Qty',
                                                                            editorOptions: {
                                                                                disabled: true,
                                                                            },
                                                                        },
                                                                        {
                                                                            caption: "L2 Remarks",
                                                                            dataField: 'L2Remarks',
                                                                            editorOptions: {
                                                                                disabled: true,
                                                                            },
                                                                        },
                                                                        {
                                                                            caption: "L2 Review Date",
                                                                            dataField: 'L2ReviewDate',
                                                                            editorOptions: {
                                                                                disabled: true,
                                                                            },
                                                                        },
                                                                        {
                                                                            caption: "L2 Qty",
                                                                            dataField: 'L2Qty',
                                                                            editorOptions: {
                                                                                disabled: true,
                                                                            },
                                                                        }]
                                                                    });
                                                                }
                                                            });

                                                        })
                                                        .appendTo(container);
                                                }

                                            },
                                            {
                                                caption: "Uploaded Files",
                                                //allowEditing: false,
                                                visible: true,
                                                width: 100,
                                                alignment: 'center',
                                                cellTemplate: function (container, options) {
                                                    $('<a/>').addClass('dx-link')
                                                        .text(options.data.RequestID)
                                                        .on('dxclick', function () {
                                                            //Do something with options.data;  
                                                            //debugger;
                                                            //var url;
                                                            $.ajax({



                                                                type: "POST",
                                                                url: "/BudgetingVKM/EncodeURL",
                                                                data: { 'RequestID': options.data.RequestID },
                                                                datatype: "json",
                                                                async: true,
                                                                success: function (data) {
                                                                    //debugger;
                                                                    //window.location.href = data;
                                                                    //url = data.HostURL;
                                                                    window.open(data.Result, '_blank');
                                                                },
                                                                error: function (e) {
                                                                    alert("Unable to redirect");
                                                                },
                                                            })
                                                        }).appendTo(container);
                                                }
                                            },



                                       // ]
                                    //},
                                ],


                                //onEditingStart: function (e) {
                                //    //debugger;
                                //    if (e.data.Reviewed_Quantity) {
                                //        //debugger;
                                //    }

                                //}, 
                                onInitNewRow: function (e) {
                                    //debugger;
                                    is_newitem = true;
                                    //e.data.Requestor = new_request.Requestor;
                                    //e.data.Reviewer_1 = new_request.Reviewer_1;
                                    //e.data.Reviewer_2 = new_request.Reviewer_2;
                                    //e.data.DEPT = new_request.DEPT;
                                    //    e.data.Group = new_request.Group;

                                    e.data.POSpocNTID = new_request.POSpocNTID;
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

                                    e.data.Reviewed_Quantity = new_request.Reviewed_Quantity;
                                    e.data.Reviewed_Cost = new_request.Reviewed_Cost;


                                },
                                onRowUpdated: function (e) {
                                    //debugger;
                                    $.notify("Item in your Queue is being Updated...Please wait!", {
                                        globalPosition: "top center",
                                        autoHideDelay: 15000,
                                        className: "success"
                                    })

                                    ////debugger;
                                    //if (e.data.OrderedQuantity > e.data.Reviewed_Quantity) {
                                    //    $.notify("Ordered Quantity cannot be greater than Reviewed Quantity, Please check again!", {
                                    //        globalPosition: "top center",
                                    //        className: "error"
                                    //    })
                                    //}
                                    debugger;

                                    //Order Price to USD conversion commented - since user inputs are received for sr,pr,invoice,order amounts in either INR/EUR/USD and USD Converted amounts are maintained separately - taken care in sp
                                    //if (e.data.OrderPrice_UserInput > 0 && e.data.Currency != undefined) {
                                    //    //debugger;
                                    //    $.ajax({

                                    //        type: "GET",
                                    //        url: "/BudgetingVKM/GetOrderPriceinUSD",
                                    //        data: { 'OrderPrice': e.data.OrderPrice_UserInput, 'Currency': e.data.Currency },
                                    //        datatype: "json",
                                    //        async: false,
                                    //        success: success_getorder_priceusd,
                                    //        error: error_getorder_priceusd

                                    //    });

                                    //    function success_getorder_priceusd(response) {
                                    //        ////debugger;
                                    //        e.data.OrderPrice = response;


                                    //    }


                                    //    function error_getorder_priceusd(response) {
                                    //        ////debugger;
                                    //        //$.notify('Error in converting the entered Order Price to USD!', {
                                    //        //    globalPosition: "top center",
                                    //        //    className: "warn"
                                    //        //});
                                    //    }
                                    //}


                                    //if (e.data.SRAwardedDate != null && e.data.SRSubmitted != null) {
                                    //    //debugger;
                                    //    srawardeddate = Date.parse(e.data.SRAwardedDate);
                                    //    srsubmitteddate = Date.parse(e.data.SRSubmitted);

                                    //    var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                                    //    // To calculate the no. of days between two dates
                                    //    var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                    //    e.data.SRApprovalDays = Difference;
                                    //}
                                    //else
                                    //    e.data.SRApprovalDays = 0;


                                    //if (e.data.RequestOrderDate != null && e.data.ELOSubmittedDate != null) {
                                    //    //debugger;
                                    //    srawardeddate = Date.parse(e.data.ELOSubmittedDate);
                                    //    srsubmitteddate = Date.parse(e.data.RequestOrderDate);

                                    //    var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                                    //    // To calculate the no. of days between two dates
                                    //    var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                    //    e.data.DaysTaken = Difference;
                                    //}
                                    //else
                                    //    e.data.DaysTaken = 0;

                                    Selected = [];
                                    e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
                                    //e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;

                                    Selected.push(e.data);
                                    ////debugger;
                                    Update(Selected, filtered_yr, '');
                                },
                                onRowInserting: function (e) {
                                    new_request = false;
                                    $.notify("New Item is being added to your cart...Please wait!", {
                                        globalPosition: "top center",
                                        className: "success"
                                    })
                                    debugger;
                                    e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
                                    if (e.data.Reviewed_Cost == null || e.data.Reviewed_Cost == undefined)
                                        e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
                                    //if (e.data.OrderPrice_UserInput > 0 && e.data.Currency != undefined) {

                                    //    $.ajax({

                                    //        type: "GET",
                                    //        url: "/BudgetingVKM/GetOrderPriceinUSD",
                                    //        data: { 'OrderPrice': e.data.OrderPrice_UserInput, 'Currency': e.data.Currency },
                                    //        datatype: "json",
                                    //        async: false,
                                    //        success: success_getorder_priceusd,
                                    //        error: error_getorder_priceusd

                                    //    });

                                    //    function success_getorder_priceusd(response) {
                                    //        debugger;
                                    //        e.data.OrderPrice = response;


                                    //    }


                                    //    function error_getorder_priceusd(response) {
                                    //        debugger;
                                    //        //$.notify('Error in converting the entered Order Price to USD!', {
                                    //        //    globalPosition: "top center",
                                    //        //    className: "warn"
                                    //        //});
                                    //    }
                                    //}

                                    Selected = [];
                                    Selected.push(e.data);
                                    Update(Selected, filtered_yr, '');
                                },
                                onRowRemoving: function (e) {

                                    Delete(e.data.RequestID, filtered_yr);

                                },


                            }).appendTo(container);
                    }

                },
            },

        });
    } else {
        dataGridLEP = $("#RequestTable_VKMSPOC").dxDataGrid({

            //dataSource: objdata,
            dataSource: response.data,

            grouping: {
                autoExpandAll: true,
            },
            twoWayBindingEnabled: false
            ,
            hoverStateEnabled: {
                enabled: true
            },
            showColumnLines: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            columnMinWidth: 50,
            showColumnLines: true,
            showRowLines: true,
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
                                $("#RequestTable_VKMSPOC").dxDataGrid("clearFilter");
                                //$("#buttonClearFilters").dxButton({
                                //    text: 'Clear Filters',
                                //    onClick: function () {
                                //        $("#RequestTable_VKMSPOC").dxDataGrid("clearFilter");
                                //    }
                                //});
                            },
                        },


                    }
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
                columnChooserButton.options.text = "Hide Fields";
                columnChooserButton.options.hint = "Hide Fields";
                columnChooserButton.showText = "always";

            },
            summary: {
                recalculateWhileEditing: true,
                totalItems: [{
                    column: "Item_Name",
                    summaryType: "count",
                    valueFormat: "number",
                    customizeText: function (e) {
                        ////debugger;
                        //I tried add 
                        //console.log(e.value)
                        return "Item Count: " + e.value;
                    }
                }, {
                    column: 'Reviewed_Cost',
                    summaryType: 'sum',
                    valueFormat: 'currency',
                    //customizeText: function (e) {
                    //    ////debugger;
                    //    //I tried add 
                    //    //console.log(e.value)
                    //    return "Review Totals: " + e.value;
                    //}
                }],
            },
            //wordWrapEnabled: true,
            noDataText: " ☺ No VKM Item Request is available in your queue !",
            columnFixing: {
                enabled: true
            },
            width: "97vw", //needed to allow horizontal scroll
            /*height: "70vh",set in layout pg*/
            columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
            //remoteOperations: true,
            /*scrolling: {
                mode: "virtual",
                rowRenderingMode: "virtual",
                columnRenderingMode: "virtual"
            },*/
            //scrolling: {
            //    rowRenderingMode: 'virtual',
            //},
            paging: {
                pageSize: 15,
            },
            pager: {
                visible: true,
                allowedPageSizes: [15, 30, 'all'],
                showPageSizeSelector: true,
                showInfo: true,
                showNavigationButtons: true,
            },
            //paging: {
            //    pageSize: 50
            //},

            editing: {
                // mode: popupedit == 1 ? "popup" : "grid", //"popup",
                mode: editmode,
                // mode: "popup",
                allowUpdating: function (e) {    //Edit access to labteam when requestortoorder triggered ; //Edit access to vkm spoc when approvedsh != true
                    //debugger;
                    //return true;
                    RequestSource = e.row.data.RequestSource; // get request source to check whether it is HOE or RFO

                    return (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && RequestSource == 'RFO' && !e.row.data.VKMSPOC_Approval) || (flag && e.row.data.RequestToOrder && e.row.data.VKMSPOC_Approval == 0 && e.row.data.RequestSource != 'RFO') || (flag && e.row.data.RequestToOrder && e.row.data.VKMSPOC_Approval == 1 && e.row.data.RequestSource == 'RFO') || (!flag && !e.row.data.ApprovedSH && !admin)//with purchase phase //(!flag && e.row.data.RequestToOrder || (!flag && !e.row.data.ApprovedSH))  --ithout purchase phasse
                    //true 
                    //if vkm spoc
                    //if vkm admin
                    //if purchase spoc
                },
                //allowDeleting: function (e) {
                //    ////debugger;
                //    return (flag && e.row.data.RequestToOrder) && isF03F01(e.row.data.Fund); //delete access to remove newly added f03 items by purchase spoc id not needed
                //},
                allowAdding: allowadd,//since both vkm spoc as well as purchase spoc can add new items

                useIcons: true,
                //popup: {
                //    //title: "",
                //    width: 900,
                //    height: 600,
                //    showTitle: true,
                //    visible: true,
                //    hideOnOutsideClick: true,
                //    //width: 450,
                //    //height: 350,
                //    resizeEnabled: true,
                //},
                //popup: {
                //    title: "Purchase SPOC Reviews",
                //},
                popup: {
                    title: "Purchase SPOC Reviews",
                    showTitle: true,
                    visible: true,
                    hideOnOutsideClick: true,
                    //width: 450,
                    //height: 350,
                    resizeEnabled: true,
                },

                form:
                {
                    items: [
                        {
                            itemType: 'group',
                            caption: 'Request Details',
                            colCount: 2,
                            colSpan: 2,
                            items: [
                                {
                                    dataField: 'BU',
                                    label: {
                                        text: 'BU'
                                    },
                                    editorOptions: {
                                        /*value: 6*/
                                    }
                                },
                                {
                                    dataField: 'OEM',
                                    label: {
                                        text: 'OEM'
                                    },

                                },
                                {
                                    dataField: 'DEPT',
                                    label: {
                                        text: 'Department'
                                    },

                                },

                                {
                                    dataField: 'Group',
                                    label: {
                                        text: 'Group'
                                    },

                                },
                                {
                                    dataField: "POSpocNTID",
                                    label: {
                                        text: 'ELO Spoc NTID'
                                    },

                                },
                                {
                                    dataField: 'OrderStatus',
                                    label: {
                                        text: 'Order Status'
                                    },
                                },

                                {
                                    dataField: 'RequestOrderDate',
                                    label: {
                                        text: 'RFO Submitted Date'
                                    },
                                    editorType: 'dxDateBox',
                                    //validationRules: [{
                                    //    type: "required",
                                    //    message: "RFO Submitted Date is required"
                                    //}],
                                    editorOptions: {
                                        //displayFormat: 'datetim',
                                        format: {
                                            type: "shortDate",
                                        },
                                        disabled: true,
                                    },
                                },


                                {
                                    dataField: 'ELOSubmittedDate',
                                    label: {
                                        text: 'ELO Submitted Date'
                                    },
                                    editorType: 'dxDateBox',
                                    editorOptions: {
                                        //displayFormat: 'datetim',
                                        format: {
                                            type: "shortDate",

                                        },
                                        disabled: true,
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],

                                },

                                {
                                    dataField: 'SRManagerNTID',
                                    label: {
                                        text: 'SR Manager'
                                    },
                                    editorOptions: {
                                        //displayFormat: 'datetim',
                                        disabled: true
                                    },
                                    validationRules: [{
                                        type: "required",

                                    }],
                                },


                                {
                                    dataField: 'DaysTaken',
                                    label: {
                                        text: 'Days Taken'
                                    },
                                    editorOptions: {
                                        //displayFormat: 'datetim',

                                        disabled: true,
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],

                                },

                                {
                                    dataField: 'SRResponsibleBuyerNTID',
                                    label: {
                                        text: 'SR Responsible Buyer'
                                    },
                                    validationRules: [{
                                        type: "required",

                                    }],


                                },

                                {
                                    dataField: 'SR_Value',
                                    label: {
                                        text: 'SR Value'
                                    },
                                }, {
                                    dataField: 'PR_Value',
                                    label: {
                                        text: 'PR Value'
                                    },
                                }, {
                                    dataField: 'Invoice_Value',
                                    label: {
                                        text: 'Invoice Value'
                                    },
                                }, {
                                    dataField: 'OrderPrice_UserInput',
                                    label: {
                                        text: 'Order Price'
                                    },
                                },
                                //{
                                //    dataField: 'OrderPrice',
                                //    label: {
                                //        text: 'Order Price(USD)'
                                //    },
                                //},

                                {
                                    dataField: 'PRNumber',
                                    label: {
                                        text: 'PR Number'
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],

                                },


                                {
                                    dataField: 'Currency',
                                    label: {
                                        text: 'Currency'
                                    },
                                },

                                {
                                    dataField: 'RFQNumber',
                                    label: {
                                        text: 'RFQ Number'
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],

                                },

                                {
                                    dataField: 'OrderID',
                                    label: {
                                        text: 'PO Number'
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],

                                },


                                {
                                    dataField: 'SRSubmitted',
                                    label: {
                                        text: 'SR/PR Submitted Date'
                                    },
                                    editorType: 'dxDateBox',
                                    editorOptions: {
                                        //displayFormat: 'datetim',
                                        format: {
                                            type: "shortDate",
                                        },
                                        disabled: true,
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],
                                },


                                {
                                    dataField: 'OrderDate',
                                    label: {
                                        text: 'PO Release Date'
                                    },
                                    editorType: 'dxDateBox',
                                    editorOptions: {

                                        format: {
                                            type: "shortDate",
                                        },
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],
                                },

                                {
                                    dataField: 'SRAwardedDate',
                                    label: {
                                        text: 'SR Awarded Date'
                                    },
                                    //editorType: 'dxDateBox',
                                    //editorOptions: {
                                    //    format: {
                                    //        type: "shortDate",
                                    //    },
                                    //},

                                    //validationRules: [{
                                    //    type: "required",

                                    //}],
                                },

                                {
                                    dataField: 'OrderedQuantity',
                                    label: {
                                        text: 'Ordered Quantity'
                                    },
                                },


                                {
                                    dataField: 'SRApprovalDays',
                                    label: {
                                        text: 'SR Approval Days'
                                    },
                                    //editorType: 'dxDateBox',
                                    editorOptions: {
                                        //displayFormat: 'datetim',
                                        disabled: true
                                    },
                                    //validationRules: [{
                                    //    type: "required",

                                    //}],
                                },


                                //{
                                //    dataField: 'OrderDate',
                                //    label: {
                                //        text: 'Order Date'
                                //    },
                                //},

                                //{
                                //    dataField: 'OrderID',
                                //    label: {
                                //        text: 'OrderID'
                                //    },
                                //},


                                {
                                    dataField: 'PORemarks',
                                    label: {
                                        text: 'Justification'
                                    },
                                },

                                {
                                    dataField: 'TentativeDeliveryDate',
                                    label: {
                                        text: 'Tentative Delivery Date'
                                    },
                                },
                                {
                                    dataField: 'ActualDeliveryDate',
                                    label: {
                                        text: 'Actual Delivery Date'
                                    },
                                },
                                {
                                    dataField: 'Description',
                                    label: {
                                        text: 'Order Status Desciption'
                                    },
                                },
                                {
                                    dataField: "LinkedRequests",
                                    label: {
                                        text: 'Linked Requests'
                                    },
                                    allowEditing: false,
                                    disabled: true,

                                },

                            ]
                        }

                    ]
                }





            },
            onContentReady: function (e) {
                ////debugger;
                e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
            },
            onCellPrepared: function (e) {
                if (e.rowType === "data" && e.column.command === "expand") {
                    debugger;
                    if (ChildList.find(x => x.LinkedRequestID == e.row.data.RequestID) == undefined) {
                        e.cellElement.removeClass("dx-datagrid-expand");
                        e.cellElement.empty();
                    }
                }
                if (e.rowType === "header" || e.rowType === "filter") {
                    e.cellElement.addClass("columnHeaderCSS");
                    e.cellElement.find("input").addClass("columnHeaderCSS");
                }

                if (e.rowType === "data" && e.column.command === 'select') {

                    RequestSource = e.row.data.RequestSource; // get request source to check whether it is HOE or RFO

                    var submitAllowed = (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && RequestSource == 'RFO' && !e.row.data.VKMSPOC_Approval)
                        || (!flag && !e.row.data.ApprovedSH && !admin);

                    if (!submitAllowed) {
                        //  debugger;
                        var instance = e.cellElement.find('.dx-select-checkbox').dxCheckBox("instance");
                        instance.option("visible", false);
                        e.cellElement.off();
                    }

                }


            },
            onSelectionChanged(e) {
                debugger;

                var grid = e.component;

                var disabledKeys = e.selectedRowsData.filter(x => (x.RequestSource == 'RFO' && x.VKMSPOC_Approval) || ((x.RequestSource == 'HOE' || x.RequestSource == '') && x.ApprovedSH)).map(x => x.RequestID);
                //disabledkeys are those requestids which has already been submitted
                if (disabledKeys.length > 0) {
                    debugger;
                    if (justDeselected) {
                        justDeselected = false;
                        grid.deselectAll();
                    }
                    else {
                        justDeselected = true;
                        grid.deselectRows(disabledKeys);
                    }

                }


                //var editingAllowed = (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && RequestSource == 'RFO' && !e.row.data.VKMSPOC_Approval) || (flag && e.row.data.RequestToOrder && e.row.data.VKMSPOC_Approval == 0 && e.row.data.RequestSource != 'RFO') || (flag && e.row.data.RequestToOrder && e.row.data.VKMSPOC_Approval == 1 && e.row.data.RequestSource == 'RFO') || (!flag && !e.row.data.ApprovedSH && !admin);

                //already submitted - no edit - planning / ordering
                // requestsource == rfo & vkmspoc approval = 1
                // requestsource == "" / hoe && approvedsh = 1-

                //  if (e.currentSelectedRowKeys.length == e.selectedRowKeys.length) {
                //var disabledKeys = e.currentSelectedRowKeys.filter(i => (!i.editingAllowed));
                //    if (e.selectedRowKeys.length == 0) {//this means that user has clicked to select all ; when currentSelectedRowKeys and selectedRowkeys have different value, this means that user has already selected all and disabled rows were deselected, and now user wants to deselect all
                //    var disabledKeys = e.selectedRowsData.filter(x => x.RequestID > 6046).map(x => x.RequestID);
                //    if (disabledKeys.length > 0)
                //        e.component.deselectRows(disabledKeys);
                //    debugger;
                //}

            },
            //onKeyUp: function (e) {
            //    //debugger;
            //},
            //onKeyDown: function (e) {
            //    //debugger;
            //},
            //focusedRowEnabled: true,
            allowColumnReordering: true,
            allowColumnResizing: true,
            keyExpr: "RequestID",
            columnResizingMode: "widget",
            columnMinWidth: 50,
            selection: {
                mode: 'multiple',
                showCheckBoxesMode: 'always',
                applyFilter: "auto",
                allowSelectAll: false
            },
            grouping: {
                autoExpandAll: true,
            },
            groupPanel: {
                visible: true,
            },
            //stateStoring: {
            //    enabled: true,
            //    type: "localStorage",
            //    storageKey: "RequestID"
            //},

            columnChooser: {
                enabled: true
            },
            //filterRow: {
            //    visible: true,
            //    /*showAllText: "(All)",*/
            //   // resetOperationText : "RESET1"

            //},
            headerFilter: {
                visible: true,
                applyFilter: "auto",
                allowSearch: true

            },
            //selection: {
            //    applyFilter: "auto"
            //},
            showBorders: true,
            //scrolling: {
            //    mode: 'infinite'
            //},
            //onInitialized: function (e) {
            //    dataGrid = e.component;

            // },
            //    selection: {
            //        mode: "multiple",
            //        deferred: true
            //},

            searchPanel: {
                visible: true,
                width: 240,
                placeholder: "Search..."
            },
            loadPanel: {
                enabled: true
            },
            //onOptionChanged: function (e) {
            //    //debugger;
            //    if (e.name === "Reviewed_Quantity") {
            //        // handle the property change here
            //        //debugger;
            //    }
            //},
            //onInput: function (e) {
            //    //debugger;
            //},
            onEditorPreparing: function (e) {

                var component = e.component,
                    rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex
                if (e.parentType === "dataRow" && e.dataField === "isProjected") {

                    // e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0) //true -f01/ f03 or apprcost <= 0 or before vkm spoc approval
                    if (e.row.data.ApprovedSH == undefined)
                        e.row.data.ApprovedSH = false;
                    if (e.row.isEditing == undefined)
                        e.row.isEditing = false;
                    e.editorElement.dxCheckBox({
                        value: e.value,
                        disabled: (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || e.row.data.ApprovedSH == false || !flag || !e.row.isEditing),
                        //readOnly: e.readOnly,
                        onValueChanged: function (ea) {

                            var isProjected;
                            if (ea.value) {
                                e.component.option('value', 1);
                                isProjected = 1;
                                //e.setValue(1);
                            }
                            else {
                                e.component.option('value', 0);
                                isProjected = 0;
                            }
                            $.ajax({

                                type: "post",
                                url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                                data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: isProjected, useryear: filtered_yr, OrdStatus: 0 },
                                datatype: "json",
                                traditional: true,
                                success: function (data) {
                                    if (data.success) {

                                        //var objdata = data.data;
                                        //$("#RequestTable_VKMSPOC").dxDataGrid({
                                        //    dataSource: objdata
                                        //});
                                        window.setTimeout(function () {
                                            component.cellValue(rowIndex, "isProjected", data.isproj);
                                            component.cellValue(rowIndex, "Q1", data.q1);
                                            component.cellValue(rowIndex, "Q2", data.q2);
                                            component.cellValue(rowIndex, "Q3", data.q3);
                                            component.cellValue(rowIndex, "Q4", data.q4);
                                            component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                            component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                            component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                        }, 1000);
                                        // OnSuccess_GetData();
                                    }
                                    else {
                                        $.notify("Unable to update; Please try later !", {
                                            globalPosition: "top center",
                                            className: "warn"
                                        })
                                    }


                                }
                            })
                        }
                    });
                    e.cancel = true;
                }
                if (e.parentType === "dataRow" && e.dataField === "Q1") {

                    e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 1 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                }
                if (e.parentType === "dataRow" && e.dataField === "Q2") {
                    e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 2 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                }
                if (e.parentType === "dataRow" && e.dataField === "Q3") {

                    e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 3 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                }
                if (e.parentType === "dataRow" && e.dataField === "Q4") {
                    e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 4 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                }
                if (e.parentType === "dataRow" && e.dataField === "Group") {
                    e.editorOptions.disabled = (typeof e.row.data.DEPT !== "number") && !e.row.isNewRow;
                    if (e.editorOptions.disabled)
                        e.editorOptions.placeholder = 'Select Dept first';

                    if (!e.editorOptions.disabled)
                        e.editorOptions.placeholder = 'Select Group';

                    e.editorOptions.disabled = !e.row.isNewRow;
                }
                if (e.dataField === "OEM" && e.parentType === "dataRow") {
                    e.editorOptions.disabled = !e.row.isNewRow;
                }


                if (e.dataField === "BU") {
                    ////debugger;
                    if (e.parentType == "dataRow") {
                        ////debugger;
                        e.editorOptions.disabled = !e.row.isNewRow;
                    }

                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);

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
                            data: { 'DEPT': component.cellValue(rowIndex, "DEPT"), BU: e.value },
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
                        //    url: "/BudgetingRequest/GetReviewer_VKMSPOC",
                        //    data: { BU: e.value },
                        //    datatype: "json",
                        //    traditional: true,
                        //    success: function (data) {

                        //        reviewer_2 = data;

                        //    }
                        //})
                        // Emulating a web service call
                        window.setTimeout(function () {
                            component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
                        }, 1000);
                    }
                }

                if (e.dataField === "DEPT") {
                    if (e.parentType == "dataRow") {
                        ////debugger;
                        e.editorOptions.disabled = !e.row.isNewRow;
                    }
                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        var bu = component.cellValue(rowIndex, "BU");
                        if (is_TwoWPselected && (e.value > 59 && e.value < 104)) {
                            BU_forItemFilter = 4;
                            window.setTimeout(function () {
                                component.cellValue(rowIndex, "Reviewer_2", "Sheeba Rani R");
                            }, 1000);
                        }

                        $.ajax({

                            type: "post",
                            url: "/BudgetingRequest/GetReviewer_HoE",
                            data: { DEPT: e.value },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {

                                reviewer_1 = data.data;

                            }
                        })

                        $.ajax({

                            type: "post",
                            url: "/BudgetingRequest/GetReviewer_VKMSPOC",
                            data: { 'DEPT': e.value, BU: component.cellValue(rowIndex, "BU") },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                //debugger;
                                reviewer_2 = data.data;
                                if (e.value == 1 && is_XCselected == true) {
                                    is_TwoWPselected = true;
                                    BU_forItemFilter = 4;
                                    reviewer_2 = "Sheeba Rani R";

                                }

                            }
                        })

                        // Emulating a web service call
                        window.setTimeout(function () {
                            component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
                            component.cellValue(rowIndex, "Reviewer_1", reviewer_1);
                        }, 1000);
                    }
                }



                if (e.dataField === "Item_Name") {

                    if (e.parentType == "dataRow") {
                        ////debugger;
                        e.editorOptions.disabled = !e.row.isNewRow;
                    }
                    //if (e.parentType == "filterRow") {
                    //    //debugger;
                    //    e.editorOptions.dataSource = Item_FilterRow;
                    //    e.editorOptions.showClearButton = true;
                    //    //e.editorName = "dxTextBox";

                    //}
                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        //$.ajax({
                        //    type: "post",
                        //    url: "/BudgetingRequest/GetUnitPrice",
                        //    data: { ItemName: e.value },
                        //    datatype: "json",
                        //    ajax: false,
                        //    traditional: true,
                        //    success: function (data) {
                        //        ////debugger;
                        //        if (data > 0)
                        //            unitprice = data;


                        //        var RevQ_sel = component.cellValue(rowIndex, "Reviewed_Quantity");
                        //        if (component.cellValue(rowIndex, "Reviewed_Quantity") != undefined && component.cellValue(rowIndex, "Reviewed_Quantity") != null) {
                        //            ////debugger;
                        //            $.ajax({

                        //                type: "post",
                        //                url: "/BudgetingVKM/GetRevCost",
                        //                data: { Reviewed_Quantity: component.cellValue(rowIndex, "Reviewed_Quantity"), Unit_Price: unitprice },
                        //                datatype: "json",
                        //                traditional: true,
                        //                success: function (data) {
                        //                    ////debugger;
                        //                    //if (data.msg) {
                        //                    //    CostEUR = "";
                        //                    //    window.setTimeout(function () {
                        //                    //       ////debugger;
                        //                    //        component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
                        //                    //    }, 1000);

                        //                    //    //$.notify(data.msg, {
                        //                    //    //    globalPosition: "top center",
                        //                    //    //    className: "success"
                        //                    //    //})
                        //                    //}
                        //                    //else {


                        //                    RevCost = data.RevCost;
                        //                    window.setTimeout(function () {
                        //                        ////debugger;
                        //                        component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                        //                    }, 1000);
                        //                    //}

                        //                }
                        //            })
                        //        }
                        //    }
                        //});

                        //$.ajax({

                        //    type: "post",
                        //    url: "/BudgetingRequest/GetCategory",
                        //    data: { ItemName: e.value },
                        //    datatype: "json",
                        //    traditional: true,
                        //    success: function (data) {

                        //        category = data;

                        //    }
                        //})

                        //$.ajax({

                        //    type: "post",
                        //    url: "/BudgetingRequest/GetCostElement",
                        //    data: { ItemName: e.value },
                        //    datatype: "json",
                        //    traditional: true,
                        //    success: function (data) {
                        //        ////debugger;
                        //        costelement = data;

                        //    }
                        //})

                        //$.ajax({

                        //    type: "post",
                        //    url: "/BudgetingRequest/GetBudgetCode",
                        //    data: { ItemName: e.value },
                        //    datatype: "json",
                        //    traditional: true,
                        //    success: function (data) {
                        //        ////debugger;
                        //        BudgetCode = data;

                        //    }
                        //})

                        ////// Emulating a web service call

                        //window.setTimeout(function () {

                        //    component.cellValue(rowIndex, "Unit_Price", unitprice);
                        //    component.cellValue(rowIndex, "Category", category);
                        //    component.cellValue(rowIndex, "Cost_Element", costelement);
                        //    component.cellValue(rowIndex, "BudgetCode", BudgetCode);

                        //},
                        //    1000);

                    }


                    if (is_newitem == true) {
                        $.ajax({

                            type: "POST",
                            url: "/BudgetingVKM/GetUnusedAvailability",
                            data: { CostElement: component.cellValue(rowIndex, "Cost_Element"), BU: component.cellValue(rowIndex, "BU"), ItemName: component.cellValue(rowIndex, "Item_Name"), Dept: component.cellValue(rowIndex, "DEPT"), VKMYear: filtered_yr },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                //debugger;
                                AvailUnusedAmt = data.AvailableUnUsedAmt;

                            }
                        })
                    }

                }

                if (e.parentType === "dataRow" && e.dataField === "Fund") {

                    e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund)); //non-editbale if f02 ; editable if f01 or f03

                }

                if (e.dataField === "Reviewed_Quantity") {

                    if (e.parentType == "dataRow") {
                        ////debugger;
                        if (e.row.data.ApprovedSH == undefined)
                            e.row.data.ApprovedSH = false;
                        e.editorOptions.disabled = e.row.data.ApprovedSH && flag;


                    }

                    e.editorOptions.valueChangeEvent = "keyup";

                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data

                    e.editorOptions.onValueChanged = function (e) {

                        onValueChanged.call(this, e);

                        var UnitPr_sel = component.cellValue(rowIndex, "Unit_Price");

                        if (component.cellValue(rowIndex, "Unit_Price") != undefined && component.cellValue(rowIndex, "Unit_Price") != null) {

                            $.ajax({

                                type: "post",
                                url: "/BudgetingVKM/GetRevCost",
                                data: { Reviewed_Quantity: e.value, Unit_Price: component.cellValue(rowIndex, "Unit_Price") },
                                datatype: "json",
                                traditional: true,
                                success: function (data) {
                                    ////debugger;
                                    //if (data.msg) {
                                    //    CostEUR = "";
                                    //    window.setTimeout(function () {
                                    //       ////debugger;
                                    //        component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
                                    //    }, 1000);

                                    //    //$.notify(data.msg, {
                                    //    //    globalPosition: "top center",
                                    //    //    className: "success"
                                    //    //})
                                    //}
                                    //else {
                                    //debugger;
                                    RevCost = data.RevCost;
                                    //alert(RevCost);
                                    //alert(flag);
                                    //alert(RequestToOrder);
                                    //alert(RequestSource);
                                    //alert(VKMSPOC_Approval);


                                    //alert('3');
                                    ////debugger;
                                    window.setTimeout(function () {
                                        //debugger;
                                        component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                                    }, 1000);

                                }
                            });

                        }
                        //////// CTG amount validation to check the CTG budget allocated for the requestor
                        //if (flag == false && RequestToOrder == true && RequestSource == 'RFO' && VKMSPOC_Approval == false) {
                        //        //debugger;
                        //        //alert('1');
                        //        $.ajax({
                        //            type: "POST",
                        //            url: "/BudgetingVKM/ValidateCTGAmount",
                        //            data: { NTID: RequestorNTID, ApprovedAmount: (UnitPr_sel * e.value), Dept: component.cellValue(rowIndex, "DEPT")  },
                        //            datatype: "json",
                        //            traditional: true,
                        //            success: function (data) {
                        //                //alert('initiating success function');
                        //                if (data.success) {
                        //                    //debugger;
                        //                    //alert('success');
                        //                    if (data.isExceeded == true) {
                        //                        //alert('exceeded');
                        //                        $.notify("Can't Update. Budget Exceeded!", {
                        //                            globalPosition: "top center",
                        //                            className: "warn"
                        //                        });
                        //                        component.cellValue(rowIndex, "Reviewed_Quantity", e.previousValue);
                        //                        window.setTimeout(function () {
                        //                            //debugger;
                        //                            component.cellValue(rowIndex, "Reviewed_Cost", (e.previousValue * UnitPr_sel));
                        //                        }, 1000);
                        //                        return false;
                        //                    }
                        //                    else {
                        //                        //alert('not exceeded');
                        //                        window.setTimeout(function () {
                        //                            //debugger;
                        //                            component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                        //                        }, 1000);
                        //                    }
                        //                }
                        //                else {
                        //                    //debugger;
                        //                    //alert('error');
                        //                    $.notify("Can't Update.!", {
                        //                        globalPosition: "top center",
                        //                        className: "warn"
                        //                    });
                        //                    //}
                        //                }
                        //            },
                        //            error: function (data) {
                        //                //alert(data);
                        //                $.notify("Can't Update.!", {
                        //                    globalPosition: "top center",
                        //                    className: "warn"
                        //                });
                        //            }
                        //        });
                        //    }

                        ////window.setTimeout(function () {
                        ////    //debugger;
                        ////    component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                        ////}, 1000);
                    }
                }
                if (e.dataField === "Currency") {
                    e.editorOptions.valueChangeEvent = "keyup";
                    var onValueChanged = e.editorOptions.onValueChanged;
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({

                            type: "post",
                            url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                            data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: 2, useryear: filtered_yr, isChange_amount: 0, ord_price: component.cellValue(rowIndex, "OrderPrice_UserInput"), currency: e.value, OrdStatus: 0 },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                if (data.success) {
                                    //debugger;
                                    //var getdata = data.data;
                                    //$("#RequestTable_VKMSPOC").dxDataGrid({
                                    //    dataSource: getdata
                                    //});
                                    window.setTimeout(function () {
                                        component.cellValue(rowIndex, "isProjected", data.isproj);
                                        component.cellValue(rowIndex, "Q1", data.q1);
                                        component.cellValue(rowIndex, "Q2", data.q2);
                                        component.cellValue(rowIndex, "Q3", data.q3);
                                        component.cellValue(rowIndex, "Q4", data.q4);
                                        component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                        component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                        component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                        component.cellValue(rowIndex, "Currency", data.currency);
                                    }, 1000);
                                }
                                else {
                                    //debugger;
                                    $.notify("Unable to update; Please try later !", {
                                        globalPosition: "top center",
                                        className: "warn"
                                    })
                                }
                            }
                        })
                    }
                }

                if (e.dataField === "OrderPrice_UserInput") {
                    e.editorOptions.valueChangeEvent = "keyup";
                    var onValueChanged = e.editorOptions.onValueChanged;
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        //debugger;

                        if (flag == true && is_newitem == true) {
                            if (e.value > AvailUnusedAmt) {
                                $.notify("Can't Update. Order Price should not exceed the available unused amount!", {
                                    globalPosition: "top center",
                                    className: "warn"
                                });
                                component.cellValue(rowIndex, "OrderPrice_UserInput", 0);
                                return false;
                            }
                        }




                        //debugger;
                        $.ajax({

                            type: "post",
                            url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                            data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: 2, useryear: filtered_yr, isChange_amount: 0, ord_price: e.value, currency: component.cellValue(rowIndex, "Currency"), OrdStatus: 0 },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                if (data.success) {
                                    //debugger;
                                    //var getdata = data.data;
                                    //$("#RequestTable_VKMSPOC").dxDataGrid({
                                    //    dataSource: getdata
                                    //});
                                    window.setTimeout(function () {
                                        component.cellValue(rowIndex, "isProjected", data.isproj);
                                        component.cellValue(rowIndex, "Q1", data.q1);
                                        component.cellValue(rowIndex, "Q2", data.q2);
                                        component.cellValue(rowIndex, "Q3", data.q3);
                                        component.cellValue(rowIndex, "Q4", data.q4);
                                        component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                        component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                        component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                        component.cellValue(rowIndex, "Currency", data.currency);
                                    }, 1000);
                                }
                                else {
                                    //debugger;
                                    $.notify("Unable to update; Please try later !", {
                                        globalPosition: "top center",
                                        className: "warn"
                                    })
                                }
                            }
                        })
                    }
                }

                if (e.dataField === "Projected_Amount") {
                    //debugger;
                    if (e.parentType === "dataRow")
                        e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or before vkm spoc approval

                    e.editorOptions.valueChangeEvent = "keyup";
                    var onValueChanged = e.editorOptions.onValueChanged;
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        $.ajax({

                            type: "post",
                            url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                            data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: 2, useryear: filtered_yr, isChange_amount: 1, proj_price: e.value, OrdStatus: 0 },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                //debugger;
                                if (data.success) {
                                    //debugger;
                                    //var getdata = data.data;
                                    //$("#RequestTable_VKMSPOC").dxDataGrid({
                                    //    dataSource: getdata
                                    //});
                                    window.setTimeout(function () {
                                        component.cellValue(rowIndex, "isProjected", data.isproj);
                                        component.cellValue(rowIndex, "Q1", data.q1);
                                        component.cellValue(rowIndex, "Q2", data.q2);
                                        component.cellValue(rowIndex, "Q3", data.q3);
                                        component.cellValue(rowIndex, "Q4", data.q4);
                                        component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                        component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                        component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                    }, 1000);
                                }
                                else {
                                    $.notify("Unable to update; Please try later !", {
                                        globalPosition: "top center",
                                        className: "warn"
                                    })
                                }


                            }
                        })
                    }
                }

                if (e.dataField === "Unused_Amount") {
                    if (e.parentType === "dataRow")
                        e.editorOptions.disabled = (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || e.row.data.ApprovedSH == false) //true -f01/ f03 or apprcost <= 0 or before vkm spoc approval

                    e.editorOptions.valueChangeEvent = "keyup";
                    var onValueChanged = e.editorOptions.onValueChanged;
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        $.ajax({

                            type: "post",
                            url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                            data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: 2, useryear: filtered_yr, isChange_amount: 2, unused_price: e.value, OrdStatus: 0 },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                //debugger;
                                if (data.success) {
                                    //debugger;
                                    //var getdata = data.data;
                                    //$("#RequestTable_VKMSPOC").dxDataGrid({
                                    //    dataSource: getdata
                                    //});
                                    window.setTimeout(function () {
                                        component.cellValue(rowIndex, "isProjected", data.isproj);
                                        component.cellValue(rowIndex, "Q1", data.q1);
                                        component.cellValue(rowIndex, "Q2", data.q2);
                                        component.cellValue(rowIndex, "Q3", data.q3);
                                        component.cellValue(rowIndex, "Q4", data.q4);
                                        component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                        component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                        component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                    }, 1000);
                                }
                                else {
                                    $.notify("Unable to update; Please try later !", {
                                        globalPosition: "top center",
                                        className: "warn"
                                    })
                                }


                            }
                        })
                    }
                }

                /////// Order Status Descrition field to be chosen or created
                if (e.dataField === 'Description') {
                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                    e.editorName = "dxAutocomplete";
                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                    }
                }

                if (e.dataField === "OrderStatus") {
                    e.editorOptions.valueChangeEvent = "keyup";
                    var onValueChanged = e.editorOptions.onValueChanged;
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        var Orderstatus_projectedUncheck = [2, 4, 5, 6, 7]; //ordered,dispatched,delivered,cancelled,closed,
                        var is_projected;
                        if (Orderstatus_projectedUncheck.indexOf(e.value) !== -1) { //projected has to be unchecked
                            is_projected = 0;
                        }
                        else {//projected has to be checked
                            is_projected = 1;
                        }
                        //debugger;
                        $.ajax({

                            type: "post",
                            url: "/BudgetingVKM/OnValueChange_inProjectedProcess",
                            data: { RequestID: component.cellValue(rowIndex, "RequestID"), isProjected: is_projected, useryear: filtered_yr, ord_price: component.cellValue(rowIndex, "OrderPrice"), currency: component.cellValue(rowIndex, "Currency"), OrdStatus: e.value },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                //debugger;
                                if (data.success) {
                                    //var getdata = data.data;
                                    //$("#RequestTable_VKMSPOC").dxDataGrid({
                                    //    dataSource: getdata
                                    //});
                                    window.setTimeout(function () {
                                        component.cellValue(rowIndex, "isProjected", data.isproj);
                                        component.cellValue(rowIndex, "Q1", data.q1);
                                        component.cellValue(rowIndex, "Q2", data.q2);
                                        component.cellValue(rowIndex, "Q3", data.q3);
                                        component.cellValue(rowIndex, "Q4", data.q4);
                                        component.cellValue(rowIndex, "Projected_Amount", data.proj_amt);
                                        component.cellValue(rowIndex, "Unused_Amount", data.unused_amt);
                                        component.cellValue(rowIndex, "OrderPrice", data.order_amt);
                                    }, 1000);
                                }
                                else {
                                    $.notify("Unable to update; Please try later !", {
                                        globalPosition: "top center",
                                        className: "warn"
                                    })
                                }


                            }
                        })
                    }
                }

                //if (e.dataField === "SRAwardedDate") {

                //    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                //    e.editorOptions.onValueChanged = function (arg) {
                //        //debugger;
                //        onValueChanged.call(this, e);
                //        //debugger;
                //        if (arg.value != undefined && component.cellValue(rowIndex, "SRSubmitted") != undefined) {
                //            srawardeddate = Date.parse(arg.value);
                //            srsubmitteddate = Date.parse(component.cellValue(rowIndex, "SRSubmitted"));

                //            var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                //            // To calculate the no. of days between two dates
                //            var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                //            var SRApprovalDays = Difference;
                //            component.cellValue(rowIndex, "SRApprovalDays", SRApprovalDays);
                //        }

                //    }
                //}

            },

            columns: [
                {

                    type: "buttons",
                    width: 100,
                    alignment: "left",
                    fixed: true,
                    fixedPosition: "left",
                    buttons: [
                        "edit", "delete",
                        {
                            hint: "Submit Item",
                            icon: "check",
                            visible: function (e) {
                                //debugger;
                                RequestSource = e.row.data.RequestSource;
                                RequestToOrder = e.row.data.RequestToOrder;
                                VKMSPOC_Approval = e.row.data.VKMSPOC_Approval;
                                Reviewed_Quantity = e.row.data.Reviewed_Quantity;
                                RequestorNTID = e.row.data.RequestorNTID;

                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return !flag && !e.row.isEditing && ((!flag && !e.row.data.ApprovedSH) || (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && e.row.data.RequestSource == "RFO" && !e.row.data.VKMSPOC_Approval && e.row.data.isCancelled != '2')) && !admin

                                // labteam - no option to submit
                                // vkmspoc approved sh=false - item not reviewed, so submit option available
                                // vkmspoc approved sh=true  - item reviewed, so submit option not needed


                            },
                            onClick: function (e) {
                                SHApprove(e.row.data.RequestID, filtered_yr);
                                e.component.refresh(true);
                                e.event.preventDefault();
                            }
                        },
                        {
                            hint: "Send Back Item",
                            icon: "fa fa-send",
                            visible: function (e) {

                                RequestSource = e.row.data.RequestSource;
                                RequestToOrder = e.row.data.RequestToOrder;
                                VKMSPOC_Approval = e.row.data.VKMSPOC_Approval;
                                Reviewed_Quantity = e.row.data.Reviewed_Quantity;
                                RequestorNTID = e.row.data.RequestorNTID;

                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                //return !flag && !e.row.isEditing && (!flag && !e.row.data.ApprovedSH) && !admin && (e.row.data.RequestSource != "RFO") //admin no option to update
                                return !flag && !e.row.isEditing && ((!flag && !e.row.data.ApprovedSH) || (!flag && e.row.data.ApprovedSH && e.row.data.RequestToOrder && e.row.data.RequestSource == "RFO" && !e.row.data.VKMSPOC_Approval && e.row.data.isCancelled != "2" && e.row.data.OrderStatus != '6')) && !admin
                            },
                            onClick: function (e) {
                                SendBack(e.row.data.RequestID, filtered_yr);
                                e.component.refresh(true);
                                e.event.preventDefault();
                            }

                        }, {
                            hint: 'File attachment',
                            icon: 'file',
                            visible: function (e) {
                                //debugger;
                                return (popupedit && e.row.data.LinkedRequestID == "");
                            },
                            onClick: function (rowItem) {
                                //debugger;
                                var fileListDisplay, noFileTmpDisplay;
                                var $ul = "";
                                noFileTmpDisplay = "none"
                                if (rowItem.row.data.Upload_File_Name != null) {
                                    if (rowItem.row.data.Upload_File_Name.toString().length == 0) {
                                        fileListDisplay = "none"
                                        noFileTmpDisplay = "block"
                                    } else {
                                        fileListDisplay = "block"
                                        noFileTmpDisplay = "none"
                                    }

                                    const myFileArray = rowItem.row.data.Upload_File_Name.split("||");

                                    if (myFileArray != "") {
                                        $ul = $('<ol style="font-size:12px;min-height:50px;max-height:80px;overflow:auto;">', { class: "mylist" }).append(
                                            myFileArray.map(country =>
                                                $("<li>").append($("<a>").text(country))
                                            )
                                        );
                                    } else {
                                        noFileTmpDisplay = "block"
                                    }
                                } else {
                                    noFileTmpDisplay = "block"
                                }

                                var linkedrequests = "NA";
                                if (rowItem.row.data.LinkedRequests != null) {
                                    linkedrequests = rowItem.row.data.LinkedRequests;
                                }

                                popupContentTemplate = function () {
                                    return $('<div style="font-size:12px;">').append(
                                        $(`<div style="display:` + fileListDisplay + `"> File Attached</div>`),
                                        $ul,
                                        $(`<div style=""> Linked Requests: ` + linkedrequests + ` </div >`),
                                        $(`<div style="display:` + noFileTmpDisplay + `;color:red;" id='fileList'>There is no file attached.</div>`),
                                        $(`<div id='FileUploadBtn'></div>`))
                                };
                                const popup = $('#widget').dxPopup({
                                    contentTemplate: popupContentTemplate,
                                    width: '30vw',
                                    height: '50vh',
                                    container: '.dx-viewport',
                                    showTitle: true,
                                    title: 'File Attachment',
                                    visible: true,
                                    dragEnabled: true,
                                    hideOnOutsideClick: true,
                                    showCloseButton: true,
                                    onHiding: function (e) {
                                        //console.log("onHiding")
                                    },
                                    onContentReady: function (e) {
                                        let fileUploader = $(FileUploadBtn).dxFileUploader({
                                            name: "file",
                                            multiple: true,
                                            accept: "*",
                                            uploadMode: "useForm",
                                            //uploadUrl: `${backendURL}AsyncFileUpload`,
                                            onValueChanged: function (e) {
                                                //debugger;

                                                //file = e.value;
                                                ////debugger;

                                                var uploadedfilename = document.querySelector('.dx-fileuploader-files-container');



                                                for (var i = 0; i < e.value.length; i++) {





                                                    //if (e.value[i].name.indexOf("Quot") != -1 || e.value[i].name.indexOf("Single Source Justification") != -1) {

                                                    file = e.value;


                                                    if (uploadedfilename != undefined) {

                                                        uploadedfilename.style.visibility = 'visible';

                                                        uploadedfilename.style.height = "100px";
                                                        uploadedfilename.style.overflow = "auto";
                                                        uploadedfilename.style.paddingTop = "0px";

                                                    }

                                                    //}

                                                    //else {

                                                    //    file = null;

                                                    //    alert('Invalid file');

                                                    //    if (uploadedfilename != undefined) {

                                                    //        uploadedfilename.style.visibility = 'hidden';

                                                    //        uploadedfilename.style.height = "0px";

                                                    //        uploadedfilename.style.paddingTop = "0px";

                                                    //    }

                                                    //}

                                                }


                                            },
                                            onUploaded: function (e) {
                                                //debugger;
                                                cellInfo.setValue("images/employees/" + e.request.responseText);
                                                retryButton.option("visible", false);
                                            },
                                            onUploadError: function (e) {
                                                //debugger;
                                                let xhttp = e.request;
                                                if (xhttp.status === 400) {
                                                    e.message = e.error.responseText;
                                                }
                                                if (xhttp.readyState === 4 && xhttp.status === 0) {
                                                    e.message = "Connection refused";
                                                }
                                                //retryButton.option("visible", true);
                                            }
                                        }).dxFileUploader("instance");
                                    },
                                    position: {
                                        at: 'center',
                                        my: 'center',
                                        collision: 'fit',
                                    },
                                    toolbarItems: [{
                                        widget: 'dxButton',
                                        toolbar: 'bottom',
                                        location: 'after',
                                        options: {
                                            text: 'Save',
                                            onClick() {
                                                //debugger;
                                                document.getElementById("loadpanel_uploadFiles").style.display = "block";
                                                if (file != undefined) {
                                                    let fileNameString = "";
                                                    let previousAttachedFileArray = rowItem.row.data.Upload_File_Name.toString().split("||");
                                                    if (file.length > 0) {
                                                        if (previousAttachedFileArray != "") {
                                                            fileNameString = rowItem.row.data.Upload_File_Name;
                                                            for (var i = 0; i < file.length; i++) {
                                                                if (!previousAttachedFileArray.includes(file[i].name)) {
                                                                    fileNameString = fileNameString + '||' + file[i].name;
                                                                }
                                                            }
                                                        } else {
                                                            for (var i = 0; i < file.length; i++) {
                                                                if (fileNameString == "") {
                                                                    fileNameString = file[i].name;
                                                                } else {
                                                                    fileNameString = fileNameString + '||' + file[i].name;
                                                                }

                                                            }
                                                        }
                                                    }
                                                    rowItem.row.data.Upload_File_Name = fileNameString;
                                                    Selected = [];
                                                    Selected.push(rowItem.row.data);
                                                    Update(Selected, filtered_yr, 'FileUpload');
                                                    popup.hide();
                                                } else {
                                                    alert("Please upload the any file.")
                                                }
                                            },
                                        },
                                    },
                                    {
                                        widget: 'dxButton',
                                        toolbar: 'bottom',
                                        location: 'after',
                                        options: {
                                            text: 'Cancel',
                                            onClick() {
                                                popup.hide();
                                            },
                                        },
                                    }
                                    ],
                                }).dxPopup('instance');

                            }
                        }]
                },
                {
                    caption: "Review Request Details",
                    alignment: "center",
                    columns: [
                        //{

                        //    dataField: "VKM_Year",
                        //    validationRules: [{ type: "required" }],
                        //    width: 100,
                        //    lookup: {
                        //        dataSource: yr_list,
                        //        valueExpr: "Year",
                        //        displayExpr: "Year"
                        //    },
                        //    allowEditing: false

                        //},
                        {

                            dataField: "RequestID",
                            allowEditing: false,
                            visible: true
                        },
                        {

                            dataField: "isProjected",

                            width: 130,
                            dataType: "boolean",
                            //allowEditing: flag,
                            visible: false,
                            //disabled: function (e) {
                            //    //debugger;
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0) //true -f01/ f03 or apprcost <= 0 or before vkm spoc approval

                            //}
                        },
                        {

                            dataField: "Q1",

                            width: 70,
                            dataType: "boolean",
                            allowEditing: flag,
                            visible: false,
                            //disabled: function (e) {
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 1) //true -f01/ f03 or apprcost <= 0 or or before vkm spoc approval or when that specific quarter is completed

                            //}
                        },
                        {

                            dataField: "Q2",

                            width: 70,
                            dataType: "boolean",
                            allowEditing: flag,
                            visible: false,
                            //disabled: function (e) {
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 2) //true -f01/ f03 or apprcost <= 0

                            //}
                        },
                        {

                            dataField: "Q3",

                            width: 70,
                            dataType: "boolean",
                            allowEditing: flag,
                            visible: false,
                            //disabled: function (e) {
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 3) //true -f01/ f03 or apprcost <= 0

                            //}
                        },
                        {

                            dataField: "Q4",

                            width: 70,
                            dataType: "boolean",
                            allowEditing: flag,
                            visible: false,
                            //disabled: function (e) {
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0 || getQuarter() > 4) //true -f01/ f03 or apprcost <= 0

                            //}
                        },
                        {
                            dataField: "Projected_Amount",
                            caption: "Projected Amt ($)",
                            width: 150,
                            dataType: "number",
                            format: { type: "currency", precision: 2 },
                            allowEditing: flag,
                            visible: false,
                            //disabled: function (e) {
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0) //true -f01/ f03 or apprcost <= 0

                            //}


                        },
                        {
                            dataField: "Unused_Amount",
                            caption: "Unused Amt ($)",
                            width: 150,
                            dataType: "number",
                            format: { type: "currency", precision: 2 },
                            allowEditing: false,
                            visible: false,
                            setCellValue: function (rowData, value) {

                                rowData.Unused_Amount = value;

                            },
                            //disabled: function (e) {
                            //    return (isF03F01(e.row.data.Fund) || e.row.data.Reviewed_Cost <= 0) //true -f01/ f03 or apprcost <= 0

                            //}

                        },
                        {

                            dataField: "BU",
                            validationRules: [{ type: "required" }],
                            width: 70,
                            lookup: {
                                dataSource: BU_list,
                                valueExpr: "ID",
                                displayExpr: "BU"
                            },
                            allowEditing: true,
                            headerFilter: {
                                dataSource: BU_headerFilter,
                                allowSearch: true
                            },
                        },

                        //},
                        {
                            dataField: "OEM",
                            width: 100,
                            validationRules: [{ type: "required" }],
                            headerFilter: {
                                dataSource: OEM_headerFilter,
                                allowSearch: true
                            },
                            lookup: {
                                dataSource: OEM_list,
                                valueExpr: "ID",
                                displayExpr: "OEM"
                            },
                            allowEditing: true

                        },
                        {
                            dataField: "DEPT",
                            caption: "Dept",
                            headerFilter: {
                                dataSource: DEPT_headerFilter,
                                allowSearch: true
                            },
                            validationRules: [{ type: "required" }],
                            setCellValue: function (rowData, value) {

                                rowData.DEPT = value;
                                rowData.Group = null;

                            },
                            width: 140,
                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: DEPT_list
                                        //filter: options.data ? ["Outdated", "=", false] : null


                                    };
                                },

                                valueExpr: "ID",
                                displayExpr: "DEPT"

                            },
                            allowEditing: true


                        },
                        {
                            dataField: "Group",
                            width: 150,
                            headerFilter: {
                                dataSource: Group_headerFilter,
                                allowSearch: true
                            },
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
                            allowEditing: true
                        },

                        {
                            dataField: "Project",
                            width: 90,
                            allowEditing: !flag,
                            visible: false

                        },
                        {
                            dataField: "Item_Name",
                            caption: "Item",
                            minWidth: 300,
                            validationRules: [{ type: "required" }],
                            lookup: {
                                dataSource: function (options) {
                                    // //debugger;
                                    return {


                                        store: /*Item_list_bkp*/ /*Item_list_New*/Item_list,

                                        filter: options.data ? [["BU", "=", BU_forItemFilter != 0 ? BU_forItemFilter : options.data.BU], 'and', ["Deleted", "=", false]] : null
                                    }


                                },
                                valueExpr: "S_No",
                                displayExpr: "Item_Name"
                            },
                            headerFilter: {
                                dataSource: Item_headerFilter,
                                allowSearch: true
                            },
                            setCellValue: function (rowData, value) {
                                ////debugger;
                                //if value.constructur.name == "Array" => it means that the item list is filtered based on the ordertype selected. At that instance, no need to fetch other details
                                if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                {
                                    rowData.Item_Name = value;
                                    rowData.Category = Item_list.find(x => x.S_No == value).Category;
                                    rowData.Cost_Element = parseInt(Item_list.find(x => x.S_No == value).Cost_Element);
                                    rowData.Unit_Price = Item_list.find(x => x.S_No == value).UnitPriceUSD;
                                    //rowData.ActualAvailableQuantity = Item_list.find(x => x.S_No == value).Actual_Available_Quantity;
                                    rowData.BudgetCode = Item_list.find(x => x.S_No == value).BudgetCode;
                                    rowData.BudgetCodeDescription = BudgetCodeList.find(x => x.Budget_Code == rowData.BudgetCode).Budget_Code_Description;
                                    rowData.UnitofMeasure = parseInt(Item_list.find(x => x.S_No == value).UOM);
                                }
                            },
                            //filterType: "include",
                            //filterValues: [3085],
                            //lookup: {
                            //    dataSource: function (options) {

                            //        return {

                            //            store: Item_list,
                            //            filter: options.data ? [["BU", "=", options.data.BU], 'and', ["Deleted", "=", false]] : null

                            //        };

                            //    },
                            //    valueExpr: "S_No",
                            //    displayExpr: "Item_Name"
                            //},
                            allowEditing: true
                        },
                        {
                            dataField: "OrderType",
                            caption: "Order Type",
                            setCellValue: function (rowData, value) {
                                ////debugger;
                                rowData.OrderType = value;
                                rowData.Item_Name = null;

                            },
                            lookup: {
                                dataSource: function (options) {
                                    //  //debugger;
                                    return {

                                        store: OrderType_list,
                                    }

                                },
                                valueExpr: "ID",
                                displayExpr: "Order_Type"
                            },
                            visible: false

                        },
                        {
                            dataField: "CostCenter",
                            caption: "Cost Center",
                            allowEditing: false,
                            visible: false
                        },
                        {
                            dataField: "BudgetCenterID",
                            caption: "Budget Center",
                            lookup: {
                                dataSource: function (options) {
                                    // //debugger;
                                    return {

                                        store: BudCenter,
                                    }

                                },
                                valueExpr: "ID",
                                displayExpr: "BudgetCenter",
                            },

                            visible: false,
                            //allowEditing: false,

                        },
                        {
                            dataField: "UnitofMeasure",
                            caption: "Unit of Measure",
                            lookup: {
                                dataSource: function (options) {
                                    // //debugger;
                                    return {

                                        store: UOM_list,


                                    }

                                },
                                valueExpr: "ID",
                                displayExpr: "UOM"
                            },
                            visible: false,
                            allowEditing: false,
                        },
                        {
                            dataField: "UnloadingPoint",
                            caption: "Unloading Point",

                            visible: false,
                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: UnloadingPoint_list,
                                    }

                                },
                                valueExpr: "ID",
                                displayExpr: "UnloadingPoint"
                            },
                        },

                        //{
                        //    dataField: 'Comments',
                        //    label: {
                        //        text: 'Comments'
                        //    },
                        //    dataType: 'string',
                        //},

                        {
                            dataField: "ActualAvailableQuantity",
                            caption: "Available Qty",
                            allowEditing: false,
                            width: 110


                        },
                        {
                            dataField: "Category",
                            caption: "Category",
                            //validationRules: [{ type: "required" }],
                            headerFilter: {
                                dataSource: Category_headerFilter,
                                allowSearch: true
                            },
                            lookup: {
                                dataSource: Category_list,
                                valueExpr: "ID",
                                displayExpr: "Category"
                            },
                            allowEditing: false,
                            visible: false

                        },
                        //{
                        //    dataField: "BudgetCode",
                        //    visible: false
                        //    },
                        {
                            dataField: "Cost_Element",
                            headerFilter: {
                                dataSource: CostElement_headerFilter,
                                allowSearch: true
                            },
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
                            headerFilter: {
                                dataSource: BudgetCode_headerFilter,
                                allowSearch: true
                            },
                            lookup: {
                                dataSource: BudgetCodeList,
                                valueExpr: "Budget_Code",
                                displayExpr: "Budget_Code"
                            },
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
                            dataField: "LabName",
                            caption: "Lab Name",
                            visible: false
                        },
                        {
                            dataField: 'RFOReqNTID',
                            setCellValue: function (rowData, value) {

                                rowData.RFOReqNTID = value;

                            },
                            visible: false,
                            allowEditing: false,
                        },
                        {
                            dataField: "RFOApprover",
                            caption: "RFO Approver",
                            visible: false

                        },
                        {
                            dataField: "QuoteAvailable",
                            caption: "Quote Available",
                            visible: false,
                            editCellTemplate: editCellTemplate


                        },
                        {
                            dataField: "GoodsRecID",
                            caption: "Goods Rec ID",
                            visible: false

                        },
                        {
                            dataField: "Required_Quantity",
                            caption: "Request Qty",
                            width: 100,
                            //validationRules: [
                            //    { type: "required" },
                            //{
                            //    type: "range",
                            //    message: "Quantity cannot be negative",
                            //    min: 0,
                            //    max: 214783647
                            //}],
                            dataType: "number",
                            setCellValue: function (rowData, value) {

                                rowData.Required_Quantity = value;

                            },
                            allowEditing: false,
                            visible: true





                        },
                        {
                            dataField: "Unit_Price",
                            caption: "Unit Price",
                            dataType: "number",
                            format: { type: "currency", precision: 0 },
                            valueFormat: "#0",
                            // validationRules: [{ type: "required" }, {
                            //    type: "range",
                            //    message: "Please enter valid price > 0",
                            //    min: 0.01,
                            //    max: Number.MAX_VALUE
                            //}],
                            allowEditing: false,
                            visible: false

                        },
                        {
                            dataField: "Total_Price",
                            caption: "Amt"
                            , calculateCellValue: function (rowData) {

                                if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
                                    return rowData.Unit_Price * rowData.Required_Quantity;
                                }
                                else
                                    return 0.0;
                            },

                            dataType: "number",
                            format: { type: "currency", precision: 0 },
                            valueFormat: "#0",
                            allowEditing: false,
                            visible: false
                        },


                        {
                            dataField: "Reviewed_Quantity",
                            caption: "Review Qty",
                            width: 100,
                            dataType: "number",
                            validationRules: [
                                { type: "required" },
                                {
                                    type: "range",
                                    message: "Quantity cannot be negative",
                                    min: -0.1,
                                    max: 214783647
                                }],
                            setCellValue: function (rowData, value) {
                                // //debugger;
                                rowData.Reviewed_Quantity = value;

                            },
                            allowEditing: function (e) {
                                // //debugger;
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return !e.row.data.ApprovedSH
                            },



                            //labteam - edit - true                  1 || 1
                            //vkmspoc - approvedsh - false - edit    0 || 1
                            //vkmspoc - approvedsh - true - not edit 0 || 0



                        },
                        {
                            dataField: "Reviewed_Cost",
                            caption: "Review Amt",
                            width: 120,
                            //calculateCellValue: function (rowData) {

                            //    //if (/*(rowData.Reviewed_Cost == null || rowData.Reviewed_Cost == undefined) && */rowData.Reviewed_Quantity > 0 && rowData.Unit_Price > 0) {
                            //    //    return rowData.Unit_Price * rowData.Reviewed_Quantity;
                            //    //}
                            //    //else if (rowData.Reviewed_Cost != null || rowData.Reviewed_Cost != undefined) {
                            //    //    return rowData.Reviewed_Cost;
                            //    //}
                            //    //else
                            //    //    return 0.0;
                            //},

                            dataType: "number",
                            format: { type: "currency", precision: 0 },
                            valueFormat: "#0",
                            allowEditing: false,
                            setCellValue: function (rowData, value) {
                                // //debugger;
                                rowData.Reviewed_Cost = value;

                            },
                        },


                        {
                            dataField: "Requestor",
                            allowEditing: false,
                            visible: false
                        },

                        {
                            dataField: "Comments",
                            caption: "Remark",
                            width: 140,
                            allowResizing: true
                            // allowEditing: false,
                        },
                        {
                            dataField: "PORemarks",
                            width: 140,
                            visible: false

                        },
                        {
                            dataField: "SubmitDate",
                            allowEditing: false,
                            visible: false
                        },
                        {
                            dataField: "OrderID",
                            caption: "PO Number",
                            visible: false,

                            allowEditing: function (e) {
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;

                                return flag || !e.row.data.ApprovedSH
                            },

                            //allowEditing: flag || !e.row.data.ApprovedSH  

                        },
                        {
                            dataField: "Currency",
                            lookup: {
                                dataSource: Currency_list,
                                valueExpr: "ID",
                                displayExpr: "Currency"
                            },
                            visible: false,
                            validationRules: [{
                                type: "required",
                                message: "Currency is required for SR,PR,PO,Invoice amounts"
                            }],
                        },
                        {
                            dataField: 'SR_Value',
                            caption: 'SR Value',
                            visible: false,

                        }, {
                            dataField: 'PR_Value',
                            caption: 'PR Value',
                            visible: false,

                        }, {
                            dataField: 'Invoice_Value',
                            visible: false,

                        }, {
                            dataField: 'OrderPrice_UserInput',
                            caption: "Order Price",
                            visible: false,
                        },
                        {
                            dataField: "OrderPrice",
                            visible: false,
                            caption: "Order Price (USD)",
                            //calculateCellValue: function (rowData) {
                            //    var orderpriceinusd;

                            //   ////debugger;
                            //    //based on currency chosen & price entered, convert the order price to usd value
                            //    if (rowData.OrderPrice >0 && rowData.Currency != undefined) {

                            //        $.ajax({

                            //            type: "GET",
                            //            url: "/BudgetingVKM/GetOrderPriceinUSD",
                            //            data: { 'OrderPrice': rowData.OrderPrice, 'Currency': rowData.Currency },
                            //            datatype: "json",
                            //            async: false,
                            //            success: success_getorder_priceusd,
                            //            error: error_getorder_priceusd

                            //        });

                            //        function success_getorder_priceusd(response) {
                            //           ////debugger;
                            //            orderpriceinusd = response;


                            //        }


                            //        function error_getorder_priceusd(response) {
                            //           ////debugger;
                            //            $.notify('Error in converting the entered Order Price to USD!', {
                            //                globalPosition: "top center",
                            //                className: "warn"
                            //            });
                            //        }
                            //    }

                            //    return orderpriceinusd;


                            //},

                            format: { type: "currency", precision: 2 },
                            valueFormat: "#0.00",
                            //allowEditing: flag || !e.row.data.ApprovedSH 
                            allowEditing: function (e) {
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return flag || !e.row.data.ApprovedSH
                            },

                        },

                        {
                            dataField: "OrderedQuantity",
                            caption: "Ordered Qty",
                            visible: false,
                            // allowEditing: flag || !e.row.data.ApprovedSH 
                            allowEditing: function (e) {
                                ////debugger;
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return flag || !e.row.data.ApprovedSH
                            },


                        },
                        {
                            dataField: "OrderStatus",
                            //headerFilter: {
                            //    dataSource: OrderStatus_headerFilter,
                            //    allowSearch: true
                            //},
                            visible: true,

                            //setCellValue: function (rowData, value) {
                            //    //debugger;
                            //    rowData.OrderStatus = value;

                            //    if (value == 3) {
                            //        var today = new Date();
                            //        var dd = String(today.getDate()).padStart(2, '0');
                            //        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                            //        var yyyy = today.getFullYear();

                            //        today = yyyy + '-' + mm + '-' + dd;
                            //        rowData.ELOSubmittedDate = today;
                            //    }
                            //    //else
                            //    //    rowData.ELOSubmittedDate = null;
                            //},
                            setCellValue: function (rowData, value, currentRowData) {
                                //debugger;
                                //if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                {
                                    rowData.OrderStatus = value;

                                    if (value == 3) {
                                        var today = new Date();
                                        var dd = String(today.getDate()).padStart(2, '0');
                                        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                                        var yyyy = today.getFullYear();

                                        today = yyyy + '-' + mm + '-' + dd;
                                        rowData.ELOSubmittedDate = today;
                                    } else
                                        rowData.ELOSubmittedDate = currentRowData.ELOSubmittedDate;



                                    if (currentRowData.RequestOrderDate != undefined) {
                                        rfodate = Date.parse(currentRowData.RequestOrderDate);
                                        elodate = Date.parse(rowData.ELOSubmittedDate);

                                        var Difference_In_Time = Math.abs(elodate - rfodate);

                                        // To calculate the no. of days between two dates
                                        var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                        rowData.DaysTaken = Difference;
                                    }


                                }

                            },

                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: OrderStatus_list,


                                    };
                                },

                                valueExpr: "ID",
                                displayExpr: "OrderStatus"

                            },
                            //allowEditing: flag || !e.row.data.ApprovedSH 
                            allowEditing: flag
                            //function (e) {
                            //if (e.row.data.ApprovedSH == undefined)
                            //    e.row.data.ApprovedSH = false;
                            //return flag //|| !e.row.data.ApprovedSH
                            //},



                        },
                        {
                            dataField: "Description",
                            caption: "Order Status Description",
                            visible: true,
                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: OrderDescription_list,


                                    };
                                },

                                valueExpr: "Description",
                                displayExpr: "Description"

                            },
                            allowEditing: flag

                        },
                        {
                            dataField: "LinkedRequests",
                            caption: 'Linked Requests',
                            visible: false,

                        },
                        //{
                        //    dataField: "LinkedRequestID",
                        //    caption: 'Linked Request ID',
                        //    visible: false,
                        //    groupIndex: 0,

                        //},



                        {
                            dataField: "Reviewer_1",
                            caption: "HOE",
                            allowEditing: false,
                            visible: false
                        },
                        {
                            dataField: "Reviewer_2",
                            caption: "VKM SPOC",
                            allowEditing: false,
                            visible: false
                        },
                        {
                            dataField: "RequiredDate",
                            dataType: "date",
                            allowEditing: true,
                            visible: false

                        },
                        //{
                        //    dataField: "RequestOrderDate",
                        //    dataType: "date",
                        //    allowEditing: false,
                        //    visible: false//flag

                        //},
                        {
                            dataField: "RequestOrderDate",
                            caption: "RFO Submitted Date",
                            dataType: "date",
                            allowEditing: false,
                            visible: false,
                            setCellValue: function (rowData, value) {

                                rowData.RFOSubmittedDate = value;

                            },
                        },
                        {
                            dataField: "OrderDate",
                            caption: "PO Release Date",
                            dataType: "date",
                            allowEditing: true,
                            visible: false//flag

                        },
                        {
                            dataField: "TentativeDeliveryDate",
                            caption: "Tentative Dt",
                            dataType: "date",
                            visible: /*flag*/false,
                            allowEditing: function (e) {
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return flag || !e.row.data.ApprovedSH
                            },
                            //allowEditing: flag || !e.row.data.ApprovedSH 

                        },
                        {
                            dataField: "ActualDeliveryDate",
                            dataType: "date",
                            caption: "Actual Dt",
                            visible: false,//flag,
                            allowEditing: function (e) {
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return flag || !e.row.data.ApprovedSH
                            },
                            //allowEditing: flag || !e.row.data.ApprovedSH 

                        },
                        {
                            dataField: "Fund",

                            setCellValue: function (rowData, value) {

                                rowData.Fund = value;


                            },
                            visible: flag,
                            allowEditing: function (e) {
                                if (e.row.data.ApprovedSH == undefined)
                                    e.row.data.ApprovedSH = false;
                                return flag || !e.row.data.ApprovedSH
                            },
                            //allowEditing: flag || !e.row.data.ApprovedSH ,
                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: Fund_list,


                                    };
                                },

                                valueExpr: "ID",
                                displayExpr: "Fund",

                            }
                        },

                        {
                            dataField: "Customer_Name",
                            allowEditing: true,
                            visible: false
                        },
                        {
                            dataField: "Customer_Dept",
                            allowEditing: true,
                            visible: false
                        },
                        {
                            dataField: "BM_Number",
                            allowEditing: true,
                            visible: false
                        },
                        {
                            dataField: "Task_ID",
                            allowEditing: true,
                            visible: false
                        },
                        {
                            dataField: "Resource_Group_Id",
                            allowEditing: true,
                            visible: false
                        },
                        {
                            dataField: "PIF_ID",
                            caption: "PIF ID (WBS)",
                            allowEditing: true,
                            visible: false
                        },
                        {
                            dataField: "Project_ID",
                            allowEditing: true,
                            visible: false

                        },
                        {
                            dataField: "Purchase_Type",
                            allowEditing: true,
                            visible: false,
                            lookup: {
                                dataSource: function (options) {
                                    return {
                                        store: PurchaseType_list,
                                    };
                                },
                                valueExpr: "ID",
                                displayExpr: "PurchaseType"
                            }

                        },
                        {
                            dataField: 'Material_Part_Number',
                            dataType: 'string',
                            visible: false,
                        },
                        {
                            dataField: "SupplierName_with_Address",
                            allowEditing: true,
                            visible: false

                        },
                        {
                            dataField: "POSpocNTID",
                            caption: "PO Spoc NTID",
                            allowEditing: false,
                            visible: false,

                        },
                        //{
                        //    dataField: "RequestOrderDate",
                        //    caption: "RFO Submitted Date",
                        //    allowEditing: false,
                        //    visible: false,
                        //    setCellValue: function (rowData, value) {

                        //        rowData.RFOSubmittedDate = value;

                        //    },
                        //},
                        {
                            dataField: "ELOSubmittedDate",
                            caption: "ELO Submitted Date",
                            allowEditing: true,
                            visible: false,
                            //setCellValue: function (rowData, value) {

                            //    rowData.ELOSubmittedDate = value;


                            //},
                            //setCellValue: function (rowData, value, currentRowData) {
                            //    //debugger;
                            //    //if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                            //    {

                            //        rowData.ELOSubmittedDate = value;
                            //        if (currentRowData.RequestOrderDate != undefined) {
                            //            rfodate = Date.parse(currentRowData.RequestOrderDate);
                            //            elodate = Date.parse(rowData.ELOSubmittedDate);

                            //            var Difference_In_Time = Math.abs(elodate - rfodate);

                            //            // To calculate the no. of days between two dates
                            //            var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                            //            rowData.DaysTaken = Difference;
                            //        }


                            //    }

                            //},

                            //calculateCellValue: function (rowData) {
                            //    //debugger;
                            //    if (rowData.OrderStatus == 3) {

                            //        var today = new Date();
                            //        var dd = String(today.getDate()).padStart(2, '0');
                            //        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                            //        var yyyy = today.getFullYear();

                            //        today = mm + '/' + dd + '/' + yyyy;

                            //        return today;
                            //    }
                            //    else
                            //        return 0;
                            //},
                        },
                        {
                            dataField: "DaysTaken",
                            caption: "Days Taken",
                            allowEditing: true,
                            visible: false,
                            dataType: "number",
                            customizeText: function (cellInfo) {
                                //debugger;
                                if (
                                    cellInfo.value === "" ||
                                    cellInfo.value === null ||
                                    cellInfo.value === undefined ||
                                    cellInfo.valueText === "NaN"
                                ) {
                                    return "NA";
                                } else {
                                    return cellInfo.valueText;
                                }
                            },
                            //calculateCellValue: function (rowData) {
                            //    //debugger;
                            //    if (rowData.ELOSubmittedDate != null && rowData.RequestOrderDate != null) {

                            //        rfodate = Date.parse(rowData.RequestOrderDate);
                            //        elodate = Date.parse(rowData.ELOSubmittedDate);

                            //        var Difference_In_Time = Math.abs(elodate - rfodate);

                            //        // To calculate the no. of days between two dates
                            //        var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                            //        return Difference;
                            //    }
                            //    else
                            //        return 0;
                            //},
                        },
                        {
                            dataField: "SRSubmitted",
                            caption: "SR Submitted",
                            dataType: "date",
                            allowEditing: true,
                            visible: false,
                            setCellValue: function (rowData, value, currentRowData) {
                                //debugger;
                                //if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                {

                                    rowData.SRSubmitted = value;
                                    if (currentRowData.SRAwardedDate != undefined) {
                                        srawardeddate = Date.parse(currentRowData.SRAwardedDate);
                                        srsubmitteddate = Date.parse(rowData.SRSubmitted);

                                        var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                                        // To calculate the no. of days between two dates
                                        var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                        rowData.SRApprovalDays = Difference;
                                    }


                                }

                            },

                            //setCellValue: function (rowData, value) { IF UNCOMMENTED, DATEBOX DOESNOT OPENS ON SINGLE CLK OF THE CELL ; OPENS AFTER TWO CLICKS ONLY 

                            //    rowData.SRSubmitted = value;
                            //}

                        },
                        {
                            dataField: "RFQNumber",
                            caption: "RFQ Number",
                            allowEditing: true,
                            visible: false,
                            setCellValue: function (rowData, value) {

                                rowData.RFQNumber = value;

                                if (value != null) {
                                    var today = new Date();
                                    var dd = String(today.getDate()).padStart(2, '0');
                                    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                                    var yyyy = today.getFullYear();

                                    today = yyyy + '-' + mm + '-' + dd;
                                    rowData.SRSubmitted = today;
                                }

                            },

                        },
                        {
                            dataField: "PRNumber",
                            caption: "PR Number",
                            allowEditing: true,
                            visible: false
                        },
                        //{
                        //    dataField: "OrderID",
                        //    caption: "PO Number",
                        //    allowEditing: true,
                        //    visible: false
                        //},
                        //{
                        //    dataField: "OrderDate",
                        //    caption: "PO Release Date",
                        //    allowEditing: true,
                        //    visible: false
                        //},
                        {
                            dataField: "SRAwardedDate",
                            caption: "SRAwardedDate",
                            dataType: "date",
                            allowEditing: true,
                            visible: false,
                            //setCellValue: function (rowData, value) {

                            //    rowData.SRAwardedDate = value;
                            //}
                            setCellValue: function (rowData, value, currentRowData) {
                                //debugger;
                                //if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                {

                                    rowData.SRAwardedDate = value;
                                    if (currentRowData.SRSubmitted != undefined) {
                                        srawardeddate = Date.parse(rowData.SRAwardedDate);
                                        srsubmitteddate = Date.parse(currentRowData.SRSubmitted);

                                        var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                                        // To calculate the no. of days between two dates
                                        var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                                        rowData.SRApprovalDays = Difference;
                                    }


                                }

                            },

                        },
                        {
                            dataField: "SRApprovalDays",
                            caption: "SR Approval Days",
                            allowEditing: true,
                            visible: false,
                            dataType: "number",
                            customizeText: function (cellInfo) {
                                //debugger;
                                if (
                                    cellInfo.value === "" ||
                                    cellInfo.value === null ||
                                    cellInfo.value === undefined ||
                                    cellInfo.valueText === "NaN"
                                ) {
                                    return "NA";
                                } else {
                                    return cellInfo.valueText;
                                }
                            },
                            setCellValue: function (rowData, value) {
                                //debugger;
                                rowData.SRApprovalDays = value;
                            }
                            //calculateCellValue: function (rowData) {
                            //    //debugger;
                            //    if (rowData.SRAwardedDate != null && rowData.SRSubmitted != null) {

                            //        srawardeddate = Date.parse(rowData.SRAwardedDate);
                            //        srsubmitteddate = Date.parse(rowData.SRSubmitted);

                            //        var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                            //        // To calculate the no. of days between two dates
                            //        var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                            //        return Difference;
                            //    }
                            //    else
                            //        return 0;
                            //},

                        },
                        {
                            dataField: "SRResponsibleBuyerNTID",
                            caption: "SR Responsible Buyer",
                            allowEditing: true,
                            visible: false,
                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: SRBuyer_list,


                                    };
                                },

                                valueExpr: "NTID",
                                displayExpr: "BuyerName"

                            },
                            setCellValue: function (rowData, value) {
                                //debugger;
                                if (value.constructor.name == "String")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                {

                                    rowData.SRResponsibleBuyerNTID = value;
                                    var mgrID = SRBuyer_list.find(x => x.NTID == value).Manager_ID;
                                    rowData.SRManagerNTID = SRManager_list.find(x => x.ID == mgrID).NTID;
                                    //debugger;
                                    /*rowData.BudgetCodeDescription = BudgetCodeList.find(x /=> x.Budget_Code == rowData.BudgetCode).Budget_Code_Description;*/
                                }
                            },

                        },
                        {
                            dataField: "SRManagerNTID",
                            caption: "SR Manager",
                            allowEditing: true,
                            visible: false,
                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: SRManager_list,


                                    };
                                },

                                valueExpr: "NTID",
                                displayExpr: "ManagerName"

                            },
                        },
                        {
                            caption: "Item Change Log",
                            allowEditing: false,
                            visible: true,

                            width: 100,
                            alignment: 'center',
                            cellTemplate: function (container, options) {
                                $('<a/>').addClass('dx-link')
                                    .text('View')
                                    .on('dxclick', function () {
                                        //debugger;
                                        //var thePath = "\\bosch.com\dfsrb\DfsIN\LOC\Kor\BE-ES\ELO\Global";

                                        //window.open("file://///bosch.com\\dfsrb\\DfsIN\\LOC\\Kor\\BE-ES\\ELO\\Global"); 
                                        //Do something with options.data;  
                                        //debugger;
                                        //alert(options.data.RequestID);
                                        $.ajax({

                                            type: "POST",
                                            url: "/BudgetingVKM/GetL2Details",
                                            data: { 'RequestID': options.data.RequestID },
                                            datatype: "json",
                                            async: true,
                                            success: function (data) {
                                                //debugger;
                                                //$("#myModal").css({ "display": "block" });
                                                //alert(data.data.RequestDate);
                                                //alert(data.data.L1Remarks);
                                                //$("#myModal").modal("show");

                                                $("#popup").dxPopup({
                                                    showTitle: true,
                                                    title: "L2 Item Change Log",
                                                    visible: true,
                                                    hideOnOutsideClick: true,
                                                    width: 450,
                                                    height: 350,
                                                    resizeEnabled: true

                                                });
                                                //debugger;
                                                //$("#dxL1Details").dxDataGrid({
                                                //    dataSource: data.data,
                                                //    //keyExpr: "EmployeeID",
                                                //    visible: true,
                                                //    columns: [{
                                                //        caption:"Remarks",
                                                //        dataField: "L1Remarks"
                                                //    }, {
                                                //        caption: "Request Date",
                                                //        dataField: "RequestDate"
                                                //    }]
                                                //});

                                                $("#dxL2Details").dxForm({
                                                    //formData: {
                                                    //    Remarks: data.data.L1Remarks,
                                                    //    RequestDate: data.data.RequestDate
                                                    //}

                                                    formData: data.data,
                                                    items: [{
                                                        caption: "L1 Remarks",
                                                        dataField: 'L1Remarks',
                                                        editorOptions: {
                                                            disabled: true,
                                                        },
                                                    },
                                                    {
                                                        caption: "L1 Submit Date",
                                                        dataField: 'L1SubmitDate',
                                                        editorOptions: {
                                                            disabled: true,
                                                        },
                                                    },
                                                    {
                                                        caption: "L1 Qty",
                                                        dataField: 'L1Qty',
                                                        editorOptions: {
                                                            disabled: true,
                                                        },
                                                    },
                                                    {
                                                        caption: "L2 Remarks",
                                                        dataField: 'L2Remarks',
                                                        editorOptions: {
                                                            disabled: true,
                                                        },
                                                    },
                                                    {
                                                        caption: "L2 Review Date",
                                                        dataField: 'L2ReviewDate',
                                                        editorOptions: {
                                                            disabled: true,
                                                        },
                                                    },
                                                    {
                                                        caption: "L2 Qty",
                                                        dataField: 'L2Qty',
                                                        editorOptions: {
                                                            disabled: true,
                                                        },
                                                    }]
                                                });
                                            }
                                        });

                                    })
                                    .appendTo(container);
                            }

                        },
                        {
                            caption: "Uploaded Files",
                            //allowEditing: false,
                            visible: true,
                            width: 100,
                            alignment: 'center',
                            cellTemplate: function (container, options) {
                                $('<a/>').addClass('dx-link')
                                    .text(options.data.RequestID)
                                    .on('dxclick', function () {
                                        //Do something with options.data;  
                                        //debugger;
                                        //var url;
                                        $.ajax({



                                            type: "POST",
                                            url: "/BudgetingVKM/EncodeURL",
                                            data: { 'RequestID': options.data.RequestID },
                                            datatype: "json",
                                            async: true,
                                            success: function (data) {
                                                //debugger;
                                                //window.location.href = data;
                                                //url = data.HostURL;
                                                window.open(data.Result, '_blank');
                                            },
                                            error: function (e) {
                                                alert("Unable to redirect");
                                            },
                                        })
                                    }).appendTo(container);
                            }
                        },



                    ]
                },
            ],


            //onEditingStart: function (e) {
            //    //debugger;
            //    if (e.data.Reviewed_Quantity) {
            //        //debugger;
            //    }

            //}, 
            onInitNewRow: function (e) {
                //debugger;
                is_newitem = true;
                //e.data.Requestor = new_request.Requestor;
                //e.data.Reviewer_1 = new_request.Reviewer_1;
                //e.data.Reviewer_2 = new_request.Reviewer_2;
                //e.data.DEPT = new_request.DEPT;
                //    e.data.Group = new_request.Group;

                e.data.POSpocNTID = new_request.POSpocNTID;
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

                e.data.Reviewed_Quantity = new_request.Reviewed_Quantity;
                e.data.Reviewed_Cost = new_request.Reviewed_Cost;


            },
            onRowUpdated: function (e) {
                //debugger;
                $.notify("Item in your Queue is being Updated...Please wait!", {
                    globalPosition: "top center",
                    autoHideDelay: 15000,
                    className: "success"
                })

                ////debugger;
                //if (e.data.OrderedQuantity > e.data.Reviewed_Quantity) {
                //    $.notify("Ordered Quantity cannot be greater than Reviewed Quantity, Please check again!", {
                //        globalPosition: "top center",
                //        className: "error"
                //    })
                //}
                debugger;

                //Order Price to USD conversion commented - since user inputs are received for sr,pr,invoice,order amounts in either INR/EUR/USD and USD Converted amounts are maintained separately - taken care in sp
                //if (e.data.OrderPrice_UserInput > 0 && e.data.Currency != undefined) {
                //    //debugger;
                //    $.ajax({

                //        type: "GET",
                //        url: "/BudgetingVKM/GetOrderPriceinUSD",
                //        data: { 'OrderPrice': e.data.OrderPrice_UserInput, 'Currency': e.data.Currency },
                //        datatype: "json",
                //        async: false,
                //        success: success_getorder_priceusd,
                //        error: error_getorder_priceusd

                //    });

                //    function success_getorder_priceusd(response) {
                //        ////debugger;
                //        e.data.OrderPrice = response;


                //    }


                //    function error_getorder_priceusd(response) {
                //        ////debugger;
                //        //$.notify('Error in converting the entered Order Price to USD!', {
                //        //    globalPosition: "top center",
                //        //    className: "warn"
                //        //});
                //    }
                //}


                //if (e.data.SRAwardedDate != null && e.data.SRSubmitted != null) {
                //    //debugger;
                //    srawardeddate = Date.parse(e.data.SRAwardedDate);
                //    srsubmitteddate = Date.parse(e.data.SRSubmitted);

                //    var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                //    // To calculate the no. of days between two dates
                //    var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                //    e.data.SRApprovalDays = Difference;
                //}
                //else
                //    e.data.SRApprovalDays = 0;


                //if (e.data.RequestOrderDate != null && e.data.ELOSubmittedDate != null) {
                //    //debugger;
                //    srawardeddate = Date.parse(e.data.ELOSubmittedDate);
                //    srsubmitteddate = Date.parse(e.data.RequestOrderDate);

                //    var Difference_In_Time = Math.abs(srawardeddate - srsubmitteddate);

                //    // To calculate the no. of days between two dates
                //    var Difference = Math.ceil(Difference_In_Time / (1000 * 60 * 60 * 24));
                //    e.data.DaysTaken = Difference;
                //}
                //else
                //    e.data.DaysTaken = 0;

                Selected = [];
                e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
                //e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;

                Selected.push(e.data);
                ////debugger;
                Update(Selected, filtered_yr, '');
            },
            onRowInserting: function (e) {
                new_request = false;
                $.notify("New Item is being added to your cart...Please wait!", {
                    globalPosition: "top center",
                    className: "success"
                })
                debugger;
                e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
                if (e.data.Reviewed_Cost == null || e.data.Reviewed_Cost == undefined)
                    e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
                //if (e.data.OrderPrice_UserInput > 0 && e.data.Currency != undefined) {

                //    $.ajax({

                //        type: "GET",
                //        url: "/BudgetingVKM/GetOrderPriceinUSD",
                //        data: { 'OrderPrice': e.data.OrderPrice_UserInput, 'Currency': e.data.Currency },
                //        datatype: "json",
                //        async: false,
                //        success: success_getorder_priceusd,
                //        error: error_getorder_priceusd

                //    });

                //    function success_getorder_priceusd(response) {
                //        debugger;
                //        e.data.OrderPrice = response;


                //    }


                //    function error_getorder_priceusd(response) {
                //        debugger;
                //        //$.notify('Error in converting the entered Order Price to USD!', {
                //        //    globalPosition: "top center",
                //        //    className: "warn"
                //        //});
                //    }
                //}

                Selected = [];
                Selected.push(e.data);
                Update(Selected, filtered_yr, '');
            },
            onRowRemoving: function (e) {

                Delete(e.data.RequestID, filtered_yr);

            },

        });
    }
    debugger;
    //scrollableDg1 = $("#RequestTable_VKMSPOC").dxDataGrid("instance").getScrollable();
    //scrollableDg2 = abcd.getScrollable();

    //scrollableDg1.on("scroll", function (e) {
    //    scrollableDg2.scrollTo(e.scrollOffset);
    //});

    //Add option for Lab Admins
    // $("#RequestTable_VKMSPOC").dxDataGrid("instance").option("editing.allowAdding", flag);









    //multiselect depts & change the dept mappings

    //var selectedRowsData = dataGridLEP.getSelectedRowsData();



    function changeDept() {
        ////debugger;


        dataGrid.getSelectedRowsData().then(function (rowData) {

            ////debugger;

            for (var i = 0; i < rowData.length; i++) {
                reqIDarray.push(rowData[i].RequestID);
            }

            if (reqIDarray.length > 0) {
                document.getElementById("DeptDetails").style.display = "block";
                $("#updateDeptGrpButton").prop('hidden', false);

                //$("#ddlDepts").prop('hidden', false);
                //document.getElementById("ddlDepts").style.display = "block";
                $(".selectpicker").selectpicker('refresh');
                //$("#Depts").prop('hidden', false);
                ////debugger;
            }


        })


    }

    $("#changedeptButton").dxButton({
        text: "Change Dept for the selected Requests",
        type: "default",
        onClick: changeDept
    });
    $("#multiSelect_Submit").dxButton({
        hint: "Submit the selected Requests to Order View",
        //type: "success",
        onClick: multiSelect_Submit
    });
    function multiSelect_Submit() {
        //debugger;
        var selectedRows = $("#RequestTable_VKMSPOC").dxDataGrid("instance").getSelectedRowsData();
        //debugger;
        for (var i = 0; i < selectedRows.length; i++) {
        //debugger;
            reqIDarray.push(selectedRows[i].RequestID);
           // alert(selectedRows[i].RequestID);
        }

        if (reqIDarray.length > 0) {
            //debugger;
            SHApprove(reqIDarray, filtered_yr);
        }
    }











    //$("#updateDeptGrpButton").dxButton({
    //    text: "Update Changes",
    //    type: "default",
    //    onClick: fnDeptGrpChange
    //});
    ////$('#ddlDepts').change(function () {
    ////   ////debugger;
    ////    var deptselected = document.getElementById('ddlDepts').value;
    ////    fnDeptChange(deptselected);
    ////   ////debugger;
    ////});
    //function fnDeptGrpChange() {

    //    var deptselected = document.getElementById('ddlDepts').value;
    //    var grpselected = document.getElementById('ddlGroups').value;
    //    var Dept, Grp;
    //   ////debugger;
    //    $.ajax({
    //        type: "GET",
    //        url: encodeURI("../BudgetingVKM/DeptID_toName"),
    //        data: { 'DeptID': deptselected },
    //        async: false,
    //        success: function (data) {
    //            Dept = data.data;
    //           ////debugger;
    //        }
    //    })
    //    $.ajax({
    //        type: "GET",
    //        url: encodeURI("../BudgetingVKM/GrpID_toName"),
    //        data: { 'GrpID': grpselected },
    //        async: false,
    //        success: function (data) {
    //            Grp = data.data;
    //           ////debugger;
    //        }
    //    })

    //   ////debugger;
    //    if (confirm('Do you confirm to update the selected list with Dept: ' + Dept + ' and Group: ' + Grp   + ' ? ')) {
    //       ////debugger;
    //        $.ajax({

    //            type: "POST",
    //            url: "/BudgetingVKM/UpdateRequestList_DeptGrpChange",
    //            data: { 'reqids': reqIDarray, 'deptselected': deptselected, 'grpselected': grpselected },
    //            async: false,
    //            success: onsuccess_deptdetails,
    //            error: onerror_deptdetails
    //        })
    //    }
    //}


    //function onsuccess_deptdetails(response) {

    //    $.notify(response.message, {
    //        globalPosition: "top center",
    //        className: "success"
    //    });

    //    reqIDarray = [];
    //    deptselected = "";
    //    deptselected = "";
    //    document.getElementById("DeptDetails").style.display = "none";
    //    $("#updateDeptGrpButton").prop('hidden', true);
    //    $(".selectpicker").selectpicker('refresh');
    //    $.ajax({
    //        type: "GET",
    //        url: "/BudgetingVKM/GetData",
    //        data: { 'year': filtered_yr },
    //        datatype: "json",
    //        async: true,
    //        success: success_refresh_getdata,
    //        error: error_refresh_getdata

    //    });

    //    function success_refresh_getdata(response) {

    //        var getdata = response.data;
    //        $("#RequestTable_VKMSPOC").dxDataGrid({
    //            dataSource: getdata
    //        });
    //    }
    //    function error_refresh_getdata(response) {

    //        $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
    //            globalPosition: "top center",
    //            className: "warn"
    //        });

    //    }
    //   ////debugger;

    //}


    //function onerror_deptdetails(response) {
    //   ////debugger;
    //    $.notify("Unable to update the Request List with the Ordering Stage Dept, Please try again!", {
    //        globalPosition: "top center",
    //        className: "warn"
    //    });
    //}

}




function OnError_GetData(response) {
    $("#RequestTable_VKMSPOC").prop('hidden', false);
    //$.notify(response.message, {
    //    globalPosition: "top center",
    //    className: "warn"
    //})
}


$.ajax({

    type: "GET",
    url: "/BudgetingVKM/CurrencyConversion",
    async: false,
    success: onsuccess_currconv,
    error: onerror_currconv
})


function onsuccess_currconv(response) {
    ////debugger;
    var currdata = (response.data);
    currtable = $("#CurrTable").dxDataGrid({

        loadPanel: {
            enabled: true
        },

        dataSource: currdata,
        editing: {
            mode: "row",
            allowUpdating: true,
            useIcons: true
        },
        columns: [
            {
                type: "buttons",
                width: 40,
                alignment: "left",
                buttons: [
                    "edit", "delete"
                ]

            },
            {
                caption: "Currency Conversion Rate Details",
                alignment: "center",
                showBorders: true,
                columns: [{
                    dataField: "Currency",
                    allowEditing: false,

                },
                    "ConversionRate"
                ]
            },
        ],
        onRowUpdated: function (e) {
            $.notify(" The latest Currency Conversion changes is being Updated...Please wait!", {
                globalPosition: "top center",
                autoHideDelay: 2000,
                className: "success"
            })
            Selected = [];
            ////debugger;
            Selected.push(e.data);
            Update_Curr(Selected);
        }


    });

}
function onerror_currconv(response) {
    //$.notify("Please try again to Fetch the Currency Conversion Table", {
    //    globalPosition: "top center",
    //    className: "warn"
    //})
}

function Update_Curr(id1) {
    ////debugger;


    $.ajax({
        type: "POST",
        url: encodeURI("../BudgetingVKM/AddOrEdit_CurrConv"),
        data: { 'curritem': id1[0] },
        success: function (data) {
            ////debugger;
            //$.notify(data.message, {
            //    globalPosition: "top center",
            //    className: "success"
            //});

            $.ajax({

                type: "GET",
                url: "/BudgetingVKM/CurrencyConversion",
                //async: false,
                success: onsuccess_currconv,
                error: onerror_currconv
            })

            function onsuccess_currconv(response) {
                ////debugger;
                $("#CurrTable").dxDataGrid({
                    dataSource: response.data
                });


                $.notify("Updating the Item Master List with the Currency Conversion Rate changes, Please wait...!", {
                    globalPosition: "top center",
                    autoHideDelay: 13000,
                    className: "info"
                });
                genSpinner_load_masterlist.classList.add('fa');
                genSpinner_load_masterlist.classList.add('fa-spinner');
                genSpinner_load_masterlist.classList.add('fa-pulse');
                document.getElementById("loadpanel_masterlist").style.display = "block";

                $.ajax({

                    type: "POST",
                    url: "/BudgetingVKM/UpdateMasterList_CurrConv",
                    data: { 'curritem': id1[0] },
                    //async: false,
                    success: onsuccess_masterlistupdate,
                    error: onerror_masterlistupdate
                })

                function onsuccess_masterlistupdate(response) {
                    genSpinner_load_masterlist.classList.remove('fa');
                    genSpinner_load_masterlist.classList.remove('fa-spinner');
                    genSpinner_load_masterlist.classList.remove('fa-pulse');
                    document.getElementById("loadpanel_masterlist").style.display = "none";
                    ////debugger;
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success",
                        autoHideDelay: 10000
                    });
                }

                function onerror_masterlistupdate(response) {
                    ////debugger;
                    //$.notify("Unable to refresh Item MasterList Table, Please try again!", {
                    //    globalPosition: "top center",
                    //    className: "warn"
                    //});
                }


            }
            function onerror_currconv(response) {
                ////debugger;
                //$.notify("Unable to refresh Currency Conversion Table, Please try again!", {
                //    globalPosition: "top center",
                //    className: "warn"
                //});
            }


        },
        error: function (data) {
            //$.notify("Unable to update, Please try again!", {
            //    globalPosition: "top center",
            //    className: "warn"
            //});

        }
    })
}





$('#btnApproveAll').click(function () {
    SHApprove(1999999999, filtered_yr);
});



function SendBack(id, filtered_yr) {
    if (confirm('Are You Sure to Send Back this Request Record ?')) {
        var genSpinner = document.querySelector("#SubmitSpinner");


        $.ajax({
            type: "POST",
            url: encodeURI("../BudgetingVKM/Sendback"),
            data: { 'id': id, 'useryear': filtered_yr },
            success: function (data) {

                $.ajax({
                    type: "GET",
                    url: "/BudgetingVKM/GetDeptSummaryData",
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




                ////debugger;
                $.ajax({
                    type: "GET",
                    url: encodeURI("../BudgetingVKM/GetBUSummaryData"),
                    data: { 'year': filtered_yr },
                    success: success_refresh_busummary,
                    error: error_refresh_busummary
                });
                function success_refresh_busummary(response) {
                    ////debugger;
                    ////debugger;
                    $("#BUSummaryTable").dxDataGrid({ dataSource: eval(response.data.data) });
                }
                function error_refresh_busummary(response) {
                    //$.notify('Unable to Refresh BU Summary right now, Please Try again later!', {
                    //    globalPosition: "top center",
                    //    className: "warn"
                    //});
                }

                chart();
                ////debugger;
                $.ajax({
                    type: "GET",
                    url: encodeURI("../BudgetingVKM/GetSectionSummaryData"),
                    data: { 'year': filtered_yr },
                    success: success_refresh_sectionsummary,
                    error: error_refresh_sectionsummary
                });
                function success_refresh_sectionsummary(data) {

                    var dataObjData_section1 = eval(data.data.data);

                    $("#sectionsummarytable").dxDataGrid({ dataSource: dataObjData_section1 });
                }
                function error_refresh_sectionsummary(response) {
                    //$.notify('Unable to Refresh Section Summary right now, Please Try again later!', {
                    //globalPosition: "top center",
                    //className: "warn"
                    //});
                }


                $.ajax({
                    type: "GET",
                    url: "/BudgetingVKM/GetData",
                    data: { 'year': filtered_yr },
                    datatype: "json",
                    async: true,
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {

                    var getdata = response.data;
                    $("#RequestTable_VKMSPOC").dxDataGrid({
                        dataSource: getdata
                    });
                }
                function error_refresh_getdata(response) {

                    //$.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                    //globalPosition: "top center",
                    //className: "warn"
                    //});

                }

                if (data.success) {

                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success",
                        autoHideDelay: 13000,
                    })

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
                            autoHideDelay: 3000,
                        })

                    }
                    function error_email(response) {
                        //$.notify("Unable to send mail to the Requestor on the sent back item!", {
                        //    globalPosition: "top center",
                        //    className: "warn"
                        //})

                    }
                }
                else {
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "error",
                        autoHideDelay: 13000
                    })
                }





            }

        });

    }
}


function SHApprove(id, filtered_yr) {
    debugger;
    var genSpinner = document.querySelector("#SubmitSpinner");
    if (confirm('Do you confirm to move this Request Record To Purchase Queue?')) {
        debugger;
        if (id == 1999999999) {
            genSpinner.classList.add('fa');
            genSpinner.classList.add('fa-spinner');
            genSpinner.classList.add('fa-pulse');
        }
        $.ajax({
            type: "POST",
            url: encodeURI("../BudgetingVKM/SHApprove"),
            data: { 'id': id, 'useryear': filtered_yr },
            success: function (data) {

                if (id == 1999999999) {

                    genSpinner.classList.remove('fa');
                    genSpinner.classList.remove('fa-spinner');
                    genSpinner.classList.remove('fa-pulse');
                }
                ////debugger;
                newobjdata = data.data;
                $("#RequestTable_VKMSPOC").dxDataGrid({ dataSource: newobjdata });


                $.ajax({
                    type: "GET",
                    url: "/BudgetingVKM/GetDeptSummaryData",
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
                    url: encodeURI("../BudgetingVKM/GetBUSummaryData"),
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

                chart();

                $.ajax({
                    type: "GET",
                    url: encodeURI("../BudgetingVKM/GetSectionSummaryData"),
                    data: { 'year': filtered_yr },
                    success: success_refresh_sectionsummary,
                    error: error_refresh_sectionsummary
                });
                function success_refresh_sectionsummary(data) {

                    var dataObjData_section1 = eval(data.data.data);

                    $("#sectionsummarytable").dxDataGrid({ dataSource: dataObjData_section1 });
                }
                function error_refresh_sectionsummary(response) {
                    //$.notify('Unable to Refresh Section Summary right now, Please Try again later!', {
                    //    globalPosition: "top center",
                    //    className: "warn"
                    //});
                }

                if (data.success) {
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success",
                        autoHideDelay: 13000
                    })

                }
                else {
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "error",
                        autoHideDelay: 13000
                    })
                }

                if (data.email) {
                    debugger;
                    if (data.email_message != "") { //if not empty, mail has been sent successfully
                        $.notify("Mail has been sent to the ELO Team to proceed for Item Procurement!", {
                            globalPosition: "top center",
                            className: "success",
                            autoHideDelay: 5000,
                        })
                    }
                    //commented since vkm spoc submit mail is handled in controller iteslf directly, since passing Required Date from VKMController to JS in ReqList was in timestamp and sending it back to BudgetingController was null
                    //$.ajax({
                    //    type: "POST",
                    //    url: encodeURI("../Budgeting/SendEmail"),
                    //    data: { 'emailnotify': data.data1 },
                    //    success: success_email,
                    //    error: error_email
                    //});

                    //function success_email(response) {
                    //    $.notify("Mail has been sent to the ELO Team to proceed for Item Procurement!", {
                    //        globalPosition: "top center",
                    //        className: "success",
                    //        autoHideDelay: 3000,
                    //    })

                    //}

                    //function error_email(response) {
                    //    $.notify("Unable to send mail to the Requestor on the sent back item!", {
                    //        globalPosition: "top center",
                    //        className: "warn"
                    //    })

                    //}
                }




            }

        });

    }
}


//function SHApprove(id, filtered_yr) {
//    ////debugger;
//    var genSpinner = document.querySelector("#SubmitSpinner");
//    if (confirm('Do you confirm to move this Request Record To Purchase Queue?')) {
//        if (id == 1999999999) {
//            genSpinner.classList.add('fa');
//            genSpinner.classList.add('fa-spinner');
//            genSpinner.classList.add('fa-pulse');
//        }
//        $.ajax({
//            type: "POST",
//            url: encodeURI("../BudgetingVKM/SHApprove"),
//            data: { 'id': id, 'useryear': filtered_yr },
//            success: function (data) {

//                if (id == 1999999999) {

//                    genSpinner.classList.remove('fa');
//                    genSpinner.classList.remove('fa-spinner');
//                    genSpinner.classList.remove('fa-pulse');
//                }
//                ////debugger;
//                newobjdata = data.data;
//                $("#RequestTable_VKMSPOC").dxDataGrid({ dataSource: newobjdata });


//                $.ajax({
//                    type: "GET",
//                    url: "/BudgetingVKM/GetDeptSummaryData",
//                    data: { 'year': filtered_yr },
//                    datatype: "json",
//                    async: true,
//                    success: success_refresh_deptsummary,
//                    error: error_refresh_deptsummary

//                });
//                function success_refresh_deptsummary(response) {

//                    var deptsummary = eval(response.data.data);
//                    $("#deptsummarytable").dxDataGrid({
//                        dataSource: deptsummary
//                    });
//                }

//                function error_refresh_deptsummary(response) {

//                    //$.notify('Unable to Refresh Dept Summary right now, Please Try again later!', {
//                    //    globalPosition: "top center",
//                    //    className: "warn"
//                    //});

//                }


//                $.ajax({
//                    type: "GET",
//                    url: encodeURI("../BudgetingVKM/GetBUSummaryData"),
//                    data: { 'year': filtered_yr },
//                    success: success_refresh_busummary,
//                    error: error_refresh_busummary
//                });
//                function success_refresh_busummary(response) {

//                    $("#BUSummaryTable").dxDataGrid({ dataSource: eval(response.data.data) });
//                }
//                function error_refresh_busummary(response) {
//                    //$.notify('Unable to Refresh BU Summary right now, Please Try again later!', {
//                    //    globalPosition: "top center",
//                    //    className: "warn"
//                    //});

//                }

//                chart();

//                $.ajax({
//                    type: "GET",
//                    url: encodeURI("../BudgetingVKM/GetSectionSummaryData"),
//                    data: { 'year': filtered_yr },
//                    success: success_refresh_sectionsummary,
//                    error: error_refresh_sectionsummary
//                });
//                function success_refresh_sectionsummary(data) {

//                    var dataObjData_section1 = eval(data.data.data);

//                    $("#sectionsummarytable").dxDataGrid({ dataSource: dataObjData_section1 });
//                }
//                function error_refresh_sectionsummary(response) {
//                    //$.notify('Unable to Refresh Section Summary right now, Please Try again later!', {
//                    //    globalPosition: "top center",
//                    //    className: "warn"
//                    //});
//                }

//                if (data.success) {
//                    $.notify(data.message, {
//                        globalPosition: "top center",
//                        className: "success",
//                        autoHideDelay: 13000
//                    })

//                }
//                else {
//                    $.notify(data.message, {
//                        globalPosition: "top center",
//                        className: "error",
//                        autoHideDelay: 13000
//                    })
//                }

//                if (data.email) {

                    
//                    $.ajax({
//                        type: "POST",
//                        url: encodeURI("../Budgeting/SendEmail"),
//                        data: { 'emailnotify': data.data1 },
//                        success: success_email,
//                        error: error_email
//                    });

//                    function success_email(response) {
//                        $.notify("Mail has been sent to the ELO Team to proceed for Item Procurement!", {
//                            globalPosition: "top center",
//                            className: "success",
//                            autoHideDelay: 3000,
//                        })

//                    }

//                    function error_email(response) {
//                        //$.notify("Unable to send mail to the Requestor on the sent back item!", {
//                        //    globalPosition: "top center",
//                        //    className: "warn"
//                        //})

//                    }
//                }
                



//            }

//        });

//    }
//}



function Update(id1, filtered_yr, from) {
    //debugger;
    if (from == '') {
        id1[0].SRAwardedDate = id1[0].SRAwardedDate != "" ? convert(id1[0].SRAwardedDate) : null;
        id1[0].SRSubmitted = id1[0].SRSubmitted != "" ? convert(id1[0].SRSubmitted) : null;
        id1[0].OrderDate = id1[0].OrderDate != "" ? convert(id1[0].OrderDate) : null;
        id1[0].ActualDeliveryDate = id1[0].ActualDeliveryDate != "" ? convert(id1[0].ActualDeliveryDate) : null;
        id1[0].TentativeDeliveryDate = id1[0].TentativeDeliveryDate != "" ? convert(id1[0].TentativeDeliveryDate) : null;
    }
    $.ajax({
        type: "POST",
        url: encodeURI("../BudgetingVKM/AddOrEdit"),
        data: { 'req': id1[0], 'useryear': filtered_yr },
        success: function (data) {

            $.ajax({
                type: "GET",
                url: "/BudgetingVKM/GetData",
                data: { 'year': filtered_yr },
                datatype: "json",
                async: true,
                success: success_refresh_getdata,
                error: error_refresh_getdata

            });

            function success_refresh_getdata(response) {
                var getdata_parent = (response.data.filter(item => item.LinkedRequestID == ""));

                ChildList = (response.data.filter(item => item.LinkedRequestID != ""));
                $("#RequestTable_VKMSPOC").dxDataGrid({
                    dataSource: getdata_parent
                });
            }
            function error_refresh_getdata(response) {

                //$.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                //    globalPosition: "top center",
                //    className: "warn"
                //});

            }
            if (file != undefined) {
                var formdata = new FormData();
                for (var i = 0; i < file.length; i++) {
                    formdata.append(file[i].name, file[i]);
                }
                formdata.append("id", data.RequestID);
                //formdata.append("image", ofile);
                //formdata.append("id", editRowID == undefined ? 0 : editRowID);
                //var x = new FormData(file[0]);
                //debugger;
                $.ajax({
                    type: "POST",
                    //dataType: "json",
                    //contentType: "multipart/form-data", //false,//"application/json; charset=utf-8;",
                    url: encodeURI("../BudgetingVKM/AsyncFileUpload"),
                    //data: //JSON.stringify({ formdata: ofile.name, id: "2" }),
                    //{ 'formdata': file, 'id': "2" },
                    data: formdata, //{ 'formdata': formdata, id: "2" },
                    cache: false,
                    contentType: false,
                    processData: false,
                    //if success, data gets refreshed internally
                    success: function (data) {
                        //debugger;
                        //InvAuth = false;
                        $.notify("File uploaded!!", {
                            globalPosition: "top center",
                            className: "success"
                        })
                        document.getElementById("loadpanel_uploadFiles").style.display = "none";
                        file = null;
                    },
                    error: function (data) {
                        //InvAuth = false;
                        $.notify("file error!!", {
                            globalPosition: "top center",
                            className: "error"
                        })
                        //debugger;
                    }
                });

            }
            //debugger;
            if (data.success) {

                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "success"
                })
                //debugger;


            }
            else {
                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "error"
                })
            }

            if (data.is_MailTrigger) {
                ////debugger;

                $.ajax({
                    type: "POST",
                    url: encodeURI("../Budgeting/SendEmail_Order"),
                    data: { 'emailnotify': data.data },
                    success: success_email,
                    error: error_email
                });

                function success_email(response) {
                    if (response.message == "Unplanned F02") {
                        $.notify("Mail has been sent to the VKM SPOC about the Unplanned F02 item!", {
                            globalPosition: "top center",
                            className: "success",
                            autoHideDelay: 3000
                        })
                    }
                    else {
                        $.notify("Mail has been sent to the Requestor about the Item Order Status!", {
                            globalPosition: "top center",
                            className: "success",
                            autoHideDelay: 3000,
                        })
                    }


                }
                function error_email(response) {
                    //$.notify("Unable to send mail to the Requestor about the Item Order Status!", {
                    //    globalPosition: "top center",
                    //    className: "warn"
                    //})

                }
            }


            $.ajax({
                type: "GET",
                url: "/BudgetingVKM/GetDeptSummaryData",
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

            ////debugger;
            $.ajax({
                type: "GET",
                url: encodeURI("../BudgetingVKM/GetBUSummaryData"),
                data: { 'year': filtered_yr },
                success: success_refresh_busummary,
                error: error_refresh_busummary
            });
            function success_refresh_busummary(response) {
                ////debugger;
                $("#BUSummaryTable").dxDataGrid({ dataSource: eval(response.data.data) });
            }
            function error_refresh_busummary(response) {
                //$.notify('Unable to Refresh BU Summary right now, Please Try again later!', {
                //globalPosition: "top center",
                //className: "warn"
                //});
            }

            chart();

            $.ajax({
                type: "GET",
                url: encodeURI("../BudgetingVKM/GetSectionSummaryData"),
                data: { 'year': filtered_yr },
                success: success_refresh_sectionsummary,
                error: error_refresh_sectionsummary
            });
            function success_refresh_sectionsummary(data) {

                var dataObjData_section1 = eval(data.data.data);

                $("#sectionsummarytable").dxDataGrid({ dataSource: dataObjData_section1 });
            }
            function error_refresh_sectionsummary(response) {
                //$.notify('Unable to Refresh Section Summary right now, Please Try again later!', {
                //globalPosition: "top center",
                //className: "warn"
                //});
            }


            ////debugger;


        }

    });

}

function Delete(id, filtered_yr) {

    $.ajax({
        type: "POST",
        url: "/BudgetingVKM/Delete",
        data: { 'id': id, 'useryear': filtered_yr },
        success: function (data) {
            newobjdata = data.data;
            $("#RequestTable_VKMSPOC").dxDataGrid({ dataSource: newobjdata });



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

$('[data-toggle="tooltip"]').tooltip();


$("#buttonClearFilters").dxButton({
    text: 'Clear Filters',
    onClick: function () {
        $("#RequestTable_VKMSPOC").dxDataGrid("clearFilter");
    }
});


//BULookup,OEMLookup,DeptLookup,GroupLookup,ItemNameLookup,CostElementLookup,CategoryLookup

//debugger;

$.ajax({
    type: "GET",
    url: encodeURI("../BudgetingVKM/InitRowValues"),
    success: OnSuccessCall_dnew,
    error: OnErrorCall_dnew

});
function OnSuccessCall_dnew(response) {
    ////debugger;
    new_request = response.data;

}
function OnErrorCall_dnew(response) {

    //$.notify('Unable to fill the initial values!', {
    //    globalPosition: "top center",
    //    className: "warn"
    //});
}


//$(function () {
//    // run the currently selected effect
//    function runEffect() {
//         // get effect type from
//        var selectedEffect = "blind";

//        var options = {};

//        // Run the effect
//        $("#effect").show(selectedEffect, options, 1000, callback);
//    };


//    function callback() {
//        setTimeout(function () {
//        $("#effect:visible").removeAttr("style").fadeOut();
//        }, 30000);
//    };

//    // Set effect from select menu value
//    $("#btn_summary").on("click", function () {
//        runEffect();
//    });


//    $("#effect").hide();


//});
$("#btn_summary").on("click", function () {
    ////debugger;
    var e = document.getElementById("effect");
    ////debugger;
    if (e.style.display == 'block')
        e.style.display = 'none';
    else
        e.style.display = 'block';
});

$("#btnCancel").click(function () {
    IsVKMSpoc();
});


$("#btnOk").click(function () {
    var genSpinner = document.querySelector("#OkSpinner");

    genSpinner.classList.add('fa');
    genSpinner.classList.add('fa-spinner');
    genSpinner.classList.add('fa-pulse');

    //debugger;
    var days = $("#days").dxNumberBox('instance').option('value');
    var hours = $("#hours").dxNumberBox('instance').option('value');
    var minutes = $("#minutes").dxNumberBox('instance').option('value');



    $.ajax({

        type: "POST",
        url: "/BudgetingVKM/UpdateTimeline",
        data: { 'Days': days, 'Hours': hours, 'Minutes': minutes },
        datatype: "json",
        async: true,
        success: function (data) {
            //debugger;
            if (data.data == "1") {
                $.notify('Timeline updated!', {

                    globalPosition: "top center",
                    className: "success",
                    autoHideDelay: 8000
                });
            }
        },
        error: function (data) {
            $.notify('Error in updating timeline!', {
                globalPosition: "top center",
                className: "success",
                autoHideDelay: 8000
            });
        }
    });



    genSpinner.classList.remove('fa');
    genSpinner.classList.remove('fa-spinner');
    genSpinner.classList.remove('fa-pulse');
});







//Export all data
$("#export").click(function () {
    var genSpinner = document.querySelector("#ExportSpinner");

    genSpinner.classList.add('fa');
    genSpinner.classList.add('fa-spinner');
    genSpinner.classList.add('fa-pulse');





    $.ajax({

        type: "POST",
        url: "/BudgetingVKM/ExportDataToExcel/",
        data: { 'useryear': filtered_yr },


        success: function (export_result) {

            var bytes = new Uint8Array(export_result.FileContents);
            var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = export_result.FileDownloadName;
            link.click();

            genSpinner.classList.remove('fa');
            genSpinner.classList.remove('fa-spinner');
            genSpinner.classList.remove('fa-pulse');

            $.notify('HOE Reviewed list is exported to an excel sheet. Please Open/Save to view the data!', {
                globalPosition: "top center",
                className: "success",
                autoHideDelay: 8000
            });

        },
        error: function () {
            alert("Error in Export. Please check again!");
        }

    });
});


$("#exportall").click(function () {
    ////debugger;
    var genSpinner = document.querySelector("#ExportallSpinner");

    genSpinner.classList.add('fa');
    genSpinner.classList.add('fa-spinner');
    genSpinner.classList.add('fa-pulse');
    ////debugger;
    $.ajax({

        type: "POST",
        url: "/BudgetingVKM/ExportDataToExcelAll/",
        data: { 'useryear': filtered_yr },


        success: function (export_result) {
            ////debugger;
            var bytes = new Uint8Array(export_result.FileContents);
            var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = export_result.FileDownloadName /*+ ".xlsx"*/;
            link.click();

            genSpinner.classList.remove('fa');
            genSpinner.classList.remove('fa-spinner');
            genSpinner.classList.remove('fa-pulse');
            $.notify('The Item Request list is exported to an excel sheet. Please Open/Save to view the data!', {
                globalPosition: "top center",
                className: "success",
                autoHideDelay: 8000
            });

        },
        error: function () {
            alert("Error in export. Please try again!");
        }

    });
});



var rdata;//this variable needs to be named the same as the parameter in the function call specified for the AjaxOptions.OnSuccess

function mySuccessFuntion(rdata) {
    //debugger;

    if (rdata.success) {
        $.ajax({

            type: "GET",
            url: "/BudgetingVKM/LookupVKM",
            async: false,
            data: { 'year': filtered_yr },
            success: onsuccess_lookupdata1,
            error: onerror_lookupdata1
        })
        if (rdata.errormsg.substr(0, 8) == "") {
            ////debugger;
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

        ////debugger;



        function onsuccess_lookupdata1(response) {
            ////debugger;
            lookup_data = response.data;
            BU_list = lookup_data.BU_List;
            OEM_list = lookup_data.OEM_List;
            DEPT_list = lookup_data.DEPT_List;
            //if (lookup_data.Groups_List != null)
            //    Group_list = lookup_data.Groups_List;
            //else
            Group_list = lookup_data.Groups_test;//Groups_oldList;
            
            Category_list = lookup_data.Category_List;
            CostElement_list = lookup_data.CostElement_List;
            OrderStatus_list = lookup_data.OrderStatus_List;
            Fund_list = lookup_data.Fund_List;
            Currency_list = lookup_data.Currency_List;
            BudgetCodeList = lookup_data.BudgetCodeList;
            SRBuyer_list = lookup_data.SRBuyerList;
            SRManager_list = lookup_data.SRManagerList;
            ////debugger;
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../BudgetingOrder/Lookup_ItemList",
                async: false,
                //data: { 'year': filtered_yr },
                data: JSON.stringify({ year: filtered_yr }),
                dataType: 'json',
                success: function (data) {
                    //debugger;
                    Item_list = data;
                },
                error: function (jqXHR, exception) {
                    //debugger;
                }

            })
        }
        function onerror_lookupdata1(response) {
            ////debugger;
            if (response.errormsg) {
                $.notify(response.errormsg, {
                    globalPosition: "top center",
                    className: "error",
                    autoHideDelay: 15000,
                });
            }
            //else {
            //    alert("Error in fetching lookup");

            //}

        }
    }
    else if (rdata.success == false) {
        ////debugger;
        $.notify(rdata.errormsg, {
            globalPosition: "top center",
            className: "warn"
        });
    }
    else {

    }
    $.ajax({
        type: "GET",
        url: "/BudgetingVKM/GetData",
        data: { 'year': filtered_yr },
        datatype: "json",
        async: true,
        success: success_refresh_getdata,
        error: error_refresh_getdata

    });

    function success_refresh_getdata(response) {

        var getdata = response.data;
        $("#RequestTable_VKMSPOC").dxDataGrid({
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
    //   ////debugger;
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

window.addEventListener("submit", function (e) {
    ////debugger;
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

function editCellTemplate(cellElement, cellInfo) {
    //debugger;
    editRowID = cellInfo.data.ID;
    //let buttonElement = document.createElement("div");
    //buttonElement.classList.add("retryButton");



    //let imageElement = document.createElement("img");
    //imageElement.classList.add("uploadedImage");
    //imageElement.setAttribute('src', `${backendURL}${cellInfo.value}`);



    let fileUploaderElement = document.createElement("div");


    //cellElement.append(imageElement);
    cellElement.append(fileUploaderElement);
    //cellElement.append(buttonElement);



    //let retryButton = $(buttonElement).dxButton({
    //    text: "Retry",
    //    visible: false,
    //    onClick: function () {
    //        // The retry UI/API is not implemented. Use a private API as shown at T611719.
    //        for (var i = 0; i < fileUploader._files.length; i++) {
    //            delete fileUploader._files[i].uploadStarted;
    //        }
    //        fileUploader.upload();
    //    }
    //}).dxButton("instance");



    //function updateQueryStringParameter(uri, key, value) {
    //    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    //    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    //    if (uri.match(re)) {
    //        return uri.replace(re, '$1' + key + "=" + value + '$2');
    //    }
    //    else {
    //        return uri + separator + key + "=" + value;
    //    }
    //}



    let fileUploader = $(fileUploaderElement).dxFileUploader({
        name: "file",
        multiple: true,
        accept: "*",
        uploadMode: "useForm",
        //uploadUrl: `${backendURL}AsyncFileUpload`,
        onValueChanged: function (e) {
            //debugger;

            //file = e.value;
            ////debugger;

            var uploadedfilename = document.querySelector('.dx-fileuploader-files-container');



            for (var i = 0; i < e.value.length; i++) {





                //if (e.value[i].name.indexOf("Quot") != -1 || e.value[i].name.indexOf("Single Source Justification") != -1) {

                file = e.value;


                if (uploadedfilename != undefined) {

                    uploadedfilename.style.visibility = 'visible';

                    uploadedfilename.style.height = "100px";
                    uploadedfilename.style.overflow = "auto";
                    uploadedfilename.style.paddingTop = "0px";

                }

                //}

                //else {

                //    file = null;

                //    alert('Invalid file');

                //    if (uploadedfilename != undefined) {

                //        uploadedfilename.style.visibility = 'hidden';

                //        uploadedfilename.style.height = "0px";

                //        uploadedfilename.style.paddingTop = "0px";

                //    }

                //}

            }


        },
        onUploaded: function (e) {
            //debugger;
            cellInfo.setValue("images/employees/" + e.request.responseText);
            retryButton.option("visible", false);
        },
        onUploadError: function (e) {
            //debugger;
            let xhttp = e.request;
            if (xhttp.status === 400) {
                e.message = e.error.responseText;
            }
            if (xhttp.readyState === 4 && xhttp.status === 0) {
                e.message = "Connection refused";
            }
            //retryButton.option("visible", true);
        }
    }).dxFileUploader("instance");
}

//**********DEPT MAPPING DETAILS************


//Old vs New Dept list where in the Lab Team can change the dept to latest for the apprvd list directly 


//$.ajax({
//    type: "GET",
//    url: encodeURI("../BudgetingVKM/GetData_DeptDetails"),
//    success: OnSuccess_GetData_DeptDetails,
//    error: OnError_GetData_DeptDetails
//});
////debugger;


//function OnSuccess_GetData_DeptDetails(response) {
//   ////debugger;
//    var objdata = (response.data);

//    dataGridLEP_DeptDetails = $("#RequestTable_VKMSPOC_DeptDetails").dxDataGrid({

//        dataSource: objdata,
//        editing: {
//            mode: "row",
//            allowUpdating: true,
//            useIcons: true
//        },

//        allowColumnReordering: true,
//        allowColumnResizing: true,
//        columnChooser: {
//            enabled: true
//        },
//        filterRow: {
//            visible: true

//        },
//        showBorders: true,
//        headerFilter: {
//            visible: true,
//            applyFilter: "auto"
//        },
//        selection: {
//            applyFilter: "auto"
//        },
//        loadPanel: {
//            enabled: true
//        },
//        paging: {
//            pageSize: 15
//        },
//        searchPanel: {
//            visible: true,
//            width: 240,
//            placeholder: "Search..."
//        },


//        columns: [
//            {
//                type: "buttons",
//                width: 90,
//                alignment: "left",
//                buttons: ["edit", "delete"
//                    //{
//                    //    hint: "Edit Dept",
//                    //    icon: "fa fa-edit",

//                    //    onClick: function (e) {
//                    //        prev_orderingdept = e.row.data.OrderingDept;
//                    //        e.component.refresh(true);
//                    //        e.event.preventDefault();
//                    //    }
//                    //},"delete"
//                ]
//            },
//            {

//                alignment: "center",
//                columns: [
//                    {
//                        caption: "DEPARTMENT DETAILS",
//                        alignment: "center",
//                        columns: [
//                            {
//                                dataField: "PlanningDept",
//                                validationRules: [{ type: "required" }],
//                                setCellValue: function (rowData, value) {
//                                   ////debugger;
//                                    rowData.PlanningDEPT = value;
//                                },

//                                lookup: {
//                                    dataSource: function (options) {
//                                       ////debugger;
//                                        return {

//                                            store: DEPT_list,
//                                            filter: options.data ? ["Outdated", "=", true] : null


//                                        };
//                                    }, 

//                                    valueExpr: "ID",
//                                    displayExpr: "DEPT"

//                                },
//                                allowEditing: false


//                            },
//                            {
//                                dataField: "OrderingDept",
//                                validationRules: [{ type: "required" }],
//                                //setCellValue: function (rowData, value) {
//                                //   ////debugger;
//                                //    rowData.Department = value;
//                                //    rowData.Group = null;

//                                //},

//                                lookup: {
//                                    dataSource: function (options) {
//                                       ////debugger;
//                                        return {

//                                            store: DEPT_list


//                                        };
//                                    },

//                                    valueExpr: "ID",
//                                    displayExpr: "DEPT"

//                                },
//                                allowEditing: true


//                            },

//                            //{
//                            //    dataField: "Ordering Stage Department",
//                            //    validationRules: [{ type: "required" }],
//                            //    setCellValue: function (rowData, value) {
//                            //       ////debugger;
//                            //        rowData.Department = value;
//                            //        rowData.Group = null;

//                            //    },

//                            //    lookup: {
//                            //        dataSource: function (options) {
//                            //           ////debugger;
//                            //            return {

//                            //                store: DEPT_list
//                            //            };
//                            //        },

//                            //        valueExpr: "ID",
//                            //        displayExpr: "DEPT"

//                            //    },
//                            //    allowEditing: false


//                            //},


//                            //{
//                            //    dataField: "Group",

//                            //    validationRules: [{ type: "required" }],

//                            //    setCellValue: function (rowData, value) {
//                            //       ////debugger;

//                            //        rowData.Group = value;

//                            //    },
//                            //    lookup: {
//                            //        dataSource: function (options) {
//                            //           ////debugger;
//                            //            return {

//                            //                store: Group_list,

//                            //                filter: options.data ? ["Dept", "=", options.data.Department] : null
//                            //            };

//                            //        },
//                            //        valueExpr: "ID",
//                            //        displayExpr: "Group"
//                            //    },
//                            //    allowEditing: true

//                            //},

//                            {
//                                dataField: "Updated_By",
//                                allowEditing: false
//                            }
//                        ],



//                    }],
//            }],



//        onRowUpdating: function (e) {
//            prev_orderingDept = e.oldData.OrderingDept;
//        },
//        onRowUpdated: function (e) {
//            $.notify(" The Details are being Updated...Please wait!", {
//                globalPosition: "top center",
//                className: "success"
//            })
//            Selected = [];

//            Selected.push(e.data);
//           ////debugger;
//            Update_DeptDetails(Selected);

//            //}

//        }



//    });
//   ////debugger;



//}

//function OnError_GetData_DeptDetails(data) {
//   ////debugger;
//    $("#RequestTable_VKMSPOC_DeptDetails").prop('hidden', false);
//    $.notify(data.message, {
//        globalPosition: "top center",
//        className: "warn"
//    })
//}



//function Update_DeptDetails(id1) {
//   ////debugger;
//    //Onclick of edit - get the current ordering dept data store in a variable - pass this to update fn  (onclick of edit button - get innerhtml)
//    var FromDept, ToDept;
//   ////debugger;
//    $.ajax({
//        type: "GET",
//        url: encodeURI("../BudgetingVKM/DeptID_toName"),
//        data: { 'DeptID': prev_orderingDept },
//        async: false,
//        success: function (data) {
//            FromDept = data.data;
//           ////debugger;
//        }
//    })
//    $.ajax({
//        type: "GET",
//        url: encodeURI("../BudgetingVKM/DeptID_toName"),
//        data: { 'DeptID': id1[0].OrderingDept },
//        async: false,
//        success: function (data) {
//            ToDept = data.data;
//           ////debugger;
//        }
//    })
//   ////debugger;
//    if (confirm('Do you confirm to update Dept: ' + FromDept + ' to ' + ToDept + ' in Reviewed List? ')) {
//        $.ajax({
//            type: "POST",
//            url: encodeURI("../BudgetingVKM/AddOrEdit_DeptDetails"),
//            data: { 'req': id1[0] },
//            success: function (data) {
//               ////debugger;
//                //newobjdata = data.data;

//                //$("#RequestTable_VKMSPOC").dxDataGrid({dataSource: newobjdata });
//                $.ajax({
//                    type: "GET",
//                    url: "/BudgetingVKM/GetData_DeptDetails",
//                    datatype: "json",
//                    async: true,
//                    success: success_refresh_getdataDeptDetails,
//                    error: error_refresh_getdataDeptDetails

//                });

//                function success_refresh_getdataDeptDetails(response) {

//                    var getdata = response.data;
//                    $("#RequestTable_VKMSPOC_DeptDetails").dxDataGrid({
//                        dataSource: getdata
//                    });




//                    //ajax call to update the approved list with the changed ordering stage Dept Details
//                    $.notify("Updating the Reviewed Request List with the ordering stage Dept Details changes, Please wait...!", {
//                        globalPosition: "top center",
//                        autoHideDelay: 3000,
//                        className: "info"
//                    });
//                    genSpinner_load_reqlist.classList.add('fa');
//                    genSpinner_load_reqlist.classList.add('fa-spinner');
//                    genSpinner_load_reqlist.classList.add('fa-pulse');
//                    document.getElementById("loadpanel_reqlist").style.display = "block";
//                   ////debugger;
//                    $.ajax({

//                        type: "POST",
//                        url: "/BudgetingVKM/UpdateRequestList_DeptChange",
//                        data: { 'reqitemlist': id1[0], 'prev_orderingDept': prev_orderingDept },
//                        //async: false,
//                        success: onsuccess_DeptChange,
//                        error: onerror_DeptChange
//                    })

//                    function onsuccess_DeptChange(response) {
//                       ////debugger;

//                        $.ajax({
//                            type: "GET",
//                            url: "/BudgetingVKM/GetData",
//                            data: { 'year': filtered_yr },
//                            datatype: "json",
//                            async: true,
//                            success: success_refresh_getdata,
//                            error: error_refresh_getdata

//                        });

//                        function success_refresh_getdata(response) {

//                            var getdata = response.data;
//                            $("#RequestTable_VKMSPOC").dxDataGrid({
//                                dataSource: getdata
//                            });
//                        }
//                        function error_refresh_getdata(response) {

//                            $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
//                                globalPosition: "top center",
//                                className: "warn"
//                            });

//                        }

//                        genSpinner_load_reqlist.classList.remove('fa');
//                        genSpinner_load_reqlist.classList.remove('fa-spinner');
//                        genSpinner_load_reqlist.classList.remove('fa-pulse');
//                        document.getElementById("loadpanel_reqlist").style.display = "none";
//                       ////debugger;
//                        $.notify(response.message, {
//                            globalPosition: "top center",
//                            className: "success"
//                        });
//                    }

//                    function onerror_DeptChange(response) {
//                       ////debugger;
//                        $.notify("Unable to refresh Reviewed Request List, Please try again!", {
//                            globalPosition: "top center",
//                            className: "warn"
//                        });


//                        genSpinner_load_reqlist.classList.remove('fa');
//                        genSpinner_load_reqlist.classList.remove('fa-spinner');
//                        genSpinner_load_reqlist.classList.remove('fa-pulse');
//                        document.getElementById("loadpanel_reqlist").style.display = "none";
//                    }



//                }
//                function error_refresh_getdataDeptDetails(response) {

//                    $.notify('Unable to Refresh Department List right now, Please Try again later!', {
//                        globalPosition: "top center",
//                        className: "warn"
//                    });

//                }



//                if (data.success) {
//                    //$.notify(data.message, {
//                    //    globalPosition: "top center",
//                    //    className: "success"
//                    //})
//                }
//                else {
//                    $.notify(data.message, {
//                        globalPosition: "top center",
//                        className: "error"
//                    })
//                }



//            }

//        });
//    }
//}


//*************GROUP MAPPING DETAILS**********************



////debugger;
//$.ajax({
//    type: "GET",
//    url: encodeURI("../BudgetingVKM/GetData_GroupDetails"),
//    success: OnSuccess_GetData_GroupDetails,
//    error: OnError_GetData_GroupDetails
//});
////debugger;
//function OnSuccess_GetData_GroupDetails(response) {
//   ////debugger;
//    var objdata = (response.data);

//    dataGridLEP_GroupDetails = $("#RequestTable_VKMSPOC_GroupDetails").dxDataGrid({

//        dataSource: objdata,
//        editing: {
//            mode: "row",
//            allowUpdating: true,
//            useIcons: true
//        },

//        allowColumnReordering: true,
//        allowColumnResizing: true,
//        columnChooser: {
//            enabled: true
//        },
//        filterRow: {
//            visible: true

//        },
//        showBorders: true,
//        headerFilter: {
//            visible: true,
//            applyFilter: "auto"
//        },
//        selection: {
//            applyFilter: "auto"
//        },
//        loadPanel: {
//            enabled: true
//        },
//        paging: {
//            pageSize: 15
//        },
//        searchPanel: {
//            visible: true,
//            width: 240,
//            placeholder: "Search..."
//        },


//        columns: [
//            {
//                type: "buttons",
//                width: 90,
//                alignment: "left",
//                buttons: ["edit", "delete"
//                    //{
//                    //    hint: "Edit Group",
//                    //    icon: "fa fa-edit",

//                    //    onClick: function (e) {
//                    //        prev_orderingGroup = e.row.data.OrderingGroup;
//                    //        e.component.refresh(true);
//                    //        e.event.preventDefault();
//                    //    }
//                    //},"delete"
//                ]
//            },
//            {

//                alignment: "center",
//                columns: [
//                    {
//                        caption: "GROUP DETAILS",
//                        alignment: "center",
//                        columns: [
//                            {
//                                dataField: "PlanningGroup",
//                                validationRules: [{ type: "required" }],
//                                setCellValue: function (rowData, value) {
//                                   ////debugger;
//                                    rowData.PlanningGroup = value;
//                                },

//                                lookup: {
//                                    dataSource: function (options) {
//                                       ////debugger;
//                                        return {

//                                            store: Group_list,
//                                            //filter: options.data ? ["Outdated", "=", true] : null


//                                        };
//                                    },

//                                    valueExpr: "ID",
//                                    displayExpr: "Group"

//                                },
//                                allowEditing: false


//                            },
//                            {
//                                dataField: "OrderingGroup",
//                                validationRules: [{ type: "required" }],
//                                //setCellValue: function (rowData, value) {
//                                //   ////debugger;
//                                //    rowData.Department = value;
//                                //    rowData.Group = null;

//                                //},

//                                lookup: {
//                                    dataSource: function (options) {
//                                       ////debugger;
//                                        return {

//                                            store: Group_list


//                                        };
//                                    },

//                                    valueExpr: "ID",
//                                    displayExpr: "Group"

//                                },
//                                allowEditing: true


//                            },

                            //{
                            //    dataField: "Ordering Stage Department",
                            //    validationRules: [{ type: "required" }],
                            //    setCellValue: function (rowData, value) {
                            //       ////debugger;
                            //        rowData.Department = value;
                            //        rowData.Group = null;

                            //    },

                            //    lookup: {
                            //        dataSource: function (options) {
                            //           ////debugger;
                            //            return {

                            //                store: Group_list
                            //            };
                            //        },

                            //        valueExpr: "ID",
                            //        displayExpr: "Group"

                            //    },
                            //    allowEditing: false


                            //},


                            //{
                            //    dataField: "Group",

                            //    validationRules: [{ type: "required" }],

                            //    setCellValue: function (rowData, value) {
                            //       ////debugger;

                            //        rowData.Group = value;

                            //    },
                            //    lookup: {
                            //        dataSource: function (options) {
                            //           ////debugger;
                            //            return {

                            //                store: Group_list,

                            //                filter: options.data ? ["Group", "=", options.data.Department] : null
                            //            };

                            //        },
                            //        valueExpr: "ID",
                            //        displayExpr: "Group"
                            //    },
                            //    allowEditing: true

                            //},

//                            {
//                                dataField: "Updated_By",
//                                allowEditing: false
//                            }
//                        ],



//                    }],
//            }],



//        onRowUpdating: function (e) {
//            prev_orderingGroup = e.oldData.OrderingGroup;
//        },
//        onRowUpdated: function (e) {
//            $.notify(" The Details are being Updated...Please wait!", {
//                globalPosition: "top center",
//                className: "success"
//            })
//            Selected = [];

//            Selected.push(e.data);
//           ////debugger;
//            Update_GroupDetails(Selected);

            //}

//        },


//    });



//}

//function OnError_GetData_GroupDetails(data) {
//   ////debugger;
//    $("#RequestTable_VKMSPOC_GroupDetails").prop('hidden', false);
//    $.notify(data.message, {
//        globalPosition: "top center",
//        className: "warn"
//    })
//}



//function Update_GroupDetails(id1) {
//   ////debugger;
//    //Onclick of edit - get the current ordering Group data store in a variable - pass this to update fn  (onclick of edit button - get innerhtml)
//    var FromGrp, ToGrp;
//   ////debugger;
//    $.ajax({
//        type: "GET",
//        url: encodeURI("../BudgetingVKM/GrpID_toName"),
//        data: { 'GrpID': prev_orderingGroup },
//        async: false,
//        success: function (data) {
//           ////debugger;
//            FromGrp = data.data;
//        }
//    })
//    $.ajax({
//        type: "GET",
//        url: encodeURI("../BudgetingVKM/GrpID_toName"),
//        data: { 'GrpID': id1[0].OrderingGroup },
//        async: false,
//        success: function (data) {
//           ////debugger;
//            ToGrp = data.data;
//        }
//    })
//   ////debugger;
//    if (confirm('Do you confirm to update Group: ' + FromGrp + ' to ' + ToGrp + ' in Reviewed List? ')) {
//        $.ajax({
//            type: "POST",
//            url: encodeURI("../BudgetingVKM/AddOrEdit_GroupDetails"),
//            data: { 'req': id1[0] },
//            success: function (data) {
//               ////debugger;
//                //newobjdata = data.data;

//                //$("#RequestTable_VKMSPOC").dxDataGrid({dataSource: newobjdata });
//                $.ajax({
//                    type: "GET",
//                    url: "/BudgetingVKM/GetData_GroupDetails",
//                    datatype: "json",
//                    async: true,
//                    success: success_refresh_getdataGroupDetails,
//                    error: error_refresh_getdataGroupDetails

//                });

//                function success_refresh_getdataGroupDetails(response) {

//                    var getdata = response.data;
//                    $("#RequestTable_VKMSPOC_GroupDetails").dxDataGrid({
//                        dataSource: getdata
//                    });




//                    //ajax call to update the approved list with the changed ordering stage Group Details
//                    $.notify("Updating the Reviewed Request List with the ordering stage Group Details changes, Please wait...!", {
//                        globalPosition: "top center",
//                        autoHideDelay: 3000,
//                        className: "info"
//                    });
//                    genSpinner_load_reqlist1.classList.add('fa');
//                    genSpinner_load_reqlist1.classList.add('fa-spinner');
//                    genSpinner_load_reqlist1.classList.add('fa-pulse');
//                    document.getElementById("loadpanel_reqlist1").style.display = "block";
//                   ////debugger;
//                    $.ajax({

//                        type: "POST",
//                        url: "/BudgetingVKM/UpdateRequestList_GroupChange",
//                        data: { 'reqitem': id1[0], 'prev_orderingGroup': prev_orderingGroup },
//                        //async: false,
//                        success: onsuccess_GroupChange,
//                        error: onerror_GroupChange
//                    })

//                    function onsuccess_GroupChange(response) {
//                       ////debugger;

//                        $.ajax({
//                            type: "GET",
//                            url: "/BudgetingVKM/GetData",
//                            data: { 'year': filtered_yr },
//                            datatype: "json",
//                            async: true,
//                            success: success_refresh_getdata,
//                            error: error_refresh_getdata

//                        });

//                        function success_refresh_getdata(response) {

//                            var getdata = response.data;
//                            $("#RequestTable_VKMSPOC").dxDataGrid({
//                                dataSource: getdata
//                            });
//                        }
//                        function error_refresh_getdata(response) {

//                            $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
//                                globalPosition: "top center",
//                                className: "warn"
//                            });

//                        }


//                        genSpinner_load_reqlist1.classList.remove('fa');
//                        genSpinner_load_reqlist1.classList.remove('fa-spinner');
//                        genSpinner_load_reqlist1.classList.remove('fa-pulse');
//                        document.getElementById("loadpanel_reqlist1").style.display = "none";
//                       ////debugger;
//                        $.notify(response.message, {
//                            globalPosition: "top center",
//                            className: "success"
//                        });
//                    }

//                    function onerror_GroupChange(response) {
//                       ////debugger;
//                        $.notify("Unable to refresh Reviewed Request List, Please try again!", {
//                            globalPosition: "top center",
//                            className: "warn"
//                        });
//                    }



//                }
//                function error_refresh_getdataGroupDetails(response) {

//                    $.notify('Unable to Refresh Group List right now, Please Try again later!', {
//                        globalPosition: "top center",
//                        className: "warn"
//                    });

//                }



//                if (data.success) {
//                    //$.notify(data.message, {
//                    //    globalPosition: "top center",
//                    //    className: "success"
//                    //})
//                }
//                else {
//                    $.notify(data.message, {
//                        globalPosition: "top center",
//                        className: "error"
//                    })
//                }



//            }

//        });
//   }
//}





