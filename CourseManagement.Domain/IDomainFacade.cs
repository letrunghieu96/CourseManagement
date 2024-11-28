using CourseManagement.Domain.Courses;
using CourseManagement.Domain.Users;

namespace CourseManagement.Domain
{
    /// <summary>
    /// Domain facade interface
    /// </summary>
    public interface IDomainFacade
    {
        IUsersRepository Users { get; }
        ICoursesRepository Courses { get; }

        void Commit();
        void Rollback();
    }
}
