
var _itDateFormat = "DD/MM/YYYY";

$(function () {
    $(".settings-item").hover(
		function () {
		    $(".fa-gear").addClass('fa-spin');
		},
  		function () {
  		    $(".fa-gear").removeClass('fa-spin');
  		}
	);

    //var itMoment = moment().locale('it');

    moment.locale('it');
});

function setupListPage() {
    $('[data-toggle="tooltip"]').tooltip();
}

function setupDetailPage() {
    var $root = $('html, body');
    $('a.internal-scroll').click(function () {
        $root.animate({
            scrollTop: $('[name="' + $.attr(this, 'href').substr(1) + '"]').offset().top - 85
        }, 500);
        return false;
    });

    $('.datepicker').datepicker({ weekStart: 1 });

    $('[data-toggle="tooltip"]').tooltip();
}

function detectIE() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf('MSIE ');
    var trident = ua.indexOf('Trident/');

    if (msie > 0) {
        // IE 10 or older => return version number
        return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
    }

    if (trident > 0) {
        // IE 11 (or newer) => return version number
        var rv = ua.indexOf('rv:');
        return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
    }

    // other browser
    return false;
}


function toggleFontAwesomeCheckbox(obj, selectText, deselectText, selectClass, deselectClass) {
    var _iconStack = $(obj);
    var _description = $(obj).parent().children('span.toggle-checkbox-description');

    if (_iconStack.children('i.fa-check').length == 0) {
        // SELECTING
        _description.text(selectText)
			.removeClass(deselectClass)
			.addClass(selectClass)
        ;

        $('<i>').addClass("fa fa-check fa-stack-1x text-success").appendTo(_iconStack);

    } else {
        // DESELECTING
        _description.text(deselectText)
			.removeClass(selectClass)
			.addClass(deselectClass)
        ;

        $('i.fa-check', _iconStack).remove();
    }

}

function isFontAwesomeChecked(checkBoxId) {
    return $(checkBoxId).find('i.fa-check').length > 0;
}


function notifyWithoutIcon(title, text, type) {
    new PNotify({
        title: title,
        text: text,
        icon: false,
        type: type
    });
}

function notifyWithFAIcon(title, text, type, icon) {
    new PNotify({
        title: '<i class="fa fa-lg fa-' + icon + '"></i><span class="text-after-icon">' + title + '</span>',
        text: text,
        icon: false,
        type: type
    });
}

function notify(title, text, type, icon) {
    if (icon == null) {
        notifyWithoutIcon(title, text, type);
    } else {
        notifyWithFAIcon(title, text, type, icon)
    }
}

function notifyInfo(text) {
    notify('Info', text, 'info', 'info-circle');
}

function notifySuccess(text) {
    notify('Successo!', text, 'success', 'smile-o');
}

function notifyWarning(text) {
    notify('Attenzione', text, null, 'warning');
}

function notifyError(text) {
    notify('Errore', text, 'error', 'frown-o');
}


function startDeleteOperation(waitControlId) {
    startGenericOperation(waitControlId, "trash-o");
}

function endDeleteOperation(waitControlId) {
    endGenericOperation(waitControlId, "trash-o");
}


function startSaveOperation(waitControlId) {
    startGenericOperation(waitControlId, "save");
}

function endSaveOperation(waitControlId) {
    endGenericOperation(waitControlId, "save");
}


function startSearchOperation(waitControlId) {
    startGenericOperation(waitControlId, "search");
}

function endSearchOperation(waitControlId) {
    endGenericOperation(waitControlId, "search");
}


function startGenericOperation(waitControlId, icon) {
    $("#" + waitControlId).removeClass("fa-" + icon);
    $("#" + waitControlId).addClass("fa-pulse fa-spinner");
}

function endGenericOperation(waitControlId, icon) {
    $("#" + waitControlId).removeClass("fa-pulse fa-spinner");
    $("#" + waitControlId).addClass("fa-" + icon);
}


function GetCheckboxListAsJson(checkboxSelector) {
    var chks = $(checkboxSelector).toArray(), obj = {};
    for (var i = 0; i < chks.length; i++) {
        obj[chks[i].name] = chks[i].checked;
    }

    return JSON.stringify(obj);
}

function clearDatepickerInput(ctrl) {
    $(ctrl)
        .parent()
        .parent()
        .find("input.datepicker:not('button')")
        .val('')
    ;
}

function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}