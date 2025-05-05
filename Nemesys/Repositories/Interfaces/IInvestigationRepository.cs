using Nemesys.Models;
using Nemesys.Models.ViewModels;

namespace Nemesys.Repositories.Interfaces
{
    public interface IInvestigationRepository
    {
        IEnumerable<Investigation> GetAllInvestigations();
        Investigation GetInvestigationById(int blogPostId);

        void CreateInvestigation(Investigation newInvestigation);

        void UpdateInvestigation(Investigation updatedInvestigation);

        void DeleteInvestigation(int id);

        IEnumerable<ReportStatus> GetAllReportStatuses();

        public Investigation GetInvestigationByIdWithReport(int id);

    }
}
