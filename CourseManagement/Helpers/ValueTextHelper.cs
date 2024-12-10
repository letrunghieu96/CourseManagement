using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseManagement.Helpers
{
    public static class ValueTextHelper
    {
        public static SelectListItem[] GetUserRoles(bool hasEmptyItem = true)
        {
            var results = new List<SelectListItem>();

            // Add an empty item
            if (hasEmptyItem) results.Add(new SelectListItem { Text = string.Empty, Value = string.Empty, });

            results.Add(new SelectListItem { Text = "Người dùng", Value = "User", });
            results.Add(new SelectListItem { Text = "Quản trị viên", Value = "Admin", });
            return results.ToArray();
        }
        public static string GetRoleText(string value)
        {
            var item = GetUserRoles(false).FirstOrDefault(item => item.Value == value);
            var text = (item != null) ? item.Text : string.Empty;
            return text;
        }

        public static SelectListItem[] GetStatuses(bool hasEmptyItem = true)
        {
            var results = new List<SelectListItem>();

            // Add an empty item
            if (hasEmptyItem) results.Add(new SelectListItem { Text = string.Empty, Value = string.Empty, });

            results.Add(new SelectListItem { Text = "Kích hoạt", Value = "1", });
            results.Add(new SelectListItem { Text = "Vô hiệu hóa", Value = "0", });
            return results.ToArray();
        }
        public static string GetStatusText(string value)
        {
            var item = GetStatuses(false).FirstOrDefault(item => item.Value == value);
            var text = (item != null) ? item.Text : string.Empty;
            return text;
        }

        public static SelectListItem[] GetCourseStatuses(bool hasEmptyItem = true)
        {
            var results = new List<SelectListItem>();

            // Add an empty item
            if (hasEmptyItem) results.Add(new SelectListItem { Text = string.Empty, Value = string.Empty, });

            results.Add(new SelectListItem { Text = "Mở đăng ký", Value = "0", });
            results.Add(new SelectListItem { Text = "Đang diễn ra", Value = "1", });
            results.Add(new SelectListItem { Text = "Hoàn thành", Value = "2", });
            results.Add(new SelectListItem { Text = "Đã hủy", Value = "3", });
            return results.ToArray();
        }
        public static string GetCourseStatusText(string value)
        {
            var item = GetCourseStatuses(false).FirstOrDefault(item => item.Value == value);
            var text = (item != null) ? item.Text : string.Empty;
            return text;
        }
    }
}
