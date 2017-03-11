(function () {

    $('#menuBtn').click(function () {
        $('#container').toggleClass('opened');
        $('#navigation').toggleClass('opened');
    })

    $('#loginBtn').click(function () {
        $('#loginFields').toggle();
    })
}
());