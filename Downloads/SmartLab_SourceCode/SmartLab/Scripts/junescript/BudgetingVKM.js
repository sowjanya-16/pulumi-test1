            //Javascript file for Budgeting VKM Details - mae9cob

            var dataGridLEP, busummarytable, deptsummarytable, sectionsummarytable;
            var BU_list, DEPT_list, Item_list, Category_list, OEM_list, Group_list, CostElement_list, OrderStatus_list, Fund_list, Currency_list;
            var dataObjData, dataObjData_section, newobjdata;
            var Selected = [];
            var unitprice, reviewer_2, category, costelement;
            var lookup_data, filtered_yr, flag;
            var genSpinner_load = document.querySelector("#load");
            var genSpinner_load_masterlist = document.querySelector("#load_masterlist");

            var dataGrid;
            var BU_forItemFilter = 0;

            var is_TwoWPselected = false;
            var is_XCselected = false;
var RevCost = 0;
var is_newitem = false;
var isPurchaseSPOC = false;
            //var prev_orderingDept, prev_orderingGroup;
            //var reqIDarray = [];   

            //var genSpinner_load_reqlist1 = document.querySelector("#load_reqlist1");
            //var genSpinner_load_reqlist = document.querySelector("#load_reqlist");
            

    
$('input[type=checkbox]').each(function () { this.checked = false; });

$.ajax({
    type: "POST",
    url: encodeURI("../Budgeting/is_PurchaseSPOC"),
    async: false,
    success: OnSuccess1
});
function OnSuccess1(data) {
    debugger;
    isPurchaseSPOC = true;
    document.getElementById("btn_summary").style.display = "block"; 
    if (data.success) {
        document.getElementById("VKMTitle").innerHTML = "Purchase SPOC Reviews";
        document.getElementById("VKMTitle").style.fontWeight = "700";
        document.getElementById("VKMTitle").style.fontSize = "45";
        document.getElementById("VKMTitle_icon").style.display = "block"; 
        document.getElementById("btnApproveAll").style.display = "none"; //no Submit All Button view for Purchse SPOC & VKMAdmin
        document.getElementById("btn_summary").style.display = "none"; 
        document.getElementById("POImport").style.display = "block"; 
        $('#RequestTable').css("marginTop", "5px");
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
        document.getElementById("VKMTitle").style.fontWeight = "700";
        document.getElementById("VKMTitle").style.fontSize = "45";
        document.getElementById("VKMTitle").style.display = "block"; 
        document.getElementById("bonaparte").style.display = "none";
        document.getElementById("VKMTitle_icon").style.display = "block"; 
        $('#RequestTable').css("marginTop", "0px");
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
            //$("#RequestTable").prop('hidden', true);

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
//debugger;
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

                if (option.value == (currentYear + 1 )) {
                    //if (option.value == (currentYear - 2)) {
                    option.defaultSelected = true;
                    //option.defaultSelected = true;
                }
                filtered_yr = $("#ddlYears").val();
                filtered_yr = parseInt(filtered_yr) - 1;
                filtered_yr = filtered_yr.toString();
               //debugger;
            }
            //Loop and add the Year values to DropDownList.





//$('#chkRequest').on('click', function () {
//    var chkRequest;
//    if (this.checked)
//        chkRequest = true;
//    else
//        chkRequest = false;
//    var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
//    if ($('.chkvkm:checked').length == 0) {
//        debugger;
//        var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
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
//   // $('#RequestTable').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
//   //// $('#RequestTable').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
//   // $('#RequestTable').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
//   // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
//   // $('#RequestTable').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
//   // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
//   // $('#RequestTable').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
//   // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
//   // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
//   // $('#RequestTable').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
//   // $('#RequestTable').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
//   // $('#RequestTable').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);
   

//});

//$('#chkItem').on('click', function () {
//    var chkItem;
//    if (this.checked)
//        chkItem = true;
//    else
//        chkItem = false;
//    var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
//    if ($('.chkvkm:checked').length == 0) {
//        debugger;
//        var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
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
//    //$('#RequestTable').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
//    //$('#RequestTable').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
//    //$('#RequestTable').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


//});
var chkRequest;
var chkItem;
var chkRFO;
var chkNonVKM;
$('#chkRequest').on('click', function () {
    
    if (this.checked)
        chkRequest = true;
    else
        chkRequest = false;
    checkboxdata();
    // $('#RequestTable').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
    //// $('#RequestTable').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
    // $('#RequestTable').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
    // $('#RequestTable').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
    // $('#RequestTable').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
    // $('#RequestTable').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
    // $('#RequestTable').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
    // $('#RequestTable').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


});

$('#chkItem').on('click', function () {
    
    if (this.checked)
        chkItem = true;
    else
        chkItem = false;
    checkboxdata();
    //$('#RequestTable').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
    //$('#RequestTable').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
    //$('#RequestTable').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


});


$('#chkRFO').on('click', function () {
    
    if (this.checked)
        chkRFO = true;
    else
        chkRFO = false;
    checkboxdata();
    // $('#RequestTable').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO); -- tking more time
    //// $('#RequestTable').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
    // $('#RequestTable').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
    //// $('#RequestTable').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
    // $('#RequestTable').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
    //// $('#RequestTable').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
    // $('#RequestTable').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
    // $('#RequestTable').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);
    // $('#RequestTable').dxDataGrid('columnOption', 'OrderID', 'visible', chkRFO);
    // $('#RequestTable').dxDataGrid('columnOption', 'OrderPrice', 'visible', chkRFO);
    // $('#RequestTable').dxDataGrid('columnOption', 'OrderedQuantity', 'visible', chkRFO);



});

$('#chkNonVKM').on('click', function () {
   
    if (this.checked)
        chkNonVKM = true;
    else
        chkNonVKM = false;
   
    checkboxdata();
    //$('#RequestTable').dxDataGrid('columnOption', 'Customer_Name', 'visible', NonVKM);
    //$('#RequestTable').dxDataGrid('columnOption', 'Customer_Dept', 'visible', NonVKM);
    //$('#RequestTable').dxDataGrid('columnOption', 'BM_Number', 'visible', NonVKM);
    //$('#RequestTable').dxDataGrid('columnOption', 'Task_ID', 'visible', NonVKM);
    //$('#RequestTable').dxDataGrid('columnOption', 'Resource_Group_Id', 'visible', NonVKM);
    //$('#RequestTable').dxDataGrid('columnOption', 'PIF_ID', 'visible', NonVKM);




});

function checkboxdata() {
    if ($('.chkvkm:checked').length == 0) {
        debugger;
        var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
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

        dataGridLEP1.endUpdate();

    }
    else if (('.chkvkm:checked').length == $('.chkvkm').length) {//chk if purchase spoc / vkm spoc
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
        dataGridLEP1.columnOption('Category', 'visible', true);
        dataGridLEP1.columnOption('Cost_Element', 'visible', true);
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
        dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);

        dataGridLEP1.columnOption('Customer_Name', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Customer_Dept', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('BM_Number', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Task_ID', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('PIF_ID', 'visible', chkNonVKM);

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

        dataGridLEP1.columnOption('Category', 'visible', chkItem);
        dataGridLEP1.columnOption('Cost_Element', 'visible', chkItem);
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
        dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);

        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);

        dataGridLEP1.columnOption('Customer_Name', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Customer_Dept', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('BM_Number', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Task_ID', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('PIF_ID', 'visible', chkNonVKM);


        dataGridLEP1.endUpdate();
    }
}
//$('#chkRFO').on('click', function () {
//    var chkRFO;
//    if (this.checked)
//        chkRFO = true;
//    else
//        chkRFO = false;
//    var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
//    if ($('.chkvkm:checked').length == 0) {
//        debugger;
//        var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
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
//   // $('#RequestTable').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO); -- tking more time
//   //// $('#RequestTable').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
//   // $('#RequestTable').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
//   //// $('#RequestTable').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
//   // $('#RequestTable').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
//   //// $('#RequestTable').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
//   // $('#RequestTable').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
//   // $('#RequestTable').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);
//   // $('#RequestTable').dxDataGrid('columnOption', 'OrderID', 'visible', chkRFO);
//   // $('#RequestTable').dxDataGrid('columnOption', 'OrderPrice', 'visible', chkRFO);
//   // $('#RequestTable').dxDataGrid('columnOption', 'OrderedQuantity', 'visible', chkRFO);



//});

//$('#chkNonVKM').on('click', function () {
//    var NonVKM;
//    if (this.checked)
//        NonVKM = true;
//    else
//        NonVKM = false;
//    var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
//    if ($('.chkvkm:checked').length == 0) {
//        debugger;
//        var dataGridLEP1 = $("#RequestTable").dxDataGrid("instance");
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
//    //$('#RequestTable').dxDataGrid('columnOption', 'Customer_Name', 'visible', NonVKM);
//    //$('#RequestTable').dxDataGrid('columnOption', 'Customer_Dept', 'visible', NonVKM);
//    //$('#RequestTable').dxDataGrid('columnOption', 'BM_Number', 'visible', NonVKM);
//    //$('#RequestTable').dxDataGrid('columnOption', 'Task_ID', 'visible', NonVKM);
//    //$('#RequestTable').dxDataGrid('columnOption', 'Resource_Group_Id', 'visible', NonVKM);
//    //$('#RequestTable').dxDataGrid('columnOption', 'PIF_ID', 'visible', NonVKM);




//});


            //At load, Display the data for Current year
            if (filtered_yr == null) {
                filtered_yr = new Date().getFullYear();
            }
            ajaxCallforRequestUI(filtered_yr);


            //Function to change year from dropdown
            function fnYearChange(yearselect) {
                //$("#RequestTable").prop('hidden', true);
                //document.getElementById("loadpanel").style.display = "block";

               // genSpinner_load = document.querySelector("#load");
                //genSpinner_load.classList.add('fa');
                //genSpinner_load.classList.add('fa-spinner');
                //genSpinner_load.classList.add('fa-pulse');
                //document.getElementById("loadpanel").style.display = "block";
                if (parseInt(yearselect.value) == new Date().getFullYear() + 1) { //2023 == 2023
                    $("#btnApproveAll").prop("hidden", false);
                }
                else {
                    $("#btnApproveAll").prop("hidden", true);//2023 < 2022 , 2022 < 2022
                    //$("#effect").prop("hidden", false);
                }

                filtered_yr = parseInt(yearselect.value) -  1;
                
                debugger;
                //if (filtered_yr + 1 != new Date().getFullYear() + 1) {
                //    document.getElementById("btnApproveAll").style.display = "none"; 
                //}
                //else {
                //    document.getElementById("btnApproveAll").style.display = "block"; 
                //}
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

                    lookup_data = response.data;
                    BU_list = lookup_data.BU_List;
                    OEM_list = lookup_data.OEM_List;
                    DEPT_list = lookup_data.DEPT_List;
                    //if (lookup_data.Groups_List != null)
                    //    Group_list = lookup_data.Groups_List;
                    //else
                    Group_list = lookup_data.Groups_test;//Groups_oldList;
                    Item_list = lookup_data.Item_List;
                    Category_list = lookup_data.Category_List;
                    CostElement_list = lookup_data.CostElement_List;
                    OrderStatus_list = lookup_data.OrderStatus_List;
                    Fund_list = lookup_data.Fund_List;
                    Currency_list = lookup_data.Currency_List;
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
               //debugger;
                $.ajax({
                    type: "GET",
                    url: encodeURI("../BudgetingVKM/GetBUSummaryData"),
                    data: { 'year': filtered_yr },
                    success: OnSuccess_GetBUSummary,
                    error: OnError_GetBUSummary
                });

               //debugger;
               
                function OnSuccess_GetBUSummary(response) {
                   //debugger;
                    var objdata = eval(response.data.data);
                   //debugger;

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
               //debugger;
                $.ajax({
                    type: "GET",
                    url: encodeURI("../BudgetingVKM/GetData"),
                    data: { 'year': filtered_yr },
                    success: OnSuccess_GetData,
                    error: OnError_GetData
                });



}

            function chart() {
   //debugger;
    $.ajax({
        type: "GET",
        url: encodeURI("../BudgetingVKM/GetBUSummaryData"),
        data: { 'year': filtered_yr, 'chart': true },
        success: OnSuccess_ChartSummaryData,
        error: OnError_ChartSummaryData
    });
}



            function OnSuccess_ChartSummaryData(data) {
   //debugger;
                dataObj = eval(data.data.data);
                if (data.message!= null && data.message == "") {
                    
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
   //debugger;
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
                
               //debugger;
                
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
                        document.getElementById("VKMTitle").innerHTML = "VKM Admin View";
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
               
                dataGridLEP = $("#RequestTable").dxDataGrid({

                    dataSource: objdata,
                    //wordWrapEnabled: true,
                    noDataText: " ☺ No VKM Item Request is available in your queue !",
                    columnFixing: {
                        enabled: true
                    },
                    width: "97vw", //needed to allow horizontal scroll
                    height: "70vh",
                    columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
                    remoteOperations: true,
                    scrolling: {
                        mode: "virtual",
                        rowRenderingMode: "virtual",
                        columnRenderingMode: "virtual"
                    },
                    //paging: {
                    //    pageSize: 50
                    //},
                    
                    editing: {
                        mode: "row",
                        allowUpdating: function (e) {    //Edit access to labteam when requestortoorder triggered ; //Edit access to vkm spoc when approvedsh != true

                            return (flag && e.row.data.RequestToOrder || (!flag && !e.row.data.ApprovedSH && !admin))//with purchase phase //(!flag && e.row.data.RequestToOrder || (!flag && !e.row.data.ApprovedSH))  --ithout purchase phasse  
                            //true 
                               //if vkm spoc
                            //if vkm admin
                            //if purchase spoc
                        },
                        allowDeleting: function (e) {
                           //debugger;
                            return (flag &&e.row.data.RequestToOrder) && isF03F01(e.row.data.Fund) ; //delete access to remove newly added f03 items by purchase spoc id not needed
                        },
                        allowAdding: true,//since both vkm spoc as well as purchase spoc can add new items
                        
                        useIcons: true
                    },
                    onContentReady: function (e) {
                       //debugger;
                        e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
                    },
                    onCellPrepared: function (e) {
                        debugger;
                        if (e.rowType === "header" || e.rowType === "filter") {
                            e.cellElement.addClass("columnHeaderCSS");
                            e.cellElement.find("input").addClass("columnHeaderCSS");
                        }

                      
                           
                    },
                    //onKeyUp: function (e) {
                    //    debugger;
                    //},
                    //onKeyDown: function (e) {
                    //    debugger;
                    //},
                    focusedRowEnabled: true,
                    allowColumnReordering: true,
                    //allowColumnResizing: true,
                    keyExpr: "RequestID",
                    //columnResizingMode: "widget",
                    columnMinWidth: 50,
                    //stateStoring: {
                    //    enabled: true,
                    //    type: "localStorage",
                    //    storageKey: "RequestID"
                    //},
                  
                    columnChooser: {
                        enabled: true
                    },
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
                    //    debugger;
                    //    if (e.name === "Reviewed_Quantity") {
                    //        // handle the property change here
                    //        debugger;
                    //    }
                    //},
                    //onInput: function (e) {
                    //    debugger;
                    //},
                    onEditorPreparing: function (e) {
                       debugger;
                        var component = e.component,
                            rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

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
                           //debugger;
                            if (e.parentType == "dataRow") {
                               //debugger;
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
                               //debugger;
                                e.editorOptions.disabled = !e.row.isNewRow;
                            }
                            var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                            e.editorOptions.onValueChanged = function (e) {
                                onValueChanged.call(this, e);

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


                                // Emulating a web service call
                                window.setTimeout(function () {
                                  
                                    component.cellValue(rowIndex, "Reviewer_1", reviewer_1);
                                }, 1000);
                            }
                        }

                       

                        if (e.dataField === "Item_Name") {

                            if (e.parentType == "dataRow") {
                               //debugger;
                                e.editorOptions.disabled = !e.row.isNewRow;
                            }
                            var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                            e.editorOptions.onValueChanged = function (e) {
                                onValueChanged.call(this, e);
                                $.ajax({
                                    type: "post",
                                    url: "/BudgetingRequest/GetUnitPrice",
                                    data: { ItemName: e.value },
                                    datatype: "json",
                                    ajax: false,
                                    traditional: true,
                                    success: function (data) {
                                       //debugger;
                                        if (data > 0)
                                            unitprice = data;


                                        var RevQ_sel = component.cellValue(rowIndex, "Reviewed_Quantity");
                                        if (component.cellValue(rowIndex, "Reviewed_Quantity") != undefined && component.cellValue(rowIndex, "Reviewed_Quantity") != null) {
                                           //debugger;
                                            $.ajax({

                                                type: "post",
                                                url: "/BudgetingVKM/GetRevCost",
                                                data: { Reviewed_Quantity: component.cellValue(rowIndex, "Reviewed_Quantity"), Unit_Price: unitprice },
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
                                                        component.cellValue(rowIndex, "Reviewed_Cost", RevCost);
                                                    }, 1000);
                                                    //}

                                                }
                                            })
                                        }
                                    }
                                });

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
                                        //debugger;
                                        costelement = data;

                                    }
                                 })

                               

                                    //// Emulating a web service call

                                window.setTimeout(function () {

                                    component.cellValue(rowIndex, "Unit_Price", unitprice);
                                    component.cellValue(rowIndex, "Category", category);
                                    component.cellValue(rowIndex, "Cost_Element", costelement);

                                },
                                    1000);

                             }

                        }

                        if (e.parentType === "dataRow" && e.dataField === "Fund") {
                         
                            e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund)); //non-editbale if f02 ; editable if f01 or f03

                        }

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

                    columns: [
                    {
                        type: "buttons",
                        width: 100,
                            alignment: "left",
                            fixed: true,
                            fixedPosition: "left",
                        buttons: [
                            "edit",  "delete",
                            {
                                hint: "Submit Item",
                                icon: "check",
                                visible: function (e) {
                                    if (e.row.data.ApprovedSH == undefined)
                                        e.row.data.ApprovedSH = false;
                                    return !flag && !e.row.isEditing && (!flag && !e.row.data.ApprovedSH) && !admin

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
                                    if (e.row.data.ApprovedSH == undefined)
                                        e.row.data.ApprovedSH = false;
                                    return !flag && !e.row.isEditing && (!flag && !e.row.data.ApprovedSH) && !admin//admin no option to update
                                },
                                onClick: function (e) {
                                    SendBack(e.row.data.RequestID, filtered_yr);
                                    e.component.refresh(true);
                                    e.event.preventDefault();
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

                                    dataField: "BU",
                                    validationRules: [{ type: "required" }],
                                    width: 70,
                                    lookup: {
                                        dataSource: BU_list,
                                        valueExpr: "ID",
                                        displayExpr: "BU"
                                    },
                                    allowEditing: true 
                                },

                   //},
                            {
                                dataField: "OEM",
                        width: 100,
                        validationRules: [{ type: "required" }],

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
                                    allowEditing: true,
                                    visible: false
                                      
                                },
                    {
                        dataField: "Item_Name",
                        caption: "Item",
                        minWidth: 300,
                        validationRules: [{ type: "required" }],
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
                                    dataField: "ActualAvailableQuantity",
                                    caption: "Available Qty",
                                    allowEditing: false,
                                    width: 110
                                   

                                },
                    {
                        dataField: "Category",
                        caption: "Category",
                        //validationRules: [{ type: "required" }],

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
                        allowEditing: flag,
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
,                        calculateCellValue: function (rowData) {

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
                            debugger;
                            rowData.Reviewed_Quantity = value;

                        },
                        allowEditing: function (e) {
                           //debugger;
                            if (e.row.data.ApprovedSH == undefined)
                                e.row.data.ApprovedSH = false;
                            return flag || !e.row.data.ApprovedSH  
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
                           debugger;
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
                                    visible: false//true
                                },
                    {
                        dataField: "OrderPrice",
                        visible: false,
                        //calculateCellValue: function (rowData) {
                        //    var orderpriceinusd;

                        //   //debugger;
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
                        //           //debugger;
                        //            orderpriceinusd = response;


                        //        }


                        //        function error_getorder_priceusd(response) {
                        //           //debugger;
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
                           //debugger;
                            if (e.row.data.ApprovedSH == undefined)
                                e.row.data.ApprovedSH = false;
                            return flag || !e.row.data.ApprovedSH
                        },


                    },
                    {
                        dataField: "OrderStatus",
                        visible: flag,

                        setCellValue: function (rowData, value) {

                            rowData.OrderStatus = value;


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
                    {
                        dataField: "RequestOrderDate",
                        dataType: "date",
                        allowEditing: false,
                        visible: false//flag

                    },
                    {
                        dataField: "OrderDate",
                        dataType: "date",
                        allowEditing: true,
                        visible: false//flag

                    },
                    {
                        dataField: "TentativeDeliveryDate",
                        caption : "Tentaive Dt",
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
                                }



                        ]
                        },
                    ],
           
                    //onEditingStart: function (e) {
                    //    debugger;
                    //    if (e.data.Reviewed_Quantity) {
                    //        debugger;
                    //    }

                    //}, 
                    onInitNewRow: function (e) {
                       debugger;
                    is_newitem = true;
                    //e.data.Requestor = new_request.Requestor;
                    //e.data.Reviewer_1 = new_request.Reviewer_1;
                    //e.data.Reviewer_2 = new_request.Reviewer_2;
                    //e.data.DEPT = new_request.DEPT;
                    //    e.data.Group = new_request.Group;


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
                        debugger;
                    $.notify("Item in your Queue is being Updated...Please wait!", {
                        globalPosition: "top center",
                        className: "success"
                    })

           //debugger;
            //if (e.data.OrderedQuantity > e.data.Reviewed_Quantity) {
            //    $.notify("Ordered Quantity cannot be greater than Reviewed Quantity, Please check again!", {
            //        globalPosition: "top center",
            //        className: "error"
            //    })
            //}
            
                
                        if (e.data.OrderPrice > 0 && e.data.Currency != undefined) {

                            $.ajax({

                                type: "GET",
                                url: "/BudgetingVKM/GetOrderPriceinUSD",
                                data: { 'OrderPrice': e.data.OrderPrice, 'Currency': e.data.Currency },
                                datatype: "json",
                                async: false,
                                success: success_getorder_priceusd,
                                error: error_getorder_priceusd

                            });

                            function success_getorder_priceusd(response) {
                               //debugger;
                                e.data.OrderPrice = response;


                            }


                            function error_getorder_priceusd(response) {
                               //debugger;
                                //$.notify('Error in converting the entered Order Price to USD!', {
                                //    globalPosition: "top center",
                                //    className: "warn"
                                //});
                            }
                        }
                    
               


            Selected = [];
            e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
            //e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
            
                    Selected.push(e.data);
                   //debugger;
            Update(Selected, filtered_yr);
        },
                    onRowInserting: function (e) {
                        new_request = false;
            $.notify("New Item is being added to your cart...Please wait!", {
                globalPosition: "top center",
                className: "success"
            })
           //debugger;
            e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
            if (e.data.Reviewed_Cost == null || e.data.Reviewed_Cost == undefined)
                e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
            if (e.data.OrderPrice > 0 && e.data.Currency != undefined) {

                $.ajax({

                    type: "GET",
                    url: "/BudgetingVKM/GetOrderPriceinUSD",
                    data: { 'OrderPrice': e.data.OrderPrice, 'Currency': e.data.Currency },
                    datatype: "json",
                    async: false,
                    success: success_getorder_priceusd,
                    error: error_getorder_priceusd

                });

                function success_getorder_priceusd(response) {
                   //debugger;
                    e.data.OrderPrice = response;


                }


                function error_getorder_priceusd(response) {
                   //debugger;
                    //$.notify('Error in converting the entered Order Price to USD!', {
                    //    globalPosition: "top center",
                    //    className: "warn"
                    //});
                }
            }
                    Selected = [];
                    Selected.push(e.data);
                    Update(Selected, filtered_yr);
                    },
                    onRowRemoving: function (e) {

                        Delete(e.data.RequestID, filtered_yr);

                    }

                })/*.dxDataGrid('instance').refresh()*/;
                //Add option for Lab Admins
               // $("#RequestTable").dxDataGrid("instance").option("editing.allowAdding", flag);
   








                //multiselect depts & change the dept mappings

                //var selectedRowsData = dataGridLEP.getSelectedRowsData();
               

               
                function changeDept() {
                   //debugger;
                    
                    dataGrid.getSelectedRowsData().then(function (rowData) {

                       //debugger;

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
                           //debugger;
                        }


                    })
                    
                      
                }
               
                $("#changedeptButton").dxButton({
                    text: "Change Dept for the selected Requests",
                    type: "default",
                    onClick: changeDept
                });

                //$("#updateDeptGrpButton").dxButton({
                //    text: "Update Changes",
                //    type: "default",
                //    onClick: fnDeptGrpChange
                //});
                ////$('#ddlDepts').change(function () {
                ////   //debugger;
                ////    var deptselected = document.getElementById('ddlDepts').value;
                ////    fnDeptChange(deptselected);
                ////   //debugger;
                ////});
                //function fnDeptGrpChange() {

                //    var deptselected = document.getElementById('ddlDepts').value;
                //    var grpselected = document.getElementById('ddlGroups').value;
                //    var Dept, Grp;
                //   //debugger;
                //    $.ajax({
                //        type: "GET",
                //        url: encodeURI("../BudgetingVKM/DeptID_toName"),
                //        data: { 'DeptID': deptselected },
                //        async: false,
                //        success: function (data) {
                //            Dept = data.data;
                //           //debugger;
                //        }
                //    })
                //    $.ajax({
                //        type: "GET",
                //        url: encodeURI("../BudgetingVKM/GrpID_toName"),
                //        data: { 'GrpID': grpselected },
                //        async: false,
                //        success: function (data) {
                //            Grp = data.data;
                //           //debugger;
                //        }
                //    })

                //   //debugger;
                //    if (confirm('Do you confirm to update the selected list with Dept: ' + Dept + ' and Group: ' + Grp   + ' ? ')) {
                //       //debugger;
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
                //        $("#RequestTable").dxDataGrid({
                //            dataSource: getdata
                //        });
                //    }
                //    function error_refresh_getdata(response) {

                //        $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                //            globalPosition: "top center",
                //            className: "warn"
                //        });

                //    }
                //   //debugger;
                   
                //}


                //function onerror_deptdetails(response) {
                //   //debugger;
                //    $.notify("Unable to update the Request List with the Ordering Stage Dept, Please try again!", {
                //        globalPosition: "top center",
                //        className: "warn"
                //    });
                //}

            }

   


            function OnError_GetData(response) {
                $("#RequestTable").prop('hidden', false);
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
   //debugger;
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
           //debugger;
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
   //debugger;
  

    $.ajax({
        type: "POST",
        url: encodeURI("../BudgetingVKM/AddOrEdit_CurrConv"),
        data: { 'curritem': id1[0] },
        success: function (data) {
           //debugger;
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
               //debugger;
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
                   //debugger;
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success",
                        autoHideDelay: 10000
                    });
                }

                function onerror_masterlistupdate(response) {
                   //debugger;
                    //$.notify("Unable to refresh Item MasterList Table, Please try again!", {
                    //    globalPosition: "top center",
                    //    className: "warn"
                    //});
                }


            }
            function onerror_currconv(response) {
               //debugger;
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




                           //debugger;
                            $.ajax({
                                type: "GET",
                                url: encodeURI("../BudgetingVKM/GetBUSummaryData"),
                                data: { 'year': filtered_yr },
                                success: success_refresh_busummary,
                                error: error_refresh_busummary
                            });
                            function success_refresh_busummary(response) {
                               //debugger;
                               //debugger;
                                $("#BUSummaryTable").dxDataGrid({ dataSource: eval(response.data.data) });
                            }
                            function error_refresh_busummary(response) {
                                //$.notify('Unable to Refresh BU Summary right now, Please Try again later!', {
                                //    globalPosition: "top center",
                                //    className: "warn"
                                //});
                            }

                            chart();
                           //debugger;
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
                                $("#RequestTable").dxDataGrid({
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
           //debugger;
                var genSpinner = document.querySelector("#SubmitSpinner");
                if (confirm('Do you confirm to move this Request Record To Purchase Queue?')) {
                    if (id == 1999999999) {
                        genSpinner.classList.add('fa');
                        genSpinner.classList.add('fa-spinner');
                        genSpinner.classList.add('fa-pulse');
                    }
                    $.ajax({
                        type: "POST",
                        url: encodeURI("../BudgetingVKM/SHApprove"),
                        data: {'id': id, 'useryear': filtered_yr },
                        success: function (data) {
                           
                                if (id == 1999999999) {

                                    genSpinner.classList.remove('fa');
                                    genSpinner.classList.remove('fa-spinner');
                                    genSpinner.classList.remove('fa-pulse');
                                }
                               //debugger;
                                newobjdata = data.data;
                                $("#RequestTable").dxDataGrid({ dataSource: newobjdata });


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

                           
                        }

                    });

                }
            }




            function Update(id1, filtered_yr) {
               //debugger;
                $.ajax({
                    type: "POST",
                    url: encodeURI("../BudgetingVKM/AddOrEdit"),
                    data: {'req': id1[0], 'useryear': filtered_yr },
                    success: function (data) {

                    //newobjdata = data.data;

                    //    $("#RequestTable").dxDataGrid({dataSource: newobjdata });

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

                       //debugger;
                        $.ajax({
                            type: "GET",
                            url: encodeURI("../BudgetingVKM/GetBUSummaryData"),
                            data: { 'year': filtered_yr },
                            success: success_refresh_busummary,
                            error: error_refresh_busummary
                        });
                        function success_refresh_busummary(response) {
                           //debugger;
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

                            $("#sectionsummarytable").dxDataGrid({dataSource: dataObjData_section1 });
                        }
                        function error_refresh_sectionsummary(response) {
                            //$.notify('Unable to Refresh Section Summary right now, Please Try again later!', {
                            //globalPosition: "top center",
                            //className: "warn"
                            //});
                        }
                       

                       //debugger;
                        if (data.is_MailTrigger) {
                           //debugger;

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
                       

                        if (data.success) {
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

                            $.notify(data.message, {
                                globalPosition: "top center",
                                className: "success"
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

            function Delete(id, filtered_yr) {
               
                    $.ajax({
                        type: "POST",
                        url: "/BudgetingVKM/Delete",
                        data: { 'id': id, 'useryear': filtered_yr },
                        success: function (data) {
                            newobjdata = data.data;
                            $("#RequestTable").dxDataGrid({ dataSource: newobjdata });



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
                $("#RequestTable").dxDataGrid("clearFilter");
                }
            });

            
            //BULookup,OEMLookup,DeptLookup,GroupLookup,ItemNameLookup,CostElementLookup,CategoryLookup

debugger;

            $.ajax({
                type: "GET",
                url: encodeURI("../BudgetingVKM/InitRowValues"),
                success: OnSuccessCall_dnew,
                error: OnErrorCall_dnew

            });
function OnSuccessCall_dnew(response) {
   //debugger;
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
   //debugger;
    var e = document.getElementById("effect");
   //debugger;
    if (e.style.display == 'block')
        e.style.display = 'none';
    else
        e.style.display = 'block';
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
                    data: {'useryear': filtered_yr },


                    success: function (export_result) {

                        var bytes = new Uint8Array(export_result.FileContents);
                        var blob = new Blob([bytes], {type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
                        var link = document.createElement('a');
                        link.href = window.URL.createObjectURL(blob);
                        link.download = export_result.FileDownloadName ;
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
               //debugger;
                 var genSpinner = document.querySelector("#ExportallSpinner");

                 genSpinner.classList.add('fa');
                 genSpinner.classList.add('fa-spinner');
                genSpinner.classList.add('fa-pulse');
               //debugger;
                $.ajax({

                    type: "POST",
                    url: "/BudgetingVKM/ExportDataToExcelAll/",
                    data: {'useryear': filtered_yr },


                    success: function (export_result) {
                       //debugger;
                        var bytes = new Uint8Array(export_result.FileContents);
                        var blob = new Blob([bytes], {type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
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
        if (rdata.errormsg == "") {
           //debugger;
            $.notify('Data Uploaded Successfully', {
                globalPosition: "top center",
                className: "success"
            });
        }
        else {
            $.notify(rdata.ErrorMsg, {
                globalPosition: "top center",
                className: "error"
            });
        }
        
       //debugger;
       


        function onsuccess_lookupdata1(response) {
           //debugger;
            lookup_data = response.data;
            BU_list = lookup_data.BU_List;
            OEM_list = lookup_data.OEM_List;
            DEPT_list = lookup_data.DEPT_List;
            //if (lookup_data.Groups_List != null)
            //    Group_list = lookup_data.Groups_List;
            //else
            Group_list = lookup_data.Groups_test;//Groups_oldList;
            Item_list = lookup_data.Item_List;
            Category_list = lookup_data.Category_List;
            CostElement_list = lookup_data.CostElement_List;
            OrderStatus_list = lookup_data.OrderStatus_List;
            Fund_list = lookup_data.Fund_List;
            Currency_list = lookup_data.Currency_List;
           //debugger;

        }
        function onerror_lookupdata1(response) {
           //debugger;
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
        url: "/BudgetingVKM/GetData",
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
}

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



//**********DEPT MAPPING DETAILS************


//Old vs New Dept list where in the Lab Team can change the dept to latest for the apprvd list directly 


//$.ajax({
//    type: "GET",
//    url: encodeURI("../BudgetingVKM/GetData_DeptDetails"),
//    success: OnSuccess_GetData_DeptDetails,
//    error: OnError_GetData_DeptDetails
//});
//debugger;


//function OnSuccess_GetData_DeptDetails(response) {
//   //debugger;
//    var objdata = (response.data);

//    dataGridLEP_DeptDetails = $("#RequestTable_DeptDetails").dxDataGrid({

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
//                                   //debugger;
//                                    rowData.PlanningDEPT = value;
//                                },

//                                lookup: {
//                                    dataSource: function (options) {
//                                       //debugger;
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
//                                //   //debugger;
//                                //    rowData.Department = value;
//                                //    rowData.Group = null;

//                                //},

//                                lookup: {
//                                    dataSource: function (options) {
//                                       //debugger;
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
//                            //       //debugger;
//                            //        rowData.Department = value;
//                            //        rowData.Group = null;

//                            //    },

//                            //    lookup: {
//                            //        dataSource: function (options) {
//                            //           //debugger;
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
//                            //       //debugger;

//                            //        rowData.Group = value;

//                            //    },
//                            //    lookup: {
//                            //        dataSource: function (options) {
//                            //           //debugger;
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
//           //debugger;
//            Update_DeptDetails(Selected);

//            //}

//        }

        

//    });
//   //debugger;

   

//}

//function OnError_GetData_DeptDetails(data) {
//   //debugger;
//    $("#RequestTable_DeptDetails").prop('hidden', false);
//    $.notify(data.message, {
//        globalPosition: "top center",
//        className: "warn"
//    })
//}



//function Update_DeptDetails(id1) {
//   //debugger;
//    //Onclick of edit - get the current ordering dept data store in a variable - pass this to update fn  (onclick of edit button - get innerhtml)
//    var FromDept, ToDept;
//   //debugger;
//    $.ajax({
//        type: "GET",
//        url: encodeURI("../BudgetingVKM/DeptID_toName"),
//        data: { 'DeptID': prev_orderingDept },
//        async: false,
//        success: function (data) {
//            FromDept = data.data;
//           //debugger;
//        }
//    })
//    $.ajax({
//        type: "GET",
//        url: encodeURI("../BudgetingVKM/DeptID_toName"),
//        data: { 'DeptID': id1[0].OrderingDept },
//        async: false,
//        success: function (data) {
//            ToDept = data.data;
//           //debugger;
//        }
//    })
//   //debugger;
//    if (confirm('Do you confirm to update Dept: ' + FromDept + ' to ' + ToDept + ' in Reviewed List? ')) {
//        $.ajax({
//            type: "POST",
//            url: encodeURI("../BudgetingVKM/AddOrEdit_DeptDetails"),
//            data: { 'req': id1[0] },
//            success: function (data) {
//               //debugger;
//                //newobjdata = data.data;

//                //$("#RequestTable").dxDataGrid({dataSource: newobjdata });
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
//                    $("#RequestTable_DeptDetails").dxDataGrid({
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
//                   //debugger;
//                    $.ajax({

//                        type: "POST",
//                        url: "/BudgetingVKM/UpdateRequestList_DeptChange",
//                        data: { 'reqitemlist': id1[0], 'prev_orderingDept': prev_orderingDept },
//                        //async: false,
//                        success: onsuccess_DeptChange,
//                        error: onerror_DeptChange
//                    })

//                    function onsuccess_DeptChange(response) {
//                       //debugger;

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
//                            $("#RequestTable").dxDataGrid({
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
//                       //debugger;
//                        $.notify(response.message, {
//                            globalPosition: "top center",
//                            className: "success"
//                        });
//                    }

//                    function onerror_DeptChange(response) {
//                       //debugger;
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



//debugger;
//$.ajax({
//    type: "GET",
//    url: encodeURI("../BudgetingVKM/GetData_GroupDetails"),
//    success: OnSuccess_GetData_GroupDetails,
//    error: OnError_GetData_GroupDetails
//});
//debugger;
//function OnSuccess_GetData_GroupDetails(response) {
//   //debugger;
//    var objdata = (response.data);

//    dataGridLEP_GroupDetails = $("#RequestTable_GroupDetails").dxDataGrid({

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
//                                   //debugger;
//                                    rowData.PlanningGroup = value;
//                                },

//                                lookup: {
//                                    dataSource: function (options) {
//                                       //debugger;
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
//                                //   //debugger;
//                                //    rowData.Department = value;
//                                //    rowData.Group = null;

//                                //},

//                                lookup: {
//                                    dataSource: function (options) {
//                                       //debugger;
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
                            //       //debugger;
                            //        rowData.Department = value;
                            //        rowData.Group = null;

                            //    },

                            //    lookup: {
                            //        dataSource: function (options) {
                            //           //debugger;
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
                            //       //debugger;

                            //        rowData.Group = value;

                            //    },
                            //    lookup: {
                            //        dataSource: function (options) {
                            //           //debugger;
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
//           //debugger;
//            Update_GroupDetails(Selected);

            //}

//        },


//    });



//}

//function OnError_GetData_GroupDetails(data) {
//   //debugger;
//    $("#RequestTable_GroupDetails").prop('hidden', false);
//    $.notify(data.message, {
//        globalPosition: "top center",
//        className: "warn"
//    })
//}



//function Update_GroupDetails(id1) {
//   //debugger;
//    //Onclick of edit - get the current ordering Group data store in a variable - pass this to update fn  (onclick of edit button - get innerhtml)
//    var FromGrp, ToGrp;
//   //debugger;
//    $.ajax({
//        type: "GET",
//        url: encodeURI("../BudgetingVKM/GrpID_toName"),
//        data: { 'GrpID': prev_orderingGroup },
//        async: false,
//        success: function (data) {
//           //debugger;
//            FromGrp = data.data;
//        }
//    })
//    $.ajax({
//        type: "GET",
//        url: encodeURI("../BudgetingVKM/GrpID_toName"),
//        data: { 'GrpID': id1[0].OrderingGroup },
//        async: false,
//        success: function (data) {
//           //debugger;
//            ToGrp = data.data;
//        }
//    })
//   //debugger;
//    if (confirm('Do you confirm to update Group: ' + FromGrp + ' to ' + ToGrp + ' in Reviewed List? ')) {
//        $.ajax({
//            type: "POST",
//            url: encodeURI("../BudgetingVKM/AddOrEdit_GroupDetails"),
//            data: { 'req': id1[0] },
//            success: function (data) {
//               //debugger;
//                //newobjdata = data.data;

//                //$("#RequestTable").dxDataGrid({dataSource: newobjdata });
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
//                    $("#RequestTable_GroupDetails").dxDataGrid({
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
//                   //debugger;
//                    $.ajax({

//                        type: "POST",
//                        url: "/BudgetingVKM/UpdateRequestList_GroupChange",
//                        data: { 'reqitem': id1[0], 'prev_orderingGroup': prev_orderingGroup },
//                        //async: false,
//                        success: onsuccess_GroupChange,
//                        error: onerror_GroupChange
//                    })

//                    function onsuccess_GroupChange(response) {
//                       //debugger;

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
//                            $("#RequestTable").dxDataGrid({
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
//                       //debugger;
//                        $.notify(response.message, {
//                            globalPosition: "top center",
//                            className: "success"
//                        });
//                    }

//                    function onerror_GroupChange(response) {
//                       //debugger;
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





     