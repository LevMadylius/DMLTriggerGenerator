$(document).ready(function () {

    var changeCheckBoxState = function (operation, checked) {
        var listInputs = $('#listClolumns li input[name=' + "\"" + operation + "\"]");
        listInputs.each(function (idx, item) {
            $(item).prop('checked', checked);
        });


    };

    $('#insertAll').on('change', function () {
        changeCheckBoxState("Insert", this.checked);

    });

    $('#updateAll').on('change', function () {
        changeCheckBoxState("Update", this.checked);

    });

    $('#deleteAll').on('change', function () {
        changeCheckBoxState("Delete", this.checked);

    });

    $('.trigger').on('change', function () {
        var self = this
        var data = {
            Name: $(this).val(),
            IsDisabled: !self.checked
        };
        var tableName = $('#list-tables .active').text();
        $.ajax({
            url: '/Api/SetTriggerState/' + tableName,
            type: 'post',
            data: data,
            success: function () {
                var temp;
                if (self.checked) {
                    temp = " enabled.";
                }
                else {
                    temp = " disabled."
                }
                alert(data.Name + temp);
            }
        });

    });



});