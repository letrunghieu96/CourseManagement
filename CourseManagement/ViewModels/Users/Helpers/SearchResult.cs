namespace CourseManagement.ViewModels.Users.Helpers
{
    public class SearchResult
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public int IsActive { get; set; }
    }
}
