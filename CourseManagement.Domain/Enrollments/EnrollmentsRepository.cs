using CourseManagement.Domain.Enrollments.Helpers;
using CourseManagement.Sql.Queries;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CourseManagement.Domain.Enrollments
{
    /// <summary>
    /// Enrollments repository interface
    /// </summary>
    public interface IEnrollmentsRepository
    {
        int Count(SearchCondition condition);
        SearchResult[] Search(SearchCondition condition);
        bool CheckExistEnrollment(int courseId, int userId);
        bool Insert(EnrollmentModel model);
        bool Delete(int courseId, int userId);
    }

    /// <summary>
    /// Enrollments repository
    /// </summary>
    internal class EnrollmentsRepository : RepositoryBase, IEnrollmentsRepository
    {
        public EnrollmentsRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction)
            : base(sqlConnection, dbTransaction)
        {
        }

        public int Count(SearchCondition condition)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("SearchWord", condition.SearchWord ?? string.Empty, DbType.String, size: 100);
                parameters.Add("DateFrom", condition.DateFrom, DbType.Date);
                parameters.Add("DateTo", condition.DateTo, DbType.Date);
                parameters.Add("Status", condition.Status, DbType.Int16);
                parameters.Add("Lecturer", condition.Lecturer ?? string.Empty, DbType.String, size: 100);
                parameters.Add("UserName", condition.UserName ?? string.Empty, DbType.String, size: 100);
                parameters.Add("UserId", condition.UserId, DbType.Int32);

                var count = _dbConnection.ExecuteScalar<int>(EnrollmentsQuery.Count, parameters, transaction: _dbTransaction);
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
                parameters.Add("DateFrom", condition.DateFrom, DbType.Date);
                parameters.Add("DateTo", condition.DateTo, DbType.Date);
                parameters.Add("Status", condition.Status, DbType.Int16);
                parameters.Add("Lecturer", condition.Lecturer ?? string.Empty, DbType.String, size: 100);
                parameters.Add("UserName", condition.UserName ?? string.Empty, DbType.String, size: 100);
                parameters.Add("UserId", condition.UserId, DbType.Int32);
                parameters.Add("BeginRowNum", condition.BeginRowNum, DbType.Int32);
                parameters.Add("RowsOfPage", condition.PageLength, DbType.Int32);
                var orderBy = Enum.TryParse(condition.OrderBy, out EnrollmentSortByEnum sortBy) ? (int)sortBy : 0;
                parameters.Add("OrderBy", "1", DbType.String, size: 2);

                var results = _dbConnection.Query<SearchResult>(EnrollmentsQuery.Search, parameters, transaction: _dbTransaction);
                return results.ToArray();
            }
            catch
            {
            }

            return new SearchResult[0];
        }

        public bool CheckExistEnrollment(int courseId, int userId)
        {
            try
            {
                // Check
                var count = _dbConnection.QueryFirstOrDefault<int>(EnrollmentsQuery.CheckExistEnrollment, new { courseId , userId }, transaction: _dbTransaction);
                return (count > 0);
            }
            catch
            {
            }

            return false;
        }

        public bool Insert(EnrollmentModel model)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("CourseId", model.CourseId, DbType.Int32);
                parameters.Add("UserId", model.UserId, DbType.Int32);

                // Insert
                var rowsAffected = _dbConnection.Execute(EnrollmentsQuery.Insert, parameters, transaction: _dbTransaction);
                return (rowsAffected > 0);
            }
            catch
            {
            }

            return false;
        }

        public bool Delete(int courseId, int userId)
        {
            try
            {
                // Delete
                var rowsAffected = _dbConnection.Execute(EnrollmentsQuery.Delete, new { courseId, userId }, transaction: _dbTransaction);
                return (rowsAffected > 0);
            }
            catch
            {
            }

            return false;
        }
    }
}
