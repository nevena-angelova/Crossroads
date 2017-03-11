(function () {
    $('body').addClass('profiles-page');

    $('.filter-input').on('input', function () {
        var val = $(this).val();
        $('.filter-input').val('');
        $(this).val(val);
        var formId = '#' + $(this).attr('data-form-id');
        $(formId).submit();
    });

}());

var Index = (function () {
    
    function filterShow() {
        $('#profilesSearchFilter').show('slow');

        $('#selectTown option:selected').removeAttr('selected');
        $('.multi-select option:selected').removeAttr('selected');

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
    }

    function filterHide() {
        $('#profilesSearchFilter').hide('slow');
    }

    return {
        filterShow: function () {
            return filterShow();
        },
        filterHide: function () {
            return filterHide();
        }
    }
}());