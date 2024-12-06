namespace CourseManagement.Domain.Enrollments
{
    public class EnrollmentModel
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public DateTime EnrollmentTime { get; set; }
    }
}
