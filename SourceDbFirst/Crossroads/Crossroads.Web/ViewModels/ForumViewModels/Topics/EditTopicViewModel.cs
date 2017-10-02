using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Crossroads.Web.ViewModels.ForumViewModels.Topics
{
    public class EditTopicViewModel : IMapFrom<Topic>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Заглавието трябва да е от {2} до {1} символа.")]
        public string Title { get; set; }

        [AllowHtml]
        [UIHint("TinyMCE")]
        [Required(ErrorMessage = "Задължително е темата да има съдържание.")]
        [Display(Name = "Съдържание:")]
        [StringLength(6000, MinimumLength = 10, ErrorMessage = "Съдържанието трябва да е от {2} до {1} символа.")]
        public string Content { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Topic, EditTopicViewModel>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(t => t.User.UserName));
        }

    }
}