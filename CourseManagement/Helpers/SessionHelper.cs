using Newtonsoft.Json;

namespace CourseManagement.Helpers
{
    /// <summary>
    /// Session helper
    /// </summary>
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            var result = (value == null) ? default(T) : JsonConvert.DeserializeObject<T>(value);

            return result;
        }
    }
}
