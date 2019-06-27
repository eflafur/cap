
function validRequiredData(obj) {
    var _controlValue = $.trim(obj.val());

    if (_controlValue == "") {
        obj.parent().removeClass("has-success").addClass("has-error");
        return false;
    } else {
        if (obj.parent().hasClass("has-error")) {
            obj.parent().addClass("has-success");
        }
        obj.parent().removeClass("has-error");
        return true;
    }
}

// VALID INTEGER
function validInteger(theNumber, allowZero) {
    //if (theNumber.length == 0)

    if (theNumber === undefined || theNumber == null || theNumber.length == 0)
        return true;

    var regex;
    if (allowZero === true) {
        regex = new RegExp(/^(0|[1-9]\d*)$/);
        return regex.test(theNumber) && parseInt(theNumber >= 0);
    } else {
        regex = new RegExp(/^[1-9]\d*$/);
        return regex.test(theNumber) && parseInt(theNumber > 0);
    }
}

// VALID NUMBER
function validNumber(theNumber) {
    if (theNumber === undefined || theNumber == null || theNumber.length == 0)
        return true;

    var regex;
    regex = new RegExp(/^[0-9]+(\,[0-9]{1,3})?$/);
    return regex.test(theNumber) && parseInt(theNumber > 0);
}

// VALID CURRENCY
function validCurrency(theNumber) {
    if (theNumber.length == 0)
        return true;

    var regex = /^[0-9]+(\,[0-9]{2})?$/;

    return regex.test(theNumber);
}

// VALID EMAIL
function validEmail(email) {

    if (email === undefined || email == null || email.length == 0)
        return true;

    var regex;
    regex = new RegExp(/^[A-Z0-9._-]+@[A-Z0-9.-]+\.[A-Z0-9.-]+$/i);
    return regex.test(email);
}

// VALIDATE NUMERIC FORM GROUP INPUT
function validateIntegerFormGroupInput(obj, allowZero) {
    var isValid = true;

    if (validInteger(obj.val(), allowZero) == false) {
        obj.parent().removeClass("has-success").addClass("has-error");
        isValid = false;
    } else {
        if (obj.parent().hasClass("has-error")) {
            obj.parent().addClass("has-success");
        }
        obj.parent().removeClass("has-error");
        isValid = true;
    }

    return isValid;
}

function validateNumericFormGroupInput(obj, allowZero) {
    var isValid = true;

    if (validNumber(obj.val()) == false) {
        obj.parent().removeClass("has-success").addClass("has-error");
        isValid = false;
    } else {
        if (obj.parent().hasClass("has-error")) {
            obj.parent().addClass("has-success");
        }
        obj.parent().removeClass("has-error");
        isValid = true;
    }

    return isValid;
}


// VALIDATE CURRENCY FORM GROUP INPUT
function validateCurrencyFormGroupInput(obj) {
    var isValid = true;

    if (validCurrency(obj.val()) == false) {
        obj.parent().removeClass("has-success").addClass("has-error");
        isValid = false;
    } else {
        if (obj.parent().hasClass("has-error")) {
            obj.parent().addClass("has-success");
        }
        obj.parent().removeClass("has-error");
        isValid = true;
    }

    return isValid;
}

// VALIDATE EMAIL FORM GROUP INPUT
function validateEmailFormGroupInput(obj) {
    var isValid = true;

    if (validEmail(obj.val()) == false) {
        obj.parent().removeClass("has-success").addClass("has-error");
        isValid = false;
    } else {
        if (obj.parent().hasClass("has-error")) {
            obj.parent().addClass("has-success");
        }
        obj.parent().removeClass("has-error");
        isValid = true;
    }

    return isValid;
}


