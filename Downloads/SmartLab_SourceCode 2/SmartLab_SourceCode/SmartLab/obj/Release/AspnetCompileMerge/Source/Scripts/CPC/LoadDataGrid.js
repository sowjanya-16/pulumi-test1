
function Load_DateDataGrid(data) {
    debugger;
    var pivotGrid = $("#date_dataGrid").dxPivotGrid({
        //allowSortingBySummary: true,
        allowExpandAll: false,
        allowSorting: false,
        allowFiltering: true,
        showBorders: true,
        showColumnGrandTotals: false,
        showRowGrandTotals: false,
        showRowTotals: false,
        showColumnTotals: false,
        height: 600,
        width: 2000,
        //columnWidth: 100,
        headerFilter: { visible: true },
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
            //store: CPCDetails1,
            fields: [{
                caption: "Lab Id",
                width: 120,
                dataField: "LabId",
                area: "row",
                expanded: true
            },{
                caption: "PC Name",
                width: 140,
                dataField: "PCName",
                area: "row",
                expanded: true
            }, {
                caption: "Loc",
                width: 50,
                dataField: "Location1",
                area: "row",
                expanded: true
            }, {
                caption: "Lab",
                width: 80,
                dataField: "Lab",
                area: "row",
                expanded: true
                //visible: false,
               
            }, {
                caption: "Project Change",
                width: 120,
                dataField: "ProjectChange",
                area: "row",
                expanded: true
                //visible: false,
               
            }, {
                    caption: "Current Project",
                    width: 120,
                    dataField: "CurrentProject",
                    area: "row",
                    expanded: true
                }, {
                    caption: "Category",
                    width: 120,
                    dataField: "Category",
                    area: "row",
                    expanded: true
                }, {
                    caption: "Type",
                    width: 70,
                    dataField: "Type",
                    area: "row",
                    expanded: true
                }, {
                    caption: "Responsible",
                    width: 120,
                    dataField: "Responsible",
                    area: "row",
                    expanded: true
                }, {
                    caption: "SetupType",
                    width: 80,
                    dataField: "SetupType",
                    area: "row",
                    expanded: true
                }, {
                dataField: "Date",
                area: "column",
                sortBy: "none",
                //dataType: "date",
                //customizeText: function (options) {
                //    return !options.value ? "" : moment(options.value).format("MMM/DD/YYYY").replace(".", "");
                //},
                expanded: true
                
               }, {
                dataField: "Act",
                area: "column",
                //sortOrder: "asc",
                sortBy: "none",
                expanded: true
            }, {
                groupname: "Date",
                visible: false,
                expanded: true
                }, {
                    caption: "Total",
                    dataField: "Value",
                    dataType: "number",
                    summaryType: "sum",
                    area: "data",
                    visible: true,
                    expanded: true,
                }],
            store: data
        },

       
        onCellPrepared: function (e) {
            //debugger;
            if (e.area === "data" && e.cell.columnPath[1] === "Total") { //&& e.column.dataField === "Value") {
                //debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value > 10) {
                    e.cellElement.css("background-color", "green");
                }
                else if (e.cell.value < 4) {
                    e.cellElement.css("background-color", "red");
                }
                else if (e.cell.value > 4 && e.cell.value < 10) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                     }
            }

            if (e.area === "data" && e.cell.columnPath[1] === "Night") { //&& e.column.dataField === "Value") {
                //debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value > 10) {
                    e.cellElement.css("background-color", "green");
                }
                //else if (e.cell.value < 4) {
                //    e.cellElement.css("background-color", "red");
                //}
                else if (e.cell.value > 6 && e.cell.value < 10) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                }
            }
        },

        onCellClick: function (e) {
            e.cancel = true;
        },

        onExporting: function (e) {
            debugger;
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('CPC_Date');

            DevExpress.excelExporter.exportPivotGrid({
                //component: e.component,
                component: $("#date_dataGrid").dxPivotGrid("instance"),
                worksheet: worksheet,
                autoFilterEnabled: true,
                customizeCell: function (options) {
                    var excelCell = options.excelCell;
                    var pivotCell = options.pivotCell;

                    if (isDataCell(pivotCell)) {
                        //debugger;
                        var appearance = getConditionalAppearance(pivotCell);
                        Object.assign(excelCell, getExcelCellFormat(appearance));
                        //Object.assign(excelCell, appearance);
                    }

                    var borderStyle = { style: "thin", color: { argb: "FF7E7E7E" } };
                    excelCell.border = {
                        bottom: borderStyle,
                        left: borderStyle,
                        right: borderStyle,
                        top: borderStyle
                    };
                }

            })

                .then(function (dataGridRange) {
                    debugger;
                    var fields = pivotGrid.getDataSource().fields();
                    var rowFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var dataFields = fields.filter(r => r.area === 'data').map(r => `[${r.summaryType}(${r.dataField}])`);
                    var columnFields = [...new Set(fields.filter(r => r.area === 'column').map(r => r.dataField))];
                    var filterFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var appliedFilters = fields.filter(r => r.filterValues !== undefined).map(r => `[${r.dataField}:${r.filterValues}]`);
                    var firstRow = worksheet.getRow(1),
                        fieldPanelCell = firstRow.getCell(4);

                    firstRow.height = 90;
                    //worksheet.mergeCells('A1:D1');
                    fieldPanelCell.value = '\n Field Panel content:'
                        + ` \n - Filter fields: [${filterFields.join(', ')}]`
                        + ` \n - Row fields: [${rowFields.join(', ')}]`
                        + ` \n - Column fields: [${columnFields.join(', ')}]`
                        + ` \n - Data fields: [${dataFields.join(', ')}]`
                        + ` \n - Applied filters: [${appliedFilters.join(', ')}]`;

                    fieldPanelCell.alignment = { horizontal: 'left', vertical: 'top', wrapText: true };
                    fieldPanelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'D8D8D8' } };

                    return Promise.resolve();
                })


                .then(function () {
                workbook.xlsx.writeBuffer().then(function (buffer) {
                    saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'CPC_Date.xlsx');
                });
            });
            e.cancel = true;
        }

    }).dxPivotGrid("instance");

    function isDataCell(cell) {
        return (cell.area === "data" && cell.columnPath[1] === "Total");
    }

    function getExcelCellFormat(appearance) {
        return {
            fill: { type: "pattern", pattern: "solid", fgColor: { argb: appearance.fill } },
            font: { color: { argb: appearance.font }, bold: appearance.bold }
        };
    }

    function getConditionalAppearance(cell) {
        //debugger;
        if (cell.value === "") {
            cell.value = 0;
        }

        if (cell.value > 10) {
            //cell.cellElement.css("background-color", "green");
            return { font: "000000", fill: "008000" };
        }
        else if (cell.value < 4) {
            //cell.cellElement.css("background-color", "red");
            return { font: "000000", fill: "FF0000" };
        }
        else if (cell.value > 4 && cell.value < 10) {
            //cell.cellElement.css("background-color", "yellow");
            return { font: "000000", fill: "FFFF00" };
        }
        else {
            //cell.cellElement.css("background-color", "white");
            return { font: "000000", fill: "FFFFFF" };
        }
    }

    function expand() {
        var dataSource = pivotGrid.getDataSource();
        //dataSource.expandHeaderItem("row", ["LabId"]);
        dataSource.expandHeaderItem("column", ["Auto"]);
    }

   
}



function Load_WeekDataGrid(data) {
    debugger;
    var pivotGrid = $("#week_dataGrid").dxPivotGrid({
        allowSortingBySummary: true,
        allowExpandAll: false,
        //allowSorting: true,
        allowFiltering: true,
        showBorders: true,
        showColumnGrandTotals: false,
        showRowGrandTotals: false,
        showRowTotals: false,
        showColumnTotals: false,
        height: 600,
        width: 2000,
        //columnWidth: 100,
        headerFilter: { visible: true },
        fieldPanel: {
            //showColumnFields: true,
            //showDataFields: true,
            showRowFields: true,
            visible: true
        },
        fieldChooser: {
            enabled: false,
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
        //    pageSize: 40
        //},
        dataSource: {
            //store: CPCDetails1,
            fields: [{
                caption: "Lab Id",
                width: 120,
                dataField: "LabId",
                area: "row",
                expanded: true
            }, {
                caption: "PC Name",
                width: 150,
                dataField: "PCName",
                area: "row",
                expanded: true
            }, {
                caption: "Loc",
                width: 60,
                dataField: "Location1",
                area: "row",
                expanded: true
            }, {
                caption: "Lab",
                width: 80,
                dataField: "Lab",
                area: "row",
                expanded: true
            }, {
                caption: "Project Change",
                width: 120,
                dataField: "ProjectChange",
                area: "row",
                expanded: true
            }, {
                caption: "Current Project",
                width: 120,
                dataField: "CurrentProject",
                area: "row",
                expanded: true
            }, {
                caption: "Category",
                width: 120,
                dataField: "Category",
                area: "row",
                expanded: true
            }, {
                caption: "Type",
                width: 90,
                dataField: "Type",
                area: "row",
                expanded: true
            }, {
                caption: "Responsible",
                width: 120,
                dataField: "Responsible",
                area: "row",
                expanded: true
            }, {
                caption: "SetupType",
                width: 80,
                dataField: "SetupType",
                area: "row",
                expanded: true
                },
                 {
                    dataField: "Des",
                    area: "column",
                    //sortOrder: "asc",
                    sortBy: "none",
                    expanded: true
                }, 
                {
                dataField: "WeekNo",
                area: "column",
                sortBy: "none",
                //dataType: "date",
                //customizeText: function (options) {
                //    return !options.value ? "" : moment(options.value).format("MMM/DD/YYYY").replace(".", "");
                //},
                expanded: true

            }, {
                dataField: "Act",
                area: "column",
                //sortOrder: "asc",
                sortBy: "none",
                expanded: true
            }, {
                groupname: "WeekNo",
                visible: false,
                expanded: true
            }, {
                caption: "Total",
                dataField: "Value",
                dataType: "number",
                summaryType: "sum",
                area: "data",
                visible: true,
                expanded: true,
            }],
            store: data
        },
        onCellPrepared: function (e) {
            //debugger;
            if (e.area === "data" && e.cell.columnPath[2] === "Total") { //&& e.column.dataField === "Value") {
                //debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value > 70) {
                    e.cellElement.css("background-color", "green");
                }
                else if (e.cell.value < 28) {
                    e.cellElement.css("background-color", "red");
                }
                else if (e.cell.value > 28 && e.cell.value < 70) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                }
            }

            if (e.area === "data" && e.cell.columnPath[2] === "Weekend") { //&& e.column.dataField === "Value") {
                //debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value >= 48) {
                    e.cellElement.css("background-color", "green");
                }
                //else if (e.cell.value < 4) {
                //    e.cellElement.css("background-color", "red");
                //}
                else if (e.cell.value > 24 && e.cell.value < 48) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                }
            }
        },

        onCellClick: function (e) {
            e.cancel = true;
        },

        onExporting: function (e) {
            debugger;
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('CPC_Week');

            DevExpress.excelExporter.exportPivotGrid({
                //component: e.component,
                component: $("#week_dataGrid").dxPivotGrid("instance"),
                worksheet: worksheet,
                autoFilterEnabled: true,
                customizeCell: function (options) {
                    var excelCell = options.excelCell;
                    var pivotCell = options.pivotCell;

                    if (isDataCell(pivotCell)) {
                        //debugger;
                        var appearance = getConditionalAppearance(pivotCell);
                        Object.assign(excelCell, getExcelCellFormat(appearance));
                        //Object.assign(excelCell, appearance);
                    }

                    var borderStyle = { style: "thin", color: { argb: "FF7E7E7E" } };
                    excelCell.border = {
                        bottom: borderStyle,
                        left: borderStyle,
                        right: borderStyle,
                        top: borderStyle
                    };
                }

            })

                .then(function (dataGridRange) {
                    debugger;
                    var fields = pivotGrid.getDataSource().fields();
                    var rowFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var dataFields = fields.filter(r => r.area === 'data').map(r => `[${r.summaryType}(${r.dataField}])`);
                    var columnFields = [...new Set(fields.filter(r => r.area === 'column').map(r => r.dataField))];
                    var filterFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var appliedFilters = fields.filter(r => r.filterValues !== undefined).map(r => `[${r.dataField}:${r.filterValues}]`);
                    var firstRow = worksheet.getRow(1),
                        fieldPanelCell = firstRow.getCell(4);

                    firstRow.height = 90;
                    //worksheet.mergeCells('A1:D1');
                    fieldPanelCell.value = '\n Field Panel content:'
                        + ` \n - Filter fields: [${filterFields.join(', ')}]`
                        + ` \n - Row fields: [${rowFields.join(', ')}]`
                        + ` \n - Column fields: [${columnFields.join(', ')}]`
                        + ` \n - Data fields: [${dataFields.join(', ')}]`
                        + ` \n - Applied filters: [${appliedFilters.join(', ')}]`;

                    fieldPanelCell.alignment = { horizontal: 'left', vertical: 'top', wrapText: true };
                    fieldPanelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'D8D8D8' } };

                    return Promise.resolve();
                })


                .then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'CPC_Week.xlsx');
                    });
                });
            e.cancel = true;
        }

    }).dxPivotGrid("instance");

    function isDataCell(cell) {
        return (cell.area === "data" && cell.columnPath[2] === "Total");
    }

    function getExcelCellFormat(appearance) {
        return {
            fill: { type: "pattern", pattern: "solid", fgColor: { argb: appearance.fill } },
            font: { color: { argb: appearance.font }, bold: appearance.bold }
        };
    }

    function getConditionalAppearance(cell) {
        //debugger;
        if (cell.value === "") {
            cell.value = 0;
        }

        if (cell.value > 70) {
            //cell.cellElement.css("background-color", "green");
            return { font: "000000", fill: "008000" };
        }
        else if (cell.value < 28) {
            //cell.cellElement.css("background-color", "red");
            return { font: "000000", fill: "FF0000" };
        }
        else if (cell.value > 28 && cell.value < 70) {
            //cell.cellElement.css("background-color", "yellow");
            return { font: "000000", fill: "FFFF00" };
        }
        else {
            //cell.cellElement.css("background-color", "white");
            return { font: "000000", fill: "FFFFFF" };
        }
    }


    function expand() {
        var dataSource = pivotGrid.getDataSource();
        //dataSource.expandHeaderItem("row", ["LabId"]);
        dataSource.expandHeaderItem("column", ["Auto"]);
    }
}



function Load_MonthDataGrid(data) {
    debugger;
    var pivotGrid = $("#month_dataGrid").dxPivotGrid({
        //allowSortingBySummary: true,
        allowExpandAll: false,
        allowSorting: false,
        allowFiltering: true,
        showBorders: true,
        showColumnGrandTotals: false,
        showRowGrandTotals: false,
        showRowTotals: false,
        showColumnTotals: false,
        height: 600,
        width: 2000,
        //columnWidth: 100,
        headerFilter: { visible: true },
        fieldPanel: {
            //showColumnFields: true,
            //showDataFields: true,
            showRowFields: true,
            visible: true
        },
        fieldChooser: {
            enabled: false,
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
        //    pageSize: 40
        //},
        dataSource: {
            //store: CPCDetails1,
            fields: [{
                caption: "Lab Id",
                width: 120,
                dataField: "LabId",
                area: "row",
                expanded: true
            }, {
                caption: "PC Name",
                width: 150,
                dataField: "PCName",
                area: "row",
                expanded: true
            }, {
                caption: "Loc",
                width: 60,
                dataField: "Location1",
                area: "row",
                expanded: true
            }, {
                caption: "Lab",
                width: 80,
                dataField: "Lab",
                area: "row",
                expanded: true
            }, {
                caption: "Project Change",
                width: 120,
                dataField: "ProjectChange",
                area: "row",
                expanded: true
            }, {
                caption: "Current Project",
                width: 120,
                dataField: "CurrentProject",
                area: "row",
                expanded: true
            }, {
                caption: "Category",
                width: 120,
                dataField: "Category",
                area: "row",
                expanded: true
            }, {
                caption: "Type",
                width: 90,
                dataField: "Type",
                area: "row",
                expanded: true
            }, {
                caption: "Responsible",
                width: 120,
                dataField: "Responsible",
                area: "row",
                expanded: true
            }, {
                caption: "SetupType",
                width: 80,
                dataField: "SetupType",
                area: "row",
                expanded: true
            }, {
                dataField: "MonthNo",
                area: "column",
                sortBy: "none",
                expanded: true

            }, {
                dataField: "Act",
                area: "column",
                //sortOrder: "asc",
                sortBy: "none",
                expanded: true
            }, {
                groupname: "MonthNo",
                visible: false,
                expanded: true
            }, {
                caption: "Total",
                dataField: "Value",
                dataType: "number",
                summaryType: "sum",
                area: "data",
                visible: true,
                expanded: true,
            }],
            store: data
        },
        onCellPrepared: function (e) {
            //debugger;
            if (e.area === "data" && e.cell.columnPath[1] === "Total") { //&& e.column.dataField === "Value") {
                //debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value > 300) {
                    e.cellElement.css("background-color", "green");
                }
                else if (e.cell.value < 120) {
                    e.cellElement.css("background-color", "red");
                }
                else if (e.cell.value > 120 && e.cell.value < 300) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                }
            }

            if (e.area === "data" && e.cell.columnPath[1] === "Weekend") { //&& e.column.dataField === "Value") {
                //debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value >= 48) {
                    e.cellElement.css("background-color", "green");
                }
                //else if (e.cell.value < 4) {
                //    e.cellElement.css("background-color", "red");
                //}
                else if (e.cell.value > 24 && e.cell.value < 48) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                }
            }

        },

        onCellClick: function (e) {
            e.cancel = true;
        },

        onExporting: function (e) {
            debugger;
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('CPC_Month');

            DevExpress.excelExporter.exportPivotGrid({
                //component: e.component,
                component: $("#month_dataGrid").dxPivotGrid("instance"),
                worksheet: worksheet,
                autoFilterEnabled: true,
                customizeCell: function (options) {
                    var excelCell = options.excelCell;
                    var pivotCell = options.pivotCell;

                    if (isDataCell(pivotCell)) {
                        //debugger;
                        var appearance = getConditionalAppearance(pivotCell);
                        Object.assign(excelCell, getExcelCellFormat(appearance));
                        //Object.assign(excelCell, appearance);
                    }

                    var borderStyle = { style: "thin", color: { argb: "FF7E7E7E" } };
                    excelCell.border = {
                        bottom: borderStyle,
                        left: borderStyle,
                        right: borderStyle,
                        top: borderStyle
                    };
                }

            })

                .then(function (dataGridRange) {
                    debugger;
                    var fields = pivotGrid.getDataSource().fields();
                    var rowFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var dataFields = fields.filter(r => r.area === 'data').map(r => `[${r.summaryType}(${r.dataField}])`);
                    var columnFields = [...new Set(fields.filter(r => r.area === 'column').map(r => r.dataField))];
                    var filterFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var appliedFilters = fields.filter(r => r.filterValues !== undefined).map(r => `[${r.dataField}:${r.filterValues}]`);
                    var firstRow = worksheet.getRow(1),
                        fieldPanelCell = firstRow.getCell(4);

                    firstRow.height = 90;
                    //worksheet.mergeCells('A1:D1');
                    fieldPanelCell.value = '\n Field Panel content:'
                        + ` \n - Filter fields: [${filterFields.join(', ')}]`
                        + ` \n - Row fields: [${rowFields.join(', ')}]`
                        + ` \n - Column fields: [${columnFields.join(', ')}]`
                        + ` \n - Data fields: [${dataFields.join(', ')}]`
                        + ` \n - Applied filters: [${appliedFilters.join(', ')}]`;

                    fieldPanelCell.alignment = { horizontal: 'left', vertical: 'top', wrapText: true };
                    fieldPanelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'D8D8D8' } };

                    return Promise.resolve();
                })


                .then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'CPC_Month.xlsx');
                    });
                });
            e.cancel = true;
        }

    }).dxPivotGrid("instance");

    function isDataCell(cell) {
        return (cell.area === "data" && cell.columnPath[1] === "Total");
    }

    function getExcelCellFormat(appearance) {
        return {
            fill: { type: "pattern", pattern: "solid", fgColor: { argb: appearance.fill } },
            font: { color: { argb: appearance.font }, bold: appearance.bold }
        };
    }

    function getConditionalAppearance(cell) {
        //debugger;
        if (cell.value === "") {
            cell.value = 0;
        }

        if (cell.value > 300) {
            //cell.cellElement.css("background-color", "green");
            return { font: "000000", fill: "008000" };
        }
        else if (cell.value < 120) {
            //cell.cellElement.css("background-color", "red");
            return { font: "000000", fill: "FF0000" };
        }
        else if (cell.value > 120 && cell.value < 300) {
            //cell.cellElement.css("background-color", "yellow");
            return { font: "000000", fill: "FFFF00" };
        }
        else {
            //cell.cellElement.css("background-color", "white");
            return { font: "000000", fill: "FFFFFF" };
        }
    }

    function expand() {
        var dataSource = pivotGrid.getDataSource();
        //dataSource.expandHeaderItem("row", ["LabId"]);
        dataSource.expandHeaderItem("column", ["Auto"]);
    }
}

////// TSG4
function location_gridTSG4(data,width) {
    debugger;
    //if (year == "2022")
    //    dxPivotGrid.option(width, 20);
    //else
    //    dxPivotGrid.option(width, 1500);
    var pivotGrid_location = $("#location_dataGridTSG4").dxPivotGrid({

        //allowSortingBySummary: true,
        
        allowSorting: false,
        allowFiltering: true,
        showBorders: true,
        showColumnGrandTotals: false,
        showRowGrandTotals: false,
        showRowTotals: false,
        showColumnTotals: false,
        height: 450,
        //width: 1650,
        width: width,
        columnWidth: 100,
        headerFilter: { visible: true },
        fieldPanel: {
            //showColumnFields: true,
            //showDataFields: true,
            showRowFields: true,
            visible: true
        },
        fieldChooser: {
            enabled: false,
            applyChangesMode: "instantly"
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
        //    pageSize: 40
        //},
        dataSource: {
           
            fields: [{

                        //caption: "Setup Type",
                        //width: 60,
                        //dataField: "SetupType",
                        //area: "row",
                        //expanded: true
                        //}, {
                        caption: "Location",
                        width: 60,
                        dataField: "Loc",
                        area: "row",
                        expanded: true
                    }, {
                        caption: "NoofTSGs",
                        width: 80,
                        dataField: "Nos",
                        dataType: "number",
                        area: "row",
                        expanded: true
                        //visible: false,


                    }, {
                        caption: "WeekNo",
                        dataField: "WeekNo",
                        area: "column",
                        sortBy: "none",
                        //width: 50,
                        //dataType: "date",
                        //customizeText: function (options) {
                        //    return !options.value ? "" : moment(options.value).format("MMM/DD/YYYY").replace(".", "");
                        //},
                        expanded: true

                    },
                    //{
                    //    caption: "Average",
                    //    dataField: "Action",
                    //    area: "column",
                    //    //sortOrder: "asc",
                    //    sortBy: "none",
                    //    expanded: true
                    //},
                    {
                        groupname: "WeekNo",
                        visible: false,
                        expanded: true
                    }, {

                        dataField: "Average",
                        dataType: "number",
                        summaryType: "sum",
                        area: "data",
                        visible: true,
                        expanded: true,
                    }],
            store: data
        },
        onCellPrepared: function (e) {
            debugger;
            if (e.area === "data" && (e.cell.columnPath[0] === "CW53" || e.cell.columnPath[0] === "CW52" || e.cell.columnPath[0] === "CW51" ||
                e.cell.columnPath[0] === "CW50" || e.cell.columnPath[0] === "CW49" || e.cell.columnPath[0] === "CW48" || e.cell.columnPath[0] === "CW47" || e.cell.columnPath[0] === "CW46" ||
                e.cell.columnPath[0] === "CW45" || e.cell.columnPath[0] === "CW44" || e.cell.columnPath[0] === "CW43" || e.cell.columnPath[0] === "CW42" || e.cell.columnPath[0] === "CW41" ||
                e.cell.columnPath[0] === "CW40" || e.cell.columnPath[0] === "CW39" || e.cell.columnPath[0] === "CW38" || e.cell.columnPath[0] === "CW37" || e.cell.columnPath[0] === "CW36" ||
                e.cell.columnPath[0] === "CW35" || e.cell.columnPath[0] === "CW34" || e.cell.columnPath[0] === "CW33" || e.cell.columnPath[0] === "CW32" || e.cell.columnPath[0] === "CW31" ||
                e.cell.columnPath[0] === "CW30" || e.cell.columnPath[0] === "CW29" || e.cell.columnPath[0] === "CW28" || e.cell.columnPath[0] === "CW27" || e.cell.columnPath[0] === "CW26" ||
                e.cell.columnPath[0] === "CW25" || e.cell.columnPath[0] === "CW24" || e.cell.columnPath[0] === "CW23" || e.cell.columnPath[0] === "CW22" || e.cell.columnPath[0] === "CW21" ||
                e.cell.columnPath[0] === "CW20" || e.cell.columnPath[0] === "CW19" || e.cell.columnPath[0] === "CW18" || e.cell.columnPath[0] === "CW17" || e.cell.columnPath[0] === "CW16" ||
                e.cell.columnPath[0] === "CW15" || e.cell.columnPath[0] === "CW14" || e.cell.columnPath[0] === "CW13" || e.cell.columnPath[0] === "CW12" || e.cell.columnPath[0] === "CW11" ||
                e.cell.columnPath[0] === "CW10" || e.cell.columnPath[0] === "CW09" || e.cell.columnPath[0] === "CW08" || e.cell.columnPath[0] === "CW07" || e.cell.columnPath[0] === "CW06" ||
                e.cell.columnPath[0] === "CW05" || e.cell.columnPath[0] === "CW04" || e.cell.columnPath[0] === "CW03" || e.cell.columnPath[0] === "CW02" || e.cell.columnPath[0] === "CW01")) { //&& e.column.dataField === "Value") {
                debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value > 70) {
                    e.cellElement.css("background-color", "green");
                }
                else if (e.cell.value < 28) {
                    e.cellElement.css("background-color", "red");
                }
                else if (e.cell.value > 28 && e.cell.value < 70) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                }
            }
        },
        onCellClick: function (e) {
            e.cancel = true;
        },

        onExporting: function (e) {
            debugger;
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('TSG4Avg_Locationwise)');

            DevExpress.excelExporter.exportPivotGrid({
                //component: e.component,
                component: $("#location_dataGridTSG4").dxPivotGrid("instance"),
                worksheet: worksheet,
                autoFilterEnabled: true,
                customizeCell: function (options) {
                    var excelCell = options.excelCell;
                    var pivotCell = options.pivotCell;

                    if (isDataCell(pivotCell)) {
                        //debugger;
                        var appearance = getConditionalAppearance(pivotCell);
                        Object.assign(excelCell, getExcelCellFormat(appearance));
                        //Object.assign(excelCell, appearance);
                    }

                    var borderStyle = { style: "thin", color: { argb: "FF7E7E7E" } };
                    excelCell.border = {
                        bottom: borderStyle,
                        left: borderStyle,
                        right: borderStyle,
                        top: borderStyle
                    };
                }

            })

                .then(function (dataGridRange) {
                    debugger;
                    var fields = pivotGrid_location.getDataSource().fields();
                    var rowFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var dataFields = fields.filter(r => r.area === 'data').map(r => `[${r.summaryType}(${r.dataField}])`);
                    var columnFields = [...new Set(fields.filter(r => r.area === 'column').map(r => r.dataField))];
                    var filterFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var appliedFilters = fields.filter(r => r.filterValues !== undefined).map(r => `[${r.dataField}:${r.filterValues}]`);
                    var firstRow = worksheet.getRow(1),
                        fieldPanelCell = firstRow.getCell(1);

                    firstRow.height = 90;
                    //worksheet.mergeCells('A1:D1');
                    fieldPanelCell.value = '\n Field Panel content:'
                        + ` \n - Filter fields: [${filterFields.join(', ')}]`
                        + ` \n - Row fields: [${rowFields.join(', ')}]`
                        + ` \n - Column fields: [${columnFields.join(', ')}]`
                        + ` \n - Data fields: [${dataFields.join(', ')}]`
                        + ` \n - Applied filters: [${appliedFilters.join(', ')}]`;

                    fieldPanelCell.alignment = { horizontal: 'left', vertical: 'top', wrapText: true };
                    fieldPanelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'D8D8D8' } };

                    return Promise.resolve();
                })


                .then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'TSG4Avg_Locationwise.xlsx');
                    });
                });
            e.cancel = true;
        }
    }).dxPivotGrid("instance");

    function isDataCell(cell) {
        return (cell.area === "data" && (cell.columnPath[0] === "CW53" || cell.columnPath[0] === "CW52" || cell.columnPath[0] === "CW51" ||
            cell.columnPath[0] === "CW50" || cell.columnPath[0] === "CW49" || cell.columnPath[0] === "CW48" || cell.columnPath[0] === "CW47" || cell.columnPath[0] === "CW46" ||
            cell.columnPath[0] === "CW45" || cell.columnPath[0] === "CW44" || cell.columnPath[0] === "CW43" || cell.columnPath[0] === "CW42" || cell.columnPath[0] === "CW41" ||
            cell.columnPath[0] === "CW40" || cell.columnPath[0] === "CW39" || cell.columnPath[0] === "CW38" || cell.columnPath[0] === "CW37" || cell.columnPath[0] === "CW36" ||
            cell.columnPath[0] === "CW35" || cell.columnPath[0] === "CW34" || cell.columnPath[0] === "CW33" || cell.columnPath[0] === "CW32" || cell.columnPath[0] === "CW31" ||
            cell.columnPath[0] === "CW30" || cell.columnPath[0] === "CW29" || cell.columnPath[0] === "CW28" || cell.columnPath[0] === "CW27" || cell.columnPath[0] === "CW26" ||
            cell.columnPath[0] === "CW25" || cell.columnPath[0] === "CW24" || cell.columnPath[0] === "CW23" || cell.columnPath[0] === "CW22" || cell.columnPath[0] === "CW21" ||
            cell.columnPath[0] === "CW20" || cell.columnPath[0] === "CW19" || cell.columnPath[0] === "CW18" || cell.columnPath[0] === "CW17" || cell.columnPath[0] === "CW16" ||
            cell.columnPath[0] === "CW15" || cell.columnPath[0] === "CW14" || cell.columnPath[0] === "CW13" || cell.columnPath[0] === "CW12" || cell.columnPath[0] === "CW11" ||
            cell.columnPath[0] === "CW10" || cell.columnPath[0] === "CW09" || cell.columnPath[0] === "CW08" || cell.columnPath[0] === "CW07" || cell.columnPath[0] === "CW06" ||
            cell.columnPath[0] === "CW05" || cell.columnPath[0] === "CW04" || cell.columnPath[0] === "CW03" || cell.columnPath[0] === "CW02" || cell.columnPath[0] === "CW01"));
    }

    function getExcelCellFormat(appearance) {
        return {
            fill: { type: "pattern", pattern: "solid", fgColor: { argb: appearance.fill } },
            font: { color: { argb: appearance.font }, bold: appearance.bold }
        };
    }

    function getConditionalAppearance(cell) {
        //debugger;
        if (cell.value === "") {
            cell.value = 0;
        }

        if (cell.value > 70) {
            //cell.cellElement.css("background-color", "green");
            return { font: "000000", fill: "008000" };
        }
        else if (cell.value < 28) {
            //cell.cellElement.css("background-color", "red");
            return { font: "000000", fill: "FF0000" };
        }
        else if (cell.value > 28 && cell.value < 70) {
            //cell.cellElement.css("background-color", "yellow");
            return { font: "000000", fill: "FFFF00" };
        }
        else {
            //cell.cellElement.css("background-color", "white");
            return { font: "000000", fill: "FFFFFF" };
        }
    }
}

// PBOX

function location_gridPBOX(data,width) {
    debugger;
    var pivotGrid_location = $("#location_dataGridPBOX").dxPivotGrid({

        //allowSortingBySummary: true,
        allowSorting: false,
        allowFiltering: true,
        showBorders: true,
        showColumnGrandTotals: false,
        showRowGrandTotals: false,
        showRowTotals: false,
        showColumnTotals: false,
        height: 450,
        width: width,
        //width: 1650,
        columnWidth: 100,
        headerFilter: { visible: true },
        fieldPanel: {
            //showColumnFields: true,
            //showDataFields: true,
            showRowFields: true,
            visible: true
        },
        fieldChooser: {
            enabled: false,
            applyChangesMode: "instantly"
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
        //    pageSize: 40
        //},
        dataSource: {

            fields: [{

                //caption: "Setup Type",
                //width: 60,
                //dataField: "SetupType",
                //area: "row",
                //expanded: true
                //}, {
                caption: "Location",
                width: 60,
                dataField: "Loc",
                area: "row",
                expanded: true
            }, {
                caption: "NoofTSGs",
                width: 80,
                dataField: "Nos",
                dataType: "number",
                area: "row",
                expanded: true
                //visible: false,


            }, {
                caption: "WeekNo",
                dataField: "WeekNo",
                area: "column",
                sortBy: "none",
                //dataType: "date",
                //customizeText: function (options) {
                //    return !options.value ? "" : moment(options.value).format("MMM/DD/YYYY").replace(".", "");
                //},
                expanded: true

            },
            //{
            //    caption: "Average",
            //    dataField: "Action",
            //    area: "column",
            //    //sortOrder: "asc",
            //    sortBy: "none",
            //    expanded: true
            //},
            {
                groupname: "WeekNo",
                visible: false,
                expanded: true
            }, {

                dataField: "Average",
                dataType: "number",
                summaryType: "sum",
                area: "data",
                visible: true,
                expanded: true,
            }],
            store: data
        },
        onCellPrepared: function (e) {
            debugger;
            if (e.area === "data" && (e.cell.columnPath[0] === "CW53" || e.cell.columnPath[0] === "CW52" || e.cell.columnPath[0] === "CW51" ||
                e.cell.columnPath[0] === "CW50" || e.cell.columnPath[0] === "CW49" || e.cell.columnPath[0] === "CW48" || e.cell.columnPath[0] === "CW47" || e.cell.columnPath[0] === "CW46" ||
                e.cell.columnPath[0] === "CW45" || e.cell.columnPath[0] === "CW44" || e.cell.columnPath[0] === "CW43" || e.cell.columnPath[0] === "CW42" || e.cell.columnPath[0] === "CW41" ||
                e.cell.columnPath[0] === "CW40" || e.cell.columnPath[0] === "CW39" || e.cell.columnPath[0] === "CW38" || e.cell.columnPath[0] === "CW37" || e.cell.columnPath[0] === "CW36" ||
                e.cell.columnPath[0] === "CW35" || e.cell.columnPath[0] === "CW34" || e.cell.columnPath[0] === "CW33" || e.cell.columnPath[0] === "CW32" || e.cell.columnPath[0] === "CW31" ||
                e.cell.columnPath[0] === "CW30" || e.cell.columnPath[0] === "CW29" || e.cell.columnPath[0] === "CW28" || e.cell.columnPath[0] === "CW27" || e.cell.columnPath[0] === "CW26" ||
                e.cell.columnPath[0] === "CW25" || e.cell.columnPath[0] === "CW24" || e.cell.columnPath[0] === "CW23" || e.cell.columnPath[0] === "CW22" || e.cell.columnPath[0] === "CW21" ||
                e.cell.columnPath[0] === "CW20" || e.cell.columnPath[0] === "CW19" || e.cell.columnPath[0] === "CW18" || e.cell.columnPath[0] === "CW17" || e.cell.columnPath[0] === "CW16" ||
                e.cell.columnPath[0] === "CW15" || e.cell.columnPath[0] === "CW14" || e.cell.columnPath[0] === "CW13" || e.cell.columnPath[0] === "CW12" || e.cell.columnPath[0] === "CW11" ||
                e.cell.columnPath[0] === "CW10" || e.cell.columnPath[0] === "CW09" || e.cell.columnPath[0] === "CW08" || e.cell.columnPath[0] === "CW07" || e.cell.columnPath[0] === "CW06" ||
                e.cell.columnPath[0] === "CW05" || e.cell.columnPath[0] === "CW04" || e.cell.columnPath[0] === "CW03" || e.cell.columnPath[0] === "CW02" || e.cell.columnPath[0] === "CW01")) { //&& e.column.dataField === "Value") {
                debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value > 70) {
                    e.cellElement.css("background-color", "green");
                }
                else if (e.cell.value < 28) {
                    e.cellElement.css("background-color", "red");
                }
                else if (e.cell.value > 28 && e.cell.value < 70) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                }
            }
        },
        onCellClick: function (e) {
            e.cancel = true;
        },

        onExporting: function (e) {
            debugger;
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('PBOXAvg_Locationwise)');

            DevExpress.excelExporter.exportPivotGrid({
                //component: e.component,
                component: $("#location_dataGridPBOX").dxPivotGrid("instance"),
                worksheet: worksheet,
                autoFilterEnabled: true,
                customizeCell: function (options) {
                    var excelCell = options.excelCell;
                    var pivotCell = options.pivotCell;

                    if (isDataCell(pivotCell)) {
                        //debugger;
                        var appearance = getConditionalAppearance(pivotCell);
                        Object.assign(excelCell, getExcelCellFormat(appearance));
                        //Object.assign(excelCell, appearance);
                    }

                    var borderStyle = { style: "thin", color: { argb: "FF7E7E7E" } };
                    excelCell.border = {
                        bottom: borderStyle,
                        left: borderStyle,
                        right: borderStyle,
                        top: borderStyle
                    };
                }

            })

                .then(function (dataGridRange) {
                    debugger;
                    var fields = pivotGrid_location.getDataSource().fields();
                    var rowFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var dataFields = fields.filter(r => r.area === 'data').map(r => `[${r.summaryType}(${r.dataField}])`);
                    var columnFields = [...new Set(fields.filter(r => r.area === 'column').map(r => r.dataField))];
                    var filterFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var appliedFilters = fields.filter(r => r.filterValues !== undefined).map(r => `[${r.dataField}:${r.filterValues}]`);
                    var firstRow = worksheet.getRow(1),
                        fieldPanelCell = firstRow.getCell(1);

                    firstRow.height = 90;
                    //worksheet.mergeCells('A1:D1');
                    fieldPanelCell.value = '\n Field Panel content:'
                        + ` \n - Filter fields: [${filterFields.join(', ')}]`
                        + ` \n - Row fields: [${rowFields.join(', ')}]`
                        + ` \n - Column fields: [${columnFields.join(', ')}]`
                        + ` \n - Data fields: [${dataFields.join(', ')}]`
                        + ` \n - Applied filters: [${appliedFilters.join(', ')}]`;

                    fieldPanelCell.alignment = { horizontal: 'left', vertical: 'top', wrapText: true };
                    fieldPanelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'D8D8D8' } };

                    return Promise.resolve();
                })


                .then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'PBOXAvg_Locationwise.xlsx');
                    });
                });
            e.cancel = true;
        }
    }).dxPivotGrid("instance");

    function isDataCell(cell) {
        return (cell.area === "data" && (cell.columnPath[0] === "CW53" || cell.columnPath[0] === "CW52" || cell.columnPath[0] === "CW51" ||
            cell.columnPath[0] === "CW50" || cell.columnPath[0] === "CW49" || cell.columnPath[0] === "CW48" || cell.columnPath[0] === "CW47" || cell.columnPath[0] === "CW46" ||
            cell.columnPath[0] === "CW45" || cell.columnPath[0] === "CW44" || cell.columnPath[0] === "CW43" || cell.columnPath[0] === "CW42" || cell.columnPath[0] === "CW41" ||
            cell.columnPath[0] === "CW40" || cell.columnPath[0] === "CW39" || cell.columnPath[0] === "CW38" || cell.columnPath[0] === "CW37" || cell.columnPath[0] === "CW36" ||
            cell.columnPath[0] === "CW35" || cell.columnPath[0] === "CW34" || cell.columnPath[0] === "CW33" || cell.columnPath[0] === "CW32" || cell.columnPath[0] === "CW31" ||
            cell.columnPath[0] === "CW30" || cell.columnPath[0] === "CW29" || cell.columnPath[0] === "CW28" || cell.columnPath[0] === "CW27" || cell.columnPath[0] === "CW26" ||
            cell.columnPath[0] === "CW25" || cell.columnPath[0] === "CW24" || cell.columnPath[0] === "CW23" || cell.columnPath[0] === "CW22" || cell.columnPath[0] === "CW21" ||
            cell.columnPath[0] === "CW20" || cell.columnPath[0] === "CW19" || cell.columnPath[0] === "CW18" || cell.columnPath[0] === "CW17" || cell.columnPath[0] === "CW16" ||
            cell.columnPath[0] === "CW15" || cell.columnPath[0] === "CW14" || cell.columnPath[0] === "CW13" || cell.columnPath[0] === "CW12" || cell.columnPath[0] === "CW11" ||
            cell.columnPath[0] === "CW10" || cell.columnPath[0] === "CW09" || cell.columnPath[0] === "CW08" || cell.columnPath[0] === "CW07" || cell.columnPath[0] === "CW06" ||
            cell.columnPath[0] === "CW05" || cell.columnPath[0] === "CW04" || cell.columnPath[0] === "CW03" || cell.columnPath[0] === "CW02" || cell.columnPath[0] === "CW01"));
    }

    function getExcelCellFormat(appearance) {
        return {
            fill: { type: "pattern", pattern: "solid", fgColor: { argb: appearance.fill } },
            font: { color: { argb: appearance.font }, bold: appearance.bold }
        };
    }

    function getConditionalAppearance(cell) {
        //debugger;
        if (cell.value === "") {
            cell.value = 0;
        }

        if (cell.value > 70) {
            //cell.cellElement.css("background-color", "green");
            return { font: "000000", fill: "008000" };
        }
        else if (cell.value < 28) {
            //cell.cellElement.css("background-color", "red");
            return { font: "000000", fill: "FF0000" };
        }
        else if (cell.value > 28 && cell.value < 70) {
            //cell.cellElement.css("background-color", "yellow");
            return { font: "000000", fill: "FFFF00" };
        }
        else {
            //cell.cellElement.css("background-color", "white");
            return { font: "000000", fill: "FFFFFF" };
        }
    }
}

///////// MLC

function location_gridMLC(data,width) {
    debugger;
    var pivotGrid_location = $("#location_dataGridMLC").dxPivotGrid({

        //allowSortingBySummary: true,
        allowSorting: false,
        allowFiltering: true,
        showBorders: true,
        showColumnGrandTotals: false,
        showRowGrandTotals: false,
        showRowTotals: false,
        showColumnTotals: false,
        height: 450,
        //width: 1650,
        width: width,
        columnWidth: 100,
        headerFilter: { visible: true },
        fieldPanel: {
            //showColumnFields: true,
            //showDataFields: true,
            showRowFields: true,
            visible: true
        },
        fieldChooser: {
            enabled: false,
            applyChangesMode: "instantly"
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
        //    pageSize: 40
        //},
        dataSource: {

            fields: [{

                //caption: "Setup Type",
                //width: 60,
                //dataField: "SetupType",
                //area: "row",
                //expanded: true
                //}, {
                caption: "Location",
                width: 60,
                dataField: "Loc",
                area: "row",
                expanded: true
            }, {
                caption: "NoofTSGs",
                width: 80,
                dataField: "Nos",
                dataType: "number",
                area: "row",
                expanded: true
                //visible: false,


            }, {
                caption: "WeekNo",
                dataField: "WeekNo",
                area: "column",
                sortBy: "none",
                //dataType: "date",
                //customizeText: function (options) {
                //    return !options.value ? "" : moment(options.value).format("MMM/DD/YYYY").replace(".", "");
                //},
                expanded: true

            },
            //{
            //    caption: "Average",
            //    dataField: "Action",
            //    area: "column",
            //    //sortOrder: "asc",
            //    sortBy: "none",
            //    expanded: true
            //},
            {
                groupname: "WeekNo",
                visible: false,
                expanded: true
            }, {

                dataField: "Average",
                dataType: "number",
                summaryType: "sum",
                area: "data",
                visible: true,
                expanded: true,
            }],
            store: data
        },
        onCellPrepared: function (e) {
            debugger;
            if (e.area === "data" && (e.cell.columnPath[0] === "CW53" || e.cell.columnPath[0] === "CW52" || e.cell.columnPath[0] === "CW51" ||
                e.cell.columnPath[0] === "CW50" || e.cell.columnPath[0] === "CW49" || e.cell.columnPath[0] === "CW48" || e.cell.columnPath[0] === "CW47" || e.cell.columnPath[0] === "CW46" ||
                e.cell.columnPath[0] === "CW45" || e.cell.columnPath[0] === "CW44" || e.cell.columnPath[0] === "CW43" || e.cell.columnPath[0] === "CW42" || e.cell.columnPath[0] === "CW41" ||
                e.cell.columnPath[0] === "CW40" || e.cell.columnPath[0] === "CW39" || e.cell.columnPath[0] === "CW38" || e.cell.columnPath[0] === "CW37" || e.cell.columnPath[0] === "CW36" ||
                e.cell.columnPath[0] === "CW35" || e.cell.columnPath[0] === "CW34" || e.cell.columnPath[0] === "CW33" || e.cell.columnPath[0] === "CW32" || e.cell.columnPath[0] === "CW31" ||
                e.cell.columnPath[0] === "CW30" || e.cell.columnPath[0] === "CW29" || e.cell.columnPath[0] === "CW28" || e.cell.columnPath[0] === "CW27" || e.cell.columnPath[0] === "CW26" ||
                e.cell.columnPath[0] === "CW25" || e.cell.columnPath[0] === "CW24" || e.cell.columnPath[0] === "CW23" || e.cell.columnPath[0] === "CW22" || e.cell.columnPath[0] === "CW21" ||
                e.cell.columnPath[0] === "CW20" || e.cell.columnPath[0] === "CW19" || e.cell.columnPath[0] === "CW18" || e.cell.columnPath[0] === "CW17" || e.cell.columnPath[0] === "CW16" ||
                e.cell.columnPath[0] === "CW15" || e.cell.columnPath[0] === "CW14" || e.cell.columnPath[0] === "CW13" || e.cell.columnPath[0] === "CW12" || e.cell.columnPath[0] === "CW11" ||
                e.cell.columnPath[0] === "CW10" || e.cell.columnPath[0] === "CW09" || e.cell.columnPath[0] === "CW08" || e.cell.columnPath[0] === "CW07" || e.cell.columnPath[0] === "CW06" ||
                e.cell.columnPath[0] === "CW05" || e.cell.columnPath[0] === "CW04" || e.cell.columnPath[0] === "CW03" || e.cell.columnPath[0] === "CW02" || e.cell.columnPath[0] === "CW01")) { //&& e.column.dataField === "Value") {
                debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value > 70) {
                    e.cellElement.css("background-color", "green");
                }
                else if (e.cell.value < 28) {
                    e.cellElement.css("background-color", "red");
                }
                else if (e.cell.value > 28 && e.cell.value < 70) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                }
            }
        },
        onCellClick: function (e) {
            e.cancel = true;
        },

        onExporting: function (e) {
            debugger;
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('MLCAvg_Locationwise)');

            DevExpress.excelExporter.exportPivotGrid({
                //component: e.component,
                component: $("#location_dataGridMLC").dxPivotGrid("instance"),
                worksheet: worksheet,
                autoFilterEnabled: true,
                customizeCell: function (options) {
                    var excelCell = options.excelCell;
                    var pivotCell = options.pivotCell;

                    if (isDataCell(pivotCell)) {
                        //debugger;
                        var appearance = getConditionalAppearance(pivotCell);
                        Object.assign(excelCell, getExcelCellFormat(appearance));
                        //Object.assign(excelCell, appearance);
                    }

                    var borderStyle = { style: "thin", color: { argb: "FF7E7E7E" } };
                    excelCell.border = {
                        bottom: borderStyle,
                        left: borderStyle,
                        right: borderStyle,
                        top: borderStyle
                    };
                }

            })

                .then(function (dataGridRange) {
                    debugger;
                    var fields = pivotGrid_location.getDataSource().fields();
                    var rowFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var dataFields = fields.filter(r => r.area === 'data').map(r => `[${r.summaryType}(${r.dataField}])`);
                    var columnFields = [...new Set(fields.filter(r => r.area === 'column').map(r => r.dataField))];
                    var filterFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var appliedFilters = fields.filter(r => r.filterValues !== undefined).map(r => `[${r.dataField}:${r.filterValues}]`);
                    var firstRow = worksheet.getRow(1),
                        fieldPanelCell = firstRow.getCell(1);

                    firstRow.height = 90;
                    //worksheet.mergeCells('A1:D1');
                    fieldPanelCell.value = '\n Field Panel content:'
                        + ` \n - Filter fields: [${filterFields.join(', ')}]`
                        + ` \n - Row fields: [${rowFields.join(', ')}]`
                        + ` \n - Column fields: [${columnFields.join(', ')}]`
                        + ` \n - Data fields: [${dataFields.join(', ')}]`
                        + ` \n - Applied filters: [${appliedFilters.join(', ')}]`;

                    fieldPanelCell.alignment = { horizontal: 'left', vertical: 'top', wrapText: true };
                    fieldPanelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'D8D8D8' } };

                    return Promise.resolve();
                })


                .then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'MLCAvg_Locationwise.xlsx');
                    });
                });
            e.cancel = true;
        }
    }).dxPivotGrid("instance");

    function isDataCell(cell) {
        return (cell.area === "data" && (cell.columnPath[0] === "CW53" || cell.columnPath[0] === "CW52" || cell.columnPath[0] === "CW51" ||
            cell.columnPath[0] === "CW50" || cell.columnPath[0] === "CW49" || cell.columnPath[0] === "CW48" || cell.columnPath[0] === "CW47" || cell.columnPath[0] === "CW46" ||
            cell.columnPath[0] === "CW45" || cell.columnPath[0] === "CW44" || cell.columnPath[0] === "CW43" || cell.columnPath[0] === "CW42" || cell.columnPath[0] === "CW41" ||
            cell.columnPath[0] === "CW40" || cell.columnPath[0] === "CW39" || cell.columnPath[0] === "CW38" || cell.columnPath[0] === "CW37" || cell.columnPath[0] === "CW36" ||
            cell.columnPath[0] === "CW35" || cell.columnPath[0] === "CW34" || cell.columnPath[0] === "CW33" || cell.columnPath[0] === "CW32" || cell.columnPath[0] === "CW31" ||
            cell.columnPath[0] === "CW30" || cell.columnPath[0] === "CW29" || cell.columnPath[0] === "CW28" || cell.columnPath[0] === "CW27" || cell.columnPath[0] === "CW26" ||
            cell.columnPath[0] === "CW25" || cell.columnPath[0] === "CW24" || cell.columnPath[0] === "CW23" || cell.columnPath[0] === "CW22" || cell.columnPath[0] === "CW21" ||
            cell.columnPath[0] === "CW20" || cell.columnPath[0] === "CW19" || cell.columnPath[0] === "CW18" || cell.columnPath[0] === "CW17" || cell.columnPath[0] === "CW16" ||
            cell.columnPath[0] === "CW15" || cell.columnPath[0] === "CW14" || cell.columnPath[0] === "CW13" || cell.columnPath[0] === "CW12" || cell.columnPath[0] === "CW11" ||
            cell.columnPath[0] === "CW10" || cell.columnPath[0] === "CW09" || cell.columnPath[0] === "CW08" || cell.columnPath[0] === "CW07" || cell.columnPath[0] === "CW06" ||
            cell.columnPath[0] === "CW05" || cell.columnPath[0] === "CW04" || cell.columnPath[0] === "CW03" || cell.columnPath[0] === "CW02" || cell.columnPath[0] === "CW01"));
    }

    function getExcelCellFormat(appearance) {
        return {
            fill: { type: "pattern", pattern: "solid", fgColor: { argb: appearance.fill } },
            font: { color: { argb: appearance.font }, bold: appearance.bold }
        };
    }

    function getConditionalAppearance(cell) {
        //debugger;
        if (cell.value === "") {
            cell.value = 0;
        }

        if (cell.value > 70) {
            //cell.cellElement.css("background-color", "green");
            return { font: "000000", fill: "008000" };
        }
        else if (cell.value < 28) {
            //cell.cellElement.css("background-color", "red");
            return { font: "000000", fill: "FF0000" };
        }
        else if (cell.value > 28 && cell.value < 70) {
            //cell.cellElement.css("background-color", "yellow");
            return { font: "000000", fill: "FFFF00" };
        }
        else {
            //cell.cellElement.css("background-color", "white");
            return { font: "000000", fill: "FFFFFF" };
        }
    }
}

////////// ACUROT

function location_gridACUROT(data,width) {
    debugger;
    var pivotGrid_location = $("#location_dataGridACUROT").dxPivotGrid({

        //allowSortingBySummary: true,
        allowSorting: false,
        allowFiltering: true,
        showBorders: true,
        showColumnGrandTotals: false,
        showRowGrandTotals: false,
        showRowTotals: false,
        showColumnTotals: false,
        height: 450,
        //width: 1650,
        width: width,
        columnWidth: 100,
        headerFilter: { visible: true },
        fieldPanel: {
            //showColumnFields: true,
            //showDataFields: true,
            showRowFields: true,
            visible: true
        },
        fieldChooser: {
            enabled: false,
            applyChangesMode: "instantly"
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
        //    pageSize: 40
        //},
        dataSource: {

            fields: [{

                //caption: "Setup Type",
                //width: 60,
                //dataField: "SetupType",
                //area: "row",
                //expanded: true
                //}, {
                caption: "Location",
                width: 60,
                dataField: "Loc",
                area: "row",
                expanded: true
            }, {
                caption: "NoofTSGs",
                width: 80,
                dataField: "Nos",
                dataType: "number",
                area: "row",
                expanded: true
                //visible: false,


            }, {
                caption: "WeekNo",
                dataField: "WeekNo",
                area: "column",
                sortBy: "none",
                //dataType: "date",
                //customizeText: function (options) {
                //    return !options.value ? "" : moment(options.value).format("MMM/DD/YYYY").replace(".", "");
                //},
                expanded: true

            },
            //{
            //    caption: "Average",
            //    dataField: "Action",
            //    area: "column",
            //    //sortOrder: "asc",
            //    sortBy: "none",
            //    expanded: true
            //},
            {
                groupname: "WeekNo",
                visible: false,
                expanded: true
            }, {

                dataField: "Average",
                dataType: "number",
                summaryType: "sum",
                area: "data",
                visible: true,
                expanded: true,
            }],
            store: data
        },
        onCellPrepared: function (e) {
            debugger;
            if (e.area === "data" && (e.cell.columnPath[0] === "CW53" || e.cell.columnPath[0] === "CW52" || e.cell.columnPath[0] === "CW51" ||
                e.cell.columnPath[0] === "CW50" || e.cell.columnPath[0] === "CW49" || e.cell.columnPath[0] === "CW48" || e.cell.columnPath[0] === "CW47" || e.cell.columnPath[0] === "CW46" ||
                e.cell.columnPath[0] === "CW45" || e.cell.columnPath[0] === "CW44" || e.cell.columnPath[0] === "CW43" || e.cell.columnPath[0] === "CW42" || e.cell.columnPath[0] === "CW41" ||
                e.cell.columnPath[0] === "CW40" || e.cell.columnPath[0] === "CW39" || e.cell.columnPath[0] === "CW38" || e.cell.columnPath[0] === "CW37" || e.cell.columnPath[0] === "CW36" ||
                e.cell.columnPath[0] === "CW35" || e.cell.columnPath[0] === "CW34" || e.cell.columnPath[0] === "CW33" || e.cell.columnPath[0] === "CW32" || e.cell.columnPath[0] === "CW31" ||
                e.cell.columnPath[0] === "CW30" || e.cell.columnPath[0] === "CW29" || e.cell.columnPath[0] === "CW28" || e.cell.columnPath[0] === "CW27" || e.cell.columnPath[0] === "CW26" ||
                e.cell.columnPath[0] === "CW25" || e.cell.columnPath[0] === "CW24" || e.cell.columnPath[0] === "CW23" || e.cell.columnPath[0] === "CW22" || e.cell.columnPath[0] === "CW21" ||
                e.cell.columnPath[0] === "CW20" || e.cell.columnPath[0] === "CW19" || e.cell.columnPath[0] === "CW18" || e.cell.columnPath[0] === "CW17" || e.cell.columnPath[0] === "CW16" ||
                e.cell.columnPath[0] === "CW15" || e.cell.columnPath[0] === "CW14" || e.cell.columnPath[0] === "CW13" || e.cell.columnPath[0] === "CW12" || e.cell.columnPath[0] === "CW11" ||
                e.cell.columnPath[0] === "CW10" || e.cell.columnPath[0] === "CW09" || e.cell.columnPath[0] === "CW08" || e.cell.columnPath[0] === "CW07" || e.cell.columnPath[0] === "CW06" ||
                e.cell.columnPath[0] === "CW05" || e.cell.columnPath[0] === "CW04" || e.cell.columnPath[0] === "CW03" || e.cell.columnPath[0] === "CW02" || e.cell.columnPath[0] === "CW01")) { //&& e.column.dataField === "Value") {
                debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value > 70) {
                    e.cellElement.css("background-color", "green");
                }
                else if (e.cell.value < 28) {
                    e.cellElement.css("background-color", "red");
                }
                else if (e.cell.value > 28 && e.cell.value < 70) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                }
            }
        },
        onCellClick: function (e) {
            e.cancel = true;
        },

        onExporting: function (e) {
            debugger;
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('ACUROTAvg_Locationwise)');

            DevExpress.excelExporter.exportPivotGrid({
                //component: e.component,
                component: $("#location_dataGridACUROT").dxPivotGrid("instance"),
                worksheet: worksheet,
                autoFilterEnabled: true,
                customizeCell: function (options) {
                    var excelCell = options.excelCell;
                    var pivotCell = options.pivotCell;

                    if (isDataCell(pivotCell)) {
                        //debugger;
                        var appearance = getConditionalAppearance(pivotCell);
                        Object.assign(excelCell, getExcelCellFormat(appearance));
                        //Object.assign(excelCell, appearance);
                    }

                    var borderStyle = { style: "thin", color: { argb: "FF7E7E7E" } };
                    excelCell.border = {
                        bottom: borderStyle,
                        left: borderStyle,
                        right: borderStyle,
                        top: borderStyle
                    };
                }

            })

                .then(function (dataGridRange) {
                    debugger;
                    var fields = pivotGrid_location.getDataSource().fields();
                    var rowFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var dataFields = fields.filter(r => r.area === 'data').map(r => `[${r.summaryType}(${r.dataField}])`);
                    var columnFields = [...new Set(fields.filter(r => r.area === 'column').map(r => r.dataField))];
                    var filterFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var appliedFilters = fields.filter(r => r.filterValues !== undefined).map(r => `[${r.dataField}:${r.filterValues}]`);
                    var firstRow = worksheet.getRow(1),
                        fieldPanelCell = firstRow.getCell(1);

                    firstRow.height = 90;
                    //worksheet.mergeCells('A1:D1');
                    fieldPanelCell.value = '\n Field Panel content:'
                        + ` \n - Filter fields: [${filterFields.join(', ')}]`
                        + ` \n - Row fields: [${rowFields.join(', ')}]`
                        + ` \n - Column fields: [${columnFields.join(', ')}]`
                        + ` \n - Data fields: [${dataFields.join(', ')}]`
                        + ` \n - Applied filters: [${appliedFilters.join(', ')}]`;

                    fieldPanelCell.alignment = { horizontal: 'left', vertical: 'top', wrapText: true };
                    fieldPanelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'D8D8D8' } };

                    return Promise.resolve();
                })


                .then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'ACUROTAvg_Locationwise.xlsx');
                    });
                });
            e.cancel = true;
        }
    }).dxPivotGrid("instance");

    function isDataCell(cell) {
        return (cell.area === "data" && (cell.columnPath[0] === "CW53" || cell.columnPath[0] === "CW52" || cell.columnPath[0] === "CW51" ||
            cell.columnPath[0] === "CW50" || cell.columnPath[0] === "CW49" || cell.columnPath[0] === "CW48" || cell.columnPath[0] === "CW47" || cell.columnPath[0] === "CW46" ||
            cell.columnPath[0] === "CW45" || cell.columnPath[0] === "CW44" || cell.columnPath[0] === "CW43" || cell.columnPath[0] === "CW42" || cell.columnPath[0] === "CW41" ||
            cell.columnPath[0] === "CW40" || cell.columnPath[0] === "CW39" || cell.columnPath[0] === "CW38" || cell.columnPath[0] === "CW37" || cell.columnPath[0] === "CW36" ||
            cell.columnPath[0] === "CW35" || cell.columnPath[0] === "CW34" || cell.columnPath[0] === "CW33" || cell.columnPath[0] === "CW32" || cell.columnPath[0] === "CW31" ||
            cell.columnPath[0] === "CW30" || cell.columnPath[0] === "CW29" || cell.columnPath[0] === "CW28" || cell.columnPath[0] === "CW27" || cell.columnPath[0] === "CW26" ||
            cell.columnPath[0] === "CW25" || cell.columnPath[0] === "CW24" || cell.columnPath[0] === "CW23" || cell.columnPath[0] === "CW22" || cell.columnPath[0] === "CW21" ||
            cell.columnPath[0] === "CW20" || cell.columnPath[0] === "CW19" || cell.columnPath[0] === "CW18" || cell.columnPath[0] === "CW17" || cell.columnPath[0] === "CW16" ||
            cell.columnPath[0] === "CW15" || cell.columnPath[0] === "CW14" || cell.columnPath[0] === "CW13" || cell.columnPath[0] === "CW12" || cell.columnPath[0] === "CW11" ||
            cell.columnPath[0] === "CW10" || cell.columnPath[0] === "CW09" || cell.columnPath[0] === "CW08" || cell.columnPath[0] === "CW07" || cell.columnPath[0] === "CW06" ||
            cell.columnPath[0] === "CW05" || cell.columnPath[0] === "CW04" || cell.columnPath[0] === "CW03" || cell.columnPath[0] === "CW02" || cell.columnPath[0] === "CW01"));
    }

    function getExcelCellFormat(appearance) {
        return {
            fill: { type: "pattern", pattern: "solid", fgColor: { argb: appearance.fill } },
            font: { color: { argb: appearance.font }, bold: appearance.bold }
        };
    }

    function getConditionalAppearance(cell) {
        //debugger;
        if (cell.value === "") {
            cell.value = 0;
        }

        if (cell.value > 70) {
            //cell.cellElement.css("background-color", "green");
            return { font: "000000", fill: "008000" };
        }
        else if (cell.value < 28) {
            //cell.cellElement.css("background-color", "red");
            return { font: "000000", fill: "FF0000" };
        }
        else if (cell.value > 28 && cell.value < 70) {
            //cell.cellElement.css("background-color", "yellow");
            return { font: "000000", fill: "FFFF00" };
        }
        else {
            //cell.cellElement.css("background-color", "white");
            return { font: "000000", fill: "FFFFFF" };
        }
    }
}


////// TSG4
function project_gridTSG4(data, width) {
    debugger;
    var pivotGrid_project = $("#project_dataGridTSG4").dxPivotGrid({
        //allowSortingBySummary: true,
        allowSorting: false,
        allowFiltering: true,
        showBorders: true,
        showColumnGrandTotals: false,
        showRowGrandTotals: false,
        showRowTotals: false,
        showColumnTotals: false,
        height: 500,
        //width: 1650,
        width: width,
        columnWidth: 100,
        headerFilter: { visible: true },
        fieldPanel: {
            //showColumnFields: true,
            //showDataFields: true,
            showRowFields: true,
            visible: true
        },
        fieldChooser: {
            enabled: false,
            applyChangesMode: "instantly"
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
        //    pageSize: 40
        //},
        dataSource: {
            fields: [{
            //    caption: "Setup Type",
            //    width: 60,
            //    dataField: "SetupType",
            //    area: "row",
            //    expanded: true
            //}, {
                width: 60,
                dataField: "Location",
                area: "row",
                expanded: true
            },{

                width: 60,
                dataField: "Project",
                area: "row",
                expanded: true
                
            }, {

                width: 500,
                dataField: "ComputerIds",
                caption: "TSGs",
                area: "row",
                expanded: true
            }, {

                width: 80,
                dataField: "Nos",
                caption: "NoofTSGs",
                area: "row",
                expanded: true
                //visible: false,
                


            },
            {
                dataField: "WeekNo",
                area: "column",
                sortBy: "none",
                //dataType: "date",
                //customizeText: function (options) {
                //    return !options.value ? "" : moment(options.value).format("MMM/DD/YYYY").replace(".", "");
                //},
                expanded: true

            }, {
                groupname: "WeekNo",
                visible: false,
                expanded: true
            }, {

                dataField: "Average",
                dataType: "number",
                summaryType: "sum",
                area: "data",
                visible: true,
                expanded: true,
            }],
            store: data
        },
        onCellPrepared: function (e) {
            debugger;
            if (e.area === "data" && (e.cell.columnPath[0] === "CW53" || e.cell.columnPath[0] === "CW52" || e.cell.columnPath[0] === "CW51" ||
                e.cell.columnPath[0] === "CW50" || e.cell.columnPath[0] === "CW49" || e.cell.columnPath[0] === "CW48" || e.cell.columnPath[0] === "CW47" || e.cell.columnPath[0] === "CW46" ||
                e.cell.columnPath[0] === "CW45" || e.cell.columnPath[0] === "CW44" || e.cell.columnPath[0] === "CW43" || e.cell.columnPath[0] === "CW42" || e.cell.columnPath[0] === "CW41" ||
                e.cell.columnPath[0] === "CW40" || e.cell.columnPath[0] === "CW39" || e.cell.columnPath[0] === "CW38" || e.cell.columnPath[0] === "CW37" || e.cell.columnPath[0] === "CW36" ||
                e.cell.columnPath[0] === "CW35" || e.cell.columnPath[0] === "CW34" || e.cell.columnPath[0] === "CW33" || e.cell.columnPath[0] === "CW32" || e.cell.columnPath[0] === "CW31" ||
                e.cell.columnPath[0] === "CW30" || e.cell.columnPath[0] === "CW29" || e.cell.columnPath[0] === "CW28" || e.cell.columnPath[0] === "CW27" || e.cell.columnPath[0] === "CW26" ||
                e.cell.columnPath[0] === "CW25" || e.cell.columnPath[0] === "CW24" || e.cell.columnPath[0] === "CW23" || e.cell.columnPath[0] === "CW22" || e.cell.columnPath[0] === "CW21" ||
                e.cell.columnPath[0] === "CW20" || e.cell.columnPath[0] === "CW19" || e.cell.columnPath[0] === "CW18" || e.cell.columnPath[0] === "CW17" || e.cell.columnPath[0] === "CW16" ||
                e.cell.columnPath[0] === "CW15" || e.cell.columnPath[0] === "CW14" || e.cell.columnPath[0] === "CW13" || e.cell.columnPath[0] === "CW12" || e.cell.columnPath[0] === "CW11" ||
                e.cell.columnPath[0] === "CW10" || e.cell.columnPath[0] === "CW09" || e.cell.columnPath[0] === "CW08" || e.cell.columnPath[0] === "CW07" || e.cell.columnPath[0] === "CW06" ||
                e.cell.columnPath[0] === "CW05" || e.cell.columnPath[0] === "CW04" || e.cell.columnPath[0] === "CW03" || e.cell.columnPath[0] === "CW02" || e.cell.columnPath[0] === "CW01"  )) { //&& e.column.dataField === "Value") {
                debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value >= 70) {
                    e.cellElement.css("background-color", "green");
                }
                else if (e.cell.value <= 28) {
                    e.cellElement.css("background-color", "red");
                }
                else if (e.cell.value > 28 && e.cell.value < 70) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                }
            }
        },
        onCellClick: function (e) {
            e.cancel = true;
        },

        onExporting: function (e) {
            debugger;
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('TSG4Avg_Projectwise)');

            DevExpress.excelExporter.exportPivotGrid({
                //component: e.component,
                component: $("#project_dataGridTSG4").dxPivotGrid("instance"),
                worksheet: worksheet,
                autoFilterEnabled: true,
                customizeCell: function (options) {
                    var excelCell = options.excelCell;
                    var pivotCell = options.pivotCell;

                    if (isDataCell(pivotCell)) {
                        //debugger;
                        var appearance = getConditionalAppearance(pivotCell);
                        Object.assign(excelCell, getExcelCellFormat(appearance));
                        //Object.assign(excelCell, appearance);
                    }

                    var borderStyle = { style: "thin", color: { argb: "FF7E7E7E" } };
                    excelCell.border = {
                        bottom: borderStyle,
                        left: borderStyle,
                        right: borderStyle,
                        top: borderStyle
                    };
                }

            })

                .then(function (dataGridRange) {
                    debugger;
                    var fields = pivotGrid_project.getDataSource().fields();
                    var rowFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var dataFields = fields.filter(r => r.area === 'data').map(r => `[${r.summaryType}(${r.dataField}])`);
                    var columnFields = [...new Set(fields.filter(r => r.area === 'column').map(r => r.dataField))];
                    var filterFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var appliedFilters = fields.filter(r => r.filterValues !== undefined).map(r => `[${r.dataField}:${r.filterValues}]`);
                    var firstRow = worksheet.getRow(1),
                        fieldPanelCell = firstRow.getCell(1);

                    firstRow.height = 90;
                    //worksheet.mergeCells('A1:D1');
                    fieldPanelCell.value = '\n Field Panel content:'
                        + ` \n - Filter fields: [${filterFields.join(', ')}]`
                        + ` \n - Row fields: [${rowFields.join(', ')}]`
                        + ` \n - Column fields: [${columnFields.join(', ')}]`
                        + ` \n - Data fields: [${dataFields.join(', ')}]`
                        + ` \n - Applied filters: [${appliedFilters.join(', ')}]`;

                    fieldPanelCell.alignment = { horizontal: 'left', vertical: 'top', wrapText: true };
                    fieldPanelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'D8D8D8' } };

                    return Promise.resolve();
                })


                .then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'TSG4Avg_Projectwise.xlsx');
                    });
                });
            e.cancel = true;
        }
    }).dxPivotGrid("instance");

    function isDataCell(cell) {
        return (cell.area === "data" && (cell.columnPath[0] === "CW53" || cell.columnPath[0] === "CW52" || cell.columnPath[0] === "CW51" ||
            cell.columnPath[0] === "CW50" || cell.columnPath[0] === "CW49" || cell.columnPath[0] === "CW48" || cell.columnPath[0] === "CW47" || cell.columnPath[0] === "CW46" ||
            cell.columnPath[0] === "CW45" || cell.columnPath[0] === "CW44" || cell.columnPath[0] === "CW43" || cell.columnPath[0] === "CW42" || cell.columnPath[0] === "CW41" ||
            cell.columnPath[0] === "CW40" || cell.columnPath[0] === "CW39" || cell.columnPath[0] === "CW38" || cell.columnPath[0] === "CW37" || cell.columnPath[0] === "CW36" ||
            cell.columnPath[0] === "CW35" || cell.columnPath[0] === "CW34" || cell.columnPath[0] === "CW33" || cell.columnPath[0] === "CW32" || cell.columnPath[0] === "CW31" ||
            cell.columnPath[0] === "CW30" || cell.columnPath[0] === "CW29" || cell.columnPath[0] === "CW28" || cell.columnPath[0] === "CW27" || cell.columnPath[0] === "CW26" ||
            cell.columnPath[0] === "CW25" || cell.columnPath[0] === "CW24" || cell.columnPath[0] === "CW23" || cell.columnPath[0] === "CW22" || cell.columnPath[0] === "CW21" ||
            cell.columnPath[0] === "CW20" || cell.columnPath[0] === "CW19" || cell.columnPath[0] === "CW18" || cell.columnPath[0] === "CW17" || cell.columnPath[0] === "CW16" ||
            cell.columnPath[0] === "CW15" || cell.columnPath[0] === "CW14" || cell.columnPath[0] === "CW13" || cell.columnPath[0] === "CW12" || cell.columnPath[0] === "CW11" ||
            cell.columnPath[0] === "CW10" || cell.columnPath[0] === "CW09" || cell.columnPath[0] === "CW08" || cell.columnPath[0] === "CW07" || cell.columnPath[0] === "CW06" ||
            cell.columnPath[0] === "CW05" || cell.columnPath[0] === "CW04" || cell.columnPath[0] === "CW03" || cell.columnPath[0] === "CW02" || cell.columnPath[0] === "CW01"));
    }

    function getExcelCellFormat(appearance) {
        return {
            fill: { type: "pattern", pattern: "solid", fgColor: { argb: appearance.fill } },
            font: { color: { argb: appearance.font }, bold: appearance.bold }
        };
    }

    function getConditionalAppearance(cell) {
        //debugger;
        if (cell.value === "") {
            cell.value = 0;
        }

        if (cell.value >= 70) {
            //cell.cellElement.css("background-color", "green");
            return { font: "000000", fill: "008000" };
        }
        else if (cell.value <= 28) {
            //cell.cellElement.css("background-color", "red");
            return { font: "000000", fill: "FF0000" };
        }
        else if (cell.value > 28 && cell.value < 70) {
            //cell.cellElement.css("background-color", "yellow");
            return { font: "000000", fill: "FFFF00" };
        }
        else {
            //cell.cellElement.css("background-color", "white");
            return { font: "000000", fill: "FFFFFF" };
        }
    }
}

///////// PBOX

function project_gridPBOX(data, width) {
    debugger;
    var pivotGrid_project = $("#project_dataGridPBOX").dxPivotGrid({
        //allowSortingBySummary: true,
        allowSorting: false,
        allowFiltering: true,
        showBorders: true,
        showColumnGrandTotals: false,
        showRowGrandTotals: false,
        showRowTotals: false,
        showColumnTotals: false,
        height: 500,
        //width: 1650,
        width: width,
        columnWidth: 100,
        headerFilter: { visible: true },
        fieldPanel: {
            //showColumnFields: true,
            //showDataFields: true,
            showRowFields: true,
            visible: true
        },
        fieldChooser: {
            enabled: false,
            applyChangesMode: "instantly"
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
        //    pageSize: 40
        //},
        dataSource: {
            fields: [{
                //    caption: "Setup Type",
                //    width: 60,
                //    dataField: "SetupType",
                //    area: "row",
                //    expanded: true
                //}, {
                width: 60,
                dataField: "Location",
                area: "row",
                expanded: true
            }, {

                width: 60,
                dataField: "Project",
                area: "row",
                expanded: true

            }, {

                width: 500,
                dataField: "ComputerIds",
                caption: "TSGs",
                area: "row",
                expanded: true
            }, {

                width: 80,
                dataField: "Nos",
                caption: "NoofTSGs",
                area: "row",
                expanded: true
                //visible: false,



            },
            {
                dataField: "WeekNo",
                area: "column",
                sortBy: "none",
                //dataType: "date",
                //customizeText: function (options) {
                //    return !options.value ? "" : moment(options.value).format("MMM/DD/YYYY").replace(".", "");
                //},
                expanded: true

            }, {
                groupname: "WeekNo",
                visible: false,
                expanded: true
            }, {

                dataField: "Average",
                dataType: "number",
                summaryType: "sum",
                area: "data",
                visible: true,
                expanded: true,
            }],
            store: data
        },
        onCellPrepared: function (e) {
            debugger;
            if (e.area === "data" && (e.cell.columnPath[0] === "CW53" || e.cell.columnPath[0] === "CW52" || e.cell.columnPath[0] === "CW51" ||
                e.cell.columnPath[0] === "CW50" || e.cell.columnPath[0] === "CW49" || e.cell.columnPath[0] === "CW48" || e.cell.columnPath[0] === "CW47" || e.cell.columnPath[0] === "CW46" ||
                e.cell.columnPath[0] === "CW45" || e.cell.columnPath[0] === "CW44" || e.cell.columnPath[0] === "CW43" || e.cell.columnPath[0] === "CW42" || e.cell.columnPath[0] === "CW41" ||
                e.cell.columnPath[0] === "CW40" || e.cell.columnPath[0] === "CW39" || e.cell.columnPath[0] === "CW38" || e.cell.columnPath[0] === "CW37" || e.cell.columnPath[0] === "CW36" ||
                e.cell.columnPath[0] === "CW35" || e.cell.columnPath[0] === "CW34" || e.cell.columnPath[0] === "CW33" || e.cell.columnPath[0] === "CW32" || e.cell.columnPath[0] === "CW31" ||
                e.cell.columnPath[0] === "CW30" || e.cell.columnPath[0] === "CW29" || e.cell.columnPath[0] === "CW28" || e.cell.columnPath[0] === "CW27" || e.cell.columnPath[0] === "CW26" ||
                e.cell.columnPath[0] === "CW25" || e.cell.columnPath[0] === "CW24" || e.cell.columnPath[0] === "CW23" || e.cell.columnPath[0] === "CW22" || e.cell.columnPath[0] === "CW21" ||
                e.cell.columnPath[0] === "CW20" || e.cell.columnPath[0] === "CW19" || e.cell.columnPath[0] === "CW18" || e.cell.columnPath[0] === "CW17" || e.cell.columnPath[0] === "CW16" ||
                e.cell.columnPath[0] === "CW15" || e.cell.columnPath[0] === "CW14" || e.cell.columnPath[0] === "CW13" || e.cell.columnPath[0] === "CW12" || e.cell.columnPath[0] === "CW11" ||
                e.cell.columnPath[0] === "CW10" || e.cell.columnPath[0] === "CW09" || e.cell.columnPath[0] === "CW08" || e.cell.columnPath[0] === "CW07" || e.cell.columnPath[0] === "CW06" ||
                e.cell.columnPath[0] === "CW05" || e.cell.columnPath[0] === "CW04" || e.cell.columnPath[0] === "CW03" || e.cell.columnPath[0] === "CW02" || e.cell.columnPath[0] === "CW01")) { //&& e.column.dataField === "Value") {
                debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value >= 70) {
                    e.cellElement.css("background-color", "green");
                }
                else if (e.cell.value <= 28) {
                    e.cellElement.css("background-color", "red");
                }
                else if (e.cell.value > 28 && e.cell.value < 70) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                }
            }
        },
        onCellClick: function (e) {
            e.cancel = true;
        },

        onExporting: function (e) {
            debugger;
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('PBOXAvg_Projectwise)');

            DevExpress.excelExporter.exportPivotGrid({
                //component: e.component,
                component: $("#project_dataGridPBOX").dxPivotGrid("instance"),
                worksheet: worksheet,
                autoFilterEnabled: true,
                customizeCell: function (options) {
                    var excelCell = options.excelCell;
                    var pivotCell = options.pivotCell;

                    if (isDataCell(pivotCell)) {
                        //debugger;
                        var appearance = getConditionalAppearance(pivotCell);
                        Object.assign(excelCell, getExcelCellFormat(appearance));
                        //Object.assign(excelCell, appearance);
                    }

                    var borderStyle = { style: "thin", color: { argb: "FF7E7E7E" } };
                    excelCell.border = {
                        bottom: borderStyle,
                        left: borderStyle,
                        right: borderStyle,
                        top: borderStyle
                    };
                }

            })

                .then(function (dataGridRange) {
                    debugger;
                    var fields = pivotGrid_project.getDataSource().fields();
                    var rowFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var dataFields = fields.filter(r => r.area === 'data').map(r => `[${r.summaryType}(${r.dataField}])`);
                    var columnFields = [...new Set(fields.filter(r => r.area === 'column').map(r => r.dataField))];
                    var filterFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var appliedFilters = fields.filter(r => r.filterValues !== undefined).map(r => `[${r.dataField}:${r.filterValues}]`);
                    var firstRow = worksheet.getRow(1),
                        fieldPanelCell = firstRow.getCell(1);

                    firstRow.height = 90;
                    //worksheet.mergeCells('A1:D1');
                    fieldPanelCell.value = '\n Field Panel content:'
                        + ` \n - Filter fields: [${filterFields.join(', ')}]`
                        + ` \n - Row fields: [${rowFields.join(', ')}]`
                        + ` \n - Column fields: [${columnFields.join(', ')}]`
                        + ` \n - Data fields: [${dataFields.join(', ')}]`
                        + ` \n - Applied filters: [${appliedFilters.join(', ')}]`;

                    fieldPanelCell.alignment = { horizontal: 'left', vertical: 'top', wrapText: true };
                    fieldPanelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'D8D8D8' } };

                    return Promise.resolve();
                })


                .then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'PBOXAvg_Projectwise.xlsx');
                    });
                });
            e.cancel = true;
        }
    }).dxPivotGrid("instance");

    function isDataCell(cell) {
        return (cell.area === "data" && (cell.columnPath[0] === "CW53" || cell.columnPath[0] === "CW52" || cell.columnPath[0] === "CW51" ||
            cell.columnPath[0] === "CW50" || cell.columnPath[0] === "CW49" || cell.columnPath[0] === "CW48" || cell.columnPath[0] === "CW47" || cell.columnPath[0] === "CW46" ||
            cell.columnPath[0] === "CW45" || cell.columnPath[0] === "CW44" || cell.columnPath[0] === "CW43" || cell.columnPath[0] === "CW42" || cell.columnPath[0] === "CW41" ||
            cell.columnPath[0] === "CW40" || cell.columnPath[0] === "CW39" || cell.columnPath[0] === "CW38" || cell.columnPath[0] === "CW37" || cell.columnPath[0] === "CW36" ||
            cell.columnPath[0] === "CW35" || cell.columnPath[0] === "CW34" || cell.columnPath[0] === "CW33" || cell.columnPath[0] === "CW32" || cell.columnPath[0] === "CW31" ||
            cell.columnPath[0] === "CW30" || cell.columnPath[0] === "CW29" || cell.columnPath[0] === "CW28" || cell.columnPath[0] === "CW27" || cell.columnPath[0] === "CW26" ||
            cell.columnPath[0] === "CW25" || cell.columnPath[0] === "CW24" || cell.columnPath[0] === "CW23" || cell.columnPath[0] === "CW22" || cell.columnPath[0] === "CW21" ||
            cell.columnPath[0] === "CW20" || cell.columnPath[0] === "CW19" || cell.columnPath[0] === "CW18" || cell.columnPath[0] === "CW17" || cell.columnPath[0] === "CW16" ||
            cell.columnPath[0] === "CW15" || cell.columnPath[0] === "CW14" || cell.columnPath[0] === "CW13" || cell.columnPath[0] === "CW12" || cell.columnPath[0] === "CW11" ||
            cell.columnPath[0] === "CW10" || cell.columnPath[0] === "CW09" || cell.columnPath[0] === "CW08" || cell.columnPath[0] === "CW07" || cell.columnPath[0] === "CW06" ||
            cell.columnPath[0] === "CW05" || cell.columnPath[0] === "CW04" || cell.columnPath[0] === "CW03" || cell.columnPath[0] === "CW02" || cell.columnPath[0] === "CW01"));
    }

    function getExcelCellFormat(appearance) {
        return {
            fill: { type: "pattern", pattern: "solid", fgColor: { argb: appearance.fill } },
            font: { color: { argb: appearance.font }, bold: appearance.bold }
        };
    }

    function getConditionalAppearance(cell) {
        //debugger;
        if (cell.value === "") {
            cell.value = 0;
        }

        if (cell.value >= 70) {
            //cell.cellElement.css("background-color", "green");
            return { font: "000000", fill: "008000" };
        }
        else if (cell.value <= 28) {
            //cell.cellElement.css("background-color", "red");
            return { font: "000000", fill: "FF0000" };
        }
        else if (cell.value > 28 && cell.value < 70) {
            //cell.cellElement.css("background-color", "yellow");
            return { font: "000000", fill: "FFFF00" };
        }
        else {
            //cell.cellElement.css("background-color", "white");
            return { font: "000000", fill: "FFFFFF" };
        }
    }
}

/////// MLC

function project_gridMLC(data, width) {
    debugger;
    var pivotGrid_project = $("#project_dataGridMLC").dxPivotGrid({
        //allowSortingBySummary: true,
        allowSorting: false,
        allowFiltering: true,
        showBorders: true,
        showColumnGrandTotals: false,
        showRowGrandTotals: false,
        showRowTotals: false,
        showColumnTotals: false,
        height: 500,
        //width: 1650,
        width: width,
        columnWidth: 100,
        headerFilter: { visible: true },
        fieldPanel: {
            //showColumnFields: true,
            //showDataFields: true,
            showRowFields: true,
            visible: true
        },
        fieldChooser: {
            enabled: false,
            applyChangesMode: "instantly"
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
        //    pageSize: 40
        //},
        dataSource: {
            fields: [{
                //    caption: "Setup Type",
                //    width: 60,
                //    dataField: "SetupType",
                //    area: "row",
                //    expanded: true
                //}, {
                width: 60,
                dataField: "Location",
                area: "row",
                expanded: true
            }, {

                width: 60,
                dataField: "Project",
                area: "row",
                expanded: true

            }, {

                width: 500,
                dataField: "ComputerIds",
                caption: "TSGs",
                area: "row",
                expanded: true
            }, {

                width: 80,
                dataField: "Nos",
                caption: "NoofTSGs",
                area: "row",
                expanded: true
                //visible: false,



            },
            {
                dataField: "WeekNo",
                area: "column",
                sortBy: "none",
                //dataType: "date",
                //customizeText: function (options) {
                //    return !options.value ? "" : moment(options.value).format("MMM/DD/YYYY").replace(".", "");
                //},
                expanded: true

            }, {
                groupname: "WeekNo",
                visible: false,
                expanded: true
            }, {

                dataField: "Average",
                dataType: "number",
                summaryType: "sum",
                area: "data",
                visible: true,
                expanded: true,
            }],
            store: data
        },
        onCellPrepared: function (e) {
            debugger;
            if (e.area === "data" && (e.cell.columnPath[0] === "CW53" || e.cell.columnPath[0] === "CW52" || e.cell.columnPath[0] === "CW51" ||
                e.cell.columnPath[0] === "CW50" || e.cell.columnPath[0] === "CW49" || e.cell.columnPath[0] === "CW48" || e.cell.columnPath[0] === "CW47" || e.cell.columnPath[0] === "CW46" ||
                e.cell.columnPath[0] === "CW45" || e.cell.columnPath[0] === "CW44" || e.cell.columnPath[0] === "CW43" || e.cell.columnPath[0] === "CW42" || e.cell.columnPath[0] === "CW41" ||
                e.cell.columnPath[0] === "CW40" || e.cell.columnPath[0] === "CW39" || e.cell.columnPath[0] === "CW38" || e.cell.columnPath[0] === "CW37" || e.cell.columnPath[0] === "CW36" ||
                e.cell.columnPath[0] === "CW35" || e.cell.columnPath[0] === "CW34" || e.cell.columnPath[0] === "CW33" || e.cell.columnPath[0] === "CW32" || e.cell.columnPath[0] === "CW31" ||
                e.cell.columnPath[0] === "CW30" || e.cell.columnPath[0] === "CW29" || e.cell.columnPath[0] === "CW28" || e.cell.columnPath[0] === "CW27" || e.cell.columnPath[0] === "CW26" ||
                e.cell.columnPath[0] === "CW25" || e.cell.columnPath[0] === "CW24" || e.cell.columnPath[0] === "CW23" || e.cell.columnPath[0] === "CW22" || e.cell.columnPath[0] === "CW21" ||
                e.cell.columnPath[0] === "CW20" || e.cell.columnPath[0] === "CW19" || e.cell.columnPath[0] === "CW18" || e.cell.columnPath[0] === "CW17" || e.cell.columnPath[0] === "CW16" ||
                e.cell.columnPath[0] === "CW15" || e.cell.columnPath[0] === "CW14" || e.cell.columnPath[0] === "CW13" || e.cell.columnPath[0] === "CW12" || e.cell.columnPath[0] === "CW11" ||
                e.cell.columnPath[0] === "CW10" || e.cell.columnPath[0] === "CW09" || e.cell.columnPath[0] === "CW08" || e.cell.columnPath[0] === "CW07" || e.cell.columnPath[0] === "CW06" ||
                e.cell.columnPath[0] === "CW05" || e.cell.columnPath[0] === "CW04" || e.cell.columnPath[0] === "CW03" || e.cell.columnPath[0] === "CW02" || e.cell.columnPath[0] === "CW01")) { //&& e.column.dataField === "Value") {
                debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value >= 70) {
                    e.cellElement.css("background-color", "green");
                }
                else if (e.cell.value <= 28) {
                    e.cellElement.css("background-color", "red");
                }
                else if (e.cell.value > 28 && e.cell.value < 70) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                }
            }
        },
        onCellClick: function (e) {
            e.cancel = true;
        },

        onExporting: function (e) {
            debugger;
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('MLCAvg_Projectwise)');

            DevExpress.excelExporter.exportPivotGrid({
                //component: e.component,
                component: $("#project_dataGridMLC").dxPivotGrid("instance"),
                worksheet: worksheet,
                autoFilterEnabled: true,
                customizeCell: function (options) {
                    var excelCell = options.excelCell;
                    var pivotCell = options.pivotCell;

                    if (isDataCell(pivotCell)) {
                        //debugger;
                        var appearance = getConditionalAppearance(pivotCell);
                        Object.assign(excelCell, getExcelCellFormat(appearance));
                        //Object.assign(excelCell, appearance);
                    }

                    var borderStyle = { style: "thin", color: { argb: "FF7E7E7E" } };
                    excelCell.border = {
                        bottom: borderStyle,
                        left: borderStyle,
                        right: borderStyle,
                        top: borderStyle
                    };
                }

            })

                .then(function (dataGridRange) {
                    debugger;
                    var fields = pivotGrid_project.getDataSource().fields();
                    var rowFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var dataFields = fields.filter(r => r.area === 'data').map(r => `[${r.summaryType}(${r.dataField}])`);
                    var columnFields = [...new Set(fields.filter(r => r.area === 'column').map(r => r.dataField))];
                    var filterFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var appliedFilters = fields.filter(r => r.filterValues !== undefined).map(r => `[${r.dataField}:${r.filterValues}]`);
                    var firstRow = worksheet.getRow(1),
                        fieldPanelCell = firstRow.getCell(1);

                    firstRow.height = 90;
                    //worksheet.mergeCells('A1:D1');
                    fieldPanelCell.value = '\n Field Panel content:'
                        + ` \n - Filter fields: [${filterFields.join(', ')}]`
                        + ` \n - Row fields: [${rowFields.join(', ')}]`
                        + ` \n - Column fields: [${columnFields.join(', ')}]`
                        + ` \n - Data fields: [${dataFields.join(', ')}]`
                        + ` \n - Applied filters: [${appliedFilters.join(', ')}]`;

                    fieldPanelCell.alignment = { horizontal: 'left', vertical: 'top', wrapText: true };
                    fieldPanelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'D8D8D8' } };

                    return Promise.resolve();
                })


                .then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'MLCAvg_Projectwise.xlsx');
                    });
                });
            e.cancel = true;
        }
    }).dxPivotGrid("instance");

    function isDataCell(cell) {
        return (cell.area === "data" && (cell.columnPath[0] === "CW53" || cell.columnPath[0] === "CW52" || cell.columnPath[0] === "CW51" ||
            cell.columnPath[0] === "CW50" || cell.columnPath[0] === "CW49" || cell.columnPath[0] === "CW48" || cell.columnPath[0] === "CW47" || cell.columnPath[0] === "CW46" ||
            cell.columnPath[0] === "CW45" || cell.columnPath[0] === "CW44" || cell.columnPath[0] === "CW43" || cell.columnPath[0] === "CW42" || cell.columnPath[0] === "CW41" ||
            cell.columnPath[0] === "CW40" || cell.columnPath[0] === "CW39" || cell.columnPath[0] === "CW38" || cell.columnPath[0] === "CW37" || cell.columnPath[0] === "CW36" ||
            cell.columnPath[0] === "CW35" || cell.columnPath[0] === "CW34" || cell.columnPath[0] === "CW33" || cell.columnPath[0] === "CW32" || cell.columnPath[0] === "CW31" ||
            cell.columnPath[0] === "CW30" || cell.columnPath[0] === "CW29" || cell.columnPath[0] === "CW28" || cell.columnPath[0] === "CW27" || cell.columnPath[0] === "CW26" ||
            cell.columnPath[0] === "CW25" || cell.columnPath[0] === "CW24" || cell.columnPath[0] === "CW23" || cell.columnPath[0] === "CW22" || cell.columnPath[0] === "CW21" ||
            cell.columnPath[0] === "CW20" || cell.columnPath[0] === "CW19" || cell.columnPath[0] === "CW18" || cell.columnPath[0] === "CW17" || cell.columnPath[0] === "CW16" ||
            cell.columnPath[0] === "CW15" || cell.columnPath[0] === "CW14" || cell.columnPath[0] === "CW13" || cell.columnPath[0] === "CW12" || cell.columnPath[0] === "CW11" ||
            cell.columnPath[0] === "CW10" || cell.columnPath[0] === "CW09" || cell.columnPath[0] === "CW08" || cell.columnPath[0] === "CW07" || cell.columnPath[0] === "CW06" ||
            cell.columnPath[0] === "CW05" || cell.columnPath[0] === "CW04" || cell.columnPath[0] === "CW03" || cell.columnPath[0] === "CW02" || cell.columnPath[0] === "CW01"));
    }

    function getExcelCellFormat(appearance) {
        return {
            fill: { type: "pattern", pattern: "solid", fgColor: { argb: appearance.fill } },
            font: { color: { argb: appearance.font }, bold: appearance.bold }
        };
    }

    function getConditionalAppearance(cell) {
        //debugger;
        if (cell.value === "") {
            cell.value = 0;
        }

        if (cell.value > 70) {
            //cell.cellElement.css("background-color", "green");
            return { font: "000000", fill: "008000" };
        }
        else if (cell.value < 28) {
            //cell.cellElement.css("background-color", "red");
            return { font: "000000", fill: "FF0000" };
        }
        else if (cell.value > 28 && cell.value < 70) {
            //cell.cellElement.css("background-color", "yellow");
            return { font: "000000", fill: "FFFF00" };
        }
        else {
            //cell.cellElement.css("background-color", "white");
            return { font: "000000", fill: "FFFFFF" };
        }
    }
}

///////// ACUROT

function project_gridACUROT(data, width) {
    debugger;
    var pivotGrid_project = $("#project_dataGridACUROT").dxPivotGrid({
        //allowSortingBySummary: true,
        allowSorting: false,
        allowFiltering: true,
        showBorders: true,
        showColumnGrandTotals: false,
        showRowGrandTotals: false,
        showRowTotals: false,
        showColumnTotals: false,
        height: 500,
        //width: 1650,
        width: width,
        columnWidth: 100,
        headerFilter: { visible: true },
        fieldPanel: {
            //showColumnFields: true,
            //showDataFields: true,
            showRowFields: true,
            visible: true
        },
        fieldChooser: {
            enabled: false,
            applyChangesMode: "instantly"
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
        //    pageSize: 40
        //},
        dataSource: {
            fields: [{
                //    caption: "Setup Type",
                //    width: 60,
                //    dataField: "SetupType",
                //    area: "row",
                //    expanded: true
                //}, {
                width: 60,
                dataField: "Location",
                area: "row",
                expanded: true
            }, {

                width: 60,
                dataField: "Project",
                area: "row",
                expanded: true

            }, {

                width: 500,
                dataField: "ComputerIds",
                caption: "TSGs",
                area: "row",
                expanded: true
            }, {

                width: 80,
                dataField: "Nos",
                caption: "NoofTSGs",
                area: "row",
                expanded: true
                //visible: false,



            },
            {
                dataField: "WeekNo",
                area: "column",
                sortBy: "none",
                //dataType: "date",
                //customizeText: function (options) {
                //    return !options.value ? "" : moment(options.value).format("MMM/DD/YYYY").replace(".", "");
                //},
                expanded: true

            }, {
                groupname: "WeekNo",
                visible: false,
                expanded: true
            }, {

                dataField: "Average",
                dataType: "number",
                summaryType: "sum",
                area: "data",
                visible: true,
                expanded: true,
            }],
            store: data
        },
        onCellPrepared: function (e) {
            debugger;
            if (e.area === "data" && (e.cell.columnPath[0] === "CW53" || e.cell.columnPath[0] === "CW52" || e.cell.columnPath[0] === "CW51" ||
                e.cell.columnPath[0] === "CW50" || e.cell.columnPath[0] === "CW49" || e.cell.columnPath[0] === "CW48" || e.cell.columnPath[0] === "CW47" || e.cell.columnPath[0] === "CW46" ||
                e.cell.columnPath[0] === "CW45" || e.cell.columnPath[0] === "CW44" || e.cell.columnPath[0] === "CW43" || e.cell.columnPath[0] === "CW42" || e.cell.columnPath[0] === "CW41" ||
                e.cell.columnPath[0] === "CW40" || e.cell.columnPath[0] === "CW39" || e.cell.columnPath[0] === "CW38" || e.cell.columnPath[0] === "CW37" || e.cell.columnPath[0] === "CW36" ||
                e.cell.columnPath[0] === "CW35" || e.cell.columnPath[0] === "CW34" || e.cell.columnPath[0] === "CW33" || e.cell.columnPath[0] === "CW32" || e.cell.columnPath[0] === "CW31" ||
                e.cell.columnPath[0] === "CW30" || e.cell.columnPath[0] === "CW29" || e.cell.columnPath[0] === "CW28" || e.cell.columnPath[0] === "CW27" || e.cell.columnPath[0] === "CW26" ||
                e.cell.columnPath[0] === "CW25" || e.cell.columnPath[0] === "CW24" || e.cell.columnPath[0] === "CW23" || e.cell.columnPath[0] === "CW22" || e.cell.columnPath[0] === "CW21" ||
                e.cell.columnPath[0] === "CW20" || e.cell.columnPath[0] === "CW19" || e.cell.columnPath[0] === "CW18" || e.cell.columnPath[0] === "CW17" || e.cell.columnPath[0] === "CW16" ||
                e.cell.columnPath[0] === "CW15" || e.cell.columnPath[0] === "CW14" || e.cell.columnPath[0] === "CW13" || e.cell.columnPath[0] === "CW12" || e.cell.columnPath[0] === "CW11" ||
                e.cell.columnPath[0] === "CW10" || e.cell.columnPath[0] === "CW09" || e.cell.columnPath[0] === "CW08" || e.cell.columnPath[0] === "CW07" || e.cell.columnPath[0] === "CW06" ||
                e.cell.columnPath[0] === "CW05" || e.cell.columnPath[0] === "CW04" || e.cell.columnPath[0] === "CW03" || e.cell.columnPath[0] === "CW02" || e.cell.columnPath[0] === "CW01")) { //&& e.column.dataField === "Value") {
                debugger;
                //e.cellElement.css("background-color", e.cell.value > 2 ? "green" : "red");
                if (e.cell.value === "") {
                    e.cell.value = 0;
                }

                if (e.cell.value >= 70) {
                    e.cellElement.css("background-color", "green");
                }
                else if (e.cell.value <= 28) {
                    e.cellElement.css("background-color", "red");
                }
                else if (e.cell.value > 28 && e.cell.value < 70) {
                    e.cellElement.css("background-color", "yellow");
                }
                else {
                    e.cellElement.css("background-color", "white");
                }
            }
        },
        onCellClick: function (e) {
            e.cancel = true;
        },

        onExporting: function (e) {
            debugger;
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('ACUROTAvg_Projectwise)');

            DevExpress.excelExporter.exportPivotGrid({
                //component: e.component,
                component: $("#project_dataGridACUROT").dxPivotGrid("instance"),
                worksheet: worksheet,
                autoFilterEnabled: true,
                customizeCell: function (options) {
                    var excelCell = options.excelCell;
                    var pivotCell = options.pivotCell;

                    if (isDataCell(pivotCell)) {
                        //debugger;
                        var appearance = getConditionalAppearance(pivotCell);
                        Object.assign(excelCell, getExcelCellFormat(appearance));
                        //Object.assign(excelCell, appearance);
                    }

                    var borderStyle = { style: "thin", color: { argb: "FF7E7E7E" } };
                    excelCell.border = {
                        bottom: borderStyle,
                        left: borderStyle,
                        right: borderStyle,
                        top: borderStyle
                    };
                }

            })

                .then(function (dataGridRange) {
                    debugger;
                    var fields = pivotGrid_project.getDataSource().fields();
                    var rowFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var dataFields = fields.filter(r => r.area === 'data').map(r => `[${r.summaryType}(${r.dataField}])`);
                    var columnFields = [...new Set(fields.filter(r => r.area === 'column').map(r => r.dataField))];
                    var filterFields = fields.filter(r => r.area === 'row').map(r => r.dataField);
                    var appliedFilters = fields.filter(r => r.filterValues !== undefined).map(r => `[${r.dataField}:${r.filterValues}]`);
                    var firstRow = worksheet.getRow(1),
                        fieldPanelCell = firstRow.getCell(1);

                    firstRow.height = 90;
                    //worksheet.mergeCells('A1:D1');
                    fieldPanelCell.value = '\n Field Panel content:'
                        + ` \n - Filter fields: [${filterFields.join(', ')}]`
                        + ` \n - Row fields: [${rowFields.join(', ')}]`
                        + ` \n - Column fields: [${columnFields.join(', ')}]`
                        + ` \n - Data fields: [${dataFields.join(', ')}]`
                        + ` \n - Applied filters: [${appliedFilters.join(', ')}]`;

                    fieldPanelCell.alignment = { horizontal: 'left', vertical: 'top', wrapText: true };
                    fieldPanelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'D8D8D8' } };

                    return Promise.resolve();
                })


                .then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'ACUROTAvg_Projectwise.xlsx');
                    });
                });
            e.cancel = true;
        }
    }).dxPivotGrid("instance");

    function isDataCell(cell) {
        return (cell.area === "data" && (cell.columnPath[0] === "CW53" || cell.columnPath[0] === "CW52" || cell.columnPath[0] === "CW51" ||
            cell.columnPath[0] === "CW50" || cell.columnPath[0] === "CW49" || cell.columnPath[0] === "CW48" || cell.columnPath[0] === "CW47" || cell.columnPath[0] === "CW46" ||
            cell.columnPath[0] === "CW45" || cell.columnPath[0] === "CW44" || cell.columnPath[0] === "CW43" || cell.columnPath[0] === "CW42" || cell.columnPath[0] === "CW41" ||
            cell.columnPath[0] === "CW40" || cell.columnPath[0] === "CW39" || cell.columnPath[0] === "CW38" || cell.columnPath[0] === "CW37" || cell.columnPath[0] === "CW36" ||
            cell.columnPath[0] === "CW35" || cell.columnPath[0] === "CW34" || cell.columnPath[0] === "CW33" || cell.columnPath[0] === "CW32" || cell.columnPath[0] === "CW31" ||
            cell.columnPath[0] === "CW30" || cell.columnPath[0] === "CW29" || cell.columnPath[0] === "CW28" || cell.columnPath[0] === "CW27" || cell.columnPath[0] === "CW26" ||
            cell.columnPath[0] === "CW25" || cell.columnPath[0] === "CW24" || cell.columnPath[0] === "CW23" || cell.columnPath[0] === "CW22" || cell.columnPath[0] === "CW21" ||
            cell.columnPath[0] === "CW20" || cell.columnPath[0] === "CW19" || cell.columnPath[0] === "CW18" || cell.columnPath[0] === "CW17" || cell.columnPath[0] === "CW16" ||
            cell.columnPath[0] === "CW15" || cell.columnPath[0] === "CW14" || cell.columnPath[0] === "CW13" || cell.columnPath[0] === "CW12" || cell.columnPath[0] === "CW11" ||
            cell.columnPath[0] === "CW10" || cell.columnPath[0] === "CW09" || cell.columnPath[0] === "CW08" || cell.columnPath[0] === "CW07" || cell.columnPath[0] === "CW06" ||
            cell.columnPath[0] === "CW05" || cell.columnPath[0] === "CW04" || cell.columnPath[0] === "CW03" || cell.columnPath[0] === "CW02" || cell.columnPath[0] === "CW01"));
    }

    function getExcelCellFormat(appearance) {
        return {
            fill: { type: "pattern", pattern: "solid", fgColor: { argb: appearance.fill } },
            font: { color: { argb: appearance.font }, bold: appearance.bold }
        };
    }

    function getConditionalAppearance(cell) {
        //debugger;
        if (cell.value === "") {
            cell.value = 0;
        }

        if (cell.value >= 70) {
            //cell.cellElement.css("background-color", "green");
            return { font: "000000", fill: "008000" };
        }
        else if (cell.value <= 28) {
            //cell.cellElement.css("background-color", "red");
            return { font: "000000", fill: "FF0000" };
        }
        else if (cell.value > 28 && cell.value < 70) {
            //cell.cellElement.css("background-color", "yellow");
            return { font: "000000", fill: "FFFF00" };
        }
        else {
            //cell.cellElement.css("background-color", "white");
            return { font: "000000", fill: "FFFFFF" };
        }
    }
}