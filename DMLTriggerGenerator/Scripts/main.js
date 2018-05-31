$(document).ready(function () {

    var selectedTable;

    $("#GenerateTrigger").click(function () {
        debugger
        var val = [];

        var tempObj = new Array();
        var arr = new Array();
        var listLis = $('#listClolumns li');

        listLis.each(function (idx, li) {
            if ($('input[type="checkbox"]:checked', li))
            {
                var tableName = $('.column-title', li).text().toString();
                tempObj.push(tableName);
                $('input[type="checkbox"]:checked', li).each(function (idx, item) {
                    tempObj.push(item.value);
                });                
                arr.push(tempObj);
                tempObj = new Array();

            }                
        });
        console.log(arr);

        $.ajax({
            url: '/Api/ProccessUserInfo/' + selectedTable,
            data: JSON.stringify(arr),
            contentType: 'application/json',
            dataType: 'json',
            type: 'post' //,
           // success: onRulesSaved
        } 
   );

        $('input[type="checkbox"][name="Insert"]:checked').each(function (i) {
            val[i] = $(this).val();
        });
    });

    $('#list-tables li').on('click', function () {
        debugger
        $('#list-tables .active').removeClass("active");
        $(this).addClass("active");
        selectedTable = $("a", this).text();
    });

    $('#GenerateModal').on('show.bs.modal', function (e) {
        debugger

        $.ajax({
            url: '/Api/GetTrackingInfo',
            type: 'get',
            success: function (data) {
                console.log(data);
            }
        });

    });
});