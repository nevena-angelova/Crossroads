using Crossroads.Models.Forum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Crossroads.Web.ViewModels.ForumViewModels.Comments
{
    public class AddCommentViewModel
    {
        [AllowHtml]
        [UIHint("TinyMCE")]
        [Required(ErrorMessage = "Съдържанието е задължително.")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Съдържанието трябва да е от {2} до {1} символа.")]
        [Display(Name = "Съдържание:")]
        public string Content { get; set; }

        public int AnswerId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}