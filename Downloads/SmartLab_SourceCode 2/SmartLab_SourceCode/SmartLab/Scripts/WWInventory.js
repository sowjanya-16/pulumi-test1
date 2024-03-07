////Javascript file for WW Inventory
var dataGridHWInventory;
var countflag = false;
var Whereabout_List, Location_List, UOM_List, PlaceType_List, LabName_List, LabType_List, ACInputType_List, HILGeneration_List;
var isWhereabout_BorrowOrRepair = false;
var isWhereabout_HIL = false;
var VisibleColumn;
var lookupFrom;

var BU_list, OEM_list, Group_list, Item_list, Item_headerFilter, Currency_list, Group_headerFilter, OEM_headerFilter, InventoryType_headerFilter;
var bmList;
var bm_data = [];



//Ajax call for getting Lookup data for dropdowns which contains information like BU,OEM
function LookUp() {
    debugger;
    $.ajax({

        type: "GET",
        url: "/WWInventory/Lookup",
        async: false,
        success: function onsuccess_lookupdata(response) {
            debugger;
            lookup_data = response.data;
            BU_list = lookup_data.BU_List;
            OEM_list = lookup_data.OEM_List;
            //DEPT_list = lookup_data.DEPT_List;
            Group_list = lookup_data.Groups_test;//Groups_oldList;
            Item_list = lookup_data.Item_List;
            Currency_list = lookup_data.Currency_List;


            Item_headerFilter = lookup_data.Item_HeaderFilter;
            Group_headerFilter = lookup_data.Group_HeaderFilter;
            OEM_headerFilter = lookup_data.OEM_HeaderFilter;
            InventoryType_headerFilter = lookup_data.InventoryType_HeaderFilter;
            SpareHW_headerFilter = lookup_data.SpareHW_HeaderFilter
            //debugger;
            //Function to send to js
            GenerateHw(BU_list, OEM_list, Group_list, Item_list, Item_headerFilter, Group_headerFilter, OEM_headerFilter, InventoryType_headerFilter);

        },
        error: function onerror_lookupdata(response) {
            //debugger;
            alert("Error lookup");

        }
    });

}
function loadVisibleColumnonGridwithTimeout() {
    setTimeout(function () {
        //debugger;
        $(document).ready(ShowGridColumns());
    }, 500);
}
function ShowGridColumns() {
    //debugger;
    var HWdataGrid = $("#HardwareTable").dxDataGrid("instance");

    //debugger;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/WWInventory/GetUserSettings",
        datatype: "json",
        data: JSON.stringify({ FormName: "HWInventory" }),
        success: function (data) {
            //debugger;
            var visibility = false;
            var response = JSON.parse(data.data);
            var num = Object.keys(response).length;
            if (num > 0) {
                HWdataGrid.beginUpdate();

                for (var i = 0; i < num; i++) {
                    if (response[i].Visibility == 1) {
                        visibility = true;
                    }
                    else {
                        visibility = false;
                    }
                    HWdataGrid.columnOption(response[i].ColumnName, 'visible', visibility);

                }
                HWdataGrid.endUpdate();
            }
        },
        error: function (jqXHR, exception) {
            //debugger;
            $.notify("Unable to get user settings! ", {
                globalPosition: "top center",
                className: "warn"
            })
        },
    });

}

$(document).ready(function () {
    //the name of uploaded file should be visible in import option placeholder once file selected
    $(".custom-file-input").on("change", function () {
        debugger;
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });


    var FormName;
    var colArray = [];
    var simpleProducts = [
        'BU_Name',
        'OEM_Name',
        'UOM_Name',
        'HW_Type_Name',
        'InventoryType_Name',
        'SerialNumber',
        'BondNumber',
        'BondDate',
        'AssetNumber',
        'Mode_Name',
        'Remarks',
        'Usage_Name',
        'Location_Name',

    ];
      for (var j = 0; j < simpleProducts.length; j++) {
        colArray.push({
            ColumnName: simpleProducts[j],
            Visibility: 1,
        });
    }
    //getVisibleColumn();
    var response = [];

    $('#userSettingIcon').on('click', function (e) {
      
        //debugger;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/WWInventory/GetColumnValues",
            datatype: "json",
            data: JSON.stringify({ FormName: "HWInventory" }),
            success: function (data) {
                //debugger;
                ////var visibility = false;
                if (data.isDBValue == 1)
                    response = JSON.parse(data.data);
                else
                    response = colArray;
                ////var num = Object.keys(response).length;
                //LoadUserSettings(response);
                $('#myUSModal').modal('show');

                //HWInvColumnList
                var divid = "#HWInvColumnList";
                $(divid).empty();
                $('<table id="tblhw"><tr><td><input type="checkbox" name ="selectall" value="-1" id="selectall"/><label for="selectall">Select All</label></td><td></td></tr></table>').appendTo(divid);

                $(divid).each(function () {
                    //debugger;
                    var tableid = $(this).find('table');

                    var num = Object.keys(response).length;
                    for (var i = 0; i < num; i++) {
                        //debugger;

                        var chkchecked1 = "";
                        var chkchecked2 = "";
                        if ((i + 2) < num && i == 0) {

                            if (response[i].Visibility == true) {
                                chkchecked1 = "checked = true";
                            }
                            else {
                                chkchecked1 = "";
                            }

                            if (response[i + 1].Visibility == true) {
                                chkchecked2 = "checked = true";
                            }
                            else {
                                chkchecked2 = "";
                            }

                            $('<tr><td><input type="checkbox" class="checkBoxClass" name ="type" value=' + response[i].ColumnName + ' id=' + response[i].ColumnName + ' ' + chkchecked1 + ' ></input > <label for=' + response[i].ColumnName + '>' + response[i].ColumnName + '</label></td > <td style="padding-left: 40px;"><input type="checkbox" name="type" class="checkBoxClass" value=' + response[i + 1].ColumnName + ' id=' + response[i + 1].ColumnName + ' ' + chkchecked2 + '></input> <label for=' + response[i + 1].ColumnName + '>' + response[i + 1].ColumnName + '</label></td></tr > ').appendTo(tableid);
                            //$('#' + response[i].ColumnName).prop('checked', response[i].Visibility).appendTo(tableid);
                        }
                        else if ((i + 2) < num) {
                            i = i + 1;

                            if (response[i].Visibility == true) {
                                chkchecked1 = "checked = true";
                            }
                            else {
                                chkchecked1 = "";
                            }

                            if (response[i + 1].Visibility == true) {
                                chkchecked2 = "checked = true";
                            }
                            else {
                                chkchecked2 = "";
                            }

                            $('<tr><td><input type="checkbox" class="checkBoxClass" name ="type" value=' + response[i].ColumnName + ' id=' + response[i].ColumnName + ' ' + chkchecked1 + '></input> <label for=' + response[i].ColumnName + '>' + response[i].ColumnName + '</label></td><td style ="padding-left: 40px;"><input type="checkbox" name ="type" class="checkBoxClass" value=' + response[i + 1].ColumnName + ' id=' + response[i + 1].ColumnName + ' ' + chkchecked2 + '></input> <label for=' + response[i + 1].ColumnName + '>' + response[i + 1].ColumnName + '</label></td></tr>').appendTo(tableid);
                        }
                        else {
                            i = i + 1;

                            if (response[i].Visibility == true) {
                                chkchecked1 = "checked = true";
                            }
                            else {
                                chkchecked1 = "";
                            }

                            $('<tr><td><input type="checkbox" class="checkBoxClass" name ="type" value=' + response[i].ColumnName + ' id=' + response[i].ColumnName + ' ' + chkchecked1 + '></input> <label for=' + response[i].ColumnName + '>' + response[i].ColumnName + '</label></td></tr>').appendTo(tableid);
                        }
                    }
                    if ($('.checkBoxClass:checked').length == $('.checkBoxClass').length) {
                        $('#selectall').prop('checked', true);
                    } else {
                        $('#selectall').prop('checked', false);
                    }
                });
                $("#selectall").on('click', function (e) {
                    //debugger;
                    $(".checkBoxClass").prop('checked', $(this).prop('checked'));
                });

                $('.checkBoxClass').on('click', function () {
                    if ($('.checkBoxClass:checked').length == $('.checkBoxClass').length) {
                        $('#selectall').prop('checked', true);
                    } else {
                        $('#selectall').prop('checked', false);
                    }
                });
            }


        });



        //$('#myUSModal').modal('show');

        ////HWInvColumnList
        //var divid = "#HWInvColumnList";
        //$(divid).empty();
        //$('<table id="tblhw"><tr><td><input type="checkbox" value="-1" id="selectall"/><label for="selectall">Select All</label></td><td></td></tr></table>').appendTo(divid);

        //$(divid).each(function () {
        //    //debugger;
        //    var tableid = $(this).find('table');

        //    var num = Object.keys(simpleProducts).length;
        //    for (var i = 0; i < num; i++) {
        //        //debugger;
        //        if ((i + 2) < num && i == 0) {
        //            $('<tr><td><input type="checkbox" class="checkBoxClass" name ="type" value=' + simpleProducts[i] + ' id=' + simpleProducts[i] + '></input> <label for=' + simpleProducts[i] + '>' + simpleProducts[i] + '</label></td><td style ="padding-left: 40px;"><input type="checkbox" name ="type" class="checkBoxClass" value=' + simpleProducts[i + 1] + ' id=' + simpleProducts[i + 1] + '></input> <label for=' + simpleProducts[i + 1] + '>' + simpleProducts[i + 1] + '</label></td></tr>').appendTo(tableid);
        //        }
        //        else if ((i + 2) < num) {
        //            i = i + 1;
        //            $('<tr><td><input type="checkbox" class="checkBoxClass" name ="type" value=' + simpleProducts[i] + ' id=' + simpleProducts[i] + '></input> <label for=' + simpleProducts[i] + '>' + simpleProducts[i] + '</label></td><td style ="padding-left: 40px;"><input type="checkbox" name ="type" class="checkBoxClass" value=' + simpleProducts[i + 1] + ' id=' + simpleProducts[i + 1] + '></input> <label for=' + simpleProducts[i + 1] + '>' + simpleProducts[i + 1] + '</label></td></tr>').appendTo(tableid);
        //        }
        //        //else {
        //        //    i = i + 1;
        //        //    $('<tr><td><input type="checkbox" class="checkBoxClass" name ="type" value=' + simpleProducts[i] + ' id=' + simpleProducts[i] + '></input> <label for=' + simpleProducts[i] + '>' + simpleProducts[i] + '</label></td></tr>').appendTo(tableid);
        //        //}
        //    }
        //});


        ////debugger;


        //$("#selectall").click(function () {
        //    //debugger;
        //    $(".checkBoxClass").prop('checked', $(this).prop('checked'));
        //});


        $('#btnSave').on('click', function (e) {
            //debugger;
            var array = [];
            $("input:checkbox[name=type]:checked").each(function () {
                array.push({
                    FormName: "HWInventory",
                    ColumnName: ($(this).val()),
                    Visibility: 1,
                });

            });

            //debugger;
            $("input:checkbox[name=type]:not(:checked)").each(function () {
                array.push({
                    FormName: "HWInventory",
                    ColumnName: ($(this).val()),
                    Visibility: 0,
                });

            });


            InsertData(array);


        });
    });


    function LoadUserSettings(response) {
        //debugger;
        $('#myUSModal').modal('show');

        //HWInvColumnList
        var divid = "#HWInvColumnList";
        $(divid).empty();
        $('<table id="tblhw"><tr><td><input type="checkbox" value="-1" id="selectall"/><label for="selectall">Select All</label></td><td></td></tr></table>').appendTo(divid);

        $(divid).each(function () {
            //debugger;
            var tableid = $(this).find('table');

            var num = Object.keys(response).length;
            for (var i = 0; i < num; i++) {
                //debugger;
                if ((i + 2) < num && i == 0) {
                    $('<tr><td><input type="checkbox" class="checkBoxClass" name ="type" value=' + response[i].ColumnName + ' id=' + response[i].ColumnName + '></input> <label for=' + response[i].ColumnName + '>' + response[i].ColumnName + '</label></td><td style ="padding-left: 40px;"><input type="checkbox" name ="type" class="checkBoxClass" value=' + response[i + 1].ColumnName + ' id=' + response[i + 1].ColumnName + '></input> <label for=' + response[i + 1].ColumnName + '>' + response[i + 1].ColumnName + '</label></td></tr>').appendTo(tableid);
                }
                else if ((i + 2) < num) {
                    i = i + 1;
                    $('<tr><td><input type="checkbox" class="checkBoxClass" name ="type" value=' + response[i].ColumnName + ' id=' + response[i].ColumnName + '></input> <label for=' + response[i].ColumnName + '>' + response[i].ColumnName + '</label></td><td style ="padding-left: 40px;"><input type="checkbox" name ="type" class="checkBoxClass" value=' + response[i + 1].ColumnName + ' id=' + response[i + 1].ColumnName + '></input> <label for=' + response[i + 1].ColumnName + '>' + response[i + 1].ColumnName + '</label></td></tr>').appendTo(tableid);
                }
                //else {
                //    i = i + 1;
                //    $('<tr><td><input type="checkbox" class="checkBoxClass" name ="type" value=' + response[i].ColumnName + ' id=' + response[i].ColumnName + '></input> <label for=' + response[i].ColumnName + '>' + response[i].ColumnName + '</label></td></tr>').appendTo(tableid);
                //}
            }
        });
    }



    function InsertData(id1) {
        //debugger;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/WWInventory/SaveUserSettings",
            datatype: "json",
            data: JSON.stringify({ items: id1 }),
            success: function (data) {
                //debugger;
                $.notify(" The details are being added...Please wait!", {
                    globalPosition: "top center",
                    className: "success"
                });

                $.notify("Saved successfully!", {
                    globalPosition: "top center",
                    className: "success"
                });

                $('#myUSModal').modal('toggle');

                LookUp();
                loadVisibleColumnonGridwithTimeout();
            },
            error: function (jqXHR, exception) {
                //debugger;
                $.notify("Unable to add the details! ", {
                    globalPosition: "top center",
                    className: "warn"
                })
            },
        });
    }






    //debugger;
    $('#tab-1').show();
    $('#tab-2').hide();
    $('#tab-3').hide();
    $('#tab-4').hide();


    $("#b2").click(function () {
        showgrid();
        $('#tab-2').show();
        $('#tab-1').hide();
        $('#tab-3').hide();
        $('#tab-4').hide();
        //$("b1").removeClass("active");
        //$("b2").addClass("active");
        $(".active-class a").removeClass("active");
        $(this).addClass("active");
        //debugger;
        //if ($(this).hasClass("active")) {
        //    // code

        //    $("#icon-add").css("display", "initial");
        //}
        //else {
        //    $("#icon-add").css("display", "none");
        //    $("#icon-hide").css("display", "initial");
        //}
    });

    //$("#b3").click(function () {
    //    $('#tab-3').show();
    //    $('#tab-1').hide();
    //    $('#tab-2').hide();
    //    $('#tab-4').hide();
    //    $(".active-class a").removeClass("active");
    //    $(this).addClass("active");
    //});

    $("#b4").click(function () {
        generate_spare(Item_list, Currency_list, SpareHW_headerFilter);
        $('#tab-4').show();
        $('#tab-1').hide();
        $('#tab-2').hide();
        $('#tab-3').hide();
        $(".active-class a").removeClass("active");
        $(this).addClass("active");


    });

    $("#b1").click(function () {
        $('#tab-1').show();
        $('#tab-2').hide();
        $('#tab-3').hide();
        $('#tab-4').hide();
        $(".active-class a").removeClass("active");
        $(this).addClass("active");
        //$("b1").removeClass("active");
        //$("b2").addClass("active");
    });

    LookUp("common");


    //debugger;
    loadVisibleColumnonGridwithTimeout();
    $("#mdlCloseBtn").click(function () {
        //document.getElementById("bookmarkStar").classList.remove("fa-star")
        //document.getElementById("bookmarkStar").classList.add("fa-star-o")
        document.getElementById("bmName").value = '';
    });
    $("#mdlCancelBtn").click(function () {
        //document.getElementById("bookmarkStar").classList.remove("fa-star")
        //document.getElementById("bookmarkStar").classList.add("fa-star-o")
        document.getElementById("bmName").value = '';
    });

    $("#bookmarkStar").click(function () {
        onBookMark();
    });
    $("#clearbookmark").click(function () {
        let dataGrids = $("#HardwareTable").dxDataGrid("instance");
        dataGrids.clearFilter();
    });
    $("#deleteBM").click(function () {
        removeBookmark(document.getElementById("addedbookmarksdd").value);
    });
    
    document.getElementById("addedbookmarksdd").onchange = function () {
       $("#HardwareTable").dxDataGrid("instance").clearFilter();
        setDefaultBookMarks(document.getElementById("addedbookmarksdd").value);
    };
    window.onkeydown = evt => {
        switch (evt.keyCode) {
            //ESC
            case 113:
                onBookMark();
                break;
            default:
                return true;
        }
        //Returning false overrides default browser event
        return false;
    };
    function onBookMark() {
        //debugger;
        document.getElementById("errorMsg").style.visibility = "hidden";
        document.getElementById("bmName").value = '';
       // document.getElementById("defaultCheckbox").checked = true;
        if ($("#HardwareTable").dxDataGrid("instance").getCombinedFilter() != undefined) {
            $('#myModal').modal('show');
            //document.getElementById("bookmarkStar").classList.remove("fa-star-o")
            //document.getElementById("bookmarkStar").classList.add("fa-star")
            getFilterDetails();

        } else {
            // alert("No filter added")
            $.notify("No filter added", {
                globalPosition: "top center",
                className: "error"
            })

        }
    }

   
});
//debugger;
//Javascript file for Hardware Inventory
function LoadBM() {
    $.ajax({

        type: "GET",
        url: "/WWInventory/GetBookMarksDetails",
        data: { 'formName': 'HW_Inventory' },
        async: false,
        success: function (response) {
            //debugger;
            bm_data = response.data;
            var bmList = [];
            for (var i = 0; i < bm_data.length; i++) {
                bmObj = { 'ID': bm_data[i].ID, 'Name': bm_data[i].BookmarkName }
                bmList.push(bmObj);
            }
            if (bm_data.length > 0) {
                document.getElementById("addedbookmarksdd").style.visibility = "visible";
                $('#addedbookmarksdd').find('option').remove();
                for (var i = 0; i < bm_data.length; i++) {
                    $('#addedbookmarksdd').append('<option value="' + bm_data[i].ID + '">' + bm_data[i].BookmarkName + "" + '</option>');
                }
            } else {
                document.getElementById("addedbookmarksdd").style.visibility = "hidden";
                $("#HardwareTable").dxDataGrid("instance").clearFilter();
                
            }
            if (bm_data.length > 0) {
                //document.getElementById("addedbookmarks").style.visibility = "visible";
                document.getElementById("deleteBM").style.visibility = "visible";
                /* bm_data.forEach((bm, i) => {
                     if (bm_data[i].DefaultValue) {
                         $("#addedbookmarks").html(bm_data[i].BookmarkName);
                     }
                 });*/

            } else {
                document.getElementById("deleteBM").style.visibility = "hidden";
            }
            setDefaultBookMarks("0");
        },
        error: function (data) {

        }
    });
}
function GenerateHw(BU_list, OEM_list, Group_list, Item_list, Item_headerFilter, Group_headerFilter, OEM_headerFilter, InventoryType_headerFilter) {
    //debugger;
    ajaxCallforHardwareUI(BU_list, OEM_list, Group_list, Item_list, Item_headerFilter, Group_headerFilter, OEM_headerFilter, InventoryType_headerFilter)

}

//Ajax calls for Inventory specific fields like Mode, Whereabout,PlaceType,LabName, Location, LabType etc
$.ajax({

    type: "GET",
    url: "/WWInventory/GetMode",
    async: false,
    success: onsuccess_getMode,
    error: onerror_getMode
})

//Success function for Mode 
function onsuccess_getMode(response) {

    lookup_data = response.data;
    Mode_List = lookup_data;
    ////debugger;

}

function onerror_getMode(response) {
    alert("Error Getting Mode Data");

}
$.ajax({

    type: "GET",
    url: "/WWInventory/GetWhereabout",
    async: false,
    success: onsuccess_getWhereabout,
    error: onerror_getWhereabout
})

function onsuccess_getWhereabout(response) {
    //debugger;
    lookup_data = response.data;
    Whereabout_List = lookup_data;
    

}

function onerror_getWhereabout(response) {
    //debugger;
    alert("Error Getting Whereabout Data");

}

$.ajax({

    type: "GET",
    url: "/WWInventory/GetLocation",
    async: false,
    success: onsuccess_getLocation,
    error: onerror_getLocation
})

function onsuccess_getLocation(response) {
    //debugger;
    lookup_data = response.data;
    Location_List = lookup_data;


}
function onerror_getLocation(response) {
    //debugger;
    alert("Error Getting Location Data");

}

$.ajax({

    type: "GET",
    url: "/WWInventory/GetUOM",
    async: false,
    success: onsuccess_getUOM,
    error: onerror_getUOM
})

function onsuccess_getUOM(response) {
    //debugger;
    lookup_data = response.data;
    UOM_List = lookup_data;


}

function onerror_getUOM(response) {
    //debugger;
    alert("Error Getting UOM Data");

}

$.ajax({

    type: "GET",
    url: "/WWInventory/GetPlaceType",
    async: false,
    success: onsuccess_getPlaceType,
    error: onerror_getPlaceType
})
 
function onsuccess_getPlaceType(response) {
    //debugger;
    lookup_data = response.data;
    PlaceType_List = lookup_data;


}
function onerror_getPlaceType(response) {
    //debugger;
    alert("Error Getting PlaceType Data");

}

$.ajax({

    type: "GET",
    url: "/WWInventory/GetLabType",
    async: false,
    success: onsuccess_getLabType,
    error: onerror_getLabType
})

function onsuccess_getLabType(response) {
    //debugger;
    lookup_data = response.data;
    LabType_List = lookup_data;


}

function onerror_getLabType(response) {
    //debugger;
    alert("Error Getting LabType Data");

}


$.ajax({

    type: "GET",
    url: "/WWInventory/GetLabNames",
    async: false,
    success: onsuccess_getLabNames,
    error: onerror_getLabNames
})

function onsuccess_getLabNames(response) {
    //debugger;
    lookup_data = response.data;
    LabName_List = lookup_data;
}

function onerror_getLabNames(response) {
    //debugger;
    alert("Error Getting LabName Data");

}


$.ajax({

    type: "GET",
    url: "/WWInventory/GetHILGeneration",
    async: false,
    success: onsuccess_getHILGeneration,
    error: onerror_getHILGeneration
})

function onsuccess_getHILGeneration(response) {
    //debugger;
    lookup_data = response.data;
    HILGeneration_List = lookup_data;


}

function onerror_getHILGeneration(response) {
    //debugger;
    alert("Error Getting HILGeneration Data");

}


$.ajax({

    type: "GET",
    url: "/WWInventory/GetACInputType",
    async: false,
    success: onsuccess_getACInputType,
    error: onerror_getACInputType
})

function onsuccess_getACInputType(response) {
    //debugger;
    lookup_data = response.data;
    ACInputType_List = lookup_data;
}

function onerror_getACInputType(response) {
    //debugger;
    alert("Error Getting ACInputType Data");

}


//Ajax call for checking the Authorization
//$.ajax({
//    type: "GET",
//    url: encodeURI("../WWInventory/checkAuth"),
//    async: false,
//    success: function (data) {
//        //debugger;
//        if (data.success) {
//            //debugger;
//            if (data.islablnventory == "1") {
//                if (data.tomodify == "1") {
//                    modflag = true;
//                }
//                else {
//                    modflag = false;
//                }
//                if (data.data == "1") {
//                    delflag = true;
//                }
//                else {
//                    delflag = false;
//                }
//            }
//            else {
//                modflag = false;
//                delflag = false;
//            }
//            //alert("Success");
//        }
//        else {
//            //debugger;

//            modflag = false;
//            delflag = false;
//            //alert("Error");
//        }
//    },
//    error: function (e) {
//        alert("Error getting data");
//    },
//});

var curryear = new Date().getFullYear();

//Ajax call to Get Hardware Inventory Data
function ajaxCallforHardwareUI(BU_list, OEM_list, Group_list, Item_list, Item_headerFilter, Group_headerFilter, OEM_headerFilter, InventoryType_headerFilter) {


    $.ajax({
        type: "GET",
        url: encodeURI("../WWInventory/GetHWData"),
        //data: { },
        success: OnSuccess_GetHardwaredata,
        error: OnError_GetHWData
    });
}
function OnSuccess_GetHardwaredata(response) {
    debugger;
    var hwdata;
    hwdata = (response.data);  

    //Assigning the hardware datalist to the object
    if (hwdata.length > 0) {
        document.getElementById("bookmarkStar").style.visibility = "visible";
        document.getElementById("clearbookmark").style.visibility = "visible";
    }
    dataGridHWInventory = $("#HardwareTable").dxDataGrid({

        dataSource: hwdata,

        //pager: {
        //    visible: true,
        //   // allowedPageSizes: [5, 10, 'all'],
        //   // showPageSizeSelector: true,
        //    showInfo: true,
        //    showNavigationButtons: true,
        //},
        loadPanel: {
            enabled: true
        },
        valueChangeEvent: 'keyup',
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
        //pager: {
        //    visible: true,
        //},
        //scrolling: {
        //    mode: "virtual",
        //    rowRenderingMode: "virtual",
        //    columnRenderingMode: "virtual"
        //},

        groupPanel: {
            visible: true,
            placeholder: "Group By Panel",
        },
        grouping: {
            autoExpandAll: true,
        },
        /*repaintChangesOnly: true,*/
        columnFixing: {
            enabled: true
        },
        columnChooser: {
            enabled: true
        },
        //assigning the variables for authorization
        editing: {
            mode: "popup",
            allowAdding: true,
            allowUpdating: true,
            allowDeleting: true,
            //allowAdding:false,


            useIcons: true,
            popup: {
                toolbarItems: [{
                    toolbar: 'bottom',
                    location: 'after',
                    widget: "dxButton",
                    options: {
                        text: "Save",
                        onClick: function (args) {
                            $("#HardwareTable").dxDataGrid('instance').saveEditData();
                        }
                    }
                }, {
                    toolbar: 'bottom',
                    location: 'after',
                    widget: "dxButton",
                    options: {
                        text: "Cancel",
                        onClick: function (args) {
                            //debugger;
                            isWhereabout_BorrowOrRepair = false;
                            isWhereabout_HIL = false;
                            var dxForm = $(".dx-form").dxForm('instance');
                            dxForm.itemOption('HIL', 'visible', isWhereabout_HIL);
                            dxForm.itemOption('Borrow_and_Repair', 'visible', isWhereabout_BorrowOrRepair);
                            $("#HardwareTable").dxDataGrid('instance').cancelEditData();

                            //your code  
                        }
                    }
                }],
                title: "Inventory Details",
                width: 900,
                height: 600,
                showTitle: true,
                visible: true,
                hideOnOutsideClick: true,
                //width: 450,
                //height: 350,
                resizeEnabled: true,
            },
            form: {
                customizeItem: function (item) {
                   
                    if (item && item.itemType === "group" && item.caption === "HIL") {
                        const editRowKey = $("#HardwareTable").dxDataGrid("instance").option('editing.editRowKey');
                        const rowIndex = $("#HardwareTable").dxDataGrid("instance").getRowIndexByKey(editRowKey);
                        var usage = $("#HardwareTable").dxDataGrid("instance").cellValue(rowIndex, "Usage");
                        
                        if (usage == 1)
                            item.visible = true;
                        else
                            item.visible = false;
                    }
                    if (item && item.itemType === "group" && item.caption === "Borrow and Repair"){
                        const editRowKey = $("#HardwareTable").dxDataGrid("instance").option('editing.editRowKey');
                        const rowIndex = $("#HardwareTable").dxDataGrid("instance").getRowIndexByKey(editRowKey);
                        var usage = $("#HardwareTable").dxDataGrid("instance").cellValue(rowIndex, "Usage");
                       
                        if ((usage == 3) || (usage== 4))
                            item.visible = true;
                        else
                            item.visible = false;
                    }
                   
                },
              
                items: [
                    {
                        itemType: 'group',
                        caption: 'Request Details',
                        colCount: 2,
                        colSpan: 2,
                        items: [

                            {
                                dataField: 'SerialNumber',

                                validationRules: [{
                                    type: "required",
                                    message: "Serial No. is required",
                                    showColonAfterLabel: true,
                                    showValidationSummary: true,
                                    validationMessageMode: 'always',
                                    validationMessagePosition: 'right',
                                }]
                            },
                            {
                                dataField: 'BU',

                                allowEditing: !countflag,
                                validationRules: [{
                                    type: "required",
                                    message: "BU is required"
                                }]

                            },

                            {
                                dataField: 'OEM',

                                validationRules: [{
                                    type: "required",
                                    message: "OEM is required"
                                }]

                                /*dataType: 'string',*/
                            },

                            {
                                dataField: 'HW_Type',
                                validationRules: [{
                                    type: "required",
                                    message: "HW Type is required"
                                }]
                                //allowEditing: false,
                            },


                            {
                                dataField: 'UOM',

                            },
                            {
                                dataField: 'BondNumber',

                            },
                            {
                                dataField: 'BondDate',
                                dataType: 'date',

                            },
                            {
                                dataField: 'AssetNumber',

                            },

                            {
                                dataField: 'Mode',

                            },
                            {
                                dataField: 'Remarks',

                            },
                            {
                                dataField: 'Usage',
                            },
                            {
                                dataField: 'Location'
                            },
                            {
                                dataField: 'InventoryType',
                                visible: false

                            },
                            //{
                            //    dataField: 'LC_Number',
                            //    // dataType: 'string',

                            //    validationRules: [{ type: "required" }],
                            //},
                            //{
                            //    dataField: 'Type',
                            //    dataType: 'string',
                            //    validationRules: [{ type: "required" }],
                            //},
                            //{
                            //    dataField: 'PCAssetNumber',
                            //    dataType: 'string',
                            //},
                            //{
                            //    dataField: 'MonitorAssetNumber1',
                            //    dataType: 'string',

                            //},
                            //{
                            //    dataField: 'MonitorAssetNumber2',
                            //    dataType: 'string',

                            //},
                            //{
                            //    dataField: 'HIL_Generation',
                            //    dataType: 'string',

                            //},

                        ],

                    },
                    {
                        itemType: 'group',
                        //visible: isWhereabout_HIL,
                        caption: 'HIL',
                        //cssClass: "DynamicField_css", //to ignore the dataField "F03" , "F05_and_F06" from being shown (reason: in general, only caption(which is displayed as a subtitle for grouped items) has to be specified for grouping items ; this caption can be used in dxForm.itemOption mtd to change the visibility. Challenge : The dxForm.itemOption mtd does not accept caption name with whitespaces (eg: "F05 and F06"). Hence, dummy dataField is provided with _ (eg: "F05_and_F06")). On using dataField along with Caption for a group of items, the dataField is also displayed as one of the items in the group, which is undesired. Hence, this has to be hidden.
                        colCount: 2,
                        colSpan: 2,
                        items: [

                            {
                                dataField: 'HIL_ID',
                               // dataType: 'string',
                            
                                validationRules: [{ type: "required" }],
                            },
                            {
                                dataField: 'Type',
                                //dataType: 'int',
                                validationRules: [{ type: "required" }],
                                allowEditing: false
                            },
                            {
                                dataField: 'PC_Asset_Number',
                                dataType: 'string',
                            },
                            {
                                dataField: 'Monitor_Asset_Number1',
                                dataType: 'string',

                            },
                            {
                                dataField: 'Monitor_Asset_Number2',
                                dataType: 'string',

                            },
                            {
                                dataField: 'HIL_Generation',
                                caption: 'ToolChain Info'
                                //dataType: 'string',

                            },
                            {
                                dataField: 'AC_InputType',
                                dataType: 'string',

                            },
                        ]
                    },
                    {
                        itemType: 'group',
                        //visible: isWhereabout_BorrowOrRepair,
                        caption: 'Borrow and Repair',
                        name: 'Borrow_and_Repair',
                        colCount: 2,
                        colSpan: 2,
                        items: [
                            {
                                dataField: 'Receiver',
                                validationRules: [{ type: "required" }],

                            },
                            {
                                dataField: 'RxDept',
                                label: { text: 'Receiver Dept/Company' },
                                dataType: 'string',
                                validationRules: [{ type: "required" }],

                            },
                            {
                                dataField: 'Start_date',
                                dataType: 'date',
                                validationRules: [{ type: "required" }],

                            },
                            {
                                dataField: 'Planned_end_date',
                                dataType: 'date',
                                validationRules: [{ type: "required" }],
                            },
                            {
                                dataField: 'End_date',
                                dataType: 'date',
                            },
                            {
                                dataField: 'Place_type',
                                validationRules: [{ type: "required" }],
                            },
                            {
                                dataField: 'Info',

                            },
                        ]
                    }

                ]
            }

        },
        onToolbarPreparing: function (e) {
            var dataGrid = e.component;

            e.toolbarOptions.items[0].showText = 'always';


        },
        width: "85vw", //needed to allow horizontal scroll
        height: "65vh",
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
        //onToolbarPreparing: function (e) {
        //    var dataGrid = e.component;

        //    e.toolbarOptions.items[0].showText = 'always';


        //},
        onEditorPreparing: function (e) {


            if (e.dataField === "OEM" && e.parentType === "dataRow") {
                e.editorOptions.disabled = !e.row.isNewRow;
            }
            if (e.dataField === "BU" && e.parentType === "dataRow") {
                e.editorOptions.disabled = !e.row.isNewRow;
            }
            if (e.dataField === "Group" && e.parentType === "dataRow") {
                e.editorOptions.disabled = !e.row.isNewRow;
            }
            if (e.dataField === "ItemName" && e.parentType === "dataRow") {
                e.editorOptions.disabled = !e.row.isNewRow;
            }
            if (e.dataField === "ItemName_Planner" && e.parentType === "dataRow") {
                e.editorOptions.disabled = !e.row.isNewRow;
            }
            if (e.dataField === "POQty" && e.parentType === "dataRow") {
                e.editorOptions.disabled = !e.row.isNewRow;
            }
            if (e.dataField === "ActualDeliveryDate" && e.parentType === "dataRow") {
                e.editorOptions.disabled = !e.row.isNewRow;
            }
            if (e.dataField === "UOM" && e.parentType === "dataRow") {
                e.editorOptions.disabled = !e.row.isNewRow;
            }

            //if (e.parentType === "dataRow") {

            //    //e.dataField.InventoryType.value = e.dataField.InventoryType2.value;
            //        var dataGridLEP1 = $("#HardwareTable").dxDataGrid("instance");
            //        dataGridLEP1.beginUpdate();
            //        dataGridLEP1.columnOption('Inventory_Type', 'visible', true);
            //        dataGridLEP1.endUpdate();
            //    }


            //var component = e.component,
            //    rowIndex = e.row && e.row.rowIndex;//new elements are positioned on the rowindex

        },

        columns: [
            {
                type: "buttons",
                width: 65,
                alignment: "left",
                fixed: true,
                fixedPosition: "left",
                buttons: [
                    "edit", "delete",
                ]
            },
            {
                visible: false,
                dataField: 'SerialNumber',
                //width: 70,
                caption: 'Serial Number',
                /*dataType: 'number',*/
                validationRules: [{
                    type: "required",
                    message: "Serial No. is required",
                    showColonAfterLabel: true,
                    showValidationSummary: true,
                    validationMessageMode: 'always',
                    validationMessagePosition: 'right',
                }]
            },
            {
                dataField: 'BU_Name',
                caption: 'BU',
            },
            {
                dataField: 'BU',
                caption: 'BU',
                lookup: {
                    dataSource: function (options) {
                        ////debugger;
                        return {

                            store: BU_list,

                        };

                    },
                    valueExpr: "ID",
                    displayExpr: "BU",

                },
                dataType: 'string',
                allowEditing: !countflag,
                validationRules: [{
                    type: "required",
                    message: "BU is required"
                }],
                visible: false

            },
            {
                dataField: 'OEM_Name',
                caption: 'OEM',
            },
            {
                dataField: 'OEM',
                caption: 'OEM',
                headerFilter: {
                    dataSource: OEM_headerFilter,
                    allowSearch: true
                },
                lookup: {
                    dataSource: function (options) {
                        ////debugger;
                        return {

                            store: OEM_list,
                        };

                    },
                    valueExpr: "ID",
                    displayExpr: "OEM"
                },
                allowEditing: !countflag,
                validationRules: [{
                    type: "required",
                    message: "OEM is required"
                }],
                visible: false

                /*dataType: 'string',*/
            },
            {
                dataField: 'HW_Type_Name',
                caption: 'HW Type',
            },
            {
                dataField: 'HW_Type',
                caption: 'HW Type',
                headerFilter: {
                    dataSource: Item_headerFilter,
                    allowSearch: true
                },

                lookup: {
                    dataSource: function (options) {
                        debugger;
                        return {
                           
                            store: Item_list,
                        }
                    },

                    valueExpr: "S_No",
                    displayExpr: "Item_Name"
                },
                dataType: 'string',
                validationRules: [{
                    type: "required",
                    message: "HW Type is required"
                }],
                visible: false
                //allowEditing: false,
            },

            {
                dataField: 'UOM_Name',
                caption: 'UOM',
            },
            {
                dataField: 'UOM',
                caption: 'UOM',
                dataType: 'string',
                lookup: {
                    dataSource: function (options) {
                        ////debugger;
                        return {

                            store: UOM_List,
                        };

                    },
                    valueExpr: "ID",
                    displayExpr: "UOM"
                },
                visible: false

                //allowEditing: false,
            },
            {
                dataField: 'BondNumber',
                //width: 70,
                caption: 'Bond Number',

                /*dataType: 'number',*/

            },
            {
                dataField: 'BondDate',
                caption: 'Bond Date',
                dataType: 'date',

            },
            {
                dataField: 'AssetNumber',
                caption: 'Asset Number',

                /*dataType: 'number',*/
            },
            {
                dataField: 'Mode_Name',
                caption: 'Mode',
            },
            {
                dataField: 'Mode',
                caption: 'Mode',
                lookup: {

                    dataSource: Mode_List,
                    displayExpr: 'Mode',
                    valueExpr: 'ID',
                },
                visible: false
                /*dataType: 'string',*/
            },
            {
                dataField: 'Remarks',
                caption: 'Remarks',
                /*dataType: 'string',*/

            },
            {
                dataField: 'Usage_Name',
                caption: 'Usage',
            },
            {
                dataField: 'Usage',
                caption: 'Usage',
                lookup: {

                    dataSource: Whereabout_List,
                    displayExpr: 'Usage',
                    valueExpr: 'ID',
                },
                validationRules: [{
                    type: "required",
                    message: "Selecting Usage is required"
                }],
                visible: false,
                setCellValue: function (rowData, value) {
                    //debugger;
                    rowData.Usage = value;
                    if (value == 1)//Fund = F03 
                    {
                        //debugger;
                        isWhereabout_HIL = true;
                        isWhereabout_BorrowOrRepair = false;
                        var dxForm = $(".dx-form").dxForm('instance');
                        dxForm.itemOption('HIL', 'visible', isWhereabout_HIL);
                        dxForm.itemOption('Borrow_and_Repair', 'visible', isWhereabout_BorrowOrRepair);
                    }
                    else if (value == 3 || value == 4) { //fund should be 5/6
                        //debugger;
                        isWhereabout_BorrowOrRepair = true;
                        isWhereabout_HIL = false;
                        var dxForm = $(".dx-form").dxForm('instance');
                        dxForm.itemOption('Borrow_and_Repair', 'visible', isWhereabout_BorrowOrRepair);
                        dxForm.itemOption('HIL', 'visible', isWhereabout_HIL);

                    }
                    else {
                        isWhereabout_BorrowOrRepair = false;
                        isWhereabout_HIL = false;
                        var dxForm = $(".dx-form").dxForm('instance');
                        dxForm.itemOption('Borrow_and_Repair', 'visible', isWhereabout_BorrowOrRepair);
                        dxForm.itemOption('HIL', 'visible', isWhereabout_HIL);
                    }
                },

                /*dataType: 'string',*/
            },
            //{
            //    dataField: 'OtherPlace_ID',
            //    //caption: 'ALM Number',

            //    /*dataType: 'number',*/
            //},
            //{
            //    dataField: 'HIL_ID',
            //    //caption: 'ALM Number',

            //    /*dataType: 'number',*/
            //},
            //{
            //    dataField: 'Diagnostics_HIL_ID',
            //    //caption: 'ALM Number',

            //    /*dataType: 'number',*/
            //},
            {
                dataField: 'Location_Name',
                caption: 'Location',
            },
            {
                dataField: 'Location',
                setCellValue: function (rowData, value) {
                    ////debugger;
                    rowData.Location = value;
                    rowData.Group = null;

                },
                lookup: {

                    dataSource: Location_List,
                    displayExpr: 'Location',
                    valueExpr: 'ID',
                },
                validationRules: [{
                    type: "required",
                    message: "Location is required"
                }],
                visible: false
                /*dataType: 'number',*/
            },

            {
                dataField: 'InventoryType',

                headerFilter: {
                    dataSource: Item_headerFilter,
                    allowSearch: true
                },

                lookup: {
                    dataSource: function (options) {
                        ////debugger;
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
                    displayExpr: "Item_Name"
                },
                dataType: 'string',
                visible: false

                //allowEditing: false,
            },
           
           
            {
                dataField: 'Receiver',
                validationRules: [{ type: "required" }],
                visible: false,
            },
            {
                dataField: 'RxDept',
                dataType: 'string',
                visible: false

            },
            {
                dataField: 'Start_date',
                dataType: 'date',
                visible: false
            },
            {
                dataField: 'Planned_end_date',
                dataType: 'date',
                visible: false,
                setCellValue: function (rowData, value, currentRowData) {

                    //if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                    //{
                    //debugger;

                    if (currentRowData.Start_date > value) {
                        $.notify(" Please enter valid Planned End Date!", {
                            globalPosition: "top center",
                            className: "error"
                        })
                        rowData.Planned_end_date = null;
                    }
                    else {
                        rowData.Planned_end_date = value;
                    }


                    // }

                },
            },
            {
                dataField: 'End_date',
                dataType: 'date',
                visible: false,
                setCellValue: function (rowData, value, currentRowData) {

                    //debugger;

                    if (currentRowData.Start_date > value) {
                        $.notify(" Please enter valid End Date!", {
                            globalPosition: "top center",
                            className: "error"
                        })
                        rowData.End_date = null;
                    }
                    else {
                        rowData.End_date = value;
                    }


                    // }

                },

            },
            {
                dataField: 'Place_type',
                validationRules: [{ type: "required" }],
                lookup: {

                    dataSource: PlaceType_List,
                    displayExpr: 'PlaceType',
                    valueExpr: 'ID',
                },
                visible: false
            },
            {
                dataField: 'Info',
                visible: false
            },

            {
                dataField: 'HIL_ID',

                lookup: {
                    dataSource: function (options) {
                        //debugger;
                        return {

                            store: LabName_List,

                            filter: options.data ? ["SiteID", "=", options.data.Location] : null
                        };

                    },
                    valueExpr: "ID",
                    displayExpr: "LabName"
                },
                dataType: 'string',
                visible: true,
                setCellValue: function (rowData, value) {
                    debugger;
                    
                    if (value.constructor.name == "Number")//then the user has selected an item. The corresponding item's details needs to be auto-set.
                    {
                        rowData.HIL_ID = value;
                        var type = LabName_List.find(x => x.ID == value).Type;
                        if (type != 0)
                            rowData.Type = type; //LabType_List.find(x => x.ID == type).LabType;
                        rowData.PC_Asset_Number = LabName_List.find(x => x.ID == value).PC_Asset_Number;
                        rowData.Monitor_Asset_Number1 = LabName_List.find(x => x.ID == value).Monitor_Asset_Number1;
                        rowData.Monitor_Asset_Number2 = LabName_List.find(x => x.ID == value).Monitor_Asset_Number2;
                        var hIL_Generation = LabName_List.find(x => x.ID == value).HIL_Generation;
                        var aC_InputType = LabName_List.find(x => x.ID == value).AC_InputType;
                        if (hIL_Generation != 0)
                            rowData.HIL_Generation = hIL_Generation;
                        if (aC_InputType != 0)
                            rowData.AC_InputType = aC_InputType;
                    }
                },

            },

            {
                dataField: 'Type',
               // dataType: 'int',
                validationRules: [{ type: "required" }],
                visible: false,
                lookup: {

                    dataSource: LabType_List,
                    displayExpr: 'LabType',
                    valueExpr: 'ID',
                },
                allowEditing: false
            },
            {
                dataField: 'PC_Asset_Number',
                dataType: 'string',
                visible: false
            },
            {
                dataField: 'Monitor_Asset_Number1',
                dataType: 'string',
                visible: false
            },
            {
                dataField: 'Monitor_Asset_Number2',
                dataType: 'string',
                visible: false
            },
            {
                dataField: 'HIL_Generation',
                caption: 'ToolChain Info',
               // dataType: 'string',
                lookup: {

                    dataSource: HILGeneration_List,
                    displayExpr: 'HILGeneration',
                    valueExpr: 'ID',
                },
                visible: false
            },
            {
                dataField: 'AC_InputType',
                dataType: 'string',
                lookup: {

                    dataSource: ACInputType_List,
                    displayExpr: 'ACInputType',
                    valueExpr: 'ID',
                },
                visible: false
            },
            {
                caption: "Logs",
                allowEditing: false,
                visible: true,

                width: 100,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<a/>').addClass('dx-link')
                        .text('View')
                        .on('dxclick', function () {
                            //debugger;

                            $.ajax({

                                type: "POST",
                                url: "/WWInventory/GetLog",
                                data: { 'HW_ID': options.data.HW_ID },
                                datatype: "json",
                                async: true,
                                success: function (data) {
                                    debugger;
                                    $("#popup").dxPopup({
                                        showTitle: true,
                                        title: "Logs",
                                        visible: true,
                                        hideOnOutsideClick: true,
                                        width: 950,
                                        height: 450,
                                        resizeEnabled: true,
                                        contentTemplate(contentElement) {
                                            $('<div />')
                                                .dxTabPanel({
                                                    items: [
                                                        {
                                                            title: "Other Place Log",
                                                            icon: "floppy",
                                                            template: function () {
                                                                //debugger;
                                                                return $("<div id='OtherPlacedatagrid'>")
                                                                    .dxDataGrid({
                                                                        width: 900,
                                                                        height: 400,
                                                                        noDataText: " ☺ This HW has not yet been repaired / borrowed !",

                                                                        dataSource: data.data,
                                                                        columns: [
                                                                            {
                                                                                dataField: 'Receiver'
                                                                            },
                                                                            {
                                                                                dataField: 'RxDept',
                                                                                caption: 'Receiver Dept/Company'
                                                                            },
                                                                            {
                                                                                dataField: 'Start_date'
                                                                            },
                                                                            {
                                                                                dataField: 'Planned_end_date'
                                                                            },
                                                                            {
                                                                                dataField: 'End_date'
                                                                            },
                                                                            {
                                                                                dataField: 'Place_type',
                                                                                lookup: {

                                                                                    dataSource: PlaceType_List,
                                                                                    displayExpr: 'PlaceType',
                                                                                    valueExpr: 'ID',
                                                                                },
                                                                            },
                                                                            {
                                                                                dataField: 'Info'
                                                                            },
                                                                            {
                                                                                dataField: 'Updated_By'
                                                                            }
                                                                        ],
                                                                    })
                                                            },
                                                        },
                                                        {
                                                            title: "HIL ",
                                                            icon: "comment",
                                                            template: function () {
                                                                //debugger;
                                                                return $("<div id='HILdatagrid'>")
                                                                    .dxDataGrid({
                                                                        width: 900,
                                                                        height: 400,
                                                                        noDataText: " ☺ This HW is not used in HIL !",

                                                                        dataSource: data.data1,
                                                                        columns: [
                                                                            {
                                                                                dataField: 'HIL_Name',
                                                                                // dataType: 'string',

                                                                                validationRules: [{ type: "required" }],
                                                                            },
                                                                            {
                                                                                dataField: 'Type',
                                                                                dataType: 'string',
                                                                                lookup: {

                                                                                    dataSource: LabType_List,
                                                                                    displayExpr: 'LabType',
                                                                                    valueExpr: 'ID',
                                                                                },
                                                                                validationRules: [{ type: "required" }],
                                                                            },
                                                                            {
                                                                                dataField: 'PC_Asset_Number',
                                                                                dataType: 'string',
                                                                            },
                                                                            {
                                                                                dataField: 'Monitor_Asset_Number1',
                                                                                dataType: 'string',

                                                                            },
                                                                            {
                                                                                dataField: 'Monitor_Asset_Number2',
                                                                                dataType: 'string',

                                                                            },
                                                                            {
                                                                                dataField: 'HIL_Generation',
                                                                                dataType: 'string',
                                                                                caption: 'ToolChain Info',
                                                                                lookup: {

                                                                                    dataSource: HILGeneration_List,
                                                                                    displayExpr: 'HILGeneration',
                                                                                    valueExpr: 'ID',
                                                                                },

                                                                            },
                                                                        ],
                                                                    })
                                                            },
                                                        },

                                                    ]
                                                })

                                                .appendTo(contentElement);
                                        },

                                    });
                                    //debugger;
                                    //$("#dxL1Details").dxDataGrid({
                                    //    dataSource: data.data,
                                    //    //keyExpr: "EmployeeID",
                                    //    visible: true,
                                    //    columns: [{
                                    //        caption:"Remarks",
                                    //        dataField: "L1Remarks"
                                    //    }, {
                                    //        caption: "Request Date",
                                    //        dataField: "RequestDate"
                                    //    }]
                                    //});

                                }
                            });

                        })
                        .appendTo(container);
                }

            },
        ],
        //summary: {

        //    groupItems: [{
        //        column: 'Quantity',
        //        summaryType: 'sum',
        //        // valueFormat: 'number',
        //        //displayFormat: 'Actual Quantity: {0}',
        //        showInGroupFooter: false,
        //        customizeText: function (e) {
        //            ////debugger;
        //            //I tried add 
        //            //console.log(e.value)
        //            return "Available Qty: " + e.value;
        //        }
        //        //alignByColumn: true,
        //        // showInHeaderFilter: true,
        //    }],
        //},
        onRowInserting: function (e) {
            //debugger;


            $.notify(" Your Hardware Inventory details is being added...Please wait!", {
                globalPosition: "top center",
                className: "success"
            })
            Selected = [];

            //e.data.InventoryType = e.data.InventoryType;
            Selected.push(e.data);
            debugger;
            UpdateHW(Selected);
            //debugger;




        },
        onRowUpdated: function (e) {
            $.notify(" Your Hardware Inventory details is being Updated...Please wait!", {
                globalPosition: "top center",
                className: "success"
            })
            Selected = [];
            //var LeadTime_tocalc_ExpReqdDt;
            //debugger;

            Selected.push(e.data);
            //debugger;
            UpdateHW(Selected);

        },
        onRowRemoving: function (e) {
            debugger;
            Delete(e.data.HW_ID);

        },
        //To add validation check if all the fields are filled or not
        onRowValidating: function (e) {
            //debugger;
            if (e.isValid == false) {
                //debugger;
                $.notify("Please fill all the necessary details", {
                    globalPosition: "top center",
                    className: "warn"
                })
            }
        }

    }).dxDataGrid('instance').refresh();
    LoadBM();
    loadVisibleColumnonGridwithTimeout();
}
//Error function for get hw data
function OnError_GetHWData() {
        //debugger;
        $("#HardwareTable").prop('hidden', false);
        $.notify("Error in Loading the page", {
            globalPosition: "top center",
            className: "warn"
        })

        //alert("Error")
}

function setDefaultBookMarks(id) {
        //debugger;
        let gridDetails = $("#HardwareTable").dxDataGrid("instance")
        gridDetails.beginUpdate();
        //console.log(bm_data);
        if (bm_data.length > 0) {
            if (id == "0") {
                bm_data.forEach((item) => {
                    if (item.DefaultValue == "True") {
                        let commaSepratorArray = item.BookmarkValue.split(",");
                       // console.log("commaSepratorArray" + commaSepratorArray)
                        if (commaSepratorArray.length > 0) {
                            commaSepratorArray.forEach((item2) => {
                                //console.log("item2" + item2)
                                let collanSepratorArray = item2.split(":");
                               // console.log("collanSepratorArray" + collanSepratorArray)

                                if (collanSepratorArray.length > 0) {
                                    //debugger;
                                    let semicollanSepratorArray = collanSepratorArray[1].split(";");
                                   // console.log("collanSepratorArray[0]" + collanSepratorArray[0])
                                   // console.log(semicollanSepratorArray)
                                    gridDetails.columnOption(collanSepratorArray[0], 'filterOperations', ['=']);
                                    gridDetails.columnOption(collanSepratorArray[0], 'filterType', 'include');
                                    gridDetails.columnOption(collanSepratorArray[0], 'filterValues', semicollanSepratorArray);
                                }



                            });
                        }
                    }
                });
            } else {
                bm_data.forEach((item) => {
                    if (item.ID == id) {
                        let commaSepratorArray = item.BookmarkValue.split(",");
                        //console.log("commaSepratorArray" + commaSepratorArray)
                        if (commaSepratorArray.length > 0) {
                            commaSepratorArray.forEach((item2) => {
                                //console.log("item2" + item2)
                                let collanSepratorArray = item2.split(":");
                                //console.log("collanSepratorArray" + collanSepratorArray)

                                if (collanSepratorArray.length > 0) {
                                    //debugger;
                                    let semicollanSepratorArray = collanSepratorArray[1].split(";");
                                    //console.log("collanSepratorArray[0]" + collanSepratorArray[0])
                                    //console.log(semicollanSepratorArray)
                                    gridDetails.columnOption(collanSepratorArray[0], 'filterOperations', ['=']);
                                    gridDetails.columnOption(collanSepratorArray[0], 'filterType', 'include');
                                    gridDetails.columnOption(collanSepratorArray[0], 'filterValues', semicollanSepratorArray);
                                }



                            });
                        }
                    }
                });
            }
        }
        gridDetails.endUpdate();

    }
function removeBookmark(idValue) {
        let text = "Do you want to remove this bookmark?";
        if (confirm(text) == true) {
            $.ajax({
                type: "POST",
                url: encodeURI("../WWInventory/DeleteBookMarks"),
                data: { 'id': idValue },
                success: function (data) {
                    $("#HardwareTable").dxDataGrid("instance").clearFilter();
                    LoadBM();
                    loadVisibleColumnonGridwithTimeout();
                    
                },
                error: function () {

                }
            });
        } else {

        }

    }

//Update function to add or edit data
function UpdateHW(id1) {
    //debugger;
    isWhereabout_BorrowOrRepair = false;
    isWhereabout_HIL = false;
    var dxForm = $(".dx-form").dxForm('instance');
    dxForm.itemOption('HIL', 'visible', isWhereabout_HIL);
    dxForm.itemOption('Borrow_and_Repair', 'visible', isWhereabout_BorrowOrRepair);
    $("#HardwareTable").dxDataGrid('instance').cancelEditData();


    //Ajax call
    $.ajax({
        type: "POST",
        url: encodeURI("../WWInventory/AddOrEdit"),
        data: { 'req': id1[0] },

        //if success, data gets refreshed internally
        success: function (data) {
            //debugger;

            //refresh data
            if (data.success) {
                $.ajax({
                    type: "GET",
                    url: encodeURI("../WWInventory/GetHWData"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    //debugger;

                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Updated the details!", {
                        globalPosition: "top center",
                        className: "success"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify('Unable to Refresh Request Items list right now, Please Try again later!', {
                        globalPosition: "top center",
                        className: "warn"
                    });

                }
   
            }
            else {
                //debugger;
                //ajax call to get data for refreshing
                $.ajax({
                    type: "GET",
                    url: encodeURI("../WWInventory/GetHWData"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    //debugger;

                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Error in updating the details!", {
                        globalPosition: "top center",
                        className: "warn"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
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

            //debugger;


        }




    });
}

//function to delete a row
function Delete(id) {
    //debugger;
    $.ajax({
        type: "POST",
        url: encodeURI("../WWInventory/Delete"),
        data: { 'id': id },
        success: function (data) {
           
            //debugger;
            if (data.success == true) {

                $.notify("Deleted Successfully!", {
                    globalPosition: "top center",
                    className: "success"
                })
                $.ajax({
                    type: "GET",
                    url: encodeURI("../WWInventory/GetHWData"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    //debugger;

                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
                        dataSource: getdata
                    });


                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
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
                    url: encodeURI("../WWInventory/GetHWData"),
                    success: success_refresh_getdata,
                    error: error_refresh_getdata

                });

                function success_refresh_getdata(response) {
                    //debugger;

                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
                        dataSource: getdata
                    });

                    $.notify("Error deleting the data, Try again!", {
                        globalPosition: "top center",
                        className: "warn"
                    })

                }
                function error_refresh_getdata(response) {



                    var getdata = response.data;
                    $("#HardwareTable").dxDataGrid({
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


var filterValue = [];
function getFilterDetails() {
        filterValue = [];
        let dataGridLEP123 = $("#HardwareTable").dxDataGrid("instance");
        var columnOptionvalue, columnOptionvalue2;

        let columnSerialNumberValue, columnBUValue, columnOEMValue, columnHWTValue,
            columnUOMValue, columnBondNumberValue, columnBondDateValue, columnAssetNumberValue,
            columnModeValue, columnRemarksValue, columnUsageValue, columnLocationValue;
        columnSerialNumberValue = "";
        columnBUValue = "";
        columnOEMValue = "";
        columnHWTValue = "";
        columnUOMValue = "";
        columnBondNumberValue = "";
        columnBondDateValue = "";
        columnAssetNumberValue = "";
        columnModeValue = "";
        columnRemarksValue = "";
        columnUsageValue = "";
        columnLocationValue = "";
        columnSerialNumberValue = dataGridLEP123.columnOption("SerialNumber", 'filterValues');
        if (columnSerialNumberValue != undefined) {
            let stringValue;
            columnSerialNumberValue.forEach((item, index) => {
                if (index == 0) {
                    stringValue = item
                } else {
                    stringValue += ";" + item;
                }
            });
            filterValue.push("SerialNumber:" + stringValue)
        }
        columnBUValue = dataGridLEP123.columnOption("BU_Name", 'filterValues');
        if (columnBUValue != undefined) {
            let stringValue;
            columnBUValue.forEach((item, index) => {
                if (index == 0) {
                    stringValue = item
                } else {
                    stringValue += ";" + item;
                }
            });
            filterValue.push("BU_Name:" + stringValue)
        }
        columnOEMValue = dataGridLEP123.columnOption("OEM_Name", 'filterValues');
        if (columnOEMValue != undefined) {
            let stringValue;
            columnOEMValue.forEach((item, index) => {
                if (index == 0) {
                    stringValue = item
                } else {
                    stringValue += ";" + item;
                }
            });
            filterValue.push("OEM_Name:" + stringValue)
        }
        columnHWTValue = dataGridLEP123.columnOption("HW_Type_Name", 'filterValues');
        if (columnHWTValue != undefined) {
            let stringValue;
            columnHWTValue.forEach((item, index) => {
                if (index == 0) {
                    stringValue = item
                } else {
                    stringValue += ";" + item;
                }
            });
            filterValue.push("HW_Type_Name:" + stringValue)
        }
        columnUOMValue = dataGridLEP123.columnOption("UOM_Name", 'filterValues');
        if (columnUOMValue != undefined) {
            let stringValue;
            columnUOMValue.forEach((item, index) => {
                if (index == 0) {
                    stringValue = item
                } else {
                    stringValue += ";" + item;
                }
            });
            filterValue.push("UOM_Name:" + stringValue)
        }
        columnBondNumberValue = dataGridLEP123.columnOption("BondNumber", 'filterValues');
        if (columnBondNumberValue != undefined) {
            let stringValue;
            columnBondNumberValue.forEach((item, index) => {
                if (index == 0) {
                    stringValue = item
                } else {
                    stringValue += ";" + item;
                }
            });
            filterValue.push("BondNumber:" + stringValue)
        }
        columnBondDateValue = dataGridLEP123.columnOption("BondDate", 'filterValues');
        if (columnBondDateValue != undefined) {
            let stringValue;
            columnBondDateValue.forEach((item, index) => {
                if (index == 0) {
                    stringValue = item
                } else {
                    stringValue += ";" + item;
                }
            });
            filterValue.push("BondDate:" + stringValue)
        }
        columnAssetNumberValue = dataGridLEP123.columnOption("AssetNumber", 'filterValues');
        if (columnAssetNumberValue != undefined) {
            let stringValue;
            columnAssetNumberValue.forEach((item, index) => {
                if (index == 0) {
                    stringValue = item
                } else {
                    stringValue += ";" + item;
                }
            });
            filterValue.push("AssetNumber:" + stringValue)
        }
        columnModeValue = dataGridLEP123.columnOption("Mode_Name", 'filterValues');
        if (columnModeValue != undefined) {
            let stringValue;
            columnModeValue.forEach((item, index) => {
                if (index == 0) {
                    stringValue = item
                } else {
                    stringValue += ";" + item;
                }
            });
            filterValue.push("Mode_Name:" + stringValue)
        }
        columnUsageValue = dataGridLEP123.columnOption("Usage_Name", 'filterValues');
        if (columnUsageValue != undefined) {
            let stringValue;
            columnUsageValue.forEach((item, index) => {
                if (index == 0) {
                    stringValue = item
                } else {
                    stringValue += ";" + item;
                }
            });
            filterValue.push("Usage_Name:" + stringValue)
        }

        columnRemarksValue = dataGridLEP123.columnOption("Remarks", 'filterValues');
        if (columnRemarksValue != undefined) {
            let stringValue;
            columnRemarksValue.forEach((item, index) => {
                if (index == 0) {
                    stringValue2 = item
                } else {
                    stringValue2 += ";" + item;
                }
            });
            filterValue.push("Remarks:" + stringValue2)
        }
        columnLocationValue = dataGridLEP123.columnOption("Location_Name", 'filterValues');
        if (columnLocationValue != undefined) {
            let stringValue;
            columnLocationValue.forEach((item, index) => {
                if (index == 0) {
                    stringValue2 = item
                } else {
                    stringValue2 += ";" + item;
                }
            });
            filterValue.push("Location_Name:" + stringValue2)
        }
        //console.log(filterValue)
        // saveBM(filterValue.toString());
    }
function saveBM() {
        if (document.getElementById("bmName").value != '' && document.getElementById("bmName").value != undefined) {
            //debugger;
            $.ajax({
                type: "POST",
                url: encodeURI("../WWInventory/AddorRemoveBookMarksDetails"),
                data: { 'details': filterValue + "", 'bmName': document.getElementById("bmName").value, 'defaultValue': $("#defaultCheckbox").is(":checked"), 'formName':"HW_Inventory" },
                success: function (response) {
                    $.notify('Your bookmark is added successfully.', {
                        globalPosition: "top center",
                        className: "success"
                    });
                    LoadBM();
                },
                error: function (response) {

                }
            })
            $('#myModal').modal('hide');
        } else {
            document.getElementById("errorMsg").style.visibility = "visible";
        }

    }

function getVisibleColumn() {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/WWInventory/GetUserSettings",
        datatype: "json",
        data: JSON.stringify({ FormName: "HWInventory" }),
        success: function (data) {
            //debugger;
            var visibility = false;
            VisibleColumn = JSON.parse(data.data);
        },
        error: function (jqXHR, exception) {
            //debugger;
            $.notify("Unable to get user settings! ", {
                globalPosition: "top center",
                className: "warn"
            })
        },
    });
}
function GetBookMarks(details) {

        //ajax call to get the data from db
        //debugger;
        $.ajax({

            type: "GET",
            url: "/WWInventory/GetBookMarksDetails",
            data: { 'details': details },
            success: onsuccess_getbookmarksdata,
            error: onerror_getbookmarksdata
        });

    }
function onsuccess_getbookmarksdata(response) { }
function onerror_getbookmarksdata(response) { }

//Import LVT & BGSW Data
function mySuccessFuntion(rdata) {

    debugger;

    if (rdata.success) {

        LookUp("common");

        if (rdata.errormsg.substr(0, 8) == "") {

            //debugger;

            $.notify('Data Uploaded Successfully', {

                globalPosition: "top center",

                className: "success"

            });
            $.ajax({
                type: "GET",
                url: encodeURI("../WWInventory/GetHWData"),
                //data: { },
                success: OnSuccess_GetHardwaredata,
                error: OnError_GetHWData
            });
        }

        else {

            $.notify(rdata.errormsg, {

                globalPosition: "top center",
                autoHideDelay: 50000,
                className: "error"

            });

            //$.notify('Data Uploaded Successfully', {

            //    globalPosition: "top center",

            //    className: "success"

            //});
            $.ajax({
                type: "GET",
                url: encodeURI("../WWInventory/GetHWData"),
                //data: { },
                success: OnSuccess_GetHardwaredata,
                error: OnError_GetHWData
            });

        }

        //debugger;




    }

    else if (rdata.success == false) {

        //debugger;

        $.notify(rdata.errormsg, {

            globalPosition: "top center",
            autoHideDelay: 50000,
            className: "warn"

        });
        $.ajax({
            type: "GET",
            url: encodeURI("../WWInventory/GetHWData"),
            //data: { },
            success: OnSuccess_GetHardwaredata,
            error: OnError_GetHWData
        });

    }

    else {

    }


    //function success_refresh_getdata(response) {

    //   //debugger;

    //    var LabList = response.data;


    //}

    //function error_refresh_getdata(response) {

    //    //error

    //}




}

function myFailureFuntion() {

    //Failure code

    $.notify('Data Uploaded Successfully', {

        globalPosition: "top center",

        className: "success"

    });

}

window.addEventListener("submit", function (e) {

    //debugger;

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

                        rdata = JSON.parse(xhr.responseText); //returned data to be parsed if it is a JSON object

                    }

                    catch (e) {

                        rdata = xhr.responseText;

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


function spinnerEnable() {

    var genSpinner = document.querySelector("#UploadSpinner");

    genSpinner.classList.add('fa');

    genSpinner.classList.add('fa-spinner');

    genSpinner.classList.add('fa-pulse');

}

