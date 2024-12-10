$(document).ajaxError(function (event, xhr, options, exc) {
    if ((xhr.status === 401)
        && (xhr.responseText === "SessionExpired")) {
        // If you receive a 401 error code and a message that the session has expired, automatically redirect to Login
        var currentUrl = window.location.pathname;
        window.location.href = "/?ReturnUrl=" + encodeURIComponent(currentUrl);
    }
});

// Avoid form resubmission
function avoidFormResubmission() {
    if (window.history.replaceState) window.history.replaceState(null, null, window.location.href);
}

// Clear error
function clearError() {
    $(".invalid-feedback").remove();
    $(".is-invalid").each(function (index, element) {
        element.classList.remove('is-invalid');
    });
    $(".select2-selection--single").css("border-color", "#ced4da");
    $('.date input').css("border-color", "#ced4da");
    $('.date div.input-group-text').css("border-color", "#ced4da");
}

// Insert after
function insertAfter(element, errorMessage) {
    if (element === null) return;

    var elementSpan = document.createElement("span");
    elementSpan.className = "error invalid-feedback";
    elementSpan.innerHTML = errorMessage;

    element.classList.add("is-invalid");
    element.after(elementSpan);
}

// Set error
function setError(id, errorMessage) {
    var element = document.getElementById(id);
    if (!element) {
        element = document.getElementById('dv' + id);
        if (!element) return; 
    }

    if (element.tagName === 'SELECT') {
        $("#select2-" + id + "-container").closest(".select2-selection--single").css("border-color", "#dc3545");
        insertAfter($("#select2-" + id + "-container").closest(".selection")[0], errorMessage);

        return;
    }

    if (element.tagName === 'INPUT') {
        if (document.getElementById('dv' + id)) {
            $('#dv' + id + ' > input').css("border-color", "#dc3545");
            $('#dv' + id + ' div.input-group-text').css("border-color", "#dc3545");
            insertAfter($('#dv' + id)[0], errorMessage);

            return;
        }
    }

    if (element.tagName === 'DIV') {
        if (document.getElementById('dv' + id)) {
            $('#dv' + id + ' > input').css("border-color", "#dc3545");
            $('#dv' + id + ' div.input-group-text').css("border-color", "#dc3545");
            insertAfter($('#dv' + id)[0], errorMessage);

            return;
        }
    }

    insertAfter(element, errorMessage);
}

// Avoid two clicks
function avoidTwoClicks(btnId) {
    const button = document.getElementById(btnId);
    if ((button != null) && !button.disabled) {
        button.disabled = true;
        // Timeout between two clicks
        setTimeout(() => {
            button.disabled = false;
        }, 1000);
    }
}

// Show password
function showPassword() {
    var password = document.querySelector("#Password");
    var confirmPassword = document.querySelector("#ConfirmPassword");
    var showPasswordCheckbox = document.querySelector("#cbShowPassword");

    if (password) {
        if (showPasswordCheckbox.checked) {
            password.type = "text";
            confirmPassword.type = "text";
        } else {
            password.type = "password";
            confirmPassword.type = "password";
        }
    }
}


// Jquery Dependency
$("input[data-type='currency']").on({
    keyup: function () {
        formatCurrency($(this));
    },
    blur: function () {
        formatCurrency($(this), "blur");
    }
});

// Format number
function formatNumber(n) {
    // format number 1000000 to 1,234,567
    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
}

// Format currency
function formatCurrency(input, blur) {
    // appends $ to value, validates decimal side
    // and puts cursor back in right position.

    // get input value
    var input_val = input.val();

    // don't validate empty input
    if (input_val === "") { return; }

    // original length
    var original_len = input_val.length;

    // initial caret position 
    var caret_pos = input.prop("selectionStart");

    // check for decimal
    if (input_val.indexOf(".") >= 0) {

        // get position of first decimal
        // this prevents multiple decimals from
        // being entered
        var decimal_pos = input_val.indexOf(".");

        // split number by decimal point
        var left_side = input_val.substring(0, decimal_pos);
        var right_side = input_val.substring(decimal_pos);

        // add commas to left side of number
        left_side = formatNumber(left_side);

        // validate right side
        right_side = formatNumber(right_side);

        // On blur make sure 2 numbers after decimal
        if (blur === "blur") {
            right_side += "00";
        }

        // Limit decimal to only 2 digits
        right_side = right_side.substring(0, 2);

        // join number by .
        input_val = left_side + "." + right_side;

    } else {
        // no decimal entered
        // add commas to number
        // remove all non-digits
        input_val = formatNumber(input_val);
    }

    // send updated string to input
    input.val(input_val);

    // put caret back in the right position
    var updated_len = input_val.length;
    caret_pos = updated_len - original_len + caret_pos;
    input[0].setSelectionRange(caret_pos, caret_pos);
}