


function generate_sp_c(Item_list)
{
    spareconfiguration(Item_list)
}


var curryear = new Date().getFullYear();


const popupc = '<div class="row">Hello</div>';


function spareconfiguration(Item_list) {
    //ajax call to get the data from db
    debugger;
    $.ajax({

        type: "GET",
        url: "/InventoryLab/GetSpareConfig",
        async: false,
        success: onsuccess_getsparecdata,
        error: onerror_getsparecdata
    })

    // success function for get data
    function onsuccess_getsparecdata(response) {
        debugger;
        sparec = (response.data);
        dataGridSpareInventory = $("#SpareConfigure").dxDataGrid({

            dataSource: sparec,
            loadPanel: {

                enabled: true
            },
            hoverStateEnabled: {
                enabled: true
            },
            columnMinWidth: 50,
            showColumnLines: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            remoteOperations: { groupPaging: true },
            searchPanel: {
                visible: true,
                highlightCaseSensitive: true,
            },
            paging: {
                pageSize: 15,
            },
            groupPanel: { visible: true },
            grouping: {
                autoExpandAll: true,
            },
            columnFixing: {
                enabled: true
            },
            columnChooser: {
                enabled: true
            },
            wordWrapEnabled: true,
            onToolbarPreparing: function (e) {
                var dataGrid = e.component;

                e.toolbarOptions.items[0].showText = 'always';


            },
            width: "97vw", //needed to allow horizontal scroll
            height: "79vh",
            columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
            columnAutoHeight: true,
            remoteOperations: false,
            showColonAfterLabel: true,
            showValidationSummary: true,
            //validationMessageMode: 'always',
            //validationMessagePosition: 'right',
            headerFilter: {
                visible: true,
                applyFilter: "auto",
                allowSearching: true
            },
            allowColumnReordering: true,
            rowAlternationEnabled: true,
            showBorders: true,
            toolbar: {
                items: [
                    'addRowButton',
                    'columnChooserButton',
                    {
                        widget: 'dxButton',

                        options: {
                            hint: "LabCar Information",
                            icon: 'preferences',
                            onClick() {
                                $("#popup").dxPopup({
                                    showTitle: true,
                                    title: "Labcar Information",
                                    visible: true,
                                    hideOnOutsideClick: true,
                                    width: 450,
                                    height: 210,
                                    resizeEnabled: true,
                                    contentTemplate: function (container) {
                                        var scrollView = $("<div id='scrollView'></div>");
                                        scrollView.append('<h5 style="text-align:center;">LabCar Count</h5>');
                                        scrollView.append('<table id="lc" style="width:100%; margin-top:1rem; text-align:center;"><tr style=" background-color: #cbdbe6;"><td colspan="2"><b>BAN</td><td><b>COB</td></td></tr><tr><td colspan="2">40</td><td>50</td></td></tr></table>');
                                        container.append(scrollView);
                                        return container;
                                    }
                                })
                            },
                        },
                    },
                    'searchPanel',
                    

                    

                ]
            },
            editing: {
                mode: 'popup',
                allowAdding: true,
                allowUpdating: true,
                allowDeleting: true,
                //allowAdding:false,
                useIcons: true,
                popup: {
                    title: "Spare Configuration Details",
                    width: 900,
                    height: 600,
                    showTitle: true,
                    visible: true,
                    hideOnOutsideClick: true,
                    //width: 450,
                    //height: 350,
                    resizeEnabled: true,
                  


                },
  



            },


            columns: [
                {
                    type: "buttons",
                    //width: 69,
                    alignment: "left",
                    fixed: true,
                    fixedPosition: "left",
                    buttons: [
                        "edit", "delete",
                    ]
                },
                {

                    dataField: 'SpareHW',
                    caption: 'Spare HW',
                
                    /* wordWrapEnabled: true,*/
                    lookup: {
                        dataSource: function (options) {
                            //debugger;
                            return {

                                store: Item_list,
                                filter: function (item) {
                                    if (item.VKM_Year == curryear) {
                                        return true;
                                    }
                                }
                            }
                        },

                        valueExpr: "S_No",
                        displayExpr: "Item_Name",
                    }
                },
                {
                    dataField: 'SpareCount',
                    caption: 'Spare Count',
                    setCellValue: function (rowData, value) {
                        rowData.SpareCount = value;
                    }
                },
                {
                    dataField: 'HWCount',
                    caption: 'HW Count',
                    setCellValue: function (rowData, value) {
                        rowData.HWCount = value;
                    }
                },
          
                {
                    dataField: 'MultiplicationFactor',
                    caption: 'Multiplication Factor',
                    setCellValue: function (rowData, value) {
                        debugger;
                        rowData.MultiplicationFactor = value;
                    }
                },
                {
                    dataField: 'SpareCalc',
                    caption: 'Spare Calculation',
                    calculateCellValue: function (rowData) {

                        debugger;

                        if (rowData.SpareCount == "" || rowData.HWCount == "" || rowData.MultiplicationFactor == "") {

                            rowData.SpareCalc = 0;
                        }
                        else {
                            rowData.SpareCalc = (rowData.SpareCount / rowData.HWCount) * rowData.MultiplicationFactor;
                        }
                       

                        return rowData.SpareCalc;
                    }
                    
                }
                

            ],
            onRowInserting: function (e) {
                debugger;

                e.data.SpareCalc = (e.data.SpareCount / e.data.HWCount) * e.data.MultiplicationFactor;
                $.notify(" Your Configuration details are being added...Please wait!", {
                    globalPosition: "top center",
                    className: "success"
                })
                Selected = [];

                //e.data.InventoryType = e.data.InventoryType;
                Selected.push(e.data);
                debugger;
                UpdateConfiguration(Selected);
               
                debugger;

            },
            onRowUpdated: function (e) {

                e.data.SpareCalc = (e.data.SpareCount / e.data.HWCount) * e.data.MultiplicationFactor;
                $.notify(" Your Configration details are being Updated...Please wait!", {
                    globalPosition: "top center",
                    className: "success"
                })
                Selected = [];
                //var LeadTime_tocalc_ExpReqdDt;
                debugger;

                Selected.push(e.data);
                debugger;

                
                UpdateConfiguration(Selected);
                AutoUpdate(e.data.SpareHW, e.data.SpareCalc)
            },
            onRowRemoving: function (e) {
                debugger;
                DeleteConfiguration(e.data.ID);

            },

        })
    }
    function onerror_getsparecdata() {
        alert("Error in get")
    }
}

function UpdateConfiguration(id1) {
    $.ajax({
        type: "POST",
        url: encodeURI("../InventoryLab/AddorEditConfiguration"),
        data: { 'req': id1[0] },

        //if success, data gets refreshed internally
        success: function (data) {
            debugger;

            //refresh data
            if (data.success) {
                $.ajax({
                    type: "GET",
                    url: encodeURI("../InventoryLab/GetSpareConfig"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    debugger;

                    var getdata = response.data;
                    $("#SpareConfigure").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Updated the details!", {
                        globalPosition: "top center",
                        className: "success"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#SpareConfigure").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }

            }
            else {
                //ajax call to get data for refreshing
                $.ajax({
                    type: "GET",
                    url: encodeURI("../InventoryLab/GetSpareConfig"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    debugger;

                    var getdata = response.data;
                    $("#SpareConfigure").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Error in updating the details!", {
                        globalPosition: "top center",
                        className: "warn"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#SpareConfigure").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }

                //setTimeout(function () { location.reload(true); }, 3000);

            }
        },

        error: function (data) {

            //InvAuth = false;
            $.notify("User is not Authorized!!", {
                globalPosition: "top center",
                className: "error"
            })

            debugger;


        }

    })

}


function DeleteConfiguration(id) {
    debugger;
    $.ajax({
        type: "POST",
        url: encodeURI("../InventoryLab/DeleteConfiguration"),
        data: { 'id': id },
        success: function (data) {
            //newobjdata = data.data;
            //$("#HardwareTable").dxDataGrid({ dataSource: newobjdata });


            debugger;
            if (data.success == true) {

                $.ajax({
                    type: "GET",
                    url: encodeURI("../InventoryLab/GetSpareConfig"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    debugger;

                    var getdata = response.data;
                    $("#SpareConfigure").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Deleted Successfully!", {
                        globalPosition: "top center",
                        className: "success"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#SpareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }
            }
            else {

                $.ajax({
                    type: "GET",
                    url: encodeURI("../InventoryLab/GetSpareConfig"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    debugger;

                    var getdata = response.data;
                    $("#SpareConfigure").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Error deleting the data, Try again!", {
                        globalPosition: "top center",
                        className: "warn"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#SpareConfigure").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }
                //setTimeout(function () { location.reload(true); }, 3000);
            }


        }

    });
    

        }
   

function AutoUpdate(sphw, spcal) {

    $.ajax({
        type: "POST",
        url: encodeURI("../InventoryLab/AutoUpdateSpare"),
        data: { sphw: sphw, spcal: spcal },

        //if success, data gets refreshed internally
        success: function (data) {
            debugger;
            //$.notify("Spare Inventory Data is also updated!", {
            //    globalPosition: "top center",
            //    className: "success"
            //})
        },
        error: function (data) {

        }
    })

}