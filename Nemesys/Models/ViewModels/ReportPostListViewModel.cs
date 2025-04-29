namespace Nemesys.Models.ViewModels
{
    public class ReportPostListViewModel
    {
        public int TotalEntries { get; set; }
        public IEnumerable<ReportPostViewModel> ReportPosts { get; set; }

    }
}
