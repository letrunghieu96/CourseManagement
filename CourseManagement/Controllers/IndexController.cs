using CourseManagement.Constants;
using CourseManagement.Domain;
using CourseManagement.Services;
using CourseManagement.ViewModels;
using CourseManagement.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Controllers
{
    public class IndexController : ControllerBase<IndexController>
    {
        private readonly int _timeoutForCookieRememberPassword;
        private const string _cookieEmployeeCode = "InventoryManagementUserName";
        private const string _cookiePassword = "InventoryManagementPassword";

        public IndexController(IConfiguration config, IDomainFacade domainFacade)
            : base(config, domainFacade)
        {
            _timeoutForCookieRememberPassword = config.GetValue<int>("TimeoutForCookieRememberPassword");
        }

        [HttpGet("/")]
        public IActionResult Index(string? returnUrl = null)
        {
            var viewModel = new LoginViewModel { ReturnUrl = returnUrl, };

            // Is remember
            if (!string.IsNullOrEmpty(Request.Cookies[_cookieEmployeeCode])
               && !string.IsNullOrEmpty(Request.Cookies[_cookiePassword]))
            {
                viewModel.UserName = Request.Cookies[_cookieEmployeeCode] ?? string.Empty;
                viewModel.Password = Request.Cookies[_cookiePassword] ?? string.Empty;
                viewModel.IsRemember = true;
            }

            return View(WebConstants.VIEW_INDEX, viewModel);
        }


        [HttpGet("/Register")]
        public IActionResult Register()
        {
            return View(WebConstants.VIEW_REGISTER, new UserViewModel());
        }

        [HttpPost("/Register")]
        public IActionResult Save(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var isSuccess = this.Service.Insert(viewModel);
            }

            return View(WebConstants.VIEW_REGISTER, viewModel);
        }


        #region +Properties
        /// <summary>Index work service</summary>
        private IndexService Service => new IndexService(_config, _domainFacade);
        #endregion
    }
}
