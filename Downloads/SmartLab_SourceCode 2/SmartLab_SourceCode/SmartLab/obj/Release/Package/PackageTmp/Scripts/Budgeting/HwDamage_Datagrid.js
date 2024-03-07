
function GenerateHwDamage(month_list, loc_list, item_list, project_list, hwstatus_list, yr_list, bu_list, oems_chosen, UserAuthorizeToEdit, filtered_yr) {
    debugger;
    var CostEUR;
    var autoFill_Location;
    var autoFill_Month;
    var autoFill_Year;
    var autoFill_Project_Team;
    var autoFill_Hw_status;
    var autoFill_Qty;
    var isautoFill_avail = false;
    debugger;
  
    debugger;
    $.ajax({
        type: "POST",
        //contentType: "application/json; charset=utf-8",
        url: "../HwDamage/GenerateHwDamage",
        data: { /*'oem': oems_chosen*/ 'yr': filtered_yr},
        //dataType: 'json',
        //traditional: true,
        success: function (data) {
            debugger;
           
            var res = JSON.parse(data.data.Data.Content);
           
            //var res1 = eval(res);
            $("#hwDamage_dataGrid").dxDataGrid({

                dataSource: res,
                //keyExpr: "ID",
                width: "97vw",  
                columnResizingMode: "nextColumn",
                columnMinWidth: 50,
                scrolling: {
                    //columnRenderingMode: 'virtual',
                    mode: "virtual",
                    rowRenderingMode: "virtual",
                    columnRenderingMode: "virtual"
                },
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
                        e.cellElement.addClass("columnHeaderCSS");
                        e.cellElement.find("input").addClass("columnHeaderCSS"); 

                        e.cellElement.css("color", "black");
                        e.cellElement.css("font-weight", "bold");
                    }

                    
                },
                noDataText: " ☺ No Damages available until now for this year!",
                editing: {
                    mode: "row",
                    allowUpdating: function (e) {
                        
                        return UserAuthorizeToEdit;
                    },
                    allowDeleting: function (e) {
                      
                        return UserAuthorizeToEdit;
                    },
                    allowAdding: function (e) {
                        
                        return UserAuthorizeToEdit;
                    },
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
                //paging: {
                //    pageSize: 100
                //},
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
                    
                    if (e.dataField === "Hw_status") {
                        
                        var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                        
                        e.editorOptions.onValueChanged = function (e) {
                            
                            onValueChanged.call(this, e);
                            debugger;
                            var Item_sel = component.cellValue(rowIndex, "Item_Name");
                            if (component.cellValue(rowIndex, "Item_Name") != undefined && component.cellValue(rowIndex, "Item_Name") != null) {
                                debugger;
                                $.ajax({

                                    type: "post",
                                    url: "/HwDamage/GetCostEUR",
                                    data: { Hw_status: e.value, ItemName: component.cellValue(rowIndex, "Item_Name")},
                                    datatype: "json",
                                    traditional: true,
                                    success: function (data) {
                                        debugger;
                                        if (data.msg) {
                                            CostEUR = "";
                                            window.setTimeout(function () {
                                                debugger;
                                                component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
                                            }, 1000);

                                            $.notify(data.msg, {
                                                globalPosition: "top center",
                                                className: "success"
                                            })
                                        }
                                        else {


                                            CostEUR = data.Cost_EUR;
                                            window.setTimeout(function () {
                                                debugger;
                                                component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
                                            }, 1000);
                                        }
                                       

                                    }
                                })
                                //// Emulating a web service call
                            
                            }
                            
                            
                        }
                    }
                    if (e.dataField === "Item_Name") {

                        var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data

                        e.editorOptions.onValueChanged = function (e) {

                            onValueChanged.call(this, e);
                            debugger;
                            var status_sel = component.cellValue(rowIndex, "Hw_status");
                            if (status_sel != undefined && status_sel != null) {
                                $.ajax({

                                    type: "post",
                                    url: "/HwDamage/GetCostEUR",
                                    data: { Hw_status: status_sel, ItemName: e.value },
                                    datatype: "json",
                                    traditional: true,
                                    success: function (data) {
                                        debugger;
                                        if (data.msg) {
                                            CostEUR = "";
                                            window.setTimeout(function () {
                                                debugger;
                                                component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
                                            }, 1000);

                                            $.notify(data.msg, {
                                                globalPosition: "top center",
                                                className: "warn"
                                            })
                                        }
                                        else {
                                            CostEUR = data.Cost_EUR;
                                            //// Emulating a web service call
                                            window.setTimeout(function () {
                                                debugger;
                                                component.cellValue(rowIndex, "Cost_inEUR", CostEUR);
                                            }, 1000);
                                        }
                                            


                                    }
                                })
                                
                            }


                        }
                    }
                },


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
                                dataField: "Year",
                                width: 90,

                                validationRules: [{ type: "required" }],
                                lookup: {
                                    dataSource: function (options) {

                                        return {
                                            store: yr_list,
                                        };
                                    },
                                    valueExpr: "Year",
                                    displayExpr: "Year"
                                },

                            },
                            {
                                dataField: "BU",
                                width: 60,

                                validationRules: [{ type: "required" }],
                                lookup: {
                                    dataSource: function (options) {

                                        return {
                                            store: bu_list,
                                        };
                                    },
                                    valueExpr: "ID",
                                    displayExpr: "BU"
                                },

                            },
                            {
                                dataField: "Location",
                                setCellValue: function (rowData, value) {
                                    debugger;
                                    rowData.Location = value;

                                },
                                width: 100,
                                lookup: {
                                    dataSource: loc_list,
                                    valueExpr: "ID",
                                    displayExpr: "Location"

                                },



                            },
                            
                            {
                                dataField: "Month",
                                width: 100,
                                validationRules: [{ type: "required" }],
                               // editCellTemplate: tagBoxEditorTemplate,
                                lookup: {
                                    dataSource: function (options) {
                                       
                                        return {
                                            store: month_list,
                                        };
                                    },
                                    valueExpr: "ID",
                                    displayExpr: "Month"
                                },
                                //cellTemplate(container, options) {
                                //    debugger;
                                //    const noBreakSpace = '\u00A0';
                                //    const text = (options.value || []).map((element) => options.column.lookup.calculateCellValue(element)).join(', ');
                                //    container.text(text || noBreakSpace).attr('title', text);
                                //},
                                //calculateFilterExpression(filterValue, selectedFilterOperation, target) {
                                //    debugger;
                                //    if (target === 'search' && typeof (filterValue) === 'string') {
                                //        return [this.dataField, 'contains', filterValue];
                                //    }
                                //    return function (data) {
                                //        return (data.Month || []).indexOf(filterValue) !== -1;
                                //    };
                                //},
                                //calculateDisplayValue: function (rowData) {
                                //    debugger;
                                //    var filterExpression = [],
                                //        values = rowData.Month;
                                //    for (var i = 0; i < values.length; i++) {
                                //        debugger;
                                //        if (i > 0) {
                                //            filterExpression.push('or');
                                //        }
                                //        filterExpression.push(['ID', values[i]]);
                                //    }
                                //    var result = $.map(DevExpress.data.query(month_list).filter(filterExpression).toArray(), function (item) {
                                //        debugger;
                                //        return item.Name;
                                //    }).join(',');
                                //    return result;
                                //},
                                //calculateFilterExpression: function (filterValues, selectedFilterOperation) {
                                //    console.log(new Date().toString());
                                //    return function (itemData) {
                                //        var array1 = itemData.Month;
                                //        var array2 = filterValues;

                                //        if (array2.length === 0)
                                //            return true;

                                //        return array1.length === array2.length && array1.every(function (value, index) { return value === array2[index] })
                                //    };
                                //}
                            },
                            {
                                dataField: "Item_Name",  
                                width: 200,
                                //caption: "Dept",
                                validationRules: [{ type: "required" }],
                                lookup: {
                                    dataSource: function (options) {
                                       
                                        return {

                                            store: item_list,
                                            filter: options.data ? [/*["Deleted", "=", false], 'and',*/ ["VKM_Year", "=", new Date().getFullYear()] ] : null //returns the current year & not deleted

                                           
                                            // filter: options.data ? [["BU", "=", BU_forItemFilter/*options.data.BU*/], 'and', ["Deleted", "=", false]] : null

                                            //where Deleted = " + 0 + " AND VKM_Year = " + DateTime.Now.Year + "
                                        };
                                    },

                                    valueExpr: "S#No",
                                    displayExpr: "Item Name"

                                },




                            },

                            {
                                dataField: "Qty",
                                width: 60,
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

                                    rowData.Qty = value;

                                },


                            },
                            {
                                dataField: "Project_Team",//month_list, loc_list, item_list, project_list
                                caption: "OEM",
                               
                                validationRules: [{ type: "required" }],
                                lookup: {
                                    dataSource: function (options) {
                                        debugger;
                                        return {

                                            store: project_list,
                                            //filter: options.data ? ["Outdated", "=", false] : null


                                        };
                                    },

                                    valueExpr: "ID",
                                    displayExpr: "OEM"

                                },



                            },
                            {
                                dataField: "Hw_status",//month_list, loc_list, item_list, project_list
                                //caption: "Dept",
                                validationRules: [{ type: "required" }],
                                lookup: {
                                    dataSource: function (options) {
                                       
                                        return {

                                            store: hwstatus_list,
                                            //filter: options.data ? ["Outdated", "=", false] : null


                                        };
                                    },

                                    valueExpr: "Status",
                                    displayExpr: "Status"

                                },
                            },
                                
                            
                            {
                                dataField: "Cost_inEUR",
                                dataType: "number",
                                caption: "Cost (in €)",
                                format: {
                                    type: "fixedPoint",
                                    precision: 0
                                },
                                allowEditing: false,
                                //width: 70,

                            },
                         
                            {
                                dataField: "Total_Price",
                                caption: "Total Price (in €)",
                                format: {
                                    type: "fixedPoint",
                                    precision: 0
                                },
                                allowEditing: false,
                                //width: 70,


                                calculateCellValue: function (rowData) {
                                    //update the bud-inv
                                    var total;
                                    if (rowData.Qty == undefined || rowData.Cost_inEUR == null || rowData.Qty == 0 || rowData.Cost_inEUR == undefined || rowData.Cost_inEUR == 0.0 || rowData.Qty == null) {

                                        total = 0.0;
                                    }
                                    else if (rowData.Qty != null && rowData.Qty != undefined && rowData.Cost_inEUR != null && rowData.Cost_inEUR != undefined) {

                                        total = rowData.Qty * rowData.Cost_inEUR;
                                    }
                                    

                                    return total;
                                }

                            },
                            {
                                dataField: "RequestorNT",
                                caption: "Updated By",
                                width: 120,
                                allowEditing: false
                            },
                            {
                                dataField: "Remarks",
                                
                                width: 120,
                               
                            }
                            

                        ]
                    }],
                summary: {
                    totalItems: [{
                        column: 'Item_Name',
                        summaryType: 'count',
                    },  {
                            column: 'Total_Price',
                            summaryType: 'sum',
                       // valueFormat: "number",
                        //valueFormat: 'currency',
                            customizeText: function (e) {
                                debugger;
                                //I tried add 
                                //console.log(e.value)
                                return "Sum: €" + e.value.toFixed(2);;
                            },
                    }],
                },
                onInitNewRow: function (e) {
                    debugger;
                    //e.data.Requestor = new_request.Requestor;
                    $.ajax({
                        type: "POST",
                        async: false,
                        url: encodeURI("../HwDamage/GetDetails_basedonUser"),
                        // data: { 'req': id1[0] },
                        success: function (data) {
                            debugger;

                            if (data.success == true) {
                                isautoFill_avail = true;
                                autoFill_Location = data.Location;
                                autoFill_Month = data.Month;
                                autoFill_Year = data.Year;
                                autoFill_Project_Team = data.Project_Team;
                                autoFill_Hw_status = data.Hw_status;
                                autoFill_Qty = data.Qty;
                                autoFill_BU = data.BU;
                                    e.data.Location = autoFill_Location;
                                    e.data.Month = autoFill_Month;
                                    e.data.Year = autoFill_Year;
                                    e.data.Project_Team = autoFill_Project_Team;
                                    e.data.Hw_status = autoFill_Hw_status;
                                    e.data.Qty = autoFill_Qty;
                                    e.data.BU = autoFill_BU;
                                    isautoFill_avail = false;
                               
                            }

                        }

                    });

                    
                },
                onRowUpdated: function (e) {
                    $.notify("Please wait, Updating details..!", {
                        globalPosition: "top center",
                        className: "success"
                    })
                    Selected = [];
                    e.data.Total_Price = e.data.Qty * e.data.Cost_inEUR;
                    if (e.data.Remarks == null)
                        e.data.Remarks = "   ";
                    debugger;
                    Selected.push(e.data);
                    Update(Selected, filtered_yr);
                },

                onRowInserting: function (e) {
                    $.notify("Please wait, Saving details..!", {
                        globalPosition: "top center",
                        className: "success"
                    })
                    
                    Selected = [];
                    debugger;
                    e.data.Total_Price = e.data.Qty * e.data.Cost_inEUR;
                    if (e.data.Remarks == null)
                        e.data.Remarks = "   ";

                    Selected.push(e.data);



                    Update(Selected, filtered_yr);
                },
                onRowRemoving: function (e) {

                    Delete(e.data.ID, filtered_yr);

                }


            });
            //function tagBoxEditorTemplate(cellElement, cellInfo) {
            //    debugger;
            //    return $('<div>').dxTagBox({
            //        dataSource: month_list,
            //        value: cellInfo.value,
            //        //valueExpr: 'ID',
            //        //displayExpr: 'FullName',
            //        showSelectionControls: true,
            //        maxDisplayedTags: 3,
            //        showMultiTagOnly: false,
            //        applyValueMode: 'useButtons',
            //        searchEnabled: true,
            //        onValueChanged(e) {
            //            debugger;
            //            cellInfo.setValue(e.value);
            //        },
            //        onSelectionChanged() {
            //            debugger;
            //            cellInfo.component.updateDimensions();
            //        },
            //    });
            //}
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
            url: encodeURI("../HwDamage/Save_HwDamagedata"),
            data: { 'req': id1[0], 'yr': filtered_yr/*, 'oem': oems_chosen*/ },
            success: function (data) {
                debugger;
                newobjdata = JSON.parse(data.data.Data.Content);

                $("#hwDamage_dataGrid").dxDataGrid({ dataSource: newobjdata });

                if (data.success == true) {
                    $.notify("Success", {
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

    function Delete(id, filtered_yr) {

        $.ajax({
            type: "POST",
            url: "/HwDamage/Delete",
            data: { 'id': id, 'yr': filtered_yr/*, 'oem': oems_chosen*/ /*, 'useryear': filtered_yr*/ },
            success: function (data) {
                
                $("#hwDamage_dataGrid").dxDataGrid({ dataSource: JSON.parse(data.data.Data.Content) });


                if (data.success == false) {
                    $.notify("Try again", {
                        globalPosition: "top center",
                        className: "error",
                        autoHideDelay: 13000,
                    })
                }
                else {
                    $.notify("Deleted Successfully!", {
                        globalPosition: "top center",
                        className: "success",
                        autoHideDelay: 10000,
                    })
                }


            }

        });
    }


}
