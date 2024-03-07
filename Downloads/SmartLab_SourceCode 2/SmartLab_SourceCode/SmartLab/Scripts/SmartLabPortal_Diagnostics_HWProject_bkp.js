//Javascript file for HW/Project Details - mae9cob
var labname, locations;


$.notify('Hello! Here you can find HW/Project info tables and Year on Year LC count charts', {
    globalPosition: "top center",
    className: "info"
});



$(document).ready(function () {
    //Initially disable/hide the buttons
    debugger;
    $("#selectLabId").prop('disabled', true);
    $(".selectpicker").selectpicker('refresh');
    debugger;
    $("#gen").prop('disabled', true);
    $("#exportContainer").prop('hidden', true);
});
debugger;
$('#StartTime').change(function () {
    validateTime();
    debugger;
});

$('#EndTime').change(function () {
    validateTime();
});

var locations_chosen = [];
function fnfetchSiteChosen() {
    locations = $('#selectSite option:selected').toArray().map(item => item.text).join(); 
}

//Function to check the validity of user entered Dates

function validateTime() {
    debugger;
    // regular expression to match required date format
    re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
    var StartTime = document.getElementById('StartTime').value;
    var EndTime = document.getElementById('EndTime').value;
    var _sTime = Date.parse(StartTime);
    var _eTime = Date.parse(EndTime);


    if (document.getElementById('StartTime').value == '' && document.getElementById('EndTime').value == '') {
        alert("Dates not selected ");
        document.getElementById('StartTime').focus();
        return false;
    }

    else if (document.getElementById('StartTime').value == '') {
        alert("Start Date not selected ");
        document.getElementById('StartTime').focus();
        return false;
    }

    else if (document.getElementById('EndTime').value == '') {
        alert("End Date not selected ");
        document.getElementById('EndTime').focus();
        return false;
    }

    else if (document.getElementById('StartTime').value != '' && !document.getElementById('StartTime').value.match(re)) {
        alert("Invalid date format: " + document.getElementById('StartTime').value);
        document.getElementById('StartTime').focus();
        return false;
    }

    else if (document.getElementById('EndTime').value != '' && !document.getElementById('EndTime').value.match(re)) {
        alert("Invalid time format: " + document.getElementById('EndTime').value);
        document.getElementById('EndTime').focus();
        return false;
    }

    else if (_sTime > _eTime) {
        alert("Inappropriate Selection of Dates");
        document.getElementById('StartTime').focus();
    }

    else
        return true;

}


var sTime;
var eTime;

//Function to validate the user inputs
function validatepage() {
    if ((document.getElementById('selectLabId').selectedIndex > -1) && validateTime()) {
        sTime = StartTime.value;
        eTime = EndTime.value;
        $("#gen").prop('disabled', false);
        $("#exportContainer").prop('hidden', true);
        return true;
    }
    else {
        return false;
    }
}




// Add LabIDs chosen by user to labids_chosen array
var labids_chosen = [];
//Onchange function of LabID
function fnLabIDChange(labidselect) {
    labids_chosen = [];

    for (var i = 0, len = labidselect.options.length; i < len; i++) {
        if (document.getElementById('selectLabId').selectedIndex != -1) {
            options = labidselect.options;
            opt = options[i];
            if (opt.selected) {
                //store the labids chosen by user from dropdown to process the relevant chart data
                labids_chosen.push(opt.value);
            }
        }
    }
    validatepage();
}



//Onclick of Get LC Info button
$("#gen").click(function () {

    //Hide Datagrid region till the new data is processed and prepared
    $("#gridContainer_d1").prop('hidden', true);
    $("#gridContainer_d2").prop('hidden', true);
    $("#gridContainer_d3").prop('hidden', true);
    $("#gridContainer_d4").prop('hidden', true);
    $("#gridContainer_d5").prop('hidden', true);
    $("#gridContainer_d6").prop('hidden', true);
    $("#gridContainer_d7").prop('hidden', true);
    $("#gridContainer_d8").prop('hidden', true);
    $("#gridContainer_d9").prop('hidden', true);
    $("#gridContainer_d10").prop('hidden', true);
    $("#gridContainer_d11").prop('hidden', true);
    $("#gridContainer_d12").prop('hidden', true);

    //Notification for user that DataGrid will be displayed here
    $.notify("Expected Tables will be loaded here", {
        className: "success",
        globalPosition: "top center",
        autoHideDelay: 7000
    });

    $.ajax({
        type: "POST",
        url: encodeURI("../Diagnostics/GetPCnamebyLabID"),
        data: { 'labid_to_pcname': labids_chosen },
        success: OnSuccess0,
        error: OnErrorCall0
    });
});



function OnSuccess0(lstPCs) {
    if (lstPCs.data.length != 0) {
        //create a variable labname 
        //var labname = [];
        //labname = $("#selectLabId").text();
        //labname = $("#selectLabId option:selected").html();

        labname = $('#selectLabId option:selected').toArray().map(item => item.text).join(); //"LC002,LC003,LC005" - string
        var labname_appended = Array.from(new Set(labname.split(','))); 


        $.ajax({
            type: "POST",
            url: encodeURI("../Diagnostics/GetTABLE1_HWDescData"),
            data: { 'pc': lstPCs.data, 'sdate': sTime, 'edate': eTime },
            success: OnSuccess_d1,
            error: OnErrorCall_d1
        });
        debugger;


        $.ajax({
            type: "POST",
            url: encodeURI("../Diagnostics/TABLE2_GetPrjDescData"),
            data: { 'pc': lstPCs.data, 'sdate': sTime, 'edate': eTime },
            success: OnSuccess_d2,
            error: OnErrorCall_d2
        });
        debugger;

        $.ajax({
            type: "POST",
            url: encodeURI("../Diagnostics/GetTABLE3_APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1Data"),
            data: { 'pc': lstPCs.data, 'sdate': sTime, 'edate': eTime },
            success: OnSuccess_d3,
            error: OnErrorCall_d3
        });


        $.ajax({
            type: "POST",
            url: encodeURI("../Diagnostics/GetTABLE4_OTSO_BOB1_Cable1_PowerSupply_HAP_IXXATData"),
            data: { 'pc': lstPCs.data, 'sdate': sTime, 'edate': eTime },
            success: OnSuccess_d4,
            error: OnErrorCall_d4
        });


        $.ajax({
            type: "POST",
            url: encodeURI("../Diagnostics/GetTABLE5_ECC_IBData"),
            data: { 'pc': lstPCs.data, 'sdate': sTime, 'edate': eTime },
            success: OnSuccess_d5,
            error: OnErrorCall_d5
        });


        $.ajax({
            type: "POST",
            url: encodeURI("../Diagnostics/GetTABLE6_GIO1Data"),
            data: { 'pc': lstPCs.data, 'sdate': sTime, 'edate': eTime },
            success: OnSuccess_d6,
            error: OnErrorCall_d6
        });


        $.ajax({
            type: "POST",
            url: encodeURI("../Diagnostics/GetTABLE7_GIO2Data"),
            data: { 'pc': lstPCs.data, 'sdate': sTime, 'edate': eTime },
            success: OnSuccess_d7,
            error: OnErrorCall_d7
        });


        $.ajax({
            type: "POST",
            url: encodeURI("../Diagnostics/GetTABLE8_LDUData"),
            data: { 'pc': lstPCs.data, 'sdate': sTime, 'edate': eTime },
            success: OnSuccess_d8,
            error: OnErrorCall_d8
        });


        $.ajax({
            type: "POST",
            url: encodeURI("../Diagnostics/GetTABLE9_PSCData"),
            data: { 'pc': lstPCs.data, 'sdate': sTime, 'edate': eTime },
            success: OnSuccess_d9,
            error: OnErrorCall_d9
        });



        $.ajax({
            type: "POST",
            url: encodeURI("../Diagnostics/GetTABLE10_VSCData"),
            data: { 'pc': lstPCs.data, 'sdate': sTime, 'edate': eTime },
            success: OnSuccess_d10,
            error: OnErrorCall_d10
        });


        $.ajax({
            type: "POST",
            url: encodeURI("../Diagnostics/GetTABLE11_WSSData"),
            data: { 'pc': lstPCs.data, 'sdate': sTime, 'edate': eTime },
            success: OnSuccess_d11,
            error: OnErrorCall_d11
        });


        $.ajax({
            type: "POST",
            url: encodeURI("../Diagnostics/GetTABLE12_WSS2Data"),
            data: { 'pc': lstPCs.data, 'sdate': sTime, 'edate': eTime },
            success: OnSuccess_d12,
            error: OnErrorCall_d12
        });

        //$.ajax({
        //    type: "POST",
        //    url: encodeURI("../Diagnostics/GetVSCLDUSummaryData"),
        //    data: { 'pc': lstPCs.data, 'sdate': sTime, 'edate': eTime },
        //    success: OnSuccess_d13,
        //    error: OnErrorCall_d13
        //});

    }
    else {
        //notify message

        $.notify('There are no PCs attached to this Lab!', {
            globalPosition: "top center",
            className: "warn"
        });
        var str = "Try Again";
        var result = str.bold();
        genText.innerHTML = result;
        $("#genSpinner").removeClass("fa fa-spinner fa-spin");
    }

}
function OnErrorCall0(response) {
    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");
    $.notify('Error in getting PC names!', {
        globalPosition: "top center",
        className: "warn"
    });
}


var isProjExtracted = false;
var isWSSExtracted = false;
function OnSuccess_d1(response) {
    var objdata = (response.data);
    debugger;
    $("#gridContainer_d1").prop('hidden', false);
    $(document).ready(function () {
        $("#exportContainer").prop('hidden', false);
    });

    $("#gridContainer_d1").dxDataGrid({

        //editing: {
        //    mode: "batch",
        //    allowUpdating: true,
        //    allowDeleting: true,
        //    allowAdding: true
        //},
        allowColumnReordering: true,
        allowColumnResizing: true,
        columnChooser: {
            enabled: true
        },
        columnFixing: {
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

        paging: {
            pageSize: 5
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        dataSource: objdata,
        columns: [
            {
                caption: "HW DETAILS",

                alignment: "center",
                columns: [
                    "LC_Name",
                    {
                        dataField: "System_name",
                        caption: "PC Name"
                    },
                    {
                        dataField: "Date",
                        caption: "Date",
                        dataType: "date"
                    },
                    {
                        dataField: "EEPName",
                        caption: "EEP Name"
                    },
                    {
                        dataField: "EEPBuildDate",
                        caption: "EEP Build Date",
                        dataType: "date"
                    },
                    {
                        dataField: "EEPDatabaseVersion",
                        caption: "EEP DB Version"
                    },
                    {
                        dataField: "RTPCName",
                        caption: "RTPC Name"
                    },
                    {
                        dataField: "RBCCAFVersion",
                        caption: "RBCCAF Version "
                    },
                    {
                        dataField: "ToolChainVersion",
                        caption: "ToolChain Version"
                    },
                    {
                        dataField: "RTPCSoftwareVersion",
                        caption: "RTPC SW Version"
                    },
                    {
                        dataField: "LabCarType",
                        caption: " LC Type"
                    }]
            }],
        //selection: {
        //    mode: "multiple"
        //},
        export: {
            enabled: true,
            fileName: "DiagnosticsGrid_HW Details"
            //allowExportSelectedData: true
        }
    });


}
function OnErrorCall_d1(response) {
    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");

    $.notify('HW error!', {
        globalPosition: "top center",
        className: "warn"
    });
}



function OnSuccess_d2(response) {
    var objdata = (response.data);
    $("#gridContainer_d2").prop('hidden', false);
    debugger;
    $("#gridContainer_d2").dxDataGrid({


        allowColumnReordering: true,
        allowColumnResizing: true,
        columnChooser: {
            enabled: true
        },
        columnFixing: {
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

        paging: {
            pageSize: 5
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        dataSource: objdata,
        columns: [
            {
                caption: "PROJECT DETAILS",
                alignment: "center",

                columns: [
                    "LC_Name",
                    {
                        dataField: "System_name",
                        caption: "PC Name"
                    },
                    {
                        dataField: "Component_name",
                        caption: "Component Name"
                    },
                    {
                        dataField: "Date",
                        caption: "Date",
                        dataType: "date"
                    },
                    {
                        dataField: "Db_Version",
                        caption: "DB Version"
                    },
                    {
                        dataField: "Details",
                        caption: "Details"
                    },
                    {
                        dataField: "Version",
                        caption: "Version"
                    },
                    "ToolVersion",
                    "EMUCable",
                    "AECUCable",
                    {
                        dataField: "MetaEditor_Ver",
                        caption: "MetaEditor Version"
                    },
                    {
                        dataField: "ProjectBuilder_Ver",
                        caption: "ProjectBuilder Version"
                    },
                    {
                        dataField: "Ascet_Ver",
                        caption: "Ascet Version"
                    },
                    {
                        dataField: "ProjectEditor_Ver",
                        caption: "ProjectEditor Version"
                    }




                ]
            }],
        //selection: {
        //    mode: "multiple"
        //},
        export: {
            enabled: true,
            fileName: "DiagnosticsGrid_Project Details"
            //allowExportSelectedData: true
        }


    });
    isProjExtracted = true;
    button_change();
    debugger;


}
function OnErrorCall_d2(response) {
    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");

    $.notify("Project Details error!", {
        globalPosition: "top center",
        className: "warn"
    });
}



function OnSuccess_d3(response) {
    var objdata = (response.data);
    $("#gridContainer_d3").prop('hidden', false);

    $("#gridContainer_d3").dxDataGrid({
        allowColumnReordering: true,
        allowColumnResizing: true,
        columnChooser: {
            enabled: true
        },
        columnFixing: {
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

        paging: {
            pageSize: 5
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        dataSource: objdata,
        columns: [
            {
                caption: "APB_BOB_CABLE_EB_ES4441_1_ES4441_2_HAP1 DETAILS",
                alignment: "center",

                columns: [
                    "LC_Name",
                    {
                        dataField: "System_name",
                        caption: "PC Name"
                    },
                    {
                        dataField: "Date",
                        caption: "Date",
                        dataType: "date"
                    },
                    {
                        dataField: "ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2",
                        caption: "APB EID"
                    },
                    {
                        dataField: "Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber",
                        caption: "BOB SN"
                    },
                    {
                        dataField: "Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision",
                        caption: "BOB Revision"
                    },
                    {
                        dataField: "Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber",
                        caption: "Cable SN"
                    },
                    {
                        dataField: "Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision",
                        caption: "Cable Revision"
                    },
                    {
                        dataField: "Kernel_EB_Cards_EB5100",
                        caption: "EB5100"
                    },
                    {
                        dataField: "Kernel_EB_Cards_EB5200",
                        caption: "EB5200"
                    },
                    {
                        dataField: "Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN",
                        caption: "ES4441 SN"
                    },
                    {
                        caption: "ES4441 FW Version",
                        calculateCellValue: function (rowData) { return rowData.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain + "." + rowData.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor + "." + rowData.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor },
                        allowHeaderFiltering: true,
                        allowSorting: true,
                        allowFiltering: true,
                        allowGrouping: true,
                        allowHiding: true,

                    },
                    {
                        dataField: "Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision",
                        caption: "ES4441 HW Revision"
                    },
                    {
                        dataField: "Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion",
                        caption: "ES4441 HW Version"
                    },
                    {
                        dataField: "Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber",
                        caption: "HAP SNo"
                    }]
            }],
        //selection: {
        //    mode: "multiple"
        //},
        export: {
            enabled: true,
            fileName: "DiagnosticsGrid_APB_BOB_CABLE_EB_ES4441_1_ES4441_2_HAP1 Details"
            //allowExportSelectedData: true
        }
    });


}
function OnErrorCall_d3(response) {
    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");

    $.notify("APB_BOB_CABLE_EB_ES4441_1_ES4441_2_HAP1 DETAILS error!", {
        globalPosition: "top center",
        className: "warn"
    });
}




function OnSuccess_d4(response) {
    var objdata = (response.data);
    $("#gridContainer_d4").prop('hidden', false);

    $("#gridContainer_d4").dxDataGrid({


        allowColumnReordering: true,
        allowColumnResizing: true,
        columnChooser: {
            enabled: true
        },
        columnFixing: {
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

        paging: {
            pageSize: 5
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        //selection: {
        //    mode: "multiple"
        //},
        export: {
            enabled: true,
            fileName: "DiagnosticsGrid_OTSO_BOB1_CABLE1_POWER SUPPLY_HAP_IXXAT2 Details"
            //allowExportSelectedData: true
        },
        dataSource: objdata,
        columns: [

            {
                caption: "OTSO_BOB1_CABLE1_POWER SUPPLY_HAP_IXXAT2 DETAILS",
                alignment: "center",

                columns: [
                    "LC_Name",
                    {
                        dataField: "System_name",
                        caption: "PC Name"
                    },
                    {
                        dataField: "Date",
                        caption: "Date",
                        dataType: "date"
                    },
                    {
                        dataField: "Kernel_Component_IPB_TemperatureReceive_OTSO1Available",
                        caption: "OTSO1"
                    },
                    {
                        dataField: "Kernel_Component_IPB_TemperatureReceive_OTSO2Available",
                        caption: "OTSO2"
                    },
                    {
                        dataField: "Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber",
                        caption: "BOB1 SNo"
                    },
                    {
                        dataField: "Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber",
                        caption: "Cable1 SNo"
                    },
                    {
                        dataField: "Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration",
                        caption: "PS Configuration"
                    },
                    {
                        dataField: "Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU",
                        caption: "PS ECU Factor"
                    },
                    {
                        dataField: "Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR",
                        caption: "PS MR Factor"
                    },
                    {
                        dataField: "Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR",
                        caption: "PS VR Factor"
                    },
                    {
                        dataField: "Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber",
                        caption: "HAP SNo"
                    },
                    {
                        dataField: "Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision",
                        caption: "HAP Revision"
                    },
                    {
                        dataField: "ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config",
                        caption: "IXXAT2 EID"
                    }]
            }]
    });


}
function OnErrorCall_d4(response) {
    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");

    $.notify("OTSO_BOB1_CABLE1_POWER SUPPLY_HAP_IXXAT2 DETAILS error!", {
        globalPosition: "top center",
        className: "warn"
    });

}



function OnSuccess_d5(response) {
    var objdata = (response.data);
    $("#gridContainer_d5").prop('hidden', false);

    $("#gridContainer_d5").dxDataGrid({


        allowColumnReordering: true,
        allowColumnResizing: true,
        columnChooser: {
            enabled: true
        },
        columnFixing: {
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

        paging: {
            pageSize: 5
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        //selection: {
        //    mode: "multiple"
        //},
        export: {
            enabled: true,
            fileName: "DiagnosticsGrid_ECC_IXXAT1 Details"
            //allowExportSelectedData: true
        },
        dataSource: objdata,
        columns:
            [
                {
                    caption: "ECC_IXXAT1 DETAILS",
                    alignment: "center",

                    columns: [
                        "LC_Name",
                        {
                            dataField: "System_name",
                            caption: "PC Name"
                        },
                        {
                            dataField: "Date",
                            caption: "Date",
                            dataType: "date"
                        },

                        {
                            dataField: "Kernel_Loadbox_Card_ECC_CARD_BoardType",
                            caption: "ECC Type"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_ECC_CARD_BoardRevision",
                            caption: "ECC Revision"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY",
                            caption: "ECC Card Version"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_ECC_Serial_No",
                            caption: "ECC SNo"
                        },
                        {
                            caption: "ECC FW Card Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_ECC_Firmware_Card_Major + "." + rowData.Kernel_Loadbox_Card_ECC_Firmware_Card_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true,
                            allowGrouping: true,
                            allowHiding: true
                        },
                        {
                            caption: "ECC FW FPGA Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major + "." + rowData.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true,
                            allowGrouping: true,
                            allowHiding: true
                        },
                        {
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_ECC_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_ECC_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_ECC_Calibration_Year },
                            caption: "ECC Calibration Date",
                            dataType: "date",
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true,
                            allowGrouping: true,
                            allowHiding: true
                        },
                        {
                            caption: "ECC NextCalibration Date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_ECC_Next_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_ECC_Next_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_ECC_Next_Calibration_Year },
                            dataType: "date",
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true,
                            allowGrouping: true,
                            allowHiding: true
                        },
                        {
                            caption: "ECC Creation Date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_ECC_Day_of_Creation + "/" + rowData.Kernel_Loadbox_Card_ECC_Month_of_Creation + "/" + rowData.Kernel_Loadbox_Card_ECC_Year_of_Creation },
                            dataType: "date",
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true,
                            allowGrouping: true,
                            allowHiding: true,
                            allowReordering: true
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_ECC_BuildVersion",
                            caption: "ECC Build Version"
                        },
                        {
                            dataField: "ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config ",
                            caption: "IXXAT1 EID"
                        }]
                }]

    });


}
function OnErrorCall_d5(response) {
    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");

    $.notify("ECC_IXXAT1 error!", {
        globalPosition: "top center",
        className: "warn"
    });
}



function OnSuccess_d6(response) {
    var objdata = (response.data);
    $("#gridContainer_d6").prop('hidden', false);


    $("#gridContainer_d6").dxDataGrid({


        allowColumnReordering: true,
        allowColumnResizing: true,
        columnChooser: {
            enabled: true
        },
        columnFixing: {
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

        paging: {
            pageSize: 5
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        //selection: {
        //    mode: "multiple"
        //},
        export: {
            enabled: true,
            fileName: "DiagnosticsGrid_GIO1 Details"
            //allowExportSelectedData: true
        },
        dataSource: objdata,


        columns:
            [
                {
                    caption: "GIO1 DETAILS",
                    alignment: "center",

                    columns: [
                        "LC_Name",

                        {
                            dataField: "System_name",
                            caption: "PC Name"
                        },
                        {
                            dataField: "Date",
                            caption: "Date",
                            dataType: "date"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_GIO1_CARD_BoardType",
                            caption: "GIO1 Type"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_GIO1_CARD_BoardRevision",
                            caption: "GIO1 Revision"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY",
                            caption: "GIO1 Card Version"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_GIO1_Serial_No",
                            caption: "GIO1 SNo"
                        },
                        {
                            caption: "GIO1 FW Card Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_GIO1_Firmware_Card_Major + "." + rowData.Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowGrouping: true,
                            allowFiltering: true
                        },
                        {
                            caption: "GIO1 FW FPGA Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major + "." + rowData.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowGrouping: true,
                            allowFiltering: true
                        },
                        {
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_GIO1_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_GIO1_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_GIO1_Calibration_Year },
                            caption: "GIO1 Calibration Date",
                            dataType: "date",
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowGrouping: true,
                            allowFiltering: true

                        },
                        {
                            caption: "GIO1 NextCalibration Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_GIO1_Next_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_GIO1_Next_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_GIO1_Next_Calibration_Year },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowGrouping: true,
                            allowFiltering: true

                        },
                        {
                            caption: "GIO1 Creation Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_GIO1_Day_of_Creation + "/" + rowData.Kernel_Loadbox_Card_GIO1_Month_of_Creation + "/" + rowData.Kernel_Loadbox_Card_GIO1_Year_of_Creation },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true,
                            allowGrouping: true,
                            allowHiding: true,
                            allowReordering: true,
                            allowResizing: true,
                            allowSearch: true,
                            autoExpandGroup: true,
                            format: "shortDateShortTime",
                            editorOptions: { type: "datetime" },
                            value: Date


                        },
                        {
                            dataField: "Kernel_Loadbox_Card_GIO1_BuildVersion",
                            caption: "GIO1 Build Version"
                        }]
                }]

    });


}
function OnErrorCall_d6(response) {
    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");


    $.notify("GIO1 error!", {
        globalPosition: "top center",
        className: "warn"
    });

}



function OnSuccess_d7(response) {
    var objdata = (response.data);
    $("#gridContainer_d7").prop('hidden', false);

    $("#gridContainer_d7").dxDataGrid({


        allowColumnReordering: true,
        allowColumnResizing: true,
        columnChooser: {
            enabled: true
        },
        columnFixing: {
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

        paging: {
            pageSize: 5
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        //selection: {
        //    mode: "multiple"
        //},
        export: {
            enabled: true,
            fileName: "DiagnosticsGrid_GIO2 Details"
            //allowExportSelectedData: true
        },
        dataSource: objdata,


        columns:
            [
                {
                    caption: "GIO2 DETAILS",
                    alignment: "center",

                    columns: [
                        "LC_Name",
                        {
                            dataField: "System_name",
                            caption: "PC Name"
                        },
                        {
                            dataField: "Date",
                            caption: "Date",
                            dataType: "date"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_GIO2_CARD_BoardType",
                            caption: "GIO2 Type"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_GIO2_CARD_BoardRevision",
                            caption: "GIO2 Revision"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY",
                            caption: "GIO2 Card Version"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_GIO2_Serial_No",
                            caption: "GIO2 SNo"
                        },
                        {
                            caption: "GIO2 FW Card Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_GIO2_Firmware_Card_Major + "." + rowData.Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true
                        },
                        {
                            caption: "GIO2 FW FPGA Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major + "." + rowData.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true
                        },
                        {
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_GIO2_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_GIO2_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_GIO2_Calibration_Year },
                            caption: "GIO2 Calibration Date",
                            dataType: "date",
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true


                        },
                        {
                            caption: "GIO2 NextCalibration Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_GIO2_Next_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_GIO2_Next_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_GIO2_Next_Calibration_Year },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            caption: "GIO2 Creation Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_GIO2_Day_of_Creation + "/" + rowData.Kernel_Loadbox_Card_GIO2_Month_of_Creation + "/" + rowData.Kernel_Loadbox_Card_GIO2_Year_of_Creation },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            dataField: "Kernel_Loadbox_Card_GIO2_BuildVersion",
                            caption: "GIO2 Build Version"
                        }]
                }]


    });


}
function OnErrorCall_d7(response) {
    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");

    $.notify("GIO2 error!", {
        globalPosition: "top center",
        className: "warn"
    });
}



function OnSuccess_d8(response) {
    var objdata = (response.data);
    $("#gridContainer_d8").prop('hidden', false);

    $("#gridContainer_d8").dxDataGrid({


        allowColumnReordering: true,
        allowColumnResizing: true,
        columnChooser: {
            enabled: true
        },
        columnFixing: {
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

        paging: {
            pageSize: 5
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        //selection: {
        //    mode: "multiple"
        //},
        export: {
            enabled: true,
            fileName: "DiagnosticsGrid_LDU Details"
            //allowExportSelectedData: true
        },
        dataSource: objdata,


        columns:
            [
                {
                    caption: "LDU DETAILS",
                    alignment: "center",

                    columns: [
                        "LC_Name",
                        {
                            dataField: "System_name",
                            caption: "PC Name"
                        },
                        {
                            dataField: "Date",
                            caption: "Date",
                            dataType: "date"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_LDU_CARD_BoardType",
                            caption: "LDU Type"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_LDU_CARD_BoardRevision",
                            caption: "LDU Revision"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY",
                            caption: "LDU Card Version"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_LDU_Serial_No",
                            caption: "LDU SNo"
                        },
                        {
                            caption: "LDU FW Card Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_LDU_Firmware_Card_Major + "." + rowData.Kernel_Loadbox_Card_LDU_Firmware_Card_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true
                        },
                        {
                            caption: "LDU FW FPGA Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major + "." + rowData.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true
                        },
                        {
                            caption: "LDU Calibration Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_LDU_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_LDU_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_LDU_Calibration_Year },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            caption: "LDU NextCalibration Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_LDU_Next_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_LDU_Next_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_LDU_Next_Calibration_Year },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            caption: "LDU Creation Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_LDU_Day_of_Creation + "/" + rowData.Kernel_Loadbox_Card_LDU_Month_of_Creation + "/" + rowData.Kernel_Loadbox_Card_LDU_Year_of_Creation },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            dataField: "Kernel_Loadbox_Card_LDU_BuildVersion",
                            caption: "LDU Build Version"
                        }]
                }]
    });



}
function OnErrorCall_d8(response) {
    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");

    $.notify("LDU error!", {
        globalPosition: "top center",
        className: "warn"
    });
}



function OnSuccess_d9(response) {
    var objdata = (response.data);
    $("#gridContainer_d9").prop('hidden', false);

    $("#gridContainer_d9").dxDataGrid({


        allowColumnReordering: true,
        allowColumnResizing: true,
        columnChooser: {
            enabled: true
        },
        columnFixing: {
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

        paging: {
            pageSize: 5
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        //selection: {
        //    mode: "multiple"
        //},
        export: {
            enabled: true,
            fileName: "DiagnosticsGrid_PSC Details"
            //allowExportSelectedData: true
        },
        dataSource: objdata,


        columns:
            [
                {
                    caption: "PSC DETAILS",
                    alignment: "center",

                    columns: [
                        "LC_Name",
                        {
                            dataField: "System_name",
                            caption: "PC Name"
                        },
                        {
                            dataField: "Date",
                            caption: "Date",
                            dataType: "date"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_PSC_CARD_BoardType",
                            caption: "PSC Type"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_PSC_CARD_BoardRevision",
                            caption: "PSC Revision"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY",
                            caption: "PSC Card Version"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_PSC_Serial_No",
                            caption: "PSC SNo"
                        },
                        {
                            caption: "PSC FW Card Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_PSC_Firmware_Card_Major + "." + rowData.Kernel_Loadbox_Card_PSC_Firmware_Card_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true
                        },
                        {
                            caption: "PSC FW FPGA Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major + "." + rowData.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true
                        },
                        {
                            caption: "PSC Calibration Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_PSC_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_PSC_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_PSC_Calibration_Year },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            caption: "PSC NextCalibration Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_PSC_Next_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_PSC_Next_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_PSC_Next_Calibration_Year },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            caption: "PSC Creation Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_PSC_Day_of_Creation + "/" + rowData.Kernel_Loadbox_Card_PSC_Month_of_Creation + "/" + rowData.Kernel_Loadbox_Card_PSC_Year_of_Creation },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            dataField: "Kernel_Loadbox_Card_PSC_BuildVersion",
                            caption: "PSC Build Version"
                        }]
                }]
    });


}
function OnErrorCall_d9(response) {
    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");

    $.notify("PSC error!", {
        globalPosition: "top center",
        className: "warn"
    });
}



function OnSuccess_d10(response) {
    var objdata = (response.data);
    $("#gridContainer_d10").prop('hidden', false);

    $("#gridContainer_d10").dxDataGrid({


        allowColumnReordering: true,
        allowColumnResizing: true,
        columnChooser: {
            enabled: true
        },
        columnFixing: {
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

        paging: {
            pageSize: 5
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        //selection: {
        //    mode: "multiple"
        //},
        export: {
            enabled: true,
            fileName: "DiagnosticsGrid_VSC Details"
            //allowExportSelectedData: true
        },
        dataSource: objdata,


        columns:
            [
                {
                    caption: "VSC DETAILS",
                    alignment: "center",

                    columns: [
                        "LC_Name",
                        {
                            dataField: "System_name",
                            caption: "PC Name"
                        },
                        {
                            dataField: "Date",
                            caption: "Date",
                            dataType: "date"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_VSC_CARD_BoardType",
                            caption: "VSC Type"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_VSC_CARD_BoardRevision",
                            caption: "VSC Revision"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY",
                            caption: "VSC Card Version"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_VSC_Serial_No",
                            caption: "VSC SNo"
                        },
                        {
                            caption: "VSC FW Card Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_VSC_Firmware_Card_Major + "." + rowData.Kernel_Loadbox_Card_VSC_Firmware_Card_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true
                        },
                        {
                            caption: "VSC FW FPGA Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major + "." + rowData.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true
                        },
                        {
                            caption: "VSC Calibration Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_VSC_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_VSC_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_VSC_Calibration_Year },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            caption: "VSC NextCalibration Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_VSC_Next_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_VSC_Next_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_VSC_Next_Calibration_Year },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            caption: "VSC Creation Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_VSC_Day_of_Creation + "/" + rowData.Kernel_Loadbox_Card_VSC_Month_of_Creation + "/" + rowData.Kernel_Loadbox_Card_VSC_Year_of_Creation },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            dataField: "Kernel_Loadbox_Card_VSC_BuildVersion",
                            caption: "VSC Build Version"
                        }]
                }]
    });


}
function OnErrorCall_d10(response) {
    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");

    $.notify("VSC error!", {
        globalPosition: "top center",
        className: "warn"
    });
}



function OnSuccess_d11(response) {
    var objdata = (response.data);
    $("#gridContainer_d11").prop('hidden', false);

    $("#gridContainer_d11").dxDataGrid({


        allowColumnReordering: true,
        allowColumnResizing: true,
        columnChooser: {
            enabled: true
        },
        columnFixing: {
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

        paging: {
            pageSize: 5
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        //selection: {
        //    mode: "multiple"
        //},
        export: {
            enabled: true,
            fileName: "DiagnosticsGrid_WSS Details"
            //allowExportSelectedData: true
        },
        dataSource: objdata,


        columns:
            [
                {
                    caption: "WSS DETAILS",
                    alignment: "center",

                    columns: [
                        "LC_Name",
                        {
                            dataField: "System_name",
                            caption: "PC Name"
                        },
                        {
                            dataField: "Date",
                            caption: "Date",
                            dataType: "date"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_WSS_CARD_BoardType",
                            caption: "WSS Type"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_WSS_CARD_BoardRevision",
                            caption: "WSS Revision"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY",
                            caption: "WSS Card Version"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_WSS_Serial_No",
                            caption: "WSS SNo"
                        },
                        {
                            caption: "WSS FW Card Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_WSS_Firmware_Card_Major + "." + rowData.Kernel_Loadbox_Card_WSS_Firmware_Card_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true
                        },
                        {
                            caption: "WSS FW FPGA Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major + "." + rowData.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true
                        },
                        {
                            caption: "WSS Calibration Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_WSS_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_WSS_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_WSS_Calibration_Year },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            caption: "WSS NextCalibration Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_WSS_Next_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_WSS_Next_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_WSS_Next_Calibration_Year },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            caption: "WSS Creation Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_WSS_Day_of_Creation + "/" + rowData.Kernel_Loadbox_Card_WSS_Month_of_Creation + "/" + rowData.Kernel_Loadbox_Card_WSS_Year_of_Creation },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            dataField: "Kernel_Loadbox_Card_WSS_BuildVersion",
                            caption: "WSS Build Version"
                        }]
                }]
    });

}
function OnErrorCall_d11(response) {
    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");

    $.notify("WSS error!", {
        globalPosition: "top center",
        className: "warn"
    });
}



function OnSuccess_d12(response) {
    var objdata = (response.data);
    $("#gridContainer_d12").prop('hidden', false);

    $("#gridContainer_d12").dxDataGrid({


        allowColumnReordering: true,
        allowColumnResizing: true,
        columnChooser: {
            enabled: true
        },
        columnFixing: {
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

        paging: {
            pageSize: 5
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        //selection: {
        //    mode: "multiple"
        //},
        export: {
            enabled: true,
            fileName: "DiagnosticsGrid_WSS2 Details"
            //allowExportSelectedData: true
        },
        dataSource: objdata,


        columns:
            [
                {
                    caption: "WSS2 DETAILS",
                    alignment: "center",

                    columns: [
                        "LC_Name",
                        {
                            dataField: "System_name",
                            caption: "PC Name"
                        },
                        {
                            dataField: "Date",
                            caption: "Date",
                            dataType: "date"
                        },

                        {
                            dataField: "Kernel_Loadbox_Card_WSS2_CARD_BoardType",
                            caption: "WSS2 Type"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_WSS2_CARD_BoardRevision",
                            caption: "WSS2 Revision"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY",
                            caption: "WSS2 Card Version"
                        },
                        {
                            dataField: "Kernel_Loadbox_Card_WSS2_Serial_No",
                            caption: "WSS2 SNo"
                        },
                        {
                            caption: "WSS2 FW Card Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_WSS2_Firmware_Card_Major + "." + rowData.Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true
                        },
                        {
                            caption: "WSS2 FW FPGA Version",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major + "." + rowData.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true
                        },
                        {
                            caption: "WSS2 Calibration Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_WSS2_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_WSS2_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_WSS2_Calibration_Year },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            caption: "WSS2 NextCalibration Date",
                            dataType: "date",
                            format: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_WSS2_Next_Calibration_Day + "/" + rowData.Kernel_Loadbox_Card_WSS2_Next_Calibration_Month + "/" + rowData.Kernel_Loadbox_Card_WSS2_Next_Calibration_Year },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true


                        },
                        {
                            caption: "WSS2 Creation Date",
                            dataType: "date",
                            calculateCellValue: function (rowData) { return rowData.Kernel_Loadbox_Card_WSS2_Day_of_Creation + "/" + rowData.Kernel_Loadbox_Card_WSS2_Month_of_Creation + "/" + rowData.Kernel_Loadbox_Card_WSS2_Year_of_Creation },
                            allowHeaderFiltering: true,
                            allowSorting: true,
                            allowFiltering: true

                        },
                        {
                            dataField: "Kernel_Loadbox_Card_WSS2_BuildVersion",
                            caption: "WSS2 Build Version"
                        }]
                }]
    });
    debugger;

    isWSSExtracted = true;
    button_change();
    debugger;
}
function OnErrorCall_d12(response) {
    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");


    $.notify("WSS2 error!", {
        globalPosition: "top center",
        className: "warn"
    });
}


//function OnSuccess_d13(response) {
//    var objdata = (response.data);
//    $("#gridContainer_d13").prop('hidden', false);

//    $("#gridContainer_d13").dxDataGrid({


//        allowColumnReordering: true,
//        allowColumnResizing: true,
//        columnChooser: {
//            enabled: true
//        },
//        columnFixing: {
//            enabled: true
//        },
//        filterRow: {
//            visible: true

//        },
//        headerFilter: {
//            visible: true,
//            applyFilter: "auto"
//        },
//        selection: {

//            applyFilter: "auto"
//        },

//        paging: {
//            pageSize: 100
//        },
//        searchPanel: {
//            visible: true,
//            width: 240,
//            placeholder: "Search..."
//        },
//        //selection: {
//        //    mode: "multiple"
//        //},
//        export: {
//            enabled: true,
//            fileName: "DiagnosticsGrid_VSCLDUSummary Details"
//            //allowExportSelectedData: true
//        },
//        dataSource: objdata,


//        columns:
//            [
//                {
//                    caption: "VSC vs LDU Summary",
//                    alignment: "center",

//                    columns: [
//                        "LC_Name",
//                        {
//                            dataField: "System_name",
//                            caption: "PC Name"
//                        },
//                        {
//                            dataField: "Date",
//                            caption: "Date",
//                            dataType: "date"
//                        },
//                        {
//                            dataField: "VSC_present"
                           
//                        },
//                        {
//                            dataField: "LDU_present"
                           
//                        }
//                       ]
//                }]
//    });
//    var str = "Get LC info";
//    var result = str.bold();
//    genText.innerHTML = result;
//    $("#genSpinner").removeClass("fa fa-spinner fa-spin");


//    $.notify("Datagrids are successfully loaded!", {
//        globalPosition: "top center",
//        className: "success"
//    });


//}
//function OnErrorCall_d13(response) {
//    var str = "Try Again";
//    var result = str.bold();
//    genText.innerHTML = result;
//    $("#genSpinner").removeClass("fa fa-spinner fa-spin");

//    $.notify("VSC vs LDU error!", {
//        globalPosition: "top center",
//        className: "warn"
//    });
   
//}




function button_change() {
    debugger;
    if (isProjExtracted == true && isWSSExtracted == true) {
        var str = "Get LC info";
        var result = str.bold();
        genText.innerHTML = result;
        $("#genSpinner").removeClass("fa fa-spinner fa-spin");


        $.notify("Datagrids are successfully loaded!", {
            globalPosition: "top center",
            className: "success"
        });
        debugger;

        isWSSExtracted = false;
        isProjExtracted = false;
    }
    debugger;
}

$(function () {

    $("#StartTime").datepicker();
    $("#EndTime").datepicker();
});


$(function () {
    var create = document.querySelector('.create');
    var genText = document.querySelector("#genText");
    var genSpinner = document.querySelector("#genSpinner");
    create.addEventListener("click", function () {
        if (validatepage()) {
            var str = "Please Wait, Fetching Data...  ";
            var result = str.bold();
            genText.innerHTML = result;

            genSpinner.classList.add('fa');
            genSpinner.classList.add('fa-spinner');
            genSpinner.classList.add('fa-pulse');

        }

    });

});


function exportGrids() {
    var today = new Date();

    var dataGrid1 = $("#gridContainer_d1").dxDataGrid("instance");
    var dataGrid2 = $("#gridContainer_d2").dxDataGrid("instance");
    var dataGrid3 = $("#gridContainer_d3").dxDataGrid("instance");
    var dataGrid4 = $("#gridContainer_d4").dxDataGrid("instance");
    var dataGrid5 = $("#gridContainer_d5").dxDataGrid("instance");
    var dataGrid6 = $("#gridContainer_d6").dxDataGrid("instance");
    var dataGrid7 = $("#gridContainer_d7").dxDataGrid("instance");
    var dataGrid8 = $("#gridContainer_d8").dxDataGrid("instance");
    var dataGrid9 = $("#gridContainer_d9").dxDataGrid("instance");
    var dataGrid10 = $("#gridContainer_d10").dxDataGrid("instance");
    var dataGrid11 = $("#gridContainer_d11").dxDataGrid("instance");
    var dataGrid12 = $("#gridContainer_d12").dxDataGrid("instance");
    var workbook = new ExcelJS.Workbook();
    var d1sheet = workbook.addWorksheet("Query Info");

    var d2sheet = workbook.addWorksheet("HWDescription");
    var d3sheet = workbook.addWorksheet("PrjDescription");
    var d4sheet = workbook.addWorksheet("APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1");
    var d5sheet = workbook.addWorksheet("OTSO_BOB1_Cable1_PowerSupply_HAP_IXXAT2");
    var d6sheet = workbook.addWorksheet("ECC_IXXAT1");
    var d7sheet = workbook.addWorksheet("GIO1");
    var d8sheet = workbook.addWorksheet("GIO2");
    var d9sheet = workbook.addWorksheet("LDU");
    var d10sheet = workbook.addWorksheet("PSC");
    var d11sheet = workbook.addWorksheet("VSC");
    var d12sheet = workbook.addWorksheet("WSS");
    var d13sheet = workbook.addWorksheet("WSS2");

    d1sheet.columns = [
        { width: 10 }, { width: 10 }, { width: 10 }, { width: 10 }, { width: 10 }, { width: 10 }, { width: 10 }, { width: 80 }
    ];
    d1sheet.getRow(4).getCell(8).value = "Query Information";
    d1sheet.getRow(4).getCell(8).font = { bold: true, size: 25, underline: "double" };
    d1sheet.getRow(4).height = 70;

    d1sheet.getRow(7).getCell(8).value = 'Start Date: ' + sTime;
   // d1sheet.getRow(7).getCell(8).value = sTime;
    d1sheet.getRow(8).getCell(8).value = 'End Date: ' + eTime;
   // d1sheet.getRow(8).getCell(8).value = eTime;

    d1sheet.getRow(9).getCell(8).value = 'Location: ' + locations;
   // d1sheet.getRow(9).getCell(8).value = locations;

    d1sheet.getRow(10).getCell(8).value = 'Lab IDs: ' + labname;
    //d1sheet.getRow(10).getCell(8).value = labname;

    d1sheet.getRow(11).getCell(8).value = "Date of Report Generation: " + today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
   // d1sheet.getRow(11).getCell(8).value = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();

    for (var j = 7; j <= 11; j++) {

        d1sheet.getRow(j).width = 1000;
        d1sheet.getRow(j).font = { size: 15 };
        d1sheet.getRow(j).getCell(8).fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: '99CCFF' },

        };
        d1sheet.getRow(j).getCell(8).border = {
            top: { style: 'double', color: { argb: '0000000' } },
            left: { style: 'double', color: { argb: '00000000' } },
            bottom: { style: 'double', color: { argb: '00000000' } },
            right: { style: 'double', color: { argb: '00000000' } }
        };

       

    }

    d2sheet.getRow(1).getCell(1).value = "Table1: HWDescription";
    d2sheet.getRow(1).getCell(1).font = { bold: true, size: 16, underline: "double" };
    d3sheet.getRow(1).getCell(1).value = "Table2: PrjDescription";
    d3sheet.getRow(1).getCell(1).font = { bold: true, size: 16, underline: "double" };
    d4sheet.getRow(1).getCell(1).value = "Table3: APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1";
    d4sheet.getRow(1).getCell(1).font = { bold: true, size: 16, underline: "double" };
    d5sheet.getRow(1).getCell(1).value = "Table4: OTSO_BOB1_Cable1_PowerSupply_HAP_IXXAT2";
    d5sheet.getRow(1).getCell(1).font = { bold: true, size: 16, underline: "double" };

    d6sheet.getRow(1).getCell(1).value = "Table5: ECC_IXXAT1";
    d6sheet.getRow(1).getCell(1).font = { bold: true, size: 16, underline: "double" };
    d7sheet.getRow(1).getCell(1).value = "Table6: GIO1";
    d7sheet.getRow(1).getCell(1).font = { bold: true, size: 16, underline: "double" };
    d8sheet.getRow(1).getCell(1).value = "Table7: GIO2";
    d8sheet.getRow(1).getCell(1).font = { bold: true, size: 16, underline: "double" };
    d9sheet.getRow(1).getCell(1).value = "Table8: LDU";
    d9sheet.getRow(1).getCell(1).font = { bold: true, size: 16, underline: "double" };

    d10sheet.getRow(1).getCell(1).value = "Table9: PSC";
    d10sheet.getRow(1).getCell(1).font = { bold: true, size: 16, underline: "double" };
    d11sheet.getRow(1).getCell(1).value = "Table10: VSC";
    d11sheet.getRow(1).getCell(1).font = { bold: true, size: 16, underline: "double" };
    d12sheet.getRow(1).getCell(1).value = "Table11: WSS";
    d12sheet.getRow(1).getCell(1).font = { bold: true, size: 16, underline: "double" };
    d13sheet.getRow(1).getCell(1).value = "Table12: WSS2";
    d13sheet.getRow(1).getCell(1).font = { bold: true, size: 16, underline: "double" };

    //Function to export several DataGrids

    DevExpress.excelExporter.exportDataGrid
        ({
            worksheet: d2sheet,
            component: dataGrid1,
            topLeftCell: { row: 3, column: 2 }

            //})
            //.then(function (cellRange) {
            //    // header
            //    var headerRow = d1sheet.getRow(2);
            //    headerRow.height = 30;
            //    //d1sheet.mergeCells(2, 1, 2, 8);

            //    headerRow.getCell(1).value = 'QUERY INFORMATION' + '\n' + 'ABCX';
            //    headerRow.getCell(1).font = { name: 'Segoe UI Light', size: 22 };
            //    headerRow.getCell(1).alignment = { horizontal: 'center' };

            //    headerRow = d1sheet.getRow(3);
            //    headerRow.getCell(2).value = 'Start Date: ' + sTime;
            //    headerRow.getCell(2).font = { name: 'Segoe UI Light', size: 22 };
            //    headerRow.getCell(2).alignment = { horizontal: 'center' };

            //    headerRow = d1sheet.getRow(4);
            //    headerRow.getCell(3).value = 'End Date: ' + eTime;
            //    headerRow.getCell(3).font = { name: 'Segoe UI Light', size: 22 };
            //    headerRow.getCell(3).alignment = { horizontal: 'center' };

            //    headerRow = d1sheet.getRow(5);
            //    headerRow.getCell(4).value = 'Location: ' + locations_chosen;
            //    headerRow.getCell(4).font = { name: 'Segoe UI Light', size: 22 };
            //    headerRow.getCell(4).alignment = { horizontal: 'center' };

            //    headerRow = d1sheet.getRow(6);
            //    headerRow.getCell(5).value = 'Lab IDs: ' + labids_chosen;
            //    headerRow.getCell(5).font = { name: 'Segoe UI Light', size: 22 };
            //    headerRow.getCell(5).alignment = { horizontal: 'center' };

            //    headerRow = d1sheet.getRow(7);
            //    headerRow.getCell(6).value = 'Date of Report Generation: '  + today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
            //    headerRow.getCell(6).font = { name: 'Segoe UI Light', size: 22 };
            //    headerRow.getCell(6).alignment = { horizontal: 'center' };


        }).then(function () {

            return DevExpress.excelExporter.exportDataGrid({
                worksheet: d3sheet,
                component: dataGrid2,
                topLeftCell: { row: 3, column: 2 }

            });
        }).then(function () {

            return DevExpress.excelExporter.exportDataGrid({
                worksheet: d4sheet,
                component: dataGrid3,
                topLeftCell: { row: 3, column: 2 }

            });
        }).then(function () {

            return DevExpress.excelExporter.exportDataGrid({
                worksheet: d5sheet,
                component: dataGrid4,
                topLeftCell: { row: 3, column: 2 }

            });
        }).then(function () {

            return DevExpress.excelExporter.exportDataGrid({
                worksheet: d6sheet,
                component: dataGrid5,
                topLeftCell: { row: 3, column: 2 }

            });
        }).then(function () {

            return DevExpress.excelExporter.exportDataGrid({
                worksheet: d7sheet,
                component: dataGrid6,
                topLeftCell: { row: 3, column: 2 }

            });
        }).then(function () {

            return DevExpress.excelExporter.exportDataGrid({
                worksheet: d8sheet,
                component: dataGrid7,
                topLeftCell: { row: 3, column: 2 }

            });
        }).then(function () {

            return DevExpress.excelExporter.exportDataGrid({
                worksheet: d9sheet,
                component: dataGrid8,
                topLeftCell: { row: 3, column: 2 }

            });
        }).then(function () {

            return DevExpress.excelExporter.exportDataGrid({
                worksheet: d10sheet,
                component: dataGrid9,
                topLeftCell: { row: 3, column: 2 }

            });
        }).then(function () {

            return DevExpress.excelExporter.exportDataGrid({
                worksheet: d11sheet,
                component: dataGrid10,
                topLeftCell: { row: 3, column: 2 }

            });
        }).then(function () {

            return DevExpress.excelExporter.exportDataGrid({
                worksheet: d12sheet,
                component: dataGrid11,
                topLeftCell: { row: 3, column: 2 }

            });
        }).then(function () {

            return DevExpress.excelExporter.exportDataGrid({
                worksheet: d13sheet,
                component: dataGrid12,
                topLeftCell: { row: 3, column: 2 }

            });
            //}).then(function () {

            //    return DevExpress.excelExporter.exportDataGrid({
            //        worksheet: d13sheet,
            //        component: dataGrid12,
            //        topLeftCell: { row: 3, column: 2 }

            //    });
        }).then(function () {
            workbook.xlsx.writeBuffer().then(function (buffer) {
                saveAs(new Blob([buffer], { type: "application/octet-stream" }), "DiagnosticsGrid" + ' ' + today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear() + ".xlsx");
            });
        });
}

