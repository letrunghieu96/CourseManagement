namespace CourseManagement.ViewModels.Courses
{
    /// <summary>
    /// Course view model
    /// </summary>
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public string? CreatorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? LastChanged { get; set; }
        public bool IsUpdate => (this.CourseId > 0);
    }
}
