function GenerateHwDamage_Repair(currency_list) {
    debugger;
    var CostEUR, repcost_eur;
    //Ajax call to Get Request Item MasterList data
    $.ajax({
        type: "GET",
        url: encodeURI("../HWDamage/GetEURINRates"),
        async: false,
        success: OnSuccess_GetEURINRates,
        error: OnError_GetEURINRates
    });
    function OnSuccess_GetEURINRates(response) {
        debugger;

        conversionEURate = response.EUR;
        conversionINRate = response.INR;
    }
    function OnError_GetEURINRates(response) {
        debugger;
        conversionEURate = 1.15;
        conversionINRate = 0.014;
    }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../HwDamage/HWDamage_Repairable",
        dataType: 'json',
        //traditional: true,
        success: function (data) {
            debugger;

            var res = JSON.parse(data.data.Data.Content);

            //var res1 = eval(res);
            $("#hwDamageRepairable_dataGrid").dxDataGrid({

                dataSource: res,
               // keyExpr: "SNo",
                columnResizingMode: "nextColumn",
                columnMinWidth: 50,
               
                onContentReady: function (e) {
                    debugger;
                    e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
                },
                
              
                onCellPrepared: function (e) {
                    if (e.rowType === "header" || e.rowType === "filter") {
                        e.cellElement.css("color", "black");
                        e.cellElement.css("font-weight", "bold");
                        e.cellElement.addClass("columnHeaderCSS");
                        e.cellElement.find("input").addClass("columnHeaderCSS");
                    }
                },
                noDataText: " ☺ No Item ! click '+' Add a row option on the top right",
                editing: {
                    mode: "row",
                    allowUpdating: function (e) {
                        return true;
                    },
                    allowDeleting: true,
                    allowAdding: true,
                    useIcons: true
                },
                onToolbarPreparing: function (e) {
                    var dataGrid = e.component;

                    e.toolbarOptions.items[0].showText = 'always';


                },
                //focusedRowEnabled: true,

                allowColumnReordering: true,
                allowColumnResizing: true,
                columnChooser: {
                    enabled: true
                },

                filterRow: {
                    visible: true

                },
                showBorders: true,
                headerFilter: {
                    visible: true,
                    applyFilter: "auto"
                },
                selection: {
                    applyFilter: "auto"
                },
                loadPanel: {
                    enabled: true
                },
                paging: {
                    pageSize: 100
                },
                searchPanel: {
                    visible: true,
                    width: 240,
                    placeholder: "Search..."
                },

                //onEditorPreparing: function (e) {
                //    var component = e.component,
                //        rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

                //    if (e.dataField === "Item_Name") {

                //        var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data

                //        e.editorOptions.onValueChanged = function (e) {

                //            onValueChanged.call(this, e);
                //            debugger;
                //            var status_sel = component.cellValue(rowIndex, "Hw_status");
                //            if (status_sel != undefined && status_sel != null) {
                //                $.ajax({

                //                    type: "post",
                //                    url: "/HwDamage/GetCostEUR",
                //                    data: { Hw_status: status_sel, ItemName: e.value },
                //                    datatype: "json",
                //                    traditional: true,
                //                    success: function (data) {
                //                        debugger;
                //                        if (data.msg) {
                //                            $.notify(data.msg, {
                //                                globalPosition: "top center",
                //                                className: "warn"
                //                            })
                //                        }
                //                        else {
                //                            CostEUR = data.Cost_EUR;
                //                            //// Emulating a web service call
                //                            window.setTimeout(function () {
                //                                debugger;
                //                                component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
                //                            }, 1000);
                //                        }



                //                    }
                //                })

                //            }


                //        }
                //    }
                //},


               
                columns: [
                    {
                        type: "buttons",
                        width: 100,
                        alignment: "left",
                        buttons: [
                            "edit", "delete"

                        ]
                    },
                    {

                        alignment: "center",
                        columns: [

                            {
                                dataField: "Item Name",
                                allowEditing: false,
                               
                                //lookup: {
                                //    dataSource: function (options) {

                                //        return {

                                //            store: loc_list,
                                //            //filter: options.data ? ["Outdated", "=", false] : null


                                //        };
                                //    },

                                //    valueExpr: "ID",
                                //    displayExpr: "Location"

                                //},



                            },
                            {
                                dataField: "NonRepairable_EUR",
                               // dataType: "number",
                                format: { precision: 2 },
                                allowEditing: false,
                                validationRules: [{ type: "required" }],

                            },

                            {
                                dataField: "Repairable_Cost",
                                dataType: "number",
                                format: { precision: 2 },
                                valueFormat: "#0.00",
                                setCellValue: function (rowData, value) {

                                    rowData.Repairable_Cost = value;
                                    
                                },
                                validationRules: [{ type: "required" }, {
                                    type: "range",
                                    message: "Please enter valid price > 0",
                                    min: 0.01,
                                    max: Number.MAX_VALUE
                                }]


                            },
                            {
                                dataField: "Repair_Currency",
                                validationRules: [{ type: "required" }],
                                setCellValue: function (rowData, value) {

                                    rowData.Repair_Currency = value;
                                },
                                lookup: {
                                    dataSource: function (options) {

                                        return {

                                            store: currency_list,


                                        };
                                    },

                                    valueExpr: "ID",
                                    displayExpr: "Currency"

                                },

                            },


                            {
                                dataField: "Repairable_Cost_EUR",
                                calculateCellValue: function (rowData) {

                                    //update the price in eur
                                    if (rowData.Repairable_Cost == undefined || rowData.Repair_Currency == undefined) {

                                        repcost_eur = 0.0;
                                    }

                                    else {
                                        if (rowData.Repair_Currency == 1)
                                            repcost_eur = rowData.Repairable_Cost * (1 / conversionEURate);
                                        else if (rowData.Repair_Currency == 2)
                                            repcost_eur = rowData.Repairable_Cost;
                                        else if (rowData.Repair_Currency == 3)
                                            repcost_eur = rowData.Repairable_Cost * (conversionINRate / conversionEURate);
                                        else
                                            repcost_eur = 0.0;
                                    }

                                    return repcost_eur;


                                },

                                dataType: "number",
                                format: {  precision: 0 },
                                valueFormat: "#0",
                                allowEditing: false

                            }, //[Repair_UpdatedAt]
                            //[Repair_UpdatedBy]
                            {
                                dataField: "Repair_UpdatedBy",
                                caption: "Updated By",
                                allowEditing: false,
                            },
                            


                        ]
                    }],

                onRowUpdated: function (e) {
                    debugger;
                    $.notify("Please wait, Updating details..!", {
                        globalPosition: "top center",
                        className: "success"
                    })
                    Selected = [];
                    if (e.data.Repair_Currency == 1)
                        e.data.Repairable_Cost_EUR = e.data.Repairable_Cost * (1 / conversionEURate);
                    else if (e.data.Repair_Currency == 2)
                        e.data.Repairable_Cost_EUR = e.data.Repairable_Cost ;
                    else if (e.data.Repair_Currency == 3)
                        e.data.Repairable_Cost_EUR = e.data.Repairable_Cost * (conversionINRate / conversionEURate);
                    else
                        e.data.Repairable_Cost_EUR = 0;
                    debugger;

                   
                    Selected.push(e.data);
                    Update(Selected);
                },

            });
          
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


    function Update(id1/*, filtered_yr*/) {
        debugger;
        $.ajax({
            type: "POST",
            url: encodeURI("../HwDamage/Save_HwDamagedata_Repairable"),
            data: { 'req': id1[0] },
            success: function (data) {
                debugger;
                res = JSON.parse(data.data.Data.Content);

                $("#hwDamageRepairable_dataGrid").dxDataGrid({ dataSource: res });

                if (data.success == true) {
                    $.notify("Sucess", {
                        globalPosition: "top center",
                        className: "success",
                        autoHideDelay: 13000,
                    })
                }
                else {
                    $.notify("Fail", {
                        globalPosition: "top center",
                        className: "error",
                        autoHideDelay: 10000,
                    })
                }

            }

        });

    }

}
