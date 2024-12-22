// Upload image
function uploadImage() {
    var formData = new FormData();
    var fileInput = $("#CourseImage")[0].files[0];
    if (fileInput) formData.append("file", fileInput);

    $.ajax({
        type: "POST",
        url: "/Courses/UploadImage",
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.isSuccess === false) {
                alert('thất bại');
                return;
            }

            if (result.isSuccess) {
                const imagePreview = document.getElementById("imagePreview");
                imagePreview.src = result.imageUrl;
                document.getElementById("imageInfo").innerHTML = '<p class="mb-0"><span>' + result.fileName + '</span></p>';
            }
        },
        error: function (xhr, exception) {
            alert('NG');
        }
    });
}

// Download file
function downloadFile(courseId, fileName) {
    const link = document.createElement("a");
    link.href = `/Courses/DownloadFile?courseId=${courseId}&fileName=${encodeURIComponent(fileName)}`;
    link.download = fileName;
    link.click();
}

// Delete file
function deleteFile(courseId, fileName) {
    var isOK = confirm(`Bạn có chắc chắn muốn xóa ${fileName}?`);
    if (isOK === false) return;

    $.ajax({
        type: "DELETE",
        url: `/Courses/DeleteFile?courseId=${courseId}&fileName=${encodeURIComponent(fileName)}`,
        success: function (result) {
            if (result.isSuccess === false) {
                alert('thất bại');
                return;
            }

            if (result.isSuccess) {
                var fileList = '';
                for (var index = 0; index < result.fileNames.length; index++) {
                    const tagA = `<a class="btn btn-link" onclick="downloadFile('${result.courseId}', '${result.fileNames[index]}')">${result.fileNames[index]}</a>`;
                    const tagButton = `<a class="btn btn-danger btn-sm" onclick="deleteFile('${result.courseId}', '${result.fileNames[index]}')"><i class="fas fa-trash"></i></a>`;
                    fileList += tagA + tagButton + '</br>';
                }

                document.getElementById("fileList").innerHTML = fileList;
                exportFile();
            }
        },
        error: function (xhr, exception) {
            alert('NG');
        }
    });
}

// Upload files
function uploadFiles() {
    var formData = new FormData();
    var fileInputs = $("#CourseFile")[0].files;

    if (fileInputs.length > 0) {
        for (var index = 0; index < fileInputs.length; index++) {
            formData.append("files", fileInputs[index]);
        }
    } 

    $.ajax({
        type: "POST",
        url: "/Courses/UploadFiles",
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.isSuccess === false) {
                alert('thất bại');
                return;
            }

            if (result.isSuccess) {
                var fileList = '';
                for (var index = 0; index < result.fileNames.length; index++) {
                    const tagA = `<a class="btn btn-link" onclick="downloadFile('${result.courseId}', '${result.fileNames[index]}')">${result.fileNames[index]}</a>`;
                    const tagButton = `<a class="btn btn-danger btn-sm" onclick="deleteFile('${result.courseId}', '${result.fileNames[index]}')"><i class="fas fa-trash"></i></a>`;
                    fileList += tagA + tagButton + '</br>';
                }

                document.getElementById("fileList").innerHTML = fileList;

                exportFile();
            }
        },
        error: function (xhr, exception) {
            alert('NG');
        }
    });
}

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
