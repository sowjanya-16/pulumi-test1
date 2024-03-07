             //Javascript file for Budgeting Inventory Details - mae9cob

var dataGridLEP, busummarytable;
var unitpriceinusd;
var Selected = [];
var genSpinner_load = document.querySelector("#load");
var BU_list, Category_list, CostElement_list, Currency_list, VendorCategory_list, BudgetCodeList, Material_Group_list, Order_Type_list, UOM_list; 
var Material_Group_list1 = [];
var conversionEURate, conversionINRate, conversionLBRate, conversionJPYRate;
var newMaterialGrp_list;
var newitem_input;
$(".custom-file-input").on("change", function () {
                    var fileName = $(this).val().split("\\").pop();
                    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
                });

               
window.onload = function () {
   
                    $("#RequestTable").prop('hidden', true);
                    
                    

                    genSpinner_load.classList.add('fa');
                    genSpinner_load.classList.add('fa-spinner');
                    genSpinner_load.classList.add('fa-pulse');
                    document.getElementById("loadpanel").style.display = "block";


                    var genSpinner = document.querySelector("#UploadSpinner");
                    genSpinner.classList.remove('fa');
                    genSpinner.classList.remove('fa-spinner');
                    genSpinner.classList.remove('fa-pulse');
                    checkFileAPI();
                    
                }
$.notify('Item Master List is being retrieved, Please wait!', {
    globalPosition: "top center",
    className: "info",
    autoHideDelay: 6000,
});
//ItemNameLookup,CostElementLookup,CategoryLookup,CurrencyLookup, VendorLookup
$.ajax({

    type: "GET",
    url: "/BudgetingInventory/Lookup",
    async: false,
    success: onsuccess_lookupdata,
    error: onerror_lookupdata
})

$.ajax({

    type: "GET",
    url: "/BudgetingInventory/MaterialGroup_Lookup",
    async: false,
    success: onsuccess_materiallookupdata,
    error: onerror_materiallookupdata
})

function onsuccess_lookupdata(response) {
    //debugger;
    lookup_data = response.data;

    Category_list = lookup_data.Category_List;
    CostElement_list = lookup_data.CostElement_List;
    Currency_list = lookup_data.Currency_List;
    VendorCategory_list = lookup_data.VendorCategory_List;
    BU_list = lookup_data.BU_List;
    BudgetCodeList = lookup_data.BudgetCodeList;
    Order_Type_list = lookup_data.Order_Type_List;
    UOM_list = lookup_data.UOM_List;

    //for (i = 0; i < Material_Group_list.length; i++) {

    //    //grmemarray.push({ Project: res[i].Project_Team.substring(1, res[i].Project_Team.length - 1)  });
    //    Material_Group_list1.push(Material_Group_list[i].Material_Group);

    //}
    debugger;
}
function onerror_lookupdata(response) {
    $.notify("Unable to fetch the lookup data!", {

        globalPosition: "top center",
        className: "warn"
    })

}
function onsuccess_materiallookupdata(response) {
    debugger;
    lookup_data = response.data;

    Material_Group_list = lookup_data.Material_Group_list;
  
    debugger;
}
function onerror_materiallookupdata(response) {
    debugger;
    $.notify("Unable to fetch the material lookup data!", {

        globalPosition: "top center",
        className: "warn"
    })

}
$.ajax({
    type: "GET",
    url: encodeURI("../BudgetingInventory/InitRowValues"),
    success: OnSuccessCall_dnew,
    error: OnErrorCall_dnew

});
function OnSuccessCall_dnew(response) {
    debugger;
    new_request = response.data;

}
function OnErrorCall_dnew(response) {

    //$.notify('Add new error!', {
    //    globalPosition: "top center",
    //    className: "warn"
    //});
}
//Ajax call to Get Request Item MasterList data
$.ajax({
    type: "GET",
    url: encodeURI("../BudgetingInventory/GetEURINRates"),
    async: false,
    success: OnSuccess_GetEURINRates,
    error: OnError_GetEURINRates
});
function OnSuccess_GetEURINRates(response) {
    //debugger;

    conversionEURate = response.EUR;
    conversionINRate = response.INR;
    conversionLBRate = response.LB;
    conversionJPYRate = response.JPY
}
function OnError_GetEURINRates(response) {
    debugger;
    conversionEURate = 1.15;
    conversionINRate = 0.014;
}

                //Ajax call to Get Request Item MasterList data
                $.ajax({
                    type: "GET",
                    url: encodeURI("../BudgetingInventory/GetData"),
                    success: OnSuccess_GetData,
                    error: OnError_GetData
                });


function OnSuccess_GetData(response) {
    debugger;
    var objdata = response;
        //(response.data);
    var yr_list = [];
    var currentYear = (new Date()).getFullYear();
    for (var i = currentYear + 1; i >= 2021; i--) {
        yr_list.push({ "Year": i });

    }
    
    //var itemMasterArray = [];
    //for (i = 0; i < res.length; i++) { // to split the comma sep Material grp into list and store as datasource - ince tgbox only accepts if array

    //    //grmemarray.push({ Project: res[i].Project_Team.substring(1, res[i].Project_Team.length - 1)  });
    //    grmemarray.push({
    //        SAP_Role: res[i].SAP_Role,
    //        VKM_Role: res[i].VKM_Role1.split(','),
    //        Year: res[i].Year,
    //        SNo: res[i].SNo,
    //        NTID: res[i].NTID,
    //        Employee_Number: res[i].Employee_Number,
    //        Employee_Name: res[i].Employee_Name,
    //        Product_Area: res[i].Product_Area_ID,
    //        Section: res[i].Section_ID,
    //        Department: res[i].Department_ID,
    //        Group: res[i].Group_ID,
    //        Level: res[i].Level,
    //        //SAP_Role          : = res[i].SAP_Role,
    //        //VKM_Role          : = res[i].RoleName,
    //        Remarks: res[i].Remarks,
    //        Plan_Sum: res[i].Plan_Sum,
    //        Jan: res[i].Jan,
    //        Feb: res[i].Feb,
    //        Mar: res[i].Mar,
    //        Apr: res[i].Apr,
    //        May: res[i].May,
    //        Jun: res[i].Jun,
    //        Jul: res[i].Jul,
    //        Aug: res[i].Aug,
    //        Sep: res[i].Sep,
    //        Oct: res[i].Oct,
    //        Nov: res[i].Nov,
    //        Dec: res[i].Dec,
    //        PYO: res[i].PYO,
    //        Updated_By: res[i].Updated_By,
    //        Updated_At: new Date(parseInt(res[i].Updated_At.substr(6)))
    //    });

    //}
    //var store = new DevExpress.data.ArrayStore({
    //    key: "ID",
    //    data: Material_Group_list
    //});
                    dataGridLEP = $("#RequestTable").dxDataGrid({

                        dataSource: objdata,
                        columnFixing: {
                            enabled: true
                        },
                        width: "97vw", //needed to allow horizontal scroll
                        height: "70vh",
                        columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
                        remoteOperations: true,
                        scrolling: {
                            mode: "virtual",
                            rowRenderingMode: "virtual",
                            columnRenderingMode: "virtual"
                        },
                        showColumnLines: true,
                        showRowLines: true,
                        rowAlternationEnabled: true,
                        hoverStateEnabled: {
                            enabled: true
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
                                        text: 'Clear Item Filters',
                                        hint: 'Clear Item Filters',
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
                                addRowButton.options.text = "Add New Item";
                                addRowButton.options.hint = "Add New Item";
                                addRowButton.showText = "always";
                            }

                            let columnChooserButton = toolbarItems.find(x => x.name === "columnChooserButton");
                            columnChooserButton.options.text = "Hide Item Details";
                            columnChooserButton.options.hint = "Hide Item Details";
                            columnChooserButton.showText = "always";

                        },
                        editing: {
                            mode: "row",
                            allowUpdating: true,
                            allowDeleting: true,
                            allowAdding: true,
                            useIcons: true
                        },
                        allowColumnReordering: true,
                        allowColumnResizing: true,
                        columnChooser: {
                            enabled: true
                        },
                        filterRow: {
                            visible: true

                        },
                        //focusedRowEnabled: true,
                        onContentReady: function (e) {
                            //debugger;
                            e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
                        },
                        keyExpr: "S_No",
                        columnResizingMode: "nextColumn",
                        columnMinWidth: 50,
                        //stateStoring: {
                        //    enabled: true,
                        //    type: "localStorage",
                        //    storageKey: "RequestID"
                        //},
                      
                        headerFilter: {
                            visible: true,
                            applyFilter: "auto",
                            alloeSearch: true
                        },
                        selection: {
                            applyFilter: "auto"
                        },
                        showBorders: true,
                        paging: {
                            pageSize: 20
                        },
                        searchPanel: {
                            visible: true,
                            width: 240,
                            placeholder: "Search..."
                        },
                        loadPanel: {
                            enabled: true
                        },

                        onEditorPreparing(e) {
                            //debugger;
                            if (e.parentType === 'dataRow' && e.dataField === 'BudgetCode') {
                                //e.editorOptions.disabled = (e.row.data.CostElementID == null || e.row.data.CostElementID == '');
                            }
                        },

                        columns: [
                            {
                                type: "buttons",

                                width: 60,
                                alignment: "left",
                                fixed: true,
                                fixedPosition: "left",
                                buttons: [
                                    "edit", "delete"
                                ]
                            },
                            {

                                alignment: "center",
                                columns: [
                                    {

                                        dataField: "VKMYear",
                                        validationRules: [{ type: "required" }],
                                        width: 95,
                                        lookup: {
                                            dataSource: yr_list,
                                            valueExpr: "Year",
                                            displayExpr: "Year"
                                        },
                                        caption: "VKM Yr",
                                        allowEditing: true

                                    },
                                    {
                                        dataField: "BU",
                                        setCellValue: function (rowData, value) {

                                            rowData.BU = value;
                                        },
                                        lookup: {
                                            dataSource: function (options) {
                                                
                                                return {

                                                    store: BU_list

                                                };
                                            },

                                            valueExpr: "ID",
                                            displayExpr: "BU"

                                        },
                                        validationRules: [{ type: "required" }],
                                    },
                                    {
                                        dataField: "Item_Name",
                                        validationRules: [{ type: "required" }],
                                        minWidth: 250

                                    },
                                    {
                                        dataField: "Category",
                                        caption: "Category",
                                        validationRules: [{type: "required" }],

                                        lookup: {
                                            dataSource: Category_list,
                                            valueExpr: "ID",
                                            displayExpr: "Category"
                                        },
                                        

                                    },
                                    {
                                        dataField: "Cost_Element",
                                        width: 140,
                                        validationRules: [{ type: "required" }],
                                        setCellValue(rowData, value) {
                                            rowData.Cost_Element = value;
                                            rowData.BudgetCode = null;
                                        },
                                        lookup: {
                                            dataSource: CostElement_list,
                                            valueExpr: "ID",
                                            displayExpr: "CostElement"
                                        }


                                    },

                                    {
                                        dataField: "BudgetCode",
                                        width: 140,
                                        validationRules: [{ type: "required" }],
                                        //setCellValue: function (rowData, value) {

                                        //    rowData.BudgetCode = value;
                                        //},
                                        lookup: {
                                            dataSource(options) {
                                               
                                                return {
                                                    store: BudgetCodeList,
                                                    filter: options.data ? ['CostElementID', '=', options.data.Cost_Element] : null,
                                                };
                                            },
                                            valueExpr: "Budget_Code",
                                            displayExpr: "Budget_Code"
                                        }


                                    },



                                    {
                                        dataField: "Unit_Price",
                                        dataType: "number",
                                        width: 115,
                                        format: { precision: 2 },
                                        valueFormat: "#0.00",
                                        setCellValue: function (rowData, value) {

                                            rowData.Unit_Price = value;
                                            unitprice = value;
                                        },
                                        validationRules: [{type: "required" }, {
                                            type: "range",
                                            message: "Please enter valid price > 0",
                                            min: 0.01,
                                            max: Number.MAX_VALUE
                                        }]


                                    },
                                    {
                                        dataField: "Currency",
                                        width: 110,
                                        validationRules: [{type: "required" }],
                                        setCellValue: function (rowData, value) {

                                            rowData.Currency = value;
                                        },
                                        lookup: {
                                            dataSource: function (options) {

                                                return {

                                                    store: Currency_list,


                                                };
                                            },
                                            valueExpr: "ID",
                                            displayExpr: "Currency"

                                        },
                                    },


                                    {
                                        dataField: "Unit_PriceUSD",
                                        width: 140,
                                        caption: "Unit Price ($)",
                                        calculateCellValue: function (rowData) {

                                            //update the price in USD
                                            if (rowData.Unit_Price == undefined || rowData.Currency == undefined) {

                                                unitpriceinusd = 0.0;
                                            }

                                            else {
                                                if (rowData.Currency == 1)
                                                    unitpriceinusd = rowData.Unit_Price;
                                                else if (rowData.Currency == 2)
                                                    unitpriceinusd = rowData.Unit_Price * conversionEURate;
                                                else if (rowData.Currency == 3)
                                                    unitpriceinusd = rowData.Unit_Price * conversionINRate;
                                                else if (rowData.Currency == 4)
                                                    unitpriceinusd = rowData.Unit_Price * conversionLBRate;
                                                else if (rowData.Currency == 5)
                                                    unitpriceinusd = rowData.Unit_Price * conversionJPYRate;
                                                else
                                                    unitpriceinusd = 0.0;

                                                //$.ajax({

                                                //    type: "GET",
                                                //    url: "/BudgetingInventory/GetUnitPriceinUSD",
                                                //    data: { 'UnitPrice': rowData.Unit_Price, 'Currency': rowData.Currency },
                                                //    datatype: "json",
                                                //    async: false,
                                                //    success: success_getunit_priceusd,
                                                //    error: error_getunit_priceusd

                                                //});

                                                //function success_getunit_priceusd(response) {

                                                //    unitpriceinusd = response;


                                                //}


                                                //function error_getunit_priceusd(response) {

                                                //    $.notify('Error - UnitPrice USD!', {
                                                //        globalPosition: "top center",
                                                //        className: "warn"
                                                //    });
                                                //} 
                                            }

                                            return unitpriceinusd;


                                        },

                                        dataType: "number",
                                        format: {type: "currency", precision: 0 },
                                        valueFormat: "#0",
                                        allowEditing: false

                                    },
                                    {
                                        dataField: "Material_Group",
                                        width: 160,
                                        validationRules: [{ type: "required" }],
                                        setCellValue: function (rowData, value) {

                                            rowData.Material_Group = value;
                                        },
                                        //editCellTemplate: tagBoxEditorTemplate_vkmrole,
                                        lookup: {
                                            dataSource: function (options) {

                                                return {

                                                    store: Material_Group_list,


                                                };
                                            },
                                            valueExpr: "ID",
                                            displayExpr: "Material_Group",
                                        //    //onCustomItemCreating: function (args) {
                                        //    //    debugger;
                                        //    //    var d = $.Deferred();
                                        //    //    debugger;
                                        //    //    var newItem = {};
                                        //    //    debugger;
                                        //    //    newItem.id = newID++;
                                        //    //    debugger;
                                        //    //    newItem.code = args.text;
                                        //    //    debugger;
                                        //    //    var result = store.createQuery().filter("code", "startswith", args.text).toArray();
                                        //    //    debugger;
                                        //    //    if (result.length) {
                                        //    //        debugger;
                                        //    //        d.resolve(result[0]);
                                        //    //    }
                                        //    //    else {
                                        //    //        debugger;
                                        //    //        store.insert(newItem);
                                        //    //        d.resolve(newItem);
                                        //    //    }
                                        //    //    debugger;
                                        //    //    return d.promise();
                                        //    //},  

                                        },
                                        //lookup: {
                                        //    dataSource: store,
                                        //    displayExpr: "Material_Group",
                                        //    valueExpr: "ID"
                                        //},
                                        editorOptions: {
                                            acceptCustomValue: true,
                                            searchEnabled: true,
                                            onCustomItemCreating: function (args) {
                                                debugger;
                                                //var d = $.Deferred();
                                                //debugger;
                                                var newMaterialGrp_list = {};
                                                //debugger;
                                                //newItem.id = 2000;
                                                //    //newID++;
                                                //debugger;
                                                //newItem.code = args.text;
                                                //debugger;
                                                $.ajax({

                                                    type: "POST",
                                                    url: "/BudgetingInventory/DynamicInsert_MaterialGroup",
                                                    data: { 'newMaterialGrpValue': args.text },

                                                    async: false,
                                                    success: onsuccess_materialdata,
                                                    error: onerror_materialdata
                                                })
                                                function onsuccess_materialdata(response) {
                                                    debugger;
                                                    newMaterialGrp_list.ID = response.data[0].ID;
                                                    newMaterialGrp_list.Material_Group = response.data[0].Material_Group;
                                                    //Material_Group_list = response.data;
                                                    $.ajax({

                                                        type: "GET",
                                                        url: "/BudgetingInventory/MaterialGroup_Lookup",
                                                        async: false,
                                                        success: onsuccess_materiallookupdata,
                                                        error: onerror_materiallookupdata
                                                    });
                                                }
                                                function onerror_materialdata() {
                                                    debugger;
                                                }
                                               
                                                debugger;
                                                //store.insert(newMaterialGrp_list);
                                                return newMaterialGrp_list;
                                                //var result = store.createQuery().filter("code", "startswith", args.text).toArray();
                                                //debugger;
                                                //if (result.length) {
                                                //    debugger;
                                                //    d.resolve(result[0]);
                                                //}
                                                //else {
                                                //    debugger;
                                                //    store.insert(newItem);
                                                //    d.resolve(newItem);
                                                //}
                                                //debugger;
                                                //return d.promise();
                                            },  
                                            //onCustomItemCreating: function (args) {
                                            //    var newItem = {};
                                            //    newItem.ID = newID++;
                                            //    newItem.Name = args.text;
                                            //    store.insert(newItem);
                                            //    return newItem;
                                            //}
                                        }
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
                                        //    debugger;
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
                                        dataField: "Order_Type",
                                        width: 150,
                                        validationRules: [{ type: "required" }],
                                        setCellValue: function (rowData, value) {

                                            rowData.Order_Type = value;
                                            rowData.UOM = null;
                                        },
                                        lookup: {
                                            dataSource: function (options) {

                                                return {

                                                    store: Order_Type_list,


                                                };
                                            },
                                            valueExpr: "ID",
                                            displayExpr: "Order_Type"

                                        },
                                    },
                                    {
                                        dataField: "UOM",
                                        width: 150,
                                        validationRules: [{ type: "required" }],
                                        setCellValue: function (rowData, value) {

                                            rowData.UOM = value;
                                        },
                                        lookup: {
                                            dataSource: function (options) {
                                                
                                                return {

                                                    store: UOM_list,
                                                    //filter: options.data ? [["BU", "=", BU_forItemFilter != 0 ? BU_forItemFilter : (options.data.BU == 1 ? 3 : (options.data.BU == 2 ? 4 : options.data.BU))], 'and', ["Deleted", "=", false]] : null
                                                    filter: options.data ? ["Order_Type", "=", options.data.Order_Type] : null

                                                };
                                            },
                                            valueExpr: "ID",
                                            displayExpr: "UOM"

                                        },
                                    },
                                    {
                                        dataField: "ActualAvailableQuantity",
                                        caption: "Available Qty",
                                        width: 140,
                                        //cellTemplate: function (container, options) {
                                        //    debugger;
                                        //    $('<a>' + options.value + '</a>')
                                        //        .attr('href', "file://bosch.com/dfsrb/DfsIN/LOC/Kor/BE-ES/ELO")
                                        //        .attr('target', '_blank')
                                        //        .appendTo(container);
                                        //}  
                                    },
                                    {
                                        dataField: "VendorCategory",
                                        caption: "Vendor",
                                        width: 110,
                                        setCellValue: function (rowData, value) {

                                            rowData.VendorCategory = value;
                                        },
                                        lookup: {
                                            dataSource: function (options) {
                                               
                                                return {

                                                    store: VendorCategory_list

                                                };
                                            },

                                            valueExpr: "ID",
                                            displayExpr: "VendorCategory"

                                        },


                                    },
                                    {
                                        dataField: "Comments"
                                    },
                                  
                                    {
                                        dataField: "Requestor",
                                        allowEditing: false,
                                        visible: false
                                    },

                                ]
                        }],
                        onInitNewRow: function (e) {
                            debugger;
                            if (new_request.VKMYear != 0)
                                e.data.VKMYear = new_request.VKMYear;
                            if (new_request.BU != 0)
                                e.data.BU = new_request.BU;
                            if (new_request.Category != 0)
                                e.data.Category = new_request.Category;
                            if (new_request.Cost_Element != 0)
                                e.data.Cost_Element = new_request.Cost_Element;
                            if (new_request.Currency != 0)
                                e.data.Currency = new_request.Currency;
                            if (new_request.VendorCategory != 0)
                                e.data.VendorCategory = new_request.VendorCategory;
                            if (new_request.BudgetCode != 0)
                                e.data.BudgetCode = new_request.BudgetCode;
                            if (new_request.Material_Group != 0)
                                e.data.Material_Group = new_request.Material_Group;
                            if (new_request.UOM != 0)
                                e.data.UOM = new_request.UOM;
                            if (new_request.Order_Type != 0)
                                e.data.Order_Type = new_request.Order_Type;

                        },

                        onRowUpdated: function (e) {
                            debugger;
                            $.notify("Row Updation in progress...Please wait!", {
                                globalPosition: "top center",
                                className: "success"
                            })
                           
                            if (e.data.Currency == 1)
                                e.data.Unit_PriceUSD = e.data.Unit_Price;
                            else if (e.data.Currency == 2)
                                e.data.Unit_PriceUSD = e.data.Unit_Price * conversionEURate;
                            else if (e.data.Currency == 3)
                                e.data.Unit_PriceUSD = e.data.Unit_Price * conversionINRate;
                            else if (e.data.Currency == 4)
                                unitpriceinusd = e.data.Unit_Price * conversionLBRate;
                            else if (e.data.Currency == 5)
                                unitpriceinusd = e.data.Unit_Price * conversionJPYRate;
                            else
                                e.data.Unit_PriceUSD = 0;

                            //$.ajax({

                            //    type: "GET",
                            //    url: "/BudgetingInventory/GetUnitPriceinUSD",
                            //    data: {'UnitPrice': e.data.Unit_Price ,'Currency': e.data.Currency },
                            //    datatype: "json",
                            //    async: false,
                            //    success: success_getunit_priceusd,
                            //    error: error_getunit_priceusd

                            //});
                            //function success_getunit_priceusd(response) {

                            //    e.data.Unit_PriceUSD = response;

                            //}

                            //function error_getunit_priceusd(response) {

                            //    $.notify('Error - UnitPrice USD!', {
                            //    globalPosition: "top center",
                            //    className: "warn"
                            //    });
                            //}


                            Selected = [];
                            Selected.push(e.data);
                            Update(Selected);
                        },

                        onRowInserting: function (e) {
                            debugger;
                            $.notify("Row Insertion in progress...Please wait!", {
                            globalPosition: "top center",
                            className: "success"
                            })
                            
                            if (e.data.Currency == 1)
                                e.data.Unit_PriceUSD = e.data.Unit_Price;
                            else if (e.data.Currency == 2)
                                e.data.Unit_PriceUSD = e.data.Unit_Price * conversionEURate;
                            else if (e.data.Currency == 3)
                                e.data.Unit_PriceUSD = e.data.Unit_Price * conversionINRate;
                            else if (e.data.Currency == 4)
                                unitpriceinusd = e.data.Unit_Price * conversionLBRate;
                            else if (e.data.Currency == 5)
                                unitpriceinusd = e.data.Unit_Price * conversionJPYRate;
                            else
                                e.data.Unit_PriceUSD = 0;
                            //$.ajax({

                            //    type: "GET",
                            //    url: "/BudgetingInventory/GetUnitPriceinUSD",
                            //    data: {'UnitPrice': e.data.Unit_Price, 'Currency': e.data.Currency },
                            //    datatype: "json",
                            //    async: false,
                            //    success: success_getunit_priceusd,
                            //    error: error_getunit_priceusd

                            //});
                            //function success_getunit_priceusd(response) {

                            //    e.data.Unit_PriceUSD = response;

                            //}

                            //function error_getunit_priceusd(response) {

                            //    $.notify('Error - UnitPrice USD!', {
                            //    globalPosition: "top center",
                            //    className: "warn"
                            //    });
                            //}
                           
                            Selected = [];
                            Selected.push(e.data);

                            Update(Selected);
                        },
                        onRowRemoving: function (e) {

                            DeleteItem(e.data.S_No);

                        }

                    }).dxDataGrid('instance').refresh();

  
                    $("#RequestTable").prop('hidden', false);
                    genSpinner_load.classList.remove('fa');
                    genSpinner_load.classList.remove('fa-spinner');
                    genSpinner_load.classList.remove('fa-pulse');
                    document.getElementById("loadpanel").style.display = "none";
                }

function OnError_GetData(response) {
    debugger;
    $.notify(response.message, {
                    globalPosition: "top center",
                    className: "warn"
                    })
                }




                $('#btnrequests').click(function () {

                    var url='/BudgetingRequest/Index';
                    window.location.href=url;
                });


                $('[data-toggle="tooltip"]').tooltip();
                $("#buttonClearFilters").dxButton({
                text: 'Clear Filters',
                    onClick: function () {
                    $("#RequestTable").dxDataGrid("clearFilter");
                    }
                });


                function Update(id1) {


                    $.ajax({
                        type: "POST",
                        url: encodeURI("../BudgetingInventory/AddOrEdit"),
                        data: { 'req': id1[0] },
                        success: function (data) {

                            $.ajax({
                                type: "GET",
                                url: "/BudgetingInventory/GetData",
                                datatype: "json",
                                async: true,
                                success: success_refresh_getdata,
                                error: error_refresh_getdata

                            });
                            function success_refresh_getdata(response) {

                                var itemlist = response;
                                $("#RequestTable").dxDataGrid({
                                    dataSource: itemlist
                                });
                            }

                            function error_refresh_getdata(response) {
                                $.notify('Unable to Refresh Item Master List right now, Please Try again later!', {
                                    globalPosition: "top center",
                                    className: "warn"
                                });
                            }

                            $.notify(data.message, {
                                globalPosition: "top center",
                                className: "success"
                            })
                        }

                    });

                }



                function DeleteItem(id) {

           
                    $.ajax({
                        type: "POST",
                        url: encodeURI("../BudgetingInventory/DeleteItem"),
                        data: { 'id': id },
                        success: function (data) {


                            $.ajax({
                                type: "GET",
                                url: "/BudgetingInventory/GetData",
                                datatype: "json",
                                async: true,
                                success: success_refresh_getdata,
                                error: error_refresh_getdata

                            });
                            function success_refresh_getdata(response) {

                                var itemlist = response;
                                $("#RequestTable").dxDataGrid({
                                    dataSource: itemlist
                                });
                            }

                            function error_refresh_getdata(response) {
                                $.notify('Unable to Refresh Item Master List right now, Please Try again later!', {
                                    globalPosition: "top center",
                                    className: "warn"
                                });
                            }


                            $.notify(data.message, {

                                globalPosition: "top center",
                                className: "success"
                             })

                    
                        }

                    });


                }


                function checkFileAPI() {
                    if (window.File && window.FileReader && window.FileList && window.Blob) {
                        reader = new FileReader();
                        return true;
                    } else {
                        alert('The File APIs are not fully supported by your browser. Fallback required.');
                        return false;
                    }
                }
                function spinnerEnable() {
                    var genSpinner = document.querySelector("#UploadSpinner");
                    genSpinner.classList.add('fa');
                    genSpinner.classList.add('fa-spinner');
                    genSpinner.classList.add('fa-pulse');
                }
                var returnedData;//this variable needs to be named the same as the parameter in the function call specified for the AjaxOptions.OnSuccess
function mySuccessFuntion(returnedData) {
                    $.ajax({ //since new material grps might have been added while uploading ; hence to refresh the list so that the new names r visible in the updated view data ; else ll show id only

                        type: "GET",
                        url: "/BudgetingInventory/MaterialGroup_Lookup",
                        async: false,
                        success: onsuccess_materiallookupdata,
                        error: onerror_materiallookupdata
                    });
                    $.ajax({
                        type: "GET",
                        url: "/BudgetingInventory/GetData",
                        datatype: "json",
                        async: true,
                        success: success_refresh_getdata,
                        error: error_refresh_getdata

                    });
                    function success_refresh_getdata(response) {
                      
                        var itemlist = response;
                        $("#RequestTable").dxDataGrid({
                            dataSource: itemlist
                        });
                    }

                    function error_refresh_getdata(response) {
                        $.notify('Unable to Refresh Item Master List right now, Please Try again later!', {
                            globalPosition: "top center",
                            className: "warn"
                        });
                    }

                    if (returnedData.errormsg) {
                   
                        $.notify(returnedData.errormsg, {

                            globalPosition: "top center",
                            autoHideDelay: 10000,
                            className: "error"
                        })
                    }
                    else {
                        $.notify(returnedData.successmsg, {

                            globalPosition: "top center",
                            autoHideDelay: 10000,
                            className: "success"
                        })

                    }
                   

                }

                function myFailureFuntion(returnedData) {
                    $.notify("Failed to import, Try again later", {

                        globalPosition: "top center",
                        className: "error"
                    })

                }


                window.addEventListener("submit", function (e) {
               
                    var form = e.target;
                    if (form.getAttribute("enctype") === "multipart/form-data") {
                        if (form.dataset.ajax) {
                            //to sync form events with request events
                            e.preventDefault();//written to block existing function -like double click on submit - incase of redundant call - only 1 submit fn should run at a time
                            e.stopImmediatePropagation();

                            //necessary for uploading files since event of uploading files should be synchronous b/w client and server though we use ajax(asynchronous call)
                            var xhr = new XMLHttpRequest();//if another request sent->refresh browser- w/o refreshing pg
                            //opening the import form
                            xhr.open(form.method, form.action);//method-POST, action-webpg; link triggers the actnresult -> returning the view to index.cshtml->renders form

                            //set the template (sending format) so that server understands how to parse it
                            xhr.setRequestHeader("x-Requested-With", "XMLHttpRequest"); // this allows 'Request.IsAjaxRequest()' to work in the controller code
                       
                            xhr.onreadystatechange = function () {
                           
                                if (xhr.readyState === XMLHttpRequest.DONE && xhr.status == 200) { //function executes once response is rx from the server
                              
                                
                                    try {
                                        returnedData = JSON.parse(xhr.responseText); //returned data to be parsed if it is a JSON object
                                   
                                    }
                                    catch (e) {
                                        returnedData = xhr.responseText;
                                    }
                                    if (form.dataset.ajaxSuccess) {
                                        eval(form.dataset.ajaxSuccess); //converts function text to real function and executes (not very safe though)

                                    }
                                    else if (form.dataset.ajaxFailure) {
                                        eval(form.dataset.ajaxFailure);
                                    }

                                    if (form.dataset.ajaxUpdate) {
                                   
                                        var genSpinner = document.querySelector("#UploadSpinner");
                                        genSpinner.classList.remove('fa');
                                        genSpinner.classList.remove('fa-spinner');
                                        genSpinner.classList.remove('fa-pulse');
                                   
                                    }
                                }
                            };

                            xhr.send(new FormData(form)); //send a request to server after importing
                        }
                    }
                }, true);

function tagBoxEditorTemplate_vkmrole(cellElement, cellInfo) {
    debugger; var maxItems = 1;
    var suspendValueChagned;
    return $('<div>').dxTagBox({
        items: Material_Group_list,
        value: cellInfo.value,
        valueExpr: 'ID',
        displayExpr: 'Material_Group',
        showSelectionControls: true,
        searchEnabled: true,
        //maxDisplayedTags: 3,
        //showMultiTagOnly: false,
        //applyValueMode: 'useButtons',
       
 
        searchEnabled: true,
        onValueChanged(e) {
            debugger;
            
            if (e.value.length > maxItems) {
                suspendValueChagned = true;
                e.component.option('values', e.previousValue);
            }
            else {
                suspendValueChagned = false;
            }
            if (suspendValueChagned) {
                suspendValueChagned = false;
                return;
            }
            else {
                cellInfo.setValue(e.value);
            }
           
        },
        onSelectionChanged() {
            debugger;
            cellInfo.component.updateDimensions();
        },
        acceptCustomValue: true,
        onCustomItemCreating(args) {
            debugger;
            const newMaterialGrpValue = args.text;
            $.ajax({

                type: "POST",
                url: "/BudgetingInventory/DynamicInsert_MaterialGroup",
                data: { 'newMaterialGrpValue': newMaterialGrpValue },

                async: false,
                success: onsuccess_materialdata,
                error: onerror_materialdata
            })
            function onsuccess_materialdata(response) {
                debugger;
                newMaterialGrp_list = response.data[0];
            }
            function onerror_materialdata() {
                debugger;
            }
            const { component } = args;
            debugger;
            const currentItems = component.option('items');
            debugger;
            currentItems.unshift(newMaterialGrp_list);
            debugger;
            component.option('items', currentItems);
            debugger;
            args.customItem = newMaterialGrp_list;
            debugger;
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

          
