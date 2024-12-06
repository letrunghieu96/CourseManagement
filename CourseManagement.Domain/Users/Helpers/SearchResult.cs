namespace CourseManagement.Domain.Users.Helpers
{
    /// <summary>
    /// Search result
    /// </summary>
    public class SearchResult
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
