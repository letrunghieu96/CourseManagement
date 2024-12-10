using CourseManagement.Domain.Users.Helpers;
using CourseManagement.Sql.Queries;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CourseManagement.Domain.Users
{
    /// <summary>
    /// Users repository interface
    /// </summary>
    public interface IUsersRepository
    {
        int GetTotal();
        int Count(SearchCondition condition);
        SearchResult[] Search(SearchCondition condition);
        UserModel CheckLogin(string email, string passwordHash);
        UserModel Get(int userId);
        bool IsExistEmail(string email, int? userId = null);
        int Insert(UserModel model);
        bool Update(int userId, UserModel model);
        bool UpdateUser(int userId, UserModel model);
        bool UpdatePassword(int userId, string passwordHash, string lastChanged);
        bool Delete(int userId);
    }

    /// <summary>
    /// Users repository
    /// </summary>
    internal class UsersRepository : RepositoryBase, IUsersRepository
    {
        public UsersRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction)
            : base(sqlConnection, dbTransaction)
        {
        }

        public int GetTotal()
        {
            try
            {
                var total = _dbConnection.ExecuteScalar<int>(UsersQuery.GetTotal, transaction: _dbTransaction);
                return total;
            }
            catch
            {
            }

            return 0;
        }

        public int Count(SearchCondition condition)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("SearchWord", condition.SearchWord ?? string.Empty, DbType.String, size: 100);
                parameters.Add("Status", condition.Status, DbType.Int16);
                parameters.Add("UserRole", condition.UserRole ?? string.Empty, DbType.String, size: 20);

                // Count
                var count = _dbConnection.ExecuteScalar<int>(UsersQuery.Count, parameters, transaction: _dbTransaction);
                return count;
            }
            catch
            {
            }

            return 0;
        }

        public SearchResult[] Search(SearchCondition condition)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("SearchWord", condition.SearchWord ?? string.Empty, DbType.String, size: 100);
                parameters.Add("Status", condition.Status, DbType.Int16);
                parameters.Add("UserRole", condition.UserRole ?? string.Empty, DbType.String, size: 20);
                parameters.Add("BeginRowNum", condition.BeginRowNum, DbType.Int32);
                parameters.Add("RowsOfPage", condition.PageLength, DbType.Int32);
                var orderBy = Enum.TryParse(condition.OrderBy, out UserSortByEnum sortBy) ? (int)sortBy : 0;
                parameters.Add("OrderBy", orderBy, DbType.String, size: 2);

                // Search
                var results = _dbConnection.Query<SearchResult>(UsersQuery.Search, parameters, transaction: _dbTransaction);
                return results.ToArray();
            }
            catch
            {
            }

            return new SearchResult[0];
        }

        public UserModel CheckLogin(string email, string passwordHash)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("Email", email, DbType.String, size: 255);
                parameters.Add("PasswordHash", passwordHash, DbType.String, size: 255);

                var model = _dbConnection.QuerySingleOrDefault<UserModel>(UsersQuery.CheckLogin, parameters, transaction: _dbTransaction);
                return model;
            }
            catch
            {
            }

            return null;
        }

        public UserModel Get(int userId)
        {
            try
            {
                var model = _dbConnection.QuerySingleOrDefault<UserModel>(UsersQuery.Get, new { userId }, transaction: _dbTransaction);
                return model;
            }
            catch
            {
            }

            return null;
        }

        public bool IsExistEmail(string email, int? userId)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("Email", email, DbType.String, size: 255);
                parameters.Add("UserId", userId, DbType.Int32);

                // Check
                var count = _dbConnection.QueryFirstOrDefault<int>(UsersQuery.CheckExistEmail, parameters, transaction: _dbTransaction);
                return (count > 0);
            }
            catch
            {
            }

            return false;
        }

        public int Insert(UserModel model)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("UserName", model.UserName, DbType.String, size: 100);
                parameters.Add("Email", model.Email, DbType.String, size: 255);
                parameters.Add("PasswordHash", model.PasswordHash, DbType.String, size: 255);
                parameters.Add("UserRole", model.UserRole, DbType.String, size: 20);
                parameters.Add("Status", model.Status, DbType.Int16);
                parameters.Add("LastChanged", model.LastChanged, DbType.String, size: 100);

                // Insert
                var id = _dbConnection.QueryFirstOrDefault<int>(UsersQuery.Insert, parameters, transaction: _dbTransaction);
                return id;
            }
            catch
            {
            }

            return 0;
        }

        public bool Update(int userId, UserModel model)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("UserId", userId, DbType.Int32);
                parameters.Add("UserName", model.UserName, DbType.String, size: 100);
                parameters.Add("Email", model.Email, DbType.String, size: 255);
                parameters.Add("UserRole", model.UserRole, DbType.String, size: 20);
                parameters.Add("Status", model.Status, DbType.Int16);
                parameters.Add("LastChanged", model.LastChanged, DbType.String, size: 100);

                // Update
                var rowsAffected = _dbConnection.Execute(UsersQuery.Update, parameters, transaction: _dbTransaction);
                return (rowsAffected > 0);
            }
            catch
            {
            }

            return false;
        }

        public bool UpdateUser(int userId, UserModel model)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("UserId", userId, DbType.Int32);
                parameters.Add("UserName", model.UserName, DbType.String, size: 100);
                parameters.Add("Email", model.Email, DbType.String, size: 255);
                parameters.Add("LastChanged", model.LastChanged, DbType.String, size: 100);

                // Update
                var rowsAffected = _dbConnection.Execute(UsersQuery.UpdateUser, parameters, transaction: _dbTransaction);
                return (rowsAffected > 0);
            }
            catch
            {
            }

            return false;
        }

        public bool UpdatePassword(int userId, string passwordHash, string lastChanged)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("UserId", userId, DbType.Int32);
                parameters.Add("PasswordHash", passwordHash, DbType.String, size: 255);
                parameters.Add("LastChanged", lastChanged, DbType.String, size: 100);

                var rowsAffected = _dbConnection.Execute(UsersQuery.UpdatePassword, parameters, transaction: _dbTransaction);
                return (rowsAffected > 0);
            }
            catch
            {
            }

            return false;
        }

        public bool Delete(int userId)
        {
            try
            {
                // Delete
                var rowsAffected = _dbConnection.Execute(UsersQuery.Delete, new { userId }, transaction: _dbTransaction);
                return (rowsAffected > 0);
            }
            catch
            {
            }

            return false;
        }
    }
}
