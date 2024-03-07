var role_list = [];

function GenerateHC() {
    debugger;
    //var LabTypes = $('#selectLabType').val();
    //var Locations = $('#selectLocation').val();


    $.ajax({
        type: "POST",
        url: "../HC_TC/GenerateHC",
        dataType: 'json',
        //traditional: true,
        success: function (data) {
            debugger;
            var textBox;
            var res = JSON.parse(data.data.Data.Content);
            var pivotGrid = $("#hc_dataGrid").dxPivotGrid({
                //allowSortingBySummary: true,
                allowExpandAll: false,
                allowSorting: false,
                allowFiltering: true,
                showBorders: true,
                showColumnGrandTotals: false,
                showRowGrandTotals: true,
                showRowTotals: false,
                showColumnTotals: false,
                //height: 600,
                width: "55vw",
                //columnWidth: 100,
                headerFilter: { visible: true },
                
                onCellPrepared: function (e) {

                   
                    e.cellElement.css("color", "black");
                    if (e.area === "column" /*|| e.area == "row"*/) {
                       // e.cellElement.css("background-color", "antiquewhite");  /*"#4080AD"#005691*/
                        
                        e.cellElement.css("font-weight", "bold");
                        e.cellElement.css("font-size", 15);

                    }
                },
                fieldPanel: {
                    //showColumnFields: true,
                    //showDataFields: true,
                 showRowFields: true,
                    visible: true,
                    showFilterFields: true,
                    allowFieldDragging: true
                },
                fieldChooser: {
                    enabled: false
                    //applyChangesMode: "instantly"
                },
                export: {
                    enabled: true
                },
                scrolling: {
                    //mode: 'virtual',
                    columnRenderingMode: "virtual"
                },
                //paging: {
                //    enabled: true,
                //    pageSize: 10
                //},
                dataSource: {

                    fields: [
                    //    {
                    //    caption: "Role",
                    //    width: 180,
                    //    dataField: "RoleName",
                    //    area: "row",
                    //    expanded: true,

                    //},
                    {
                        caption: "SkillSet",
                        width: 250,
                        dataField: "SkillSetName",
                        area: "row",
                        expanded: true
                    },

                    {
                        dataField: "Year",
                        area: "column",
                       
                        sortBy: "none",
                        //dataType: "date",
                        //customizeText: function (options) {
                        //    return !options.value ? "" : moment(options.value).format("MMM/DD/YYYY").replace(".", "");
                        //},
                        expanded: true

                    },

                    {
                        groupname: "Year",
                        visible: false,
                        expanded: true
                    },
                    {
                        caption: "Plan",
                        dataField: "Plan",
                        dataType: "number",
                        summaryType: "sum",
                        area: "data",
                        format: { type: 'fixedPoint', precision: 2 }  ,
                        visible: true,
                        expanded: true,
                    },
                    {
                        caption: "Utilize",
                        dataField: "Utilize",
                        dataType: "number",
                        format: { type: 'fixedPoint', precision: 3 }  ,
                        summaryType: "sum",
                        area: "data",
                        visible: true,
                        expanded: true,
                    }
                    ],
                    store: res
                },
                onCellClick: function (e) {
                    debugger;
                    if (e.area == 'data' && e.columnIndex % 2 == 1) {
                        debugger;
                        if (textBox) {
                            var parent = textBox.element().parent();
                            textBox.element().remove();
                            textBox = null;
                            parent.children().show();
                            parent.css('padding', e.cellElement.css('padding'));
                        }
                        e.cellElement.children().hide();
                        var oldPadding = e.cellElement.css('padding');
                        e.cellElement.css('padding', 0);
                        textBox = $('<div>')
                            .appendTo(e.cellElement)
                            .on('click', function (ev) {
                                ev.stopPropagation();
                            })
                            .dxTextBox({
                                value: e.cell.value,
                                height: e.cellElement.height(),
                                onValueChanged: function (arg) {
                                    debugger;
                                    const previousValue = arg.previousValue;
                                    const newValue = arg.value;
                                    var cellData = {
                                        row: e.cell.rowPath,
                                        col: e.cell.columnPath
                                    };
                                    debugger;
                                    $.ajax({
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        url: "../HC_TC/saveUtilizeCount",
                                        data: JSON.stringify({ newhc: newValue,/* oldhc: previousValue,*/ row: e.cell.rowPath, column: e.cell.columnPath }),
                                        dataType: 'json',
                                        //traditional: true,
                                        success: function (data) {
                                            debugger;
                                            GenerateHC();
                                            $.notify("HC Utilization updated successfully..!", {
                                                globalPosition: "top center",
                                                className: "success"
                                            })
                                        },
                                        error: function (data) {
                                            debugger;
                                        }
                                    });
                                    //Send newValue and cellData to your data service and update data according to your requirements
                                    textBox.element().remove();
                                    textBox = null;
                                    // Event handling commands go here
                                },
                                //onKeyUp: function (arg) {
                                //    debugger;
                                //    if (arg.event.key == 9) {
                                //        var newValue = arg.component.option('value');
                                //        var cellData = {
                                //            row: e.cell.rowPath,
                                //            col: e.cell.columnPath
                                //        };
                                //        debugger;
                                //        //Send newValue and cellData to your data service and update data according to your requirements
                                //        textBox.element().remove();
                                //        textBox = null;
                                //        //pivotGrid.option('dataSource').reload();
                                //    }
                                //    else {

                                //    }
                                //},
                                onFocusOut: function (arg) {
                                    var parent = textBox.element().parent();
                                    textBox.element().remove();
                                    textBox = null;
                                    parent.children().show();
                                    parent.css('padding', oldPadding);
                                }
                            })
                            .dxTextBox('instance');
                        textBox.focus();
                    }
                }



            }).dxPivotGrid("instance");
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

function GenerateAddHC(role_list1, skill_list, yr_list) {
   
    for (i = 0; i < role_list1.length; i++) {

        //hcarray.push({ Project: res[i].Project_Team.substring(1, res[i].Project_Team.length - 1)  });
        role_list.push(role_list1[i].RoleName);

    }
    debugger;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../HC_TC/GenerateAddHC",
        dataType: 'json',
        //traditional: true,
        success: addHC_success,
           
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
    function addHC_success(data) {
        debugger;

        var res = JSON.parse(data.data.Data.Content);
        var hcarray = [];
        for (i = 0; i < res.length; i++) { // to split the comma sep Roles into list and store as datasource - ince tgbox only accepts if array

            //hcarray.push({ Project: res[i].Project_Team.substring(1, res[i].Project_Team.length - 1)  });
            hcarray.push({ Role: res[i].Role.split(','), SkillSet: res[i].SkillSet, Year: res[i].Year, Plan: res[i].Plan, Utilize: res[i].Utilize, ID: res[i].ID });

        }
        //var res1 = eval(res);
        $("#hc_dataGrid_withAdd").dxDataGrid({

            dataSource: hcarray,
            keyExpr: "ID",
            columnResizingMode: "nextColumn",
            columnMinWidth: 50,
            stateStoring: {
                enabled: true,
                type: "localStorage",
                storageKey: "ID"
            },
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
                            dataField: "Role",


                            editCellTemplate: tagBoxEditorTemplate,
                            lookup: {
                                dataSource: function (options) {

                                    return {

                                        store: role_list,
                                    };
                                },

                                //valueExpr: "ID",
                                //displayExpr: "Location"

                            },
                            cellTemplate(container, options) {
                                debugger;
                                var items;
                                //if (options.value.includes("[")) {
                                //    items = eval(options.value);
                                //}
                                //else
                                items = options.value;
                                const noBreakSpace = '\u00A0';
                                const text = (items || []).map((element) => options.column.lookup.calculateCellValue(element)).join(', ');
                                container.text(text || noBreakSpace).attr('title', text);
                            },
                            calculateFilterExpression(filterValue, selectedFilterOperation, target) {
                                debugger;
                                if (target === 'search' && typeof (filterValue) === 'string') {
                                    return [this.dataField, 'contains', filterValue];
                                }
                                return function (data) {
                                    debugger;
                                    return (data.Project_Team || []).indexOf(filterValue) !== -1;
                                };
                            },


                        },
                        {
                            dataField: "SkillSet",

                            validationRules: [{ type: "required" }],
                            lookup: {
                                dataSource: function (options) {

                                    return {
                                        store: skill_list,
                                    };
                                },
                                valueExpr: "ID",
                                displayExpr: "SkillSetName"
                            },

                        },
                        {
                            dataField: "Year",
                            width: 100,
                            validationRules: [{ type: "required" }],
                            // editCellTemplate: tagBoxEditorTemplate,
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
                            dataField: "Plan",
                            width: 100,
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

                                rowData.Plan = value;

                            },


                        },

                        {
                            dataField: "Utilize",
                            width: 100,

                        }

                    ]
                }],

            onRowUpdated: function (e) {
                $.notify("Please wait, Updating details..!", {
                    globalPosition: "top center",
                    className: "success"
                })
                Selected = [];
                debugger;
                Selected.push(e.data);
                Update(Selected);
            },

            onRowInserting: function (e) {
                $.notify("Please wait, Saving details..!", {
                    globalPosition: "top center",
                    className: "success"
                })

                Selected = [];
                debugger;

                Selected.push(e.data);



                Update(Selected);
            },
            onRowRemoving: function (e) {

                Delete(e.data.ID/*, filtered_yr*/);

            }


        });
        function tagBoxEditorTemplate(cellElement, cellInfo) {
            debugger;
            return $('<div>').dxTagBox({
                items: role_list,
                value: cellInfo.value,
                //valueExpr: 'OEM',
                //displayExpr: 'OEM',
                showSelectionControls: true,
                searchEnabled: true,
                //maxDisplayedTags: 3,
                //showMultiTagOnly: false,
                //applyValueMode: 'useButtons',
                searchEnabled: true,
                onValueChanged(e) {
                    debugger;
                    cellInfo.setValue(e.value);
                },
                onSelectionChanged() {
                    debugger;
                    cellInfo.component.updateDimensions();
                },
                acceptCustomValue: true,
                onCustomItemCreating(args) {
                    debugger;
                    const newValue = args.text;
                    const { component } = args;
                    const currentItems = component.option('items');
                    currentItems.unshift(newValue);
                    component.option('items', currentItems);
                    args.customItem = newValue;
                    //return newValue;
                    // Generates a new 'id'
                    //let nextId = 1000;
                    ////selectBoxData.store().totalCount().done(count => { nextId = count + 1 });
                    //// Creates a new entry
                    //args.customItem = { ID: 1000, OEM: args.text };
                    //// Adds the entry to the data source
                    //project_list.store().insert(args.customItem);
                    // Reloads the data source
                    // project_list.reload();
                    //cellInfo.setValue(1000);

                    //    debugger;
                    //var filterExpression = [];   
                    ////filterExpression.push(['ID', nextId]);
                    //var processedArray = DevExpress.data.query(project_list._store._array).filter(["OEM", "=", args.text]).select("ID").toArray();
                    //var newItem = {};
                    //newItem.ID = (processedArray.length == 0 ? args.text : processedArray[0].ID);
                    //newItem.NAME = args.text;
                    //args.customItem = newItem;
                    //cellInfo.value.push(1000);
                    //for (var i = 0; i < cellInfo.value.length; i++) {
                    //    if (i > 0) {
                    //        filterExpression.push('or');
                    //    }
                    //    filterExpression.push(['ID', cellInfo.value[i]]);
                    //}
                    //debugger;
                    //var result = $.map(DevExpress.data.query(project_list._store._array).filter(filterExpression).toArray(), function (item) {
                    //    return item.OEM;
                    //}).join(',');
                    //return result;




                    //return newValue;
                },





            });
        }

    }


    function Update(id1/*, filtered_yr*/) {
        debugger;
        $.ajax({
            type: "POST",
            url: encodeURI("../HC_TC/Save_HCdata"),
            data: { 'req': id1[0] },
            success: function (data) {
                debugger;
                //newobjdata = JSON.parse(data.data.Data.Content);
                addHC_success(data);
                //$("#hc_dataGrid_withAdd").dxDataGrid({ dataSource: newobjdata });

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

    function Delete(id/*, filtered_yr*/) {

        $.ajax({
            type: "POST",
            url: "/HC_TC/Delete",
            data: { 'id': id/*, 'useryear': filtered_yr*/ },
            success: function (data) {

                //$("#hc_dataGrid_withAdd").dxDataGrid({ dataSource: JSON.parse(data.data.Data.Content) });

                addHC_success(data);
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