using System.Composition;

namespace Nemesys.Models.ViewModels
{
    public class InvestigationViewModel
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public AuthorViewModel Author { get; set; }
        public string LoggedInUserId { get; set; }
        public ReportStatusViewModel ReportStatus { get; set; }

        public ReportPost Report { get; set; }
    }
}
