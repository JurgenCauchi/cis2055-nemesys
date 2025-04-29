using System.ComponentModel.DataAnnotations;

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
        //Property used to bind user selection.
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

    }
}
