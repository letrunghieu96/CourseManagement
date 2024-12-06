using System.ComponentModel.DataAnnotations;

namespace CourseManagement.ViewModels
{
    /// <summary>
    /// Login view model
    /// </summary>
    public class LoginViewModel
    {
        #region +Properties
        [Required(ErrorMessage = "Email là trường bắt buộc")]
        [EmailAddress(ErrorMessage = "Email sai định dạng")]
        [StringLength(255, ErrorMessage = "Email có chiều dài tối đa 255 kí tự")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc")]
        [StringLength(20, ErrorMessage = "Mật khẩu có chiều dài tối đa 20 kí tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool IsRemember { get; set; }
        public string? LoginMessage { get; set; }
        public string? ReturnUrl { get; set; }
        #endregion
    }
}
