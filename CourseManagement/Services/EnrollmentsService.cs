using CourseManagement.Domain;
using CourseManagement.Domain.Enrollments.Helpers;
using CourseManagement.ViewModels;
using CourseManagement.ViewModels.Enrollments;

namespace CourseManagement.Services
{
    public class EnrollmentsService : ServiceBase
    {
        public EnrollmentsService(IConfiguration config, IDomainFacade domainFacade) : base(config, domainFacade)
        {
        }

        public EnrollmentListViewModel CreateListViewModel(SearchCondition condition)
        {
            if (!string.IsNullOrEmpty(condition.DateFrom)) condition.DateFrom = ConvertDate(condition.DateFrom)?.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(condition.DateTo)) condition.DateTo = ConvertDate(condition.DateTo)?.ToString("yyyy-MM-dd");

            var total = _domainFacade.Enrollments.Count(condition);
            var searchResults = _domainFacade.Enrollments.Search(condition);
            var viewModel = new EnrollmentListViewModel
            {
                Condition = condition,
                Results = searchResults,
                Pagination = new PaginationViewModel
                {
                    PageLength = condition.PageLength,
                    CurrentPage = condition.PageNo,
                    Total = total,
                },
            };

            return viewModel;
        }
    }
}
