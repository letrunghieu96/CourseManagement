using System.ComponentModel.DataAnnotations;

namespace CourseManagement.ViewModels.Courses
{
    /// <summary>
    /// Course view model
    /// </summary>
    public class CourseViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Mã khóa học là trường bắt buộc")]
        [StringLength(20, ErrorMessage = "Mã khóa học có chiều dài tối đa 20 kí tự")]
        public string? CourseCode { get; set; }

        [Required(ErrorMessage = "Tên khóa học là trường bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên khóa học có chiều dài tối đa 255 kí tự")]
        public string? CourseName { get; set; }

        [Required(ErrorMessage = "Thời lượng là trường bắt buộc")]
        public string? MainContent { get; set; }

        [Required(ErrorMessage = "Thời lượng là trường bắt buộc")]
        public string? Duration { get; set; }

        [Required(ErrorMessage = "Bắt đầu là trường bắt buộc")]
        public string? StartDate { get; set; }

        [Required(ErrorMessage = "Kết thúc là trường bắt buộc")]
        public string? EndDate { get; set; }

        [Required(ErrorMessage = "Giá tiền là trường bắt buộc")]
        public string? Price { get; set; }

        [Required(ErrorMessage = "Giảng viên là trường bắt buộc")]
        [StringLength(100, ErrorMessage = "Giảng viên có chiều dài tối đa 100 kí tự")]
        public string? Lecturer { get; set; }

        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? LastChanged { get; set; }
        public bool IsExistEnrollment { get; set; }
        public bool IsUpdate => (this.CourseId > 0);
    }
}
