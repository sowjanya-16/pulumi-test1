function GenerateTC(codelist,yearlist) {
    debugger;
    var dataGrid_tc;
    //var LabTypes = $('#selectLabType').val();
    //var Locations = $('#selectLocation').val();

    //$.notify('Loading the data. Please wait!', {
    //    globalPosition: "top center",
    //    className: "warn"
    //});



    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../HC_TC/GenerateTC",
        dataType: 'json',
        //traditional: true,
        success: function (data) {
            debugger;
            
            var res = JSON.parse(data.data.Data.Content);
            dataGrid_tc = $("#tc_dataGrid").dxDataGrid({

                dataSource: res,
                keyExpr: "ID",
                columnResizingMode: "nextColumn",
                columnMinWidth: 50,
                //stateStoring: {
                //    enabled: true,
                //    type: "localStorage",
                //    storageKey: "RequestID"
                //},
               
                onContentReady: function (e) {
                    debugger;
                    e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
                },
                //noDataText: encodeMessage(noDataTextUnsafe),
                //showRequiredMark: true,
                //RequiredMark: '*',
                //columnResizingMode: "widget",
                //columnMinWidth: 100,
                onCellPrepared: function (e) {
                    if (e.rowType === "header" || e.rowType === "filter") {
                        //e.cellElement.addClass("columnHeaderCSS");
                        //e.cellElement.find("input").addClass("columnHeaderCSS");

                       
                            e.cellElement.css("color", "black");
                            e.cellElement.css("font-weight", "bold");

                         
                    }
                },
                noDataText: " ☺ No Item ! click '+' Add a row option on the top right",
                editing: {
                    mode: "row",
                    allowUpdating: function (e) {
                        return true;
                    },
                    //allowDeleting: function (e) {
                    //    if (e.row.data.ApprovalHoE == undefined)
                    //        e.row.data.ApprovalHoE = false;
                    //    e.row.data.ApprovalHoE = false;
                    //    return !e.row.data.ApprovalHoE
                    //},//true,
                    allowAdding: true,
                    useIcons: true
                },
                onToolbarPreparing: function (e) {
                    var dataGrid = e.component;

                    e.toolbarOptions.items[0].showText = 'always';


                },
                focusedRowEnabled: true,

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

                grouping: {
                    autoExpandAll: true,
                },
                groupPanel: {
                    visible: true,
                },
                onEditorPreparing: function (e) {
                    var component = e.component,
                        rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex


                    if (e.dataField === "Budget_Plan") {
                        debugger;

                        var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                        e.editorOptions.onValueChanged = function (e) {
                            debugger;
                            onValueChanged.call(this, e);
                            debugger;
                            $.ajax({
                                type: "post",
                                url: "/HC_TC/GetBalanceBudget",
                                data: {
                                    BudgPlan: e.value, Inv: component.cellValue(rowIndex, "Invoice")                                },
                               
                                datatype: "json",
                                success: function (data) {
                                    //debugger;
                                    var remain_budget;
                                    remain_budget = data.Bal_Budget;
                                    
                                    window.setTimeout(function () {
                                        //debugger;
                                        component.cellValue(rowIndex, "Bud_Inv", remain_budget);
                                    }, 1000);
                                },
                                error: function (data) {
                                    //debugger;
                                }
                            });


                            $.ajax({
                                type: "post",
                                url: "/HC_TC/GetAvailableBudget",
                                data: {
                                    BudgPlan: e.value, Inv: component.cellValue(rowIndex, "Invoice"), Open: component.cellValue(rowIndex, "Open")
                                },

                                datatype: "json",
                                success: function (data) {
                                    //debugger;
                                    var avail_budget;
                                    avail_budget = data.Bal_Budget;

                                    window.setTimeout(function () {
                                        //debugger;
                                        component.cellValue(rowIndex, "Available", avail_budget);
                                    }, 1000);
                                },
                                error: function (data) {
                                    //debugger;
                                }
                            });

                        }

                    }

                    if (e.dataField === "Invoice") {
                        //debugger;

                        var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                        e.editorOptions.onValueChanged = function (e) {
                            //debugger;
                            onValueChanged.call(this, e);
                            //debugger;
                            $.ajax({
                                type: "post",
                                url: "/HC_TC/GetBalanceBudget",
                                data: {
                                    BudgPlan: component.cellValue(rowIndex, "Budget_Plan"), Inv: e.value,
                                },

                                datatype: "json",
                                success: function (data) {
                                    //debugger;
                                    var remain_budget;
                                    remain_budget = data.Bal_Budget;

                                    window.setTimeout(function () {
                                        //debugger;
                                        component.cellValue(rowIndex, "Bud_Inv", remain_budget);
                                    }, 1000);
                                },
                                error: function (data) {
                                    //debugger;
                                }
                            });

                            $.ajax({
                                type: "post",
                                url: "/HC_TC/GetAvailableBudget",
                                data: {
                                    Inv: e.value, BudgPlan: component.cellValue(rowIndex, "Budget_Plan"), Open: component.cellValue(rowIndex, "Open")
                                },

                                datatype: "json",
                                success: function (data) {
                                    //debugger;
                                    var avail_budget;
                                    avail_budget = data.Bal_Budget;

                                    window.setTimeout(function () {
                                        //debugger;
                                        component.cellValue(rowIndex, "Available", avail_budget);
                                    }, 1000);
                                },
                                error: function (data) {
                                    //debugger;
                                }
                            });
                        }

                    }



                   

                    if (e.dataField === "Open") {
                        debugger;

                        var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                        e.editorOptions.onValueChanged = function (e) {
                            debugger;
                            onValueChanged.call(this, e);
                            debugger;
                           $.ajax({
                                type: "post",
                                url: "/HC_TC/GetAvailableBudget",
                                data: {
                                    Open: e.value, Inv: component.cellValue(rowIndex, "Invoice"), BudgPlan: component.cellValue(rowIndex, "Budget_Plan")
                                },

                                datatype: "json",
                                success: function (data) {
                                    //debugger;
                                    var avail_budget;
                                    avail_budget = data.Bal_Budget;

                                    window.setTimeout(function () {
                                        //debugger;
                                        component.cellValue(rowIndex, "Available", avail_budget);
                                    }, 1000);
                                },
                                error: function (data) {
                                    //debugger;
                                }
                            });
                        }

                    }



                    //if (e.dataField === "Item_Name") {

                    //    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                    //    e.editorOptions.onValueChanged = function (e) {
                    //        onValueChanged.call(this, e);
                    //        $.ajax({
                    //            type: "post",
                    //            url: "/BudgetingRequest/GetUnitPrice",
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
                    //            url: "/BudgetingRequest/GetCategory",
                    //            data: { ItemName: e.value },
                    //            datatype: "json",
                    //            traditional: true,
                    //            success: function (data) {
                    //                category = data;

                    //            }
                    //        })

                    //        $.ajax({

                    //            type: "post",
                    //            url: "/BudgetingRequest/GetCostElement",
                    //            data: { ItemName: e.value },
                    //            datatype: "json",
                    //            traditional: true,
                    //            success: function (data) {
                    //                costelement = data;

                    //            }
                    //        })

                    //        $.ajax({

                    //            type: "post",
                    //            url: "/BudgetingRequest/GetActualAvailableQuantity",
                    //            data: { ItemName: e.value },
                    //            datatype: "json",
                    //            traditional: true,
                    //            success: function (data) {
                    //                debugger;
                    //                actualavailquantity = data;

                    //            }
                    //        })

                    //        window.setTimeout(function () {

                    //            component.cellValue(rowIndex, "Unit_Price", unitprice);
                    //            component.cellValue(rowIndex, "Category", category);
                    //            component.cellValue(rowIndex, "Cost_Element", costelement);
                    //            component.cellValue(rowIndex, "ActualAvailableQuantity", actualavailquantity);

                    //        },
                    //            1000);


                    //    }

                    //}


                },

                columns: [
                    {
                        type: "buttons",
                        width: 100,
                        alignment: "left",
                        buttons: [
                            "edit", /*"delete",*/
                            //{
                            //    hint: "Submit",
                            //    icon: "check",
                            //    text: "Submit Item Request",
                            //    visible: function (e) {
                            //        if (e.row.data.ApprovalHoE == undefined)
                            //            e.row.data.ApprovalHoE = false;

                            //        return !countdownflag && !e.row.isEditing && !e.row.data.ApprovalHoE/*&& !isOrderApproved(e.row.data.OrderStatus)*/;

                            //    },
                            //    onClick: function (e) {
                            //        HoEApprove(e.row.data.RequestID, filtered_yr);
                            //        e.component.refresh(true);
                            //        e.event.preventDefault();
                            //    }
                            //}
                        ]
                    },
                    {

                        alignment: "center",
                        columns: [

                            {
                                dataField: "Cmmt_Item",
                                //caption: "Dept",
                                validationRules: [{ type: "required" }],
                                //setCellValue: function (rowData, value) {
                                //    debugger;
                                //    rowData.DEPT = value;
                                //    rowData.Group = null;

                                //},
                                width: 130,
                                lookup: {
                                    dataSource: function (options) {
                                        debugger;
                                        return {

                                            store: codelist,
                                            //filter: options.data ? ["Outdated", "=", false] : null


                                        };
                                    },

                                    valueExpr: "Cmmt_ItemCode",
                                    displayExpr: "Cmmt_ItemCode"

                                },



                            },
                            {
                                dataField: "Year",
                                //caption: "Dept",
                                validationRules: [{ type: "required" }],
                                //groupIndex: 0,

                                //setCellValue: function (rowData, value) {
                                //    debugger;
                                //    rowData.DEPT = value;
                                //    rowData.Group = null;

                                //},
                                width: 130,
                                lookup: {
                                    dataSource: function (options) {
                                        debugger;
                                        return {

                                            store: yearlist,
                                            //filter: options.data ? ["Outdated", "=", false] : null


                                        };
                                    },

                                    valueExpr: "Year",
                                    displayExpr: "Year"

                                },



                            },


                            'Budget_Plan',
                            'Invoice',
                            {
                                dataField: "Bud_Inv",
                                caption: "Plan-Invoice",
                               // width: 70,
                                allowEditing: false,
                                setCellValue: function (rowData, value) {
                                    debugger;
                                    rowData.Bud_Inv = value;

                                },

                                //calculateCellValue: function (rowData) {
                                //    //update the bud-inv
                                //    var remain_budget;
                                //    if (rowData.Budget_Plan == undefined || rowData.Budget_Plan == null || rowData.Budget_Plan == 0) {

                                //        remain_budget = 0;
                                //    }
                                //    else if (rowData.Invoice == undefined || rowData.Invoice == null || rowData.Invoice == 0) {

                                //        remain_budget = rowData.Budget_Plan;
                                //    }
                                //    else {

                                //        remain_budget = rowData.Budget_Plan - rowData.Invoice;

                                //    }

                                //    return remain_budget;
                                //}

                            },
                            'Open',
                            {
                                dataField: "Available",
                                //width: 70,
                                allowEditing: false,
                                setCellValue: function (rowData, value) {
                                    debugger;
                                    rowData.Available = value;

                                },

                                //calculateCellValue: function (rowData) {
                                //    debugger;
                                //    //update the bud-inv
                                //    var remain_budget;
                                //    if (rowData.Budget_Plan == undefined || rowData.Budget_Plan == null || rowData.Budget_Plan == 0) {

                                //        remain_budget = 0;
                                //    }
                                //    else if (rowData.Invoice != null && rowData.Invoice != undefined && rowData.Open != null && rowData.Open != undefined) {

                                //        remain_budget = rowData.Budget_Plan - (rowData.Invoice + rowData.Open);
                                //    }
                                //    else if (rowData.Invoice == null && rowData.Invoice == undefined){

                                //        remain_budget = rowData.Budget_Plan - rowData.Open;

                                //    }
                                //    else
                                //        remain_budget = rowData.Budget_Plan - rowData.Invoice;

                                //    return remain_budget;
                                //}

                            },
                            

                        ]
                    }],

               

                onRowUpdated: function (e) {
                    $.notify(" Your Item Request is being Updated...Please wait!", {
                        globalPosition: "top center",
                        className: "success"
                    })
                    Selected = [];
                    debugger;
                    Selected.push(e.data);
                    Update(Selected);
                },

                onRowInserting: function (e) {
                    $.notify("New Item is being added to your cart...Please wait!", {
                        globalPosition: "top center",
                        className: "success"
                    })
                    
                    Selected = [];
                    debugger;
                    Selected.push(e.data);



                    Update(Selected);
                },
                //onRowRemoving: function (e) {

                //    Delete(e.data.RequestID, filtered_yr);

                //}


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


    function Update(id1, filtered_yr) {
        debugger;
        $.ajax({
            type: "POST",
            url: encodeURI("../HC_TC/Save_TCdata"),
            data: { 'req': id1[0]},
            success: function (data) {
                debugger;
                newobjdata = JSON.parse(data.data.Data.Content);

                $("#tc_dataGrid").dxDataGrid({ dataSource: newobjdata });

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

