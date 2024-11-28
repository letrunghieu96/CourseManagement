namespace CourseManagement.Domain.Courses
{
    /// <summary>
    /// Course repository interface
    /// </summary>
    public interface ICoursesRepository
    {
        int Count(object parameters);
        IEnumerable<CourseModel> Search(object parameters);
        CourseModel Get(int userId);
        bool IsExistCourseCode(int courseId, string courseCode);
        int Insert(CourseModel model);
        bool Update(int courseId, CourseModel model);
        bool Delete(int courseId, string lastChanged);
    }
}
