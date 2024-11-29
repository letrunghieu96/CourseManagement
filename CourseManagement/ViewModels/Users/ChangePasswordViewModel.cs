using System.ComponentModel.DataAnnotations;

namespace CourseManagement.ViewModels.Users
{
    public class ChangePasswordViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc")]
        [StringLength(30, ErrorMessage = "Mật khẩu có chiều dài tối đa 30 kí tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu (xác nhận) là trường bắt buộc")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu và Mật khẩu (xác nhận) không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
