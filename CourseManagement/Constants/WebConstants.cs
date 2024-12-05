namespace CourseManagement.Constants
{
    /// <summary>
    /// Web constants
    /// </summary>
    public class WebConstants
    {
        // Controller
        public const string CONTROLLER_INDEX = "Index";
        public const string CONTROLLER_USERS = "Users";
        public const string CONTROLLER_COURSES = "Courses";

        // Url
        public const string PAGE_HOME = "/Home";
        public const string PAGE_USERS_LIST = "/Users/List";
        public const string PAGE_COURSES_LIST = "/Courses/List";
        public const string PAGE_COURSES_REGISTER = "/Courses/Register";

        // View and partial view
        public const string VIEW_INDEX = "~/Views/Index/Index.cshtml";
        public const string VIEW_REGISTER = "~/Views/Index/Register.cshtml";

        public const string VIEW_USERS_LIST = "~/Views/Users/List.cshtml";
        public const string PARTIAL_VIEW_USERS_SEARCH_CONDITION = "~/Views/Users/_SearchCondition.cshtml";
        public const string PARTIAL_VIEW_USERS_SEARCH_RESULTS = "~/Views/Users/_SearchResults.cshtml";
        public const string PARTIAL_VIEW_USERS_REGISTER = "~/Views/Users/_Register.cshtml";
        public const string PARTIAL_VIEW_USERS_CHANGE_PASSWORD = "~/Views/Users/_ChangePassword.cshtml";

        public const string VIEW_COURSES_LIST = "~/Views/Courses/List.cshtml";
        public const string PARTIAL_VIEW_COURSES_SEARCH_CONDITION = "~/Views/Courses/_SearchCondition.cshtml";
        public const string PARTIAL_VIEW_COURSES_SEARCH_RESULTS = "~/Views/Courses/_SearchResults.cshtml";
        public const string PARTIAL_VIEW_COURSES_REGISTER = "~/Views/Courses/Register.cshtml";

        public const string PARTIAL_VIEW_SIDEBAR_MENU = "~/Views/Shared/_SidebarMenu.cshtml";
        public const string PARTIAL_VIEW_PAGINATION = "~/Views/Shared/_Pagination.cshtml";

        // Value
        public const string CONST_ROLE_USER = "User";
        public const string CONST_ROLE_ADMIN = "Admin";

        public const string DATE_FORMAT_VN = "dd/MM/yyyy";
        public const string DATE_TIME_FORMAT_VN = "dd/MM/yyyy HH:mm:ss";
    }
}
