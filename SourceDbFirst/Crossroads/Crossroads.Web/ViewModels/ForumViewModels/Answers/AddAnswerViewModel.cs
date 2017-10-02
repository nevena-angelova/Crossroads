using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Crossroads.Web.ViewModels.ForumViewModels.Answers
{
    public class AddAnswerViewModel
    {
        [AllowHtml]
        [UIHint("TinyMCE")]
        [Required(ErrorMessage = "Съдържанието е задължително.")]
        [Display(Name = "Съдържание:")]
        [StringLength(6000, MinimumLength = 10, ErrorMessage = "Съдържанието трябва да е от {2} до {1} символа.")]
        public string Content { get; set; }

        public int TopicId { get; set; }
    }
}