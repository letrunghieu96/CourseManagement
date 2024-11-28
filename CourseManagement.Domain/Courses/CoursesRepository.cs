using CourseManagement.Sql.Queries;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CourseManagement.Domain.Courses
{
    internal class CoursesRepository : RepositoryBase, ICoursesRepository
    {
        public CoursesRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction)
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

        public IEnumerable<CourseModel> Search(object parameters)
        {
            try
            {
                var results = _dbConnection.Query<CourseModel>(
                    "spUsers_Search",
                    parameters,
                    transaction: _dbTransaction,
                    commandType: CommandType.StoredProcedure);
                return results;
            }
            catch
            {
            }

            return Enumerable.Empty<CourseModel>();
        }


        public CourseModel Get(int courseId)
        {
            try
            {
                var model = _dbConnection.QuerySingleOrDefault<CourseModel>(CoursesQuery.Get, new { courseId }, transaction: _dbTransaction);
                return model;
            }
            catch
            {
            }

            return null;
        }

        public bool IsExistCourseCode(int courseId, string courseCode)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("CourseId", courseId, DbType.Int32);
                parameters.Add("CourseCode", courseCode, DbType.String, size: 20);

                // Check
                var count = _dbConnection.QueryFirstOrDefault<int>(CoursesQuery.CheckExistCourseCode, parameters, transaction: _dbTransaction);
                return (count > 0);
            }
            catch
            {
            }

            return false;
        }


        public int Insert(CourseModel model)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("CourseCode", model.CourseCode, DbType.String, size: 50);
                parameters.Add("CourseName", model.CourseName, DbType.String, size: 255);
                parameters.Add("Description", model.Description, DbType.String);
                parameters.Add("Duration", model.Duration, DbType.Int32);
                parameters.Add("StartDate", model.StartDate, DbType.Date);
                parameters.Add("EndDate", model.EndDate, DbType.Date);
                parameters.Add("Price", model.Price, DbType.Decimal);
                parameters.Add("IsActive", model.IsActive, DbType.Int16);
                parameters.Add("LastChanged", model.LastChanged, DbType.String, size: 100);

                // Insert
                var id = _dbConnection.QueryFirstOrDefault<int>(CoursesQuery.Insert, parameters, transaction: _dbTransaction);
                return id;
            }
            catch
            {
            }

            return 0;
        }

        public bool Update(int courseId, CourseModel model)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("UserId", courseId, DbType.Int32);
                parameters.Add("CourseCode", model.CourseCode, DbType.String, size: 50);
                parameters.Add("CourseName", model.CourseName, DbType.String, size: 255);
                parameters.Add("Description", model.Description, DbType.String);
                parameters.Add("Duration", model.Duration, DbType.Int32);
                parameters.Add("StartDate", model.StartDate, DbType.Date);
                parameters.Add("EndDate", model.EndDate, DbType.Date);
                parameters.Add("Price", model.Price, DbType.Decimal);
                parameters.Add("IsActive", model.IsActive, DbType.Int16);
                parameters.Add("LastChanged", model.LastChanged, DbType.String, size: 100);

                // Update
                var rowsAffected = _dbConnection.Execute(CoursesQuery.Update, parameters, transaction: _dbTransaction);
                return (rowsAffected > 0);
            }
            catch
            {
            }

            return false;
        }

        public bool Delete(int courseId, string lastChanged)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("CourseId", courseId, DbType.Int32);
                parameters.Add("LastChanged", lastChanged, DbType.String, size: 100);

                // Delete
                var rowsAffected = _dbConnection.Execute(CoursesQuery.Delete, parameters, transaction: _dbTransaction);
                return (rowsAffected > 0);
            }
            catch
            {
            }

            return false;
        }
    }
}
