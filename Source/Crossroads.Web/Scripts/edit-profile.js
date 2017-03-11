(function () {
    $('body').addClass('edit-profile-page');

    $('.choose-image input[type="file"]').change(function () {
        var inputFile = $(this);
        $('#inputFileName').text(inputFile.val());
    })

    $.validator.addMethod('date', function (value, element) {
        if (this.optional(element)) {
            return true;
        }
        var ok = true;
        try {
            $.datepicker.parseDate('dd/mm/yy', value);
        }
        catch (err) {
            ok = false;
        }
        return ok;
    });

    $("#datepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "1930:" + new Date().getFullYear(),
        monthNamesShort: ["Януари", "Февруари", "Март", "Април", "Май", "Юни", "Юли", "Август", "Септ", "Октомври", "Ноември", "Декември"],
        dayNamesMin: ["Пн.", "Вт.", "Ср.", "Чт.", "Пт.", "Сб.", "Нд."],
        dateFormat: 'dd/mm/yy'
    });

    $(".multi-select").multiselect({
        selectedText: "# от # избрани",
        noneSelectedText: "Изберете опция",
        checkAllText: 'Изберете всички',
        uncheckAllText: 'Откажете всички',
        show: ["explode", 500],
        hide: ["explode", 500],
        position: {
            my: 'left bottom',
            at: 'left top'
        }
    });

}());