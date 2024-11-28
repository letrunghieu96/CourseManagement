using CourseManagement.Domain.Courses;
using CourseManagement.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace CourseManagement.Domain.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ICoursesRepository, CoursesRepository>();
            services.AddScoped<IDomainFacade, DomainFacade>();
        }
    }
}
