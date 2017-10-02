using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Categories
{
    public class CategoryInputViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително")]
        [StringLength(90, MinimumLength = 3, ErrorMessage = "Заглавието трябва да е от {2} до {1} символа.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Описанието е задължително")]
        [StringLength(130, MinimumLength = 3, ErrorMessage = "Описанието трябва да е от {2} до {1} символа.")]
        public string Description { get; set; }
    }
}