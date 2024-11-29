namespace CourseManagement.Models
{
    /// <summary>
    /// Login model
    /// </summary>
    public class LoginModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
