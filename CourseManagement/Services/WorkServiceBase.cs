using CourseManagement.Domain;

namespace CourseManagement.Services
{
    public class WorkServiceBase
    {
        /// <summary>Configuration interface</summary>
        private readonly IConfiguration _config;
        protected readonly IDomainFacade _domainFacade;

        public WorkServiceBase(IConfiguration config, IDomainFacade domainFacade)
        {
            _config = config;
            _domainFacade = domainFacade;
        }
    }
}
