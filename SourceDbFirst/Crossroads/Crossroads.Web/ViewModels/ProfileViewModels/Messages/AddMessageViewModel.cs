using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace Crossroads.Web.ViewModels.ProfileViewModels.Messages
{
    public class AddMessageViewModel
    {
        [Required(ErrorMessage = "Заглавието е задължително.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Заглавието трябва да е от {2} до {1} символа.")]
        public string Title { get; set; }

        public int UserId { get; set; }

        public int? AnswerMsgId { get; set; }

        [AllowHtml]
        [UIHint("TinyMCE")]
        [Required(ErrorMessage = "Съдържанието е задължително.")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Съдържанието трябва да е от {2} до {1} символа.")]
        public string Content { get; set; }
    }
}