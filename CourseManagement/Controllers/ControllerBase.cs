using CourseManagement.Constants;
using CourseManagement.Domain;
using CourseManagement.Helpers;
using CourseManagement.Models;
using CourseManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CourseManagement.Controllers
{
    [Route("[controller]/[action]")]
    public class ControllerBase<T> : Controller
    {
        protected readonly IConfiguration _config;
        protected readonly IDomainFacade _domainFacade;

        public ControllerBase(IConfiguration config, IDomainFacade domainFacade)
        {
            _config = config;
            _domainFacade = domainFacade;

            InitConfigSetting();
        }

        private void InitConfigSetting()
        {
            var pageLengths = _config.GetValue<string>("PageLengths");
            if (!string.IsNullOrEmpty(pageLengths)) ConfigConstants.PAGE_LENGTHS = pageLengths.Split(",").Select(length => int.Parse(length)).ToArray();

            //ConfigConstants.ApplicationInfo = new ApplicationInfoModel
            //{
            //    CompanyName = _config.GetValue<string>("ApplicationInfo:CompanyName") ?? string.Empty,
            //    TaxCode = _config.GetValue<string>("ApplicationInfo:TaxCode") ?? string.Empty,
            //    Address = _config.GetValue<string>("ApplicationInfo:Address") ?? string.Empty,
            //    PhoneNumber = _config.GetValue<string>("ApplicationInfo:PhoneNumber") ?? string.Empty,
            //    Director = _config.GetValue<string>("ApplicationInfo:Director") ?? string.Empty,
            //};
        }

        [HttpGet]
        public async Task<IActionResult> LoadPagination() => await Task.FromResult(PartialView(WebConstants.PARTIAL_VIEW_PAGINATION));

        /// <summary>
        /// Create errors
        /// </summary>
        /// <param name="modelState">Model state</param>
        /// <returns>Errors</returns>
        protected ErrorMessageList? CreateErrors(ModelStateDictionary? modelState)
        {
            if (modelState == null) return null;

            var errors = new ErrorMessageList();
            foreach (var field in modelState)
            {
                if (field.Value.ValidationState == ModelValidationState.Valid) continue;

                errors.Add(
                    field.Key,
                    string.Join(". ", field.Value.Errors.Select(error => error.ErrorMessage)));
            }

            return errors;
        }

        protected string GetCreatedMessage(bool isSuccess, string objName)
        {
            var message = string.Format(isSuccess ? ErrorMessageHelper.CreatedSuccessfully : ErrorMessageHelper.CreatedFailed, objName);
            return message;
        }

        protected string GetUpdatedMessage(bool isSuccess, string objName)
        {
            var message = string.Format(isSuccess ? ErrorMessageHelper.UpdatedSuccessfully : ErrorMessageHelper.UpdatedFailed, objName);
            return message;
        }

        protected string GetDeletedMessage(bool isSuccess, string objName)
        {
            var message = string.Format(isSuccess ? ErrorMessageHelper.DeletedSuccessfully : ErrorMessageHelper.DeletedFailed, objName);
            return message;
        }

        protected LoginModel? UserLogin => SessionHelper.GetObjectFromJson<LoginModel>(HttpContext.Session, "UserLogin") ?? null;
        public bool IsUserLogedIn => (this.UserLogin != null);
        public int UserId => (this.UserLogin != null) ? this.UserLogin.UserId : 0;
        public string UserFullName => (this.UserLogin != null) ? this.UserLogin.FullName : string.Empty;
        public string UserRole => (this.UserLogin != null) ? this.UserLogin.Role : string.Empty;
        public bool IsAdmin => (this.UserRole == WebConstants.CONST_ROLE_ADMIN);
    }
}
