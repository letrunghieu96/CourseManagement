using CourseManagement.Domain.Users.Helpers;

namespace CourseManagement.Domain.Users
{
    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUsersRepository
    {
        int Count(SearchCondition condition);
        SearchResult[] Search(SearchCondition condition);
        UserModel CheckLogin(string email, string passwordHash);
        UserModel Get(int userId);
        bool IsExistEmail(int userId, string email);
        int Insert(UserModel model);
        bool Update(int userId, UserModel model);
        bool UpdatePassword(int userId, string passwordHash, string lastChanged);
        bool Delete(int userId, string lastChanged);
    }
}
