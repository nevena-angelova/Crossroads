using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Answers
{
    public class AdminEditAnswerViewModel : IMapFrom<Answer>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Съдържанието е задължително.")]
        [AllowHtml]
        [UIHint("TinyMCE")]
        [StringLength(6000, MinimumLength = 10, ErrorMessage = "Съдържанието трябва да е от {2} до {1} символа.")]
        public string Content { get; set; }

        [Display(Name = "Махни флаговете:")]
        public bool RemoveFlags { get; set; }
    }
}