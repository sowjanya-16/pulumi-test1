var selectedTC;
$(function () {
    debugger;

    

    var ddlYearsTC = document.getElementById("ddlYearsTC");
    var currentYear = (new Date()).getFullYear();
    for (var i = 2021; i <= currentYear; i++) {
        var option = document.createElement("OPTION");
        option.innerHTML = i;
        option.value = i;
        ddlYearsTC.appendChild(option);
        // if (option.value == currentYear || option.value == (currentYear + 1) || option.value == (currentYear - 2)) {
        if (option.value == currentYear) {
            option.defaultSelected = true;
        }
        debugger;
        selectedTC = $("#ddlYearsTC").val();
        //}
    }


    //$("#ddlYearsTC").datepicker({
    //    format: "yyyy",
    //    viewMode: "years",
    //    minViewMode: "years",
    //    autoclose: true

    //}); 
});
function fnYearTCChange() {
    debugger;
    selectedTC = $("#ddlYearsTC").val();
    GenerateTCData();
}
function GenerateTCData() {
    debugger;
    

    $('#cards').css("display", "block");
    $('#tc_grid').css("display", "none");
    $('#tc_grid_title').css("display", "none");
    
    $.ajax({
        type: "POST",
        
        url: "../SLCockpit/TCData",
        data: { 'Year': selectedTC },
        dataType: 'json',
        //traditional: true,
        success: function (data) {
            debugger;

            var tcdata = JSON.parse(data.data.Data.Content);
            if (tcdata[0] == undefined) {
                document.getElementById('TC_BudgetPlan').innerHTML = "NA";
                document.getElementById('TC_BudgetAvailable').innerHTML = "NA";
                document.getElementById('TC_BudgetUsed').innerHTML = "NA";

            }
            else if (tcdata[0].Budget_Plan == "$0"){
                document.getElementById('TC_BudgetPlan').innerHTML = "NA";
                document.getElementById('TC_BudgetAvailable').innerHTML = tcdata[0].Available;
                document.getElementById('TC_BudgetUsed').innerHTML = tcdata[0].Used;
            }
            else if (tcdata[0].Available == "$0") {
                document.getElementById('TC_BudgetAvailable').innerHTML = "NA";
                document.getElementById('TC_BudgetPlan').innerHTML = tcdata[0].Budget_Plan;
                document.getElementById('TC_BudgetUsed').innerHTML = tcdata[0].Used;
            }
            else if (tcdata[0].Used == "$0") {
                document.getElementById('TC_BudgetUsed').innerHTML = "NA";
                document.getElementById('TC_BudgetPlan').innerHTML = tcdata[0].Budget_Plan;
                document.getElementById('TC_BudgetAvailable').innerHTML = tcdata[0].Available;
            }
            else {
                document.getElementById('TC_BudgetPlan').innerHTML =  tcdata[0].Budget_Plan;
                document.getElementById('TC_BudgetAvailable').innerHTML = tcdata[0].Available;
                document.getElementById('TC_BudgetUsed').innerHTML = tcdata[0].Used;
            }
          
           
        }
        ,
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


function GenerateTCGrid() {
    debugger;
    

    

    $.ajax({
        type: "POST",
        url: "../HC_TC/GenerateTC",
        dataType: 'json',
        data: { 'Year': selectedTC},
        success: function (data) {
            debugger;
            $('#cards').css("display", "none");
            $('#tc_grid').css("display", "block");
            $('#hc_grid_title').css("display", "none");
           
            $('#tc_grid_title').css("display", "block");
          
            var res = JSON.parse(data.data.Data.Content);
            var dataGrid_tc = $("#tc_grid").dxDataGrid({

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
                    //debugger;
                    e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
                },
                //noDataText: encodeMessage(noDataTextUnsafe),
                //showRequiredMark: true,
                //RequiredMark: '*',
                //columnResizingMode: "widget",
                //columnMinWidth: 100,
                onCellPrepared: function (e) {
                    //e.cellElement.css('color', '#0000');
               
                    //if (e.rowType === "header" || e.rowType === "filter") {
                    //    e.cellElement.css('font-weight', 'bold');
                    
                    //    //e.cellElement.addClass("columnHeaderCSS");
                    //    //e.cellElement.find("input").addClass("columnHeaderCSS");
                    //}
                },
                noDataText: " ☺ No Item ! click '+' Add a row option on the top right",

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
                width:1000,

                onEditorPreparing: function (e) {
                    var component = e.component,
                        rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

                },

                columns: [
                    {

                        alignment: "center",
                        columns: [

                            {
                                dataField: "Cmmt_Item",
                                //caption: "Dept",
                                validationRules: [{ type: "required" }],

                            },
                            {
                                dataField: "Year",
                                //caption: "Dept",
                                validationRules: [{ type: "required" }],
                               
                            },


                            {
                                dataField: 'Budget_Plan',
                                width: 150,
                            },
                            'Invoice',
                            {
                                dataField: "Bud-Inv",
                               
                                calculateCellValue: function (rowData) {
                                    //update the bud-inv
                                    var remain_budget;
                                    if (rowData.Budget_Plan == undefined || rowData.Budget_Plan == null || rowData.Budget_Plan == 0) {

                                        remain_budget = 0;
                                    }
                                    else if (rowData.Invoice == undefined || rowData.Invoice == null || rowData.Invoice == 0) {

                                        remain_budget = rowData.Budget_Plan;
                                    }
                                    else {

                                        remain_budget = rowData.Budget_Plan - rowData.Invoice;

                                    }

                                    return remain_budget;
                                }

                            },
                            'Open',
                            {
                                dataField: "Available",
                                
                                calculateCellValue: function (rowData) {
                                    //update the bud-inv
                                    var remain_budget;
                                    if (rowData.Budget_Plan == undefined || rowData.Budget_Plan == null || rowData.Budget_Plan == 0) {

                                        remain_budget = 0;
                                    }
                                    else if (rowData.Invoice != null && rowData.Invoice != undefined && rowData.Open != null && rowData.Open != undefined) {

                                        remain_budget = rowData.Budget_Plan - (rowData.Invoice + rowData.Open);
                                    }
                                    else if (rowData.Invoice == null && rowData.Invoice == undefined) {

                                        remain_budget = rowData.Budget_Plan - rowData.Open;

                                    }
                                    else
                                        remain_budget = rowData.Budget_Plan - rowData.Invoice;

                                    return remain_budget;
                                }

                            },

                        ]
                    }],


            });

           
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

    $('#backButton').dxButton({
        text: 'Back',
        icon: 'chevronleft',
        visible: true, //false,
        onClick() {
            debugger;

            //$('#cards').css("display", "block");
            //$('#tc_grid').css("display", "none");
            //this.option('visible', false);

            document.getElementById("cards").style.display = "block";
            document.getElementById("DeliveryStatus").style.display = "none";
            document.getElementById("chartdiv_infra").style.display = "none"; 
            document.getElementById("hwDamage_oemchart").style.display = "none";
            document.getElementById("hc_grid").style.display = "none";
            document.getElementById("tc_grid").style.display = "none";
            document.getElementById("tc_grid_title").style.display = "none";
            document.getElementById("breadcrumb_nav1").style.display = "block";
            document.getElementById("breadcrumb_nav2").style.display = "none";
            document.getElementById("twoyr_view").style.display = "block";
            document.getElementById("costelement_view").style.display = "none";
            document.getElementById("category_view").style.display = "none";
            document.getElementById("item_view").style.display = "none";
            document.getElementById("threeyr_view").style.display = "none";
            var bd1 = document.getElementById("item_nav");
            bd1.style.color = "grey";
            var bd2 = document.getElementById("costelement_nav");
            bd2.style.color = "grey";
            var bd3 = document.getElementById("category_nav");
            bd3.style.color = "grey";
            var bd4 = document.getElementById("twoyr_nav");
            bd4.style.color = "black";
            var bd5 = document.getElementById("threeyr_nav");
            bd5.style.color = "grey";

            this.option('visible', false);

        },
    });
   
}


