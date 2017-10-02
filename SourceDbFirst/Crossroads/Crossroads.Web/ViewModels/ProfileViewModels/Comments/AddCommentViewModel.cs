using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Crossroads.Web.ViewModels.ProfileViewModels.Comments
{
    public class AddCommentViewModel
    {
        public int UserId { get; set; }

        [UIHint("TinyMCE"), AllowHtml]
        [Required(ErrorMessage = "Съдържанието е задължително.")]
        [StringLength(100000, ErrorMessage = "Коментарът трябва да е до {1} символа.")]
        public string Content { get; set; }
    }
}