using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nemesys.Models.ViewModels
{
    public class EditInvestigationViewModel
    {
        public int Id { get; set; }

        public int ReportId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1500, ErrorMessage = "Description cannot be longer than 1500 characters")]

        public string Description { get; set; }


        public List<SelectListItem>? AvailableReports { get; set; }

        [Display(Name = "Report Status")]
        public int ReportStatusId { get; set; }

        public List<ReportStatusViewModel>? ReportStatusList { get; set; }

        public ReportPost? Report { get; set; }
    }
}
