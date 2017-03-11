(function () {
    $('body').addClass('display-profile-page');
}());

var DisplayProfile = (function () {

    function formShow(id) {
        $('.mce-form').hide();
        $('#' + id).show('slow');
    }

    function formHide(id) {
        $('#' + id).hide('slow');
    }

    function messageOnSuccess(id) {
        var msgDiv = $('#' + id);
        if (msgDiv.is(':hidden')) {
            msgDiv.show();
        }

        msgDiv.fadeOut(2000, function () { msgDiv.empty(); });
    }

    function profileMessagesShow($this) {
        $("#profileMessages").show('slow');
        $this.text("Скрий")
    }

    function profileMessagesHide($this) {
        if ($("#profileMessages").css('display') == 'block') {
            $("#profileMessages").hide('slow');
            $this.text("Покажи");
            return false;
        }
        return true;
    }

    function editedOnSuccess(id) {
        $('#' + id).hide('slow');
        $('#' + id).show('slide', { direction: 'right' }, 'slow');
    }

    function formAndNoCommentsHide(id) {
        formHide(id);
        var comments = $('#noComments');
        if (comments.css('display') != 'none') {
            comments.hide();
        }
    }

    function messageContentShow($this) {
        var parentDiv = $this.parent();
        if (parentDiv.hasClass('msg-not-read')) {
            var unreadMessages = $('#unreadMessages strong');
            var count = unreadMessages.text();
            count -= 1;
            unreadMessages.text(count);
            parentDiv.removeClass('msg-not-read');
            $("#hasMessages").removeClass('true');
        }

        parentDiv.next('.msg-container').show('slow');
        $this.text("Скрий");
    }

    function messageContentHide($this) {
        var parentDiv = $this.parent();
        var currentMsgContent = parentDiv.next('.msg-container');
        if (currentMsgContent.css('display') == 'block') {
            currentMsgContent.hide('slow');
            $this.text("Детайли");
            return false;
        }

        return true;
    }

    return {
        formShow: function (id) {
            return formShow(id);
        },
        formHide: function (id) {
            return formHide(id);
        },
        messageOnSuccess: function (id) {
            return messageOnSuccess(id);
        },
        profileMessagesShow: function ($this) {
            return profileMessagesShow($this);
        },
        profileMessagesHide: function ($this) {
            return profileMessagesHide($this);
        },
        editedOnSuccess: function (id) {
            return editedOnSuccess(id);
        },
        formAndNoCommentsHide: function (id) {
            return formAndNoCommentsHide(id);
        },
        messageContentShow: function ($this) {
            return messageContentShow($this);
        },
        messageContentHide: function ($this) {
            return messageContentHide($this);
        },
    }
}());