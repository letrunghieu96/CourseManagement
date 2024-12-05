using CourseManagement.Domain.Courses.Helpers;

namespace CourseManagement.ViewModels.Courses
{
    /// <summary>
    /// Course list view model
    /// </summary>
    public class CourseListViewModel
    {
        public SearchCondition Condition { get; set; } = new SearchCondition();
        public SearchResult[] Results { get; set; } = new SearchResult[0];
        public PaginationViewModel Pagination { get; set; } = new PaginationViewModel();
    }
}
