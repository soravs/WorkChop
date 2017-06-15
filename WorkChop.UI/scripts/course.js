$(document).ready(function () {
    var sideBar = (function () {
        var $sideMenu = $('.sidebar');
        var $button = $('#side-bar-btn');

        function _init() {
            var pageWidth = window.innerWidth;
            if (pageWidth > 767) {
                $sideMenu.addClass('open').removeClass('closed');
                $button.addClass('open').removeClass('closed');
            } else {
                $sideMenu.addClass('closed').removeClass('open');
                $button.addClass('closed').removeClass('open');
            }
        }

        function toggleMenu() {
            if ($sideMenu.hasClass('open')) {
                $sideMenu.removeClass('open').addClass('closed');
                $button.removeClass('open').addClass('closed');
            } else {
                $sideMenu.removeClass('closed').addClass('open');
                $button.removeClass('closed').addClass('open');
            }
        }

        _init();
        window.onresize = _init;

        $button.on('click', toggleMenu);



        //$('.menu-item-con .main-one').addClass('active');

       

    })()


});


