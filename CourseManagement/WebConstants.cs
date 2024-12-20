namespace CourseManagement
{
    /// <summary>
    /// Web constants
    /// </summary>
    public class WebConstants
    {
        // Config
        public static string CLIENT_VERSION = string.Empty;
        public static int[] PAGE_LENGTHS = new int[0];

        // Url
        public const string PAGE_HOME = "/Home";
        public const string PAGE_USERS_LIST = "/Users/List";
        public const string PAGE_COURSES_LIST = "/Courses/List";
        public const string PAGE_COURSES_REGISTER = "/Courses/Register";
        public const string PAGE_ENROLLMENTS_LIST = "/Enrollments/List";

        // View and partial view
        public const string VIEW_INDEX = "~/Views/Index/Index.cshtml";
        public const string VIEW_REGISTER = "~/Views/Index/Register.cshtml";

        public const string VIEW_USERS_LIST = "~/Views/Users/List.cshtml";
        public const string PARTIAL_VIEW_USERS_SEARCH_CONDITION = "~/Views/Users/_SearchCondition.cshtml";
        public const string PARTIAL_VIEW_USERS_SEARCH_RESULTS = "~/Views/Users/_SearchResults.cshtml";
        public const string PARTIAL_VIEW_USERS_REGISTER = "~/Views/Users/_Register.cshtml";
        public const string PARTIAL_VIEW_USERS_CHANGE_PASSWORD = "~/Views/Users/_ChangePassword.cshtml";
        public const string PARTIAL_VIEW_USERS_EDIT = "~/Views/Users/_Edit.cshtml";

        public const string VIEW_COURSES_LIST = "~/Views/Courses/List.cshtml";
        public const string PARTIAL_VIEW_COURSES_SEARCH_CONDITION = "~/Views/Courses/_SearchCondition.cshtml";
        public const string PARTIAL_VIEW_COURSES_SEARCH_RESULTS = "~/Views/Courses/_SearchResults.cshtml";
        public const string PARTIAL_VIEW_COURSES_REGISTER = "~/Views/Courses/Register.cshtml";

        public const string VIEW_ENROLLMENTS_LIST = "~/Views/Enrollments/List.cshtml";
        public const string PARTIAL_VIEW_ENROLLMENTS_SEARCH_CONDITION = "~/Views/Enrollments/_SearchCondition.cshtml";
        public const string PARTIAL_VIEW_ENROLLMENTS_SEARCH_RESULTS = "~/Views/Enrollments/_SearchResults.cshtml";

        public const string PARTIAL_VIEW_SIDEBAR_MENU = "~/Views/Shared/_SidebarMenu.cshtml";
        public const string PARTIAL_VIEW_PAGINATION = "~/Views/Shared/_Pagination.cshtml";

        // Value
        public const string DATE_FORMAT_VN = "dd/MM/yyyy";
        public const string DATE_TIME_FORMAT_VN = "dd/MM/yyyy HH:mm:ss";
        public const string FOLDER_PATH_COURSES = @"Contents\Courses";
        public const int LIMIT_NUMBER_COURSES_UPLOAD_FILES = 5;
        public const int LIMIT_SIZE_COURSES_UPLOAD_FILES = 10; // MB
    }
}
