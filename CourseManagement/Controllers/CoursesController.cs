using CourseManagement.Domain;
using CourseManagement.Domain.Courses.Helpers;
using CourseManagement.Helpers;
using CourseManagement.Models;
using CourseManagement.Services;
using CourseManagement.ViewModels;
using CourseManagement.ViewModels.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace CourseManagement.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class CoursesController : ControllerBase
    {
        public CoursesController(IConfiguration config, IDomainFacade domainFacade)
            : base(config, domainFacade)
        {
        }

        [HttpGet]
        public IActionResult List()
        {
            var viewModel = this.Service.CreateListViewModel(new SearchCondition());
            TempData["paging"] = JsonConvert.SerializeObject(viewModel.Pagination);

            return View(WebConstants.VIEW_COURSES_LIST, viewModel);
        }

        [HttpGet]
        public IActionResult Search(SearchCondition condition)
        {
            var viewModel = this.Service.CreateListViewModel(condition);
            TempData["paging"] = JsonConvert.SerializeObject(viewModel.Pagination);

            return PartialView(WebConstants.PARTIAL_VIEW_COURSES_SEARCH_RESULTS, viewModel.Results);
        }

        [HttpGet("{courseId:int?}")]
        public IActionResult Register(int courseId)
        {
            var viewModel = this.Service.Get(courseId, this.UserId);
            return PartialView(WebConstants.PARTIAL_VIEW_COURSES_REGISTER, viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save(CourseViewModel viewModel)
        {
            // Check file
            var isUpdate = viewModel.IsUpdate;
            var uniqueCourseId = HttpContext.Session.GetString("UniqueCourseId");
            if (string.IsNullOrEmpty(uniqueCourseId))
            {
                uniqueCourseId = Guid.NewGuid().ToString("N");
                HttpContext.Session.SetString("UniqueCourseId", uniqueCourseId);
            }
            if (!isUpdate)
            {
                var isExistImage = this.Service.IsExistImage(uniqueCourseId);
                if (!isExistImage) ModelState.AddModelError("CourseImage", string.Format(ErrorMessageHelper.RequiredError, "Ảnh bìa"));
            }
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Check exist
            viewModel.CourseCode = viewModel.CourseCode?.ToUpper();
            var regex = new Regex("^[a-zA-Z0-9]+$");
            if (!regex.IsMatch(viewModel.CourseCode ?? string.Empty))
            {
                ModelState.AddModelError("CourseCode", string.Format(ErrorMessageHelper.InvalidParameter, "Mã", viewModel.CourseCode));
                if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });
            }

            if (this.Service.IsExistCourseCode(viewModel.CourseCode, viewModel.CourseId)) ModelState.AddModelError("CourseCode", string.Format(ErrorMessageHelper.ExistError, "Mã"));
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Save
            viewModel.LastChanged = this.UserName;
            viewModel.CourseFile = this.Service.GetCourseFile(uniqueCourseId);
            var isSuccess = isUpdate
                ? await this.Service.Update(viewModel)
                : await this.Service.Create(viewModel, uniqueCourseId);

            // Result
            var jsonResult = new JsonResultViewModel
            {
                IsSuccess = isSuccess,
                Message = isUpdate ? GetUpdatedMessage(isSuccess, "Khóa học") : GetCreatedMessage(isSuccess, "Khóa học"),
                Path = isSuccess ? $"{WebConstants.PAGE_COURSES_REGISTER}/{viewModel.CourseId}" : string.Empty,
            };
            return Json(jsonResult);
        }

        [HttpDelete("Delete/{courseId}")]
        public IActionResult Delete(int courseId)
        {
            // Delete
            var isSuccess = this.Service.Delete(courseId);

            // Result
            var jsonResult = new JsonResultViewModel
            {
                IsSuccess = isSuccess,
                Message = GetDeletedMessage(isSuccess, "Khóa học"),
            };
            return Json(jsonResult);
        }

        [HttpPost]
        public IActionResult RegisterEnrollment(int courseId)
        {
            // Register
            var isSuccess = this.Service.RegisterEnrollment(courseId, this.UserId);

            // Result
            var jsonResult = new JsonResultViewModel
            {
                IsSuccess = isSuccess,
                Message = isSuccess ? ErrorMessageHelper.RegistrationSuccessfully : ErrorMessageHelper.RegistrationFailed,
            };
            return Json(jsonResult);
        }

        [HttpDelete("DeleteEnrollment/{courseId}")]
        public IActionResult DeleteEnrollment(int courseId)
        {
            // Delete
            var isSuccess = this.Service.DeleteEnrollment(courseId, this.UserId);

            // Result
            var jsonResult = new JsonResultViewModel
            {
                IsSuccess = isSuccess,
                Message = isSuccess ? ErrorMessageHelper.UnsubscribeSuccessfully : ErrorMessageHelper.UnsubscribeFailed,
            };
            return Json(jsonResult);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var uniqueCourseId = HttpContext.Session.GetString("UniqueCourseId");
            if (string.IsNullOrEmpty(uniqueCourseId))
            {
                uniqueCourseId = Guid.NewGuid().ToString("N");
                HttpContext.Session.SetString("UniqueCourseId", uniqueCourseId);
            }

            // Upload image
            var isSuccess = await this.Service.UploadImage(uniqueCourseId, file);
            return Json(
                new {
                    IsSuccess = isSuccess,
                    ImageUrl = $"\\{this.Service.GetImageUrl(uniqueCourseId, file.FileName)}",
                    FileName = file.FileName,
                });
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile[] files)
        {
            var uniqueCourseId = HttpContext.Session.GetString("UniqueCourseId");
            if (string.IsNullOrEmpty(uniqueCourseId))
            {
                uniqueCourseId = Guid.NewGuid().ToString("N");
                HttpContext.Session.SetString("UniqueCourseId", uniqueCourseId);
            }

            // Upload files
            var isSuccess = await this.Service.UploadFiles(uniqueCourseId, files);
            return Json(
                new
                {
                    IsSuccess = isSuccess,
                    CourseId = uniqueCourseId,
                    FileNames = this.Service.GetFileNames(uniqueCourseId).Select(fileName => fileName),
                });
        }

        [HttpDelete]
        public IActionResult DeleteFile(string courseId, string fileName)
        {
            // Delete
            var isSuccess = this.Service.DeleteFile(courseId, fileName);
            return Json(
                new
                {
                    IsSuccess = isSuccess,
                    CourseId = courseId,
                    FileNames = this.Service.GetFileNames(courseId).Select(fileName => fileName),
                });
        }

        [HttpGet]
        public IActionResult DownloadFile(string courseId, string fileName)
        {
            var stream = this.Service.GetFile(courseId, fileName);
            return new FileStreamResult(stream, "application/octet-stream")
            {
                FileDownloadName = fileName,
            };
        }

        private CoursesService Service => new CoursesService(_config, _domainFacade);
    }
}
