using CourseManagement.Domain.Courses;
using CourseManagement.Domain.Enrollments;
using CourseManagement.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace CourseManagement.Domain
{
    /// <summary>
    /// Service collection extension
    /// </summary>
    public static class ServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ICoursesRepository, CoursesRepository>();
            services.AddScoped<IEnrollmentsRepository, EnrollmentsRepository>();
            services.AddScoped<IDomainFacade, DomainFacade>();
        }
    }
}
