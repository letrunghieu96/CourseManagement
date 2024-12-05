using CourseManagement.Domain;
using CourseManagement.Domain.Users;
using CourseManagement.Domain.Users.Helpers;
using CourseManagement.ViewModels;
using CourseManagement.ViewModels.Users;

namespace CourseManagement.Services
{
    /// <summary>
    /// Users service
    /// </summary>
    public class UsersService : ServiceBase
    {
        #region +Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Configuration interface</param>
        public UsersService(IConfiguration config, IDomainFacade domainFacade) : base(config, domainFacade)
        {
        }
        #endregion

        public UserListViewModel CreateListViewModel(SearchCondition condition)
        {
            var total = _domainFacade.Users.Count(condition);
            var searchResults = _domainFacade.Users.Search(condition);
            var viewModel = new UserListViewModel
            {
                Condition = condition,
                Results = searchResults,
                Pagination = new PaginationViewModel
                {
                    PageLength = condition.PageLength,
                    CurrentPage = condition.PageNo,
                    Total = total,
                },
            };

            return viewModel;
        }

        public UserViewModel Get(int userId)
        {
            var viewModel = new UserViewModel
            {
                UserId = userId,
            };
            var model = _domainFacade.Users.Get(userId);
            if (model == null) return viewModel;

            viewModel.FullName = model.FullName;
            viewModel.Email = model.Email;
            viewModel.Role = model.Role;
            viewModel.IsActive = (model.IsActive == 1);
            viewModel.CreatedAt = model.CreatedAt;
            viewModel.UpdatedAt = model.UpdatedAt;
            viewModel.LastChanged = model.LastChanged;

            return viewModel;
        }

        public bool Create(UserViewModel viewModel)
        {
            var model = new UserModel
            {
                FullName = viewModel.FullName,
                Email = viewModel.Email,
                Role = viewModel.Role,
                PasswordHash = HashPassword(viewModel.Password),
                IsActive = viewModel.IsActive ? 1 : 0,
                LastChanged = viewModel.LastChanged,
            };
            var userId = _domainFacade.Users.Insert(model);
            if (userId > 0) _domainFacade.Commit();

            return (userId > 0);
        }

        public bool Update(UserViewModel viewModel)
        {
            var model = new UserModel
            {
                FullName = viewModel.FullName,
                Email = viewModel.Email,
                Role = viewModel.Role,
                IsActive = viewModel.IsActive ? 1 : 0,
                LastChanged = viewModel.LastChanged,
            };
            var isSuccess = _domainFacade.Users.Update(viewModel.UserId, model);
            if (isSuccess) _domainFacade.Commit();

            return isSuccess;
        }

        public bool ChangePassword(int userId, string password, string lastChanged)
        {
            var isSuccess = _domainFacade.Users.UpdatePassword(userId, HashPassword(password), lastChanged);
            if (isSuccess) _domainFacade.Commit();

            return isSuccess;
        }

        public bool Delete(int userId)
        {
            var isSuccess = _domainFacade.Users.Delete(userId);
            if (isSuccess) _domainFacade.Commit();

            return isSuccess;
        }
    }
}
