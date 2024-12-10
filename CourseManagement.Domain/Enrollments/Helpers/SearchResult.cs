namespace CourseManagement.Domain.Enrollments.Helpers
{
    public class SearchResult
    {
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public string Lecturer { get; set; }
        public DateTime EnrollmentTime { get; set; }
        public string UserName { get; set; }
    }
}
