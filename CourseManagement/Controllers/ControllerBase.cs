using CourseManagement.Constants;
using CourseManagement.Domain;
using Microsoft.AspNetCore.Mvc;

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
            //if (!string.IsNullOrEmpty(pageLengths)) ConfigConstants.PageLengths = pageLengths.Split(",").Select(length => int.Parse(length)).ToArray();

            //ConfigConstants.ApplicationInfo = new ApplicationInfoModel
            //{
            //    CompanyName = _config.GetValue<string>("ApplicationInfo:CompanyName") ?? string.Empty,
            //    TaxCode = _config.GetValue<string>("ApplicationInfo:TaxCode") ?? string.Empty,
            //    Address = _config.GetValue<string>("ApplicationInfo:Address") ?? string.Empty,
            //    PhoneNumber = _config.GetValue<string>("ApplicationInfo:PhoneNumber") ?? string.Empty,
            //    Director = _config.GetValue<string>("ApplicationInfo:Director") ?? string.Empty,
            //};
        }
    }
}
