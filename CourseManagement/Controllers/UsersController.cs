using CourseManagement.Constants;
using CourseManagement.Domain;
using CourseManagement.Domain.Users.Helpers;
using CourseManagement.Helpers;
using CourseManagement.Services;
using CourseManagement.ViewModels;
using CourseManagement.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CourseManagement.Controllers
{
    public class UsersController : ControllerBase<UsersController>
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
            if (this.Service.IsExistEmail(viewModel.UserId, viewModel.Email)) ModelState.AddModelError("Email", string.Format(ErrorMessageHelper.ExistError, "Email"));
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Save
            viewModel.LastChanged = this.UserFullName;
            var isSuccess = isUpdate
                ? this.Service.Update(viewModel)
                : this.Service.Create(viewModel);
            if (isSuccess) _domainFacade.Commit();

            // Result
            var jsonResult = new JsonResultViewModel
            {
                IsSuccess = isSuccess,
                Message = isUpdate ? GetUpdatedMessage(isSuccess, "Người dùng") : GetCreatedMessage(isSuccess, "Người dùng"),
            };
            return Json(jsonResult);
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Change password
            var isSuccess = this.Service.ChangePassword(viewModel.UserId, viewModel.Password, this.UserFullName);
            if (isSuccess) _domainFacade.Commit();

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
            var isSuccess = this.Service.Delete(userId, this.UserFullName);
            if (isSuccess) _domainFacade.Commit();

            // Result
            var jsonResult = new JsonResultViewModel
            {
                IsSuccess = isSuccess,
                Message = GetDeletedMessage(isSuccess, "Người dùng"),
            };
            return Json(jsonResult);
        }

        private UsersService Service => new UsersService(_config, _domainFacade);
    }
}
