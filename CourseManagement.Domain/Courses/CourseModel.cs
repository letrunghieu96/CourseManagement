namespace CourseManagement.Domain.Courses
{
    /// <summary>
    /// Course model
    /// </summary>
    public class CourseModel : ModelBase
    {
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public int IsActive { get; set; }
    }
}
