namespace CourseManagement.Models
{
    /// <summary>
    /// Login model
    /// </summary>
    public class LoginModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
    }
}
