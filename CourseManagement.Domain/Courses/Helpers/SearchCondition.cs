namespace CourseManagement.Domain.Courses.Helpers
{
    /// <summary>
    /// Search condition
    /// </summary>
    public class SearchCondition
    {
        public string SearchWord { get; set; }
        public string Status { get; set; }
        public string StartDateFrom { get; set; }
        public string StartDateTo { get; set; }
        public string EndDateFrom { get; set; }
        public string EndDateTo { get; set; }
        public string Lecturer { get; set; }
        public int PageLength { get; set; } = 10;
        public int PageNo { get; set; } = 1;
        public int BeginRowNum => (this.PageNo - 1) * this.PageLength;
        public string OrderBy { get; set; }
    }
}
