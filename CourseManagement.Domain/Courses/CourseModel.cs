namespace CourseManagement.Domain.Courses
{
    /// <summary>
    /// Course model
    /// </summary>
    public class CourseModel : CourseExtendModel
    {
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseImage { get; set; }
        public string MainContent { get; set; }
        public string CourseFile { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public string Lecturer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string LastChanged { get; set; }
    }

    /// <summary>
    /// Course extend model
    /// </summary>
    public class CourseExtendModel
    {
    }
}
