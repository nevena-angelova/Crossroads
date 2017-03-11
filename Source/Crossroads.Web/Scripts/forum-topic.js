(function () {
    $('body').addClass('topic-details-page');

    $(document).ajaxError(function (e, xhr) {
        if (xhr.status == 403) {
            var response = $.parseJSON(xhr.responseText);
            window.location = response.LogOnUrl;
        }
    });

    $(document).on('click', '.vote-error a', function () {
        $(this).parents('.vote-error').fadeOut();
    });

}());

var DisplayTopic = (function () {

    function formShow(id) {
        $(id).show('slow');
    }

    function formHide(id) {
        $(id).hide('slow');
    }

    function editedOnSuccess(id) {
        $(id).hide();
        $(id).show('slide', { direction: 'right' }, 'slow');
    }

    function onFlagBtnClick(id) {
        var flagBtn = $(id);
        if (flagBtn.text() == 'Flag') {
            flagBtn.text('Unflag');
        }
        else {
            flagBtn.text('Flag');
        }
    }

    return {
        formShow: function (id) {
            return formShow(id);
        },
        formHide: function (id) {
            return formHide(id);
        },
        editedOnSuccess: function (id) {
            return editedOnSuccess(id);
        },
        onFlagBtnClick: function (id) {
            return onFlagBtnClick(id);
        }
    }
}());