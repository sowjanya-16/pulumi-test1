   
debugger;
$.ajax({
 
    type: "GET",
    url: "/TSUA/GetOemforTitle",
    datatype: "json",
    success: OnSuccess_GetOEM,
    error: OnError_GetOEM
});

function OnSuccess_GetOEM(data) {
    debugger;
    document.getElementById("heading").innerHTML = data.data + " " + "USERS";
    document.getElementById("heading").style.fontWeight = 'bold';
}
function OnError_GetOEM(data) {

}

$.ajax({
    type: "GET",
    url: "/TSUA/ProjectUsersDataGrid",
    datatype: "json",
    success: OnSuccess_GetData,
    error: OnError_GetData
});

//success function to fecth the data and use it for Data Grid
function OnSuccess_GetData(response)
{
    debugger;
    $.notify('Users list will be loaded in a while, Please wait!', {
        globalPosition: "top center",
        className: "info",
        autoHideDelay: 3000,
    });
    var objdata = response.data;
    //document.getElementById("heading").innerHTML = objdata[0].USER_OEM + " " + "USERS";
    //document.getElementById("heading").style.fontWeight = 'bold';

       $('#gridContainer').dxDataGrid
       ({
           //assign primary key as Id
           dataSource: objdata,
                        keyExpr: "Id",
                        showColumnLines: true,
                        showRowLines: true,
                        rowAlternationEnabled: true,
                        showBorders: true,
                        allowSorting: true,
                        headerFilter: {
        visible: true,
                            applyFilter: "auto"
                    },

                    columns: [{
        //for keeping the edit and delete column on the right most side
        type: "buttons",
                        width: 100,
                        alignment: "left",
                        fixed: true,
                        fixedPosition: "left",
                        buttons: [
                            "edit", "delete",
                        ],

                    },

                    {
                        alignment: "center",
                        // assign the columns to be filled with data present in object
                        columns: [
                            //{
        //    dataField: 'Id',
        //    width: 60,
        //},

        {

            dataField: 'USER_NTID',
            caption: 'OEM USER NTID',
            width: 200,
            //for capitalizing the entire word
            cellTemplate: function (container, options) {
                $("<div>")
                    .text(options.value.toUpperCase())
                    .appendTo(container);
            }

        },
        //{

        //    dataField: 'USER_OEM',
        //    width: 200,

        //},
        {

            dataField: 'USER_NAME',
            caption: 'OEM USER NAME',
            width: 300,

        },
        {

            dataField: 'PROJECT',
            width: 200,

        },
        {

            dataField: 'PROJECT_RESPONSIBLE',
            caption: 'OEM RESPONSIBLE',
            width: 200,

        },
        {

            dataField: 'LOCATION',
            width: 200,

        },

                        ],
                    }],
                    //FUNCTION FOR INSERTION OF VALUES/DATA
                    onRowInserted: function (e) {
                        debugger;
                        Selected = [];
                        Selected.push(e.data);
                        UpdateData(Selected);

                    },
                    //FUNCTION FOR UPDATION OF VALUES/DATA
                    onRowUpdated: function (e) {
                        debugger;
                        Selected = []
                        Selected.push(e.data);
                        UpdateData(Selected);
                    },

                    //FUNCTION FOR REMOVING A ROW IN A DATA
                    onRowRemoved: function (e) {
                        debugger;
                        //Selected = []
                        //Selected.Push(e.data.Id);
                        DeleteData(e.data.Id);

                    },

                    allowColumnReordering: true,
                    allowColumnResizing: true,
           columnAutoWidth: true,
                            columnFixing: {
            enabled: true
           },
           //scroll bars
           onContentReady: function (e) {
               debugger;
               e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
           },
                    scrolling: {
                       columnRenderingMode: 'virtual',
                        mode: "virtual",
                        rowRenderingMode: "virtual",
                    },
           noDataText: " ☺ No Users are currently available in your queue. ",
                    hoverStateEnabled:
                    {
        enabled: true
                    },

                    filterRow: {
        visible: true
                    },

                    selection: {
        applyFilter: "auto"
                    },
                    paging: {
        pageSize: 100
                    },


                    searchPanel: {
        visible: true,
                        width: 240,
                        placeholder: "Search..."
                    },


                    loadPanel: {
        enabled: true
                    },

                    columnChooser: {
        enabled: true
                    },

                    //columnMinWidth: 50,
                    showColumnLines: true,
                    showRowLines: true,
                    rowAlternationEnabled: true,
                    columnFixing: {
        enabled: true
                    },



                    // on providing the user NTID, populate the other fields automatically
                    onEditorPreparing: function (e) {
     		        //e.editorElement.addClass("uppercase");
                        var component = e.component,
                            rowIndex = e.row && e.row.rowIndex;
                        if (e.dataField === "USER_NTID") {
                            var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                            e.editorOptions.onValueChanged = function (e) {
        onValueChanged.call(this, e);
                                var FullName, Location, Ntid, AdminName;

                                debugger;

                                $.ajax({
                                    // ajax call to fetch the data from Action result based on ntid as input
                                    type: "post",
                                    url: "/TSUA/GetSpotOnData", //GetRequestorDetails_Planning_EMProxy
                                    data: {NTID: e.value },
                                    datatype: "json",
                                    traditional: true,
                                    success: function (data) {

                                        debugger;
                                        if (data.success) {
                                            // assgin the data coming from object to local variables
                                            FullName = data.spotOnData.EmpName;
                                            Location = data.spotOnData.EmpLocation;
                                            Ntid = data.spotOnData.EmpNTID;
                                            AdminName = data.spotOnData.AdminName;
                                            window.setTimeout(function () {
                                                component.cellValue(rowIndex, "USER_NAME", FullName);
                                                component.cellValue(rowIndex, "LOCATION", Location);
                                                component.cellValue(rowIndex, "USER_NTID", Ntid);
                                                component.cellValue(rowIndex, "PROJECT_RESPONSIBLE", AdminName);
                                            }, 1000);

                                        }
                                        else {
                                            $.notify(data.message, {
                                                globalPosition: "top center",
                                                className: "error"
                                            })
                                    }

                                        debugger;


                                    }
                                })

                                debugger;

                            }
                        }
     },

                    editing:
                    {

                        mode: 'row',
                        allowUpdating: true,
                        allowAdding: true,
                        allowDeleting: true,
                        useIcons: true,
                        repaintChangesOnly: true,
                        showBorders: true,
                        headerFilter: {
                        visible: true,
                        },

                    width: "98vw", //needed to allow horizontal scroll
                    height: "58vh",
                    columnAutoWidth: true, //needed to allow horizontal scroll - column area expanding when there are more columns instead of fixed area with conjusted columns
                    remoteOperations: true,



                    },


                }).dxDataGrid("instance").refresh();

}
function OnError_GetData(data) {
    $("#gridContainer").prop('hidden', false);
    
}




// update data and send to the action result as parameters to save new data
        function UpdateData(id1) {
            debugger;
            $.ajax({
        type: "POST",
                url: encodeURI("/TSUA/AddorEditData"),
                data: {'req': id1[0] },
                success: function (data) {
                    debugger;
                    var getdata = data.data;
                    $("#gridContainer").dxDataGrid({
        dataSource: getdata
                    });


                    if (data.success) {
        $.notify(data.message, {
            globalPosition: "top center",
            className: "success"
        })
    }
                    else {
        $.notify(data.message, {
            globalPosition: "top center",
            className: "error"
        })
    }
                }
            });
        }


// delete data and send to the action result as parameters to save data
        function DeleteData(id) {
            debugger;
            $.ajax({
        type: "POST",
                url: "/TSUA/DeleteUserData",
                data: {'Id': id },
                success: function (data) {
        newobjdata = data.data;
                    $("#gridContainer").dxDataGrid({
        dataSource: newobjdata
                    });
                    $.notify(data.message, {
        globalPostion: "top center",
                        className: "success"
                    })
                }
            });
        }




          


        


