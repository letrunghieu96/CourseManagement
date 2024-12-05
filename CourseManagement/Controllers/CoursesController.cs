﻿using CourseManagement.Constants;
using CourseManagement.Domain;
using CourseManagement.Domain.Courses.Helpers;
using CourseManagement.Helpers;
using CourseManagement.Services;
using CourseManagement.ViewModels;
using CourseManagement.ViewModels.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CourseManagement.Controllers
{
    [Authorize]
    public class CoursesController : ControllerBase<CoursesController>
    {
        public CoursesController(IConfiguration config, IDomainFacade domainFacade)
            : base(config, domainFacade)
        {
        }

        [HttpGet]
        public IActionResult List()
        {
            var viewModel = this.Service.CreateListViewModel(new SearchCondition());
            viewModel.Results.ToList().ForEach(result => result.CanDelete = (this.IsAdmin || (result.CreatedBy == this.UserId)));
            TempData["paging"] = JsonConvert.SerializeObject(viewModel.Pagination);

            return View(WebConstants.VIEW_COURSES_LIST, viewModel);
        }

        [HttpGet]
        public IActionResult Search(SearchCondition condition)
        {
            var viewModel = this.Service.CreateListViewModel(condition);
            viewModel.Results.ToList().ForEach(result => result.CanDelete = (this.IsAdmin || (result.CreatedBy == this.UserId)));
            TempData["paging"] = JsonConvert.SerializeObject(viewModel.Pagination);

            return PartialView(WebConstants.PARTIAL_VIEW_COURSES_SEARCH_RESULTS, viewModel.Results);
        }

        [HttpGet("{courseId:int?}")]
        public IActionResult Register(int courseId)
        {
            var viewModel = this.Service.Get(courseId);
            return PartialView(WebConstants.PARTIAL_VIEW_COURSES_REGISTER, viewModel);
        }

        [HttpPost]
        public IActionResult Save(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Check exist
            if (this.Service.IsExistCourseCode(viewModel.CourseId, viewModel.CourseCode)) ModelState.AddModelError("CourseCode", string.Format(ErrorMessageHelper.ExistError, "Mã"));
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Save
            var isUpdate = viewModel.IsUpdate;
            if (!isUpdate) viewModel.CreatedBy = this.UserId;
            viewModel.LastChanged = this.UserFullName;
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

        private CoursesService Service => new CoursesService(_config, _domainFacade);
    }
}