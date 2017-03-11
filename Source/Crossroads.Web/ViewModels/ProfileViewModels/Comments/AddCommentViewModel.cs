using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Mappings;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Crossroads.Web.ViewModels.ProfileViewModels.Comments
{
    public class AddCommentViewModel
    {
        public int ProfileId { get; set; }

        [UIHint("TinyMCE"), AllowHtml]
        [Required(ErrorMessage = "Съдържанието е задължително.")]
        [StringLength(2500, ErrorMessage = "Коментарът трябва да е до {1} символа.")]
        public string Content { get; set; }
    }
}