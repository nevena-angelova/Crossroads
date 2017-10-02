using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Topics
{
    public class AdminEditTopicViewModel : IMapFrom<Topic>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Display(Name = "Категория:")]
        [Required(ErrorMessage = "Категорията е задължителна.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително")]
        [Display(Name = "Заглавие:")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Заглавието трябва да е от {2} до {1} символа.")]
        public string Title { get; set; }

        [AllowHtml]
        [UIHint("TinyMCE")]
        [Required(ErrorMessage = "Задължително е темата да има съдържание.")]
        [Display(Name = "Съдържание:")]
        [StringLength(6000, MinimumLength = 10, ErrorMessage = "Съдържанието трябва да е от {2} до {1} символа.")]
        public string Content { get; set; }

        [Display(Name = "Приоритетна:")]
        public bool IsPriority { get; set; }

        [Display(Name = "Махни флаговете:")]
        public bool RemoveFlags { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Topic, AdminEditTopicViewModel>()
                .ForMember(m => m.IsPriority, opt => opt.MapFrom(t => t.Priority != null ? true : false));
        }
    }
}