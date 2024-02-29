var BU_list, OEM_list, DEPT_list, Group_list, Item_list, OrderStatus_list, Fund_list, Currency_list, Mode_List;
var DifferenceofDates = "";
var trigger_toUpdateVKM = false;
var param_toUpdateVKM_RequestID = ""
var checkflag = 0;
var curryear = new Date().getFullYear();
Item_list_custom = [];
var BANLabcar = 40, COBLabcar = 50;

$.ajax({

    type: "GET",
    url: "/POSubItem/Lookup",
    async: false,
    success: onsuccess_lookupdata,
    error: onerror_lookupdata
})


function onsuccess_lookupdata(response) {
    debugger;
    lookup_data = response.data;
    BU_list = lookup_data.BU_List;
    OEM_list = lookup_data.OEM_List;
    DEPT_list = lookup_data.DEPT_List;
    //if (lookup_data.Groups_List != null)
    //    Group_list = lookup_data.Groups_List;
    //else
    Group_list = lookup_data.Groups_test;//Groups_oldList;
    Item_list = lookup_data.Item_List;
   
    OrderStatus_list = lookup_data.OrderStatus_List;
    Fund_list = lookup_data.Fund_List;
    Currency_list = lookup_data.Currency_List;
    debugger;
    Item_list_custom = Item_list.filter(function (item) {
        return (item.VKM_Year === curryear);
    });
    debugger;

}

function onerror_lookupdata(response) {
    alert("Error lookup");

}

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
    debugger;

}


function onerror_getMode(response) {
    alert("Error Getting Mode Data");

}
showgrid(BU_list, OEM_list, Group_list, Item_list, DEPT_list, OrderStatus_list, Fund_list, Currency_list);



function showgrid(BU_list, OEM_list, Group_list, Item_list, DEPT_list, OrderStatus_list, Fund_list, Currency_list) {
    debugger;
    $.ajax({
        type: "POST",
        url: "/POSubItem/GetPODetails",
        datatype: "json",
        success: function (data) {
            //debugger;
            if (data.data.length > 0) {
                debugger;
                //var res = JSON.parse(data.data.Data.Content);
                var res = eval(data.data);
                LoadDataGrid(res);

            }
        },
        error: function (jqXHR, exception) {
            debugger;
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            $('#post').html(msg);
        }
    });
}

function LoadDataGrid(res) {

    debugger;

    $("#gridContainerPO").dxDataGrid({
        dataSource: res,
        columnAutoWidth: true,
        width: "97vw",
        height: "83vh",
        editing: {
            mode: "row",
            allowUpdating: true,
            //allowDeleting: true,
            //allowAdding: true,
            useIcons: true
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        filterRow: {
            visible: true

        },
        loadPanel: {
            enabled: true
        },
        headerFilter: {
            visible: true,
            applyFilter: "auto"
        },
        allowColumnReordering: true,
        grouping: {
            autoExpandAll: true,
        },
        groupPanel: {
            visible: true,
        },
        columnChooser: {
            enabled: true
        },
        showBorders: true,
        onContentReady: function (e) {
            //debugger;
            e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
        },
        export: {
            enabled: true
        },
        columnFixing: {
            enabled: true
        },

        columns: [
            {
                type: "buttons",
                width: 60,
                alignment: "left",
                buttons: [
                    "edit", "delete",
                    {
                        hint: "Send to Inventory",   //button for adding data to HW Inventory
                        icon: "movetofolder",
                        visible: function (e) {

                            if (e.row.data.Currentstatus == 5) {           //Visibilty if orderstatus is delivered

                                return true;
                            }

                        },
                        onClick: function (e) {              //Sending the row data to the controller
                            //LinkToInventory(e.row.data);
                            debugger;
                            //popup for choosing the inventory
                            $("#popup1").dxPopup({
                                showTitle: true,
                                title: "Choose the Inventory",
                                visible: true,
                                hideOnOutsideClick: true,
                                //width: 450,
                                //height: 350,
                                resizeEnabled: true,
                                //To add the scroll bar
                                contentTemplate: function (container) {
                                    var scrollView = $("<div id='scrollView'></div>");
                                    //var content = $("<div></div>");
                                    //scrollView.append(content);
                                    scrollView.append('<div id="radioGroupContainer" style="padding-bottom: 20px;"></div>');
                                    scrollView.append('<form id="HwInvDetails" method = "post"></form>');
                                    scrollView.append('<div id="spareinv" style="display:none;"></div>');
                                    scrollView.append('<div id="compinv" style="display:none;" ></div>');
                                    //scrollView.append('<div id="button"></div>');

                                    scrollView.dxScrollView({
                                        height: '100%',
                                        width: '100%'

                                    });

                                    container.append(scrollView);

                                    return container;
                                }

                            });
                            //radiobutton menu
                            $(function () {
                                $("#radioGroupContainer").dxRadioGroup({
                                    dataSource: ["Hardware Inventory", "Spare Inventory", "Component Inventory"],
                                    layout: "horizontal",
                                    value: "Hardware Inventory",
                                    onValueChanged: function (e) {
                                        const previousValue = e.previousValue;
                                        const newValue = e.value;
                                        // Event handling commands go here
                                        //alert("Button."+e.value)
                                        debugger;
                                        if (newValue == "Hardware Inventory") {
                                            checkflag = 0;
                                            debugger;
                                            $('#spareinv').hide();
                                            $('#compinv').hide();
                                            $('#button').show();
                                            $('#HwInvDetails').show();
                                        }
                                        else if (newValue == "Spare Inventory") {
                                            checkflag = 1;
                                            debugger;
                                            $('#HwInvDetails').hide();
                                            $('#compinv').hide();
                                            $('#spareinv').show();
                                            $('#button').hide();
                                        }
                                        else if (newValue == "Component Inventory") {
                                            checkflag = 2;
                                            debugger;
                                            $('#HwInvDetails').hide();
                                            $('#compinv').show();
                                            $('#spareinv').hide();
                                            $('#button').hide();
                                        }
                                    }
                                });
                            });

                            //$("#button").dxButton({
                            //    text: "Submit",
                            //    //type: "success",
                            //    //useSubmitBehavior: true,
                            //    onClick: function (e) {
                            //        //Sending the row data to the controller
                            //        debugger;
                            //        var fdata = $("#HwInvDetails").dxForm("instance").option("formData") 
                            //        LinkToInventory(fdata);
                            //    }

                            //});

                            //Ajax call for getting spare data
                            $.ajax({
                                type: "POST",
                          
                                url: "/POSubItem/getPOdetails_sp",
                                data: { 'req': e.row.data },
                                //datatype: "json",
                                success: function (data) {
                                    debugger;
                                    var poqty = data.data[0].POQuantity;
                                    if (data.data[0].Plant == "W270") {
                                        data.data[0].Plant = "BAN";
                                    }
                                    else if (data.data[0].Plant == "6525") {
                                        data.data[0].Plant = "COB";
                                    }
                                    $("#spareinv").dxForm({


                                        formData: data.data[0],
                                        items: [{
                                            itemType: 'group',
                                            caption: 'Spare Inventory Details',
                                            colCount: 2,

                                            items: [

                                                {
                                                    dataField: 'SpareHW',
                                                    label: {
                                                        text: 'Spare HW'
                                                    },
                                                    //label: 'Inventory Type',
                                                    editorType: 'dxSelectBox',
                                                    editorOptions: {
                                                        items: Item_list_custom,
                                                        searchEnabled: true,
                                                        displayExpr: 'Item_Name',
                                                        valueExpr: 'S_No',
                                                        valueChangeEvent: 'change',
                                                        onValueChanged: function (e) {
                                                            debugger;


                                                            $("#HwInvDetails").dxForm("instance").updateData("InventoryType", e.value);
                                                        }
                                                    },
                                                    validationRules: [{
                                                        type: "required",
                                                        message: "Selecting Inventory Type is Required"
                                                    }]
                                                },
                                                {
                                                    dataField: 'SpareQuantity',
                                                    label: {
                                                        text: 'Spare Quantity'
                                                    },
                                                    editorType: 'dxNumberBox',
                                                    editorOptions: {
                                                        showSpinButtons: true,
                                                        min: 0,
                                                        max: poqty,
                                                        valueChangeEvent: 'change',
                                                        //logic to handle HW Quantity and Spare quantity changes
                                                        onValueChanged: function (e) {
                                                            debugger;
                                                            $("#HwInvDetails").dxForm("instance").updateData("Quantity", poqty - e.value);
                                                            if (checkflag == 1) {
                                                                DevExpress.ui.notify({
                                                                    message: "Remaining Quantity is added to Hardware Inventory"
                                                                });
                                                            }

                                                        }
                                                    }
                                                },

                                                {
                                                    dataField: 'POQuantity',
                                                    //width: 70,
                                                    label: {
                                                        text: 'PO Qty'
                                                    },
                                                    editorOptions: {
                                                        disabled: true,
                                                    }

                                                    //validationRules: [{
                                                    //    type: "required",
                                                    //    message: "Bond No. is required"
                                                    //}]
                                                },



                                                {
                                                    dataField: 'Plant',
                                                    //width: 70,
                                                    label: {
                                                        text: 'Location'
                                                    },
                                                    editorOptions: {
                                                        disabled: true,
                                                    }


                                                },
                                                {
                                                    dataField: 'UnitPrice',
                                                    //width: 70,
                                                    label: {
                                                        text: 'Price',
                                                     /*   visible:false,*/
                                                    },
                                                    editorOptions: {
                                                        disabled: true,
                                                        /*visible: false,*/
                                                    }
                                                },
                                                {
                                                    dataField: 'Currency',
                                                    //width: 70,
                                                    label: {
                                                        text: 'Currency'
                                                    },
                                                    editorType: 'dxSelectBox',
                                                    editorOptions: {
                                                        items: Currency_list,
                                                        displayExpr: 'Currency',
                                                        valueExpr: 'ID',
                                                        disabled: true,
                                                    },

                                                },
                                                //{
                                                //    dataField: 'Status',
                                                //    //width: 70,
                                                //    label: {
                                                //        text: 'Status',
                                                //        /*   visible:false,*/
                                                //    },
                                                //    editorOptions: {
                                                        
                                                //        /*visible: false,*/
                                                //    }
                                                //},
                                                //{
                                                //    dataField: 'BANUnderRepair',
                                                //    //width: 70,
                                                //    label: {
                                                //        text: 'BAN Under Repair',
                                                //        /*   visible:false,*/
                                                //    },
                                                //    editorOptions: {
                                                       
                                                //        value :"0"
                                                //        /*visible: false,*/
                                                //    }
                                                //},
                                                //{
                                                //    dataField: 'COBUnderRepair',
                                                //    //width: 70,
                                                //    label: {
                                                //        text: 'COB Under Repair',
                                                //        /*   visible:false,*/
                                                //    },
                                                //    editorOptions: {
                                                    
                                                //        value: "0"
                                                //        /*visible: false,*/
                                                //    }
                                                //},
                                            ],

                                        },
                                   
                                            {
                                                itemType: 'button',
                                                horizontalAlignment: 'center',
                                                buttonOptions: {
                                                    text: 'Submit',
                                                    type: 'success',

                                                    onClick: function (e) {
                                                        debugger;
                                                        var fdata = $("#HwInvDetails").dxForm("instance").option("formData");
                                                        var spdata = $("#spareinv").dxForm("instance").option("formData");
                                                        var valresult = $("#HwInvDetails").dxForm("instance").validate();
                                                        var valresults = $("#spareinv").dxForm("instance").validate();
                                                        //submit condition to submit the hardware inventory form

                                                        if (fdata.Quantity > 0 && spdata.SpareQuantity > 0) {
                                                            if (valresult.isValid && valresults.isValid) {
                                                                debugger;
                                                                LinkToInventory(fdata);
                                                                LinkToSpare(spdata);
                                                                $("#popup1").dxPopup("hide");

                                                            }
                                                            else {
                                                                DevExpress.ui.notify({
                                                                    message: "Please fill all the details in Hardware Inventory!"
                                                                });
                                                                $("#popup1").dxPopup("show");
                                                            }
                                                        }

                                                        else if (fdata.Quantity > 0) {


                                                            if (valresult.isValid) {
                                                                debugger;
                                                                LinkToInventory(fdata);
                                                                $("#popup1").dxPopup("hide");

                                                            }
                                                            else {
                                                                DevExpress.ui.notify({
                                                                    message: "Please fill all the details in Hardware Inventory!"
                                                                });
                                                                $("#popup1").dxPopup("show");
                                                            }
                                                        }
                                                        //submit func for spare inventory
                                                        else if (spdata.SpareQuantity > 0) {

                                                            if (valresults.isValid) {
                                                                debugger;

                                                                LinkToSpare(spdata);
                                                                $("#popup1").dxPopup("hide");
                                                            }
                                                            else {
                                                                DevExpress.ui.notify({
                                                                    message: "Please fill all the details in Spare Inventory!"
                                                                });
                                                                $("#popup1").dxPopup("show");
                                                            }
                                                        }

                                                        //if ((valresult.isValid || valresults.isValid) && (valresult.isValid && valresults.isValid)) {
                                                        //    $("#popup1").dxPopup("hide");
                                                        //}
                                                        /*$("#popup1").dxPopup("hide");*/

                                                    }

                                            },

                                        },
                                        ]
                                    });

                                },

                            });


                            
                            //Component inventory

                            $("#compinv").dxForm({


                                //formData: data.data[0],
                                items: [{
                                    itemType: 'group',
                                    caption: 'Component Inventory Details',
                                }]
                            });


                            //Hardware Inventory form details
                            $.ajax({
                                type: "POST",
                                //contentType: "application/json; charset=utf-8",
                                url: "/POSubItem/getPOdetails_inv",
                                data: { 'req': e.row.data },
                                //datatype: "json",
                                success: function (data) {

                                    debugger;
                                    data.data[0].Quantity = data.data[0].POQuantity;
                                    var poqty = data.data[0].POQuantity;
                                    $("#HwInvDetails").dxForm({


                                        formData: data.data[0],
                                        items: [{
                                            itemType: 'group',
                                            caption: 'Hardware Inventory Details',
                                            colCount: 2,

                                            items: [
                                                {
                                                    dataField: 'InventoryType',
                                                    label: {
                                                        text: 'Inventory Type'
                                                    },
                                                    //label: 'Inventory Type',
                                                    editorType: 'dxSelectBox',
                                                    editorOptions: {
                                                        
                                                        items: Item_list_custom,
                                                        displayExpr: 'Item_Name',
                                                        valueExpr: 'S_No',
                                                        searchEnabled: true,
                                                        valueChangeEvent: 'change',
                                                        onValueChanged: function (e) {
                                                            debugger;


                                                            $("#spareinv").dxForm("instance").updateData("SpareHW", e.value);
                                                        }
                                                    },
                                                    validationRules: [{
                                                        type: "required",
                                                        message: "Selecting Inventory Type is Required"
                                                    }]


                                                },

                                                {
                                                    dataField: 'SerialNo',
                                                    //width: 70,
                                                    label: {
                                                        text: 'Serial Number'
                                                    },
                                                    /*dataType: 'number',*/
                                                    validationRules: [{
                                                        type: "required",
                                                        message: "Serial No. is required",
                                                        showColonAfterLabel: true,
                                                        showValidationSummary: true,
                                                        validationMessageMode: 'always',
                                                        validationMessagePosition: 'bottom',
                                                    }]
                                                },
                                                {
                                                    dataField: 'BondNo',
                                                    //width: 70,
                                                    label: 'Bond Number',
                                                    validationRules: [{
                                                        type: "required",
                                                        message: "Bond No. is required"
                                                    }]
                                                    /*dataType: 'number',*/

                                                },
                                                {
                                                    dataField: 'BondDate',
                                                    label: {
                                                        text: 'Bond Date'
                                                    },
                                                    //dataType: 'date',
                                                    //format: "MM/dd/yyyy",
                                                    //caption: 'Bond Date',
                                                    editorType: 'dxDateBox',
                                                    editorOptions: {
                                                        //displayFormat: 'datetim',
                                                        format: {
                                                            type: "shortDate",
                                                        },

                                                        useMaskBehavior: true,


                                                    },
                                                    validationRules: [{
                                                        type: "required",
                                                        message: "Bond Date is required"
                                                    }]
                                                },
                                                {
                                                    dataField: 'AssetNo',
                                                    label: {
                                                        text: 'Asset Number'
                                                    },
                                                    validationRules: [{
                                                        type: "required",
                                                        message: "Asset No. is required"
                                                    }]
                                                    /*dataType: 'number',*/
                                                },
                                                {
                                                    dataField: 'HardwareResponsible',
                                                    label: {
                                                        text: 'HW Responsible'
                                                    },
                                                    validationRules: [{
                                                        type: "required",
                                                        message: "HW Responsible is required"
                                                    }]
                                                    /*dataType: 'string',*/
                                                },
                                                {
                                                    dataField: 'HandoverTo',
                                                    label: {
                                                        text: 'Handover To'
                                                    },
                                                    validationRules: [{
                                                        type: "required",
                                                        message: "Handover To is required"
                                                    }]
                                                    /*dataType: 'string',*/
                                                },
                                                {
                                                    dataField: 'Mode',
                                                    label: 'Mode',
                                                    editorType: 'dxSelectBox',
                                                    editorOptions: {
                                                        items: Mode_List,
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
                                                    label: {
                                                        text: 'Remarks'
                                                    },

                                                    validationRules: [{
                                                        type: "required",
                                                        message: "Remarks is required"
                                                    }]
                                                },
                                                {
                                                    dataField: 'ALMNo',
                                                    label: {
                                                        text: 'ALM Number'
                                                    },
                                                    validationRules: [{
                                                        type: "required",
                                                        message: "ALM No. is required"
                                                    }]
                                                },
                                                {
                                                    dataField: 'BU',
                                                    label: {
                                                        text: 'BU'
                                                    },
                                                    editorType: 'dxSelectBox',
                                                    editorOptions: {
                                                        items: BU_list,
                                                        displayExpr: 'BU',
                                                        valueExpr: 'ID',
                                                        disabled: true,
                                                    },



                                                },
                                                {
                                                    dataField: 'OEM',
                                                    label: {
                                                        text: 'OEM'
                                                    },
                                                    editorType: 'dxSelectBox',
                                                    editorOptions: {
                                                        items: OEM_list,
                                                        displayExpr: 'OEM',
                                                        valueExpr: 'ID',
                                                        disabled: true,
                                                    },

                                                },
                                                {
                                                    dataField: 'GROUP',
                                                    label: {
                                                        text: 'Group'
                                                    },
                                                    editorType: 'dxSelectBox',
                                                    editorOptions: {
                                                        items: Group_list,
                                                        displayExpr: 'Group',
                                                        valueExpr: 'ID',
                                                        disabled: true,
                                                        searchEnabled: true,
                                                    },

                                                },
                                                {
                                                    dataField: 'ItemDescription',
                                                    label: {
                                                        text: 'Item Name'
                                                    },
                                                    editorOptions: {

                                                        disabled: true,
                                                    },
                                                    //dataType: 'string',
                                                    //editorType: 'dxSelectBox',
                                                    //editorOptions: {
                                                    //    items: Item_list,
                                                    //    displayExpr: 'Item_Name',
                                                    //    valueExpr: 'S_No',
                                                    //    disabled: true,
                                                    //},


                                                },
                                                ////{
                                                ////    dataField: 'ItemName',
                                                ////    label: {
                                                ////        text: 'Item Name From Planner'
                                                ////    },
                                                ////    editorType: 'dxSelectBox',
                                                ////    editorOptions: {
                                                ////        items: Item_list5,
                                                ////        displayExpr: 'Item_Name',
                                                ////        valueExpr: 'S_No',
                                                ////        disabled: true,
                                                ////        searchEnabled: true,
                                                ////    },

                                                ////},
                                                {
                                                    dataField: 'POQuantity',
                                                    label: {
                                                        text: 'PO Quantity'
                                                    },
                                                    editorOptions: {
                                                        disabled: true
                                                    },

                                                },
                                                {
                                                    dataField: 'Quantity',
                                                    label: {
                                                        text: 'HW Quantity'
                                                    },
                                                    editorType: 'dxNumberBox',
                                                    editorOptions: {
                                                        showSpinButtons: true,
                                                        min: "0",
                                                        max: poqty,
                                                        valueChangeEvent: 'change',
                                                        //logic to implement the logic between the POQuantity, Spare qty and Hw qty
                                                        onValueChanged: function (e) {
                                                            debugger;
                                                            $("#spareinv").dxForm("instance").updateData("SpareQuantity", poqty - e.value);

                                                            if (checkflag == 0) {
                                                                DevExpress.ui.notify({
                                                                    message: "Remaining Quantity is added to Spare Inventory"
                                                                });
                                                            }

                                                        }
                                                    }

                                                },
                                                {
                                                    dataField: 'UOM',
                                                    label: {
                                                        text: 'UOM'
                                                    },
                                                    editorOptions: {
                                                        disabled: true
                                                    },

                                                },

                                                {
                                                    dataField: 'ActualDeliveryDate',
                                                    label: {
                                                        text: 'Actual Delivery Date'
                                                    },
                                                    editorOptions: {
                                                        disabled: true
                                                    },

                                                },


                                            ],



                                        },
                                        {
                                            itemType: 'button',
                                            horizontalAlignment: 'center',
                                            buttonOptions: {
                                                text: 'Submit',
                                                type: 'success',

                                                onClick: function (e) {
                                                    debugger;
                                                    var fdata = $("#HwInvDetails").dxForm("instance").option("formData");
                                                    var spdata = $("#spareinv").dxForm("instance").option("formData");
                                                    var valresult = $("#HwInvDetails").dxForm("instance").validate();
                                                    var valresults = $("#spareinv").dxForm("instance").validate();
                                                    //submit condition to submit the hardware inventory form

                                                    if (fdata.Quantity > 0 && spdata.SpareQuantity > 0)
                                                    {
                                                        if (valresult.isValid && valresults.isValid) {
                                                            debugger;
                                                            LinkToInventory(fdata);
                                                            LinkToSpare(spdata);
                                                            $("#popup1").dxPopup("hide");

                                                        }
                                                        else {
                                                            DevExpress.ui.notify({
                                                                message: "Please fill all the details in Hardware Inventory!"
                                                            });
                                                            $("#popup1").dxPopup("show");
                                                        }
                                                    }
                                                    
                                                    else if(fdata.Quantity > 0) {
                                                        

                                                        if (valresult.isValid) {
                                                            debugger;
                                                            LinkToInventory(fdata);
                                                            $("#popup1").dxPopup("hide");

                                                        }
                                                        else {
                                                            DevExpress.ui.notify({
                                                                message: "Please fill all the details in Hardware Inventory!"
                                                            });
                                                            $("#popup1").dxPopup("show");
                                                        }
                                                    }
                                                    //submit func for spare inventory
                                                    else if (spdata.SpareQuantity > 0) {
                                                        
                                                        if (valresults.isValid  ) {
                                                            debugger;
                                                            
                                                            LinkToSpare(spdata);
                                                            $("#popup1").dxPopup("hide");
                                                        }
                                                        else {
                                                            DevExpress.ui.notify({
                                                                message: "Please fill all the details in Spare Inventory!"
                                                            });
                                                            $("#popup1").dxPopup("show");
                                                        }
                                                    }

                                                    //if ((valresult.isValid || valresults.isValid) && (valresult.isValid && valresults.isValid)) {
                                                    //    $("#popup1").dxPopup("hide");
                                                    //}
                                                    /*$("#popup1").dxPopup("hide");*/

                                                }

                                            },

                                        },


                                        ],


                                    });



                                },
                            });

                        },
                    }
                ],



                fixed: true,
                fixedPosition: "left"
            },
            {

                alignment: "center",
                columns: [
                    //{
                    //    dataField: "BU",
                    //    width: 70,
                    //    groupIndex: 0,
                    //    //setCellValue: function (rowData, value) {
                    //    //    debugger;
                    //    //    rowData.BU = value;

                    //    //},
                    //    //lookup: {
                    //    //    dataSource: function (options) {
                    //    //        debugger;
                    //    //        return {

                    //    //            store: BU_list,
                    //    //        };

                    //    //    },
                    //    //    valueExpr: "ID",
                    //    //    displayExpr: "BU"
                    //    //},
                    //    allowEditing: false
                    //},



                    //{
                    //    dataField: "OEM",
                    //    groupIndex: 0,
                    //    width: 90,
                    //    //lookup: {
                    //    //    dataSource: OEM_list,
                    //    //    valueExpr: "ID",
                    //    //    displayExpr: "OEM"
                    //    //},
                    //    allowEditing: false

                    //},
                    //{
                    //    dataField: "Dept",
                    //    groupIndex: 0,
                    //    caption: "Dept",
                    //    //setCellValue: function (rowData, value) {
                    //    //    debugger;
                    //    //    rowData.DEPT = value;
                    //    //    rowData.Group = null;

                    //    //},
                    //    width: 130,
                    //    //lookup: {
                    //    //    dataSource: function (options) {
                    //    //        debugger;
                    //    //        return {

                    //    //            store: DEPT_list,
                    //    //            filter: options.data ? ["Outdated", "=", false] : null


                    //    //        };
                    //    //    },

                    //    //    valueExpr: "ID",
                    //    //    displayExpr: "DEPT"

                    //    //},
                    //    allowEditing: false
                    //},
                    //{
                    //    dataField: "GROUP",
                    //    groupIndex: 0,
                    //    width: 100,
                    //    //validationRules: [{ type: "required" }],
                    //    //lookup: {
                    //    //    dataSource: function (options) {

                    //    //        return {

                    //    //            store: Group_list,

                    //    //            filter: options.data ? ["Dept", "=", options.data.DEPT] : null
                    //    //        };

                    //    //    },
                    //    //    valueExpr: "ID",
                    //    //    displayExpr: "Group"
                    //    //},
                    //    allowEditing: false

                    //},

                    {
                        dataField: "RequestID",
                        allowEditing: false,
                        visible: false
                    },
                    {
                        dataField: "ItemName",
                        //width: 400,
                        groupIndex: 0,
                        //validationRules: [{ type: "required" }],

                        //CAS 2: testing for same masterlist for bus
                        lookup: {
                            dataSource: function (options) {
                                //debugger;
                                return {


                                    store: Item_list,

                                    //filter: options.data ? [["BU", "=", BU_forItemFilter/*options.data.BU*/], 'and', ["Deleted", "=", false]] : null
                                }



                            },

                            valueExpr: "S_No",
                            displayExpr: "Item_Name"
                        },

                        //calculateSortValue: function (data) {
                        //    debugger;
                        //    const value = this.calculateCellValue(data);
                        //    return this.lookup.calculateCellValue(value);
                        //},
                        allowEditing: false

                    },

                    //{
                    //    dataField: "Unit_Price",
                    //    caption: "Unit Price",
                    //    dataType: "number",
                    //    format: { type: "currency", precision: 0 },
                    //    valueFormat: "#0",
                    //    allowEditing: false,
                    //    validationRules: [{ type: "required" }, {
                    //        type: "range",
                    //        message: "Please enter valid price > 0",
                    //        min: 0.01,
                    //        max: Number.MAX_VALUE
                    //    }],
                    //    allowEditing: false,
                    //    visible: false


                    //},
                    //{
                    //    dataField: "Total_Price",
                    //    width: 100,
                    //    calculateCellValue: function (rowData) {

                    //        if (rowData.Required_Quantity > 0 && rowData.Unit_Price > 0) {
                    //            return rowData.Unit_Price * rowData.Required_Quantity;
                    //        }
                    //        else
                    //            return 0.0;
                    //    },

                    //    dataType: "number",
                    //    format: { type: "currency", precision: 0 },
                    //    valueFormat: "#0",
                    //    allowEditing: false
                    //},


                    {
                        dataField: "VKMYear",
                        caption: "VKM Yr",
                        allowEditing: false,
                        visible: false
                    },
                    {
                        dataField: "PONumber",
                        caption: "PO",
                        allowEditing: false,
                    },
                    {
                        dataField: "PIFID",
                        allowEditing: false,
                        visible: false
                    },
                    , {
                        dataField: "Fund",
                        setCellValue: function (rowData, value) {
                            //debugger;
                            rowData.Fund = value;

                        },
                        lookup: {
                            dataSource: function (options) {
                                //debugger;
                                return {

                                    store: Fund_list,
                                };

                            },
                            valueExpr: "ID",
                            displayExpr: "Fund"
                        },
                        allowEditing: false,
                    },
                    {
                        dataField: "FundCenter",
                        allowEditing: false,
                        visible: false
                    },
                    , {
                        dataField: "BudgetCode",
                        allowEditing: false,
                        visible: false
                    },

                    , {
                        dataField: "ItemDescription",
                        width: 250,
                        allowEditing: false
                    },
                    {
                        dataField: "POQuantity",
                        allowEditing: false,
                        caption: "PO Qty"
                    },
                    , {
                        dataField: "UOM",
                        allowEditing: false,
                        visible: false
                    },

                    {
                        dataField: "UnitPrice",
                        allowEditing: false

                    },
                    {
                        dataField: "Netvalue",
                        allowEditing: false,

                    },
                    {
                        dataField: "Netvalue_USD",
                        //allowEditing: false,
                        visible: false
                    },

                    , {
                        dataField: "Currency",
                        allowEditing: false,
                        visible: false,
                        setCellValue: function (rowData, value) {
                            //debugger;
                            rowData.Currency = value;

                        },
                        lookup: {
                            dataSource: function (options) {
                                //debugger;
                                return {

                                    store: Currency_list,
                                };

                            },
                            valueExpr: "ID",
                            displayExpr: "Currency"
                        },
                    }
                    , {
                        dataField: "Plant",
                        allowEditing: true,
                    },
                    {
                        dataField: "POCreatedOn",
                        caption: "Order Dt",
                        dataType: "date",
                        allowEditing: false,
                    },
                    {
                        dataField: "VendorName",
                        caption: "Vendor",
                        allowEditing: false,
                    },
                    //{
                    //    dataField: "DeliveryDateRequested",
                    //    allowEditing: true,
                    //    caption: "Delivery Requested"
                    //},
                    {
                        caption: "Tentative",
                        columns: [

                            {
                                dataField: "CW"
                            },
                            {
                                dataField: "TentativeDeliveryDate",
                                allowEditing: true,
                                caption: "Dt",
                                dataType: "date",
                            }
                        ]
                    },
                    {
                        dataField: "ActualDeliveryDate",
                        allowEditing: true,
                        caption: "Actual Dt",
                        dataType: "date",
                    },
                    {
                        dataField: "DifferenceinDeliveRyDate",
                        allowEditing: false,
                        caption: "Plan-Actual"
                    },

                    , {
                        dataField: "ActalAmt",
                        visible: false
                    }
                    , {
                        dataField: "NegotiatedAmt",
                        visible: false
                    }
                    , {
                        dataField: "Savings",
                        visible: false
                    }
                    , {
                        dataField: "Currentstatus",
                        caption: "Status",
                        setCellValue: function (rowData, value) {
                            //debugger;
                            rowData.Currentstatus = value;
                            //if (rowData.Currentstatus === "Delivered") {
                            //    buttons[0].visible = true;
                            //}


                        },
                        lookup: {
                            dataSource: function (options) {
                                //debugger;
                                return {

                                    store: OrderStatus_list,
                                };

                            },
                            valueExpr: "ID",
                            displayExpr: "OrderStatus"
                        },
                    }
                    , "PORemarks"


                ]
            }],
        onEditorPreparing: function (e) {
            var component = e.component,
                rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

            if (e.dataField === "ActualDeliveryDate") {
                //debugger;
                var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                e.editorOptions.onValueChanged = function (e) {
                    onValueChanged.call(this, e);
                    //debugger;
                    var tentativedeldt = component.cellValue(rowIndex, "TentativeDeliveryDate");

                    if (tentativedeldt != undefined && tentativedeldt != null && tentativedeldt != "" && e.value != undefined && e.value != null && e.value != "") {
                        //debugger;
                        $.ajax({

                            type: "post",
                            url: "/POSubItem/GetDateDifference",
                            data: { Tentative: component.cellValue(rowIndex, "TentativeDeliveryDate"), Actual: e.value },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                //debugger;



                                DifferenceofDates = data.data;
                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "DifferenceinDeliveRyDate", DifferenceofDates);
                                }, 1000);



                            }
                        })
                        //// Emulating a web service call

                    }
                    else {
                        window.setTimeout(function () {
                            //debugger;
                            component.cellValue(rowIndex, "DifferenceinDeliveRyDate", "");
                        }, 1000);
                    }
                    //if (component.cellValue(rowIndex, "RequestID") != undefined && component.cellValue(rowIndex, "RequestID") != null && component.cellValue(rowIndex, "RequestID") != "") {
                    //    debugger;
                    //    $.ajax({

                    //        type: "post",
                    //        url: "/POSubItem/UpdateDetailsinVKM",
                    //        data: { RequestID: component.cellValue(rowIndex, "RequestID") },
                    //        datatype: "json",
                    //        traditional: true,
                    //        success: function (data) {
                    //            debugger;
                    //            // reviewer_2 = data;


                    //        }
                    //    })
                    //}
                    trigger_toUpdateVKM = true;
                    //debugger;
                    param_toUpdateVKM_RequestID = component.cellValue(rowIndex, "RequestID");
                }
            }


            if (e.dataField === "TentativeDeliveryDate") {
                //debugger;
                //var Tentaive = new Date(e.value).toISOString().slice(0, 10);
                var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                e.editorOptions.onValueChanged = function (e) {
                    onValueChanged.call(this, e);
                    //debugger;
                    //if (component.cellValue(rowIndex, "RequestID") != undefined && component.cellValue(rowIndex, "RequestID") != null && component.cellValue(rowIndex, "RequestID") != "") {
                    //    debugger;
                    //    $.ajax({

                    //        type: "post",
                    //        url: "/POSubItem/UpdateDetailsinVKM",
                    //        data: { RequestID: component.cellValue(rowIndex, "RequestID") },
                    //        datatype: "json",
                    //        traditional: true,
                    //        success: function (data) {
                    //            debugger;
                    //            // reviewer_2 = data;


                    //        }
                    //    })
                    //}
                    trigger_toUpdateVKM = true;
                    //debugger;
                    param_toUpdateVKM_RequestID = component.cellValue(rowIndex, "RequestID");

                    var actaldeldt = component.cellValue(rowIndex, "ActualDeliveryDate");
                    if (actaldeldt != undefined && actaldeldt != null && actaldeldt != "" && e.value != undefined && e.value != null && e.value != "") {
                        //debugger;
                        $.ajax({

                            type: "post",
                            url: "/POSubItem/GetDateDifference",
                            data: { Tentative: e.value, Actual: component.cellValue(rowIndex, "ActualDeliveryDate") },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                //debugger;

                                DifferenceofDates = data.data;
                                //// Emulating a web service call
                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "DifferenceinDeliveRyDate", DifferenceofDates);
                                }, 1000);



                            }
                            , error: function (data) {
                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "DifferenceinDeliveRyDate", "");
                                }, 1000);

                            }
                        })


                    }
                    else {
                        window.setTimeout(function () {
                            //debugger;
                            component.cellValue(rowIndex, "DifferenceinDeliveRyDate", "");
                        }, 1000);
                    }
                    //debugger;


                    //// Emulating a web service call

                    //window.setTimeout(function () {
                    //    debugger;
                    //    component.cellValue(rowIndex, "DifferenceinDeliveRyDate", DifferenceofDates);
                    //}, 1000)
                }
            }


            if (e.dataField === "Currentstatus") {

                var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                e.editorOptions.onValueChanged = function (e) {
                    onValueChanged.call(this, e);
                    //debugger;

                    //if (component.cellValue(rowIndex, "RequestID") != undefined && component.cellValue(rowIndex, "RequestID") != null && component.cellValue(rowIndex, "RequestID") != "") {
                    //    debugger;
                    //    $.ajax({

                    //        type: "post",
                    //        url: "/POSubItem/UpdateDetailsinVKM",
                    //        data: { RequestID: component.cellValue(rowIndex, "RequestID") },
                    //        datatype: "json",
                    //        traditional: true,
                    //        success: function (data) {
                    //            debugger;
                    //            // reviewer_2 = data;


                    //        }
                    //    })
                    //}
                    trigger_toUpdateVKM = true;
                    //debugger;
                    param_toUpdateVKM_RequestID = component.cellValue(rowIndex, "RequestID");
                }
            }
            //debugger;


        },
        onRowInserting: function (e) {
            //debugger;
            $.notify(" Your Purchase Order details is being added...Please wait!", {
                globalPosition: "top center",
                className: "success"
            })
            Selected = [];
            //var LeadTime_tocalc_ExpReqdDt;
            //debugger;
            // e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
            // e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
            Selected.push(e.data);
            //debugger;
            Save_POdata(Selected);
        },
        onRowUpdated: function (e) {
            $.notify(" Your Purchase Order details is being Updated...Please wait!", {
                globalPosition: "top center",
                className: "success"
            })
            Selected = [];
            //var LeadTime_tocalc_ExpReqdDt;
            //debugger;
            // e.data.Total_Price = e.data.Unit_Price * e.data.Required_Quantity;
            // e.data.Reviewed_Cost = e.data.Unit_Price * e.data.Reviewed_Quantity;
            Selected.push(e.data);
            //debugger;
            Save_POdata(Selected);
        },

    });
}

//Function to convert the date to ddmmyyyy format to adjust the date receiving from HW form

function convert(str) {
    debugger;
    var date = new Date(str),
        mnth = ("0" + (date.getMonth() + 1)).slice(-2),
        day = ("0" + date.getDate()).slice(-2);
    return [mnth, day, date.getFullYear(),].join("/");
}

//Function for passing the row to the controller
function LinkToInventory(id1) {

    debugger;
    var bdate = id1.BondDate;
    var cdate = convert(bdate);
    id1.BondDate = cdate;

    $.ajax({
        type: "POST",
        //contentType: "application/json; charset=utf-8",
        url: "/POSubItem/LinkToInventory",
        data: { 'req': id1 },
        //datatype: "json",
        success: function (data) {
            //$("#popup1").dxPopup("hide");
            DevExpress.ui.notify({
                message: "Data Added to Hardware Inventory!"
            });
            //debugger;

        },
        error: function (jqXHR, exception) {
            //debugger;
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            $('#post').html(msg);
        },
    });
}

//function to send the spare data to controller
function LinkToSpare(id1) {

    debugger;
    //$.ajax({
    //    type: "POST",
    //    url: encodeURI("../InventoryLab/GetSpareCalc"),
    //    data: { 'id': id1.SpareHW },

    //    //if success, data gets refreshed internally
    //    success: function (e) {
    //        debugger;

    //        id1.push({ "SpareCalc": e.data });
    //        id1.push({ "BANreqd": e.data * BANLabcar });
    //        id1.push({ "BANreqd": e.data * COBLabcar });

    //        //$("#SpareTable").dxDataGrid("columnOption", "SpareCalc", spcalc);
    //        alert("success")
    //    },
    //    error: function (e) {
    //        alert("error getting data from configuration");
    //    }

    //});


    $.ajax({
        type: "POST",
        //contentType: "application/json; charset=utf-8",
        url: "/POSubItem/LinkToSpare",
        data: { 'req': id1 },
        //datatype: "json",
        success: function (data) {
            //$("#popup1").dxPopup("hide");
            DevExpress.ui.notify({
                message: "Data added to Spare Inventory!"
            });
            //debugger;

        },
        error: function (jqXHR, exception) {
            //debugger;
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            $('#post').html(msg);
        },
    });
}


function Save_POdata(id1) {
    //debugger;
    $.ajax({
        type: "POST",
        //contentType: "application/json; charset=utf-8",
        url: "/POSubItem/Save_POdata",
        data: { 'req': id1[0] },
        //datatype: "json",
        success: function (data) {
            $.notify(" Updated Successfully!", {
                globalPosition: "top center",
                className: "success"
            })
            //debugger;
            if (trigger_toUpdateVKM) {
                $.ajax({

                    type: "post",
                    url: "/POSubItem/UpdateDetailsinVKM",
                    data: { RequestID: param_toUpdateVKM_RequestID },
                    datatype: "json",
                    traditional: true,
                    success: function (data) {
                        //debugger;
                        // reviewer_2 = data;
                        trigger_toUpdateVKM = false;
                        param_toUpdateVKM_RequestID = "";

                    }
                })

            }

        },
        error: function (jqXHR, exception) {
            debugger;
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            $('#post').html(msg);
        },
    });
}