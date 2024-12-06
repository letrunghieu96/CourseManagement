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
