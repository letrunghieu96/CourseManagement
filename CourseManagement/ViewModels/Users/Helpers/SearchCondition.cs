namespace CourseManagement.ViewModels.Users.Helpers
{
    /// <summary>
    /// Search condition
    /// </summary>
    public class SearchCondition
    {
        public string? SearchWord { get; set; }
        public int? IsActive { get; set; }
        public string? Role { get; set; }
        public int PageNo { get; set; }
        public string? OrderBy { get; set; }
    }
}
