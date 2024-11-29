using CourseManagement.Constants;
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
                FullName = user.FullName,
                Role = user.Role,
            };
            return model;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="viewModel">User view model</param>
		/// <returns>True: if the create is successful, otherwise: False</returns>
        public bool Create(UserViewModel viewModel)
        {
            var model = new UserModel
            {
                FullName = viewModel.FullName,
                Email = viewModel.Email,
                PasswordHash = HashPassword(viewModel.Password ?? string.Empty),
                Role = viewModel.Role,
                IsActive = 1,
                LastChanged = WebConstants.CONST_LAST_CHANGED_USER,
            };
            var userId = _domainFacade.Users.Insert(model);
            return (userId > 0);
        }
    }
}
