// Save
function save() {
    avoidTwoClicks("btnSave");

    var imageSrc = $("#imagePreview")[0].src;
    $("#hfCourseImage").val(imageSrc);
    var input = $("#frmInput").serialize();

    var formData = new FormData();

    // Lấy file từ input file
    var fileInput = $("#CourseImage")[0].files[0];
    if (fileInput) {
        formData.append("CourseImage", fileInput); // Thêm file vào FormData
    }

    // Lấy dữ liệu từ form (dạng serialize)
    var serializedData = $("#frmInput").serializeArray(); // Lấy dữ liệu thành mảng

    // Duyệt qua từng cặp key-value và thêm vào FormData
    $.each(serializedData, function (index, field) {
        formData.append(field.name, field.value);
    });

    $.ajax({
        type: "POST",
        url: "/Courses/Save",
        data: formData,
        processData: false,
        contentType: false,
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

// Register enrollment
function registerEnrollment(courseId) {
    var isOK = confirm(`Bạn có chắc chắn muốn Đăng ký khóa học này không?`);
    if (isOK === false) return;

    $.ajax({
        type: "POST",
        url: `/Courses/RegisterEnrollment`,
        data: { courseId },
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

// Delete enrollment
function deleteEnrollment(enrollmentId) {
    var isOK = confirm(`Bạn có chắc chắn muốn Hủy đăng ký khóa học không?`);
    if (isOK === false) return;

    $.ajax({
        type: "DELETE",
        url: `/Courses/DeleteEnrollment/${enrollmentId}`,
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

// Display image
function displayImage() {
    const fileInput = document.getElementById("CourseImage");
    if (fileInput.files.length === 0) return;

    const imagePreview = document.getElementById("imagePreview");
    const file = fileInput.files[0];

    // Image
    const reader = new FileReader();
    reader.addEventListener("load", (event) => {
        imagePreview.src = event.target.result;
        imagePreview.classList.remove("d-none");
    });
    reader.readAsDataURL(file);

    // Image info
    document.getElementById("imageInfo").innerHTML = '<p class="mb-0"><span>' + file.name + '</span> (<span><strong>' + (file.size / 1024).toFixed(2) + '</strong> KB</span>)</p>';
}