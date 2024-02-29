var dataObjData;
var dataObj;

var selected = [];
var selected1;
var selected2;
var BU_list, Item_list, Category_list, OEM_list, Group_list, Group_list_2020, CostElement_list;
var dataGridCockpit;
var costelement_selected;
var categoryid_selected;
var ItemID;
var BUList;
var selectedthree = [];
var selectedthree1, selectedthree2, selectedthree3;
var selected_ccxc;
var isthreeyr_flag;
var current_tab; //store curent tab(if 3 yr tab open -> View changed -> both 2yr & 3 yr tab data ll be updated but success notifiction should be shown only for active tab's content updtion)
var genSpinner_vkm = document.querySelector("#load_vkm");
var genSpinner_vkm_chart = document.querySelector("#load_vkm_chart");
var entry_counter = 0;
var isthreeyr_datafetched; //if three yr data fetched = true => show the chart on switching to threeyr_nav ; else not show chart since still loading phase

function GenerateCostEltSummary() {
    
    //window.onload = function () {
    //    $.notify('Comparison Table and Chart will be rendered here. Please wait !!', {
    //        globalPosition: "top center",
    //        className: "info",
    //        autoHideDelay: 20000,
    //    });
    //}
    ////debugger;
    $(document).ready(function () {

       // //debugger;
        $('#load_vkm').prop('hidden', false);
        $('#load_vkm_chart').prop('hidden', false);
        //<i id="load_vkm" style="font-size:8vh;margin-top:80px;margin-left:29vh;"></i>
        //document.getElementById("infra_card").style.width = "25%";
        //document.getElementById("load_vkm").style.marginLeft = "109vh";
        genSpinner_vkm.classList.add('fa');
        genSpinner_vkm.classList.add('fa-spinner');
        genSpinner_vkm.classList.add('fa-pulse');

        genSpinner_vkm_chart.classList.add('fa');
        genSpinner_vkm_chart.classList.add('fa-spinner');
        genSpinner_vkm_chart.classList.add('fa-pulse');


        $.ajax({

            type: "GET",
            url: "/Cockpit/Lookup",
            success: onsuccess_lookupdata,
            error: onerror_lookupdata
        });


        function onsuccess_lookupdata(response) {
            ////debugger;
            lookup_data = response.data;
            BU_list = lookup_data.BU_List;
            OEM_list = lookup_data.OEM_List;
            DEPT_list = lookup_data.DEPT_List;
            Group_list = lookup_data.Groups_test;
            Item_list = lookup_data.Item_List;
            Category_list = lookup_data.Category_List;
            CostElement_list = lookup_data.CostElement_List;
        }

        function onerror_lookupdata(response) {
            alert("Error in fetching lookup");

        }

       // //debugger;
        var ddlYears1 = document.getElementById("ddlYears1");
        var ddlYears2 = document.getElementById("ddlYears2");
        var currentYear = (new Date()).getFullYear();
        for (var i = 2021 ; i <= currentYear ; i++) {
            var option = document.createElement("OPTION");
            option.innerHTML = i;
            option.value = i;
            ddlYears1.appendChild(option);
            //month <= june => Year1 = CY - 1
            //else             Year1 = CY 
            if (new Date().getMonth() + 1 <= 6) {
                //PY
                if (option.value == currentYear - 1) {
                    option.defaultSelected = true;
                }
            }
            else {//CY
                if (option.value == currentYear) {
                    option.defaultSelected = true;
                }
            }
            //if (option.value == currentYear) {
            //    option.defaultSelected = true;
            //}
           // //debugger;
            selected1 = $("#ddlYears1").val();
            //}
        }

        for (var i = currentYear; i <= currentYear + 1; i++) {
            debugger;
           
                var option = document.createElement("OPTION");
                option.innerHTML = i;
            option.value = i;
           
            if (new Date().getMonth() + 1 <= 6 && option.value == currentYear)
                ddlYears2.appendChild(option);
            else if (option.value != currentYear)
                ddlYears2.appendChild(option);
            //month <= june => Year2 = CY
            //else             Year2 = CY + 1
                if (new Date().getMonth() + 1 <= 6) {
           
                    //CY
                    if (option.value == currentYear ) {
                        option.defaultSelected = true;
                    }
                }
                else {//NY
                    if (option.value == currentYear + 1) {
                        option.defaultSelected = true;
                    }
                }

                ////if (option.value == currentYear || option.value == (currentYear + 1) || option.value == (currentYear - 2)) {
                ///* july , aug , sep - NY
                //* others - PY
                //*/
                //if (new Date().getMonth() + 1 == 7 || new Date().getMonth() + 1 == 8 || new Date().getMonth() + 1 == 9) {
                //    //NY
                //    if (option.value == currentYear + 1) {
                //        option.defaultSelected = true;
                //    }
                //}
                //else {//PY
                //    if (option.value == currentYear - 1) {
                //        option.defaultSelected = true;
                //    }
                //}
           
            selected2 = $("#ddlYears2").val();
            $(".selectpicker").selectpicker('refresh');

            //}
            
           
        }
        debugger;
        //three yr view
        var ddlYearsthree_1 = document.getElementById("ddlYearsthree_1");
        var ddlYearsthree_2 = document.getElementById("ddlYearsthree_2");
        var ddlYearsthree_3 = document.getElementById("ddlYearsthree_3");
        var currentYear = (new Date()).getFullYear();
        ////debugger;
        for (var i = 2021; i <= currentYear + 1; i++) {
            var option = document.createElement("OPTION");
            option.innerHTML = i;
            option.value = i;
            ddlYearsthree_1.appendChild(option);
            if (option.value == currentYear - 1) {
                option.defaultSelected = true;
            }
            selectedthree1 = $("#ddlYearsthree_1").val();
        }
        for (var i = 2021; i <= currentYear + 1; i++) {
            var option = document.createElement("OPTION");
            option.innerHTML = i;
            option.value = i;
            ddlYearsthree_2.appendChild(option);
            if (option.value == currentYear) {
                option.defaultSelected = true;
                
            }
            selectedthree2 = $("#ddlYearsthree_2").val();
           // //debugger;
            
        }
        for (var i = 2021; i <= currentYear + 1; i++) {
            var option = document.createElement("OPTION");
            option.innerHTML = i;
            option.value = i;
            ddlYearsthree_3.appendChild(option);
            if (option.value == currentYear + 1) {
                option.defaultSelected = true;
            }
            selectedthree3 = $("#ddlYearsthree_3").val();
        }
        selected.push(selected1);
        selected.push(selected2);
       // //debugger;
        selectedthree.push(selectedthree1);
        selectedthree.push(selectedthree2);
        selectedthree.push(selectedthree3);
        selected_ccxc = $("#ddlCC_XC").val();


        $(".selectpicker").selectpicker('refresh');
       
        get_bus();
        $('#gen').click();
        //isthreeyr_flag = true;
        $.ajax({
            type: "POST",
            url: "/Cockpit/VKMSummaryData/",
            data: { 'years': selectedthree, 'chart': true, 'buList': BUList, 'selected_ccxc': selected_ccxc },
            success: OnSuccess_ChartSummaryData,
            error: OnError_ChartSummaryData
        });

    

    });

    //var onchangefunctions = function () {
    //    return {
    //        init: function () {
    //            fnYear1Change();
    //            fnYear2Change();
    //            fnYearthree1Change();
    //            fnYearthree2Change();
    //            fnYearthree3Change();
    //            fnCCXCChange();
    //        }
    //    };}();
    //jQuery(document).ready(function () {
    //    onchangefunctions.init();
    //});

   
    $(function () {
        var create = document.querySelector('.create');
        var genText = document.querySelector("#genText");
        var genSpinner = document.querySelector("#genSpinner");
        create.addEventListener("click", function () {

            var str = "Please Wait, Comparing...  ";
            var result = str.bold();
            genText.innerHTML = result;

            genSpinner.classList.add('fa');
            genSpinner.classList.add('fa-spinner');
            genSpinner.classList.add('fa-pulse');

        });

    });

    $("#gen").click(function () {
       debugger;
        entry_counter++;
        $('#load_vkm').prop('hidden', false);
        $('#load_vkm_chart').prop('hidden', false);
        if (entry_counter > 1) {
            document.getElementById("load_vkm").style.marginLeft = "90vh";
            //document.getElementById("load_vkm_chart").style.marginLeft = "70vh";
        }
        else {
            document.getElementById("load_vkm").style.marginLeft = "10vh";
        }
           
        genSpinner_vkm.classList.add('fa');
        genSpinner_vkm.classList.add('fa-spinner');
        genSpinner_vkm.classList.add('fa-pulse');

        genSpinner_vkm_chart.classList.add('fa');
        genSpinner_vkm_chart.classList.add('fa-spinner');
        genSpinner_vkm_chart.classList.add('fa-pulse');

        vkmsummary_table();
        //vkmsummary_chart();
        ////debugger;




    })




    function vkmsummary_table() {
        debugger;
        store_forquarterly(selected, BUList, selected_ccxc);
        debugger;
        $.ajax({
            type: "POST",
            url: "/Cockpit/VKMSummaryData/",
            data: { 'years': selected, 'buList': BUList, 'selected_ccxc': selected_ccxc },
            //async: false,
            success: OnSuccess_VKMSummaryData,
            error: OnError_VKMSummaryData
        });

    }
    
    


    function OnSuccess_VKMSummaryData(data) {
        debugger;
        var quarterly = [];
        // dataObjData = eval(data.data);
        dataObjData = eval(data.data.data);
       // dataObjCol = eval(data.data.columns);

        ////debugger;
        if (data.message != "") {
           // //debugger;
            document.getElementById("Overall_bud_title").innerHTML = "VKM " + selected1 + " NE-CC Budget Summary";
            document.getElementById("Total_Plan").innerHTML =  data.PlanTotal;
            document.getElementById("Total_Utilize").innerHTML =  data.UtilTotal;
            document.getElementById("Overall_bud_title").style.fontWeight = "600";

            //var vkm_overall = data.vkm_overall.overall;
            //$('#tooltiptext_plan').dxPivotGrid({
            //    allowSortingBySummary: true,
            //    allowFiltering: true,
            //   showBorders: true,
            //   showColumnGrandTotals: false,
            //   showRowGrandTotals: true,
            //   showRowTotals: true,
            //   showColumnTotals: true,
            //    fieldChooser: {
            //        enabled: true,
            //        height: 400,
            //    },
            //    dataSource: {
            //      fields: [{
                    
            //        width: 120,
            //          dataField: 'VKM_elt',
            //        area: 'row',
            //        },

            //            {
            //                dataField: 'VKM_yr' ,//new Date(parseInt(data.data[0].RequestDate.substr(6))),
            //       // dataType: 'date',
            //                area: 'column',
            //            },

            //        {
                    
            //        dataField: 'Plan',
            //        dataType: 'number',
            //        summaryType: 'sum',
            //        format: 'currency',
            //        area: 'data',
            //            },
            //            {
                            
            //                dataField: 'Used',
            //                dataType: 'number',
            //                summaryType: 'sum',
            //                format: 'currency',
            //                area: 'data',
            //            },

            //        ],
            //        store: vkm_overall,
            //    },
            //}).dxPivotGrid('instance');      

           // for (i = 0; i < dataObjData.length; i++) { // to split the comma sep Roles into list and store as datasource - ince tgbox only accepts if array

           //     //grmemarray.push({ Project: res[i].Project_Team.substring(1, res[i].Project_Team.length - 1)  });
           //     quarterly.push({
           //         VKMYr: parseInt(new Date(parseInt(dataObjData[i].RequestDate.substr(6))).toISOString().slice(0, 4)) + 1, //"/Date(timestamp)/" to new Date("yyyy-MM-dd") format
                    
           //         CostElement: dataObjData[i].CostElement == "1" ? "MAE" : (dataObjData[i].CostElement == "2" ? "Non-MAE" : (dataObjData[i].CostElement == "3" ? "Software" : "")),
           //         ApprCost: Math.round(dataObjData[i].ApprCost),
           //         OrderPrice: Math.round(dataObjData[i].OrderPrice),
           //         OrderDate: dataObjData[i].OrderDate != null ? new Date(new Date(parseInt(dataObjData[i].OrderDate.substr(6))).toISOString().slice(0, 10)) : "", 

                   
           //     });

           // }
                  
           //vkmsummary =  $('#VKMSummaryTable').dxPivotGrid({
           //     allowSortingBySummary: true,
           //     allowFiltering: true,
           //    showBorders: true,
           //    showColumnGrandTotals: false,
           //    showRowGrandTotals: true,
           //    showRowTotals: true,
           //    showColumnTotals: true,
           //     fieldChooser: {
           //         enabled: true,
           //         height: 400,
           //     },
           //     dataSource: {
           //       fields: [{
           //         caption: 'CostElement',
           //         width: 120,
           //         dataField: 'CostElement',
           //         area: 'row',
           //         //sortBySummaryField: 'Total',
           //         },


           //             {
           //                 dataField: 'OrderDate',//new Date(parseInt(data.data[0].RequestDate.substr(6))),
           //                 dataType: 'date',
           //                 area: 'row',
           //                 caption: "Utilize group"
           //             },


           //         //{
           //         //caption: 'City',
           //         //dataField: 'city',
           //         //width: 150,
           //         //area: 'row',
           //         //},
           //             {
           //                 dataField: 'VKMYr' ,//new Date(parseInt(data.data[0].RequestDate.substr(6))),
           //        // dataType: 'date',
           //                 area: 'column',
           //         caption : "P"
           //             },
                       
           //         {
           //         caption: 'Plan',
           //         dataField: 'ApprCost',
           //         dataType: 'number',
           //         summaryType: 'sum',
           //         format: 'currency',
           //         area: 'data',
           //             },
                      
           //             {
           //                 groupName: 'RequestDate',//new Date(parseInt(data.data[0].RequestDate.substr(6))).toISOString().slice(0,10),// new Date('RequestDate'),
           //                 groupInterval: 'quarter',
           //                 visible: false,
           //             },
           //             {
           //                 groupName: 'VKMYr',//new Date(parseInt(data.data[0].RequestDate.substr(6))).toISOString().slice(0,10),// new Date('RequestDate'),
           //                 //groupInterval: 'month',
           //                // visible: false,
           //             },



           //            // {
           //            //     dataField: 'ActualDeliveryDate',//new Date(parseInt(data.data[0].RequestDate.substr(6))),
           //            //     dataType: 'date',
           //            //     area: 'column',
           //            //     caption: "U"
           //          //   },
                
           //             {
           //                 caption: 'Utilized',
           //                 dataField: 'OrderPrice',
           //                 dataType: 'number',
           //                 summaryType: 'sum',
           //                 format: 'currency',
           //                 area: 'data',
           //             },

           //            // {
           //            //     groupName: 'ActualDeliveryDate',//new Date(parseInt(data.data[0].RequestDate.substr(6))).toISOString().slice(0,10),// new Date('RequestDate'),
           //            //     groupInterval: 'quarter',
           //            //     visible: true,//false,
           //            // },

           //         ],
           //         store: quarterly,
           //     },
           // }).dxPivotGrid('instance');      




            vkmsummary = $("#VKMSummaryTable").dxDataGrid({
                //onRowPrepared: function (e) {
                //    e.rowElement.css({ height: 5 });
                //} ,
                columnAutoWidth: true,
                columnMinWidth: 122,
               
                width: "96.8vw",
                height: "22.5em",
                scrolling: {
                    mode: "virtual",
                    rowRenderingMode: "virtual",
                    columnRenderingMode: "virtual"
                },
                dataSource: dataObjData,
                showColumnLines: true,
                showRowLines: true,
                // rowAlternationEnabled: true,
                sorting: {
                    mode: "none" // or "multiple" | "single"
                },
                // columns: dataObjCol,
                showBorders: true,
                //export: {
                //    enabled: true,
                //    fileName: "VKM Summary Details"
                //},
                remoteOperations: true,
                showColumnLines: true,
                showRowLines: true,
                // rowAlternationEnabled: true,
                allowColumnReordering: true,
                allowColumnResizing: true,
                columnChooser: {
                    enabled: true
                },
                //onContentReady: function (e) {
                //    var columnChooserView = e.component.getView("columnChooserView");
                //    if (!columnChooserView._popupContainer) {
                //        columnChooserView._initializePopupContainer();
                //        columnChooserView.render();
                //        columnChooserView._popupContainer.option("position", { of: e.element, my: "right top", at: "right top", offset: "0 50" });
                //    }
                //}  ,
                cssClass: "ColumnsAutomatic",
                loadPanel: {
                    enabled: true
                },
                hoverStateEnabled: {
                    enabled: true,

                },
               
                onCellPrepared: function (e) {
                   
                    if (e.rowType == "header" && e.column.caption != undefined && e.column.caption.includes("%") && e.column.caption.includes("🠕🠗")) {
                        if (e.column.dataField.includes("Plan")) {
                           
                            e.cellElement.mouseover(function (arg) {
                                e.cellElement.addClass('dx-datagrid-action');

                                tooltipInstance.option("contentTemplate", function (contentElement) {
                                    contentElement.html(`<div class='tooltipContent' style='outline-style: solid; color: black; outline-width:thin;'><div>Plan % Change (` + selected2 + `-` + selected1 + `)</div >`);
                                });

                                tooltipInstance.show(arg.target);

                            });

                            e.cellElement.mouseout(function (arg) {


                                tooltipInstance.hide();
                            });
                        }
                        else {


                            e.cellElement.mouseover(function (arg) {
                                e.cellElement.addClass('dx-datagrid-action');

                                tooltipInstance.option("contentTemplate", function (contentElement) {
                                    contentElement.html(`<div class='tooltipContent' style='outline-style: solid; color: black; outline-width:thin;'><div>Used % Change (` + selected2 + `-` + selected1 + `)</div >`);
                                });

                                tooltipInstance.show(arg.target);

                            });

                            e.cellElement.mouseout(function (arg) {


                                tooltipInstance.hide();
                            });
                        }
                    }
                    else if (e.rowType == "header" && e.column.caption != undefined && e.column.caption.includes("🠕🠗")) {
                        if (e.column.dataField.includes("Plan")) {
                            e.cellElement.mouseover(function (arg) {
                                e.cellElement.addClass('dx-datagrid-action');

                                tooltipInstance.option("contentTemplate", function (contentElement) {
                                    contentElement.html(`<div class='tooltipContent' style='outline-style: solid; color: black; outline-width:thin;'><div>Plan Change (` + selected2 + `-` + selected1 + `)</div >`);
                                });

                                tooltipInstance.show(arg.target);

                            });

                            e.cellElement.mouseout(function (arg) {


                                tooltipInstance.hide();
                            });
                        }
                        else {


                            e.cellElement.mouseover(function (arg) {
                                e.cellElement.addClass('dx-datagrid-action');

                                tooltipInstance.option("contentTemplate", function (contentElement) {
                                    contentElement.html(`<div class='tooltipContent' style='outline-style: solid; color: black; outline-width:thin;'><div>Used Change (` + selected2 + `-` + selected1 + `)</div >`);
                                });

                                tooltipInstance.show(arg.target);

                            });

                            e.cellElement.mouseout(function (arg) {


                                tooltipInstance.hide();
                            });
                        }
                    }
                    else if (e.rowType == "header" && e.column.caption != undefined && e.column.caption.includes("%")) {
                       
                        if (e.column.dataField.includes("Used")) {
                          
                            e.cellElement.mouseover(function (arg) {
                                e.cellElement.addClass('dx-datagrid-action');

                                tooltipInstance.option("contentTemplate", function (contentElement) {
                                    contentElement.html(`<div class='tooltipContent' style='outline-style: solid; color: black; outline-width:thin;'><div>` + selected1 + ` Used %</div >`);
                                });

                                tooltipInstance.show(arg.target);

                            });

                            e.cellElement.mouseout(function (arg) {


                                tooltipInstance.hide();
                            });
                        }
                        if (e.column.dataField.includes("Projected")) {

                            e.cellElement.mouseover(function (arg) {
                                e.cellElement.addClass('dx-datagrid-action');

                                tooltipInstance.option("contentTemplate", function (contentElement) {
                                    contentElement.html(`<div class='tooltipContent' style='outline-style: solid; color: black; outline-width:thin;'><div>` + selected1 + ` Projected %</div >`);
                                });

                                tooltipInstance.show(arg.target);

                            });

                            e.cellElement.mouseout(function (arg) {


                                tooltipInstance.hide();
                            });
                        }
                        if (e.column.dataField.includes("Unused")) {

                            e.cellElement.mouseover(function (arg) {
                                e.cellElement.addClass('dx-datagrid-action');

                                tooltipInstance.option("contentTemplate", function (contentElement) {
                                    contentElement.html(`<div class='tooltipContent' style='outline-style: solid; color: black; outline-width:thin;'><div>` + selected1 + ` Unused %</div >`);
                                });

                                tooltipInstance.show(arg.target);

                            });

                            e.cellElement.mouseout(function (arg) {


                                tooltipInstance.hide();
                            });
                        }
                    }


                    if (e.column.dataField.includes(selected2) && e.column.dataField.includes(selected1)) { //&& e.column.dataField === "Value") {

                    }
                    else {
                        if (e.rowType === "data" && e.column.dataField.includes(selected1)) { //&& e.column.dataField === "Value") {

                            //e.cellElement.css("background-color", "#DFABD5"); //pink


                            //e.cellElement.css("width", "150"); //width djustment 
                            //e.cellElement.css("background-color", "#f6ddcc"); //biscccuit color

                            e.cellElement.css("background-color", "#f9ebff");//lilacc

                        }
                        else if (e.rowType === "data" && e.column.dataField.includes(selected2)) { //&& e.column.dataField === "Value") {

                            e.cellElement.css("background-color", "#e3f2fd");
                                //"#ffede3");//biscccuit lite
                            //"#AFE1AF");  //Green
                            //e.cellElement.css("width", "150"); //width djustment 
                        }
                        if ((e.column.dataField.includes(selected1) || e.column.dataField.includes(selected2)) && !e.column.dataField.includes("Projected"))  {
                           // //debugger;
                          //  e.cellElement.css("width", "89"); //width djustment of cost elt column
                        }

                        if (e.rowType === "data" && e.column.dataField.includes("Used")) {
                            ////debugger;
                            if (e.column.dataField.includes("🠕🠗") || e.column.dataField.includes("%")) {

                            }
                            else {
                                //for .replace() this doesnot replaces all => use split("/").join("_") for this function
                                var used = parseInt(e.value.replace('$', '').split(',').join('').replace('%', '').replace('(', '-').replace(')', ''));
                                var plan = parseInt(e.component.cellValue(e.rowIndex, e.columnIndex - 1).replace('$', '').split(',').join('').replace('%', '').replace('(', '-').replace(')', ''));
                                var used_percent = (used * 100 / plan);
                                
                                    //if (used_percent.toFixed(2) <= 80) {
                                    //    ////debugger;
                                    //    e.cellElement.addClass('low');//green
                                    //}
                                    //else if (used_percent.toFixed(2) > 80 || used_percent.toFixed(2) <= 100) {
                                    //    e.cellElement.addClass('mid');//orange
                                    //}
                                    //else if (used_percent.toFixed(2) > 100) {
                                    //    e.cellElement.addClass('high');//red
                                //}
                                debugger;
                                if (used <= plan) {
                                    //debugger;
                                    e.cellElement.addClass('low');//green
                                }
                                else {
                                    e.cellElement.addClass('high');//red
                                }
                                
                            }
                            
                                   

                        }
                       
                        if (e.rowType === "data" && e.column.dataField.includes("🠕🠗")) {
                            //debugger;
                                //e.cellElement.css("width", "90"); //width djustment of cost elt column
                            //const fieldData = parseFloat(e.value.replace('$', '').replace(',', '').replace('%', '').replace('(', '-').replace(')',''));
                            //let fieldHtml = '';
                           
                            //    if (fieldData) {
                            //        e.cellElement.addClass((fieldData > 0) ? 'inc' : 'dec');
                            //        if (e.column.dataField.includes("%")) {//%change
                            //            fieldHtml += `<div class='diff'>${
                            //                Math.abs(fieldData) + '%'
                            //                }  </div>`;
                            //        }
                            //        else {//Amount change
                            //            fieldHtml += `<div class='diff'>${
                            //                  //'$' + Math.abs(fieldData).toFixed(0)
                            //                DevExpress.localization.formatNumber(fieldData, { type: 'currency', currency: 'USD', precision: 0 })
                            //                }  </div>`;
                            //        }

                            //    } else {
                            //        fieldHtml = fieldData;
                            //    }
                            //    e.cellElement.html(fieldHtml);
                            if (e.column.dataField.includes("%")) {

                            }
                            else {
                                //if (e.column.dataField.includes("Plan")) {
                                //    e.cellElement.mouseover(function (arg) {
                                //        e.cellElement.addClass('dx-datagrid-action');

                                //        tooltipInstance.option("contentTemplate", function (contentElement) {
                                //            contentElement.html(`<div class='tooltipContent' style='outline-style: solid; color: black; outline-width:thin;'><div>Plan Change (` + selected2 + `-` + selected1 + `)</div >`);
                                //        });

                                //        tooltipInstance.show(arg.target);

                                //    });

                                //    e.cellElement.mouseout(function (arg) {


                                //        tooltipInstance.hide();
                                //    });
                                //}
                                //else {




                                //    e.cellElement.mouseover(function (arg) {
                                //        e.cellElement.addClass('dx-datagrid-action');

                                //        tooltipInstance.option("contentTemplate", function (contentElement) {
                                //            contentElement.html(`<div class='tooltipContent' style='outline-style: solid; color: black; outline-width:thin;'><div>Used Change (` + selected2 + `-` + selected1 + `)</div >`);
                                //        });

                                //        tooltipInstance.show(arg.target);

                                //    });

                                //    e.cellElement.mouseout(function (arg) {


                                //        tooltipInstance.hide();
                                //    });
                                //}
                            }
                          
                        }
                        //if (e.column.dataField.includes("Cost")) {
                        //    e.cellElement.css("width", "109"); 
                        //}
                        //if (e.column.dataField.includes("Projected")) {
                        //   // //debugger;
                        //    e.cellElement.css("width", "121"); //width djustment of cost elt column
                        //}
                    }
                    if (e.rowType === "data" && e.column.dataField.includes("%") && e.column.dataField.includes("🠕🠗")) { //&& e.column.dataField === "Value") {

                        //e.cellElement.css("background-color", "#DFABD5"); //pink


                        //e.cellElement.css("width", "150"); //width djustment 
                        //e.cellElement.css("background-color", "#f6ddcc"); //biscccuit color

                        //e.cellElement.css("background-color", "#f9ebff");//lilacc

                    }
                    else if (e.rowType === "data" && e.column.dataField.includes("%")) {
                        e.cellElement.css("background-color", "#f9ebff");//lilacc

                    }
                    else if (e.rowType === "data" && e.column.dataField.includes("%")) {
                        e.cellElement.css("background-color", "#f9ebff");//lilacc

                    }
                    if (e.rowType === "data" && e.column.dataField != undefined && e.column.dataField === "CostElement") {  //Tooltip - specific CostElement for Comparison

                        //e.cellElement.css("background-color", "#005691");
                        e.cellElement.css("color", "black");
                        if (e.value != "Totals") {
                            e.cellElement.mouseover(function (arg) {
                                e.cellElement.css("color", "black");
                                e.cellElement.addClass('dx-datagrid-action');
                                e.cellElement.css("font-weight", "bold");

                                //e.cellElement.css("pointer", "none");
                                //e.cellElement.css("background-color", "blue");

                                tooltipInstance.option("contentTemplate", function (contentElement) {
                                    contentElement.html(`<div class='tooltipContent' style='outline-style: solid; color: black; outline-width:thin;'><div>Click here to view Comparison of Cost Element DrillDown</div>`);
                                });

                                tooltipInstance.show(arg.target);

                            });

                            e.cellElement.mouseout(function (arg) {

                                //e.cellElement.css("color", "white");
                                //e.cellElement.css("background-color", "#005691");
                                e.cellElement.css("font-weight", "normal");
                                tooltipInstance.hide();
                            });
                        }
                    }


                },



                onCellClick: function (e) {
                    ////debugger;
                    get_bus();

                    if (e.rowType == 'data' && e.column.dataField == "CostElement" && e.value != "Totals") {  //DrillDown of specific CostElement for Comparison
                       // //debugger;
                        $.notify('Loading, Please wait !', {
                            globalPosition: "top center",
                            className: "warn",
                            autoHideDelay: 5000
                        });
                        costelement_selected = e.value;

                        //document.getElementById("cards").style.display = "block";
                        //document.getElementById("chartdiv_infra").style.display = "none";
                        //document.getElementById("hc_grid").style.display = "none";
                        //document.getElementById("breadcrumb_nav1").style.display = "block";
                        //document.getElementById("breadcrumb_nav2").style.display = "none";
                        //document.getElementById("twoyr_view").style.display = "block";
                        //document.getElementById("costelement_view").style.display = "none";
                        //document.getElementById("category_view").style.display = "none";
                        //document.getElementById("item_view").style.display = "none";
                        //document.getElementById("threeyr_view").style.display = "none";
                        //var bd1 = document.getElementById("item_nav");
                        //bd1.style.color = "white";
                        //var bd2 = document.getElementById("costelement_nav");
                        //bd2.style.color = "white";
                        //var bd3 = document.getElementById("category_nav");
                        //bd3.style.color = "white";
                        //var bd4 = document.getElementById("twoyr_nav");
                        //bd4.style.color = "black";
                        //var bd5 = document.getElementById("threeyr_nav");
                        //bd5.style.color = "white";

                        //$("div").css("display", "block")

                        //$("#CostElementDrillDown_Title").css("display","none");
                        //$("#CostElementDrillDown").css("display","none");
                        //$("#CategoryDrillDown_Title").css("display","none");
                        //$("#CategoryDrillDown").css("display","none");
                        //$("#Itemview_Title").css("display","none");
                        //$("#RequestTable_Cockpit").css("display","none");

                        vkmdrilldown_costelt(selected, costelement_selected, BUList, selected_ccxc);


                        //$.ajax({
                        //    type: "POST",
                        //    url: "/Cockpit/CostElementDrillDown_comparison/",
                        //    data: {
                        //        'years': selected, 'CostElement_Chosen': costelement_selected, 'buList': BUList, 'selected_ccxc': selected_ccxc
                        //    },
                        //    success: OnSuccess_CostElementDrillDown_comparison,
                        //    error: OnError_CostElementDrillDown
                        //});


                    }

                    document.getElementById("CostElementDrillDown_Title").innerHTML = " COST ELEMENT BASED DRILL DOWN " + " - " + costelement_selected;
                    document.getElementById("CostElementDrillDown_Title").style.fontWeight = "700";



                }

            });
            var tooltipInstance = $("#tooltipContainer").dxTooltip({
                position: "right"
            }).dxTooltip("instance");
            if (current_tab == "twoyr") {
                $.notify('Comparison Table loaded successfully !', {
                    globalPosition: "top center",
                    className: "success",
                    autoHideDelay: 20000
                });
            }


            $("#VKMSummaryTable").css("display","block");
            $(".info_grid").css("display", "block");


            var str = "Compare";
            var result = str.bold();
            genText.innerHTML = result;
            $("#genSpinner").removeClass("fa fa-spinner fa-spin");


        }
        genSpinner_vkm.classList.remove('fa');
        genSpinner_vkm.classList.remove('fa-spinner');
        genSpinner_vkm.classList.remove('fa-pulse');

        genSpinner_vkm_chart.classList.remove('fa');
        genSpinner_vkm_chart.classList.remove('fa-spinner');
        genSpinner_vkm_chart.classList.remove('fa-pulse');
        $('#load_vkm').prop('hidden', true);
        $('#load_vkm_chart').prop('hidden', true);


        $("#gen").css('disabled', true);


    }  




    function OnError_VKMSummaryData(response) {
        debugger;
        $.notify('Unable to load VKM Summary right now, Please Try again later!', {
            globalPosition: "top center",
            className: "warn"
        });
        var str = "Try Again";
        var result = str.bold();
        genText.innerHTML = result;
        $("#genSpinner").removeClass("fa fa-spinner fa-spin");
    }


    

    function OnSuccess_CostElementDrillDown_comparison(data) {
        ////debugger;
        var objdata = eval(data.data.data);
        $('#costelement_nav').click();
        if (data.message != "") {

            CostElementDrillDown = $("#CostElementDrillDown").dxDataGrid({

                dataSource: objdata,
                showColumnLines: true,
                showRowLines: true,
                rowAlternationEnabled: true,
                allowColumnReordering: true,
                allowColumnResizing: true,
                //columnChooser: {
                //    enabled: true
                //},
                //filterRow: {
                //    visible: true

                //},
                showBorders: true,
                //export: {
                //    enabled: true,
                //    fileName: "CostElement Details"
                //},
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
                hoverStateEnabled: {
                    enabled: true
                },
                scrolling: {
                    mode: "virtual",
                    rowRenderingMode: "virtual",
                    columnRenderingMode: "virtual"
                },
                //paging: {
               //     pageSize: 10
               // },
                //searchPanel: {
                //    visible: true,
                //    width: 240,
                //    placeholder: "Search..."
                //},

                onCellPrepared: function (e) {


                    if (e.rowType === "data" && e.column.dataField != undefined && e.column.dataField === "Category Name") {  //Tooltip - specific CostElement for Comparison

                        e.cellElement.mouseover(function (arg) {
                            tooltipInstance_costelt.option("contentTemplate", function (contentElement) {
                                contentElement.html(`<div class='tooltipContent' style='outline-style: solid; color: black; outline-width:thin;'><div>Click here to view Comparison of Category DrillDown</div>`);
                            });

                            tooltipInstance_costelt.show(arg.target);
                        });

                        e.cellElement.mouseout(function (arg) {
                            tooltipInstance_costelt.hide();
                        });
                    }

                },

                onCellClick: function (e) {
                    ////debugger;
                    get_bus();

                    if (e.rowType == 'data' && e.column.dataField == "Category Name") {  //DrillDown of specific CostElement for Comparison
                        categoryid_selected = e.value;


                        $("#CategoryDrillDown_Title").css("display","none");
                        $("#CategoryDrillDown").css("display","none");
                        $("#Itemview_Title").css("display","none");
                        $("#RequestTable_Cockpit").css("display","none");

                        ////debugger;
                        $.ajax({
                            type: "POST",
                            url: "/Cockpit/CategoryDrillDown_comparison/",
                            data: { 'years': selected, 'CostElement_Chosen': costelement_selected, 'Category_Chosen': categoryid_selected, 'buList': BUList, 'selected_ccxc': selected_ccxc },

                            success: OnSuccess_CategoryDrillDown_comparison,
                            error: OnError_CategoryDrillDown
                        });

                        document.getElementById("CategoryDrillDown_Title").innerHTML = " CATEGORY BASED DRILL DOWN " + " - " + categoryid_selected;
                        document.getElementById("CategoryDrillDown_Title").style.fontWeight = "700";
                        $.notify('Loading, Please wait !', {
                            globalPosition: "top center",
                            className: "warn",
                            autoHideDelay: 5000
                        });
                    }

                },

            });
            var tooltipInstance_costelt = $("#tooltipContainer_costelt").dxTooltip({
                // position: "right"
            }).dxTooltip("instance");

            $("#CostElementDrillDown_Title").css("display","block");
            $("#CostElementDrillDown").css("display","block");

            $.notify('Cost Element drilldown loaded successfully !', {
                globalPosition: "top center",
                className: "success",
                autoHideDelay: 20000
            });

            var str = "Compare";
            var result = str.bold();
            genText.innerHTML = result;
            $("#genSpinner").removeClass("fa fa-spinner fa-spin");


        }

        //$('#costelement_nav').css({ "color": "black" });

    }


    function OnError_CostElementDrillDown(response) {
        ////debugger;
        $.notify('Unable to load CostElement based Drill Down right now, Please Try again later!', {
            globalPosition: "top center",
            className: "warn"
        });
    }
             


    function OnSuccess_CategoryDrillDown_comparison(data) {
        ////debugger;
        var objdata = eval(data.data.data);
        $('#category_nav').click();


        CategoryDrillDown = $("#CategoryDrillDown").dxDataGrid({

            dataSource: objdata,
            showColumnLines: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            allowColumnReordering: true,
            allowColumnResizing: true,

            //columnChooser: {
            //    enabled: true
            //},
            //filterRow: {
            //    visible: true

            //},
            showBorders: true,
            headerFilter: {
                visible: true,
                applyFilter: "auto"
            },
            selection: {
                applyFilter: "auto"
            },
            //export: {
            //    enabled: true,
            //    fileName: "Category DrillDown Details"
            //},
            loadPanel: {
                enabled: true
            },
            hoverStateEnabled: {
                enabled: true
            },
            //paging: {
            //    pageSize: 10
            //},
            scrolling: {
                mode: "virtual",
                rowRenderingMode: "virtual",
                columnRenderingMode: "virtual"
            },
            //searchPanel: {
            //    visible: true,
            //    width: 240,
            //    placeholder: "Search..."
            //},

            onCellPrepared: function (e) {


                if (e.rowType === "data" && e.column.dataField != undefined && e.column.dataField === "Item Name") {  //Tooltip - specific CostElement for Comparison

                    e.cellElement.mouseover(function (arg) {
                        tooltipInstance_category.option("contentTemplate", function (contentElement) {
                            contentElement.html(`<div class='tooltipContent' style='outline-style: solid; color: black; outline-width:thin;'><div>Click here to view Item Details</div>`);
                        });

                        tooltipInstance_category.show(arg.target);
                    });

                    e.cellElement.mouseout(function (arg) {
                        tooltipInstance_category.hide();
                    });
                }

            },

            onCellClick: function (e) {
                ////debugger;
                get_bus();

                if (e.rowType == 'data' && e.column.dataField == "Item Name") {  //DrillDown of specific CostElement for Comparison
                    itemname_selected = e.value;


                    $("#Itemview_Title").css("display","none");
                    $("#RequestTable_Cockpit").css("display","none");

                    $.ajax({
                        type: "GET",
                        url: "/Cockpit/ItemID/",
                        //async: false,
                        data: { 'Item_Chosen': itemname_selected },

                        success: OnSuccess_ItemName

                    });
                    function OnSuccess_ItemName(response) {
                        ////debugger;
                        ItemID = response.data;
                        $.ajax({
                            type: "POST",
                            url: "/Cockpit/ItemDrillDown/",
                            data: { 'CostElement_Chosen': costelement_selected, 'Category_Chosen': categoryid_selected, 'Item_Chosen': ItemID, 'buList': BUList, 'years': selected },
                            success: OnSuccess_ItemDrillDown,
                            error: OnError_ItemDrillDown

                        });


                        $.notify('Loading, Please wait !', {
                            globalPosition: "top center",
                            className: "warn",
                            autoHideDelay: 5000
                        });
                    }


                }

            },


        });
        var tooltipInstance_category = $("#tooltipContainer_category").dxTooltip({
            //position: "right"
        }).dxTooltip("instance");


        $("#CategoryDrillDown_Title").css("display","block");
        $("#CategoryDrillDown").css("display","block");

        $.notify('Category drilldown loaded successfully !', {
            globalPosition: "top center",
            className: "success",
            autoHideDelay: 20000
        });
    }


    function OnError_CategoryDrillDown(response) {
        ////debugger;
        $.notify('Unable to load Category based Drill Down right now, Please Try again later!', {
            globalPosition: "top center",
            className: "warn"
        });
    }



    function OnSuccess_ItemDrillDown(response) {
        ////debugger;
        var objdata_item = response.data;
        $('#item_nav').click();


        dataGridCockpit = $("#RequestTable_Cockpit").dxDataGrid({

            dataSource: objdata_item,
            columnResizingMode: "nextColumn",
            columnMinWidth: 50,
            columnAutoWidth: true,
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
            //export: {
            //    enabled: true,
            //    fileName: "Item Details"
            //},
            loadPanel: {
                enabled: true
            },
            scrolling: {
                mode: "virtual",
                rowRenderingMode: "virtual",
                columnRenderingMode: "virtual"
            },
           // paging: {
           //     pageSize: 50
           // },
            //searchPanel: {
            //    visible: true,
            //    width: 240,
            //    placeholder: "Search..."
            //},



            columns: [
                {
                    dataField: "BU",
                    width: 50,

                    setCellValue: function (rowData, value) {
                        ////debugger;
                        rowData.BU = value;

                    },
                    lookup: {
                        dataSource: function (options) {

                            return {

                                store: BU_list,
                            };

                        },
                        valueExpr: "ID",
                        displayExpr: "BU"
                    },

                },



                {
                    dataField: "OEM",
                    validationRules: [{ type: "required" }],
                    width: 80,
                    lookup: {
                        dataSource: OEM_list,
                        valueExpr: "ID",
                        displayExpr: "OEM"
                    },


                },
                {
                    dataField: "DEPT",
                    caption: "Dept",
                    validationRules: [{ type: "required" }],
                    setCellValue: function (rowData, value) {

                        rowData.DEPT = value;
                        rowData.Group = null;

                    },
                    width: 100,
                    lookup: {
                        dataSource: function (options) {

                            return {

                                store: DEPT_list,
                                filter: options.data ? ["Outdated", "=", false] : null


                            };
                        },

                        valueExpr: "ID",
                        displayExpr: "DEPT"

                    },



                },
                {
                    dataField: "Group",
                    width: 100,
                    validationRules: [{ type: "required" }],
                    lookup: {
                        dataSource: function (options) {

                            return {

                                store: Group_list,
                                filter: options.data ? ["Dept", "=", options.data.DEPT] : null
                            };

                        },
                        valueExpr: "ID",
                        displayExpr: "Group"
                    },



                },


                {
                    dataField: "Item_Name",
                    width: 250,
                    validationRules: [{ type: "required" }],

                    lookup: {
                        dataSource: function (options) {

                            return {

                                store: Item_list,

                                filter: options.data ? [["BU", "=", options.data.BU], 'and', ["Deleted", "=", false]] : null
                            }


                        },
                        valueExpr: "S_No",
                        displayExpr: "Item_Name"
                    },


                },
                {
                    dataField: "Category",
                    caption: "Category",
                    validationRules: [{ type: "required" }],
                    lookup: {
                        dataSource: Category_list,
                        valueExpr: "ID",
                        displayExpr: "Category"
                    },
                    allowEditing: false,
                    visible: false,


                },
                {
                    dataField: "Cost_Element",

                    lookup: {
                        dataSource: CostElement_list,
                        valueExpr: "ID",
                        displayExpr: "CostElement"
                    },
                    allowEditing: false,
                    visible: false


                },

                {
                    dataField: "Required_Quantity",
                    caption: "ReqQuantity",
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

                        rowData.Required_Quantity = value;

                    }
                },
                {
                    dataField: "Reviewed_Quantity",
                    caption: "RevQuantity",

                    dataType: "number"
                },
                {
                    dataField: "Ordered_Quantity",

                    visible: false
                },
                {
                    dataField: "Unit_Price",

                    visible: false


                },
                {
                    dataField: "Total_Price",

                    dataType: "number",
                    format: { type: "currency", precision: 2 },
                    valueFormat: "#0.00",
                    allowEditing: false,
                    visible: false
                },
                //Reviewed_Cost
                {
                    dataField: "Reviewed_Cost",
                    caption: "RevCost",


                    dataType: "number",
                    format: { type: "currency", precision: 2 },
                    valueFormat: "#0.00",
                    allowEditing: false
                },
                {
                    dataField: "OrderPrice",

                    format: { type: "currency", precision: 2 },
                    valueFormat: "#0.00",
                    visible: false


                },


                {
                    dataField: "Requestor",
                    allowEditing: false,
                    //visible: false
                },
                {
                    dataField: "Reviewer_1",
                    allowEditing: false,
                    visible: false
                },
                {
                    dataField: "Reviewer_2",
                    allowEditing: false,
                    visible: false
                },
                {
                    dataField: "Comments",

                    visible: false

                },
                {
                    dataField: "RequestDate",
                    allowEditing: false



                },

                //{
                //    dataField: "LeadTime",
                //    width: 50,
                //    caption: "LeadTime (in days)",
                //    visible: false,
                //    calculateCellValue: function (rowData) {
                //        //update the LeadTime
                //        if (rowData.Item_Name == undefined || rowData.Item_Name == null) {

                //            leadtime1 = "";
                //        }

                //        else {

                //            $.ajax({

                //                type: "GET",
                //                url: "/BudgetingRequest/GetLeadTime",
                //                data: { 'ItemName': rowData.Item_Name },
                //                datatype: "json",
                //                async: false,
                //                success: success_getleadtime,

                //            });

                //            function success_getleadtime(response) {

                //                if (response == 0)
                //                    leadtime1 = "";
                //                else
                //                    leadtime1 = response;

                //            }

                //        }

                //        return leadtime1;
                //    }

                //},
                {
                    dataField: "Request_Status"
                }



            ]

        });
        ////debugger;
        document.getElementById("Itemview_Title").innerHTML = "ITEM DETAILED VIEW " + " - " + itemname_selected;
        document.getElementById("Itemview_Title").style.fontWeight = "700";

        $("#Itemview_Title").css("display","block");
        $("#RequestTable_Cockpit").css("display","block");

        $.notify('Item Detailed View is loaded successfully !', {
            globalPosition: "top center",
            className: "success",
            autoHideDelay: 20000
        });
    }
    function OnError_ItemDrillDown(response) {

        $.notify('Please Try Again !', {
            globalPosition: "top center",
            className: "warn"
        });
    }

    $('#item_nav').click(function () {
        $('#cards').css("display","none");
        $('#breadcrumb_nav1').css("display", "none");
        document.getElementById("DeliveryStatus").style.display = "none";
        $('#backButton').dxButton('instance').option('visible', true);
        $('#breadcrumb_nav2').css("display","block");
        //window.location.href = params;
        $('#category_view').css("display","none");
        $('#twoyr_view').css("display","none");
        $('#costelement_view').css("display","none");
        $('#item_view').css("display","block");
        $('#Itemview_Title').css("display","block");
        $('#threeyr_view').css("display","none");

        //var bd1 = document.getElementById("item_nav");
        //bd1.style.color = "black";
        //var bd2 = document.getElementById("costelement_nav");
        //bd2.style.color = "grey";
        //var bd3 = document.getElementById("category_nav");
        //bd3.style.color = "grey";
        var bd4 = document.getElementById("twoyr_nav");
        bd4.style.color = "grey";
        var bd5 = document.getElementById("threeyr_nav");
        bd5.style.color = "grey";

    });
    $('#costelement_nav').click(function () {
        ////debugger;
        $('#cards').css("display","none");
        $('#breadcrumb_nav1').css("display","none");
        $('#backButton').dxButton('instance').option('visible', true);
        $('#breadcrumb_nav2').css("display","block");
        

        $('#category_view').css("display","none");
        $('#twoyr_view').css("display","none");
        $('#costelement_view').css("display","block");
        $('#item_view').css("display","none");
        $('#threeyr_view').css("display","none");
        //$('#costelement_nav').css({ "color": "black" });

        var bd1 = document.getElementById("item_nav");
        bd1.style.color = "grey";
        var bd2 = document.getElementById("costelement_nav");
        bd2.style.color = "black";
        var bd3 = document.getElementById("category_nav");
        bd3.style.color = "grey";
        var bd4 = document.getElementById("twoyr_nav");
        bd4.style.color = "grey";
        var bd5 = document.getElementById("threeyr_nav");
        bd5.style.color = "grey";

    });
    $('#category_nav').click(function () {
        $('#cards').css("display","none");
        $('#breadcrumb_nav1').css("display","none");
        $('#backButton').dxButton('instance').option('visible', true);
        $('#breadcrumb_nav2').css("display","block");

        $('#category_view').css("display","block");
        $('#costelement_view').css("display","none");
        $('#twoyr_view').css("display","none");
        $('#item_view').css("display","none");
        $('#threeyr_view').css("display","none");
        //$('#category_nav').css({ "color": "black" });

        var bd1 = document.getElementById("item_nav");
        bd1.style.color = "grey";
        var bd2 = document.getElementById("costelement_nav");
        bd2.style.color = "grey";
        var bd3 = document.getElementById("category_nav");
        bd3.style.color = "black";
        var bd4 = document.getElementById("twoyr_nav");
        bd4.style.color = "grey";
        var bd5 = document.getElementById("threeyr_nav");
        bd5.style.color = "grey";

    });
    $('#twoyr_nav').click(function () {
        //debugger;
        $('#VkmViewDiv').css("width", "");
        $('#VkmViewDiv').css("left", "330px");
        $('#cards').css("display", "block");
        $('#Quarterly_Util').css("display", "block");

        $('#breadcrumb_nav1').css("display","block");
        $('#backButton').dxButton('instance').option('visible', false);
        $('#breadcrumb_nav2').css("display","none");

        current_tab = "twoyr";
        $('#twoyr_view').css("display", "block");
        $('.info_grid').css("display", "block");
        $('.info_chart').css("display", "none");
        $('#costelement_view').css("display","none");
        $('#category_view').css("display","none");
        $('#item_view').css("display","none");
        $('#threeyr_view').css("display","none");
        //$('#twoyr_nav').css({ "color": "black" });

        //var bd1 = document.getElementById("item_nav");
        //bd1.style.color = "grey";
        //var bd2 = document.getElementById("costelement_nav");
        //bd2.style.color = "grey";
        //var bd3 = document.getElementById("category_nav");
        //bd3.style.color = "grey";
        var bd4 = document.getElementById("twoyr_nav");
        bd4.style.color = "black";
        var bd5 = document.getElementById("threeyr_nav");
        bd5.style.color = "grey";
    });
    $('#threeyr_nav').click(function () {
        debugger;
        $('#Quarterly_Util').css("display", "none");
        $('#VkmViewDiv').css("width", "45%");
        $('#VkmViewDiv').css("left", "310px");
        current_tab = "threeyr";
        $('#cards').css("display", "block");
        $('.info_grid').css("display", "none");
        $('.info_chart').css("display", "block");
        $('#breadcrumb_nav1').css("display","block");
        $('#backButton').dxButton('instance').option('visible', false);
        $('#breadcrumb_nav2').css("display","none");

        $('#twoyr_view').css("display","none");
        $('#costelement_view').css("display","none");
        $('#category_view').css("display","none");
        $('#item_view').css("display", "none"); 

        $('#threeyr_view').css("display", "block");
        if (isthreeyr_datafetched) {

            $('#chartthree').css("display", "block");
            $(".info_chart").css("display", "block");
        }
        else {
            $('#chartthree').css("display", "none");
            $(".info_chart").css("display", "none");
        }
  

      //  var bd1 = document.getElementById("item_nav");
      //  bd1.style.color = "grey";
     //   var bd2 = document.getElementById("costelement_nav");
     //   bd2.style.color = "grey";
     //   var bd3 = document.getElementById("category_nav");
     //   bd3.style.color = "grey";
        var bd4 = document.getElementById("twoyr_nav");
        bd4.style.color = "grey";
        var bd5 = document.getElementById("threeyr_nav");
        bd5.style.color = "black";

    });
    $('#backButton').dxButton({
        text: 'Back',
        icon: 'chevronleft',
        visible: false,
        onClick() {
            ////debugger;
            $('#cards').css("display","block");
            $('#breadcrumb_nav1').css("display","block");
            $('#backButton').dxButton('instance').option('visible', false);
            $('#breadcrumb_nav2').css("display","none");
            current_tab = "twoyr";
            $('#twoyr_view').css("display","block");
            $('#costelement_view').css("display","none");
            $('#category_view').css("display","none");
            $('#item_view').css("display","none");
            $('#threeyr_view').css("display","none");
            //$('#twoyr_nav').css({ "color": "black" });

            var bd1 = document.getElementById("item_nav");
            bd1.style.color = "grey";
            var bd2 = document.getElementById("costelement_nav");
            bd2.style.color = "grey";
            var bd3 = document.getElementById("category_nav");
            bd3.style.color = "grey";
            var bd4 = document.getElementById("twoyr_nav");
            bd4.style.color = "blue";
            var bd5 = document.getElementById("threeyr_nav");
            bd5.style.color = "grey";
          
            this.option('visible', false);
            $("#gen").css('disabled', false);
            $('#gen').click();
        },
    });
    //$('#item_nav1').click(function () {

    //    //window.location.href = params;
    //    $('#category_view').css("display","none");
    //    $('#twoyr_view').css("display","none");
    //    $('#costelement_view').css("display","none");
    //    $('#item_view').css("display","block");
    //    $('#Itemview_Title').css("display","block");
    //    $('#threeyr_view').css("display","none");

    //    var bd1 = document.getElementById("item_nav");
    //    bd1.style.color = "blue";
    //    var bd2 = document.getElementById("costelement_nav");
    //    bd2.style.color = "grey";
    //    var bd3 = document.getElementById("category_nav");
    //    bd3.style.color = "grey";
    //    var bd4 = document.getElementById("twoyr_nav");
    //    bd4.style.color = "grey";
    //    var bd5 = document.getElementById("threeyr_nav");
    //    bd5.style.color = "grey";

    //});
    //$('#costelement_nav1').click(function () {
    //    ////debugger;
    //    $('#cards').css("display","none");
    //    $('#breadcrumb_nav').css("display","block");


    //    $('#category_view').css("display","none");
    //    $('#twoyr_view').css("display","none");
    //    $('#costelement_view').css("display","block");
    //    $('#item_view').css("display","none");
    //    $('#threeyr_view').css("display","none");
    //    //$('#costelement_nav').css({ "color": "black" });

    //    var bd1 = document.getElementById("item_nav1");
    //    bd1.style.color = "grey";
    //    var bd2 = document.getElementById("costelement_nav1");
    //    bd2.style.color = "blue";
    //    var bd3 = document.getElementById("category_nav1");
    //    bd3.style.color = "grey";
    //    var bd4 = document.getElementById("twoyr_nav1");
    //    bd4.style.color = "grey";
    //    var bd5 = document.getElementById("threeyr_nav1");
    //    bd5.style.color = "grey";

    //});
    //$('#category_nav1').click(function () {

    //    $('#category_view').css("display","block");
    //    $('#costelement_view').css("display","none");
    //    $('#twoyr_view').css("display","none");
    //    $('#item_view').css("display","none");
    //    $('#threeyr_view').css("display","none");
    //    //$('#category_nav').css({ "color": "black" });

    //    var bd1 = document.getElementById("item_nav1");
    //    bd1.style.color = "grey";
    //    var bd2 = document.getElementById("costelement_nav1");
    //    bd2.style.color = "grey";
    //    var bd3 = document.getElementById("category_nav1");
    //    bd3.style.color = "blue";
    //    var bd4 = document.getElementById("twoyr_nav1");
    //    bd4.style.color = "grey";
    //    var bd5 = document.getElementById("threeyr_nav1");
    //    bd5.style.color = "grey";

    //});
    //$('#twoyr_nav1').click(function () {
    //    current_tab = "twoyr";
    //    $('#twoyr_view').css("display","block");
    //    $('#costelement_view').css("display","none");
    //    $('#category_view').css("display","none");
    //    $('#item_view').css("display","none");
    //    $('#threeyr_view').css("display","none");
    //    //$('#twoyr_nav').css({ "color": "black" });

    //    var bd1 = document.getElementById("item_nav1");
    //    bd1.style.color = "grey";
    //    var bd2 = document.getElementById("costelement_nav1");
    //    bd2.style.color = "grey";
    //    var bd3 = document.getElementById("category_nav1");
    //    bd3.style.color = "grey";
    //    var bd4 = document.getElementById("twoyr_nav1");
    //    bd4.style.color = "blue";
    //    var bd5 = document.getElementById("threeyr_nav1");
    //    bd5.style.color = "grey";
    //});
    //$('#threeyr_nav1').click(function () {
    //    current_tab = "threeyr";
    //    $('#twoyr_view').css("display","none");
    //    $('#costelement_view').css("display","none");
    //    $('#category_view').css("display","none");
    //    $('#item_view').css("display","none");
    //    $('#threeyr_view').css("display","block");

    //    var bd1 = document.getElementById("item_nav1");
    //    bd1.style.color = "grey";
    //    var bd2 = document.getElementById("costelement_nav1");
    //    bd2.style.color = "grey";
    //    var bd3 = document.getElementById("category_nav1");
    //    bd3.style.color = "grey";
    //    var bd4 = document.getElementById("twoyr_nav1");
    //    bd4.style.color = "grey";
    //    var bd5 = document.getElementById("threeyr_nav1");
    //    bd5.style.color = "blue";

    //});
}



function fnYear1Change() {
    ////debugger;

    var currentYear = (new Date()).getFullYear();              
    selected = [];
    selected1 = $("#ddlYears1").val();
    var sel_yr = parseInt(selected1);
    debugger;
    $("#ddlYears2").html("");
   // document.getElementById("ddlYear2").innerHTML = "";
    debugger;
    for (var i = sel_yr + 1; i <= (currentYear + 1); i++) {
        debugger;
        if (i != sel_yr) {
            var option = document.createElement("OPTION");
            option.innerHTML = i;
            option.value = i;
            ddlYears2.appendChild(option);
            //if (option.value == currentYear || option.value == (currentYear + 1) || option.value == (currentYear - 2)) {
       

            if (option.value == sel_yr + 1) {
                option.defaultSelected = true;
            }
            selected2 = $("#ddlYears2").val();
            //}
        }

    }

    $(".selectpicker").selectpicker('refresh');

    debugger;

    //if (parseInt($("#ddlYears1").val()) > parseInt(selected2)) {
    //    $.notify('Please select a year lesser than the other!!', {
    //        globalPosition: "top center",
    //        className: "info",
    //        autoHideDelay: 20000
    //    });
    //}
    //else {
        selected.push(selected1);
        selected.push(selected2);
        var checkyr = checkyr_validity(selected);
        if (checkyr) {
            //notify();
            $("#VKMSummaryTable").css("display", "none");
            $(".info_grid").css("display", "none");
            $(".info_chart").css("display", "none");
            $("#chartthree").css("display", "none");
            $("#chart").css("display","none");
            $("#CostElementDrillDown_Title").css("display","none");
            $("#CostElementDrillDown").css("display","none");
            $("#CategoryDrillDown_Title").css("display","none");
            $("#CategoryDrillDown").css("display","none");
            $("#Itemview_Title").css("display","none");
            $("#RequestTable_Cockpit").css("display","none");

            $("#gen").css('disabled', false);
            $('#gen').click();

            overall_summary(selected1);
        }
    //}

}
function fnYear2Change() {   
   debugger;     


    selected = [];
    selected2 = $("#ddlYears2").val();
    //if (parseInt($("#ddlYears2").val()) < parseInt(selected1)) {
    //    $.notify('Please select a year greater than the other!!', {
    //        globalPosition: "top center",
    //        className: "info",
    //        autoHideDelay: 20000
    //    });
    //}
    //else {

        selected.push(selected1);
        selected.push(selected2);
    var checkyr = checkyr_validity(selected);
    debugger;
        if (checkyr) {
            //notify();
            $("#VKMSummaryTable").css("display", "none");
            $(".info_grid").css("display", "none");
            $(".info_chart").css("display", "none");

            $("#chartthree").css("display", "none");
            $("#chart").css("display","none");
            $("#CostElementDrillDown_Title").css("display","none");
            $("#CostElementDrillDown").css("display","none");
            $("#CategoryDrillDown_Title").css("display","none");
            $("#CategoryDrillDown").css("display","none");
            $("#Itemview_Title").css("display","none");
            $("#RequestTable_Cockpit").css("display","none");

            $("#gen").css('disabled', false);
            $('#gen').click();
        }

   // }
}

function fnCCXCChange() {
    debugger;
    selected_ccxc = $('#ddlCC_XC').val();
    isthreeyr_datafetched = false;
    //notify();
    get_bus();
    debugger;
    if (selectedthree != undefined && selectedthree.length == 3) {
        isthreeyr_flag = true;
        $.ajax({
            type: "POST",
            //async: false,
            url: "/Cockpit/VKMSummaryData/",
            data: { 'years': selectedthree, 'chart': true, 'buList': BUList, 'selected_ccxc': selected_ccxc },
            success: OnSuccess_ChartSummaryData,
            error: OnError_ChartSummaryData
        });
    }
    //debugger;
    if (current_tab != "threeyr") {
        $('#twoyr_nav').click();
    }
    $("#VKMSummaryTable").css("display", "none");
    $(".info_grid").css("display", "none");
    $(".info_chart").css("display", "none");

    $("#chartthree").css("display", "none");
    $("#chart").css("display","none");
    $("#CostElementDrillDown_Title").css("display","none");
    $("#CostElementDrillDown").css("display","none");
    $("#CategoryDrillDown_Title").css("display","none");
    $("#CategoryDrillDown").css("display","none");
    $("#Itemview_Title").css("display","none");
    $("#RequestTable_Cockpit").css("display","none");


    $("#gen").css('disabled', false);
    $('#gen').click();



}
function fnYearthree1Change() {
    ////debugger;
    selectedthree = [];
    selectedthree1 = $("#ddlYearsthree_1").val();
    selectedthree.push(selectedthree1);
    if (selectedthree2 != undefined)
        selectedthree.push(selectedthree2);
    if (selectedthree3 != undefined)
        selectedthree.push(selectedthree3);

    if (selectedthree.length == 3) {
        var checkyr = checkyr_validity(selectedthree);
        if (checkyr) {
            isthreeyr_datafetched = false;
            $("#chartthree").css("display", "none");
            $(".info_chart").css("display", "none");
        $('#load_vkm_chart').prop('hidden', false);
            genSpinner_vkm_chart.classList.add('fa');
            genSpinner_vkm_chart.classList.add('fa-spinner');
            genSpinner_vkm_chart.classList.add('fa-pulse');

            $.notify('Comparison Chart will be rendered here. Please wait !!', {
                globalPosition: "top center",
                className: "info",
                autoHideDelay: 20000
            });
            isthreeyr_flag = true;
            $.ajax({
                type: "POST",
                url: "/Cockpit/VKMSummaryData/",
                data: { 'years': selectedthree, 'chart': true, 'buList': BUList, 'selected_ccxc': selected_ccxc },
                success: OnSuccess_ChartSummaryData,
                error: OnError_ChartSummaryData
            });
        }
    }


}
function fnYearthree2Change() {
    ////debugger;
    selectedthree = [];
    selectedthree2 = $("#ddlYearsthree_2").val();
    if (selectedthree1 != undefined)
        selectedthree.push(selectedthree1);
    selectedthree.push(selectedthree2);
    if (selectedthree3 != undefined)
        selectedthree.push(selectedthree3);;

    if (selectedthree.length == 3) {
        var checkyr = checkyr_validity(selectedthree);
        if (checkyr) {
            isthreeyr_datafetched = false;
            $("#chartthree").css("display", "none");
            $(".info_chart").css("display", "none");
            $('#load_vkm_chart').prop('hidden', false);
            genSpinner_vkm_chart.classList.add('fa');
            genSpinner_vkm_chart.classList.add('fa-spinner');
            genSpinner_vkm_chart.classList.add('fa-pulse');

            $.notify('Comparison Chart will be rendered here. Please wait !!', {
                globalPosition: "top center",
                className: "info",
                autoHideDelay: 20000
            });
            isthreeyr_flag = true;
            $.ajax({
                type: "POST",
                url: "/Cockpit/VKMSummaryData/",
                data: { 'years': selectedthree, 'chart': true, 'buList': BUList, 'selected_ccxc': selected_ccxc },
                success: OnSuccess_ChartSummaryData,
                error: OnError_ChartSummaryData
            });
        }
    }

}
function fnYearthree3Change() {
    ////debugger;
    selectedthree = [];
    selectedthree3 = $("#ddlYearsthree_3").val();
    if (selectedthree1 != undefined)
        selectedthree.push(selectedthree1);
    if (selectedthree2 != undefined)
        selectedthree.push(selectedthree2);
    selectedthree.push(selectedthree3);

    if (selectedthree.length == 3) {
        var checkyr = checkyr_validity(selectedthree);
        if (checkyr) {
            isthreeyr_datafetched = false;
            $("#chartthree").css("display", "none");
            $(".info_chart").css("display", "none");
            $('#load_vkm_chart').prop('hidden', false);
            genSpinner_vkm_chart.classList.add('fa');
            genSpinner_vkm_chart.classList.add('fa-spinner');
            genSpinner_vkm_chart.classList.add('fa-pulse');

            $.notify('Comparison Chart will be rendered here. Please wait !!', {
                globalPosition: "top center",
                className: "info",
                autoHideDelay: 20000
            });

            isthreeyr_flag = true;

            $.ajax({
                type: "POST",
                url: "/Cockpit/VKMSummaryData/",
                data: { 'years': selectedthree, 'chart': true, 'buList': BUList, 'selected_ccxc': selected_ccxc },
                success: OnSuccess_ChartSummaryData,
                error: OnError_ChartSummaryData
            });
        }
    }

}
function get_bus() {
    selected_ccxc = $('#ddlCC_XC').val();
    debugger;
    
    $.ajax({
        type: "POST",
        url: "/Cockpit/BUs_forpresentNTID_CCXC/",
        data: { 'ccxc': true, 'selected_ccxc': selected_ccxc },
        async: false,
        success: OnSuccess_BUs_forpresentNTID,
        error: OnError_BUs_forpresentNTID
    });
}

function OnSuccess_BUs_forpresentNTID(response) {
    debugger;
    BUList = response.data;
    if (!response.success) {
        //$.notify('Unable to load BUs of the current user right now, Please Try again later!', {
        //    globalPosition: "top center",
        //    className: "error"
        //});
    }
}
function OnError_BUs_forpresentNTID(response) {
    debugger;
    //$.notify('Unable to load BUs of the current user right now, Please Try again later!', {
    //    globalPosition: "top center",
    //    className: "warn"
    //});
}

function checkyr_validity(yrs) {

    var checkduplicateyr = yrs.length === new Set(yrs).size; //arr.some((val, i) => arr.indexOf(val) !== i) --some() method checks if any of elements in array pass a test
    if (!checkduplicateyr)
        $.notify('Same year has been selected multiple times, Please check!', {
            globalPosition: "top center",
            className: "error"
        });

    ////debugger;
    return checkduplicateyr;
}
function OnSuccess_ChartSummaryData(data) {
   // //debugger;
    //var chartData = [];
    $('#load_vkm_chart').prop('hidden', true);
    dataObj = eval(data.data.data);


    //for (i = 0; i < dataObj.length; i++) {
      
    //    var c_Planned = [];
    //    var c_Utilized = [];
    //    var c_Percentage = [];  
    //    //var Totals = [];
    //    ////debugger;
    //    c_Planned.push({ c_planned: dataObj[i].Planned });
    //    c_Utilized.push({ c_utilized: dataObj[i].Utilized });
    //    c_Percentage.push({ c_percentage: dataObj[i].Percentage_Utilization });
    //    year.push({ year: dataObj[i].Year });
    //    ////debugger;
    //    //chartData.push({
    //    //    year: dataObj[i].Year, Plannedvkm: c_Planned, Utilizedvkm: c_Utilized, Percentagevkm: c_Percentage
    //    //});

    //    //chartData.push({ year: dataObj[i].Year, plannedMAE: parseInt(dataObj[i].Planned_MAE), plannedNMAE: parseInt(dataObj[i].Planned_Non_MAE), plannedsw: parseInt(dataObj[i].Planned_Software), plannedTotals: parseInt(dataObj[i].Planned_Total), utilizedMAE: parseInt(dataObj[i].Utilized_MAE), utilizedNMAE: parseInt(dataObj[i].Utilized_Non_MAE), utilizedsw: parseInt(dataObj[i].Utilized_Software), utilizedTotals: parseInt(dataObj[i].Utilized_Total), perMAE: parseInt(dataObj[i].Per_MAE_Utilization), perNMAE: parseInt(dataObj[i].Per_NMAE_Utilization), perSW: parseInt(dataObj[i].Per_Software_Utilization), perTotals: parseInt(dataObj[i].Per_Totals_Utilization) });
    //}

    if (selectedthree != undefined && selectedthree.length == 3) {

        $("#chartthree").css("display", "none");
        $(".info_chart").css("display", "none");

        genSpinner_vkm_chart.classList.add('fa');
        genSpinner_vkm_chart.classList.add('fa-spinner');
        genSpinner_vkm_chart.classList.add('fa-pulse');

        //debugger;
        $("#chartthree").dxChart({

            dataSource: dataObj,
            resolveLabelOverlapping: 'stack',
            //scrollBar: {
            //    visible: true
            //},
            //zoomAndPan: {
            //    argumentAxis: "both",

            //},
            
         //   size: {
         //       height: 190,
         //       width: 1125
        //    },
            //title: {
            //    text: "VKM Comparison Chart (2020 - 2021)",
            //    font: {
            //        size: 26,
            //        weight: 800,
            //        color: "black"
            //    },


            //},

            //    new DevExpress.data.DataSource({
            //    store: chartData,

            //    map: function (item) {
            //        ////debugger;
            //        return $.extend({

            //            plMAE: item.AllMAE[0].plannedMAE,
            //            utMAE: item.AllMAE[0].utilizedMAE,
            //            plNMAE: item.AllNMAE[0].plannedNMAE,
            //            utNMAE: item.AllNMAE[0].utilizedNMAE,
            //            plsw: item.AllSW[0].plannedsw,
            //            utsw: item.AllSW[0].utilizedsw,
            //            plTotals: item.AllTotals[0].plannedTotals,
            //            utTotals: item.AllTotals[0].utilizedTotals,
            //            //year_CostE: item
            //        }, item);
            //    }
            //}),

            commonSeriesSettings: {
                argumentField: "Year",
                type: "bar",
                hoverMode: "allArgumentPoints",
                selectionMode: "allArgumentPoints",
                barWidth: 30

            },




            series: [

                {
                    valueField: "Planned", name: "Plan",
                    color: '#40AADB',
                    //label: {

                    //    visible: true,

                    //    format: {
                    //        type: "currency",
                    //        precision: 0
                    //    },
                       

                    //    //    customizeText: function (arg) {
                    //    //        return arg.value + ;
                    //    //},


                    //}
                },
                {
                    valueField: "Utilized", name: "Utilize",
                    //label: {

                    //    visible: true,
                    //    format: {
                    //        type: "currency",
                    //        precision: 0
                    //    }
                    //},
                    color: '#9b59b6'
                        //'#c39bd3' //'#af7ac5'
 //'#a349c4'
                },


                //{ valueField: "plMAE", name: "Planned MAE" },
                //{ valueField: "utMAE", name: "Utilized MAE" },
                //{ valueField: "plNMAE", name: "Planned Non-MAE" },
                //{ valueField: "utNMAE", name: "Utilized Non-MAE" },
                //{ valueField: "plsw", name: "Planned Software" },
                //{ valueField: "utsw", name: "Utilized Software" },
                //{ valueField: "plTotals", name: "Planned Total" },
                //{ valueField: "utTotals", name: "Utilized Total" },
                {
                    valueField: "Percentage_Utilization",
                    axis: "Percentage_VKM",
                    type: "line",
                    name: "% Utilize",
                    label: {

                        visible: true,
                        format: {
                            type: "fixedpoint",
                            precision: 0
                        },
                        customizeText: function (e) {
                           
                            return e.valueText + "%";
                        }
                    },
                    color: '#b84d8a'
                }


            ],



            legend: {
                //position: "outside", // or "outside"
                //horizontalAlignment: "right", // or "left" | "right"
                //verticalAlignment: "top", // or "bottom"


                //horizontalAlignment: "right",
                //horizontalAlignment: "center", // or "left" | "right"
                //verticalAlignment: "bottom",
                //border: { visible: true },
                //columnCount: 3,
                font: {
                    color: "black",
                    family: "Arial",
                    size: 16

                },

            },
            argumentAxis: {
                label: {

                    font: {
                        color: "black",
                        family: "Arial",
                        size: 14

                    },
                },
                //constantLines: [
                //    { value: 'Totals 2022' }

                //]//,
                //label: {

                //    customizeText: function (e) {

                //        return "<p style='color:black'>" + e.valueText + "</p>";

                //    }

                //}

            },
            valueAxis: [


                {

                    position: 'left',
                    font: {
                        color: "black",
                        family: "Arial",
                        size: 16

                    },
                    valueMarginsEnabled: false,
                    title: {
                        text: "VKM Budget (in $)",
                        font: {
                            color: "black",
                            family: "Arial",
                            size: 16

                        },
                    },
                    label: {
                        font: {
                            color: "black",
                            family: "Arial",
                            size: 14

                        },
                    },
                    //constantLines: [{
                    //    //label: {
                    //    //    text: "Low Average"
                    //    //},
                    //    //width: 2,
                    //    value: 6000000
                    //    //color: "#8c8cff",
                    //    //dashStyle: "dash"
                    //    //paddingLeftRight : 
                    //}]
                },

                {
                    name: "Percentage_VKM",
                    position: "right",
                    grid: {
                        visible: true
                    },
                    title: {
                        text: "Percent Utilization",
                        font: {
                            color: "black",
                            family: "Arial",
                            size: 16

                        },
                    },
                    label: {
                        font: {
                            color: "black",
                            family: "Arial",
                            size: 14

                        },
                    },
                    //constantLines: [
                    //    { value: 40 },
                    //    { value: 20 }

                    //]
                }
            ],


            //export: {
            //    enabled: true,
            //    fileName: "VKM Comparison Chart"
            //},
            tooltip: {
                enabled: true,
                location: "edge",
                format: {
                    type: "fixedpoint",
                    precision: 2
                },
                customizeTooltip: function (arg) {
                    debugger;
                    if(arg.seriesName == "% Utilize")
                        return { text: arg.valueText + "%"};
                    else
                        return { text: arg.argument + " " + arg.point.series.name + " : " + "$" + arg.valueText };
                    //if (arg.percent != null)
                    //    return { text:   arg.argument + " : " + arg.valueText + "\n" + arg.point.series.name + ":" + arg.percent }
                    //else
                    //    return { text:  arg.valueText }
                    // //debugger;;

                }
            }
        });
        //////debugger; 
        //var str = "Compare";
        //var result = str.bold();
        //genText.innerHTML = result;
        //$("#genSpinner").removeClass("fa fa-spinner fa-spin");
        isthreeyr_datafetched = true
        if (current_tab == "threeyr") {
            //debugger;

            genSpinner_vkm_chart.classList.remove('fa');
            genSpinner_vkm_chart.classList.remove('fa-spinner');
            genSpinner_vkm_chart.classList.remove('fa-pulse');

            $("#chartthree").css("display", "block");
            $(".info_chart").css("display", "block");

            $.notify('Comparison Chart loaded successfully !', {
                globalPosition: "top center",
                className: "success",
                autoHideDelay: 20000
            });
        }
        isthreeyr_flag = false;
        //$("#gen").css('disabled', true);

    }
    else {
        if (selected.length == 2) {
            ////debugger;
            $("#chart").dxChart({

                dataSource: dataObj,
                resolveLabelOverlapping: 'stack',
                scrollBar: {
                    visible: true
                },
                zoomAndPan: {
                    argumentAxis: "both",

                },
                height: 50,
                //size: {
                //    height: 250,
                //    width: 1150
                //},
                //title: {
                //    text: "VKM Comparison Chart (2020 - 2021)",
                //    font: {
                //        size: 26,
                //        weight: 800,
                //        color: "black"
                //    },


                //},

                //    new DevExpress.data.DataSource({
                //    store: chartData,

                //    map: function (item) {
                //        ////debugger;
                //        return $.extend({

                //            plMAE: item.AllMAE[0].plannedMAE,
                //            utMAE: item.AllMAE[0].utilizedMAE,
                //            plNMAE: item.AllNMAE[0].plannedNMAE,
                //            utNMAE: item.AllNMAE[0].utilizedNMAE,
                //            plsw: item.AllSW[0].plannedsw,
                //            utsw: item.AllSW[0].utilizedsw,
                //            plTotals: item.AllTotals[0].plannedTotals,
                //            utTotals: item.AllTotals[0].utilizedTotals,
                //            //year_CostE: item
                //        }, item);
                //    }
                //}),

                commonSeriesSettings: {
                    argumentField: "Year",
                    type: "bar",
                    hoverMode: "allArgumentPoints",
                    selectionMode: "allArgumentPoints",

                },


                series: [

                    {
                        valueField: "Planned", name: "Planned_VKM",
                        label: {

                            visible: true,
                            format: {
                                type: "currency",
                                precision: 0
                            },

                            //    customizeText: function (arg) {
                            //        return arg.value + ;
                            //},


                        }
                    },
                    {
                        valueField: "Utilized", name: "Utilized_VKM",
                        label: {

                            visible: true,
                            format: {
                                type: "currency",
                                precision: 0
                            }
                        }
                    },


                    //{ valueField: "plMAE", name: "Planned MAE" },
                    //{ valueField: "utMAE", name: "Utilized MAE" },
                    //{ valueField: "plNMAE", name: "Planned Non-MAE" },
                    //{ valueField: "utNMAE", name: "Utilized Non-MAE" },
                    //{ valueField: "plsw", name: "Planned Software" },
                    //{ valueField: "utsw", name: "Utilized Software" },
                    //{ valueField: "plTotals", name: "Planned Total" },
                    //{ valueField: "utTotals", name: "Utilized Total" },
                    {
                        valueField: "Percentage_Utilization",
                        axis: "Percentage_VKM",
                        type: "line",
                        name: "Percentage Utilization",
                        label: {

                            visible: true,
                            format: {
                                type: "fixedpoint",
                                precision: 0
                            },
                            customizeText: function (e) {
                                return e.valueText + "%";
                            }
                        }

                    }


                ],



                legend: {
                    //position: "outside", // or "outside"
                    //horizontalAlignment: "right", // or "left" | "right"
                    //verticalAlignment: "top", // or "bottom"


                    //horizontalAlignment: "right",
                    //horizontalAlignment: "center", // or "left" | "right"
                    //verticalAlignment: "bottom",
                    //border: { visible: true },
                    //columnCount: 3,
                    font: {
                        color: "black",
                        family: "Arial",
                        size: 16

                    },

                },
                argumentAxis: {
                    label: {

                        customizeText: function (e) {

                            return "<p style='color:black'>" + e.valueText + "</p>";

                        },
                       
                            font: {
                                color: "black",
                                family: "Arial",
                                size: 14

                            },
                        

                    },
                    //constantLines: [
                    //    {
                    //        value: "Totals 2021",

                    //        paddingLeftRight: 100,
                    //        verticalAlignment : "left"

                    //    }

                    //]
                },
                valueAxis: [


                    {

                        position: 'left',
                        font: {
                            color: "black",
                            family: "Arial",
                            size: 16

                        },
                        valueMarginsEnabled: false,
                        title: {
                            text: "VKM Budget (in $)",
                            font: {
                                color: "black",
                                family: "Arial",
                                size: 16

                            },
                        }
                    },

                    {
                        name: "Percentage_VKM",
                        position: "right",
                        grid: {
                            visible: true
                        },
                        title: {
                            text: "Percentage Utilization",
                            font: {
                                color: "black",
                                family: "Arial",
                                size: 16

                            },
                        },
                        label: {
                            font: {
                                color: "black",
                                family: "Arial",
                                size: 16

                            },
                        }
                    }
                ],


                //export: {
                //    enabled: true,
                //    fileName: "VKM Comparison Chart"
                //},
                tooltip: {
                    enabled: true
                }
            });

            $("#chart").css("display","block");

            //var str = "Compare";
            //var result = str.bold();
            //genText.innerHTML = result;
            //$("#genSpinner").removeClass("fa fa-spinner fa-spin");

            if (current_tab == "twoyr") {
                $.notify('Comparison Chart loaded successfully !', {
                    globalPosition: "top center",
                    className: "success",
                    autoHideDelay: 20000
                });
            }

            //$("#gen").css('disabled', true); 
        }

    }



}


function OnError_ChartSummaryData(response) {
   // //debugger;
    $.notify('Unable to load VKM Cockpit Chart right now, Please Try again later!', {
        globalPosition: "top center",
        className: "warn"
    });

    var str = "Try Again";
    var result = str.bold();
    genText.innerHTML = result;
    $("#genSpinner").removeClass("fa fa-spinner fa-spin");
}


