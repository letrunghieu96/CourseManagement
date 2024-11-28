namespace CourseManagement.Domain.Users
{
    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUsersRepository
    {
        int Count(object parameters);
        IEnumerable<UserModel> Search(object parameters);
        UserModel CheckLogin(string userName, string passwordHash);
        UserModel Get(int userId);
        bool IsExistUserName(int userId, string userName);
        bool IsExistEmail(int userId, string email);
        int Insert(UserModel model);
        bool Update(int userId, UserModel model);
        bool UpdatePassword(int userId, string passwordHash, string lastChanged);
        bool Delete(int userId, string lastChanged);
    }
}
