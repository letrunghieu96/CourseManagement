using CourseManagement.Domain;
using CourseManagement.Domain.Users;
using CourseManagement.ViewModels.Users;

namespace CourseManagement.Services
{
    /// <summary>
    /// Index service
    /// </summary>
    public class IndexService : WorkServiceBase
    {
        #region +Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Configuration interface</param>
        public IndexService(IConfiguration config, IDomainFacade domainFacade) : base(config, domainFacade)
        {
        }
        #endregion

        public bool Insert(UserViewModel viewModel)
        {
            var userModel = new UserModel
            {
                UserName = viewModel.UserName,
                PasswordHash = viewModel.Password,
                FullName = viewModel.FullName,
                Email = viewModel.Email,
                Role = viewModel.Role,
                IsActive = 1,
            };
            var userId = _domainFacade.Users.Insert(userModel);

            return (userId > 0);
        }
    }
}
