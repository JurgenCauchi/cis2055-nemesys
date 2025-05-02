namespace Nemesys.Models
{
    public class ReportPost
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }

        public string Location { get; set; }

        //Foreign Key - navigation property (name + key as the  property name)
        public int CategoryId { get; set; }
        //Reference navigation property
        public Category Category { get; set; }
        //public int ReadCount { get; set; }

        public int ReportStatusId { get; set; }
        public ReportStatus ReportStatus { get; set; }

        public int HazardTypeId { get; set; }
        public HazardType Hazard { get; set; }

        public List<ReportUpvote> Upvotes { get; set; }



        public DateTime UpdatedDate { get; set; }

        public string? UserId { get; set; }
        public AppUser User { get; set; }
    }
}
