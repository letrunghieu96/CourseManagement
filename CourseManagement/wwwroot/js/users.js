// Register
function register(id) {
    $('#modalInputForm').load(`/Users/Register/${id}`, function () {
        $("#modalInputForm").modal("show");
        // Select 2
        $('.select2').select2();
    });
}

// Save
function save() {
    avoidTwoClicks("btnSave");

    var input = $("#frmInput").serialize();
    $.ajax({
        type: "POST",
        url: "/Users/Save",
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
                $("#modalInputForm").modal("hide");
                setTimeout(function () { location.reload(); }, 3000);
            }
        },
        error: function (xhr, exception) {
            alert('NG');
        }
    });
}

// Delete
function deleteUser(userId, userName) {
    var isOK = confirm(`Bạn có chắc chắn muốn xóa Người dùng "${userName}" không?`);
    if (isOK === false) return;

    $.ajax({
        type: "DELETE",
        url: `/Users/Delete/${userId}`,
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