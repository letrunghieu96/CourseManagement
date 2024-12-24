using CourseManagement.Domain;
using CourseManagement.Domain.Courses.Helpers;
using CourseManagement.Helpers;
using CourseManagement.Services;
using CourseManagement.ViewModels;
using CourseManagement.ViewModels.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var uniqueFolderName = (!string.IsNullOrEmpty(viewModel.FolderName)) ? viewModel.FolderName : Guid.NewGuid().ToString("N");
            HttpContext.Session.SetString("UniqueFolderName", uniqueFolderName);
            return PartialView(WebConstants.PARTIAL_VIEW_COURSES_REGISTER, viewModel);
        }

        [HttpPost]
        public IActionResult Save(CourseViewModel viewModel)
        {
            // Check file
            var isUpdate = viewModel.IsUpdate;
            var uniqueFolderName = HttpContext.Session.GetString("UniqueFolderName") ?? string.Empty;
            if (!isUpdate)
            {
                var isExistImage = this.Service.IsExistImage(uniqueFolderName);
                if (!isExistImage) ModelState.AddModelError("CourseImage", string.Format(ErrorMessageHelper.RequiredError, "Ảnh bìa"));
            }
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Check exist code
            viewModel.CourseCode = viewModel.CourseCode?.ToUpper();
            var regex = new Regex("^[a-zA-Z0-9]+$");
            if (!regex.IsMatch(viewModel.CourseCode ?? string.Empty))
            {
                ModelState.AddModelError("CourseCode", string.Format(ErrorMessageHelper.InvalidParameter, "Mã", viewModel.CourseCode));
                if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });
            }
            if (this.Service.IsExistCourseCode(viewModel.CourseCode, viewModel.CourseId)) ModelState.AddModelError("CourseCode", string.Format(ErrorMessageHelper.ExistError, "Mã"));

            // Check date range
            if (!this.Service.CheckDateRange(viewModel.StartDate, viewModel.EndDate)) ModelState.AddModelError("EndDate", string.Format(ErrorMessageHelper.InvalidParameter, "Kết thúc", viewModel.EndDate));
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Save
            viewModel.LastChanged = this.UserName;
            var isSuccess = isUpdate
                ? this.Service.Update(viewModel)
                : this.Service.Create(viewModel, uniqueFolderName);
            if (isSuccess && !isUpdate) HttpContext.Session.Remove("UniqueFolderName");

            // Result
            var jsonResult = new JsonResultViewModel
            {
                IsSuccess = isSuccess,
                Message = isUpdate ? GetUpdatedMessage(isSuccess, "Khóa học") : GetCreatedMessage(isSuccess, "Khóa học"),
                Path = isSuccess ? $"{WebConstants.PAGE_COURSES_REGISTER}/{viewModel.CourseId}" : string.Empty,
            };
            return Json(jsonResult);
        }

        [HttpDelete("{courseId}")]
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

        [HttpDelete("{courseId}")]
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
            // Upload image
            var uniqueFolderName = HttpContext.Session.GetString("UniqueFolderName") ?? string.Empty;
            var isSuccess = await this.Service.UploadImage(uniqueFolderName, file);
            var jsonResult = new
            {
                IsSuccess = isSuccess,
                ImageUrl = this.Service.GetImageUrl(uniqueFolderName, file.FileName),
                FileName = file.FileName,
            };
            return Json(jsonResult);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile[] files)
        {
            // Upload files
            var uniqueFolderName = HttpContext.Session.GetString("UniqueFolderName") ?? string.Empty;
            var isSuccess = await this.Service.UploadFiles(uniqueFolderName, files);
            var jsonResult = new
            {
                IsSuccess = isSuccess,
                FolderName = uniqueFolderName,
                FileNames = this.Service.GetFileNames(uniqueFolderName).Select(fileName => fileName),
            };
            return Json(jsonResult);
        }

        [HttpDelete]
        public IActionResult DeleteFile(string folderName, string fileName)
        {
            // Delete
            var isSuccess = this.Service.DeleteFile(folderName, fileName);
            var jsonResult = new
            {
                IsSuccess = isSuccess,
                FolderName = folderName,
                FileNames = this.Service.GetFileNames(folderName).Select(fileName => fileName),
            };
            return Json(jsonResult);
        }

        [HttpGet]
        public IActionResult DownloadFile(string folderName, string fileName)
        {
            var stream = this.Service.GetFile(folderName, fileName);
            return new FileStreamResult(stream, "application/octet-stream")
            {
                FileDownloadName = fileName,
            };
        }

        private CoursesService Service => new CoursesService(_config, _domainFacade);
    }
}
