using Microsoft.AspNetCore.Mvc;
using Nemesys.Repositories.Interfaces;

namespace Nemesys.Controllers
{
    public class HallOfFameController : Controller
    {

        private readonly IReportRepository reportrepo;

        public HallOfFameController(IReportRepository reportrepo)
        {
            this.reportrepo = reportrepo;
        }

        public IActionResult Index()
        {
            var CurrentYear = DateTime.Now.Year;
            var Results = reportrepo.GetReporterRankings(CurrentYear);
            return View(Results);
        }
    }
}
