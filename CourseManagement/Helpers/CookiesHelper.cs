namespace CourseManagement.Helpers
{
    /// <summary>
    /// Cookies helper
    /// </summary>
    public static class CookiesHelper
    {
        public static void Set(this IResponseCookies response, string key, string value, int expireTime)
        {
            var option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(expireTime);
            response.Append(key, value, option);
        }

        public static void Remove(this IResponseCookies response, string key)
        {
            response.Delete(key);
        }
    }
}
