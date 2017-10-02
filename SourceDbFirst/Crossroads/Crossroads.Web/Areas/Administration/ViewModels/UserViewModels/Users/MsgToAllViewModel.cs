using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Users
{
    public class MsgToAllViewModel
    {
        [Required(ErrorMessage = "Заглавието е задължително")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Заглавието трябва да е от {2} до {1} символа.")]
        public string Title { get; set; }

        [AllowHtml]
        [UIHint("TinyMCE")]
        [Required(ErrorMessage = "Задължително е съобщението да има съдържание.")]
        [StringLength(6000, MinimumLength = 10, ErrorMessage = "Съдържанието трябва да е от {2} до {1} символа.")]
        public string Content { get; set; }

        public DateTime DateCreated { get; set; }
    }
}