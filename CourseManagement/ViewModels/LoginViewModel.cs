using System.ComponentModel.DataAnnotations;

namespace CourseManagement.ViewModels
{
    /// <summary>
    /// Login view model
    /// </summary>
    public class LoginViewModel
    {
        #region +Properties
        [Required(ErrorMessage = "Tên đăng nhập là trường bắt buộc")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool IsRemember { get; set; }
        public string? LoginMessage { get; set; }
        public string? ReturnUrl { get; set; }
        #endregion
    }
}
