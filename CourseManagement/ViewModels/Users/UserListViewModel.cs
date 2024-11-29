using CourseManagement.Domain.Users.Helpers;

namespace CourseManagement.ViewModels.Users
{
    /// <summary>
    /// User list view model
    /// </summary>
    public class UserListViewModel
    {
        public SearchCondition Condition { get; set; } = new SearchCondition();
        public SearchResult[] Results { get; set; } = new SearchResult[0];
        public PaginationViewModel Pagination { get; set; } = new PaginationViewModel();
    }
}
