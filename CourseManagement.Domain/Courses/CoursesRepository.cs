using CourseManagement.Domain.Courses.Helpers;
using CourseManagement.Sql.Queries;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CourseManagement.Domain.Courses
{
    /// <summary>
    /// Courses repository interface
    /// </summary>
    public interface ICoursesRepository
    {
        int GetTotal();
        SearchResult[] GetTopLatest(int top);
        SearchResult[] GetTopRegistrations(int top);
        int Count(SearchCondition condition);
        SearchResult[] Search(SearchCondition condition);
        CourseModel Get(int userId);
        bool IsExistCourseCode(string courseCode, int? courseId = null);
        int Insert(CourseModel model);
        bool Update(int courseId, CourseModel model);
        bool Delete(int courseId);
    }

    /// <summary>
    /// Courses repository
    /// </summary>
    internal class CoursesRepository : RepositoryBase, ICoursesRepository
    {
        public CoursesRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction)
            : base(sqlConnection, dbTransaction)
        {
        }

        public int GetTotal()
        {
            try
            {
                var total = _dbConnection.ExecuteScalar<int>(CoursesQuery.GetTotal, transaction: _dbTransaction);
                return total;
            }
            catch
            {
            }

            return 0;
        }

        public SearchResult[] GetTopLatest(int top)
        {
            try
            {
                var results = _dbConnection.Query<SearchResult>(CoursesQuery.GetTopLatest, new { top }, transaction: _dbTransaction);
                return results.ToArray();
            }
            catch
            {
            }

            return new SearchResult[0];
        }

        public SearchResult[] GetTopRegistrations(int top)
        {
            try
            {
                var results = _dbConnection.Query<SearchResult>(CoursesQuery.GetTopRegistrations, new { top }, transaction: _dbTransaction);
                return results.ToArray();
            }
            catch
            {
            }

            return new SearchResult[0];
        }

        public int Count(SearchCondition condition)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("SearchWord", condition.SearchWord ?? string.Empty, DbType.String, size: 100);
                parameters.Add("StartDateFrom", condition.StartDateFrom, DbType.Date);
                parameters.Add("StartDateTo", condition.StartDateTo, DbType.Date);
                parameters.Add("EndDateFrom", condition.EndDateFrom, DbType.Date);
                parameters.Add("EndDateTo", condition.EndDateTo, DbType.Date);
                parameters.Add("Status", condition.Status, DbType.Int16);
                parameters.Add("Lecturer", condition.Lecturer ?? string.Empty, DbType.String, size: 100);

                var count = _dbConnection.ExecuteScalar<int>(CoursesQuery.Count, parameters, transaction: _dbTransaction);
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
                parameters.Add("StartDateFrom", condition.StartDateFrom, DbType.Date);
                parameters.Add("StartDateTo", condition.StartDateTo, DbType.Date);
                parameters.Add("EndDateFrom", condition.EndDateFrom, DbType.Date);
                parameters.Add("EndDateTo", condition.EndDateTo, DbType.Date);
                parameters.Add("Status", condition.Status, DbType.Int16);
                parameters.Add("Lecturer", condition.Lecturer ?? string.Empty, DbType.String, size: 100);
                parameters.Add("BeginRowNum", condition.BeginRowNum, DbType.Int32);
                parameters.Add("RowsOfPage", condition.PageLength, DbType.Int32);
                var orderBy = Enum.TryParse(condition.OrderBy, out CourseSortByEnum sortBy) ? (int)sortBy : 0;
                parameters.Add("OrderBy", orderBy, DbType.String, size: 2);

                var results = _dbConnection.Query<SearchResult>(CoursesQuery.Search, parameters, transaction: _dbTransaction);
                return results.ToArray();
            }
            catch
            {
            }

            return new SearchResult[0];
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

        public bool IsExistCourseCode(string courseCode, int? courseId)
        {
            try
            {
                // Parameters
                var parameters = new DynamicParameters();
                parameters.Add("CourseCode", courseCode, DbType.String, size: 20);
                parameters.Add("CourseId", courseId, DbType.Int32);

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
                parameters.Add("CourseImage", model.CourseImage, DbType.String, size: 255);
                parameters.Add("MainContent", model.MainContent, DbType.String);
                parameters.Add("CourseFile", model.CourseFile, DbType.String, size: 255);
                parameters.Add("Duration", model.Duration, DbType.Int32);
                parameters.Add("StartDate", model.StartDate, DbType.Date);
                parameters.Add("EndDate", model.EndDate, DbType.Date);
                parameters.Add("Price", model.Price, DbType.Decimal);
                parameters.Add("Status", model.Status, DbType.Int16);
                parameters.Add("Lecturer", model.Lecturer, DbType.String, size: 100);
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
                parameters.Add("CourseId", courseId, DbType.Int32);
                parameters.Add("CourseCode", model.CourseCode, DbType.String, size: 50);
                parameters.Add("CourseName", model.CourseName, DbType.String, size: 255);
                parameters.Add("CourseImage", model.CourseImage, DbType.String, size: 255);
                parameters.Add("MainContent", model.MainContent, DbType.String);
                parameters.Add("CourseFile", model.CourseFile, DbType.String, size: 255);
                parameters.Add("Duration", model.Duration, DbType.Int32);
                parameters.Add("StartDate", model.StartDate, DbType.Date);
                parameters.Add("EndDate", model.EndDate, DbType.Date);
                parameters.Add("Price", model.Price, DbType.Decimal);
                parameters.Add("Status", model.Status, DbType.Int16);
                parameters.Add("Lecturer", model.Lecturer, DbType.String, size: 100);
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

        public bool Delete(int courseId)
        {
            try
            {
                // Delete
                var rowsAffected = _dbConnection.Execute(CoursesQuery.Delete, new { courseId }, transaction: _dbTransaction);
                return (rowsAffected > 0);
            }
            catch
            {
            }

            return false;
        }
    }
}
