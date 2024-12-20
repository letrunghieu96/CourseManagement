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
            return PartialView(WebConstants.PARTIAL_VIEW_COURSES_REGISTER, viewModel);
        }

        [HttpPost]
        public IActionResult Save(CourseViewModel viewModel)
        {
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
            var isUpdate = viewModel.IsUpdate;
            viewModel.LastChanged = this.UserName;
            var isSuccess = isUpdate
                ? this.Service.Update(viewModel)
                : this.Service.Create(viewModel);

            // Result
            var jsonResult = new JsonResultViewModel
            {
                IsSuccess = isSuccess,
                Message = isUpdate ? GetUpdatedMessage(isSuccess, "Khóa học") : GetCreatedMessage(isSuccess, "Khóa học"),
                Path = isSuccess ?  $"{WebConstants.PAGE_COURSES_REGISTER}/{viewModel.CourseId}" : string.Empty,
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

        private CoursesService Service => new CoursesService(_config, _domainFacade);
    }
}
