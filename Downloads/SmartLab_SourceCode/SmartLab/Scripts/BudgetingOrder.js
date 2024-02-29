//Javascript file for Budgeting Request Details - mae9cob    


var dataGrid_order;
var newobjdata;
var BU_list, DEPT_list, Item_list, Category_list, OEM_list, Group_list, CostElement_list, OrderStatus_list, Fund_list, BudgetCodeList, PurchaseType_list, UOM_list, UnloadingPoint_list, OrderType_list, BudgetCodedesc, RFOApprover_list, CostCenter_list, BudgetCenter_list, OrderDescription_list;
var Selected = [];
var Item_list_custom; //, UOMlist; 
var unitprice, reviewer_2, category, costelement, leadtime, BudgetCode /*,*/ /*OrderTypeList, *//*UnloadingPointList,*/ /*rfoapproverlist,*/ /*CostCenter*/;
var lookup_data, new_request, BudCenter;
var filtered_yr;
var leadtime1;
var genSpinner_load = document.querySelector("#load");
var SubItemList;
var addnewitem_flag = false;

var objdata_rfoview;
var Item_headerFilter, DEPT_headerFilter, Group_headerFilter, BU_headerFilter, OEM_headerFilter, Category_headerFilter, CostElement_headerFilter, OrderStatus_headerFilter, OrderDescription_headerFilter;
/*Dynamic Fund Specific Fields - flag to detect whether F03/F05 and F06 specific fields are required based on user's selection of Fund*/
var isFund_F03 = false;
var isFund_F05orF06 = false;
var popupadd, popupedit, gridedit;
var file; //uploaded file info is stored in this variable
var localQuoteAvail = '';
var localVendorValue = '';
var vendor_type = "";
var fileUploaderFromPopup;
var popupContentTemplate;
var Requestsobj;
var openpopup = false;
var reqArray = [];
var selectedItems = [];
var $ul, ul;
var grid;
let savecont = false;
let toolbarItems;

var SelectedRequests = "";
var dropDownBox = "";
var RequestID = "";
var presentUserNTID;


var QAYes, QANo, VTSpecific, VTAny, VTDisplay, FileUploadDisplay, SSJFileLinkDisplay, NSFileLinkDisplay, fileListDisplay, noFileTmpDisplay;

$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});

var oems_chosen = [];

//////////////// OEM Selection
//var oems_chosen = new Array();
//$('input:checkbox[name=Types]:checked').each(function () {
//    //debugger;
//    Types.push($(this).val())
//});
//function fnOEMChange(oem) {
//    //debugger;
//    oems_chosen = [];
//    //oems_chosen = new Array();
//    for (var i = 0, len = oem.options.length; i < len; i++) {
//        if (document.getElementById('selectOEM').selectedIndex != -1) {
//            options = oem.options;
//            opt = options[i];
//            if (opt.selected) {
//                //store the labids chosen by user from dropdown to process the relevant chart data
//                oems_chosen.push(opt.value);
//            }
//        }
//    }
//    ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);


//}
//function fnOEMselectChange(oem) {
//    //debugger;
//    oems_chosen = [];
//    //oems_chosen = new Array();
//    for (var i = 0, len = oem.options.length; i < len; i++) {
//        if (document.getElementById('selectOEM').selectedIndex != -1) {
//            options = oem.options;
//            opt = options[i];
//            if (opt.selected) {
//                //store the labids chosen by user from dropdown to process the relevant chart data
//                oems_chosen.push(opt.value);
//            }
//        }
//    }
//    ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);

//}


//$.ajax({

//    type: "GET",
//    url: "/BudgetingOrder/Lookup",
//    async: false,
//    success: onsuccess_lookupdata,
//    error: onerror_lookupdata
//})


//function onsuccess_lookupdata(response) {

//    lookup_data = response.data;
//    BU_list = lookup_data.BU_List;
//    OEM_list = lookup_data.OEM_List;
//    DEPT_list = lookup_data.DEPT_List;
//    Group_list = lookup_data.Groups_test;
//    Item_list = lookup_data.Item_List;
//    Category_list = lookup_data.Category_List;
//    CostElement_list = lookup_data.CostElement_List;
//    OrderStatus_list = lookup_data.OrderStatus_List;
//    Fund_list = lookup_data.Fund_List;

//    //Item_list_New = lookup_data.Item_List1;

//}  
//function onerror_lookupdata(response) {
//    alert("Error in fetching lookup");

//}

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

function mySuccessFuntion(rdata) {
    //debugger;

    if (rdata.success) {
        $.ajax({
            type: "GET",
            url: "/BudgetingOrder/GetData",
            data: { 'year': filtered_yr },
            datatype: "json",
            async: true,
            success: success_refresh_getdata,
            error: error_refresh_getdata

        });
        $.ajax({

            type: "GET",
            url: "/BudgetingOrder/Lookup",
            async: false,
            data: { 'year': filtered_yr },
            success: onsuccess_lookupdata,
            error: onerror_lookupdata
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

        function onsuccess_lookupdata_Group(response) {
            //debugger;
            Group_list = JSON.parse(response.groupList.Content);

        }

        function onerror_lookupdata_Group(response) {
            //debugger;
            alert("Error in fetching Group lookup");

        }

        $.ajax({
            type: "GET",
            url: "/BudgetingOrder/GetData",
            data: { 'year': filtered_yr },
            datatype: "json",
            async: true,
            success: success_refresh_getdata,
            error: error_refresh_getdata

        });

        function success_refresh_getdata(response) {

            var getdata = response.data;
            $("#RequestTable_RFO").dxDataGrid({
                dataSource: getdata
            });
        }
        function error_refresh_getdata(response) {

            //$.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
            //    globalPosition: "top center",
            //    className: "warn"
            //});

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
        url: "/BudgetingOrder/GetData",
        data: { 'year': filtered_yr },
        datatype: "json",
        async: true,
        success: success_refresh_getdata,
        error: error_refresh_getdata

    });

    function success_refresh_getdata(response) {

        var getdata = response.data;
        $("#RequestTable_RFO").dxDataGrid({
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





function handleChangeQuoteAvail(src) {
    //debugger;
    if (src.id == "QAY") {
        localQuoteAvail = 'Yes'
        document.getElementById("vendorInputlayout").style.display = "block";
        document.getElementById("SSJFileLink").style.display = "none";
        document.getElementById("NSFileLink").style.display = "none";
        document.getElementById("FileUploadBtn").style.display = "none";
    } else {
        localQuoteAvail = 'No'
        document.getElementById("vendorInputlayout").style.display = "none";

        document.getElementById("SSJFileLink").style.display = "none";
        document.getElementById("NSFileLink").style.display = "block";
        document.getElementById("FileUploadBtn").style.display = "block";
    }
    document.getElementById("VTY").checked = false;
    document.getElementById("VTN").checked = false;
    localVendorValue = '';
};
function handleChangeVendorType(src) {
    //debugger;
    document.getElementById("FileUploadBtn").style.display = "block";
    if (src.id == "VTY") {
        localVendorValue = 'Specific'
        if (localQuoteAvail == 'Yes') {
            document.getElementById("SSJFileLink").style.display = "block";
            document.getElementById("NSFileLink").style.display = "none";
        }
    } else {
        localVendorValue = 'Any'

        document.getElementById("SSJFileLink").style.display = "none";
        document.getElementById("NSFileLink").style.display = "block";
    }
};
$.ajax({
    type: "GET",
    url: encodeURI("../BudgetingOrder/InitRowValues"),
    success: OnSuccessCall_dnew,
    error: OnErrorCall_dnew

});
function OnSuccessCall_dnew(response) {

    //debugger;

    //$.ajax({

    //    type: "post",
    //    url: encodeURI("../BudgetingOrder/GetRFOBudgetCenter"),
    //    data: { 'deptid': e.data.DEPT },
    //    async: false,
    //    success: function (data) {
    //        //debugger;
    //        BudCenter = data.data;

    //        /*e.data.DEPT = BudgetCenter;*/
    //    }
    //})
    //debugger;
    new_request = response.data;
    //popupadd=response.popupadd;
    //popupedit=response.popupedit;
    //gridedit = response.gridedit;
    //debugger;
}
function OnErrorCall_dnew(response) {

    //$.notify('Add new error!', {
    //    globalPosition: "top center",
    //    className: "warn"
    //});
}




//Loading indicator on load of the Request module while fetching the Item Requests
window.onload = function () {
    ////debugger;
    document.getElementById("loadpanel").style.display = "block";


    genSpinner_load.classList.add('fa');
    genSpinner_load.classList.add('fa-spinner');
    genSpinner_load.classList.add('fa-pulse');
    $("#RequestTable_RFO").prop('hidden', true);

    //$("#chkRFO").attr("autocomplete", "off");
    //$("#chkRequest").attr("autocomplete", "off");
    //$("#chkItem").attr("autocomplete", "off");

    //document.getElementById('chkRFO').reset();
    //document.getElementById('chkRequest').reset();
    //document.getElementById('chkItem').reset();




    //$("#chkRFO").prop("checked", false);
    //$("#chkRequest").prop("checked", false);
    //$("#chkItem").prop("checked", false);

    //chkRFO
    //chkItem
    //chkRequest
    localVendorValue = '';
    localQuoteAvail = '';
};



//Reference the DropDownList for Year to be selected by Requestor
var ddlYears = document.getElementById("ddlYears");
//Determine the Current Year.
var currentYear = (new Date()).getFullYear();
////debugger;
//Loop and add the Year values to DropDownList.
//for (var i = currentYear; i >= 2020; i--) {
//    var option = document.createElement("OPTION");
//    option.innerHTML = i;
//    option.value = i;
//    ddlYears.appendChild(option);
//}
////Loop and add the Year values to DropDownList.
for (var i = currentYear+1; i >= 2022; i--) {
    var option = document.createElement("OPTION");
    option.innerHTML = i;
    option.value = i;
    ddlYears.appendChild(option);

    if (option.value == (currentYear+1)) {
        //if (option.value == (currentYear - 2)) {
        option.defaultSelected = true;
        //option.defaultSelected = true;
    }
    filtered_yr = $("#ddlYears").val();
    filtered_yr = parseInt(filtered_yr) - 1;
    filtered_yr = filtered_yr.toString();
    ////debugger;
}




//At load, Display the data for Current year
if (filtered_yr == null) {
    filtered_yr = new Date().getFullYear();
}
////debugger;
//$('.selectpicker').selectpicker('selectAll');//it wil hit fnoemchange to select all & then execute ajaxcallforrequestui
ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);
////debugger;

//Function to change year from dropdown
function fnYearChange(yearselect) {
    ////debugger;
    $("#RequestTable_RFO").prop('hidden', true);
    document.getElementById("loadpanel").style.display = "block";

    genSpinner_load = document.querySelector("#load");
    genSpinner_load.classList.add('fa');
    genSpinner_load.classList.add('fa-spinner');
    genSpinner_load.classList.add('fa-pulse');
    filtered_yr = yearselect.value;

    filtered_yr = parseInt(yearselect.value) - 1;
    filtered_yr = filtered_yr.toString();
    ////debugger;
    //Ajax call to Get Request Item Data
    ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);



}


function ajaxCallforRequestUI(filtered_yr) {

    $(':checkbox').prop('checked', false);
    /*****T To populate sub items ordered under the Planned Items *****/
    //$.ajax({
    //    type: "POST",
    //    url: "/BudgetingOrder/GetPODetails",
    //    datatype: "json",
    //    success: function (data) {
    //        ////debugger;
    //        if (data.data.length > 0) {
    //            ////debugger;
    //            //var res = JSON.parse(data.data.Data.Content);
    //            SubItemList = eval(data.data);
    //            //LoadDataGrid(res);

    //        }
    //    },
    //    error: function (jqXHR, exception) {
    //        ////debugger;
    //        var msg = '';
    //        if (jqXHR.status === 0) {
    //            msg = 'Not connect.\n Verify Network.';
    //        } else if (jqXHR.status == 404) {
    //            msg = 'Requested page not found. [404]';
    //        } else if (jqXHR.status == 500) {
    //            msg = 'Internal Server Error [500].';
    //        } else if (exception === 'parsererror') {
    //            msg = 'Requested JSON parse failed.';
    //        } else if (exception === 'timeout') {
    //            msg = 'Time out error.';
    //        } else if (exception === 'abort') {
    //            msg = 'Ajax request aborted.';
    //        } else {
    //            msg = 'Uncaught Error.\n' + jqXHR.responseText;
    //        }
    //        $('#post').html(msg);
    //    }
    //});

    /****** To populate the user selection dropdown lists *******/
    $.ajax({

        type: "GET",
        url: "/BudgetingOrder/Lookup",
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
        //Group_list = lookup_data.Groups_test;
        //Item_list = lookup_data.Item_List;

        Category_list = lookup_data.Category_List;
        CostElement_list = lookup_data.CostElement_List;
        OrderStatus_list = lookup_data.OrderStatus_List;
        Fund_list = lookup_data.Fund_List;
        BudgetCodeList = lookup_data.BudgetCodeList;
        PurchaseType_list = lookup_data.PurchaseType_List;
        UnloadingPoint_list = lookup_data.UnloadingPoint_List;
        OrderType_list = lookup_data.Order_Type_List;
        UOM_list = lookup_data.UOM_List;
        RFOApprover_list = lookup_data.RFOApprover_List;
        CostCenter_list = lookup_data.CostCenter_List;
        BudCenter = lookup_data.BudgetCenter_List;
        OrderDescription_list = lookup_data.OrderDescription_List;



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

        //debugger;
        //Item_list_custom = Item_list.filter(function (item) {
        //    return (item.VKM_Year === curryear);
        //});
        //Item_list_New = lookup_data.Item_List1;
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
            //success: onsuccess_lookupdata_Item,
            //error: onerror_lookupdata_Item
        })
        $.ajax({

            type: "GET",
            url: "/BudgetingOrder/Lookup_GroupList",
            async: false,
            data: { 'year': filtered_yr },
            success: onsuccess_lookupdata_Group,
            error: onerror_lookupdata_Group
        })
        //debugger;
    }

    function onerror_lookupdata(response) {
        alert("Error in fetching lookup");

    }

    //function onsuccess_lookupdata_Item(response) {
    //    //debugger;


    //    Item_list = JSON.parse(response.itemList.Content);

    //    //Item_list_custom = Item_list.filter(function (item) {
    //    //    return (item.VKM_Year === curryear);
    //    //});
    //    //Item_list_New = lookup_data.Item_List1;

    //}

    //function onerror_lookupdata_Item(response) {
    //    //debugger;
    //    alert("Error in fetching Item lookup");

    //}

    function onsuccess_lookupdata_Group(response) {
        //debugger;
        Group_list = JSON.parse(response.groupList.Content);

    }

    function onerror_lookupdata_Group(response) {
        //debugger;
        alert("Error in fetching Group lookup");

    }


    //Ajax call to Get Request Item Data
    ////debugger;
    $.ajax({
        type: "POST",
        url: encodeURI("../BudgetingOrder/GetData"),
        data: { 'year': filtered_yr },
        success: OnSuccess_GetData,
        error: OnError_GetData
    });


    function OnSuccess_GetData(response) {
        //debugger;
        //if (response.success) {
        objdata_rfoview = (response.data);
        popupadd = response.popupadd;
        popupedit = response.popupedit;
        gridedit = response.gridedit;
        presentUserNTID = response.UserNTID;
        //For initial req - to allow user to add f01, f03 req from cc - this req was not to be developed
        //var isF03F01 = function (position) {

        //    if (position == undefined)
        //        return true;
        //    else
        //        return position && [1, 3].indexOf(position) >= 0;

        //};
        var isDelivered = function (position) {//cancelled also included

            //CHANGE
            return position && [5, 6, 10].indexOf(position) >= 0;

        };


        //Hide the Loading indicator once the Request List is fetched
        genSpinner_load.classList.remove('fa');
        genSpinner_load.classList.remove('fa-spinner');
        genSpinner_load.classList.remove('fa-pulse');
        document.getElementById("loadpanel").style.display = "none";
        $("#RequestTable_RFO").prop('hidden', false);
        //debugger;
        $(function () {
            grid = $("#RequestTable_RFO").dxDataGrid({

                dataSource: objdata_rfoview,
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
                                },
                            },


                        }
                    ]
                },
                onToolbarPreparing: function (e) {
                    toolbarItems = e.toolbarOptions.items;

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
                width: "96.8vw",
                height: "76vh",
                showColumnLines: true,
                showRowLines: true,
                rowAlternationEnabled: true,
                hoverStateEnabled: {
                    enabled: true
                },
                editing: {
                    mode: popupedit == true ? "popup" : "row", //"popup",

                    allowAdding: popupadd,

                    allowUpdating: function (e) {
                        // //debugger;
                        return (response.success && !response.isDashboard_flag && (isDelivered(e.row.data.OrderStatus) || !(e.row.data.RequestToOrder))); //if success = true - access to update and view ; false - access to only view
                    },
                    //allowDeleting: function (e) {

                    //    return !(e.row.data.RequestToOrder) && isF03F01(e.row.data.Fund);
                    //},
                    useIcons: true,
                    popup: {
                        title: "Lab RFO",
                        width: 900,
                        height: 900,
                        showTitle: true,
                        visible: true,
                        hideOnOutsideClick: true,
                        //width: 450,
                        //height: 350,
                        resizeEnabled: true,
                        toolbarItems: [{
                            locateInMenu: 'always',
                            widget: 'dxButton',
                            toolbar: 'bottom',

                            //location: 'before',
                            options: {
                                //type: "success",
                                //elementAttr: {
                                //    id: "savecnt",
                                //    class: "save"
                                //},
                                stylingMode: "outlined",
                                icon: "repeat",
                                text: "Save & Continue",
                                hint: "Kindly use this button for if you want to add multiple requests",
                                visible: true,
                                onClick: function () {
                                    savecont = true;
                                    var dataGrid = $("#RequestTable_RFO").dxDataGrid("instance");
                                    dataGrid.saveEditData();

                                    DevExpress.ui.notify({
                                        message: "Request saved successfully",
                                        className: "success",
                                        width: 500,
                                        position: {
                                            my: "top",
                                            at: "top",
                                            of: "#container"
                                        }
                                    });
                                },
                            },
                        }, {
                            widget: 'dxButton',
                            toolbar: 'bottom',
                            location: 'after',
                            options: {
                                //type: "success",
                                stylingMode: "outlined",
                                icon: "save",
                                text: "Save",
                                onClick: function () {
                                    savecont = false;
                                    var dataGrid = $("#RequestTable_RFO").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                    //grid.saveEditData();


                                },
                            },
                        },
                        {
                            widget: 'dxButton',
                            toolbar: 'bottom',
                            location: 'after',
                            options: {
                                text: 'Cancel',
                                onClick: function () {
                                    var dataGrid = $("#RequestTable_RFO").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                    // grid.cancelEditData();
                                },
                            },
                        }],

                    },
                    form: {
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
                                        //editorOptions: {
                                        //    value: 6
                                        //},
                                        visible: true
                                    },
                                    {
                                        dataField: 'OEM',
                                        label: {
                                            text: 'Customer',
                                            template: contentTemplate
                                        },

                                        //template: (data, element) => {
                                        //    //debugger;
                                        //        const lineBreak = '<br>';
                                        //        const infoIcon = '<i id="helpedInfo" class="dx-icon dx-icon-info"></i>';
                                        //        const labelText = `Additional${lineBreak}${data.text}`;

                                        //        element.append(`<div id='template-content'>${infoIcon}${labelText}</div>`);

                                        //        $('<div>').dxTooltip({
                                        //            target: '#helpedInfo',
                                        //            showEvent: 'mouseenter',
                                        //            hideEvent: 'mouseleave',
                                        //            contentTemplate(args) {
                                        //                args.html('<div id="tooltip-content">This field must not exceed 200 characters</div>');
                                        //            },
                                        //        }).appendTo(element);
                                        //    },


                                        //},
                                        dataType: 'string',
                                        validationRules: [{
                                            type: "required"
                                        }],


                                    },
                                    {
                                        dataField: 'DEPT',
                                        label: {
                                            text: 'Department'
                                        },
                                        dataType: 'string',
                                        validationRules: [{
                                            type: "required"
                                        }],
                                    },

                                    {
                                        dataField: 'Group',
                                        label: {
                                            text: 'Group'
                                        },
                                        dataType: 'string',
                                        validationRules: [{
                                            type: "required"
                                        }],
                                    },


                                    {
                                        dataField: 'GoodsRecID',
                                        label: {
                                            text: 'Goods Recipient ID'
                                        },
                                        dataType: 'string',
                                        validationRules: [{
                                            type: "required"
                                        }],

                                        editorOptions: {
                                            buttons: [{
                                                name: 'password',
                                                location: 'after',
                                                options: {
                                                    icon: 'Content/images/info-i-frame.png',
                                                    type: 'default',
                                                    hint: "Eg: MAE9COB"
                                                    //onClick: () => changePasswordMode('Password'),
                                                },

                                            }],
                                        }
                                    },
                                    {
                                        dataField: 'RFOReqNTID',
                                        label: {
                                            text: 'RFO Requestor NTID'
                                        },
                                        //validationRules: [{
                                        //    type: "required"
                                        //}],
                                        //editorOptions: {
                                        //    value: RequestorNTID,
                                        //}
                                    },
                                    {
                                        dataField: 'LabName',
                                        label: {
                                            text: 'Lab Name'
                                        },
                                        dataType: 'string',
                                        validationRules: [{
                                            type: "required"
                                        }],
                                        editorOptions: {
                                            buttons: [{
                                                name: 'password',
                                                location: 'after',
                                                options: {
                                                    icon: 'Content/images/info-i-frame.png',
                                                    type: 'default',
                                                    hint: "Eg: HV Lab / C203"
                                                    //onClick: () => changePasswordMode('Password'),
                                                },

                                            }],
                                        }
                                    },
                                    {
                                        dataField: 'OrderType',
                                        editorType: 'dxSelectBox',
                                        editorOptions: {
                                            items: OrderType_list,
                                            displayExpr: 'Order_Type',
                                            valueExpr: 'ID',
                                            searchEnabled: true,
                                        },
                                        label: {
                                            text: 'Order Type'
                                        },
                                        dataType: 'string',
                                        validationRules: [{
                                            type: "required"
                                        }],
                                    },
                                    {
                                        dataField: 'UnloadingPoint',
                                        label: {
                                            text: 'Unloading Point'
                                        },
                                        editorType: 'dxSelectBox',
                                        editorOptions: {
                                            items: UnloadingPoint_list,
                                            displayExpr: 'UnloadingPoint',
                                            valueExpr: 'ID',
                                            searchEnabled: true,
                                        },
                                        validationRules: [{
                                            type: "required"
                                        }],

                                    },
                                    {
                                        dataField: 'BudgetCenterID',
                                        label: {
                                            text: 'Budget Centre'
                                        },
                                        //dataType: 'string',
                                        editorType: 'dxSelectBox',
                                        editorOptions:
                                        {
                                            items: BudCenter,
                                            displayExpr: 'BudgetCenter',
                                            valueExpr: 'ID',
                                            searchEnabled: true,
                                            noDataText: "No Data. Kindly contact ELO Team",
                                        },
                                        validationRules: [{
                                            type: "required"
                                        }],

                                    },

                                    {
                                        dataField: 'CostCenter',
                                        label: {
                                            text: 'Cost Centre'
                                        },
                                        dataType: 'string',
                                        validationRules: [{
                                            type: "required"
                                        }],
                                    },

                                    {
                                        dataField: 'RFOApprover',
                                        label: {
                                            text: 'RFO Approver'
                                        },
                                        dataType: 'string',
                                        editorType: 'dxSelectBox',
                                        editorOptions:
                                        {
                                            //items: RFOApprover_list,
                                            //displayExpr: 'Section_Dept_Grp',
                                            //valueExpr: 'Section_Dept_Grp',
                                            //searchEnabled: true,
                                        },
                                        validationRules: [{
                                            type: "required"
                                        }],

                                    },

                                    //{
                                    //    dataField: 'Comments',
                                    //    label: {
                                    //        text: 'Comments'
                                    //    },
                                    //    dataType: 'string',
                                    //},
                                    {
                                        dataField: 'PORemarks',
                                        label: {
                                            text: 'Item Justification'
                                        },
                                        dataType: 'string',

                                    },
                                ],

                            },

                            {
                                itemType: 'group',
                                caption: 'Item Details',
                                colCount: 1,
                                colSpan: 2,
                                items: [
                                    {
                                        dataField: 'Item_Name',
                                        label: {
                                            text: 'Item Name'
                                        },
                                        editorType: 'dxSelectBox',
                                        editorOptions: {
                                            items: Item_list,
                                            displayExpr: 'Item_Name',
                                            valueExpr: 'S_No',
                                            noDataText: "No Item. Kindly contact ELO Team",
                                            //filter: function (e) {
                                            //    //debugger;
                                            //    return (e.BU == 6 && e.OrderType == OrderType)
                                            //}
                                        },
                                        validationRules: [{
                                            type: "required"
                                        }],
                                    },
                                ],
                            },



                            {
                                itemType: 'group',
                                //caption: 'Item Details',
                                colCount: 2,
                                colSpan: 2,
                                items: [

                                    {
                                        dataField: 'Reviewed_Quantity',
                                        editorType: 'dxNumberBox',
                                        editorOptions: {
                                            showSpinButtons: true,
                                            min: 0,
                                        },
                                        label: {
                                            text: 'Quantity'
                                        },
                                        dataType: 'string',
                                        validationRules: [{
                                            type: "required"
                                        }],
                                    },
                                    {
                                        dataField: 'Reviewed_Cost',
                                        label: {
                                            text: 'Total Cost ($)'
                                        },
                                        dataType: "number",
                                        format: { type: "currency", precision: 2 },
                                    },
                                    {
                                        dataField: 'BudgetCode',
                                        label: {
                                            text: 'Budget Code'
                                        },
                                        dataType: 'string',
                                    },
                                    {
                                        dataField: 'BudgetCodeDescription',
                                        label: {
                                            text: 'Budget Code Description'
                                        },
                                        dataType: 'string',
                                    },
                                    {
                                        dataField: 'Cost_Element',
                                        label: {
                                            text: 'Cost Element'
                                        },
                                        dataType: 'string',
                                    },
                                    {
                                        dataField: 'Unit_Price',
                                        label: {
                                            text: 'Unit Price ($)'
                                        },
                                        //dataType: 'number',
                                        format: { type: 'currency', precision: 2 },
                                    },
                                    {
                                        dataField: 'UnitofMeasure',
                                        label: {
                                            text: 'Unit of Measure'
                                        },
                                        dataType: 'string',
                                        //editorType: 'dxSelectBox',
                                        //editorOptions: {
                                        //    items: UOM_list,
                                        //    displayExpr: 'UOM',
                                        //    valueExpr: 'ID',
                                        //    //filter: function (e) {
                                        //    //    //debugger;
                                        //    //    return (e.BU == 6 && e.OrderType == OrderType)
                                        //    //}
                                        //}
                                    },
                                    //{
                                    //    dataField: 'Currency',
                                    //    label: {
                                    //        text: 'Currency'
                                    //    },
                                    //    dataType: 'string',
                                    //},
                                    {
                                        dataField: 'Fund',
                                        label: {
                                            text: 'Fund'
                                        },
                                        dataType: 'string',
                                        validationRules: [{
                                            type: "required"
                                        }],
                                        editorOptions: {
                                            buttons: [
                                                {
                                                    name: 'password',
                                                    location: 'after',
                                                    options: {
                                                        icon: 'Content/images/info-i-frame.png',
                                                        type: 'default',
                                                        hint: "F01-BGSW A/c ;" + '\n' + " F02-VKM A/c ;" + '\n' + "F03-Customer-At Actuals ; " + '\n' + " F04-Customer Consolidated ;" + '\n' + " F05-Loan ; " + '\n' + " F06-FOC ;" + '\n' + " F07-Product ; " + '\n' + " F08-Global"
                                                        //onClick: () => changePasswordMode('Password'),
                                                    },

                                                },
                                                //{
                                                //    name: 'dropdown',
                                                //    location: 'before',
                                                //    options: {
                                                //        icon: 'dx-dropdowneditor-icon',
                                                //        type: 'default',
                                                //        hint: "F01-BGSW A/c ;" + '\n' + " F02-VKM A/c ;" + '\n' + "F03-Customer-At Actuals ; " + '\n' + " F04-Customer Consolidated ;" + '\n' + " F05-Loan ; " + '\n' + " F06-FOC ;" + '\n' + " F07-Product ; " + '\n' + " F08-Global"
                                                //        //onClick: () => changePasswordMode('Password'),
                                                //    },

                                                //},

                                            ],
                                        }
                                    },
                                    {
                                        dataField: 'RequiredDate',
                                        label: {
                                            text: 'Expected Delivery Date'
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
                                            //disabled: true,
                                        },
                                    },

                                ]
                            },
                            {
                                itemType: 'group',
                                visible: false,
                                caption: 'F03',
                                //cssClass: "DynamicField_css", //to ignore the dataField "F03" , "F05_and_F06" from being shown (reason: in general, only caption(which is displayed as a subtitle for grouped items) has to be specified for grouping items ; this caption can be used in dxForm.itemOption mtd to change the visibility. Challenge : The dxForm.itemOption mtd does not accept caption name with whitespaces (eg: "F05 and F06"). Hence, dummy dataField is provided with _ (eg: "F05_and_F06")). On using dataField along with Caption for a group of items, the dataField is also displayed as one of the items in the group, which is undesired. Hence, this has to be hidden.
                                colCount: 2,
                                colSpan: 2,
                                items: [

                                    {
                                        dataField: 'BM_Number',
                                        dataType: 'string',
                                        validationRules: [{ type: "required" }],
                                    },
                                    {
                                        dataField: 'Resource_Group_Id',
                                        dataType: 'string',
                                        validationRules: [{ type: "required" }],
                                    },
                                    {
                                        dataField: 'PIF_ID',
                                        dataType: 'string',
                                        validationRules: [{ type: "required" }],
                                    },
                                    {
                                        dataField: 'Material_Part_Number',
                                        dataType: 'string',
                                        validationRules: [{ type: "required" }],
                                    },
                                    {
                                        dataField: 'SupplierName_with_Address',
                                        cssClass: "SupplierName_css",
                                        dataType: 'string',
                                        validationRules: [{ type: "required" }],
                                    },


                                ]
                            },
                            {
                                itemType: 'group',
                                visible: false,
                                caption: 'F05 and F06',
                                name: 'F05_and_F06',
                                colCount: 2,
                                colSpan: 2,
                                items: [
                                    {
                                        dataField: 'Purchase_Type',
                                        cssClass: "Purchase_Type_css",
                                        editorType: 'dxSelectBox',
                                        editorOptions: {
                                            items: PurchaseType_list,
                                            displayExpr: 'PurchaseType',
                                            valueExpr: 'ID',
                                        },
                                        validationRules: [{ type: "required" }],
                                    },
                                    {
                                        dataField: 'Project_ID',
                                        dataType: 'string',

                                    },
                                    {
                                        dataField: 'SupplierName_with_Address',
                                        cssClass: "SupplierName_css",
                                        dataType: 'string',
                                        validationRules: [{ type: "required" }],
                                    },



                                ]
                            }
                        ]
                    }
                },
                onContentReady: function (e) {

                    e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
                },
                onCellPrepared: function (e) {
                    if (e.rowType === "header" || e.rowType === "filter") {
                        e.cellElement.addClass("columnHeaderCSS");
                        e.cellElement.find("input").addClass("columnHeaderCSS");
                    }
                },
                onEditingStart: function (e) {
                    //debugger;
                    var dataGrid = $("#RequestTable_RFO").dxDataGrid("instance");
                    if (e.data.RFOApprover.constructor.name != "Number" || e.data.RFOApprover == 0) {
                        var selectedRowIndex = e.component.getRowIndexByKey(e.key);
                        dataGrid.cellValue(selectedRowIndex, "RFOApprover", Group_list.find(x => x.ID == e.data.Group).Group);
                    }
                    //dataGrid.saveEditData();

                    if (e.data.BudgetCenterID.constructor.name != "Number" || e.data.BudgetCenterID == 0) {
                        $.ajax({

                            type: "post",
                            url: "/BudgetingOrder/GetRFOBudgetCenter",
                            data: { 'deptid': e.data.DEPT },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {

                                BudCenter = data.data;
                                if (BudCenter.length == 0) {

                                    //$.notify("Please Contact ELO Team to find BudgetCenter details !", {
                                    //    elementPosition: "right",
                                    //    className: "error",
                                    //    autoHideDelay: 13000,
                                    //});
                                    alert("Please Contact ELO Team to find BudgetCenter details !");

                                }
                                else {
                                    //debugger;
                                    dataGrid.cellValue(selectedRowIndex, "BudgetCenterID", BudCenter);
                                    // dataGrid.saveEditData();
                                }

                            }
                        })

                    }

                    //ajaxCallforRequestUI(filtered_yr);
                    e.component.option('editing').popup.toolbarItems[0].options.visible = false;


                },
                onInitNewRow: function (e) {
                    //debugger;
                    addnewitem_flag = true;
                    //e.data.Requestor = new_request.Requestor;
                    e.data.Reviewer_1 = new_request.Reviewer_1;
                    e.data.Reviewer_2 = new_request.Reviewer_2;
                    e.data.DEPT = new_request.DEPT;
                    e.data.Group = new_request.Group;
                    e.data.BU = new_request.BU;
                    e.data.OEM = new_request.OEM == 0 ? "" : new_request.OEM;
                    e.data.RFOReqNTID = new_request.RequestorNTID;
                    e.data.RFOApprover = Group_list.find(x => x.ID == new_request.Group).Group;
                    //debugger;

                    e.data.BudgetCenterID = new_request.BudgetcenterList;
                    BudCenter = new_request.BudgetcenterList;

                    /*component.cellValue(rowIndex, "BudgetCenter", BudgetCenter);*/
                    e.component.option('editing').popup.toolbarItems[0].options.visible = true;


                },

                columnAutoWidth: true,
                allowColumnReordering: true,
                allowColumnResizing: true,
                //focusedRowEnabled: true,
                allowColumnReordering: true,
                allowColumnResizing: true,
                keyExpr: "RequestID",

                columnResizingMode: "widget",
                columnChooser: {
                    enabled: true
                },
                //filterRow: {
                //    visible: true

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
                loadPanel: {
                    enabled: true
                },
                //    paging: {
                //        pageSize: 100
                //},

                //searchPanel: {
                //    visible: true,
                //    width: 240,
                //    placeholder: "Search..."
                //},
                //scrolling: {
                //    columnRenderingMode: "virtual"
                //},
                //scrolling: {
                //    mode: "virtual",
                //    rowRenderingMode: "virtual",
                //    columnRenderingMode: "virtual"
                //},
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
                columns: [
                    {
                        type: "buttons",
                        width: 90,
                        alignment: "left",
                        buttons: [{
                            hint: 'File attachment',
                            icon: 'file',
                            visible: function (e) {
                                debugger;
                                return (((response.success && !e.row.isEditing) || (!e.row.isEditing && isDelivered(e.row.data.OrderStatus))) && (e.row.data.LinkedRequestID == "")) && !response.isDashboard_flag && !gridedit ;
                            },
                            onClick: function (rowItem) {
                                //debugger;
                                openpopup = true;
                                RequestID = rowItem.row.data.RequestID;

                                localQuoteAvail = '';
                                localVendorValue = '';
                                QAYes = "unchecked";
                                QANo = "checked";
                                VTDisplay = "none"
                                VTAny = "unchecked";
                                VTSpecific = "unchecked";
                                SSJFileLinkDisplay = "none"
                                NSFileLinkDisplay = "block"
                                FileUploadDisplay = "block";

                                //debugger;
                                //var QAYes, QANo, VTSpecific, VTAny, VTDisplay, FileUploadDisplay, SSJFileLinkDisplay, NSFileLinkDisplay, fileListDisplay, noFileTmpDisplay;
                                if (rowItem.row.data.QuoteAvailable == "Yes") {
                                    QAYes = "checked";
                                    QANo = "unchecked";
                                    VTDisplay = "block"
                                    localQuoteAvail = "Yes";
                                    FileUploadDisplay = "none";
                                } else if (rowItem.row.data.QuoteAvailable == "" || rowItem.row.data.QuoteAvailable == "No") {
                                    QAYes = "unchecked";
                                    QANo = "checked";
                                    VTDisplay = "none"
                                    SSJFileLinkDisplay = "none"
                                    NSFileLinkDisplay = "block"
                                    localQuoteAvail = "No";
                                    FileUploadDisplay = "block";
                                }

                                if (rowItem.row.data.Quote_Vendor_Type == "Specific") {
                                    VTSpecific = "checked";
                                    VTAny = "unchecked";
                                    SSJFileLinkDisplay = "block"
                                    NSFileLinkDisplay = "none"
                                    localVendorValue = 'Specific';
                                    FileUploadDisplay = "block";
                                } else if (rowItem.row.data.Quote_Vendor_Type == "Any") {
                                    VTSpecific = "unchecked";
                                    VTAny = "checked";
                                    SSJFileLinkDisplay = "none"
                                    NSFileLinkDisplay = "block"
                                    localVendorValue = 'Any';
                                    FileUploadDisplay = "block";
                                }
                                if (rowItem.row.data.Upload_File_Name.toString().length == 0) {
                                    fileListDisplay = "none"
                                    noFileTmpDisplay = "block"
                                } else {
                                    fileListDisplay = "block"
                                    noFileTmpDisplay = "none"
                                }
                                const myFileArray = rowItem.row.data.Upload_File_Name.split("||");

                                if (myFileArray != "") {
                                    //debugger;
                                    $ul = $('<ol style="font-size:12px;min-height:50px;max-height:80px;overflow:auto;">', { class: "mylist" }).append(
                                        myFileArray.map(country =>
                                            $("<li>").append($("<a>").text(country))
                                        )
                                    );
                                    ul = $ul[0].innerHTML;
                                } else {
                                    $ul = "";
                                    ul = "";
                                    noFileTmpDisplay = "block"
                                }

                                //popupContentTemplate = ShowPopup($ul, rowItem);
                                popupContentTemplate = function () {
                                    return $('<div style="font-size:12px;">').append(
                                        $(`<table style="border-collapse: collapse;width: 100%;">
                                              <tr style="border: 1px solid #dddddd;text-align: left;padding: 8px;background-color:#faebd7">
                                                <th style="border: 1px solid #dddddd;">DEPT </th>
                                                <th style="border: 1px solid #dddddd;">Item </th>
                                                <th style="border: 1px solid #dddddd;">Qty </th>
                                              </tr>
                                              <tr style="border: 1px solid #dddddd;text-align: left;padding: 8px;">
                                                <td style="border: 1px solid #dddddd;">`+ rowItem.row.cells[3].text + `</td>
                                                <td style="border: 1px solid #dddddd;">`+ rowItem.row.cells[5].text + `</td>
                                                <td style="border: 1px solid #dddddd;">`+ rowItem.row.cells[7].text + `</td>
                                              </tr>
                                           </table><br>
`),

                                        $(`<div style = "display:flex;"><div style="float:left;width:40%;"><p style="width=50%;">Quote Available: <span>
                                                                    <input type="radio" id="QAY" name="Quote_Available" onchange="handleChangeQuoteAvail(this);" `+ QAYes + ` style="transform: scale(1.3);margin-left: 10px;cursor:pointer"> <label style="padding: 8px;">Yes</label>
                                                                    <input type="radio" id="QAN" name="Quote_Available" onchange="handleChangeQuoteAvail(this);" `+ QANo + ` style="transform: scale(1.3);cursor:pointer"><label style="padding: 8px;">No</label>
                                                                </span>
                                            <span></span></p>
                                    <p id="vendorInputlayout" style="display:` + VTDisplay + `;margin-top:-20px;">Vendor : <span>
                                                                    <input type="radio" id="VTY" name="Vendor_type" onchange="handleChangeVendorType(this);" `+ VTSpecific + ` style="transform: scale(1.3);margin-left: 10px;cursor:pointer"> <label style="padding: 8px;">Specific</label>
                                                                    <input type="radio" id="VTN" name="Vendor_type" onchange="handleChangeVendorType(this);" `+ VTAny + ` style="transform: scale(1.3);cursor:pointer"><label style="padding: 8px;">Any</label>
                                                                </span>
                                            <span></span></p>
                                    <div style="display:` + fileListDisplay + `"> File Attached :</div>
                                    ` + ul + `
                                    <div style="display:` + noFileTmpDisplay + `;color:red;" id='fileList'>There is no file attached.</div>
                                    <div style="display:` + FileUploadDisplay + `;" id='FileUploadBtn'></div>
                                    <div style="display:` + SSJFileLinkDisplay + `" id='SSJFileLink'></div>
                                    <div style="display:` + NSFileLinkDisplay + `" id='NSFileLink'></div></div>
                                    <div style= "float:right;width:60%;"><div class="panel panel-default">
                                    <div class="panel-heading">Request Details</div><div class="panel-body">
                                    <table><tr><td><div style="font-size:12px;" class="dx-field">
                                    <div class="dx-field-label">From Date</div>
                                    <div class="dx-field-value">
                                    <div style="font-size:12px;width:50px;" id="fromDate"></div></div></td></tr><tr><td><div style="font-size:12px;"class="dx-field">
                                    <div style="font-size:12px;" class="dx-field-label">To Date</div>
                                    <div class="dx-field-value">
                                    <div style="font-size:12px;" id="toDate"></div>
                                    </div></td></tr><tr><td><div class="dx-field">
                                    <div style="font-size:12px;" class="dx-field-label">Requests</div>
                                    <div class="dx-field-value">
                                      <div style="font-size:12px;float:left;width:210px;" id="reqdropdown"></div>
                                    </div>
                                  </div></td></tr></table>
                                    
                                    </div></div>
                                    </div></div>`)
                                    );
                                };



                                const popup = $('#widget').dxPopup({
                                    contentTemplate: popupContentTemplate,
                                    width: '50vw',
                                    height: '85vh',
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
                                        //debugger;

                                        var now = new Date();
                                        var sdate, edate;
                                        edate = now.setDate(now.getDate());
                                        now = new Date();
                                        sdate = now.setDate(now.getDate() - 5);

                                        //sdate = fdate;
                                        //edate = tdate;

                                        $('#fromDate').dxDateBox({
                                            displayFormat: 'dd/MM/yyyy',
                                            type: 'date',
                                            value: sdate,
                                            inputAttr: { 'aria-label': 'Date' },
                                            onValueChanged(data) {
                                                //debugger;
                                                //sdate = new Date(data.value);
                                                sdate = data.value;

                                                if (sdate == null)
                                                    sdate = 0;
                                                //sdate = new Date(now.setDate(now.getDate() - 5));
                                                if (edate == null)
                                                    edate = 0;
                                                //edate = new Date(now.setDate(now.getDate()));
                                                ReloadRequests(RequestID, sdate, edate, openpopup);
                                            },
                                        });
                                        $('#toDate').dxDateBox({
                                            displayFormat: 'dd/MM/yyyy',
                                            type: 'date',
                                            value: edate,
                                            inputAttr: { 'aria-label': 'Date' },
                                            onValueChanged(data) {
                                                //debugger;
                                                //edate = new Date(data.value);
                                                edate = data.value;
                                                if (sdate == null)
                                                    sdate = 0;
                                                //sdate = new Date(now.setDate(now.getDate() - 5));
                                                if (edate == null)
                                                    edate = 0;
                                                //edate = new Date(now.setDate(now.getDate()));
                                                ReloadRequests(RequestID, sdate, edate, openpopup);
                                            },
                                        });

                                        if (sdate == null) {
                                            //debugger;
                                            sdate = 0;
                                            //sdate = new Date(now.setDate(now.getDate() - 5));
                                        }

                                        if (edate == null) {
                                            //debugger;
                                            edate = 0;
                                            //edate = new Date(now.setDate(now.getDate()));
                                        }



                                        ReloadRequests(RequestID, sdate, edate, openpopup);

                                        if (rowItem.row.data.QuoteAvailable == 'Yes') {
                                            if (rowItem.row.data.Quote_Vendor_Type == 'Specific') {
                                                localVendorValue = 'Specific';
                                            } else if (rowItem.row.data.Quote_Vendor_Type == 'Any') {
                                                localVendorValue = 'Any';
                                            }
                                        }
                                        var fileName = "Single Source Justification.doc";
                                        var url = "Templates/" + fileName;
                                        //SSJFileLink = document.getElementById("SSJFileLink");
                                        let downloadtemplate = $('<a/>').addClass('dx-link')
                                            .text('Click here to download SSJ template')
                                            .attr('href', url)
                                            .on('dxclick', function () {
                                                //Do something with options.data;  
                                                //debugger;
                                                $.ajax({
                                                    url: url,
                                                    cache: false,
                                                    xhr: function () {
                                                        var xhr = new XMLHttpRequest();
                                                        xhr.onreadystatechange = function () {
                                                            if (xhr.readyState == 2) {
                                                                if (xhr.status == 200) {
                                                                    xhr.responseType = "blob";
                                                                } else {
                                                                    xhr.responseType = "text";
                                                                }
                                                            }
                                                        };
                                                        return xhr;
                                                    },
                                                    success: function (data) {
                                                        //Convert the Byte Data to BLOB object.
                                                        var blob = new Blob([data], { type: "application/octetstream" });

                                                        //Check the Browser type and download the File.
                                                        var isIE = false || !!document.documentMode;
                                                        if (isIE) {
                                                            window.navigator.msSaveBlob(blob, fileName);
                                                        } else {
                                                            var url = window.URL || window.webkitURL;
                                                            link = url.createObjectURL(blob);
                                                            var a = $("<a />");
                                                            a.attr("download", fileName);
                                                            a.attr("href", link);
                                                            $("body").append(a);
                                                            a[0].click();
                                                            $("body").remove(a);
                                                        }
                                                    }
                                                });





                                            }).appendTo(SSJFileLink);

                                        var url2 = "Templates/Neutral Specification.doc";
                                        let downloadtemplate2 = $('<a/>').addClass('dx-link')
                                            .text('Click here to download NS template')
                                            .attr('href', url2)
                                            .on('dxclick', function () {
                                                //Do something with options.data;  
                                                //debugger;

                                                $.ajax({
                                                    url: url2,
                                                    cache: false,
                                                    xhr: function () {
                                                        var xhr = new XMLHttpRequest();
                                                        xhr.onreadystatechange = function () {
                                                            if (xhr.readyState == 2) {
                                                                if (xhr.status == 200) {
                                                                    xhr.responseType = "blob";
                                                                } else {
                                                                    xhr.responseType = "text";
                                                                }
                                                            }
                                                        };
                                                        return xhr;
                                                    },
                                                    success: function (data) {
                                                        //Convert the Byte Data to BLOB object.
                                                        var blob = new Blob([data], { type: "application/octetstream" });



                                                        //Check the Browser type and download the File.
                                                        var isIE = false || !!document.documentMode;
                                                        if (isIE) {
                                                            window.navigator.msSaveBlob(blob, 'Neutral Specification.doc');
                                                        } else {
                                                            var url = window.URL || window.webkitURL;
                                                            link2 = url.createObjectURL(blob);
                                                            var a = $("<a />");
                                                            a.attr("download", 'Neutral Specification.doc');
                                                            a.attr("href", link2);
                                                            $("body").append(a);
                                                            a[0].click();
                                                            $("body").remove(a);
                                                        }
                                                    }
                                                });





                                            }).appendTo(NSFileLink);

                                        fileUploaderFromPopup = $(FileUploadBtn).dxFileUploader({
                                            name: "file",
                                            multiple: true,
                                            //accept: "*",
                                            selectButtonText: "Attach a file.",
                                            allowedFileExtensions: ['.pdf', '.docx', '.doc'],
                                            uploadMode: "useForm",

                                            //uploadUrl: `${backendURL}AsyncFileUpload`,
                                            onValueChanged: function (e) {
                                                //debugger;

                                                var uploadedfilename = document.querySelector('.dx-fileuploader-files-container');
                                                var invalidFile = false;


                                                for (var i = 0; i < e.value.length; i++) {



                                                    if (localQuoteAvail == 'Yes' && localVendorValue == 'Specific') {

                                                        if (e.value[i].name.indexOf("Quot") != -1 || e.value[i].name.indexOf("Single Source Justification") != -1) {

                                                            file = e.value;

                                                            if (uploadedfilename != undefined) {

                                                                uploadedfilename.style.visibility = 'visible';

                                                                uploadedfilename.style.minHeight = "100px";
                                                                uploadedfilename.style.maxHeight = "150px";
                                                                uploadedfilename.style.overflow = "auto";

                                                                uploadedfilename.style.paddingTop = "0px";

                                                            }

                                                        }

                                                        else {

                                                            file = null;
                                                            invalidFile = true;

                                                            if (uploadedfilename != undefined) {

                                                                uploadedfilename.style.visibility = 'hidden';

                                                                uploadedfilename.style.height = "0px";

                                                                uploadedfilename.style.paddingTop = "0px";

                                                            }

                                                        }
                                                    } else {
                                                        if (e.value[i].name.indexOf("Quot") != -1 || e.value[i].name.indexOf("Neutral Specification") != -1) {

                                                            file = e.value;


                                                            if (uploadedfilename != undefined) {

                                                                uploadedfilename.style.visibility = 'visible';

                                                                uploadedfilename.style.minHeight = "100px";
                                                                uploadedfilename.style.maxHeight = "150px";
                                                                uploadedfilename.style.overflow = "auto";

                                                                uploadedfilename.style.paddingTop = "0px";

                                                            }

                                                        }

                                                        else {

                                                            file = null;

                                                            invalidFile = true;

                                                            if (uploadedfilename != undefined) {

                                                                uploadedfilename.style.visibility = 'hidden';

                                                                uploadedfilename.style.height = "0px";

                                                                uploadedfilename.style.paddingTop = "0px";

                                                            }

                                                        }
                                                    }

                                                }

                                                if (invalidFile) {
                                                    if (localVendorValue == "Specific") {
                                                        alert('Invalid file - Please upload SSJ template file');
                                                    } else {
                                                        alert('Invalid file - Please upload NS template file');
                                                    }

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

                                                if ((localQuoteAvail == "Yes" && localVendorValue != "") || localQuoteAvail == "No") {

                                                    if ((file != undefined && ul.length == 0) || (file != undefined && ul.length != 0)) {
                                                        //debugger;
                                                        rowItem.row.data.Quote_Vendor_Type = localVendorValue;
                                                        rowItem.row.data.QuoteAvailable = localQuoteAvail;
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
                                                        addnewitem_flag = false;
                                                        rowItem.row.data.BU = 6;
                                                        //e.data.RFOReqNTID = RFOReqNTID;
                                                        //rowItem.row.data.Total_Price = rowItem.row.data.Unit_Price * rowItem.row.data.Required_Quantity;
                                                        rowItem.row.data.Reviewed_Cost = rowItem.row.data.Unit_Price * rowItem.row.data.Reviewed_Quantity;
                                                        Selected = [];
                                                        Selected.push(rowItem.row.data);

                                                        //debugger;

                                                        Update(Selected, filtered_yr);
                                                        popup.hide();
                                                    }
                                                    else if (file == undefined && ul.length == 0) {
                                                        alert("Please upload the any file.")
                                                    }
                                                    else {
                                                        rowItem.row.data.Quote_Vendor_Type = localVendorValue;
                                                        rowItem.row.data.QuoteAvailable = localQuoteAvail;

                                                        addnewitem_flag = false;
                                                        rowItem.row.data.BU = 6;
                                                        //e.data.RFOReqNTID = RFOReqNTID;
                                                        //rowItem.row.data.Total_Price = rowItem.row.data.Unit_Price * rowItem.row.data.Required_Quantity;
                                                        rowItem.row.data.Reviewed_Cost = rowItem.row.data.Unit_Price * rowItem.row.data.Reviewed_Quantity;
                                                        Selected = [];
                                                        Selected.push(rowItem.row.data);

                                                        //debugger;

                                                        Update(Selected, filtered_yr);
                                                        popup.hide();
                                                    }

                                                } else {
                                                    alert("Please select vendor type.")
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
                        },
                            "edit",//, "delete",
                        {
                            hint: "Submit",
                            icon: "check",
                            visible: function (e) {
                                ////debugger;
                                return ((response.success && !e.row.isEditing && !(e.row.data.RequestToOrder)) || (!e.row.isEditing && isDelivered(e.row.data.OrderStatus))) && !response.isDashboard_flag;/*&& !isOrderApproved(e.row.data.OrderStatus)*/;

                            },
                            onClick: function (e) {
                                if (e.row.data.Upload_File_Name.toString().length > 0) {
                                    if (e.row.data.RequiredDate && !e.row.data.RequestToOrder) {

                                        var ReqdDate = e.row.data.RequiredDate;
                                        //                //debugger;
                                        $.ajax({

                                            type: "GET",
                                            url: "/BudgetingOrder/ValidateRequiredDate",
                                            data: { 'RequiredDate': ReqdDate },
                                            datatype: "json",
                                            async: false,
                                            success: success_validateReqdDate,

                                        });
                                        function success_validateReqdDate(info) {
                                            ////debugger;
                                            if (info) {
                                                $.notify(info, {
                                                    globalPosition: "top center",
                                                    className: "error"
                                                })
                                            }
                                            else {
                                                LabAdminApprove(e.row.data.RequestID, filtered_yr);
                                            }
                                        }

                                    }
                                    else {

                                        LabAdminApprove(e.row.data.RequestID, filtered_yr);
                                    }

                                    e.component.refresh(true);
                                    e.event.preventDefault();
                                } else {
                                    $.notify("No file attachment found. Please upload file before submit.", {
                                        globalPosition: "top center",
                                        className: "warn"
                                    })
                                }
                            }

                        }]
                    },
                    {

                        alignment: "center",
                        columns: [
                            {
                                dataField: "RequestID",
                                caption: "Request ID"
                            },
                            {
                                dataField: "BU",

                                width: 65,
                                /*validationRules: [{ type: "required" }],*/

                                lookup: {
                                    dataSource: BU_list,
                                    valueExpr: "ID",
                                    displayExpr: "BU"
                                },
                                headerFilter: {
                                    dataSource: BU_headerFilter,
                                    allowSearch: true
                                },
                                setCellValue: function (rowData, value) {
                                    //debugger;
                                    rowData.BU = value;
                                    rowData.Item_Name = null;

                                },

                            },
                            {
                                dataField: "OEM",
                                caption: "Customer",
                                label: {
                                    template: contentTemplate
                                    //template: (data, element) => {
                                    //    //debugger;
                                    //    const lineBreak = '<br>';
                                    //    const infoIcon = '<i id="helpedInfo" class="dx-icon dx-icon-info"></i>';
                                    //    const labelText = `Additional${lineBreak}${data.text}`;

                                    //    element.append(`<div id='template-content'>${infoIcon}${labelText}</div>`);

                                    //    $('<div>').dxTooltip({
                                    //        target: '#helpedInfo',
                                    //        showEvent: 'mouseenter',
                                    //        hideEvent: 'mouseleave',
                                    //        contentTemplate(args) {
                                    //            args.html('<div id="tooltip-content">This field must not exceed 200 characters</div>');
                                    //        },
                                    //    }).appendTo(element);
                                    //},
                                },
                                validationRules: [{ type: "required" }],
                                width: 70,
                                lookup: {
                                    dataSource: OEM_list,
                                    valueExpr: "ID",
                                    displayExpr: "OEM"
                                },
                                headerFilter: {
                                    dataSource: OEM_headerFilter,
                                    allowSearch: true
                                },


                            },
                            {
                                dataField: "DEPT",
                                caption: "Dept",
                                validationRules: [{ type: "required" }],
                                headerFilter: {
                                    dataSource: DEPT_headerFilter,
                                    allowSearch: true
                                },
                                setCellValue: function (rowData, value, currentRowData) {

                                    if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                    {
                                        //debugger;
                                        rowData.DEPT = value;
                                        rowData.Group = null;
                                        rowData.RFOApprover = null;
                                        if (currentRowData.UnloadingPoint != undefined) {
                                            var unloadingPt_Location = UnloadingPoint_list.find(x => x.ID == currentRowData.UnloadingPoint).Location;
                                            var dept_name = DEPT_list.find(x => x.ID == rowData.DEPT).DEPT;
                                            rowData.CostCenter = CostCenter_list.find(x => x.UnloadingPoint_Location == unloadingPt_Location && x.Dept == dept_name).CostCenter;

                                        }


                                    }

                                },
                                width: 90,
                                lookup: {
                                    dataSource: function (options) {
                                        ////debugger;
                                        return {

                                            store: DEPT_list,
                                            filter: options.data ? ["Outdated", "=", false] : null

                                        };
                                    },

                                    valueExpr: "ID",
                                    displayExpr: "DEPT"

                                },


                            },
                            {
                                dataField: "Group",
                                width: 90,
                                headerFilter: {
                                    dataSource: Group_headerFilter,
                                    allowSearch: true
                                },
                                validationRules: [{ type: "required" }],
                                lookup: {
                                    dataSource: function (options) {
                                        //debugger;
                                        return {

                                            store: Group_list,

                                            filter: options.data ? ["Dept", "=", options.data.DEPT] : null
                                        };

                                    },
                                    valueExpr: "ID",
                                    displayExpr: "Group"
                                },
                                setCellValue: function (rowData, value) {
                                    //debugger;
                                    if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                    {
                                        rowData.Group = value;
                                        //rowData.RfOApprover = value;
                                        rowData.RFOApprover = Group_list.find(x => x.ID == value).Group;
                                    }
                                }


                            },


                            {
                                dataField: "Item_Name",
                                width: 500,
                                headerFilter: {
                                    dataSource: Item_headerFilter,
                                    allowSearch: true
                                },
                                validationRules: [{ type: "required" }],
                                lookup: {
                                    dataSource: function (options) {
                                        debugger;
                                        return {

                                            store: Item_list,
                                            paginate: true, //enable paging when list has huge amount of data, else takes more time to load
                                            pageSize: 30 ,
                                            //filter: function (e) {
                                            //    //debugger;
                                            //    if (e.BU == Item_list.find(x => x.S_No).BU && e.OrderType == Item_list.find(x => x.S_No).OrderType) {
                                            //        return true;
                                            //    }
                                            //}

                                            filter: options.data ? [["BU", "=", options.data.BU], 'and', ["Order_Type", "=", options.data.OrderType], 'and', ["Deleted", "=", false]] : null

                                        }

                                    },
                                    valueExpr: "S_No",
                                    displayExpr: "Item_Name",
                                     
                                },
                                calculateSortValue: function (data) {
                                    ////debugger;
                                    const value = this.calculateCellValue(data);
                                    return this.lookup.calculateCellValue(value);
                                },
                                setCellValue: function (rowData, value) {
                                    //debugger;
                                    //if value.constructur.name == "Array" => it means that the item list is filtered based on the ordertype selected. At that instance, no need to fetch other details
                                    if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                    {
                                        rowData.Item_Name = value;
                                        rowData.Category = Item_list.find(x => x.S_No == value).Category;
                                        rowData.Cost_Element = parseInt(Item_list.find(x => x.S_No == value).Cost_Element);
                                        rowData.Unit_Price = Item_list.find(x => x.S_No == value).UnitPriceUSD.toFixed(2);
                                        //rowData.ActualAvailableQuantity = Item_list.find(x => x.S_No == value).Actual_Available_Quantity;
                                        rowData.BudgetCode = Item_list.find(x => x.S_No == value).BudgetCode;
                                        rowData.BudgetCodeDescription = BudgetCodeList.find(x => x.Budget_Code == rowData.BudgetCode).Budget_Code_Description;
                                        rowData.UnitofMeasure = parseInt(Item_list.find(x => x.S_No == value).UOM);
                                    }
                                },

                            },
                            {
                                dataField: "OrderType",
                                caption: "Order Type",
                                setCellValue: function (rowData, value) {
                                    //debugger;
                                    rowData.OrderType = value;
                                    rowData.Item_Name = null;

                                },
                                lookup: {
                                    dataSource: function (options) {
                                        //debugger;
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
                                        //debugger;
                                        return {

                                            store: BudCenter,
                                        }

                                    },
                                    valueExpr: "ID",
                                    displayExpr: "BudgetCenter",
                                },

                                visible: false,
                                validationRules: [{ type: "required" }],
                                //allowEditing: false,

                            },
                            {
                                dataField: "UnitofMeasure",
                                caption: "Unit of Measure",
                                lookup: {
                                    dataSource: function (options) {
                                        //debugger;
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
                                setCellValue: function (rowData, value, currentRowData) {
                                    //debugger;
                                    //if value.constructur.name == "Array" => it means that the item list is filtered based on the ordertype selected. At that instance, no need to fetch other details
                                    if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                                    {
                                        rowData.UnloadingPoint = value;
                                        var unloadingPt_Location = UnloadingPoint_list.find(x => x.ID == value).Location;
                                        var dept_name = DEPT_list.find(x => x.ID == currentRowData.DEPT).DEPT;
                                        rowData.CostCenter = CostCenter_list.find(x => x.UnloadingPoint_Location == unloadingPt_Location && x.Dept == dept_name).CostCenter;
                                        //Item_list.find(x => x.S_No == value).Category;

                                    }
                                },
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
                            {
                                dataField: "Category",
                                caption: "Category",
                                validationRules: [{ type: "required" }],
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
                            {
                                dataField: "Cost_Element",
                                //headerFilter: {
                                //    //dataSource: CostElement_headerFilter,
                                //    allowSearch: true
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
                                headerFilter: {
                                    dataSource: BudgetCode_headerFilter,
                                    allowSearch: true
                                },
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
                                visible: false,
                                lookup: {
                                    dataSource: RFOApprover_list,
                                    valueExpr: "Section_Dept_Grp",
                                    displayExpr: "Section_Dept_Grp"
                                },

                            },
                            {
                                dataField: "GoodsRecID",
                                caption: "Goods Rec ID",
                                visible: false

                            },
                            {
                                dataField: "Required_Quantity",
                                caption: "Required Qty",

                                dataType: "number",
                                setCellValue: function (rowData, value) {

                                    rowData.Required_Quantity = value;

                                },
                                //allowEditing: false,
                                visible: false


                            },
                            {
                                dataField: "Reviewed_Quantity",
                                caption: "Review Qty",

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

                                    rowData.Reviewed_Quantity = value;

                                },


                            },
                            //{
                            //    dataField: "ActualAvailableQuantity",
                            //    caption: "Available Qty",
                            //    allowEditing: false,
                            //    width: 102
                            //},
                            {
                                dataField: "Unit_Price",
                                caption: "Unit Price",
                                dataType: "number",
                                format: { type: "currency", precision: 2 },
                                valueFormat: "#0.00",

                                validationRules: [{ type: "required" }, {
                                    type: "range",
                                    message: "Please enter valid price > 0",
                                    min: 0.01,
                                    max: Number.MAX_VALUE
                                }],
                                allowEditing: false,
                                visible: false


                            },
                            //   {
                            //    dataField: "RQuantity",
                            //    caption: "Qty",

                            //    dataType: "number",
                            //    setCellValue: function (rowData, value) {

                            //        rowData.Quantity = value;

                            //    },
                            //    //allowEditing: false,
                            //    visible: false


                            //},

                            {
                                dataField: "Total_Price",
                                caption: "Required Cost",

                                calculateCellValue: function (rowData) {

                                    if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
                                        return (rowData.Unit_Price * rowData.Required_Quantity).toFixed(2);
                                    }
                                    else
                                        return 0.0;
                                },

                                dataType: "number",
                                format: { type: "currency", precision: 2 },
                                valueFormat: "#0.00",
                                allowEditing: false,
                                visible: false
                            },
                            {
                                dataField: "Reviewed_Cost",

                                calculateCellValue: function (rowData) {

                                    //if (rowData.Reviewed_Quantity > 0 && rowData.Unit_Price > 0) {
                                    //    return rowData.Unit_Price * rowData.Reviewed_Quantity;
                                    //}
                                    //else
                                    //    return 0.0

                                    //if ((rowData.Reviewed_Cost == null || rowData.Reviewed_Cost == undefined) && rowData.Reviewed_Quantity > 0 && rowData.Unit_Price > 0) {
                                    //    return rowData.Unit_Price * rowData.Reviewed_Quantity;
                                    //}
                                    //else if (rowData.Reviewed_Cost != null || rowData.Reviewed_Cost != undefined) {
                                    //    return rowData.Reviewed_Cost;
                                    //}
                                    //else
                                    //    return 0.0;;

                                    if (rowData.Reviewed_Quantity > 0 && rowData.Unit_Price > 0) {
                                        return (rowData.Unit_Price * rowData.Reviewed_Quantity).toFixed(2);
                                    }
                                    else
                                        return 0.0;
                                },

                                dataType: "number",
                                format: { type: "currency", precision: 2 },
                                valueFormat: "#0.00",
                                allowEditing: false
                            },

                            {
                                dataField: "Requestor",
                                allowEditing: false,
                                visible: false
                            },
                            {
                                dataField: "Reviewer_1",
                                allowEditing: false,
                                visible: false
                            },
                            {
                                dataField: "Reviewer_2",
                                allowEditing: false,
                                visible: false
                            },
                            {
                                dataField: "SubmitDate",
                                allowEditing: false,
                                visible: false
                            },
                            {
                                dataField: "RequiredDate",
                                dataType: "date",
                                visible: true

                            },
                            {
                                dataField: "RequestOrderDate",
                                dataType: "date",
                                allowEditing: false,
                                visible: false

                            },
                            {
                                dataField: "OrderDate",
                                dataType: "date",
                                allowEditing: false,
                                visible: true

                            },
                            {
                                dataField: "TentativeDeliveryDate",
                                dataType: "date",
                                allowEditing: false,
                                visible: false

                            },
                            {
                                dataField: "Comments",
                                visible: false,
                                //allowEditing: false,
                            },
                            {
                                dataField: "PORemarks",
                                width: 140,
                                caption: "Item Justification"

                            },
                            {
                                dataField: "PRNumber",
                                width: 140,
                                caption: "PR Number"
                            },
                            {
                                dataField: "OrderID",
                                width: 140,
                                caption: "PO Number"
                            },
                            //{
                            //    dataField: "LeadTime",
                            //    caption: "LeadTime (in days)",
                            //    allowEditing: false,
                            //    visible: true,
                            //    calculateCellValue: function (rowData) {
                            //        //update the LeadTime
                            //        if (rowData.Item_Name == undefined || rowData.Item_Name == null) {

                            //            leadtime1 = "";
                            //        }

                            //        else {

                            //            $.ajax({

                            //                type: "GET",
                            //                url: "/BudgetingOrder/GetLeadTime",
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
                                dataField: "OrderPrice",
                                dataType: "number",
                                format: { type: "currency", precision: 2 },
                                valueFormat: "#0.00",

                                //validationRules: [{ type: "required" }, {
                                //    type: "range",
                                //    message: "Please enter valid price > 0",
                                //    min: 0.01,
                                //    max: Number.MAX_VALUE
                                //}],
                                allowEditing: false,
                                //visible: false


                            },
                            {
                                dataField: "OrderedQuantity",
                                caption: "Ordered Qty",
                                visible: false,
                                // allowEditing: flag || !e.row.data.ApprovedSH 
                                allowEditing: false



                            },
                            {
                                dataField: "OrderStatus",

                                setCellValue: function (rowData, value) {

                                    rowData.OrderStatus = value;


                                },
                                lookup: {
                                    dataSource: function (options) {

                                        return {

                                            store: OrderStatus_list,
                                            filter: options.data ? ["OrderStatus", "=", "Closed"] : null


                                        };
                                    },

                                    valueExpr: "ID",
                                    displayExpr: "OrderStatus",

                                }
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
                            },
                            {
                                dataField: "ActualDeliveryDate",
                                dataType: "date",
                                allowEditing: false,
                                visible: false

                            },
                            {
                                dataField: "Fund",

                                setCellValue: function (rowData, value) {
                                    //debugger;
                                    rowData.Fund = value;
                                    if (value == 3)//Fund = F03 
                                    {
                                        //debugger;
                                        isFund_F03 = true;
                                        isFund_F05orF06 = false;
                                        var dxForm = $(".dx-form").dxForm('instance');
                                        dxForm.itemOption('F03', 'visible', isFund_F03);
                                        dxForm.itemOption('F05_and_F06', 'visible', isFund_F05orF06);
                                    }
                                    else if (value == 5 || value == 6) { //fund should be 5/6
                                        //debugger;
                                        isFund_F05orF06 = true;
                                        isFund_F03 = false;
                                        var dxForm = $(".dx-form").dxForm('instance');
                                        dxForm.itemOption('F05_and_F06', 'visible', isFund_F05orF06);
                                        dxForm.itemOption('F03', 'visible', isFund_F03);

                                    }
                                    else {
                                        isFund_F05orF06 = false;
                                        isFund_F03 = false;
                                        var dxForm = $(".dx-form").dxForm('instance');
                                        dxForm.itemOption('F05_and_F06', 'visible', isFund_F05orF06);
                                        dxForm.itemOption('F03', 'visible', isFund_F03);
                                    }
                                },

                                lookup: {
                                    dataSource: function (options) {

                                        return {

                                            store: Fund_list,


                                        };
                                    },

                                    valueExpr: "ID",
                                    displayExpr: "Fund",
                                    allowEditing: false

                                }
                            },
                            {
                                dataField: "BM_Number",
                                allowEditing: true,
                                visible: false

                            },
                            {
                                dataField: "PIF_ID",
                                allowEditing: true,
                                visible: false

                            },
                            {
                                dataField: "Task_ID",
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
                                dataField: "SupplierName_with_Address",
                                allowEditing: true,
                                visible: false

                            },
                            {
                                dataField: "Resource_Group_Id",
                                allowEditing: true,
                                visible: false
                            },

                        ]
                    }],
               

                onEditorPreparing: function (e) {



                    if (e.parentType === "dataRow" && e.dataField === "BU") {

                        if (e.row.data.ApprovedSH == undefined)
                            e.row.data.ApprovedSH = false;
                        if (e.row.data.RequestToOrder == true)
                            e.editorOptions.readOnly = true;
                        // to handle when RequestToOrder = false (request not yet triggered for ordering) - BU should be editable for f01,f03 req - initial req
                        else
                            e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH); //|| !addnewitem_flag;/*isValuepresent(e.value);*/
                    }
                    if (e.parentType === "dataRow" && e.dataField === "OEM") {
                        if (e.row.data.ApprovedSH == undefined)
                            e.row.data.ApprovedSH = false;
                        if (e.row.data.RequestToOrder == true)
                            e.editorOptions.readOnly = true;
                        else
                            e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
                    }
                    if (e.parentType === "dataRow" && e.dataField === "DEPT") {
                        if (e.row.data.ApprovedSH == undefined)
                            e.row.data.ApprovedSH = false;
                        if (e.row.data.RequestToOrder == true)
                            e.editorOptions.readOnly = true;
                        else
                            e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
                    }
                    if (e.parentType === "dataRow" && e.dataField === "Group") {
                        if (e.row.data.ApprovedSH == undefined)
                            e.row.data.ApprovedSH = false;
                        if (e.row.data.RequestToOrder == true)
                            e.editorOptions.readOnly = true;
                        else
                            e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
                    }
                    if (e.parentType === "dataRow" && e.dataField === "Item_Name") {
                        if (e.row.data.ApprovedSH == undefined)
                            e.row.data.ApprovedSH = false;
                        if (e.row.data.RequestToOrder == true)
                            e.editorOptions.readOnly = true;
                        else
                            e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
                    }
                    if (e.parentType === "dataRow" && e.dataField === "Reviewed_Quantity") {
                        if (e.row.data.ApprovedSH == undefined)
                            e.row.data.ApprovedSH = false;
                        if (e.row.data.RequestToOrder == true)
                            e.editorOptions.readOnly = true;
                        else
                            e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
                    }
                    if (e.parentType === "dataRow" && e.dataField === "Reviewed_Cost") {
                        if (e.row.data.ApprovedSH == undefined)
                            e.row.data.ApprovedSH = false;
                        if (e.row.data.RequestToOrder == true)
                            e.editorOptions.readOnly = true;
                        else
                            e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
                    }
                    if (e.parentType === "dataRow" && e.dataField === "Fund") {
                        if (e.row.data.ApprovedSH == undefined)
                            e.row.data.ApprovedSH = false;
                        if (e.row.data.RequestToOrder == true)
                            e.editorOptions.readOnly = true;
                        else
                            e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH && !popupedit);
                    }
                    if (e.parentType === "dataRow" && e.dataField === "OrderStatus") {

                        e.editorOptions.readOnly = !isDelivered(e.row.data.OrderStatus);
                    }
                    if (e.parentType === "dataRow" && e.dataField === "RequiredDate") {
                        //debugger;
                        e.editorOptions.readOnly = e.row.data.RequestToOrder && (!isDelivered(e.row.data.OrderStatus) || isF03F01(e.row.data.Fund));  //1 & (0  || fo3)
                    }
                    if (e.parentType === "dataRow" && e.dataField === "Required_Quantity") {
                        if (e.row.data.ApprovedSH == undefined)
                            e.row.data.ApprovedSH = false;
                        if (e.row.data.RequestToOrder == true)
                            e.editorOptions.readOnly = true;
                        else
                            e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
                    }
                    if (e.parentType === "dataRow" && e.dataField === "BudgetCenterID") {
                        //debugger;
                        if (e.row.data.BudgetCenterID.constructor.name == "Number" && e.row.data.BudgetCenterID != 0)
                            e.editorOptions.readOnly = true;
                    }

                    var component = e.component,
                        rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

                    if (e.parentType === "dataRow" && e.dataField === "Group") {

                        e.editorOptions.disabled = (typeof e.row.data.DEPT !== "number");
                        if (e.editorOptions.disabled)
                            e.editorOptions.placeholder = 'Select Dept first';
                        if (!e.editorOptions.disabled)
                            e.editorOptions.placeholder = 'Select Group';

                    }


                    if (e.dataField === "DEPT" /*|| e.dataField === "UnloadingPoint"*/) {

                        var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                        e.editorOptions.onValueChanged = function (e) {
                            onValueChanged.call(this, e);
                            /*var upid = component.cellValue(rowIndex, "UnloadingPoint");*/
                            //var deptid = component.cellValue(rowIndex, "DEPT");
                            /*if (e.value != undefined && e.value != null && e.value != "") {*/
                            //debugger;
                            //$.ajax({

                            //    type: "post",
                            //    url: "/BudgetingOrder/GetCostCenter",
                            //    data: { 'deptid': deptid, 'upid': upid },
                            //    datatype: "json",
                            //    traditional: true,
                            //    success: function (data) {
                            //        //debugger;
                            //        CostCenter = data.data;
                            //        window.setTimeout(function () {
                            //            //debugger;
                            //            component.cellValue(rowIndex, "CostCenter", CostCenter);

                            //        }, 1000);

                            //    }

                            //})

                            $.ajax({

                                type: "post",
                                url: "/BudgetingOrder/GetRFOBudgetCenter",
                                data: { 'deptid': e.value },
                                datatype: "json",
                                traditional: true,
                                success: function (data) {
                                    //debugger;
                                    BudCenter = data.data;
                                    if (BudCenter.length == 0) {
                                        //debugger;
                                        //$.notify("Please Contact ELO Team to find BudgetCenter details !", {
                                        //    elementPosition: "right",
                                        //    className: "error",
                                        //    autoHideDelay: 13000,
                                        //});
                                        alert("Please Contact ELO Team to find BudgetCenter details !");
                                        //debugger;
                                    }
                                    else {
                                        window.setTimeout(function () {
                                            //debugger;
                                            component.cellValue(rowIndex, "BudgetCenterID", BudCenter);


                                        }, 1000);
                                    }

                                }
                            })
                            $.ajax({

                                type: "post",
                                url: "/BudgetingRequest/GetReviewer",
                                data: { DEPT: e.value, BU: component.cellValue(rowIndex, "BU") },
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
                            window.setTimeout(function () {
                                ////debugger;
                                /*  component.cellValue(rowIndex, "Reviewer_1", reviewer_1);*/
                                component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
                            }, 1000);


                            //    // Emulating a web service call
                            //window.setTimeout(function () {
                            //    //debugger;
                            //        component.cellValue(rowIndex, "BudgetCenterID", BudCenter);
                            //        //component.cellValue(rowIndex, "CostCenter", CostCenter);

                            //}, 1000);
                        }
                        //else {
                        //    window.setTimeout(function () {
                        //        component.cellValue(rowIndex, "BudgetCenter", "");

                        //    }, 1000);
                        //}

                    }


                    if (e.dataField === "RFOApprover") {
                        var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                        e.editorName = "dxAutocomplete";
                        //e.editorName.cellInfo.value = e.value;
                        //e.editorOptions.placeholder = 'Select Emp Name';
                        e.editorOptions.onValueChanged = function (e) {
                            ////debugger;
                            onValueChanged.call(this, e);
                            //debugger;
                        }
                    }


                    //if (e.dataField === "BU") {

                    //    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                    //    e.editorOptions.onValueChanged = function (e) {
                    //        onValueChanged.call(this, e);


                    //        $.ajax({

                    //            type: "post",
                    //            url: "/BudgetingOrder/GetReviewer",
                    //            data: { BU: e.value },
                    //            datatype: "json",
                    //            traditional: true,
                    //            success: function (data) {

                    //                reviewer_2 = data;

                    //            }
                    //        })
                    //        // Emulating a web service call
                    //        window.setTimeout(function () {
                    //            component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
                    //            //component.cellValue(rowIndex, "Item_Name", Item_list);
                    //        }, 1000);
                    //    }
                    //}

                    if (e.dataField === "GoodsRecID") {
                        var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                        e.editorOptions.onValueChanged = function (e) {
                            onValueChanged.call(this, e);
                            var FullName, Department, Group, Ntid;

                            //debugger;

                            $.ajax({

                                type: "post",
                                url: "/BudgetingOrder/ValidateGoodsRecID", //GetRequestorDetails_Planning_EMProxy
                                data: { NTID: e.value },
                                datatype: "json",
                                traditional: true,
                                success: function (data) {

                                    //debugger;
                                    if (data.success) {
                                        Ntid = data.data.NTID;
                                        window.setTimeout(function () {
                                            component.cellValue(rowIndex, "GoodsRecID", Ntid);
                                        }, 1000);

                                    }
                                    else {
                                        //$.notify(data.message, {
                                        //    globalPosition: "top center",
                                        //    className: "error"
                                        //})
                                        //DevExpress.ui.notify({
                                        //    message: "Goods Reciepient ID is not valid",
                                        //    className: "error"
                                        //});
                                        DevExpress.ui.notify({
                                            message: "Goods Recipient ID is not valid",
                                            className: "error",
                                            width: 500,
                                            position: {
                                                my: "top",
                                                at: "top",
                                                of: "#container"
                                            }
                                        });
                                    }

                                    //debugger;


                                }
                            })
                            // Emulating a web service call

                            //debugger;

                        }
                    }
                    //if (e.dataField === "OrderType") {



                    //    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                    //    e.editorOptions.onValueChanged = function (e) {
                    //        onValueChanged.call(this, e);

                    //        //alert("changed ordertype")
                    //        //var buselected = component.cellValue(rowIndex, "BU");
                    //        var buselected = 6;
                    //        var ordert = component.cellValue(rowIndex, "OrderType");
                    //        if (buselected != undefined && buselected != null && buselected != "" && ordert != undefined && ordert != null && ordert != "") {
                    //            //debugger;

                    //            //$.ajax({

                    //            //    type: "post",
                    //            //    url: "/BudgetingOrder/GetUOM",
                    //            //    data: { 'ordert': ordert },
                    //            //    datatype: "json",
                    //            //    traditional: true,
                    //            //    success: function (data) {
                    //            //        //debugger;
                    //            //        UOMlist = data.data;

                    //            //    }
                    //            //})

                    //            //$.ajax({
                    //            //    //type: "post",
                    //            //    url: "/BudgetingOrder/GetItemName",
                    //            //    data: { 'buselected': buselected, 'ordert': ordert },
                    //            //    datatype: "json",
                    //            //    async: false,
                    //            //    traditional: true,
                    //            //    success: function (data) {
                    //            //        //debugger;
                    //            //        Item_list = data.data;

                    //            //    }
                    //            //})


                    //            //window.setTimeout(function () {
                    //            //    //debugger;
                    //            //    component.cellValue(rowIndex, "Item_Name", Item_list);
                    //            //    //component.cellValue(rowIndex, "UnitofMeasure", UOMlist);

                    //            //},
                    //            //    1000);
                    //        }
                    //        else {

                    //            //debugger;
                    //            component.cellValue(rowIndex, "Item_Name", "");
                    //            //component.cellValue(rowIndex, "UnitofMeasure", "");

                    //        }
                    //    }
                    //}


                    //if (e.dataField === "Item_Name") {

                    //    //debugger;

                    //    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                    //    e.editorOptions.onValueChanged = function (e) {
                    //        onValueChanged.call(this, e);

                    //        $.ajax({
                    //            type: "post",
                    //            url: "/BudgetingOrder/GetUnitPrice",
                    //            data: { ItemName: e.value },
                    //            datatype: "json",
                    //            traditional: true,
                    //            success: function (data) {

                    //                if (data > 0)
                    //                    unitprice = data;

                    //            }
                    //        })

                    //        $.ajax({

                    //            type: "post",
                    //            url: "/BudgetingOrder/GetCategory",
                    //            data: { ItemName: e.value },
                    //            datatype: "json",
                    //            traditional: true,
                    //            success: function (data) {
                    //                category = data;

                    //            }
                    //        })

                    //        $.ajax({

                    //            type: "post",
                    //            url: "/BudgetingOrder/GetCostElement",
                    //            data: { ItemName: e.value },
                    //            datatype: "json",
                    //            traditional: true,
                    //            success: function (data) {
                    //                costelement = data;

                    //            }
                    //        })

                    //        //var BudgetCodedesc;
                    //        //debugger;
                    //        $.ajax({

                    //            type: "post",
                    //            url: "/BudgetingOrder/GetBudgetCode",
                    //            data: { ItemName: e.value },
                    //            datatype: "json",
                    //            traditional: true,
                    //            success: function (data) {
                    //                //debugger;
                    //                BudgetCode = data.data;
                    //                BudgetCodedesc = data.BudgetCodedesc;

                    //            }
                    //        })



                    //        window.setTimeout(function () {

                    //            //debugger;
                    //            component.cellValue(rowIndex, "Unit_Price", unitprice);
                    //            component.cellValue(rowIndex, "Category", category);
                    //            component.cellValue(rowIndex, "Cost_Element", costelement);
                    //            component.cellValue(rowIndex, "BudgetCode", BudgetCode);
                    //            component.cellValue(rowIndex, "BudgetCodeDescription", BudgetCodedesc);

                    //        },
                    //            1000);


                    //    }

                    //}

                },
                onRowUpdated: function (e) {
                    $.notify(" Your Item Request is being Updated...Please wait!", {
                        globalPosition: "top center",
                        className: "success"
                    })
                    Selected = [];
                    //var LeadTime_tocalc_ExpReqdDt;
                    ////debugger;
                    // e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
                    // e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
                    if (e.data.RequiredDate && !e.data.RequestToOrder) {

                        //    $.ajax({

                        //        type: "GET",
                        //        url: "/BudgetingOrder/GetLeadTime",
                        //        data: { 'ItemName': e.data.Item_Name },
                        //        datatype: "json",
                        //        async: false,
                        //        success: success_getleadtime,

                        //    });

                        //    function success_getleadtime(response) {
                        //        //debugger;
                        //        if (response == 0) {
                        //            LeadTime_tocalc_ExpReqdDt = "";
                        //            Selected.push(e.data);
                        //            //debugger;
                        //            Update(Selected, filtered_yr);
                        //        }
                        //        else
                        //        {
                        //            LeadTime_tocalc_ExpReqdDt = response;     
                        var ReqdDate = e.data.RequiredDate;
                        //            //debugger;
                        $.ajax({

                            type: "GET",
                            url: "/BudgetingOrder/ValidateRequiredDate",
                            data: { /*'LeadTime': LeadTime_tocalc_ExpReqdDt,*/ 'RequiredDate': ReqdDate },
                            datatype: "json",
                            async: false,
                            success: success_validateReqdDate,

                        });
                        function success_validateReqdDate(info) {
                            ////debugger;
                            if (info) {
                                $.notify(info, {
                                    globalPosition: "top center",
                                    className: "error"
                                })
                            }
                            else {
                                Selected.push(e.data);
                                //debugger;
                                Update(Selected, filtered_yr);
                            }
                        }

                        //        }


                        //    }


                    }
                    else {
                        Selected.push(e.data);
                        ////debugger;
                        Update(Selected, filtered_yr);

                    }

                },

                onRowInserting: function (e) {
                    addnewitem_flag = false;
                    e.data.BU = 6;
                    //e.data.RFOReqNTID = RFOReqNTID;
                    //e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
                    e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
                    Selected = [];
                    Selected.push(e.data);

                    //debugger;

                    Update(Selected, filtered_yr);
                },
                onRowRemoving: function (e) {

                    Delete(e.data.RequestID, filtered_yr);

                },
                //masterDetail: {
                //    enabled: true,

                //    template(container, options) {
                //        debugger;
                //        if (options.data.OrderDate != "" && options.data.OrderDate != null && options.data.OrderDate != undefined) {
                //            const currentRequestData = options.data;

                //            $('<div>')
                //                .addClass('master-detail-caption')
                //                .text(`${currentRequestData.Item_Name} Purchase details:`)
                //                .appendTo(container);

                //            $('<div>')
                //                .dxDataGrid({
                //                    columnAutoWidth: true,
                //                    showBorders: true,
                //                    headerFilter: {
                //                        visible: true,
                //                        applyFilter: "auto"
                //                    },
                //                    searchPanel: {
                //                        visible: true,
                //                        width: 240,
                //                        placeholder: "Search..."
                //                    },
                //                    columns: [

                //                        {

                //                            alignment: "center",
                //                            columns: [

                //                                {
                //                                    dataField: "RequestID",
                //                                    allowEditing: false,
                //                                    visible: false
                //                                },

                //                                //{
                //                                //    dataField: "Unit_Price",
                //                                //    caption: "Unit Price",
                //                                //    dataType: "number",
                //                                //    format: { type: "currency", precision: 0 },
                //                                //    valueFormat: "#0",
                //                                //    allowEditing: false,
                //                                //    validationRules: [{ type: "required" }, {
                //                                //        type: "range",
                //                                //        message: "Please enter valid price > 0",
                //                                //        min: 0.01,
                //                                //        max: Number.MAX_VALUE
                //                                //    }],
                //                                //    allowEditing: false,
                //                                //    visible: false


                //                                //},
                //                                //{
                //                                //    dataField: "Total_Price",
                //                                //    width: 100,
                //                                //    calculateCellValue: function (rowData) {

                //                                //        if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
                //                                //            return rowData.Unit_Price * rowData.Required_Quantity;
                //                                //        }
                //                                //        else
                //                                //            return 0.0;
                //                                //    },

                //                                //    dataType: "number",
                //                                //    format: { type: "currency", precision: 0 },
                //                                //    valueFormat: "#0",
                //                                //    allowEditing: false
                //                                //},
                //                                , {
                //                                    dataField: "ItemDescription",
                //                                    width: 250
                //                                },
                //                                {
                //                                    dataField: "PONumber",
                //                                    caption: "PO",
                //                                    allowEditing: false,
                //                                },



                //                                , {
                //                                    dataField: "BudgetCode",
                //                                    allowEditing: false,
                //                                    visible: false
                //                                },


                //                                {
                //                                    dataField: "POQuantity",
                //                                    allowEditing: false,
                //                                    caption: "PO Qty"
                //                                },

                //                                {
                //                                    dataField: "Netvalue_USD",
                //                                    allowEditing: false,
                //                                    visible: false
                //                                },


                //                                {
                //                                    dataField: "POCreatedOn",
                //                                    caption: "Order Dt",
                //                                    dataType: "date",
                //                                    allowEditing: true,
                //                                },



                //                                {
                //                                    dataField: "TentativeDeliveryDate",
                //                                    allowEditing: true,
                //                                    caption: "Tentative",

                //                                    dataType: "date",
                //                                },


                //                                {
                //                                    dataField: "ActualDeliveryDate",
                //                                    allowEditing: true,
                //                                    caption: "Actual Dt",
                //                                    dataType: "date",
                //                                },

                //                                , {
                //                                    dataField: "Currentstatus",
                //                                    caption: "Status",
                //                                    setCellValue: function (rowData, value) {
                //                                        //debugger;
                //                                        rowData.Fund = value;

                //                                    },
                //                                    lookup: {
                //                                        dataSource: function (options) {
                //                                            //debugger;
                //                                            return {

                //                                                store: OrderStatus_list,
                //                                            };

                //                                        },
                //                                        valueExpr: "ID",
                //                                        displayExpr: "OrderStatus"
                //                                    },
                //                                }

                //                            ]
                //                        }],
                //                    //dataSource: function (options1) {
                //                    //    //debugger;
                //                    //    return {

                //                    //        store: SubItemList,
                //                    //        filter: options1.data ? ['RequestID', '=', options.key] : null
                //                    //    };
                //                    //},
                //                    //dataSource: new DevExpress.data.DataSource({
                //                    //    store: SubItemList,
                //                    //    filter: ['RequestID', '=', options.key],
                //                    //}),
                //                    dataSource: new DevExpress.data.DataSource({
                //                        store: new DevExpress.data.ArrayStore({
                //                            key: 'ID',
                //                            data: SubItemList,
                //                        }),
                //                        filter: ['RequestID', '=', options.key],
                //                    }),
                //                }).appendTo(container);
                //        }

                //    },
                //}
            }).dxDataGrid("instance");
        });

        // }
        //else {
        //    //debugger;
        //    $.notify(response.message, {
        //        globalPosition: "top center",
        //        className: "error"
        //    })

        //    //Hide the Loading indicator once the Request List is fetched
        //    genSpinner_load.classList.remove('fa');
        //    genSpinner_load.classList.remove('fa-spinner');
        //    genSpinner_load.classList.remove('fa-pulse');
        //    document.getElementById("loadpanel").style.display = "none";
        //    $("#RequestTable_RFO").prop('hidden', false);
        //}




    }

    function ShowPopup($ul, rowItem) {
        //debugger;
        var parentdiv = $('<div id ="pdiv" style="font-size:12px;">');
        var maindiv = $('<div id ="mdiv" style="font-size:12px;">');
        var childdiv1 = $('<div id ="cdiv1" style="font-size:12px;width:50%;float:left;">');
        var childdiv2 = $('<div id ="cdiv2" style="font-size:12px;width:50%;float:right;">');
        //$('<div style="font-size:12px;">').append(
        $(`<table style="border-collapse: collapse;width: 100%;">
                                              <tr style="border: 1px solid #dddddd;text-align: left;padding: 8px;background-color:#faebd7">
                                                <th style="border: 1px solid #dddddd;">DEPT </th>
                                                <th style="border: 1px solid #dddddd;">Item </th>
                                                <th style="border: 1px solid #dddddd;">Qty </th>
                                              </tr>
                                              <tr style="border: 1px solid #dddddd;text-align: left;padding: 8px;">
                                                <td style="border: 1px solid #dddddd;">`+ rowItem.row.cells[3].text + `</td>
                                                <td style="border: 1px solid #dddddd;">`+ rowItem.row.cells[5].text + `</td>
                                                <td style="border: 1px solid #dddddd;">`+ rowItem.row.cells[7].text + `</td>
                                              </tr>
                                           </table><br>
`).appendTo(parentdiv);


        $(`<p style="">Quote Available: <span>
                                                                    <input type="radio" id="QAY" name="Quote_Available" onchange="handleChangeQuoteAvail(this);" `+ QAYes + ` style="transform: scale(1.3);margin-left: 10px;cursor:pointer"> <label style="padding: 8px;">Yes</label>
                                                                    <input type="radio" id="QAN" name="Quote_Available" onchange="handleChangeQuoteAvail(this);" `+ QANo + ` style="transform: scale(1.3);cursor:pointer"><label style="padding: 8px;">No</label>
                                                                </span>
                                            <span></span></p>`).appendTo(childdiv1);
        $(`<p id="vendorInputlayout" style="display:` + VTDisplay + `;margin-top:-20px;">Vendor : <span>
                                                                    <input type="radio" id="VTY" name="Vendor_type" onchange="handleChangeVendorType(this);" `+ VTSpecific + ` style="transform: scale(1.3);margin-left: 10px;cursor:pointer"> <label style="padding: 8px;">Specific</label>
                                                                    <input type="radio" id="VTN" name="Vendor_type" onchange="handleChangeVendorType(this);" `+ VTAny + ` style="transform: scale(1.3);cursor:pointer"><label style="padding: 8px;">Any</label>
                                                                </span>
                                            <span></span></p>`).appendTo(childdiv1);
        $(`<div style="display:` + fileListDisplay + `"> File Attached</div>`).appendTo(childdiv1);
        $ul.appendTo(childdiv1);
        $(`<div style="display:` + noFileTmpDisplay + `;color:red;" id='fileList'>There is no file attached.</div>`).appendTo(childdiv1);
        $(`<div style="display:` + FileUploadDisplay + `;" id='FileUploadBtn'></div>`).appendTo(childdiv1);
        $(`<div style="display:` + SSJFileLinkDisplay + `" id='SSJFileLink'></div>`).appendTo(childdiv1);
        $(`<div style="display:` + NSFileLinkDisplay + `" id='NSFileLink'></div>`).appendTo(childdiv1);

        $(maindiv).appendTo(parentdiv);
        $(childdiv1).appendTo(maindiv);
        $(childdiv2).appendTo(maindiv);
        //);

        $(maindiv).appendTo(parentdiv);
        $(childdiv1).appendTo(maindiv);
    }

    function OnError_GetData(response) {
        $("#RequestTable_RFO").prop('hidden', false);
        $.notify(data.message, {
            globalPosition: "top center",
            className: "warn"
        })
    }

}




$('#btnSubmitAll').click(function () {
    LabAdminApprove(1999999999, filtered_yr);
});

//Export data
$("#export").click(function () {
    debugger;
    var workbook = new ExcelJS.Workbook();
    var worksheet = workbook.addWorksheet("SheetName");
    var today = new Date();
    DevExpress.excelExporter.exportDataGrid({
        component: $("#RequestTable_RFO").dxDataGrid("instance"),
        worksheet: worksheet
    }).then(function () {
        workbook.xlsx.writeBuffer().then(function (buffer) {
            saveAs(new Blob([buffer], { type: "application/octet-stream" }), "VKM " + (parseInt(filtered_yr) +1) + ' ' + presentUserNTID + ' OrderList_' + today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear() + ".xlsx");

        });

    });

});



//$.ajax({

//    type: "post",
//    //url: "/BudgetingOrder/GetOrderType",
//    //data: { ItemName: e.value },
//    //datatype: "json",
//    //traditional: true,
//    url: encodeURI("../BudgetingOrder/GetOrderType"),
//    async: false,
//    success: function (data) {
//        //debugger;
//        OrderTypeList = data;
//        RFOReqNTID = data.presentUserNTID;

//    }
//})




//$.ajax({

//    type: "get",
//    //url: "/BudgetingOrder/GetOrderType",
//    //data: { ItemName: e.value },
//    //datatype: "json",
//    //traditional: true,
//    url: encodeURI("../BudgetingOrder/RFOApprover"),
//    async: false,
//    success: function (data) {
//        //debugger;
//        rfoapproverlist = data;

//    }
//})
//$.ajax({

//    type: "post",
//    url: encodeURI("../BudgetingOrder/GetUnloadingPoint"),
//    async: false,
//    success: function (data) {
//        //debugger;
//        UnloadingPointList = data;
//    }
//})

//$.ajax({

//    type: "post",
//    url: encodeURI("../BudgetingOrder/GetRFOBudgetCenter"),
//    data: { 'deptid': e.value },
//    async: false,
//    success: function (data) {
//        //debugger;
//        BudgetCenter = data;
//    }
//})

function LabAdminApprove(id, filtered_yr) {
    ////debugger;
    if (id == undefined) {
        $.notify('Please check the Fund and Try again later!', {
            globalPosition: "top center",
            autoHideDelay: 20000,
            className: "error"
        });
    }
    else {

        ////debugger;
        if (confirm('Do you confirm to place Request to Order the item(s)?')) {

            var genSpinner = document.querySelector("#SubmitSpinner");
            if (id == 1999999999) {
                genSpinner.classList.add('fa');
                genSpinner.classList.add('fa-spinner');
                genSpinner.classList.add('fa-pulse');
            }

            $.ajax({
                type: "POST",
                url: encodeURI("../BudgetingOrder/LabAdminApprove"),
                data: { 'id': id, 'useryear': filtered_yr },
                success: function (data) {

                    if (id == 1999999999) {

                        genSpinner.classList.remove('fa');
                        genSpinner.classList.remove('fa-spinner');
                        genSpinner.classList.remove('fa-pulse');
                    }




                    $.ajax({
                        type: "POST",
                        url: "/BudgetingOrder/GetData",
                        data: { 'year': filtered_yr },
                        datatype: "json",
                        async: true,
                        success: success_refresh_getdata,
                        error: error_refresh_getdata

                    });
                    function success_refresh_getdata(response) {

                        var getdata = response.data;
                        $("#RequestTable_RFO").dxDataGrid({
                            dataSource: getdata
                        });
                    }
                    function error_refresh_getdata(response) {

                        $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                            globalPosition: "top center",
                            className: "warn"
                        });

                    }

                    ////debugger;
                    if (data.is_MailTrigger) {
                        ////debugger;

                        $.ajax({
                            type: "POST",
                            url: encodeURI("../Budgeting/SendEmail_Order"),
                            data: { 'emailnotify': data.data, 'emailnotifyRFOApprover': data.emailnotifyRFOApprover },
                            success: success_email,
                            error: error_email
                        });

                        function success_email(response) {
                            $.notify("Mail has been sent to the LabTeam about your Request to Order!", {
                                globalPosition: "top center",
                                className: "success"
                            })

                        }
                        function error_email(response) {
                            //$.notify("Unable to send mail to the LabTeam about your Request to Order!!", {
                            //    globalPosition: "top center",
                            //    className: "warn"
                            //})

                        }
                    }



                    if (data.success) {

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
    }
}

function Update(id1, filtered_yr) {
    //debugger;

    //if (!id1[0].ApprovedSH && id1[0].Fund == 2) {
    //    ////debugger;
    //    $.notify('Cannot add F02 items right now since Request Window has been closed.' + '\n' + ' Only F01/F03 items can be added at this stage!', {
    //        globalPosition: "top center",
    //        autoHideDelay: 20000,
    //        className: "error"
    //    });
    //}
    //else {
    //var selectedValues = [];
    if (SelectedRequests == "") {
        //selectedValues = $('#reqdropdown').dxDropDownBox("option", "value"); //dropDownBox.option("selectedItemKeys");
        if ($('#reqdropdown').dxDropDownBox("option", "value") != null)
            SelectedRequests = $('#reqdropdown').dxDropDownBox("option", "value").join(",");
    }


    //debugger;
    //var checkedValues = selectedItems
    //    .filter(item => item.isSelected)
    //    .map(item => item.Value);
    ////debugger;
    //SelectedRequests = checkedValues;

    $.ajax({
        type: "POST",
        url: encodeURI("../BudgetingOrder/AddOrEdit"),
        data: { 'req': id1[0], 'useryear': filtered_yr, 'popupedit': popupedit, 'Requests': SelectedRequests }, //if popupedit is true => it is bgsw ordering wherein Reqd Dt in not filled - then auto fill with Current+10 weeks; else no need to auto-fill for CC
        success: function (data) {
            //debugger;
            if (data.success) {
                $.notify("Request saved successfully !", {
                    globalPosition: "top center",
                    className: "success"
                });
                $.ajax({
                    type: "POST",
                    url: "/BudgetingOrder/GetData",
                    data: { 'year': filtered_yr },
                    datatype: "json",
                    async: true,
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {

                    var getdata = response.data;
                    $("#RequestTable_RFO").dxDataGrid({
                        dataSource: getdata
                    });
                }
                function error_refresh_getdata(response) {
                    ////debugger;
                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }

            }
            else {

                if (data.RequestID == 0) {
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "error"
                    })
                }
                else {
                    $.notify("Please try again", {
                        globalPosition: "top center",
                        className: "error"
                    })
                }
                $.ajax({
                    type: "POST",
                    url: "/BudgetingOrder/GetData",
                    data: { 'year': filtered_yr },
                    datatype: "json",
                    async: true,
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {

                    var getdata = response.data;
                    $("#RequestTable_RFO").dxDataGrid({
                        dataSource: getdata
                    });
                }
                function error_refresh_getdata(response) {
                    ////debugger;
                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }

            }
            if (data.success) {

                //debugger;
                if (file != undefined) { //in CC edit, file will not be uploaded
                    var formdata = new FormData();
                    for (var i = 0; i < file.length; i++) {
                        formdata.append(file[i].name, file[i]);
                    }
                    formdata.append("id", data.RequestID);
                    if (SelectedRequests != "") {
                        formdata.append("Requests", SelectedRequests);
                    }
                    //formdata.append("image", ofile);
                    //formdata.append("id", editRowID == undefined ? 0 : editRowID);
                    //var x = new FormData(file[0]);
                    //debugger;
                    $.ajax({
                        type: "POST",
                        //dataType: "json",
                        //contentType: "multipart/form-data", //false,//"application/json; charset=utf-8;",
                        url: encodeURI("../BudgetingOrder/AsyncFileUpload"),
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
                            $.notify("File has been uploaded successfully!!", {
                                globalPosition: "top center",
                                className: "success"
                            })
                            file = null;


                        },



                        error: function (data) {
                            //InvAuth = false;
                            $.notify("Error in uploading file. Please try later!!", {
                                globalPosition: "top center",
                                className: "error"
                            })

                            //debugger;
                        }

                    });
                }

            }
            //debugger;
            if (savecont) {
                //debugger;
                let addRowButton = toolbarItems.find(x => x.name === "addRowButton");
                addRowButton.options.onClick();
                debugger

                function onsuccess_lookupdata(response) {
                    //debugger;
                    lookup_data = response.data;
                    BU_list = lookup_data.BU_List;
                    OEM_list = lookup_data.OEM_List;
                    DEPT_list = lookup_data.DEPT_List;
                    //Group_list = lookup_data.Groups_test;
                    //Item_list = lookup_data.Item_List;

                    Category_list = lookup_data.Category_List;
                    CostElement_list = lookup_data.CostElement_List;
                    OrderStatus_list = lookup_data.OrderStatus_List;
                    Fund_list = lookup_data.Fund_List;
                    BudgetCodeList = lookup_data.BudgetCodeList;
                    PurchaseType_list = lookup_data.PurchaseType_List;
                    UnloadingPoint_list = lookup_data.UnloadingPoint_List;
                    OrderType_list = lookup_data.Order_Type_List;
                    UOM_list = lookup_data.UOM_List;
                    RFOApprover_list = lookup_data.RFOApprover_List;
                    CostCenter_list = lookup_data.CostCenter_List;
                    BudCenter = lookup_data.BudgetCenter_List;


                    Item_headerFilter = lookup_data.Item_HeaderFilter;
                    DEPT_headerFilter = lookup_data.DEPT_HeaderFilter;
                    Group_headerFilter = lookup_data.Group_HeaderFilter;
                    BU_headerFilter = lookup_data.BU_HeaderFilter;
                    OEM_headerFilter = lookup_data.OEM_HeaderFilter;
                    Category_headerFilter = lookup_data.Category_HeaderFilter;
                    CostElement_headerFilter = lookup_data.CostElement_HeaderFilter;
                    OrderStatus_headerFilter = lookup_data.OrderStatus_HeaderFilter;
                    BudgetCode_headerFilter = lookup_data.BudgetCode_HeaderFilter;
                    //debugger;
                    //Item_list_custom = Item_list.filter(function (item) {
                    //    return (item.VKM_Year === curryear);
                    //});
                    //Item_list_New = lookup_data.Item_List1;
                    $.ajax({

                        type: "GET",
                        url: "/BudgetingOrder/Lookup_ItemList",
                        async: false,
                        data: { 'year': filtered_yr },
                        success: function (data) {
                            //debugger;
                            Item_list = data;
                        },
                        error: function (jqXHR, exception) {
                            //debugger;
                        }
                    })
                    $.ajax({

                        type: "GET",
                        url: "/BudgetingOrder/Lookup_GroupList",
                        async: false,
                        data: { 'year': filtered_yr },
                        success: onsuccess_lookupdata_Group,
                        error: onerror_lookupdata_Group
                    })


                }

                function onerror_lookupdata(response) {
                    alert("Error in fetching lookup");

                }
                function onsuccess_lookupdata_Group(response) {
                    //debugger;
                    Group_list = JSON.parse(response.groupList.Content);

                }

                function onerror_lookupdata_Group(response) {
                    //debugger;
                    alert("Error in fetching Group lookup");

                }


                //function onsuccess_lookupdata_Item(response) {
                //    //debugger;


                //    Item_list = JSON.parse(response.itemList.Content);

                //    //Item_list_custom = Item_list.filter(function (item) {
                //    //    return (item.VKM_Year === curryear);
                //    //});
                //    //Item_list_New = lookup_data.Item_List1;

                //}

                //function onerror_lookupdata_Item(response) {
                //    //debugger;
                //    alert("Error in fetching Item lookup");

                //}

                /****** To populate the user selection dropdown lists *******/
                $.ajax({

                    type: "GET",
                    url: "/BudgetingOrder/Lookup",
                    async: false,
                    data: { 'year': filtered_yr },
                    success: onsuccess_lookupdata,
                    error: onerror_lookupdata
                })


            }


            //debugger;

        }

    });


    //}


}

function Delete(id, filtered_yr) {

    $.ajax({
        type: "POST",
        url: "/BudgetingOrder/Delete",
        data: { 'id': id, 'useryear': filtered_yr },
        success: function (data) {
            newobjdata = data.data;
            $("#RequestTable_RFO").dxDataGrid({ dataSource: newobjdata });
        }



    });

    $.notify(data.message, {
        globalPosition: "top center",
        className: "success"
    })

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







//$("#buttonClearFilters").dxButton({
//    text: 'Clear Filters',
//    onClick: function () {
//        $("#RequestTable_RFO").dxDataGrid("clearFilter");
//    }
//});

$('[data-toggle="tooltip"]').tooltip();

//BULookup,OEMLookup,DeptLookup,GroupLookup,ItemNameLookup,CostElementLookup,CategoryLookup




////Export data
//$("#export").click(function () {
//    //debugger;
//    $.ajax({

//        type: "POST",
//        url: "/BudgetingOrder/ExportDataToExcel/",
//        data: { 'useryear': filtered_yr },


//        success: function (export_result) {
//            //debugger;

//            var bytes = new Uint8Array(export_result.FileContents);
//            var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
//            var link = document.createElement('a');
//            link.href = window.URL.createObjectURL(blob);
//            link.download = export_result.FileDownloadName;
//            link.click();

//        },
//        error: function () {
//            alert("export error");
//        }

//    });
//});


//$('#chkRequest').on('click', function () {
//    var chkRequest;
//    if (this.checked)
//        chkRequest = true;
//    else
//        chkRequest = false;
//    var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
//    dataGridLEP1.beginUpdate();
//    dataGridLEP1.columnOption('OEM', 'visible', chkRequest);
//    dataGridLEP1.columnOption('Required_Quantity', 'visible', chkRequest);
//    dataGridLEP1.columnOption('Total_Price', 'visible', chkRequest);
//    dataGridLEP1.columnOption('Requestor', 'visible', chkRequest);
//    dataGridLEP1.columnOption('Reviewer_1', 'visible', chkRequest);
//    dataGridLEP1.columnOption('Reviewer_2', 'visible', chkRequest);
//    dataGridLEP1.columnOption('SubmitDate', 'visible', chkRequest);
//    dataGridLEP1.columnOption('Comments', 'visible', chkRequest);
//    dataGridLEP1.columnOption('Project', 'visible', chkRequest);
//    dataGridLEP1.endUpdate();
//    // $('#RequestTable_RFO').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
//    //// $('#RequestTable').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
//    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
//    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


//});

//$('#chkRFO').on('click', function () {
//    var chkRFO;
//    if (this.checked)
//        chkRFO = true;
//    else
//        chkRFO = false;
//    var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
//    dataGridLEP1.beginUpdate();
//    dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', chkRFO);
//    //dataGridLEP1.columnOption('Fund', 'visible', chkRFO);
//    dataGridLEP1.columnOption('LeadTime', 'visible', chkRFO);
//    //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRFO);
//    dataGridLEP1.columnOption('RequiredDate', 'visible', chkRFO);
//    //dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
//    dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
//    dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', chkRFO);
//    dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
//    dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
//    dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);
//    dataGridLEP1.endUpdate();
//    // $('#RequestTable').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO); -- tking more time
//    //// $('#RequestTable').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
//    //// $('#RequestTable').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
//    //// $('#RequestTable').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'OrderID', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'OrderPrice', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'OrderedQuantity', 'visible', chkRFO);



//});

//$('#chkItem').on('click', function () {
//    var chkItem;
//    if (this.checked)
//        chkItem = true;
//    else
//        chkItem = false;
//    var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
//    dataGridLEP1.beginUpdate();
//    dataGridLEP1.columnOption('Category', 'visible', chkItem);
//    dataGridLEP1.columnOption('Cost_Element', 'visible', chkItem);
//    dataGridLEP1.columnOption('Unit_Price', 'visible', chkItem);
//    dataGridLEP1.endUpdate();
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
        //debugger;
        var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
        dataGridLEP1.beginUpdate();
        dataGridLEP1.columnOption('OEM', 'visible', true);
        dataGridLEP1.columnOption('Requestor', 'visible', false);
        dataGridLEP1.columnOption('Reviewer_1', 'visible', false);
        dataGridLEP1.columnOption('Reviewer_2', 'visible', false);
        dataGridLEP1.columnOption('Project', 'visible', false);
        dataGridLEP1.columnOption('RequestDate', 'visible', false);
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
        dataGridLEP1.columnOption('RequiredDate', 'visible', true);
        dataGridLEP1.columnOption('RequestOrderDate', 'visible', false);
        dataGridLEP1.columnOption('OrderDate', 'visible', false);
        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', false);
        dataGridLEP1.columnOption('OrderID', 'visible', false);
        dataGridLEP1.columnOption('PRNumber', 'visible', false);
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
        dataGridLEP1.columnOption('LeadTime', 'visible', true);

        /*BGSW RFO FIELDS*/
        dataGridLEP1.columnOption('OrderType', 'visible', false);
        dataGridLEP1.columnOption('CostCenter', 'visible', false);
        dataGridLEP1.columnOption('BudgetCenterID', 'visible', false);
        dataGridLEP1.columnOption('UnitofMeasure', 'visible', false);
        dataGridLEP1.columnOption('UnloadingPoint', 'visible', false);
        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', false);
        dataGridLEP1.columnOption('LabName', 'visible', false);
        dataGridLEP1.columnOption('RFOReqNTID', 'visible', false);
        dataGridLEP1.columnOption('RFOApprover', 'visible', false);
        dataGridLEP1.columnOption('QuoteAvailable', 'visible', false);
        dataGridLEP1.columnOption('GoodsRecID', 'visible', false);

        dataGridLEP1.columnOption('Material_Part_Number', 'visible', false);
        dataGridLEP1.columnOption('SupplierName_with_Address', 'visible', false);
        dataGridLEP1.columnOption('Purchase_Type', 'visible', false);
        dataGridLEP1.columnOption('Project_ID', 'visible', false);

        dataGridLEP1.endUpdate();

    }
    else if (('.chkvkm:checked').length == $('.chkvkm').length) {//chk if purchase spoc / vkm spoc
        var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
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


        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', true);
        dataGridLEP1.columnOption('LeadTime', 'visible', true);
        //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRFO);
        dataGridLEP1.columnOption('RequiredDate', 'visible', true);
        dataGridLEP1.columnOption('RequestOrderDate', 'visible', true);
        dataGridLEP1.columnOption('OrderDate', 'visible', true);
        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', true);
        dataGridLEP1.columnOption('OrderID', 'visible', true);
        dataGridLEP1.columnOption('PRNumber', 'visible', true);
        dataGridLEP1.columnOption('OrderPrice', 'visible', true);
        dataGridLEP1.columnOption('OrderedQuantity', 'visible', true);

        dataGridLEP1.columnOption('Customer_Name', 'visible', true);
        dataGridLEP1.columnOption('Customer_Dept', 'visible', true);
        dataGridLEP1.columnOption('BM_Number', 'visible', true);
        dataGridLEP1.columnOption('Task_ID', 'visible', true);
        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', true);
        dataGridLEP1.columnOption('PIF_ID', 'visible', true);

        dataGridLEP1.columnOption('BU', 'visible', true);
        dataGridLEP1.columnOption('DEPT', 'visible', true);
        dataGridLEP1.columnOption('Group', 'visible', true);
        dataGridLEP1.columnOption('Item_Name', 'visible', true);
        dataGridLEP1.columnOption('LeadTime', 'visible', true);

        /*BGSW RFO FIELDS*/
        dataGridLEP1.columnOption('OrderType', 'visible', popupedit == true ? true : false);
        dataGridLEP1.columnOption('CostCenter', 'visible', popupedit == true ? true : false);
        dataGridLEP1.columnOption('BudgetCenterID', 'visible', popupedit == true ? true : false);
        dataGridLEP1.columnOption('UnitofMeasure', 'visible', popupedit == true ? true : false);
        dataGridLEP1.columnOption('UnloadingPoint', 'visible', popupedit == true ? true : false);
        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', popupedit == true ? true : false);
        dataGridLEP1.columnOption('LabName', 'visible', popupedit == true ? true : false);
        dataGridLEP1.columnOption('RFOReqNTID', 'visible', popupedit == true ? true : false);
        dataGridLEP1.columnOption('RFOApprover', 'visible', popupedit == true ? true : false);
        dataGridLEP1.columnOption('QuoteAvailable', 'visible', popupedit == true ? true : false);
        dataGridLEP1.columnOption('GoodsRecID', 'visible', popupedit == true ? true : false);

        dataGridLEP1.columnOption('Material_Part_Number', 'visible', popupedit == true ? true : false);
        dataGridLEP1.columnOption('SupplierName_with_Address', 'visible', popupedit == true ? true : false);
        dataGridLEP1.columnOption('Purchase_Type', 'visible', popupedit == true ? true : false);
        dataGridLEP1.columnOption('Project_ID', 'visible', popupedit == true ? true : false);


        dataGridLEP1.endUpdate();
    }
    else {
        var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
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
        dataGridLEP1.columnOption('BudgetCode', 'visible', chkItem);
        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
        dataGridLEP1.columnOption('Unit_Price', 'visible', true);


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
        dataGridLEP1.columnOption('PRNumber', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
        dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);

        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);
        dataGridLEP1.columnOption('LeadTime', 'visible', true);

        dataGridLEP1.columnOption('Customer_Name', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Customer_Dept', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('BM_Number', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Task_ID', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', chkNonVKM);
        dataGridLEP1.columnOption('PIF_ID', 'visible', chkNonVKM);


        /*BGSW RFO FIELDS*/
        dataGridLEP1.columnOption('OrderType', 'visible', chkRFO && (popupedit == true ? true : false));
        dataGridLEP1.columnOption('CostCenter', 'visible', chkRFO && (popupedit == true ? true : false));
        dataGridLEP1.columnOption('BudgetCenterID', 'visible', chkRFO && (popupedit == true ? true : false));
        dataGridLEP1.columnOption('UnitofMeasure', 'visible', chkRFO && (popupedit == true ? true : false));
        dataGridLEP1.columnOption('UnloadingPoint', 'visible', chkRFO && (popupedit == true ? true : false));
        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', chkItem);
        dataGridLEP1.columnOption('LabName', 'visible', chkRFO && (popupedit == true ? true : false));
        dataGridLEP1.columnOption('RFOReqNTID', 'visible', chkRFO && (popupedit == true ? true : false));
        dataGridLEP1.columnOption('RFOApprover', 'visible', chkRFO && (popupedit == true ? true : false));
        dataGridLEP1.columnOption('QuoteAvailable', 'visible', chkRFO && (popupedit == true ? true : false));
        dataGridLEP1.columnOption('GoodsRecID', 'visible', chkRFO && (popupedit == true ? true : false));


        dataGridLEP1.columnOption('Material_Part_Number', 'visible', chkNonVKM && (popupedit == true ? true : false));
        dataGridLEP1.columnOption('SupplierName_with_Address', 'visible', chkNonVKM && (popupedit == true ? true : false));
        dataGridLEP1.columnOption('Purchase_Type', 'visible', chkNonVKM && (popupedit == true ? true : false));
        dataGridLEP1.columnOption('Project_ID', 'visible', chkNonVKM && (popupedit == true ? true : false));

        dataGridLEP1.endUpdate();
    }
}
var mainradioButtonGroup, subradioButtonGroup, fileUploader;
function editCellTemplate(cellElement, cellInfo) {
    //debugger;
    cellInfo.setValue(cellInfo.data.QuoteAvailable);
    let mainradioGroupContainer = document.createElement("div");
    //let subradioGroupContainer = document.createElement("div");



    var subradioGroupContainer = $("<div class='subradio-element'></div>");



    var fileUploadContainer = $("<div class='fileupload-element'></div>");
    var fileUploaderElement = $("<div class='file1-element'></div>"); //document.createElement("div");
    var fileUploaderElement1 = $("<div class='file2-element'></div>"); //document.createElement("div");
    var fileUploaderElement2 = $("<div class='file3-element'></div>"); //document.createElement("div");



    var fileDownloadContainer = $("<div class='fileDownload-element'></div>");
    var filedownloadElement1 = $("<div class='filedownload1'></div>");
    var filedownloadElement2 = $("<div class='filedownload2'></div>");
    var filedownloadElement3 = $("<div class='filedownload3'></div>");

    //cellElement.append(imageElement);
    cellElement.append(mainradioGroupContainer);
    mainradioButtonGroup = $(mainradioGroupContainer).dxRadioGroup({
        dataSource: [
            { text: "Yes" },
            { text: "No" }



        ],
        layout: "horizontal",
        onValueChanged: function (e) {
            //debugger;
            //const previousValue = e.previousValue.text;
            //const newValue = e.value.text;
            cellInfo.setValue(e.value.text);
            localQuoteAvail = e.value.text;
            if (e.value.text == "Yes") {
                cellElement.append(subradioGroupContainer);
                subradioButtonGroup = $(subradioGroupContainer).dxRadioGroup({
                    dataSource: [
                        { text: "Specific Vendor" },
                        { text: "Any Vendor" }



                    ],



                    layout: "horizontal",
                    onValueChanged: function (e) {
                        vendor_type = e.value.text;
                        localVendorValue = e.value.text;
                        //debugger;
                        if (e.value.text == "Specific Vendor") {
                            fileUploadContainer.empty();
                            fileUploadContainer.append(fileUploaderElement);
                            subradioGroupContainer.append(fileUploadContainer);
                            filedownloadElement2.empty();
                            filedownloadElement3.empty();
                            filedownloadElement1.empty();
                            filedownloadElement1 = $("<div class='filedownload1'></div>");
                            subradioGroupContainer.append(filedownloadElement1);
                            fileUploader = $(fileUploaderElement).dxFileUploader({
                                name: "file",
                                multiple: true,
                                //accept: "*",
                                allowedFileExtensions: ['.pdf', '.docx', '.doc'],
                                uploadMode: "useForm",
                                //uploadUrl: `${backendURL}AsyncFileUpload`,
                                onValueChanged: function (e) {
                                    //debugger;
                                    //url = e.component.option("uploadUrl");
                                    //url = updateQueryStringParameter(url, "id", editRowID);
                                    //e.component.option("uploadUrl", url);
                                    //file = e.value;
                                    ////debugger;



                                    var uploadedfilename = document.querySelector('.dx-fileuploader-files-container');



                                    for (var i = 0; i < e.value.length; i++) {





                                        if (e.value[i].name.indexOf("Quot") != -1 || e.value[i].name.indexOf("Single Source Justification") != -1) {

                                            file = e.value;


                                            if (uploadedfilename != undefined) {

                                                uploadedfilename.style.visibility = 'visible';

                                                uploadedfilename.style.height = "50px";

                                                uploadedfilename.style.paddingTop = "0px";

                                            }

                                        }

                                        else {

                                            file = null;

                                            alert('Invalid file');

                                            if (uploadedfilename != undefined) {

                                                uploadedfilename.style.visibility = 'hidden';

                                                uploadedfilename.style.height = "0px";

                                                uploadedfilename.style.paddingTop = "0px";

                                            }

                                        }

                                    }



                                    //let reader = new FileReader();
                                    //reader.onload = function (args) {
                                    //    imageElement.setAttribute('src', args.target.result);
                                    //}
                                    //reader.readAsDataURL(e.value[0]); // convert to base64 string
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



                            var fileName = "Single Source Justification.doc";
                            var url = "Templates/" + fileName;
                            let downloadtemplate = $('<a/>').addClass('dx-link')
                                .text('Click here to download SSJ template')
                                .attr('href', url)
                                .on('dxclick', function () {
                                    //Do something with options.data;  
                                    //debugger;
                                    //alert(options.data.RequestID);
                                    //$.ajax({



                                    //    type: "POST",
                                    //    url: "/BudgetingOrder/DownloadTemplate",
                                    //    data: { 'Type': 'NS' },
                                    //    datatype: "json",
                                    //    async: true,
                                    //    success: function (data) {
                                    //        //debugger;
                                    //        window.open(data.Result, '_blank');
                                    //    }
                                    //});



                                    //var currentObject = 'NS';
                                    //$.get('@Url.Action("DownloadTemplate", "BudgetingOrder")', { Type: currentObject });
                                    //var url = '@Html.Raw((Url.Action("GetTSOUDetails", "SLCockpit", new { SDate = "_sdate", EDate = "_edate" ,Location= "_Locations_sel", Labtype= "_Labtype_sel" })))';





                                    //url = "C:/UserDrive/GHB1COB/SmartLab_2023/Master/SmartLab_SourceCode/SmartLab/Templates/" + fileName;
                                    $.ajax({
                                        url: url,
                                        cache: false,
                                        xhr: function () {
                                            var xhr = new XMLHttpRequest();
                                            xhr.onreadystatechange = function () {
                                                if (xhr.readyState == 2) {
                                                    if (xhr.status == 200) {
                                                        xhr.responseType = "blob";
                                                    } else {
                                                        xhr.responseType = "text";
                                                    }
                                                }
                                            };
                                            return xhr;
                                        },
                                        success: function (data) {
                                            //Convert the Byte Data to BLOB object.
                                            var blob = new Blob([data], { type: "application/octetstream" });



                                            //Check the Browser type and download the File.
                                            var isIE = false || !!document.documentMode;
                                            if (isIE) {
                                                window.navigator.msSaveBlob(blob, fileName);
                                            } else {
                                                var url = window.URL || window.webkitURL;
                                                link = url.createObjectURL(blob);
                                                var a = $("<a />");
                                                a.attr("download", fileName);
                                                a.attr("href", link);
                                                $("body").append(a);
                                                a[0].click();
                                                $("body").remove(a);
                                            }
                                        }
                                    });





                                }).appendTo(filedownloadElement1);
                        }
                        else {
                            //$(this).closest('.file1-element').remove();



                            fileUploadContainer.empty();
                            fileUploadContainer.append(fileUploaderElement1);
                            subradioGroupContainer.append(fileUploadContainer);
                            filedownloadElement1.empty();
                            filedownloadElement3.empty();
                            filedownloadElement2.empty();
                            filedownloadElement2 = $("<div class='filedownload2'></div>");
                            subradioGroupContainer.append(filedownloadElement2);





                            let fileUploader = $(fileUploaderElement1).dxFileUploader({
                                name: "file",
                                multiple: true,
                                //accept: "*",
                                allowedFileExtensions: ['.pdf', '.docx', '.doc'],
                                uploadMode: "useForm",
                                //uploadUrl: `${backendURL}AsyncFileUpload`,
                                onValueChanged: function (e) {
                                    //debugger;

                                    var uploadedfilename = document.querySelector('.dx-fileuploader-files-container');



                                    for (var i = 0; i < e.value.length; i++) {





                                        if (e.value[i].name.indexOf("Quot") != -1 || e.value[i].name.indexOf("Neutral Specification") != -1) {

                                            file = e.value;


                                            if (uploadedfilename != undefined) {

                                                uploadedfilename.style.visibility = 'visible';

                                                uploadedfilename.style.height = "50px";

                                                uploadedfilename.style.paddingTop = "0px";

                                            }

                                        }

                                        else {

                                            file = null;

                                            alert('Invalid file');

                                            if (uploadedfilename != undefined) {

                                                uploadedfilename.style.visibility = 'hidden';

                                                uploadedfilename.style.height = "0px";

                                                uploadedfilename.style.paddingTop = "0px";

                                            }

                                        }

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



                            var fileName = "Neutral Specification.doc";
                            var url = "Templates/" + fileName;
                            let downloadtemplate = $('<a/>').addClass('dx-link')
                                .text('Click here to download NS template')
                                .attr('href', url)
                                .on('dxclick', function () {
                                    //Do something with options.data;  
                                    //debugger;
                                    //alert(options.data.RequestID);
                                    //$.ajax({



                                    //    type: "POST",
                                    //    url: "/BudgetingOrder/DownloadTemplate",
                                    //    data: { 'Type': 'NS' },
                                    //    datatype: "json",
                                    //    async: true,
                                    //    success: function (data) {
                                    //        //debugger;
                                    //        window.open(data.Result, '_blank');
                                    //    }
                                    //});



                                    //var currentObject = 'NS';
                                    //$.get('@Url.Action("DownloadTemplate", "BudgetingOrder")', { Type: currentObject });
                                    //var url = '@Html.Raw((Url.Action("GetTSOUDetails", "SLCockpit", new { SDate = "_sdate", EDate = "_edate" ,Location= "_Locations_sel", Labtype= "_Labtype_sel" })))';





                                    //url = "C:/UserDrive/GHB1COB/SmartLab_2023/Master/SmartLab_SourceCode/SmartLab/Templates/" + fileName;
                                    $.ajax({
                                        url: url,
                                        cache: false,
                                        xhr: function () {
                                            var xhr = new XMLHttpRequest();
                                            xhr.onreadystatechange = function () {
                                                if (xhr.readyState == 2) {
                                                    if (xhr.status == 200) {
                                                        xhr.responseType = "blob";
                                                    } else {
                                                        xhr.responseType = "text";
                                                    }
                                                }
                                            };
                                            return xhr;
                                        },
                                        success: function (data) {
                                            //Convert the Byte Data to BLOB object.
                                            var blob = new Blob([data], { type: "application/octetstream" });



                                            //Check the Browser type and download the File.
                                            var isIE = false || !!document.documentMode;
                                            if (isIE) {
                                                window.navigator.msSaveBlob(blob, fileName);
                                            } else {
                                                var url = window.URL || window.webkitURL;
                                                link = url.createObjectURL(blob);
                                                var a = $("<a />");
                                                a.attr("download", fileName);
                                                a.attr("href", link);
                                                $("body").append(a);
                                                a[0].click();
                                                $("body").remove(a);
                                            }
                                        }
                                    });





                                }).appendTo(filedownloadElement2);



                        }
                    }
                }).dxRadioGroup("instance");







            }
            else {
                cellElement.append(subradioGroupContainer);
                subradioGroupContainer.empty();
                fileUploadContainer.empty();
                fileUploadContainer.append(fileUploaderElement2);
                subradioGroupContainer.append(fileUploadContainer);
                filedownloadElement3.empty();
                filedownloadElement3 = $("<div class='filedownload3'></div>");
                subradioGroupContainer.append(filedownloadElement3);
                let fileUploader = $(fileUploaderElement2).dxFileUploader({
                    name: "file",
                    multiple: true,
                    //accept: "*",
                    allowedFileExtensions: ['.pdf', '.docx', '.doc'],
                    uploadMode: "useForm",
                    //showFileList: true,
                    //onValueChanged: function (e) {
                    //    //debugger;
                    //    var values = e.component.option("values");
                    //    $.each(values, function (index, value) {
                    //        if (value.name.indexOf(".png") < 3) {
                    //            e.element
                    //                .find(".dx-fileuploader-files-container .dx-fileuploader-cancel-button")
                    //                .eq(index)
                    //                .trigger("dxclick");
                    //        }
                    //    });
                    //},
                    ////uploadUrl: `${backendURL}AsyncFileUpload`,
                    onValueChanged: function (e) {
                        //debugger;
                        //url = e.component.option("uploadUrl");
                        //url = updateQueryStringParameter(url, "id", editRowID);
                        //e.component.option("uploadUrl", url);
                        for (var i = 0; i < e.value.length; i++) {

                            if (e.value[i].name.indexOf("Quot") != -1 || e.value[i].name.indexOf("Neutral Specification") != -1) {
                                file = e.value;
                            }
                            else {
                                file = null;
                                alert('Invalid file');
                            }
                        }

                        //file = e.value;
                        //debugger;
                        // e.value[0].name
                        //let reader = new FileReader();
                        //reader.onload = function (args) {
                        //    imageElement.setAttribute('src', args.target.result);
                        //}
                        //reader.readAsDataURL(e.value[0]); // convert to base64 string
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






                var fileName = "Neutral Specification.doc";
                var url = "Templates/" + fileName;
                let downloadtemplate = $('<a/>').addClass('dx-link')
                    .text('Click here to download NS template')
                    .attr('href', url)
                    .on('dxclick', function () {
                        //Do something with options.data;  
                        //debugger;
                        //alert(options.data.RequestID);
                        //$.ajax({



                        //    type: "POST",
                        //    url: "/BudgetingOrder/DownloadTemplate",
                        //    data: { 'Type': 'NS' },
                        //    datatype: "json",
                        //    async: true,
                        //    success: function (data) {
                        //        //debugger;
                        //        window.open(data.Result, '_blank');
                        //    }
                        //});



                        //var currentObject = 'NS';
                        //$.get('@Url.Action("DownloadTemplate", "BudgetingOrder")', { Type: currentObject });
                        //var url = '@Html.Raw((Url.Action("GetTSOUDetails", "SLCockpit", new { SDate = "_sdate", EDate = "_edate" ,Location= "_Locations_sel", Labtype= "_Labtype_sel" })))';




                        //url = "C:/UserDrive/GHB1COB/SmartLab_2023/Master/SmartLab_SourceCode/SmartLab/Templates/" + fileName;
                        $.ajax({
                            url: url,
                            cache: false,
                            xhr: function () {
                                var xhr = new XMLHttpRequest();
                                xhr.onreadystatechange = function () {
                                    if (xhr.readyState == 2) {
                                        if (xhr.status == 200) {
                                            xhr.responseType = "blob";
                                        } else {
                                            xhr.responseType = "text";
                                        }
                                    }
                                };
                                return xhr;
                            },
                            success: function (data) {
                                //Convert the Byte Data to BLOB object.
                                var blob = new Blob([data], { type: "application/octetstream" });



                                //Check the Browser type and download the File.
                                var isIE = false || !!document.documentMode;
                                if (isIE) {
                                    window.navigator.msSaveBlob(blob, fileName);
                                } else {
                                    var url = window.URL || window.webkitURL;
                                    link = url.createObjectURL(blob);
                                    var a = $("<a />");
                                    a.attr("download", fileName);
                                    a.attr("href", link);
                                    $("body").append(a);
                                    a[0].click();
                                    $("body").remove(a);
                                }
                            }
                        });





                    }).appendTo(filedownloadElement3);
            }






        }
    }).dxRadioGroup("instance");
}

function ReloadRequests(requestid, sdate, edate, openpopup) {
    //debugger;
    //sdate = new Date(sdate);
    //edate = new Date(edate);
    //var startdate = (sdate.toISOString()).split("T")[0];
    //var enddate = (edate.toISOString()).split("T")[0];
    if (openpopup == true) {
        //debugger;

        $.ajax({
            type: "GET",
            url: encodeURI("../BudgetingOrder/GetlinkedRequests"),
            data: { 'RequestID': requestid, 'StartDate': sdate, 'EndDate': edate },
            success: function (data) {
                //debugger;

                if (data.success == true) {
                    Requestsobj = data.data;
                                        
                    reqArray = [];
                    selectedItems = [];
                    for (var j = 0; j < Requestsobj.length; j++) {
                        if (Requestsobj[j].isSelected == true) {
                            selectedItems.push(Requestsobj[j].LinkedRequestID);
                        }
                        reqArray.push({
                            Text: Requestsobj[j].LinkedRequestID,
                            Value: Requestsobj[j].LinkedRequestID,
                            isSelected: Requestsobj[j].isSelected,
                        });
                    }

                    ////// **************** //////////////
                    //dropDownBox = $('#reqdropdown').dxDropDownBox({
                    //    value: selectedItems,
                    //    showClearButton: true,
                    //    placeholder: 'Select a value...',
                    //    valueExpr: 'Value',
                    //    displayExpr: 'Text',
                    //    searchEnabled: true,
                    //    dataSource: new DevExpress.data.ArrayStore({
                    //        data: reqArray,
                    //        key: 'Value'
                    //    }),
                    //    contentTemplate: function (e) {
                    //        //debugger;
                    //        const value = e.component.option("value");
                    //        const v = e.component.option("value");
                    //        var $list = $("<div>").dxList({
                    //            dataSource: e.component.option("dataSource"),
                    //            dataSource: new DevExpress.data.ArrayStore({
                    //                data: reqArray,
                    //                key: 'Value'
                    //            }),
                    //            valueExpr: 'Value',
                    //            displayExpr: 'Text',
                    //            height: 170,
                    //            showSelectionControls: true,
                    //            //selectionMode: "all",
                    //            selectedItemKeys: value,
                    //            parentIdExpr: 'Value',
                    //            selectionMode: 'all',
                    //            selectByClick: true,
                    //            showCheckBoxesMode: 'normal',
                    //            onSelectionChanged: function (e) {
                    //                debugger;
                    //                var selectedKeys = e.component.option("selectedItemKeys");
                    //                e.component.option("value", selectedKeys);
                    //                SelectedRequests = e.component.option("selectedItemKeys").join(', ');

                                    
                    //                //selectedItems = allItems.filter(item => selectedKeys.includes(item.Value));


                    //                //$('#reqdropdown').text(e.component.option('selectedItemKeys').join(', '))
                    //            },
                    //            onValueChanged: function (args) {
                    //                debugger;
                    //                const { value } = args;
                    //                //syncListSelection(e.component, value);
                    //                //value.forEach((key) => {
                    //                //    e.component.selectItem(key);
                    //                //});

                    //                const selectedValues = args.value;
                    //                const unselectedValues = args.previousValue.filter(value => !selectedValues.includes(value));
                    //                $list.dxList("option", "selectedItemKeys", args.value);
                    //            },
                    //            //onContentReady(args) {
                    //            //    debugger;
                    //            //    if (!v) {
                    //            //        args.component.unselectAll();
                    //            //        return;
                    //            //    }

                    //            //    args.component.option('selectedItemKeys', v);
                    //            //},
                    //            //onContentReady: function (args) {
                    //            //    //debugger;
                    //            //    SelectedRequests = "";
                    //            //    syncListSelection(args.component, value);
                    //            //    //$list.dxList("option", "selectedItemKeys", args.component.option("value"));
                    //            //},

                    //        });

                    //        //var list = $list.dxList('instance');

                    //        //e.component.on('valueChanged', (args) => {
                    //        //    debugger;
                    //        //    const { value } = args;
                    //        //    //syncListSelection(list, value);

                    //        //    if (!value) {
                    //        //        list.unselectAll();
                    //        //        return;
                    //        //    }

                    //        //    list.option('selectedItemKeys', value);
                    //        //});

                    //        //var selectAllItem = { Value: "selectAll", Text: "Select All", selected: false };
                    //        //$list.dxList("instance").option("items", [selectAllItem, ...e.component.option("dataSource")]);


                    //        return $list;
                    //    },




                    //}).dxDropDownBox("instance");

                    //debugger;
                    //dropDownBox.option('value', selectedItems);
                    //dropDownBox.repaint();

                    ////// **************** //////////////
                    //////////////////////

                    const syncListSelection = function (listInstance, value) {
                        debugger;
                        if (!value) {
                            listInstance.unselectAll();
                            return;
                        }

                        listInstance.option('selectedItemKeys', value);
                    }

                    //const makeAsyncDataSource = function (jsonFile) {
                    //    return new DevExpress.data.CustomStore({
                    //        loadMode: 'raw',
                    //        key: 'ID',
                    //        load() {
                    //            return $.getJSON(`https://js.devexpress.com/Demos/WidgetsGallery/JSDemos/data/${jsonFile}`);
                    //        },
                    //    });
                    //};

                    $('#reqdropdown').dxDropDownBox({
                        value: selectedItems,
                        valueExpr: 'Value',
                        displayExpr: 'Text',
                        placeholder: 'Select a value...',
                        showClearButton: true,
                        //dataSource: makeAsyncDataSource('treeProducts.json'),
                        dataSource: new DevExpress.data.ArrayStore({
                            data: reqArray,
                            key: 'Value'
                        }),
                        contentTemplate(e) {
                            debugger;
                            const v = e.component.option('value');
                            const $list = $('<div>').dxList({
                                dataSource: e.component.getDataSource(),
                                keyExpr: 'Value',
                                selectionMode: 'all',
                                displayExpr: 'Text',
                                showSelectionControls: true,
                                height: 230,
                                onContentReady(args) {
                                    debugger;
                                    syncListSelection(args.component, v);
                                },
                                onSelectionChanged(args) {
                                    debugger;
                                    const selectedKeys = args.component.option('selectedItemKeys');
                                    e.component.option('value', selectedKeys);
                                },
                            });

                            list = $list.dxList('instance');

                            e.component.on('valueChanged', (args) => {
                                debugger;
                                const { value } = args;
                                syncListSelection(list, value);
                            });

                            return $list;
                        },
                    });


                    /////////////////////

                }
            },
            error: function (data) {
                //debugger;
                $.notify('Error in getting requestids!', {
                    globalPosition: "top center",
                    className: "warn"
                });
            },
        });


    }




    const syncListSelection = function (listInstance, value) {
        //debugger;
        if (!value) {
            listInstance.unselectAll();
            return;
        }

        listInstance.option('selectedItemKeys', value);
    }
}

function contentTemplate(data, element) {
    //debugger;
    const lineBreak = '<br>';
    const infoIcon = '<i id="helpedInfo" class="dx-icon dx-icon-info"></i>';
    const labelText = `Additional${lineBreak}${data.text}`;

    element.append(`<div id='template-content'>${infoIcon}${labelText}</div>`);

    $('<div>').dxTooltip({
        target: '#helpedInfo',
        showEvent: 'mouseenter',
        hideEvent: 'mouseleave',
        contentTemplate(args) {
            args.html('<div id="tooltip-content">This field must not exceed 200 characters</div>');
        },
    }).appendTo(element);
}


//$('#chkItem').on('click', function () {
//    var chkItem;
//    if (this.checked)
//        chkItem = true;
//    else
//        chkItem = false;

//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


//});

//$('#chkRequest').on('click', function () {
//    var chkRequest;
//    if (this.checked)
//        chkRequest = true;
//    else
//        chkRequest = false;

//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


//});

//$('#chkRFO').on('click', function () {
//    var chkRFO;
//    if (this.checked)
//        chkRFO = true;
//    else
//        chkRFO = false;

//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
//    $('#RequestTable_RFO').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);



//});

//////Javascript file for Budgeting Request Details - mae9cob


////var dataGrid_order;
////var newobjdata;
////var BU_list, DEPT_list, Item_list, Category_list, OEM_list, Group_list, CostElement_list, OrderStatus_list, Fund_list, BudgetCodeList;
////var Selected = [];
////var unitprice, reviewer_2, category, costelement, leadtime, BudgetCode;
////var lookup_data, new_request;
////var filtered_yr;
////var leadtime1;
////var genSpinner_load = document.querySelector("#load");
////var SubItemList;
////var addnewitem_flag = false;

////var objdata_rfoview;
////var Item_headerFilter, DEPT_headerFilter, Group_headerFilter, BU_headerFilter, OEM_headerFilter, Category_headerFilter, CostElement_headerFilter, OrderStatus_headerFilter, BudgetCode_headerFilter;




////$(".custom-file-input").on("change", function () {
////    var fileName = $(this).val().split("\\").pop();
////    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
////});

////var oems_chosen = [];

//////var oems_chosen = new Array();
//////$('input:checkbox[name=Types]:checked').each(function () {
//////    //debugger;
//////    Types.push($(this).val())
//////});
//////function fnOEMChange(oem) {
//////    //debugger;
//////    oems_chosen = [];
//////    //oems_chosen = new Array();
//////    for (var i = 0, len = oem.options.length; i < len; i++) {
//////        if (document.getElementById('selectOEM').selectedIndex != -1) {
//////            options = oem.options;
//////            opt = options[i];
//////            if (opt.selected) {
//////                //store the labids chosen by user from dropdown to process the relevant chart data
//////                oems_chosen.push(opt.value);
//////            }
//////        }
//////    }
//////    ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);


//////}
//////function fnOEMselectChange(oem) {
//////    //debugger;
//////    oems_chosen = [];
//////    //oems_chosen = new Array();
//////    for (var i = 0, len = oem.options.length; i < len; i++) {
//////        if (document.getElementById('selectOEM').selectedIndex != -1) {
//////            options = oem.options;
//////            opt = options[i];
//////            if (opt.selected) {
//////                //store the labids chosen by user from dropdown to process the relevant chart data
//////                oems_chosen.push(opt.value);
//////            }
//////        }
//////    }
//////    ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);

//////}


//////$.ajax({

//////    type: "GET",
//////    url: "/BudgetingOrder/Lookup",
//////    async: false,
//////    success: onsuccess_lookupdata,
//////    error: onerror_lookupdata
//////})


//////function onsuccess_lookupdata(response) {

//////    lookup_data = response.data;
//////    BU_list = lookup_data.BU_List;
//////    OEM_list = lookup_data.OEM_List;
//////    DEPT_list = lookup_data.DEPT_List;
//////    Group_list = lookup_data.Groups_test;
//////    Item_list = lookup_data.Item_List;
//////    Category_list = lookup_data.Category_List;
//////    CostElement_list = lookup_data.CostElement_List;
//////    OrderStatus_list = lookup_data.OrderStatus_List;
//////    Fund_list = lookup_data.Fund_List;

//////    //Item_list_New = lookup_data.Item_List1;

//////}

//////function onerror_lookupdata(response) {
//////    alert("Error in fetching lookup");

//////}



//////$.ajax({
//////    type: "GET",
//////    url: encodeURI("../BudgetingOrder/InitRowValues"),
//////    success: OnSuccessCall_dnew,
//////    error: OnErrorCall_dnew

//////});
//////function OnSuccessCall_dnew(response) {

//////    new_request = response.data;

//////}
//////function OnErrorCall_dnew(response) {

//////    $.notify('Add new error!', {
//////        globalPosition: "top center",
//////        className: "warn"
//////    });
//////}




//////Loading indicator on load of the Request module while fetching the Item Requests
////window.onload = function () {
////    //debugger;
////    document.getElementById("loadpanel").style.display = "block";


////    genSpinner_load.classList.add('fa');
////    genSpinner_load.classList.add('fa-spinner');
////    genSpinner_load.classList.add('fa-pulse');
////    $("#RequestTable_RFO").prop('hidden', true);

////    //$("#chkRFO").attr("autocomplete", "off");
////    //$("#chkRequest").attr("autocomplete", "off");
////    //$("#chkItem").attr("autocomplete", "off");

////    //document.getElementById('chkRFO').reset();
////    //document.getElementById('chkRequest').reset();
////    //document.getElementById('chkItem').reset();




////    //$("#chkRFO").prop("checked", false);
////    //$("#chkRequest").prop("checked", false);
////    //$("#chkItem").prop("checked", false);

////    //chkRFO
////    //chkItem
////    //chkRequest
////};



//////Reference the DropDownList for Year to be selected by Requestor
////var ddlYears = document.getElementById("ddlYears");
//////Determine the Current Year.
////var currentYear = (new Date()).getFullYear();
//////debugger;
//////Loop and add the Year values to DropDownList.
//////for (var i = currentYear; i >= 2020; i--) {
//////    var option = document.createElement("OPTION");
//////    option.innerHTML = i;
//////    option.value = i;
//////    ddlYears.appendChild(option);
//////}
////////Loop and add the Year values to DropDownList.
////for (var i = currentYear+1; i >= 2022; i--) {
////    var option = document.createElement("OPTION");
////    option.innerHTML = i;
////    option.value = i;
////    ddlYears.appendChild(option);

////    if (option.value == (currentYear + 1)) {
////        //if (option.value == (currentYear - 2)) {
////        option.defaultSelected = true;
////        //option.defaultSelected = true;
////    }
////    filtered_yr = $("#ddlYears").val();
////    filtered_yr = parseInt(filtered_yr) - 1;
////    filtered_yr = filtered_yr.toString();
////    //debugger;
////}




//////At load, Display the data for Current year
////if (filtered_yr == null) {
////    filtered_yr = new Date().getFullYear();
////}
//////debugger;
//////$('.selectpicker').selectpicker('selectAll');//it wil hit fnoemchange to select all & then execute ajaxcallforrequestui
////ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);
//////debugger;

//////Function to change year from dropdown
////function fnYearChange(yearselect) {
////    //debugger;
////    $("#RequestTable_RFO").prop('hidden', true);
////    document.getElementById("loadpanel").style.display = "block";

////    genSpinner_load = document.querySelector("#load");
////    genSpinner_load.classList.add('fa');
////    genSpinner_load.classList.add('fa-spinner');
////    genSpinner_load.classList.add('fa-pulse');
////    filtered_yr = yearselect.value;

////    filtered_yr = parseInt(yearselect.value) - 1;
////    filtered_yr = filtered_yr.toString();
////    //debugger;
////    //Ajax call to Get Request Item Data
////    ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);



////}


////function ajaxCallforRequestUI(filtered_yr) {

////    $(':checkbox').prop('checked', false);
////    $.ajax({
////        type: "POST",
////        url: "/BudgetingOrder/GetPODetails",
////        datatype: "json",
////        success: function (data) {
////            //debugger;
////            if (data.data.length > 0) {
////                //debugger;
////                //var res = JSON.parse(data.data.Data.Content);
////                SubItemList = eval(data.data);
////                //LoadDataGrid(res);

////            }
////        },
////        error: function (jqXHR, exception) {
////            //debugger;
////            var msg = '';
////            if (jqXHR.status === 0) {
////                msg = 'Not connect.\n Verify Network.';
////            } else if (jqXHR.status == 404) {
////                msg = 'Requested page not found. [404]';
////            } else if (jqXHR.status == 500) {
////                msg = 'Internal Server Error [500].';
////            } else if (exception === 'parsererror') {
////                msg = 'Requested JSON parse failed.';
////            } else if (exception === 'timeout') {
////                msg = 'Time out error.';
////            } else if (exception === 'abort') {
////                msg = 'Ajax request aborted.';
////            } else {
////                msg = 'Uncaught Error.\n' + jqXHR.responseText;
////            }
////            $('#post').html(msg);
////        }
////    });

////    $.ajax({

////        type: "GET",
////        url: "/BudgetingOrder/Lookup",
////        async: false,
////        data: { 'year': filtered_yr },
////        success: onsuccess_lookupdata,
////        error: onerror_lookupdata
////    })


////    function onsuccess_lookupdata(response) {

////        lookup_data = response.data;
////        BU_list = lookup_data.BU_List;
////        OEM_list = lookup_data.OEM_List;
////        DEPT_list = lookup_data.DEPT_List;
////        Group_list = lookup_data.Groups_test;
////        Item_list = lookup_data.Item_List;
////        Category_list = lookup_data.Category_List;
////        CostElement_list = lookup_data.CostElement_List;
////        OrderStatus_list = lookup_data.OrderStatus_List;
////        Fund_list = lookup_data.Fund_List;
////        BudgetCodeList = lookup_data.BudgetCodeList;

////        Item_headerFilter = lookup_data.Item_HeaderFilter;
////        DEPT_headerFilter = lookup_data.DEPT_HeaderFilter;
////        Group_headerFilter = lookup_data.Group_HeaderFilter;
////        BU_headerFilter = lookup_data.BU_HeaderFilter;
////        OEM_headerFilter = lookup_data.OEM_HeaderFilter;
////        Category_headerFilter = lookup_data.Category_HeaderFilter;
////        CostElement_headerFilter = lookup_data.CostElement_HeaderFilter;
////        OrderStatus_headerFilter = lookup_data.OrderStatus_HeaderFilter;
////        BudgetCode_headerFilter = lookup_data.BudgetCode_HeaderFilter;

////        //Item_list_New = lookup_data.Item_List1;

////    }

////    function onerror_lookupdata(response) {
////        alert("Error in fetching lookup");

////    }



////    //Ajax call to Get Request Item Data
////    //debugger;
////    $.ajax({
////        type: "POST",
////        url: encodeURI("../BudgetingOrder/GetData"),
////        data: { 'year': filtered_yr },
////        success: OnSuccess_GetData,
////        error: OnError_GetData
////    });


////    function OnSuccess_GetData(response) {
////        //debugger;
////        //if (response.success) {
////        objdata_rfoview = (response.data);

////        var isF03F01 = function (position) {

////            if (position == undefined)
////                return true;
////            else
////                return position && [1, 3].indexOf(position) >= 0;

////        };
////        var isDelivered = function (position) {//cancelled also included

////            //CHANGE
////            return position && [5, 6, 10].indexOf(position) >= 0;

////        };


////        //Hide the Loading indicator once the Request List is fetched
////        genSpinner_load.classList.remove('fa');
////        genSpinner_load.classList.remove('fa-spinner');
////        genSpinner_load.classList.remove('fa-pulse');
////        document.getElementById("loadpanel").style.display = "none";
////        $("#RequestTable_RFO").prop('hidden', false);
////        //debugger;
////        var c = $("#RequestTable_RFO").dxDataGrid({

////            dataSource: objdata_rfoview,
////            width: "96.8vw",
////            height: "76vh",

////            editing: {
////                mode: "row",

////                // allowAdding: true,

////                allowUpdating: function (e) {
////                    //debugger;
////                    return (response.success && !response.isDashboard_flag && (isDelivered(e.row.data.OrderStatus) || !(e.row.data.RequestToOrder))); //if success = true - access to update and view ; false - access to only view
////                },
////                //allowDeleting: function (e) {

////                //    return !(e.row.data.RequestToOrder) && isF03F01(e.row.data.Fund);
////                //},
////                useIcons: true
////            },
////            onContentReady: function (e) {

////                e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
////            },
////            onCellPrepared: function (e) {
////                if (e.rowType === "header" || e.rowType === "filter") {
////                    e.cellElement.addClass("columnHeaderCSS");
////                    e.cellElement.find("input").addClass("columnHeaderCSS");
////                }
////            },
////            onInitNewRow: function (e) {

////                addnewitem_flag = true;
////                e.data.Requestor = new_request.Requestor;
////                e.data.Reviewer_1 = new_request.Reviewer_1;
////                e.data.Reviewer_2 = new_request.Reviewer_2;
////                e.data.DEPT = new_request.DEPT;
////                e.data.Group = new_request.Group;



////            },

////            columnAutoWidth: true,
////            allowColumnReordering: true,
////            allowColumnResizing: true,
////            focusedRowEnabled: true,
////            allowColumnReordering: true,
////            allowColumnResizing: true,
////            keyExpr: "RequestID",

////            columnResizingMode: "widget",
////            columnChooser: {
////                enabled: true
////            },
////            //filterRow: {
////            //    visible: true

////            //},
////            showBorders: true,
////            headerFilter: {
////                visible: true,
////                applyFilter: "auto",
////                allowSearching: true
////            },
////            selection: {
////                applyFilter: "auto"
////            },
////            loadPanel: {
////                enabled: true
////            },
////            //    paging: {
////            //        pageSize: 100
////            //},

////            //searchPanel: {
////            //    visible: true,
////            //    width: 240,
////            //    placeholder: "Search..."
////            //},
////            //scrolling: {
////            //    columnRenderingMode: "virtual"
////            //},
////            scrolling: {
////                mode: "virtual",
////                rowRenderingMode: "virtual",
////                columnRenderingMode: "virtual"
////            },
////            columns: [
////                {
////                    type: "buttons",
////                    width: 90,
////                    alignment: "left",
////                    buttons: [
////                        "edit",//, "delete",
////                        {
////                            hint: "Submit",
////                            icon: "check",
////                            visible: function (e) {
////                                //debugger;
////                                return ((response.success && !e.row.isEditing && !(e.row.data.RequestToOrder)) || (!e.row.isEditing && isDelivered(e.row.data.OrderStatus))) && !response.isDashboard_flag;/*&& !isOrderApproved(e.row.data.OrderStatus)*/;

////                            },
////                            onClick: function (e) {
////                                //    var LeadTime_tocalc_ExpReqdDt;
////                                //    //debugger;
////                                //    // e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
////                                //    // e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
////                                if (e.row.data.RequiredDate && !e.row.data.RequestToOrder) {

////                                    //        $.ajax({

////                                    //            type: "GET",
////                                    //            url: "/BudgetingOrder/GetLeadTime",
////                                    //            data: { 'ItemName': e.row.data.Item_Name },
////                                    //            datatype: "json",
////                                    //            async: false,
////                                    //            success: success_getleadtime,

////                                    //        });

////                                    //        function success_getleadtime(response) {
////                                    //            //debugger;
////                                    //            if (response == 0) {
////                                    //                LeadTime_tocalc_ExpReqdDt = "";

////                                    //                LabAdminApprove(e.row.data.RequestID, filtered_yr);
////                                    //            }
////                                    //            else {
////                                    //                LeadTime_tocalc_ExpReqdDt = response;
////                                    var ReqdDate = e.row.data.RequiredDate;
////                                    //                //debugger;
////                                    $.ajax({

////                                        type: "GET",
////                                        url: "/BudgetingOrder/ValidateRequiredDate",
////                                        data: { 'RequiredDate': ReqdDate },
////                                        datatype: "json",
////                                        async: false,
////                                        success: success_validateReqdDate,

////                                    });
////                                    function success_validateReqdDate(info) {
////                                        //debugger;
////                                        if (info) {
////                                            $.notify(info, {
////                                                globalPosition: "top center",
////                                                className: "error"
////                                            })
////                                        }
////                                        else {
////                                            LabAdminApprove(e.row.data.RequestID, filtered_yr);
////                                        }
////                                    }

////                                    //   }


////                                    //        }


////                                }
////                                else {

////                                    LabAdminApprove(e.row.data.RequestID, filtered_yr);
////                                }



////                                e.component.refresh(true);
////                                e.event.preventDefault();
////                            }

////                        }]
////                },
////                {

////                    alignment: "center",
////                    columns: [
////                        {
////                            dataField: "BU",

////                            width: 65,
////                            validationRules: [{ type: "required" }],

////                            lookup: {
////                                dataSource: BU_list,
////                                valueExpr: "ID",
////                                displayExpr: "BU"
////                            },
////                            headerFilter: {
////                                dataSource: BU_headerFilter,
////                                allowSearch: true
////                            },


////                        },
////                        {
////                            dataField: "OEM",
////                            validationRules: [{ type: "required" }],
////                            width: 70,
////                            lookup: {
////                                dataSource: OEM_list,
////                                valueExpr: "ID",
////                                displayExpr: "OEM"
////                            },
////                            headerFilter: {
////                                dataSource: OEM_headerFilter,
////                                allowSearch: true
////                            },


////                        },
////                        {
////                            dataField: "DEPT",
////                            caption: "Dept",
////                            validationRules: [{ type: "required" }],
////                            headerFilter: {
////                                dataSource: DEPT_headerFilter,
////                                allowSearch: true
////                            },
////                            setCellValue: function (rowData, value) {

////                                rowData.DEPT = value;
////                                rowData.Group = null;

////                            },
////                            width: 90,
////                            lookup: {
////                                dataSource: function (options) {

////                                    return {

////                                        store: DEPT_list,


////                                    };
////                                },

////                                valueExpr: "ID",
////                                displayExpr: "DEPT"

////                            },


////                        },
////                        {
////                            dataField: "Group",
////                            width: 90,
////                            headerFilter: {
////                                dataSource: Group_headerFilter,
////                                allowSearch: true
////                            },
////                            validationRules: [{ type: "required" }],
////                            lookup: {
////                                dataSource: function (options) {

////                                    return {

////                                        store: Group_list,

////                                        filter: options.data ? ["Dept", "=", options.data.DEPT] : null
////                                    };

////                                },
////                                valueExpr: "ID",
////                                displayExpr: "Group"
////                            },


////                        },


////                        {
////                            dataField: "Item_Name",
////                            width: 200,
////                            headerFilter: {
////                                dataSource: Item_headerFilter,
////                                allowSearch: true
////                            },
////                            validationRules: [{ type: "required" }],
////                            lookup: {
////                                dataSource: function (options) {

////                                    return {

////                                        store: Item_list,
////                                       // filter: options.data ? ["Deleted", "=", false] : null

////                                    };

////                                },
////                                valueExpr: "S_No",
////                                displayExpr: "Item_Name"
////                            },
////                            calculateSortValue: function (data) {
////                                //debugger;
////                                const value = this.calculateCellValue(data);
////                                return this.lookup.calculateCellValue(value);
////                            },


////                        },
////                        {
////                            dataField: "Category",
////                            caption: "Category",
////                            validationRules: [{ type: "required" }],
////                            headerFilter: {
////                                dataSource: Category_headerFilter,
////                                allowSearch: true
////                            },
////                            lookup: {
////                                dataSource: Category_list,
////                                valueExpr: "ID",
////                                displayExpr: "Category"
////                            },
////                            allowEditing: false,
////                            visible: false

////                        },
////                        {
////                            dataField: "Cost_Element",
////                            headerFilter: {
////                                dataSource: CostElement_headerFilter,
////                                allowSearch: true
////                            },
////                            lookup: {
////                                dataSource: CostElement_list,
////                                valueExpr: "ID",
////                                displayExpr: "CostElement"
////                            },
////                            allowEditing: false,
////                            visible: false


////                        },
////                        {
////                            dataField: "BudgetCode",
////                            headerFilter: {
////                                dataSource: BudgetCode_headerFilter,
////                                allowSearch: true
////                            },
////                            lookup: {
////                                dataSource: BudgetCodeList,
////                                valueExpr: "BudgetCode",
////                                displayExpr: "BudgetCode"
////                            },
////                            allowEditing: false,
////                            visible: false


////                        },
////                        {
////                            dataField: "Required_Quantity",
////                            caption: "Required Qty",

////                            dataType: "number",
////                            setCellValue: function (rowData, value) {

////                                rowData.Required_Quantity = value;

////                            },
////                            allowEditing: false,
////                            visible: false


////                        },
////                        {
////                            dataField: "Reviewed_Quantity",
////                            caption: "Review Qty",

////                            validationRules: [
////                                { type: "required" },
////                                {
////                                    type: "range",
////                                    message: "Please enter valid count > 0",
////                                    min: 0,
////                                    max: 214783647
////                                }],
////                            dataType: "number",
////                            setCellValue: function (rowData, value) {

////                                rowData.Reviewed_Quantity = value;

////                            },


////                        },
////                        {
////                            dataField: "ActualAvailableQuantity",
////                            caption: "Available Qty",
////                            allowEditing: false,
////                            width: 102
////                        },
////                        {
////                            dataField: "Unit_Price",
////                            caption: "Unit Price",
////                            dataType: "number",
////                            format: { type: "currency", precision: 2 },
////                            valueFormat: "#0.00",

////                            validationRules: [{ type: "required" }, {
////                                type: "range",
////                                message: "Please enter valid price > 0",
////                                min: 0.01,
////                                max: Number.MAX_VALUE
////                            }],
////                            allowEditing: false,
////                            visible: false


////                        },
////                        {
////                            dataField: "Total_Price",
////                            caption:"Required Cost",

////                            calculateCellValue: function (rowData) {

////                                if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
////                                    return rowData.Unit_Price * rowData.Required_Quantity;
////                                }
////                                else
////                                    return 0.0;
////                            },

////                            dataType: "number",
////                            format: { type: "currency", precision: 2 },
////                            valueFormat: "#0.00",
////                            allowEditing: false,
////                            visible: false
////                        },
////                        {
////                            dataField: "Reviewed_Cost",

////                            calculateCellValue: function (rowData) {

////                                //if (rowData.Reviewed_Quantity > 0 && rowData.Unit_Price > 0) {
////                                //    return rowData.Unit_Price * rowData.Reviewed_Quantity;
////                                //}
////                                //else
////                                //    return 0.0

////                                if ((rowData.Reviewed_Cost == null || rowData.Reviewed_Cost == undefined) && rowData.Reviewed_Quantity > 0 && rowData.Unit_Price > 0) {
////                                    return rowData.Unit_Price * rowData.Reviewed_Quantity;
////                                }
////                                else if (rowData.Reviewed_Cost != null || rowData.Reviewed_Cost != undefined) {
////                                    return rowData.Reviewed_Cost;
////                                }
////                                else
////                                    return 0.0;;
////                            },

////                            dataType: "number",
////                            format: { type: "currency", precision: 2 },
////                            valueFormat: "#0.00",
////                            allowEditing: false
////                        },

////                        {
////                            dataField: "Requestor",
////                            allowEditing: false,
////                            visible: false
////                        },
////                        {
////                            dataField: "Reviewer_1",
////                            allowEditing: false,
////                            visible: false
////                        },
////                        {
////                            dataField: "Reviewer_2",
////                            allowEditing: false,
////                            visible: false
////                        },
////                        {
////                            dataField: "SubmitDate",
////                            allowEditing: false,
////                            visible: false
////                        },
////                        {
////                            dataField: "RequiredDate",
////                            dataType: "date",
////                            visible: true

////                        },
////                        {
////                            dataField: "RequestOrderDate",
////                            dataType: "date",
////                            allowEditing: false,
////                            visible: false

////                        },
////                        {
////                            dataField: "OrderDate",
////                            dataType: "date",
////                            allowEditing: false,
////                            visible: true

////                        },
////                        {
////                            dataField: "TentativeDeliveryDate",
////                            dataType: "date",
////                            allowEditing: false,
////                            visible: false

////                        },
////                        {
////                            dataField: "Comments",
////                            visible: false,
////                            allowEditing: false,
////                        },
////                        {
////                            dataField: "PORemarks",
////                            width: 140,

////                        },
////                        {
////                            dataField: "LeadTime",
////                            caption: "LeadTime (in days)",
////                            allowEditing: false,
////                            visible: true,
////                            calculateCellValue: function (rowData) {
////                                //update the LeadTime
////                                if (rowData.Item_Name == undefined || rowData.Item_Name == null) {

////                                    leadtime1 = "";
////                                }

////                                else {

////                                    $.ajax({

////                                        type: "GET",
////                                        url: "/BudgetingOrder/GetLeadTime",
////                                        data: { 'ItemName': rowData.Item_Name },
////                                        datatype: "json",
////                                        async: false,
////                                        success: success_getleadtime,

////                                    });

////                                    function success_getleadtime(response) {

////                                        if (response == 0)
////                                            leadtime1 = "";
////                                        else
////                                            leadtime1 = response;

////                                    }

////                                }

////                                return leadtime1;
////                            }

////                        },

////                        {
////                            dataField: "OrderPrice",
////                            dataType: "number",
////                            format: { type: "currency", precision: 2 },
////                            valueFormat: "#0.00",

////                            //validationRules: [{ type: "required" }, {
////                            //    type: "range",
////                            //    message: "Please enter valid price > 0",
////                            //    min: 0.01,
////                            //    max: Number.MAX_VALUE
////                            //}],
////                            allowEditing: false,
////                            //visible: false


////                        },
////                        {
////                            dataField: "OrderedQuantity",
////                            caption: "Ordered Qty",
////                            visible: false,
////                            // allowEditing: flag || !e.row.data.ApprovedSH 
////                            allowEditing: false



////                        },
////                        {
////                            dataField: "OrderStatus",

////                            setCellValue: function (rowData, value) {

////                                rowData.OrderStatus = value;


////                            },
////                            lookup: {
////                                dataSource: function (options) {

////                                    return {

////                                        store: OrderStatus_list,
////                                        filter: options.data ? ["OrderStatus", "=", "Closed"] : null


////                                    };
////                                },

////                                valueExpr: "ID",
////                                displayExpr: "OrderStatus",

////                            }
////                        },
////                        {
////                            dataField: "ActualDeliveryDate",
////                            dataType: "date",
////                            allowEditing: false,
////                            visible: false

////                        },
////                        {
////                            dataField: "Fund",

////                            setCellValue: function (rowData, value) {

////                                rowData.Fund = value;


////                            },
////                            lookup: {
////                                dataSource: function (options) {

////                                    return {

////                                        store: Fund_list,


////                                    };
////                                },

////                                valueExpr: "ID",
////                                displayExpr: "Fund",
////                                allowEditing: false

////                            }
////                        },




////                    ]
////                }],


////            onEditorPreparing: function (e) {



////                if (e.parentType === "dataRow" && e.dataField === "BU") {

////                    if (e.row.data.ApprovedSH == undefined)
////                        e.row.data.ApprovedSH = false;
////                    if (e.row.data.RequestToOrder == true)
////                        e.editorOptions.readOnly = true;
////                    else
////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH); //|| !addnewitem_flag;/*isValuepresent(e.value);*/
////                }
////                if (e.parentType === "dataRow" && e.dataField === "OEM") {
////                    if (e.row.data.ApprovedSH == undefined)
////                        e.row.data.ApprovedSH = false;
////                    if (e.row.data.RequestToOrder == true)
////                        e.editorOptions.readOnly = true;
////                    else
////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
////                }
////                if (e.parentType === "dataRow" && e.dataField === "DEPT") {
////                    if (e.row.data.ApprovedSH == undefined)
////                        e.row.data.ApprovedSH = false;
////                    if (e.row.data.RequestToOrder == true)
////                        e.editorOptions.readOnly = true;
////                    else
////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
////                }
////                if (e.parentType === "dataRow" && e.dataField === "Group") {
////                    if (e.row.data.ApprovedSH == undefined)
////                        e.row.data.ApprovedSH = false;
////                    if (e.row.data.RequestToOrder == true)
////                        e.editorOptions.readOnly = true;
////                    else
////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
////                }
////                if (e.parentType === "dataRow" && e.dataField === "Item_Name") {
////                    if (e.row.data.ApprovedSH == undefined)
////                        e.row.data.ApprovedSH = false;
////                    if (e.row.data.RequestToOrder == true)
////                        e.editorOptions.readOnly = true;
////                    else
////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
////                }
////                if (e.parentType === "dataRow" && e.dataField === "Reviewed_Quantity") {
////                    if (e.row.data.ApprovedSH == undefined)
////                        e.row.data.ApprovedSH = false;
////                    if (e.row.data.RequestToOrder == true)
////                        e.editorOptions.readOnly = true;
////                    else
////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
////                }
////                if (e.parentType === "dataRow" && e.dataField === "Reviewed_Cost") {
////                    if (e.row.data.ApprovedSH == undefined)
////                        e.row.data.ApprovedSH = false;
////                    if (e.row.data.RequestToOrder == true)
////                        e.editorOptions.readOnly = true;
////                    else
////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
////                }
////                if (e.parentType === "dataRow" && e.dataField === "Fund") {
////                    if (e.row.data.ApprovedSH == undefined)
////                        e.row.data.ApprovedSH = false;
////                    if (e.row.data.RequestToOrder == true)
////                        e.editorOptions.readOnly = true;
////                    else
////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
////                }
////                if (e.parentType === "dataRow" && e.dataField === "OrderStatus") {

////                    e.editorOptions.readOnly = !isDelivered(e.row.data.OrderStatus);
////                }
////                if (e.parentType === "dataRow" && e.dataField === "RequiredDate") {
////                    //debugger;
////                    e.editorOptions.readOnly = e.row.data.RequestToOrder && (!isDelivered(e.row.data.OrderStatus) || isF03F01(e.row.data.Fund));  //1 & (0  || fo3)
////                }


////                var component = e.component,
////                    rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

////                if (e.parentType === "dataRow" && e.dataField === "Group") {

////                    e.editorOptions.disabled = (typeof e.row.data.DEPT !== "number");
////                    if (e.editorOptions.disabled)
////                        e.editorOptions.placeholder = 'Select Dept first';
////                    if (!e.editorOptions.disabled)
////                        e.editorOptions.placeholder = 'Select Group';

////                }



////                if (e.dataField === "BU") {

////                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
////                    e.editorOptions.onValueChanged = function (e) {
////                        onValueChanged.call(this, e);

////                        $.ajax({

////                            type: "post",
////                            url: "/BudgetingOrder/GetReviewer",
////                            data: { BU: e.value },
////                            datatype: "json",
////                            traditional: true,
////                            success: function (data) {

////                                reviewer_2 = data;

////                            }
////                        })
////                        // Emulating a web service call
////                        window.setTimeout(function () {
////                            component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
////                        }, 1000);
////                    }
////                }


////                if (e.dataField === "Item_Name") {

////                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
////                    e.editorOptions.onValueChanged = function (e) {
////                        onValueChanged.call(this, e);
////                        $.ajax({
////                            type: "post",
////                            url: "/BudgetingOrder/GetUnitPrice",
////                            data: { ItemName: e.value },
////                            datatype: "json",
////                            traditional: true,
////                            success: function (data) {

////                                if (data > 0)
////                                    unitprice = data;

////                            }
////                        })

////                        $.ajax({

////                            type: "post",
////                            url: "/BudgetingOrder/GetCategory",
////                            data: { ItemName: e.value },
////                            datatype: "json",
////                            traditional: true,
////                            success: function (data) {
////                                category = data;

////                            }
////                        })

////                        $.ajax({

////                            type: "post",
////                            url: "/BudgetingOrder/GetCostElement",
////                            data: { ItemName: e.value },
////                            datatype: "json",
////                            traditional: true,
////                            success: function (data) {
////                                costelement = data;

////                            }
////                        })

////                        $.ajax({

////                            type: "post",
////                            url: "/BudgetingOrder/GetBudgetCode",
////                            data: { ItemName: e.value },
////                            datatype: "json",
////                            traditional: true,
////                            success: function (data) {
////                                BudgetCode = data;

////                            }
////                        })

////                        window.setTimeout(function () {

////                            component.cellValue(rowIndex, "Unit_Price", unitprice);
////                            component.cellValue(rowIndex, "Category", category);
////                            component.cellValue(rowIndex, "Cost_Element", costelement);
////                            component.cellValue(rowIndex, "BudgetCode", BudgetCode);

////                        },
////                            1000);


////                    }

////                }


////            },
////            onRowUpdated: function (e) {
////                $.notify(" Your Item Request is being Updated...Please wait!", {
////                    globalPosition: "top center",
////                    className: "success"
////                })
////                Selected = [];
////                //var LeadTime_tocalc_ExpReqdDt;
////                //debugger;
////                // e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
////                // e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
////                if (e.data.RequiredDate && !e.data.RequestToOrder) {

////                    //    $.ajax({

////                    //        type: "GET",
////                    //        url: "/BudgetingOrder/GetLeadTime",
////                    //        data: { 'ItemName': e.data.Item_Name },
////                    //        datatype: "json",
////                    //        async: false,
////                    //        success: success_getleadtime,

////                    //    });

////                    //    function success_getleadtime(response) {
////                    //        //debugger;
////                    //        if (response == 0) {
////                    //            LeadTime_tocalc_ExpReqdDt = "";
////                    //            Selected.push(e.data);
////                    //            //debugger;
////                    //            Update(Selected, filtered_yr);
////                    //        }
////                    //        else
////                    //        {
////                    //            LeadTime_tocalc_ExpReqdDt = response;     
////                    var ReqdDate = e.data.RequiredDate;
////                    //            //debugger;
////                    $.ajax({

////                        type: "GET",
////                        url: "/BudgetingOrder/ValidateRequiredDate",
////                        data: { /*'LeadTime': LeadTime_tocalc_ExpReqdDt,*/ 'RequiredDate': ReqdDate },
////                        datatype: "json",
////                        async: false,
////                        success: success_validateReqdDate,

////                    });
////                    function success_validateReqdDate(info) {
////                        //debugger;
////                        if (info) {
////                            $.notify(info, {
////                                globalPosition: "top center",
////                                className: "error"
////                            })
////                        }
////                        else {
////                            Selected.push(e.data);
////                            //debugger;
////                            Update(Selected, filtered_yr);
////                        }
////                    }

////                    //        }


////                    //    }


////                }
////                else {
////                    Selected.push(e.data);
////                    //debugger;
////                    Update(Selected, filtered_yr);

////                }

////            },

////            onRowInserting: function (e) {
////                addnewitem_flag = false;

////                // e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
////                // e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
////                Selected = [];
////                Selected.push(e.data);



////                Update(Selected, filtered_yr);
////            },
////            onRowRemoving: function (e) {

////                Delete(e.data.RequestID, filtered_yr);

////            },
////            //masterDetail: {
////            //    enabled: true,

////            //    template(container, options) {
////            //        //debugger;
////            //        if (options.data.OrderDate != "" && options.data.OrderDate != null && options.data.OrderDate != undefined) {
////            //            const currentRequestData = options.data;

////            //            $('<div>')
////            //                .addClass('master-detail-caption')
////            //                .text(`${currentRequestData.Item_Name} Purchase details:`)
////            //                .appendTo(container);

////            //            $('<div>')
////            //                .dxDataGrid({
////            //                    columnAutoWidth: true,
////            //                    showBorders: true,
////            //                    headerFilter: {
////            //                        visible: true,
////            //                        applyFilter: "auto"
////            //                    },
////            //                    searchPanel: {
////            //                        visible: true,
////            //                        width: 240,
////            //                        placeholder: "Search..."
////            //                    },
////            //                    columns: [

////            //                        {

////            //                            alignment: "center",
////            //                            columns: [

////            //                                {
////            //                                    dataField: "RequestID",
////            //                                    allowEditing: false,
////            //                                    visible: false
////            //                                },

////            //                                //{
////            //                                //    dataField: "Unit_Price",
////            //                                //    caption: "Unit Price",
////            //                                //    dataType: "number",
////            //                                //    format: { type: "currency", precision: 0 },
////            //                                //    valueFormat: "#0",
////            //                                //    allowEditing: false,
////            //                                //    validationRules: [{ type: "required" }, {
////            //                                //        type: "range",
////            //                                //        message: "Please enter valid price > 0",
////            //                                //        min: 0.01,
////            //                                //        max: Number.MAX_VALUE
////            //                                //    }],
////            //                                //    allowEditing: false,
////            //                                //    visible: false


////            //                                //},
////            //                                //{
////            //                                //    dataField: "Total_Price",
////            //                                //    width: 100,
////            //                                //    calculateCellValue: function (rowData) {

////            //                                //        if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
////            //                                //            return rowData.Unit_Price * rowData.Required_Quantity;
////            //                                //        }
////            //                                //        else
////            //                                //            return 0.0;
////            //                                //    },

////            //                                //    dataType: "number",
////            //                                //    format: { type: "currency", precision: 0 },
////            //                                //    valueFormat: "#0",
////            //                                //    allowEditing: false
////            //                                //},
////            //                                , {
////            //                                    dataField: "ItemDescription",
////            //                                    width: 250
////            //                                },
////            //                                {
////            //                                    dataField: "PONumber",
////            //                                    caption: "PO",
////            //                                    allowEditing: false,
////            //                                },



////            //                                , {
////            //                                    dataField: "BudgetCode",
////            //                                    allowEditing: false,
////            //                                    visible: false
////            //                                },


////            //                                {
////            //                                    dataField: "POQuantity",
////            //                                    allowEditing: false,
////            //                                    caption: "PO Qty"
////            //                                },

////            //                                {
////            //                                    dataField: "Netvalue_USD",
////            //                                    allowEditing: false,
////            //                                    visible: false
////            //                                },


////            //                                {
////            //                                    dataField: "POCreatedOn",
////            //                                    caption: "Order Dt",
////            //                                    dataType: "date",
////            //                                    allowEditing: true,
////            //                                },



////            //                                {
////            //                                    dataField: "TentativeDeliveryDate",
////            //                                    allowEditing: true,
////            //                                    caption: "Tentative",

////            //                                    dataType: "date",
////            //                                },


////            //                                {
////            //                                    dataField: "ActualDeliveryDate",
////            //                                    allowEditing: true,
////            //                                    caption: "Actual Dt",
////            //                                    dataType: "date",
////            //                                },

////            //                                , {
////            //                                    dataField: "Currentstatus",
////            //                                    caption: "Status",
////            //                                    setCellValue: function (rowData, value) {
////            //                                        //debugger;
////            //                                        rowData.Fund = value;

////            //                                    },
////            //                                    lookup: {
////            //                                        dataSource: function (options) {
////            //                                            //debugger;
////            //                                            return {

////            //                                                store: OrderStatus_list,
////            //                                            };

////            //                                        },
////            //                                        valueExpr: "ID",
////            //                                        displayExpr: "OrderStatus"
////            //                                    },
////            //                                }

////            //                            ]
////            //                        }],
////            //                    //dataSource: function (options1) {
////            //                    //    //debugger;
////            //                    //    return {

////            //                    //        store: SubItemList,
////            //                    //        filter: options1.data ? ['RequestID', '=', options.key] : null
////            //                    //    };
////            //                    //},
////            //                    //dataSource: new DevExpress.data.DataSource({
////            //                    //    store: SubItemList,
////            //                    //    filter: ['RequestID', '=', options.key],
////            //                    //}),
////            //                    dataSource: new DevExpress.data.DataSource({
////            //                        store: new DevExpress.data.ArrayStore({
////            //                            key: 'ID',
////            //                            data: SubItemList,
////            //                        }),
////            //                        filter: ['RequestID', '=', options.key],
////            //                    }),
////            //                }).appendTo(container);
////            //        }

////            //    },
////            //}
////        });


////        // }
////        //else {
////        //    //debugger;
////        //    $.notify(response.message, {
////        //        globalPosition: "top center",
////        //        className: "error"
////        //    })

////        //    //Hide the Loading indicator once the Request List is fetched
////        //    genSpinner_load.classList.remove('fa');
////        //    genSpinner_load.classList.remove('fa-spinner');
////        //    genSpinner_load.classList.remove('fa-pulse');
////        //    document.getElementById("loadpanel").style.display = "none";
////        //    $("#RequestTable_RFO").prop('hidden', false);
////        //}




////    }

////    function OnError_GetData(response) {
////        $("#RequestTable_RFO").prop('hidden', false);
////        $.notify(data.message, {
////            globalPosition: "top center",
////            className: "warn"
////        })
////    }

////}




////$('#btnSubmitAll').click(function () {
////    LabAdminApprove(1999999999, filtered_yr);
////});




////function LabAdminApprove(id, filtered_yr) {
////    //debugger;
////    if (id == undefined) {
////        $.notify('Please check the Fund and Try again later!', {
////            globalPosition: "top center",
////            autoHideDelay: 20000,
////            className: "error"
////        });
////    }
////    else {

////        //debugger;
////        if (confirm('Do you confirm to place Request to Order the item(s)?')) {

////            var genSpinner = document.querySelector("#SubmitSpinner");
////            if (id == 1999999999) {
////                genSpinner.classList.add('fa');
////                genSpinner.classList.add('fa-spinner');
////                genSpinner.classList.add('fa-pulse');
////            }

////            $.ajax({
////                type: "POST",
////                url: encodeURI("../BudgetingOrder/LabAdminApprove"),
////                data: { 'id': id, 'useryear': filtered_yr },
////                success: function (data) {

////                    if (id == 1999999999) {

////                        genSpinner.classList.remove('fa');
////                        genSpinner.classList.remove('fa-spinner');
////                        genSpinner.classList.remove('fa-pulse');
////                    }




////                    $.ajax({
////                        type: "POST",
////                        url: "/BudgetingOrder/GetData",
////                        data: { 'year': filtered_yr },
////                        datatype: "json",
////                        async: true,
////                        success: success_refresh_getdata,
////                        error: error_refresh_getdata

////                    });
////                    function success_refresh_getdata(response) {

////                        var getdata = response.data;
////                        $("#RequestTable_RFO").dxDataGrid({
////                            dataSource: getdata
////                        });
////                    }
////                    function error_refresh_getdata(response) {

////                        $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
////                            globalPosition: "top center",
////                            className: "warn"
////                        });

////                    }

////                    //debugger;
////                    if (data.is_MailTrigger) {
////                        //debugger;

////                        $.ajax({
////                            type: "POST",
////                            url: encodeURI("../Budgeting/SendEmail_Order"),
////                            data: { 'emailnotify': data.data },
////                            success: success_email,
////                            error: error_email
////                        });

////                        function success_email(response) {
////                            $.notify("Mail has been sent to the LabTeam about your Request to Order!", {
////                                globalPosition: "top center",
////                                className: "success"
////                            })

////                        }
////                        function error_email(response) {
////                            $.notify("Unable to send mail to the LabTeam about your Request to Order!!", {
////                                globalPosition: "top center",
////                                className: "warn"
////                            })

////                        }
////                    }



////                    if (data.success) {

////                        $.notify(data.message, {
////                            globalPosition: "top center",
////                            className: "success"
////                        })
////                    }
////                    else {
////                        $.notify(data.message, {
////                            globalPosition: "top center",
////                            className: "error"
////                        })
////                    }



////                }
////            });



////        }
////    }
////}

////function Update(id1, filtered_yr) {
////    //debugger;

////    if (!id1[0].ApprovedSH && id1[0].Fund == 2) {
////        //debugger;
////        $.notify('Cannot add F02 items right now since Request Window has been closed.' + '\n' + ' Only F01/F03 items can be added at this stage!', {
////            globalPosition: "top center",
////            autoHideDelay: 20000,
////            className: "error"
////        });
////    }
////    else {
////        $.ajax({
////            type: "POST",
////            url: encodeURI("../BudgetingOrder/AddOrEdit"),
////            data: { 'req': id1[0], 'useryear': filtered_yr },
////            success: function (data) {
////                //debugger;
////                //newobjdata = data.data;

////                //$("#RequestTable_RFO").dxDataGrid({dataSource: newobjdata });
////                $.ajax({
////                    type: "POST",
////                    url: "/BudgetingOrder/GetData",
////                    data: { 'year': filtered_yr },
////                    datatype: "json",
////                    async: true,
////                    success: success_refresh_getdata,
////                    error: error_refresh_getdata

////                });

////                function success_refresh_getdata(response) {

////                    var getdata = response.data;
////                    $("#RequestTable_RFO").dxDataGrid({
////                        dataSource: getdata
////                    });
////                }
////                function error_refresh_getdata(response) {
////                    //debugger;
////                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
////                        globalPosition: "top center",
////                        className: "warn"
////                    });

////                }



////                if (data.success) {
////                    $.notify(data.message, {
////                        globalPosition: "top center",
////                        className: "success"
////                    })
////                }
////                else {
////                    $.notify(data.message, {
////                        globalPosition: "top center",
////                        className: "error"
////                    })
////                }



////            }

////        });


////    }


////}

////function Delete(id, filtered_yr) {

////    $.ajax({
////        type: "POST",
////        url: "/BudgetingOrder/Delete",
////        data: { 'id': id, 'useryear': filtered_yr },
////        success: function (data) {
////            newobjdata = data.data;
////            $("#RequestTable_RFO").dxDataGrid({ dataSource: newobjdata });
////        }



////    });

////    $.notify(data.message, {
////        globalPosition: "top center",
////        className: "success"
////    })

////}




//////$(function () {
//////    // run the currently selected effect
//////    function runEffect() {
//////        // get effect type from
//////        var selectedEffect = "blind";

//////        var options = {};

//////        // Run the effect
//////        $("#effect").show(selectedEffect, options, 1000, callback);
//////    };

//////    function callback() {
//////        setTimeout(function () {
//////            $("#effect:visible").removeAttr("style").fadeOut();
//////        }, 60000);
//////    };

//////    // Set effect from select menu value
//////    $("#btn_summary").on("click", function () {
//////        runEffect();
//////    });

//////    $("#effect").hide();
//////});







////$("#buttonClearFilters").dxButton({
////    text: 'Clear Filters',
////    onClick: function () {
////        $("#RequestTable_RFO").dxDataGrid("clearFilter");
////    }
////});

////$('[data-toggle="tooltip"]').tooltip();

//////BULookup,OEMLookup,DeptLookup,GroupLookup,ItemNameLookup,CostElementLookup,CategoryLookup




//////Export data
////$("#export").click(function () {
////    //debugger;
////    $.ajax({

////        type: "POST",
////        url: "/BudgetingOrder/ExportDataToExcel/",
////        data: { 'useryear': filtered_yr },


////        success: function (export_result) {
////            //debugger;

////            var bytes = new Uint8Array(export_result.FileContents);
////            var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
////            var link = document.createElement('a');
////            link.href = window.URL.createObjectURL(blob);
////            link.download = export_result.FileDownloadName;
////            link.click();

////        },
////        error: function () {
////            alert("export error");
////        }

////    });
////});


//////$('#chkRequest').on('click', function () {
//////    var chkRequest;
//////    if (this.checked)
//////        chkRequest = true;
//////    else
//////        chkRequest = false;
//////    var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
//////    dataGridLEP1.beginUpdate();
//////    dataGridLEP1.columnOption('OEM', 'visible', chkRequest);
//////    dataGridLEP1.columnOption('Required_Quantity', 'visible', chkRequest);
//////    dataGridLEP1.columnOption('Total_Price', 'visible', chkRequest);
//////    dataGridLEP1.columnOption('Requestor', 'visible', chkRequest);
//////    dataGridLEP1.columnOption('Reviewer_1', 'visible', chkRequest);
//////    dataGridLEP1.columnOption('Reviewer_2', 'visible', chkRequest);
//////    dataGridLEP1.columnOption('SubmitDate', 'visible', chkRequest);
//////    dataGridLEP1.columnOption('Comments', 'visible', chkRequest);
//////    dataGridLEP1.columnOption('Project', 'visible', chkRequest);
//////    dataGridLEP1.endUpdate();
//////    // $('#RequestTable_RFO').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
//////    //// $('#RequestTable').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
//////    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
//////    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


//////});

//////$('#chkRFO').on('click', function () {
//////    var chkRFO;
//////    if (this.checked)
//////        chkRFO = true;
//////    else
//////        chkRFO = false;
//////    var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
//////    dataGridLEP1.beginUpdate();
//////    dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', chkRFO);
//////    //dataGridLEP1.columnOption('Fund', 'visible', chkRFO);
//////    dataGridLEP1.columnOption('LeadTime', 'visible', chkRFO);
//////    //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRFO);
//////    dataGridLEP1.columnOption('RequiredDate', 'visible', chkRFO);
//////    //dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
//////    dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
//////    dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', chkRFO);
//////    dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
//////    dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
//////    dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);
//////    dataGridLEP1.endUpdate();
//////    // $('#RequestTable').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO); -- tking more time
//////    //// $('#RequestTable').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
//////    //// $('#RequestTable').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
//////    //// $('#RequestTable').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderID', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderPrice', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderedQuantity', 'visible', chkRFO);



//////});

//////$('#chkItem').on('click', function () {
//////    var chkItem;
//////    if (this.checked)
//////        chkItem = true;
//////    else
//////        chkItem = false;
//////    var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
//////    dataGridLEP1.beginUpdate();
//////    dataGridLEP1.columnOption('Category', 'visible', chkItem);
//////    dataGridLEP1.columnOption('Cost_Element', 'visible', chkItem);
//////    dataGridLEP1.columnOption('Unit_Price', 'visible', chkItem);
//////    dataGridLEP1.endUpdate();
//////    //$('#RequestTable').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
//////    //$('#RequestTable').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
//////    //$('#RequestTable').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


//////});







////var chkRequest;
////var chkItem;
////var chkRFO;
////var chkNonVKM;
////$('#chkRequest').on('click', function () {

////    if (this.checked)
////        chkRequest = true;
////    else
////        chkRequest = false;
////    checkboxdata();
////    // $('#RequestTable').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
////    //// $('#RequestTable').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
////    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
////    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


////});

////$('#chkItem').on('click', function () {

////    if (this.checked)
////        chkItem = true;
////    else
////        chkItem = false;
////    checkboxdata();
////    //$('#RequestTable').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
////    //$('#RequestTable').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
////    //$('#RequestTable').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


////});


////$('#chkRFO').on('click', function () {

////    if (this.checked)
////        chkRFO = true;
////    else
////        chkRFO = false;
////    checkboxdata();
////    // $('#RequestTable').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO); -- tking more time
////    //// $('#RequestTable').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
////    //// $('#RequestTable').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
////    //// $('#RequestTable').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderID', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderPrice', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderedQuantity', 'visible', chkRFO);



////});

////$('#chkNonVKM').on('click', function () {

////    if (this.checked)
////        chkNonVKM = true;
////    else
////        chkNonVKM = false;

////    checkboxdata();
////    //$('#RequestTable').dxDataGrid('columnOption', 'Customer_Name', 'visible', NonVKM);
////    //$('#RequestTable').dxDataGrid('columnOption', 'Customer_Dept', 'visible', NonVKM);
////    //$('#RequestTable').dxDataGrid('columnOption', 'BM_Number', 'visible', NonVKM);
////    //$('#RequestTable').dxDataGrid('columnOption', 'Task_ID', 'visible', NonVKM);
////    //$('#RequestTable').dxDataGrid('columnOption', 'Resource_Group_Id', 'visible', NonVKM);
////    //$('#RequestTable').dxDataGrid('columnOption', 'PIF_ID', 'visible', NonVKM);




////});

////function checkboxdata() {
////    if ($('.chkvkm:checked').length == 0) {
////        //debugger;
////        var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
////        dataGridLEP1.beginUpdate();
////        dataGridLEP1.columnOption('OEM', 'visible', true);
////        dataGridLEP1.columnOption('Requestor', 'visible', false);
////        dataGridLEP1.columnOption('Reviewer_1', 'visible', false);
////        dataGridLEP1.columnOption('Reviewer_2', 'visible', false);
////        dataGridLEP1.columnOption('Project', 'visible', false);
////        dataGridLEP1.columnOption('RequestDate', 'visible', false);
////        dataGridLEP1.columnOption('Category', 'visible', false);
////        dataGridLEP1.columnOption('Cost_Element', 'visible', false);
////        dataGridLEP1.columnOption('BudgetCode', 'visible', false);
////        dataGridLEP1.columnOption('Unit_Price', 'visible', false);
////        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
////        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
////        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);


////        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', false);
////        dataGridLEP1.columnOption('LeadTime', 'visible', false);
////        //dataGridLEP1.columnOption('OrderStatus', 'visible', true);
////        dataGridLEP1.columnOption('RequiredDate', 'visible', false);
////        dataGridLEP1.columnOption('RequestOrderDate', 'visible', false);
////        dataGridLEP1.columnOption('OrderDate', 'visible', false);
////        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', false);
////        dataGridLEP1.columnOption('OrderID', 'visible', false);
////        dataGridLEP1.columnOption('OrderPrice', 'visible', false);
////        dataGridLEP1.columnOption('OrderedQuantity', 'visible', false);

////        dataGridLEP1.columnOption('Customer_Name', 'visible', false);
////        dataGridLEP1.columnOption('Customer_Dept', 'visible', false);
////        dataGridLEP1.columnOption('BM_Number', 'visible', false);
////        dataGridLEP1.columnOption('Task_ID', 'visible', false);
////        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', false);
////        dataGridLEP1.columnOption('PIF_ID', 'visible', false);

////        dataGridLEP1.columnOption('BU', 'visible', true);
////        dataGridLEP1.columnOption('DEPT', 'visible', true);
////        dataGridLEP1.columnOption('Group', 'visible', true);
////        dataGridLEP1.columnOption('Item_Name', 'visible', true);
////        dataGridLEP1.columnOption('LeadTime', 'visible', true);

////        dataGridLEP1.endUpdate();

////    }
////    else if (('.chkvkm:checked').length == $('.chkvkm').length) {//chk if purchase spoc / vkm spoc
////        var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
////        dataGridLEP1.beginUpdate();
////        dataGridLEP1.columnOption('OEM', 'visible', true);
////        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
////        dataGridLEP1.columnOption('Total_Price', 'visible', true);
////        dataGridLEP1.columnOption('Requestor', 'visible', true);
////        dataGridLEP1.columnOption('Reviewer_1', 'visible', true);
////        dataGridLEP1.columnOption('Reviewer_2', 'visible', true);
////        dataGridLEP1.columnOption('SubmitDate', 'visible', true);
////        dataGridLEP1.columnOption('Comments', 'visible', true);
////        dataGridLEP1.columnOption('Project', 'visible', true);
////        dataGridLEP1.columnOption('Category', 'visible', true);
////        dataGridLEP1.columnOption('Cost_Element', 'visible', true);
////        dataGridLEP1.columnOption('BudgetCode', 'visible', true);
////        dataGridLEP1.columnOption('Unit_Price', 'visible', true);
////        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
////        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
////        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);


////        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', chkRFO);
////        dataGridLEP1.columnOption('LeadTime', 'visible', chkRFO);
////        //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRFO);
////        dataGridLEP1.columnOption('RequiredDate', 'visible', chkRFO);
////        dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
////        dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
////        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', chkRFO);
////        dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
////        dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
////        dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);

////        dataGridLEP1.columnOption('Customer_Name', 'visible', chkNonVKM);
////        dataGridLEP1.columnOption('Customer_Dept', 'visible', chkNonVKM);
////        dataGridLEP1.columnOption('BM_Number', 'visible', chkNonVKM);
////        dataGridLEP1.columnOption('Task_ID', 'visible', chkNonVKM);
////        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', chkNonVKM);
////        dataGridLEP1.columnOption('PIF_ID', 'visible', chkNonVKM);

////        dataGridLEP1.columnOption('BU', 'visible', true);
////        dataGridLEP1.columnOption('DEPT', 'visible', true);
////        dataGridLEP1.columnOption('Group', 'visible', true);
////        dataGridLEP1.columnOption('Item_Name', 'visible', true);
////        dataGridLEP1.columnOption('LeadTime', 'visible', true);

////        dataGridLEP1.endUpdate();
////    }
////    else {
////        var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
////        dataGridLEP1.beginUpdate();
////        dataGridLEP1.columnOption('OEM', 'visible', chkRequest);
////        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
////        dataGridLEP1.columnOption('Total_Price', 'visible', true);
////        dataGridLEP1.columnOption('Requestor', 'visible', chkRequest);
////        dataGridLEP1.columnOption('Reviewer_1', 'visible', chkRequest);
////        dataGridLEP1.columnOption('Reviewer_2', 'visible', chkRequest);
////        dataGridLEP1.columnOption('SubmitDate', 'visible', chkRequest);
////        dataGridLEP1.columnOption('Comments', 'visible', chkRequest);
////        dataGridLEP1.columnOption('Project', 'visible', chkRequest);

////        dataGridLEP1.columnOption('Category', 'visible', chkItem);
////        dataGridLEP1.columnOption('Cost_Element', 'visible', chkItem);
////        dataGridLEP1.columnOption('BudgetCode', 'visible', chkItem);
////        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
////        dataGridLEP1.columnOption('Unit_Price', 'visible', true);


////        dataGridLEP1.columnOption('BU', 'visible', chkRequest);
////        dataGridLEP1.columnOption('DEPT', 'visible', chkRequest);
////        dataGridLEP1.columnOption('Group', 'visible', chkRequest);
////        dataGridLEP1.columnOption('Item_Name', 'visible', true);

////        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', chkRFO);
////        dataGridLEP1.columnOption('LeadTime', 'visible', chkRFO);
////        //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRequest);
////        dataGridLEP1.columnOption('RequiredDate', 'visible', chkRFO);
////        dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
////        dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
////        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', chkRFO);
////        dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
////        dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
////        dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);

////        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
////        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);
////        dataGridLEP1.columnOption('LeadTime', 'visible', true);

////        dataGridLEP1.columnOption('Customer_Name', 'visible', chkNonVKM);
////        dataGridLEP1.columnOption('Customer_Dept', 'visible', chkNonVKM);
////        dataGridLEP1.columnOption('BM_Number', 'visible', chkNonVKM);
////        dataGridLEP1.columnOption('Task_ID', 'visible', chkNonVKM);
////        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', chkNonVKM);
////        dataGridLEP1.columnOption('PIF_ID', 'visible', chkNonVKM);


////        dataGridLEP1.endUpdate();
////    }
////}



//////$('#chkItem').on('click', function () {
//////    var chkItem;
//////    if (this.checked)
//////        chkItem = true;
//////    else
//////        chkItem = false;

//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


//////});

//////$('#chkRequest').on('click', function () {
//////    var chkRequest;
//////    if (this.checked)
//////        chkRequest = true;
//////    else
//////        chkRequest = false;

//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


//////});

//////$('#chkRFO').on('click', function () {
//////    var chkRFO;
//////    if (this.checked)
//////        chkRFO = true;
//////    else
//////        chkRFO = false;

//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
//////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);



//////});



////Javascript file for Budgeting Request Details - mae9cob    


//var dataGrid_order;
//var newobjdata;
//var BU_list, DEPT_list, Item_list, Category_list, OEM_list, Group_list, CostElement_list, OrderStatus_list, Fund_list, BudgetCodeList, PurchaseType_list, UOM_list, UnloadingPoint_list, OrderType_list, BudgetCodedesc, RFOApprover_list, CostCenter_list, BudgetCenter_list;
//var Selected = [];
//var Item_list_custom; //, UOMlist; 
//var unitprice, reviewer_2, category, costelement, leadtime, BudgetCode /*,*/ /*OrderTypeList, *//*UnloadingPointList,*/ /*rfoapproverlist,*/ /*CostCenter*/;
//var lookup_data, new_request, BudCenter;
//var filtered_yr;
//var leadtime1;
//var genSpinner_load = document.querySelector("#load");
//var SubItemList;
//var addnewitem_flag = false;

//var objdata_rfoview;
//var Item_headerFilter, DEPT_headerFilter, Group_headerFilter, BU_headerFilter, OEM_headerFilter, Category_headerFilter, CostElement_headerFilter, OrderStatus_headerFilter, BudgetCode_headerFilter;
///*Dynamic Fund Specific Fields - flag to detect whether F03/F05 and F06 specific fields are required based on user's selection of Fund*/
//var isFund_F03 = false; 
//var isFund_F05orF06 = false; 
//var popupadd, popupedit, gridedit;
//var file; //uploaded file info is stored in this variable


//$(".custom-file-input").on("change", function () {
//    var fileName = $(this).val().split("\\").pop();
//    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
//});

//var oems_chosen = [];

////////////////// OEM Selection
////var oems_chosen = new Array();
////$('input:checkbox[name=Types]:checked').each(function () {
////    //debugger;
////    Types.push($(this).val())
////});
////function fnOEMChange(oem) {
////    //debugger;
////    oems_chosen = [];
////    //oems_chosen = new Array();
////    for (var i = 0, len = oem.options.length; i < len; i++) {
////        if (document.getElementById('selectOEM').selectedIndex != -1) {
////            options = oem.options;
////            opt = options[i];
////            if (opt.selected) {
////                //store the labids chosen by user from dropdown to process the relevant chart data
////                oems_chosen.push(opt.value);
////            }
////        }
////    }
////    ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);


////}
////function fnOEMselectChange(oem) {
////    //debugger;
////    oems_chosen = [];
////    //oems_chosen = new Array();
////    for (var i = 0, len = oem.options.length; i < len; i++) {
////        if (document.getElementById('selectOEM').selectedIndex != -1) {
////            options = oem.options;
////            opt = options[i];
////            if (opt.selected) {
////                //store the labids chosen by user from dropdown to process the relevant chart data
////                oems_chosen.push(opt.value);
////            }
////        }
////    }
////    ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);

////}


////$.ajax({

////    type: "GET",
////    url: "/BudgetingOrder/Lookup",
////    async: false,
////    success: onsuccess_lookupdata,
////    error: onerror_lookupdata
////})


////function onsuccess_lookupdata(response) {

////    lookup_data = response.data;
////    BU_list = lookup_data.BU_List;
////    OEM_list = lookup_data.OEM_List;
////    DEPT_list = lookup_data.DEPT_List;
////    Group_list = lookup_data.Groups_test;
////    Item_list = lookup_data.Item_List;
////    Category_list = lookup_data.Category_List;
////    CostElement_list = lookup_data.CostElement_List;
////    OrderStatus_list = lookup_data.OrderStatus_List;
////    Fund_list = lookup_data.Fund_List;

////    //Item_list_New = lookup_data.Item_List1;

////}

////function onerror_lookupdata(response) {
////    alert("Error in fetching lookup");

////}

//$.ajax({
//    type: "GET",
//    url: encodeURI("../BudgetingOrder/InitRowValues"),
//    success: OnSuccessCall_dnew,
//    error: OnErrorCall_dnew

//});
//function OnSuccessCall_dnew(response) {

//    //debugger;

//    //$.ajax({

//    //    type: "post",
//    //    url: encodeURI("../BudgetingOrder/GetRFOBudgetCenter"),
//    //    data: { 'deptid': e.data.DEPT },
//    //    async: false,
//    //    success: function (data) {
//    //        //debugger;
//    //        BudCenter = data.data;

//    //        /*e.data.DEPT = BudgetCenter;*/
//    //    }
//    //})
//    //debugger;
//    new_request = response.data;
//    //popupadd=response.popupadd;
//    //popupedit=response.popupedit;
//    //gridedit = response.gridedit;
//    //debugger;
//}
//function OnErrorCall_dnew(response) {

//    //$.notify('Add new error!', {
//    //    globalPosition: "top center",
//    //    className: "warn"
//    //});
//}




////Loading indicator on load of the Request module while fetching the Item Requests
//window.onload = function () {
//    ////debugger;
//    document.getElementById("loadpanel").style.display = "block";


//    genSpinner_load.classList.add('fa');
//    genSpinner_load.classList.add('fa-spinner');
//    genSpinner_load.classList.add('fa-pulse');
//    $("#RequestTable_RFO").prop('hidden', true);

//    //$("#chkRFO").attr("autocomplete", "off");
//    //$("#chkRequest").attr("autocomplete", "off");
//    //$("#chkItem").attr("autocomplete", "off");

//    //document.getElementById('chkRFO').reset();
//    //document.getElementById('chkRequest').reset();
//    //document.getElementById('chkItem').reset();




//    //$("#chkRFO").prop("checked", false);
//    //$("#chkRequest").prop("checked", false);
//    //$("#chkItem").prop("checked", false);

//    //chkRFO
//    //chkItem
//    //chkRequest
//};



////Reference the DropDownList for Year to be selected by Requestor
//var ddlYears = document.getElementById("ddlYears");
////Determine the Current Year.
//var currentYear = (new Date()).getFullYear();
//////debugger;
////Loop and add the Year values to DropDownList.
////for (var i = currentYear; i >= 2020; i--) {
////    var option = document.createElement("OPTION");
////    option.innerHTML = i;
////    option.value = i;
////    ddlYears.appendChild(option);
////}
//////Loop and add the Year values to DropDownList.
//for (var i = currentYear + 1; i >= 2022; i--) {
//    var option = document.createElement("OPTION");
//    option.innerHTML = i;
//    option.value = i;
//    ddlYears.appendChild(option);

//    if (option.value == (currentYear + 1)) {
//        //if (option.value == (currentYear - 2)) {
//        option.defaultSelected = true;
//        //option.defaultSelected = true;
//    }
//    filtered_yr = $("#ddlYears").val();
//    filtered_yr = parseInt(filtered_yr) - 1;
//    filtered_yr = filtered_yr.toString();
//    ////debugger;
//}




////At load, Display the data for Current year
//if (filtered_yr == null) {
//    filtered_yr = new Date().getFullYear();
//}
//////debugger;
////$('.selectpicker').selectpicker('selectAll');//it wil hit fnoemchange to select all & then execute ajaxcallforrequestui
//ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);
//////debugger;

////Function to change year from dropdown
//function fnYearChange(yearselect) {
//    ////debugger;
//    $("#RequestTable_RFO").prop('hidden', true);
//    document.getElementById("loadpanel").style.display = "block";

//    genSpinner_load = document.querySelector("#load");
//    genSpinner_load.classList.add('fa');
//    genSpinner_load.classList.add('fa-spinner');
//    genSpinner_load.classList.add('fa-pulse');
//    filtered_yr = yearselect.value;

//    filtered_yr = parseInt(yearselect.value) - 1;
//    filtered_yr = filtered_yr.toString();
//    ////debugger;
//    //Ajax call to Get Request Item Data
//    ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);



//}


//function ajaxCallforRequestUI(filtered_yr) {

//    $(':checkbox').prop('checked', false);
//    /*****T To populate sub items ordered under the Planned Items *****/
//    //$.ajax({
//    //    type: "POST",
//    //    url: "/BudgetingOrder/GetPODetails",
//    //    datatype: "json",
//    //    success: function (data) {
//    //        ////debugger;
//    //        if (data.data.length > 0) {
//    //            ////debugger;
//    //            //var res = JSON.parse(data.data.Data.Content);
//    //            SubItemList = eval(data.data);
//    //            //LoadDataGrid(res);

//    //        }
//    //    },
//    //    error: function (jqXHR, exception) {
//    //        ////debugger;
//    //        var msg = '';
//    //        if (jqXHR.status === 0) {
//    //            msg = 'Not connect.\n Verify Network.';
//    //        } else if (jqXHR.status == 404) {
//    //            msg = 'Requested page not found. [404]';
//    //        } else if (jqXHR.status == 500) {
//    //            msg = 'Internal Server Error [500].';
//    //        } else if (exception === 'parsererror') {
//    //            msg = 'Requested JSON parse failed.';
//    //        } else if (exception === 'timeout') {
//    //            msg = 'Time out error.';
//    //        } else if (exception === 'abort') {
//    //            msg = 'Ajax request aborted.';
//    //        } else {
//    //            msg = 'Uncaught Error.\n' + jqXHR.responseText;
//    //        }
//    //        $('#post').html(msg);
//    //    }
//    //});

//    /****** To populate the user selection dropdown lists *******/
//    $.ajax({

//        type: "GET",
//        url: "/BudgetingOrder/Lookup",
//        async: false,
//        data: { 'year': filtered_yr },
//        success: onsuccess_lookupdata,
//        error: onerror_lookupdata
//    })


//    function onsuccess_lookupdata(response) {

//        lookup_data = response.data;
//        BU_list = lookup_data.BU_List;
//        OEM_list = lookup_data.OEM_List;
//        DEPT_list = lookup_data.DEPT_List;
//        Group_list = lookup_data.Groups_test;
//        Item_list = lookup_data.Item_List;
//        Category_list = lookup_data.Category_List;
//        CostElement_list = lookup_data.CostElement_List;
//        OrderStatus_list = lookup_data.OrderStatus_List;
//        Fund_list = lookup_data.Fund_List;
//        BudgetCodeList = lookup_data.BudgetCodeList;
//        PurchaseType_list = lookup_data.PurchaseType_List;
//        UnloadingPoint_list = lookup_data.UnloadingPoint_List;
//        OrderType_list = lookup_data.Order_Type_List;
//        UOM_list = lookup_data.UOM_List;
//        RFOApprover_list = lookup_data.RFOApprover_List;
//        CostCenter_list = lookup_data.CostCenter_List;
//        BudCenter = lookup_data.BudgetCenter_List;


//        Item_headerFilter = lookup_data.Item_HeaderFilter;
//        DEPT_headerFilter = lookup_data.DEPT_HeaderFilter;
//        Group_headerFilter = lookup_data.Group_HeaderFilter;
//        BU_headerFilter = lookup_data.BU_HeaderFilter;
//        OEM_headerFilter = lookup_data.OEM_HeaderFilter;
//        Category_headerFilter = lookup_data.Category_HeaderFilter;
//        CostElement_headerFilter = lookup_data.CostElement_HeaderFilter;
//        OrderStatus_headerFilter = lookup_data.OrderStatus_HeaderFilter;
//        BudgetCode_headerFilter = lookup_data.BudgetCode_HeaderFilter;
//        //debugger;
//        //Item_list_custom = Item_list.filter(function (item) {
//        //    return (item.VKM_Year === curryear);
//        //});
//        //Item_list_New = lookup_data.Item_List1;

//    }

//    function onerror_lookupdata(response) {
//        alert("Error in fetching lookup");

//    }



//    //Ajax call to Get Request Item Data
//    ////debugger;
//    $.ajax({
//        type: "POST",
//        url: encodeURI("../BudgetingOrder/GetData"),
//        data: { 'year': filtered_yr },
//        success: OnSuccess_GetData,
//        error: OnError_GetData
//    });


//    function OnSuccess_GetData(response) {
//        ////debugger;
//        //if (response.success) {
//        objdata_rfoview = (response.data);
//        popupadd = response.popupadd;
//        popupedit = response.popupedit;
//        gridedit = response.gridedit;
//        //For initial req - to allow user to add f01, f03 req from cc - this req was not to be developed
//        //var isF03F01 = function (position) {

//        //    if (position == undefined)
//        //        return true;
//        //    else
//        //        return position && [1, 3].indexOf(position) >= 0;

//        //};
//        var isDelivered = function (position) {//cancelled also included

//            //CHANGE
//            return position && [5, 6, 10].indexOf(position) >= 0;

//        };


//        //Hide the Loading indicator once the Request List is fetched
//        genSpinner_load.classList.remove('fa');
//        genSpinner_load.classList.remove('fa-spinner');
//        genSpinner_load.classList.remove('fa-pulse');
//        document.getElementById("loadpanel").style.display = "none";
//        $("#RequestTable_RFO").prop('hidden', false);
//        //debugger;
//        $("#RequestTable_RFO").dxDataGrid({

//            dataSource: objdata_rfoview,
//            keyExpr: "RequestID",
//            toolbar: {
//                items: [
//                    'addRowButton',
//                    'columnChooserButton',
//                    {
//                        location: 'after',
//                        widget: 'dxButton',
//                        options: {
//                            icon: 'refresh',
//                            text: 'Clear Request Filters',
//                            hint: 'Clear Request Filters',
//                            onClick() {
//                                $("#RequestTable").dxDataGrid("clearFilter");
//                            },
//                        },


//                    }
//                ]
//            },
//            onToolbarPreparing: function (e) {
//                let toolbarItems = e.toolbarOptions.items;

//                let addRowButton = toolbarItems.find(x => x.name === "addRowButton");
//                if (addRowButton.options != undefined) { //undefined when any of the previous vkm year selected and add button is hidden
//                    addRowButton.options.text = "Add New Request";
//                    addRowButton.options.hint = "Add New Request";
//                    addRowButton.showText = "always";
//                }

//                let columnChooserButton = toolbarItems.find(x => x.name === "columnChooserButton");
//                columnChooserButton.options.text = "Hide Fields";
//                columnChooserButton.options.hint = "Hide Fields";
//                columnChooserButton.showText = "always";

//            },
//            width: "96.8vw",
//            height: "76vh",
//            showColumnLines: true,
//            showRowLines: true,
//            rowAlternationEnabled: true,
//            hoverStateEnabled: {
//                enabled: true
//            },
//            editing: {
//                mode: popupedit == true ? "popup" : "row", //"popup",

//                allowAdding: popupadd,

//                allowUpdating: function (e) {
//                   // //debugger;
//                    return (response.success && !response.isDashboard_flag && (isDelivered(e.row.data.OrderStatus) || !(e.row.data.RequestToOrder))); //if success = true - access to update and view ; false - access to only view
//                },
//                //allowDeleting: function (e) {

//                //    return !(e.row.data.RequestToOrder) && isF03F01(e.row.data.Fund);
//                //},
//                useIcons: true,
//                popup: {
//                    title: "Lab RFO",
//                    width: 900,
//                    height: 900,
//                    showTitle: true,
//                    visible: true,
//                    hideOnOutsideClick: true,
//                    //width: 450,
//                    //height: 350,
//                    resizeEnabled: true,
//                },
//                form: {
//                    items: [
//                        {
//                            itemType: 'group',
//                            caption: 'Request Details',
//                            colCount: 2,
//                            colSpan: 2,
//                            items: [
//                                {
//                                    dataField: 'BU',
//                                    label: {
//                                        text: 'BU'
//                                    },
//                                    editorOptions: {
//                                        value: 6
//                                    },
//                                    visible: false
//                                },
//                                {
//                                    dataField: 'OEM',
//                                    label: {
//                                        text: 'Customer'
//                                    },
//                                    dataType: 'string',
//                                    validationRules: [{
//                                        type: "required"
//                                    }],
//                                },
//                                {
//                                    dataField: 'DEPT',
//                                    label: {
//                                        text: 'Department'
//                                    },
//                                    dataType: 'string',
//                                    validationRules: [{
//                                        type: "required"
//                                    }],
//                                },

//                                {
//                                    dataField: 'Group',
//                                    label: {
//                                        text: 'Group'
//                                    },
//                                    dataType: 'string',
//                                    validationRules: [{
//                                        type: "required"
//                                    }],
//                                },


//                                {
//                                    dataField: 'GoodsRecID',
//                                    label: {
//                                        text: 'Goods Recipient ID'
//                                    },
//                                    dataType: 'string',
//                                    validationRules: [{
//                                        type: "required"
//                                    }],
//                                },
//                                {
//                                    dataField: 'RFOReqNTID', 
//                                    label: {
//                                        text: 'RFO Requestor NTID'
//                                    },
//                                    //validationRules: [{
//                                    //    type: "required"
//                                    //}],
//                                    //editorOptions: {
//                                    //    value: RequestorNTID,
//                                    //}
//                                },
//                                {
//                                    dataField: 'LabName',
//                                    label: {
//                                        text: 'Lab Name'
//                                    },
//                                    dataType: 'string',
//                                    validationRules: [{
//                                        type: "required"
//                                    }],
//                                },
//                                {
//                                    dataField: 'OrderType',
//                                    editorType: 'dxSelectBox',
//                                    editorOptions: {
//                                        items: OrderType_list,
//                                        displayExpr: 'Order_Type',
//                                        valueExpr: 'ID',
//                                        searchEnabled: true,
//                                    },
//                                    label: {
//                                        text: 'Order Type'
//                                    },
//                                    dataType: 'string',
//                                    validationRules: [{
//                                        type: "required"
//                                    }],
//                                },
//                                {
//                                    dataField: 'UnloadingPoint',
//                                    label: {
//                                        text: 'Unloading Point'
//                                    },
//                                    editorType: 'dxSelectBox',
//                                    editorOptions: {
//                                        items: UnloadingPoint_list,
//                                        displayExpr: 'UnloadingPoint',
//                                        valueExpr: 'ID',
//                                        searchEnabled: true,
//                                    },
//                                    validationRules: [{
//                                        type: "required"
//                                    }],

//                                },
//                                {
//                                    dataField: 'BudgetCenterID',
//                                    label: {
//                                        text: 'Budget Centre'
//                                    },
//                                    //dataType: 'string',
//                                    editorType: 'dxSelectBox',
//                                    editorOptions:
//                                    {
//                                        items: BudCenter,
//                                        displayExpr: 'BudgetCenter',
//                                        valueExpr: 'ID',
//                                        searchEnabled: true,
//                                        noDataText: "No Data. Kindly contact ELO Team",
//                                    },
//                                    validationRules: [{
//                                        type: "required"
//                                    }],

//                                },

//                                {
//                                    dataField: 'CostCenter',
//                                    label: {
//                                        text: 'Cost Centre'
//                                    },
//                                    dataType: 'string',
//                                    validationRules: [{
//                                        type: "required"
//                                    }],
//                                },

//                                {
//                                    dataField: 'RFOApprover',
//                                    label: {
//                                        text: 'RFO Approver'
//                                    },
//                                    dataType: 'string',
//                                    editorType: 'dxSelectBox',
//                                    editorOptions:
//                                    {
//                                        //items: RFOApprover_list,
//                                        //displayExpr: 'Section_Dept_Grp',
//                                        //valueExpr: 'Section_Dept_Grp',
//                                        //searchEnabled: true,
//                                    },
//                                    validationRules: [{
//                                        type: "required"
//                                    }],

//                                },

//                                //{
//                                //    dataField: 'Comments',
//                                //    label: {
//                                //        text: 'Comments'
//                                //    },
//                                //    dataType: 'string',
//                                //},
//                                {
//                                    dataField: 'PORemarks',
//                                    label: {
//                                        text: 'Item Justification'
//                                    },
//                                    dataType: 'string',

//                                },
//                            ],

//                        },
//                        {
//                            itemType: 'group',
//                            caption: 'Item Details',
//                            colCount: 2,
//                            colSpan: 2,
//                            items: [
//                                {
//                                    dataField: 'Item_Name',
//                                    label: {
//                                        text: 'Item Name'
//                                    },
//                                    editorType: 'dxSelectBox',
//                                    editorOptions: {
//                                        items: Item_list,
//                                        displayExpr: 'Item_Name',
//                                        valueExpr: 'S_No',
//                                        //filter: function (e) {
//                                        //    //debugger;
//                                        //    return (e.BU == 6 && e.OrderType == OrderType)
//                                        //}
//                                    },
//                                    validationRules: [{
//                                        type: "required"
//                                    }],
//                                },
//                                {
//                                    dataField: 'Required_Quantity',
//                                    editorType: 'dxNumberBox',
//                                    editorOptions: {
//                                        showSpinButtons: true,
//                                        min: 0,
//                                    },
//                                    label: {
//                                        text: 'Quantity'
//                                    },
//                                    dataType: 'string',
//                                    validationRules: [{
//                                        type: "required"
//                                    }],
//                                },
//                                {
//                                    dataField: 'Total_Price',
//                                    label: {
//                                        text: 'Total Cost ($)'
//                                    },
//                                    dataType: "number",
//                                    format: { type: "currency", precision: 2 },
//                                },
//                                {
//                                    dataField: 'BudgetCode',
//                                    label: {
//                                        text: 'Budget Code'
//                                    },
//                                    dataType: 'string',
//                                },
//                                {
//                                    dataField: 'BudgetCodeDescription',
//                                    label: {
//                                        text: 'Budget Code Description'
//                                    },
//                                    dataType: 'string',
//                                },
//                                {
//                                    dataField: 'Cost_Element',
//                                    label: {
//                                        text: 'Cost Element'
//                                    },
//                                    dataType: 'string',
//                                },
//                                {
//                                    dataField: 'Unit_Price',
//                                    label: {
//                                        text: 'Unit Price ($)'
//                                    },
//                                    //dataType: 'number',
//                                    format: { type: 'currency', precision: 2 },
//                                },
//                                {
//                                    dataField: 'UnitofMeasure',
//                                    label: {
//                                        text: 'Unit of Measure'
//                                    },
//                                    dataType: 'string',
//                                    //editorType: 'dxSelectBox',
//                                    //editorOptions: {
//                                    //    items: UOM_list,
//                                    //    displayExpr: 'UOM',
//                                    //    valueExpr: 'ID',
//                                    //    //filter: function (e) {
//                                    //    //    //debugger;
//                                    //    //    return (e.BU == 6 && e.OrderType == OrderType)
//                                    //    //}
//                                    //}
//                                },
//                                //{
//                                //    dataField: 'Currency',
//                                //    label: {
//                                //        text: 'Currency'
//                                //    },
//                                //    dataType: 'string',
//                                //},
//                                {
//                                    dataField: 'Fund',
//                                    label: {
//                                        text: 'Fund'
//                                    },
//                                    dataType: 'string',
//                                    validationRules: [{
//                                        type: "required"
//                                    }],
//                                },

//                                {
//                                    dataField: 'QuoteAvailable',
//                                    label: {
//                                        text: 'Quote Available'
//                                    },
//                                    editorType: 'dxRadioGroup',
//                                    editorOptions: {

//                                        dataSource: ["Yes", "No"],
//                                        layout: "horizontal",


//                                    },
//                                    dataType: 'string',
//                                    validationRules: [{
//                                        type: "required"
//                                    }],
//                                },

//                            ]
//                        },
//                        {
//                            itemType: 'group',
//                            visible: false,
//                           caption: 'F03',
//                            //cssClass: "DynamicField_css", //to ignore the dataField "F03" , "F05_and_F06" from being shown (reason: in general, only caption(which is displayed as a subtitle for grouped items) has to be specified for grouping items ; this caption can be used in dxForm.itemOption mtd to change the visibility. Challenge : The dxForm.itemOption mtd does not accept caption name with whitespaces (eg: "F05 and F06"). Hence, dummy dataField is provided with _ (eg: "F05_and_F06")). On using dataField along with Caption for a group of items, the dataField is also displayed as one of the items in the group, which is undesired. Hence, this has to be hidden.
//                            colCount: 2,
//                            colSpan: 2,
//                            items: [

//                                {
//                                    dataField: 'BM_Number',
//                                    dataType: 'string',
//                                    validationRules: [{ type: "required" }],
//                                },                         
//                                {
//                                    dataField: 'Resource_Group_Id',
//                                    dataType: 'string',
//                                    validationRules: [{ type: "required" }],
//                                },
//                                {
//                                    dataField: 'PIF_ID',
//                                    dataType: 'string',
//                                    validationRules: [{ type: "required" }],
//                                },
//                                {
//                                    dataField: 'Material_Part_Number',
//                                    dataType: 'string',
//                                    validationRules: [{ type: "required" }],
//                                },
//                                {
//                                    dataField: 'SupplierName_with_Address',
//                                    cssClass: "SupplierName_css",
//                                    dataType: 'string',
//                                    validationRules: [{ type: "required" }],
//                                },


//                            ]
//                        },
//                        {
//                            itemType: 'group',
//                            visible: false,
//                            caption: 'F05 and F06',
//                            name: 'F05_and_F06',
//                            colCount: 2,
//                            colSpan: 2,
//                            items: [
//                                {
//                                    dataField: 'Purchase_Type',
//                                    cssClass: "Purchase_Type_css",
//                                    editorType: 'dxSelectBox',
//                                    editorOptions: {
//                                        items: PurchaseType_list,
//                                        displayExpr: 'PurchaseType',
//                                        valueExpr: 'ID',
//                                    },
//                                    validationRules: [{ type: "required" }],
//                                },
//                                {
//                                    dataField: 'Project_ID',
//                                    dataType: 'string',

//                                },
//                                {
//                                    dataField: 'SupplierName_with_Address',
//                                    cssClass: "SupplierName_css",
//                                    dataType: 'string',
//                                    validationRules: [{ type: "required" }],
//                                },



//                            ]
//                        }
//                    ]
//                }
//            },
//            onContentReady: function (e) {

//                e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
//            },
//            onCellPrepared: function (e) {
//                if (e.rowType === "header" || e.rowType === "filter") {
//                    e.cellElement.addClass("columnHeaderCSS");
//                    e.cellElement.find("input").addClass("columnHeaderCSS");
//                }
//            },
//             onEditingStart: function (e) {
//                 //debugger;
//                 if (e.data.BudgetCenterID.constructor.name != "Number") {
//                     var selectedRowIndex = e.component.getRowIndexByKey(e.key);
//                     var dataGrid = $("#RequestTable_RFO").dxDataGrid("instance");
//                     dataGrid.cellValue(selectedRowIndex, "RFOApprover", Group_list.find(x => x.ID == e.data.Group).Group);
//                 } 
//                 //dataGrid.saveEditData();

//                 if (e.data.BudgetCenterID.constructor.name != "Number") {
//                      $.ajax({

//                     type: "post",
//                     url: "/BudgetingOrder/GetRFOBudgetCenter",
//                     data: { 'deptid': e.data.DEPT },
//                     datatype: "json",
//                     traditional: true,
//                     success: function (data) {
//                         //debugger;
//                         BudCenter = data.data;
//                         if (BudCenter.length == 0) {
//                             //debugger;
//                             //$.notify("Please Contact ELO Team to find BudgetCenter details !", {
//                             //    elementPosition: "right",
//                             //    className: "error",
//                             //    autoHideDelay: 13000,
//                             //});
//                             alert("Please Contact ELO Team to find BudgetCenter details !");
//                             //debugger;
//                         }
//                         else {
//                             dataGrid.cellValue(selectedRowIndex, "BudgetCenterID", BudCenter);
//                            // dataGrid.saveEditData();
//                         }

//                     }
//                      })

//                 }



//            }, 
//            onInitNewRow: function (e) {
//                //debugger;
//                addnewitem_flag = true;
//                //e.data.Requestor = new_request.Requestor;
//                e.data.Reviewer_1 = new_request.Reviewer_1;
//                e.data.Reviewer_2 = new_request.Reviewer_2;
//                e.data.DEPT = new_request.DEPT;
//                e.data.Group = new_request.Group;
//                e.data.BU = new_request.BU;
//                e.data.OEM = new_request.OEM == 0 ? "" : new_request.OEM ;
//                e.data.RFOReqNTID = new_request.RequestorNTID;
//                e.data.RFOApprover = Group_list.find(x => x.ID == new_request.Group).Group;
//                //debugger;

//                e.data.BudgetCenterID = new_request.BudgetcenterList;
//                BudCenter = new_request.BudgetcenterList;

//                /*component.cellValue(rowIndex, "BudgetCenter", BudgetCenter);*/

//            },

//            columnAutoWidth: true,
//            allowColumnReordering: true,
//            allowColumnResizing: true,
//            //focusedRowEnabled: true,
//            allowColumnReordering: true,
//            allowColumnResizing: true,
//            keyExpr: "RequestID",

//            columnResizingMode: "widget",
//            columnChooser: {
//                enabled: true
//            },
//            //filterRow: {
//            //    visible: true

//            //},
//            showBorders: true,
//            headerFilter: {
//                visible: true,
//                applyFilter: "auto",
//                allowSearch: true
//            },
//            selection: {
//                applyFilter: "auto"
//            },
//            loadPanel: {
//                enabled: true
//            },
//            //    paging: {
//            //        pageSize: 100
//            //},

//            //searchPanel: {
//            //    visible: true,
//            //    width: 240,
//            //    placeholder: "Search..."
//            //},
//            //scrolling: {
//            //    columnRenderingMode: "virtual"
//            //},
//            scrolling: {
//                mode: "virtual",
//                rowRenderingMode: "virtual",
//                columnRenderingMode: "virtual"
//            },
//            columns: [
//                {
//                    type: "buttons",
//                    width: 90,
//                    alignment: "left",
//                    buttons: [
//                        "edit",//, "delete",
//                        {
//                            hint: "Submit",
//                            icon: "check",
//                            visible: function (e) {
//                                ////debugger;
//                                return ((response.success && !e.row.isEditing && !(e.row.data.RequestToOrder)) || (!e.row.isEditing && isDelivered(e.row.data.OrderStatus))) && !response.isDashboard_flag;/*&& !isOrderApproved(e.row.data.OrderStatus)*/;

//                            },
//                            onClick: function (e) {
//                                //    var LeadTime_tocalc_ExpReqdDt;
//                                //    //debugger;
//                                //    // e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
//                                //    // e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
//                                if (e.row.data.RequiredDate && !e.row.data.RequestToOrder) {

//                                    //        $.ajax({

//                                    //            type: "GET",
//                                    //            url: "/BudgetingOrder/GetLeadTime",
//                                    //            data: { 'ItemName': e.row.data.Item_Name },
//                                    //            datatype: "json",
//                                    //            async: false,
//                                    //            success: success_getleadtime,

//                                    //        });

//                                    //        function success_getleadtime(response) {
//                                    //            //debugger;
//                                    //            if (response == 0) {
//                                    //                LeadTime_tocalc_ExpReqdDt = "";

//                                    //                LabAdminApprove(e.row.data.RequestID, filtered_yr);
//                                    //            }
//                                    //            else {
//                                    //                LeadTime_tocalc_ExpReqdDt = response;
//                                    var ReqdDate = e.row.data.RequiredDate;
//                                    //                //debugger;
//                                    $.ajax({

//                                        type: "GET",
//                                        url: "/BudgetingOrder/ValidateRequiredDate",
//                                        data: { 'RequiredDate': ReqdDate },
//                                        datatype: "json",
//                                        async: false,
//                                        success: success_validateReqdDate,

//                                    });
//                                    function success_validateReqdDate(info) {
//                                        ////debugger;
//                                        if (info) {
//                                            $.notify(info, {
//                                                globalPosition: "top center",
//                                                className: "error"
//                                            })
//                                        }
//                                        else {
//                                            LabAdminApprove(e.row.data.RequestID, filtered_yr);
//                                        }
//                                    }

//                                    //   }


//                                    //        }


//                                }
//                                else {

//                                    LabAdminApprove(e.row.data.RequestID, filtered_yr);
//                                }



//                                e.component.refresh(true);
//                                e.event.preventDefault();
//                            }

//                        }]
//                },
//                {

//                    alignment: "center",
//                    columns: [
//                        {
//                            dataField: "BU",

//                            width: 65,
//                            /*validationRules: [{ type: "required" }],*/

//                            lookup: {
//                                dataSource: BU_list,
//                                valueExpr: "ID",
//                                displayExpr: "BU"
//                            },
//                            headerFilter: {
//                                dataSource: BU_headerFilter,
//                                allowSearch: true
//                            },


//                        },
//                        {
//                            dataField: "OEM",
//                            validationRules: [{ type: "required" }],
//                            width: 70,
//                            lookup: {
//                                dataSource: OEM_list,
//                                valueExpr: "ID",
//                                displayExpr: "OEM"
//                            },
//                            headerFilter: {
//                                dataSource: OEM_headerFilter,
//                                allowSearch: true
//                            },


//                        },
//                        {
//                            dataField: "DEPT",
//                            caption: "Dept",
//                            validationRules: [{ type: "required" }],
//                            headerFilter: {
//                                dataSource: DEPT_headerFilter,
//                                allowSearch: true
//                            },
//                            setCellValue: function (rowData, value, currentRowData) {

//                                if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
//                                {
//                                    //debugger;
//                                    rowData.DEPT = value;
//                                    rowData.Group = null;
//                                    rowData.RFOApprover = null;
//                                    if (currentRowData.UnloadingPoint != undefined) {
//                                        var unloadingPt_Location = UnloadingPoint_list.find(x => x.ID == currentRowData.UnloadingPoint).Location;
//                                        var dept_name = DEPT_list.find(x => x.ID == rowData.DEPT).DEPT;
//                                        rowData.CostCenter = CostCenter_list.find(x => x.UnloadingPoint_Location == unloadingPt_Location && x.Dept == dept_name).CostCenter;

//                                    }


//                                }

//                            },
//                            width: 90,
//                            lookup: {
//                                dataSource: function (options) {
//                                    ////debugger;
//                                    return {

//                                        store: DEPT_list,
//                                        filter: options.data ? ["Outdated", "=", false] : null

//                                    };
//                                },

//                                valueExpr: "ID",
//                                displayExpr: "DEPT"

//                            },


//                        },
//                        {
//                            dataField: "Group",
//                            width: 90,
//                            headerFilter: {
//                                dataSource: Group_headerFilter,
//                                allowSearch: true
//                            },
//                            validationRules: [{ type: "required" }],
//                            lookup: {
//                                dataSource: function (options) {

//                                    return {

//                                        store: Group_list,

//                                        filter: options.data ? ["Dept", "=", options.data.DEPT] : null
//                                    };

//                                },
//                                valueExpr: "ID",
//                                displayExpr: "Group"
//                            },
//                            setCellValue: function (rowData, value) {
//                                //debugger;
//                                if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
//                                {
//                                    rowData.Group = value;
//                                    //rowData.RfOApprover = value;
//                                    rowData.RFOApprover = Group_list.find(x => x.ID == value).Group;
//                                }
//                            }


//                        },


//                        {
//                            dataField: "Item_Name",
//                            width: 200,
//                            headerFilter: {
//                                dataSource: Item_headerFilter,
//                                allowSearch: true
//                            },
//                            validationRules: [{ type: "required" }],
//                            lookup: {
//                                dataSource: function (options) {
//                                    //debugger;
//                                    return {

//                                        store: Item_list,
//                                        //filter: function (e) {
//                                        //    //debugger;
//                                        //    if (e.BU == Item_list.find(x => x.S_No).BU && e.OrderType == Item_list.find(x => x.S_No).OrderType) {
//                                        //        return true;
//                                        //    }
//                                        //}

//                                        filter: options.data ? [["BU", "=", 6], 'and', ["Order_Type", "=", options.data.OrderType]] : null

//                                    }

//                                },
//                                valueExpr: "S_No",
//                                displayExpr: "Item_Name"
//                            },
//                            calculateSortValue: function (data) {
//                                ////debugger;
//                                const value = this.calculateCellValue(data);
//                                return this.lookup.calculateCellValue(value);
//                            },
//                            setCellValue: function (rowData, value) {
//                                //debugger;
//                                //if value.constructur.name == "Array" => it means that the item list is filtered based on the ordertype selected. At that instance, no need to fetch other details
//                                if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
//                                {
//                                        rowData.Item_Name = value;
//                                        rowData.Category = Item_list.find(x => x.S_No == value).Category;
//                                        rowData.Cost_Element = parseInt(Item_list.find(x => x.S_No == value).Cost_Element);
//                                        rowData.Unit_Price = Item_list.find(x => x.S_No == value).UnitPriceUSD;
//                                        //rowData.ActualAvailableQuantity = Item_list.find(x => x.S_No == value).Actual_Available_Quantity;
//                                        rowData.BudgetCode = Item_list.find(x => x.S_No == value).BudgetCode;
//                                        rowData.BudgetCodeDescription = BudgetCodeList.find(x => x.Budget_Code == rowData.BudgetCode).Budget_Code_Description;
//                                    rowData.UnitofMeasure = parseInt(Item_list.find(x => x.S_No == value).UOM);
//                                }
//                            },

//                        },
//                        {
//                            dataField: "OrderType",
//                            caption: "Order Type",
//                            setCellValue: function (rowData, value) {
//                                //debugger;
//                                rowData.OrderType = value;
//                                rowData.Item_Name = null;

//                            },
//                            lookup: {
//                                dataSource: function (options) {
//                                    //debugger;
//                                    return {

//                                        store: OrderType_list,
//                                    }

//                                },
//                                valueExpr: "ID",
//                                displayExpr: "Order_Type"
//                            },
//                            visible: false

//                        },
//                        {
//                            dataField: "CostCenter",
//                            caption: "Cost Center",
//                            allowEditing: false,
//                            visible: false
//                        },
//                        {
//                            dataField: "BudgetCenterID",
//                            caption: "Budget Center",
//                            lookup: {
//                                dataSource: function (options) {
//                                    //debugger;
//                                    return {

//                                        store: BudCenter,
//                                    }

//                                },
//                                valueExpr: "ID",
//                                displayExpr: "BudgetCenter",
//                            },

//                            visible: false,
//                            //allowEditing: false,

//                        },
//                        {
//                            dataField: "UnitofMeasure",
//                            caption: "Unit of Measure",
//                            lookup: {
//                                dataSource: function (options) {
//                                    //debugger;
//                                    return {

//                                        store: UOM_list,


//                                    }

//                                },
//                                valueExpr: "ID",
//                                displayExpr: "UOM"
//                            },
//                            visible: false,
//                            allowEditing: false,
//                        },
//                        {
//                            dataField: "UnloadingPoint",
//                            caption: "Unloading Point",
//                            setCellValue: function (rowData, value, currentRowData) {
//                                //debugger;
//                                //if value.constructur.name == "Array" => it means that the item list is filtered based on the ordertype selected. At that instance, no need to fetch other details
//                                if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
//                                {
//                                    rowData.UnloadingPoint = value;
//                                    var unloadingPt_Location = UnloadingPoint_list.find(x => x.ID == value).Location;
//                                    var dept_name = DEPT_list.find(x => x.ID == currentRowData.DEPT).DEPT;
//                                    rowData.CostCenter = CostCenter_list.find(x => x.UnloadingPoint_Location == unloadingPt_Location && x.Dept == dept_name).CostCenter;
//                                        //Item_list.find(x => x.S_No == value).Category;

//                                }
//                            },
//                            visible: false,
//                            lookup: {
//                                dataSource: function (options) {

//                                    return {

//                                        store: UnloadingPoint_list,
//                                    }

//                                },
//                                valueExpr: "ID",
//                                displayExpr: "UnloadingPoint"
//                            },
//                        },
//                        {
//                            dataField: "Category",
//                            caption: "Category",
//                            validationRules: [{ type: "required" }],
//                            headerFilter: {
//                                dataSource: Category_headerFilter,
//                                allowSearch: true
//                            },
//                            lookup: {
//                                dataSource: Category_list,
//                                valueExpr: "ID",
//                                displayExpr: "Category"
//                            },
//                            allowEditing: false,
//                            visible: false

//                        },
//                        {
//                            dataField: "Cost_Element",
//                            //headerFilter: {
//                            //    //dataSource: CostElement_headerFilter,
//                            //    allowSearch: true
//                            //},
//                            lookup: {
//                                dataSource: CostElement_list,
//                                valueExpr: "ID",
//                                displayExpr: "CostElement"
//                            },
//                            allowEditing: false,
//                            visible: false


//                        },
//                        {
//                            dataField: "BudgetCode",
//                            headerFilter: {
//                                dataSource: BudgetCode_headerFilter,
//                                allowSearch: true
//                            },
//                            //lookup: {
//                            //    dataSource: BudgetCodeList,
//                            //    valueExpr: "BudgetCode",
//                            //    displayExpr: "BudgetCode"
//                            //},
//                            allowEditing: false,
//                            visible: false


//                        },
//                        {
//                            dataField: "BudgetCodeDescription",
//                            caption:"Budget Code Description",
//                            visible: false,
//                            allowEditing: false,
//                        },
//                        {
//                            dataField: "LabName",
//                            caption: "Lab Name",
//                            visible: false
//                        },
//                        {
//                            dataField: 'RFOReqNTID',
//                            setCellValue: function (rowData, value) {

//                                rowData.RFOReqNTID = value;

//                            },
//                            visible: false,
//                            allowEditing: false,
//                        },
//                        {
//                            dataField: "RFOApprover",
//                            caption: "RFO Approver",
//                            visible: false,
//                            lookup: {
//                                dataSource: RFOApprover_list,
//                                valueExpr: "Section_Dept_Grp",
//                                displayExpr: "Section_Dept_Grp"
//                            },

//                        },
//                        {
//                            dataField: "QuoteAvailable",
//                            caption: "Quote Available",
//                            visible: false,
//                            editCellTemplate: editCellTemplate


//                        },
//                        {
//                            dataField: "GoodsRecID",
//                            caption: "Goods Rec ID",
//                            visible: false

//                        },
//                        {
//                            dataField: "Required_Quantity",
//                            caption: "Required Qty",

//                            dataType: "number",
//                            setCellValue: function (rowData, value) {

//                                rowData.Required_Quantity = value;

//                            },
//                            //allowEditing: false,
//                            visible: false


//                        },
//                        {
//                            dataField: "Reviewed_Quantity",
//                            caption: "Review Qty",

//                            validationRules: [
//                                { type: "required" },
//                                {
//                                    type: "range",
//                                    message: "Please enter valid count > 0",
//                                    min: 0,
//                                    max: 214783647
//                                }],
//                            dataType: "number",
//                            setCellValue: function (rowData, value) {

//                                rowData.Reviewed_Quantity = value;

//                            },


//                        },
//                        //{
//                        //    dataField: "ActualAvailableQuantity",
//                        //    caption: "Available Qty",
//                        //    allowEditing: false,
//                        //    width: 102
//                        //},
//                        {
//                            dataField: "Unit_Price",
//                            caption: "Unit Price",
//                            dataType: "number",
//                            format: { type: "currency", precision: 2 },
//                            valueFormat: "#0.00",

//                            validationRules: [{ type: "required" }, {
//                                type: "range",
//                                message: "Please enter valid price > 0",
//                                min: 0.01,
//                                max: Number.MAX_VALUE
//                            }],
//                            allowEditing: false,
//                            visible: false


//                        },
//                        //   {
//                        //    dataField: "RQuantity",
//                        //    caption: "Qty",

//                        //    dataType: "number",
//                        //    setCellValue: function (rowData, value) {

//                        //        rowData.Quantity = value;

//                        //    },
//                        //    //allowEditing: false,
//                        //    visible: false


//                        //},

//                        {
//                            dataField: "Total_Price",
//                            caption: "Required Cost",

//                            calculateCellValue: function (rowData) {

//                                if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
//                                    return rowData.Unit_Price * rowData.Required_Quantity;
//                                }
//                                else
//                                    return 0.0;
//                            },

//                            dataType: "number",
//                            format: { type: "currency", precision: 2 },
//                            valueFormat: "#0.00",
//                            allowEditing: false,
//                            visible: false
//                        },
//                        {
//                            dataField: "Reviewed_Cost",

//                            calculateCellValue: function (rowData) {

//                                //if (rowData.Reviewed_Quantity > 0 && rowData.Unit_Price > 0) {
//                                //    return rowData.Unit_Price * rowData.Reviewed_Quantity;
//                                //}
//                                //else
//                                //    return 0.0

//                                if ((rowData.Reviewed_Cost == null || rowData.Reviewed_Cost == undefined) && rowData.Reviewed_Quantity > 0 && rowData.Unit_Price > 0) {
//                                    return rowData.Unit_Price * rowData.Reviewed_Quantity;
//                                }
//                                else if (rowData.Reviewed_Cost != null || rowData.Reviewed_Cost != undefined) {
//                                    return rowData.Reviewed_Cost;
//                                }
//                                else
//                                    return 0.0;;
//                            },

//                            dataType: "number",
//                            format: { type: "currency", precision: 2 },
//                            valueFormat: "#0.00",
//                            allowEditing: false
//                        },

//                        {
//                            dataField: "Requestor",
//                            allowEditing: false,
//                            visible: false
//                        },
//                        {
//                            dataField: "Reviewer_1",
//                            allowEditing: false,
//                            visible: false
//                        },
//                        {
//                            dataField: "Reviewer_2",
//                            allowEditing: false,
//                            visible: false
//                        },
//                        {
//                            dataField: "SubmitDate",
//                            allowEditing: false,
//                            visible: false
//                        },
//                        {
//                            dataField: "RequiredDate",
//                            dataType: "date",
//                            visible: true

//                        },
//                        {
//                            dataField: "RequestOrderDate",
//                            dataType: "date",
//                            allowEditing: false,
//                            visible: false

//                        },
//                        {
//                            dataField: "OrderDate",
//                            dataType: "date",
//                            allowEditing: false,
//                            visible: true

//                        },
//                        {
//                            dataField: "TentativeDeliveryDate",
//                            dataType: "date",
//                            allowEditing: false,
//                            visible: false

//                        },
//                        {
//                            dataField: "Comments",
//                            visible: false,
//                            //allowEditing: false,
//                        },
//                        {
//                            dataField: "PORemarks",
//                            width: 140,
//                            caption: "Item Justification"

//                        },
//                        //{
//                        //    dataField: "LeadTime",
//                        //    caption: "LeadTime (in days)",
//                        //    allowEditing: false,
//                        //    visible: true,
//                        //    calculateCellValue: function (rowData) {
//                        //        //update the LeadTime
//                        //        if (rowData.Item_Name == undefined || rowData.Item_Name == null) {

//                        //            leadtime1 = "";
//                        //        }

//                        //        else {

//                        //            $.ajax({

//                        //                type: "GET",
//                        //                url: "/BudgetingOrder/GetLeadTime",
//                        //                data: { 'ItemName': rowData.Item_Name },
//                        //                datatype: "json",
//                        //                async: false,
//                        //                success: success_getleadtime,

//                        //            });

//                        //            function success_getleadtime(response) {

//                        //                if (response == 0)
//                        //                    leadtime1 = "";
//                        //                else
//                        //                    leadtime1 = response;

//                        //            }

//                        //        }

//                        //        return leadtime1;
//                        //    }

//                        //},

//                        {
//                            dataField: "OrderPrice",
//                            dataType: "number",
//                            format: { type: "currency", precision: 2 },
//                            valueFormat: "#0.00",

//                            //validationRules: [{ type: "required" }, {
//                            //    type: "range",
//                            //    message: "Please enter valid price > 0",
//                            //    min: 0.01,
//                            //    max: Number.MAX_VALUE
//                            //}],
//                            allowEditing: false,
//                            //visible: false


//                        },
//                        {
//                            dataField: "OrderedQuantity",
//                            caption: "Ordered Qty",
//                            visible: false,
//                            // allowEditing: flag || !e.row.data.ApprovedSH 
//                            allowEditing: false



//                        },
//                        {
//                            dataField: "OrderStatus",

//                            setCellValue: function (rowData, value) {

//                                rowData.OrderStatus = value;


//                            },
//                            lookup: {
//                                dataSource: function (options) {

//                                    return {

//                                        store: OrderStatus_list,
//                                        filter: options.data ? ["OrderStatus", "=", "Closed"] : null


//                                    };
//                                },

//                                valueExpr: "ID",
//                                displayExpr: "OrderStatus",

//                            }
//                        },
//                        {
//                            dataField: "ActualDeliveryDate",
//                            dataType: "date",
//                            allowEditing: false,
//                            visible: false

//                        },
//                        {
//                            dataField: "Fund",

//                            setCellValue: function (rowData, value) {
//                                //debugger;
//                                rowData.Fund = value;
//                                if (value == 3)//Fund = F03 
//                                {
//                                    //debugger;
//                                    isFund_F03 = true;
//                                    isFund_F05orF06 = false;
//                                    var dxForm = $(".dx-form").dxForm('instance');
//                                    dxForm.itemOption('F03', 'visible', isFund_F03);
//                                    dxForm.itemOption('F05_and_F06', 'visible', isFund_F05orF06);
//                                }
//                                else if (value == 5 || value == 6) { //fund should be 5/6
//                                    //debugger;
//                                    isFund_F05orF06 = true;
//                                    isFund_F03 = false;
//                                    var dxForm = $(".dx-form").dxForm('instance');
//                                    dxForm.itemOption('F05_and_F06', 'visible', isFund_F05orF06);
//                                    dxForm.itemOption('F03', 'visible', isFund_F03);

//                                }
//                                else {
//                                    isFund_F05orF06 = false;
//                                    isFund_F03 = false;
//                                    var dxForm = $(".dx-form").dxForm('instance');
//                                    dxForm.itemOption('F05_and_F06', 'visible', isFund_F05orF06);
//                                    dxForm.itemOption('F03', 'visible', isFund_F03);
//                                }
//                            },

//                            lookup: {
//                                dataSource: function (options) {

//                                    return {

//                                        store: Fund_list,


//                                    };
//                                },

//                                valueExpr: "ID",
//                                displayExpr: "Fund",
//                                allowEditing: false

//                            }
//                        },
//                        {
//                            dataField: "BM_Number",
//                            allowEditing: true,
//                            visible: false

//                        },
//                        {
//                            dataField: "PIF_ID",
//                            allowEditing: true,
//                            visible: false

//                        },
//                        {
//                            dataField: "Task_ID",
//                            allowEditing: true,
//                            visible: false

//                        },
//                        {
//                            dataField: "Project_ID",
//                            allowEditing: true,
//                            visible: false

//                        },
//                        {
//                            dataField: "Purchase_Type",
//                            allowEditing: true,
//                            visible: false,
//                            lookup: {
//                                dataSource: function (options) {
//                                    return {
//                                        store: PurchaseType_list,
//                                    };
//                                },
//                                valueExpr: "ID",
//                                displayExpr: "PurchaseType"
//                            }

//                        },
//                        {
//                            dataField: "SupplierName_with_Address",
//                            allowEditing: true,
//                            visible: false

//                        },
//                        {
//                            dataField: "Resource_Group_Id",
//                            allowEditing: true,
//                            visible: false
//                        },

//                    ]
//                }],


//            onEditorPreparing: function (e) {



//                if (e.parentType === "dataRow" && e.dataField === "BU") {

//                    if (e.row.data.ApprovedSH == undefined)
//                        e.row.data.ApprovedSH = false;
//                    if (e.row.data.RequestToOrder == true)
//                        e.editorOptions.readOnly = true;
//                        // to handle when RequestToOrder = false (request not yet triggered for ordering) - BU should be editable for f01,f03 req - initial req
//                    else
//                        e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH); //|| !addnewitem_flag;/*isValuepresent(e.value);*/
//                }
//                if (e.parentType === "dataRow" && e.dataField === "OEM") {
//                    if (e.row.data.ApprovedSH == undefined)
//                        e.row.data.ApprovedSH = false;
//                    if (e.row.data.RequestToOrder == true)
//                        e.editorOptions.readOnly = true;
//                    else
//                        e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
//                }
//                if (e.parentType === "dataRow" && e.dataField === "DEPT") {
//                    if (e.row.data.ApprovedSH == undefined)
//                        e.row.data.ApprovedSH = false;
//                    if (e.row.data.RequestToOrder == true)
//                        e.editorOptions.readOnly = true;
//                    else
//                        e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
//                }
//                if (e.parentType === "dataRow" && e.dataField === "Group") {
//                    if (e.row.data.ApprovedSH == undefined)
//                        e.row.data.ApprovedSH = false;
//                    if (e.row.data.RequestToOrder == true)
//                        e.editorOptions.readOnly = true;
//                    else
//                        e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
//                }
//                if (e.parentType === "dataRow" && e.dataField === "Item_Name") {
//                    if (e.row.data.ApprovedSH == undefined)
//                        e.row.data.ApprovedSH = false;
//                    if (e.row.data.RequestToOrder == true)
//                        e.editorOptions.readOnly = true;
//                    else
//                        e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
//                }
//                if (e.parentType === "dataRow" && e.dataField === "Reviewed_Quantity") {
//                    if (e.row.data.ApprovedSH == undefined)
//                        e.row.data.ApprovedSH = false;
//                    if (e.row.data.RequestToOrder == true)
//                        e.editorOptions.readOnly = true;
//                    else
//                        e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
//                }
//                if (e.parentType === "dataRow" && e.dataField === "Reviewed_Cost") {
//                    if (e.row.data.ApprovedSH == undefined)
//                        e.row.data.ApprovedSH = false;
//                    if (e.row.data.RequestToOrder == true)
//                        e.editorOptions.readOnly = true;
//                    else
//                        e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
//                }
//                if (e.parentType === "dataRow" && e.dataField === "Fund") {
//                    if (e.row.data.ApprovedSH == undefined)
//                        e.row.data.ApprovedSH = false;
//                    if (e.row.data.RequestToOrder == true)
//                        e.editorOptions.readOnly = true;
//                    else
//                        e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH && !popupedit);
//                }
//                if (e.parentType === "dataRow" && e.dataField === "OrderStatus") {

//                    e.editorOptions.readOnly = !isDelivered(e.row.data.OrderStatus);
//                }
//                if (e.parentType === "dataRow" && e.dataField === "RequiredDate") {
//                    //debugger;
//                    e.editorOptions.readOnly = e.row.data.RequestToOrder && (!isDelivered(e.row.data.OrderStatus) || isF03F01(e.row.data.Fund));  //1 & (0  || fo3)
//                }
//                if (e.parentType === "dataRow" && e.dataField === "Required_Quantity") {
//                    if (e.row.data.ApprovedSH == undefined)
//                        e.row.data.ApprovedSH = false;
//                    if (e.row.data.RequestToOrder == true)
//                        e.editorOptions.readOnly = true;
//                    else
//                        e.editorOptions.readOnly = (/*!isF03F01(e.row.data.Fund) &&*/ e.row.data.ApprovedSH);
//                }
//                if (e.parentType === "dataRow" && e.dataField === "BudgetCenterID") {
//                    //debugger;
//                    if (e.row.data.BudgetCenterID.constructor.name == "Number")
//                        e.editorOptions.readOnly = true;
//                }

//                var component = e.component,
//                    rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

//                if (e.parentType === "dataRow" && e.dataField === "Group") {

//                    e.editorOptions.disabled = (typeof e.row.data.DEPT !== "number");
//                    if (e.editorOptions.disabled)
//                        e.editorOptions.placeholder = 'Select Dept first';
//                    if (!e.editorOptions.disabled)
//                        e.editorOptions.placeholder = 'Select Group';

//                }


//                if (e.dataField === "DEPT" /*|| e.dataField === "UnloadingPoint"*/) {

//                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
//                    e.editorOptions.onValueChanged = function (e) {
//                        onValueChanged.call(this, e);
//                        /*var upid = component.cellValue(rowIndex, "UnloadingPoint");*/
//                        //var deptid = component.cellValue(rowIndex, "DEPT");
//                        /*if (e.value != undefined && e.value != null && e.value != "") {*/
//                        //debugger;
//                        //$.ajax({

//                        //    type: "post",
//                        //    url: "/BudgetingOrder/GetCostCenter",
//                        //    data: { 'deptid': deptid, 'upid': upid },
//                        //    datatype: "json",
//                        //    traditional: true,
//                        //    success: function (data) {
//                        //        //debugger;
//                        //        CostCenter = data.data;
//                        //        window.setTimeout(function () {
//                        //            //debugger;
//                        //            component.cellValue(rowIndex, "CostCenter", CostCenter);

//                        //        }, 1000);

//                        //    }

//                        //})

//                        $.ajax({

//                            type: "post",
//                            url: "/BudgetingOrder/GetRFOBudgetCenter",
//                            data: { 'deptid': e.value },
//                            datatype: "json",
//                            traditional: true,
//                            success: function (data) {
//                                //debugger;
//                                BudCenter = data.data;
//                                if (BudCenter.length == 0) {
//                                    //debugger;
//                                    //$.notify("Please Contact ELO Team to find BudgetCenter details !", {
//                                    //    elementPosition: "right",
//                                    //    className: "error",
//                                    //    autoHideDelay: 13000,
//                                    //});
//                                    alert("Please Contact ELO Team to find BudgetCenter details !");
//                                    //debugger;
//                                }
//                                else {
//                                    window.setTimeout(function () {
//                                        //debugger;
//                                        component.cellValue(rowIndex, "BudgetCenterID", BudCenter);


//                                    }, 1000);
//                                }

//                                }
//                        })
//                        $.ajax({

//                            type: "post",
//                            url: "/BudgetingRequest/GetReviewer",
//                            data: { DEPT: e.value, BU: component.cellValue(rowIndex, "BU") },
//                            datatype: "json",
//                            traditional: true,
//                            success: function (data) {

//                                if (data.success)
//                                    reviewer_2 = data.data;
//                                else {
//                                    $.notify("Unable to find " + data.data + " 's VKM SPOC Details. Kindly contact SmartLab Team for assistance", {
//                                        globalPosition: "top center",
//                                        className: "warn"
//                                    })
//                                    reviewer_2 = "NA";
//                                }
//                                //reviewer_2 = data;
//                                if (e.value == 1 && is_XCselected == true) {
//                                    is_TwoWPselected = true;
//                                    BU_forItemFilter = 4;
//                                    //reviewer_2 = "Sheeba Rani R";
//                                }

//                            }
//                        })
//                        window.setTimeout(function () {
//                            ////debugger;
//                          /*  component.cellValue(rowIndex, "Reviewer_1", reviewer_1);*/
//                            component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
//                        }, 1000);


//                        //    // Emulating a web service call
//                        //window.setTimeout(function () {
//                        //    //debugger;
//                        //        component.cellValue(rowIndex, "BudgetCenterID", BudCenter);
//                        //        //component.cellValue(rowIndex, "CostCenter", CostCenter);

//                        //}, 1000);
//                        }
//                        //else {
//                        //    window.setTimeout(function () {
//                        //        component.cellValue(rowIndex, "BudgetCenter", "");

//                        //    }, 1000);
//                        //}

//                    }


//                if (e.dataField === "RFOApprover") {
//                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
//                    e.editorName = "dxAutocomplete";
//                    //e.editorName.cellInfo.value = e.value;
//                    //e.editorOptions.placeholder = 'Select Emp Name';
//                    e.editorOptions.onValueChanged = function (e) {
//                        ////debugger;
//                        onValueChanged.call(this, e);
//                        //debugger;
//                    }
//                }


//                //if (e.dataField === "BU") {

//                //    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
//                //    e.editorOptions.onValueChanged = function (e) {
//                //        onValueChanged.call(this, e);


//                //        $.ajax({

//                //            type: "post",
//                //            url: "/BudgetingOrder/GetReviewer",
//                //            data: { BU: e.value },
//                //            datatype: "json",
//                //            traditional: true,
//                //            success: function (data) {

//                //                reviewer_2 = data;

//                //            }
//                //        })
//                //        // Emulating a web service call
//                //        window.setTimeout(function () {
//                //            component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
//                //            //component.cellValue(rowIndex, "Item_Name", Item_list);
//                //        }, 1000);
//                //    }
//                //}

//                if (e.dataField === "GoodsRecID") {
//                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
//                    e.editorOptions.onValueChanged = function (e) {
//                        onValueChanged.call(this, e);
//                        var FullName, Department, Group, Ntid;

//                        //debugger;

//                        $.ajax({

//                            type: "post",
//                            url: "/BudgetingOrder/ValidateGoodsRecID", //GetRequestorDetails_Planning_EMProxy
//                            data: { NTID: e.value },
//                            datatype: "json",
//                            traditional: true,
//                            success: function (data) {

//                                //debugger;
//                                if (data.success) {
//                                    Ntid = data.data.NTID;
//                                    window.setTimeout(function () {
//                                        component.cellValue(rowIndex, "GoodsRecID", Ntid);
//                                    }, 1000);

//                                }
//                                else {
//                                    //$.notify(data.message, {
//                                    //    globalPosition: "top center",
//                                    //    className: "error"
//                                    //})
//                                    DevExpress.ui.notify({
//                                        message: "Goods Reciepient ID is not valid",
//                                        className: "error"
//                                    });
//                                }

//                                //debugger;


//                            }
//                        })
//                        // Emulating a web service call

//                        //debugger;

//                    }
//                }
//                //if (e.dataField === "OrderType") {



//                //    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
//                //    e.editorOptions.onValueChanged = function (e) {
//                //        onValueChanged.call(this, e);

//                //        //alert("changed ordertype")
//                //        //var buselected = component.cellValue(rowIndex, "BU");
//                //        var buselected = 6;
//                //        var ordert = component.cellValue(rowIndex, "OrderType");
//                //        if (buselected != undefined && buselected != null && buselected != "" && ordert != undefined && ordert != null && ordert != "") {
//                //            //debugger;

//                //            //$.ajax({

//                //            //    type: "post",
//                //            //    url: "/BudgetingOrder/GetUOM",
//                //            //    data: { 'ordert': ordert },
//                //            //    datatype: "json",
//                //            //    traditional: true,
//                //            //    success: function (data) {
//                //            //        //debugger;
//                //            //        UOMlist = data.data;

//                //            //    }
//                //            //})

//                //            //$.ajax({
//                //            //    //type: "post",
//                //            //    url: "/BudgetingOrder/GetItemName",
//                //            //    data: { 'buselected': buselected, 'ordert': ordert },
//                //            //    datatype: "json",
//                //            //    async: false,
//                //            //    traditional: true,
//                //            //    success: function (data) {
//                //            //        //debugger;
//                //            //        Item_list = data.data;

//                //            //    }
//                //            //})


//                //            //window.setTimeout(function () {
//                //            //    //debugger;
//                //            //    component.cellValue(rowIndex, "Item_Name", Item_list);
//                //            //    //component.cellValue(rowIndex, "UnitofMeasure", UOMlist);

//                //            //},
//                //            //    1000);
//                //        }
//                //        else {

//                //            //debugger;
//                //            component.cellValue(rowIndex, "Item_Name", "");
//                //            //component.cellValue(rowIndex, "UnitofMeasure", "");

//                //        }
//                //    }
//                //}


//                //if (e.dataField === "Item_Name") {

//                //    //debugger;

//                //    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
//                //    e.editorOptions.onValueChanged = function (e) {
//                //        onValueChanged.call(this, e);

//                //        $.ajax({
//                //            type: "post",
//                //            url: "/BudgetingOrder/GetUnitPrice",
//                //            data: { ItemName: e.value },
//                //            datatype: "json",
//                //            traditional: true,
//                //            success: function (data) {

//                //                if (data > 0)
//                //                    unitprice = data;

//                //            }
//                //        })

//                //        $.ajax({

//                //            type: "post",
//                //            url: "/BudgetingOrder/GetCategory",
//                //            data: { ItemName: e.value },
//                //            datatype: "json",
//                //            traditional: true,
//                //            success: function (data) {
//                //                category = data;

//                //            }
//                //        })

//                //        $.ajax({

//                //            type: "post",
//                //            url: "/BudgetingOrder/GetCostElement",
//                //            data: { ItemName: e.value },
//                //            datatype: "json",
//                //            traditional: true,
//                //            success: function (data) {
//                //                costelement = data;

//                //            }
//                //        })

//                //        //var BudgetCodedesc;
//                //        //debugger;
//                //        $.ajax({

//                //            type: "post",
//                //            url: "/BudgetingOrder/GetBudgetCode",
//                //            data: { ItemName: e.value },
//                //            datatype: "json",
//                //            traditional: true,
//                //            success: function (data) {
//                //                //debugger;
//                //                BudgetCode = data.data;
//                //                BudgetCodedesc = data.BudgetCodedesc;

//                //            }
//                //        })



//                //        window.setTimeout(function () {

//                //            //debugger;
//                //            component.cellValue(rowIndex, "Unit_Price", unitprice);
//                //            component.cellValue(rowIndex, "Category", category);
//                //            component.cellValue(rowIndex, "Cost_Element", costelement);
//                //            component.cellValue(rowIndex, "BudgetCode", BudgetCode);
//                //            component.cellValue(rowIndex, "BudgetCodeDescription", BudgetCodedesc);

//                //        },
//                //            1000);


//                //    }

//                //}

//            },
//            onRowUpdated: function (e) {
//                $.notify(" Your Item Request is being Updated...Please wait!", {
//                    globalPosition: "top center",
//                    className: "success"
//                })
//                Selected = [];
//                //var LeadTime_tocalc_ExpReqdDt;
//                ////debugger;
//                // e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
//                // e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
//                if (e.data.RequiredDate && !e.data.RequestToOrder) {

//                    //    $.ajax({

//                    //        type: "GET",
//                    //        url: "/BudgetingOrder/GetLeadTime",
//                    //        data: { 'ItemName': e.data.Item_Name },
//                    //        datatype: "json",
//                    //        async: false,
//                    //        success: success_getleadtime,

//                    //    });

//                    //    function success_getleadtime(response) {
//                    //        //debugger;
//                    //        if (response == 0) {
//                    //            LeadTime_tocalc_ExpReqdDt = "";
//                    //            Selected.push(e.data);
//                    //            //debugger;
//                    //            Update(Selected, filtered_yr);
//                    //        }
//                    //        else
//                    //        {
//                    //            LeadTime_tocalc_ExpReqdDt = response;     
//                    var ReqdDate = e.data.RequiredDate;
//                    //            //debugger;
//                    $.ajax({

//                        type: "GET",
//                        url: "/BudgetingOrder/ValidateRequiredDate",
//                        data: { /*'LeadTime': LeadTime_tocalc_ExpReqdDt,*/ 'RequiredDate': ReqdDate },
//                        datatype: "json",
//                        async: false,
//                        success: success_validateReqdDate,

//                    });
//                    function success_validateReqdDate(info) {
//                        ////debugger;
//                        if (info) {
//                            $.notify(info, {
//                                globalPosition: "top center",
//                                className: "error"
//                            })
//                        }
//                        else {
//                            Selected.push(e.data);
//                            ////debugger;
//                            Update(Selected, filtered_yr);
//                        }
//                    }

//                    //        }


//                    //    }


//                }
//                else {
//                    Selected.push(e.data);
//                    ////debugger;
//                    Update(Selected, filtered_yr);

//                }

//            },

//            onRowInserting: function (e) {
//                addnewitem_flag = false;
//                e.data.BU = 6;
//                //e.data.RFOReqNTID = RFOReqNTID;
//                e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
//                // e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
//                Selected = [];
//                Selected.push(e.data);

//                //debugger;

//                Update(Selected, filtered_yr);
//            },
//            onRowRemoving: function (e) {

//                Delete(e.data.RequestID, filtered_yr);

//            },
//            //masterDetail: {
//            //    enabled: true,

//            //    template(container, options) {
//            //        //debugger;
//            //        if (options.data.OrderDate != "" && options.data.OrderDate != null && options.data.OrderDate != undefined) {
//            //            const currentRequestData = options.data;

//            //            $('<div>')
//            //                .addClass('master-detail-caption')
//            //                .text(`${currentRequestData.Item_Name} Purchase details:`)
//            //                .appendTo(container);

//            //            $('<div>')
//            //                .dxDataGrid({
//            //                    columnAutoWidth: true,
//            //                    showBorders: true,
//            //                    headerFilter: {
//            //                        visible: true,
//            //                        applyFilter: "auto"
//            //                    },
//            //                    searchPanel: {
//            //                        visible: true,
//            //                        width: 240,
//            //                        placeholder: "Search..."
//            //                    },
//            //                    columns: [

//            //                        {

//            //                            alignment: "center",
//            //                            columns: [

//            //                                {
//            //                                    dataField: "RequestID",
//            //                                    allowEditing: false,
//            //                                    visible: false
//            //                                },

//            //                                //{
//            //                                //    dataField: "Unit_Price",
//            //                                //    caption: "Unit Price",
//            //                                //    dataType: "number",
//            //                                //    format: { type: "currency", precision: 0 },
//            //                                //    valueFormat: "#0",
//            //                                //    allowEditing: false,
//            //                                //    validationRules: [{ type: "required" }, {
//            //                                //        type: "range",
//            //                                //        message: "Please enter valid price > 0",
//            //                                //        min: 0.01,
//            //                                //        max: Number.MAX_VALUE
//            //                                //    }],
//            //                                //    allowEditing: false,
//            //                                //    visible: false


//            //                                //},
//            //                                //{
//            //                                //    dataField: "Total_Price",
//            //                                //    width: 100,
//            //                                //    calculateCellValue: function (rowData) {

//            //                                //        if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
//            //                                //            return rowData.Unit_Price * rowData.Required_Quantity;
//            //                                //        }
//            //                                //        else
//            //                                //            return 0.0;
//            //                                //    },

//            //                                //    dataType: "number",
//            //                                //    format: { type: "currency", precision: 0 },
//            //                                //    valueFormat: "#0",
//            //                                //    allowEditing: false
//            //                                //},
//            //                                , {
//            //                                    dataField: "ItemDescription",
//            //                                    width: 250
//            //                                },
//            //                                {
//            //                                    dataField: "PONumber",
//            //                                    caption: "PO",
//            //                                    allowEditing: false,
//            //                                },



//            //                                , {
//            //                                    dataField: "BudgetCode",
//            //                                    allowEditing: false,
//            //                                    visible: false
//            //                                },


//            //                                {
//            //                                    dataField: "POQuantity",
//            //                                    allowEditing: false,
//            //                                    caption: "PO Qty"
//            //                                },

//            //                                {
//            //                                    dataField: "Netvalue_USD",
//            //                                    allowEditing: false,
//            //                                    visible: false
//            //                                },


//            //                                {
//            //                                    dataField: "POCreatedOn",
//            //                                    caption: "Order Dt",
//            //                                    dataType: "date",
//            //                                    allowEditing: true,
//            //                                },



//            //                                {
//            //                                    dataField: "TentativeDeliveryDate",
//            //                                    allowEditing: true,
//            //                                    caption: "Tentative",

//            //                                    dataType: "date",
//            //                                },


//            //                                {
//            //                                    dataField: "ActualDeliveryDate",
//            //                                    allowEditing: true,
//            //                                    caption: "Actual Dt",
//            //                                    dataType: "date",
//            //                                },

//            //                                , {
//            //                                    dataField: "Currentstatus",
//            //                                    caption: "Status",
//            //                                    setCellValue: function (rowData, value) {
//            //                                        //debugger;
//            //                                        rowData.Fund = value;

//            //                                    },
//            //                                    lookup: {
//            //                                        dataSource: function (options) {
//            //                                            //debugger;
//            //                                            return {

//            //                                                store: OrderStatus_list,
//            //                                            };

//            //                                        },
//            //                                        valueExpr: "ID",
//            //                                        displayExpr: "OrderStatus"
//            //                                    },
//            //                                }

//            //                            ]
//            //                        }],
//            //                    //dataSource: function (options1) {
//            //                    //    //debugger;
//            //                    //    return {

//            //                    //        store: SubItemList,
//            //                    //        filter: options1.data ? ['RequestID', '=', options.key] : null
//            //                    //    };
//            //                    //},
//            //                    //dataSource: new DevExpress.data.DataSource({
//            //                    //    store: SubItemList,
//            //                    //    filter: ['RequestID', '=', options.key],
//            //                    //}),
//            //                    dataSource: new DevExpress.data.DataSource({
//            //                        store: new DevExpress.data.ArrayStore({
//            //                            key: 'ID',
//            //                            data: SubItemList,
//            //                        }),
//            //                        filter: ['RequestID', '=', options.key],
//            //                    }),
//            //                }).appendTo(container);
//            //        }

//            //    },
//            //}
//        });


//        // }
//        //else {
//        //    //debugger;
//        //    $.notify(response.message, {
//        //        globalPosition: "top center",
//        //        className: "error"
//        //    })

//        //    //Hide the Loading indicator once the Request List is fetched
//        //    genSpinner_load.classList.remove('fa');
//        //    genSpinner_load.classList.remove('fa-spinner');
//        //    genSpinner_load.classList.remove('fa-pulse');
//        //    document.getElementById("loadpanel").style.display = "none";
//        //    $("#RequestTable_RFO").prop('hidden', false);
//        //}




//    }

//    function OnError_GetData(response) {
//        $("#RequestTable_RFO").prop('hidden', false);
//        $.notify(data.message, {
//            globalPosition: "top center",
//            className: "warn"
//        })
//    }

//}




//$('#btnSubmitAll').click(function () {
//    LabAdminApprove(1999999999, filtered_yr);
//});

////$.ajax({

////    type: "post",
////    //url: "/BudgetingOrder/GetOrderType",
////    //data: { ItemName: e.value },
////    //datatype: "json",
////    //traditional: true,
////    url: encodeURI("../BudgetingOrder/GetOrderType"),
////    async: false,
////    success: function (data) {
////        //debugger;
////        OrderTypeList = data;
////        RFOReqNTID = data.presentUserNTID;

////    }
////})




////$.ajax({

////    type: "get",
////    //url: "/BudgetingOrder/GetOrderType",
////    //data: { ItemName: e.value },
////    //datatype: "json",
////    //traditional: true,
////    url: encodeURI("../BudgetingOrder/RFOApprover"),
////    async: false,
////    success: function (data) {
////        //debugger;
////        rfoapproverlist = data;

////    }
////})
////$.ajax({

////    type: "post",
////    url: encodeURI("../BudgetingOrder/GetUnloadingPoint"),
////    async: false,
////    success: function (data) {
////        //debugger;
////        UnloadingPointList = data;
////    }
////})

////$.ajax({

////    type: "post",
////    url: encodeURI("../BudgetingOrder/GetRFOBudgetCenter"),
////    data: { 'deptid': e.value },
////    async: false,
////    success: function (data) {
////        //debugger;
////        BudgetCenter = data;
////    }
////})

//function LabAdminApprove(id, filtered_yr) {
//    ////debugger;
//    if (id == undefined) {
//        $.notify('Please check the Fund and Try again later!', {
//            globalPosition: "top center",
//            autoHideDelay: 20000,
//            className: "error"
//        });
//    }
//    else {

//        ////debugger;
//        if (confirm('Do you confirm to place Request to Order the item(s)?')) {

//            var genSpinner = document.querySelector("#SubmitSpinner");
//            if (id == 1999999999) {
//                genSpinner.classList.add('fa');
//                genSpinner.classList.add('fa-spinner');
//                genSpinner.classList.add('fa-pulse');
//            }

//            $.ajax({
//                type: "POST",
//                url: encodeURI("../BudgetingOrder/LabAdminApprove"),
//                data: { 'id': id, 'useryear': filtered_yr },
//                success: function (data) {

//                    if (id == 1999999999) {

//                        genSpinner.classList.remove('fa');
//                        genSpinner.classList.remove('fa-spinner');
//                        genSpinner.classList.remove('fa-pulse');
//                    }




//                    $.ajax({
//                        type: "POST",
//                        url: "/BudgetingOrder/GetData",
//                        data: { 'year': filtered_yr },
//                        datatype: "json",
//                        async: true,
//                        success: success_refresh_getdata,
//                        error: error_refresh_getdata

//                    });
//                    function success_refresh_getdata(response) {

//                        var getdata = response.data;
//                        $("#RequestTable_RFO").dxDataGrid({
//                            dataSource: getdata
//                        });
//                    }
//                    function error_refresh_getdata(response) {

//                        $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
//                            globalPosition: "top center",
//                            className: "warn"
//                        });

//                    }

//                    ////debugger;
//                    if (data.is_MailTrigger) {
//                        ////debugger;

//                        $.ajax({
//                            type: "POST",
//                            url: encodeURI("../Budgeting/SendEmail_Order"),
//                            data: { 'emailnotify': data.data },
//                            success: success_email,
//                            error: error_email
//                        });

//                        function success_email(response) {
//                            $.notify("Mail has been sent to the LabTeam about your Request to Order!", {
//                                globalPosition: "top center",
//                                className: "success"
//                            })

//                        }
//                        function error_email(response) {
//                            $.notify("Unable to send mail to the LabTeam about your Request to Order!!", {
//                                globalPosition: "top center",
//                                className: "warn"
//                            })

//                        }
//                    }



//                    if (data.success) {

//                        $.notify(data.message, {
//                            globalPosition: "top center",
//                            className: "success"
//                        })
//                    }
//                    else {
//                        $.notify(data.message, {
//                            globalPosition: "top center",
//                            className: "error"
//                        })
//                    }



//                }
//            });



//        }
//    }
//}

//function Update(id1, filtered_yr) {
//    //debugger;

//    //if (!id1[0].ApprovedSH && id1[0].Fund == 2) {
//    //    ////debugger;
//    //    $.notify('Cannot add F02 items right now since Request Window has been closed.' + '\n' + ' Only F01/F03 items can be added at this stage!', {
//    //        globalPosition: "top center",
//    //        autoHideDelay: 20000,
//    //        className: "error"
//    //    });
//    //}
//    //else {
//        $.ajax({
//            type: "POST",
//            url: encodeURI("../BudgetingOrder/AddOrEdit"),
//            data: { 'req': id1[0], 'useryear': filtered_yr, 'popupedit': popupedit }, //if popupedit is true => it is bgsw ordering wherein Reqd Dt in not filled - then auto fill with Current+10 weeks; else no need to auto-fill for CC
//            success: function (data) {
//                //debugger;
//                if (data.success) {
//                    $.notify("Request saved successfully !", {
//                        globalPosition: "top center",
//                        className: "success"
//                    })
//                }
//                else {
//                    $.notify("Please try again", {
//                        globalPosition: "top center",
//                        className: "error"
//                    })
//                }
//                if (data.success) {

//                    //debugger;
//                    if (file != undefined) { //in CC edit, file will not be uploaded
//                        var formdata = new FormData();
//                        for (var i = 0; i < file.length; i++) {
//                            formdata.append(file[i].name, file[i]);
//                        }
//                        formdata.append("id", data.RequestID);
//                        //formdata.append("image", ofile);
//                        //formdata.append("id", editRowID == undefined ? 0 : editRowID);
//                        //var x = new FormData(file[0]);
//                        //debugger;
//                        $.ajax({
//                            type: "POST",
//                            //dataType: "json",
//                            //contentType: "multipart/form-data", //false,//"application/json; charset=utf-8;",
//                            url: encodeURI("../BudgetingOrder/AsyncFileUpload"),
//                            //data: //JSON.stringify({ formdata: ofile.name, id: "2" }),
//                            //{ 'formdata': file, 'id': "2" },
//                            data: formdata, //{ 'formdata': formdata, id: "2" },
//                            cache: false,
//                            contentType: false,
//                            processData: false,



//                            //if success, data gets refreshed internally
//                            success: function (data) {
//                                //debugger;
//                                //InvAuth = false;
//                                $.notify("File has been uploaded successfully!!", {
//                                    globalPosition: "top center",
//                                    className: "success"
//                                })



//                            },



//                            error: function (data) {
//                                //InvAuth = false;
//                                $.notify("Error in uploading file. Please try later!!", {
//                                    globalPosition: "top center",
//                                    className: "error"
//                                })

//                                //debugger;
//                            }

//                        });
//                    }

//                }

//                $.ajax({
//                    type: "POST",
//                    url: "/BudgetingOrder/GetData",
//                    data: { 'year': filtered_yr },
//                    datatype: "json",
//                    async: true,
//                    success: success_refresh_getdata,
//                    error: error_refresh_getdata

//                });

//                function success_refresh_getdata(response) {

//                    var getdata = response.data;
//                    $("#RequestTable_RFO").dxDataGrid({
//                        dataSource: getdata
//                    });
//                }
//                function error_refresh_getdata(response) {
//                    ////debugger;
//                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
//                        globalPosition: "top center",
//                        className: "warn"
//                    });

//                }

//            }

//        });


//    //}


//}

//function Delete(id, filtered_yr) {

//    $.ajax({
//        type: "POST",
//        url: "/BudgetingOrder/Delete",
//        data: { 'id': id, 'useryear': filtered_yr },
//        success: function (data) {
//            newobjdata = data.data;
//            $("#RequestTable_RFO").dxDataGrid({ dataSource: newobjdata });
//        }



//    });

//    $.notify(data.message, {
//        globalPosition: "top center",
//        className: "success"
//    })

//}




////$(function () {
////    // run the currently selected effect
////    function runEffect() {
////        // get effect type from
////        var selectedEffect = "blind";

////        var options = {};

////        // Run the effect
////        $("#effect").show(selectedEffect, options, 1000, callback);
////    };

////    function callback() {
////        setTimeout(function () {
////            $("#effect:visible").removeAttr("style").fadeOut();
////        }, 60000);
////    };

////    // Set effect from select menu value
////    $("#btn_summary").on("click", function () {
////        runEffect();
////    });

////    $("#effect").hide();
////});







////$("#buttonClearFilters").dxButton({
////    text: 'Clear Filters',
////    onClick: function () {
////        $("#RequestTable_RFO").dxDataGrid("clearFilter");
////    }
////});

//$('[data-toggle="tooltip"]').tooltip();

////BULookup,OEMLookup,DeptLookup,GroupLookup,ItemNameLookup,CostElementLookup,CategoryLookup




////Export data
//$("#export").click(function () {
//    //debugger;
//    $.ajax({

//        type: "POST",
//        url: "/BudgetingOrder/ExportDataToExcel/",
//        data: { 'useryear': filtered_yr },


//        success: function (export_result) {
//            //debugger;

//            var bytes = new Uint8Array(export_result.FileContents);
//            var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
//            var link = document.createElement('a');
//            link.href = window.URL.createObjectURL(blob);
//            link.download = export_result.FileDownloadName;
//            link.click();

//        },
//        error: function () {
//            alert("export error");
//        }

//    });
//});


////$('#chkRequest').on('click', function () {
////    var chkRequest;
////    if (this.checked)
////        chkRequest = true;
////    else
////        chkRequest = false;
////    var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
////    dataGridLEP1.beginUpdate();
////    dataGridLEP1.columnOption('OEM', 'visible', chkRequest);
////    dataGridLEP1.columnOption('Required_Quantity', 'visible', chkRequest);
////    dataGridLEP1.columnOption('Total_Price', 'visible', chkRequest);
////    dataGridLEP1.columnOption('Requestor', 'visible', chkRequest);
////    dataGridLEP1.columnOption('Reviewer_1', 'visible', chkRequest);
////    dataGridLEP1.columnOption('Reviewer_2', 'visible', chkRequest);
////    dataGridLEP1.columnOption('SubmitDate', 'visible', chkRequest);
////    dataGridLEP1.columnOption('Comments', 'visible', chkRequest);
////    dataGridLEP1.columnOption('Project', 'visible', chkRequest);
////    dataGridLEP1.endUpdate();
////    // $('#RequestTable_RFO').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
////    //// $('#RequestTable').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
////    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
////    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
////    // $('#RequestTable').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


////});

////$('#chkRFO').on('click', function () {
////    var chkRFO;
////    if (this.checked)
////        chkRFO = true;
////    else
////        chkRFO = false;
////    var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
////    dataGridLEP1.beginUpdate();
////    dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', chkRFO);
////    //dataGridLEP1.columnOption('Fund', 'visible', chkRFO);
////    dataGridLEP1.columnOption('LeadTime', 'visible', chkRFO);
////    //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRFO);
////    dataGridLEP1.columnOption('RequiredDate', 'visible', chkRFO);
////    //dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
////    dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
////    dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', chkRFO);
////    dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
////    dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
////    dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);
////    dataGridLEP1.endUpdate();
////    // $('#RequestTable').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO); -- tking more time
////    //// $('#RequestTable').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
////    //// $('#RequestTable').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
////    //// $('#RequestTable').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderID', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderPrice', 'visible', chkRFO);
////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderedQuantity', 'visible', chkRFO);



////});

////$('#chkItem').on('click', function () {
////    var chkItem;
////    if (this.checked)
////        chkItem = true;
////    else
////        chkItem = false;
////    var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
////    dataGridLEP1.beginUpdate();
////    dataGridLEP1.columnOption('Category', 'visible', chkItem);
////    dataGridLEP1.columnOption('Cost_Element', 'visible', chkItem);
////    dataGridLEP1.columnOption('Unit_Price', 'visible', chkItem);
////    dataGridLEP1.endUpdate();
////    //$('#RequestTable').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
////    //$('#RequestTable').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
////    //$('#RequestTable').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


////});







//var chkRequest;
//var chkItem;
//var chkRFO;
//var chkNonVKM;
//$('#chkRequest').on('click', function () {

//    if (this.checked)
//        chkRequest = true;
//    else
//        chkRequest = false;
//    checkboxdata();
//    // $('#RequestTable').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
//    //// $('#RequestTable').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
//    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
//    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
//    // $('#RequestTable').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


//});

//$('#chkItem').on('click', function () {

//    if (this.checked)
//        chkItem = true;
//    else
//        chkItem = false;
//    checkboxdata();
//    //$('#RequestTable').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
//    //$('#RequestTable').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
//    //$('#RequestTable').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


//});


//$('#chkRFO').on('click', function () {

//    if (this.checked)
//        chkRFO = true;
//    else
//        chkRFO = false;
//    checkboxdata();
//    // $('#RequestTable').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO); -- tking more time
//    //// $('#RequestTable').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
//    //// $('#RequestTable').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
//    //// $('#RequestTable').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'OrderID', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'OrderPrice', 'visible', chkRFO);
//    // $('#RequestTable').dxDataGrid('columnOption', 'OrderedQuantity', 'visible', chkRFO);



//});

//$('#chkNonVKM').on('click', function () {

//    if (this.checked)
//        chkNonVKM = true;
//    else
//        chkNonVKM = false;

//    checkboxdata();
//    //$('#RequestTable').dxDataGrid('columnOption', 'Customer_Name', 'visible', NonVKM);
//    //$('#RequestTable').dxDataGrid('columnOption', 'Customer_Dept', 'visible', NonVKM);
//    //$('#RequestTable').dxDataGrid('columnOption', 'BM_Number', 'visible', NonVKM);
//    //$('#RequestTable').dxDataGrid('columnOption', 'Task_ID', 'visible', NonVKM);
//    //$('#RequestTable').dxDataGrid('columnOption', 'Resource_Group_Id', 'visible', NonVKM);
//    //$('#RequestTable').dxDataGrid('columnOption', 'PIF_ID', 'visible', NonVKM);




//});

//function checkboxdata() {
//    if ($('.chkvkm:checked').length == 0) {
//        //debugger;
//        var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
//        dataGridLEP1.beginUpdate();
//        dataGridLEP1.columnOption('OEM', 'visible', true);
//        dataGridLEP1.columnOption('Requestor', 'visible', false);
//        dataGridLEP1.columnOption('Reviewer_1', 'visible', false);
//        dataGridLEP1.columnOption('Reviewer_2', 'visible', false);
//        dataGridLEP1.columnOption('Project', 'visible', false);
//        dataGridLEP1.columnOption('RequestDate', 'visible', false);
//        dataGridLEP1.columnOption('Category', 'visible', false);
//        dataGridLEP1.columnOption('Cost_Element', 'visible', false);
//        dataGridLEP1.columnOption('BudgetCode', 'visible', false);
//        dataGridLEP1.columnOption('Unit_Price', 'visible', false);
//        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
//        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
//        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);


//        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', false);
//        dataGridLEP1.columnOption('LeadTime', 'visible', false);
//        //dataGridLEP1.columnOption('OrderStatus', 'visible', true);
//        dataGridLEP1.columnOption('RequiredDate', 'visible', true);
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

//        dataGridLEP1.columnOption('BU', 'visible', true);
//        dataGridLEP1.columnOption('DEPT', 'visible', true);
//        dataGridLEP1.columnOption('Group', 'visible', true);
//        dataGridLEP1.columnOption('Item_Name', 'visible', true);
//        dataGridLEP1.columnOption('LeadTime', 'visible', true);

//        /*BGSW RFO FIELDS*/
//        dataGridLEP1.columnOption('OrderType', 'visible', false);
//        dataGridLEP1.columnOption('CostCenter', 'visible', false);
//        dataGridLEP1.columnOption('BudgetCenterID', 'visible', false);
//        dataGridLEP1.columnOption('UnitofMeasure', 'visible', false);
//        dataGridLEP1.columnOption('UnloadingPoint', 'visible', false);
//        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', false);
//        dataGridLEP1.columnOption('LabName', 'visible', false);
//        dataGridLEP1.columnOption('RFOReqNTID', 'visible', false);
//        dataGridLEP1.columnOption('RFOApprover', 'visible', false);
//        dataGridLEP1.columnOption('QuoteAvailable', 'visible', false);
//        dataGridLEP1.columnOption('GoodsRecID', 'visible', false);

//        dataGridLEP1.columnOption('Material_Part_Number', 'visible', false);
//        dataGridLEP1.columnOption('SupplierName_with_Address', 'visible', false);
//        dataGridLEP1.columnOption('Purchase_Type', 'visible', false);
//        dataGridLEP1.columnOption('Project_ID', 'visible', false);

//        dataGridLEP1.endUpdate();

//    }
//    else if (('.chkvkm:checked').length == $('.chkvkm').length) {//chk if purchase spoc / vkm spoc
//        var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
//        dataGridLEP1.beginUpdate();
//        dataGridLEP1.columnOption('OEM', 'visible', true);
//        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
//        dataGridLEP1.columnOption('Total_Price', 'visible', true);
//        dataGridLEP1.columnOption('Requestor', 'visible', true);
//        dataGridLEP1.columnOption('Reviewer_1', 'visible', true);
//        dataGridLEP1.columnOption('Reviewer_2', 'visible', true);
//        dataGridLEP1.columnOption('SubmitDate', 'visible', true);
//        dataGridLEP1.columnOption('Comments', 'visible', true);
//        dataGridLEP1.columnOption('Project', 'visible', true);
//        dataGridLEP1.columnOption('Category', 'visible', true);
//        dataGridLEP1.columnOption('Cost_Element', 'visible', true);
//        dataGridLEP1.columnOption('BudgetCode', 'visible', true);
//        dataGridLEP1.columnOption('Unit_Price', 'visible', true);
//        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
//        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
//        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);


//        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', true);
//        dataGridLEP1.columnOption('LeadTime', 'visible', true);
//        //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRFO);
//        dataGridLEP1.columnOption('RequiredDate', 'visible', true);
//        dataGridLEP1.columnOption('RequestOrderDate', 'visible', true);
//        dataGridLEP1.columnOption('OrderDate', 'visible', true);
//        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', true);
//        dataGridLEP1.columnOption('OrderID', 'visible', true);
//        dataGridLEP1.columnOption('OrderPrice', 'visible', true);
//        dataGridLEP1.columnOption('OrderedQuantity', 'visible', true);

//        dataGridLEP1.columnOption('Customer_Name', 'visible', true);
//        dataGridLEP1.columnOption('Customer_Dept', 'visible', true);
//        dataGridLEP1.columnOption('BM_Number', 'visible', true);
//        dataGridLEP1.columnOption('Task_ID', 'visible', true);
//        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', true);
//        dataGridLEP1.columnOption('PIF_ID', 'visible', true);

//        dataGridLEP1.columnOption('BU', 'visible', true);
//        dataGridLEP1.columnOption('DEPT', 'visible', true);
//        dataGridLEP1.columnOption('Group', 'visible', true);
//        dataGridLEP1.columnOption('Item_Name', 'visible', true);
//        dataGridLEP1.columnOption('LeadTime', 'visible', true);

//        /*BGSW RFO FIELDS*/
//        dataGridLEP1.columnOption('OrderType', 'visible', popupedit == true ? true : false);
//        dataGridLEP1.columnOption('CostCenter', 'visible', popupedit == true ? true : false);
//        dataGridLEP1.columnOption('BudgetCenterID', 'visible', popupedit == true ? true : false);
//        dataGridLEP1.columnOption('UnitofMeasure', 'visible', popupedit == true ? true : false);
//        dataGridLEP1.columnOption('UnloadingPoint', 'visible', popupedit == true ? true : false);
//        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', popupedit == true ? true : false);
//        dataGridLEP1.columnOption('LabName', 'visible', popupedit == true ? true : false);
//        dataGridLEP1.columnOption('RFOReqNTID', 'visible', popupedit == true ? true : false);
//        dataGridLEP1.columnOption('RFOApprover', 'visible', popupedit == true ? true : false);
//        dataGridLEP1.columnOption('QuoteAvailable', 'visible', popupedit == true ? true : false);
//        dataGridLEP1.columnOption('GoodsRecID', 'visible', popupedit == true ? true : false);

//        dataGridLEP1.columnOption('Material_Part_Number', 'visible', popupedit == true ? true : false);
//        dataGridLEP1.columnOption('SupplierName_with_Address', 'visible', popupedit == true ? true : false);
//        dataGridLEP1.columnOption('Purchase_Type', 'visible', popupedit == true ? true : false);
//        dataGridLEP1.columnOption('Project_ID', 'visible', popupedit == true ? true : false);


//        dataGridLEP1.endUpdate();
//    }
//    else {
//        var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
//        dataGridLEP1.beginUpdate();
//        dataGridLEP1.columnOption('OEM', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
//        dataGridLEP1.columnOption('Total_Price', 'visible', true);
//        dataGridLEP1.columnOption('Requestor', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Reviewer_1', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Reviewer_2', 'visible', chkRequest);
//        dataGridLEP1.columnOption('SubmitDate', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Comments', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Project', 'visible', chkRequest);

//        dataGridLEP1.columnOption('Category', 'visible', chkItem);
//        dataGridLEP1.columnOption('Cost_Element', 'visible', chkItem);
//        dataGridLEP1.columnOption('BudgetCode', 'visible', chkItem);
//        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
//        dataGridLEP1.columnOption('Unit_Price', 'visible', true);


//        dataGridLEP1.columnOption('BU', 'visible', chkRequest);
//        dataGridLEP1.columnOption('DEPT', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Group', 'visible', chkRequest);
//        dataGridLEP1.columnOption('Item_Name', 'visible', true);

//        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', chkRFO);
//        dataGridLEP1.columnOption('LeadTime', 'visible', chkRFO);
//        //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRequest);
//        dataGridLEP1.columnOption('RequiredDate', 'visible', chkRFO);
//        dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
//        dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
//        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', chkRFO);
//        dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
//        dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
//        dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);

//        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
//        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);
//        dataGridLEP1.columnOption('LeadTime', 'visible', true);

//        dataGridLEP1.columnOption('Customer_Name', 'visible', chkNonVKM);
//        dataGridLEP1.columnOption('Customer_Dept', 'visible', chkNonVKM);
//        dataGridLEP1.columnOption('BM_Number', 'visible', chkNonVKM);
//        dataGridLEP1.columnOption('Task_ID', 'visible', chkNonVKM);
//        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', chkNonVKM);
//        dataGridLEP1.columnOption('PIF_ID', 'visible', chkNonVKM);


//        /*BGSW RFO FIELDS*/
//        dataGridLEP1.columnOption('OrderType', 'visible', chkRFO && (popupedit == true ? true : false));
//        dataGridLEP1.columnOption('CostCenter', 'visible', chkRFO && (popupedit == true ? true : false));
//        dataGridLEP1.columnOption('BudgetCenterID', 'visible', chkRFO && (popupedit == true ? true : false));
//        dataGridLEP1.columnOption('UnitofMeasure', 'visible', chkRFO && (popupedit == true ? true : false));
//        dataGridLEP1.columnOption('UnloadingPoint', 'visible', chkRFO && (popupedit == true ? true : false));
//        dataGridLEP1.columnOption('BudgetCodeDescription', 'visible', chkItem);
//        dataGridLEP1.columnOption('LabName', 'visible', chkRFO && (popupedit == true ? true : false));
//        dataGridLEP1.columnOption('RFOReqNTID', 'visible', chkRFO && (popupedit == true ? true : false));
//        dataGridLEP1.columnOption('RFOApprover', 'visible', chkRFO && (popupedit == true ? true : false));
//        dataGridLEP1.columnOption('QuoteAvailable', 'visible', chkRFO && (popupedit == true ? true : false));
//        dataGridLEP1.columnOption('GoodsRecID', 'visible', chkRFO && (popupedit == true ? true : false));


//        dataGridLEP1.columnOption('Material_Part_Number', 'visible', chkNonVKM && (popupedit == true ? true : false));
//        dataGridLEP1.columnOption('SupplierName_with_Address', 'visible', chkNonVKM && (popupedit == true ? true : false));
//        dataGridLEP1.columnOption('Purchase_Type', 'visible', chkNonVKM && (popupedit == true ? true : false));
//        dataGridLEP1.columnOption('Project_ID', 'visible', chkNonVKM && (popupedit == true ? true : false));

//        dataGridLEP1.endUpdate();
//    }
//}

//function editCellTemplate(cellElement, cellInfo) {
//    //debugger;
//    cellInfo.setValue(cellInfo.data.QuoteAvailable);
//    let mainradioGroupContainer = document.createElement("div");
//    //let subradioGroupContainer = document.createElement("div");



//    var subradioGroupContainer = $("<div class='subradio-element'></div>");



//    var fileUploadContainer = $("<div class='fileupload-element'></div>");
//    var fileUploaderElement = $("<div class='file1-element'></div>"); //document.createElement("div");
//    var fileUploaderElement1 = $("<div class='file2-element'></div>"); //document.createElement("div");
//    var fileUploaderElement2 = $("<div class='file3-element'></div>"); //document.createElement("div");



//    var fileDownloadContainer = $("<div class='fileDownload-element'></div>");
//    var filedownloadElement1 = $("<div class='filedownload1'></div>");
//    var filedownloadElement2 = $("<div class='filedownload2'></div>");
//    var filedownloadElement3 = $("<div class='filedownload3'></div>");

//    //cellElement.append(imageElement);
//    cellElement.append(mainradioGroupContainer);
//    let mainradioButtonGroup = $(mainradioGroupContainer).dxRadioGroup({
//        dataSource: [
//            { text: "Yes" },
//            { text: "No" }



//        ],
//        layout: "horizontal",
//        onValueChanged: function (e) {
//            //debugger;
//            //const previousValue = e.previousValue.text;
//            //const newValue = e.value.text;
//            cellInfo.setValue(e.value.text);

//            if (e.value.text == "Yes") {
//                cellElement.append(subradioGroupContainer);
//                let subradioButtonGroup = $(subradioGroupContainer).dxRadioGroup({
//                    dataSource: [
//                        { text: "Specific Vendor" },
//                        { text: "Any Vendor" }



//                    ],



//                    layout: "horizontal",
//                    onValueChanged: function (e) {
//                        //debugger;



//                        if (e.value.text == "Specific Vendor") {
//                            fileUploadContainer.empty();
//                            fileUploadContainer.append(fileUploaderElement);
//                            subradioGroupContainer.append(fileUploadContainer);
//                            filedownloadElement2.empty();
//                            filedownloadElement3.empty();
//                            filedownloadElement1.empty();
//                            filedownloadElement1 = $("<div class='filedownload1'></div>");
//                            subradioGroupContainer.append(filedownloadElement1);
//                            let fileUploader = $(fileUploaderElement).dxFileUploader({
//                                name: "file",
//                                multiple: true,
//                                //accept: "*",
//                                allowedFileExtensions: ['.pdf', '.docx', '.doc'],
//                                uploadMode: "useForm",
//                                //uploadUrl: `${backendURL}AsyncFileUpload`,
//                                onValueChanged: function (e) {
//                                    //debugger;
//                                    //url = e.component.option("uploadUrl");
//                                    //url = updateQueryStringParameter(url, "id", editRowID);
//                                    //e.component.option("uploadUrl", url);
//                                    //file = e.value;
//                                    ////debugger;



//                                        var uploadedfilename = document.querySelector('.dx-fileuploader-files-container');



//                                        for (var i = 0; i < e.value.length; i++) {





//                                            if (e.value[i].name.indexOf("Quot") != -1 || e.value[i].name.indexOf("Single Source Justification") != -1) {

//                                                file = e.value;


//                                                if (uploadedfilename != undefined) {

//                                                    uploadedfilename.style.visibility = 'visible';

//                                                    uploadedfilename.style.height = "50px";

//                                                    uploadedfilename.style.paddingTop = "0px";

//                                                }

//                                            }

//                                            else {

//                                                file = null;

//                                                alert('Invalid file');

//                                                if (uploadedfilename != undefined) {

//                                                    uploadedfilename.style.visibility = 'hidden';

//                                                    uploadedfilename.style.height = "0px";

//                                                    uploadedfilename.style.paddingTop = "0px";

//                                                }

//                                            }

//                                        }



//                                    //let reader = new FileReader();
//                                    //reader.onload = function (args) {
//                                    //    imageElement.setAttribute('src', args.target.result);
//                                    //}
//                                    //reader.readAsDataURL(e.value[0]); // convert to base64 string
//                                },
//                                onUploaded: function (e) {
//                                    //debugger;
//                                    cellInfo.setValue("images/employees/" + e.request.responseText);
//                                    retryButton.option("visible", false);
//                                },
//                                onUploadError: function (e) {
//                                    //debugger;
//                                    let xhttp = e.request;
//                                    if (xhttp.status === 400) {
//                                        e.message = e.error.responseText;
//                                    }
//                                    if (xhttp.readyState === 4 && xhttp.status === 0) {
//                                        e.message = "Connection refused";
//                                    }
//                                    //retryButton.option("visible", true);
//                                }
//                            }).dxFileUploader("instance");



//                            var fileName = "Single Source Justification.doc";
//                            var url = "Templates/" + fileName;
//                            let downloadtemplate = $('<a/>').addClass('dx-link')
//                                .text('Click here to download SSJ template')
//                                .attr('href', url)
//                                .on('dxclick', function () {
//                                    //Do something with options.data;  
//                                    //debugger;
//                                    //alert(options.data.RequestID);
//                                    //$.ajax({



//                                    //    type: "POST",
//                                    //    url: "/BudgetingOrder/DownloadTemplate",
//                                    //    data: { 'Type': 'NS' },
//                                    //    datatype: "json",
//                                    //    async: true,
//                                    //    success: function (data) {
//                                    //        //debugger;
//                                    //        window.open(data.Result, '_blank');
//                                    //    }
//                                    //});



//                                    //var currentObject = 'NS';
//                                    //$.get('@Url.Action("DownloadTemplate", "BudgetingOrder")', { Type: currentObject });
//                                    //var url = '@Html.Raw((Url.Action("GetTSOUDetails", "SLCockpit", new { SDate = "_sdate", EDate = "_edate" ,Location= "_Locations_sel", Labtype= "_Labtype_sel" })))';





//                                    //url = "C:/UserDrive/GHB1COB/SmartLab_2023/Master/SmartLab_SourceCode/SmartLab/Templates/" + fileName;
//                                    $.ajax({
//                                        url: url,
//                                        cache: false,
//                                        xhr: function () {
//                                            var xhr = new XMLHttpRequest();
//                                            xhr.onreadystatechange = function () {
//                                                if (xhr.readyState == 2) {
//                                                    if (xhr.status == 200) {
//                                                        xhr.responseType = "blob";
//                                                    } else {
//                                                        xhr.responseType = "text";
//                                                    }
//                                                }
//                                            };
//                                            return xhr;
//                                        },
//                                        success: function (data) {
//                                            //Convert the Byte Data to BLOB object.
//                                            var blob = new Blob([data], { type: "application/octetstream" });



//                                            //Check the Browser type and download the File.
//                                            var isIE = false || !!document.documentMode;
//                                            if (isIE) {
//                                                window.navigator.msSaveBlob(blob, fileName);
//                                            } else {
//                                                var url = window.URL || window.webkitURL;
//                                                link = url.createObjectURL(blob);
//                                                var a = $("<a />");
//                                                a.attr("download", fileName);
//                                                a.attr("href", link);
//                                                $("body").append(a);
//                                                a[0].click();
//                                                $("body").remove(a);
//                                            }
//                                        }
//                                    });





//                                }).appendTo(filedownloadElement1);
//                        }
//                        else {
//                            //$(this).closest('.file1-element').remove();



//                            fileUploadContainer.empty();
//                            fileUploadContainer.append(fileUploaderElement1);
//                            subradioGroupContainer.append(fileUploadContainer);
//                            filedownloadElement1.empty();
//                            filedownloadElement3.empty();
//                            filedownloadElement2.empty();
//                            filedownloadElement2 = $("<div class='filedownload2'></div>");
//                            subradioGroupContainer.append(filedownloadElement2);





//                            let fileUploader = $(fileUploaderElement1).dxFileUploader({
//                                name: "file",
//                                multiple: true,
//                                //accept: "*",
//                                allowedFileExtensions: ['.pdf', '.docx', '.doc'],
//                                uploadMode: "useForm",
//                                //uploadUrl: `${backendURL}AsyncFileUpload`,
//                                onValueChanged: function (e) {
//                                    //debugger;

//                                    var uploadedfilename = document.querySelector('.dx-fileuploader-files-container');



//                                    for (var i = 0; i < e.value.length; i++) {





//                                        if (e.value[i].name.indexOf("Quot") != -1 || e.value[i].name.indexOf("Neutral Specification") != -1) {

//                                            file = e.value;


//                                            if (uploadedfilename != undefined) {

//                                                uploadedfilename.style.visibility = 'visible';

//                                                uploadedfilename.style.height = "50px";

//                                                uploadedfilename.style.paddingTop = "0px";

//                                            }

//                                        }

//                                        else {

//                                            file = null;

//                                            alert('Invalid file');

//                                            if (uploadedfilename != undefined) {

//                                                uploadedfilename.style.visibility = 'hidden';

//                                                uploadedfilename.style.height = "0px";

//                                                uploadedfilename.style.paddingTop = "0px";

//                                            }

//                                        }

//                                    }




//                                },
//                                onUploaded: function (e) {
//                                    //debugger;
//                                    cellInfo.setValue("images/employees/" + e.request.responseText);
//                                    retryButton.option("visible", false);
//                                },
//                                onUploadError: function (e) {
//                                    //debugger;
//                                    let xhttp = e.request;
//                                    if (xhttp.status === 400) {
//                                        e.message = e.error.responseText;
//                                    }
//                                    if (xhttp.readyState === 4 && xhttp.status === 0) {
//                                        e.message = "Connection refused";
//                                    }
//                                    //retryButton.option("visible", true);
//                                }
//                            }).dxFileUploader("instance");



//                            var fileName = "Neutral Specification.doc";
//                            var url = "Templates/" + fileName;
//                            let downloadtemplate = $('<a/>').addClass('dx-link')
//                                .text('Click here to download NS template')
//                                .attr('href', url)
//                                .on('dxclick', function () {
//                                    //Do something with options.data;  
//                                    //debugger;
//                                    //alert(options.data.RequestID);
//                                    //$.ajax({



//                                    //    type: "POST",
//                                    //    url: "/BudgetingOrder/DownloadTemplate",
//                                    //    data: { 'Type': 'NS' },
//                                    //    datatype: "json",
//                                    //    async: true,
//                                    //    success: function (data) {
//                                    //        //debugger;
//                                    //        window.open(data.Result, '_blank');
//                                    //    }
//                                    //});



//                                    //var currentObject = 'NS';
//                                    //$.get('@Url.Action("DownloadTemplate", "BudgetingOrder")', { Type: currentObject });
//                                    //var url = '@Html.Raw((Url.Action("GetTSOUDetails", "SLCockpit", new { SDate = "_sdate", EDate = "_edate" ,Location= "_Locations_sel", Labtype= "_Labtype_sel" })))';





//                                    //url = "C:/UserDrive/GHB1COB/SmartLab_2023/Master/SmartLab_SourceCode/SmartLab/Templates/" + fileName;
//                                    $.ajax({
//                                        url: url,
//                                        cache: false,
//                                        xhr: function () {
//                                            var xhr = new XMLHttpRequest();
//                                            xhr.onreadystatechange = function () {
//                                                if (xhr.readyState == 2) {
//                                                    if (xhr.status == 200) {
//                                                        xhr.responseType = "blob";
//                                                    } else {
//                                                        xhr.responseType = "text";
//                                                    }
//                                                }
//                                            };
//                                            return xhr;
//                                        },
//                                        success: function (data) {
//                                            //Convert the Byte Data to BLOB object.
//                                            var blob = new Blob([data], { type: "application/octetstream" });



//                                            //Check the Browser type and download the File.
//                                            var isIE = false || !!document.documentMode;
//                                            if (isIE) {
//                                                window.navigator.msSaveBlob(blob, fileName);
//                                            } else {
//                                                var url = window.URL || window.webkitURL;
//                                                link = url.createObjectURL(blob);
//                                                var a = $("<a />");
//                                                a.attr("download", fileName);
//                                                a.attr("href", link);
//                                                $("body").append(a);
//                                                a[0].click();
//                                                $("body").remove(a);
//                                            }
//                                        }
//                                    });





//                                }).appendTo(filedownloadElement2);



//                        }
//                    }
//                }).dxRadioGroup("instance");







//            }
//            else {
//                cellElement.append(subradioGroupContainer);
//                subradioGroupContainer.empty();
//                fileUploadContainer.empty();
//                fileUploadContainer.append(fileUploaderElement2);
//                subradioGroupContainer.append(fileUploadContainer);
//                filedownloadElement3.empty();
//                filedownloadElement3 = $("<div class='filedownload3'></div>");
//                subradioGroupContainer.append(filedownloadElement3);
//                let fileUploader = $(fileUploaderElement2).dxFileUploader({
//                    name: "file",
//                    multiple: true,
//                    //accept: "*",
//                    allowedFileExtensions: ['.pdf', '.docx','.doc'],
//                    uploadMode: "useForm",
//                    //showFileList: true,
//                    //onValueChanged: function (e) {
//                    //    //debugger;
//                    //    var values = e.component.option("values");
//                    //    $.each(values, function (index, value) {
//                    //        if (value.name.indexOf(".png") < 3) {
//                    //            e.element
//                    //                .find(".dx-fileuploader-files-container .dx-fileuploader-cancel-button")
//                    //                .eq(index)
//                    //                .trigger("dxclick");
//                    //        }
//                    //    });
//                    //},
//                    ////uploadUrl: `${backendURL}AsyncFileUpload`,
//                    onValueChanged: function (e) {
//                        //debugger;
//                        //url = e.component.option("uploadUrl");
//                        //url = updateQueryStringParameter(url, "id", editRowID);
//                        //e.component.option("uploadUrl", url);
//                        for (var i = 0; i < e.value.length; i++) {

//                            if (e.value[i].name.indexOf("Quot") != -1 || e.value[i].name.indexOf("Neutral Specification") != -1) {
//                                file = e.value;
//                            }
//                            else {
//                                file = null;
//                                alert('Invalid file');
//                            }
//                        }

//                        //file = e.value;
//                        //debugger;
//                        // e.value[0].name
//                        //let reader = new FileReader();
//                        //reader.onload = function (args) {
//                        //    imageElement.setAttribute('src', args.target.result);
//                        //}
//                        //reader.readAsDataURL(e.value[0]); // convert to base64 string
//                    },
//                    onUploaded: function (e) {
//                        //debugger;
//                        cellInfo.setValue("images/employees/" + e.request.responseText);
//                        retryButton.option("visible", false);
//                    },
//                    onUploadError: function (e) {
//                        //debugger;
//                        let xhttp = e.request;
//                        if (xhttp.status === 400) {
//                            e.message = e.error.responseText;
//                        }
//                        if (xhttp.readyState === 4 && xhttp.status === 0) {
//                            e.message = "Connection refused";
//                        }
//                        //retryButton.option("visible", true);
//                    }
//                }).dxFileUploader("instance");






//                var fileName = "Neutral Specification.doc";
//                var url = "Templates/" + fileName;
//                let downloadtemplate = $('<a/>').addClass('dx-link')
//                    .text('Click here to download NS template')
//                    .attr('href', url)
//                    .on('dxclick', function () {
//                        //Do something with options.data;  
//                        //debugger;
//                        //alert(options.data.RequestID);
//                        //$.ajax({



//                        //    type: "POST",
//                        //    url: "/BudgetingOrder/DownloadTemplate",
//                        //    data: { 'Type': 'NS' },
//                        //    datatype: "json",
//                        //    async: true,
//                        //    success: function (data) {
//                        //        //debugger;
//                        //        window.open(data.Result, '_blank');
//                        //    }
//                        //});



//                        //var currentObject = 'NS';
//                        //$.get('@Url.Action("DownloadTemplate", "BudgetingOrder")', { Type: currentObject });
//                        //var url = '@Html.Raw((Url.Action("GetTSOUDetails", "SLCockpit", new { SDate = "_sdate", EDate = "_edate" ,Location= "_Locations_sel", Labtype= "_Labtype_sel" })))';




//                        //url = "C:/UserDrive/GHB1COB/SmartLab_2023/Master/SmartLab_SourceCode/SmartLab/Templates/" + fileName;
//                        $.ajax({
//                            url: url,
//                            cache: false,
//                            xhr: function () {
//                                var xhr = new XMLHttpRequest();
//                                xhr.onreadystatechange = function () {
//                                    if (xhr.readyState == 2) {
//                                        if (xhr.status == 200) {
//                                            xhr.responseType = "blob";
//                                        } else {
//                                            xhr.responseType = "text";
//                                        }
//                                    }
//                                };
//                                return xhr;
//                            },
//                            success: function (data) {
//                                //Convert the Byte Data to BLOB object.
//                                var blob = new Blob([data], { type: "application/octetstream" });



//                                //Check the Browser type and download the File.
//                                var isIE = false || !!document.documentMode;
//                                if (isIE) {
//                                    window.navigator.msSaveBlob(blob, fileName);
//                                } else {
//                                    var url = window.URL || window.webkitURL;
//                                    link = url.createObjectURL(blob);
//                                    var a = $("<a />");
//                                    a.attr("download", fileName);
//                                    a.attr("href", link);
//                                    $("body").append(a);
//                                    a[0].click();
//                                    $("body").remove(a);
//                                }
//                            }
//                        });





//                    }).appendTo(filedownloadElement3);
//            }






//        }
//    }).dxRadioGroup("instance");
//}

////$('#chkItem').on('click', function () {
////    var chkItem;
////    if (this.checked)
////        chkItem = true;
////    else
////        chkItem = false;

////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


////});

////$('#chkRequest').on('click', function () {
////    var chkRequest;
////    if (this.checked)
////        chkRequest = true;
////    else
////        chkRequest = false;

////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


////});

////$('#chkRFO').on('click', function () {
////    var chkRFO;
////    if (this.checked)
////        chkRFO = true;
////    else
////        chkRFO = false;

////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);



////});

////////Javascript file for Budgeting Request Details - mae9cob


//////var dataGrid_order;
//////var newobjdata;
//////var BU_list, DEPT_list, Item_list, Category_list, OEM_list, Group_list, CostElement_list, OrderStatus_list, Fund_list, BudgetCodeList;
//////var Selected = [];
//////var unitprice, reviewer_2, category, costelement, leadtime, BudgetCode;
//////var lookup_data, new_request;
//////var filtered_yr;
//////var leadtime1;
//////var genSpinner_load = document.querySelector("#load");
//////var SubItemList;
//////var addnewitem_flag = false;

//////var objdata_rfoview;
//////var Item_headerFilter, DEPT_headerFilter, Group_headerFilter, BU_headerFilter, OEM_headerFilter, Category_headerFilter, CostElement_headerFilter, OrderStatus_headerFilter, BudgetCode_headerFilter;




//////$(".custom-file-input").on("change", function () {
//////    var fileName = $(this).val().split("\\").pop();
//////    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
//////});

//////var oems_chosen = [];

////////var oems_chosen = new Array();
////////$('input:checkbox[name=Types]:checked').each(function () {
////////    //debugger;
////////    Types.push($(this).val())
////////});
////////function fnOEMChange(oem) {
////////    //debugger;
////////    oems_chosen = [];
////////    //oems_chosen = new Array();
////////    for (var i = 0, len = oem.options.length; i < len; i++) {
////////        if (document.getElementById('selectOEM').selectedIndex != -1) {
////////            options = oem.options;
////////            opt = options[i];
////////            if (opt.selected) {
////////                //store the labids chosen by user from dropdown to process the relevant chart data
////////                oems_chosen.push(opt.value);
////////            }
////////        }
////////    }
////////    ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);


////////}
////////function fnOEMselectChange(oem) {
////////    //debugger;
////////    oems_chosen = [];
////////    //oems_chosen = new Array();
////////    for (var i = 0, len = oem.options.length; i < len; i++) {
////////        if (document.getElementById('selectOEM').selectedIndex != -1) {
////////            options = oem.options;
////////            opt = options[i];
////////            if (opt.selected) {
////////                //store the labids chosen by user from dropdown to process the relevant chart data
////////                oems_chosen.push(opt.value);
////////            }
////////        }
////////    }
////////    ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);

////////}


////////$.ajax({

////////    type: "GET",
////////    url: "/BudgetingOrder/Lookup",
////////    async: false,
////////    success: onsuccess_lookupdata,
////////    error: onerror_lookupdata
////////})


////////function onsuccess_lookupdata(response) {

////////    lookup_data = response.data;
////////    BU_list = lookup_data.BU_List;
////////    OEM_list = lookup_data.OEM_List;
////////    DEPT_list = lookup_data.DEPT_List;
////////    Group_list = lookup_data.Groups_test;
////////    Item_list = lookup_data.Item_List;
////////    Category_list = lookup_data.Category_List;
////////    CostElement_list = lookup_data.CostElement_List;
////////    OrderStatus_list = lookup_data.OrderStatus_List;
////////    Fund_list = lookup_data.Fund_List;

////////    //Item_list_New = lookup_data.Item_List1;

////////}

////////function onerror_lookupdata(response) {
////////    alert("Error in fetching lookup");

////////}



////////$.ajax({
////////    type: "GET",
////////    url: encodeURI("../BudgetingOrder/InitRowValues"),
////////    success: OnSuccessCall_dnew,
////////    error: OnErrorCall_dnew

////////});
////////function OnSuccessCall_dnew(response) {

////////    new_request = response.data;

////////}
////////function OnErrorCall_dnew(response) {

////////    $.notify('Add new error!', {
////////        globalPosition: "top center",
////////        className: "warn"
////////    });
////////}




////////Loading indicator on load of the Request module while fetching the Item Requests
//////window.onload = function () {
//////    //debugger;
//////    document.getElementById("loadpanel").style.display = "block";


//////    genSpinner_load.classList.add('fa');
//////    genSpinner_load.classList.add('fa-spinner');
//////    genSpinner_load.classList.add('fa-pulse');
//////    $("#RequestTable_RFO").prop('hidden', true);

//////    //$("#chkRFO").attr("autocomplete", "off");
//////    //$("#chkRequest").attr("autocomplete", "off");
//////    //$("#chkItem").attr("autocomplete", "off");

//////    //document.getElementById('chkRFO').reset();
//////    //document.getElementById('chkRequest').reset();
//////    //document.getElementById('chkItem').reset();




//////    //$("#chkRFO").prop("checked", false);
//////    //$("#chkRequest").prop("checked", false);
//////    //$("#chkItem").prop("checked", false);

//////    //chkRFO
//////    //chkItem
//////    //chkRequest
//////};



////////Reference the DropDownList for Year to be selected by Requestor
//////var ddlYears = document.getElementById("ddlYears");
////////Determine the Current Year.
//////var currentYear = (new Date()).getFullYear();
////////debugger;
////////Loop and add the Year values to DropDownList.
////////for (var i = currentYear; i >= 2020; i--) {
////////    var option = document.createElement("OPTION");
////////    option.innerHTML = i;
////////    option.value = i;
////////    ddlYears.appendChild(option);
////////}
//////////Loop and add the Year values to DropDownList.
//////for (var i = currentYear+1; i >= 2022; i--) {
//////    var option = document.createElement("OPTION");
//////    option.innerHTML = i;
//////    option.value = i;
//////    ddlYears.appendChild(option);

//////    if (option.value == (currentYear + 1)) {
//////        //if (option.value == (currentYear - 2)) {
//////        option.defaultSelected = true;
//////        //option.defaultSelected = true;
//////    }
//////    filtered_yr = $("#ddlYears").val();
//////    filtered_yr = parseInt(filtered_yr) - 1;
//////    filtered_yr = filtered_yr.toString();
//////    //debugger;
//////}




////////At load, Display the data for Current year
//////if (filtered_yr == null) {
//////    filtered_yr = new Date().getFullYear();
//////}
////////debugger;
////////$('.selectpicker').selectpicker('selectAll');//it wil hit fnoemchange to select all & then execute ajaxcallforrequestui
//////ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);
////////debugger;

////////Function to change year from dropdown
//////function fnYearChange(yearselect) {
//////    //debugger;
//////    $("#RequestTable_RFO").prop('hidden', true);
//////    document.getElementById("loadpanel").style.display = "block";

//////    genSpinner_load = document.querySelector("#load");
//////    genSpinner_load.classList.add('fa');
//////    genSpinner_load.classList.add('fa-spinner');
//////    genSpinner_load.classList.add('fa-pulse');
//////    filtered_yr = yearselect.value;

//////    filtered_yr = parseInt(yearselect.value) - 1;
//////    filtered_yr = filtered_yr.toString();
//////    //debugger;
//////    //Ajax call to Get Request Item Data
//////    ajaxCallforRequestUI(filtered_yr/*,oems_chosen*/);



//////}


//////function ajaxCallforRequestUI(filtered_yr) {

//////    $(':checkbox').prop('checked', false);
//////    $.ajax({
//////        type: "POST",
//////        url: "/BudgetingOrder/GetPODetails",
//////        datatype: "json",
//////        success: function (data) {
//////            //debugger;
//////            if (data.data.length > 0) {
//////                //debugger;
//////                //var res = JSON.parse(data.data.Data.Content);
//////                SubItemList = eval(data.data);
//////                //LoadDataGrid(res);

//////            }
//////        },
//////        error: function (jqXHR, exception) {
//////            //debugger;
//////            var msg = '';
//////            if (jqXHR.status === 0) {
//////                msg = 'Not connect.\n Verify Network.';
//////            } else if (jqXHR.status == 404) {
//////                msg = 'Requested page not found. [404]';
//////            } else if (jqXHR.status == 500) {
//////                msg = 'Internal Server Error [500].';
//////            } else if (exception === 'parsererror') {
//////                msg = 'Requested JSON parse failed.';
//////            } else if (exception === 'timeout') {
//////                msg = 'Time out error.';
//////            } else if (exception === 'abort') {
//////                msg = 'Ajax request aborted.';
//////            } else {
//////                msg = 'Uncaught Error.\n' + jqXHR.responseText;
//////            }
//////            $('#post').html(msg);
//////        }
//////    });

//////    $.ajax({

//////        type: "GET",
//////        url: "/BudgetingOrder/Lookup",
//////        async: false,
//////        data: { 'year': filtered_yr },
//////        success: onsuccess_lookupdata,
//////        error: onerror_lookupdata
//////    })


//////    function onsuccess_lookupdata(response) {

//////        lookup_data = response.data;
//////        BU_list = lookup_data.BU_List;
//////        OEM_list = lookup_data.OEM_List;
//////        DEPT_list = lookup_data.DEPT_List;
//////        Group_list = lookup_data.Groups_test;
//////        Item_list = lookup_data.Item_List;
//////        Category_list = lookup_data.Category_List;
//////        CostElement_list = lookup_data.CostElement_List;
//////        OrderStatus_list = lookup_data.OrderStatus_List;
//////        Fund_list = lookup_data.Fund_List;
//////        BudgetCodeList = lookup_data.BudgetCodeList;

//////        Item_headerFilter = lookup_data.Item_HeaderFilter;
//////        DEPT_headerFilter = lookup_data.DEPT_HeaderFilter;
//////        Group_headerFilter = lookup_data.Group_HeaderFilter;
//////        BU_headerFilter = lookup_data.BU_HeaderFilter;
//////        OEM_headerFilter = lookup_data.OEM_HeaderFilter;
//////        Category_headerFilter = lookup_data.Category_HeaderFilter;
//////        CostElement_headerFilter = lookup_data.CostElement_HeaderFilter;
//////        OrderStatus_headerFilter = lookup_data.OrderStatus_HeaderFilter;
//////        BudgetCode_headerFilter = lookup_data.BudgetCode_HeaderFilter;

//////        //Item_list_New = lookup_data.Item_List1;

//////    }

//////    function onerror_lookupdata(response) {
//////        alert("Error in fetching lookup");

//////    }



//////    //Ajax call to Get Request Item Data
//////    //debugger;
//////    $.ajax({
//////        type: "POST",
//////        url: encodeURI("../BudgetingOrder/GetData"),
//////        data: { 'year': filtered_yr },
//////        success: OnSuccess_GetData,
//////        error: OnError_GetData
//////    });


//////    function OnSuccess_GetData(response) {
//////        //debugger;
//////        //if (response.success) {
//////        objdata_rfoview = (response.data);

//////        var isF03F01 = function (position) {

//////            if (position == undefined)
//////                return true;
//////            else
//////                return position && [1, 3].indexOf(position) >= 0;

//////        };
//////        var isDelivered = function (position) {//cancelled also included

//////            //CHANGE
//////            return position && [5, 6, 10].indexOf(position) >= 0;

//////        };


//////        //Hide the Loading indicator once the Request List is fetched
//////        genSpinner_load.classList.remove('fa');
//////        genSpinner_load.classList.remove('fa-spinner');
//////        genSpinner_load.classList.remove('fa-pulse');
//////        document.getElementById("loadpanel").style.display = "none";
//////        $("#RequestTable_RFO").prop('hidden', false);
//////        //debugger;
//////        var c = $("#RequestTable_RFO").dxDataGrid({

//////            dataSource: objdata_rfoview,
//////            width: "96.8vw",
//////            height: "76vh",

//////            editing: {
//////                mode: "row",

//////                // allowAdding: true,

//////                allowUpdating: function (e) {
//////                    //debugger;
//////                    return (response.success && !response.isDashboard_flag && (isDelivered(e.row.data.OrderStatus) || !(e.row.data.RequestToOrder))); //if success = true - access to update and view ; false - access to only view
//////                },
//////                //allowDeleting: function (e) {

//////                //    return !(e.row.data.RequestToOrder) && isF03F01(e.row.data.Fund);
//////                //},
//////                useIcons: true
//////            },
//////            onContentReady: function (e) {

//////                e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
//////            },
//////            onCellPrepared: function (e) {
//////                if (e.rowType === "header" || e.rowType === "filter") {
//////                    e.cellElement.addClass("columnHeaderCSS");
//////                    e.cellElement.find("input").addClass("columnHeaderCSS");
//////                }
//////            },
//////            onInitNewRow: function (e) {

//////                addnewitem_flag = true;
//////                e.data.Requestor = new_request.Requestor;
//////                e.data.Reviewer_1 = new_request.Reviewer_1;
//////                e.data.Reviewer_2 = new_request.Reviewer_2;
//////                e.data.DEPT = new_request.DEPT;
//////                e.data.Group = new_request.Group;



//////            },

//////            columnAutoWidth: true,
//////            allowColumnReordering: true,
//////            allowColumnResizing: true,
//////            focusedRowEnabled: true,
//////            allowColumnReordering: true,
//////            allowColumnResizing: true,
//////            keyExpr: "RequestID",

//////            columnResizingMode: "widget",
//////            columnChooser: {
//////                enabled: true
//////            },
//////            //filterRow: {
//////            //    visible: true

//////            //},
//////            showBorders: true,
//////            headerFilter: {
//////                visible: true,
//////                applyFilter: "auto",
//////                allowSearching: true
//////            },
//////            selection: {
//////                applyFilter: "auto"
//////            },
//////            loadPanel: {
//////                enabled: true
//////            },
//////            //    paging: {
//////            //        pageSize: 100
//////            //},

//////            //searchPanel: {
//////            //    visible: true,
//////            //    width: 240,
//////            //    placeholder: "Search..."
//////            //},
//////            //scrolling: {
//////            //    columnRenderingMode: "virtual"
//////            //},
//////            scrolling: {
//////                mode: "virtual",
//////                rowRenderingMode: "virtual",
//////                columnRenderingMode: "virtual"
//////            },
//////            columns: [
//////                {
//////                    type: "buttons",
//////                    width: 90,
//////                    alignment: "left",
//////                    buttons: [
//////                        "edit",//, "delete",
//////                        {
//////                            hint: "Submit",
//////                            icon: "check",
//////                            visible: function (e) {
//////                                //debugger;
//////                                return ((response.success && !e.row.isEditing && !(e.row.data.RequestToOrder)) || (!e.row.isEditing && isDelivered(e.row.data.OrderStatus))) && !response.isDashboard_flag;/*&& !isOrderApproved(e.row.data.OrderStatus)*/;

//////                            },
//////                            onClick: function (e) {
//////                                //    var LeadTime_tocalc_ExpReqdDt;
//////                                //    //debugger;
//////                                //    // e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
//////                                //    // e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
//////                                if (e.row.data.RequiredDate && !e.row.data.RequestToOrder) {

//////                                    //        $.ajax({

//////                                    //            type: "GET",
//////                                    //            url: "/BudgetingOrder/GetLeadTime",
//////                                    //            data: { 'ItemName': e.row.data.Item_Name },
//////                                    //            datatype: "json",
//////                                    //            async: false,
//////                                    //            success: success_getleadtime,

//////                                    //        });

//////                                    //        function success_getleadtime(response) {
//////                                    //            //debugger;
//////                                    //            if (response == 0) {
//////                                    //                LeadTime_tocalc_ExpReqdDt = "";

//////                                    //                LabAdminApprove(e.row.data.RequestID, filtered_yr);
//////                                    //            }
//////                                    //            else {
//////                                    //                LeadTime_tocalc_ExpReqdDt = response;
//////                                    var ReqdDate = e.row.data.RequiredDate;
//////                                    //                //debugger;
//////                                    $.ajax({

//////                                        type: "GET",
//////                                        url: "/BudgetingOrder/ValidateRequiredDate",
//////                                        data: { 'RequiredDate': ReqdDate },
//////                                        datatype: "json",
//////                                        async: false,
//////                                        success: success_validateReqdDate,

//////                                    });
//////                                    function success_validateReqdDate(info) {
//////                                        //debugger;
//////                                        if (info) {
//////                                            $.notify(info, {
//////                                                globalPosition: "top center",
//////                                                className: "error"
//////                                            })
//////                                        }
//////                                        else {
//////                                            LabAdminApprove(e.row.data.RequestID, filtered_yr);
//////                                        }
//////                                    }

//////                                    //   }


//////                                    //        }


//////                                }
//////                                else {

//////                                    LabAdminApprove(e.row.data.RequestID, filtered_yr);
//////                                }



//////                                e.component.refresh(true);
//////                                e.event.preventDefault();
//////                            }

//////                        }]
//////                },
//////                {

//////                    alignment: "center",
//////                    columns: [
//////                        {
//////                            dataField: "BU",

//////                            width: 65,
//////                            validationRules: [{ type: "required" }],

//////                            lookup: {
//////                                dataSource: BU_list,
//////                                valueExpr: "ID",
//////                                displayExpr: "BU"
//////                            },
//////                            headerFilter: {
//////                                dataSource: BU_headerFilter,
//////                                allowSearch: true
//////                            },


//////                        },
//////                        {
//////                            dataField: "OEM",
//////                            validationRules: [{ type: "required" }],
//////                            width: 70,
//////                            lookup: {
//////                                dataSource: OEM_list,
//////                                valueExpr: "ID",
//////                                displayExpr: "OEM"
//////                            },
//////                            headerFilter: {
//////                                dataSource: OEM_headerFilter,
//////                                allowSearch: true
//////                            },


//////                        },
//////                        {
//////                            dataField: "DEPT",
//////                            caption: "Dept",
//////                            validationRules: [{ type: "required" }],
//////                            headerFilter: {
//////                                dataSource: DEPT_headerFilter,
//////                                allowSearch: true
//////                            },
//////                            setCellValue: function (rowData, value) {

//////                                rowData.DEPT = value;
//////                                rowData.Group = null;

//////                            },
//////                            width: 90,
//////                            lookup: {
//////                                dataSource: function (options) {

//////                                    return {

//////                                        store: DEPT_list,


//////                                    };
//////                                },

//////                                valueExpr: "ID",
//////                                displayExpr: "DEPT"

//////                            },


//////                        },
//////                        {
//////                            dataField: "Group",
//////                            width: 90,
//////                            headerFilter: {
//////                                dataSource: Group_headerFilter,
//////                                allowSearch: true
//////                            },
//////                            validationRules: [{ type: "required" }],
//////                            lookup: {
//////                                dataSource: function (options) {

//////                                    return {

//////                                        store: Group_list,

//////                                        filter: options.data ? ["Dept", "=", options.data.DEPT] : null
//////                                    };

//////                                },
//////                                valueExpr: "ID",
//////                                displayExpr: "Group"
//////                            },


//////                        },


//////                        {
//////                            dataField: "Item_Name",
//////                            width: 200,
//////                            headerFilter: {
//////                                dataSource: Item_headerFilter,
//////                                allowSearch: true
//////                            },
//////                            validationRules: [{ type: "required" }],
//////                            lookup: {
//////                                dataSource: function (options) {

//////                                    return {

//////                                        store: Item_list,
//////                                       // filter: options.data ? ["Deleted", "=", false] : null

//////                                    };

//////                                },
//////                                valueExpr: "S_No",
//////                                displayExpr: "Item_Name"
//////                            },
//////                            calculateSortValue: function (data) {
//////                                //debugger;
//////                                const value = this.calculateCellValue(data);
//////                                return this.lookup.calculateCellValue(value);
//////                            },


//////                        },
//////                        {
//////                            dataField: "Category",
//////                            caption: "Category",
//////                            validationRules: [{ type: "required" }],
//////                            headerFilter: {
//////                                dataSource: Category_headerFilter,
//////                                allowSearch: true
//////                            },
//////                            lookup: {
//////                                dataSource: Category_list,
//////                                valueExpr: "ID",
//////                                displayExpr: "Category"
//////                            },
//////                            allowEditing: false,
//////                            visible: false

//////                        },
//////                        {
//////                            dataField: "Cost_Element",
//////                            headerFilter: {
//////                                dataSource: CostElement_headerFilter,
//////                                allowSearch: true
//////                            },
//////                            lookup: {
//////                                dataSource: CostElement_list,
//////                                valueExpr: "ID",
//////                                displayExpr: "CostElement"
//////                            },
//////                            allowEditing: false,
//////                            visible: false


//////                        },
//////                        {
//////                            dataField: "BudgetCode",
//////                            headerFilter: {
//////                                dataSource: BudgetCode_headerFilter,
//////                                allowSearch: true
//////                            },
//////                            lookup: {
//////                                dataSource: BudgetCodeList,
//////                                valueExpr: "BudgetCode",
//////                                displayExpr: "BudgetCode"
//////                            },
//////                            allowEditing: false,
//////                            visible: false


//////                        },
//////                        {
//////                            dataField: "Required_Quantity",
//////                            caption: "Required Qty",

//////                            dataType: "number",
//////                            setCellValue: function (rowData, value) {

//////                                rowData.Required_Quantity = value;

//////                            },
//////                            allowEditing: false,
//////                            visible: false


//////                        },
//////                        {
//////                            dataField: "Reviewed_Quantity",
//////                            caption: "Review Qty",

//////                            validationRules: [
//////                                { type: "required" },
//////                                {
//////                                    type: "range",
//////                                    message: "Please enter valid count > 0",
//////                                    min: 0,
//////                                    max: 214783647
//////                                }],
//////                            dataType: "number",
//////                            setCellValue: function (rowData, value) {

//////                                rowData.Reviewed_Quantity = value;

//////                            },


//////                        },
//////                        {
//////                            dataField: "ActualAvailableQuantity",
//////                            caption: "Available Qty",
//////                            allowEditing: false,
//////                            width: 102
//////                        },
//////                        {
//////                            dataField: "Unit_Price",
//////                            caption: "Unit Price",
//////                            dataType: "number",
//////                            format: { type: "currency", precision: 2 },
//////                            valueFormat: "#0.00",

//////                            validationRules: [{ type: "required" }, {
//////                                type: "range",
//////                                message: "Please enter valid price > 0",
//////                                min: 0.01,
//////                                max: Number.MAX_VALUE
//////                            }],
//////                            allowEditing: false,
//////                            visible: false


//////                        },
//////                        {
//////                            dataField: "Total_Price",
//////                            caption:"Required Cost",

//////                            calculateCellValue: function (rowData) {

//////                                if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
//////                                    return rowData.Unit_Price * rowData.Required_Quantity;
//////                                }
//////                                else
//////                                    return 0.0;
//////                            },

//////                            dataType: "number",
//////                            format: { type: "currency", precision: 2 },
//////                            valueFormat: "#0.00",
//////                            allowEditing: false,
//////                            visible: false
//////                        },
//////                        {
//////                            dataField: "Reviewed_Cost",

//////                            calculateCellValue: function (rowData) {

//////                                //if (rowData.Reviewed_Quantity > 0 && rowData.Unit_Price > 0) {
//////                                //    return rowData.Unit_Price * rowData.Reviewed_Quantity;
//////                                //}
//////                                //else
//////                                //    return 0.0

//////                                if ((rowData.Reviewed_Cost == null || rowData.Reviewed_Cost == undefined) && rowData.Reviewed_Quantity > 0 && rowData.Unit_Price > 0) {
//////                                    return rowData.Unit_Price * rowData.Reviewed_Quantity;
//////                                }
//////                                else if (rowData.Reviewed_Cost != null || rowData.Reviewed_Cost != undefined) {
//////                                    return rowData.Reviewed_Cost;
//////                                }
//////                                else
//////                                    return 0.0;;
//////                            },

//////                            dataType: "number",
//////                            format: { type: "currency", precision: 2 },
//////                            valueFormat: "#0.00",
//////                            allowEditing: false
//////                        },

//////                        {
//////                            dataField: "Requestor",
//////                            allowEditing: false,
//////                            visible: false
//////                        },
//////                        {
//////                            dataField: "Reviewer_1",
//////                            allowEditing: false,
//////                            visible: false
//////                        },
//////                        {
//////                            dataField: "Reviewer_2",
//////                            allowEditing: false,
//////                            visible: false
//////                        },
//////                        {
//////                            dataField: "SubmitDate",
//////                            allowEditing: false,
//////                            visible: false
//////                        },
//////                        {
//////                            dataField: "RequiredDate",
//////                            dataType: "date",
//////                            visible: true

//////                        },
//////                        {
//////                            dataField: "RequestOrderDate",
//////                            dataType: "date",
//////                            allowEditing: false,
//////                            visible: false

//////                        },
//////                        {
//////                            dataField: "OrderDate",
//////                            dataType: "date",
//////                            allowEditing: false,
//////                            visible: true

//////                        },
//////                        {
//////                            dataField: "TentativeDeliveryDate",
//////                            dataType: "date",
//////                            allowEditing: false,
//////                            visible: false

//////                        },
//////                        {
//////                            dataField: "Comments",
//////                            visible: false,
//////                            allowEditing: false,
//////                        },
//////                        {
//////                            dataField: "PORemarks",
//////                            width: 140,

//////                        },
//////                        {
//////                            dataField: "LeadTime",
//////                            caption: "LeadTime (in days)",
//////                            allowEditing: false,
//////                            visible: true,
//////                            calculateCellValue: function (rowData) {
//////                                //update the LeadTime
//////                                if (rowData.Item_Name == undefined || rowData.Item_Name == null) {

//////                                    leadtime1 = "";
//////                                }

//////                                else {

//////                                    $.ajax({

//////                                        type: "GET",
//////                                        url: "/BudgetingOrder/GetLeadTime",
//////                                        data: { 'ItemName': rowData.Item_Name },
//////                                        datatype: "json",
//////                                        async: false,
//////                                        success: success_getleadtime,

//////                                    });

//////                                    function success_getleadtime(response) {

//////                                        if (response == 0)
//////                                            leadtime1 = "";
//////                                        else
//////                                            leadtime1 = response;

//////                                    }

//////                                }

//////                                return leadtime1;
//////                            }

//////                        },

//////                        {
//////                            dataField: "OrderPrice",
//////                            dataType: "number",
//////                            format: { type: "currency", precision: 2 },
//////                            valueFormat: "#0.00",

//////                            //validationRules: [{ type: "required" }, {
//////                            //    type: "range",
//////                            //    message: "Please enter valid price > 0",
//////                            //    min: 0.01,
//////                            //    max: Number.MAX_VALUE
//////                            //}],
//////                            allowEditing: false,
//////                            //visible: false


//////                        },
//////                        {
//////                            dataField: "OrderedQuantity",
//////                            caption: "Ordered Qty",
//////                            visible: false,
//////                            // allowEditing: flag || !e.row.data.ApprovedSH 
//////                            allowEditing: false



//////                        },
//////                        {
//////                            dataField: "OrderStatus",

//////                            setCellValue: function (rowData, value) {

//////                                rowData.OrderStatus = value;


//////                            },
//////                            lookup: {
//////                                dataSource: function (options) {

//////                                    return {

//////                                        store: OrderStatus_list,
//////                                        filter: options.data ? ["OrderStatus", "=", "Closed"] : null


//////                                    };
//////                                },

//////                                valueExpr: "ID",
//////                                displayExpr: "OrderStatus",

//////                            }
//////                        },
//////                        {
//////                            dataField: "ActualDeliveryDate",
//////                            dataType: "date",
//////                            allowEditing: false,
//////                            visible: false

//////                        },
//////                        {
//////                            dataField: "Fund",

//////                            setCellValue: function (rowData, value) {

//////                                rowData.Fund = value;


//////                            },
//////                            lookup: {
//////                                dataSource: function (options) {

//////                                    return {

//////                                        store: Fund_list,


//////                                    };
//////                                },

//////                                valueExpr: "ID",
//////                                displayExpr: "Fund",
//////                                allowEditing: false

//////                            }
//////                        },




//////                    ]
//////                }],


//////            onEditorPreparing: function (e) {



//////                if (e.parentType === "dataRow" && e.dataField === "BU") {

//////                    if (e.row.data.ApprovedSH == undefined)
//////                        e.row.data.ApprovedSH = false;
//////                    if (e.row.data.RequestToOrder == true)
//////                        e.editorOptions.readOnly = true;
//////                    else
//////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH); //|| !addnewitem_flag;/*isValuepresent(e.value);*/
//////                }
//////                if (e.parentType === "dataRow" && e.dataField === "OEM") {
//////                    if (e.row.data.ApprovedSH == undefined)
//////                        e.row.data.ApprovedSH = false;
//////                    if (e.row.data.RequestToOrder == true)
//////                        e.editorOptions.readOnly = true;
//////                    else
//////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
//////                }
//////                if (e.parentType === "dataRow" && e.dataField === "DEPT") {
//////                    if (e.row.data.ApprovedSH == undefined)
//////                        e.row.data.ApprovedSH = false;
//////                    if (e.row.data.RequestToOrder == true)
//////                        e.editorOptions.readOnly = true;
//////                    else
//////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
//////                }
//////                if (e.parentType === "dataRow" && e.dataField === "Group") {
//////                    if (e.row.data.ApprovedSH == undefined)
//////                        e.row.data.ApprovedSH = false;
//////                    if (e.row.data.RequestToOrder == true)
//////                        e.editorOptions.readOnly = true;
//////                    else
//////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
//////                }
//////                if (e.parentType === "dataRow" && e.dataField === "Item_Name") {
//////                    if (e.row.data.ApprovedSH == undefined)
//////                        e.row.data.ApprovedSH = false;
//////                    if (e.row.data.RequestToOrder == true)
//////                        e.editorOptions.readOnly = true;
//////                    else
//////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
//////                }
//////                if (e.parentType === "dataRow" && e.dataField === "Reviewed_Quantity") {
//////                    if (e.row.data.ApprovedSH == undefined)
//////                        e.row.data.ApprovedSH = false;
//////                    if (e.row.data.RequestToOrder == true)
//////                        e.editorOptions.readOnly = true;
//////                    else
//////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
//////                }
//////                if (e.parentType === "dataRow" && e.dataField === "Reviewed_Cost") {
//////                    if (e.row.data.ApprovedSH == undefined)
//////                        e.row.data.ApprovedSH = false;
//////                    if (e.row.data.RequestToOrder == true)
//////                        e.editorOptions.readOnly = true;
//////                    else
//////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
//////                }
//////                if (e.parentType === "dataRow" && e.dataField === "Fund") {
//////                    if (e.row.data.ApprovedSH == undefined)
//////                        e.row.data.ApprovedSH = false;
//////                    if (e.row.data.RequestToOrder == true)
//////                        e.editorOptions.readOnly = true;
//////                    else
//////                        e.editorOptions.readOnly = (!isF03F01(e.row.data.Fund) && e.row.data.ApprovedSH);
//////                }
//////                if (e.parentType === "dataRow" && e.dataField === "OrderStatus") {

//////                    e.editorOptions.readOnly = !isDelivered(e.row.data.OrderStatus);
//////                }
//////                if (e.parentType === "dataRow" && e.dataField === "RequiredDate") {
//////                    //debugger;
//////                    e.editorOptions.readOnly = e.row.data.RequestToOrder && (!isDelivered(e.row.data.OrderStatus) || isF03F01(e.row.data.Fund));  //1 & (0  || fo3)
//////                }


//////                var component = e.component,
//////                    rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

//////                if (e.parentType === "dataRow" && e.dataField === "Group") {

//////                    e.editorOptions.disabled = (typeof e.row.data.DEPT !== "number");
//////                    if (e.editorOptions.disabled)
//////                        e.editorOptions.placeholder = 'Select Dept first';
//////                    if (!e.editorOptions.disabled)
//////                        e.editorOptions.placeholder = 'Select Group';

//////                }



//////                if (e.dataField === "BU") {

//////                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
//////                    e.editorOptions.onValueChanged = function (e) {
//////                        onValueChanged.call(this, e);

//////                        $.ajax({

//////                            type: "post",
//////                            url: "/BudgetingOrder/GetReviewer",
//////                            data: { BU: e.value },
//////                            datatype: "json",
//////                            traditional: true,
//////                            success: function (data) {

//////                                reviewer_2 = data;

//////                            }
//////                        })
//////                        // Emulating a web service call
//////                        window.setTimeout(function () {
//////                            component.cellValue(rowIndex, "Reviewer_2", reviewer_2);
//////                        }, 1000);
//////                    }
//////                }


//////                if (e.dataField === "Item_Name") {

//////                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
//////                    e.editorOptions.onValueChanged = function (e) {
//////                        onValueChanged.call(this, e);
//////                        $.ajax({
//////                            type: "post",
//////                            url: "/BudgetingOrder/GetUnitPrice",
//////                            data: { ItemName: e.value },
//////                            datatype: "json",
//////                            traditional: true,
//////                            success: function (data) {

//////                                if (data > 0)
//////                                    unitprice = data;

//////                            }
//////                        })

//////                        $.ajax({

//////                            type: "post",
//////                            url: "/BudgetingOrder/GetCategory",
//////                            data: { ItemName: e.value },
//////                            datatype: "json",
//////                            traditional: true,
//////                            success: function (data) {
//////                                category = data;

//////                            }
//////                        })

//////                        $.ajax({

//////                            type: "post",
//////                            url: "/BudgetingOrder/GetCostElement",
//////                            data: { ItemName: e.value },
//////                            datatype: "json",
//////                            traditional: true,
//////                            success: function (data) {
//////                                costelement = data;

//////                            }
//////                        })

//////                        $.ajax({

//////                            type: "post",
//////                            url: "/BudgetingOrder/GetBudgetCode",
//////                            data: { ItemName: e.value },
//////                            datatype: "json",
//////                            traditional: true,
//////                            success: function (data) {
//////                                BudgetCode = data;

//////                            }
//////                        })

//////                        window.setTimeout(function () {

//////                            component.cellValue(rowIndex, "Unit_Price", unitprice);
//////                            component.cellValue(rowIndex, "Category", category);
//////                            component.cellValue(rowIndex, "Cost_Element", costelement);
//////                            component.cellValue(rowIndex, "BudgetCode", BudgetCode);

//////                        },
//////                            1000);


//////                    }

//////                }


//////            },
//////            onRowUpdated: function (e) {
//////                $.notify(" Your Item Request is being Updated...Please wait!", {
//////                    globalPosition: "top center",
//////                    className: "success"
//////                })
//////                Selected = [];
//////                //var LeadTime_tocalc_ExpReqdDt;
//////                //debugger;
//////                // e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
//////                // e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
//////                if (e.data.RequiredDate && !e.data.RequestToOrder) {

//////                    //    $.ajax({

//////                    //        type: "GET",
//////                    //        url: "/BudgetingOrder/GetLeadTime",
//////                    //        data: { 'ItemName': e.data.Item_Name },
//////                    //        datatype: "json",
//////                    //        async: false,
//////                    //        success: success_getleadtime,

//////                    //    });

//////                    //    function success_getleadtime(response) {
//////                    //        //debugger;
//////                    //        if (response == 0) {
//////                    //            LeadTime_tocalc_ExpReqdDt = "";
//////                    //            Selected.push(e.data);
//////                    //            //debugger;
//////                    //            Update(Selected, filtered_yr);
//////                    //        }
//////                    //        else
//////                    //        {
//////                    //            LeadTime_tocalc_ExpReqdDt = response;     
//////                    var ReqdDate = e.data.RequiredDate;
//////                    //            //debugger;
//////                    $.ajax({

//////                        type: "GET",
//////                        url: "/BudgetingOrder/ValidateRequiredDate",
//////                        data: { /*'LeadTime': LeadTime_tocalc_ExpReqdDt,*/ 'RequiredDate': ReqdDate },
//////                        datatype: "json",
//////                        async: false,
//////                        success: success_validateReqdDate,

//////                    });
//////                    function success_validateReqdDate(info) {
//////                        //debugger;
//////                        if (info) {
//////                            $.notify(info, {
//////                                globalPosition: "top center",
//////                                className: "error"
//////                            })
//////                        }
//////                        else {
//////                            Selected.push(e.data);
//////                            //debugger;
//////                            Update(Selected, filtered_yr);
//////                        }
//////                    }

//////                    //        }


//////                    //    }


//////                }
//////                else {
//////                    Selected.push(e.data);
//////                    //debugger;
//////                    Update(Selected, filtered_yr);

//////                }

//////            },

//////            onRowInserting: function (e) {
//////                addnewitem_flag = false;

//////                // e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
//////                // e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
//////                Selected = [];
//////                Selected.push(e.data);



//////                Update(Selected, filtered_yr);
//////            },
//////            onRowRemoving: function (e) {

//////                Delete(e.data.RequestID, filtered_yr);

//////            },
//////            //masterDetail: {
//////            //    enabled: true,

//////            //    template(container, options) {
//////            //        //debugger;
//////            //        if (options.data.OrderDate != "" && options.data.OrderDate != null && options.data.OrderDate != undefined) {
//////            //            const currentRequestData = options.data;

//////            //            $('<div>')
//////            //                .addClass('master-detail-caption')
//////            //                .text(`${currentRequestData.Item_Name} Purchase details:`)
//////            //                .appendTo(container);

//////            //            $('<div>')
//////            //                .dxDataGrid({
//////            //                    columnAutoWidth: true,
//////            //                    showBorders: true,
//////            //                    headerFilter: {
//////            //                        visible: true,
//////            //                        applyFilter: "auto"
//////            //                    },
//////            //                    searchPanel: {
//////            //                        visible: true,
//////            //                        width: 240,
//////            //                        placeholder: "Search..."
//////            //                    },
//////            //                    columns: [

//////            //                        {

//////            //                            alignment: "center",
//////            //                            columns: [

//////            //                                {
//////            //                                    dataField: "RequestID",
//////            //                                    allowEditing: false,
//////            //                                    visible: false
//////            //                                },

//////            //                                //{
//////            //                                //    dataField: "Unit_Price",
//////            //                                //    caption: "Unit Price",
//////            //                                //    dataType: "number",
//////            //                                //    format: { type: "currency", precision: 0 },
//////            //                                //    valueFormat: "#0",
//////            //                                //    allowEditing: false,
//////            //                                //    validationRules: [{ type: "required" }, {
//////            //                                //        type: "range",
//////            //                                //        message: "Please enter valid price > 0",
//////            //                                //        min: 0.01,
//////            //                                //        max: Number.MAX_VALUE
//////            //                                //    }],
//////            //                                //    allowEditing: false,
//////            //                                //    visible: false


//////            //                                //},
//////            //                                //{
//////            //                                //    dataField: "Total_Price",
//////            //                                //    width: 100,
//////            //                                //    calculateCellValue: function (rowData) {

//////            //                                //        if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
//////            //                                //            return rowData.Unit_Price * rowData.Required_Quantity;
//////            //                                //        }
//////            //                                //        else
//////            //                                //            return 0.0;
//////            //                                //    },

//////            //                                //    dataType: "number",
//////            //                                //    format: { type: "currency", precision: 0 },
//////            //                                //    valueFormat: "#0",
//////            //                                //    allowEditing: false
//////            //                                //},
//////            //                                , {
//////            //                                    dataField: "ItemDescription",
//////            //                                    width: 250
//////            //                                },
//////            //                                {
//////            //                                    dataField: "PONumber",
//////            //                                    caption: "PO",
//////            //                                    allowEditing: false,
//////            //                                },



//////            //                                , {
//////            //                                    dataField: "BudgetCode",
//////            //                                    allowEditing: false,
//////            //                                    visible: false
//////            //                                },


//////            //                                {
//////            //                                    dataField: "POQuantity",
//////            //                                    allowEditing: false,
//////            //                                    caption: "PO Qty"
//////            //                                },

//////            //                                {
//////            //                                    dataField: "Netvalue_USD",
//////            //                                    allowEditing: false,
//////            //                                    visible: false
//////            //                                },


//////            //                                {
//////            //                                    dataField: "POCreatedOn",
//////            //                                    caption: "Order Dt",
//////            //                                    dataType: "date",
//////            //                                    allowEditing: true,
//////            //                                },



//////            //                                {
//////            //                                    dataField: "TentativeDeliveryDate",
//////            //                                    allowEditing: true,
//////            //                                    caption: "Tentative",

//////            //                                    dataType: "date",
//////            //                                },


//////            //                                {
//////            //                                    dataField: "ActualDeliveryDate",
//////            //                                    allowEditing: true,
//////            //                                    caption: "Actual Dt",
//////            //                                    dataType: "date",
//////            //                                },

//////            //                                , {
//////            //                                    dataField: "Currentstatus",
//////            //                                    caption: "Status",
//////            //                                    setCellValue: function (rowData, value) {
//////            //                                        //debugger;
//////            //                                        rowData.Fund = value;

//////            //                                    },
//////            //                                    lookup: {
//////            //                                        dataSource: function (options) {
//////            //                                            //debugger;
//////            //                                            return {

//////            //                                                store: OrderStatus_list,
//////            //                                            };

//////            //                                        },
//////            //                                        valueExpr: "ID",
//////            //                                        displayExpr: "OrderStatus"
//////            //                                    },
//////            //                                }

//////            //                            ]
//////            //                        }],
//////            //                    //dataSource: function (options1) {
//////            //                    //    //debugger;
//////            //                    //    return {

//////            //                    //        store: SubItemList,
//////            //                    //        filter: options1.data ? ['RequestID', '=', options.key] : null
//////            //                    //    };
//////            //                    //},
//////            //                    //dataSource: new DevExpress.data.DataSource({
//////            //                    //    store: SubItemList,
//////            //                    //    filter: ['RequestID', '=', options.key],
//////            //                    //}),
//////            //                    dataSource: new DevExpress.data.DataSource({
//////            //                        store: new DevExpress.data.ArrayStore({
//////            //                            key: 'ID',
//////            //                            data: SubItemList,
//////            //                        }),
//////            //                        filter: ['RequestID', '=', options.key],
//////            //                    }),
//////            //                }).appendTo(container);
//////            //        }

//////            //    },
//////            //}
//////        });


//////        // }
//////        //else {
//////        //    //debugger;
//////        //    $.notify(response.message, {
//////        //        globalPosition: "top center",
//////        //        className: "error"
//////        //    })

//////        //    //Hide the Loading indicator once the Request List is fetched
//////        //    genSpinner_load.classList.remove('fa');
//////        //    genSpinner_load.classList.remove('fa-spinner');
//////        //    genSpinner_load.classList.remove('fa-pulse');
//////        //    document.getElementById("loadpanel").style.display = "none";
//////        //    $("#RequestTable_RFO").prop('hidden', false);
//////        //}




//////    }

//////    function OnError_GetData(response) {
//////        $("#RequestTable_RFO").prop('hidden', false);
//////        $.notify(data.message, {
//////            globalPosition: "top center",
//////            className: "warn"
//////        })
//////    }

//////}




//////$('#btnSubmitAll').click(function () {
//////    LabAdminApprove(1999999999, filtered_yr);
//////});




//////function LabAdminApprove(id, filtered_yr) {
//////    //debugger;
//////    if (id == undefined) {
//////        $.notify('Please check the Fund and Try again later!', {
//////            globalPosition: "top center",
//////            autoHideDelay: 20000,
//////            className: "error"
//////        });
//////    }
//////    else {

//////        //debugger;
//////        if (confirm('Do you confirm to place Request to Order the item(s)?')) {

//////            var genSpinner = document.querySelector("#SubmitSpinner");
//////            if (id == 1999999999) {
//////                genSpinner.classList.add('fa');
//////                genSpinner.classList.add('fa-spinner');
//////                genSpinner.classList.add('fa-pulse');
//////            }

//////            $.ajax({
//////                type: "POST",
//////                url: encodeURI("../BudgetingOrder/LabAdminApprove"),
//////                data: { 'id': id, 'useryear': filtered_yr },
//////                success: function (data) {

//////                    if (id == 1999999999) {

//////                        genSpinner.classList.remove('fa');
//////                        genSpinner.classList.remove('fa-spinner');
//////                        genSpinner.classList.remove('fa-pulse');
//////                    }




//////                    $.ajax({
//////                        type: "POST",
//////                        url: "/BudgetingOrder/GetData",
//////                        data: { 'year': filtered_yr },
//////                        datatype: "json",
//////                        async: true,
//////                        success: success_refresh_getdata,
//////                        error: error_refresh_getdata

//////                    });
//////                    function success_refresh_getdata(response) {

//////                        var getdata = response.data;
//////                        $("#RequestTable_RFO").dxDataGrid({
//////                            dataSource: getdata
//////                        });
//////                    }
//////                    function error_refresh_getdata(response) {

//////                        $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
//////                            globalPosition: "top center",
//////                            className: "warn"
//////                        });

//////                    }

//////                    //debugger;
//////                    if (data.is_MailTrigger) {
//////                        //debugger;

//////                        $.ajax({
//////                            type: "POST",
//////                            url: encodeURI("../Budgeting/SendEmail_Order"),
//////                            data: { 'emailnotify': data.data },
//////                            success: success_email,
//////                            error: error_email
//////                        });

//////                        function success_email(response) {
//////                            $.notify("Mail has been sent to the LabTeam about your Request to Order!", {
//////                                globalPosition: "top center",
//////                                className: "success"
//////                            })

//////                        }
//////                        function error_email(response) {
//////                            $.notify("Unable to send mail to the LabTeam about your Request to Order!!", {
//////                                globalPosition: "top center",
//////                                className: "warn"
//////                            })

//////                        }
//////                    }



//////                    if (data.success) {

//////                        $.notify(data.message, {
//////                            globalPosition: "top center",
//////                            className: "success"
//////                        })
//////                    }
//////                    else {
//////                        $.notify(data.message, {
//////                            globalPosition: "top center",
//////                            className: "error"
//////                        })
//////                    }



//////                }
//////            });



//////        }
//////    }
//////}

//////function Update(id1, filtered_yr) {
//////    //debugger;

//////    if (!id1[0].ApprovedSH && id1[0].Fund == 2) {
//////        //debugger;
//////        $.notify('Cannot add F02 items right now since Request Window has been closed.' + '\n' + ' Only F01/F03 items can be added at this stage!', {
//////            globalPosition: "top center",
//////            autoHideDelay: 20000,
//////            className: "error"
//////        });
//////    }
//////    else {
//////        $.ajax({
//////            type: "POST",
//////            url: encodeURI("../BudgetingOrder/AddOrEdit"),
//////            data: { 'req': id1[0], 'useryear': filtered_yr },
//////            success: function (data) {
//////                //debugger;
//////                //newobjdata = data.data;

//////                //$("#RequestTable_RFO").dxDataGrid({dataSource: newobjdata });
//////                $.ajax({
//////                    type: "POST",
//////                    url: "/BudgetingOrder/GetData",
//////                    data: { 'year': filtered_yr },
//////                    datatype: "json",
//////                    async: true,
//////                    success: success_refresh_getdata,
//////                    error: error_refresh_getdata

//////                });

//////                function success_refresh_getdata(response) {

//////                    var getdata = response.data;
//////                    $("#RequestTable_RFO").dxDataGrid({
//////                        dataSource: getdata
//////                    });
//////                }
//////                function error_refresh_getdata(response) {
//////                    //debugger;
//////                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
//////                        globalPosition: "top center",
//////                        className: "warn"
//////                    });

//////                }



//////                if (data.success) {
//////                    $.notify(data.message, {
//////                        globalPosition: "top center",
//////                        className: "success"
//////                    })
//////                }
//////                else {
//////                    $.notify(data.message, {
//////                        globalPosition: "top center",
//////                        className: "error"
//////                    })
//////                }



//////            }

//////        });


//////    }


//////}

//////function Delete(id, filtered_yr) {

//////    $.ajax({
//////        type: "POST",
//////        url: "/BudgetingOrder/Delete",
//////        data: { 'id': id, 'useryear': filtered_yr },
//////        success: function (data) {
//////            newobjdata = data.data;
//////            $("#RequestTable_RFO").dxDataGrid({ dataSource: newobjdata });
//////        }



//////    });

//////    $.notify(data.message, {
//////        globalPosition: "top center",
//////        className: "success"
//////    })

//////}




////////$(function () {
////////    // run the currently selected effect
////////    function runEffect() {
////////        // get effect type from
////////        var selectedEffect = "blind";

////////        var options = {};

////////        // Run the effect
////////        $("#effect").show(selectedEffect, options, 1000, callback);
////////    };

////////    function callback() {
////////        setTimeout(function () {
////////            $("#effect:visible").removeAttr("style").fadeOut();
////////        }, 60000);
////////    };

////////    // Set effect from select menu value
////////    $("#btn_summary").on("click", function () {
////////        runEffect();
////////    });

////////    $("#effect").hide();
////////});







//////$("#buttonClearFilters").dxButton({
//////    text: 'Clear Filters',
//////    onClick: function () {
//////        $("#RequestTable_RFO").dxDataGrid("clearFilter");
//////    }
//////});

//////$('[data-toggle="tooltip"]').tooltip();

////////BULookup,OEMLookup,DeptLookup,GroupLookup,ItemNameLookup,CostElementLookup,CategoryLookup




////////Export data
//////$("#export").click(function () {
//////    //debugger;
//////    $.ajax({

//////        type: "POST",
//////        url: "/BudgetingOrder/ExportDataToExcel/",
//////        data: { 'useryear': filtered_yr },


//////        success: function (export_result) {
//////            //debugger;

//////            var bytes = new Uint8Array(export_result.FileContents);
//////            var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
//////            var link = document.createElement('a');
//////            link.href = window.URL.createObjectURL(blob);
//////            link.download = export_result.FileDownloadName;
//////            link.click();

//////        },
//////        error: function () {
//////            alert("export error");
//////        }

//////    });
//////});


////////$('#chkRequest').on('click', function () {
////////    var chkRequest;
////////    if (this.checked)
////////        chkRequest = true;
////////    else
////////        chkRequest = false;
////////    var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
////////    dataGridLEP1.beginUpdate();
////////    dataGridLEP1.columnOption('OEM', 'visible', chkRequest);
////////    dataGridLEP1.columnOption('Required_Quantity', 'visible', chkRequest);
////////    dataGridLEP1.columnOption('Total_Price', 'visible', chkRequest);
////////    dataGridLEP1.columnOption('Requestor', 'visible', chkRequest);
////////    dataGridLEP1.columnOption('Reviewer_1', 'visible', chkRequest);
////////    dataGridLEP1.columnOption('Reviewer_2', 'visible', chkRequest);
////////    dataGridLEP1.columnOption('SubmitDate', 'visible', chkRequest);
////////    dataGridLEP1.columnOption('Comments', 'visible', chkRequest);
////////    dataGridLEP1.columnOption('Project', 'visible', chkRequest);
////////    dataGridLEP1.endUpdate();
////////    // $('#RequestTable_RFO').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
////////    //// $('#RequestTable').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
////////    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
////////    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


////////});

////////$('#chkRFO').on('click', function () {
////////    var chkRFO;
////////    if (this.checked)
////////        chkRFO = true;
////////    else
////////        chkRFO = false;
////////    var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
////////    dataGridLEP1.beginUpdate();
////////    dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', chkRFO);
////////    //dataGridLEP1.columnOption('Fund', 'visible', chkRFO);
////////    dataGridLEP1.columnOption('LeadTime', 'visible', chkRFO);
////////    //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRFO);
////////    dataGridLEP1.columnOption('RequiredDate', 'visible', chkRFO);
////////    //dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
////////    dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
////////    dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', chkRFO);
////////    dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
////////    dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
////////    dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);
////////    dataGridLEP1.endUpdate();
////////    // $('#RequestTable').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO); -- tking more time
////////    //// $('#RequestTable').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
////////    //// $('#RequestTable').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
////////    //// $('#RequestTable').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderID', 'visible', chkRFO);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderPrice', 'visible', chkRFO);
////////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderedQuantity', 'visible', chkRFO);



////////});

////////$('#chkItem').on('click', function () {
////////    var chkItem;
////////    if (this.checked)
////////        chkItem = true;
////////    else
////////        chkItem = false;
////////    var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
////////    dataGridLEP1.beginUpdate();
////////    dataGridLEP1.columnOption('Category', 'visible', chkItem);
////////    dataGridLEP1.columnOption('Cost_Element', 'visible', chkItem);
////////    dataGridLEP1.columnOption('Unit_Price', 'visible', chkItem);
////////    dataGridLEP1.endUpdate();
////////    //$('#RequestTable').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
////////    //$('#RequestTable').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
////////    //$('#RequestTable').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


////////});







//////var chkRequest;
//////var chkItem;
//////var chkRFO;
//////var chkNonVKM;
//////$('#chkRequest').on('click', function () {

//////    if (this.checked)
//////        chkRequest = true;
//////    else
//////        chkRequest = false;
//////    checkboxdata();
//////    // $('#RequestTable').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
//////    //// $('#RequestTable').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
//////    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
//////    // //$('#RequestTable').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


//////});

//////$('#chkItem').on('click', function () {

//////    if (this.checked)
//////        chkItem = true;
//////    else
//////        chkItem = false;
//////    checkboxdata();
//////    //$('#RequestTable').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
//////    //$('#RequestTable').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
//////    //$('#RequestTable').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


//////});


//////$('#chkRFO').on('click', function () {

//////    if (this.checked)
//////        chkRFO = true;
//////    else
//////        chkRFO = false;
//////    checkboxdata();
//////    // $('#RequestTable').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO); -- tking more time
//////    //// $('#RequestTable').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
//////    //// $('#RequestTable').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
//////    //// $('#RequestTable').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderID', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderPrice', 'visible', chkRFO);
//////    // $('#RequestTable').dxDataGrid('columnOption', 'OrderedQuantity', 'visible', chkRFO);



//////});

//////$('#chkNonVKM').on('click', function () {

//////    if (this.checked)
//////        chkNonVKM = true;
//////    else
//////        chkNonVKM = false;

//////    checkboxdata();
//////    //$('#RequestTable').dxDataGrid('columnOption', 'Customer_Name', 'visible', NonVKM);
//////    //$('#RequestTable').dxDataGrid('columnOption', 'Customer_Dept', 'visible', NonVKM);
//////    //$('#RequestTable').dxDataGrid('columnOption', 'BM_Number', 'visible', NonVKM);
//////    //$('#RequestTable').dxDataGrid('columnOption', 'Task_ID', 'visible', NonVKM);
//////    //$('#RequestTable').dxDataGrid('columnOption', 'Resource_Group_Id', 'visible', NonVKM);
//////    //$('#RequestTable').dxDataGrid('columnOption', 'PIF_ID', 'visible', NonVKM);




//////});

//////function checkboxdata() {
//////    if ($('.chkvkm:checked').length == 0) {
//////        //debugger;
//////        var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
//////        dataGridLEP1.beginUpdate();
//////        dataGridLEP1.columnOption('OEM', 'visible', true);
//////        dataGridLEP1.columnOption('Requestor', 'visible', false);
//////        dataGridLEP1.columnOption('Reviewer_1', 'visible', false);
//////        dataGridLEP1.columnOption('Reviewer_2', 'visible', false);
//////        dataGridLEP1.columnOption('Project', 'visible', false);
//////        dataGridLEP1.columnOption('RequestDate', 'visible', false);
//////        dataGridLEP1.columnOption('Category', 'visible', false);
//////        dataGridLEP1.columnOption('Cost_Element', 'visible', false);
//////        dataGridLEP1.columnOption('BudgetCode', 'visible', false);
//////        dataGridLEP1.columnOption('Unit_Price', 'visible', false);
//////        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
//////        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
//////        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);


//////        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', false);
//////        dataGridLEP1.columnOption('LeadTime', 'visible', false);
//////        //dataGridLEP1.columnOption('OrderStatus', 'visible', true);
//////        dataGridLEP1.columnOption('RequiredDate', 'visible', false);
//////        dataGridLEP1.columnOption('RequestOrderDate', 'visible', false);
//////        dataGridLEP1.columnOption('OrderDate', 'visible', false);
//////        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', false);
//////        dataGridLEP1.columnOption('OrderID', 'visible', false);
//////        dataGridLEP1.columnOption('OrderPrice', 'visible', false);
//////        dataGridLEP1.columnOption('OrderedQuantity', 'visible', false);

//////        dataGridLEP1.columnOption('Customer_Name', 'visible', false);
//////        dataGridLEP1.columnOption('Customer_Dept', 'visible', false);
//////        dataGridLEP1.columnOption('BM_Number', 'visible', false);
//////        dataGridLEP1.columnOption('Task_ID', 'visible', false);
//////        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', false);
//////        dataGridLEP1.columnOption('PIF_ID', 'visible', false);

//////        dataGridLEP1.columnOption('BU', 'visible', true);
//////        dataGridLEP1.columnOption('DEPT', 'visible', true);
//////        dataGridLEP1.columnOption('Group', 'visible', true);
//////        dataGridLEP1.columnOption('Item_Name', 'visible', true);
//////        dataGridLEP1.columnOption('LeadTime', 'visible', true);

//////        dataGridLEP1.endUpdate();

//////    }
//////    else if (('.chkvkm:checked').length == $('.chkvkm').length) {//chk if purchase spoc / vkm spoc
//////        var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
//////        dataGridLEP1.beginUpdate();
//////        dataGridLEP1.columnOption('OEM', 'visible', true);
//////        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
//////        dataGridLEP1.columnOption('Total_Price', 'visible', true);
//////        dataGridLEP1.columnOption('Requestor', 'visible', true);
//////        dataGridLEP1.columnOption('Reviewer_1', 'visible', true);
//////        dataGridLEP1.columnOption('Reviewer_2', 'visible', true);
//////        dataGridLEP1.columnOption('SubmitDate', 'visible', true);
//////        dataGridLEP1.columnOption('Comments', 'visible', true);
//////        dataGridLEP1.columnOption('Project', 'visible', true);
//////        dataGridLEP1.columnOption('Category', 'visible', true);
//////        dataGridLEP1.columnOption('Cost_Element', 'visible', true);
//////        dataGridLEP1.columnOption('BudgetCode', 'visible', true);
//////        dataGridLEP1.columnOption('Unit_Price', 'visible', true);
//////        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
//////        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
//////        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);


//////        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('LeadTime', 'visible', chkRFO);
//////        //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('RequiredDate', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);

//////        dataGridLEP1.columnOption('Customer_Name', 'visible', chkNonVKM);
//////        dataGridLEP1.columnOption('Customer_Dept', 'visible', chkNonVKM);
//////        dataGridLEP1.columnOption('BM_Number', 'visible', chkNonVKM);
//////        dataGridLEP1.columnOption('Task_ID', 'visible', chkNonVKM);
//////        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', chkNonVKM);
//////        dataGridLEP1.columnOption('PIF_ID', 'visible', chkNonVKM);

//////        dataGridLEP1.columnOption('BU', 'visible', true);
//////        dataGridLEP1.columnOption('DEPT', 'visible', true);
//////        dataGridLEP1.columnOption('Group', 'visible', true);
//////        dataGridLEP1.columnOption('Item_Name', 'visible', true);
//////        dataGridLEP1.columnOption('LeadTime', 'visible', true);

//////        dataGridLEP1.endUpdate();
//////    }
//////    else {
//////        var dataGridLEP1 = $("#RequestTable_RFO").dxDataGrid("instance");
//////        dataGridLEP1.beginUpdate();
//////        dataGridLEP1.columnOption('OEM', 'visible', chkRequest);
//////        dataGridLEP1.columnOption('Required_Quantity', 'visible', true);
//////        dataGridLEP1.columnOption('Total_Price', 'visible', true);
//////        dataGridLEP1.columnOption('Requestor', 'visible', chkRequest);
//////        dataGridLEP1.columnOption('Reviewer_1', 'visible', chkRequest);
//////        dataGridLEP1.columnOption('Reviewer_2', 'visible', chkRequest);
//////        dataGridLEP1.columnOption('SubmitDate', 'visible', chkRequest);
//////        dataGridLEP1.columnOption('Comments', 'visible', chkRequest);
//////        dataGridLEP1.columnOption('Project', 'visible', chkRequest);

//////        dataGridLEP1.columnOption('Category', 'visible', chkItem);
//////        dataGridLEP1.columnOption('Cost_Element', 'visible', chkItem);
//////        dataGridLEP1.columnOption('BudgetCode', 'visible', chkItem);
//////        dataGridLEP1.columnOption('ActualAvailableQuantity', 'visible', true);
//////        dataGridLEP1.columnOption('Unit_Price', 'visible', true);


//////        dataGridLEP1.columnOption('BU', 'visible', chkRequest);
//////        dataGridLEP1.columnOption('DEPT', 'visible', chkRequest);
//////        dataGridLEP1.columnOption('Group', 'visible', chkRequest);
//////        dataGridLEP1.columnOption('Item_Name', 'visible', true);

//////        dataGridLEP1.columnOption('ActualDeliveryDate', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('LeadTime', 'visible', chkRFO);
//////        //dataGridLEP1.columnOption('OrderStatus', 'visible', chkRequest);
//////        dataGridLEP1.columnOption('RequiredDate', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('RequestOrderDate', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('OrderDate', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('TentativeDeliveryDate', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('OrderID', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('OrderPrice', 'visible', chkRFO);
//////        dataGridLEP1.columnOption('OrderedQuantity', 'visible', chkRFO);

//////        dataGridLEP1.columnOption('Reviewed_Quantity', 'visible', true);
//////        dataGridLEP1.columnOption('Reviewed_Cost', 'visible', true);
//////        dataGridLEP1.columnOption('LeadTime', 'visible', true);

//////        dataGridLEP1.columnOption('Customer_Name', 'visible', chkNonVKM);
//////        dataGridLEP1.columnOption('Customer_Dept', 'visible', chkNonVKM);
//////        dataGridLEP1.columnOption('BM_Number', 'visible', chkNonVKM);
//////        dataGridLEP1.columnOption('Task_ID', 'visible', chkNonVKM);
//////        dataGridLEP1.columnOption('Resource_Group_Id', 'visible', chkNonVKM);
//////        dataGridLEP1.columnOption('PIF_ID', 'visible', chkNonVKM);


//////        dataGridLEP1.endUpdate();
//////    }
//////}



////////$('#chkItem').on('click', function () {
////////    var chkItem;
////////    if (this.checked)
////////        chkItem = true;
////////    else
////////        chkItem = false;

////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Category', 'visible', chkItem);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Cost_Element', 'visible', chkItem);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Unit_Price', 'visible', chkItem);


////////});

////////$('#chkRequest').on('click', function () {
////////    var chkRequest;
////////    if (this.checked)
////////        chkRequest = true;
////////    else
////////        chkRequest = false;

////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'OEM', 'visible', chkRequest);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'DEPT', 'visible', chkRequest);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Required_Quantity', 'visible', chkRequest);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewed_Quantity', 'visible', chkRequest);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Total_Price', 'visible', chkRequest);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewed_Cost', 'visible', chkRequest);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Requestor', 'visible', chkRequest);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewer_1', 'visible', chkRequest);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Reviewer_2', 'visible', chkRequest);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'SubmitDate', 'visible', chkRequest);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Comments', 'visible', chkRequest);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Project', 'visible', chkRequest);


////////});

////////$('#chkRFO').on('click', function () {
////////    var chkRFO;
////////    if (this.checked)
////////        chkRFO = true;
////////    else
////////        chkRFO = false;

////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'ActualDeliveryDate', 'visible', chkRFO);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'Fund', 'visible', chkRFO);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'LeadTime', 'visible', chkRFO);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'OrderStatus', 'visible', chkRFO);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'RequiredDate', 'visible', chkRFO);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'RequestOrderDate', 'visible', chkRFO);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'OrderDate', 'visible', chkRFO);
////////    $('#RequestTable_RFO').dxDataGrid('columnOption', 'TentativeDeliveryDate', 'visible', chkRFO);



////////});
