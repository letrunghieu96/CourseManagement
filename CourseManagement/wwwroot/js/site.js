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