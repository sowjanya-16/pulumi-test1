
var vkm_role_list = [];
var filtered_yr;
var Is_VKMSPOCFlag = false;



function IsVKMSpoc() {
    debugger;
    $.ajax({

        type: "GET",
        url: "/GRMEMSupport/Get_IsVKMSpoc",
        datatype: "json",
        async: true,
        success: function (data) {
            debugger;
            if (data.data == "1") {
                Is_VKMSPOCFlag = true;
            }
            else {
                Is_VKMSPOCFlag = false;
            }
        },
        error: function (data) {
            debugger;
            Is_VKMSPOCFlag = false;
        },
    });
}





function GenerateGRMEM(product_area_list, section_list, department_list, group_list, sap_role_list, vkm_role_list1, yr_list, spoton_emp_list) {
   
    
    for (i = 0; i < vkm_role_list1.length; i++) {

        //grmemarray.push({ Project: res[i].Project_Team.substring(1, res[i].Project_Team.length - 1)  });
        vkm_role_list.push(vkm_role_list1[i].SkillSetName);

    }

    var month_array = [];
    month_array.push("Jan");
    month_array.push("Feb");
    month_array.push("Mar");
    month_array.push("Apr");
    month_array.push("May");
    month_array.push("Jun");
    month_array.push("Jul");
    month_array.push("Aug");
    month_array.push("Sep");
    month_array.push("Oct");
    month_array.push("Nov");
    month_array.push("Dec");

    
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "../GRMEMSupport/GenerateGRMEM",
        data: { 'year': filtered_yr },
        //dataType: 'json',
        //traditional: true,
        async: false,
        success: addGRMEM_success,

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
    function addGRMEM_success(data) {
       
        var emp_name, emp_no, section, dept, grp, saprole, yr, pyo, plan_sum, level, NTID;
        var res = JSON.parse(data.data.Data.Content);
        var grmemarray = [];
        for (i = 0; i < res.length; i++) { // to split the comma sep Roles into list and store as datasource - ince tgbox only accepts if array

            //grmemarray.push({ Project: res[i].Project_Team.substring(1, res[i].Project_Team.length - 1)  });
            grmemarray.push({
                SAP_Role: res[i].SAP_Role,
                VKM_Role: res[i].VKM_Role1.split(','),
                Year: res[i].Year,
                SNo: res[i].SNo,
                NTID: res[i].NTID,
                Employee_Number     : res[i].Employee_Number,
                Employee_Name       : res[i].Employee_Name  ,
                Product_Area        : res[i].Product_Area_ID 	,
                Section             : res[i].Section_ID 		,
                Department          : res[i].Department_ID 	,
                Group               : res[i].Group_ID 		    ,
                Level               : res[i].Level 		    ,
                //SAP_Role          : = res[i].SAP_Role,
                //VKM_Role          : = res[i].RoleName,
                Remarks             : res[i].Remarks    	,	
                Plan_Sum            : res[i].Plan_Sum   	,	
                Jan                 : res[i].Jan   			,
                Feb                 : res[i].Feb   			,
                Mar                 : res[i].Mar   			,
                Apr                 : res[i].Apr   			,
                May                 : res[i].May   			,
                Jun                 : res[i].Jun   			,
                Jul                 : res[i].Jul   			,
                Aug                 : res[i].Aug   			,
                Sep                 : res[i].Sep   			,
                Oct                 : res[i].Oct   			,
                Nov                 : res[i].Nov   			,
                Dec                 : res[i].Dec   			,
                PYO                 : res[i].PYO   			,
                Updated_By: res[i].Updated_By,
                Updated_At: new Date(parseInt(res[i].Updated_At.substr(6)))
            });

        }
        //debugger;

        //var res1 = eval(res);
        var dataGrid;
        var needSummaryUpdate = false;

        $("#grmem_dataGrid").dxDataGrid({

            dataSource: grmemarray,
            keyExpr: "SNo",
            //columnResizingMode: "nextColumn",
            //columnMinWidth: 50,
            //stateStoring: {
            //    enabled: true,
            //    type: "localStorage",
            //    storageKey: "SNo"
            //},

            onContentReady: function (e) {
               
                e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());

              
                $("#Sum").text("Sum: " + e.component.getTotalSummaryValue("Sum_Plan"));  
                $("#1").text("Jan: " + e.component.getTotalSummaryValue("Jan"));  
                $("#2").text("Feb: " + e.component.getTotalSummaryValue("Feb"));  
                $("#3").text("Mar: " + e.component.getTotalSummaryValue("Mar"));  
                $("#4").text("Apr: " + e.component.getTotalSummaryValue("Apr"));  
                $("#5").text("May: " + e.component.getTotalSummaryValue("May"));  
                $("#6").text("Jun: " + e.component.getTotalSummaryValue("Jun"));  
                $("#7").text("Jul: " + e.component.getTotalSummaryValue("Jul"));  
                $("#8").text("Aug: " + e.component.getTotalSummaryValue("Aug"));  
                $("#9").text("Sep: " + e.component.getTotalSummaryValue("Sep"));  
                $("#10").text("Oct: " + e.component.getTotalSummaryValue("Oct"));  
                $("#11").text("Nov: " + e.component.getTotalSummaryValue("Nov"));  
                $("#12").text("Dec: " + e.component.getTotalSummaryValue("Dec"));  
                $("#PYO").text("PYO: " + e.component.getTotalSummaryValue("PYO"));  
              
                //dataGrid = e.component;
                //if (needSummaryUpdate) {
                //    needSummaryUpdate = true;
                //    //e.component.refresh();
                //}

                //e.element.find(".dx-datagrid-total-footer")
                //    .css("border-top", 0)
                //    .css("border-bottom", "1px solid #d3d3d3")
                //    .insertBefore(e.element.find(".dx-datagrid-rowsview"))

            },
          
            onCellPrepared: function (e) {
                if (e.rowType === "header" || e.rowType === "filter") {
                    e.cellElement.css("color", "black");
                    e.cellElement.css("font-weight", "bold");

                    e.cellElement.addClass("columnHeaderCSS");
                    e.cellElement.find("input").addClass("columnHeaderCSS");
                }
            },
            toolbar: {
                items: [
                    'addRowButton',
                    'columnChooserButton',
                    {
                        location: 'after',
                        widget: 'dxButton',
                        options: {
                            icon: 'refresh',
                            text: 'Clear Filters',
                            hint: 'Clear HeadCount Input Filters',
                            onClick() {
                                $("#RequestTable").dxDataGrid("clearFilter");
                            },
                        },


                    }
                ]
            },
            onToolbarPreparing: function (e) {
                let toolbarItems = e.toolbarOptions.items;

                let addRowButton = toolbarItems.find(x => x.name === "addRowButton");
                if (addRowButton.options != undefined) { //undefined when any of the previous vkm year selected and add button is hidden
                    addRowButton.options.text = "Add HeadCount Input";
                    addRowButton.options.hint = "Add HeadCount Input";
                    addRowButton.showText = "always";
                }

                let columnChooserButton = toolbarItems.find(x => x.name === "columnChooserButton");
                columnChooserButton.options.text = "Hide Fields";
                columnChooserButton.options.hint = "Hide Fields";
                columnChooserButton.showText = "always";

            },
            noDataText: " ☺ EM/Support persons input data not available ! Click '+' Add a row option on the top right",
            editing: {
                mode: "row",
                allowUpdating: function (e) {
                    //return true;
                    return Is_VKMSPOCFlag;
                },
                allowDeleting: Is_VKMSPOCFlag,
                allowAdding: Is_VKMSPOCFlag,
                useIcons: Is_VKMSPOCFlag
                //allowDeleting: true,
                //allowAdding: true,
                //useIcons: true
            },
            export: {
                enabled: true,
                fileName: "VKM HeadCount Planning_" + String((new Date()).getDate()).padStart(2, '0') + '.' + String((new Date()).getMonth() + 1).padStart(2, '0') + '.' + (new Date()).getFullYear() //dd/mm/yyyy
                //allowExportSelectedData: true
            },
            //onToolbarPreparing: function (e) {
               
            //    var dataGrid = e.component;

            //    e.toolbarOptions.items[0].showText = 'always';


            //},
            //focusedRowEnabled: true,
            showColumnLines: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            allowColumnReordering: true,
            allowColumnResizing: true,
            hoverStateEnabled: {
                enabled: true
            },
            columnChooser: {
                enabled: true
            },

            filterRow: {
                visible: false

            },
            showBorders: true,
            headerFilter: {
                visible: true,
                applyFilter: "auto",
                allowSearch: true
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
            width: "95vw",    //1590, //needed to allow horizontal scroll
            //height: 620,
            columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
            //remoteOperations: { groupPaging: true },
            scrolling: {
                //columnRenderingMode: 'virtual',
                mode: "virtual",
                //rowRenderingMode: "virtual",
                //columnRenderingMode: "virtual"
            },
             repaintChangesOnly: true,
            columnFixing: {
                enabled: true,
            },
            grouping: {
                autoExpandAll: true,
            },
            groupPanel: {
                visible: true,
            },
            //wordWrapEnabled: true,
            searchPanel: {
                visible: true,
                width: 240,
                placeholder: "Search..."
            },
             
            columns: [
                //{
                //    type: "buttons",
                //    alignment: "left",
                //    buttons: [
                //        "edit", "delete"

                //    ]
                //},
                {
                    dataField: "SNo",
                    visible: false
                },
                
                {
                    dataField: "Employee_Number",
                    caption: "Emp No",
                    width: 100,
                    visible: true,
                    allowEditing: false
                },
                {
                    dataField: "Employee_Name",
                    fixed: true,
                    caption: "Emp Name",
                    visible: true,
                    minWidth: 150,
                    //editCellTemplate: AutocompleteEditorTemplate,
                    lookup: {
                        dataSource: function (options) {
                            debugger;
                            return {
                                store: spoton_emp_list,
                            };
                        },
                        valueExpr: "Employee_Name",
                        displayExpr: "Employee_Name"
                    },
                },
                {
                    dataField: "NTID",
                    width: 150,
                    fixed: true,
                    disabled: true,
                    allowEditing: false
                    //lookup: {
                    //    dataSource: function (options) {
                    //        debugger;
                    //        return {
                    //            store: spoton_emp_list,
                    //        };
                    //    },
                    //    valueExpr: "NTID",
                    //    displayExpr: "NTID"
                    //},
                },
                {
                    dataField: "SAP_Role",
                    visible: false,
                    allowEditing: false,
                    minWidth: 150,
                    lookup: {
                        dataSource: function (options) {
                            debugger;
                            return {
                                store: sap_role_list,
                            };
                        },
                        valueExpr: "ID",
                        displayExpr: "RoleName"
                    },

                },
                {
                    dataField: "VKM_Role",
                    width: 300,
                    validationRules: [{ type: "required" }],
                    

                    //editCellTemplate: tagBoxEditorTemplate_vkmrole,
                    lookup: {
                        dataSource: function (options) {

                            return {

                                store: vkm_role_list,
                            };
                        },

                        //valueExpr: "ID",
                        //displayExpr: "Location"

                    },
                    //cellTemplate(container, options) {
                    //    debugger;
                    //    var items;
                    //    //if (options.value.includes("[")) {
                    //    //    items = eval(options.value);
                    //    //}
                    //    //else
                    //    items = options.value;
                    //    const noBreakSpace = '\u00A0';
                    //    const text = (items || []).map((element) => options.column.lookup.calculateCellValue(element)).join(', ');
                    //    container.text(text || noBreakSpace).attr('title', text);
                    //},
                    //calculateFilterExpression(filterValue, selectedFilterOperation, target) {
                    //    debugger;
                    //    if (target === 'search' && typeof (filterValue) === 'string') {
                    //        return [this.dataField, 'contains', filterValue];
                    //    }
                    //    return function (data) {
                    //        debugger;
                    //        return (data.Project_Team || []).indexOf(filterValue) !== -1;
                    //    };
                    //},



                },
                {
                    dataField: "Product_Area",
                    width: 70,
                    caption: "BU",
                    visible: true,
                    allowEditing: false,
                    lookup: {
                        dataSource: function (options) {

                            return {

                                store: product_area_list

                                //filter: options.data ? ["Outdated", "=", true] : null


                            };
                        },

                        valueExpr: "ID",
                        displayExpr: "BU"

                    },
                },
                {
                    dataField: "Section",
                    visible: true,
                    allowEditing: false,
                    minWidth: 100,
                    lookup: {
                        dataSource: function (options) {

                            return {

                                store: section_list

                                //filter: options.data ? ["Outdated", "=", true] : null


                            };
                        },

                        valueExpr: "ID",
                        displayExpr: "Section"

                    },
                },
                {
                    dataField: "Department",
                    caption: "Dept",
                    allowEditing: false,
                    minWidth: 150,
                    visible: true,
                    //groupIndex: 0,
                    // fixed: true,
                    lookup: {
                        dataSource: function (options) {

                            return {

                                store: department_list

                                //filter: options.data ? ["Outdated", "=", true] : null


                            };
                        },

                        valueExpr: "ID",
                        displayExpr: "DEPT"

                    },
                },
                {
                    dataField: "Group",
                    minWidth: 150,
                    allowEditing: false,
                    lookup: {
                        dataSource: function (options) {
                            debugger;
                            return {

                                store: group_list,

                                filter: options.data ? ["Dept", "=", options.data.Department] : null


                            };
                        },

                        valueExpr: "ID",
                        displayExpr: "Group"

                    },
                },
                {
                    dataField: "Level",
                    width: 100,
                    visible: true,
                    allowEditing: false

                },
                {
                    dataField: "Year",
                    width: 100,
                    //visible: false,
                    dataType: "number",
                    validationRules: [{ type: "required" }],
                    // editCellTemplate: tagBoxEditorTemplate_saprole,

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
                    dataField: "Plan_Sum",
                    width: 100,
                    allowEditing: false,
                    //calculateCellValue: function (rowData) {
                    //    //update 
                    //    //debugger;
                    //    var ps = 0.0;
                    //    //for (i = 0; i < month_array.length; i++) {
                    //    //    //debugger;
                    //        //if (rowData.month_array[i] != undefined && rowData.month_array[i] != null) {
                    //        //    ps += rowData.month_array[i];
                    //        //}
                    //        if (rowData.Jan != undefined && rowData.Jan != null) {
                    //            ps += rowData.Jan;
                    //        }
                    //        if (rowData.Feb != undefined && rowData.Feb != null) {
                    //            ps += rowData.Feb;
                    //        }
                    //        if (rowData.Mar != undefined && rowData.Mar != null) {
                    //            ps += rowData.Mar;
                    //        }
                    //        if (rowData.Apr != undefined && rowData.Apr != null) {
                    //            ps += rowData.Apr;
                    //        }
                    //        if (rowData.May != undefined && rowData.May != null) {
                    //            ps += rowData.May;
                    //        }
                    //        if (rowData.Jun != undefined && rowData.Jun != null) {
                    //            ps += rowData.Jun;
                    //        }
                    //        if (rowData.Jul != undefined && rowData.Jul != null) {
                    //            ps += rowData.Jul;
                    //        }
                    //        if (rowData.Aug != undefined && rowData.Aug != null) {
                    //            ps += rowData.Aug;
                    //        }
                    //        if (rowData.Sep != undefined && rowData.Sep != null) {
                    //            ps += rowData.Sep;
                    //        }
                    //        if (rowData.Oct != undefined && rowData.Oct != null) {
                    //            ps += rowData.Oct;
                    //        }
                    //        if (rowData.Nov != undefined && rowData.Nov != null) {
                    //            ps += rowData.Nov;
                    //        }
                    //        if (rowData.Dec != undefined && rowData.Dec != null) {
                    //            ps += rowData.Dec;
                    //        }
                    //   // }

                    //    return ps;
                    //}

                },
                {
                    dataField: "Jan",
                    width: 60,
                    //validationRules: [
                    //    { type: "required" },
                    //    {
                    //        type: "range",
                    //        message: "Please enter valid count > 0",
                    //        min: 0,
                    //        max: 214783647
                    //    }],
                    dataType: "number",
                    setCellValue: function (rowData, value) {

                        rowData.Jan = value;

                    },


                },
                {
                    dataField: "Feb",
                    width: 60,
                    //validationRules: [
                    //    { type: "required" },
                    //    {
                    //        type: "range",
                    //        message: "Please enter valid count > 0",
                    //        min: 0,
                    //        max: 214783647
                    //    }],
                    dataType: "number",
                },
                {
                    dataField: "Mar",
                    width: 60,
                    //validationRules: [
                    //    { type: "required" },
                    //    {
                    //        type: "range",
                    //        message: "Please enter valid count > 0",
                    //        min: 0,
                    //        max: 214783647
                    //    }],
                    dataType: "number",
                },
                {
                    dataField: "Apr",
                    width: 60,
                    //validationRules: [
                    //    { type: "required" },
                    //    {
                    //        type: "range",
                    //        message: "Please enter valid count > 0",
                    //        min: 0,
                    //        max: 214783647
                    //    }],
                    dataType: "number",
                },
                {
                    dataField: "May",
                    width: 60,
                    //validationRules: [
                    //    { type: "required" },
                    //    {
                    //        type: "range",
                    //        message: "Please enter valid count > 0",
                    //        min: 0,
                    //        max: 214783647
                    //    }],
                    dataType: "number",
                },
                {
                    dataField: "Jun",
                    width: 60,
                    //validationRules: [
                    //    { type: "required" },
                    //    {
                    //        type: "range",
                    //        message: "Please enter valid count > 0",
                    //        min: 0,
                    //        max: 214783647
                    //    }],
                    dataType: "number",
                },
                {
                    dataField: "Jul",
                    width: 60,
                    //validationRules: [
                    //    { type: "required" },
                    //    {
                    //        type: "range",
                    //        message: "Please enter valid count > 0",
                    //        min: 0,
                    //        max: 214783647
                    //    }],
                    dataType: "number",
                },
                {
                    dataField: "Aug",
                    width: 60,
                    //validationRules: [
                    //    { type: "required" },
                    //    {
                    //        type: "range",
                    //        message: "Please enter valid count > 0",
                    //        min: 0,
                    //        max: 214783647
                    //    }],
                    dataType: "number",
                },
                {
                    dataField: "Sep",
                    width: 60,
                    //validationRules: [
                    //    { type: "required" },
                    //    {
                    //        type: "range",
                    //        message: "Please enter valid count > 0",
                    //        min: 0,
                    //        max: 214783647
                    //    }],
                    dataType: "number",
                },
                {
                    dataField: "Oct",
                    width: 60,
                    //validationRules: [
                    //    { type: "required" },
                    //    {
                    //        type: "range",
                    //        message: "Please enter valid count > 0",
                    //        min: 0,
                    //        max: 214783647
                    //    }],
                    dataType: "number",
                },
                {
                    dataField: "Nov",
                    width: 60,
                    //validationRules: [
                    //    { type: "required" },
                    //    {
                    //        type: "range",
                    //        message: "Please enter valid count > 0",
                    //        min: 0,
                    //        max: 214783647
                    //    }],
                    dataType: "number",
                },
                {
                    dataField: "Dec",
                    width: 60,
                    //calculateCellValue: function (data) {
                    //    //debugger;
                    //    //return e.Dec;
                    //  /*  if (!dataGrid)*/ return data.Dec;

                    //    //var editingController = dataGrid.getController("editing"),
                    //    //    editDataIndex = editingController.getIndexByKey(data.SNo, editingController._editData),
                    //    //    editData = editingController._editData[editDataIndex];
                    //    //return editData && editData.type === "update" && editData.data && editData.data.Dec !== undefined ? editData.data.Dec : data.Dec;

                    //},
                    //validationRules: [
                    //    { type: "required" },
                    //    {
                    //        type: "range",
                    //        message: "Please enter valid count > 0",
                    //        min: 0,
                    //        max: 214783647
                    //    }],
                    dataType: "number",
                },
                {
                    dataField: "PYO",
                    width: 100,
                    allowEditing: false,
                    format: { type: 'fixedPoint', precision: 2 }, 
                    //calculateCellValue: function (rowData) {
                    //    //update the bud-inv
                    //    //debugger;
                    //    var pyo;
                    //    if (rowData.Plan_Sum == undefined || rowData.Plan_Sum == null || rowData.Plan_Sum == 0) {

                    //        pyo = 0;
                    //    }
                    //    else {

                    //        pyo = rowData.Plan_Sum/12;

                    //    }

                    //    return pyo;
                    //}

                },
                {
                    dataField: "Remarks",
                    width: 100,
                    allowEditing: true,
                    visible: true
                },
                {
                    dataField: "Updated_By",
                    width: 110,
                    allowEditing: false,
                },
                {
                    dataField: "Updated_At",
                    width: 110,
                    dataType: "datetime",
                    allowEditing: false
                },

                {
                    dataField: "fake1",
                    width: 110,
                    visible: false
                },
                {
                    dataField: "fake2",
                    width: 110,
                    visible: false
                },
                {
                    dataField: "fake3",
                    width: 110,
                    visible: false
                },
                {
                    dataField: "fake4",
                    width: 110,
                    visible: false
                },
                {
                    dataField: "fake5",
                    width: 110,
                    visible: false
                },
                {
                    dataField: "fake6",
                    width: 110,
                    visible: false
                },
                {
                    dataField: "fake7",
                    width: 110,
                    visible: false
                },
                {
                    dataField: "fake8",
                    width: 110,
                    visible: false
                },
                {
                    dataField: "fake9",
                    width: 110,
                    visible: false
                },
                {
                    dataField: "fake10",
                    width: 110,
                    visible: false
                },
                {
                    dataField: "fake11",
                    width: 110,
                    visible: false
                },
                {
                    dataField: "fake12",
                    width: 110,
                    visible: false
                },
                //{
                //    dataField: "fake",
                //    fixed: true,
                //    allowReordering: false,
                //    visible: true,
                //    fixedPosition: "right",
                //    showInColumnChooser: "false",
                //    // width="0.01"
                //},
                //{
                //    dataField: "fake",
                //    fixed: true,
                //    allowReordering: false,
                //    visible: true,
                //    fixedPosition: "right",
                //    showInColumnChooser: "false",
                //    // width="0.01"
                //},
                //{
                //    dataField: "fake",
                //    fixed: true,
                //    allowReordering: false,
                //    visible: true,
                //    fixedPosition: "right",
                //    showInColumnChooser: "false",
                //    // width="0.01"
                //}
                
            ],
            summary: {
                recalculateWhileEditing: true,
                totalItems: [
                    {
                        column: "NTID",
                        summaryType: "count",
                        valueFormat: "number",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "No. of Persons: " + e.value;
                        }
                    },
                    {
                        column: "Plan_Sum",
                        alignByColumn: true, 
                        summaryType: "sum",
                        valueFormat: "number",
                        showInColumn: "Plan_Sum",
                       // alignment: "center" ,
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "Sum: " + e.value;
                        },

                    },
                     {
                         column: "Jan",
                         alignByColumn: true,
                        summaryType: "sum",
                         valueFormat: "number",
                         showInColumn: "Jan",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return  e.value;
                        }
                    },
                    {
                        column: "Feb",
                        alignByColumn: true,
                        summaryType: "sum",
                        valueFormat: "number",
                        showInColumn: "Feb",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return  e.value;
                        }
                    },
                    {
                        column: "Mar",
                        alignByColumn: true,
                        summaryType: "sum",
                        valueFormat: "number",
                        showInColumn: "Mar",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return  e.value;
                        }
                    },
                    {
                        column: "Apr",
                        alignByColumn: true,
                        summaryType: "sum",
                        valueFormat: "number",
                        showInColumn: "Apr",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return  e.value;
                        }
                    },
                    {
                        column: "May",
                        alignByColumn: true,
                        showInColumn: "May",
                        summaryType: "sum",
                        valueFormat: "number",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return  e.value;
                        }
                    },
                    {
                        column: "Jun",
                        alignByColumn: true,
                        showInColumn: "Jun",
                        summaryType: "sum",
                        valueFormat: "number",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return e.value;
                        }
                    },
                    
                    {
                        column: "Jul",
                        alignByColumn: true,
                        showInColumn: "Jul",
                        summaryType: "sum",
                        valueFormat: "number",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return e.value;
                        }
                    },
                    {
                        column: "Aug",
                        alignByColumn: true,
                        showInColumn: "Aug",
                        summaryType: "sum",
                        valueFormat: "number",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return e.value;
                        }
                    },
                    {
                        column: "Sep",
                        alignByColumn: true,
                        summaryType: "sum",
                        showInColumn: "Sep",
                        valueFormat: "number",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return e.value;
                        }
                    },
                    {
                        column: "Oct",
                        alignByColumn: true,
                        summaryType: "sum",
                        showInColumn: "Oct",
                        valueFormat: "number",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return e.value;
                        }
                    },
                    {
                        column: "Nov",
                        alignByColumn: true,
                        showInColumn: "Nov",
                        summaryType: "sum",
                        valueFormat: "number",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return e.value;
                        }
                    },
                    {
                        column: "Dec",
                        alignByColumn: true,
                        showInColumn: "Dec",
                        summaryType: "sum",
                        valueFormat: "number",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return e.value;
                        },
                        showInColumn: "Dec",
                        alignment: "center" 
                    },
                    {
                        column: "PYO",
                        alignByColumn: true,
                        summaryType: "sum",
                        showInColumn: "PYO",
                        valueFormat: "number",
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "PYO: " + e.value.toFixed(2);
                        }
                    },
                    {
                        column: "fake1",
                       
                        alignByColumn: true,
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "       ";
                        }
                    },
                    {
                        column: "fake2",

                        alignByColumn: true,
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "       ";
                        }
                    },
                    {
                        column: "fake3",

                        alignByColumn: true,
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "       ";
                        }
                    },
                    {
                        column: "fake4",

                        alignByColumn: true,
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "       ";
                        }
                    },
                    {
                        column: "fake5",

                        alignByColumn: true,
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "       ";
                        }
                    },
                    {
                        column: "fake6",

                        alignByColumn: true,
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "       ";
                        }
                    },
                    {
                        column: "fake7",

                        alignByColumn: true,
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "       ";
                        }
                    },
                    {
                        column: "fake8",

                        alignByColumn: true,
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "       ";
                        }
                    },
                    {
                        column: "fake9",

                        alignByColumn: true,
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "       ";
                        }
                    },
                    {
                        column: "fake10",

                        alignByColumn: true,
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "       ";
                        }
                    },
                    {
                        column: "fake11",

                        alignByColumn: true,
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "       ";
                        }
                    },
                    {
                        column: "fake12",

                        alignByColumn: true,
                        customizeText: function (e) {
                            //debugger;
                            //I tried add 
                            //console.log(e.value)
                            return "       ";
                        }
                    },

                ],
            },

            onEditorPreparing: function (e) {
                //debugger;
                var component = e.component,
                    rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

                if (e.parentType === "dataRow" && e.dataField === "Group") {
                    debugger;
                    e.editorOptions.disabled = (typeof e.row.data.Department !== "number");
                    if (e.editorOptions.disabled)
                        e.editorOptions.placeholder = 'Select Dept first';

                    if (!e.editorOptions.disabled)
                        e.editorOptions.placeholder = 'Select Group';
                }




                if (e.dataField === "Employee_Name") {
                    e.editorOptions.onFocusOut = function (x) {
                        $.ajax({
                            type: "post",
                            url: "/GRMEMSupport/GetNTID_basedDetails",
                             data: { Ntid: e.row.data.Employee_Name },
                            datatype: "json",
                            traditional: true,
                            success: function (data) {
                                debugger;
                                //var res;
                                // res = JSON.parse(data.data.Data.Content);
                                if (data.success == true) {
                                    NTID = data.NTID;
                                    emp_name = data.Employee_Name;
                                    emp_no = data.Employee_Number;
                                    section = data.Section;
                                    dept = data.Department;
                                    grp = data.Group;
                                    saprole = data.SAP_Role;//.split(',');
                                    level = data.Level;
                                    yr = data.Year;
                                    bu = data.Product_Area;

 

                                        component.cellValue(rowIndex, "NTID", NTID);
                                        component.cellValue(rowIndex, "Employee_Name", emp_name);
                                        component.cellValue(rowIndex, "Employee_Number", emp_no);
                                        component.cellValue(rowIndex, "Section", section);
                                        component.cellValue(rowIndex, "Department", dept);
                                        component.cellValue(rowIndex, "Group", grp);
                                        component.cellValue(rowIndex, "SAP_Role", saprole);
                                        component.cellValue(rowIndex, "Level", level);
                                        component.cellValue(rowIndex, "Year", yr);
                                        component.cellValue(rowIndex, "Product_Area", bu);
                                }
                                else {
                                    $.notify(data.msg, {
                                        globalPosition: "top center",
                                        className: "error",
                                        autoHideDelay: 20000
                                    })
                                }
                            }
                        });
                    }
                    debugger;
                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                    e.editorName = "dxAutocomplete";
                    //e.editorName.cellInfo.value = e.value;
                    //e.editorOptions.placeholder = 'Select Emp Name';
                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                        debugger;

 

                    }

 

                }

                //if (e.dataField === "Employee_Name") {

                //    debugger;
                //    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data
                //    e.editorName = "dxAutocomplete";
                //    debugger;
                //    //e.editorName.cellInfo.value = e.value;
                //    //e.editorOptions.placeholder = 'Select Emp Name';
                //    e.editorOptions.onValueChanged = function (e) {
                //        debugger;
                //        onValueChanged.call(this, e);
                //        debugger;

                //        $.ajax({
                //            type: "post",
                //            url: "/GRMEMSupport/GetNTID_basedDetails",
                //            data: { Ntid: e.value },
                //            datatype: "json",
                //            traditional: true,
                //            success: function (data) {
                //                debugger;
                //                //var res;
                //                // res = JSON.parse(data.data.Data.Content);
                //                if (data.success == true) {
                //                    NTID = data.NTID;
                //                    emp_name = data.Employee_Name;
                //                    emp_no = data.Employee_Number;
                //                    section = data.Section;
                //                    dept = data.Department;
                //                    grp = data.Group;
                //                    saprole = data.SAP_Role;//.split(',');
                //                    level = data.Level;
                //                    yr = data.Year;
                //                    bu = data.Product_Area;

                //                    window.setTimeout(function () {

                //                        component.cellValue(rowIndex, "NTID", NTID);
                //                        component.cellValue(rowIndex, "Employee_Name", emp_name);
                //                        component.cellValue(rowIndex, "Employee_Number", emp_no);
                //                        component.cellValue(rowIndex, "Section", section);
                //                        component.cellValue(rowIndex, "Department", dept);
                //                        component.cellValue(rowIndex, "Group", grp);
                //                        component.cellValue(rowIndex, "SAP_Role", saprole);
                //                        component.cellValue(rowIndex, "Level", level);
                //                        component.cellValue(rowIndex, "Year", yr);
                //                        component.cellValue(rowIndex, "Product_Area", bu);
                //                    },
                //                        1000);
                //                }
                //                else {
                //                    $.notify(data.msg, {
                //                        globalPosition: "top center",
                //                        className: "error",
                //                        autoHideDelay: 20000
                //                    })
                //                }
                //            }
                //        });
                //        //$.ajax({
                //        //    type: "post",
                //        //    url: "/GRMEMSupport/GetProductArea_basedonUser",
                //        //    datatype: "json",
                //        //    traditional: true,
                //        //    success: function (data) {
                //        //        //debugger;
                //        //        if (data.success == true) {
                //        //            bu = data.data;

                //        //            window.setTimeout(function () {

                //        //                component.cellValue(rowIndex, "Product_Area", bu);

                //        //            }, 1000);
                //        //        }
                //        //        else {
                //        //            //bu = data.data;

                //        //            window.setTimeout(function () {

                //        //                component.cellValue(rowIndex, "Product_Area", "0");

                //        //            }, 1000);
                //        //        }

                //        //    }
                //        //})



                //    }

                //}


                if (e.dataField === "Jan") {
                    //debugger;

                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({
                            type: "post",
                            url: "/GRMEMSupport/GetPYO_PlanSum",
                            data: {
                                Jan: e.value, Feb: component.cellValue(rowIndex, "Feb"), Mar: component.cellValue(rowIndex, "Mar"),
                                Apr: component.cellValue(rowIndex, "Apr"), May: component.cellValue(rowIndex, "May"), Jun: component.cellValue(rowIndex, "Jun"),
                                Jul: component.cellValue(rowIndex, "Jul"), Aug: component.cellValue(rowIndex, "Aug"), Sep: component.cellValue(rowIndex, "Sep"),
                                Oct: component.cellValue(rowIndex, "Oct"), Nov: component.cellValue(rowIndex, "Nov"), Dec: component.cellValue(rowIndex, "Dec")
                            },
                            //url: "/GRMEMSupport/GetPYO",
                            // data: { Plan_Sum: component.cellValue(rowIndex, "Plan_Sum") },
                            datatype: "json",
                            success: function (data) {
                                //debugger;
                                plan_sum = data.Plan_Sum;
                                pyo = data.PYO;

                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "Plan_Sum", plan_sum);
                                    component.cellValue(rowIndex, "PYO", pyo);
                                }, 1000);
                            },
                            error: function (data) {
                                //debugger;
                            }
                        });

                    }

                }
                if (e.dataField === "Feb") {
                    //debugger;

                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({
                            type: "post",
                            url: "/GRMEMSupport/GetPYO_PlanSum",
                            data: {
                                Jan: component.cellValue(rowIndex, "Jan"), Feb: e.value, Mar: component.cellValue(rowIndex, "Mar"),
                                Apr: component.cellValue(rowIndex, "Apr"), May: component.cellValue(rowIndex, "May"), Jun: component.cellValue(rowIndex, "Jun"),
                                Jul: component.cellValue(rowIndex, "Jul"), Aug: component.cellValue(rowIndex, "Aug"), Sep: component.cellValue(rowIndex, "Sep"),
                                Oct: component.cellValue(rowIndex, "Oct"), Nov: component.cellValue(rowIndex, "Nov"), Dec: component.cellValue(rowIndex, "Dec")
                            },
                            //url: "/GRMEMSupport/GetPYO",
                            // data: { Plan_Sum: component.cellValue(rowIndex, "Plan_Sum") },
                            datatype: "json",
                            success: function (data) {
                                //debugger;
                                plan_sum = data.Plan_Sum;
                                pyo = data.PYO;

                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "Plan_Sum", plan_sum);
                                    component.cellValue(rowIndex, "PYO", pyo);
                                }, 1000);
                            },
                            error: function (data) {
                                //debugger;
                            }
                        });

                    }

                }
                if (e.dataField === "Mar") {
                    //debugger;

                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({
                            type: "post",
                            url: "/GRMEMSupport/GetPYO_PlanSum",
                            data: {
                                Jan: component.cellValue(rowIndex, "Jan"), Feb: component.cellValue(rowIndex, "Feb"), Mar: e.value,
                                Apr: component.cellValue(rowIndex, "Apr"), May: component.cellValue(rowIndex, "May"), Jun: component.cellValue(rowIndex, "Jun"),
                                Jul: component.cellValue(rowIndex, "Jul"), Aug: component.cellValue(rowIndex, "Aug"), Sep: component.cellValue(rowIndex, "Sep"),
                                Oct: component.cellValue(rowIndex, "Oct"), Nov: component.cellValue(rowIndex, "Nov"), Dec: component.cellValue(rowIndex, "Dec")
                            },
                            //url: "/GRMEMSupport/GetPYO",
                            // data: { Plan_Sum: component.cellValue(rowIndex, "Plan_Sum") },
                            datatype: "json",
                            success: function (data) {
                                //debugger;
                                plan_sum = data.Plan_Sum;
                                pyo = data.PYO;

                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "Plan_Sum", plan_sum);
                                    component.cellValue(rowIndex, "PYO", pyo);
                                }, 1000);
                            },
                            error: function (data) {
                                //debugger;
                            }
                        });

                    }

                }
                if (e.dataField === "Apr") {
                    //debugger;

                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({
                            type: "post",
                            url: "/GRMEMSupport/GetPYO_PlanSum",
                            data: {
                                Jan: component.cellValue(rowIndex, "Jan"), Feb: component.cellValue(rowIndex, "Feb"), Mar: component.cellValue(rowIndex, "Mar"),
                                Apr: e.value, May: component.cellValue(rowIndex, "May"), Jun: component.cellValue(rowIndex, "Jun"),
                                Jul: component.cellValue(rowIndex, "Jul"), Aug: component.cellValue(rowIndex, "Aug"), Sep: component.cellValue(rowIndex, "Sep"),
                                Oct: component.cellValue(rowIndex, "Oct"), Nov: component.cellValue(rowIndex, "Nov"), Dec: component.cellValue(rowIndex, "Dec")
                            },
                            //url: "/GRMEMSupport/GetPYO",
                            // data: { Plan_Sum: component.cellValue(rowIndex, "Plan_Sum") },
                            datatype: "json",
                            success: function (data) {
                                //debugger;
                                plan_sum = data.Plan_Sum;
                                pyo = data.PYO;

                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "Plan_Sum", plan_sum);
                                    component.cellValue(rowIndex, "PYO", pyo);
                                }, 1000);
                            },
                            error: function (data) {
                                //debugger;
                            }
                        });

                    }

                }
                if (e.dataField === "May") {
                    //debugger;

                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({
                            type: "post",
                            url: "/GRMEMSupport/GetPYO_PlanSum",
                            data: {
                                Jan: component.cellValue(rowIndex, "Jan"), Feb: component.cellValue(rowIndex, "Feb"), Mar: component.cellValue(rowIndex, "Mar"),
                                Apr: component.cellValue(rowIndex, "Apr"), May: e.value, Jun: component.cellValue(rowIndex, "Jun"),
                                Jul: component.cellValue(rowIndex, "Jul"), Aug: component.cellValue(rowIndex, "Aug"), Sep: component.cellValue(rowIndex, "Sep"),
                                Oct: component.cellValue(rowIndex, "Oct"), Nov: component.cellValue(rowIndex, "Nov"), Dec: component.cellValue(rowIndex, "Dec")
                            },
                            //url: "/GRMEMSupport/GetPYO",
                            // data: { Plan_Sum: component.cellValue(rowIndex, "Plan_Sum") },
                            datatype: "json",
                            success: function (data) {
                                //debugger;
                                plan_sum = data.Plan_Sum;
                                pyo = data.PYO;

                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "Plan_Sum", plan_sum);
                                    component.cellValue(rowIndex, "PYO", pyo);
                                }, 1000);
                            },
                            error: function (data) {
                                //debugger;
                            }
                        });

                    }

                }
                if (e.dataField === "Jun") {
                    //debugger;

                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({
                            type: "post",
                            url: "/GRMEMSupport/GetPYO_PlanSum",
                            data: {
                                Jan: component.cellValue(rowIndex, "Jan"), Feb: component.cellValue(rowIndex, "Feb"), Mar: component.cellValue(rowIndex, "Mar"),
                                Apr: component.cellValue(rowIndex, "Apr"), May: component.cellValue(rowIndex, "May"), Jun: e.value,
                                Jul: component.cellValue(rowIndex, "Jul"), Aug: component.cellValue(rowIndex, "Aug"), Sep: component.cellValue(rowIndex, "Sep"),
                                Oct: component.cellValue(rowIndex, "Oct"), Nov: component.cellValue(rowIndex, "Nov"), Dec: component.cellValue(rowIndex, "Dec")
                            },
                            //url: "/GRMEMSupport/GetPYO",
                            // data: { Plan_Sum: component.cellValue(rowIndex, "Plan_Sum") },
                            datatype: "json",
                            success: function (data) {
                                //debugger;
                                plan_sum = data.Plan_Sum;
                                pyo = data.PYO;

                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "Plan_Sum", plan_sum);
                                    component.cellValue(rowIndex, "PYO", pyo);
                                }, 1000);
                            },
                            error: function (data) {
                                //debugger;
                            }
                        });

                    }

                }
                if (e.dataField === "Jul") {
                    //debugger;

                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({
                            type: "post",
                            url: "/GRMEMSupport/GetPYO_PlanSum",
                            data: {
                                Jan: component.cellValue(rowIndex, "Jan"), Feb: component.cellValue(rowIndex, "Feb"), Mar: component.cellValue(rowIndex, "Mar"),
                                Apr: component.cellValue(rowIndex, "Apr"), May: component.cellValue(rowIndex, "May"), Jun: component.cellValue(rowIndex, "Jun"),
                                Jul: e.value, Aug: component.cellValue(rowIndex, "Aug"), Sep: component.cellValue(rowIndex, "Sep"),
                                Oct: component.cellValue(rowIndex, "Oct"), Nov: component.cellValue(rowIndex, "Nov"), Dec: component.cellValue(rowIndex, "Dec")
                            },
                            //url: "/GRMEMSupport/GetPYO",
                            // data: { Plan_Sum: component.cellValue(rowIndex, "Plan_Sum") },
                            datatype: "json",
                            success: function (data) {
                                //debugger;
                                plan_sum = data.Plan_Sum;
                                pyo = data.PYO;

                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "Plan_Sum", plan_sum);
                                    component.cellValue(rowIndex, "PYO", pyo);
                                }, 1000);
                            },
                            error: function (data) {
                                //debugger;
                            }
                        });

                    }

                }
                if (e.dataField === "Aug") {
                    //debugger;

                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({
                            type: "post",
                            url: "/GRMEMSupport/GetPYO_PlanSum",
                            data: {
                                Jan: component.cellValue(rowIndex, "Jan"), Feb: component.cellValue(rowIndex, "Feb"), Mar: component.cellValue(rowIndex, "Mar"),
                                Apr: component.cellValue(rowIndex, "Apr"), May: component.cellValue(rowIndex, "May"), Jun: component.cellValue(rowIndex, "Jun"),
                                Jul: component.cellValue(rowIndex, "Jul"), Aug: e.value, Sep: component.cellValue(rowIndex, "Sep"),
                                Oct: component.cellValue(rowIndex, "Oct"), Nov: component.cellValue(rowIndex, "Nov"), Dec: component.cellValue(rowIndex, "Dec")
                            },
                            //url: "/GRMEMSupport/GetPYO",
                            // data: { Plan_Sum: component.cellValue(rowIndex, "Plan_Sum") },
                            datatype: "json",
                            success: function (data) {
                                //debugger;
                                plan_sum = data.Plan_Sum;
                                pyo = data.PYO;

                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "Plan_Sum", plan_sum);
                                    component.cellValue(rowIndex, "PYO", pyo);
                                }, 1000);
                            },
                            error: function (data) {
                                //debugger;
                            }
                        });

                    }

                }
                if (e.dataField === "Sep") {
                    //debugger;

                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({
                            type: "post",
                            url: "/GRMEMSupport/GetPYO_PlanSum",
                            data: {
                                Jan: component.cellValue(rowIndex, "Jan"), Feb: component.cellValue(rowIndex, "Feb"), Mar: component.cellValue(rowIndex, "Mar"),
                                Apr: component.cellValue(rowIndex, "Apr"), May: component.cellValue(rowIndex, "May"), Jun: component.cellValue(rowIndex, "Jun"),
                                Jul: component.cellValue(rowIndex, "Jul"), Aug: component.cellValue(rowIndex, "Aug"), Sep: e.value,
                                Oct: component.cellValue(rowIndex, "Oct"), Nov: component.cellValue(rowIndex, "Nov"), Dec: component.cellValue(rowIndex, "Dec")
                            },
                            //url: "/GRMEMSupport/GetPYO",
                            // data: { Plan_Sum: component.cellValue(rowIndex, "Plan_Sum") },
                            datatype: "json",
                            success: function (data) {
                                //debugger;
                                plan_sum = data.Plan_Sum;
                                pyo = data.PYO;

                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "Plan_Sum", plan_sum);
                                    component.cellValue(rowIndex, "PYO", pyo);
                                }, 1000);
                            },
                            error: function (data) {
                                //debugger;
                            }
                        });

                    }

                }
                if (e.dataField === "Oct") {
                    //debugger;

                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({
                            type: "post",
                            url: "/GRMEMSupport/GetPYO_PlanSum",
                            data: {
                                Jan: component.cellValue(rowIndex, "Jan"), Feb: component.cellValue(rowIndex, "Feb"), Mar: component.cellValue(rowIndex, "Mar"),
                                Apr: component.cellValue(rowIndex, "Apr"), May: component.cellValue(rowIndex, "May"), Jun: component.cellValue(rowIndex, "Jun"),
                                Jul: component.cellValue(rowIndex, "Jul"), Aug: component.cellValue(rowIndex, "Aug"), Sep: component.cellValue(rowIndex, "Sep"),
                                Oct: e.value, Nov: component.cellValue(rowIndex, "Nov"), Dec: component.cellValue(rowIndex, "Dec")
                            },
                            //url: "/GRMEMSupport/GetPYO",
                            // data: { Plan_Sum: component.cellValue(rowIndex, "Plan_Sum") },
                            datatype: "json",
                            success: function (data) {
                                //debugger;
                                plan_sum = data.Plan_Sum;
                                pyo = data.PYO;

                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "Plan_Sum", plan_sum);
                                    component.cellValue(rowIndex, "PYO", pyo);
                                }, 1000);
                            },
                            error: function (data) {
                                //debugger;
                            }
                        });

                    }

                }
                if (e.dataField === "Nov") {
                    //debugger;

                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({
                            type: "post",
                            url: "/GRMEMSupport/GetPYO_PlanSum",
                            data: {
                                Jan: component.cellValue(rowIndex, "Jan"), Feb: component.cellValue(rowIndex, "Feb"), Mar: component.cellValue(rowIndex, "Mar"),
                                Apr: component.cellValue(rowIndex, "Apr"), May: component.cellValue(rowIndex, "May"), Jun: component.cellValue(rowIndex, "Jun"),
                                Jul: component.cellValue(rowIndex, "Jul"), Aug: component.cellValue(rowIndex, "Aug"), Sep: component.cellValue(rowIndex, "Sep"),
                                Oct: component.cellValue(rowIndex, "Oct"), Nov: e.value, Dec: component.cellValue(rowIndex, "Dec")
                            },
                            //url: "/GRMEMSupport/GetPYO",
                            // data: { Plan_Sum: component.cellValue(rowIndex, "Plan_Sum") },
                            datatype: "json",
                            success: function (data) {
                                //debugger;
                                plan_sum = data.Plan_Sum;
                                pyo = data.PYO;

                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "Plan_Sum", plan_sum);
                                    component.cellValue(rowIndex, "PYO", pyo);
                                }, 1000);
                            },
                            error: function (data) {
                                //debugger;
                            }
                        });

                    }

                }
                if (e.dataField === "Dec") {
                    //debugger;

                    var onValueChanged = e.editorOptions.onValueChanged;//event for itemname; makes sure that the itemname is modified data


                    e.editorOptions.onValueChanged = function (e) {
                        //debugger;
                        needSummaryUpdate = true;
                        onValueChanged.call(this, e);
                        //debugger;
                        $.ajax({
                            type: "post",
                            url: "/GRMEMSupport/GetPYO_PlanSum",
                            data: {
                                Jan: component.cellValue(rowIndex, "Jan"), Feb: component.cellValue(rowIndex, "Feb"), Mar: component.cellValue(rowIndex, "Mar"),
                                Apr: component.cellValue(rowIndex, "Apr"), May: component.cellValue(rowIndex, "May"), Jun: component.cellValue(rowIndex, "Jun"),
                                Jul: component.cellValue(rowIndex, "Jul"), Aug: component.cellValue(rowIndex, "Aug"), Sep: component.cellValue(rowIndex, "Sep"),
                                Oct: component.cellValue(rowIndex, "Oct"), Nov: component.cellValue(rowIndex, "Nov"), Dec: e.value
                            },
                            //url: "/GRMEMSupport/GetPYO",
                            // data: { Plan_Sum: component.cellValue(rowIndex, "Plan_Sum") },
                            datatype: "json",
                            success: function (data) {
                                //debugger;
                                plan_sum = data.Plan_Sum;
                                pyo = data.PYO;

                                window.setTimeout(function () {
                                    //debugger;
                                    component.cellValue(rowIndex, "Plan_Sum", plan_sum);
                                    component.cellValue(rowIndex, "PYO", pyo);
                                }, 1000);
                            },
                            error: function (data) {
                                //debugger;
                            }
                        });

                    }

                }
            },

            onRowUpdated: function (e) {
                $.notify("Please wait, Updating details..!", {
                    globalPosition: "top center",
                    className: "success",
                    autoHideDelay: 2000,
                })
                Selected = [];
                //debugger;
                Selected.push(e.data);
                Update(Selected);

                



            },

            onRowInserting: function (e) {
                $.notify("Please wait, Saving details..!", {
                    globalPosition: "top center",
                    className: "success",
                    autoHideDelay: 2000,
                })

                Selected = [];
                debugger;

                Selected.push(e.data);



                Update(Selected);
            },
            onRowRemoving: function (e) {
                //debugger;
                Delete(e.data.SNo/*, filtered_yr*/);

            },
           



        });


       
        //    var dataGrid = $("#RequestTable").dxDataGrid("instance");

        //     dataGrid.option("sorting", false);


        //dataGrid.columnOption('headerFilter', 'visible', false);
          

        function AutocompleteEditorTemplate(cellElement, cellInfo) {
            debugger;
            return $('<div>').dxAutocomplete({
                dataSource: spoton_emp_list,
                valueExpr: "Employee_Name",
                displayExpr: "Employee_Name",
                value: cellInfo.value,
                onSelectionChanged(e) {
                    $.ajax({
                        type: "post",
                        url: "/GRMEMSupport/GetNTID_basedDetails",
                        data: { Ntid: e.value },
                        datatype: "json",
                        traditional: true,
                        success: function (data) {
                            debugger;
                            //var res;
                            // res = JSON.parse(data.data.Data.Content);
                            if (data.success == true) {
                                NTID = data.NTID;
                                emp_name = data.Employee_Name;
                                emp_no = data.Employee_Number;
                                section = data.Section;
                                dept = data.Department;
                                grp = data.Group;
                                saprole = data.SAP_Role;//.split(',');
                                level = data.Level;
                                yr = data.Year;
                                debugger;
                                //contentTemplate(e) {
                                cellInfo.component.updateDimensions();
                                    //return $('<div>').dxDataGrid({
                                    //    //dataSource: grmemarray,
                                    //    //remoteOperations: true,
                                    //    //columns: ['FullName', 'Title', 'Department'],
                                    //    //hoverStateEnabled: true,
                                    //    //scrolling: { mode: 'virtual' },
                                    //    //height: 250,

                                    //    selection: { mode: 'single' },
                                    //    selectedRowKeys: [cellInfo.value],
                                    //    focusedRowEnabled: true,
                                    //    focusedRowKey: cellInfo.value,
                                    //    onSelectionChanged(selectionChangedArgs) {
                                    //        e.component.option('value', selectionChangedArgs.selectedRowKeys[0]);
                                    //        cellInfo.setValue(selectionChangedArgs.selectedRowKeys[0]);
                                    //        if (selectionChangedArgs.selectedRowKeys.length > 0) {
                                    //            e.component.close();
                                    //        }
                                    //    },
                                    //});

                                //}

                                debugger;
                                //window.setTimeout(function (cellInfo) {

                                    cellInfo.component.cellValue(rowIndex, "NTID", NTID);
                                    cellInfo.component.cellValue(rowIndex, "Employee_Name", emp_name);
                                    cellInfo.component.cellValue(rowIndex, "Employee_Number", emp_no);
                                    cellInfo.component.cellValue(rowIndex, "Section", section);
                                    cellInfo.component.cellValue(rowIndex, "Department", dept);
                                    cellInfo.component.cellValue(rowIndex, "Group", grp);
                                    cellInfo.component.cellValue(rowIndex, "SAP_Role", saprole);
                                    cellInfo.component.cellValue(rowIndex, "Level", level);
                                    cellInfo.component.cellValue(rowIndex, "Year", yr);
                                //},
                                //    1000);
                            }
                            else {
                                $.notify(data.msg, {
                                    globalPosition: "top center",
                                    className: "error",
                                    autoHideDelay: 20000
                                })
                            }
                        }
                    });
                }
                

            });
        }


        function tagBoxEditorTemplate_vkmrole(cellElement, cellInfo) {
            //debugger;
            return $('<div>').dxTagBox({
                items: vkm_role_list,
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
                    //debugger;
                    cellInfo.setValue(e.value);
                },
                onSelectionChanged() {
                    //debugger;
                    cellInfo.component.updateDimensions();
                },
                acceptCustomValue: true,
                onCustomItemCreating(args) {
                    //debugger;
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

                    //    //debugger;
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
                    ////debugger;
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
        //debugger;
        $.ajax({
            type: "POST",
            url: encodeURI("../GRMEMSupport/Savedata"),
            data: { 'req': id1[0] },
            success: function (data) {
                debugger;
                //newobjdata = JSON.parse(data.data.Data.Content);
                addGRMEM_success(data);
                //$("#hc_dataGrid_withAdd").dxDataGrid({ dataSource: newobjdata });
                
                //$("#grmem_dataGrid").dxDataGrid({ dataSource: newobjdata });
                if (data.success == true) {
                    //$.notify("Updated successfully", {
                    //    globalPosition: "top center",
                    //    className: "success",
                    //    autoHideDelay: 13000,
                    //});


                    filtered_yr = $("#ddlYears").val();
                    filtered_yr = parseInt(filtered_yr);
                    filtered_yr = filtered_yr.toString();

                    $.ajax({
                        type: "GET",
                        contentType: "application/json; charset=utf-8",
                        url: "../GRMEMSupport/GenerateGRMEM",
                        data: { 'year': filtered_yr },
                        //dataType: 'json',
                        //traditional: true,
                        async: false,
                        success: addGRMEM_success,

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
                else {
                    $.notify("Please Retry later", {
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
            url: "/GRMEMSupport/Delete",
            data: { 'id': id/*, 'useryear': filtered_yr*/ },
            success: function (data) {

                //$("#hc_dataGrid_withAdd").dxDataGrid({ dataSource: JSON.parse(data.data.Data.Content) });

               // addHC_success(data);
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