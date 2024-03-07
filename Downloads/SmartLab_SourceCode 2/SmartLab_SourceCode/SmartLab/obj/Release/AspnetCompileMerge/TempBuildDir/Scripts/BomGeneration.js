////$(() =>
////{
////    $('#gridContainer').dxDataGrid(
////        {
///*        /*dataSource:*/mployees*/
////            /* Modification */

////            allowSorting: true,
////            headerFilter: {
////                visible: true,
////                applyFilter: "auto"
////            },


////            export: {
////                enabled: true,

////            },

////            valueChangeEvent: 'keyup',




////            // Exporting to Excel File using the Described Name Function

////            onExporting(e) {
////                const workbook = new ExcelJS.Workbook();
////                const worksheet = workbook.addWorksheet('LabCarDetails');

////                worksheet.columns = [

////                    { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 },

////                ];




////                DevExpress.excelExporter.exportDataGrid({
////                    component: e.component,
////                    worksheet,
////                    autoFilterEnabled: true,
////                }).then(() => {
////                    workbook.xlsx.writeBuffer().then((buffer) => {
////                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'LabCarDetails.xlsx');
////                    });
////                });
////                e.cancel = true;
////            },






////        columns: 
////            [
////                {
////                    dataField: 'Prefix',
////                    caption: 'Title',
////                    width: 150,
////                },
////                {
////                    dataField: 'FirstName',
////                    caption: 'FirstName',
////                    width: 150,
////                },
////                {
////                    dataField: 'LastName',
////                    caption: 'LastName',
////                    width: 150,
////                },

////                {
////                    dataField: 'Position',
////                    caption: 'Position',
////                    width: 150,
////                },

               
              

////            ],






            



////            allowColumnReordering: true,
////            allowColumnResizing: true,
////            /*
////            scrolling: {
////                columnRenderingMode: 'virtual',
////                mode: "virtual",
////                rowRenderingMode: "virtual",
////            },
    
////            */

////            hoverStateEnabled:
////            {
////                enabled: true
////            },

////            filterRow: {
////                visible: true
////            },

////            selection: {
////                applyFilter: "auto"
////            },

////            paging: {
////                pageSize: 10,
////            },


////            pager: {
////                visible: true,
////                allowedPageSizes: [10, 'all'],
////                showPageSizeSelector: true,
////                showInfo: true,
////                showNavigationButtons: true,
////            },



////            searchPanel: {
////                visible: true,
////                width: 240,
////                placeholder: "Search...",
////                highlightCaseSensitive: true,
////            },


////            loadPanel: {
////                enabled: true
////            },

////            columnChooser: {
////                enabled: true
////            },

////            width: "96vw", //needed to allow horizontal scroll
////            height: "60vh",
////            columnMinWidth: 50,
////            showColumnLines: true,
////            showRowLines: true,
////            rowAlternationEnabled: true,
////            remoteOperations: false,
////            showColonAfterLabel: true,
////            showValidationSummary: true,
////            columnFixing: {
////                enabled: true
////            },



////            columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
////            remoteOperations: true,


////            groupPanel: {
////                visible: true,
////                placeholder: "Group By Panel",
////            },
////            grouping: {
////                autoExpandAll: true,
////            },

////            onToolbarPreparing: function (e) {
////                var dataGrid = e.component;

////                e.toolbarOptions.items[0].showText = 'always';


////            },


////            repaintChangesOnly: true,

////   getBOMdatahowBorders: tgetBOMdata////            headerFilter: {
////                visible: true,
////                applyFilter: "auto",
////                allowSearching: true,
////                allowHeaderFiltering: true,

////            },
////    });
////});

    //$.ajax({
    //    type: "GET",
    //    url: encodeURI("../InventoryLab/GetHWData"),
    //    //data: { },
    //    success: OnSuccess_GetHardwaredata,
    //    error: OnError_GetHWData
    //});

/*$(() => {*/
var datagrid
const uom= [
    'EA',
    'MTR',
];
const scope = [
    'Supplier',
    'Bosch',
];

$.ajax({
    type: "GET",
    url: encodeURI("../InventoryLab/checkAuth"),
    async: false,
    success: function (data) {
        debugger;
        if (data.success) {
            debugger;
            if (data.ispmtlnventory == "1") {
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

            
/*            InvAuth = true;*/
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

$.ajax({
    type: "GET",
    url: encodeURI("../InventoryPMT/getBOMDatabase"),
    //data: { },
    success: OnSuccess_getBOMDatabase,
    error: function () {
        alert("Error");
    }
});

function OnSuccess_getBOMDatabase(response) {
    debugger;



    datagrid = $("#BOMDatabaseTable").dxDataGrid({
        

        dataSource: response.data,

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
            groupPanel: {
                visible: true,
                placeholder: "Group By Panel",
            },
            grouping: {
                autoExpandAll: true,
            },
            repaintChangesOnly: true,
            columnFixing: {
                enabled: true
            },
            columnChooser: {
                enabled: true
            },
            export: {
                enabled: true,

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
            width: "96vw", //needed to allow horizontal scroll
            height: "75vh",
            columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
             columnAutoHeight: true,
             showScrollbar: 'always',
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
                     {

                         /*location: 'after',*/
                         widget: 'dxSelectBox',
                         location: 'before',
                         /*locateInMenu: 'always',*/
                         options: {
                             searchEnabled: true,
                             placeholder: 'Product Search',
                             width:250,
                         },

                     },
                   'addRowButton',
                   
                     'columnChooserButton',
                     'exportButton'                     ,
                /*'searchPanel',*/

            ]
        },
        onExporting(e) {
                const workbook = new ExcelJS.Workbook();
                const worksheet = workbook.addWorksheet('BOMDatabaseDetails');

                worksheet.columns = [

                    { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 },

                ];




                DevExpress.excelExporter.exportDataGrid({
                    component: e.component,
                    worksheet,
                    autoFilterEnabled: true,
                }).then(() => {
                    workbook.xlsx.writeBuffer().then((buffer) => {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'BOMDatabaseDetails.xlsx');
                    });
                });
                e.cancel = true;
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
                    dataField: 'Product',
                    //width: 70,
                    caption: 'Product',
                    //validationRules: [{
                    //    type: "required",
                    //    message: "Bond No. is required"
                    //}]
                },
                {
                    dataField: 'ComponentName',
                    //width: 70,
                    caption: 'Component Name',
                },
                {
                    dataField: 'Group',
                    //width: 70,
                    caption: 'Group',
                },
                {
                    dataField: 'Subgroup',
                    //width: 70,
                    caption: 'Subgroup',
                },
                {
                    dataField: 'UOM',
                    //width: 70,
                    caption: 'UOM',
                    lookup: {
                        dataSource: uom,
                    }
                },
                {
                    dataField: 'PartNo',
                    //width: 70,
                    caption: 'PartNo',
                },
                {
                    dataField: 'Quantity',
                    //width: 70,
                    caption: 'Quantity',
                },
                {
                    dataField: 'Make',
                    //width: 70,
                    caption: 'Make',
                },
                {
                    dataField: 'ScopeOfMaterial',
                    //width: 70,
                    caption: 'Scope Of Material',
                    lookup: {
                        dataSource: scope,
                    }
                },

            ],
            onRowInserting: function (e) {



            },
            onRowUpdated: function (e) {
    

            },
            onRowRemoving: function (e) {
      

            },
            //To add validation check if all the fields are filled or not
            onRowValidating: function (e) {
     
            }

        }).dxDataGrid('instance').refresh();



    }

/*});*/
