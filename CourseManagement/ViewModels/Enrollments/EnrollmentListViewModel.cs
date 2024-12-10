using CourseManagement.Domain.Enrollments.Helpers;

namespace CourseManagement.ViewModels.Enrollments
{
    public class EnrollmentListViewModel
    {
        public SearchCondition Condition { get; set; } = new SearchCondition();
        public SearchResult[] Results { get; set; } = new SearchResult[0];
        public PaginationViewModel Pagination { get; set; } = new PaginationViewModel();
    }
}
