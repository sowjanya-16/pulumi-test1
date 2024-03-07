// MAIN CODE

//$(document).ready(function () {
//    showgrid();
//});

var modflag, delflag, InvAuth;

$.ajax({
    type: "GET",
    url: encodeURI("../InventoryLab/checkAuth"),
    async: false,
    success: function (data) {
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

            //InvAuth = true;
            //alert("Success");
        }
        else {
            debugger;

            modflag = false;
            delflag = false;
            //alert("Error");
        }
            //alert("Success");
        
    },
    error: function (e) {
        alert("Error getting data");
    },
});





// AJAX Connection and CRUD Operations 


function showgrid() {
    $.ajax(
        {

            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/InventoryLab/GetLabCarDetails",
            datatype: "json",
            //async: true,
            success: function (data) {
                debugger;
                if (data.success == false) {
                    $.notify(data.message,
                        {
                            globalPosition: "top center",
                            className: "error"
                        });
                }
                else {
                    if (data.data.length > 0) {
                        LoadDataGrid(data.data);
                    }
                    else {
                        alert('error');
                    }
                }
            },
            error: function (jqXHR, exception) {
                alert('error');
            }

        });

}

//LoadData

function LoadDataGrid(data) {
    debugger;
    $("#gridContainer").dxDataGrid({
        dataSource: data,


        /* Modification */

        allowSorting: true,
        headerFilter: {
            visible: true,
            applyFilter: "auto"
        },


        export: {
            enabled: true,

        },

        allowColumnReordering: true,
        allowColumnResizing: true,
        /*
        scrolling: {
            columnRenderingMode: 'virtual',
            mode: "virtual",
            rowRenderingMode: "virtual",
        },

        */

        hoverStateEnabled:
        {
            enabled: true
        },

        filterRow: {
            visible: true
        },

        selection: {
            applyFilter: "auto"
        },

        paging: {
            pageSize: 15,
        },


        //pager: {
        //    visible: true,
        //    allowedPageSizes: [10, 'all'],
        //    showPageSizeSelector: true,
        //    showInfo: true,
        //    showNavigationButtons: true,
        //},



        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search...",
            //highlightCaseSensitive: true,
        },


        loadPanel: {
            enabled: true
        },

        columnChooser: {
            enabled: true
        },
        editing:
        {
            mode: 'row',
            allowUpdating: modflag,
            allowAdding: modflag,
            allowDeleting: delflag,

            useIcons: true,
            mode: 'popup',
            popup:
            {
                title: 'LabCar Details Info',
                showTitle: true,
                width: 1300,
                height: 600,
            },


            // Pop Up Form 

            form:
            {
                items:
                    [
                        // Group 1 - Lab Car Information
                        {
                            itemType: 'group',
                            colCount: 2,
                            colSpan: 2,
                            caption: 'Lab Car Information',
                            items:
                                [
                                    'Location',
                                    'LCNumber',
                                    'Type',
                                    'BondNo',
                                    'BondDate',
                                    'AssetNo',
                                    'InputSupply',
                                    'PCAssetNumber',
                                    'MonitorAssetNumber1',
                                    'MonitorAssetNumber2',
                                    'RTPCProcessorType',
                                    'RTPCManufacturer',
                                    'RTPCCards',
                                    'EB5200SerialNumber',
                                    'IB600SerialNumber',
                                    'IB200SerialNumber',
                                    'LDUEMU',
                                    'VSC',
                                    'WSS2',
                                    'LDUAECU',
                                    'HSPlusInterfaceSerialNumber',
                                    'HSXInterfaceSerialNumber',
                                ],
                        },


                        // Group 2 - Software Information


                        {
                            itemType: 'group',
                            colCount: 2,
                            colSpan: 2,
                            caption: 'Software Information (Installed in PC)',
                            items:
                                [
                                    'SoftwareLicenseName',
                                    'SoftwareLicenseVersion',
                                ],

                        },


                        // Group 3 - Hardware Information

                        {
                            itemType: 'group',
                            colCount: 2,
                            colSpan: 2,
                            caption: 'Hardware Information',
                            items:
                                [

                                    'HardwareModel',
                                    'HardwareSerialNumber',
                                    'HardwareLicenseName',
                                    'HardwareLicenseVersion',
                                    'HardwareLicenseSerialNumber',

                                    'HardwareLicenseNameOptional_1',
                                    'HardwareLicenseVersionOptional_1',
                                    'HardwareLicenseSerialNumberOptional_1',
                                    'HardwareLicenseNameOptional_2',
                                    'HardwareLicenseVersionOptional_2',
                                    'HardwareLicenseSerialNumberOptional_2',

                                    'MeasurementInterfaceModel',
                                    'MeasurementInterfaceSerialNumber',
                                    'Motsim',
                                    'PowerSupply'
                                ],

                        }

                    ],

            },
        },

        width: "85vw", //needed to allow horizontal scroll
        height: "75vh",
        columnMinWidth: 50,
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        remoteOperations: { groupPaging: true },
        showColonAfterLabel: true,
        showValidationSummary: true,
        columnFixing: {
            enabled: true
        },



        columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
        //remoteOperations: true,


        groupPanel: {
            visible: true,
            placeholder: "Group By Panel",
        },
        grouping: {
            autoExpandAll: true,
        },

        onToolbarPreparing: function (e) {
            var dataGrid = e.component;

            e.toolbarOptions.items[0].showText = 'always';


        },

        repaintChangesOnly: true,

        showBorders: true,


        headerFilter: {
            visible: true,
            applyFilter: "auto",
            allowSearching: true,
            allowHeaderFiltering: true,

        },

       

        valueChangeEvent: 'keyup',




        // Exporting to Excel File using the Described Name Function

        onExporting(e) {
            const workbook = new ExcelJS.Workbook();
            const worksheet = workbook.addWorksheet('LabCarDetails');

            worksheet.columns = [

                { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 }, { width: 30 },

            ];




            DevExpress.excelExporter.exportDataGrid({
                component: e.component,
                worksheet,
                autoFilterEnabled: true,
            }).then(() => {
                workbook.xlsx.writeBuffer().then((buffer) => {
                    saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'LabCarDetails.xlsx');
                });
            });
            e.cancel = true;
        },


        columns:
            [
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
                //PART 1
                {
                    caption: '',
                    innerHeight: 40,
                    columns:
                        [

                            {

                                caption: "Location",
                                dataField: "Location",
                                width: 130,
                                area: "row",
                                allowEditing: true,
                                allowFiltering: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],

                                setCellValue(rowData, value) {
                                    rowData.Location = value;
                                },

                                lookup: {
                                    dataSource: locations,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },

                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }


                            },

                            {

                                caption: "LC Number",
                                width: 180,
                                dataField: "LCNumber",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],

                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },

                            {
                                caption: "Type",
                                dataField: "Type",
                                width: 162,
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.Type = value;
                                },

                                lookup: {
                                    dataSource: types,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],

                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }

                            },

                            {

                                caption: "Bond Number",
                                width: 186,
                                dataField: "BondNo",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            

                            {
                                dataField: 'BondDate',
                                caption: 'Bond Date',
                                dataType: 'date',
                                format: "yyyy-MM-dd",
                                width: 160,

                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                
                                
                                validationRules: [{
                                    type: "required",
                                    message: "Bond Date is required"
                                }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            {

                                caption: "Asset Number",
                                width: 198,
                                dataField: "AssetNo",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],

                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },

                            


                            {

                                caption: "Input Supply",
                                width: 170,
                                dataField: "InputSupply",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.InputSupply = value;
                                },

                                lookup: {
                                    dataSource: inputsupplies,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],

                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }

                            },


                            {

                                caption: "PC Asset Number",
                                width: 187,
                                dataField: "PCAssetNumber",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],

                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            {

                                caption: "Monitor Asset Number1",
                                width: 245,
                                dataField: "MonitorAssetNumber1",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],

                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },



                            {

                                caption: "Monitor Asset Number2",
                                width: 245,
                                dataField: "MonitorAssetNumber2",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],

                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }

                            },



                            {

                                caption: "RTPC Processor Type",
                                width: 240,
                                dataField: "RTPCProcessorType",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.RTPCProcessorType = value;
                                },
                                lookup:
                                {
                                    dataSource: rtpcprocessortypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],

                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },




                            {

                                caption: "RTPC Manufacturer",
                                width: 240,
                                dataField: "RTPCManufacturer",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.RTPCManufacturer = value;
                                },
                                lookup:
                                {
                                    dataSource: rtpcmanufacturertypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            {

                                caption: "RTPC Cards",
                                width: 200,
                                dataField: "RTPCCards",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                selection: { mode: 'multiple' },
                                setCellValue(rowData, value) {
                                    rowData.RTPCCards = value;
                                },
                                lookup:
                                {
                                    dataSource: rtpccardstypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },



                            {

                                caption: "EB5200 SerialNumber",
                                width: 235,
                                dataField: "EB5200SerialNumber",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },



                            {

                                caption: "IB600 SerialNumber",
                                width: 235,
                                dataField: "IB600SerialNumber",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            {

                                caption: "IB200 SerialNumber",
                                width: 235,
                                dataField: "IB200SerialNumber",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },



                            {

                                caption: "LDU EMU",
                                width: 190,
                                dataField: "LDUEMU",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.LDUEMU = value;
                                },
                                lookup:
                                {
                                    dataSource: lduemutypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            {

                                caption: "VSC",
                                width: 125,
                                dataField: "VSC",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.VSC = value;
                                },
                                lookup:
                                {
                                    dataSource: vsctypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },



                            {

                                caption: "WSS2",
                                width: 135,
                                dataField: "WSS2",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.WSS2 = value;
                                },
                                lookup:
                                {
                                    dataSource: wss2types,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },



                            {

                                caption: "LDU AECU",
                                width: 145,
                                dataField: "LDUAECU",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.LDUAECU = value;
                                },
                                lookup:
                                {
                                    dataSource: lduaecutypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            {

                                caption: "HS+ Interface SerialNumber",
                                width: 265,
                                dataField: "HSPlusInterfaceSerialNumber",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            {

                                caption: "HSX Interface SerialNumber",
                                width: 245,
                                dataField: "HSXInterfaceSerialNumber",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },
                        ],

                },

                // Part 2 ( Vector)
                {
                    caption: 'VECTOR',
                    alignment: "center",
                    headerCellTemplate: function (container, options) {
                        $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                    },
                    columns:
                        [
                            {

                                caption: "Software License Name",
                                width: 220,
                                dataField: "SoftwareLicenseName",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.SoftwareLicenseName = value;
                                },
                                lookup:
                                {
                                    dataSource: softwarelicensenametypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            {

                                caption: "Software License Version",
                                width: 240,
                                dataField: "SoftwareLicenseVersion",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.SoftwareLicenseVersion = value;
                                },
                                lookup:
                                {
                                    dataSource: softwarelicenseversiontypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            {

                                caption: "Hardware Model",
                                width: 160,
                                dataField: "HardwareModel",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.HardwareModel = value;
                                },
                                lookup:
                                {
                                    dataSource: hardwaremodeltypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },

                            {

                                caption: "Hardware SerialNumber",
                                width: 210,
                                dataField: "HardwareSerialNumber",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            {

                                caption: "Hardware License Name",
                                width: 230,
                                dataField: "HardwareLicenseName",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.HardwareLicenseName = value;
                                },
                                lookup:
                                {
                                    dataSource: hardwarelicensenametypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },



                            {

                                caption: "Hardware License Version",
                                width: 230,
                                dataField: "HardwareLicenseVersion",
                                area: "row",
                                alignment: "center",
                                allowEditing: true,
                                expanded: true,
                                setCellValue(rowData, value) {
                                    rowData.HardwareLicenseVersion = value;
                                },
                                lookup:
                                {
                                    dataSource: hardwarelicenseversiontypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            {

                                caption: "Hardware License SerialNumber",
                                width: 270,
                                dataField: "HardwareLicenseSerialNumber",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            //optionals


                            {

                                caption: "Hardware License Name",
                                width: 270,
                                dataField: "HardwareLicenseNameOptional_1",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.HardwareLicenseNameOptional_1 = value;
                                },
                                lookup:
                                {
                                    dataSource: hardwarelicensenameoptional1types,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }

                            },



                            {

                                caption: "Hardware License Version",
                                width: 300,
                                dataField: "HardwareLicenseVersionOptional_1",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.HardwareLicenseVersionOptional_1 = value;
                                },
                                lookup:
                                {
                                    dataSource: hardwarelicenseversionoptional1types,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }

                            },

                            {

                                caption: "Hardware License SerialNumber",
                                width: 270,
                                dataField: "HardwareLicenseSerialNumberOptional_1",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }

                            },



                            {

                                caption: "Hardware License Name",
                                width: 295,
                                dataField: "HardwareLicenseNameOptional_2",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },





                            {

                                caption: "Hardware License Version",
                                width: 300,
                                dataField: "HardwareLicenseVersionOptional_2",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.HardwareLicenseVersionOptional_2 = value;
                                },
                                lookup:
                                {
                                    dataSource: hardwarelicenseversionoptional2types,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },


                            {

                                caption: "Hardware License SerialNumber",
                                width: 320,
                                dataField: "HardwareLicenseSerialNumberOptional_2",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },



                            {

                                caption: "Measurement Interface Model",
                                width: 230,
                                dataField: "MeasurementInterfaceModel",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.MeasurementInterfaceModel = value;
                                },
                                lookup:
                                {
                                    dataSource: measurementinterfacemodeltypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },



                            {

                                caption: "Measurement Interface SerialNumber",
                                width: 280,
                                dataField: "MeasurementInterfaceSerialNumber",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },



                            {

                                caption: "Motsim",
                                width: 148,
                                dataField: "Motsim",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.Motsim = value;
                                },
                                lookup:
                                {
                                    dataSource: motismtypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }

                            },


                            {
                                caption: "Power Supply",
                                width: 190,
                                dataField: "PowerSupply",
                                area: "row",
                                allowEditing: true,
                                expanded: true,
                                alignment: "center",
                                setCellValue(rowData, value) {
                                    rowData.PowerSupply = value;
                                },
                                lookup:
                                {
                                    dataSource: powersupplytypes,
                                    displayExpr: 'Name',
                                    valueExpr: 'Name',
                                },
                                validationRules: [{ type: 'required' }],
                                headerCellTemplate: function (container, options) {
                                    $("<div>").append(options.column.caption).css("color", "#000000").appendTo(container);
                                }
                            },
                        ]
                }
            ],

        

        

        
        // Function for Insertion

        onRowInserted: function (e) {
            debugger;
            Selected = [];
            Selected.push(e.data);
            InsertData(Selected);
        },



        // Function for Updation


        onRowUpdated: function (e) {
            debugger;
            Selected = [];
            Selected.push(e.data);
            UpdateData(Selected);
        },


        // Function for Removing a Row in Data 

        onRowRemoved: function (e) {
            debugger;
            Selected = [];
            Selected.push(e.data);
            DeleteData(Selected);
        },



        


        // Editing Option + Pop up Form 

        //editing:
        //{
        //    mode: 'row',
        //    allowUpdating: true,
        //    allowAdding: true,
        //    allowDeleting: true,

        //    useIcons: true,
        //    mode: 'popup',
        //    popup:
        //    {
        //        title: 'LabCar Details Info',
        //        showTitle: true,
        //        width: 1300,
        //        height: 600,
        //    },


        //    // Pop Up Form 

        //    form:
        //    {
        //        items:
        //            [
        //                // Group 1 - Lab Car Information
        //                {
        //                    itemType: 'group',
        //                    colCount: 2,
        //                    colSpan: 2,
        //                    caption: 'Lab Car Information',
        //                    items:
        //                        [
        //                            'Location',
        //                            'LCNumber',
        //                            'Type',
        //                            'BondNo',
        //                            'BondDate',
        //                            'AssetNo',
        //                            'InputSupply',
        //                            'PCAssetNumber',
        //                            'MonitorAssetNumber1',
        //                            'MonitorAssetNumber2',
        //                            'RTPCProcessorType',
        //                            'RTPCManufacturer',
        //                            'RTPCCards',
        //                            'EB5200SerialNumber',
        //                            'IB600SerialNumber',
        //                            'IB200SerialNumber',
        //                            'LDUEMU',
        //                            'VSC',
        //                            'WSS2',
        //                            'LDUAECU',
        //                            'HSPlusInterfaceSerialNumber',
        //                            'HSXInterfaceSerialNumber',
        //                        ],
        //                },


        //                // Group 2 - Software Information


        //                {
        //                    itemType: 'group',
        //                    colCount: 2,
        //                    colSpan: 2,
        //                    caption: 'Software Information (Installed in PC)',
        //                    items:
        //                        [
        //                            'SoftwareLicenseName',
        //                            'SoftwareLicenseVersion',
        //                        ],

        //                },


        //                // Group 3 - Hardware Information

        //                {
        //                    itemType: 'group',
        //                    colCount: 2,
        //                    colSpan: 2,
        //                    caption: 'Hardware Information',
        //                    items:
        //                        [

        //                            'HardwareModel',
        //                            'HardwareSerialNumber',
        //                            'HardwareLicenseName',
        //                            'HardwareLicenseVersion',
        //                            'HardwareLicenseSerialNumber',

        //                            'HardwareLicenseNameOptional_1',
        //                            'HardwareLicenseVersionOptional_1',
        //                            'HardwareLicenseSerialNumberOptional_1',
        //                            'HardwareLicenseNameOptional_2',
        //                            'HardwareLicenseVersionOptional_2',
        //                            'HardwareLicenseSerialNumberOptional_2',

        //                            'MeasurementInterfaceModel',
        //                            'MeasurementInterfaceSerialNumber',
        //                            'Motsim',
        //                            'PowerSupply'
        //                        ],

        //                }

        //                ],

        //    },
        //},



        




    }).dxDataGrid('instance').refresh();
}


function getBondYearMonth(data) {
    var date = new Date(data.BondDate);
    return new Date(date.getFullYear(), date.getMonth(), 1);
}



//InsertData

function InsertData(id1) {
    debugger;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/InventoryLab/InsertLabCarDetails",
        datatype: "json",
        data: JSON.stringify({ item: id1[0] }),
        success: function (data) {
            //alert("Data Inserted");
            $.notify(" The details are being added...Please wait!", {
                globalPosition: "top center",
                className: "success"
            });

            $.notify("Saved successfully!", {
                globalPosition: "top center",
                className: "success"
            });

        },
        error: function (jqXHR, exception) {
            debugger;
            $.notify("Unable to add the details! ", {
                globalPosition: "top center",
                className: "warn"
            })
        },
    });
}






//UpdateData


function UpdateData(id1) {
    debugger;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/InventoryLab/UpdateLabCarDetails",
        datatype: "json",
        data: JSON.stringify({ item: id1[0] }),
        success: function (data) {

            $.notify(" The details are being updated...Please wait!", {
                globalPosition: "top center",
                className: "success"
            });

            $.notify("Updated successfully!", {
                globalPosition: "top center",
                className: "success"
            });



        },
        error: function (jqXHR, exception) {
            debugger;
            $.notify("Unable to update the details! ", {
                globalPosition: "top center",
                className: "warn"
            })
        },
    });
}





//Delete Data

function DeleteData(id1) {
    debugger;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/InventoryLab/DeleteLabCarDetails",
        datatype: "json",
        data: JSON.stringify({ item: id1[0] }),
        success: function (data) {
            $.notify("Deleted successfully", {
                globalPosition: "top center",
                className: "success"
            });


        },
        error: function (jqXHR, exception) {
            debugger;
            $.notify("Unable to delete the details! ", {
                globalPosition: "top center",
                className: "warn"
            });
        },
    });
}





//LookUps For All Options

const inputsupplies = [
    {
        ID: 1,
        Name: '1PH',
    }, {
        ID: 2,
        Name: '3PH',
    }];



const types = [
    {
        ID: 1,
        Name: 'CCHIL',
    }, {
        ID: 2,
        Name: 'ET',
    }];



const rtpcprocessortypes = [
    {
        ID: 1,
        Name: 'i7',
    }, {
        ID: 2,
        Name: 'i5',
    }];



const rtpcmanufacturertypes = [
    {
        ID: 1,
        Name: 'Bechtle',
    }, {
        ID: 2,
        Name: 'ETAS',
    }];



const lduemutypes = [
    {
        ID: 1,
        Name: 'YES',
    }, {
        ID: 2,
        Name: 'NO',
    }];



const vsctypes = [
    {
        ID: 1,
        Name: 'YES',
    }, {
        ID: 2,
        Name: 'NO',
    }];



const wss2types = [
    {
        ID: 1,
        Name: 'YES',
    }, {
        ID: 2,
        Name: 'NO',
    }];



const lduaecutypes = [
    {
        ID: 1,
        Name: 'YES',
    }, {
        ID: 2,
        Name: 'NO',
    }];



const softwarelicensenametypes = [
    {
        ID: 1,
        Name: 'CANoe',
    }, {
        ID: 2,
        Name: 'CANalyzer',
    }];



const hardwarelicensenametypes = [
    {
        ID: 1,
        Name: 'CANoe',
    }, {
        ID: 2,
        Name: 'CANalyzer',
    }];



const softwarelicenseversiontypes = [
    {
        ID: 1,
        Name: 10,
    },
    {
        ID: 2,
        Name: 11,
    },

    {
        ID: 3,
        Name: 12,
    },

    {
        ID: 4,
        Name: 13,
    },

    {
        ID: 5,
        Name: 14,
    },


    {
        ID: 6,
        Name: 15,
    },

    {
        ID: 7,
        Name: 16,
    },
];




const hardwarelicenseversiontypes = [
    {
        ID: 1,
        Name: 10,
    },
    {
        ID: 2,
        Name: 11,
    },

    {
        ID: 3,
        Name: 12,
    },

    {
        ID: 4,
        Name: 13,
    },

    {
        ID: 5,
        Name: 14,
    },


    {
        ID: 6,
        Name: 15,
    },

    {
        ID: 7,
        Name: 16,
    },
];



const rtpccardstypes = [
    {
        ID: 1,
        Name: 'IB200',
    }, {
        ID: 2,
        Name: 'IB600',
    },
    {
        ID: 3,
        Name: 'EB5200',
    },
    {
        ID: 4,
        Name: 'IB200 & IB600',
    },

    {
        ID: 5,
        Name: 'IB200 & EB5200',
    },

    {
        ID: 6,
        Name: 'IB600 & EB5200',
    },

    {
        ID: 7,
        Name: 'IB200 & IB600 & EB5200',
    },
];



const hardwaremodeltypes = [
    {
        ID: 1,
        Name: 'CancaseXL',
    }, {
        ID: 2,
        Name: 'VN1610',
    },
    {
        ID: 3,
        Name: 'VN1630',
    },
    {
        ID: 4,
        Name: 'VN1630A',
    },
    {
        ID: 5,
        Name: 'VN7600',
    },
    {
        ID: 6,
        Name: 'VN7610',
    },
    {
        ID: 7,
        Name: 'VN7640',
    },
    {
        ID: 8,
        Name: 'VN8914',
    },
    {
        ID: 9,
        Name: 'VN8974',
    },
    {
        ID: 10,
        Name: 'VN8910',
    },
    {
        ID: 11,
        Name: 'VN8910A',
    },
];



const locations = [
    {
        ID: 1,
        Name: 'COB',
    }, {
        ID: 2,
        Name: 'BAN',
    }];




const hardwarelicensenameoptional1types = [
    {
        ID: 1,
        Name: 'Option.Flexray',
    }, {
        ID: 2,
        Name: 'Option.LIN',
    },

    {
        ID: 3,
        Name: 'Option.Ethernet',
    },

    {
        ID: 4,
        Name: 'Option.Diva',
    },

    {
        ID: 5,
        Name: 'XCP',
    },

];



const hardwarelicenseversionoptional1types = [
    {
        ID: 1,
        Name: 10,
    },
    {
        ID: 2,
        Name: 11,
    },

    {
        ID: 3,
        Name: 12,
    },

    {
        ID: 4,
        Name: 13,
    },

    {
        ID: 5,
        Name: 14,
    },


    {
        ID: 6,
        Name: 15,
    },

    {
        ID: 7,
        Name: 16,
    },

    {
        ID: 8,
        Name: 20,
    },
];



const hardwarelicenseversionoptional2types = [
    {
        ID: 1,
        Name: 10,
    },
    {
        ID: 2,
        Name: 11,
    },

    {
        ID: 3,
        Name: 12,
    },

    {
        ID: 4,
        Name: 13,
    },

    {
        ID: 5,
        Name: 14,
    },


    {
        ID: 6,
        Name: 15,
    },

    {
        ID: 7,
        Name: 16,
    },

    {
        ID: 8,
        Name: 20,
    },
];




const measurementinterfacemodeltypes = [
    {
        ID: 1,
        Name: 'VX1131',
    }, {
        ID: 2,
        Name: 'VX1132',
    },
    {
        ID: 3,
        Name: 'VX1135',
    },
];




const motismtypes = [
    {
        ID: 1,
        Name: 'YES',
    }, {
        ID: 2,
        Name: 'NO',
    }];



const powersupplytypes = [
    {
        ID: 1,
        Name: 'SINGLE',
    }, {
        ID: 2,
        Name: 'DUAL',
    }];


