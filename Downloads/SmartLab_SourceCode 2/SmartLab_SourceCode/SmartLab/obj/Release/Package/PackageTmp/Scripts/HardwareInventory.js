////Javascript file for Hardware Inventory

//var bqty, cqty;



var spcalc;

var unitpriceinusd;
var BANLabcar = 40, COBLabcar = 50;
//var BU_list, OEM_list, DEPT_list, Group_list, Item_list,Mode_List;
var dataGridHWInventory;
var countflag = false;
//var modflag = false;
//var delflag = false;
//var InvAuth = false;

//function from cshtml
function GenerateHw(BU_list, OEM_list, Group_list, Item_list, Item_headerFilter, Group_headerFilter, OEM_headerFilter, InventoryType_headerFilter) {

    debugger;


    ajaxCallforHardwareUI(BU_list, OEM_list, Group_list, Item_list, Item_headerFilter, Group_headerFilter, OEM_headerFilter, InventoryType_headerFilter)

}

//function from cshtml


function generate_spare(Item_list, Currency_list, SpareHW_headerFilter) {
    spare(Item_list, Currency_list, SpareHW_headerFilter);

}

//Ajax call for Mode column
$.ajax({

    type: "GET",
    url: "/InventoryLab/GetMode",
    async: false,
    success: onsuccess_getMode,
    error: onerror_getMode
})



//Success function for Mode 
function onsuccess_getMode(response) {

    lookup_data = response.data;
    Mode_List = lookup_data;
    //debugger;

}


function onerror_getMode(response) {
    alert("Error Getting Mode Data");

}


//Ajax call for checking the Authorization


$.ajax({
    type: "GET",
    url: encodeURI("../InventoryLab/checkAuth"),
    async: false,
    success: function (data) {
        debugger;
        if (data.success) {
            debugger;
            if (data.islablnventory == "1") {
                if (data.tomodify == "1") {
                    modflag = true;
                }
                else {
                    modflag = false;
                }
                if (data.data == "1") {
                    delflag = true;
                }
                else {
                    delflag = false;
                }
            }
            else {
                modflag = false;
                delflag = false;
            }
            //alert("Success");
        }
        else {
            debugger;

            modflag = false;
            delflag = false;
            //alert("Error");
        }
    },
    error: function (e) {
        alert("Error getting data");
    },
});




var curryear = new Date().getFullYear();


//Ajax call to Get Hardware Inventory Data
function ajaxCallforHardwareUI(BU_list, OEM_list, Group_list, Item_list, Item_headerFilter, Group_headerFilter, OEM_headerFilter, InventoryType_headerFilter) {

    

    $.ajax({
        type: "GET",
        url: encodeURI("../InventoryLab/GetHWData"),
        //data: { },
        success: OnSuccess_GetHardwaredata,
        error: OnError_GetHWData
    });

    function OnSuccess_GetHardwaredata(response) {
        debugger;
        var hwdata;
        hwdata = (response.data);                //Assigning the hardware datalist to the object

        dataGridHWInventory = $("#HardwareTable").dxDataGrid({

            dataSource: hwdata,

            //pager: {
            //    visible: true,
            //   // allowedPageSizes: [5, 10, 'all'],
            //   // showPageSizeSelector: true,
            //    showInfo: true,
            //    showNavigationButtons: true,
            //},
            loadPanel: {
                enabled: true
            },
            valueChangeEvent: 'keyup',
            hoverStateEnabled: {
                enabled: true
            },
            columnMinWidth: 50,
            showColumnLines: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            remoteOperations: { groupPaging: true },
            searchPanel: {
                visible: true,
                highlightCaseSensitive: true,
            },
            paging: {
                pageSize: 15,
            },
            //pager: {
            //    visible: true,
            //},
            //scrolling: {
            //    mode: "virtual",
            //    rowRenderingMode: "virtual",
            //    columnRenderingMode: "virtual"
            //},
            
            groupPanel: {
                visible: true,
                placeholder: "Group By Panel",
            },
            grouping: {
                autoExpandAll: true,
            },
            /*repaintChangesOnly: true,*/
            columnFixing: {
                enabled: true
            },
            columnChooser: {
                enabled: true
            },
            //assigning the variables for authorization
            editing: {
                mode: "popup",
                allowAdding: modflag,
                allowUpdating: modflag,
                allowDeleting: delflag,
                //allowAdding:false,


                useIcons: true,
                popup: {
                    title: "Inventory Details",
                    width: 900,
                    height: 600,
                    showTitle: true,
                    visible: true,
                    hideOnOutsideClick: true,
                    //width: 450,
                    //height: 350,
                    resizeEnabled: true,
                },
                //form: {
                //}

            },
            onToolbarPreparing: function (e) {
                var dataGrid = e.component;

                e.toolbarOptions.items[0].showText = 'always';


            },
            width: "85vw", //needed to allow horizontal scroll
            height: "75vh",
            columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
            columnAutoHeight: true,
            remoteOperations: false,
            showColonAfterLabel: true,
            showValidationSummary: true,
            //validationMessageMode: 'always',
            //validationMessagePosition: 'right',
            headerFilter: {
                visible: true,
                applyFilter: "auto",
                allowSearching: true
            },
            allowColumnReordering: true,
            rowAlternationEnabled: true,
            showBorders: true,
            //onToolbarPreparing: function (e) {
            //    var dataGrid = e.component;

            //    e.toolbarOptions.items[0].showText = 'always';


            //},
            onEditorPreparing: function (e) {


                if (e.dataField === "OEM" && e.parentType === "dataRow") {
                    e.editorOptions.disabled = !e.row.isNewRow;
                }
                if (e.dataField === "BU" && e.parentType === "dataRow") {
                    e.editorOptions.disabled = !e.row.isNewRow;
                }
                if (e.dataField === "Group" && e.parentType === "dataRow") {
                    e.editorOptions.disabled = !e.row.isNewRow;
                }
                if (e.dataField === "ItemName" && e.parentType === "dataRow") {
                    e.editorOptions.disabled = !e.row.isNewRow;
                }
                if (e.dataField === "ItemName_Planner" && e.parentType === "dataRow") {
                    e.editorOptions.disabled = !e.row.isNewRow;
                }
                if (e.dataField === "POQty" && e.parentType === "dataRow") {
                    e.editorOptions.disabled = !e.row.isNewRow;
                }
                if (e.dataField === "ActualDeliveryDate" && e.parentType === "dataRow") {
                    e.editorOptions.disabled = !e.row.isNewRow;
                }
                if (e.dataField === "UOM" && e.parentType === "dataRow") {
                    e.editorOptions.disabled = !e.row.isNewRow;
                }

                //if (e.parentType === "dataRow") {

                //    //e.dataField.InventoryType.value = e.dataField.InventoryType2.value;
                //        var dataGridLEP1 = $("#HardwareTable").dxDataGrid("instance");
                //        dataGridLEP1.beginUpdate();
                //        dataGridLEP1.columnOption('Inventory_Type', 'visible', true);
                //        dataGridLEP1.endUpdate();
                //    }


                //var component = e.component,
                //    rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

            },

            columns: [
                {
                    type: "buttons",
                    width: 65,
                    alignment: "left",
                    fixed: true,
                    fixedPosition: "left",
                    buttons: [
                        "edit", "delete",
                    ]
                },
                {

                    dataField: 'InventoryType',
                    caption: 'Inventory Type',
                    groupIndex: 0,
                    headerFilter: {
                        dataSource: InventoryType_headerFilter,
                        allowSearch: true
                    },
                    //visible: false,
                    lookup: {
                        dataSource: function (options) {
                            //debugger;
                            return {
                                store: Item_list,
                                filter: function (item) {
                                    if (item.VKM_Year == curryear) {
                                        return true;
                                    }
                                }

                            }
                        },

                        valueExpr: "S_No",
                        displayExpr: "Item_Name",
                    },
                    dataType: 'string',
                    validationRules: [{
                        type: "required",
                        message: "Inventory Type is required"
                    }],
                },
                {
                    dataField: 'SerialNo',
                    //width: 70,
                    caption: 'Serial Number',
                    /*dataType: 'number',*/
                    validationRules: [{
                        type: "required",
                        message: "Serial No. is required",
                        showColonAfterLabel: true,
                        showValidationSummary: true,
                        validationMessageMode: 'always',
                        validationMessagePosition: 'right',
                    }]
                },
                {
                    dataField: 'BondNo',
                    //width: 70,
                    caption: 'Bond Number',
                    validationRules: [{
                        type: "required",
                        message: "Bond No. is required"
                    }]
                    /*dataType: 'number',*/

                },
                {
                    dataField: 'BondDate',
                    caption: 'Bond Date',
                    dataType: 'date',
                    validationRules: [{
                        type: "required",
                        message: "Bond Date is required"
                    }]
                },
                {
                    dataField: 'AssetNo',
                    caption: 'Asset Number',
                    validationRules: [{
                        type: "required",
                        message: "Asset No. is required"
                    }]
                    /*dataType: 'number',*/
                },
                {
                    dataField: 'HardwareResponsible',
                    caption: 'Hardware Responsible',
                    validationRules: [{
                        type: "required",
                        message: "HW Responsible is required"
                    }]
                    /*dataType: 'string',*/
                },
                {
                    dataField: 'HandoverTo',
                    caption: 'Handover To',
                    validationRules: [{
                        type: "required",
                        message: "Handover To is required"
                    }]
                    /*dataType: 'string',*/
                },
                {
                    dataField: 'Mode',
                    caption: 'Mode',
                    lookup: {

                        dataSource: Mode_List,
                        displayExpr: 'Mode',
                        valueExpr: 'ID',
                    },
                    validationRules: [{
                        type: "required",
                        message: "Selecting Mode is required"
                    }]
                    /*dataType: 'string',*/
                },
                {
                    dataField: 'Remarks',
                    caption: 'Remarks',
                    /*dataType: 'string',*/
                    validationRules: [{
                        type: "required",
                        message: "Remarks is required"
                    }]
                },
                {
                    dataField: 'ALMNo',
                    caption: 'ALM Number',
                    validationRules: [{
                        type: "required",
                        message: "ALM No. is required"
                    }]
                    /*dataType: 'number',*/
                },
                {
                    dataField: 'BU',
                    caption: 'BU',
                    //setCellValue: function (rowData, value) {
                    //    debugger;
                    //    rowData.BU = value;
                    //    rowData.Item_Name = null;

                    //},
                    lookup: {
                        dataSource: function (options) {
                            //debugger;
                            return {

                                store: BU_list,

                            };

                        },
                        valueExpr: "ID",
                        displayExpr: "BU",

                    },
                    dataType: 'string',
                    allowEditing: !countflag,
                    validationRules: [{
                        type: "required",
                        message: "BU is required"
                    }]

                },
                {
                    dataField: 'OEM',
                    caption: 'OEM',
                    headerFilter: {
                        dataSource: OEM_headerFilter,
                        allowSearch: true
                    },
                    lookup: {
                        dataSource: function (options) {
                            //debugger;
                            return {

                                store: OEM_list,
                            };

                        },
                        valueExpr: "ID",
                        displayExpr: "OEM"
                    },
                    allowEditing: !countflag,
                    validationRules: [{
                        type: "required",
                        message: "OEM is required"
                    }]

                    /*dataType: 'string',*/
                },
                {
                    dataField: 'Group',
                    caption: 'Group',
                    headerFilter: {
                        dataSource: Group_headerFilter,
                        allowSearch: true
                    },
                    lookup: {
                        dataSource: function (options) {

                            return {

                                store: Group_list,
                                filter: function (item) {
                                    if (item.Outdated == false) {
                                        return true;
                                    }
                                }
                            };

                        },
                        valueExpr: "ID",
                        displayExpr: "Group"
                    },
                    validationRules: [{
                        type: "required",
                        message: "Group is required"
                    }]
                    //allowEditing: !countflag,
                    /*dataType: 'string',*/
                },
                {
                    dataField: 'ItemName',
                    caption: 'Item Name',
                    dataType: 'string',
                    allowEditing: !countflag,
                    validationRules: [{
                        type: "required",
                        message: "Item Name is required"
                    }]

                },
                {
                    dataField: 'ItemName_Planner',
                    caption: 'Item Name From Planner',
                    headerFilter: {
                        dataSource: Item_headerFilter,
                        allowSearch: true
                    },

                    lookup: {
                        dataSource: function (options) {
                            //debugger;
                            return {
                                store: Item_list,

                                filter: function (item) {
                                    if (item.VKM_Year == curryear) {
                                        return true;
                                    }
                                }

                                //filter: function (item) {
                                //    if (item.VKM_Year == curryear) {
                                //        return true;
                                //    }
                                //}

                            }
                        },

                        valueExpr: "S_No",
                        displayExpr: "Item_Name"
                    },
                    dataType: 'string',
                    visible: false,
                    //allowEditing: false,
                },
                //{
                //    dataField: 'POQty',
                //    caption: 'PO Quantity',
                //    // dataType: 'string',

                //    validationRules: [{
                //        type: "required",
                //        message: "Qty is required"
                //    }],
                //    //dataType: 'number',
                //    //alignment:left,
                //    //allowEditing: false,
                //},
                {
                    dataField: 'POQty',
                    caption: 'PO Quantity',
                    dataType: 'string',
                    validationRules: [{
                        type: "required",
                        message: "Qty is required"
                    }],
                },
                {
                    dataField: 'Quantity',
                    caption: 'HW Quantity',
                    dataType: 'string',
                    // dataType: 'string',
                    validationRules: [{
                        type: "required",
                        message: "Qty is required"
                    }],

                },
                {
                    dataField: 'UOM',
                    caption: 'UOM',
                    dataType: 'string',
                    validationRules: [{
                        type: "required",
                        message: "UOM is required"
                    }]
                    //allowEditing: false,
                },
                //{
                //    dataField: 'AvailableQty',
                //    caption: 'Available Quantity',
                ///*dataType: 'number',*/

                //},
                {
                    dataField: 'ActualDeliveryDate',
                    caption: 'Actual Delivery Date',
                    dataType: 'date',
                    validationRules: [{
                        type: "required",
                        message: "Actual Delivery Date is required",
                    }]
                    //allowEditing: false,
                },

            ],
            summary: {

                groupItems: [{
                    column: 'Quantity',
                    summaryType: 'sum',
                    // valueFormat: 'number',
                    //displayFormat: 'Actual Quantity: {0}',
                    showInGroupFooter: false,
                    customizeText: function (e) {
                        //debugger;
                        //I tried add 
                        //console.log(e.value)
                        return "Available Qty: " + e.value;
                    }
                    //alignByColumn: true,
                    // showInHeaderFilter: true,
                }],
            },
            onRowInserting: function (e) {
                debugger;


                $.notify(" Your Hardware Inventory details is being added...Please wait!", {
                    globalPosition: "top center",
                    className: "success"
                })
                Selected = [];

                //e.data.InventoryType = e.data.InventoryType;
                Selected.push(e.data);
                debugger;
                UpdateHW(Selected);
                debugger;




            },
            onRowUpdated: function (e) {
                $.notify(" Your Hardware Inventory details is being Updated...Please wait!", {
                    globalPosition: "top center",
                    className: "success"
                })
                Selected = [];
                //var LeadTime_tocalc_ExpReqdDt;
                debugger;

                Selected.push(e.data);
                debugger;
                UpdateHW(Selected);

            },
            onRowRemoving: function (e) {
                debugger;
                Delete(e.data.ID);

            },
            //To add validation check if all the fields are filled or not
            onRowValidating: function (e) {
                debugger;
                if (e.isValid == false) {
                    debugger;
                    $.notify("Please fill all the necessary details", {
                        globalPosition: "top center",
                        className: "warn"
                    })
                }
            }

        }).dxDataGrid('instance').refresh();


    }

    //Error function for get hw data
    function OnError_GetHWData() {
        debugger;
        $("#HardwareTable").prop('hidden', false);
        $.notify("Error in Loading the page", {
            globalPosition: "top center",
            className: "warn"
        })

        //alert("Error")
    }
}

//Update function to add or edit data
function UpdateHW(id1) {
    debugger;
    //Ajax call
    $.ajax({
        type: "POST",
        url: encodeURI("../InventoryLab/AddOrEdit"),
        data: { 'req': id1[0] },

        //if success, data gets refreshed internally
        success: function (data) {
            debugger;

            //refresh data
            if (data.success) {
                $.ajax({
                    type: "GET",
                    url: encodeURI("../InventoryLab/GetHWData"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    debugger;

                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Updated the details!", {
                        globalPosition: "top center",
                        className: "success"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }
   
            }
            else {
                debugger;
                //ajax call to get data for refreshing
                $.ajax({
                    type: "GET",
                    url: encodeURI("../InventoryLab/GetHWData"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    debugger;

                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Error in updating the details!", {
                        globalPosition: "top center",
                        className: "warn"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }

                //setTimeout(function () { location.reload(true); }, 3000);

            }
        },

        error: function (data) {

            //InvAuth = false;
            $.notify("User is not Authorized!!", {
                globalPosition: "top center",
                className: "error"
            })

            debugger;


        }




    });
}


//function to delete a row
function Delete(id) {
    debugger;
    $.ajax({
        type: "POST",
        url: encodeURI("../InventoryLab/Delete"),
        data: { 'id': id },
        success: function (data) {
            //newobjdata = data.data;
            //$("#HardwareTable").dxDataGrid({ dataSource: newobjdata });


            debugger;
            if (data.success == true) {

                $.ajax({
                    type: "GET",
                    url: encodeURI("../InventoryLab/GetHWData"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    debugger;

                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Deleted Successfully!", {
                        globalPosition: "top center",
                        className: "success"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }
            }
            else {

                $.ajax({
                    type: "GET",
                    url: encodeURI("../InventoryLab/GetHWData"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    debugger;

                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Error deleting the data, Try again!", {
                        globalPosition: "top center",
                        className: "warn"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }
                //setTimeout(function () { location.reload(true); }, 3000);
            }


        }

    });


}

//Function for spare inventory ui


function spare(Item_list, Currency_list,SpareHW_headerFilter) {

    //ajax call to get the data from db
    debugger;
    $.ajax({

        type: "GET",
        url: "/InventoryLab/GetSpareData",
        async: false,
        success: onsuccess_getsparedata,
        error: onerror_getsparedata
    })

        // success function for get data
    function onsuccess_getsparedata(response) {
        debugger;
        sparedata = (response.data);
        dataGridSpareInventory = $("#SpareTable").dxDataGrid({

            dataSource: sparedata,

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
            remoteOperations: { groupPaging: true },
            searchPanel: {
                visible: true,
                highlightCaseSensitive: true,
            },
            paging: {
                pageSize: 15,
            },
            groupPanel: { visible: true },
            grouping: {
                autoExpandAll: true,
            },
            columnFixing: {
                enabled: true
            },
            columnChooser: {
                enabled: true
            },
            wordWrapEnabled: true,
            onToolbarPreparing: function (e) {
                var dataGrid = e.component;

                e.toolbarOptions.items[0].showText = 'always';


            },
            width: "85vw", //needed to allow horizontal scroll
            height: "75vh",
            columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
            columnAutoHeight: true,
            remoteOperations: false,
            showColonAfterLabel: true,
            showValidationSummary: true,
            //validationMessageMode: 'always',
            //validationMessagePosition: 'right',
            headerFilter: {
                visible: true,
                applyFilter: "auto",
                allowSearching: true
            },
            allowColumnReordering: true,
            rowAlternationEnabled: true,
            showBorders: true,
            toolbar: {
                items: [
                    'addRowButton',
                    {

                        /*location: 'after',*/
                        widget: 'dxButton',
                        hint: 'Spare Configuration',
                        options: {
                            icon: 'preferences',
                            onClick() {
                                /*dataGrid.refresh();*/
                                window.location.href = "../InventoryLab/SpareConfiguration";
                            },
                        },

                    },
                    'columnChooserButton',
                    'searchPanel',

                ]
            },
            
            editing: {
                mode: 'popup',
                allowAdding: modflag,
                allowUpdating: modflag,
                allowDeleting: delflag,
                //allowAdding:false,
                useIcons: true,
                popup: {
                    title: "Spare Inventory Details",
                    width: 900,
                    height: 600,
                    showTitle: true,
                    visible: true,
                    hideOnOutsideClick: true,
                    //width: 450,
                    //height: 350,
                    resizeEnabled: true,
                },
                form: {
                    items: [
                        {
                            itemType: 'group',
                            colCount: 2,
                            colSpan: 2,
                            items: [
                                {
                                    dataField: 'SpareHW',
                                    label: {
                                        text: 'Spare HW'
                                    },
                                    dataType: 'string',
                                },
                                {
                                    dataField: 'SpareCalc',
                                    label: {
                                        text: 'Spare Calculation'
                                    },
                                    editorOptions: {
                                        disabled: true,
                                    }
                                    
                                }
                            ]
                        },
                        {
                            itemType: 'group',
                            caption:'Spare Quantity Available',
                            colCount: 3,
                            colSpan: 2,
                            items: [{

                                dataField: 'BANQty',
                                label: {
                                    text: 'BAN'
                                },
                                editorType: 'dxNumberBox',
                                editorOptions: {
                                    showSpinButtons: true,
                                    min: 0,
                                    value:0,
                                    max: Number.MAX_VALUE,
                                }
                            },
                                {
                                    dataField: 'COBQty',
                                    label: {
                                        text: 'COB'

                                    },
                                    editorType: 'dxNumberBox',
                                    editorOptions: {
                                        showSpinButtons: true,
                                        min: 0,
                                        value: 0,
                                        max: Number.MAX_VALUE,
                                        //setDefaultcellValues: function (e) {
                                        //    debugger;
                                        //    if (e.dataField == "COBQty") {
                                        //        e.editorOptions.value = 0;
                                        //        e.setValue(e.editorOptions.value);
                                        //    }
                                        //}
                                    }
                                },
                                {
                                    dataField: 'TotalQty',
                                    label: {
                                        text: 'Total'
                                    },
                                    editorType: 'dxNumberBox',
                                    editorOptions: {
                                        disabled: true,

                                    },


                                }
                            ]
                        },
                        {
                            itemType: 'group',
                            caption: 'Required',
                            colCount: 2,
                            colSpan: 2,
                            items: [{

                                dataField: 'BANreqd',
                                label: {
                                    text: 'BAN'

                                

                                },
                                editorOptions: {
                                    disabled: true,

                                },

                            },
                            {
                                dataField: 'COBreqd',
                                label: {
                                    text: 'COB'

                                

                                },
                                editorOptions: {
                                    disabled: true,

                                },

                            },
                            ]
                        },
                        {
                            itemType: 'group',
                            caption: 'Under Repair',
                            colCount: 2,
                            colSpan: 2,
                            items: [{

                                dataField: 'BANUnderRepair',
                                label: {
                                    text: 'BAN'

                                

                                },
                                //editorOptions: {
                                //    value:"0"
                                //}

                            },
                            {
                                dataField: 'COBUnderRepair',
                                label: {
                                    text: 'COB'

                                

                                },
                                //editorOptions: {
                                //    value: "0"
                                //}

                            },
                            ]
                        },
                        {
                            itemType: 'group',
                            caption: 'Difference',
                            colCount: 2,
                            colSpan: 2,
                            items: [{

                                dataField: 'BANdiff',
                                label: {
                                    text: 'BAN'

                                
                                },
                                editorOptions: {
                                    disabled: true,

                                },

                            },
                            {
                                dataField: 'COBdiff',
                                label: {
                                    text: 'COB'
 
                                
 
                                },
                                editorOptions: {
                                    disabled: true,

                                },

 
                            },
                            ]
                        },
                        {
                            itemType: 'group',
                            colCount: 2,
                            colSpan: 2,
                            items: [{

                                dataField: 'Status',
                                label: {
                                    text: 'Status'
                                }
                            },
                            {
                                dataField: 'Currency',
                                label: {
                                    text: 'Currency'
                                },
                                editorType: 'dxSelectBox',
                                editorOptions: {
                                    items: Currency_list,
                                    displayExpr: 'Currency',
                                    valueExpr: 'ID',
                                   
                                },
                            },
                              {

                                  dataField: 'PriceOriginal',
                                    label: {
                                        text: 'Price'
                                    }
                                },
                                {

                                    dataField: 'PriceUSD',
                                    editorType: 'dxNumberBox',
                                    //format: '$ #',
                                    label: {
                                        text: 'Price in USD'
                                    },
                                    editorOptions: {
 
                                     
 
                                        format: '$ #.#',
                                        disabled: true,

 
                                    }
                                },
                            ]
                        },
                        {
                            itemType: 'group',
                            caption: 'Total Price',
                            colCount: 2,
                            colSpan: 2,
                            items: [{

                                dataField: 'BANTotalPrice',
                                label: {
                                    text: 'BAN'
 
                                },
                                editorOptions: {
                                    disabled: true,
                                    format: '$ #.#',

                                },

 
                            },
                            {
                                dataField: 'COBTotalPrice',
                                label: {
                                    text: 'COB'

                                

                                },
                                editorOptions: {
                                    disabled: true,
                                    format: '$ #.#',

                                },


                            },
                            ]
                        },
                    ]
                }

                    
                
            },
           
           
            columns: [
                {
                    type: "buttons",
                    //width: 69,
                    alignment: "left",
                    fixed: true,
                    fixedPosition: "left",
                    buttons: [
                        "edit", "delete",
                    ]
                },
                {

                    dataField: 'SpareHW',
                    caption: 'Spare HW',
                    width: 200,
 

                    headerFilter: {
                        dataSource: SpareHW_headerFilter,
                        allowSearch: true
                    },
 
                    //setCellValue: function (rowData, value) {
                    //    debugger;
                      
                    //    rowData.SpareHW = value;
                    //    GetSpareCalculation(rowData.SpareHW);
                    //},
                   /* wordWrapEnabled: true,*/
                    lookup: {
                        dataSource: function (options) {
                            //debugger;
                            return {

                                store: Item_list,
 
                                //filter: function (item) {
                                //    if (item.VKM_Year == curryear) {
                                //        return true;
                                //    }
                                //}

                                filter: function (item) {
                                    if (item.VKM_Year == curryear) {
                                        return true;
                                    }
                                }
 
                            }
                        },

                        valueExpr: "S_No",
                        displayExpr: "Item_Name",
                    },
                    dataType: 'string',
                    validationRules: [{
                        type: "required",
                        message: "Spare HW is required"
                    }],
                },
                {
                    caption: 'Spare Quantity',
                    columns: [
                        {
                            dataField: 'BANQty',
                            caption: 'BAN',
                            valueFormat: 'currency',
 
                            value:0,
 
                            setCellValue: function (rowData, value) {
                                debugger;
                               
                                rowData.BANQty = value;
                                //bqty = value;
                                //rowData.COBQty = cqty;
                                //rowData.TotalQty = bqty + cqty;

 

                            },
                            validationRules: [{
                                type: "required",
                                message: "BAN Quantity is required"
                            }],
 

                        },
                        {
                            dataField: 'COBQty',
                            caption: 'COB',
                        /*datatype:'number',*/
                            
                            setCellValue: function (rowData, value) {
                                debugger;
                                /*rowData.BANQty = bqty;*/
                                rowData.COBQty = value;                                                                    
                            },
                            validationRules: [{
                                type: "required",
                                message: "COB Qty is required"
                            }],
 
                            //width: 80,

                        },
                        {
                            dataField: 'TotalQty',
                            caption: 'Total',
                            calculateCellValue: function (rowData) {
                           /*     debugger;*/
                                rowData.COBQty = parseInt(rowData.COBQty);
                                rowData.BANQty = parseInt(rowData.BANQty);
                                rowData.TotalQty = rowData.BANQty + rowData.COBQty;

                                return rowData.TotalQty;
                            }
                           
                        }

                    ]
                },
                {
                    caption: 'Spare Calculation',
                    dataField: 'SpareCalc',
                    //calculateCellValue: function (rowData) {
                    //    debugger;
 
                    //    GetSpareCalculation(rowData.SpareHW);

                    //    return "yes";
 
                    //    return spcalc;
 
                    //}
                    //width: 150,
                    //calculateDisplayValue: function (rowData) {
                    //    GetSpareCalculation(rowData.SpareHW);
                    //}
                },
                {
                    caption: 'Required',
                    columns: [
                        {
                            dataField: 'BANreqd',
                            caption: 'BAN',
                            /*dataType:"number",*/
                            setCellValue: function (rowData, value) {
                                rowData.BANreqd = value;
                            }
                            //width: 80,

                        },
                        {
                            dataField: 'COBreqd',
                            caption: 'COB',
                            /*dataType: "number",*/
                            setCellValue: function (rowData, value) {
                                rowData.COBreqd = value;
                            }
                            //width: 80,

                        },
                      

                    ]
                },
                {
                    caption: 'Under Repair',
                    columns: [
                        {
                            dataField: 'BANUnderRepair',
                            caption: 'BAN',
 
                            /*dataType:"number",*/
                            value: "0",
                            //setCellValue:"0",
 
                            //width: 80,

                        },
                        {
                            dataField: 'COBUnderRepair',
                            caption: 'COB',
                            value: "0",
                            /*dataType: "number",*/
                            //setCellValue: "0",
                            ////width: 80,

                        },


                    ]
                },
                {
                    caption: 'Difference',
                    columns: [
                        {
                            dataField: 'BANdiff',
                            caption: 'BAN',
                            /*dataType: "number",*/
                            setCellValue: function (rowData, value) {
                                rowData.BANdiff = value
                            },
                            calculateCellValue: function (rowData) {
                                rowData.BANreqd = parseInt(rowData.BANreqd);
                                rowData.BANdiff = rowData.BANreqd - rowData.BANQty;

                                return rowData.BANdiff;
                            }
                            //width: 80,

                        },
                        {
                            dataField: 'COBdiff',
                            caption: 'COB',
                            /*dataType: "number",*/
                            setCellValue: function (rowData, value) {
                                rowData.COBdiff = value
                            },
                            calculateCellValue: function (rowData) {
                                debugger;
                                rowData.COBreqd = parseInt(rowData.COBreqd);
                                rowData.COBdiff = rowData.COBreqd - rowData.COBQty;

                                return rowData.COBdiff;
                            }
                            //width: 80,

                        },


                    ]
                },
                {
                    caption: 'Status',
                    dataField: 'Status',
                    //validationRules: [{
                    //    type: "required",
                    //    message: "Status is required"
                    //}],
                    //width: 150,

                },
                {
                    caption: 'Price',
                    dataField: 'PriceOriginal',
                    setCellValue: function (rowData, value) {

                        rowData.PriceOriginal = value;
                        //unitprice = value;
                    },
                    validationRules: [{
                        type: "required",
                        message: "Unit Price is required"
                    }],

                    //width: 150,

                },
                {
                    caption: 'Currency',
                    dataField: 'Currency',
                    //setCellValue: function (rowData, value) {

                    //    rowData.Currency = value;
                    //},
                    //width: 150,
                    lookup: {
                        dataSource: function (options) {
                            //debugger;
                            return {
                                store: Currency_list,
                            }
                        },

                        valueExpr: "ID",
                        displayExpr: "Currency",
                    },
                    validationRules: [{
                        type: "required",
                        message: "Currency is required"
                    }],
                },
                {
                    caption: 'PriceUSD',
                    dataField: 'PriceUSD',
                    setCellValue: function (rowData,value) {

                        rowData.PriceUSD = value

                    },
                    dataType: "number",
                    format: { type: "currency", precision: 0 },
                    valueFormat: "#0",

                    //width: 150,
                }, 
                {
                    caption: 'Total Price',
                    columns: [
                        {
                            dataField: 'BANTotalPrice',
                            caption: 'BAN',
                            format: { type: "$ #0.##;-$ #0.##", },
                            /*valueFormat: 'currency',*/
                            calculateCellValue: function (rowData) {
                                var calc1 = rowData.BANTotalPrice;
                                calc1 = rowData.PriceUSD * rowData.BANdiff;

                                return calc1;

                            }

                            //width: 90,

                        },
                        {
                            dataField: 'COBTotalPrice',
                            caption: 'COB',
                            format: { type: "$ #0.##;-$ #0.##",},
                            //valueFormat: "$ #0.##;(#0.##)",
                            //valueFormat: 'currency',
                            
                            calculateCellValue: function (rowData) {
                                var calc2 = rowData.COBTotalPrice;
                                calc2 = rowData.PriceUSD * rowData.COBdiff;

                                return calc2;

                            }
                            //width: 90,

                        },


                    ]
                },
             
            ],
 
            //onEditorPreparing: function (e) {
            //    var component = e.component,
            //        rowIndex = e.row && e.row.rowIndex;
            //    debugger;
            //    if (e.dataField === "BANQty" && e.parentType === "dataRow") {
            //        /*var onValueChanged = e.editorOptions.onValueChanged;*/
            //        e.editorOptions.onValueChanged = function (e) {
            //            //onValueChanged.call(this, e);
            //            var cobqty = component.cellValue(rowIndex, "COBQty");
            //            var totalqty = e.value + cobqty;
            //            component.cellValue(rowIndex, "TotalQty", totalqty);
            //        }
            //    }
            //},
            //onEditingStart: function (e) {
            //    debugger;
            //    if (e.dataField === "BANQty" && e.parentType === "dataRow") {
            //        bqty = e.value;
            //    }
            //},
        
 
            onEditorPreparing: function (e) {
                var component = e.component,
                    rowIndex = e.row && e.row.rowIndex;
                if (e.dataField === 'SpareHW') {
                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);

                        if (e.value != undefined && e.value != null && e.value != "") {
                            $.ajax({
                                type: "POST",
                                url: encodeURI("../InventoryLab/GetSpareCalc"),
                                data: { 'id': e.value },

                                //if success, data gets refreshed internally
                                success: function (e) {
                                    debugger;

                                    spcalc = e.data;

                                    window.setTimeout(function () {
                                        debugger;
                                        component.cellValue(rowIndex, "SpareCalc", spcalc);

                                        component.cellValue(rowIndex, "BANreqd", Math.round(spcalc * BANLabcar));
                                        component.cellValue(rowIndex, "COBreqd", Math.round(spcalc * COBLabcar));
                                    }, 1000);
                                    //$("#SpareTable").dxDataGrid("columnOption", "SpareCalc", spcalc);
                                    //alert("success")
                                },
                                error: function (e) {
                                    alert("error getting data from configuration");
                                }
                            });
                        }
                        else {
                            
                                debugger;
                                component.cellValue(rowIndex, "SpareCalc","");
                                component.cellValue(rowIndex, "BANreqd", "");
                                component.cellValue(rowIndex, "COBreqd", "");
                           
                        }
                       

                    }

                }

                if (e.dataField === 'Currency' || e.dataField === 'PriceOriginal') {
                    var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                    e.editorOptions.onValueChanged = function (e) {
                        onValueChanged.call(this, e);
                        debugger;
                        var unitprice = component.cellValue(rowIndex, "PriceOriginal");
                        var curr = component.cellValue(rowIndex, "Currency");

                        if (unitprice != undefined && unitprice != null && unitprice != "" && e.value != undefined && e.value != null && e.value != "" && curr != undefined && curr != null && curr != "") {
                            $.ajax({
                                type: "POST",
                                url: encodeURI("../InventoryLab/GetEURINRates"),
                                data: { 'ID': curr },
                                async: false,
                                success: OnSuccess_GetEURINRates,
                                error: OnError_GetEURINRates
                            });
                            function OnSuccess_GetEURINRates(response) {
                                debugger;

                                conversionRate = response.data[0].CurrencyRate;
                                var usdprice = parseFloat(unitprice * conversionRate).toFixed(2);
                               /* window.setTimeout(function () {*/
                                    //debugger;
                                    component.cellValue(rowIndex, "PriceUSD", usdprice);
                                /*}, 10000);*/
                                
                                debugger;
                            }
                            function OnError_GetEURINRates(response) {
                                debugger;
                                conversionEURate = 1.15;
                                conversionINRate = 0.014;
                            }
                        }
                        else {
                            window.setTimeout(function () {
                                //debugger;
                                component.cellValue(rowIndex, "PriceUSD", "");
                            }, 1000);
                        }
                       

                    }
                }

                
            },
            onRowInserting: function (e) {
                debugger;

                e.data.TotalQty = e.data.BANQty + e.data.COBQty;
                e.data.BANdiff = e.data.BANreqd - e.data.BANQty;
                e.data.COBdiff = e.data.COBreqd - e.data.COBQty;
                e.data.BANTotalPrice = e.data.PriceUSD * e.data.BANdiff;
                e.data.COBTotalPrice = e.data.PriceUSD * e.data.COBdiff;

 

                $.notify(" Your Spare Inventory details is being added...Please wait!", {
                    globalPosition: "top center",
                    className: "success"
                })
                Selected = [];

                //e.data.InventoryType = e.data.InventoryType;
                Selected.push(e.data);
                debugger;
                UpdateSpare(Selected);
               
                debugger;




            },
            onRowUpdated: function (e) {
 
              

                e.data.TotalQty = e.data.BANQty + e.data.COBQty;
                e.data.BANdiff = e.data.BANreqd - e.data.BANQty;
                e.data.COBdiff = e.data.COBreqd - e.data.COBQty;
                e.data.BANTotalPrice = e.data.PriceUSD * e.data.BANdiff;
                e.data.COBTotalPrice = e.data.PriceUSD * e.data.COBdiff;
                $.notify(" Your Spare Inventory details is being Updated...Please wait!", {
 
                    globalPosition: "top center",
                    className: "success"
                })
                Selected = [];
                //var LeadTime_tocalc_ExpReqdDt;
                debugger;

                Selected.push(e.data);
                debugger;
                UpdateSpare(Selected);

            },
            onRowRemoving: function (e) {
                debugger;
                DeleteSpare(e.data.ID);

            },
 
 
            onRowValidating: function (e) {
                debugger;
                if (e.isValid == false) {
                    debugger;
                    $.notify("Please fill all the necessary details", {
                        globalPosition: "top center",
                        className: "warn"
                    })
                }
            }
 
        })
    }
    function onerror_getsparedata() {
        alert("Error in get")
    }
}

function UpdateSpare(id1) {
    debugger;
    //Ajax call
    $.ajax({
        type: "POST",
        url: encodeURI("../InventoryLab/AddOrEditSpare"),
        data: { 'req': id1[0] },

        //if success, data gets refreshed internally
        success: function (data) {
            debugger;

            //refresh data
            if (data.success) {
                $.ajax({
                    type: "GET",
                    url: encodeURI("../InventoryLab/GetSpareData"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    debugger;

                    var getdata = response.data;
                    $("#SpareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Updated the details!", {
                        globalPosition: "top center",
                        className: "success"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#SpareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }

            }
            else {
                //ajax call to get data for refreshing
                $.ajax({
                    type: "GET",
                    url: encodeURI("../InventoryLab/GetSpareData"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    debugger;

                    var getdata = response.data;
                    $("#SpareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Error in updating the details!", {
                        globalPosition: "top center",
                        className: "warn"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#SpareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }

                //setTimeout(function () { location.reload(true); }, 3000);

            }
        },

        error: function (data) {

            //InvAuth = false;
            $.notify("User is not Authorized!!", {
                globalPosition: "top center",
                className: "error"
            })

            debugger;


        }




    });
}

 
function GetSpareCalculation(id) {
    debugger;
    //Ajax call
    $.ajax({
        type: "POST",
        url: encodeURI("../InventoryLab/GetSpareCalc"),
        data: { 'id': id },

        //if success, data gets refreshed internally
        success: function (e) {
            spcalc = e.data;
            alert("success")
        },
        error: function (e) {
            alert("error getting data from configuration");
        }
 
//function GetSpareCalculation(id) {
//    debugger;
//    //Ajax call
//    $.ajax({
//        type: "POST",
//        url: encodeURI("../InventoryLab/GetSpareCalc"),
//        data: { 'id': id },

//        //if success, data gets refreshed internally
//        success: function (e) {
//            debugger;
            
//            spcalc = e.data;
//            rowData.SpareCalc = spcalc;
//            //$("#SpareTable").dxDataGrid("columnOption", "SpareCalc", spcalc);
//            alert("success")
//        },
//        error: function (e) {
//            alert("error getting data from configuration");
//        }
 




 
    });
}
 
//    });
//}
 

function DeleteSpare(id) {
    debugger;
    $.ajax({
        type: "POST",
        url: encodeURI("../InventoryLab/DeleteSpare"),
        data: { 'id': id },
        success: function (data) {
            //newobjdata = data.data;
            //$("#HardwareTable").dxDataGrid({ dataSource: newobjdata });


            debugger;
            if (data.success == true) {

                $.ajax({
                    type: "GET",
                    url: encodeURI("../InventoryLab/GetSpareData"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    debugger;

                    var getdata = response.data;
                    $("#SpareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Deleted Successfully!", {
                        globalPosition: "top center",
                        className: "success"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#SpareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }
            }
            else {

                $.ajax({
                    type: "GET",
                    url: encodeURI("../InventoryLab/GetSpareData"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    debugger;

                    var getdata = response.data;
                    $("#SpareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Error deleting the data, Try again!", {
                        globalPosition: "top center",
                        className: "warn"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#SpareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }
                //setTimeout(function () { location.reload(true); }, 3000);
            }


        }

    });


 
}
 



 
