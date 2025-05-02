namespace Nemesys.Models.ViewModels
{
    public class ReportPostViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public int ReadCount { get; set; }
        public CategoryViewModel Category { get; set; }
        public AuthorViewModel Author { get; set; }
        public string LoggedInUserId { get; set; }

        public int UpvoteCount { get; set; }
        public bool HasUpvoted { get; set; }


        public string Location { get; set; }

        public HazardTypeViewModel HazardType { get; set; }

        public ReportStatusViewModel ReportStatus { get; set; }
    }
}
