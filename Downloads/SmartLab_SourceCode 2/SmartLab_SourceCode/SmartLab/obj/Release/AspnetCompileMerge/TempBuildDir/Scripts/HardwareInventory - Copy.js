//Javascript file for Hardware Inventory


var BU_list, OEM_list, DEPT_list, Group_list, Item_list,Mode_List;
var dataGridHWInventory;

//Ajax call for getting Lookup data which contains information like BU,OEM 
$.ajax({

    type: "GET",
    url: "/InventoryLab/Lookup",
    async: false,
    success: onsuccess_lookupdata,
    error: onerror_lookupdata
})


//Ajax call for Mode column
$.ajax({

    type: "GET",
    url: "/InventoryLab/GetMode",
    async: false,
    success: onsuccess_getMode,
    error: onerror_getMode
})

//success function for Lookups
function onsuccess_lookupdata(response) {

    lookup_data = response.data;
    BU_list = lookup_data.BU_List;
    OEM_list = lookup_data.OEM_List;
    //DEPT_list = lookup_data.DEPT_List;
    Group_list = lookup_data.Groups_test;//Groups_oldList;
    Item_list = lookup_data.Item_List;
    debugger;

}

//Success function for Mode 
function onsuccess_getMode(response) {

    lookup_data = response.data;
    Mode_List = lookup_data;
    debugger;

}

//Error function for Lookup
function onerror_lookupdata(response) {
    alert("Error lookup");
    debugger;
}

function onerror_getMode(response) {
    alert("Error Getting Mode Data");

}
ajaxCallforHardwareUI()




   //Ajax call to Get Hardware Inventory Data
function ajaxCallforHardwareUI() {

    $.ajax({
        type: "GET",
        url: encodeURI("../InventoryLab/GetHWData"),
        //data: { },
        success: OnSuccess_GetHardwaredata,
        error: OnError_GetHWData
    });
}


function OnSuccess_GetHardwaredata(response) {
    debugger;
    var hwdata;
    hwdata = (response.data);                //Assigning the hardware datalist to the object

    dataGridHWInventory = $("#HardwareTable").dxDataGrid({

        dataSource: hwdata,
        paging: {
            pageSize: 100,
        },
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
        remoteOperations: false,
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true,
        },
        groupPanel: { visible: true },
        grouping: {
            autoExpandAll: false,
        },
        columnFixing: {
            enabled: true
        },
        editing: {
            mode: "row",
            allowUpdating: true,
            allowDeleting: true,
            allowAdding: true,
            useIcons: true
        },
        onToolbarPreparing: function (e) {
            var dataGrid = e.component;

            e.toolbarOptions.items[0].showText = 'always';


        },
        width: "1100px", //needed to allow horizontal scroll
        //height: "400px",
        columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
        columnAutoHeight: true,
        remoteOperations: true,
        scrolling: {
            mode: "virtual",
            rowRenderingMode: "virtual",
            columnRenderingMode: "virtual"
        },
        headerFilter: {
            visible: true,
            applyFilter: "auto",
            allowSearching: true
        },
        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        onToolbarPreparing: function (e) {
            var dataGrid = e.component;

            e.toolbarOptions.items[0].showText = 'always';


        },
        onEditorPreparing: function (e) {
            var component = e.component,
                rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex
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
                ]
            },
            {
                dataField: 'InventoryType',
                //width: 90,
                caption: 'Inventory Type',
                /*dataType: 'string',*/
            },
            {
                dataField: 'SerialNo',
                //width: 70,
                caption: 'Serial Number',
                /*dataType: 'number',*/
                align:'left',
            },
            {
                dataField: 'BondNo',
                //width: 70,
                caption: 'Bond Number',
                /*dataType: 'number',*/

            },
            {
                dataField: 'BondDate',
                caption: 'Bond Date',
                /*dataType: 'date',*/
            },
            {
                dataField: 'AssetNo',
                caption: 'Asset Number',
                /*dataType: 'number',*/
            },
            {
                dataField: 'HardwareResponsible',
                caption: 'Hardware Responsible',
                /*dataType: 'string',*/
            },
            {
                dataField: 'HandoverTo',
                caption: 'Handover To',
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
                /*dataType: 'string',*/
            },
            {
                dataField: 'Remarks',
                caption: 'Remarks',
                /*dataType: 'string',*/
            },
            {
                dataField: 'ALMNo',
                caption: 'ALM Number',
                /*dataType: 'number',*/
            },
            {
                dataField: 'BU',
                caption: 'BU',
                setCellValue: function (rowData, value) {
                    //debugger;
                    rowData.BU = value;
                    rowData.Item_Name = null;

                },
                lookup: {
                                        dataSource: function (options) {
                                            debugger;
                                            return {

                                                store: BU_list,
                                            };

                                        },
                                        valueExpr: "ID",
                                        displayExpr: "BU"
                         },
                dataType: 'string',
            },
            {
                dataField: 'OEM',
                caption: 'OEM',
                lookup: {
                                        dataSource: OEM_list,
                                        valueExpr: "ID",
                                        displayExpr: "OEM"
                       },
                /*dataType: 'string',*/
            },
            {
                dataField: 'Group',
                caption: 'Group',
                lookup: {
                                        dataSource: function (options) {

                                            return {

                                                store: Group_list,
                                            };

                                        },
                                        valueExpr: "ID",
                                        displayExpr: "Group"
                        },
                /*dataType: 'string',*/
            },
            {
                dataField: 'ItemName',
                caption: 'Item Name',
                dataType: 'string',

            },
            {
                dataField: 'ItemName_Planner',
                caption: 'Item Name From Planner',
                lookup: {
                    dataSource: function (options) {
                        debugger;
                        return {
                            store: Item_list,
                        }
                    },

                    valueExpr: "S_No",
                    displayExpr: "Item_Name"
                },
                dataType: 'string',
                visible: false,
            },
            {
                dataField: 'Qty',
                caption: 'Quantity',
                dataType: 'string',
            },
            {
                dataField: 'UOM',
                caption: 'UOM',
                dataType: 'string',
            },
            {
                dataField: 'AvailableQty',
                caption: 'Available Quantity',
                /*dataType: 'number',*/
            },
            {
                dataField: 'ActualDeliveryDate',
                caption: 'Actual Delivery Date',
                /*dataType: 'date',*/
            },

        ],

    });


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