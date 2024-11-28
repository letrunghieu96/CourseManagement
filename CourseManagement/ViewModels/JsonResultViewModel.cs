namespace CourseManagement.ViewModels
{
    /// <summary>
    /// Json result view model
    /// </summary>
    public class JsonResultViewModel
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public ErrorMessageList? Errors { get; set; }
        public string? Path { get; set; }
    }

    /// <summary>
    /// Error message list
    /// </summary>
    public class ErrorMessageList : List<KeyValuePair<string, string>>
    {
        public void Add(string key, string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            base.Add(new KeyValuePair<string, string>(key, value));
        }
    }
}
