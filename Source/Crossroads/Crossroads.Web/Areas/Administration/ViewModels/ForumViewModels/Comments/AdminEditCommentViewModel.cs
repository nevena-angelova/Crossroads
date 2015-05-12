using Crossroads.Models.Forum;
using Crossroads.Web.Infrastructure.Mappings;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Comments
{
    public class AdminEditCommentViewModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Съдържанието е задължително.")]
        [AllowHtml]
        [UIHint("TinyMCE")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Съдържанието трябва да е от {2} до {1} символа.")]
        public string Content { get; set; }
    }
}