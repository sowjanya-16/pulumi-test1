            //Javascript file for Budgeting Request Details - mae9cob    


            var dataGridLEP, busummarytable, deptsummarytable;
            var dataObjData, newobjdata;
            var BU_list, DEPT_list, Item_list, Category_list, OEM_list, Group_list, CostElement_list, OrderStatus_list, Fund_list;
            var Selected = [];
            var unitprice, reviewer_2, category, costelement, leadtime;
            var lookup_data, new_request;
var leadtime1, prev_orderingdept;
var genSpinner_load = document.querySelector("#load");

var genSpinner_load_masterlist = document.querySelector("#load_reqlist");
var addnewitem_flag = false;
var Is_VKMSPOCFlag = false;

    

            $(".custom-file-input").on("change", function () {
                var fileName = $(this).val().split("\\").pop();
                $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
            });

IsVKMSpoc();
function IsVKMSpoc() {
    debugger;
    $.ajax({

        type: "GET",
        url: "/BudgetingLabAdmin/Get_IsVKMSpoc",
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




function spinnerEnable() {
    var genSpinner = document.querySelector("#UploadSpinner");
    genSpinner.classList.add('fa');
    genSpinner.classList.add('fa-spinner');
    genSpinner.classList.add('fa-pulse');
}

function spinnerEnable1() {
    var genSpinner = document.querySelector("#UploadSpinner1");
    genSpinner.classList.add('fa');
    genSpinner.classList.add('fa-spinner');
    genSpinner.classList.add('fa-pulse');
}
function spinnerEnableRFO() {
var genSpinner = document.querySelector("#UploadSpinnerRFO");
    genSpinner.classList.add('fa');
    genSpinner.classList.add('fa-spinner');
    genSpinner.classList.add('fa-pulse');
}

function spinnerEnableVKMRole() {
    var genSpinner = document.querySelector("#UploadSpinnerVKMRole");
    genSpinner.classList.add('fa');
    genSpinner.classList.add('fa-spinner');
    genSpinner.classList.add('fa-pulse');
}


            //Loading indicator on load of the Request module while fetching the Item Requests
            window.onload = function () {
                
                document.getElementById("loadpanel").style.display = "block";

                
                genSpinner_load.classList.add('fa');
                genSpinner_load.classList.add('fa-spinner');
                genSpinner_load.classList.add('fa-pulse');
                $("#RequestTable_VKMPlanning").prop('hidden', true);
                //$("#RequestTable_VKMOrdering").prop('hidden', true);


                var genSpinner = document.querySelector("#UploadSpinner");
                genSpinner.classList.remove('fa');
                genSpinner.classList.remove('fa-spinner');
                genSpinner.classList.remove('fa-pulse');
                checkFileAPI();
            };


$.ajax({

    type: "GET",
    url: "/BudgetingLabAdmin/Lookup",
    async: false,
    success: onsuccess_lookupdata,
    error: onerror_lookupdata
})


function onsuccess_lookupdata(response) {
    debugger;
    lookup_data = response.data;
    
    //var dpt = lookup_data.DEPT_List;
    //for (i = 0; i < dpt.length; i++) {

    //    if (dpt[i].Outdated == false) {

    //        DEPT_list.push(dpt[i]);
    //    }
    //}
    DEPT_list = lookup_data.DEPT_List;
    Group_list = lookup_data.Groups_test;
   

}

function onerror_lookupdata(response) {
    alert("Error in fetching lookup");

}

//*******************************************Request Stage**************************************//
          
                //Ajax call to Get Request Item Data
  
                $.ajax({
                    type: "GET",
                    url: encodeURI("../BudgetingLabAdmin/GetData_VKMPlanning"),
                    success: OnSuccess_GetData,
                    error: OnError_GetData
                });


function OnSuccess_GetData(response) {
   
                    var objdata = (response.data);
                   
                    
                    //Hide the Loading indicator once the Request List is fetched
                    genSpinner_load.classList.remove('fa');
                    genSpinner_load.classList.remove('fa-spinner');
                    genSpinner_load.classList.remove('fa-pulse');
                    document.getElementById("loadpanel").style.display = "none";
                    $("#RequestTable_VKMPlanning").prop('hidden', false);
                   
                    dataGridLEP = $("#RequestTable_VKMPlanning").dxDataGrid({
                       
                        dataSource: objdata,
                        keyExpr: "ID",
                        editing: {
                            mode: "row",
                           
                            allowAdding: true,
                            
                            allowUpdating: function (e) {
                                return true;
                            },
                            allowDeleting: function (e) {

                                return true;
                            },
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
                        onContentReady: function (e) {
                            debugger;
                            e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
                        },
                        
                        columnResizingMode: "nextColumn",
                        columnMinWidth: 50,
                        //stateStoring: {
                        //    enabled: true,
                        //    type: "localStorage",
                        //    storageKey: "ID"
                        //},
                        focusedRowEnabled: true,
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
                        export: {
                            enabled: true,
                            fileName: "EM List"
                        },
                        columns: [
                            {
                                type: "buttons",
                                width: 90,
                                alignment: "left",
                                buttons: [
                                    "edit", "delete"
                                    ]
                            },
                            {

                                alignment: "center",
                                columns: [
                                    {
                                        caption: "EM DETAILS",
                                        alignment: "center",
                                        columns: [

                                            
                                            {
                                                dataField: "NTID",
                                                caption: "EM NTID"
                                            },
                                            {
                                                dataField: "FullName",
                                                caption: "EM Name"
                                            },
                                            {
                                                dataField: "Department",
                                                validationRules: [{ type: "required" }],
                                                setCellValue: function (rowData, value) {
                                                    rowData.Department = value;
                                                    rowData.Group = null;

                                                },

                                                lookup: {
                                                    dataSource: function (options) {
                                                        return {

                                                            store: DEPT_list


                                                        };
                                                    },

                                                    valueExpr: "ID",
                                                    displayExpr: "DEPT"

                                                },
                                                allowEditing: true


                                            },
                                            {
                                                dataField: "Group",

                                                validationRules: [{ type: "required" }],

                                                setCellValue: function (rowData, value) {

                                                    rowData.Group = value;

                                                },
                                                lookup: {
                                                    dataSource: function (options) {
                                                        return {

                                                            store: Group_list,

                                                            filter: options.data ? ["Dept", "=", options.data.Department] : null
                                                        };

                                                    },
                                                    valueExpr: "ID",
                                                    displayExpr: "Group"
                                                },
                                                allowEditing: true

                                            },
                                            {
                                                dataField: "Proxy_NTID_EM",
                                                caption: "Proxy NTID"
                                            },
                                            {
                                                dataField: "Proxy_FullName_EM",
                                                caption: "Proxy Name"
                                            },
                                           
                                            {
                                                dataField: "Updated_By",
                                                allowEditing: false
                                            }
                                        ],



                                    }],
                            }],

                       
                        onEditorPreparing: function (e) {

                            var component = e.component,
                                rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

                            //if (e.parentType === "dataRow" && e.dataField === "Group") {

                            //    e.editorOptions.disabled = (typeof e.row.data.DEPT !== "number");
                            //    if (e.editorOptions.disabled)
                            //        e.editorOptions.placeholder = 'Select Dept first';
                            //    if (!e.editorOptions.disabled)
                            //        e.editorOptions.placeholder = 'Select Group';

                            //}

                            

                            if (e.dataField === "NTID") {
                                var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                                e.editorOptions.onValueChanged = function (e) {
                                    onValueChanged.call(this, e);
                                    var FullName, Department, Group, Ntid;

                                    debugger;

                                    $.ajax({

                                        type: "post",
                                        url: "/BudgetingLabAdmin/GetRequestorDetails_Planning", //GetRequestorDetails_Planning_EMProxy
                                        data: { NTID: e.value },
                                        datatype: "json",
                                        traditional: true,
                                        success: function (data) {
                                           
                                            debugger;
                                            if (data.success){
                                                FullName = data.data.FullName;
                                                Department = data.data.Department;
                                                Group = data.data.Group;
                                                Ntid = data.data.NTID;     
                                                window.setTimeout(function () {

                                                    component.cellValue(rowIndex, "FullName", FullName);
                                                    component.cellValue(rowIndex, "Department", Department);
                                                    component.cellValue(rowIndex, "Group", Group);
                                                    component.cellValue(rowIndex, "NTID", Ntid);
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
                                    // Emulating a web service call
                                 
                                    debugger;
                                    //$.ajax({

                                    //    type: "post",
                                    //    url: "/BudgetingLabAdmin/ValidateNTID_ifalready_exist_Planning",
                                    //    data: { NTID: e.value },
                                    //    datatype: "json",
                                    //    traditional: true,
                                    //    success: function (data) {

                                    //        debugger;
                                    //        if (data.success) {
                                    //            $.notify(data.message, {
                                    //                globalPosition: "top center",
                                    //                className: "success"
                                    //            })
                                    //        }
                                    //        else {
                                    //            $.notify(data.message, {
                                    //                globalPosition: "top center",
                                    //                className: "error"
                                    //            })
                                    //        }

                                    //        debugger;


                                    //    }
                                    //})
                                }
                            }
                            if (e.dataField === "Proxy_NTID_EM") {
                                
                                var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                                e.editorOptions.onValueChanged = function (e) {
                                    onValueChanged.call(this, e);
                                    var FullName, Ntid;

                                    debugger;

                                    $.ajax({

                                        type: "post",
                                        url: "/BudgetingLabAdmin/GetRequestorDetails_Planning_EMProxy",
                                        data: { NTID: e.value },
                                        datatype: "json",
                                        traditional: true,
                                        success: function (data) {
                                            debugger;
                                            if (data.success) {
                                                FullName = data.data.FullName;
                                                Ntid = data.data.NTID;
                                                window.setTimeout(function () {
                                                    debugger;
                                                    component.cellValue(rowIndex, "Proxy_FullName_EM", FullName);
                                                    component.cellValue(rowIndex, "Proxy_NTID_EM", Ntid);
                                                }, 1000);
                                            }
                                            else {
                                                $.notify(data.message, {
                                                    globalPosition: "top center",
                                                    className: "error"
                                                })
                                            }




                                        }
                                    })
                                    debugger;
                                    // Emulating a web service call
                                   
                                }
                            }


                           

                            


                        },
                        onRowUpdated: function (e) {
                            $.notify(" The Details are being Updated...Please wait!", {
                                globalPosition: "top center",
                                className: "success"
                            })
                            Selected = [];
                           
                                Selected.push(e.data);
                                debugger;
                                Update(Selected);
                            
                        },

                        onRowInserting: function (e) {
                           
                            Selected = [];
                            Selected.push(e.data);



                            Update(Selected);
                        },
                        onRowRemoving: function (e) {
                            debugger;
                            Delete(e.data.NTID);

                        }
                       
                    });
                    

                     
                }

                function OnError_GetData(data) {
                    $("#RequestTable_VKMPlanning").prop('hidden', false);
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "warn"
                    })
                }

            

function Update(id1) {
    debugger;

    //if (id == undefined) {
    //    $.notify('Please check the Fund and Try again later!', {
    //        globalPosition: "top center",
    //        autoHideDelay: 20000,
    //        className: "error"
    //    });
    //}
        $.ajax({
            type: "POST",
            url: encodeURI("../BudgetingLabAdmin/AddOrEdit_VKMPlanning"),
            data: { 'req': id1[0]},
            success: function (data) {
                debugger;
                //newobjdata = data.data;

                        //$("#RequestTable").dxDataGrid({dataSource: newobjdata });
                        $.ajax({
                            type: "GET",
                            url: "/BudgetingLabAdmin/GetData_VKMPlanning",
                            datatype: "json",
                            async: true,
                            success: success_refresh_getdata,
                            error: error_refresh_getdata

                        });

                        function success_refresh_getdata(response) {

                            var getdata = response.data;
                            $("#RequestTable_VKMPlanning").dxDataGrid({
                                dataSource: getdata
                            });
                        }
                        function error_refresh_getdata(response) {

                            $.notify('Unable to Refresh Planning Stage Requestor List right now, Please Try again later!', {
                                globalPosition: "top center",
                                className: "warn"
                            });

                        }

                        

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

                
function Delete(id) {
    debugger;
    $.ajax({
        type: "POST",
        url: "/BudgetingLabAdmin/Delete_VKMPlanning",
        data: { 'ntid': id},
        success: function (data) {
            newobjdata = data.data;
            $("#RequestTable_VKMPlanning").dxDataGrid({ dataSource: newobjdata });


            $.notify(data.message, {
                globalPosition: "top center",
                className: "success"
            })
        }
       


    });

    

}


var returnedData;//this variable needs to be named the same as the parameter in the function call specified for the AjaxOptions.OnSuccess
function mySuccessFuntion(returnedData) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/BudgetingLabAdmin/GetData_VKMPlanning",
        datatype: "json",
        async: true,
        success: success_refresh_getdata,
        error: error_refresh_getdata

    });
    function success_refresh_getdata(response) {
        debugger;
        var itemlist = response.data;
        $("#RequestTable_VKMPlanning").dxDataGrid({
            dataSource: itemlist
        });
    }

    function error_refresh_getdata(response) {
        $.notify('Unable to Refresh VKM Planning Requestor List right now, Please Try again later!', {
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


//planning
window.addEventListener("submit", function (e) {
    debugger;
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
                        returnedData1 = JSON.parse(xhr.responseText);
                        returnedDataRFO = JSON.parse(xhr.responseText);
                        returnedVKMData1 = JSON.parse(xhr.responseText);

                    }
                    catch (e) {
                        returnedData = xhr.responseText;
                        returnedData1 = xhr.responseText;
                        returnedVKMData1 = xhr.responseText;
                        returnedDataRFO = xhr.responseText;

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

                        var genSpinner = document.querySelector("#UploadSpinner1");
                        genSpinner.classList.remove('fa');
                        genSpinner.classList.remove('fa-spinner');
                        genSpinner.classList.remove('fa-pulse');

                        var genSpinner = document.querySelector("#UploadSpinnerRFO");
                        genSpinner.classList.remove('fa');
                        genSpinner.classList.remove('fa-spinner');
                        genSpinner.classList.remove('fa-pulse');

                        var genSpinner = document.querySelector("#UploadSpinnerVKMRole");
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



$("#buttonClearFilters").dxButton({
    text: 'Clear Filters',
    onClick: function () {
        $("#RequestTable_VKMPlanning").dxDataGrid("clearFilter");
    }
});




            //$(function () {
            //    // run the currently selected effect
            //    function runEffect() {
            //        // get effect type from
            //        var selectedEffect = "blind";

            //        var options = {};

            //        // Run the effect
            //        $("#effect").show(selectedEffect, options, 1000, callback);
            //    };

            //    function callback() {
            //        setTimeout(function () {
            //            $("#effect:visible").removeAttr("style").fadeOut();
            //        }, 60000);
            //    };

            //    // Set effect from select menu value
            //    $("#btn_summary").on("click", function () {
            //        runEffect();
            //    });

            //    $("#effect").hide();
            //});




//******************HOE*************************//

//********************************************************PLANNING STAGE - HOE***********************************//


//Ajax call to Get Request Item Data

$.ajax({
    type: "GET",
    url: encodeURI("../BudgetingLabAdmin/GetData_VKMPlanning_HOE"),
    success: OnSuccess_GetData_HOE,
    error: OnError_GetData_HOE
});


function OnSuccess_GetData_HOE(response) {
    var objdata = (response.data);


    //Hide the Loading indicator once the Request List is fetched
    genSpinner_load.classList.remove('fa');
    genSpinner_load.classList.remove('fa-spinner');
    genSpinner_load.classList.remove('fa-pulse');
    document.getElementById("loadpanel").style.display = "none";
    $("#RequestTable_HOE").prop('hidden', false);

    dataGridLEP_Order = $("#RequestTable_HOE").dxDataGrid({

        dataSource: objdata,
        editing: {
            mode: "row",

            allowAdding: true,

            allowUpdating: function (e) {
                return true;
            },
            allowDeleting: function (e) {

                return true;
            },
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
        onContentReady: function (e) {
            debugger;
            e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
        },
        keyExpr: "ID",
        focusedRowEnabled: true,
        columnResizingMode: "nextColumn",
        columnMinWidth: 50,
        //stateStoring: {
        //    enabled: true,
        //    type: "localStorage",
        //    storageKey: "RequestID"
        //},
       

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
            pageSize: 100,
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        export: {
            enabled: true,
            fileName: "HOE List"
        },


        columns: [
            {
                type: "buttons",
                width: 90,
                alignment: "left",
                buttons: [
                    "edit", "delete"
                ]
            },
            {

                alignment: "center",
                columns: [
                    {
                        caption: "HOE DETAILS",
                        alignment: "center",
                        columns: [

                            "HOE_NTID",
                            {
                                dataField: "HOE_FullName",
                                caption: "HOE Name"
                            },
                           

                            {
                                dataField: "Department",
                                validationRules: [{ type: "required" }],
                                setCellValue: function (rowData, value) {
                                    rowData.Department = value;
                                    rowData.Group = null;

                                },

                                lookup: {
                                    dataSource: function (options) {
                                        return {

                                            store: DEPT_list
                                            //filter: options.data ? ["Outdated", "=", true] : null


                                        };
                                    },

                                    valueExpr: "ID",
                                    displayExpr: "DEPT"

                                },
                                allowEditing: true


                            },
                            "Proxy_NTID",
                            {
                                dataField: "Proxy_FullName",
                                caption: "Proxy Name"
                            },
                            
                            {
                                dataField: "Updated_By",
                                allowEditing: false
                            }
                        ],



                    }],
            }],



        onEditorPreparing: function (e) {



            var component = e.component,
                rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex


            if (e.dataField === "HOE_NTID") {
                var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                e.editorOptions.onValueChanged = function (e) {
                    onValueChanged.call(this, e);
                    var FullName, Department, Ntid;



                    $.ajax({

                        type: "post",
                        url: "/BudgetingLabAdmin/GetRequestorDetails_Planning_HOE",
                        data: { NTID: e.value },
                        datatype: "json",
                        traditional: true,
                        success: function (data) {

                            if (data.success) {
                                FullName = data.data.FullName;
                                Department = data.data.Department;
                                Group = data.data.Group;
                                Ntid = data.data.NTID;
                                // Emulating a web service call
                                window.setTimeout(function () {
                                    component.cellValue(rowIndex, "HOE_FullName", FullName);
                                    component.cellValue(rowIndex, "Department", Department);
                                    component.cellValue(rowIndex, "HOE_NTID", Ntid);
                                }, 1000);

                            }
                            else {
                                $.notify(data.message, {
                                    globalPosition: "top center",
                                    className: "error"
                                })
                            }




                        }
                    })
                   
                }
            }


            if (e.dataField === "Proxy_NTID") {
                var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                e.editorOptions.onValueChanged = function (e) {
                    onValueChanged.call(this, e);
                    var FullName, Ntid;



                    $.ajax({

                        type: "post",
                        url: "/BudgetingLabAdmin/GetRequestorDetails_Planning_HOEProxy",
                        data: { NTID: e.value },
                        datatype: "json",
                        traditional: true,
                        success: function (data) {

                            if (data.success) {
                                FullName = data.data.FullName;
                                Ntid = data.data.NTID;
                                // Emulating a web service call
                                window.setTimeout(function () {
                                    component.cellValue(rowIndex, "Proxy_FullName", FullName);
                                    component.cellValue(rowIndex, "Proxy_NTID", Ntid);
                                }, 1000);

                            }
                            else {
                                $.notify(data.message, {
                                    globalPosition: "top center",
                                    className: "error"
                                })
                            }




                        }
                    })
                   
                }
            }






        },
        onRowUpdated: function (e) {
            $.notify(" The Details are being Updated...Please wait!", {
                globalPosition: "top center",
                className: "success"
            })
            Selected = [];

            Selected.push(e.data);
            Update1(Selected);

            //}

        },

        onRowInserting: function (e) {

            Selected = [];
            Selected.push(e.data);



            Update1(Selected);
        },
        onRowRemoving: function (e) {
            debugger;
            Delete1(e.data.HOE_NTID);

        }

    });



}

function OnError_GetData_HOE(data) {
    debugger;
    $("#RequestTable_HOE").prop('hidden', false);
    $.notify(data.message, {
        globalPosition: "top center",
        className: "warn"
    })
}



function Update1(id1) {
    debugger;


    $.ajax({
        type: "POST",
        url: encodeURI("../BudgetingLabAdmin/AddOrEdit_VKMPlanning_HOE"),
        data: { 'req': id1[0] },
        success: function (data) {
            debugger;
            //newobjdata = data.data;

            //$("#RequestTable").dxDataGrid({dataSource: newobjdata });
            $.ajax({
                type: "GET",
                url: "/BudgetingLabAdmin/GetData_VKMPlanning_HOE",
                datatype: "json",
                async: true,
                success: success_refresh_getdata1,
                error: error_refresh_getdata1

            });

            function success_refresh_getdata1(response) {

                var getdata = response.data;
                $("#RequestTable_HOE").dxDataGrid({
                    dataSource: getdata
                });
            }
            function error_refresh_getdata1(response) {

                $.notify('Unable to Refresh HOE List right now, Please Try again later!', {
                    globalPosition: "top center",
                    className: "warn"
                });

            }



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


function Delete1(id) {

    debugger;
    $.ajax({
        type: "POST",
        url: "/BudgetingLabAdmin/Delete_VKMPlanning_HOE",
        data: { 'ntid': id },
        success: function (data) {
            newobjdata = data.data;
            $("#RequestTable_HOE").dxDataGrid({ dataSource: newobjdata });


            $.notify(data.message, {
                globalPosition: "top center",
                className: "success"
            })
        }



    });



}



var returnedData1;//this variable needs to be named the same as the parameter in the function call specified for the AjaxOptions.OnSuccess
debugger;
function mySuccessFuntion1(returnedData1) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/BudgetingLabAdmin/GetData_VKMPlanning_HOE",
        datatype: "json",
        async: true,
        success: success_refresh_getdata1,
        error: error_refresh_getdata1

    });
    function success_refresh_getdata1(response) {
        debugger;
        var itemlist = response.data;
        $("#RequestTable_HOE").dxDataGrid({
            dataSource: itemlist
        });
    }

    function error_refresh_getdata1(response) {
        $.notify('Unable to Refresh VKM HOE List right now, Please Try again later!', {
            globalPosition: "top center",
            className: "warn"
        });
    }

    if (returnedData1.errormsg) {

        $.notify(returnedData1.errormsg, {

            globalPosition: "top center",
            autoHideDelay: 10000,
            className: "error"
        })
    }
    else {
        $.notify(returnedData1.successmsg, {

            globalPosition: "top center",
            autoHideDelay: 10000,
            className: "success"
        })

    }


}

function myFailureFuntion1(returnedData1) {
    $.notify("Failed to import, Try again later", {

        globalPosition: "top center",
        className: "error"
    })

}



$("#buttonClearFilters1").dxButton({
    text: 'Clear Filters',
    onClick: function () {
        $("#RequestTable_VKMOrdering").dxDataGrid("clearFilter");
    }
});








//********************************************************ORDERING STAGE***********************************//


////Ajax call to Get Request Item Data

$.ajax({
    type: "GET",
    url: encodeURI("../BudgetingLabAdmin/GetData_VKMOrdering"),
    success: OnSuccess_GetData_RFOOrder,
    error: OnError_GetData_RFOOrder
});


function OnSuccess_GetData_RFOOrder(response) {
    var objdata = (response.data);


    //Hide the Loading indicator once the Request List is fetched
    genSpinner_load.classList.remove('fa');
    genSpinner_load.classList.remove('fa-spinner');
    genSpinner_load.classList.remove('fa-pulse');
    document.getElementById("loadpanel").style.display = "none";
    $("#RequestTable_VKMRFO").prop('hidden', false);

    dataGridLEP_RFO = $("#RequestTable_VKMRFO").dxDataGrid({

        dataSource: objdata,
        editing: {
            mode: "row",

            allowAdding: true,

            allowUpdating: function (e) {
                return true;
            },
            allowDeleting: function (e) {

                return true; 
            },
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
            pageSize: 15
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },


        columns: [
            {
                type: "buttons",
                width: 90,
                alignment: "left",
                buttons: [
                    "edit", "delete"
                ]
            },
            {

                alignment: "center",
                columns: [
                    {
                        caption: "ORDERING STAGE REQUESTOR DETAILS",
                        alignment: "center",
                        columns: [

                            "NTID",

                            "FullName",

                            {
                                dataField: "Department",
                                validationRules: [{ type: "required" }],
                                setCellValue: function (rowData, value) {
                                    rowData.Department = value;
                                    rowData.Group = null;

                                },

                                lookup: {
                                    dataSource: function (options) {
                                        return {

                                            store: DEPT_list
                                            //filter: options.data ? ["Outdated", "=", true] : null


                                        };
                                    },

                                    valueExpr: "ID",
                                    displayExpr: "DEPT"

                                },
                                allowEditing: true


                            },
                            {
                                dataField: "Group",

                                validationRules: [{ type: "required" }],

                                setCellValue: function (rowData, value) {
                                    

                                    rowData.Group = value;

                                },
                                lookup: {
                                    dataSource: function (options) {
                                        
                                        return {

                                            store: Group_list,

                                            filter: options.data ? ["Dept", "=", options.data.Department] : null
                                        };

                                    },
                                    valueExpr: "ID",
                                    displayExpr: "Group"
                                },
                                allowEditing: true

                            },

                            {
                                dataField: "Updated_By",
                                allowEditing: false
                            }
                        ],



                    }],
            }],



        onEditorPreparing: function (e) {



            var component = e.component,
                rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

           
            if (e.dataField === "NTID") {
                var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                e.editorOptions.onValueChanged = function (e) {
                    onValueChanged.call(this, e);
                    var FullName, Department, Group, Ntid;
                    
                    //$.ajax({

                    //    type: "post",
                    //    url: "/BudgetingLabAdmin/ValidateNTID_ifalready_exist_Ordering",
                    //    data: { NTID: e.value },
                    //    datatype: "json",
                    //    traditional: true,
                    //    success: function (data) {

                    //        debugger;
                    //        if (data.success) {
                    //            $.notify(data.message, {
                    //                globalPosition: "top center",
                    //                className: "success"
                    //            })
                    //        }
                    //        else {
                    //            $.notify(data.message, {
                    //                globalPosition: "top center",
                    //                className: "error"
                    //            })
                    //        }

                    //        debugger;


                    //    }
                    //})


                    $.ajax({

                        type: "post",
                        url: "/BudgetingLabAdmin/GetRequestorDetails_Ordering",
                        data: { NTID: e.value },
                        datatype: "json",
                        traditional: true,
                        success: function (data) {
                            debugger;
                            if (data.success) {
                                FullName = data.data.FullName;
                                Department = data.data.Department;
                                Group = data.data.Group;
                                Ntid = data.data.NTID;
                                // Emulating a web service call
                                window.setTimeout(function () {
                                    debugger;
                                    component.cellValue(rowIndex, "FullName", FullName);
                                    component.cellValue(rowIndex, "Department", Department);
                                    component.cellValue(rowIndex, "Group", Group);
                                    component.cellValue(rowIndex, "NTID", Ntid);
                                }, 1000);
	
                            }
                            else {
                                $.notify(data.message, {
                                    globalPosition: "top center",
                                    className: "error"
                                })
                            }

                          


                        }
                    })
                   
                }
            }







        },
        onRowUpdated: function (e) {
            $.notify(" The Details are being Updated...Please wait!", {
                globalPosition: "top center",
                className: "success"
            })
            Selected = [];

            Selected.push(e.data);
            debugger;
            UpdateRFO(Selected);

            //}

        },

        onRowInserting: function (e) {

            Selected = [];
            Selected.push(e.data);



            UpdateRFO(Selected);
        },
        onRowRemoving: function (e) {
            debugger;
            DeleteRFO(e.data.NTID);

        }

    });



}

function OnError_GetData_RFOOrder(data) {
    debugger;
    $("#RequestTable_VKMRFO").prop('hidden', false);
    $.notify(data.message, {
        globalPosition: "top center",
        className: "warn"
    })
}



function UpdateRFO(id1) {
    debugger;


    $.ajax({
        type: "POST",
        url: encodeURI("../BudgetingLabAdmin/AddOrEdit_VKMOrdering"),
        data: { 'req': id1[0] },
        success: function (data) {
            debugger;
            //newobjdata = data.data;

            //$("#RequestTable").dxDataGrid({dataSource: newobjdata });
            $.ajax({
                type: "GET",
                url: "/BudgetingLabAdmin/GetData_VKMOrdering",
                datatype: "json",
                async: true,
                success: success_refresh_getdata1,
                error: error_refresh_getdata1

            });

            function success_refresh_getdata1(response) {

                var getdata = response.data;
                $("#RequestTable_VKMOrdering").dxDataGrid({
                    dataSource: getdata
                });
            }
            function error_refresh_getdata1(response) {

                $.notify('Unable to Refresh Ordering Stage Requestor List right now, Please Try again later!', {
                    globalPosition: "top center",
                    className: "warn"
                });

            }



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


function DeleteRFO(id) {

    debugger;
    $.ajax({
        type: "POST",
        url: "/BudgetingLabAdmin/Delete_VKMOrdering",
        data: { 'ntid': id },
        success: function (data) {
            newobjdata = data.data;
            $("#RequestTable_VKMOrdering").dxDataGrid({ dataSource: newobjdata });


            $.notify(data.message, {
                globalPosition: "top center",
                className: "success"
            })
        }



    });

    

}



var returnedDataRFO;//this variable needs to be named the same as the parameter in the function call specified for the AjaxOptions.OnSuccess
debugger;
function mySuccessFuntionRFO(returnedDataRFO) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/BudgetingLabAdmin/GetData_VKMOrdering",
        datatype: "json",
        async: true,
        success: success_refresh_getdata1,
        error: error_refresh_getdata1

    });
    function success_refresh_getdata1(response) {
        debugger;
        var itemlist = response.data;
        $("#RequestTable_VKMRFO").dxDataGrid({
            dataSource: itemlist
        });
    }

    function error_refresh_getdata1(response) {
        $.notify('Unable to Refresh VKM Ordering Requestor List right now, Please Try again later!', {
            globalPosition: "top center",
            className: "warn"
        });
    }

    if (returnedData1.errormsg) {

        $.notify(returnedData1.errormsg, {

            globalPosition: "top center",
            autoHideDelay: 10000,
            className: "error"
        })
    }
    else {
        $.notify(returnedData1.successmsg, {

            globalPosition: "top center",
            autoHideDelay: 10000,
            className: "success"
        })

    }


}

function myFailureFuntionRFO(returnedDataRFO) {
    $.notify("Failed to import, Try again later", {

        globalPosition: "top center",
        className: "error"
    })

}



$("#buttonClearFiltersRFO").dxButton({
    text: 'Clear Filters',
    onClick: function () {
        $("#RequestTable_VKMRFO").dxDataGrid("clearFilter");
    }
});


var returnedVKMData1;//this variable needs to be named the same as the parameter in the function call specified for the AjaxOptions.OnSuccess
debugger;
function mySuccessFuntion1(returnedVKMData1) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/BudgetingLabAdmin/GetData_VKMRole",
        datatype: "json",
        async: true,
        success: success_refresh_getdata1,
        error: error_refresh_getdata1

    });
    function success_refresh_getdata1(response) {
        debugger;
        var itemlist = response.data;
        $("#RequestTable_VKMRole").dxDataGrid({
            dataSource: itemlist
        });
    }

    function error_refresh_getdata1(response) {
        $.notify('Unable to Refresh VKM Role List right now, Please Try again later!', {
            globalPosition: "top center",
            className: "warn"
        });
    }

    if (returnedVKMData1.errormsg) {

        $.notify(returnedVKMData1.errormsg, {

            globalPosition: "top center",
            autoHideDelay: 10000,
            className: "error"
        })
    }
    else {
        $.notify(returnedVKMData1.successmsg, {

            globalPosition: "top center",
            autoHideDelay: 10000,
            className: "success"
        })

    }


}

function myFailureFuntion1(returnedVKMData1) {
    $.notify("Failed to import, Try again later", {

        globalPosition: "top center",
        className: "error"
    })

}


$.ajax({
    type: "GET",
    url: encodeURI("../BudgetingLabAdmin/GetData_VKMRole"),
    success: OnSuccess_GetData_VKMRole,
    error: OnError_GetData_VKMRole
});
var dataGridVKMRole;

function OnSuccess_GetData_VKMRole(response) {
    var objdata = (response.data);


    //Hide the Loading indicator once the Request List is fetched
    genSpinner_load.classList.remove('fa');
    genSpinner_load.classList.remove('fa-spinner');
    genSpinner_load.classList.remove('fa-pulse');
    document.getElementById("loadpanel").style.display = "none";
    $("#RequestTable_VKMRole").prop('hidden', false);

    dataGridVKMRole = $("#RequestTable_VKMRole").dxDataGrid({

        dataSource: objdata,
        editing: {
            mode: "row",

            //allowAdding: true,

            //allowUpdating: function (e) {
            //    return true;
            //},
            //allowDeleting: function (e) {

            //    return true;
            //},
            //useIcons: true

            allowAdding: Is_VKMSPOCFlag,

            allowUpdating: function (e) {
                return Is_VKMSPOCFlag;
            },
            allowDeleting: function (e) {

                return Is_VKMSPOCFlag;
            },
            useIcons: Is_VKMSPOCFlag
        },

        allowColumnReordering: true,
        allowColumnResizing: true,
        columnChooser: {
            enabled: true
        },
        filterRow: {
            visible: true

        },
        onContentReady: function (e) {
            debugger;
            e.element.find('.dx-datagrid-headers').toggleClass('dx-state-disabled', e.component.hasEditData());
        },
        keyExpr: "ID",
        focusedRowEnabled: true,
        columnResizingMode: "nextColumn",
        columnMinWidth: 50,
        //stateStoring: {
        //    enabled: true,
        //    type: "localStorage",
        //    storageKey: "RequestID"
        //},


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
            pageSize: 100,
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        export: {
            enabled: true,
            fileName: "VKMRole List"
        },


        columns: [
            {
                type: "buttons",
                width: 90,
                alignment: "left",
                buttons: [
                    "edit", "delete"
                ]
            },
            {

                alignment: "center",
                columns: [
                    {
                        caption: "VKM ROLE DETAILS",
                        alignment: "center",
                        columns: [

                            {
                                dataField: "ID",
                                caption: "ID",
                                visible: false

                            }, {

                                dataField: "SkillSetName",
                                caption: "Role",
                                visible: true
                            }
                          
                        ],



                    }],
            }],



        onEditorPreparing: function (e) {



            var component = e.component,
                rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex


            if (e.dataField === "SkillSetName") {
                var onValueChanged = e.editorOptions.onValueChanged;//event for BU; makes sure that the BU is modified data
                e.editorOptions.onValueChanged = function (e) {
                    onValueChanged.call(this, e);
                    var SkillSetName;



                    $.ajax({

                        type: "post",
                        url: "/BudgetingLabAdmin/GetData_VKMRole",
                        data: { ID: e.value },
                        datatype: "json",
                        traditional: true,
                        success: function (data) {

                            if (data.success) {
                                SkillSetName = data.data.SkillSetName;
                               
                                // Emulating a web service call
                                window.setTimeout(function () {
                                    component.cellValue(rowIndex, "Role", SkillSetName);
                                    
                                }, 1000);

                            }
                            else {
                                $.notify(data.message, {
                                    globalPosition: "top center",
                                    className: "error"
                                })
                            }




                        }
                    })

                }
            }


        },
        onRowUpdated: function (e) {
            $.notify(" The Details are being Updated...Please wait!", {
                globalPosition: "top center",
                className: "success"
            })
            Selected = [];

            Selected.push(e.data);
            UpdateVKMRole(Selected);

            //}

        },

        onRowInserting: function (e) {

            Selected = [];
            Selected.push(e.data);



            UpdateVKMRole(Selected);
        },
        onRowRemoving: function (e) {
            debugger;
            //DeleteVKMRole(e.data.SkillSetName);
            DeleteVKMRole(e.data.ID);

        }

    });



}

function OnError_GetData_VKMRole(data) {
    debugger;
    $("#RequestTable_VKMRole").prop('hidden', false);
    $.notify(data.message, {
        globalPosition: "top center",
        className: "warn"
    })
}



function UpdateVKMRole(id1) {
    debugger;


    $.ajax({
        type: "POST",
        url: encodeURI("../BudgetingLabAdmin/AddOrEdit_VKMRole"),
        data: { 'req': id1[0] },
        success: function (data) {
            debugger;
            //newobjdata = data.data;

            //$("#RequestTable").dxDataGrid({dataSource: newobjdata });
            $.ajax({
                type: "GET",
                url: "/BudgetingLabAdmin/GetData_VKMRole",
                datatype: "json",
                async: true,
                success: success_refresh_getdata1,
                error: error_refresh_getdata1

            });

            function success_refresh_getdata1(response) {

                var getdata = response.data;
                $("#RequestTable_VKMRole").dxDataGrid({
                    dataSource: getdata
                });
            }
            function error_refresh_getdata1(response) {

                $.notify('Unable to Refresh VKM Role List right now, Please Try again later!', {
                    globalPosition: "top center",
                    className: "warn"
                });

            }



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


function DeleteVKMRole(id) {

    debugger;
    $.ajax({
        type: "POST",
        url: "/BudgetingLabAdmin/Delete_VKMRole",
        data: { 'ID': id },
        success: function (data) {
            newobjdata = data.data;
            $("#RequestTable_VKMRole").dxDataGrid({ dataSource: newobjdata });


            $.notify(data.message, {
                globalPosition: "top center",
                className: "success"
            })
        }



    });



}

$("#buttonClearFiltersVKMRole").dxButton({
    text: 'Clear Filters',
    onClick: function () {
        $("#RequestTable_VKMRole").dxDataGrid("clearFilter");
    }
});




//ordering

//window.addEventListener("submit", function (e) {
//    debugger;
//    var form = e.target;
//    if (form.getAttribute("enctype") === "multipart/form-data") {
//        if (form.dataset.ajax) {
//            //to sync form events with request events
//            e.preventDefault();//written to block existing function -like double click on submit - incase of redundant call - only 1 submit fn should run at a time
//            e.stopImmediatePropagation();

//            //necessary for uploading files since event of uploading files should be synchronous b/w client and server though we use ajax(asynchronous call)
//            var xhr = new XMLHttpRequest();//if another request sent->refresh browser- w/o refreshing pg
//            //opening the import form  
//            xhr.open(form.method, form.action);//method-POST, action-webpg; link triggers the actnresult -> returning the view to index.cshtml->renders form

//            //set the template (sending format) so that server understands how to parse it
//            xhr.setRequestHeader("x-Requested-With", "XMLHttpRequest"); // this allows 'Request.IsAjaxRequest()' to work in the controller code

//            xhr.onreadystatechange = function () {

//                if (xhr.readyState === XMLHttpRequest.DONE && xhr.status == 200) { //function executes once response is rx from the server


//                    try {
//                        returnedData1 = JSON.parse(xhr.responseText); //returned data to be parsed if it is a JSON object

//                    }
//                    catch (e) {
//                        returnedData1 = xhr.responseText;
//                    }
//                    if (form.dataset.ajaxSuccess) {
//                        eval(form.dataset.ajaxSuccess); //converts function text to real function and executes (not very safe though)

//                    }
//                    else if (form.dataset.ajaxFailure) {
//                        eval(form.dataset.ajaxFailure);
//                    }

//                    if (form.dataset.ajaxUpdate) {

//                        var genSpinner = document.querySelector("#UploadSpinner");
//                        genSpinner.classList.remove('fa');
//                        genSpinner.classList.remove('fa-spinner');
//                        genSpinner.classList.remove('fa-pulse');

//                    }
//                }
//            };

//            xhr.send(new FormData(form)); //send a request to server after importing
//        }
//    }
//}, true);










//*******************COMMON***********************//

           
$('[data-toggle="tooltip"]').tooltip();


function checkFileAPI() {
    if (window.File && window.FileReader && window.FileList && window.Blob) {
        reader = new FileReader();
        return true;
    } else {
        alert('The File APIs are not fully supported by your browser. Fallback required.');
        return false;
    }
}



            ////Export data
            //$("#export").click(function () {
            //    debugger;
            //    $.ajax({

            //        type: "POST",
            //        url: "/BudgetingLabAdmin/ExportDataToExcel/",
            //        data: {'useryear': filtered_yr },


            //        success: function (export_result) {
            //            debugger;

            //            var bytes = new Uint8Array(export_result.FileContents);
            //            var blob = new Blob([bytes], {type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            //            var link = document.createElement('a');
            //            link.href = window.URL.createObjectURL(blob);
            //            link.download = export_result.FileDownloadName ;
            //            link.click();

            //        },
            //        error: function () {
            //            alert("export error");
            //        }

            //    });
            //});





