using CourseManagement.Sql.Queries;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CourseManagement.Domain.Users
{
    internal class UsersRepository : RepositoryBase, IUsersRepository
    {
        public UsersRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction)
            : base(sqlConnection, dbTransaction)
        {
        }

        public int Count(object parameters)
        {
            try
            {
                var count = _dbConnection.ExecuteScalar<int>(
                    "spUsers_Count",
                    parameters,
                    transaction: _dbTransaction,
                    commandType: CommandType.StoredProcedure);
                return count;
            }
            catch
            {
            }

            return 0;
        }

        public IEnumerable<UserModel> Search(object parameters)
        {
            try
            {
                var results = _dbConnection.Query<UserModel>(
                    "spUsers_Search",
                    parameters,
                    transaction: _dbTransaction,
                    commandType: CommandType.StoredProcedure);
                return results;
            }
            catch
            {
            }

            return Enumerable.Empty<UserModel>();
        }


        public UserModel CheckLogin(string userName, string passwordHash)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("UserName", userName, DbType.String, size: 255);
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

        public bool IsExistUserName(int userId, string userName)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("UserId", userId, DbType.Int32);
                parameters.Add("UserName", userName, DbType.String, size: 255);

                // Check
                var count = _dbConnection.QueryFirstOrDefault<int>(UsersQuery.CheckExistUserName, parameters, transaction: _dbTransaction);
                return (count > 0);
            }
            catch
            {
            }

            return false;
        }

        public bool IsExistEmail(int userId, string email)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("UserId", userId, DbType.Int32);
                parameters.Add("Email", email, DbType.String, size: 255);

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
                parameters.Add("UserName", model.UserName, DbType.String, size: 50);
                parameters.Add("PasswordHash", model.PasswordHash, DbType.String, size: 255);
                parameters.Add("FullName", model.FullName, DbType.String, size: 100);
                parameters.Add("Email", model.Email, DbType.String, size: 255);
                parameters.Add("Role", model.Role, DbType.String, size: 20);
                parameters.Add("IsActive", model.IsActive, DbType.Int16);
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
                parameters.Add("UserName", model.UserName, DbType.String, size: 50);
                parameters.Add("FullName", model.FullName, DbType.String, size: 100);
                parameters.Add("Email", model.Email, DbType.String, size: 255);
                parameters.Add("Role", model.Role, DbType.String, size: 20);
                parameters.Add("IsActive", model.IsActive, DbType.Int16);
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

        public bool Delete(int userId, string lastChanged)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("UserId", userId, DbType.Int32);
                parameters.Add("LastChanged", lastChanged, DbType.String, size: 100);

                // Delete
                var rowsAffected = _dbConnection.Execute(UsersQuery.Delete, parameters, transaction: _dbTransaction);
                return (rowsAffected > 0);
            }
            catch
            {
            }

            return false;
        }
    }
}
