namespace Nemesys.Models.ViewModels
{
    public class InvestigationListViewModel
    {

        public int TotalEntries { get; set; }
        public IEnumerable<InvestigationViewModel> Investigations { get; set; }
    }
}
