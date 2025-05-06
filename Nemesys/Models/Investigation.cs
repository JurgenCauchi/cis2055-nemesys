namespace Nemesys.Models
{
    public class Investigation
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public int ReportId { get; set; }

        public string Description { get; set; }

        public int ReportStatusId { get; set; }
        public ReportStatus ReportStatus { get; set; }
        public ReportPost? Report { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string? UserId { get; set; }
        public AppUser User { get; set; }

        public int? CategoryId { get; set; }

        public int? HazardTypeId { get; set; }
    }
}
