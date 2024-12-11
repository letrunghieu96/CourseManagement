using CourseManagement.Domain;
using CourseManagement.Helpers;
using CourseManagement.Models;
using CourseManagement.Services;
using CourseManagement.ViewModels;
using CourseManagement.ViewModels.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseManagement.Controllers
{
    public class IndexController : ControllerBase
    {
        private readonly int _timeoutForCookieRememberPassword;
        private const string _cookieEmail = "CourseManagement.Email";
        private const string _cookiePassword = "CourseManagement.Password";

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
            if (!string.IsNullOrEmpty(Request.Cookies[_cookieEmail])
               && !string.IsNullOrEmpty(Request.Cookies[_cookiePassword]))
            {
                viewModel.Email = Request.Cookies[_cookieEmail] ?? string.Empty;
                viewModel.Password = Request.Cookies[_cookiePassword] ?? string.Empty;
                viewModel.IsRemember = true;
            }

            return View(WebConstants.VIEW_INDEX, viewModel);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            // Invalid input
            if (!ModelState.IsValid) return View(WebConstants.VIEW_INDEX, viewModel);

            // Remember sign in information
            if (viewModel.IsRemember)
            {
                CookieHelper.Set(HttpContext.Response.Cookies, _cookieEmail, viewModel.Email, _timeoutForCookieRememberPassword);
                CookieHelper.Set(HttpContext.Response.Cookies, _cookiePassword, viewModel.Password, _timeoutForCookieRememberPassword);
            }
            else
            {
                CookieHelper.Remove(HttpContext.Response.Cookies, _cookieEmail);
                CookieHelper.Remove(HttpContext.Response.Cookies, _cookiePassword);
            }

            var userLogin = this.Service.Login(viewModel.Email, viewModel.Password);
            if (userLogin == null)
            {
                ModelState.AddModelError("LoginMessage", $"{ErrorMessageHelper.LoggedInFailed}\r\n{ErrorMessageHelper.WrongLoginInfo}");
                return View(WebConstants.VIEW_INDEX, viewModel);
            }

            // Set info
            SetUserLoginInfo(userLogin);

            var page = !string.IsNullOrEmpty(viewModel.ReturnUrl) ? viewModel.ReturnUrl : WebConstants.PAGE_HOME;
            return LocalRedirect(page);
        }

        [HttpGet("/Register")]
        public IActionResult Register()
        {
            return View(WebConstants.VIEW_REGISTER, new UserViewModel());
        }

        [HttpPost("/Create")]
        public IActionResult Create(UserViewModel viewModel)
        {
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Check exist
            if (this.Service.IsExistEmail(viewModel.Email)) ModelState.AddModelError("Email", string.Format(ErrorMessageHelper.ExistError, "Email"));
            if (!ModelState.IsValid) return Json(new JsonResultViewModel { IsSuccess = false, Errors = CreateErrors(ModelState) });

            // Create
            var userId = this.Service.Create(viewModel);
            var isSuccess = (userId > 0);

            // Set info
            if (isSuccess)
            {
                var userLogin = new LoginModel
                {
                    UserId = userId,
                    UserName = viewModel.UserName,
                    UserRole = viewModel.UserRole,
                };
                SetUserLoginInfo(userLogin);
            }

            // Result
            var jsonResult = new JsonResultViewModel
            {
                IsSuccess = isSuccess,
                Message = isSuccess ? ErrorMessageHelper.RegistrationSuccessfully : ErrorMessageHelper.RegistrationFailed,
                Path = isSuccess ? WebConstants.PAGE_HOME : string.Empty,
            };
            return Json(jsonResult);
        }

        [Authorize]
        [HttpGet("/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Response.Cookies.Delete("CourseManagement.AspNetCore.Cookies");
            HttpContext.SignOutAsync();

            return RedirectToAction("Index");
        }

        private void SetUserLoginInfo(LoginModel model)
        {
            // Save info
            SessionHelper.SetObjectAsJson(HttpContext.Session, "UserLogin", model);
            HttpContext.Session.SetString("UserName", this.UserName);
            HttpContext.Session.SetString("UserRole", ValueTextHelper.GetRoleText(this.UserRole));

            // Set claims
            var authClaims = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            authClaims.AddClaim(new Claim(ClaimTypes.NameIdentifier, this.UserId.ToString()));
            authClaims.AddClaim(new Claim(ClaimTypes.Role, this.UserRole));
            authClaims.AddClaim(new Claim(ClaimTypes.Name, this.UserName));
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(authClaims));
        }

        private IndexService Service => new IndexService(_config, _domainFacade);
    }
}
