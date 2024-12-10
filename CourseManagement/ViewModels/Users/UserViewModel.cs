using System.ComponentModel.DataAnnotations;

namespace CourseManagement.ViewModels.Users
{
    /// <summary>
    /// User view model
    /// </summary>
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Email là trường bắt buộc")]
        [EmailAddress(ErrorMessage = "Email sai định dạng")]
        [StringLength(255, ErrorMessage = "Email có chiều dài tối đa 255 kí tự")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc")]
        [StringLength(20, ErrorMessage = "Mật khẩu có chiều dài tối đa 20 kí tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nhập lại mật khẩu là trường bắt buộc")]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên là trường bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ và tên có chiều dài tối đa 100 kí tự")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vai trò là trường bắt buộc")]
        public string UserRole { get; set; } = "User";

        public bool Status { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? LastChanged { get; set; }

        public string? MessageRegister { get; set; }
        public bool IsUpdate => (this.UserId > 0);
    }
}
