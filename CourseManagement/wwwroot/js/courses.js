// Upload image
function uploadImage() {
    var formData = new FormData();
    var fileInput = $("#CourseImage")[0].files[0];
    const maxFileSize = 10 * 1024 * 1024; // 10MB
    if (fileInput && fileInput.size > maxFileSize) {
        $("#CourseImage").val("");
        alert('Dung lượng file vượt quá giới hạn cho phép (10MB)');
        return;
    }
    formData.append("file", fileInput);

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
function downloadFile(folderName, fileName) {
    const link = document.createElement("a");
    link.href = `/Courses/DownloadFile?folderName=${folderName}&fileName=${encodeURIComponent(fileName)}`;
    link.download = fileName;
    link.click();
}

// Delete file
function deleteFile(folderName, fileName) {
    var isOK = confirm(`Bạn có chắc chắn muốn xóa ${fileName}?`);
    if (isOK === false) return;

    $.ajax({
        type: "DELETE",
        url: `/Courses/DeleteFile?folderName=${folderName}&fileName=${encodeURIComponent(fileName)}`,
        success: function (result) {
            if (result.isSuccess === false) {
                alert('thất bại');
                return;
            }

            if (result.isSuccess) {
                var fileList = '';
                for (var index = 0; index < result.fileNames.length; index++) {
                    const tagA = `<a class="btn btn-link" onclick="downloadFile('${result.folderName}', '${result.fileNames[index]}')">${result.fileNames[index]}</a>`;
                    const tagButton = `<a class="btn btn-danger btn-sm" onclick="deleteFile('${result.folderName}', '${result.fileNames[index]}')"><i class="fas fa-trash"></i></a>`;
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
    var fileInputs = $("#CourseFiles")[0].files;
    const maxFileSize = 10 * 1024 * 1024; // 10MB
    const maxFiles = 5; // 5 file

    if (fileInputs.length > maxFiles) {
        $("#CourseFiles").val("");
        alert(`Chỉ được upload tối đa ${maxFiles} file.`);
        return;
    }

    let errorMessage = "";
    if (fileInputs.length > 0) {
        for (var index = 0; index < fileInputs.length; index++) {

            if (fileInputs[index].size > maxFileSize) {
                errorMessage += `\nFile "${fileInputs[index].name}" vượt quá giới hạn dung lượng (10MB).`;
                continue;
            }

            formData.append("files", fileInputs[index]);
        }
    }

    if (errorMessage) {
        $("#CourseFiles").val("");
        alert(errorMessage);
        return;
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
                    const tagA = `<a class="btn btn-link" onclick="downloadFile('${result.folderName}', '${result.fileNames[index]}')">${result.fileNames[index]}</a>`;
                    const tagButton = `<a class="btn btn-danger btn-sm" onclick="deleteFile('${result.folderName}', '${result.fileNames[index]}')"><i class="fas fa-trash"></i></a>`;
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
