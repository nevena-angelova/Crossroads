using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Crossroads.Web.ViewModels.ForumViewModels.Topics
{
    public class AddTopicViewModel
    {
        [Required(ErrorMessage = "Заглавието е задължително")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Заглавието трябва да е от {2} до {1} символа.")]
        public string Title { get; set; }

        [AllowHtml]
        [UIHint("TinyMCE")]
        [Required(ErrorMessage = "Задължително е темата да има съдържание.")]
        [StringLength(6000, MinimumLength = 10, ErrorMessage = "Съдържанието трябва да е от {2} до {1} символа.")]
        public string Content { get; set; }

        public int CategoryId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}