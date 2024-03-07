
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
                    fieldPanelCell.value = 'Field Panel content:'
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
            }, {
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
            if (e.area === "data" && e.cell.columnPath[1] === "Total") { //&& e.column.dataField === "Value") {
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
                    fieldPanelCell.value = 'Field Panel content:'
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
                    fieldPanelCell.value = 'Field Panel content:'
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