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

        //function floatingGroup(element) {
        //    var label = $(element).find('.floating-group__label')[0];
        //    var input = $(element).find('.floating-group__input')[0];

        //    var $label = $(label);
        //    var $input = $(input);

        //    if (checkIfEmpty(input)) {
        //        $label.addClass('floating-group__label--empty');
        //    } else {
        //        $label.addClass('floating-group__label--occupied');
        //    }

        //    $input.on('focus', function () {
        //        $label.addClass('floating-group__label--occupied');
        //    });

        //    $input.on('focusout', function () {
        //        if (checkIfEmpty(input)) {
        //            $label.removeClass('floating-group__label--occupied');
        //        }
        //    });


        //    function checkIfEmpty(inputField) {
        //        if (inputField!=undefined && inputField.value === '') {
        //            return true;
        //        }

        //        return false;
        //    }
        //}

        //$('.floating-group').each(function (index, value) {
          
        //    if (value != undefined) {
        //        var float = value;
        //        float = floatingGroup(float);
        //    }

        //});


        $('.menu-item-con .main-one').addClass('active');

        //$('#simple-editor').trumbowyg();

    })()


});


