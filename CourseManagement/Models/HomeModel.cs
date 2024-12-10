using CourseManagement.Domain.Courses.Helpers;

namespace CourseManagement.Models
{
    public class HomeModel
    {
        public int TotalUsers { get; set; }
        public int TotalCourses { get; set; }
        public SearchResult[] TopRegistrations { get; set; } = new SearchResult[0];
        public SearchResult[] LatestCourses { get; set; } = new SearchResult[0];
    }
}
