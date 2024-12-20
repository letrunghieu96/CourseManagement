using Azure;
using CourseManagement.Domain;
using CourseManagement.Domain.Courses;
using CourseManagement.Domain.Courses.Helpers;
using CourseManagement.Domain.Enrollments;
using CourseManagement.ViewModels;
using CourseManagement.ViewModels.Courses;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace CourseManagement.Services
{
    public class CoursesService : ServiceBase
    {
        /// <summary>Upload file extensions</summary>
        private readonly string[] _uploadImageExtensions;
        private readonly string[] _uploadFileExtensions;
        private readonly string _uploadCoursesFolderPath;

        public CoursesService(IConfiguration config, IDomainFacade domainFacade) : base(config, domainFacade)
        {
            _uploadImageExtensions = _config.GetValue<string>("UploadImageExtensions").Split(",");
            _uploadFileExtensions = _config.GetValue<string>("UploadFileExtensions").Split(",");

#if DEBUG
            _uploadCoursesFolderPath = WebConstants.FOLDER_PATH_COURSES;
#else
            _uploadCoursesFolderPath = Path.Combine(_config.GetValue<string>("PhysicalPath"), WebConstants.FOLDER_PATH_COURSES);
#endif
        }

        public CourseListViewModel CreateListViewModel(SearchCondition condition)
        {
            if (!string.IsNullOrEmpty(condition.StartDateFrom)) condition.StartDateFrom = ConvertDate(condition.StartDateFrom)?.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(condition.StartDateTo)) condition.StartDateTo = ConvertDate(condition.StartDateTo)?.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(condition.EndDateFrom)) condition.EndDateFrom = ConvertDate(condition.EndDateFrom)?.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(condition.EndDateTo)) condition.EndDateTo = ConvertDate(condition.EndDateTo)?.ToString("yyyy-MM-dd");

            var total = _domainFacade.Courses.Count(condition);
            var searchResults = _domainFacade.Courses.Search(condition);
            var viewModel = new CourseListViewModel
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

        public CourseViewModel Get(int courseId, int userId)
        {
            var viewModel = new CourseViewModel
            {
                CourseId = courseId,
                StartDate = DateTime.Now.ToString(WebConstants.DATE_FORMAT_VN),
            };
            var model = _domainFacade.Courses.Get(courseId);
            if (model == null) return viewModel;

            viewModel.CourseCode = model.CourseCode;
            viewModel.CourseName = model.CourseName;
            viewModel.MainContent = model.MainContent;
            viewModel.Duration = model.Duration.ToString("#,###");
            viewModel.StartDate = model.StartDate.ToString(WebConstants.DATE_FORMAT_VN);
            viewModel.EndDate = model.EndDate.ToString(WebConstants.DATE_FORMAT_VN);
            viewModel.Price = model.Price.ToString("#,###");
            viewModel.Status = model.Status;
            viewModel.Lecturer = model.Lecturer;
            viewModel.CreatedAt = model.CreatedAt;
            viewModel.UpdatedAt = model.UpdatedAt;
            viewModel.LastChanged = model.LastChanged;
            if (model.Status == 0)
            {
                var isExistEnrollment = _domainFacade.Enrollments.CheckExistEnrollment(courseId, userId);
                viewModel.CanRegister = !isExistEnrollment;
                viewModel.CanCancel = isExistEnrollment;
            }

            return viewModel;
        }

        public bool IsExistCourseCode(string? courseCode, int? courseId)
        {
            if (string.IsNullOrEmpty(courseCode)) return false;

            var isExist = _domainFacade.Courses.IsExistCourseCode(courseCode, courseId);
            return isExist;
        }

        public bool Create(CourseViewModel viewModel)
        {
            var model = new CourseModel
            {
                CourseCode = viewModel.CourseCode,
                CourseName = viewModel.CourseName,
                CourseImage = viewModel.CourseImage?.FileName,
                MainContent = viewModel.MainContent ?? string.Empty,
                Duration = int.Parse(viewModel.Duration ?? "0"),
                StartDate = ConvertDate(viewModel.StartDate) ?? DateTime.Now,
                EndDate = ConvertDate(viewModel.EndDate) ?? DateTime.Now,
                Price = decimal.Parse(viewModel.Price ?? "0"),
                Status = 0,
                Lecturer = viewModel.Lecturer,
                LastChanged = viewModel.LastChanged,
            };
            // Insert
            var courseId = _domainFacade.Courses.Insert(model);
            if (courseId > 0)
            {
                viewModel.CourseId = courseId;
                _domainFacade.Commit();

                return true;
            }

            return false;
        }

        public bool Update(CourseViewModel viewModel)
        {
            var model = new CourseModel
            {
                CourseCode = viewModel.CourseCode,
                CourseName = viewModel.CourseName,
                MainContent = viewModel.MainContent ?? string.Empty,
                Duration = int.Parse(viewModel.Duration ?? "0", NumberStyles.AllowThousands),
                StartDate = ConvertDate(viewModel.StartDate) ?? DateTime.Now,
                EndDate = ConvertDate(viewModel.EndDate) ?? DateTime.Now,
                Price = decimal.Parse(viewModel.Price ?? "0"),
                Status = viewModel.Status,
                Lecturer = viewModel.Lecturer,
                LastChanged = viewModel.LastChanged,
            };
            // Update
            var isSuccess = _domainFacade.Courses.Update(viewModel.CourseId, model);
            if (isSuccess) _domainFacade.Commit();

            return isSuccess;
        }

        public bool Delete(int courseId)
        {
            // Delete
            var isSuccess = _domainFacade.Courses.Delete(courseId);
            if (isSuccess) _domainFacade.Commit();

            return isSuccess;
        }

        public bool RegisterEnrollment(int courseId, int userId)
        {
            var model = new EnrollmentModel
            {
                CourseId = courseId,
                UserId = userId,
            };

            // Insert
            var isSuccess = _domainFacade.Enrollments.Insert(model);
            if (isSuccess) _domainFacade.Commit();

            return isSuccess;
        }

        public bool DeleteEnrollment(int courseId, int userId)
        {
            // Delete
            var isSuccess = _domainFacade.Enrollments.Delete(courseId, userId);
            if (isSuccess) _domainFacade.Commit();

            return isSuccess;
        }
    }
}
