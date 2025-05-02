using Nemesys.Models;

public class ReportUpvote
{
    public int Id { get; set; }

    public int ReportPostId { get; set; }
    public ReportPost ReportPost { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }
}
