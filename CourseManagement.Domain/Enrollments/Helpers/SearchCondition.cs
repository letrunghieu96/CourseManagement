namespace CourseManagement.Domain.Enrollments.Helpers
{
    public class SearchCondition
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string SearchWord { get; set; }
        public string Status { get; set; }
        public string Lecturer { get; set; }
        public string UserName { get; set; }
        public int? UserId { get; set; }
        public int PageLength { get; set; } = 10;
        public int PageNo { get; set; } = 1;
        public int BeginRowNum => (this.PageNo - 1) * this.PageLength;
        public string OrderBy { get; set; }
    }
}
