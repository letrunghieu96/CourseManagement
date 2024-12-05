using CourseManagement.Constants;
using CourseManagement.Domain;
using CourseManagement.Domain.Courses;
using CourseManagement.Domain.Courses.Helpers;
using CourseManagement.ViewModels;
using CourseManagement.ViewModels.Courses;

namespace CourseManagement.Services
{
    public class CoursesService : ServiceBase
    {
        public CoursesService(IConfiguration config, IDomainFacade domainFacade) : base(config, domainFacade)
        {
        }

        public CourseListViewModel CreateListViewModel(SearchCondition condition)
        {
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

        public CourseViewModel Get(int courseId)
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
            viewModel.Duration = model.Duration;
            viewModel.StartDate = model.StartDate.ToString(WebConstants.DATE_FORMAT_VN);
            viewModel.EndDate = model.EndDate.ToString(WebConstants.DATE_FORMAT_VN);
            viewModel.Price = model.Price;
            viewModel.IsActive = (model.IsActive == 1);
            viewModel.Description = model.Description;
            viewModel.CreatedBy = model.CreatedBy;
            viewModel.CreatorName = model.CreatorName;
            viewModel.CreatedAt = model.CreatedAt;
            viewModel.UpdatedAt = model.UpdatedAt;
            viewModel.LastChanged = model.LastChanged;

            return viewModel;
        }

        public bool IsExistCourseCode(int courseId, string? courseCode)
        {
            if (string.IsNullOrEmpty(courseCode)) return false;

            var isExist = _domainFacade.Courses.IsExistCourseCode(courseId, courseCode);
            return isExist;
        }

        public bool Create(CourseViewModel viewModel)
        {
            var model = new CourseModel
            {
                CourseCode = viewModel.CourseCode,
                CourseName = viewModel.CourseName,
                Duration = viewModel.Duration,
                StartDate = ConvertDate(viewModel.StartDate) ?? DateTime.Now,
                EndDate = ConvertDate(viewModel.EndDate) ?? DateTime.Now,
                Price = viewModel.Price,
                Description = viewModel.Description ?? string.Empty,
                IsActive = viewModel.IsActive ? 1 : 0,
                CreatedBy = viewModel.CreatedBy,
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
                Duration = viewModel.Duration,
                StartDate = ConvertDate(viewModel.StartDate) ?? DateTime.Now,
                EndDate = ConvertDate(viewModel.EndDate) ?? DateTime.Now,
                Price = viewModel.Price,
                Description = viewModel.Description ?? string.Empty,
                IsActive = viewModel.IsActive ? 1 : 0,
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
    }
}
