// Save
function save() {
    avoidTwoClicks("btnSave");

    var input = $("#frmInput").serialize();
    $.ajax({
        type: "POST",
        url: "/Courses/Save",
        data: input,
        success: function (result) {
            if ((result.isSuccess === false)
                && (result.errors != null)
                && (result.errors.length > 0)) {

                // Clear
                clearError();

                // Set error
                for (var index = 0; index < result.errors.length; index++) {
                    setError(result.errors[index].key, result.errors[index].value);
                }

                return;
            }

            $(document).Toasts('create', {
                class: result.isSuccess ? 'bg-info' : 'bg-danger',
                title: 'Thông báo',
                subtitle: '',
                body: result.message
            });

            if (result.isSuccess) {
                setTimeout(function () { window.location = result.path; }, 3000);
            }
        },
        error: function (xhr, exception) {
            alert('NG');
        }
    });
}

// Delete
function deleteCourse(courseId, courseCode) {
    var isOK = confirm(`Bạn có chắc chắn muốn xóa Khóa học "${courseCode}" không?`);
    if (isOK === false) return;

    $.ajax({
        type: "DELETE",
        url: `/Courses/Delete/${courseId}`,
        success: function (result) {
            $(document).Toasts('create', {
                class: result.isSuccess ? 'bg-info' : 'bg-danger',
                title: 'Thông báo',
                subtitle: '',
                body: result.message
            });

            if (result.isSuccess) setTimeout(function () { location.reload(); }, 3000);
        },
        error: function (xhr, exception) {
            alert('NG');
        }
    });
}