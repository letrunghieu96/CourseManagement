using CourseManagement.Domain;
using CourseManagement.Domain.Users;
using CourseManagement.Models;
using CourseManagement.ViewModels.Users;

namespace CourseManagement.Services
{
    /// <summary>
    /// Index service
    /// </summary>
    public class IndexService : ServiceBase
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

        public LoginModel? Login(string email, string password)
        {
            var user = _domainFacade.Users.CheckLogin(email, HashPassword(password));
            if (user == null) return null;

            var model = new LoginModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserRole = user.UserRole,
            };
            return model;
        }

        public int Create(UserViewModel viewModel)
        {
            var model = new UserModel
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                PasswordHash = HashPassword(viewModel.Password ?? string.Empty),
                UserRole = viewModel.UserRole,
                Status = 1,
                LastChanged = "User",
            };
            var userId = _domainFacade.Users.Insert(model);
            if (userId > 0) _domainFacade.Commit();

            return userId;
        }
    }
}
