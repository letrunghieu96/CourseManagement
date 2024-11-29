using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseManagement.Helpers
{
    public static class ValueTextHelper
    {
        public static SelectListItem[] GetRoles(bool hasEmptyItem = true)
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
            var item = GetRoles(false).FirstOrDefault(item => item.Value == value);
            var text = (item != null) ? item.Text : string.Empty;
            return text;
        }

        public static SelectListItem[] GetActivities(bool hasEmptyItem = true)
        {
            var results = new List<SelectListItem>();

            // Add an empty item
            if (hasEmptyItem) results.Add(new SelectListItem { Text = string.Empty, Value = string.Empty, });

            results.Add(new SelectListItem { Text = "Kích hoạt", Value = "1", });
            results.Add(new SelectListItem { Text = "Vô hiệu hóa", Value = "0", });
            return results.ToArray();
        }
        public static string GetActivityText(string value)
        {
            var item = GetActivities(false).FirstOrDefault(item => item.Value == value);
            var text = (item != null) ? item.Text : string.Empty;
            return text;
        }
    }
}
