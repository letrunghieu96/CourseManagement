namespace CourseManagement.Domain.Users.Helpers
{
    /// <summary>
    /// Search result
    /// </summary>
    public class SearchResult
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
