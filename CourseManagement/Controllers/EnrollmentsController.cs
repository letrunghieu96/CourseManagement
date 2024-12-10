using CourseManagement.Domain;
using CourseManagement.Domain.Enrollments.Helpers;
using CourseManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CourseManagement.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class EnrollmentsController : ControllerBase
    {
        public EnrollmentsController(IConfiguration config, IDomainFacade domainFacade)
            : base(config, domainFacade)
        {
        }

        [HttpGet]
        public IActionResult List()
        {
            var condition = new SearchCondition();
            if (this.UserRole == "User") condition.UserId = this.UserId;
            var viewModel = this.Service.CreateListViewModel(condition);
            TempData["paging"] = JsonConvert.SerializeObject(viewModel.Pagination);

            return View(WebConstants.VIEW_ENROLLMENTS_LIST, viewModel);
        }

        [HttpGet]
        public IActionResult Search(SearchCondition condition)
        {
            if (this.UserRole == "User") condition.UserId = this.UserId;
            var viewModel = this.Service.CreateListViewModel(condition);
            TempData["paging"] = JsonConvert.SerializeObject(viewModel.Pagination);

            return PartialView(WebConstants.PARTIAL_VIEW_ENROLLMENTS_SEARCH_RESULTS, viewModel.Results);
        }

        private EnrollmentsService Service => new EnrollmentsService(_config, _domainFacade);
    }
}
