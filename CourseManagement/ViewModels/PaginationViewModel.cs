using System.Text;

namespace CourseManagement.ViewModels
{
    public class PaginationViewModel
    {
        private const string _formatGoToPage = @"<li class=""paginate_button page-item""><a class=""page-link"" onclick=""{0}"" aria-label=""{1}""><span aria-hidden=""true"">{2}</span><span class=""sr-only""></span></a></li>";
        private const string _formatPageItem = @"<li class=""paginate_button page-item {0}""><a class=""page-link"" onclick=""{1}"">{2}</a></li>";
        private const int _firstPage = 1;

        private string CreatePagesHtml()
        {
            var pageCount = (int)((this.Total - 1) / this.PageLength) + 1;
            var previousPage = ((this.CurrentPage - 1) > pageCount) ? pageCount : this.CurrentPage - 1;
            var nextPage = ((this.CurrentPage + 1) > pageCount) ? pageCount : this.CurrentPage + 1;

            var format = new StringBuilder();
            format.Append(@"<ul class=""pagination"">");

            if ((pageCount > _firstPage)
                && (this.CurrentPage != 1))
            {
                format.AppendFormat(
                    _formatGoToPage,
                    "changePageNo('" + previousPage.ToString() + "')",
                    "Previous",
                    "&laquo;");
            }

            format.AppendFormat(
                    _formatPageItem,
                    (_firstPage == this.CurrentPage) ? "active" : string.Empty,
                    "changePageNo('" + _firstPage.ToString() + "')",
                    _firstPage.ToString());

            for (int index = 2; index < pageCount; index++)
            {
                var pageNo = index.ToString();

                if (((index >= this.CurrentPage - 2) && (index <= this.CurrentPage))
                    || ((index <= this.CurrentPage + 2) && (index >= this.CurrentPage)))
                {
                    pageNo = index.ToString();
                }
                else if ((index == this.CurrentPage - 3) || (index == this.CurrentPage + 3))
                {
                    pageNo = "...";
                }
                else
                {
                    continue;
                }

                format.AppendFormat(
                    _formatPageItem,
                    (index == this.CurrentPage) ? "active" : string.Empty,
                    "changePageNo('" + index.ToString() + "')",
                    pageNo);
            }

            if (pageCount > _firstPage)
            {
                format.AppendFormat(
                    _formatPageItem,
                    (pageCount == this.CurrentPage) ? "active" : string.Empty,
                    "changePageNo('" + pageCount.ToString() + "')",
                    pageCount);

                if (this.CurrentPage != pageCount)
                {
                    format.AppendFormat(
                        _formatGoToPage,
                        "changePageNo('" + nextPage.ToString() + "')",
                        "Next",
                        "&raquo;");
                }
            }

            format.Append(@"</ul>");

            return format.ToString();
        }

        public int PageLength { get; set; }
        public long Total { get; set; }
        public int CurrentPage { get; set; }
        public int BeginRowNum => (this.Total > 0) ? (((this.CurrentPage - 1) * this.PageLength) + 1) : 0;
        public int RowsOfPage => (this.Total > 0) ? (this.CurrentPage * this.PageLength) : 0;
        public string PagesHtml => CreatePagesHtml();
    }
}
