var dropdown = function () {
    var loadLabTypes = function () {
        debugger;
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: "../CPC/GetLabTypes",
            dataType: 'json',
            success: function (data) {
                //debugger;
                //alert(data);
                for (var i = 0; i < data.data.length; i++) {
                    //debugger;
                    $("#selectLabType").append(
                        $('<option/>', {
                            value: data.data[i].Value,
                            html: data.data[i].Text
                        }));
                }
            }
        });
    }

    var loadLocations = function () {
        debugger;
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: "../CPC/GetLocation",
            //data: { LabTypes: LabTypes },
            dataType: 'json',
            success: function (data) {
                debugger;
                //alert(data);
                for (var i = 0; i < data.data.length; i++) {
                    debugger;
                    $("#selectLocation").append(
                        $('<option/>', {
                            value: data.data[i].Value,
                            html: data.data[i].Text
                        }));
                }
            }
        });
    }

    var loadLabIds = function (LabTypes, Locations) {
        debugger;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../LabCar/GetLabIDs",
            data: JSON.stringify({ LabTypes: LabTypes, Locations: Locations }),
            dataType: 'json',
            //traditional: true,
            success: function (data) {
                debugger;
                //alert(data);
                for (var i = 0; i < data.data.length; i++) {
                    debugger;
                    $("#selectLabId").append(
                        $('<option/>', {
                            value: data.data[i].Value,
                            html: data.data[i].Text
                        }));
                }
            }
        });
    }

    function GenerateCPC() {
        /*$('#view').on('click', function () {*/
            debugger;
            var LabTypes = $('#selectLabType').val();
            var Locations = $('#selectLocation').val();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../CPC/GenerateCPC",
                data: JSON.stringify({ LabTypes: LabTypes, Locations: Locations }),
                dataType: 'json',
                //traditional: true,
                success: function (data) {
                    debugger;
                    //alert(data);
                    if (data.data.length > 0) {
                        LoadDataGrid();
                    }
                    else {
                        $('#myModal').modal();
                    }
                }
            });
        //});
    }


    return {
        init: function () {
            loadLabTypes();
            loadLocations();
            //LabSelect();
        }
    };
}();
jQuery(document).ready(function () {
    dropdown.init();
});