using CourseManagement.Domain;
using CourseManagement.Domain.Users;
using CourseManagement.Domain.Users.Helpers;
using CourseManagement.Helpers;
using CourseManagement.Services;
using CourseManagement.ViewModels;
using CourseManagement.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CourseManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        public UsersController(IConfiguration config, IDomainFacade domainFacade)
            : base(config, domainFacade)
        {
        }

        [HttpGet]
        public IActionResult List()
        {
            var viewModel = this.Service.CreateListViewModel(new SearchCondition());
            TempData["paging"] = JsonConvert.SerializeObject(viewModel.Pagination);

            return View(WebConstants.VIEW_USERS_LIST, viewModel);
        }

        [HttpGet]
        public IActionResult Search(SearchCondition condition)
        {
            var viewModel = this.Service.CreateListViewModel(condition);
            TempData["paging"] = JsonConvert.SerializeObject(viewModel.Pagination);

            return PartialView(WebConstants.PARTIAL_VIEW_USERS_SEARCH_RESULTS, viewModel.Results);
        }


        [HttpGet("{userId}")]
        public IActionResult Register(int userId)
        {
            var viewModel = this.Service.Get(userId);
            return PartialView(WebConstants.PARTIAL_VIEW_USERS_REGISTER, viewModel);
        }

        [HttpPost]
        public IActionResult Save(UserViewModel viewModel)
        {
            var isUpdate = viewModel.IsUpdate;
            if (isUpdate)
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Check exist
            if (this.Service.IsExistEmail(viewModel.Email, viewModel.UserId)) ModelState.AddModelError("Email", string.Format(ErrorMessageHelper.ExistError, "Email"));
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Save
            viewModel.LastChanged = this.UserName;
            var isSuccess = isUpdate
                ? this.Service.Update(viewModel)
                : this.Service.Create(viewModel);

            // Result
            var jsonResult = new JsonResultViewModel
            {
                IsSuccess = isSuccess,
                Message = isUpdate ? GetUpdatedMessage(isSuccess, "Người dùng") : GetCreatedMessage(isSuccess, "Người dùng"),
            };
            return Json(jsonResult);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Change password
            var isSuccess = this.Service.ChangePassword(viewModel.UserId, viewModel.Password, this.UserName);

            // Result
            var jsonResult = new JsonResultViewModel
            {
                IsSuccess = isSuccess,
                Message = GetUpdatedMessage(isSuccess, "Mật khẩu"),
            };
            return Json(jsonResult);
        }

        [HttpDelete("{userId}")]
        public IActionResult Delete(int userId)
        {
            // Delete
            var isSuccess = this.Service.Delete(userId);
            var jsonResult = new JsonResultViewModel
            {
                IsSuccess = isSuccess,
                Message = GetDeletedMessage(isSuccess, "Người dùng"),
            };
            return Json(jsonResult);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Edit()
        {
            var viewModel = this.Service.Get(this.UserId);
            return PartialView(WebConstants.PARTIAL_VIEW_USERS_EDIT, viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult UpdateUser(UserViewModel viewModel)
        {
            // Validate
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Check exist
            var id = (this.UserLogin != null) ? this.UserLogin.UserId : 0;
            if (this.Service.IsExistEmail(viewModel.Email, viewModel.UserId)) ModelState.AddModelError("Email", string.Format(ErrorMessageHelper.ExistError, "Email"));
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            var model = new UserModel
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                LastChanged = this.UserName,
            };
            var isSuccess = this.Service.UpdateUser(id, model);
            if (isSuccess) HttpContext.Session.SetString("UserName", viewModel.UserName);

            // Result
            var result = new
            {
                IsSuccess = isSuccess,
                Message = GetUpdatedMessage(isSuccess, "Thông tin"),
            };
            return Json(result);
        }

        private UsersService Service => new UsersService(_config, _domainFacade);
    }
}
