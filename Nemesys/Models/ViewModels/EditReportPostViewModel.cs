using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Nemesys.Models.ViewModels
{
    public class EditReportPostViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "A title is required")]
        [StringLength(50)]
        [Display(Name = "Blog heading")]

        public string Title { get; set; }
        [Display(Name = "Blog Post Category")]
        [Required(ErrorMessage = "Category is required")]

        public int CategoryId { get; set; }
        //Property used solely to populate drop down
        public List<CategoryViewModel>? CategoryList { get; set; }
        [Required(ErrorMessage = "Blog post content is required")]
        [StringLength(1500, ErrorMessage = "Blog post cannot be longer than 1500 characters")]

        public string Content { get; set; }
        public string? ImageUrl { get; set; } //Used to prepare the edit page
        [Display(Name = "Featured Image")]
        public IFormFile? ImageToUpload { get; set; } //used only when submitting form

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Display(Name = "Hazard Type")]
        [Required(ErrorMessage = "Hazard Type is required")]
        public int HazardTypeId { get; set; }
        [BindNever, ValidateNever]
        public List<HazardTypeViewModel>? HazardTypeList { get; set; }

        [Display(Name = "Report Status")]
        public int ReportStatusId { get; set; }
        [BindNever, ValidateNever]
        public List<ReportStatusViewModel>? ReportStatusList { get; set; }


    }
}
