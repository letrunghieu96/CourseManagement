using CourseManagement.Domain;
using CourseManagement.Domain.Courses;
using CourseManagement.Domain.Courses.Helpers;
using CourseManagement.Domain.Enrollments;
using CourseManagement.ViewModels;
using CourseManagement.ViewModels.Courses;
using System.Globalization;

namespace CourseManagement.Services
{
    public class CoursesService : ServiceBase
    {
        /// <summary>Upload file extensions</summary>
        private readonly string _uploadCoursesFolderPath;

        public CoursesService(IConfiguration config, IDomainFacade domainFacade) : base(config, domainFacade)
        {
#if DEBUG
            _uploadCoursesFolderPath = "Contents";
#else
            _uploadCoursesFolderPath = Path.Combine(_config.GetValue<string>("PhysicalPath"), "Contents");
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
            if (total > 0) searchResults.ToList().ForEach(result => result.CourseImage = GetImage(result.FolderName));
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
            viewModel.FolderName = model.FolderName;
            viewModel.CourseImage = GetImage(model.FolderName);
            viewModel.CourseFiles = GetFileNames(model.FolderName);
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

        public bool CheckDateRange(string? startDate, string? endDate)
        {
            // Compare
            var result = ConvertDate(endDate) >= ConvertDate(startDate);
            return result;
        }

        public bool Create(CourseViewModel viewModel, string folderName)
        {
            var model = new CourseModel
            {
                CourseCode = viewModel.CourseCode,
                CourseName = viewModel.CourseName,
                FolderName = folderName,
                MainContent = viewModel.MainContent ?? string.Empty,
                Duration = int.Parse(viewModel.Duration ?? "0", NumberStyles.AllowThousands),
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
            // Delete file
            var model = _domainFacade.Courses.Get(courseId);
            var directoryPath = Path.Combine(_uploadCoursesFolderPath, model.FolderName);
            DeleteDirectory(directoryPath);

            // Delete course
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

        public async Task<bool> UploadImage(string folderName, IFormFile file)
        {
            try
            {
                if (file == null) return false;

                // Check folder
                var imageFolderPath = Path.Combine(_uploadCoursesFolderPath, folderName, "Image");
                if (Directory.Exists(imageFolderPath)) DeleteDirectory(imageFolderPath);
                Directory.CreateDirectory(imageFolderPath);

                // Save file
                var filePath = Path.Combine(imageFolderPath, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return true;
            }
            catch
            {
            }

            return false;
        }

        public bool IsExistImage(string folderName)
        {
            var filePath = Path.Combine(_uploadCoursesFolderPath, folderName, "Image");
            if (!Directory.Exists(filePath)) return false;

            var files = Directory.GetFiles(filePath);
            return (files.Length > 0);
        }

        public string GetImage(string folderName)
        {
            var fileName = string.Empty;
            var fileImagePath = Path.Combine(_uploadCoursesFolderPath, folderName, "Image");
            if (!Directory.Exists(fileImagePath)) return fileName;

            foreach (var file in Directory.GetFiles(fileImagePath))
            {
                fileName = Path.GetFileName(file);
                break;
            }
            return $"\\Contents\\{folderName}\\Image\\{fileName}" ?? string.Empty;
        }

        public string GetImageUrl(string folderName, string courseImage)
        {
            var filePath = $"\\Contents\\{folderName}\\Image\\{courseImage}";
            return filePath;
        }

        public void DeleteDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                foreach (var file in Directory.GetFiles(directoryPath))
                {
                    File.Delete(file);
                }

                foreach (var dir in Directory.GetDirectories(directoryPath))
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(directoryPath);
            }
        }

        public async Task<bool> UploadFiles(string folderName, IFormFile[]? files)
        {
            try
            {
                // Check folder
                var filesFolderPath = Path.Combine(_uploadCoursesFolderPath, folderName, "Files");
                if (!Directory.Exists(filesFolderPath)) Directory.CreateDirectory(filesFolderPath);

                // Save files
                if (files == null || files.Length == 0) return true;
                foreach (IFormFile file in files)
                {
                    var filePath = Path.Combine(filesFolderPath, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                return true;
            }
            catch
            {
            }

            return false;
        }

        public string[] GetFileNames(string folderName)
        {
            var fileNames = new List<string>();
            var folderPath = Path.Combine(_uploadCoursesFolderPath, folderName, "Files");
            if (!Directory.Exists(folderPath)) return fileNames.ToArray();

            foreach (var file in Directory.GetFiles(folderPath))
            {
                fileNames.Add(Path.GetFileName(file));
            }

            return fileNames.ToArray();
        }

        public FileStream GetFile(string folderName, string fileName)
        {
            var filePath = Path.Combine(_uploadCoursesFolderPath, folderName, "Files", fileName);
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return stream;
        }

        public bool DeleteFile(string folderName, string fileName)
        {
            try
            {
                var filePath = Path.Combine(_uploadCoursesFolderPath, folderName, "Files", fileName);
                File.Delete(filePath);

                return true;
            }
            catch
            {
            }

            return false;
        }
    }
}
