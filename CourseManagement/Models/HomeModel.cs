namespace CourseManagement.Models
{
    public class HomeModel
    {
        public int TotalUsers {  get; set; }
        public int TotalCourses {  get; set; }
        public Domain.Users.Helpers.SearchResult[] LatestUsers { get; set; } = new Domain.Users.Helpers.SearchResult[0];
        public Domain.Courses.Helpers.SearchResult[] LatestCourses {  get; set; } = new Domain.Courses.Helpers.SearchResult[0];
    }
}
