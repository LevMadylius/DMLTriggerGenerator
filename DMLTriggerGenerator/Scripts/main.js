$(document).ready(function () {

    var selectedTable;

    var showInfoString = function (data) {
        var splitted = data.split(/\n/);
        $.each(splitted, function (index, value) {
            $(".modal-body").append('<p class="text-big">' + value + '</p>');
        });
    }

    $('#GenerateModal').on('hidden.bs.modal', function () {
        debugger
        $.ajax({
            url: '/Api/ClearTrackingModel/',
            type: 'post',
        });

        $("p", ".modal-content").remove();
    })

    $('#btn-Confirm').on('click', function () {
        debugger
        $.ajax({
            url: '/Api/GenerateTrackingMechanism/',
            type: 'post',
            success: function () {
                alert("Process completed");
            }
        });
    });

    var proccessInfo = function () {
        
        var tempObj = new Array();
        var arr = new Array();
        var listLis = $('#listClolumns li');

        listLis.each(function (idx, li) {
            if ($('input[type="checkbox"]:checked', li)) {
                var columnName = $('#column-title', li).text().toString();
                tempObj.push(columnName);
                $('input[type="checkbox"]:checked', li).each(function (idx, item) {
                    tempObj.push(item.value);
                });
                arr.push(tempObj);
                tempObj = new Array();

            }
        });
        $.ajax({
            url: '/Api/ProccessUserInfo/' + selectedTable,
            data: JSON.stringify(arr),
            contentType: 'application/json',
            dataType: 'json',
            type: 'post',
            complete: function () {
                $.ajax({
                    url: '/Api/GetTrackingInfo',
                    type: 'get',
                    success: function (data) {
                        debugger
                        console.log(data);
                        showInfoString(data);
                    }
                });
            }
        });
    };

    $("#GenerateTrigger").click(function () {


    });

    $('#list-tables li').on('click', function () {
        debugger
        $('#list-tables .active').removeClass("active");
        $(this).addClass("active");
        selectedTable = $("a", this).text();
    });

    $('#GenerateModal').on('show.bs.modal', function (e) {
        debugger
        proccessInfo()

        //_.defer(function () {
            
        //});
        

    });
});