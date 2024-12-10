using CourseManagement.Domain;
using CourseManagement.Models;

namespace CourseManagement.Services
{
    public class HomeService : ServiceBase
    {
        public HomeService(IConfiguration config, IDomainFacade domainFacade) : base(config, domainFacade)
        {
        }

        public HomeModel CreateModel()
        {
            var model = new HomeModel
            {
                TotalCourses = _domainFacade.Courses.GetTotal(),
                TotalUsers = _domainFacade.Users.GetTotal(),
                LatestCourses = _domainFacade.Courses.GetTopLatest(5),
                TopRegistrations = _domainFacade.Courses.GetTopRegistrations(5),
            };
            return model;
        }
    }
}
